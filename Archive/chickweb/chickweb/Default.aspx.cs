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
            this.cam3.Click += new EventHandler(cam3_Click);
        }

        void cam1_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Response.Redirect("/?c=1");
            }

            this.cam = 1;
            this.cam1.Enabled = false;
            this.refreshLink.NavigateUrl = @"/?c=1";
            this.webcam1.ImageUrl = "getimg.aspx?c=1";
        }

        void cam2_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Response.Redirect("/?c=2");
            }

            this.cam = 2;
            this.cam2.Enabled = false;
            this.refreshLink.NavigateUrl = @"/?c=2";
            this.webcam1.ImageUrl = "getimg.aspx?c=2";
        }

        void cam3_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Response.Redirect("/?c=3");
            }

            this.cam = 3;
            this.cam3.Enabled = false;
            this.refreshLink.NavigateUrl = @"/?c=3";
            this.webcam1.ImageUrl = "getimg.aspx?c=3";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cam1.Enabled = true;
            this.cam2.Enabled = true;
            this.cam3.Enabled = true;

            string c = Request.QueryString["c"];
            if (!string.IsNullOrEmpty(c))
            {
                this.cam = int.Parse(c);
            }
            else
            {
                this.cam = 1;
            }
        
            if (this.cam == 1)
            {
                this.cam1_Click(null, null);
            }
            else if (this.cam == 2)
            {
                this.cam2_Click(null, null);
            }
            else if (this.cam == 3)
            {
                this.cam3_Click(null, null);
            }
        }

        protected int GetCameraNumber()
        {
            return this.cam;
        }
    }
}
