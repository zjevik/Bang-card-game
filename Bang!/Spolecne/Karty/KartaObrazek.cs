using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    public class KartaObrazek
    {
        static public List<System.Drawing.Image> obrazky = new List<System.Drawing.Image>();

        public static void Inicialize()
        {
            System.Threading.Thread.Sleep(1000);

            obrazky.Add(global::Bang_.Properties.Resources.Sherif);
            obrazky.Add(global::Bang_.Properties.Resources.Vice);
            obrazky.Add(global::Bang_.Properties.Resources.Bandita);
            obrazky.Add(global::Bang_.Properties.Resources.Odpadlik);

            obrazky.Add(global::Bang_.Properties.Resources.PostavaZezadu);
            obrazky.Add(global::Bang_.Properties.Resources.Patrony);
            obrazky.Add(global::Bang_.Properties.Resources.Bangzezadu);

            obrazky.Add(global::Bang_.Properties.Resources.BartCassidy);
            obrazky.Add(global::Bang_.Properties.Resources.BlackJack);
            obrazky.Add(global::Bang_.Properties.Resources.CalamityJanet);
            obrazky.Add(global::Bang_.Properties.Resources.ElGringo);
            obrazky.Add(global::Bang_.Properties.Resources.JesseJones);
            obrazky.Add(global::Bang_.Properties.Resources.Jourdonnais);
            obrazky.Add(global::Bang_.Properties.Resources.KitCarlson);
            obrazky.Add(global::Bang_.Properties.Resources.LuckyDuke);
            obrazky.Add(global::Bang_.Properties.Resources.PaulRegret);
            obrazky.Add(global::Bang_.Properties.Resources.PedroRamirez);
            obrazky.Add(global::Bang_.Properties.Resources.RoseDoolan);
            obrazky.Add(global::Bang_.Properties.Resources.SidKetchum);
            obrazky.Add(global::Bang_.Properties.Resources.SlabTheKiller);
            obrazky.Add(global::Bang_.Properties.Resources.SuzyLafayette);
            obrazky.Add(global::Bang_.Properties.Resources.VultureSam);
            obrazky.Add(global::Bang_.Properties.Resources.WillyTheKid);

            obrazky.Add(global::Bang_.Properties.Resources.Apalosa_Apik_);
            obrazky.Add(global::Bang_.Properties.Resources.Barel_Kpik_);
            obrazky.Add(global::Bang_.Properties.Resources.Barel_Qpik_);
            //obrazky.Add(global::Bang_.Properties.Resources.Dynamit_2srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Mustang_8srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Mustang_9srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Remington_Kkri_);
            obrazky.Add(global::Bang_.Properties.Resources.Rev_Carabine_Akri_);
            obrazky.Add(global::Bang_.Properties.Resources.Scofield_Jkri_);
            obrazky.Add(global::Bang_.Properties.Resources.Scofield_Qkri_);
            obrazky.Add(global::Bang_.Properties.Resources.Schofield_Kpik_);
            //obrazky.Add(global::Bang_.Properties.Resources.Vezeni_10pik_);
            //obrazky.Add(global::Bang_.Properties.Resources.Vezeni_4srd_);
            //obrazky.Add(global::Bang_.Properties.Resources.Vezeni_Jpik_);
            obrazky.Add(global::Bang_.Properties.Resources.Volcanic_10kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Volcanic_10pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Winchester_8pik_);

            obrazky.Add(global::Bang_.Properties.Resources.Bang_10kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_2kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_2kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_3kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_3kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_4kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_4kriz_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_5kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_5kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_6kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_6kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_7kar);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_7kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_8_kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_8kar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_9kar_); //40...
            obrazky.Add(global::Bang_.Properties.Resources.Bang_9kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Akar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Apik_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Asrd_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Jkar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Kkar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Ksrd_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Qkar_);
            obrazky.Add(global::Bang_.Properties.Resources.Bang_Qsrd_);
            //obrazky.Add(global::Bang_.Properties.Resources.CatBalou_10kar_);    //50...
            //obrazky.Add(global::Bang_.Properties.Resources.CatBalou_9kar_);
            //obrazky.Add(global::Bang_.Properties.Resources.CatBalou_Jkar_);
            //obrazky.Add(global::Bang_.Properties.Resources.CatBalou_Ksrd_);
            obrazky.Add(global::Bang_.Properties.Resources.Dostavnik_3srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Dostavnik_9pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Dostavnik_9pik_);
            //obrazky.Add(global::Bang_.Properties.Resources.Duel_8kriz_);
            //obrazky.Add(global::Bang_.Properties.Resources.Duel_Jpik_);
            //obrazky.Add(global::Bang_.Properties.Resources.Duel_Qkar_);
            //obrazky.Add(global::Bang_.Properties.Resources.Hokynarstvi_9kri_);  //60...
            //obrazky.Add(global::Bang_.Properties.Resources.Hokynarstvi_Qpik_);
            obrazky.Add(global::Bang_.Properties.Resources.Indiani_Akar_);
            obrazky.Add(global::Bang_.Properties.Resources.Indiani_Kkar_);
            obrazky.Add(global::Bang_.Properties.Resources.Kulomet_10srd_);
            //obrazky.Add(global::Bang_.Properties.Resources.Panika_8kar_);
            //obrazky.Add(global::Bang_.Properties.Resources.Panika_Asrd_);
            //obrazky.Add(global::Bang_.Properties.Resources.Panika_Jsrd_);
            //obrazky.Add(global::Bang_.Properties.Resources.Panika_Qsrd_);
            obrazky.Add(global::Bang_.Properties.Resources.Pivo_10srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Pivo_6srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Pivo_7srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Pivo_8srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Pivo_9srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Pivo_Jsrd_);
            obrazky.Add(global::Bang_.Properties.Resources.salon_5srd_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_10kri_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_2pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_3pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_4pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_5pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_6pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_7pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_8pik_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_Akri_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_Jkri_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_Kkri_);
            obrazky.Add(global::Bang_.Properties.Resources.Vedle_Qkri_);
        }
    }
}
