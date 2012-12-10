using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;

namespace ChickWebApp
{
    public partial class AboutDialog : ChildWindow
    {
        public AboutDialog()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            if (!string.IsNullOrEmpty(assembly.FullName))
            {
                string[] parts = assembly.FullName.Split(',');
                this.text.Text += parts[1].Replace("=", " ");
            }
        }

        void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
