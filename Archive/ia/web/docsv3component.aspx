<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="docsv3component.aspx.cs" AutoEventWireup="false" Inherits="vbsw.docsv3component" %>
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
			<h3>Component Documentation</h3>
			<p>
				'Components' can be customized to create an installation that meets your needs. 
				If you'd like to include different system components or different versions of 
				system components then those included with the sample, you can add/remove/edit 
				the component list.
			</p>
			<p>
				You can add/edit/delete components on the 'Components' tab in the 'Settings 
				Utility'. When the IA program is run, every component that is checked will be verified. The order of the list is the order in which 
				components are checked.
			</p>
			<p>
				The components are checked as follows:<br>
				<ol>
					<li>
					Load component list, ignoring components that are unchecked in 'Settings 
					Utility'.
					<li>
					Check whether each component is installed (see below for details on how this is 
					done. If installed, drop this component from 'to install' list. If IA attempted 
					to install this component before and it is still not installed the user is 
					prompted if they'd like to retry installing the component.
					<li>
					Next, each component's dependencies are checked. If any dependencies are not 
					installed, the component is removed from the 'to install' list. (For example, 
					MDAC 2.6 depends on IE 4.01 SP2 and DCOM95 or greater to install, so MDAC 2.6 
					can't be installed until after these components)
					<li>
					The final list is checked once more to see if any components require an 
					'Immediate Reboot'. The list is truncated at that component. (For example, an 
					'Immediate Reboot' is needed after Internet Explorer or an NT Service Pack is 
					installed.)
					<li>
					If any of the components on the install list required a computer reboot, the user 
					is prompted to restart and when Windows start, IA starts over at step 1.
					<li>
						After all components have been installed/verified, your setup/msi is launched.</li>
				</ol>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>General Settings</b><br>
								<ul>
									<li>
										<b>Name:</b>
									Full name of component, used in dialogs, etc.
									<li>
										<b>URL:</b>
									Location of component on web for downloading (only currently used by Settings 
									Utility)
									<li>
										<b>Component Check Type:</b>
										<ul>
											<li>
												<b>File Version Check:</b>
											Checks a file's version. If the version on the target computer is greater then 
											or equal to the version specified, the component is not installed.
											<li>
												<b>Registry Version Check:</b>
											Same as File Version Check except checks in a registry key for the version 
											number.
											<li>
												<b>Registry Key Check:</b>
											Checks whether a registry key exists and matches the given value. If so, 
											assumes component is installed already.
											<li>
												<b>NT Service Pack Check:</b>
											Checks for an NT Service Pack level
											<LI>
												<STRONG>.Net Framework Version Check</STRONG>:<STRONG>&nbsp; </STRONG>Check for 
												a specific version of the .Net framework</LI>
										</ul>
									</li>
								</ul>
							<P></P>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/general_settings_full.jpg"><img src="Images/v300/component/general_settings_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>OS Requirements</b><br>
							<P>Specify whatever OS this component can be installed on with this tab.</P>
							<P>Notes: Specifying the same min and max OS versions will check only that version. 
								A value of 0 for both will check all versions, assuming both NT and 9x are 
								checked.
							</P>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/os_req_full.jpg"><img src="Images/v300/component/os_req_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>Installation</b><br>
								<ul>
									<li>
										<b>Message:</b>
									Message displayed above progress bar during installation
									<li>
										<b>Command:</b>
									Relative command line to install component.
									<li>
										<b>CmdLine:</b> Any options you want passed to the command. The first word <b>must 
											be</b>
									the name of the EXE (for example, 'setup.exe /s').
									<li>
										<b>Time:</b>
									The estimated time the installation will take in seconds. It is best to 
									overestimate, since when the time has elapsed, the progress bar will stop 
									moving.
									<li>
										<b>Reboot:</b> The type of reboot required for this component.
										<ul>
											<li>
												<b>No Reboot Required:</b>
											After this component is installed, the computer does not need to be restarted 
											before running your setup program.
											<li>
												<b>Batch Reboot:</b>
											After this component is installed, other components can be installed, but the 
											system must be restarted before running your setup program.
											<li>
												<b>Immediate Reboot:</b> After this component is installed, the system must be 
												restarted before continuing setup.</li>
										</ul>
									</li>
								</ul>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/installation_full.jpg"><img src="Images/v300/component/installation_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>Dependencies</b><br>
								Check the components that must be installed before the current component. For 
								example, in order to install MDAC 2.6, Internet Explorer 4.01 SP2 or greater 
								and DCOM95 must be installed. To prevent this component from being installed 
								until the others have been verified, check IE and DCOM.
							</p>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/dependencies_full.jpg"><img src="Images/v300/component/dependencies_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>Notes</b><br>
								Specify any special notes you may have about the component. Only used by the 
								Settings Utility.
							</p>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/notes_full.jpg"><img src="Images/v300/component/notes_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>File Version Check</b><br>
								<ul>
									<li>
										<b>DLL:</b>
									Specify the DLL name to check. Use the displayed variables for system paths.
									<li>
										<b>Version:</b> Specify the minimum version of the DLL you will allow without 
										installing the component</li>
								</ul>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/file_ver_check_full.jpg"><img src="Images/v300/component/file_ver_check_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>Registry Version Check</b><br>
								<ul>
									<li>
										<b>Key:</b>
									Specify the registry key to check. You must specify the full name. A trailing 
									'\' will look at the 'Default' entry.
									<li>
										<b>Version:</b> Specify the minimum version you will allow without installing 
										the component</li>
								</ul>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/reg_ver_check_full.jpg"><img src="Images/v300/component/reg_ver_check_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>Registry Key Check</b><br>
								<ul>
									<li>
										<b>Key:</b>
									Specify the registry key to check. You must specify the full name. A trailing 
									'\' will look at the 'Default' entry.
									<li>
										<b>Value:</b> Specify the value that must be set to not install this component</li>
								</ul>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/reg_key_check_full.jpg"><img src="Images/v300/component/reg_key_check_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>NT Service Pack Check</b><br>
								<ul>
									<li>
										<b>Service Pack:</b> Specify the full service pack name. Be sure to specify the 
										OS version on the OS Requirements tab.</li>
								</ul>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/nt_serv_pack_check_full.jpg"><img src="Images/v300/component/nt_serv_pack_check_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr>
				<table width="100%" cellpadding="4" cellspacing="0">
					<tr>
						<td valign="top">
							<p>
								<b>.Net Framework Check</b><br>
								<ul>
									<li>
										<b>Version:</b> Specify the full version of the .Net framework you'd like to 
										check for.</li>
								</ul>
						<td width="250">
							<a href="image.aspx?url=Images/v300/component/net_framework_full.jpg"><img src="Images/v300/component/net_framework_thumb.jpg" border="0"></a>
						</td>
					</tr>
				</table>
				<hr noshade>
			<p>
				To download now, click <a href="download.aspx">here</a><br>
				To view the button documentation, click <a href="docsv3button.aspx">here</a>.<br>
				To return to the main documentation, click <a href="docs.aspx">here</a>.<br>
				If you have any questions, feel free to ask from the <a href="feedback.aspx">feedback</a>
				page.
			</p>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
