using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Bang_.Spolecne;

namespace Bang_
{
    class DialogHlaska : Form
    {

        public DialogHlaska()
        {
            // Musíme nastavit standardní záhlaví
            // a okraj dialogového okna:
            FormBorderStyle = FormBorderStyle.FixedDialog;
            // Nemá se zobrazovat na hlavním panelu:
            ShowInTaskbar = false;
            // Nebude systémová nabídka:
            ControlBox = false;
            // Nebude tlačítko maximalizovat:
            MaximizeBox = false;
            // ani minimalizovat:
            MinimizeBox = false;

            // velikost okna:
            ClientSize = new Size(300,150);
        }

        public DialogHlaska(string name, string text) : this()
        {
            Text = name;

            Button btnOK = new Button();
            btnOK.Parent = this;
            btnOK.Size = new Size(5 * Font.Height, 7 * Font.Height / 4);
            btnOK.Text = "OK";
            btnOK.Click += new EventHandler(btnOK_Click);

            Label hlaska = new Label();
            hlaska.Parent = this;
            hlaska.Text = text;
            hlaska.AutoEllipsis = true;
            hlaska.AutoSize = false;
            hlaska.Size = new Size(ClientSize.Width - 10, Font.Height * 6);
            hlaska.Font = new Font(hlaska.Font.FontFamily, 10);
            //hlaska.ForeColor = System.Drawing.Color.Red;

            hlaska.Location = new Point((ClientSize.Width - hlaska.Width) / 2, ClientSize.Height / 2 - 50);
            btnOK.Location = new Point(ClientSize.Width -
            (2 * Font.Height + btnOK.Width),
            ClientSize.Height - (Font.Height + btnOK.Height));
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        public DialogHlaska(int k, Hra parr) : this()
        {
            // asi nejlepsi by bylo to volat s parentem at se nemusi predavat ty karty a tak 
            // k je cislo akce
            // 3 ... vyber postav
            //switch (k)
            //{
            //    case 3:
            //        this.parent = parr;

            //        Button btnP1 = new Button();
            //        btnP1.Parent = this;
            //        btnP1.Size = new Size(120, 7 * Font.Height / 4);
            //        btnP1.Text = parent.postavy[0].Nazev;
            //        btnP1.Click += new EventHandler(btnP1_Click);

            //        Button btnP2 = new Button();
            //        btnP2.Parent = this;
            //        btnP2.Size = new Size(120, 7 * Font.Height / 4);
            //        btnP2.Text = parent.postavy[1].Nazev;
            //        btnP2.Click += new EventHandler(btnP2_Click);

            //        Label hlaska = new Label();
            //        hlaska.Parent = this;
            //        hlaska.Text = "Vyberte postavu";
            //        hlaska.AutoEllipsis = true;
            //        hlaska.AutoSize = false;
            //        hlaska.Size = new Size(ClientSize.Width - 10, Font.Height * 6);
            //        hlaska.Font = new Font(hlaska.Font.FontFamily, 10);

            //        //hlaska.Location = new Point((ClientSize.Width - hlaska.Width) / 2, ClientSize.Height / 2 - 50);
            //        btnP1.Location = new Point(30,30);
            //        btnP2.Location = new Point(150,30);
            //        //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //        this.ResumeLayout(false);
            //        break;
            //}
        }

      // Při kliku na tlačítko OK:
      private void btnOK_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.OK;
      }

      private void btnP2_Click(object sender, EventArgs e)
      {
          DialogResult = DialogResult.No; //postava 2
      }

      private void btnP1_Click(object sender, EventArgs e)
      {
          DialogResult = DialogResult.Yes; //postava 1
      }

      private void InitializeComponent()
      {
          this.SuspendLayout();
          // 
          // DialogHlaska
          // 
          this.ClientSize = new System.Drawing.Size(284, 262);
          this.Name = "DialogHlaska";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.ResumeLayout(false);

      }
    }
}
