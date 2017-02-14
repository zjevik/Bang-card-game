using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Bang_.Spolecne.Karty
{
    public class KartaImage<T> : PictureBox where T : Karta
    {
        Hra parent;
        T karta;
        Point point;
        Size size;
        Boolean animace;
        Boolean specialniAkce = true;       // protože kartu vytváříme dvakrát a podruhé nechceme, aby se provedla inicializace schopností
        Point nahled;

        public KartaImage(Hra parent, T karta, Point point, Size size, int zpusobPosunu, Boolean animace) //rodič, vlastní karta, která se má zobrazit, bod, který bude uprostřed karty
        {
            this.karta = karta;
            this.parent = parent;
            this.point = point;
            this.size = size;
            this.animace = animace;
            this.nahled = parent.KartaNahled.Location;
            Inicialization();
        }
        public KartaImage(Hra parent, T karta, Point point, Size size, int zpusobPosunu, Boolean animace, Boolean specAkce) 
        {
            this.specialniAkce = specAkce;
            this.karta = karta;
            this.parent = parent;
            this.point = point;
            this.size = size;
            this.animace = animace;
            this.nahled = parent.KartaNahled.Location;
            Inicialization();
        }
        public void SetNahledPozition(Point nah)
        {
            this.nahled = nah;
        }
        private void Inicialization()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BackColor = System.Drawing.Color.Violet;
            this.Location = new Point(point.X-95,point.Y-135);
            this.Name = "pictureBox1";
            this.Size = size;
            //this.Size = new Size(new Point(400, 400));
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            //String t = Directory.GetCurrentDirectory();
            //Bitmap bmp = (Bitmap)global::Bang_.Properties.Resources.;
            //this.Image = (Image)bmp;
            this.Image = karta.GetObrazek();
            //this.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            this.MouseHover += new System.EventHandler(MMouseHover);
            this.MouseLeave += new System.EventHandler(MMouseLeave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(MMouseClick);
            if ((karta is PostavaKarta) && (this.specialniAkce))  // pokud je karta instancí Postavy, pak naváže metodu PostavaClick
            {
                this.MouseClick += new MouseEventHandler(PostavaClick);
            }
            if (karta is KartaHneda)
            {
                this.MouseClick += new MouseEventHandler(KartaHnedaClick);
            }
            if (karta is KartaModra)
            {
                this.MouseClick += new MouseEventHandler(KartaModraClick);
            }
            this.BringToFront();
            //GraphicsPath gp = new GraphicsPath();
            //PointF[] p = new PointF[9];
            //Font f;
            //p[0] = new PointF(70, 0);
            //p[1] = new PointF(170, 0);
            //p[2] = new PointF(240, 70);
            //p[3] = new PointF(240, 170);
            //p[4] = new PointF(170, 240);
            //p[5] = new PointF(70, 240);
            //p[6] = new PointF(0, 170);
            //p[7] = new PointF(0, 70);
            //p[8] = new PointF(70, 0);
            //gp.AddPolygon(p);
            //Region r = new Region(gp);
            //this.Region = r;
        }

        void KartaModraClick(object sender, MouseEventArgs e)  //pokud klikneme na modrou hrací kartu
        {
            KartaModra temp = (KartaModra)(Karta)karta;
            temp.Akce(this.parent, temp);
        }

        void KartaHnedaClick(object sender, MouseEventArgs e) //pokud klikneme na hnědou hrací kartu
        {
            KartaHneda temp = (KartaHneda)(Karta)karta;
            temp.Akce(this.parent, temp);
        }

        public void MMouseHover(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            if (animace)
            {
                if (this.size == (new Size(new Point(120, 80))))
                {
                    parent.KartaNahled.Image = (Image)this.Image.Clone();
                    parent.KartaNahled.Image.RotateFlip(RotateFlipType.Rotate90FlipXY);

                }
                else parent.KartaNahled.Image = this.Image;
                
                Application.DoEvents();
                for (int i = 0; i < 201; i++)
		        {
                    parent.KartaNahled.Location = new Point(parent.KartaNahled.Location.X - 1, parent.KartaNahled.Location.Y);                             
                }
                //parent.KartaNahled.Location = new Point(this.nahled.X - 200, this.nahled.Y);
            }
            
            //this.Location = new Point(this.Location.X - 80, this.Location.Y);
        }

        public void MMouseLeave(object sender, EventArgs e)
        {
            //this.Location = new Point(this.Location.X + 80, this.Location.Y);
            if (animace)
            {
                for (int i = 0; i < 201; i++)
                {
                    parent.KartaNahled.Location = new Point(parent.KartaNahled.Location.X + 1, parent.KartaNahled.Location.Y);
                }
                parent.KartaNahled.Location = this.nahled;
            }
            //parent.KartaNahled.Image = null;
        }

        public void MMouseClick(object sender, EventArgs e)
        {
        }

        public void PostavaClick(object sender, EventArgs e)
        {
            parent.Postava = (PostavaKarta)(Karta)karta;
            //parent.Controls.Remove(this);
            this.Invoke(new MethodInvoker(delegate() { parent.VyberPostav(); }));
            foreach (var item in parent.postavy)
            {
                parent.Controls.Remove(item);
            }
            parent.postavy = null;
            PostavaKarta temp = (PostavaKarta)(Karta)karta;
            temp.InicializaceVlastnosti(this.parent, temp);
           
        }

    }
}
