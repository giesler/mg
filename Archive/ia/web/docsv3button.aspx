<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="docsv3button.aspx.cs" AutoEventWireup="false" Inherits="vbsw.docsv3button" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Documentation: V3</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="docsv2adv" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<h3>Button Documentation</h3>
			<p>
				'Buttons' can be customized to perform a variety of actions. Double click a 
				button on the Button tab to modify button settings.
			</p>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>General Settings</b><br>
							<ul>
								<li>
									<b>Name:</b>
								Button name.&nbsp; Only used in settings utility and log file
								<LI>
									<STRONG>Run Component Check:</STRONG>&nbsp; If you want to run the check of 
								system component before performing the button action, check here.
								<LI>
									<STRONG>Splash Dialog:&nbsp; </STRONG>The behavior of the splash dialog when 
									the button action is performed can be specified here.</LI>
							</ul>
						<P></P>
					<td width="250">
						<a href="image.aspx?url=Images/v300/button/general_settings_full.jpg"><img src="Images/v300/button/general_settings_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Button Action</b><br>
							<UL>
								<LI>
									<STRONG>Run a program</STRONG>:&nbsp; You can specify a program to run when the 
									button is pressed.&nbsp; (You can also use the same variables as in component file version checking, for 
									example, to specify the Windows directory, use %WinPath%).&nbsp; The 'cmd line' entry must start with the 
									EXE name specified in 'Setup/MSI'.&nbsp; (Unless you are using an MSI, in which 
									case you can leave the cmd line blank)&nbsp; You can also specify if you'd like 
									the user to restart following setup.</LI>
								<LI>
									<STRONG>Launch a URL:&nbsp; </STRONG>Opens the specified URL</LI>
								<LI>
									<STRONG>Open a File:</STRONG>&nbsp; Opens a file with the associated 
									application (for example, 'readme.txt' will open in Notepad)</LI>
								<LI>
									<STRONG>Cancel</STRONG>:&nbsp; Closes splash dialog and ends setup</LI></UL>
					<td width="250">
						<a href="image.aspx?url=Images/v300/button/button_action_full.jpg"><img src="Images/v300/button/button_action_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Images</b>
						<P>Specify the images for various states of the buttons.&nbsp; The placement is the 
							position of the top left corner of the image relative to the top left corner of 
							the dialog.&nbsp; The image sizes must be the same.&nbsp; You can also specify 
							a mouse cursor (.cur file) to use when the mouse is over the button.</P>
					<td width="250">
						<a href="image.aspx?url=Images/v300/button/images_full.jpg"><img src="Images/v300/button/images_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<P>
							<b>Sounds</b></P>
						<P>Specify sounds for various mouse button/action states.<br>
							&nbsp;</P>
					<td width="250">
						<a href="image.aspx?url=Images/v300/button/sounds_full.jpg"><img src="Images/v300/button/sounds_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr noshade>
			<p>
				To download now, click <a href="download.aspx">here</a><br>
				To view the component documentation, click <a href="docsv3component.aspx">here</a>.<br>
				To return to the main documentation, click <a href="docs.aspx">here</a>.<br>
				If you have any questions, feel free to ask from the <a href="feedback.aspx">feedback</a>
				page.
			</p>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
