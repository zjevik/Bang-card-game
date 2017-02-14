using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Bang_.Spolecne;

namespace Bang_
{
    public partial class Server : Form
    {
        private TcpListener serverSocket;
        private Spojeni client;
        private Spojeni serverConnection;
        private List<Spojeni> klienti = new List<Spojeni>();
        private List<Label> seznamKlientu = new List<Label>();
        private int port = 5436;
        private List<Tlacitko> tlacitka;
        public String name, adresa;
        private int posun;

        public void JmenoKlienta(int left, int top, int height, int width, string text, int size)
        {
            Label l = new Label();
            l.Left = left;
            l.Top = top;
            Size s = new Size(width, height);
            l.Size = s;
            //l.BackColor = Color.Aqua;
            l.Text = text;
            l.Font = new Font(l.Font.FontFamily, size);
            seznamKlientu.Add(l);
            this.Invoke(new MethodInvoker(delegate() {Controls.Add(l);}));  //musí se použít, protože jsme v jiném, než MAIN vláknu a jen MAIN vlákno může přistupovat k akcím/grafice na formu
        }

        public void Otevri()
        {
            while (true)
            {
                client = new Spojeni(serverSocket.AcceptTcpClient());
                client.SetName();
                Console.WriteLine("Klient připojen. Jeho jméno je: {0}",client.name);                
                klienti.Add(client);    //přidá instanci klienta do Listu pro další práci
            }
        }

        public Server(String name)
        {
            InitializeComponent();
            this.Text = "Hráč " + name;
            this.name = name;
            Console.WriteLine("Vaše jméno je: " + this.name);
            Console.WriteLine("---------------------------");
            string myHost = Dns.GetHostName();
            IPHostEntry myIPs = Dns.GetHostEntry(myHost);

            posun = 10;
            int pocet = 0;
            tlacitka = new List<Tlacitko>();
            Tlacitko tlacitko;
            foreach (IPAddress myIP in myIPs.AddressList)
            {
                if (!myIP.ToString().Contains(":"))
                {
                    pocet++;
                    Console.WriteLine(myIP.ToString());
                    tlacitko = new Tlacitko(this);
                    tlacitko.Name = pocet.ToString();
                    tlacitko.Text = myIP.ToString();
                    tlacitko.Top = posun;
                    posun += 30;
                    tlacitko.Left = 10;
                    tlacitka.Add(tlacitko);
                    this.Controls.Add(tlacitko);
                }
            }

            //// pro nás ještě localhost...
            //pocet++;
            //string locIP = "127.0.0.1";
            //Console.WriteLine(locIP.ToString());
            //tlacitko = new Tlacitko(this);
            //tlacitko.Name = pocet.ToString();
            //tlacitko.Text = locIP;
            //tlacitko.Top = posun;
            //posun += 30;
            //tlacitko.Left = 10;
            //this.Controls.Add(tlacitko);
        }

        public void Pripoj(string adresa)
        {
            foreach (Tlacitko t in tlacitka)    // odstaraní zbytek tlačítek z formu
            {
                t.Dispose();
            }
            Console.WriteLine("Server spuštěn na adrese: {0}",adresa);
            this.adresa = adresa;
            IPAddress ipAdresa = Dns.GetHostAddresses(adresa).First();
            serverSocket = new TcpListener(ipAdresa, port);
            serverSocket.Start();
            Thread otevriPripojeni = new Thread(new ThreadStart(Otevri));
            otevriPripojeni.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            posun = 30;
            foreach (Label l in seznamKlientu)      // smažu předchozí labely
            {
                l.Dispose();
            }
            seznamKlientu = new List<Label>();  // vytvořím nový List pro labely
            List<Spojeni> klientiTmp = new List<Spojeni> (klienti);
            foreach (Spojeni c in klientiTmp)  
            {
                try     //zjistím, jestli je klient "živý" tím se autom nastaví hodnota c.spojeni.Connected
                {
                    c.SendText("test připojení");
                }
                catch (Exception)
                {
                    Console.WriteLine("klientovi '{0}' nelze poslat data - stream neexistuje",c.name);
                }
                if (!c.spojeni.Connected)
                {
                    Console.WriteLine("odebrán klient, name: {0}", c.name);
                    klienti.Remove(c);                    
                    //break;      //když zněním ten List, tak foreach hodí chybu, tak musím počkat znovu na časovač
                }
                else
                {
                    posun += 22;
                    JmenoKlienta(15, posun, 20, 100, c.name, 12);  //přidá na form label se jménem připojeného klienta a uloží label do Listu seznamKlientu
                    //Console.WriteLine("přidán label, name: {0}",c.name);
                }
            }
            switch (klienti.Count) //aktuální stav hráčů pro hru
            {
                case 3:
                    this.startButton.Enabled = true;
                    this.slozeniFunkci.Text = "1 x Sceriffo, 2 x Bandita, 1 x Odpadlík";
                    break;
                case 4:
                    this.slozeniFunkci.Text = "1 x Sceriffo, 1 x Vice, 2 x Bandita, 1 x Odpadlík";
                    break;
                case 5:
                    this.slozeniFunkci.Text = "1 x Sceriffo, 1 x Vice, 3 x Bandita, 1 x Odpadlík";
                    break;
                case 6:
                    this.slozeniFunkci.Text = "1 x Sceriffo, 2 x Vice, 3 x Bandita, 1 x Odpadlík";
                    break;
                default:
                    this.startButton.Enabled = false;
                    this.slozeniFunkci.Text = "Málo hráčů pro hru !!!";
                    break;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            foreach (Spojeni spoj in klienti)
            {
                spoj.SendText("start");
            }
            TcpClient serverStream = new TcpClient();

            serverStream.Connect(adresa, 5436);
            serverConnection = new Spojeni(serverStream);
            serverConnection.SendText(name);    // připojení "serverového" hráče
            Thread t = new Thread(new ThreadStart(SpustHru));   //zapnutí HRY u ser. hráče
            t.Start();
            Console.WriteLine("Počet připojených klientů: " + klienti.Count);

            Thread tt = new Thread(new ThreadStart(SpustServer));   //zapnutí HERNÍHO SERVERU
            tt.Start();

            this.Dispose();
        }

        public void SpustServer()
        {
            HerniServer hs = new HerniServer(klienti);
        }

        public void SpustHru()
        {
            Application.Run(new Hra(serverConnection, name));
        }
    }


    class Tlacitko : System.Windows.Forms.Button
    {
        Server parent;

        public Tlacitko(Server parent)
        {
            this.Click += new System.EventHandler(CClick);
            this.parent = parent;
            this.Size = new Size(this.Size.Width+10,this.Size.Height);
        }

        public void CClick(object sender, EventArgs e)
        {
            parent.Pripoj(this.Text);
            Label t = new Label();  //Label o tom, na jaká IP běží server
            t.Text = "Server běží na IP: " + this.Text;
            Size s = new Size(t.Size.Width+130, t.Size.Height);
            //t.BackColor = Color.Aqua;
            t.Size = s;
            t.Top = 10;
            t.Left = 5;
            t.Font = new Font(t.Font.FontFamily, 10);
            parent.Controls.Add(t);
        }
    }
}
