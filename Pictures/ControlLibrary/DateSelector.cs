#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class DateSelector : UserControl
    {
        public DateSelector(DateCollection dates, string fieldName)
        {
            InitializeComponent();

            LoadTreeView(dates, fieldName);

            this.tv.HideSelection = false;
        }

        private void LoadTreeView(DateCollection dates, string fieldName)
        {
            foreach (DateItem item in dates)
            {
                TreeNode nYear = GetNode(tv.Nodes, item.Year.ToString());
                nYear.Tag = "DatePart(yyyy, " + fieldName + ") = " + nYear.Text;
                if (item.Year == DateTime.Now.Year)
                {
                    nYear.Expand();
                }

                TreeNode nMonth = GetNode(nYear.Nodes, MonthString(item.Month));
                nMonth.Tag = "DatePart(yyyy, " + fieldName + ") = " + nYear.Text + " AND "
                    + "DatePart(mm, " + fieldName + ") = " + item.Month.ToString();

                TreeNode nDay = GetNode(nMonth.Nodes, nMonth.Text + " " + item.Day.ToString());
                nDay.Tag = "DatePart(yyyy, " + fieldName + ") = " + nYear.Text + " AND "
                    + "DatePart(mm, " + fieldName + ") = " + item.Month.ToString() + " AND "
                    + "DatePart(dd, " + fieldName + ") = " + item.Day.ToString();
            }
        }

        private String MonthString(int Month)
        {
            switch (Month)
            {
                case 1: { return "January"; }
                case 2: { return "February"; }
                case 3: { return "March"; }
                case 4: { return "April"; }
                case 5: { return "May"; }
                case 6: { return "June"; }
                case 7: { return "July"; }
                case 8: { return "August"; }
                case 9: { return "September"; }
                case 10: { return "October"; }
                case 11: { return "November"; }
                case 12: { return "December"; }
            }
            return "Invalid Month";
        }


        private TreeNode GetNode(TreeNodeCollection cNodes, String sNode)
        {
            foreach (TreeNode n in cNodes)
            {
                if (n.Text == sNode)
                    return n;
            }

            // not found, so add
            return cNodes.Add(sNode);

        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (null != ItemSelected)
                {
                    TreeNode nCur = tv.SelectedNode;
                    WhereClause = nCur.Tag.ToString();
                    ItemSelected(this, EventArgs.Empty);
                }
            }

        }

        public event EventHandler ItemSelected;

        public string WhereClause;
    }
}
