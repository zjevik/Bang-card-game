using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    [Serializable]
    class PozorovatelBalickuKaret
    {
        public void Update(Object sender, MyEventArgs e)
        {
            Console.WriteLine("Events: Balicek karet byl zamichan !");
            //TODO muze se vyuzit toho MyEventArgs a neco vice vypisovat
        }
    }
}
