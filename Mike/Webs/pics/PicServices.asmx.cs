using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

namespace pics
{
	/// <summary>
	/// Summary description for PicServices.
	/// </summary>
	public class PicServices : System.Web.Services.WebService
	{
		public PicServices()
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

		public DataSetCategory GetCategory(int categoryId)
		{
            
			SqlConnection cn	= new SqlConnection(pics.Config.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter("dbo.p_Category_Get", cn);
			da.SelectCommand.CommandType	= CommandType.StoredProcedure;
			da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.Int);
			da.SelectCommand.Parameters["@categoryId"].Value = categoryId;

			DataSetCategory ds = new DataSetCategory();
            da.Fill(ds, "Category");
		    
			return ds;

		}
	}
}
