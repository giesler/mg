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

namespace pics
{
	/// <summary>
	/// Summary description for SearchRun.
	/// </summary>
	public class SearchRun : System.Web.UI.Page
	{
		protected string redirectHeader;

		public SearchRun()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (Request.QueryString["go"] != null) 
			{
				// load the person's info
				PersonInfo pi = (PersonInfo) Session["PersonInfo"];

				// make sure we have a valid search id
				if (Request.QueryString["id"] == null)
					Response.Redirect("SearchCriteria.aspx");	

				string id = Request.QueryString["id"];
	
				// get the byte array from the guid passed
				XMGuid.Init();
				XMGuid g = new XMGuid(id);

				// set up connection and such to run search
				SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
				SqlCommand cmd	 = new SqlCommand("sp_Search_RunSearch", cn);
				cmd.CommandType	 = CommandType.StoredProcedure;
				cmd.Parameters.Add("@SearchID", g.Buffer);
				cmd.Parameters.Add("@PersonID", pi.PersonID);
				cmd.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
				cmd.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
	
				// Run the sp
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
	
				// check for results and redirect as appropriate
				if (Convert.ToInt32(cmd.Parameters["@TotalCount"].Value) > 0)
					Response.Redirect("SearchResults.aspx?id=" + id);
				else
					Response.Redirect("SearchCriteria.aspx?id=" + id + "&noresults=1");

			} 
			else 
			{

                // output a header to redirect
				redirectHeader = "<META content=\"1; URL=" + Request.Url + "&go=1\" http-equiv=REFRESH>";

			}
		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		public string RedirectHeader 
		{
			get 
			{
				return redirectHeader;
			}
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
