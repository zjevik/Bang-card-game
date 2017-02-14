using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class PostavaKarta : Karta
    {
        public int PocetZivotu;

        // musí mít atributy jako počet životů a nějaké další, které pak zapíšeme do atributů u jednotlivé hry.
        public PostavaKarta(int obrazek, String nazev, int ziv) : base(obrazek, nazev)
        {
            this.PocetZivotu = ziv;
        }

        public void InicializaceVlastnosti(Hra hra, PostavaKarta karta)
        // bud si to navolim tady, ale promenne musi byt public a nebo zavolam nejakou metodu do Hra a tam si to vyresi sama
        {
            switch (this.Nazev)
            {
                case "Bart Cassidy": //NOTE: to jde udelat!
                    //Kdykoli je zasžen, lízne si kartu 
                    break;

                case "Black Jack": //NOTE: ne
                    //Ukáže druhou kartu kterou si líže, pokud je srdcová nebo kárová bere si ještě jednu
                    break;

                case "Calamity Janet": //NOTE: nevim
                    //Může hrát kartu BANG! místo Vedle! a obráceně.
                    hra.schBangVedle = true;
                    break;

                case "El Gringo": //NOTE: ne
                    //Pokaždé když je zasažen hráčem vezme si kartu z ruky tohoto hráče
                    break;

                case "Jesse Jones": //NOTE: nevim
                    //První kartu si může vzít z rukou některého hráče
                    break;

                case "Jourdonnais": //NOTE: ano
                    //Má BAREL
                    hra.schBarel = true;
                    break;

                case "Kit Carlson": //NOTE: asi ne
                    //Podívá se na vrchní tři karty z balíčku a vybere si dvě, třetí tam vrátí
                    break;

                case "Lucky Duke": //NOTE: asi ne
                    //Pokaždé, když si "líže!" kartu, vezme si vrchní dvě a vybere si z nich jednu.
                    break;

                case "Paul Regret": //NOTE: ano
                    //Jeho vzdálenost od všechn hráčů je o jedna vštší
                    hra.posunOd = 1;
                    break;

                case "Pedro Ramirez": //NOTE: asi ne
                    //Prvni kartu si muze vzit z odhazovaciho balicku
                    break;

                case "Rose Doolan": //NOTE: ano
                    //Vidí všechny hráče na vzdálenost o jedna menší
                    hra.posunK = 1;
                    break;

                case "Sid Ketchum": //NOTE: spise ne
                    //Za 2 vyhozené karty si může vrátit jeden život
                    break;

                case "Slab The Killer": //NOTE: slo by, ale asi ne
                    //Hráč potřeuje 2 karty Vedle! proti jeho kartě BANG!
                    hra.schDveVedle = true;
                    break;

                case "Suzy Lafayette": //NOTE: mohlo by 
                    //Jakmile nemá žádnou kartu v ruce, jednu si vezme
                    break;

                case "Vulture Sam": //NOTE: slo by
                    //Kdykoli je některý hráč vyřazen ze hry, vezme si do ruky všechny Karty tohoto hráče
                    break;

                case "Willy The Kid": //NOTE: ano
                    //Má VOLCANIC
                    hra.schVolcanic = true;
                    break;


                default:
                    break;
            }
        }
    }
}
