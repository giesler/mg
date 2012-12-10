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
using System.Windows.Data;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    public class ListSummaryItemFormatter: IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                ListEx list = (ListEx)value;
                if (list.Items != null && list.Items.Count > 0)
                {
                    return string.Format("{0} ({1})", list.Name, list.Items.Count);
                }
                else
                {
                    return list.Name;
                }
            }
            else
            {
                throw new ArgumentNullException("value");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
