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
using msn2.net.Pictures;
using System.Linq;
using System.Collections.Generic;

namespace pics.Controls.Mobile
{
    public partial class Search : System.Web.UI.Page
    {
        List<DateItem> dates = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            dates = PicContext.Current.PictureManager.GetPictureDates();

            if (Page.IsPostBack == false)
            {
                var fromYears = dates.Select(d => d.Year).Distinct();
                AddValues(this.fromYear, fromYears);
                this.fromYear.SelectedIndex = this.fromYear.Items.Count - 1;
                this.OnFromYearChanged(this, EventArgs.Empty);

                var toYears = dates.Select(d => d.Year).Distinct();
                AddValues(this.toYear, toYears);
                this.toYear.SelectedIndex = this.toYear.Items.Count - 1;
                this.OnToYearChanged(this, EventArgs.Empty);
            }
        }

        void AddValues(DropDownList dd, IEnumerable<int> list)
        {
            foreach (int i in list)
            {
                dd.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        void AddMonths(DropDownList dd, IEnumerable<int> list)
        {
            foreach (int i in list)
            {
                string month = PictureManager.MonthString(i);
                dd.Items.Add(new ListItem(month, i.ToString()));
            }
        }

        protected void OnFromMonthChanged(object sender, EventArgs e)
        {
            this.fromDay.Items.Clear();
            int year = int.Parse(this.fromYear.SelectedValue);
            int month = int.Parse(this.fromMonth.SelectedValue);

            var days = from d in this.dates
                       where d.Year == year && d.Month == month
                       select d.Day;
            AddValues(this.fromDay, days.Distinct());
            this.fromDay.SelectedIndex = 0;
        }

        protected void OnFromYearChanged(object sender, EventArgs e)
        {
            this.fromMonth.Items.Clear();
            this.fromDay.Items.Clear();
            int year = int.Parse(this.fromYear.SelectedValue);

            var fromMonths = from d in this.dates
                             where d.Year == year
                             select d.Month;
            AddMonths(this.fromMonth, fromMonths.Distinct());

            this.fromMonth.SelectedIndex = 0;
            OnFromMonthChanged(this, EventArgs.Empty);
        }

        protected void OnToMonthChanged(object sender, EventArgs e)
        {
            this.toDay.Items.Clear();
            int year = int.Parse(this.toYear.SelectedValue);
            int month = int.Parse(this.toMonth.SelectedValue);

            var days = from d in this.dates
                       where d.Year == year && d.Month == month
                       select d.Day;
            AddValues(this.toDay, days.Distinct());
            this.toDay.SelectedIndex = this.toDay.Items.Count - 1;
        }

        protected void OnToYearChanged(object sender, EventArgs e)
        {
            this.toMonth.Items.Clear();
            this.toDay.Items.Clear();
            int year = int.Parse(this.toYear.SelectedValue);

            var toMonths = from d in this.dates
                           where d.Year == year
                           select d.Month;
            AddMonths(this.toMonth, toMonths.Distinct());

            this.toMonth.SelectedIndex = this.toMonth.Items.Count - 1;
            OnToMonthChanged(this, EventArgs.Empty);
        }

        protected void OnSearchClick(object sender, EventArgs e)
        {
            string url = string.Format("Pictures.aspx?from={0}/{1}/{2}&to={3}/{4}/{5}",
                this.fromMonth.SelectedValue,
                this.fromDay.SelectedValue,
                this.fromYear.SelectedValue,
                this.toMonth.SelectedValue,
                this.toDay.SelectedValue,
                this.toYear.SelectedValue);

            Response.Redirect(url);
        }
    }
}
