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
using msn2.net.Pictures;

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
		protected pics.Controls.PictureTasks picTaskList;
		protected pics.Controls.ContentPanel picTasks;
		protected pics.Controls.Sidebar Sidebar1;
		protected System.Web.UI.WebControls.Panel youAreHerePanel;
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
					LoadSearch(new Guid(Request.QueryString["id"]));

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

		private void LoadSearch(Guid id) 
		{
			// get the byte array from the guid passed

			// create a connection and set up a command
			SqlConnection cn = new SqlConnection(PicContext.Current.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("sp_Search_Load", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@SearchID", id);
			cmd.Parameters.Add("@PersonID", PicContext.Current.CurrentUser.Id);

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

		private Guid SaveSearch() 
		{

			// figure out a 'friendly' search description
			string searchText = "";
			
			// create a connection and set up a command
			SqlConnection cn = new SqlConnection(PicContext.Current.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("sp_Search_NewSearch", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@SearchID", SqlDbType.UniqueIdentifier);
			cmd.Parameters["@SearchID"].Direction = ParameterDirection.Output;
			cmd.Parameters.Add("@PersonID", SqlDbType.Int);
			cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@PictureDateStart", SqlDbType.SmallDateTime);
			cmd.Parameters.Add("@PictureDateEnd", SqlDbType.SmallDateTime);
			cmd.Parameters.Add("@PersonSearchOption", SqlDbType.SmallInt);

			// set the params on the sp
			cmd.Parameters["@PersonID"].Value = PicContext.Current.CurrentUser.Id;
			if (description.Text.Length > 0) 
			{
				cmd.Parameters["@Description"].Value = description.Text;
				searchText = searchText + "Description: " + description.Text + "<br>";
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
				searchText = searchText + "Date between " + pictureDateStart.Text + " and " + pictureDateEnd.Text + "<br>";
			} 
			else if (pictureDateStart.Text.Length > 0 && pictureDateEnd.Text.Length == 0) 
			{
				searchText = searchText + "Date after " + pictureDateStart.Text + "<br>";
			} 
			else if (pictureDateStart.Text.Length == 0 && pictureDateEnd.Text.Length > 0) 
			{
				searchText = searchText + "Date before " + pictureDateEnd.Text + "<br>";
			}
			
			// run the SP
			cn.Open();
			cmd.ExecuteNonQuery();
			
			// get the guid returned
			Guid searchId = (Guid) cmd.Parameters["@SearchID"].Value;

			// now we want to do the selected people
			SqlCommand cmdPerson = new SqlCommand("sp_Search_AddPerson", cn);
			cmdPerson.CommandType = CommandType.StoredProcedure;
			cmdPerson.Parameters.Add("@SearchID", searchId);
			cmdPerson.Parameters.Add("@PersonID", SqlDbType.Int);
            
			// loop through selected people
			foreach (ListItem li in peopleSelector.SelectedPeople) 
			{
				cmdPerson.Parameters["@PersonID"].Value = li.Value;
				cmdPerson.ExecuteNonQuery();

				// add to friendly message
				searchText = searchText + li.Text + "<br>";
			}
			
			// save the search text
			SqlCommand cmdUpdate = new SqlCommand("dbo.sp_Search_UpdateDescription", cn);
			cmdUpdate.CommandType = CommandType.StoredProcedure;
			cmdUpdate.Parameters.Add("@SearchID", searchId);
			cmdUpdate.Parameters.Add("@SearchDescription", searchText);
			cmdUpdate.ExecuteNonQuery();

			cn.Close();
            
			return searchId;
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

			Guid searchId = SaveSearch();
			Response.Redirect("SearchRun.aspx?id=" + searchId.ToString());
		}

		private void reset_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("SearchCriteria.aspx");
		}

	}
}
