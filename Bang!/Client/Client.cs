using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Bang_.Spolecne;

namespace Bang_
{
    public partial class Client : Form
    {
        private TcpClient serverStream = new TcpClient();
        public String name;                        //jmeno klienta
        //private Stream output;               //Stream na komunikaci se serverem
        private Spojeni serverConnection;

        public Client(String jmeno, Boolean spust)
        {
            this.name = jmeno; 
            Console.WriteLine("Vaše jméno je: " + this.name);
            Console.WriteLine("---------------------------");
            InitializeComponent();
            this.Text = "Hráč " + jmeno;
            if (spust)
            {
                button1_Click(new object(), new EventArgs());
                this.WindowState = FormWindowState.Minimized;
            }
        }

        public void SpustHru()
        {
            Application.Run(new Hra(serverConnection,name));
        }

        private void ServerCom()
        {
            String message = "";
            while (serverConnection.RecieveText(ref message))
            {
                if (message.CompareTo("start") == 0)
                {
                    this.Invoke(new MethodInvoker(delegate() { Cursor = System.Windows.Forms.Cursors.Default; }));
                    Console.WriteLine("Hra byla odstartovana !");
                    Thread t = new Thread(new ThreadStart(SpustHru));
                    t.Start();
                    this.Invoke(new MethodInvoker(delegate() { this.Dispose(); })); 
                    break;     // dodělat zapnutí noveho herniho formu 
                }

                if (message.CompareTo("test připojení") == 0)
                {
                    this.Invoke(new MethodInvoker(delegate() { Cursor = System.Windows.Forms.Cursors.WaitCursor; }));
                    //Console.WriteLine("...");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serverStream.Connect(IPbox.Text, 5436);
                serverConnection = new Spojeni(serverStream); 
                serverConnection.SendText(name);
                Thread spojeni = new Thread(new ThreadStart(ServerCom));
                spojeni.Start();
                this.IPbox.Enabled = false;
                this.button1.Enabled = false;
            }
            catch (SocketException)
            {
                Console.WriteLine("Požadované PC není v dosahu");
            }
        }
    }
}
