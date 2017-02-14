using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bang_.Spolecne
{
    public class Spojeni
    {
        public TcpClient spojeni;
        public string name;
        public Stream stream;
        BinaryFormatter biFormatter = new BinaryFormatter();

        public Spojeni()
        {
        }

        public Spojeni(TcpClient spojeni) 
        {
            this.spojeni = spojeni;
            //this.name = name;
            stream = spojeni.GetStream();
        }

        public void Send(Transit s)
        {
            try
            {
                Console.WriteLine("Send: " + s.obsah);
                biFormatter.Serialize(stream, s);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new HracSeOdpojilException("Chyba se strými - asi se někdo odpojil.");
            }
        }

        public void SendText(string text)
        {
            try
            {
                biFormatter.Serialize(stream, text);
                stream.Flush();
            }
            catch(Exception e)
            {
                throw new HracSeOdpojilException("Chyba se strými - asi se někdo odpojil.");
            }
        }

        public bool RecieveText(ref String s)
        {
            try
            {
                s = (String)biFormatter.Deserialize(stream);
                return true;
            }
            catch (Exception e)
            {
                throw new HracSeOdpojilException("Chyba se strými - asi se někdo odpojil.");
            }
        }

        public Transit Recieve()
        {
            try
            {
                return (Transit)biFormatter.Deserialize(stream);
            }
            catch(Exception e)
            {
                throw new HracSeOdpojilException("Chyba se strými - asi se někdo odpojil.");
                return new Transit();
            }
        }

        public void SetName()
        {
            name = (String)biFormatter.Deserialize(stream);
        }
    }
}
