using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class Karta
    {        
        public CisloKarty Cislo { get; set; }
        public String Nazev { get; set; }
        public byte Druh { get; set; }//1=funkce, 2=postava, 3=modrá, 4=hnědá
        private int Obrazek { set; get; }
        public String Owner { set; get; }

        public Image GetObrazek()
        {
            return KartaObrazek.obrazky[Obrazek];
        }

        public Karta(int obrazek, String nazev)
        {
            this.Obrazek = obrazek;
            this.Nazev = nazev;
        }

        public Karta(int obrazek, String nazev, CisloKarty cislo)
        {
            this.Obrazek = obrazek;
            this.Nazev = nazev;
            this.Cislo = cislo;
        }

        public Karta CopyKart()
        {
            Karta help = new Karta(this.Obrazek, this.Nazev, this.Cislo);
            return help;
        }
    }
}
