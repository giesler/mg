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
	/// Summary description for SearchCriteria.
	/// </summary>
	public class SearchCriteria : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox description;
		protected System.Web.UI.WebControls.TextBox pictureDateStart;
		protected System.Web.UI.WebControls.TextBox pictureDateEnd;
		protected System.Web.UI.WebControls.Button search;
		protected System.Web.UI.WebControls.Button reset;
		protected RadioButtonList personSearchOption;
		protected Controls.PeopleSelector peopleSelector;
		protected Label pictureDateStartBad;
		protected System.Web.UI.WebControls.Label noResults;
		protected pics.Controls.Header header;
		protected Label pictureDateEndBad;
	
		public SearchCriteria()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				
				// check if we should load a search
				if (Request.QueryString["id"] != null) 
				{
					LoadSearch(Request.QueryString["id"]);

					// check if no results
					if (Request.QueryString["noresults"] != null) 
						noResults.Visible = true;
				}

			}
		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.search.Click += new System.EventHandler(this.search_Click);
			this.reset.Click += new System.EventHandler(this.reset_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void LoadSearch(string id) 
		{
			// get the current person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			// get the byte array from the guid passed
			XMGuid.Init();
			XMGuid g = new XMGuid(id);

			// create a connection and set up a command
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("sp_Search_Load", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@SearchID", g.Buffer);
			cmd.Parameters.Add("@PersonID", pi.PersonID);

			// run the command
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.Read()) 
			{
				if (dr["Description"].ToString() != null)
					description.Text = dr["Description"].ToString();

				if (dr["PictureDateStart"].ToString().Length > 0)
					pictureDateStart.Text = Convert.ToDateTime(dr["PictureDateStart"].ToString()).ToShortDateString();

				if (dr["PictureDateEnd"].ToString().Length > 0)
					pictureDateEnd.Text = Convert.ToDateTime(dr["PictureDateEnd"].ToString()).ToShortDateString();

				personSearchOption.SelectedIndex = Convert.ToInt16(dr["PersonSearchOption"]);

				// now read in people saved for this search
				if (dr.NextResult()) 
				{
					while (dr.Read()) 
					{
						// add the selected people
						peopleSelector.AddPerson(Convert.ToInt32(dr["PersonID"]), dr["FullName"].ToString());
					}


				}

			}

			// done, close stuff
			dr.Close();
			cn.Close();
		}

		private string SaveSearch() 
		{

			// figure out a 'friendly' search description
			string searchText = "";
			
			// get the current person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			// create a connection and set up a command
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("sp_Search_NewSearch", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@SearchID", SqlDbType.NVarChar, 36);
			cmd.Parameters["@SearchID"].Direction = ParameterDirection.Output;
			cmd.Parameters.Add("@PersonID", SqlDbType.Int);
			cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@PictureDateStart", SqlDbType.SmallDateTime);
			cmd.Parameters.Add("@PictureDateEnd", SqlDbType.SmallDateTime);
			cmd.Parameters.Add("@PersonSearchOption", SqlDbType.Bit);

			// set the params on the sp
			cmd.Parameters["@PersonID"].Value = pi.PersonID;
			if (description.Text.Length > 0) 
			{
				cmd.Parameters["@Description"].Value = description.Text;
				searchText = searchText + "Description: <b>" + description.Text + "</b><br>";
			}
			if (pictureDateStart.Text.Length > 0) 
			{
				cmd.Parameters["@PictureDateStart"].Value = Convert.ToDateTime(pictureDateStart.Text);
			}
			if (pictureDateEnd.Text.Length > 0) 
			{
				cmd.Parameters["@PictureDateEnd"].Value = Convert.ToDateTime(pictureDateEnd.Text);
			}
			cmd.Parameters["@PersonSearchOption"].Value = Convert.ToInt16(personSearchOption.SelectedItem.Value);

			// figure out date to show in friendly msg
			if (pictureDateStart.Text.Length > 0 && pictureDateEnd.Text.Length > 0) 
			{
				searchText = searchText + "Date between <b>" + pictureDateStart.Text + "</b> and <b>" + pictureDateEnd.Text + "</b><br>";
			} 
			else if (pictureDateStart.Text.Length > 0 && pictureDateEnd.Text.Length == 0) 
			{
				searchText = searchText + "Date after <b>" + pictureDateStart.Text + "</b><br>";
			} 
			else if (pictureDateStart.Text.Length == 0 && pictureDateEnd.Text.Length > 0) 
			{
				searchText = searchText + "Date before <b>" + pictureDateEnd.Text + "</b><br>";
			}
			
			// run the SP
			cn.Open();
			cmd.ExecuteNonQuery();
			
			// get the guid returned
			XMGuid.Init();
			string searchid = cmd.Parameters["@SearchID"].Value.ToString();
			XMGuid g = new XMGuid(searchid.Substring(2));

			// now we want to do the selected people
			SqlCommand cmdPerson = new SqlCommand("sp_Search_AddPerson", cn);
			cmdPerson.CommandType = CommandType.StoredProcedure;
			cmdPerson.Parameters.Add("@SearchID", g.Buffer);
			cmdPerson.Parameters.Add("@PersonID", SqlDbType.Int);
            
			// loop through selected people
			foreach (ListItem li in peopleSelector.SelectedPeople) 
			{
				cmdPerson.Parameters["@PersonID"].Value = li.Value;
				cmdPerson.ExecuteNonQuery();

				// add to friendly message
				searchText = searchText + "<b>" + li.Text + "</b><br>";
			}
			
			// save the search text
			SqlCommand cmdUpdate = new SqlCommand("dbo.sp_Search_UpdateDescription", cn);
			cmdUpdate.CommandType = CommandType.StoredProcedure;
			cmdUpdate.Parameters.Add("@SearchID", g.Buffer);
			cmdUpdate.Parameters.Add("@SearchDescription", searchText);
			cmdUpdate.ExecuteNonQuery();

			cn.Close();
            
			return (cmd.Parameters["@SearchID"].Value.ToString().Substring(2));
		}

		private void search_Click(object sender, System.EventArgs e)
		{
			// check for valid dates
			try 
			{
				if (pictureDateStart.Text.Length > 0) 
					System.DateTime.Parse(pictureDateStart.Text);
			}
			catch (FormatException) 
			{
				pictureDateStartBad.Visible = true;
			}
			try 
			{
				if (pictureDateEnd.Text.Length > 0) 
					System.DateTime.Parse(pictureDateEnd.Text);
			}
			catch (FormatException) 
			{
				pictureDateEndBad.Visible = true;
			}

			String id = SaveSearch();
			Response.Redirect("SearchRun.aspx?id=" + id);
		}

		private void reset_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("SearchCriteria.aspx");
		}

	}
}
