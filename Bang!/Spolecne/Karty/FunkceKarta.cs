using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class FunkceKarta : Karta
    {
        public int Funkce { set; get; }
        // 1 ... Šerif
        // 2 ... Vice
        // 3 ... Bandita
        // 4 ... Odpadlík

        public FunkceKarta(int obrazek, String nazev, int funkce) : base(obrazek, nazev)
        {
            this.Funkce = funkce;
        }
    }
}
