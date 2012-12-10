<%@ Control Language="c#" AutoEventWireup="false" Codebehind="_News.ascx.cs" Inherits="AMS_Welcome.AMSNews"%>
<%@ OutputCache Duration="60" VaryByParam="none" %>
<asp:Repeater id="rpNews" runat="server">
	<HeaderTemplate>
	</HeaderTemplate>
	<ItemTemplate>
		<p>
			<%# ShowTitle(DataBinder.Eval(Container.DataItem, "NewsTitle")) %>
			<font color="gray" size="-1"><i>
					<%# ShowTime(DataBinder.Eval(Container.DataItem, "NewsDateTime")) %>
				</i></font>
			<br>
			<%# DataBinder.Eval(Container.DataItem, "NewsContent") %>
		</p>
	</ItemTemplate>
	<FooterTemplate>
	</FooterTemplate>
</asp:Repeater>
