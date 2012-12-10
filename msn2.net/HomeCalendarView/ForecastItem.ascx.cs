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

namespace HomeCalendarView
{
    public enum TemperatureExtreme
    {
        High,
        Low
    }

    public partial class ForecastItem : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string ImageUrl
        {
            get
            {
                return this.image.ImageUrl;
            }
            set
            {
                this.image.ImageUrl = value;
            }
        }

        public string ImageAltText
        {
            get
            {
                return this.image.ToolTip;
            }
            set
            {
                this.image.ToolTip = value;
            }
        }

        public string Temperature
        {
            get
            {
                return this.tempText.Text;
            }
            set
            {
                this.tempText.Text = value;
            }
        }

        public string Precipitation
        {
            get
            {
                return this.precipText.Text;
            }
            set
            {
                //this.precipText.Text = value;
            }
        }

        public TemperatureExtreme TemperatureExtreme
        {
            get
            {
                return this.tempText.CssClass == "hiText" ? TemperatureExtreme.High : TemperatureExtreme.Low;
            }
            set
            {
                this.tempText.CssClass = value == TemperatureExtreme.High ? "hiText" : "loText";
            }
        }
        
    }
}