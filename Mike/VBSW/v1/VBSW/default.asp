<%
dim sTitle, iOffset, sSubTitle
sTitle = "Home"
sSubTitle = "VB Setup Wrapper"
iOffset = 0

%>

<!-- #include file="_common/header.inc"-->

<table cellpadding="7">
	<tr>
		<td><img align="left" src="autorun.jpg" WIDTH="200" HEIGHT="109"></td>
		<td>Welcome to the homepage of the VB Setup Wrapper (VBSW).  VBSW acts as a 'wrapper' to your 
installation; it handles updating the system files your installation may require.  It is 
written in C++, and will run on any Win32 version (Win95a, Win95b, Win98, WinMe, WinNT, Win2k) 
Best of all it is completely <i>free</i>! 
Check out the <a href="docs/">docs</a>, then <a href="download/">download</a> the program.
	</td>
		<td><img src="pbar.jpg" WIDTH="188" HEIGHT="70"></td>
	</tr>
</table>

<hr noshade>

<p>
<b>January 5, 2000</b><br>
Still working on version 2.  I got busy with work and other things over the past.... long time, and 
didn't have time to work on it.  I'm not sure an ETA on a final release, though I hope to have 
an alpha/beta release sometime this month.
</p>

<p>
<b>August 19, 2000</b><br>
I am working on VBSW version 2.0.  The new version should have the following features:
<ul>
	<li>Customizable system components - you can specify the DLL you want checked. (So you can install different versions of MDAC, VB runtime, IE, etc.</li>
	<li>NT Service Pack level check and install</li>
	<li>Graphical buttons with OnMouseOver and OnMouseClick button images you can specify</li>
  <li>Sizable 'autorun' dialog</li>
	<li>Debugging mode which will specify components checked to a log file</li>
</ul>
If you have any suggestions feel free to let me know by sending <a href="feedback/">feedback</a>.  If you'd like to test it when ready, send <a href="feedback/">feedback</a> as well, letting me know. 
When will it be ready?  Probably not for another month or so.
</p>

<p>
<b>August 1, 2000 - VBSW Version 1.01</b>
<ul>
	<li>Fixed code to check for Windows Installer version 1.1 (was checking for 1.0)</li>
	<li>Added support for updating comctl32.dll (via 50comupd.exe - see <a href="docs/">docs</a>)</li>
</ul>
</p>

<p>
<b>August 1, 2000</b><br>
I apologize if you were trying to access this site between 7 am and 9:30 am CST today.  The site 
was down because my power was out.
</p>

<p>
<b>July 25, 2000</b><br>
I'm not going to promise future features, but I am working on another version that will allow 
you to specify, in an INI file, all the system components and corresponding versions you would 
like updated.  ETA?  I'm not sure.
</p>

<hr noshade>

<br><br><br>

<center>
<table>
	<tr>
		<td colSpan="3" align="center"><b><font size="-1">VB Links</font></b></td>
	</tr><tr>
		<td align="center"><a href="http://msdn.microsoft.com/default.asp" target="_blank"><img src="_common/image/msdn-logo.gif" border="0" WIDTH="80" HEIGHT="40"></a></td>
		<td>&nbsp;</td>
		<td align="center"><a href="http://www.vbwire.com" target="_blank"><img src="_common/image/vbwire.gif" border="0" WIDTH="100" HEIGHT="30"></a>
	</tr>
</table>
</center>

<!-- #include file="_common/footer.inc"-->
