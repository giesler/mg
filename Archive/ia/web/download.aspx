<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="download.aspx.cs" AutoEventWireup="false" Inherits="vbsw.downloadbutton" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>download</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
		<script language="JavaScript"><!--
			function dlfile() {
				if ('<%=DownloadFile%>' != '')
					window.location = '<%=DownloadFile%>';
			}
			// -->
		</script>
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" onload="dlfile();">
		<form id="download" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<asp:Panel ID="downloadPanel" Runat="server">
				<P>Downloading Install Assistant:
				</P>
				<OL>
					<LI>
						Read the <A href="docs.aspx">docs</A>
					<LI>
					Fill out the form below.
					<LI>
						After downloading, feel free to provide <A href="feedback.aspx">feedback</A>. 
						I'd really like to hear what you like and what you'd like to see added.&nbsp; 
						If you use IA, send me a <A href="about.aspx">postcard</A>.
					</LI>
				</OL>
				<P><B>Note:</B> By downloading IA you agree to this <A href="about.aspx">disclaimer</A>.</P>
				<TABLE class="clsTable" cellSpacing="0" cellPadding="4" align="center" bgColor="black" border="0">
					<TR>
						<TD class="clsTableHeader" align="middle" colSpan="2">Download VBSW</TD>
					</TR>
					<TR>
						<TD class="clsTableBodyEven" vAlign="top">Email Address:</TD>
						<TD class="clsTableBodyEven" vAlign="top">
							<asp:TextBox id="email" Runat="server" MaxLength="100"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD class="clsTableBodyOdd" vAlign="top">Subscribe:</TD>
						<TD class="clsTableBodyOdd" vAlign="top">
							<TABLE>
								<TR>
									<TD vAlign="top">
										<asp:CheckBox id="subscribe" Runat="server" Font-Size="X-Small" Checked="True"></asp:CheckBox></TD>
									<TD><FONT size="-2"><LABEL for="subscribe">Check to be notified when new versions are 
												released.<BR>
												<I>Your address will not be sold or released to anyone.</I></LABEL></FONT></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD class="clsTableBodyEven" vAlign="top">Select Version:</TD>
						<TD class="clsTableBodyEven" vAlign="top">
							<asp:RadioButtonList id="version" Runat="server" Font-Size="X-Small">
								<asp:ListItem Value="300" Selected="True">3.00 (Current Version) (231k)</asp:ListItem>
								<asp:ListItem Value="200">2.00 &lt;i&gt;(139k)&lt;/i&gt;</asp:ListItem>
								<asp:ListItem Value="101">1.01 &lt;i&gt;(132k)&lt;/i&gt;</asp:ListItem>
								<asp:ListItem Value="100">1.00 &lt;i&gt;(130k)&lt;/i&gt;</asp:ListItem>
							</asp:RadioButtonList></TD>
					</TR>
					<TR>
						<TD class="clsTableFooter" colSpan="2">
							<TABLE width="100%">
								<TR>
									<TD align="middle">
										<asp:Button id="downloadNow" runat="server" Width="100px" CssClass="clsButton" Text="Download"></asp:Button></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</asp:Panel>
			<asp:Panel Runat="server" ID="donePanel" Visible="False">
				<P>Your download should begin shortly. If it does not, click <A 
href="<%=DownloadFile%>">here</A> to start the download.
					<BR>
					<BR>
					<BR>
				</P>
				<P>If you would like to view the documentation, click <A href="docs.aspx">here</A>.
				</P>
			</asp:Panel>
			<br>
			<br>
			<br>
			<br>
			<br>
			<br>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
			<P></P>
		</form>
	</body>
</HTML>
