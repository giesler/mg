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
        public DateSelector(List<DateItem> dates, string fieldName)
        {
            InitializeComponent();

            LoadTreeView(dates, fieldName, tv.Nodes, 0);

            this.tv.HideSelection = false;
        }

        public static void LoadTreeView(List<DateItem> dates, string fieldName, TreeNodeCollection rootCollection, int imageIndex)
        {
            foreach (DateItem item in dates)
            {
                DateFilterTreeNode nYear = GetNode(rootCollection, item.Year.ToString(), imageIndex);
                nYear.Tag = "DatePart(yyyy, " + fieldName + ") = " + nYear.Text;
                //if (item.Year == DateTime.Now.Year)
                //{
                //    nYear.Expand();
                //}

                DateFilterTreeNode nMonth = GetNode(nYear.Nodes, PictureManager.MonthString(item.Month), imageIndex);
                nMonth.Tag = "DatePart(yyyy, " + fieldName + ") = " + nYear.Text + " AND "
                    + "DatePart(mm, " + fieldName + ") = " + item.Month.ToString();

                DateFilterTreeNode nDay = GetNode(nMonth.Nodes, nMonth.Text + " " + item.Day.ToString(), imageIndex);
                nDay.Tag = "DatePart(yyyy, " + fieldName + ") = " + nYear.Text + " AND "
                    + "DatePart(mm, " + fieldName + ") = " + item.Month.ToString() + " AND "
                    + "DatePart(dd, " + fieldName + ") = " + item.Day.ToString();
            }
        }
        
        private static DateFilterTreeNode GetNode(TreeNodeCollection cNodes, string sNode, int imageIndex)
        {
            foreach (DateFilterTreeNode n in cNodes)
            {
                if (n.Text == sNode)
                    return n;
            }

            // not found, so add
            DateFilterTreeNode node = new DateFilterTreeNode(sNode);
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;
            cNodes.Add(node);
            return node;
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

    public class DateFilterTreeNode : TreeNode
    {
        public DateFilterTreeNode(string text) : base(text) { }
    }
}
