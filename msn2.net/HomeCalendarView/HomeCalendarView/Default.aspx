<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HomeCalendarView._Default" %>

<%@ Register Src="CalendarItemDisplay.ascx" TagName="CalendarItemDisplay" TagPrefix="uc1" %>
<%@ Register Src="ForecastItem.ascx" TagName="ForecastItem" TagPrefix="ucb" %>
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
        .propText
        {
            font-size: smaller;
        }
        .dimText
        {
            font-size: smaller;
            color: Gray;
        }
        .hiText
        {
            color: Red;
            font-size: 10pt;
            font-weight: bold;
        }
        .loText
        {
            color: Blue;
            font-size: 10pt;
            font-weight: bold;
        }
        .altColor
        {
            background-color: #E8F7F7;
        }
        .dayHeader
        {
            border-bottom-color: Gray;
            border-bottom-width: 1px;
            border-bottom-style: solid;
            padding: 2px 2px 2px 2px;
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
        .nowTemp
        {
            font-size: 16pt;
            font-weight: bold;
            color: Navy;
        }
        .headerRow
        {
            background-color: #ececec;
            padding: 2px;
            height: 69px;
            border-bottom-color: Gray;
            border-bottom-width: 1px;
            border-bottom-style: solid;
        }
        .today
        {
            font-size: 14pt;
            font-weight: bold;
        }
        .cityName
        {
            font-size: 8pt;
        }
        .eventList
        {
            padding: 2px;
        }
        .vertLabel
        {
            writing-mode: tb-rl;
            filter: flipv fliph;
            font-size: smaller;
            color: Gray;
        }
        .fPanel
        {
            width: 100%;
            height: 100%;
            overflow: hidden;
        }
        .fItem
        {
            width: 45%;
            float: left;
            height: 100%;
        }
        .fItems
        {
            width: 100%;
            height: 58px;
        }
        .dayPanel
        {
            width: 25%;
            float: left;
            height: 100%;
        }
        .dayPanelAlt
        {
            width: 25%;
            float: left;
            height: 100%;
            background-color: #E8F7F7;
        }
    </style>
</head>
<body class="defaultText" style="border: solid 1px silver; padding: 0px 0px 0px 0px;
    margin: 0px 0px 0px 0px">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server" ID="updatePanel" RenderMode="Inline">
        <ContentTemplate>
            <asp:Timer ID="refreshTimer" runat="server" Interval="30000" Enabled="true" />
            <asp:Timer ID="dataLoadTimer" runat="server" Interval="5000" Enabled="false" />
            <div class="headerRow">
                <table width="100%" border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td valign="top" width="80">
                            <table border="1" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="webcamUrl" runat="server" BorderWidth="0" NavigateUrl="~/Webcam.aspx" Target=_blank>
                                            <asp:Image runat="server" ID="webcamPicture" AlternateText="Webcam" Height="60" Width="80" ImageUrl="~/Webcam.aspx" BorderWidth="0" />
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td rowspan="2" width="8">
                            &nbsp;
                        </td>
                        <td width="150" valign="top">
                            <asp:Label runat="server" ID="todayLabel" CssClass="today">today</asp:Label><br />
                            <small>
                                <asp:Label runat="server" ID="todayDateLabel" /></small>
                                <br />
                                <br />
                                                        <asp:LinkButton runat="server" ID="selectKirkland" CssClass="cityName" Text="Kirkland"
                            OnClick="onCityClick" />
                        &nbsp;|&nbsp;<asp:LinkButton runat="server" ID="selectPackwood" CssClass="cityName"
                            OnClick="onCityClick" Text="Packwood" />

                        </td>
                        <td rowspan="2" width="8">
                            &nbsp;
                        </td>
                        <td rowspan="2" valign="top">
                            <br />
                            <uc1:CalendarItemDisplay ID="todaysEvents" runat="server" Visible="false" />
                        </td>
                        <td rowspan="2" width="250" valign="top">
                            <asp:Panel Style="text-align: center; vertical-align: middle; height: 100%; width: 100%;"
                                class="dimText" runat="server" ID="currentConditionsLoading" runat="server">
                                <asp:Label runat="server" ID="currentLoadingMessage">loading current conditions...</asp:Label>
                            </asp:Panel>
                            <table cellpadding="2" cellspacing="0" runat="server" id="currentConditionsTable"
                                visible="false">
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:Label runat="server" ID="currentTemp" CssClass="nowTemp">32</asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="tempUpdateTime" CssClass="propText" ForeColor="Gray" />
                                    </td>
                                    <td>
                                        <asp:Image runat="server" ID="currentCondition" />
                                    </td>
                                    <td>
                                        <table cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td class="propText">
                                                    <asp:Label runat="server" ID="currentConditionText">Loading...</asp:Label><br />
                                                    Wind:
                                                    <div style="display: inline">
                                                        <asp:Label runat="server" ID="windLabel" /></div>
                                                    <br />
                                                    <asp:Label runat="server" ID="windChill" /><br />
                                                    Visibility:
                                                    <asp:Label runat="server" ID="visibility" />
                                                    m
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" rowspan="2" width="120" id="warningCell" runat="server" visible="false" valign="top">
                            <table cellpadding="2" cellspacing="0">
                                <tr>
                                    <td style="font-size: 8pt; color: Red; font-weight: bold">
                                        <asp:Label runat="server" ID="severeWeatherAlertLabel">Severe&nbsp;Weather&nbsp;Alert</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="warnings" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" rowspan="2" width="170px" valign="top">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="todayForecastLabel" CssClass="vertLabel">forecast</asp:Label>
                                    </td>
                                    <td>
                                        <div runat="server" id="todayForeastDiv" visible="false">
                                            <div style="width: 160px; border: solid 1px gray; padding: 2px;" runat="server" id="todayForecastInnerDiv">
                                                <div style="float: left; width: 50%" runat="server" id="todayHighDiv">
                                                    <ucb:ForecastItem ID="todayHigh" runat="server" TemperatureExtreme="High" Visible="false" />
                                                </div>
                                                <div style="float: left; width: 50%" runat="server" id="todayLowDiv">
                                                    <ucb:ForecastItem ID="todayLow" runat="server" TemperatureExtreme="Low" Visible="false" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="dimText" runat="server" id="todayForecastLoading" style="width: 160px;
                                            height: 60px; border: solid 1px gray; padding: 2px; vertical-align: middle; text-align: center">
                                            loading...
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Label Font-Size="X-Small" ForeColor="Gray" runat="server" ID="lastUpdateTime"
                                Visible="false"></asp:Label>&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel runat="server" ID="warningPanel" Width="100%" Visible="false">
                <table cellpadding="3" cellspacing="0" border="0" width="100%" height="100%">
                    <tr>
                        <td valign="top" style="width: 200px">
                            <asp:Label runat="server" ID="warningTitle" Text="Severe Weather Alert" Font-Bold="true"
                                ForeColor="Red" />
                            <br />
                            <br />
                            <asp:LinkButton runat="server" ID="closeWarning" Text="Close" />
                        </td>
                        <td>
                            <div style="overflow: scroll; height: 110px; width: 500px; border: solid 1px red;">
                                <asp:Label runat="server" ID="warningText" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="forecastPanel" CssClass="fPanel">
                <div class="dayPanel">
                    <div class="dayHeader">
                        <asp:Label runat="server" ID="day1Label" CssClass="dateLabel">tomorrow</asp:Label>
                    </div>
                    <div class="fItems">
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day1High" TemperatureExtreme="High" Visible="false" />
                        </div>
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day1Low" TemperatureExtreme="Low" Visible="false" />
                        </div>
                    </div>
                    <div class="eventList" style="height: 16px; visibility: hidden; position:fixed">
                        <uc1:CalendarItemDisplay ID="day1Events" runat="server" />
                    </div>
                </div>
                <div class="dayPanelAlt">
                    <div class="dayHeader">
                        <asp:Label runat="server" ID="day2Label" CssClass="dateLabel">Day 2</asp:Label>
                    </div>
                    <div class="fItems">
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day2High" TemperatureExtreme="High" Visible="false" />
                        </div>
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day2Low" TemperatureExtreme="Low" Visible="false" />
                        </div>
                    </div>
                    <div class="eventList" style="height: 16px; visibility: hidden; position:fixed">
                        <uc1:CalendarItemDisplay ID="day2Events" runat="server" />
                    </div>
                </div>
                <div class="dayPanel">
                    <div class="dayHeader">
                        <asp:Label runat="server" ID="day3Label" CssClass="dateLabel">Day 3</asp:Label>
                    </div>
                    <div class="fItems">
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day3High" TemperatureExtreme="High" Visible="false" />
                        </div>
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day3Low" TemperatureExtreme="Low" Visible="false" />
                        </div>
                    </div>
                    <div class="eventList" style="height: 16px; visibility: hidden; position:fixed">
                        <uc1:CalendarItemDisplay ID="day3Events" runat="server" />
                    </div>
                </div>
                <div class="dayPanelAlt">
                    <div class="dayHeader">
                        <asp:Label runat="server" ID="day4Label" CssClass="dateLabel">Day 4</asp:Label>
                    </div>
                    <div class="fItems">
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day4High" TemperatureExtreme="High" Visible="false" />
                        </div>
                        <div class="fItem">
                            <ucb:ForecastItem runat="server" ID="day4Low" TemperatureExtreme="Low" Visible="false" />
                        </div>
                    </div>
                    <div class="eventList" style="height: 16px; visibility: hidden; position:fixed">
                        <uc1:CalendarItemDisplay ID="day4Events" runat="server" />
                    </div>
                </div>
                <div style="height: 15px; padding: 2px 2px 2px 2px; width: 100%; background-color: #B6BCC4; visibility: hidden; position: fixed">
                    <div style="float: left">
                        <!--Next&nbsp;2&nbsp;weeks:&nbsp;&nbsp;-->
                    </div>
                    <div style="float: left">
                        <asp:Panel runat="server" ID="upcomingEvents" />
                    </div>
                    <div style="float: right; padding: 2px 4px 2px 2px">
                        Calendar: <a href="http://home.msn2.net/Lists/Events/NewForm.aspx?RootFolder=%2FLists%2FEvents&Source=http%3A%2F%2Fhome%2Emsn2%2Enet%2FLists%2FEvents%2Fcalendar%2Easpx"
                            target="_top">Add</a> | <a href="http://home.msn2.net/Lists/Events/" target="_top">View</a>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
