<%@ Page language="c#" Codebehind="news.aspx.cs" AutoEventWireup="false" Inherits="vbsw.news" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>news</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="default" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<table width="100%" border="0" cellpadding="4" cellspacing="0" class="clsTable" bgcolor="black" align="center">
				<tr>
					<td class="clsTableHeader" align="middle" colSpan="2">Old News</td>
				</tr>
				<tr>
					<td class="clsTableBodyOdd" valign="top"><b>January 14, 2001 - VBSW 2.00 Release</b></td>
					<td class="clsTableBodyOdd" valign="top">
						Version 2 is now available!
					</td>
				</tr>
				<tr>
					<td class="clsTableBodyEven" valign="top"><b>January 9, 2001 - VBSW 2.00 BETA</b></td>
					<td class="clsTableBodyEven" valign="top">
						This new release has a TON of new features. Fully customizable autorun dialog 
						(you can specify a background and button images), fully customizable system 
						component checking portion, and a bunch of other 'under the hood' improvements. 
						For full details, check the <a href="docs.aspx">documentation</a>.
					</td>
				</tr>
				<tr>
					<td class="clsTableBodyOdd" valign="top"><b>August 19, 2000</b></td>
					<td class="clsTableBodyOdd" valign="top">
						I am working on VBSW version 2.0. The new version should have the following 
						features:
						<ul>
							<li>
							Customizable system components - you can specify the DLL you want checked. (So 
							you can install different versions of MDAC, VB runtime, IE, etc.
							<li>
							NT Service Pack level check and install
							<li>
							Graphical buttons with OnMouseOver and OnMouseClick button images you can 
							specify
							<li>
							Sizable 'autorun' dialog
							<li>
								Debugging mode which will specify components checked to a log file
							</li>
						</ul>
						If you have any suggestions feel free to let me know by sending <a href="feedback.aspx">
							feedback</a>. If you'd like to test it when ready, send <a href="feedback.aspx">
							feedback</a> as well, letting me know. When will it be ready? Probably not 
						for another month or so (or longer...).
					</td>
				</tr>
				<tr>
					<td class="clsTableBodyEven" valign="top"><b>August 1, 2000 - VBSW Version 1.01</b></td>
					<td class="clsTableBodyEven" valign="top">
						<ul>
							<li>
							Fixed code to check for Windows Installer version 1.1 (was checking for 1.0)
							<li>
								Added support for updating comctl32.dll (via 50comupd.exe - see <a href="docs.aspx">
									docs</a>)</li>
						</ul>
					</td>
				</tr>
				<tr>
					<td class="clsTableBodyOdd" valign="top"><b>July 25, 2000</b></td>
					<td class="clsTableBodyOdd" valign="top">
						I'm not going to promise future features, but I am working on another version 
						that will allow you to specify, in an INI file, all the system components and 
						corresponding versions you would like updated. ETA? I'm not sure.
					</td>
				</tr>
			</table>
			<P>&nbsp;</P>
			<P>
				<uc1:Footer id="Footer1" runat="server"></uc1:Footer></P>
		</form>
		<P></P>
	</body>
</HTML>
