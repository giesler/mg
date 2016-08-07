﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sonos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SonosService.SonosClient client = new SonosService.SonosClient();
        this.players.DataSource = client.GetPlayers();
        this.players.DataBind();
    }

    protected string GetUrl(string url, string suffix)
    {
        return string.Format("http://{0}:1400/{1}", url, suffix);
    }

    protected string FormatBool(bool val)
    {
        return val ? "x" : " ";
    }

    protected string GetCaps(SonosService.ZonePlayerStatus player)
    {
        return (player.Coordinator ? " c" : " ") + (player.WifiEnabled ? " w" : "");
    }

    protected string GetRebootUrl(string ip)
    {
        return GetUrl(ip, "reboot");
    }

    protected string GetTopoUrl(string ip)
    {
        return GetUrl(ip, "status/topology");
    }

    protected string GetSupportUrl(string ip)
    {
        return GetUrl(ip, "support/review");
    }

    protected void players_DataBound(object sender, EventArgs e)
    {
        //for (int i = this.players.Rows.Count - 1; i > 0; i--)
        //{
        //    GridViewRow row = this.players.Rows[i];
        //    GridViewRow previousRow = this.players.Rows[i - 1];
        //    for (int j = 0; j < row.Cells.Count; j++)
        //    {
        //        if (row.Cells[j].Text == previousRow.Cells[j].Text)
        //        {
        //            if (previousRow.Cells[j].RowSpan == 0)
        //            {
        //                if (row.Cells[j].RowSpan == 0)
        //                {
        //                    previousRow.Cells[j].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
        //                }
        //                row.Cells[j].Visible = false;
        //            }
        //        }
        //    }
        //}
    }
}