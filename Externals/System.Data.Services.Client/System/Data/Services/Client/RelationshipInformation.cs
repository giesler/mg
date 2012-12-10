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

namespace System.Data.Services.Client
{
    internal class RelationshipInformation
    {
        public DataServiceContext Context { get; set; }
        public object Parent { get; set; }
        public string PropertyName { get; set; }


        // Methods
        internal RelationshipInformation(DataServiceContext context, object parent, string propertyName)
        {
            this.Context = context;
            this.Parent = parent;
            this.PropertyName = propertyName;
        }
    }
}
