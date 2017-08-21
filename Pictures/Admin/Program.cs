using System;
using System.Windows.Forms;
using msn2.net.Pictures.Controls;

namespace msn2.net.Pictures
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();

                fMain f = new fMain();

                Application.Run(f);
            }
            catch (Exception ex)
            {
                ExceptionDialog dialog = new ExceptionDialog(ex);
                dialog.ShowDialog();
            }
        }
    }
}
