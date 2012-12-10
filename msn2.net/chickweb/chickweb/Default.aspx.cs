using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace chickweb
{
    public partial class _Default : System.Web.UI.Page
    {
        private int cam = 1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.cam1.Click += new EventHandler(cam1_Click);
            this.cam2.Click += new EventHandler(cam2_Click);
        }

        void cam2_Click(object sender, EventArgs e)
        {
            this.cam = 2;
            this.cam1.Enabled = true;
            this.cam2.Enabled = false;
            this.refreshLink.NavigateUrl = @"/?c=2";
        }

        void cam1_Click(object sender, EventArgs e)
        {
            this.cam = 1;
            this.cam1.Enabled = false;
            this.cam2.Enabled = true;
            this.refreshLink.NavigateUrl = @"/?c=1";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string c = Request.QueryString["c"];
                if (!string.IsNullOrEmpty(c))
                {
                    this.cam = int.Parse(c);
                }
            }
        }

        protected int GetCameraNumber()
        {
            return this.cam;
        }
    }
}
