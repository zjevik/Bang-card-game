using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Bang_.Spolecne;

namespace Bang_
{
    static class Global
    {
        public static List<TcpClient> Klienti { get; set; }

        //public static Player Prehravac { get; set; } //property pro třídu Player
        //statická proto, aby mohla existovat jen jedna instance a mohla hrát celou dobu
    }
}
