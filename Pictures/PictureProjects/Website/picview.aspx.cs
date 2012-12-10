using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using pics.Controls;
using msn2.net.Pictures;
using System.Text;

namespace pics
{
    /// <summary>
    /// Summary description for picview.
    /// </summary>
    public partial class picview : Page, ICallbackEventHandler
    {
        #region Declares

        protected System.Web.UI.WebControls.Panel pnlDescription;
        protected System.Web.UI.WebControls.Panel pnlPeople;
        protected System.Web.UI.WebControls.Panel Panel1;
        protected System.Web.UI.HtmlControls.HtmlTableRow titleRow;
        protected System.Web.UI.WebControls.HyperLink Hyperlink1;
        protected System.Web.UI.WebControls.HyperLink Hyperlink2;
        protected System.Web.UI.WebControls.Panel Panel2;
        protected System.Web.UI.WebControls.Label Label8;
        protected System.Web.UI.WebControls.HyperLink Hyperlink3;
        protected System.Web.UI.WebControls.Panel Panel3;
        protected string m_HttpRefreshURL;
        protected System.Web.UI.WebControls.LinkButton clearBasket;
        protected int pictureId;
        protected string ratingServerCallbackFunction;
        protected string callbackReturnValue = null;

        #endregion

        public static string BuildRandomPageUrl(int pictureId, string refUrl)
        {
            string url = "picview.aspx?p=" + pictureId.ToString()
                + "&type=random&RefUrl=" + HttpContext.Current.Server.UrlEncode(refUrl);

            return url;
        }

        public string RatingServerCallbackFunction
        {
            get
            {
                return this.ratingServerCallbackFunction;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ratingServerCallbackFunction = Page.ClientScript.GetCallbackEventReference(
                this,
                "rating",
                "OnRatingSaved",
                "rating");

            if (ViewState["pictureId"] != null)
            {
                this.pictureId = Convert.ToInt32(ViewState["pictureId"].ToString());
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Set link to return to list
            if (Request.QueryString["RefURL"] != null)
            {
                //lnkReturn.Text = "Return<br />to list";
                lnkReturn.NavigateUrl = Request.QueryString["RefURL"];
            }

            // make sure we have a picid
            if (Request.QueryString["r"] == null && Request.QueryString["p"] == null)
            {
                lblTitle.Text = "There was no record number passed to the page.";
            }
            else
            {
                // figure out the source type
                string sourceType = Request.QueryString["type"];

                // init connection and command
                SqlConnection cn = new SqlConnection(PicContext.Current.Config.ConnectionString);

                // Set up SP to retreive pictures
                SqlCommand cmdPic = new SqlCommand();
                if (sourceType.Equals("category"))
                {
                    cmdPic.CommandText = "p_Category_GetPictures";	/// switch based on type
                    SetCategory(Convert.ToInt32(Request.QueryString["c"]));

                    SetSortFields(cmdPic);
                }
                else if (sourceType.Equals("search"))
                {
                    cmdPic.CommandText = "p_Search_GetPictures";	/// switch based on type
                }
                else if (sourceType.Equals("random"))
                {
                    cmdPic.CommandText = "p_GetPicture";
                }
                cmdPic.CommandType = CommandType.StoredProcedure;
                cmdPic.Connection = cn;
                SqlDataAdapter daPic = new SqlDataAdapter(cmdPic);

                if (sourceType.Equals("category"))
                {
                    //lblCategory.Text	= "toget: category";
                }
                else if (sourceType.Equals("search"))
                {
                    lblCategory.Text = "Search Results";
                }
                else if (sourceType.Equals("random"))
                {
                    lblCategory.Text = "Random Picture";
                }

                // set up params on the SP
                if (sourceType.Equals("category"))
                {
                    cmdPic.Parameters.AddWithValue("@CategoryID", Convert.ToInt32(Request.QueryString["c"]));
                }
                else if (sourceType.Equals("search"))
                {
                    Guid id = new Guid(Request.QueryString["id"]);
                    cmdPic.Parameters.AddWithValue("@SearchID", id);
                }
                else if (sourceType.Equals("random"))
                {
                    cmdPic.Parameters.AddWithValue("@PictureID", Convert.ToInt32(Request.QueryString["p"]));
                }
                cmdPic.Parameters.AddWithValue("@StartRecord", Convert.ToInt32(Request.QueryString["r"]));
                cmdPic.Parameters.AddWithValue("@ReturnCount", 1);
                cmdPic.Parameters.AddWithValue("@MaxHeight", 700);
                cmdPic.Parameters.AddWithValue("@MaxWidth", 750);
                cmdPic.Parameters.AddWithValue("@PersonID", PicContext.Current.CurrentUser.Id);
                cmdPic.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
                cmdPic.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

                // run the SP, set datasource to the picture list
                cn.Open();
                DataSet ds = new DataSet();
                daPic.Fill(ds, "Picture");
                cn.Close();

                DataRow dr = ds.Tables[0].Rows[0];

                // now set the controls on the page
                if (!dr.IsNull("Title") && dr["Title"].ToString().Length > 0)
                    lblTitle.Text = dr["Title"].ToString() + "<br />";
                else
                    lblTitle.Visible = false;
                if (!dr.IsNull("PictureDate"))
                    lblPictureDate.Text = Convert.ToDateTime(dr["PictureDate"]).ToString("ddd, MMM d \"'\"yy");
                else
                    lblPictureDate.Visible = false;
                if (!dr.IsNull("Description") && dr["Description"].ToString().Length > 0)
                {
                    lblPictureDesc.Text = dr["Description"].ToString();
                }
                else
                {
                    lblPictureDesc.Text = "";
                    descriptonBottomSplitter.Visible = false;
                }
                if (!dr.IsNull("Rating"))
                {
                    ClientScript.RegisterHiddenField("ratingValue", dr["Rating"].ToString());
                    decimal average = Decimal.Parse(dr["AverageRating"].ToString());
                    string averageText = this.GetAverageText(average);
                    averageRating.Controls.Add(new HtmlLiteral(averageText));
                }
                else
                {
                    ClientScript.RegisterHiddenField("ratingValue", "0");
                }
                if (!dr.IsNull("PictureByFullName"))
                {
                    pictureBy.Text = "<<br /> />Taken by: " + dr["PictureByFullName"].ToString();
                }
                else
                {
                    pictureBy.Text = "";
                }

                // now create the picture
                PictureImageControl curPic = new PictureImageControl();
                //curPic.Filename = dr["Filename"].ToString();
                pictureId = (int)dr["PictureId"];
                if (this.ViewState["pictureId"] == null)
                {
                    this.ViewState.Add("pictureId", pictureId);
                }
                else
                {
                    this.ViewState["pictureId"] = pictureId;
                }
                curPic.SetPictureById(pictureId, 700, 750);
                curPic.Height = Convert.ToInt32(dr["Height"]);
                curPic.Width = Convert.ToInt32(dr["Width"]);
                curPic.ID = "currentPicture";
                tdPicture.Controls.Add(curPic);

                if (Global.AdminMode)
                {
                    PictureEditFormLink ef = new PictureEditFormLink((int)dr["PictureId"]);
                    editLinkPanel.Controls.Add(ef);

                    leftPanel.Visible = true;

                    securityList.DataSource = PicContext.Current.PictureManager.GetPictureGroups(pictureId);
                    securityList.DataBind();

                    categoryList.DataSource = PicContext.Current.PictureManager.GetPictureCategories(pictureId);
                    categoryList.DataBind();

                    LoadTasks();
                }

                // now read people
                dlPerson.DataSource = PicContext.Current.PictureManager.GetPicturePeople(pictureId);
                dlPerson.DataBind();

                // Now set page controls
                int intCurRec = Convert.ToInt32(Request.QueryString["r"]);
                int intTotalCount = Convert.ToInt32(cmdPic.Parameters["@TotalCount"].Value);

                String strURL = Request.Path + "?" + Request.ServerVariables["QUERY_STRING"];
                int intRecPos = strURL.IndexOf("r=");
                if (intRecPos > 0)
                    strURL = strURL.Substring(0, intRecPos) + "r={0}"
                        + strURL.Substring(strURL.IndexOf("&", intRecPos));

                // check if in slideshow mode
                if (Request.QueryString["ss"] != null)
                {

                    if (intCurRec == intTotalCount)
                    {
                        Control c = LoadControl("Controls//AutoTimer.ascx");
                        AutoTimer at = (AutoTimer)c;
                        at.NavigateUrl = lnkReturn.NavigateUrl;
                        at.Seconds = 6;
                        at.Visible = true;
                        //						m_HttpRefreshURL = "10;URL=" + lnkReturn.NavigateUrl;
                        panelNext.Controls.Add(at);

                        lnkNext.Visible = true;
                        lnkNext.NavigateUrl = lnkReturn.NavigateUrl;
                    }
                    else
                    {
                        Control c = LoadControl("Controls//AutoTimer.ascx");
                        AutoTimer at = (AutoTimer)c;
                        at.NavigateUrl = strURL.Replace("{0}", Convert.ToString(intCurRec + 1)) + "#title";
                        at.Seconds = 6;
                        at.Visible = true;
                        panelNext.Controls.Add(at);

                        lnkNext.Visible = true;
                        lnkNext.NavigateUrl = lnkReturn.NavigateUrl;
                    }

                    nextBarNote.Visible = true;

                }
                else
                {

                    // make sure not on first picture
                    if (intCurRec > 1)
                    {
                        lnkPrevious.Visible = true;
                        lnkPrevious.NavigateUrl = strURL.Replace("{0}", Convert.ToString(intCurRec - 1));
                    }

                    // make sure not on last picture
                    if (intCurRec < intTotalCount && !sourceType.Equals("random"))
                    {
                        lnkNext.Visible = true;
                        lnkNext.NavigateUrl = strURL.Replace("{0}", Convert.ToString(intCurRec + 1));
                    }
                    else if (sourceType.Equals("random"))
                    {
                        //http://localhost/pics/picview.aspx?p=1615&type=random&RefUrl=default.aspx
                        lnkNext.Visible = true;
                        int randomPictureId = PicContext.Current.PictureManager.GetRandomPicture().Id;
                        lnkNext.NavigateUrl = picview.BuildRandomPageUrl(randomPictureId,
                            Request.QueryString["refUrl"]);
                    }
                }

                if (sourceType.Equals("random"))
                {
                    pictureLocation.Visible = false;
                }
                else
                {
                    lblPicture.Text = intCurRec.ToString();
                    lblPictures.Text = intTotalCount.ToString();
                }


                // if in random mode, hide page controls
                //				if (sourceType.Equals("random"))
                //					pnlPageControls.Visible = false;

            }

        }

        public static PictureSortField GetSortFieldById(int id)
        {
            // also in categories.aspx.cs

            PictureSortField sortField = PictureSortField.DatePictureTaken;
            switch (id)
            {
                case 0:
                    sortField = PictureSortField.DatePictureTaken;
                    break;
                case 1:
                    sortField = PictureSortField.DatePictureAdded;
                    break;
                case 2:
                    sortField = PictureSortField.DatePictureUpdated;
                    break;
                default:
                    throw new ApplicationException("The sort order querystring variable value for 'sf' was not recognized");
            }

            return sortField;
        }


        private void SetSortFields(SqlCommand cmd)
        {
            string sqlSortField = "PictureDate";

            if (Request.QueryString["sf"] != null)
            {
                int sortFieldId = Convert.ToInt32(Request.QueryString["sf"]);
                PictureSortField sortField = GetSortFieldById(sortFieldId);
                sqlSortField = PictureManager.GetSqlSortFieldName(sortField);
            }

            if (Request.QueryString["so"] != null)
            {
                if (Request.QueryString["so"] == "1")
                {
                    sqlSortField += " DESC";
                }
            }

            cmd.Parameters.AddWithValue("@SortFieldName", sqlSortField);
        }

        private void LoadTasks()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<B>Tasks</B><br />");
            sb.Append("<a href=\"javascript:EditPic()\" class=\"infoPanelText\">Edit picture details</a><<br /> />");
            sb.Append("<a href=\"javascript:AddToCat()\" class=\"infoPanelText\">Add to category...</a><<br /> />");
            sb.Append("<a href=\"javascript:SetAsCatPic()\" class=\"infoPanelText\">Set as category index pic</a><<br /> />");

            taskList.Controls.AddAt(1, new HtmlLiteral(sb.ToString()));

            // Server side task javascript
            sb = new StringBuilder();
            int personId = PicContext.Current.CurrentUser.Id;
            sb.Append("<script language=\"javascript\">");
            sb.Append("function SetAsCatPic() { ");
            sb.Append(" if (document.all['pictureTasks'] != null) {");
            sb.Append("   var pId  = " + personId.ToString() + ";");
            sb.Append("   var t    = document.all['pictureTasks'];");
            sb.Append("   t.SetAsCategoryPic(pId);");
            sb.Append(" }");
            sb.Append("}");
            sb.Append("function EditPic() { ");
            sb.Append(" if (document.all['pictureTasks'] != null) {");
            sb.Append("   var pId  = " + personId.ToString() + ";");
            sb.Append("   var t    = document.all['pictureTasks'];");
            sb.Append("   t.EditPicture(pId);");
            sb.Append("   document.location.href = document.location.href;");
            sb.Append(" }");
            sb.Append("}");
            sb.Append("function AddToCat() { ");
            sb.Append(" if (document.all['pictureTasks'] != null) {");
            sb.Append("   var pId  = " + personId.ToString() + ";");
            sb.Append("   var t    = document.all['pictureTasks'];");
            sb.Append("   t.AddToCategory(pId);");
            sb.Append("   document.location.href = document.location.href;");
            sb.Append(" }");
            sb.Append("}");
            sb.Append("// </script>");

            ClientScript.RegisterClientScriptBlock(this.GetType(), "setCatPicScript", sb.ToString());

        }

        private void Page_Init(object sender, EventArgs e)
        {
            clearBasket = new LinkButton();
            clearBasket.Text = "Clear basket";
            clearBasket.Click += new EventHandler(clearBasket_Click);
            clearBasket.Visible = false;
            taskList.Controls.Add(clearBasket);
        }

        public String HttpRefreshURL
        {
            get
            {
                return m_HttpRefreshURL;
            }
        }

        private void SetCategory(int categoryId)
        {
            CategoryManager catMan = PicContext.Current.CategoryManager;
            Category cat = catMan.GetCategory(categoryId);
            lblCategory.Text = cat.Name;

        }
        private void clearBasket_Click(object sender, EventArgs e)
        {

        }

        private string GetAverageText(decimal average)
        {
            return string.Format("Average: {0:0.0}", average);
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            Trace.Write("RaiseCallbackEvent started");

            string returnValue = string.Empty;

            int rating = Convert.ToInt32(eventArgument);

            decimal average = PicContext.Current.PictureManager.RatePicture(pictureId, rating);
            returnValue = this.GetAverageText(average);

            Trace.Write("RaiseCallbackEvent ended: " + returnValue);

            callbackReturnValue = returnValue;
        }

        public string GetCallbackResult()
        {
            return callbackReturnValue;
        }

    }
}