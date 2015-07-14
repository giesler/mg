﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="false" EnableEventValidation="false" EnableSessionState="False" ViewStateMode="Disabled" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="Stylesheet" href="css/default.css" media="all" />
    <title>ts.msn2.net</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta name="viewport" content="width=device-width" /> 

    <style type="text/css">
        A:link
        {
            text-decoration: none;
            color: navy;
        }

        .headerLink A:link
        {
            text-decoration: none;
            color: white;
        }

        .headerLink A:visited
        {
            text-decoration: none;
            color: white;
        }

        .subheader
        {
            font-size: x-small;
            padding-top: 4px;
            margin-bottom: -2px;
        }

        .topsubheader
        {
            font-size: x-small;
            margin-bottom: -2px;
        }
    </style>

</head>
<body>
    <div style="background: black; color: white; padding: 6px; font-weight: bold" class="headerLink">
        <a href="http://www.msn2.net/">MSN2.NET</a>: <a href="http://home.msn2.net">HOME</a> | <a href="http://cams.msn2.net/">CAMS</a> |  <a href="http://control.msn2.net/">CONTROL</a> | 
	    <a href="http://ts.msn2.net/">TS</a>
        <br />
    </div>
    <div class="mainContent">
        <form id="form1" runat="server">
            <asp:SqlDataSource ID="sqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:WakeOnLanConnectionString %>"
                SelectCommand="GetStatusOfComputers" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <div class="computerList">
                <asp:DataList ID="lstComputers" runat="server" DataKeyField="ComputerId" DataSourceID="sqlDataSource"
                    Width="100%" OnItemDataBound="lstComputers_ItemDataBound" OnItemCommand="lstComputers_ItemCommand">
                    <HeaderTemplate>
                        <div>
                            <div style="width: 162px; height: 10px; float: left">
                                <asp:DropDownList ID="computerType" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="computerType_SelectedIndexChanged">
                                    <asp:ListItem Text="Clients" Value="Clients" Selected="True" />
                                    <asp:ListItem Text="Servers" Value="Servers" />
                                </asp:DropDownList>
                            </div>
                            <div class="computerUptime">
                                <i>Uptime</i>
                            </div>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="computer">
                            <div class="computerImage">
                                <asp:HyperLink ID="rdpLink" runat="server">
                                    <asp:Image ID="imgComputer" runat="server" />
                                </asp:HyperLink>
                            </div>
                            <div class="computerName">
                                <asp:Label ID="DisplayNameLabel" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                                <div class="computerWakeUp">
                                    <asp:LinkButton ID="lnkWakeUp" runat="server" Visible="False" CommandName="WakeUp">Wake Up</asp:LinkButton>
                                    <asp:LinkButton ID="lnkSuspend" runat="server" Visible="False" CommandName="Suspend">Suspend</asp:LinkButton>
                                    <asp:Label ID="status" runat="server" Visible="false" />
                                </div>
                            </div>
                            <div class="computerUptime">
                                <asp:Label ID="UptimeTodayLabel" runat="server" Text='<%# Eval("UptimeToday") %>'></asp:Label>h today<br />
                                <asp:Label ID="UptimeThisWeekLabel" runat="server" Text='<%# Eval("UptimeThisWeek") %>'></asp:Label>h this week<br />
                                <asp:Label ID="UptimeTotalLabel" runat="server" Text='<%# Eval("UptimeTotal") %>'></asp:Label>h total                        
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="bottomLinks">
                <asp:HyperLink ID="lnkAddNewComputer" runat="server" NavigateUrl="~/AddNewComputer.aspx">Add New Computer</asp:HyperLink>
            </div>
        </form>
        <script language="javascript" type="text/javascript">//<!--

            function Refresh() {
                window.location.reload();
            }

            setTimeout("Refresh()", <%= GetRefreshInterval() %>);

            // <%= GetMachineInfo() %>
            // -->
        </script>
    </div>
</body>
</html>
