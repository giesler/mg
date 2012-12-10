using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Xml;
using msn2.net.BarMonkey;

namespace BarMonkey
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
         

            
        }

        public BarMonkeyContext BarMonkeyContext
        {
            get
            {
                return BarMonkeyContext.Current;
            }
        }
    }
}
