using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bang_.Spolecne
{
    public partial class Nastaveni : Form
    {
        public Nastaveni(bool vyber)
        {
            InitializeComponent();

            button1.Enabled = vyber;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileDialog.ShowDialog();
        }

        private void fileDialog_FileOk(object sender, CancelEventArgs e)
        {
            lbSouboru.Items.AddRange(fileDialog.FileNames);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Uloz();
            
        }

        private void btnSmaz_Click(object sender, EventArgs e)
        {
            foreach (int index in lbSouboru.SelectedIndices)
            {
                lbSouboru.Items.RemoveAt(index);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Global.Prehravac.PausePlay();
            Uloz();
            
            this.Dispose();
        }

        private void Uloz() //uloží list souborů z listBoxu a nastavení Volume do statické třídy pro přehrávání zvuku
        {
            
        }


    }
}
