<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForecastItem.ascx.cs"
    Inherits="HomeCalendarView.ForecastItem" %>
<table>
    <tr>
        <td rowspan="2">
            <asp:Image runat="server" Height="50" ID="image" />
        </td>
        <td>
            <asp:Label runat="server" CssClass="hiText" ID="tempText">44</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" CssClass="precipText" ID="precipText"></asp:Label>
        </td>
    </tr>
</table>
