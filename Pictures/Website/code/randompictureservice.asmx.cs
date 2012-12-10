using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using msn2.net.Pictures;

namespace pics
{
	/// <summary>
	/// Summary description for RandomPictureService.
	/// </summary>
	public class RandomPictureService : System.Web.Services.WebService
	{
		public RandomPictureService()
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

		// WEB SERVICE EXAMPLE
		// The HelloWorld() example service returns the string Hello World
		// To build, uncomment the following lines then save and build the project
		// To test this web service, press F5

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}

		private byte[] RandomImage(int personId)
		{
			SqlConnection cn  = new SqlConnection(PicContext.Current.Config.ConnectionString);
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_RandomPicture", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@PersonID", personId);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", 125);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", 125);

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			// figure out path to image
			String strAppPath = HttpContext.Current.Request.ApplicationPath;
			if (!strAppPath.Equals("/"))  
				strAppPath = strAppPath + "/";

			string filename = dsPics.Tables["Pictures"].DefaultView[0].Row["FileName"].ToString();

			filename = strAppPath + "piccache/" + filename.Replace(@"\", @"/");

			Bitmap img = new Bitmap(HttpContext.Current.Request.MapPath(filename));

			MemoryStream memStream = new MemoryStream();
			img.Save(memStream, ImageFormat.Jpeg);

			return memStream.GetBuffer();

			// see http://www.xmlwebservices.cc/ - image retereival web service
		}
	
	}

}
