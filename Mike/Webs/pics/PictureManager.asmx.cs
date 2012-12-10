using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace pics
{
	/// <summary>
	/// Summary description for PictureManager.
	/// </summary>
	public class PictureManager : System.Web.Services.WebService
	{
		public PictureManager()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		public void AddToCategory(int pictureId, int categoryId)
		{
			SqlConnection cn	= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("p_Picture_AddToCategory", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);

			cmd.Parameters["@pictureId"].Value	= pictureId;
			cmd.Parameters["@categoryId"].Value	= categoryId;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

		}

		public void RemoveFromCategory(int pictureId, int categoryId)
		{
			SqlConnection cn	= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("p_Picture_RemoveFromCategory", cn); 
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);

			cmd.Parameters["@pictureId"].Value	= pictureId;
			cmd.Parameters["@categoryId"].Value	= categoryId;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

		}

		[WebMethod(true)]
		public byte[] GetPictureImage(int pictureId, int maxWidth, int maxHeight)
		{
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
            
			String strAppPath = HttpContext.Current.Request.ApplicationPath;
			if (!strAppPath.Equals("/"))  
				strAppPath = strAppPath + "/";

			string filename = dr["FileName"].ToString();

			filename = strAppPath + "piccache/" + filename.Replace(@"\", @"/");

			Bitmap img = new Bitmap(HttpContext.Current.Request.MapPath(filename));

			MemoryStream memStream = new MemoryStream();
			img.Save(memStream, ImageFormat.Jpeg);

			return memStream.GetBuffer();

			
		}
	}

	public class PictureCollection: CollectionBase
	{
		public void Add(PictureData pictureData)
		{
			InnerList.Add(pictureData);
		}
		public PictureData this[int index]
		{
			get
			{
				return (PictureData) InnerList[index];
			}
		}
	}

	public class PictureData
	{
		public int PictureId;
	}
}
