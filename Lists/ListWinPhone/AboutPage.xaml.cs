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
using Microsoft.Phone.Controls;
using System.Reflection;

namespace giesler.org.lists
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();

            Assembly a = Assembly.GetExecutingAssembly();
            AssemblyName an = new AssemblyName(a.FullName);

            this.details.Text = string.Format("Version {0}{1}{1}(c) 2011 - Mike Giesler", an.Version, Environment.NewLine);
        }
    }
}