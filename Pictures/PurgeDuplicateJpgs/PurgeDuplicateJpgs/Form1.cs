using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurgeDuplicateJpgs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.Visible = true;
                this.Refresh();

                string path = dialog.SelectedPath;
                List<string> purge = new List<string>();

                FindDupes(path, purge);

                if (purge.Count > 0)
                {
                    string msg = string.Format("Purge {0} jpg files?", purge.Count);
                    if (MessageBox.Show(msg, "Purge", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.Refresh();
                        foreach (string file in purge)
                        {
                            File.Delete(file);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No files to purge", "Purge", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            this.Close();
        }

        private static void FindDupes(string path, List<string> purge)
        {
            string[] files = Directory.GetFiles(path, "*.cr2", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string jpgFile = file.ToLower().Replace("cr2", "jpg");
                if (File.Exists(jpgFile))
                {
                    purge.Add(jpgFile);
                }
            }

            string[] jpgs = Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly);
            foreach (string jpg in jpgs)
            {
                string chefJpg = jpg.ToLower().Replace(".jpg", "-chef.jpg");
                if (File.Exists(chefJpg))
                {
                    purge.Add(chefJpg);
                }
            }

            foreach (string subdir in Directory.GetDirectories(path))
            {
                FindDupes(subdir, purge);
            }
        }

    }
}
