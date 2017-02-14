using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang_.Spolecne.Karty
{
    [Serializable]
    public class MyEventArgs : EventArgs
    {
        /// <summary>
        /// Message carried with the event
        /// </summary>
        internal string Message { get; set; }
    }
}
