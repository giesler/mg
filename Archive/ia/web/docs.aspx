<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="docs.aspx.cs" AutoEventWireup="false" Inherits="vbsw.docs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>docs</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="docs" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<p>
				Docs: <b>Version 3</b> | <a href="docsv2.aspx">Verison 2</a> | <a href="docsv1.aspx">
					Verison 1</a>
			</p>
			<h3>Introduction</h3>
			<p>This documentation describes how Install Assistant (IA) works, how to set up the 
				directory structure, and then how to customize the program to meet your needs.
			</p>
			<p>
				IA is designed to install system related files for your program. It will 
				restart the target computer as needed and resume installation. Once all 
				necessary system files have been updated, your setup program / Windows 
				Installer package will run.&nbsp; You can now create a fully customizable 
				'splash' screen.
			</p>
			<table width="75%" border="1" cellpadding="3" cellspacing="0" align="center">
				<tr>
					<td class="clsTableBodyEven">
						<b>Upgrading from Verison 2</b>: The format of settings.ini and the CD layout 
						has changed somewhat since version 2. To convert a v2 layout to v3, rename the 
						folder 'vbsw' to 'ia'. You can then open the layout in the Settings Utility. 
						You will also need to add new buttons to replace the 'Install' and 'Cancel buttons 
						for your setup/msi.
					</td>
				</tr>
			</table>
			<h3>Steps</h3>
			<ol>
				<li>
				Read this entire page
				<li>
					<a href="download.aspx">Download</a>
				&nbsp;IA.
				<li>
				Using the included 'IA Settings Utility', customize the sample installation or 
				create a new installation.
				<li>
				Download the components your installation requires, and extract any files that 
				need extracting.
				<li>
					Deploy by CD or network share.</li>
			</ol>
			<hr noshade>
			<h3>IA Settings Utility</h3>
			<img src="Images/v300/cd_root.jpg" align="right"> The IA Settings Utility 
			allows you to customize your installation. This utility requires the <a href="http://download.microsoft.com/download/vb60pro/Redist/sp5/WIN98Me/EN-US/vbrun60sp5.exe">
				VB6 runtime files</a>, and IE4.0 or higher (ONLY the utility has these 
			requirements - the main program does not!). A basic installation has the 
			directory structure shown to the right.
			<ul>
				<LI>
				\ia - contains the settings file (settings.ini) and images used by Autorun.exe
				<LI>
				\setup or \msi - contains either the 'setup.exe' or 'Intall Package.msi' for 
				your installation
				<li>
				\sysfiles - contains installers for system components
				<li>
				Autorun.exe - Main program (IA)
				<li>
				Autorun.inf - When used at CD root, causes autorun.exe to launch when CD is 
				inserted.
				<li>
					setup.exe - (optional) a copy of autorun.exe, renamed to setup.exe. (In the 
					settings utility you can specify that if the program name is 'setup.exe', skip 
					the splash screen.)</li>
			</ul>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Program Settings</b>
						</p>
						<p>
							The best way to get started is to use the sample layout included with the 
							download. Open 'Settings Utility.exe' and from the file menu, open the path to 
							the 'Sample CD' folder. The screen will look similar to the screen on the 
							right.
						</p>
						<UL>
							<li>
								<b>Name:</b>
							Specifies the name to use on dialogs and title bars
							<li>
								<b>Root:</b> Read only, simply displays the current root</li></UL>
						<P>&nbsp;</P>
					</td>
					<td width="225">
						<a href="image.aspx?url=Images/v300/program_settings_full.jpg"><img src="Images/v300/program_settings_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Components</b>
						</p>
						<P>Components are individual system component you would like checked. To skip 
							checking a component, uncheck it. The 'Sample CD' only includes folders for the 
							components, you must download each component yourself (if you choose to supply 
							the component from your own source, be SURE the version is the same - see 
							advanced docs).
							<BR>
							<br>
							Note:&nbsp; Several components require special downloading/extracting.&nbsp; 
							For details 'Edit' the component and go to the 'Notes' tab.&nbsp; [IE and 
							WinNT/2000 service packs require special attention]</P>
						<P>
							The component checking is fully customizable. If you decide to customize the 
							components, please read the <a href="docsv3component.aspx">component documentation</a>
							to fully understand everything involved.
							<br>
							<br>
						</P>
					</td>
					<td width="225">
						<a href="image.aspx?url=Images/v300/components_full.jpg"><img src="Images/v300/components_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Buttons</b>
						</p>
						<P>
							The buttons tab allows you to customize the number and type of buttons that 
							appear on your splash dialog.&nbsp; You can specify the button action (run 
							program, launch URL, launch file, or cancel), images (standard, mouseover, and 
							mouseclick), placement, and sound effects.&nbsp; On this tab you can also 
							specify which button is the default button and which is the cancel button (for 
							'enter' and 'esc' keys when the dialog is opened).
						</P>
						<P>For details on modifying buttons, read the <a href="docsv3button.aspx">button 
								documentation</a>.</P>
					</td>
					<td width="225">
						<a href="image.aspx?url=Images/v300/buttons_full.jpg"><img src="Images/v300/buttons_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Global Options</b>
						</p>
						<p>
							This tab allows you to set several misc. options.
						</p>
						<UL>
							<LI>
								<STRONG>Reboot Options</STRONG>
							- you can customize whether or not the reboot dialog is timed, and if so, its 
							timeout value
							<LI>
								<STRONG>Single Instance</STRONG>
							- if you only want one copy of autorun/setup to run at a time, check this 
							box.&nbsp; You can also set an error message to display if the program is run 
							when another instance is already running.
							<LI>
								<STRONG>User Defined Mutex</STRONG>
							- Chances are you know if you need this/want to use this feature.
							<LI>
								<STRONG>Enable Logging</STRONG> - Create a log file (ia.log) in the user's temp 
								directory. This is very useful for debugging.</LI></UL>
					</td>
					<td width="225">
						<a href="image.aspx?url=Images/v300/global_full.jpg"><img src="Images/v300/global_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>Splash</b>
						</p>
						<p>
							This tab allows you to control the splash dialog behavior and appearance
						</p>
						<UL>
							<LI>
								<STRONG>Dialog Options</STRONG>
							- whether or not to display the splash dialog
							<LI>
								<STRONG>Confirm Setup Run </STRONG>
							- Yes/No message box text to confirm running setup (or whatever program you 
							name it that skips the splash)
							<LI>
								<STRONG>Background</STRONG>
							- Bitmap image to use for the splash dialog
							<LI>
								<STRONG>Hide titlebar</STRONG>
							- will hide title bar and window border on splash dialog
							<LI>
								<STRONG>Sound Effects</STRONG> - OnOpen and OnClose sounds to be played as the 
								dialog is opened and closed</LI></UL>
					</td>
					<td width="225">
						<a href="image.aspx?url=Images/v300/splash_full.jpg"><img src="Images/v300/splash_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<hr>
			<table width="100%" cellpadding="4" cellspacing="0">
				<tr>
					<td valign="top">
						<p>
							<b>OS</b>
						</p>
						<p>
							This tab allows you to specify the OS requirements for IA to run.&nbsp; If run 
							on an unsupported OS, a message will be displayed to the user.&nbsp; You can 
							customize the message on this tab.
						</p>
					</td>
					<td width="225">
						<a href="image.aspx?url=Images/v300/os_full.jpg"><img src="Images/v300/os_thumb.jpg" border="0"></a>
					</td>
				</tr>
			</table>
			<P>&nbsp;</P>
			<P>&nbsp;</P>
			<P>
				<hr noshade>
			<P></P>
			<h3>Misc Notes</h3>
			<UL>
				<li>
				When a system component is installed, a registry entry is created 
				(HKCU\Software\Install Assistant\Installed Components). If the component fails 
				to install for some reason, the user is prompted if they would like to retry 
				installing the component.
				<li>
				You can override the 'logging' setting by specifying '/log' on the command line 
				when launching autorun.exe.
				<li>
					Clicking 'Cancel' on the progress dialog cancels setup AFTER the current 
					component finishes installing.</li></UL>
			<hr noshade>
			<P>
				To download now, click <a href="download.aspx">here</a><br>
				For the component documentation, click <a href="docsv3component.aspx">here</a>&nbsp;<BR>
				For the button documentation, click <a href="docsv3button.aspx">here</a>&nbsp;<BR>
				If you have any questions, feel free to ask from the <a href="feedback.aspx">feedback</a>
				page.
			</P>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
