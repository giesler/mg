using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class AddNewComputer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!UserManagement.IsAdministrator())
            Response.Redirect("Default.aspx");

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["WakeOnLanConnectionString"].ConnectionString))
        {
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("CreateNewComputer", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@DisplayName", this.txtDisplayName.Text);
            sqlCommand.Parameters.AddWithValue("@IPAddress", this.txtHostnameOrAddress.Text);           

            sqlCommand.ExecuteNonQuery();

            Response.Redirect("Default.aspx");
        }
    }
}
