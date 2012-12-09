<%@ Language=VBScript
     %>

<%
'Option Explicit
%>
<!--#include file="_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "VBSW Home"
mstrOffset		 = ""

if Request("email") <> "" then
	Response.Cookies("email") = Request("email")
	Response.Cookies("email").Expires = #January 1, 2002#
end if

%>
<!-- #include file="_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="_common/gPageHeader.asp"-->

<a href="images/v200/vbsw-autorun.jpg"><img src="images/v200/vbsw-autorun-sm.jpg" align="left" border="0" WIDTH="200" HEIGHT="150"></a>
<a href="images/v200/vbsw-setup.jpg"><img src="images/v200/vbsw-setup-sm.jpg" align="right" border="0" WIDTH="200" HEIGHT="74"></a>
<p>
Welcome to the homepage of the VB Setup Wrapper (VBSW).  VBSW acts as a 'wrapper' 
for your installation; it handles updating the system files your installation may 
require.  It is written in C, and can therefore be run on any Win32 version.  Check 
out the <a href="docs/">docs</a> then <a href="download/">download</a> the program.
</p>
<p><font size="-1">Click on the images to view full size images.</font></p>

<p>&nbsp;</p>

<table width="100%" <%=GetTTag%>>
	<tr>
		<td <%=GetTHeaderTag%> colSpan="2">News</td>
	</tr><tr>
		<td <%=GetTBodyTag(0)%>><b>January 14, 2001 - VBSW 2.00 Release</b></td>
		<td <%=GetTBodyTag(0)%>>
			Version 2 is now available!
		</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>><b>January 9, 2001 - VBSW 2.00 BETA</b></td>
		<td <%=GetTBodyTag(0)%>>
			This new release has a TON of new features.  Fully customizable autorun dialog (you can 
			specify a background and button images), fully customizable system component checking 
			portion, and a bunch of other 'under the hood' improvements.  For full details, check 
			the <a href="docs/">documentation</a>.
		</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>><b>August 19, 2000</b></td>
		<td <%=GetTBodyTag(0)%>>
			I am working on VBSW version 2.0. The new version should have the following features: 
			<ul>
				<li>Customizable system components - you can specify the DLL you want checked. 
					(So you can install different versions of MDAC, VB runtime, IE, etc. </li>
				<li>NT Service Pack level check and install</li>
				<li>Graphical buttons with OnMouseOver and OnMouseClick button images you can specify</li>
				<li>Sizable 'autorun' dialog</li>
				<li>Debugging mode which will specify components checked to a log file </li>
			</ul>
			If you have any suggestions feel free to let me know by sending 
			<a href="feedback/">feedback</a>. If you'd like to test it when ready, send 
			<a href="feedback/">feedback</a> as well, letting me know. When will it be ready? 
			Probably not for another month or so (or longer...).
		</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>><b>August 1, 2000 - VBSW Version 1.01</b></td>
		<td <%=GetTBodyTag(0)%>>
			<ul>
				<li>Fixed code to check for Windows Installer version 1.1 (was checking for 1.0)</li>
				<li>Added support for updating comctl32.dll (via 50comupd.exe - see <a href="docs/">docs</a>)</li>
			</ul>
		</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>><b>July 25, 2000</b></td>
		<td <%=GetTBodyTag(0)%>>
			I'm not going to promise future features, but I am working on another version 
			that will allow you to specify, in an INI file, all the system components and 
			corresponding versions you would like updated. ETA? I'm not sure. 
		</td>
	</tr>
</table>

<table width="100%">
	<tr>
		<td align="center" width="50%"><a href="http://msdn.microsoft.com"><img src="images/msdn-logo.gif" border="0" WIDTH="80" HEIGHT="40"></a></td>
		<td align="center" width="50%"><a href="http://vbwire.com/"><img src="images/vbwire.gif" border="0" WIDTH="100" HEIGHT="30"></a></td>
	</tr>
</table>

<!--#include file="_common/gPageFooter.asp"-->

</body>
</html>
