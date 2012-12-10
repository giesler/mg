<%@ Page language="c#" Codebehind="docsv2adv.aspx.cs" AutoEventWireup="false" Inherits="vbsw.docsv2adv" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>docsv2adv</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</head>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="docsv2adv" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<h3>Advanced Documentation</h3>
			<p>
				'Components' can be customized to create an installation that meets your needs. 
				If you'd like to include different system components or different versions of 
				system components then those included with the sample, you can add/remove/edit 
				the component list.
			</p>
			<p>
				<b>Component Modification</b>
			</p>
			<p>
				You can add/edit/delete components on the 'Components' tab in the 'Settings 
				Utility'. When the VBSW program is run, every component that is has it's check 
				box checked will be verified. The order of the list is the order in which 
				components are checked.
			</p>
			<p>
				The components are checked as follows:<br>
				<ol>
					<li>
						Load component list, ignoring components that are unchecked in 'Settings 
						Utility'.</li>
					<li>
						Check whether each component is installed (see below for details on how this is 
						done. If installed, drop this component from 'to install' list. If VBSW 
						attempted to install this component before and it is still not installed the 
						user is prompted if they'd like to retry installing the component.
					</li>
					<li>
						Next, each component's dependencies are checked. If any dependencies are not 
						installed, the component is removed from the 'to install' list. (For example, 
						MDAC 2.6 depends on IE 4.01 SP2 and DCOM95 or greater to install, so MDAC 2.6 
						can't be installed until after these components)
					</li>
					<li>
						The final list is checked once more to see if any components require an 
						'Immediate Reboot'. The list is truncated at that component. (For example, an 
						'Immediate Reboot' is needed after Internet Explorer or an NT Service Pack is 
						installed.)</li>
					<li>
						After going through the list of components to install and installing the 
						components, if any component required a reboot, the computer is restarted, then 
						return to step 1.</li>
					<li>
						After all components have been installed/verified, your setup/msi is launched.</li>
				</ol>
			</p>
			<p>
				<b>Modifying A Component</b>
			</p>
			<p>
				<a href="Images/v200/comp/su-general-full.jpg"><img src="Images/v200/comp/su-general-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>General Settings</b><br />
				<ul>
					<li>
						<b>ID:</b> Short name of component</li>
					<li>
						<b>Name:</b> Full name of component, used in dialogs, etc.</li>
					<li>
						<b>URL:</b> Location of component on web for downloading (only currently used 
						by Settings Utility)</li>
					<li>
						<b>Component Check Type:</b>
						<ul>
							<li>
								<b>File Version Check:</b> Checks a file's version. If the version on the 
								target computer is greater then or equal to the version specified, the 
								component is not installed.</li>
							<li>
								<b>Registry Version Check:</b> Same as File Version Check except checks in a 
								registry key for the version number.</li>
							<li>
								<b>Registry Key Check:</b> Checks whether a registry key exists and matches the 
								given value. If so, assumes component is installed already.</li>
							<li>
								<b>NT Service Pack Check:</b> Checks for an NT Service Pack level</li>
						</ul>
					</li>
				</ul>
			</p>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-osreq-full.jpg"><img src="Images/v200/comp/su-osreq-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>OS Requirements</b><br />
				<ul>
					<li>
						<b>Min OS Version:</b> Minimum OS version required to check this component</li>
					<li>
						<b>Max OS Version:</b> Maximum OS version required to check this component.</li>
					<li>
						<b>Windows NT (NT4, 2000):</b> Check this component on Windows NT (or 2000 or 
						higher) computers.</li>
					<li>
						<b>Windows 9x (95, 98, Me):</b> Check this component on Windows 9x computers.</li>
				</ul>
				Notes: Specifying the same min and max OS versions will check only that 
				version. A value of 0 for both will check all versions, assuming both NT and 9x 
				are checked.
			</p>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-install-full.jpg"><img src="Images/v200/comp/su-install-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>Installation</b><br />
				<ul>
					<li>
						<b>Message:</b> Message displayed above progress bar during installation</li>
					<li>
						<b>Command:</b> Relative command line to install component.</li>
					<li>
						<b>CmdLine:</b> Any options you want passed to the command. The first word <b>must 
							be</b> the name of the EXE (for example, 'setup.exe /s').</li>
					<li>
						<b>Time:</b> The estimated time the installation will take in seconds. It is 
						best to overestimate, since when the time has elapsed, the progress bar will 
						stop moving.</li>
					<li>
						<b>Reboot:</b> The type of reboot required for this component.
						<ul>
							<li>
								<b>No Reboot Required:</b> After this component is installed, the computer does 
								not need to be restarted before running your setup program.</li>
							<li>
								<b>Batch Reboot:</b> After this component is installed, other components can be 
								installed, but the system must be restarted before running your setup program.</li>
							<li>
								<b>Immediate Reboot:</b> After this component is installed, the system must be 
								restarted before continuing setup.</li>
						</ul>
					</li>
				</ul>
			</p>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-depends-full.jpg"><img src="Images/v200/comp/su-depends-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>Dependencies</b><br />
				Check the components that must be installed before the current component. For 
				example, in order to install MDAC 2.6, Internet Explorer 4.01 SP2 or greater 
				and DCOM95 must be installed. To prevent this component from being installed 
				until the others have been verified, check IE and DCOM.
			</p>
			<br>
			<br>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-notes-full.jpg"><img src="Images/v200/comp/su-notes-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>Notes</b><br />
				Specify any special notes you may have about the component. Only used by the 
				Settings Utility.
			</p>
			<br>
			<br>
			<br>
			<br>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-filever-full.jpg"><img src="Images/v200/comp/su-filever-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>File Version Check</b><br />
				<ul>
					<li>
						<b>DLL:</b> Specify the DLL name to check. Use the displayed variables for 
						system paths.</li>
					<li>
						<b>Version:</b> Specify the minimum version of the DLL you will allow without 
						installing the component</li>
				</ul>
			</p>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-regver-full.jpg"><img src="Images/v200/comp/su-regver-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>Registry Version Check</b><br />
				<ul>
					<li>
						<b>Key:</b> Specify the registry key to check. You must specify the full name. 
						A trailing '\' will look at the 'Default' entry.</li>
					<li>
						<b>Version:</b> Specify the minimum version you will allow without installing 
						the component</li>
				</ul>
			</p>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-regkey-full.jpg"><img src="Images/v200/comp/su-regkey-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>Registry Key Check</b><br />
				<ul>
					<li>
						<b>Key:</b> Specify the registry key to check. You must specify the full name. 
						A trailing '\' will look at the 'Default' entry.</li>
					<li>
						<b>Value:</b> Specify the value that must be set to not install this component</li>
				</ul>
			</p>
			<br>
			<br>
			<br>
			<p>
				<a href="Images/v200/comp/su-ntservpack-full.jpg"><img src="Images/v200/comp/su-ntservpack-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="150"></a>
				<b>NT Service Pack Check</b><br />
				<ul>
					<li>
						<b>Service Pack:</b> Specify the full service pack name. Be sure to specify the 
						OS version on the OS Requirements tab.</li>
				</ul>
			</p>
			<br>
			<br>
			<hr noshade>
			<p>
				To download now, click <a href="download.aspx">here</a><br>
				If you have any questions, feel free to ask from the <a href="feedback.aspx">feedback</a>
				page.
			</p>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</html>
