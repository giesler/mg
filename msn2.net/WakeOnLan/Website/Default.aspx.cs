using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page 
{
    bool starting = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.lnkAddNewComputer.Visible = false; // UserManagement.IsAdministrator();      
    }

    protected void lstComputers_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
               e.Item.ItemType == ListItemType.AlternatingItem)
        {
            System.Data.DataRowView drv = (System.Data.DataRowView)(e.Item.DataItem);
            int statusId = 0;

            if (!drv.Row.IsNull("StatusId"))
                statusId = (int)drv.Row["StatusId"];

            Image imgComputer = (Image)e.Item.FindControl("imgComputer");
            HyperLink rdpLink = (HyperLink)e.Item.FindControl("rdpLink");

            switch (statusId)
            {                
                case 2:
                    imgComputer.ImageUrl = "images/ComputerRunning.gif";
                    imgComputer.AlternateText = "Computer Running";
                    rdpLink.NavigateUrl = "rdp.aspx?id=" + drv.Row["ComputerID"].ToString();
                    break;
                case 3:
                    imgComputer.ImageUrl = "images/ComputerPoweringUp.gif";
                    imgComputer.AlternateText = "Computer Powering Up";
                    starting = true;
                    break;
                default:
                    imgComputer.ImageUrl = "images/ComputerOff.gif";
                    imgComputer.AlternateText = "Computer Off";

                    //Show wake up button
                    LinkButton lnkWakeUp = (LinkButton)e.Item.FindControl("lnkWakeUp");
                    lnkWakeUp.Visible = true;

                    lnkWakeUp.CommandArgument = drv.Row["ComputerID"].ToString();
                    
                    break;
            }

            
        }
    }

    protected void lstComputers_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "WakeUp")
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["WakeOnLanConnectionString"].ConnectionString))
            {
                sqlConnection.Open();

                int computerId = Convert.ToInt32(e.CommandArgument);

                SqlCommand sqlCommand = new SqlCommand("GetMACAddress", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ComputerId", computerId);
                string macAddress = sqlCommand.ExecuteScalar() as string;              

                if (!String.IsNullOrEmpty(macAddress))
                {
                    //Wake up given PC
                    PowerManager.WakeUp(macAddress);

                    //Save powering up state to the DB                    

                    sqlCommand = new SqlCommand("SaveComputerState", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ComputerId", computerId);
                    sqlCommand.Parameters.AddWithValue("@StateId", 3);

                    sqlCommand.ExecuteNonQuery();

                    Response.Redirect("./");
               }
            }
        }
    }

    protected int GetRefreshInterval()
    {
        return starting ? 5000 : (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
    }
}
