using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bang_.Spolecne.Karty;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bang_.Spolecne
{
    public partial class Hra : Form
    {        
        static Point stred;

        public Spojeni spojeni;
        private String jmeno, nazevSouboru;
        private Thread vlakno;
        private Dictionary<String, int> poradi; // {Dictionary s pořadím hráčů}
        private FunkceKarta funkce;
        private KartaImage<FunkceKarta> mojeFunkce;
        private ListKaret<Karta> special; //indexy 0-neznamaFunkce,1-serif,2-patrony,3-NeznamaKarta
        private Dictionary<string, Karta> kartyFunkceHracu = new Dictionary<string, Karta>();
        private ListKaret<Karta> kartyNaRuce, kartyNaStole;
        private List<String> dosazitelniHraci;  //seznam hráčů, na které dostřelím/vidím
        public Dictionary<String, PostavaKarta> postavyHracu = new Dictionary<String, PostavaKarta>();
        public List<KartaImage<PostavaKarta>> postavy = new List<KartaImage<PostavaKarta>>();
        public List<KartaImage<Karta>> kartyNaFormu = new List<KartaImage<Karta>>();
        public List<KartaImage<Karta>> kartyNaFormuMoje = new List<KartaImage<Karta>>();
        private List<KartaImage<PostavaKarta>> kartyNaFormuPostavy = new List<KartaImage<PostavaKarta>>();
        public Dictionary<String, KartaImage<PostavaKarta>> kartyPostavyHracu = new Dictionary<String, KartaImage<PostavaKarta>>();
        public int maxZivotu, aktZivotu, posunOd = 0, serif, posunK = 0, dostrel = 1;
        public KartaImage<PostavaKarta> kartaPostavaMoje;
        private Dictionary<String, int> aktZivotyHracu = new Dictionary<string, int>();
        public Boolean volcanic, schVolcanic, barel, schBarel, schBangVedle, schDveVedle, zahranyBang, pistol=false;
        public Boolean aktivniTah=false, odhazovaniKaret=false;
        private PostavaKarta postava;
        public KartaImage<Karta> KartaNaOdhazovacimBalicku; //Predelat na Karta!!!
        public Karta KartaNaOdhazovBal;
        private Point nahledPoz;
        public string vybran;
        private Karta KartaPrijata;
        private Boolean repaintDisabled = false;
        private KartaImage<PostavaKarta> kartPostav;
        private KartaImage<Karta> kartPatr;


        public PostavaKarta Postava //uložení postavy         
        {
            get{ return postava; }
            set
            {
                postava = value;
            }
        }
        
        public Hra(Spojeni s,String jmeno)
        {
            this.spojeni = s;
            this.jmeno = jmeno;
            InitializeComponent();
            bKonec.Enabled = false;
            btn_ZahodKarty.Visible = false;
            stred = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
            this.Text = "Hráč "+jmeno;
            nahledPoz = new Point(KartaNahled.Location.X + KartaNahled.Size.Width, KartaNahled.Location.Y);
            KartaNahled.Location = nahledPoz;
            this.DoubleBuffered = true;
            kartyNaRuce = new ListKaret<Karta>(0);
            kartyNaStole = new ListKaret<Karta>(0);
            special = new ListKaret<Karta>(4);
            this.aktZivotu = 0;
            volcanic = false;
            schVolcanic = false;
            

            vlakno = new Thread(new ThreadStart(Naslouchac));
            vlakno.Start();
        }

        #region Chat
        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)   
        {
            if (e.KeyChar == (int)Keys.Enter)   //při zmáčknutí anteru se zpráva pošle na server
            {
                Transit chat = new Transit();
                chat.obsah = 1;
                chat.text = tbInput.Text;
                tbInput.Text = "";
                spojeni.Send(chat);
            }
        }

        private void ZobrazChat(String s)
        {
            tbChat.Text = s;
        }
        #endregion
        
        private void Naslouchac()   // přijímá zpávy od serveru a dále je zpracovává
        {
            Transit transit;
            while (true)
            {
                transit = spojeni.Recieve();
                Console.WriteLine("transit: " + transit.obsah);
                if (transit.obsah == 101)    // přijal funkci
                {
                    funkce =(FunkceKarta)transit.objekt;
                    Console.WriteLine(funkce.Nazev);
                    aktZivotu = 0;
                    if (funkce.Nazev.Equals("Serif")) aktZivotu++;
                    KartaImage<FunkceKarta> ki = new KartaImage<FunkceKarta>(this, funkce, new Point(300,ClientSize.Height-130), new Size(new Point(80,120)),1,true);
                    this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(ki); }));
                    this.Invoke(new MethodInvoker(delegate() { ki.BringToFront(); }));                    
                    mojeFunkce = ki;
                }
                if (transit.obsah == 105)
                {
                    serif = (int)transit.objekt;
                }
                if (transit.obsah == 106) //prijaty vsechny postavy a zobrazeni !!
                {
                    postavyHracu = (Dictionary<String, PostavaKarta>)transit.objekt;
                    int cislo = 0;
                    aktZivotyHracu = new Dictionary<string, int>();
                    kartyPostavyHracu = new Dictionary<string, KartaImage<PostavaKarta>>();
                    foreach (var item in postavyHracu)
                    {
                        //Console.WriteLine("jmeno hrace "+item.Key);
                        if (poradi.TryGetValue(item.Key,out cislo))
                        {
                            //Console.WriteLine("Poradi daneho hrace " + cislo);
                            if (cislo != 0)
                            {
                                KartaImage<Karta> pkk = new KartaImage<Karta>(this, special.ElementAt(2), new Point(105 + (180 * (cislo - 1)), 190 + (30 * 1)), new Size(new Point(80, 120)), 1, false, false);
                                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(pkk); }));
                                this.Invoke(new MethodInvoker(delegate() { pkk.BringToFront(); }));                                
                                kartyNaFormu.Add(pkk);

                                int ss = 0;
                                if (cislo == serif) ss = 1;
                                aktZivotyHracu.Add(item.Key, item.Value.PocetZivotu + ss); // pouzit na dostani aktZivotu
                                KartaImage<PostavaKarta> pk = new KartaImage<PostavaKarta>(this, item.Value, new Point(105 + (180 * (cislo - 1)), 200 + (30 * 1) + (item.Value.PocetZivotu+ss)*20), new Size(new Point(80, 120)), 1, true, false);
                                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(pk); }));
                                this.Invoke(new MethodInvoker(delegate() { pk.BringToFront(); }));
                                kartyNaFormuPostavy.Add(pk);
                                kartyPostavyHracu.Add(item.Key, pk);
                            }
                        }
                    }
                    
                }
                if (transit.obsah == 102)   // přijal výběr postav
                {
                    Console.WriteLine("(102) - Dostupné postavy:");
                    int i = 0;
                    postavy = new List<KartaImage<PostavaKarta>>();
                    foreach (PostavaKarta postava in transit.list)
                    {
                        Console.WriteLine(postava.Nazev);
                        KartaImage<PostavaKarta> ki = new KartaImage<PostavaKarta>(this, postava, new Point(stred.X - 95 + (i * 200), stred.Y),new Size(new Point(175,240)),1,false);
                        postavy.Add(ki);
                        this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(ki); }));
                        i++;

                        //porade poslat serveru co si sem si vybral at to rozesle ostatnim
                    }
                }
                if (transit.obsah == 103)   //prijal rozlozeni hracu
                {
                    poradi = (Dictionary<String, int>)transit.objekt; 
                    Dictionary<String, int> newPoradi = new Dictionary<string, int>();
                    int uprav, intItem;
                    bool jde = poradi.TryGetValue(this.jmeno, out uprav);
                    if (!jde)
                    {
                        throw new Exception("Nastala chyba. Slovník, který hra obdržela od herního serveru neobsahuje informace pro vaší hru. \nProsím kontaktujte výrobce aplikace.");
                    }
                    int posun = uprav;
                    if ((serif - uprav) < 0) serif += poradi.Count;
                    serif -= uprav;
                    foreach (var item in poradi.Keys)   // uprav je číslo o kolik se má "otočit" pořadí hráčů, aby se to dobře vykreslovalo
                    {
                        if (poradi.TryGetValue(item, out intItem))
                        {
                            if ((intItem - uprav) < 0)
                            {
                                intItem += poradi.Count;
                            }
                            intItem -= uprav;

                            newPoradi.Add(item, intItem);
                        }
                    }
                    poradi = newPoradi;
                    this.Invoke(new MethodInvoker(delegate() { NastavJmena(); }));   //Nastavi jmena podle poradi hracu
                }
                if (transit.obsah == 1)     // přijal novou zprávu (výpis zpráv)
                {
                    this.Invoke(new MethodInvoker(delegate() { ZobrazChat(transit.text); }));
                    //tbChat.Text = transit.text;
                }
                if (transit.obsah == 2)     // přijal novou kartu
                {
                    kartyNaRuce.Add((Karta)transit.objekt);
                    this.Invoke(new MethodInvoker(delegate() { Repaint(); }));
                    //tbChat.Text = transit.text;
                }
                if (transit.obsah == 3) //přijal, že má zahájit kolo
                {
                    StartKola();
                }
                if (transit.obsah == 4) //přijal nové rozložení karet na stole
                {
                    this.Invoke(new MethodInvoker(delegate() { Repaint((Dictionary<String, Dictionary<String, ListKaret<Karta>>>)transit.objekt); }));
                }
                if (transit.obsah == 5) //přijal nový seznam hráčů na který dostřelí
                {
                    Dictionary<string, int> slovnikPosunu = (Dictionary<string,int>)transit.objekt;
                    List<String> tmpDosazitelniHraci = new List<string>();
                    int dostrel = int.Parse((String)transit.text);
                    int myPoradi;
                    if (!poradi.TryGetValue(jmeno,out myPoradi)) { /*TODO error, že něco je blbě*/};
                    //TODO jestli je v poradi i nula, tak musí být ++ oba tryGetValue()!!!
                    int myVzdalenost, myTmpVzdalenost;;
                    foreach (var item in poradi.Keys)
                    {
                        poradi.TryGetValue(item, out myVzdalenost);
                        myTmpVzdalenost = Math.Abs(myPoradi - myVzdalenost);
                        if (myVzdalenost > myPoradi)
                        {
                            if (Math.Abs(myPoradi + poradi.Count - myVzdalenost) < myTmpVzdalenost)
                            {
                                myTmpVzdalenost = Math.Abs(myPoradi + poradi.Count - myVzdalenost);
                            }
                        }
                        else
                        {
                            if (Math.Abs(myPoradi - poradi.Count - myVzdalenost) < myTmpVzdalenost)
                            {
                                myTmpVzdalenost = Math.Abs(myPoradi - poradi.Count - myVzdalenost);
                            }
                        }
                        int posun = 0;
                        slovnikPosunu.TryGetValue(item,out posun);
                        if (myTmpVzdalenost == 0) continue;
                        
                        if ((dostrel + posunK) >= myTmpVzdalenost + posun)
                        {
                            tmpDosazitelniHraci.Add(item);
                        }
                    }
                    dosazitelniHraci = tmpDosazitelniHraci;

                }
                if (transit.obsah == 6) //někdo na mě vystřelil
                {
                    Thread tr = new Thread(new ThreadStart(ZasahThread));
                    tr.Start(); // vlákno, protože musíme zárověň příjimat všechny zprávy od serveru

                    //this.Invoke(new MethodInvoker(delegate() { Zasah(transit.text); }));
                    System.Console.WriteLine("byl jsem zasažen od "+ transit.text + "(6)");
                }
                if (transit.obsah == 7)  // mám poslat na server, změnu mé vzdálenosti
                {
                    transit.list = new List<object>();
                    transit.list.Add(posunOd);
                    transit.list.Add(posunK);
                    spojeni.Send(transit);
                }
                if (transit.obsah == 8) //odhozena karta
                {
                    this.Invoke(new MethodInvoker(delegate() { KartaDoBalicku((Karta)transit.objekt); }));
                }
                if (transit.obsah == 9) //zahrán salon, přidá život
                {
                    this.Invoke(new MethodInvoker(delegate() { Pivo(); }));
                }
                if (transit.obsah == 12) //jde na mě indián
                {
                    this.Invoke(new MethodInvoker(delegate() { Indian(transit.text); }));
                }
                if (transit.obsah == 13) //přijmi kartu, pro potřebu něčeho jiného, než dát jí do ruky a ulož do KartaPrijata
                {
                    KartaPrijata = (Karta)transit.objekt;

                }
                if (transit.obsah == 14) //poslat na server všechny údaje pro uložení stavu hry
                {
                    Dictionary<string, object> slovnik = new Dictionary<string, object>();
                    slovnik.Add("jmeno", jmeno);
                    slovnik.Add("poradi", poradi);
                    slovnik.Add("funkce", funkce);
                    slovnik.Add("special", special);
                    slovnik.Add("kartyNaRuce", kartyNaRuce);
                    slovnik.Add("kartyNaStole", kartyNaStole);
                    slovnik.Add("dosazitelniHraci", dosazitelniHraci);
                    slovnik.Add("postavyHracu", postavyHracu);
                    slovnik.Add("maxZivotu", maxZivotu);
                    slovnik.Add("aktZivotu", aktZivotu);
                    slovnik.Add("posunOd", posunOd);
                    slovnik.Add("posunK", posunK);
                    slovnik.Add("serif", serif);
                    slovnik.Add("dostrel", dostrel);
                    slovnik.Add("volcanic", volcanic);
                    slovnik.Add("schVolcanic", schVolcanic);
                    slovnik.Add("barel", barel);
                    slovnik.Add("schBarel", schBarel);
                    slovnik.Add("schBangVedle", schBangVedle);
                    slovnik.Add("schDveVedle", schDveVedle);
                    slovnik.Add("zahranyBang", zahranyBang);
                    slovnik.Add("pistol", pistol);
                    slovnik.Add("aktZivotyHracu", aktZivotyHracu);
                    slovnik.Add("aktivniTah", aktivniTah);
                    slovnik.Add("odhazovaniKaret", odhazovaniKaret);
                    slovnik.Add("kartaNaOdhazovacimBalicku", KartaNaOdhazovBal);
                    slovnik.Add("postava", postava);
                    slovnik.Add("kartyFunkceHracu", kartyFunkceHracu);
                    slovnik.Add("btnKonec", bKonec.Enabled);

                    Transit t = new Transit();
                    t.obsah = 15;
                    t.objekt = slovnik;
                    spojeni.Send(t);
                }
                if (transit.obsah == 16) //dorazil seznam uložených her na serveru
                {
                    this.Invoke(new MethodInvoker(delegate() { HraPanel hp = new HraPanel((FileInfo[])transit.objekt, this); }));                    
                }
                if (transit.obsah == 17) //aktualizace  zivotu
                {
                    aktZivotyHracu = (Dictionary<string, int>)transit.objekt;
                    this.Invoke(new MethodInvoker(delegate() { Repaint(); }));
                }
                if (transit.obsah == 18) //má načíst hru
                {
                    if (transit.text.CompareTo("chyba") == 0)
                    {
                        this.Invoke(new MethodInvoker(delegate() { HraPanel hp = new HraPanel("Hru nelze načíst, protože nesouhlasí jména hráčů.", this); }));
                    }
                    else 
                    {
                        Dictionary<string, object> slovnik = (Dictionary<string, object>)transit.objekt;
                        object pom;
                        slovnik.TryGetValue("jmeno", out pom);
                        jmeno = (string)pom;
                        slovnik.TryGetValue("funkce", out pom);
                        funkce = (FunkceKarta)pom;
                        slovnik.TryGetValue("poradi", out pom);
                        poradi = (Dictionary<string,int>)pom;
                        slovnik.TryGetValue("funkce", out pom);
                        funkce = (FunkceKarta)pom;
                        slovnik.TryGetValue("special", out pom);
                        special = (ListKaret<Karta>)pom;
                        slovnik.TryGetValue("kartyNaRuce", out pom);
                        kartyNaRuce = (ListKaret<Karta>)pom;
                        slovnik.TryGetValue("kartyNaStole", out pom);
                        kartyNaStole = (ListKaret<Karta>)pom;
                        slovnik.TryGetValue("dosazitelniHraci", out pom);
                        dosazitelniHraci = (List<string>)pom;
                        slovnik.TryGetValue("postavyHracu", out pom);
                        postavyHracu = (Dictionary<string, PostavaKarta>)pom;
                        slovnik.TryGetValue("maxZivotu", out pom);
                        maxZivotu = (int)pom;
                        slovnik.TryGetValue("aktZivotu", out pom);
                        aktZivotu = (int)pom;
                        slovnik.TryGetValue("posunOd", out pom);
                        posunOd = (int)pom;
                        slovnik.TryGetValue("posunK", out pom);
                        posunK = (int)pom;
                        slovnik.TryGetValue("serif", out pom);
                        serif = (int)pom;
                        slovnik.TryGetValue("dostrel", out pom);
                        dostrel = (int)pom;
                        slovnik.TryGetValue("volcanic", out pom);
                        volcanic = (bool)pom;
                        slovnik.TryGetValue("schVolcanic", out pom);
                        schVolcanic = (bool)pom;
                        slovnik.TryGetValue("barel", out pom);
                        barel = (bool)pom;
                        slovnik.TryGetValue("schBarel", out pom);
                        schBarel = (bool)pom;
                        slovnik.TryGetValue("schBangVedle", out pom);
                        schBangVedle = (bool)pom;
                        slovnik.TryGetValue("schDveVedle", out pom);
                        schDveVedle = (bool)pom;
                        slovnik.TryGetValue("zahranyBang", out pom);
                        zahranyBang = (bool)pom;
                        slovnik.TryGetValue("pistol", out pom);
                        pistol = (bool)pom;
                        slovnik.TryGetValue("aktZivotyHracu", out pom);
                        aktZivotyHracu = (Dictionary<string,int>)pom;
                        slovnik.TryGetValue("aktivniTah", out pom);
                        aktivniTah = (bool)pom;
                        slovnik.TryGetValue("odhazovaniKaret", out pom);
                        odhazovaniKaret = (bool)pom;
                        slovnik.TryGetValue("kartaNaOdhazovacimBalicku", out pom);
                        KartaNaOdhazovBal = (Karta)pom;// Predelat !!
                        slovnik.TryGetValue("postava", out pom);
                        postava = (PostavaKarta)pom;
                        slovnik.TryGetValue("kartyFunkceHracu", out pom);
                        kartyFunkceHracu = (Dictionary<string, Karta>)pom;
                        slovnik.TryGetValue("btnKonec", out pom);
                        bKonec.Enabled = (bool)pom;

                        this.Invoke(new MethodInvoker(delegate() { Repaint(); }));

                        foreach (var item in poradi)
                        {
                            switch (item.Value)
                            {
                                case 1:
                                    lName1.Text = item.Key;
                                    break;
                                case 2:
                                    lName2.Text = item.Key;
                                    break;
                                case 3:
                                    lName3.Text = item.Key;
                                    break;
                                case 4:
                                    lName4.Text = item.Key;
                                    break;
                                case 5:
                                    lName5.Text = item.Key;
                                    break;
                            }
                        }

                    }
                }
                if (transit.obsah == 19) //prijal funkci mrtveho hrace
                {
                    Dictionary<string, Karta> hhelp = new Dictionary<string, Karta>();
                    hhelp.Add(transit.text, (FunkceKarta)transit.objekt);
                    foreach (var item in kartyFunkceHracu)//vlozi prijatou funkci a pak ostatni
                    {
                        if (hhelp.ContainsKey(item.Key)) continue;
                        else
                        {
                            hhelp.Add(item.Key, item.Value);
                        }
                    }
                    kartyFunkceHracu = hhelp;                    
                }
                if (transit.obsah == 21) //serif padl a nebo zbyl jeden hrac, konec hry
                {
                    this.Invoke(new MethodInvoker(delegate() { 
                        //this.Controls.Clear();
                        Console.WriteLine("pocet controls je " + this.Controls.Count);
                        foreach (var item in kartyNaFormu)
                        {
                            this.Controls.Remove(item);
                        }
                        foreach (var item in kartyNaFormuMoje)
                        {
                            this.Controls.Remove(item);
                        }
                        foreach (var item in kartyNaFormuPostavy)
                        {
                            this.Controls.Remove(item);
                        }
                        this.Controls.Remove(mojeFunkce);
                        this.Controls.Remove(mojeFunkce);
                        this.Controls.Remove(kartPostav);
                        this.Controls.Remove(kartPatr);
                        this.Controls.Remove(KartaNaOdhazovacimBalicku);
                        this.Controls.Remove(KartaNaOdhazovacimBalicku);
                        if (KartaNaOdhazovacimBalicku != null) this.Controls.Remove(KartaNaOdhazovacimBalicku);
                        konecHryReinicializace();
                        new HraPanel(transit.text, this);
                    }));
                }
                if (transit.obsah == 22) //serif padl a nebo zbyl jeden hrac, konec hry
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        //this.Controls.Clear();
                        Console.WriteLine("pocet controls je " + this.Controls.Count);
                        foreach (var item in kartyNaFormu)
                        {
                            this.Controls.Remove(item);
                        }
                        foreach (var item in kartyNaFormuMoje)
                        {
                            this.Controls.Remove(item);
                        }
                        foreach (var item in kartyNaFormuPostavy)
                        {
                            this.Controls.Remove(item);
                        }
                        this.Controls.Remove(mojeFunkce);
                        this.Controls.Remove(mojeFunkce);
                        this.Controls.Remove(kartPostav);
                        this.Controls.Remove(kartPatr);
                        this.Controls.Remove(KartaNaOdhazovacimBalicku);
                        this.Controls.Remove(KartaNaOdhazovacimBalicku);
                        kartyFunkceHracu = new Dictionary<string, Karta>();
                        if (KartaNaOdhazovacimBalicku != null) this.Controls.Remove(KartaNaOdhazovacimBalicku);
                        konecHryReinicializace();
                    }));
                }
            }
        }

        private void konecHryReinicializace()
        {
            bKonec.Enabled = false;
            poradi = new Dictionary<String, int>(); // {Dictionary s pořadím hráčů}
            this.kartyFunkceHracu = new Dictionary<string, Karta>();
            kartyNaRuce = new ListKaret<Karta>(0); kartyNaStole = new ListKaret<Karta>(0);
            postavyHracu = new Dictionary<String, PostavaKarta>();
            postavy = new List<KartaImage<PostavaKarta>>();
            this.kartyNaFormu = new List<KartaImage<Karta>>();
            this.kartyNaFormuMoje = new List<KartaImage<Karta>>();
            kartyNaFormuPostavy = new List<KartaImage<PostavaKarta>>();
            kartyPostavyHracu = new Dictionary<String, KartaImage<PostavaKarta>>();
            posunOd = 0; serif = 0; posunK = 0; dostrel = 1;
            this.maxZivotu = 0; this.aktZivotu = 0;
            aktZivotyHracu = new Dictionary<string, int>();
            volcanic = false; schVolcanic= false; barel = false; schBarel = false; schBangVedle = false; schDveVedle = false; zahranyBang = false; pistol=false;
            aktivniTah = false; odhazovaniKaret = false;
            repaintDisabled = true;
        }
        public void OdectiZivot()
        {
            this.aktZivotu--;
            Transit t;
            t = new Transit();
            t.obsah = 17;
            t.text = aktZivotu.ToString();
            spojeni.Send(t);
            if (aktZivotu <= 0) 
            {
                Karta[] help = new Karta[kartyNaRuce.Count];
                for (int i = 0; i < help.Length; i++)
                {
                    help[i] = kartyNaRuce.ElementAt(i).CopyKart();
                }
                foreach (var item in help)
                {
                    if (item.Nazev.CompareTo("Pivo") == 0)
                    {   
                        if(aktZivotu==1)break;
                        aktZivotu++; //posmrtne pivo
                        ZahodKartuZRuky(item);
                        t = new Transit();
                        t.obsah = 17;
                        t.text = aktZivotu.ToString();
                        spojeni.Send(t);
                    }
                }
                if (aktZivotu <= 0)//jsem mrtev //TODO hlaska ze si mrtev
                    //TODO pokud sem serif nebo posledni tak konec hry!!
                {
                    help = new Karta[kartyNaRuce.Count]; 
                    kartyNaRuce.CopyTo(help);
                    foreach (var item in help)
                    {
                        ZahodKartuZRuky(item);
                    }
                    help = new Karta[kartyNaStole.Count]; 
                    kartyNaStole.CopyTo(help);
                    foreach (var item in help)
                    {
                        ZahodKartuZeStolu(item);
                    }
                    t.obsah = 4;
                    Dictionary<String, ListKaret<Karta>> karty = new Dictionary<string, ListKaret<Karta>>();
                    karty.Add("ruka", kartyNaRuce);
                    karty.Add("stul", kartyNaStole);
                    t.objekt = karty;
                    spojeni.Send(t);

                    t = new Transit();
                    t.obsah = 19; //server si zjisti co sem byl a podle toho ukonci hru !
                    t.objekt = funkce;
                    spojeni.Send(t);
                }
            }

            System.Console.WriteLine("aktuální počet životů je: " + aktZivotu);
            Repaint();  //aby se vykreslil správný počet životů
                            //TODO udělat to animací
        }

        private void Indian(String nazevHrace) //běží na mě indián
        {
            bool mamBang = false;
            Karta karta = null;

            foreach (var kar in this.kartyNaRuce) //zjistí, jestli mám kartu Bang
            {
                if (kar.Nazev.CompareTo("Bang") == 0)
                {
                    mamBang = true;
                    karta = kar;
                    break;
                }
            }

            if (mamBang == false) //když ne, tak mi odečte život
            {
                OdectiZivot(); 
            }
            else //když jo, takase mě zeptá, jestli ji má použít
            {
                HraPanel hp = new HraPanel("indian", this, karta);
            }

        }

        private void ZasahThread() //kvůli barelu, který musí vzít ze serveru kartu
        {
            //this.Invoke(new MethodInvoker(delegate() {  }));
            Zasah("t");
        }

        private void Zasah(String nazevHrace) //zpracuje to, když na mě někdo vystřelil
        {
            bool minul = false, vedle = false;
            Karta karta = null;
            //zjistíme, jestli se nějak nemůžeme bránit
            int bar = 0;
            if (this.barel) bar++;
            if (this.schBarel) bar++;

            for (int i = 0; i < bar; i++) //pokud máme "nějaký" barel
            {
                Transit t = new Transit();
                t.obsah = 13;
                KartaPrijata = null;
                spojeni.Send(t);
                while (KartaPrijata == null) { Thread.Sleep(50); }
                if (KartaPrijata.Cislo.InColour("srdce"))
                {
                    minul = true;
                }
                this.Invoke(new MethodInvoker(delegate() { ZahodKartu(KartaPrijata); }));                
            }

            foreach (var kar in kartyNaRuce)
            {
                if (kar.Nazev.CompareTo("Vedle") == 0)
                {
                    karta = kar;
                    vedle = true;
                    break;
                }
            }

            if ((vedle == true) && (minul == false)) //pokud máme na ruce vedle
            {
                //hláška, jestli má použít vedle z ruky
                this.Invoke(new MethodInvoker(delegate() { HraPanel hp = new HraPanel("strela" + nazevHrace, this, karta); }));
                //HraPanel hp = new HraPanel("strela" + nazevHrace, this, karta);
            }

            if (!minul && (vedle == false)) //?? - co to ma delat? odepisuje to zivot navic!
            {
                //System.Console.WriteLine(nazevHrace + " mě zasáhnul - mám o život míň");
                //Console.WriteLine("---- volam OdectiZivot() , radek 370 ---- ");
                this.Invoke(new MethodInvoker(delegate() { OdectiZivot(); }));                
            }
        }

        private void KartaDoBalicku(Karta k) //vykresli kartu co je zahozena do balicku a otoci ji o 270°
        {
            this.Controls.Remove(KartaNaOdhazovacimBalicku);
            KartaNaOdhazovBal = k;
            KartaNaOdhazovacimBalicku = new KartaImage<Karta>(this, k, new Point(ClientSize.Width - 120, ClientSize.Height / 2 + 90), new Size(new Point(120, 80)), 1, true);
            this.Controls.Add(KartaNaOdhazovacimBalicku);
            KartaNaOdhazovacimBalicku.BringToFront();
            KartaNaOdhazovacimBalicku.Image = (Image)KartaNaOdhazovacimBalicku.Image.Clone();
            KartaNaOdhazovacimBalicku.Image.RotateFlip(RotateFlipType.Rotate270FlipXY);
        }

        // překreslí jen moje karty na formuláři
        private void Repaint()
        {
            if (repaintDisabled) return;
            if (KartaNaOdhazovBal != null)
            {
                KartaDoBalicku(KartaNaOdhazovBal);
            }
            foreach (var item in kartyNaFormuMoje)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in kartyNaFormuPostavy)
            {
                this.Controls.Remove(item);
            }
            this.Controls.Remove(mojeFunkce);
            KartaImage<FunkceKarta> ki = new KartaImage<FunkceKarta>(this, funkce, new Point(300, ClientSize.Height - 130), new Size(new Point(80, 120)), 1, true);
            this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(ki); }));
            this.Invoke(new MethodInvoker(delegate() { ki.BringToFront(); }));
            mojeFunkce = ki;
            foreach (var por in poradi)// prekresleni aktualnich zivotu
            {
                PostavaKarta tmp;
                if(postavyHracu.TryGetValue(por.Key,out tmp))
                {
                    int ziv;
                    if (aktZivotyHracu.TryGetValue(por.Key, out ziv))
                    {
                        KartaImage<PostavaKarta> pk = new KartaImage<PostavaKarta>(this, tmp, new Point(105 + (180 * (por.Value - 1)), 200 + (30 * 1) + (ziv) * 20), new Size(new Point(80, 120)), 1, true, false);
                        this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(pk); }));
                        pk.BringToFront();
                        kartyNaFormuPostavy.Add(pk);
                    }
                }
            }
            mojeFunkce.BringToFront();
            mojeFunkce.Location = new Point(300-95,ClientSize.Height-130-135);
            int ii = 0;
            KartaImage<Karta> k;
            foreach (var iitem in kartyNaStole)
            {
                iitem.Owner = this.Name;
                k = new KartaImage<Karta>(this, iitem, new Point(480 + (30 * ii++), ClientSize.Height - 130), new Size(new Point(80, 120)), 1, true);
                this.Controls.Add(k);
                kartyNaFormuMoje.Add(k);
            }
            ii = 0;
            foreach (var iitem in kartyNaRuce)
            {
                iitem.Owner = this.Name;
                k = new KartaImage<Karta>(this, iitem, new Point(480 + (30 * ii++), ClientSize.Height), new Size(new Point(80, 120)), 1, true);
                this.Controls.Add(k);
                kartyNaFormuMoje.Add(k);
            }
            // karta tve postavy a aktualni zivoty
            k = new KartaImage<Karta>(this, special.ElementAt(2), new Point(390 + (30 * 0), ClientSize.Height - 130), new Size(new Point(80, 120)), 1, false);
            this.Controls.Add(k);
            kartyNaFormuMoje.Add(k);
            KartaImage<PostavaKarta> kk = new KartaImage<PostavaKarta>(this, Postava, new Point(390 + (30 * 0), ClientSize.Height - 130 + (aktZivotu * 20) + 10), new Size(new Point(80, 120)), 1, true);
            this.Controls.Remove(kartaPostavaMoje);
            this.Controls.Add(kk);
            kartaPostavaMoje = kk;
            kk.BringToFront();
        }

        // překreslí všechny karty na formuláři
        private void Repaint(Dictionary<String, Dictionary<String, ListKaret<Karta>>> kartaKartyUHracu)
        {

            // TODO dodelat smazani a prekresleni karet(stav zivotu) z listu postavyOstatnichHracu 
            foreach (var item in kartyNaFormu)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in kartyNaFormuMoje)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in kartyNaFormuPostavy)
            {
                this.Controls.Remove(item);
            }
            KartaImage<Karta> k;
            kartyNaFormu = new List<KartaImage<Karta>>();
            kartyNaFormuMoje = new List<KartaImage<Karta>>();
            int i = 0;

            foreach (var por in poradi) //prekresleni funkcnich karet
            {
                if (por.Value == serif)
                {
                    if (por.Value == 0) continue;
                    k = new KartaImage<Karta>(this, special.ElementAt(1), new Point(105 + (180 * (por.Value - 1)), 190 + (30 * 0)), new Size(new Point(80, 120)), 1, true);
                    this.Controls.Add(k);
                    kartyNaFormu.Add(k);
                    k.BringToFront();
                }
                else
                {
                    if (por.Value == 0) continue;
                    Karta kart;
                    kartyFunkceHracu.TryGetValue(por.Key, out kart);
                    k = new KartaImage<Karta>(this, kart, new Point(105 + (180 * (por.Value - 1)), 190 + (30 * 0)), new Size(new Point(80, 120)), 1, true);
                    this.Controls.Add(k);
                    kartyNaFormu.Add(k);
                    k.BringToFront();
                }
                if (por.Value != 0) // vykresleni postav a zivotu ostatnich hracu podle aktZivotyHracu
                {
                    KartaImage<Karta> pkk = new KartaImage<Karta>(this, special.ElementAt(2), new Point(105 + (180 * (por.Value - 1)), 190 + (30 * 1)), new Size(new Point(80, 120)), 1, false, false);
                    this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(pkk); }));
                    pkk.BringToFront();
                    kartyNaFormu.Add(pkk);

                    PostavaKarta tmp;
                    if(postavyHracu.TryGetValue(por.Key,out tmp))
                    {
                        int ziv;
                        if (aktZivotyHracu.TryGetValue(por.Key, out ziv))
                        {
                            KartaImage<PostavaKarta> pk = new KartaImage<PostavaKarta>(this, tmp, new Point(105 + (180 * (por.Value - 1)), 200 + (30 * 1) + (ziv) * 20), new Size(new Point(80, 120)), 1, true, false);
                            this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(pk); }));
                            pk.BringToFront();
                            kartyNaFormuPostavy.Add(pk);
                        }
                    }
                }
            }
            //Console.WriteLine("Nové repaint, jména přijatých Hráčů:");
            foreach (var item in kartaKartyUHracu.Keys)
            {
                
                if (item.CompareTo(jmeno) != 0) //pokud to jsou cizí karty
                {
                    //Console.Write(item + " počet karet pro vykreslení je: ");
                    Dictionary<String, ListKaret<Karta>> dict;  // slovník s kartama jednotlivého hráče {ruka, stul}
                    if (kartaKartyUHracu.TryGetValue(item, out dict))
                    {
                        ListKaret<Karta> seznam;
                        foreach (var por in poradi)
                        {
                            if (por.Key.Equals(item)) i = por.Value - 1; //priradi prislusne i podle toho na jake pozici sedi dany hrac    
                        }
                        if (dict.TryGetValue("ruka", out seznam))
                        {
                            //Console.WriteLine(seznam.Count);
                            /*
                            foreach (var iiitem in seznam)
                            {
                                k = new KartaImage<Karta>(this, iiitem, new Point(105 + (180 * i), 160 + (30 * ii++)), new Size(new Point(80, 120)), 1, true);
                                this.Controls.Add(k);
                                kartyNaFormu.Add(k);
                                k.BringToFront();
                            }*/
                            for (int ls = 0; ls < seznam.Count; ls++)
                            {
                                k = new KartaImage<Karta>(this, special.ElementAt(3), new Point(105 + (180 * i) + ls * 20, 190 + (30 * 9)), new Size(new Point(80, 120)), 1, false);//karta bang zezadu
                                this.Controls.Add(k);
                                kartyNaFormu.Add(k);
                                k.BringToFront();
                            }
                        }
                        if (dict.TryGetValue("stul", out seznam))
                        {
                            int ii = 0;
                            foreach (var iiitem in seznam)
                            {
                                iiitem.Owner = item;
                                //Console.WriteLine("Vlastnikem teto modre vylozene karty je "+item);
                                k = new KartaImage<Karta>(this, iiitem, new Point(195 + (180 * i), 170 + (30 * ii++)), new Size(new Point(80, 120)), 1, true);
                                this.Controls.Add(k);
                                kartyNaFormu.Add(k);
                                k.BringToFront();
                            }
                        }
                    }
                    //i++; //zbytecne kdyz i nastavuju nahore 
                }
                else    // pokud vykreslujeme vlastní karty
                {
                    int ii = 0;
                    foreach (var iitem in kartyNaStole)
                    {
                        k = new KartaImage<Karta>(this, iitem, new Point(480 + (30 * ii++), ClientSize.Height - 130), new Size(new Point(80, 120)), 1, true);
                        this.Controls.Add(k);
                        kartyNaFormuMoje.Add(k);
                    }
                    ii = 0;
                    foreach (var iitem in kartyNaRuce)
                    {
                        k = new KartaImage<Karta>(this, iitem, new Point(480 + (30 * ii++), ClientSize.Height), new Size(new Point(80, 120)), 1, true);
                        this.Controls.Add(k);
                        kartyNaFormuMoje.Add(k);
                    }
                    // karta tve postavy a aktualni zivoty
                    k = new KartaImage<Karta>(this, special.ElementAt(2), new Point(390 + (30 * 0), ClientSize.Height - 130), new Size(new Point(80, 120)), 1, false);
                    this.Controls.Add(k);
                    kartyNaFormuMoje.Add(k);
                    this.Controls.Remove(kartaPostavaMoje);
                    KartaImage<PostavaKarta>kk = new KartaImage<PostavaKarta>(this, Postava, new Point(390 + (30 * 0), ClientSize.Height - 130 + (aktZivotu * 20) + 10), new Size(new Point(80, 120)), 1, true);
                    this.Controls.Add(kk);
                    kartaPostavaMoje = kk;
                    kk.BringToFront();
                }
            }
        }

        #region StartKola() a KonecKola()
        private void StartKola()
        {
            zahranyBang = false;
            aktivniTah = true;
            if (aktZivotu <= 0)
            {
                KonecKola(); //TODO vyresit dostrel, nejak me prehlidnout !!
                return;
            }
            Console.WriteLine("probíhá mé kolo!");

            Transit t = new Transit();
            t.obsah = 2;
            for (int i = 0; i < 2; i++)
            {
                spojeni.Send(t);
            }
            this.Invoke(new MethodInvoker(delegate() { bKonec.Enabled = true; }));

            t = new Transit();  //autosave
            t.obsah = 14;
            t.text = "autoSave";
            spojeni.Send(t);
        }

        private void KonecKola()
        {
            this.Invoke(new MethodInvoker(delegate() { bKonec.Enabled = false; }));
            aktivniTah = false;
            Console.WriteLine("konec kola");
            Transit t = new Transit();
            t.obsah = 4;
            Dictionary<String, ListKaret<Karta>> karty = new Dictionary<string, ListKaret<Karta>>();
            karty.Add("ruka", kartyNaRuce);
            karty.Add("stul", kartyNaStole);
            t.objekt = karty;
            spojeni.Send(t);

            t = new Transit();
            t.obsah = 3;
            spojeni.Send(t);
        }
        #endregion

        public void VyberPostav()
        {
            Console.WriteLine("Vybral postavu " + Postava.Nazev);
            foreach (var item in postavy)
            {
                this.Controls.Remove(item);
            }
            Transit t = new Transit();
            t.obsah = 105;
            t.objekt = Postava;
            spojeni.Send(t);
            aktZivotu += Postava.PocetZivotu;
            maxZivotu = aktZivotu;
            // karta tve postavy a aktualni zivoty
            //KartaImage<Karta> k;
            kartPatr = new KartaImage<Karta>(this, special.ElementAt(2), new Point(390 + (30 * 0), ClientSize.Height - 130), new Size(new Point(80, 120)), 1, false);
            this.Controls.Add(kartPatr);
            kartyNaFormuMoje.Add(kartPatr);
            if (kartaPostavaMoje != null)
            {
                this.Invoke(new MethodInvoker(delegate() { this.Controls.Remove(kartaPostavaMoje); }));
                this.Invoke(new MethodInvoker(delegate() { kartaPostavaMoje.SendToBack(); }));
            }
            Application.DoEvents();
            kartPostav = new KartaImage<PostavaKarta>(this, Postava, new Point(390 + (30 * 0), ClientSize.Height - 130 + (aktZivotu * 20) + 10), new Size(new Point(80, 120)), 1, true, false);
            this.Controls.Add(kartPostav);
            kartaPostavaMoje = kartPostav;
            kartPostav.BringToFront();
        }

        #region Tlacitka konec a zahodKarty
        private void bKonec_Click(object sender, EventArgs e)
        {
            if (kartyNaRuce.Count > aktZivotu)
            {
                //vypsat nekde hlasku: Mate moc karet bud jeste hrajte nebo nejmene (kartyNaRuce.Count - aktZivotu) karet zahodte!
                HraPanel hp = new HraPanel("Máte moc karet, buď ještě budete hrát, nebo nejméně " + (kartyNaRuce.Count - aktZivotu) + " karet zahoďte", this);
                
                btn_ZahodKarty.Visible = true;
                btn_ZahodKarty.Enabled = true;
            }
            else
            {
                btn_ZahodKarty.Visible = false;
                KonecKola();
                odhazovaniKaret = false;
            }
        }

        private void btn_ZahodKarty_Click(object sender, EventArgs e)
        {
            //vypsat nekde hlasku: Kliknutim na kartu ji odhodite !!
            HraPanel hp = new HraPanel("Kliknutím na kartu ji odhodíte", this);
            odhazovaniKaret = true;
            Console.WriteLine("jsem v modu odhazovani karet pred koncem tahu ...");
        }
        #endregion

        public void Vyloz(Karta karta, int typ) //vylozi modrou kartu z ruky na stul
        {
            /* Vyloz(karta, typ)
             * typ:
             * 1    - Volcanic
             * 2    - Scofield
             * 3    - Remington
             * 4    - Rev Carabine
             * 5    - Winchester
             * 6    - Mustang
             * 7    - Apalosa
             * 8    - barel
             * 9    - vezeni
             * 10   - dynamit
             * */
            foreach (var item in kartyNaStole)//kdyz uz ma stejnou kartu na stole tak nic neprovede
	        {
        		 if(item.Nazev.Equals(karta.Nazev)) return;
	        }

            switch (typ)
            {   
                case 1: 
                    this.dostrel = 1; //nastavi se dostrel ze zbrane
                    this.volcanic = true;
                    goto case 0;              
                case 2:
                    this.dostrel = 2;
                    this.volcanic = false;
                    goto case 0;
                case 3:
                    this.dostrel = 3;
                    this.volcanic = false;
                    goto case 0;
                case 4:
                    this.dostrel = 4;
                    this.volcanic = false;
                    goto case 0;
                case 5:
                    this.dostrel = 5;
                    this.volcanic = false;
                    goto case 0;
                case 6:
                    this.posunOd++;
                    break;
                case 7:
                    this.posunK++;
                    break;
                case 8:
                    this.barel = true;
                    break;
                case 9:
                    return; //TODO vezeni
                case 10:
                    return; //TODO dynamit
                case 0:
                    if (pistol)// pokud uz mam pistol
                    { //TODO zeptat se jestli chce zahodit tu co ma a dat tam tuhle
                        foreach (var item in kartyNaStole)//kdyz mam pistol, tak ji zahodim a nahradim
                        {
                            if ((item.Nazev.Equals("Scofield")) || (item.Nazev.Equals("Winchester")) || (item.Nazev.Equals("Rev. Carabine")) || (item.Nazev.Equals("Remington")) || (item.Nazev.Equals("Volcanic")))
                            {
                                ZahodKartuZeStolu(item);
                                break;
                            }
                        }
                    }
                    pistol = true;
                    break;
            }
            kartyNaRuce.Remove(karta);
            KartaNahled.Location = nahledPoz;
            kartyNaStole.Add(karta);
            
            Repaint();
            Transit t;
            t = new Transit();
            t.obsah = 4;
            Dictionary<String, ListKaret<Karta>> karty = new Dictionary<string, ListKaret<Karta>>();
            karty.Add("ruka", kartyNaRuce);
            karty.Add("stul", kartyNaStole);
            t.objekt = karty;
            spojeni.Send(t); //prekresleni pro vsechny hrace

        }

        public void Pivo(Karta karta)
        {
            if (aktZivotu < maxZivotu)
            {
                aktZivotu++;
                Transit t;
                t = new Transit();
                t.obsah = 17;
                t.text = aktZivotu.ToString();
                spojeni.Send(t);
                ZahodKartuZRuky(karta);
                //KartaNahled.Location = nahledPoz;
                //Repaint();
                //TODO poslat ostatnim ze jsem si dolecil zivot
                Console.WriteLine("pridal jsem si zivot kartou Pivo");
            }
        }

        public void Pivo() //pivo bez parametru muze volat kdyz dostane salon 
        {
            if (aktZivotu < maxZivotu)
            {
                aktZivotu++;
                Transit t;
                t = new Transit();
                t.obsah = 17;
                t.text = aktZivotu.ToString();
                spojeni.Send(t);
                //CHYBA
                //Repaint(); Kdyz to tady je tak to pada, coz je spatne !!! 
                //TODO: poslat si aktualni pocet zivotu
            }

        }

        public void Salon(Karta karta)
        {
            ZahodKartuZRuky(karta);
            Transit t;
            t = new Transit();
            t.obsah = 9;
            spojeni.Send(t);

        }

        public void Priber(Karta karta, int pocet) 
        {
            ZahodKartuZRuky(karta);
            //KartaNahled.Location = nahledPoz; //doporucuju za kazdym remove at se srovna nahled na sve misto
            Transit t;
            t = new Transit();
            t.obsah = 2;
            for (int i = 0; i < pocet; i++)
            {
                spojeni.Send(t);
            }
            //TODO poslat vsem ostatnim repaint
        }

        public void ZahodKartu(Karta karta) //vyhodí dočasnou kartu
        {
            Transit t = new Transit();
            t.obsah = 8;
            t.objekt = karta;
            spojeni.Send(t);
            karta = null;
        }

        public void ZahodKartuZRuky(Karta karta)
        {
            if (!kartyNaRuce.Contains(karta)) return; //zajisti aby neposlal do odhazovaciho balicku vylozenou kartu 
            kartyNaRuce.Remove(karta);
            Transit t;
            t = new Transit();
            t.obsah = 8;
            t.objekt = karta;
            spojeni.Send(t);
            KartaNahled.Location = nahledPoz;
            Repaint();
            //TODO poslat vsem ostatnim repaint
        }

        public void ZahodKartuZeStolu(Karta karta)
        {
            //NOTE
            //zatim se vola jen kdyz vykladas jinou zbran, takze neni treba nic resit ohledne tech 
            //vlastnosti, kdyby se volalo pri CatBalou nebo Panice tak uz se to musi vykutit tady
            kartyNaStole.Remove(karta);
            Transit t;
            t = new Transit();
            t.obsah = 8;
            t.objekt = karta;
            spojeni.Send(t);
            KartaNahled.Location = nahledPoz;
            Repaint();
            //TODO poslat vsem ostatnim repaint
        }


        public void Vystrel(Karta karta, int vzdalenost)  //tuhle metodu invokujeme, když chceme vystřelit Bang na někoho
            //pokud je vzdalenost = 0, pak je to bang a vzdalenost bereme z instance hráče
        {
            aktivniTah = false; // Aby nemohl znovu klikat na karty nez vystreli!
            //System.Console.WriteLine("příprava na výstřel");
            if (vzdalenost == 0)
            {
                vzdalenost = this.dostrel;
            }
            //zjistit, jestli v tomhle tahu už nehrál bang a pokud jo, tak když nemá spec vlastnost, tak už nemůže hrát
            if (zahranyBang && !volcanic && !schVolcanic)
            {
                //TODO dodělat, hlášku, že už hrál bang a že nemůže hrát dva za jedno kolo
                HraPanel hp = new HraPanel("V tomto kole už byla zahrána karta Bang! - nemůžete hrát další.", this);
                return;
            }
            //změnit proměnou, že seznam dostřelů není aktuální
            this.dosazitelniHraci = null;
            //poslat serveru žádost, že mi má vrátit seznam lidí na které dostřelím, pokud střílím kolem sebe na danou (poslanou) vzdálenost, 
            Transit posli = new Transit();
            posli.obsah = 5;
            posli.objekt = vzdalenost;
            spojeni.Send(posli);
            //System.Console.WriteLine("čekám na seznam od serveru");
            //čekat dokud se seznam neaktualizuje
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            while (this.dosazitelniHraci == null)
            {
                System.Threading.Thread.Sleep(10);
            }
            Cursor = System.Windows.Forms.Cursors.Default;

            //System.Console.WriteLine("seznam dorazil");
            //zavolat statickou metodu DialogHlasky, která mi řekne, koho z daného seznamu jsem vybral

            HraPanel h = new HraPanel(this.dosazitelniHraci.ToArray(), this, karta);
            //poslání oznámení o zásahu už probíhá ve třídě HraPanel

        }

        private object Create(string[] p, Hra hra)
        {
            throw new NotImplementedException();
        }

        private void NastavJmena() //nastavi jmena
        {
            foreach (var item in poradi)
            {
                switch (item.Value)
                {
                    case 1:
                        lName1.Text = item.Key;
                        break;
                    case 2:
                        lName2.Text = item.Key;
                        break;
                    case 3:
                        lName3.Text = item.Key;
                        break;
                    case 4:
                        lName4.Text = item.Key;
                        break;
                    case 5:
                        lName5.Text = item.Key;
                        break;
                }
                KartaImage<Karta> k;
                if (item.Value == serif)
                {
                    if (!kartyFunkceHracu.ContainsKey(item.Key)) kartyFunkceHracu.Add(item.Key, special.ElementAt(1));
                    if (item.Value == 0) continue;
                    k = new KartaImage<Karta>(this, special.ElementAt(1), new Point(105 + (180 * (item.Value-1)), 190 + (30 * 0)), new Size(new Point(80, 120)), 1, true);
                    this.Controls.Add(k);
                    kartyNaFormu.Add(k);
                    k.BringToFront();
                }
                else
                {
                    if (item.Value == 0) continue;
                    if (!kartyFunkceHracu.ContainsKey(item.Key)) kartyFunkceHracu.Add(item.Key, special.ElementAt(0));
                    k = new KartaImage<Karta>(this, special.ElementAt(0), new Point(105 + (180 * (item.Value - 1)), 190 + (30 * 0)), new Size(new Point(80, 120)), 1, true);
                    this.Controls.Add(k);
                    kartyNaFormu.Add(k);
                    k.BringToFront();
                }
            }
        }

        public void Kulomet(Karta karta)
        {
            ZahodKartuZRuky(karta);
            Transit t;
            t = new Transit();
            t.obsah = 10;
            spojeni.Send(t);
        }

        public void Indiani(Karta karta)
        {
            ZahodKartuZRuky(karta);
            Transit t;
            t = new Transit();
            t.obsah = 11;
            spojeni.Send(t);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lName1_Click(object sender, EventArgs e)
        {

        }

        private void Hra_Resize(object sender, EventArgs e)
        {
            //TODO: porad to jeste blbne, hlavne nahled, jezdi si kde chce, ale karty sou uz srovnane 

            //this.nahledPoz = new Point(ClientSize.Width - 220 + KartaNahled.Size.Width, ClientSize.Height - 350);
            this.KartaNahled.Location = new Point(ClientSize.Width - 215 + KartaNahled.Size.Width, ClientSize.Height - 330);
            
            if ((funkce != null) && (postava != null)) //provizorni reseni
            {
                Repaint();
            }
        }

        private void uložToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();*/

            nazevSouboru = DateTime.Now.ToString("dd-MM-yyyy HH.mm").Normalize(NormalizationForm.FormD);

            Transit t = new Transit();
            t.text = nazevSouboru;
            t.obsah = 14;
            spojeni.Send(t); //pošle na server, aby zahájil sekvenci pro uložení hry.
        }

        private void načtiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transit t = new Transit();
            t.obsah = 16;
            t.text = "";
            spojeni.Send(t);
            Console.WriteLine("zahájení načítání");
        }

        private void zpětToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transit t = new Transit();
            t.obsah = 20;
            spojeni.Send(t);
        }

        private void nastaveníToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(nastaveni));
            t.Start();
        }

        private void nastaveni()
        {
            Application.Run(new Nastaveni(false));
        }

        private void nováHraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in kartyNaFormu)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in kartyNaFormuMoje)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in kartyNaFormuPostavy)
            {
                this.Controls.Remove(item);
            }
            this.Controls.Remove(mojeFunkce);
            this.Controls.Remove(mojeFunkce);
            this.Controls.Remove(kartPostav);
            this.Controls.Remove(kartPatr);
            this.Controls.Remove(KartaNaOdhazovacimBalicku);
            this.Controls.Remove(KartaNaOdhazovacimBalicku);
            kartyFunkceHracu = new Dictionary<string, Karta>();
            konecHryReinicializace();
            Transit t = new Transit();
            t.obsah = 22;
            spojeni.Send(t);
        }
    }
}
