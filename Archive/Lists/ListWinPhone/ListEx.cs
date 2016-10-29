using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    public class ListEx: List
    {
        public ListEx()
        {
            this.Items = new System.Collections.Generic.List<ListItemEx>();
        }

        public System.Collections.Generic.List<ListItemEx> Items { get; set; }

        public override string ToString()
        {
            return base.Name;
        }
    }
}
