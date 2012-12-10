<%@ Page language="c#" Codebehind="maillist.aspx.cs" AutoEventWireup="false" Inherits="vbsw.maillist" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>maillist</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="maillist" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:Panel runat="server" ID="pnlMain">
				<P>You can subscribe and unsubscribe to the mailling list below. If you are 
					subscribed, you will receive notification of new versions or any critical 
					issues. Your name will not be sold or provided to anyone. Simply enter your 
					email address, and click 'Subscribe' or 'Unsubscribe'.
				</P>
				<TABLE class="clsTable" cellSpacing="0" cellPadding="4" align="center" bgColor="black" border="0">
					<TR>
						<TD class="clsTableHeader" align="middle" colSpan="2">Mail List Subscription</TD>
					</TR>
					<TR>
						<TD class="clsTableBodyOdd" vAlign="top">Your Email:</TD>
						<TD class="clsTableBodyOdd" vAlign="top">
							<asp:TextBox id="email" MaxLength="40" Runat="server"></asp:TextBox>
							<asp:RequiredFieldValidator id="RequiredFieldValidator1" Runat="server" Display="Dynamic" ControlToValidate="email" ErrorMessage="*"></asp:RequiredFieldValidator><BR>
							<asp:Label id="lblNote" Runat="server" Font-Italic="true"></asp:Label></TD>
					</TR>
					<TR>
						<TD class="clsTableBodyEven" vAlign="top">Action:</TD>
						<TD class="clsTableBodyEven" vAlign="top">
							<asp:RadioButtonList id="action" Runat="server" CellSpacing="0" CellPadding="0" Font-Size="X-Small">
								<asp:ListItem Value="Subscribe" Selected="True">Subscribe</asp:ListItem>
								<asp:ListItem Value="Unsubscribe">Unsubscribe</asp:ListItem>
							</asp:RadioButtonList></TD>
					</TR>
					<TR>
						<TD class="clsTableFooter" colSpan="2">
							<TABLE width="100%">
								<TR>
									<TD align="middle">
										<asp:Button id="OK" Runat="server" Width="100" Text=" OK " CssClass="clsButton"></asp:Button></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</asp:Panel>
			<asp:Panel Runat="server" ID="pnlDone" Width="100%">
				<P>
					<asp:Label id="actionResult" Runat="server"></asp:Label></P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
			</asp:Panel>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
