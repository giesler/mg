<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "VBSW Documentation (v1.0x)"
mstrOffset		 = "../"
mstrArea			 = "docs"
%>

<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<table class="clsNoteTable" align="center">
	<tr>
		<td <%=GetTBodyTag(0)%>><img src="<%=mstrOffset%>images/warn.gif"></td>
		<td <%=GetTBodyTag(0)%>>This is beta documentation for version 2.00!<br>See the sidebar for past documentation.</td>
	</tr>
</table>


<br>

<h3>Introduction</h3>

<p>
This document will describe how the VBSW program works, how to set up the directory 
structure it needs, and how to get help.
</p>

<p>
The VBSW v1.0 will install the system files and components needed by most VB programs.  It allows 
you to use either the Package &amp; Deployment Wizard or Visual Studio Installer to install your 
VB program.  The program checks for the following components by default:
<ul>
	<li><b>Internet Explorer 3.02</b> - Required by MDAC 2.5</li>
	<li><b>DCOM 95</b> - Required by MDAC 2.5</li>
	<li><b>MDAC 2.5</b> - Required for any data related apps</li>
	<li><b>VB 6.0 SP4 Runtime Files</b> - Required for any VB app</li>
	<li><b>Windows Installer</b> - Required to use the Visual Studio Installer</li>
</ul>	
Any components requiring updating are updated automatically and the user is prompted to reboot if required 
the computer.  VBSW then continues when the computer is started again.  Once all system files have been updated, 
your setup program is started.
</p>

<br>

<h3>Steps</h3>

<p>
<ol>
	<li>Read this entire page.</li>
	<li>Download the VBSW zip file</li>
	<li>Download the components required for your installation - the links are below.  Place files appropriately in the directory structure</li>
	<li>Add your installation program to \setup</li>
	<li>Customize your splash.bmp and settings.ini</li>
	<li>Deploy by CD or network share</li>
</ol>

<br>

<h3>Directory Structure</h3>

<p>
The below structure is required for this version of VBSW, assuming you want to 
include all dependencies.  Click each file to jump to more information about the file.
</p>

<table <%=GetTTag%>>
	<tr <%=GetTHeaderTag%>>
		<td>File</td>
		<td>Description</td>
		<td>Location</td>
		<td>Details</td>
	</tr><tr>
		<td <%=GetTBodyTag(0)%>>\runtime\ie\ie5setup.exe</td>
		<td <%=GetTBodyTag(0)%>>IE 5.5 Install and associated files</td>
		<td <%=GetTBodyTag(0)%>><a href="http://download.microsoft.com/download/ie55/install/5.5/win98/en-us/ie5setup.exe">ie5setup.exe</a></td>
		<td <%=GetTBodyTag(0)%>>Download then run with the following command line to download components for all operating systems.<br>
								&lt;path to ie5setup.exe&gt;\ie5setup.exe /c:&quot;ie5wzd.exe /d /s:&quot;&quot;#E&quot;&quot;&quot;</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\runtime\dcom95.exe</td>
		<td <%=GetTBodyTag(0)%>>DCOM 95, required on Win95</td>
		<td <%=GetTBodyTag(0)%>><a href="http://download.microsoft.com/msdownload/dcom/95/x86/en/dcom95.exe">dcom95.exe</a></td>
		<td <%=GetTBodyTag(0)%>>&nbsp;</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\runtime\InstMsiA.exe</td>
		<td <%=GetTBodyTag(0)%>>Windows Installer for Win9x</td>
		<td <%=GetTBodyTag(0)%>><a href="http://download.microsoft.com/download/platformsdk/wininst/1.1/W9X/EN-US/InstMsi.exe">InstMsi.exe</a></td>
		<td <%=GetTBodyTag(0)%>>Download then rename to InstMsiA.exe)</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\runtime\InstMsiW.exe</td>
		<td <%=GetTBodyTag(0)%>>Windows Installer for WinNT</td>
		<td <%=GetTBodyTag(0)%>><a href="http://download.microsoft.com/download/platformsdk/wininst/1.1/NT4/EN-US/InstMsi.exe">InstMsi.exe</a></td>
		<td <%=GetTBodyTag(0)%>>Download then rename to InstMsiW.exe</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\runtime\mdac_typ.exe</td>
		<td <%=GetTBodyTag(0)%>>MDAC 2.5</td>
		<td <%=GetTBodyTag(0)%>><a href="http://www.microsoft.com/data/download_250rtm.htm" target="_blank">mdac_typ.exe</a></td>
		<td <%=GetTBodyTag(0)%>>Download page, select a language then download</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\runtime\VBRUN60.exe</td>
		<td <%=GetTBodyTag(0)%>>VB Runtime Installer 6.0</td>
		<td <%=GetTBodyTag(0)%>><a href="http://download.microsoft.com/download/vb60pro/Redist/sp4/win98/EN-US/VBRun60sp4.exe">vbrun60sp4.exe</a></td>
		<td <%=GetTBodyTag(0)%>>Download then rename to vbrun60.exe</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\setup\????</td>
		<td <%=GetTBodyTag(0)%>>???.MSI or setup.exe</td>
		<td <%=GetTBodyTag(0)%>>Your installation program</td>
		<td <%=GetTBodyTag(0)%>>Provided by you.  This may be either a Windows Installer package (.MSI) or VB Package and Deployment (setup.exe).
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\autorun.exe</td>
		<td <%=GetTBodyTag(0)%>>Main program</td>
		<td <%=GetTBodyTag(0)%>>Included in Zip file</td>
		<td <%=GetTBodyTag(0)%>>Launched by autorun.inf when CD inserted.  Displays dialog with install and cancel button.</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\autorun.inf</td>
		<td <%=GetTBodyTag(0)%>>Settings for a CD root</td>
		<td <%=GetTBodyTag(0)%>>Included in Zip file</td>
		<td <%=GetTBodyTag(0)%>>Sets the icon for the CD and sets 'autorun.exe' to run when CD is inserted.  Must be at root of CD.</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\settings.ini</td>
		<td <%=GetTBodyTag(0)%>>Settings for Autorun</td>
		<td <%=GetTBodyTag(0)%>>Included in Zip file</td>
		<td <%=GetTBodyTag(0)%>>Allows you to set several settings for autorun.exe.  See comments in the file for details</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\setup.exe</td>
		<td <%=GetTBodyTag(0)%>>Main program without a startup dialog</td>
		<td <%=GetTBodyTag(0)%>>Included in Zip file</td>
		<td <%=GetTBodyTag(0)%>>Bypasses the dialog with splash.bmp</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\splash.bmp</td>
		<td <%=GetTBodyTag(0)%>>Bitmap image for autorun.exe</td>
		<td <%=GetTBodyTag(0)%>>Template included in Zip file</td>
		<td <%=GetTBodyTag(0)%>>Modify the template with a company logo, or whatever you want.</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>\runtime\50comupd.exe<br><font color="red" size="-1"><i>VBSW version 1.01</i></td>
		<td <%=GetTBodyTag(0)%>>Updates comctl32.dll with IE 5.0 version</td>
		<td <%=GetTBodyTag(0)%>><a href="http://download.microsoft.com/download/platformsdk/Comctl32/5.80.2614.3600/W9XNT4/EN-US/50comupd.exe">50comupd.exe</a></td>
		<td <%=GetTBodyTag(0)%>>To include in setup, you must change the line ComCtlDLL=0 to ComCtlDLL=1 in settings.ini</td>
	</tr>
</table>

<br>
<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>