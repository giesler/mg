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

                if (item.EventDate.Hour > 0)
                {
                    Label hourLabel = new Label();
                    hourLabel.Text = item.EventDate.ToString("h tt") + ": ";
                    if (item.EventDate.Minute > 0)
                    {
                        hourLabel.Text = item.EventDate.ToString("h:mm tt") + ": ";
                    }
                    cell.Controls.Add(hourLabel);
                }

                HyperLink lnk = new HyperLink();
                lnk.Text = item.Title;
                lnk.Target = "_top";
                string toolTip = string.Empty;
                if (item.Location.Length > 0)
                {
                    toolTip = item.Location + Environment.NewLine;
                }
                if (item.EventDate.Date == item.EndDate.Date)
                {
                    toolTip += string.Format("{0:h:mm tt} - {1:h:mm tt}", item.EventDate, item.EndDate);
                }
                else
                {
                    toolTip += string.Format("{0:h:mm tt} - {1:MM/dd h:mm tt}", item.EventDate, item.EndDate);
                }
                lnk.ToolTip = toolTip;
                lnk.NavigateUrl = item.Url;
                cell.Controls.Add(lnk);
            }

            if (items.Count == 0)
            {
                this.noEvents.Visible = true;
            }
        }
    }
}