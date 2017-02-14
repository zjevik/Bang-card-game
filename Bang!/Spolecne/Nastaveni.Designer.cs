namespace Bang_.Spolecne
{
    partial class Nastaveni
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSmaz = new System.Windows.Forms.Button();
            this.lbSouboru = new System.Windows.Forms.ListBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.tbHlasitost = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbHlasitost)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSmaz);
            this.groupBox1.Controls.Add(this.lbSouboru);
            this.groupBox1.Controls.Add(this.btnPlay);
            this.groupBox1.Controls.Add(this.tbHlasitost);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(27, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 325);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nastavení hudby";
            // 
            // btnSmaz
            // 
            this.btnSmaz.Location = new System.Drawing.Point(143, 99);
            this.btnSmaz.Name = "btnSmaz";
            this.btnSmaz.Size = new System.Drawing.Size(96, 23);
            this.btnSmaz.TabIndex = 11;
            this.btnSmaz.Text = "Smaž ozn.";
            this.btnSmaz.UseVisualStyleBackColor = true;
            this.btnSmaz.Click += new System.EventHandler(this.btnSmaz_Click);
            // 
            // lbSouboru
            // 
            this.lbSouboru.FormattingEnabled = true;
            this.lbSouboru.HorizontalScrollbar = true;
            this.lbSouboru.Location = new System.Drawing.Point(6, 70);
            this.lbSouboru.Name = "lbSouboru";
            this.lbSouboru.ScrollAlwaysVisible = true;
            this.lbSouboru.Size = new System.Drawing.Size(120, 251);
            this.lbSouboru.TabIndex = 3;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(164, 296);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // tbHlasitost
            // 
            this.tbHlasitost.LargeChange = 500;
            this.tbHlasitost.Location = new System.Drawing.Point(6, 19);
            this.tbHlasitost.Maximum = 0;
            this.tbHlasitost.Minimum = -10000;
            this.tbHlasitost.Name = "tbHlasitost";
            this.tbHlasitost.Size = new System.Drawing.Size(233, 45);
            this.tbHlasitost.SmallChange = 100;
            this.tbHlasitost.TabIndex = 10;
            this.tbHlasitost.TickFrequency = 500;
            this.tbHlasitost.Value = -1500;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Vyber soubory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "fileDialog";
            this.fileDialog.Filter = "Music files|*.mp3;*.wav;*.wma";
            this.fileDialog.Multiselect = true;
            this.fileDialog.Title = "Vyberte soubory s hudbou";
            this.fileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.fileDialog_FileOk);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(611, 370);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Ulož";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Nastaveni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 405);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Name = "Nastaveni";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nastaveni";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbHlasitost)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbSouboru;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TrackBar tbHlasitost;
        private System.Windows.Forms.Button btnSmaz;
        private System.Windows.Forms.Button btnSave;
    }
}