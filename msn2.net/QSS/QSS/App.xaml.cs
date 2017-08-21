using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace msn2.net
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow owner = null;
            int index = 0;

            foreach (Screen screen in Screen.AllScreens)
            {
                if (index != 0 || !e.Args.Contains("/s"))
                {
                    MainWindow window = new MainWindow(e.Args);
                    window.Top = screen.Bounds.Top;
                    window.Left = screen.Bounds.Left;
                    window.Topmost = true;
                    window.Height = screen.Bounds.Height;
                    window.Width = screen.Bounds.Width;

                    if (owner == null)
                    {
                        owner = window;
                    }
                    else
                    {
                        window.Owner = owner;
                    }

                    window.Show();

                    if (e.Args.Contains("/p"))
                    {
                        break;
                    }
                }
                index++;
            }
        }
    }
}
