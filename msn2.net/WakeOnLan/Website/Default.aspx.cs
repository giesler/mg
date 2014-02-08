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
using System.Net;

public partial class _Default : System.Web.UI.Page 
{
    bool switchingState = false;

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
            bool supportSuspend = (bool)drv.Row["SupportsSuspend"];

            Image imgComputer = (Image)e.Item.FindControl("imgComputer");
            HyperLink rdpLink = (HyperLink)e.Item.FindControl("rdpLink");

            switch (statusId)
            {                
                case 2:
                    imgComputer.ImageUrl = "images/ComputerRunning.gif";
                    imgComputer.AlternateText = "Computer Running";
                    rdpLink.NavigateUrl = "rdp.aspx?id=" + drv.Row["ComputerID"].ToString();

                    if (supportSuspend)
                    {
                        LinkButton lnkSuspend = (LinkButton)e.Item.FindControl("lnkSuspend");
                        lnkSuspend.Visible = true;
                        lnkSuspend.CommandArgument = drv.Row["ComputerID"].ToString();
                    }

                    break;
                case 3:
                    imgComputer.ImageUrl = "images/ComputerPoweringUp.gif";
                    switchingState = true;

                    Label status = (Label)e.Item.FindControl("status");
                    status.Visible = true;
                    status.Text = "Starting up";

                    break;
                case 4:
                    imgComputer.ImageUrl = "images/ComputerPoweringDown.gif";
                    imgComputer.AlternateText = "Computer sleeping";
                    switchingState = true;

                    Label statusSleep = (Label)e.Item.FindControl("status");
                    statusSleep.Visible = true;
                    statusSleep.Text = "Sleeping";

                    break;
                default:
                    imgComputer.ImageUrl = "images/ComputerOff.gif";
                    imgComputer.AlternateText = "Computer Off";

                    if (supportSuspend)
                    {
                        LinkButton lnkWakeUp = (LinkButton)e.Item.FindControl("lnkWakeUp");
                        lnkWakeUp.Visible = true;
                        lnkWakeUp.CommandArgument = drv.Row["ComputerID"].ToString();
                    }

                    break;
            }

            
        }
    }

    protected void lstComputers_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string powerAccessKey = ConfigurationManager.AppSettings["powerManagerAccessKey"];
        if (string.IsNullOrEmpty(powerAccessKey))
        {
            throw new Exception("No power manager access key is defined in web.config");
        }

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
                    DevicePowerService.DevicePowerClient powerClient = new DevicePowerService.DevicePowerClient();
                    powerClient.Resume(powerAccessKey, macAddress);

                    //Save powering up state to the DB                   
                    SetComputerState(sqlConnection, computerId, 3);

                    Response.Redirect("./");
               }

                sqlConnection.Close();
            }
        }
        else if (e.CommandName == "Suspend")
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["WakeOnLanConnectionString"].ConnectionString))
            {
                sqlConnection.Open();

                int computerId = Convert.ToInt32(e.CommandArgument);

                SqlCommand cmd = new SqlCommand("select IPAddress from dbo.Computers where ComputerID = @id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", computerId);

                string ip = cmd.ExecuteScalar().ToString();

                if (!string.IsNullOrEmpty(ip))
                {
                    try
                    {
                        DevicePowerService.DevicePowerClient powerClient = new DevicePowerService.DevicePowerClient();
                        powerClient.Suspend(powerAccessKey, ip);

                        //Save powering up state to the DB                   
                        SetComputerState(sqlConnection, computerId, 4);

                        Response.Redirect("./");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Error connecting - " + ex.Message);
                        Response.End();
                    }
                }
            }

        }
    }

    private static void SetComputerState(SqlConnection sqlConnection, int computerId, int stateId)
    {
        SqlCommand sqlCommand = new SqlCommand("SaveComputerState", sqlConnection);
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@ComputerId", computerId);
        sqlCommand.Parameters.AddWithValue("@StateId", stateId);

        sqlCommand.ExecuteNonQuery();
    }

    protected int GetRefreshInterval()
    {
        return switchingState ? 5000 : (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
    }

    protected void computerType_SelectedIndexChanged(object sender, EventArgs e)
    {
//        Response.Redirect("./?ct=" + this.computerType.SelectedValue);
    }
}
