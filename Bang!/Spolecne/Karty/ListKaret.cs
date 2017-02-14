using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class ListKaret<T> : List<T> where T : Karta
    {
        public event EventHandler<MyEventArgs> StateUpdate;

        private MyEventArgs data;


        public ListKaret(int k) : this(k,0)
        {
        }

        public ListKaret()
        {
        }

        public void print()
        {
            foreach (var item in this)
            {
                Console.WriteLine(item.Nazev);
            }
        }

        public ListKaret(int k, int l)  //podle toho jaký chce vytvořit
            //(1,N) .. list s N kartami funkcí
            //(2,) ... list s kartami spaciálních schopností
            //(3,) ... list s hracími kartami
            //(4,) ... list se specialnimi kartami
        {
            int i = 0;
            switch (k)
            {
                case 1:
                    switch (l)
                    {
                        case 4:
                            this.Add((T)(Karta)new FunkceKarta(0, "Serif", 1));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(3, "Odpadlik", 4));
                            break;
                        case 5:
                            this.Add((T)(Karta)new FunkceKarta(0, "Serif", 1));
                            this.Add((T)(Karta)new FunkceKarta(1, "Vice", 2));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(3, "Odpadlik", 4));
                            break;
                        case 6:
                            this.Add((T)(Karta)new FunkceKarta(0, "Serif", 1));
                            this.Add((T)(Karta)new FunkceKarta(1, "Vice", 2));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(3, "Odpadlik", 4));                            
                            break;
                        case 7:
                            this.Add((T)(Karta)new FunkceKarta(0, "Serif", 1));
                            this.Add((T)(Karta)new FunkceKarta(1, "Vice", 2));
                            this.Add((T)(Karta)new FunkceKarta(1, "Vice", 2));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(2, "Bandita", 3));
                            this.Add((T)(Karta)new FunkceKarta(3, "Odpadlik", 4));                            
                            break;
                    }
                    break;

                case 4:
                    this.Add((T)new Karta(4,"NeznamaFunkce"));
                    this.Add((T)(Karta)new FunkceKarta(0, "Serif", 1));
                    this.Add((T)new Karta(5,"Patrony"));
                    this.Add((T)new Karta(6, "NeznamaKarta"));
                    break;

                case 2:
                    i = 7;
                    this.Add((T)(Karta)new PostavaKarta(i++, "Bart Cassidy",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Black Jack",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Calamity Janet", 4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "El Gringo",3));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Jesse Jones",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Jourdonnais",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Kit Carlson",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Lucky Duke",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Paul Regret",3));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Pedro Ramirez",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Rose Doolan",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Sid Ketchum",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Slab The Killer",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Suzy Lafayette",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Vulture Sam",4));
                    this.Add((T)(Karta)new PostavaKarta(i++, "Willy The Kid",4));
                    // i =20
                    break;
                case 3:
                    i = 23;
                    /* Modre */
                    //Apalosa
                    this.Add((T)(Karta)new KartaModra(i++, "Apalosa", new CisloKarty(3, 1)));
                    //Barel
                    this.Add((T)(Karta)new KartaModra(i++, "Barel", new CisloKarty(3, 13)));
                    this.Add((T)(Karta)new KartaModra(i++, "Barel", new CisloKarty(3, 12)));
                    //Dynamit
                    //this.Add((T)(Karta)new KartaModra(i++, "Dynamit", new CisloKarty(1, 2)));
                    ///// ------- Nove pridane 29.3.2010
                    this.Add((T)(Karta)new KartaModra(i++, "Mustang", new CisloKarty(1, 8)));
                    this.Add((T)(Karta)new KartaModra(i++, "Mustang", new CisloKarty(1, 9)));

                    this.Add((T)(Karta)new KartaModra(i++, "Remington", new CisloKarty(4, 13)));
                    this.Add((T)(Karta)new KartaModra(i++, "Rev. Carabine", new CisloKarty(4, 1)));
                    this.Add((T)(Karta)new KartaModra(i++, "Scofield", new CisloKarty(4, 11)));
                    this.Add((T)(Karta)new KartaModra(i++, "Scofield", new CisloKarty(4, 12)));
                    this.Add((T)(Karta)new KartaModra(i++, "Scofield", new CisloKarty(4, 13)));

                    //this.Add((T)(Karta)new KartaModra(i++, "Vezeni", new CisloKarty(3, 10)));
                    //this.Add((T)(Karta)new KartaModra(i++, "Vezeni", new CisloKarty(1, 4)));
                    //this.Add((T)(Karta)new KartaModra(i++, "Vezeni", new CisloKarty(3, 11)));

                    this.Add((T)(Karta)new KartaModra(i++, "Volcanic", new CisloKarty(4, 10)));
                    this.Add((T)(Karta)new KartaModra(i++, "Volcanic", new CisloKarty(3, 10)));

                    this.Add((T)(Karta)new KartaModra(i++, "Winchester", new CisloKarty(3, 8)));

                    /* Hnede */
                    //Bang
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 10)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 2)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 2)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 3)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 3)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 4)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 4)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 5)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 5)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 6)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 6)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 7)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 7)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 8)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 8)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 9)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(4, 9)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 1)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(3, 1)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(1, 1)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 11)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 13)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(1, 13)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(2, 12)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Bang", new CisloKarty(1, 12)));
                    //CatBalou
                    //this.Add((T)(Karta)new KartaHneda(i++, "Cat Balou", new CisloKarty(2, 10)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Cat Balou", new CisloKarty(2, 9)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Cat Balou", new CisloKarty(2, 11)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Cat Balou", new CisloKarty(1, 13)));
                    //Dostavnik
                    this.Add((T)(Karta)new KartaHneda(i++, "Wells Fargo", new CisloKarty(1, 3)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Dostavnik", new CisloKarty(3, 9)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Dostavnik", new CisloKarty(3, 9)));
                    //Duel
                    //this.Add((T)(Karta)new KartaHneda(i++, "Duel", new CisloKarty(4, 8)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Duel", new CisloKarty(3, 11)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Duel", new CisloKarty(2, 12)));
                    //Hokynarstvi
                    //this.Add((T)(Karta)new KartaHneda(i++, "Hokynarstvi", new CisloKarty(4, 9)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Hokynarstvi", new CisloKarty(3, 12)));
                    //Indiani
                    this.Add((T)(Karta)new KartaHneda(i++, "Indiani", new CisloKarty(2, 1)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Indiani", new CisloKarty(2, 13)));
                    //Kulomet
                    this.Add((T)(Karta)new KartaHneda(i++, "Kulomet", new CisloKarty(1, 10)));
                   

                    ///// ---- Nove pridane 29.32010
                    //this.Add((T)(Karta)new KartaHneda(i++, "Panika", new CisloKarty(2, 8)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Panika", new CisloKarty(1, 1)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Panika", new CisloKarty(1, 11)));
                    //this.Add((T)(Karta)new KartaHneda(i++, "Panika", new CisloKarty(1, 12)));

                    this.Add((T)(Karta)new KartaHneda(i++, "Pivo", new CisloKarty(1, 10)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Pivo", new CisloKarty(1, 6)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Pivo", new CisloKarty(1, 7)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Pivo", new CisloKarty(1, 8)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Pivo", new CisloKarty(1, 9)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Pivo", new CisloKarty(1, 11)));

                    this.Add((T)(Karta)new KartaHneda(i++, "Salon", new CisloKarty(1, 5)));

                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(4, 10)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 2)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 3)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 4)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 5)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 6)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 7)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(3, 8)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(4, 1)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(4, 11)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(4, 13)));
                    this.Add((T)(Karta)new KartaHneda(i++, "Vedle", new CisloKarty(4, 12)));
                    

                    break;
            }
                     
        }

        public void Zamichej()
        {
            Random r = new Random();
            T p;
            int t;
            for (int i = 0; i < this.Count*5; i++)
            {
                t = r.Next(this.Count);
                p = this.ElementAt(t);
                this.RemoveAt(t);
                this.Add(p);
            }

            if (StateUpdate != null)
            StateUpdate(this, data);
        }
    }
}
