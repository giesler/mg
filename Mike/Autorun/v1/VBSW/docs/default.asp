<%
dim sTitle, iOffset, sSubTitle
sTitle = "Documentation"
sSubTitle = "VB Setup Wrapper"
iOffset = 1

%>

<!-- #include file="../_common/header.inc"-->

<br>

<h3>Introduction</h3>

<p>
This document will describe how the VBSW program works, how to set up the directory 
structure it needs, and how to get help.
</p>

<p>
The VBSW v1.0 will install the system files and components needed by most VB programs.  It allows 
you to use either the Package & Deployment Wizard or Visual Studio Installer to install your 
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

<table border=0 cellpadding=3>
	<tr class=clsRptTop>
		<td>File</td>
		<td>Description</td>
		<td>Location</td>
		<td>Details</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\runtime\ie\ie5setup.exe</td>
		<td valign=top>IE 5.5 Install and associated files</td>
		<td valign=top><a href="http://download.microsoft.com/download/ie55/install/5.5/win98/en-us/ie5setup.exe">ie5setup.exe</a></td>
		<td valign=top>Download then run with the following command line to download components for all operating systems.<br>
								<path to ie5setup.exe>\ie5setup.exe /c:"ie5wzd.exe /d /s:""#E"""</td>
	</tr><tr class=clsRptBody2>
		<td valign=top>\runtime\dcom95.exe</td>
		<td valign=top>DCOM 95, required on Win95</td>
		<td valign=top><a href="http://download.microsoft.com/msdownload/dcom/95/x86/en/dcom95.exe">dcom95.exe</a></td>
		<td valign=top>&nbsp;</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\runtime\InstMsiA.exe</td>
		<td valign=top>Windows Installer for Win9x</td>
		<td valign=top><a href="http://download.microsoft.com/download/platformsdk/wininst/1.1/W9X/EN-US/InstMsi.exe">InstMsi.exe</a></td>
		<td valign=top>Download then rename to InstMsiA.exe)</td>
	</tr><tr class=clsRptBody2>
		<td valign=top>\runtime\InstMsiW.exe</td>
		<td valign=top>Windows Installer for WinNT</td>
		<td valign=top><a href="http://download.microsoft.com/download/platformsdk/wininst/1.1/NT4/EN-US/InstMsi.exe">InstMsi.exe</a></td>
		<td valign=top>Download then rename to InstMsiW.exe</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\runtime\mdac_typ.exe</td>
		<td valign=top>MDAC 2.5</td>
		<td valign=top><a href="http://www.microsoft.com/data/download_250rtm.htm" target=_blank>mdac_typ.exe</a></td>
		<td valign=top>Download page, select a language then download</td>
	</tr><tr class=clsRptBody2>
		<td valign=top>\runtime\VBRUN60.exe</td>
		<td valign=top>VB Runtime Installer 6.0</td>
		<td valign=top><a href="http://download.microsoft.com/download/vb60pro/Redist/sp4/win98/EN-US/VBRun60sp4.exe">vbrun60sp4.exe</a></td>
		<td valign=top>Download then rename to vbrun60.exe</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\setup\????</td>
		<td valign=top>???.MSI or setup.exe</td>
		<td valign=top>Your installation program</td>
		<td valign=top>Provided by you.  This may be either a Windows Installer package (.MSI) or VB Package and Deployment (setup.exe).
	</tr><tr class=clsRptBody2>
		<td valign=top>\autorun.exe</td>
		<td valign=top>Main program</td>
		<td valign=top>Included in Zip file</td>
		<td valign=top>Launched by autorun.inf when CD inserted.  Displays dialog with install and cancel button.</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\autorun.inf</td>
		<td valign=top>Settings for a CD root</td>
		<td valign=top>Included in Zip file</td>
		<td valign=top>Sets the icon for the CD and sets 'autorun.exe' to run when CD is inserted.  Must be at root of CD.</td>
	</tr><tr class=clsRptBody2>
		<td valign=top>\settings.ini</td>
		<td valign=top>Settings for Autorun</td>
		<td valign=top>Included in Zip file</td>
		<td valign=top>Allows you to set several settings for autorun.exe.  See comments in the file for details</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\setup.exe</td>
		<td valign=top>Main program without a startup dialog</td>
		<td valign=top>Included in Zip file</td>
		<td valign=top>Bypasses the dialog with splash.bmp</td>
	</tr><tr class=clsRptBody2>
		<td valign=top>\splash.bmp</td>
		<td valign=top>Bitmap image for autorun.exe</td>
		<td valign=top>Template included in Zip file</td>
		<td valign=top>Modify the template with a company logo, or whatever you want.</td>
	</tr><tr class=clsRptBody1>
		<td valign=top>\runtime\50comupd.exe<br><font color="red" size=-1><i>VBSW version 1.01</i></td>
		<td valign=top>Updates comctl32.dll with IE 5.0 version</td>
		<td valign=top><a href="http://download.microsoft.com/download/platformsdk/Comctl32/5.80.2614.3600/W9XNT4/EN-US/50comupd.exe">50comupd.exe</a></td>
		<td valign=top>To include in setup, you must change the line ComCtlDLL=0 to ComCtlDLL=1 in settings.ini</td>
	</tr>
</table>

<br>


<!-- #include file="../_common/footer.inc"-->
