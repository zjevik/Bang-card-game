using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Bang_
{
    [Serializable]      //třída je serializovatelná
    public class Transit
    {
        public List<Object> list;
        public String text;
        public Object objekt;
        public int obsah; // udává co je v třídě obsaženo...
        // 101 ... karta funkce hráče
        // 102 ... list postav
        // 103  ... rozmisteni hracu 
        // 1  ... chat
        // 2  ... dej kartu
        // 3  ... konec/začátek tahu
        // 4  ... updatuj zobrazené karty
        // 5  ... seznam hráčů v dostřelu
        // 6  ... střela na hráče
        // 7  ... poslání serveru, jestli nemám nějak upravenou vzdálenost
        // 8  ... odhození karty do odhazovacího balíčku
        // 9  ... salón
        // 10 ... kulomet
        // 11 ... indiáni
        // 12 ... zabij si svého indiána :)
        // 13 ... přijmi kartu, pro potřebu něčeho jiného, než dát jí do ruky a ulož do KartaPrijata
        // 14 ... transmision to save game
        // 15 ... transmision to save game
        // 16 ... načtení hry
        // 17 ... aktualizace zivotu
        // 18 ... načítání hry
        // 19 ... otacim svou funkci, sem mrtev
        // 20 ... načtení autosavu - Zpět
        // 21 ... zprava s tim ze je serif mrtev, konec hry
        // 22 ... nová hra
    }
}
