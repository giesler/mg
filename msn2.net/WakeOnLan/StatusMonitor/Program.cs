// --------------------------------------------------------------------------
// Copyright (c) MEMOS Software s.r.o. All rights reserved.
//
// Wake On LAN Status monitor
// 
// File     : Program.cs
// Author   : Lukas Neumann <lukas.neumann@memos.cz>
// Created  : 080614
//
// -------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;

namespace StatusMonitor
{
    class Program
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        /// <summary>
        /// Ping timeout (in ms)
        /// </summary>
        private const int PING_TIMEOUT = 1000;

        static void Main(string[] args)
        {
            try
            {
                //Open the database connection
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter("GetManagedComputers", Settings.Default.WakeOnLanConnectionString))
                {
                    DataSet computersDataSet = new DataSet();
                    dataAdapter.Fill(computersDataSet);

                    //Try to ping each computer
                    foreach (DataRow computer in computersDataSet.Tables[0].Rows)
                    {
                        ProcessComputer((Int32)computer["ComputerID"], computer["IPAddress"] as string);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ProcessComputer(int computerId, string hostNameOrAddress)
        {
            //Ping the computer
            bool isActive = IsComputerAccessible(hostNameOrAddress);

            //Try to obtain the MAC address
            string macAddress = null;
            if (isActive)            
                macAddress = GetMACAddress(hostNameOrAddress);
                
            
            using (SqlConnection sqlConnection = new SqlConnection(Settings.Default.WakeOnLanConnectionString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SaveComputerState", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ComputerId", computerId);
                sqlCommand.Parameters.AddWithValue("@StateId", isActive ? 2 : 1);
                if (!String.IsNullOrEmpty(macAddress))
                    sqlCommand.Parameters.AddWithValue("@MACAddress", macAddress);

                sqlCommand.ExecuteNonQuery();

            }
            

        }

        private static string GetMACAddress(string hostNameOrAddress)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(hostNameOrAddress);
            if (hostEntry.AddressList.Length == 0)
                return null;

            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            IPAddress v4Address = IPAddress.Broadcast;
            foreach (IPAddress address in hostEntry.AddressList)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    v4Address = address;
                    break;
                }
            }

            var ipBytes = v4Address.GetAddressBytes();
            var ip = (uint)ipBytes[3] << 24;
            ip += (uint)ipBytes[2] << 16;
            ip += (uint)ipBytes[1] << 8;
            ip += (uint)ipBytes[0];

            if (SendARP((int)ip, 0, macAddr, ref macAddrLen) != 0)
                return null;

            StringBuilder macAddressString = new StringBuilder();
            for (int i = 0; i < macAddr.Length; i++)
            {
                if (macAddressString.Length > 0)
                    macAddressString.Append(":");

                macAddressString.AppendFormat("{0:x2}", macAddr[i]);
            }
            
            return macAddressString.ToString();            
        }

        private static bool IsComputerAccessible(string hostNameOrAddress)
        {
            Ping pingSender = new Ping();
            try
            {
                PingReply reply = pingSender.Send(hostNameOrAddress, PING_TIMEOUT);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
