using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Bang_.Spolecne;

namespace Bang_
{
    public partial class Welcome : Form
    {
        static String name;
        static bool zapni = false;

        public Welcome()
        {
            InitializeComponent();
           
            Application.Run(new Nastaveni(true));
            
            Thread t = new Thread(new ThreadStart( Bang_.Spolecne.Karty.KartaObrazek.Inicialize ));
            t.Priority = ThreadPriority.Lowest;  // zahájení načítání obrázků karet do RAM (cca 200MB)
            t.Start();
        }

        public static void VlaknoServer()
        {
            Application.Run(new Server(name));
        }

        public static void VlaknoClient()
        {
            Application.Run(new Client(name, zapni));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
                //tohle bude v REÁLU!!
            //{     
            //    DialogHlaska dlg = new DialogHlaska("Dialogové okno aplikace Bang!", "Nezadali jste jméno,\npo kliknutí na OK zadejte jméno a\nakci opakujte.");
            //    dlg.ShowDialog();
            //}
            {
                name = "Server";
                Thread t = new Thread(new ThreadStart(VlaknoServer));
                t.Start();
                this.Dispose();
            }
            else
            {
                name = this.textBox1.Text;
                Thread t = new Thread(new ThreadStart(VlaknoServer));
                t.Start();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            //tohle bude v REÁLU!!
            //{
            //    DialogHlaska dlg = new DialogHlaska("Dialogové okno aplikace Bang!", "Nezadali jste jméno,\npo kliknutí na OK zadejte jméno a\nakci opakujte.");
            //    dlg.ShowDialog();
            //}
            {
                Random r = new Random();
                name = "Klient - " + r.Next(9,100).ToString();
                zapni = true;
                Thread t = new Thread(new ThreadStart(VlaknoClient));
                t.Start();
                this.Dispose();
            }
            else
            {
                name = this.textBox1.Text;
                Thread t = new Thread(new ThreadStart(VlaknoClient));
                t.Start();
                this.Dispose();
            }
        }
    }
}
