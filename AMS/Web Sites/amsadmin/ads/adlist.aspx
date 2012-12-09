<%@ Page language="c#" Codebehind="adlist.aspx.cs" AutoEventWireup="false" Inherits="amsadmin.ads.adlist" smartNavigation="True"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="adlist" method="post" runat="server">
			<asp:label id="lblTitle" style="Z-INDEX: 100; LEFT: 73px; POSITION: absolute; TOP: 345px" runat="server" Width="376px" Height="21px">Company Ads</asp:label>
			<asp:CheckBox id="chkActive" style="Z-INDEX: 116; LEFT: 188px; POSITION: absolute; TOP: 146px" runat="server" Width="283px" Text="Active"></asp:CheckBox>
			<asp:Label id="Label7" style="Z-INDEX: 113; LEFT: 16px; POSITION: absolute; TOP: 16px" runat="server" Height="19px" Width="95px">You are here:</asp:Label>
			<asp:Label id="Label6" style="Z-INDEX: 103; LEFT: 120px; POSITION: absolute; TOP: 16px" runat="server" Height="19px" Width="88px">AMS Admin</asp:Label>
			<asp:Label id="Label5" style="Z-INDEX: 106; LEFT: 216px; POSITION: absolute; TOP: 16px" runat="server" Height="24px" Width="8px">\</asp:Label>
			<asp:Label id="Label4" style="Z-INDEX: 107; LEFT: 240px; POSITION: absolute; TOP: 16px" runat="server" Height="19px" Width="96px">Ad Companies</asp:Label>
			<asp:label id="Label3" style="Z-INDEX: 104; LEFT: 72px; POSITION: absolute; TOP: 175px" runat="server" Width="108px" Height="18px">URL:</asp:label>
			<asp:label id="Label1" style="Z-INDEX: 101; LEFT: 71px; POSITION: absolute; TOP: 85px" runat="server" Width="300px" Height="20px">Company Information</asp:label>
			<asp:label id="Label2" style="Z-INDEX: 102; LEFT: 72px; POSITION: absolute; TOP: 116px" runat="server" Width="108px" Height="18px">Name:</asp:label>
			<asp:textbox id="txtName" style="Z-INDEX: 108; LEFT: 188px; POSITION: absolute; TOP: 117px" runat="server" Width="293px" Height="20px"></asp:textbox>
			<asp:textbox id="txtURL" style="Z-INDEX: 109; LEFT: 187px; POSITION: absolute; TOP: 174px" runat="server" Width="298px" Height="21px"></asp:textbox>
			<asp:button id="btnSave" style="Z-INDEX: 110; LEFT: 398px; POSITION: absolute; TOP: 289px" runat="server" Width="83px" Height="26px" Text="Update"></asp:button>
			<asp:requiredfieldvalidator id="RequiredFieldValidator1" style="Z-INDEX: 111; LEFT: 495px; POSITION: absolute; TOP: 121px" runat="server" Width="228px" Height="15px" ErrorMessage="Company Name is required!" ControlToValidate="txtName"></asp:requiredfieldvalidator>
			<asp:DataGrid id="dgAds" style="Z-INDEX: 112; LEFT: 54px; POSITION: absolute; TOP: 375px" runat="server" Height="100px" Width="463px" AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderColor="#999999" BorderWidth="1px" GridLines="Vertical" BorderStyle="None">
				<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
				<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
				<Columns>
					<asp:HyperLinkColumn DataNavigateUrlField="AdID" DataNavigateUrlFormatString="ad.aspx?adid={0}" DataTextField="AdTitle" HeaderText="Title"></asp:HyperLinkColumn>
				</Columns>
			</asp:DataGrid>
			<asp:TextBox id="TextBox1" style="Z-INDEX: 114; LEFT: 188px; POSITION: absolute; TOP: 207px" runat="server" Height="69px" Width="301px"></asp:TextBox>
			<asp:Label id="Label8" style="Z-INDEX: 115; LEFT: 74px; POSITION: absolute; TOP: 214px" runat="server" Width="104px">Comments:</asp:Label>
		</form>
	</body>
</HTML>
