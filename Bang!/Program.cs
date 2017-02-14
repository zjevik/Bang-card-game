using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Bang_.Spolecne.Karty;
using Bang_.Spolecne;
using System.IO;

namespace Bang_
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DirectoryInfo autoSave = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\autosave");
            if (!autoSave.Exists)
            {
                autoSave.Create();
            }
            else
            {
                autoSave.Delete(true);
                autoSave.Create();
            }

            DirectoryInfo save = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\save");
            if (!save.Exists) save.Create();

            Application.Run(new Welcome());
            //Application.Run(new Bang_.Spolecne.Nastaveni());            

        }
    }
}
