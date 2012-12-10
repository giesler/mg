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
using System.Drawing.Imaging;

namespace pics
{
	/// <summary>
	/// Summary description for GetItem.
	/// </summary>
	public class GetItem : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			int pictureId		= Convert.ToInt32(Request.QueryString["p"]);
			int maxWidth		= Convert.ToInt32(Request.QueryString["mw"]);
			int maxHeight		= Convert.ToInt32(Request.QueryString["mh"]);
			
			// load the person's info
			PersonInfo pi = (PersonInfo) HttpContext.Current.Session["PersonInfo"];

			SqlConnection cn = new SqlConnection(Config.ConnectionString);

			// Set up SP to retreive picture
			SqlCommand cmdPic    = new SqlCommand();
			cmdPic.CommandText = "p_GetPicture";
			cmdPic.CommandType   = CommandType.StoredProcedure;
			cmdPic.Connection    = cn;
			SqlDataAdapter daPic = new SqlDataAdapter(cmdPic);

			// set up params on the SP
			cmdPic.Parameters.Add("@PictureID", pictureId);
			cmdPic.Parameters.Add("@StartRecord", 0);
			cmdPic.Parameters.Add("@ReturnCount", 1);
			cmdPic.Parameters.Add("@MaxHeight", maxHeight);
			cmdPic.Parameters.Add("@MaxWidth", maxWidth);
			cmdPic.Parameters.Add("@PersonID", pi.PersonID);
			cmdPic.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			cmdPic.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSet ds		= new DataSet();
			daPic.Fill(ds, "Picture");
			DataRow dr		= ds.Tables[0].Rows[0];
			cn.Close();
            
			string strCache		= Global.PictureCacheLocation;

			string filename = dr["FileName"].ToString();

			filename = strCache + filename.Replace(@"\", @"/");

			Response.ContentType	 = "image/jpeg";
			using (Bitmap img = new Bitmap(filename))
			{
				img.Save(Response.OutputStream, ImageFormat.Jpeg);
			}

			Response.End();

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
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

