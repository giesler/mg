using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using msn2.net.BarMonkey;

namespace BarMonkey
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<Drink> Drinks { get; set; }
        public static List<Container> Containers { get; set; }
        public static string[] Messages {get; set;}
        public static bool GoToNewDrinkPage { get; set; }
        public static bool IsPinValid { get; set; }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (File.Exists("motd.txt"))
            {
                App.Messages = File.ReadAllLines("motd.txt");
            }
        }
    }
}
