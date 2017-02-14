namespace Bang_.Spolecne
{
    partial class Hra
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbInput = new System.Windows.Forms.TextBox();
            this.tbChat = new System.Windows.Forms.RichTextBox();
            this.KartaNahled = new System.Windows.Forms.PictureBox();
            this.bKonec = new System.Windows.Forms.Button();
            this.lName3 = new System.Windows.Forms.Label();
            this.lName2 = new System.Windows.Forms.Label();
            this.lName4 = new System.Windows.Forms.Label();
            this.lName5 = new System.Windows.Forms.Label();
            this.btn_ZahodKarty = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.souborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nastaveníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uložToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.načtiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zpětToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lName1 = new System.Windows.Forms.Label();
            this.nováHraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.KartaNahled)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbInput
            // 
            this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbInput.Location = new System.Drawing.Point(2, 738);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(190, 20);
            this.tbInput.TabIndex = 1;
            this.tbInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbInput_KeyPress);
            // 
            // tbChat
            // 
            this.tbChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbChat.BackColor = System.Drawing.SystemColors.Window;
            this.tbChat.Cursor = System.Windows.Forms.Cursors.Default;
            this.tbChat.Location = new System.Drawing.Point(2, 553);
            this.tbChat.Name = "tbChat";
            this.tbChat.ReadOnly = true;
            this.tbChat.Size = new System.Drawing.Size(190, 179);
            this.tbChat.TabIndex = 3;
            this.tbChat.Text = "";
            // 
            // KartaNahled
            // 
            this.KartaNahled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.KartaNahled.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.KartaNahled.Location = new System.Drawing.Point(805, 449);
            this.KartaNahled.Name = "KartaNahled";
            this.KartaNahled.Size = new System.Drawing.Size(200, 309);
            this.KartaNahled.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.KartaNahled.TabIndex = 4;
            this.KartaNahled.TabStop = false;
            // 
            // bKonec
            // 
            this.bKonec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bKonec.Location = new System.Drawing.Point(2, 524);
            this.bKonec.Name = "bKonec";
            this.bKonec.Size = new System.Drawing.Size(190, 23);
            this.bKonec.TabIndex = 5;
            this.bKonec.Text = "Konec kola";
            this.bKonec.UseVisualStyleBackColor = true;
            this.bKonec.Click += new System.EventHandler(this.bKonec_Click);
            // 
            // lName3
            // 
            this.lName3.Location = new System.Drawing.Point(369, 32);
            this.lName3.Name = "lName3";
            this.lName3.Size = new System.Drawing.Size(80, 13);
            this.lName3.TabIndex = 7;
            this.lName3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lName2
            // 
            this.lName2.Location = new System.Drawing.Point(191, 32);
            this.lName2.Name = "lName2";
            this.lName2.Size = new System.Drawing.Size(80, 15);
            this.lName2.TabIndex = 8;
            this.lName2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lName4
            // 
            this.lName4.Location = new System.Drawing.Point(550, 32);
            this.lName4.Name = "lName4";
            this.lName4.Size = new System.Drawing.Size(80, 13);
            this.lName4.TabIndex = 9;
            this.lName4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lName5
            // 
            this.lName5.Location = new System.Drawing.Point(730, 32);
            this.lName5.Name = "lName5";
            this.lName5.Size = new System.Drawing.Size(80, 13);
            this.lName5.TabIndex = 10;
            // 
            // btn_ZahodKarty
            // 
            this.btn_ZahodKarty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ZahodKarty.Location = new System.Drawing.Point(2, 495);
            this.btn_ZahodKarty.Name = "btn_ZahodKarty";
            this.btn_ZahodKarty.Size = new System.Drawing.Size(190, 23);
            this.btn_ZahodKarty.TabIndex = 17;
            this.btn_ZahodKarty.Text = "Zahodit karty";
            this.btn_ZahodKarty.UseVisualStyleBackColor = true;
            this.btn_ZahodKarty.Click += new System.EventHandler(this.btn_ZahodKarty_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.souborToolStripMenuItem,
            this.zpětToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // souborToolStripMenuItem
            // 
            this.souborToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nováHraToolStripMenuItem,
            this.načtiToolStripMenuItem,
            this.uložToolStripMenuItem,
            this.nastaveníToolStripMenuItem});
            this.souborToolStripMenuItem.Name = "souborToolStripMenuItem";
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.souborToolStripMenuItem.Text = "Soubor";
            // 
            // nastaveníToolStripMenuItem
            // 
            this.nastaveníToolStripMenuItem.Name = "nastaveníToolStripMenuItem";
            this.nastaveníToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nastaveníToolStripMenuItem.Text = "Nastavení";
            this.nastaveníToolStripMenuItem.Click += new System.EventHandler(this.nastaveníToolStripMenuItem_Click);
            // 
            // uložToolStripMenuItem
            // 
            this.uložToolStripMenuItem.Name = "uložToolStripMenuItem";
            this.uložToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.uložToolStripMenuItem.Text = "Uložit";
            this.uložToolStripMenuItem.Click += new System.EventHandler(this.uložToolStripMenuItem_Click);
            // 
            // načtiToolStripMenuItem
            // 
            this.načtiToolStripMenuItem.Name = "načtiToolStripMenuItem";
            this.načtiToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.načtiToolStripMenuItem.Text = "Načti";
            this.načtiToolStripMenuItem.Click += new System.EventHandler(this.načtiToolStripMenuItem_Click);
            // 
            // zpětToolStripMenuItem
            // 
            this.zpětToolStripMenuItem.Name = "zpětToolStripMenuItem";
            this.zpětToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.zpětToolStripMenuItem.Text = "Zpět";
            this.zpětToolStripMenuItem.Click += new System.EventHandler(this.zpětToolStripMenuItem_Click);
            // 
            // lName1
            // 
            this.lName1.Location = new System.Drawing.Point(12, 32);
            this.lName1.Name = "lName1";
            this.lName1.Size = new System.Drawing.Size(80, 13);
            this.lName1.TabIndex = 19;
            this.lName1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nováHraToolStripMenuItem
            // 
            this.nováHraToolStripMenuItem.Name = "nováHraToolStripMenuItem";
            this.nováHraToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nováHraToolStripMenuItem.Text = "Nová hra";
            this.nováHraToolStripMenuItem.Click += new System.EventHandler(this.nováHraToolStripMenuItem_Click);
            // 
            // Hra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1008, 762);
            this.Controls.Add(this.lName1);
            this.Controls.Add(this.btn_ZahodKarty);
            this.Controls.Add(this.KartaNahled);
            this.Controls.Add(this.lName5);
            this.Controls.Add(this.lName4);
            this.Controls.Add(this.lName2);
            this.Controls.Add(this.lName3);
            this.Controls.Add(this.bKonec);
            this.Controls.Add(this.tbChat);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Hra";
            this.Text = "Hra";
            this.Resize += new System.EventHandler(this.Hra_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.KartaNahled)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.RichTextBox tbChat;
        public System.Windows.Forms.PictureBox KartaNahled;
        private System.Windows.Forms.Button bKonec;
        private System.Windows.Forms.Label lName3;
        private System.Windows.Forms.Label lName2;
        private System.Windows.Forms.Label lName4;
        private System.Windows.Forms.Label lName5;
        private System.Windows.Forms.Button btn_ZahodKarty;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem souborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nastaveníToolStripMenuItem;
        private System.Windows.Forms.Label lName1;
        private System.Windows.Forms.ToolStripMenuItem uložToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem načtiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zpětToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nováHraToolStripMenuItem;
    }
}