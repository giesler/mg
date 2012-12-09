<%@ Page language="c#" Codebehind="all.aspx.cs" AutoEventWireup="false" Inherits="amsadmin.ads.all" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript (ECMAScript)">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="all" method="post" runat="server">
			<asp:DataGrid id="dgCompany" style="Z-INDEX: 101; LEFT: 49px; POSITION: absolute; TOP: 78px" runat="server" Width="408px" Height="152px" BorderStyle="None" GridLines="Vertical" BorderWidth="1px" BorderColor="#999999" BackColor="White" CellPadding="3" AutoGenerateColumns="False">
				<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
				<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="#DCDCDC"></AlternatingItemStyle>
				<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
				<Columns>
					<asp:HyperLinkColumn Text="Company" DataNavigateUrlField="CompanyID" DataNavigateUrlFormatString="adlist.aspx?companyid={0}" DataTextField="CompanyName" HeaderText="Company"></asp:HyperLinkColumn>
					<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="CompanyURL" DataTextField="CompanyURL" HeaderText="Company URL"></asp:HyperLinkColumn>
				</Columns>
			</asp:DataGrid>
			<asp:Label id="Label1" style="Z-INDEX: 102; LEFT: 16px; POSITION: absolute; TOP: 16px" runat="server" Width="95px" Height="19px">You are here:</asp:Label>
			<asp:Label id="Label2" style="Z-INDEX: 103; LEFT: 120px; POSITION: absolute; TOP: 16px" runat="server" Width="88px" Height="19px">AMS Admin</asp:Label>
			<asp:Label id="Label3" style="Z-INDEX: 104; LEFT: 216px; POSITION: absolute; TOP: 16px" runat="server" Width="8px" Height="24px">\</asp:Label>
			<asp:Label id="Label4" style="Z-INDEX: 105; LEFT: 240px; POSITION: absolute; TOP: 16px" runat="server" Width="96px" Height="19px">Ad Companies</asp:Label>
		</form>
	</body>
</HTML>
