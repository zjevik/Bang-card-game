using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Bang_.Spolecne.Karty;
using System.Windows.Forms;

namespace Bang_.Spolecne
{
    public class HraPanel : System.Windows.Forms.Panel
    {
        Hra parent;
        Karta karta;

        public HraPanel(String[] jmenaHracu, Hra parent, Karta karta)
        {
            this.parent = parent;
            this.karta = karta;
            this.Top = (parent.Height / 2) - ((50 + 30 * jmenaHracu.Length)/2);
            this.Left = parent.Width / 2 - 50;

            this.Height = 50 + 30*jmenaHracu.Length;
            this.Width = 100;
            System.Windows.Forms.Button[] b = new System.Windows.Forms.Button[jmenaHracu.Length];
            for (int i = 0; i < jmenaHracu.Length; i++)
            {
                b[i] = new System.Windows.Forms.Button();
                b[i].Text = jmenaHracu[i];
                b[i].Name = "shoot";
                b[i].Top = 25 + 30 * i;
                b[i].Parent = this;
                b[i].MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
                this.Controls.Add(b[i]);
            }
            Button ok = new Button();
            ok.Top = 25 + 30 * jmenaHracu.Length;
            ok.Text = "zpět";
            ok.Name = "back";
            ok.Parent = this;
            ok.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
            this.Controls.Add(ok);
            parent.Controls.Add(this);
            this.BringToFront();
        }

        public HraPanel(FileInfo[] soubory, Hra parent)
        {
            this.parent = parent;
            this.Top = (parent.Height / 2) - 100;
            this.Left = parent.Width / 2 - 75;
            this.Width = 150;
            this.Height = 200;

            ListBox lb = new ListBox();
            foreach (var soubor in soubory)
            {
                lb.Items.Add(soubor.Name);
            }
            lb.Tag = soubory;
            lb.Click += new System.EventHandler(lb_Click);
            this.Controls.Add(lb);

            Button ok = new Button();
            ok.Top = 160;
            ok.Left = 150 - ok.Width;
            ok.Text = "Zpět";
            ok.Name = "continue";
            ok.Parent = this;
            ok.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
            this.Controls.Add(ok);

            parent.Controls.Add(this);
            this.BringToFront();
        }

        void lb_Click(object sender, EventArgs e) //vybrání souboru, který se má načíst
        {
            Transit t = new Transit();
            t.obsah = 16;
            ListBox lb = (ListBox)sender;
            t.text = (string)lb.Items[lb.SelectedIndex];
            foreach (var soubor in (FileInfo[])lb.Tag)
            {
                if (soubor.Name.CompareTo(t.text) == 0) t.objekt = soubor;
            }
            parent.spojeni.Send(t); // poslání na server, že má zkusit nahrát tenhle soubor

            this.Dispose(true);
        }

        public HraPanel(String text, Hra parent) //zobrazí daný text
        {
            this.parent = parent;
            this.Top = (parent.Height / 2) - 100;
            this.Left = parent.Width / 2 - 75;
            this.Width = 150;
            this.Height = 200;

            
            RichTextBox popisek = new RichTextBox();
            popisek.Width = 150;
            popisek.Height = 150;
            //popisek.Enabled = false;
            popisek.Text = text;
            popisek.Parent = this;
            this.Controls.Add(popisek);

            Button ok = new Button();
            ok.Top = 160;
            ok.Left = 150-ok.Width;
            ok.Text = "OK";
            ok.Name = "continue";
            ok.Parent = this;
            ok.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
            this.Controls.Add(ok);

            parent.Controls.Add(this);
            this.BringToFront();
        }

        public HraPanel(String mode, Hra parent, Karta karta) //pro indiána
        {
            this.parent = parent;
            this.karta = karta;                    
            this.Top = (parent.Height / 2) - 100;
            this.Left = parent.Width / 2 - 75;
            this.Width = 150;
            this.Height = 200;
            switch (mode.Substring(0,6))
            {
                case "indian":
                    RichTextBox popisek = new RichTextBox();
                    popisek.Width = 150;
                    popisek.Height = 150;
                    popisek.Text = "Zaútočili na vás indiáni. Chcete se bránit kartou Bang? Pokud tak neučiníte, tak vám bude ubrán jeden život";
                    popisek.Parent = this;
                    this.Controls.Add(popisek);

                    Button ok = new Button();
                    ok.Top = 160;
                    ok.Left = 150 - ok.Width;
                    ok.Text = "Bránit se!";
                    ok.Name = "attack";
                    ok.Parent = this;
                    ok.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
                    this.Controls.Add(ok);

                    Button ne = new Button();
                    ne.Top = 160;
                    ne.Text = "Nebránit se";
                    ne.Name = "letItBe";
                    ne.Parent = this;
                    ne.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
                    this.Controls.Add(ne);
                    break;
                case "strela":
                    RichTextBox popisek1 = new RichTextBox();
                    popisek1.Width = 150;
                    popisek1.Height = 150;
                    popisek1.Text = "Hráč " + mode.Substring(6) + " na vás vystřelil, chcete na obranu použít kartu Vedle?";
                    popisek1.Parent = this;
                    this.Controls.Add(popisek1);

                    Button ok1 = new Button();
                    ok1.Top = 160;
                    ok1.Left = 150 - ok1.Width;
                    ok1.Text = "Bránit se!";
                    ok1.Name = "deffend";
                    ok1.Parent = this;
                    ok1.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
                    this.Controls.Add(ok1);

                    Button ne1 = new Button();
                    ne1.Top = 160;
                    ne1.Text = "Nebránit se";
                    ne1.Name = "letItBe";
                    ne1.Parent = this;
                    ne1.MouseClick += new System.Windows.Forms.MouseEventHandler(myClick);
                    this.Controls.Add(ne1);
                    break;

            }

            parent.Controls.Add(this);
            this.BringToFront();
        }

        /*
        public HraPanel(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            System.Windows.Forms.Button[] b = new System.Windows.Forms.Button[4];
            for (int i = 0; i < 3; i++)
            {
                b[i] = new System.Windows.Forms.Button();
                b[i].Text = Convert.ToString(i);
                b[i].Top = 30 * i;
                b[i].Parent = this;
                b[i].MouseClick += new System.Windows.Forms.MouseEventHandler(Click);
                this.Controls.Add(b[i]);
            }
        }
        */

        void myClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;

            switch (btn.Name)
            {
                case "continue": //tlačítko zpět obyč
                    this.Dispose(true);
                    break;
                case "back": //lačítko zpět při bangu
                    this.Dispose(true);
                    parent.zahranyBang = false;
                    parent.aktivniTah = true;
                    break;
                case "attack":
                    parent.ZahodKartuZRuky(karta);
                    break;
                case "letItBe":
                    //Console.WriteLine("---- volam OdectiZivot() , radek 180, HraPanel ---- ");
                    parent.OdectiZivot();
                    break;
                case "deffend":
                    //Console.WriteLine(karta.Nazev + " tady!!!");
                    parent.ZahodKartuZRuky(karta);
                    break;
                case "shoot":
                    Transit posli = new Transit();  //poslání, že vystřelil
                    posli.obsah = 6;
                    posli.text = btn.Text;
                    parent.spojeni.Send(posli);
                    parent.ZahodKartuZRuky(karta);
                    parent.zahranyBang = true;
                    parent.aktivniTah = true;
                    break;
            }
            this.Dispose(true);
        }



    }
}
