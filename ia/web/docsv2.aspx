<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="docsv2.aspx.cs" AutoEventWireup="false" Inherits="vbsw.docsv2" %>
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
			Docs: <a href="docs.aspx">Version 3</a> | <b>Verison 2</b> | <a href="docsv1.aspx">Verison 1</a>
			</p>

			<H3>Introduction</H3>
			<p>
				This documentation will describe how the VBSW program works, how to set up the 
				directory structure, and then how to customize the program to meet your needs.
			</p>
			<p>
				VBSW was orignally designed to install system related files for VB6 programs. 
				It will restart the target computer as needed and resume installation. Once all 
				system files needed have been updated, your setup program / Windows Installer 
				package will run.
			</p>
			<h3>Steps</h3>
			<ol>
				<li>
				Read this entire page
				<li>
					<a href="../download/">Download</a>
				VBSW.
				<li>
				Using the included 'VBSW Settings Utility', customize the included installation 
				or create a new installation.
				<li>
				Download the components your installation needs, extracting any files that need 
				to be extracted.
				<li>
					Deploy by CD or network share.</li>
			</ol>
			<hr noshade>
			<h3>VBSW Settings Utility</h3>
			<img src="Images/v200/folderlist.jpg" align="right" WIDTH="128" HEIGHT="145"> The 
			VBSW Settings Utility allows you to customize your installation. This utility 
			requires the VB6 runtime files, and IE4.0 or higher (ONLY the utility has these 
			requirements!). A basic installation has the directory structure shown to the 
			right.
			<ul>
				<li>
				\setup or \msi - contains either the 'setup.exe' or 'Intall Package.msi' for 
				your installation
				<li>
				\sysfiles - contains installs for system components to update
				<li>
				\vbsw - contains the settings file (settings.ini) and images used by 
				Autorun.exe
				<li>
				Autorun.exe - Main program (VBSW)
				<li>
				Autorun.inf - When used at CD root, causes autorun.exe to launch when CD 
				inserted.
				<li>
					setup.exe - (optional) a copy of autorun.exe, renamed to setup.exe. Launching 
					this program skips the 'Autorun' dialog.</li>
			</ul>
			<p>
				<b>Program Settings</b>
			</p>
			<p>
				<a href="Images/v200/su-progset-full.jpg"><img src="Images/v200/su-progset-sm.jpg" align="right" border="0" WIDTH="300" HEIGHT="271"></a>
				The best way to get started is to use the sample layout included with the 
				download. Open 'Settings Utility.exe' and from the file menu, open the path to 
				the sample 'CD Root'. The screen will look similiar to the screen on the right.
			</p>
			<ul>
				<li>
					<b>Name:</b>
				Specifies the name to use on dialogs and title bars
				<li>
					<b>Setup/MSI:</b>
				Specifies the setup.exe or .msi package to install after the system components 
				have been updated.
				<li>
					<b>Cmd Line:</b>
				Any command line options you want to pass to your installation. For an EXE 
				install, you must also specify the EXE name here, for example 'setup.exe /x'. 
				For an MSI, simply specify the options, for example '/j'.
				<li>
					<b>Root:</b>
				Read only, simply displays the current root
				<li>
					<b>Background:</b> Bitmap image used as the background for the autorun dialog. 
					The autorun dialog will automatically resize to the size of the bitmap image 
					you specify.</li>
			</ul>
			<p>
				<b>Components</b>
			</p>
			<p>
				<a href="Images/v200/su-components-full.jpg"><img src="Images/v200/su-components-sm.jpg" align="right" border="0" WIDTH="300" HEIGHT="271"></a>
				Components are each individual system component you would like checked. To skip 
				checking a component, uncheck it. The sample CD Root only includes folders for 
				the components, you must download each component yourself (if you choose to 
				supply the component from your own source, be SURE the version is the same - 
				see advanced docs).
				<br>
				Note: The two service pack installs require you to extract the downloaded 
				files, see 'Notes' after double clicking the component.
				<br>
			</p>
			<p>
				You can fully customize the component checking. If you decide to customize the 
				components please read the <a href="docsv2adv.aspx">advanced documentation</a> to 
				fully understand everything involved.
				<br>
				<br>
				<br>
			</p>
			<p>
				<b>Buttons</b>
			</p>
			<p>
				<a href="Images/v200/su-buttons-full.jpg"><img src="Images/v200/su-buttons-sm.jpg" align="right" border="0" WIDTH="300" HEIGHT="271"></a>
				The buttons tab allows you to specify bitmap images to use for the 'Install' 
				button and the 'Cancel' button. The images should all be the same size. If 
				you'd like different images for when the mouse is moved over the image, or when 
				the button is clicked, simply specify the images you would like to use. You 
				must also set the positioning for the images relative to the upper left corner 
				of the background image specified in 'Program Settings'.
			</p>
			<hr noshade>
			<h3>Misc Other Notes</h3>
			<ul>
				<li>
				VBSW creates a log file (VBSW Log &lt;date/time&gt;.log) each time it is run in 
				the user's Temp directory. This is a good point to start any troubleshooting.
				<li>
				When a system component is installed, a registry entry is created 
				(HKCU\Software\giesler.org\VBSW\Installed Components). If the component fails 
				to install for some reason, the user is prompted if they would like to retry 
				installing the component.
				<li>
				Clicking 'Cancel' on the progress dialog cancels setup AFTER the current 
				component finishes installing.
				<li>
					When a reboot is required, a dialog that times out after 15 seconds and 
					automatically restarts is displayed.</li>
			</ul>
			<hr noshade>
			<p>
				To download now, click <a href="download.aspx">here</a><br>
				For the advanced documentation, click <a href="docsv2adv.aspx">here</a>.<br>
				If you have any questions, feel free to ask from the <a href="feedback.aspx">feedback</a>
				page.
			</p>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
