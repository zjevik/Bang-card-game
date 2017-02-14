using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class CisloKarty
    {
        private int barva;
        private int cislo;

        public CisloKarty(String barva, int cislo) //barva srdce, piky, kara, krize (1-13)
        {
            switch (barva)
            {
                case "srdce":
                    this.barva = 1;
                    break;
                case "kara":
                    this.barva = 2;
                    break;
                case "piky":
                    this.barva = 3;
                    break;
                case "krize":
                    this.barva = 4;
                    break;
                default:
                    this.barva = -1;
                    break;
            }
            this.cislo = cislo;
        }

        public CisloKarty(int barva, int cislo)
        {
            this.barva = barva;
            this.cislo = cislo;
        }

        public bool Equals(CisloKarty karta)
        {
            if ((this.barva == karta.barva) && (this.cislo == karta.cislo)) return true;
            return false;
        }

        public bool InRange(int min, int max) //porovnava jen rozsah karty
        {
            if ((min <= this.cislo)&&(this.cislo <= max)) return true;
            return false;
        }

        public bool InColourAndRange(CisloKarty karta, int min, int max) //porovnava barvu i rozsah
        {
            if ((this.barva == karta.barva) && ((min <= karta.cislo)&&(karta.cislo <= max))) return true;
            return false;
        }

        public bool InColourAndRange(int barva, int min, int max) //porovnava barvu i rozsah
        {
            if ((this.barva == barva) && ((min <= this.cislo) && (this.cislo <= max))) return true;
            return false;
        }

        public bool InColourAndRange(String barva, int min, int max) //porovnava barvu i rozsah
        {
            CisloKarty krt = new CisloKarty(barva, this.cislo);
            if ((this.barva == krt.barva) && ((min <= this.cislo) && (this.cislo <= max))) return true;
            return false;
        }

        public bool InColour(String barva) //porovná barvu
        {
            return InColourAndRange(barva, 0, 15);
        }
    }
}
