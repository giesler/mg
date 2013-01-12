using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

public partial class rdp : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("./");
        }

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["WakeOnLanConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("select DisplayName, RdpUserName, RdpUri from dbo.Computers where ComputerID = @id", cn);
        cmd.Parameters.AddWithValue("@id", Int32.Parse(Request.QueryString["id"]));

        SqlDataReader dr = null;
        string name = null;
        string rdpUri = null;
        string rdpUsername = null;

        try
        {
            cn.Open();
            dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);

            dr.Read();
            name = dr["DisplayName"].ToString();
            rdpUri = dr["RdpUri"].ToString();
            rdpUsername = dr["RdpUsername"].ToString();
        }
        finally
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }
            if (cn.State == System.Data.ConnectionState.Open)
            {
                cn.Close();
            }
        }
        
        string rdpContents = File.ReadAllText(Server.MapPath("DefaultRdp.txt")).Replace("COMPUTERNAME", rdpUri);

        base.Response.Clear();
        base.Response.AddHeader("Content-Type", "text");
        base.Response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".rdp");
        base.Response.Write(rdpContents);
        base.Response.End();

        Response.End();
    }
}