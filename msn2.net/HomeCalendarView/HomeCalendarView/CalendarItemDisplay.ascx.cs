using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

namespace HomeCalendarView
{
    public partial class CalendarItemDisplay : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindToEventList(List<CalendarItem> items)
        {
            Table t = new Table();
            this.Controls.Add(t);

            foreach (CalendarItem item in items.OrderBy(ci=>ci.EventDate))
            {
                TableRow row = new TableRow();
                t.Rows.Add(row);

                TableCell cell = new TableCell();
                row.Cells.Add(cell);

                HyperLink lnk = new HyperLink();
                lnk.Text = item.Title;
                lnk.Target = "_top";
                //lnk.NavigateUrl = item.Url;
                cell.Controls.Add(lnk);
            }

            if (items.Count == 0)
            {
                this.noEvents.Visible = true;
            }
        }
    }
}