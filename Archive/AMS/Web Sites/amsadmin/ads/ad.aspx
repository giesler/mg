<%@ Page language="c#" Codebehind="ad.aspx.cs" AutoEventWireup="false" Inherits="amsadmin.ads.ad" smartNavigation="True"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript (ECMAScript)">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="ad" method="post" runat="server">
			<asp:HyperLink id="lnkCompany" style="Z-INDEX: 101; LEFT: 40px; POSITION: absolute; TOP: 40px" runat="server" Width="404px" Height="19px">HyperLink</asp:HyperLink>
			<asp:Label id="Label14" style="Z-INDEX: 126; LEFT: 85px; POSITION: absolute; TOP: 633px" runat="server" Height="24px" Width="88px">Image URL:</asp:Label>
			<asp:Label id="Label13" style="Z-INDEX: 125; LEFT: 87px; POSITION: absolute; TOP: 594px" runat="server" Height="24px" Width="88px">Image File:</asp:Label>
			<asp:DropDownList id="DropDownList2" style="Z-INDEX: 122; LEFT: 200px; POSITION: absolute; TOP: 552px" runat="server" Width="280px" Height="19px">
				<asp:ListItem Value="Standard Image" Selected="True">Standard Image</asp:ListItem>
				<asp:ListItem Value="Hosted Image">Hosted Image</asp:ListItem>
				<asp:ListItem Value="HTML" Selected="True">HTML</asp:ListItem>
			</asp:DropDownList>
			<asp:Label id="Label9" style="Z-INDEX: 112; LEFT: 88px; POSITION: absolute; TOP: 344px" runat="server" Width="72px" Height="19px">Goal Type:</asp:Label>
			<asp:Label id="Label11" style="Z-INDEX: 120; LEFT: 312px; POSITION: absolute; TOP: 400px" runat="server" Width="59px" Height="19px">Current:</asp:Label>
			<asp:TextBox id="TextBox4" style="Z-INDEX: 119; LEFT: 376px; POSITION: absolute; TOP: 400px" runat="server" Width="88px" Height="23px" Enabled="False"></asp:TextBox>
			<asp:Label id="Label10" style="Z-INDEX: 117; LEFT: 312px; POSITION: absolute; TOP: 376px" runat="server" Width="59px" Height="19px">Current:</asp:Label>
			<asp:TextBox id="TextBox2" style="Z-INDEX: 116; LEFT: 192px; POSITION: absolute; TOP: 400px" runat="server" Width="88px" Height="21px"></asp:TextBox>
			<asp:Label id="Label8" style="Z-INDEX: 115; LEFT: 88px; POSITION: absolute; TOP: 400px" runat="server" Width="77px" Height="20px">Clicks:</asp:Label>
			<asp:Label id="Label6" style="Z-INDEX: 111; LEFT: 88px; POSITION: absolute; TOP: 344px" runat="server" Width="72px" Height="19px">Goal Type:</asp:Label>
			<asp:Label id="Label5" style="Z-INDEX: 109; LEFT: 32px; POSITION: absolute; TOP: 512px" runat="server" Width="647px" Height="20px" BackColor="#E0E0E0">Ad Goals</asp:Label>
			<asp:Label id="Label2" style="Z-INDEX: 108; LEFT: 32px; POSITION: absolute; TOP: 304px" runat="server" Width="647px" Height="20px" BackColor="#E0E0E0">Ad Goals</asp:Label>
			<asp:Label id="Label4" style="Z-INDEX: 107; LEFT: 88px; POSITION: absolute; TOP: 192px" runat="server" Width="72px" Height="19px">Comments:</asp:Label>
			<asp:TextBox id="txtName" style="Z-INDEX: 102; LEFT: 192px; POSITION: absolute; TOP: 120px" runat="server" Width="432px" Height="26px"></asp:TextBox>
			<asp:Label id="Label1" style="Z-INDEX: 103; LEFT: 80px; POSITION: absolute; TOP: 120px" runat="server" Width="72px" Height="19px">Name:</asp:Label>
			<asp:Label id="Label3" style="Z-INDEX: 104; LEFT: 32px; POSITION: absolute; TOP: 88px" runat="server" Width="646px" Height="20px" BackColor="#E0E0E0">Ad Information</asp:Label>
			<asp:CheckBox id="chkActive" style="Z-INDEX: 105; LEFT: 192px; POSITION: absolute; TOP: 152px" runat="server" Width="283px" Text="Active"></asp:CheckBox>
			<asp:TextBox id="txtComments" style="Z-INDEX: 106; LEFT: 192px; POSITION: absolute; TOP: 184px" runat="server" Width="433px" Height="100px"></asp:TextBox>
			<asp:DropDownList id="DropDownList1" style="Z-INDEX: 110; LEFT: 192px; POSITION: absolute; TOP: 344px" runat="server" Width="280px" Height="19px">
				<asp:ListItem Value="None" Selected="True">None</asp:ListItem>
				<asp:ListItem Value="Requests" Selected="True">Requests</asp:ListItem>
				<asp:ListItem Value="Clicks">Clicks</asp:ListItem>
			</asp:DropDownList>
			<asp:TextBox id="TextBox1" style="Z-INDEX: 113; LEFT: 192px; POSITION: absolute; TOP: 376px" runat="server" Width="88px" Height="21px"></asp:TextBox>
			<asp:Label id="Label7" style="Z-INDEX: 114; LEFT: 88px; POSITION: absolute; TOP: 376px" runat="server" Width="77px" Height="20px">Requests:</asp:Label>
			<asp:TextBox id="TextBox3" style="Z-INDEX: 118; LEFT: 376px; POSITION: absolute; TOP: 376px" runat="server" Width="89px" Height="18px" Enabled="False"></asp:TextBox>
			<asp:Label id="Label12" style="Z-INDEX: 121; LEFT: 80px; POSITION: absolute; TOP: 552px" runat="server" Width="88px" Height="24px">Type:</asp:Label>
			<INPUT style="Z-INDEX: 123; LEFT: 197px; WIDTH: 457px; POSITION: absolute; TOP: 594px; HEIGHT: 22px" type="file" size="57">
			<INPUT style="Z-INDEX: 124; LEFT: 195px; WIDTH: 463px; POSITION: absolute; TOP: 633px; HEIGHT: 21px" type="text" size="71">
			<asp:Label id="Label15" style="Z-INDEX: 127; LEFT: 84px; POSITION: absolute; TOP: 679px" runat="server" Height="23px" Width="96px">Target URL:</asp:Label>
			<asp:TextBox id="TextBox5" style="Z-INDEX: 128; LEFT: 200px; POSITION: absolute; TOP: 678px" runat="server" Height="20px" Width="461px"></asp:TextBox>
			<asp:TextBox id="TextBox6" style="Z-INDEX: 129; LEFT: 200px; POSITION: absolute; TOP: 714px" runat="server" Height="69px" Width="462px"></asp:TextBox>
			<asp:Label id="Label16" style="Z-INDEX: 130; LEFT: 86px; POSITION: absolute; TOP: 720px" runat="server" Height="25px" Width="95px">HTML:</asp:Label>
			<asp:Button id="Button1" style="Z-INDEX: 132; LEFT: 319px; POSITION: absolute; TOP: 812px" runat="server" Height="33px" Width="129px" Text="Save &amp; Preview Ad"></asp:Button>
		</form>
	</body>
</HTML>
