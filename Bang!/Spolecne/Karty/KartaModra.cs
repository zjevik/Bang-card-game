using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bang_.Spolecne.Karty
{
    [Serializable]      //třída je serializovatelná
    public class KartaModra : Karta
    {

        public KartaModra(int obrazek, String nazev, CisloKarty cislo) : base(obrazek, nazev, cislo) { }

        public void Akce(Hra hra, KartaModra me)    //tohle by se dalo ošéfovat pomocí delegátů v konstruktoru
        // a bylo by to elegantnější
        {
            //Console.WriteLine("Vlastnikem karty na kterou bylo klinuto je :" + me.Owner+" !");
            if ((me.Owner != hra.Name) && (me.Owner != null))
            {
                //Console.WriteLine("Tohle neni tva karta!!!");
                return;

            }
            if (hra.odhazovaniKaret)
            {
                hra.Invoke(new MethodInvoker(delegate() { hra.ZahodKartuZRuky((Karta)me); }));
                return;
            }

            if (hra.aktivniTah) //pouze ve svem tahu
            {
                switch (this.Nazev)
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
                    case "Barel": 
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 8); }));
                        break;

                    case "Mustang":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 6); }));
                        break;

                    case "Apalosa":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 7); }));
                        break;

                    case "Volcanic":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 1); }));
                        break;

                    case "Remington":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 3); }));
                        break;

                    case "Rev. Carabine":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 4); }));
                        break;

                    case "Scofield":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 2); }));
                        break;

                    case "Winchester":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 5); }));
                        break;
                    
                    case "Vezeni":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 9); }));
                        break;

                    case "Dynamit":
                        hra.Invoke(new MethodInvoker(delegate() { hra.Vyloz((Karta)me, 10); }));
                        break;

                    default:
                        break;
                }
            }
        }
    }


}
