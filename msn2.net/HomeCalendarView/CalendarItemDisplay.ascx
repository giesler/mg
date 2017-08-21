<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarItemDisplay.ascx.cs"
    Inherits="HomeCalendarView.CalendarItemDisplay" %>
<asp:Repeater runat="server" ID="eventList">
    <ItemTemplate>
        Item: <asp:Label ID="Label1" runat="server" Text='<% DataBinder.Eval(Container.DataItem, "Title")%>'></asp:Label><br />
    </ItemTemplate>
</asp:Repeater>
<asp:Panel runat="server" ID="noEvents" CssClass="noEventsText" Visible="false">
    No events.
</asp:Panel>