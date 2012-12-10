using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace msn2.net.Pictures
{
    public class GroupManager
    {
        private string connectionString;

        internal GroupManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public PersonGroup GetGroup(int groupId)
        {
            SqlConnection cn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand("up_GroupManager_GetGroup", cn);
            SqlDataReader dr = null;
            PersonGroup group = null;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@groupId", SqlDbType.Int);
            cmd.Parameters["@groupId"].Value = groupId;

            try
            {
                cn.Open();

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    group = new PersonGroup(
                        dr.GetInt32(0),
                        dr.GetString(1));
                }

                dr.NextResult();

                while (dr.Read())
                {
                    group.Members.Add(new PersonInfo(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetString(2)));
                }

            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            return group;
        }
    }
}
