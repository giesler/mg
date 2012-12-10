<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HomeCalendarView._Default" %>

<%@ Register Src="CalendarItemDisplay.ascx" TagName="CalendarItemDisplay" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>home.msn2.net Calendar</title>
    <style>
        .precipText
        {
            font-size: smaller;
            color: Gray;
        	font-weight: bold;
        }
        .hiText
        {
            color: Blue;
            font-size: 10pt;
        	font-weight: bold;
        }
        .loText
        {
            color: Red;
            font-size: 10pt;
        	font-weight: bold;
        }
        .altColor
        {
            background-color: #E8F7F7;
        }
        .defaultText
        {
            font-size: 8pt;
            font-family: Tahoma;
            margin: 0px;
        }
        .noEventsText
        {
        	color: Gray;
        }
        .dateLabel
        {
        	font-size: 10pt;
        	font-weight: bold;
        }
    </style>
</head>
<body class="defaultText">
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellpadding="3" cellspacing="0" border="0" height="100%">
            <tr>
                <td width="10%" class="dateLabel">
                    today
                </td>
                <td width="10%" align="center">
                    <asp:Label runat="server" CssClass="hiText" ID="todayHighTemp">45</asp:Label>
                    /
                    <asp:Label runat="server" CssClass="loText" ID="todayLowTemp">32</asp:Label>
                </td>
                <td width="10%" class="altColor">
                    <asp:Label runat="server" ID="day1Label" CssClass="dateLabel">Day 1</asp:Label>
                </td>
                <td width="10%" class="altColor" align="center">
                    <asp:Label runat="server" CssClass="hiText" ID="day1High">45</asp:Label>
                    /
                    <asp:Label runat="server" CssClass="loText" ID="day1Low">32</asp:Label>
                </td>
                <td width="10%">
                    <asp:Label runat="server" ID="day2Label" CssClass="dateLabel">Day 2</asp:Label>
                </td>
                <td width="10%" align="center">
                    <asp:Label runat="server" CssClass="hiText" ID="day2High">44</asp:Label>
                    /
                    <asp:Label runat="server" CssClass="loText" ID="day2Low">33</asp:Label>
                </td>
                <td width="10%" class="altColor">
                    <asp:Label runat="server" ID="day3Label" CssClass="dateLabel">Day 3</asp:Label>
                </td>
                <td width="10%" class="altColor" align="center">
                    <asp:Label runat="server" CssClass="hiText" ID="day3High">44</asp:Label>
                    /
                    <asp:Label runat="server" CssClass="loText" ID="day3Low">31</asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Image runat="server" Height="35" ID="todayWeatherImage1" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipToday1" />
                </td>
                <td align="center">
                    <asp:Image runat="server" Height="35" ID="todayWeatherImage2" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipToday2">80%</asp:Label>
                </td>
                <td align="center" class="altColor">
                    <asp:Image runat="server" Height="35" ID="day1Image1" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipDay1am">30%</asp:Label>
                </td>
                <td align="center" class="altColor">
                    <asp:Image runat="server" Height="35" ID="day1Image2" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipDay1pm">10%</asp:Label>
                </td>
                <td align="center">
                    <asp:Image runat="server" Height="35" ID="day2Image1" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipDay2am" />
                </td>
                <td align="center">
                    <asp:Image runat="server" Height="35" ID="day2Image2" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipDay2pm">80%</asp:Label>
                </td>
                <td align="center" class="altColor">
                    <asp:Image runat="server" Height="35" ID="day3Image1" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipDay3am">30%</asp:Label>
                </td>
                <td align="center" class="altColor">
                    <asp:Image runat="server" Height="35" ID="day3Image2" />
                    <asp:Label runat="server" CssClass="precipText" ID="precipDay3pm">10%</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <small>
                        <asp:Label runat="server" ID="todayDateLabel" /></small><br />
                    <uc1:CalendarItemDisplay ID="todaysEvents" runat="server" />
                </td>
                <td colspan="2" class="altColor">
                    <uc1:CalendarItemDisplay ID="day1Events" runat="server" />
                </td>
                <td colspan="2">
                    <uc1:CalendarItemDisplay ID="day2Events" runat="server" />
                </td>
                <td colspan="2" class="altColor">
                    <uc1:CalendarItemDisplay ID="day3Events" runat="server" />
                </td>
            </tr>
            <tr style="background-color: #B6BCC4">
                <td colspan="8">
                    <table>
                        <tr>
                            <td>
                                Next&nbsp;2&nbsp;weeks:
                            </td>
                            <td>
                                <asp:Panel runat="server" ID="upcomingEvents" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
