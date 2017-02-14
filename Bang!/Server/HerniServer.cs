using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime;
using System.Collections;
using Bang_.Spolecne;
using Bang_.Spolecne.Karty;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bang_
{
    public class HerniServer
    {
        private List<Spojeni> klienti = new List<Spojeni>();
        private List<string> mrtviHraci = new List<string>();
        private ListKaret<Karta> kartaBalikNepouzity= new ListKaret<Karta>(3);
        private ListKaret<Karta> kartaBalikVyhozeny = new ListKaret<Karta>(0);
        public Dictionary<String,PostavaKarta> postavyHracu = new Dictionary<String, PostavaKarta>();
        private Dictionary<String, Dictionary<String, ListKaret<Karta>>> kartaKartyUHracu = new Dictionary<string, Dictionary<string, ListKaret<Karta>>>();
        private Dictionary<String, int> poradi = new Dictionary<string,int>(), seznamHracuTmp;
        private Dictionary<String, int> hraciZivoty = new Dictionary<string, int>();
        private ListKaret<PostavaKarta> postavyBalikKaret = new ListKaret<PostavaKarta>(2);
        private int serif, aktualniHrac, autoSave = 0;
        private String chat = "", nazevSouboru = "";
        private bool seznamHracuBool;
        private ListKaret<FunkceKarta> funkceBalikKaret;
        private Dictionary<string, Dictionary<string, object>> slovnikProUlozeni = new Dictionary<string, Dictionary<string, object>>();

        #region konstruktor, balík karet, pošle hráčům jejich funkce a postavy
        public HerniServer(List<Spojeni> klienti)
        {
            this.klienti = klienti;

            //Console.Clear();
            Console.WriteLine("Herní server byl zapnut!");
            SpojeniRun sr;
            foreach (Spojeni s in klienti)  //vytvoření tolik naslouchacích vláken jako je poček klientů
            {
                sr = new SpojeniRun(s, this);
                Thread t = new Thread(new ThreadStart(sr.Naslouchej));
                t.Start();
            }
            PozorovatelBalickuKaret pozBalKar = new PozorovatelBalickuKaret();
            kartaBalikNepouzity.StateUpdate += pozBalKar.Update;

            kartaBalikNepouzity.Zamichej();

            funkceBalikKaret = new ListKaret<FunkceKarta>(1, klienti.Count);
            postavyBalikKaret.Zamichej();
            funkceBalikKaret.Zamichej();
            System.Threading.Thread.Sleep(3000);
            serverZahajHru();

            /*
            Transit zahaj = new Transit();
            zahaj.obsah = 3;
            klienti.ElementAt(aktualniHrac++).Send(zahaj);
             */
        }
        #endregion

        #region Konec hry, nova hra, zamicha a procisti
        private void serverReinicialize()
        {
            //// znovunabehnuti promennych pro dalsi hru
            this.kartaBalikNepouzity = new ListKaret<Karta>(3);
            this.kartaBalikVyhozeny = new ListKaret<Karta>(0);
            this.mrtviHraci = new List<string>();
            this.postavyHracu = new Dictionary<String, PostavaKarta>();
            this.kartaKartyUHracu = new Dictionary<string, Dictionary<string, ListKaret<Karta>>>();
            this.poradi = new Dictionary<string, int>();
            this.hraciZivoty = new Dictionary<string, int>();
            this.postavyBalikKaret = new ListKaret<PostavaKarta>(2);
            this.serif = 0; this.aktualniHrac = 0; this.autoSave = 0;
            this.chat = ""; this.nazevSouboru = "";
            //parent.seznamHracuBool;
            this.slovnikProUlozeni = new Dictionary<string, Dictionary<string, object>>();

            this.kartaBalikNepouzity.Zamichej();

            this.funkceBalikKaret = new ListKaret<FunkceKarta>(1, this.klienti.Count);
            this.postavyBalikKaret.Zamichej();
            this.funkceBalikKaret.Zamichej();
        }

        private void serverZahajHru()
        {
            foreach (var klient in klienti)
            {
                this.poradi.Add(klient.name, klienti.IndexOf(klient));
                Transit funkce = new Transit();
                funkce.obsah = 101;
                funkce.objekt = funkceBalikKaret.ElementAt(0);
                if (funkceBalikKaret.ElementAt(0).Funkce == 1)
                {
                    serif = klienti.IndexOf(klient);
                    aktualniHrac = serif;
                }
                funkceBalikKaret.RemoveAt(0);
                klient.Send(funkce);

            }
            foreach (var klient in klienti)
            {

                Transit SherifIs = new Transit();
                SherifIs.obsah = 105;
                SherifIs.objekt = serif;
                klient.Send(SherifIs);

                Transit poradi = new Transit();
                poradi.obsah = 103;
                poradi.objekt = this.poradi;
                klient.Send(poradi);
            }
            System.Threading.Thread.Sleep(5000);

            Transit postavy = new Transit();
            postavy.obsah = 102;
            postavy.list = new List<Object>();
            postavy.list.Add(postavyBalikKaret.ElementAt(0));
            postavyBalikKaret.RemoveAt(0);
            postavy.list.Add(postavyBalikKaret.ElementAt(0));
            postavyBalikKaret.RemoveAt(0);
            klienti.ElementAt(aktualniHrac++).Send(postavy);
        }
        #endregion

        private class SpojeniRun : Spojeni
        {
            public HerniServer parent;

            public SpojeniRun(Spojeni s, HerniServer par)
            {
                this.spojeni = s.spojeni;
                this.name = s.name;
                this.stream = s.stream;
                this.parent = par;
            }

            public void Naslouchej()    // metoda, která čte vstup od jednotlivých hráčů a pak s ním pracuje
            {
                Transit transit;
                Transit posli;
                while (true)
                {
                    posli = new Transit();
                    transit = Recieve();
                    Console.WriteLine("transit: " + transit.obsah);

                    #region zpráva do chatovacího okna (1)
                    if (transit.obsah == 1) // 1 znamená zpráva do chatovacího okna
                    {
                        Console.WriteLine("Přijal jsem zprávu od: " + name + " ve tvaru: " + transit.text + "(1)");

                        parent.chat += name + ": " + transit.text + "\n";
                        posli.obsah = 1;
                        posli.text = parent.chat;
                        foreach (Spojeni s in parent.klienti)
                        {
                            s.Send(posli);
                        }
                    }
                    #endregion

                    #region klient si vyžádal kartu (2)
                    if (transit.obsah == 2)
                    {
                        Console.WriteLine("vydána karta - " + name + "(2)");
                        if (parent.kartaBalikNepouzity.Count == 0)  //pokud je balíček s kartami prázdný
                        {
                            parent.kartaBalikNepouzity.AddRange(parent.kartaBalikVyhozeny);
                            parent.kartaBalikVyhozeny.Clear();
                            parent.kartaBalikNepouzity.Zamichej();
                        }
                        posli.obsah = 2;
                        posli.objekt = parent.kartaBalikNepouzity.ElementAt(0);
                        parent.kartaBalikNepouzity.RemoveAt(0);
                        Send(posli);
                    }
                    #endregion

                    #region předání tahu dalšímu hráči v pořadí (3)
                    if (transit.obsah == 3)
                    {
                        //if (parent.test++ == 10) continue; //ukonci hru po nekolika zatich
                        if (parent.aktualniHrac == parent.klienti.Count)
                        {
                            parent.aktualniHrac = 0;
                        }
                        posli.obsah = 3;
                        parent.klienti.ElementAt(parent.aktualniHrac++).Send(posli);       
                    }
                    #endregion

                    #region přijetí postavové karty a předání iniciativy dále(105)
                    if (transit.obsah == 105)
                    {
                        parent.postavyHracu.Add(parent.klienti.ElementAt(parent.aktualniHrac-1).name,(PostavaKarta)transit.objekt);
                        if (parent.aktualniHrac - 1 == parent.serif)//ulozi pocet zivotu do parent.hraciZivoty
                        {
                            parent.hraciZivoty.Add(parent.klienti.ElementAt(parent.aktualniHrac - 1).name, ((PostavaKarta)transit.objekt).PocetZivotu + 1);
                        }
                        else
                        {
                            parent.hraciZivoty.Add(parent.klienti.ElementAt(parent.aktualniHrac - 1).name, ((PostavaKarta)transit.objekt).PocetZivotu);
                        }
                        if (parent.aktualniHrac == parent.klienti.Count)
                        {
                            parent.aktualniHrac = 0;
                        }
                        if (parent.aktualniHrac == parent.serif)
                        {
                            foreach (var item in parent.klienti)
                            {
                                Transit postavyVsech = new Transit();
                                postavyVsech.obsah = 106;
                                postavyVsech.objekt = parent.postavyHracu;
                                item.Send(postavyVsech);
                            }
                            //TODO: rozdat na zacatku vsem karty podle zivotu
                            Transit zahaj = new Transit();
                            zahaj.obsah = 3;
                            parent.klienti.ElementAt(parent.aktualniHrac++).Send(zahaj);
                        }
                        else
                        {
                            Transit postavy = new Transit();
                            postavy.obsah = 102;
                            postavy.list = new List<Object>();
                            postavy.list.Add(parent.postavyBalikKaret.ElementAt(0));
                            parent.postavyBalikKaret.RemoveAt(0);
                            postavy.list.Add(parent.postavyBalikKaret.ElementAt(0));
                            parent.postavyBalikKaret.RemoveAt(0);
                            parent.klienti.ElementAt(parent.aktualniHrac++).Send(postavy);
                        }
                        
                    }
                    #endregion

                    #region poslání ostatním hráčům aktualizovaný seznam karet (4)
                    if (transit.obsah == 4)
                    {
                        if (parent.kartaKartyUHracu.ContainsKey(this.name))
                        {
                            parent.kartaKartyUHracu.Remove(this.name);
                            //Console.WriteLine("kartaKartyUHracu obsahoval " + this.name);
                        }
                        parent.kartaKartyUHracu.Add(this.name, (Dictionary<String, ListKaret<Karta>>)transit.objekt);
                        posli.obsah = 4;
                        posli.objekt = parent.kartaKartyUHracu;
                        foreach (var item in parent.klienti)
                        {
                            item.Send(posli);
                        }
                    }
                    #endregion

                    #region poslání seznamu hráčů na které vidí daný hráč (5)
                    if (transit.obsah == 5)
                    {
                        int vzdalenost = (int)transit.objekt;
                        parent.seznamHracuBool = false;
                        parent.seznamHracuTmp = new Dictionary<string, int>();
                        posli.obsah = 7;
                        //Console.WriteLine("SERVER: záskávám seznam vzdáleností od hráčů.");
                        foreach (Spojeni item in parent.klienti)
                        {
                            if (item.spojeni != this.spojeni) //neposílám sobě samému
                            {
                                item.Send(posli);
                            }
                            
                        }

                        while (!parent.seznamHracuBool)
                        {
                            Thread.Sleep(10);
                        }

                        posli = new Transit();
                        posli.obsah = 5;
                        posli.objekt = parent.seznamHracuTmp;
                        posli.text = Convert.ToString(vzdalenost);
                        Send(posli);    // odeslán slovník, kde jsou posuny OD ke všem hráčům
                        //Console.WriteLine("SERVER: seznam získán, posílám zpět.");
                    }
                    #endregion

                    #region poslání hráči, že byl zasažen (6)
                    if (transit.obsah == 6)
                    {
                        foreach (var item in parent.klienti)
                        {
                            if (item.name.Equals(transit.text))
                            {
                                posli.obsah = 6;
                                posli.text = name;
                                item.Send(posli);
                                System.Console.WriteLine("hráč " + name + " vystřelil na " + item.name +"(6)");
                            }
                        }
                    }
                    #endregion

                    #region přijmutí posunutí daného hráče (7)
                    if (transit.obsah == 7)
                    {
                        //Console.WriteLine("SERVER: přijal jsem nastavení vzdálenosti od: " + name);
                        parent.seznamHracuTmp.Add(name, (int)transit.list.ElementAt(0));
                        if (parent.seznamHracuTmp.Count == (parent.klienti.Count-1))  //bez sebe sama
                        {
                            parent.seznamHracuBool = true;
                        }
                    }
                    #endregion

                    #region odhození karty do odhazovacího balíčku (8)
                    if (transit.obsah == 8)
                    {
                        parent.kartaBalikVyhozeny.Add((Karta)transit.objekt);
                        //Console.WriteLine("Prijal sem kartu do odhazovaciho balicku");
                        //TODO: poslat vsem ostatnim a ukazat ze na kupce nahore - mozna
                        
                        posli.obsah = 8;
                        posli.objekt = (Karta)transit.objekt;
                        foreach (var item in parent.klienti)
                        {
                            item.Send(posli);
                        }
                            // LOW PRIORITY
                    }
                    #endregion

                    #region zahrana karta salon (pivo pro vsechny) (9)
                    if (transit.obsah == 9)
                    {
                        posli.obsah = 9;
                        foreach (var item in parent.klienti)
                        {
                            if (parent.mrtviHraci.Contains(item.name)) continue;
                            item.Send(posli);
                        }
                    }
                    #endregion

                    #region Kulomet - vystřel na všechny ostatní (10)
                    if (transit.obsah == 10)
                    {
                        foreach (var item in parent.klienti)
                        {
                            if (!item.name.Equals(this.name))
                            {
                                posli.obsah = 6;
                                posli.text = name;
                                item.Send(posli);
                            }
                        }
                    }
                    #endregion

                    #region indiáni - každý musí vyhodit bang (11)
                    if (transit.obsah == 11)
                    {
                        foreach (var item in parent.klienti)
                        {
                            if (!item.name.Equals(this.name))
                            {
                                posli.obsah = 12;
                                posli.text = name;
                                item.Send(posli);
                            }
                        }
                    }
                    #endregion

                    #region pošli kartu pro potřeby nějaké spec. karty (13)
                    if (transit.obsah == 13)
                    {
                        Console.WriteLine("vydána karta - " + name + "(13)");
                        if (parent.kartaBalikNepouzity.Count == 0)  //pokud je balíček s kartami prázdný
                        {
                            parent.kartaBalikNepouzity.AddRange(parent.kartaBalikVyhozeny);
                            parent.kartaBalikVyhozeny.Clear();
                            parent.kartaBalikNepouzity.Zamichej();
                        }
                        posli.obsah = 13;
                        posli.objekt = parent.kartaBalikNepouzity.ElementAt(0);
                        parent.kartaBalikNepouzity.RemoveAt(0);
                        Send(posli);
                    }
                    #endregion

                    #region inicializace uložení hry (14)
                    if (transit.obsah == 14)
                    {
                        //parent.slovnikFullBool = false;
                        parent.slovnikProUlozeni = new Dictionary<string, Dictionary<string, object>>();
                        parent.nazevSouboru = transit.text;
                        foreach (var item in parent.klienti)
                        {
                            transit = new Transit();
                            transit.obsah = 14;
                            item.Send(transit);
                        }

                        //while (!parent.slovnikFullBool) // dokud všichni klienti nepošlou svoje nastavení
                        //{
                        //    Thread.Sleep(10);
                        //}

                        //transit = new Transit();
                        //transit.obsah = 15;
                        //transit.objekt = parent.slovnikProUlozeni;
                    }
                    #endregion

                    #region uložení hry od klienta (15)
                    if (transit.obsah == 15)
                    {
                        try
                        {
                            parent.slovnikProUlozeni.Add(name, (Dictionary<string, object>)transit.objekt);
                            if (parent.klienti.Count == parent.slovnikProUlozeni.Count)
                            {
                                //parent.slovnikFullBool = true;
                                String verdict = "Vyskytla se chyba!";
                                FileInfo fi;
                                if (parent.nazevSouboru.CompareTo("autoSave") == 0)
                                {
                                    parent.nazevSouboru = @"autoSave" + parent.autoSave.ToString() + ".bng";
                                    fi = new FileInfo(Directory.GetCurrentDirectory() + @"\autosave\" + parent.nazevSouboru);
                                    parent.autoSave++;
                                }
                                else
                                {
                                    string cesta = Directory.GetCurrentDirectory() + @"\save\" + parent.nazevSouboru + ".bng";
                                    fi = new FileInfo(cesta);
                                    //parent.nazevSouboru += @".bng";
                                }

                                using (BinaryWriter bw = new BinaryWriter(File.Open(fi.FullName, FileMode.Create)))
                                {
                                    BinaryFormatter biFormatter = new BinaryFormatter();
                                
                                    //přidání důležitých prvků do save file od serveru
                               
                                    Dictionary<string, object> tmp = new Dictionary<string, object>();
                                    tmp.Add("kartaBalikNepouzity",parent.kartaBalikNepouzity);
                                    tmp.Add("kartaBalikVyhozeny", parent.kartaBalikVyhozeny);
                                    tmp.Add("hraciZivoty", parent.hraciZivoty);
                                    tmp.Add("kartaKartyUHracu", parent.kartaKartyUHracu);
                                    tmp.Add("poradi", parent.poradi);
                                    tmp.Add("serif", parent.serif);
                                    parent.slovnikProUlozeni.Add(".serverovy_slovnik", tmp);
                               
                                    biFormatter.Serialize(bw.BaseStream, parent.slovnikProUlozeni);
                                    verdict = "Uložení proběhlo v pořádku.";
                                }
                                Console.WriteLine(verdict + "(15)");
                            }

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error" + "(15)");
                        }
                    }
                    #endregion

                    #region načtení hry (16)
                    if (transit.obsah == 16)
                    {
                        if (transit.text.CompareTo("") == 0) //hráč si vyžádal seznam her na načtení
                        {
                            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\save\");
                            FileInfo[] fi = di.GetFiles("*.bng");
                            transit.objekt = fi;
                            Send(transit);
                        }
                        else  //hráč si přeje nahrát danou hru
                        {
                            FileInfo fi = (FileInfo)transit.objekt;
                            Dictionary<string, Dictionary<string, object>> ulozenaHra = null;
                            using (BinaryReader br = new BinaryReader(fi.Open(FileMode.Open)))
                            {
                                BinaryFormatter biFormatter = new BinaryFormatter();

                                ulozenaHra = (Dictionary<string, Dictionary<string, object>>)biFormatter.Deserialize(br.BaseStream);
                            }
                            bool mistake = false;
                            foreach (var klient in parent.klienti)
                            {
                                if (!ulozenaHra.ContainsKey(klient.name)) mistake = true;
                            }
                            if (mistake)
                            {
                                Transit t = new Transit();
                                t.obsah = 18;
                                t.text = "chyba";
                                Send(t);
                            }
                            else
                            {
                                
                                Dictionary<string, object> pom;
                              
                                ulozenaHra.TryGetValue(".serverovy_slovnik", out pom);
                                object pomm;
                                pom.TryGetValue("kartaBalikNepouzity", out pomm);
                                parent.kartaBalikNepouzity = (ListKaret<Karta>)pomm;
                                pom.TryGetValue("kartaBalikVyhozeny", out pomm);
                                parent.kartaBalikVyhozeny = (ListKaret<Karta>)pomm;
                                pom.TryGetValue("hraciZivoty", out pomm);
                                parent.hraciZivoty = (Dictionary<string,int>)pomm;
                                pom.TryGetValue("kartaKartyUHracu",out pomm);
                                parent.kartaKartyUHracu = (Dictionary<String, Dictionary<String, ListKaret<Karta>>>)pomm;
                                pom.TryGetValue("poradi",out pomm);
                                parent.poradi = (Dictionary<string,int>)pomm;
                                pom.TryGetValue("serif",out pomm);
                                parent.serif = (int)pomm;
                          
                                foreach (var klient in parent.klienti)
                                {
                                    Transit t = new Transit();
                                    t.obsah = 18;
                                    t.text = "";                                    
                                    ulozenaHra.TryGetValue(klient.name, out pom);
                                    t.objekt = pom;
                                    klient.Send(t);

                                    Dictionary<string, object> slovnik = (Dictionary<string, object>)pom;
                                    Dictionary<String, ListKaret<Karta>> karty = new Dictionary<string, ListKaret<Karta>>();
                                    slovnik.TryGetValue("kartyNaRuce", out pomm);
                                    karty.Add("ruka", (ListKaret<Karta>)pomm);
                                    slovnik.TryGetValue("kartyNaStole", out pomm);
                                    karty.Add("stul", (ListKaret<Karta>)pomm);

                                    if (parent.kartaKartyUHracu.ContainsKey(this.name))
                                    {
                                        parent.kartaKartyUHracu.Remove(this.name);
                                        //Console.WriteLine("kartaKartyUHracu obsahoval " + this.name);
                                    }
                                    parent.kartaKartyUHracu.Add(this.name, karty);
                                }

                                foreach (var klient in parent.klienti)
                                {
                                    posli.obsah = 4;
                                    posli.objekt = parent.kartaKartyUHracu;
                                    foreach (var item in parent.klienti)
                                    {
                                        item.Send(posli);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region aktualizace životů (17)
                    if (transit.obsah == 17) //prijimam aktuali zivoty
                    {
                        Dictionary<String, int> hh = new Dictionary<string, int>();
                        foreach (var item in parent.hraciZivoty)
                        {
                            if (item.Key == this.name)
                            {
                                hh.Add(item.Key, int.Parse(transit.text));
                            }
                            else
                            {
                                hh.Add(item.Key, item.Value);
                            }
                        }
                        parent.hraciZivoty = hh;
                        posli.obsah = 17;
                        posli.objekt = parent.hraciZivoty;
                        foreach (var item in parent.klienti)
                        {
                            if (item.name == this.name)
                            {
                                continue;
                            }
                            else item.Send(posli);
                        }
                    }
                    #endregion

                    #region prijeti funkce mrtveho hrace (19)
                    if (transit.obsah == 19)
                    {
                        posli.obsah = 19;
                        FunkceKarta fk = (FunkceKarta)transit.objekt;
                        posli.text = this.name;
                        parent.mrtviHraci.Add(this.name);
                        posli.objekt = (FunkceKarta)transit.objekt;
                        foreach (var item in parent.klienti)
                        {
                            if (item.name == this.name) continue;//at to neposilam sam sobe
                            item.Send(posli);
                        }
                        if (parent.mrtviHraci.Count >= parent.klienti.Count - 1)
                        {
                            //zbyl posledni hrac, konec hry !
                            posli.text = "Zustal posledni hrac ve hre, konec hry";//vyhrali ???
                            posli.obsah = 21;
                            foreach (var item in parent.klienti)
                            {
                                item.Send(posli);
                            }
                            parent.serverReinicialize();
                        }
                        if (fk.Funkce == 1)
                        {
                            //Serif je mrtev, konec hry
                            posli.text = "Serif je mrtev, konec hry";//vyhrali ???
                            posli.obsah = 21;
                            foreach (var item in parent.klienti)
                            {
                                item.Send(posli);
                            }
                        }
                    }
                    #endregion

                    #region načtení minulého autosavu (20)
                    if (transit.obsah == 20)
                    {
                        parent.autoSave--;
                        if (parent.autoSave < 0) return;
                        FileInfo fi = new FileInfo(Directory.GetCurrentDirectory() + @"\autosave\" + @"autoSave" + parent.autoSave.ToString() + ".bng");
                        Console.WriteLine("Načtení: " + fi.Name);
                        Dictionary<string, Dictionary<string, object>> ulozenaHra = null;
                        using (BinaryReader br = new BinaryReader(fi.Open(FileMode.Open)))
                        {
                            BinaryFormatter biFormatter = new BinaryFormatter();

                            ulozenaHra = (Dictionary<string, Dictionary<string, object>>)biFormatter.Deserialize(br.BaseStream);
                        }

                        Dictionary<string, object> pom;
                        
                        ulozenaHra.TryGetValue(".serverovy_slovnik", out pom);
                        object pomm;
                        /*
                        pom.TryGetValue("kartaBalikNepouzity", out pomm);
                        parent.kartaBalikNepouzity = (ListKaret<Karta>)pomm;
                        pom.TryGetValue("kartaBalikVyhozeny", out pomm);
                        parent.kartaBalikVyhozeny = (ListKaret<Karta>)pomm;
                        pom.TryGetValue("hraciZivoty", out pomm);
                        parent.hraciZivoty = (Dictionary<string, int>)pomm;
                        pom.TryGetValue("kartaKartyUHracu", out pomm);
                        parent.kartaKartyUHracu = (Dictionary<String, Dictionary<String, ListKaret<Karta>>>)pomm;
                        pom.TryGetValue("poradi", out pomm);
                        parent.poradi = (Dictionary<string, int>)pomm;
                        pom.TryGetValue("serif", out pomm);
                        parent.serif = (int)pomm;
                        */
                        
                        foreach (var klient in parent.klienti)
                        {
                            Transit t = new Transit();
                            t.obsah = 18;
                            t.text = "";
                            ulozenaHra.TryGetValue(klient.name, out pom);
                            t.objekt = pom;
                            klient.Send(t);

                            Dictionary<String, ListKaret<Karta>> karty = new Dictionary<string, ListKaret<Karta>>();
                            pom.TryGetValue("kartyNaRuce", out pomm);
                            karty.Add("ruka", (ListKaret<Karta>)pomm);
                            pom.TryGetValue("kartyNaStole", out pomm);
                            karty.Add("stul", (ListKaret<Karta>)pomm);

                            if (parent.kartaKartyUHracu.ContainsKey(this.name))
                            {
                                parent.kartaKartyUHracu.Remove(this.name);
                                //Console.WriteLine("kartaKartyUHracu obsahoval " + this.name);
                            }
                            parent.kartaKartyUHracu.Add(this.name, karty);
                        }

                        foreach (var klient in parent.klienti)
                        {
                            posli.obsah = 4;
                            posli.objekt = parent.kartaKartyUHracu;
                            foreach (var item in parent.klienti)
                            {
                                item.Send(posli);
                            }
                        }
                        fi.Delete();
                    }
                    #endregion

                    #region nová hra (22)
                    if (transit.obsah == 22)
                    {
                        foreach (var item in parent.klienti)
                        {
                            if (item.name == this.name) continue;//at to neposilam sam sobe
                            item.Send(posli);
                        }

                        parent.serverReinicialize();
                        parent.serverZahajHru();
                    }
                    #endregion
                }
            }
        }
    }
}

