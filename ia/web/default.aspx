<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="vbsw._default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>default</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="default" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<table cellpadding="3" cellspacing="0" border="0" width="100%">
				<tr>
					<td><a href="image.aspx?url=Images/v300/autorun_full.jpg"><img src="Images/v300/autorun_thumb.jpg" border="0"></a></td>
					<td>
						<P>Welcome to the homepage of Install Assistant (IA). IA acts as a 'wrapper' for 
							your installation; it handles the updating of any system files your 
							installation may require, and allows you to build a customized 'splash' screen 
							you can display before running setup. It is written in C, and can be run on any 
							Win32 version - Windows 95, 98, Me, NT4, 2000, and XP. IA is <a href="about.aspx">postcard-ware</a>. 
							Check out the <A href="docs.aspx">docs</A> then <A href="download.aspx">download</A>
							the program.
						</P>
					</td>
				</tr>
			</table>
			<table cellpadding="3" cellspacing="0" border="0" width="100%">
				<tr>
					<td>
						<P><EM>IA used to be named 'VB Setup Wrapper'.&nbsp; It has been renamed to reflect the 
								fact it can be used for other languages besides VB, and offers many more 
								features then a simple 'wrapper' program.</EM></P>
						<P><EM><FONT size="2"><STRONG>You can click on images to view a full size picture throughout 
										this site.</STRONG></FONT></EM></P>
					</td>
					<td>
						<a href="image.aspx?url=Images/v300/setup_full.jpg"><img src="images/v300/setup_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<P>&nbsp;</P>
			<table width="100%" border="0" cellpadding="4" cellspacing="0" class="clsTable" bgcolor="black" align="center">
				<tr>
					<td class="clsTableHeader" align="middle" colSpan="2">News</td>
				</tr>
				<TR>
					<TD class="clsTableBodyOdd" vAlign="top">
						<P><STRONG>January 25, 2002<BR>
								-&nbsp;v 3.00</STRONG></P>
					</TD>
					<TD class="clsTableBodyOdd" vAlign="top">
						<p>
							Version 3 is now available! The new features are:</p>
						<ul>
							<li>
							Splash dialog can now have as many buttons as you'd like
							<li>
							Buttons: Can now launch web pages, programs, documents, or browse folders
							<li>
							Buttons: Can have sounds associated with them (MouseEnter, MouseExit, 
							MouseDown, MouseUp)
							<li>
							Buttons: Can choose whether or not to do a component check when a button is 
							pressed
							<li>
							Buttons: Can prompt user to restart when a button action completes
							<li>
							Buttons: Can redisplay splash dialog after a button action completes
							<li>
							Buttons: Can specify custom cursor to display for MouseOver
							<li>
							Components: Added ability to 'always install' a component
							<li>
							Components: Improved OS requirements specifications
							<LI>
							Components: Added native support for checking/installing the .Net framework
							<li>
							General: Can now customize reboot prompt (timing and auto reboot)
							<li>
							General: Can prevent multiple instances from starting
							<li>
							General: Can prevent splash from showing when a user-defined mutex is defined
							<li>
							General: Can enable/disable logging
							<li>
							General: Improved/more detailed logging
							<li>
							Splash: Can set to display/not display/only display when program is named xxx
							<li>
							Splash: Can hide title bar
							<li>
							Splash: Sound effects for when splash opens and closes
							<li>
								Settings Utility: Misc. improvements</li>
						</ul>
					</TD>
				</TR>
				<tr>
					<TD class="clsTableBodyEven" vAlign="top" colspan="2">
						<a href="news.aspx">Older news...</a>
					</TD>
				</tr>
			</table>
			<P><FONT size="2"></FONT>&nbsp;</P>
			<table width="100%" border="0" cellpadding="4" cellspacing="0" class="clsTable" bgcolor="black" align="center">
				<tr>
					<td class="clsTableHeader" align="middle" colSpan="3">Resources</td>
				</tr>
				<tr>
					<td class="clsTableBodyOdd" valign="top" align="middle" width="33%"><a href="http://vbwire.com/"><img src="Images/vbwire.gif" border="0"></a></td>
					<td class="clsTableBodyOdd" valign="top" align="middle" width="33%"><a href="http://msdn.microsoft.com/"><img src="Images/msdn-logo.gif" border="0"></a></td>
					<td class="clsTableBodyOdd" valign="top" align="middle" width="33%"><a href="http://devdex.com/"><img src="Images/devdex.gif" border="0"></a></td>
				</tr>
			</table>
			<P>&nbsp;</P>
		</form>
		<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		<P></P>
	</body>
</HTML>
