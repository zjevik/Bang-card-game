using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class KartaHneda : Karta
    {
        public KartaHneda(int obrazek, String nazev, CisloKarty cislo) : base(obrazek, nazev, cislo) {  }

        public void Akce(Hra hra, KartaHneda me)    //tohle by se dalo ošéfovat pomocí delegátů v konstruktoru
                                                    // a bylo by to elegantnější
        {
            if (hra.odhazovaniKaret)
            {
                hra.Invoke(new MethodInvoker(delegate() { hra.ZahodKartuZRuky((Karta)me); }));
                return;
            }

            if (hra.aktivniTah) //pouze ve svem tahu
            {
                switch (this.Nazev)
                {
                    case "Bang":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vystrel((Karta)me, 0); }));
                        break;

                    case "Wells Fargo":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Priber((Karta)me, 3); }));
                        break;

                    case "Dostavnik":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Priber((Karta)me, 2); }));
                        break;

                    case "Pivo":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Pivo((Karta)me); }));
                        break;

                    case "Salon":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Salon((Karta)me); }));
                        break;

                    case "Kulomet":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Kulomet((Karta)me); }));
                        break;

                    case "Indiani":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Indiani((Karta)me); }));
                        break;

                    /* TODO:
                     * "Hokynarstvi"
                     * "CatBalou"
                     * "Panika"
                     * "Indiani" - neco jako bang na vsechny s tim rozdilem ze vsichni potrebuji odhodit bang
                     * */

                    default:
                        break;
                }
            }
        }
    }
}
