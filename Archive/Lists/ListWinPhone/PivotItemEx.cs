﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace giesler.org.lists
{
    public class PivotItemEx: PivotItem
    {
        public PivotItemEx() : base() { }

        public bool IsLoaded { get; set; }
    }
}
