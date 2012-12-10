<%@ Page language="c#" Codebehind="feedback.aspx.cs" AutoEventWireup="false" Inherits="vbsw.feedback" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>feedback</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="feedback" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:Panel ID="pnlEnterInfo" Runat="server" Width="100%">
				<P>Need help? Find a bug? Have a suggestion? Fill out the form below. You can also 
					send email directly to vbsw@nospam.giesler.org - just remove the 'nospam' part.
				</P>
				<TABLE class="clsTable" cellSpacing="0" cellPadding="4" align="center" bgColor="black" border="0">
					<TR>
						<TD class="clsTableHeader" align="middle" colSpan="2">Feedback / Help</TD>
					</TR>
					<TR>
						<TD class="clsTableBodyEven" vAlign="top">Your Name:</TD>
						<TD class="clsTableBodyEven" vAlign="top">
							<asp:TextBox id="name" Runat="server" MaxLength="33"></asp:TextBox>
							<asp:RequiredFieldValidator id="RequiredFieldValidator1" Runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="name"></asp:RequiredFieldValidator></TD>
					</TR>
					<TR>
						<TD class="clsTableBodyOdd" vAlign="top">Your Email:</TD>
						<TD class="clsTableBodyOdd" vAlign="top">
							<asp:TextBox id="email" Runat="server" MaxLength="40"></asp:TextBox>
							<asp:RequiredFieldValidator id="Requiredfieldvalidator2" Runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="email"></asp:RequiredFieldValidator></TD>
					<TR>
						<TD class="clsTableBodyEven" vAlign="top">Issue:</TD>
						<TD class="clsTableBodyEven" vAlign="top">
							<asp:RadioButtonList id="subject" Runat="server" Font-Size="X-Small" CellSpacing="0" CellPadding="0">
								<asp:ListItem Value="Help" Selected="True">Help</asp:ListItem>
								<asp:ListItem Value="Suggestion">Suggestion</asp:ListItem>
								<asp:ListItem Value="Bug Report">Bug Report</asp:ListItem>
								<asp:ListItem Value="Other">Other</asp:ListItem>
							</asp:RadioButtonList></TD>
					</TR>
					<TR>
						<TD class="clsTableBodyOdd" vAlign="top" colSpan="2">
							<asp:TextBox id="body" Runat="server" TextMode="MultiLine" Columns="40" Rows="5"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD class="clsTableFooter" colSpan="2">
							<TABLE width="100%">
								<TR>
									<TD align="middle">
										<asp:Button id="buttonSend" Width="100" Runat="server" Text="Send" CssClass="clsButton"></asp:Button>&nbsp;
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
			</asp:Panel>
			<asp:Panel Runat="server" ID="pnlDone" Visible="False" Width="100%">
				<P>Thanks for the feedback. If you had a question or problem, you should hear back 
					soon.
				</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
			</asp:Panel>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
