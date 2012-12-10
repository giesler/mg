﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace QSS
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

            foreach (Screen screen in Screen.AllScreens)
            {
                MainWindow window = new MainWindow();
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
                    window.Closed += new EventHandler(OnChildWindowClosed);
                }
                                
                window.Show();
            }
        }

        void OnChildWindowClosed(object sender, EventArgs e)
        {
            MainWindow window = (MainWindow)sender;
            window.Owner.Close();
        }
    }
}
