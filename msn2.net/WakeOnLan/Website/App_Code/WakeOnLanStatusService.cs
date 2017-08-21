using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Activation;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class WakeOnLanStatusService : IWakeOnLanStatusService
{
    static string connectionString = ConfigurationManager.ConnectionStrings["WakeOnLanConnectionString"].ConnectionString;

    public IEnumerable<Device> GetDevices()
    {
        List<Device> devices = new List<Device>();

        //Open the database connection
        using (SqlDataAdapter dataAdapter = new SqlDataAdapter("GetManagedComputers", connectionString))
        {
            DataSet computersDataSet = new DataSet();
            dataAdapter.Fill(computersDataSet);

            //Try to ping each computer
            foreach (DataRow computer in computersDataSet.Tables[0].Rows)
            {
                devices.Add(new Device { Id = (int) computer["ComputerID"], HostNameOrIPAddress = computer["IPAddress"].ToString() });
            }
        }

        return devices;
    }

    public void UpdateDeviceStatus(int id, bool isActive, string macAddress)
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("SaveComputerState", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ComputerId", id);
            sqlCommand.Parameters.AddWithValue("@StateId", isActive ? 2 : 1);
            if (!String.IsNullOrEmpty(macAddress))
                sqlCommand.Parameters.AddWithValue("@MACAddress", macAddress);

            sqlCommand.ExecuteNonQuery();
        }
    }
}
