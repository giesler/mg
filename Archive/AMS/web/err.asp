<%@ Language=VBScript %>
<%
Option Explicit
%>
<html>
<head>
<title>Adult Media Swapper</title>
<link rel="stylesheet" type="text/css" href="/ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">

<table width="100%" cellpadding="0" cellspacing="0" background="/img/top_wash.gif">
	<tr>
		<td width="108" rowSpan="2"><a href="/home/"><img src="/img/logo_wash.jpg" width="130" height="120" border="0"></a></td>
		<td width="500" height="100"><img src="/img/header.jpg" width="500" height="100"></td>
		<td align="right" height="100"><a href="/download/"><img src="/img/download_now.jpg" width="140" height="100" border="0"></a></td>
		<td width="5" height="100"><img src="/img/trans.gif" width="5" height="100"></td>
	</tr><tr>
		<td colspan="3" height="20" bgcolor="white"><img src="/imgage/trans.gif" width="5" height="5"></td>
	</tr>
</table>

<table width="100%" align="center" cellpadding="0" cellspacing="0">
	<tr>
		<td width="8" bgcolor="white"></td>
		<td width="125" bgcolor="white" valign="top">

			<table cellpadding="0" cellspacing="0" border="0">
			<tr><td><a class="clsMenu" href="/home/"><img border="0" src="/img/plus.gif" WIDTH="9" HEIGHT="9"></a>&nbsp;</td><td colspan="4"><a class="clsMenu" href="/home/">Home</a><br></td></tr><tr><td><a class="clsMenu" href="/about/"><img border="0" src="/img/plus.gif" WIDTH="9" HEIGHT="9"></a>&nbsp;</td><td colspan="4"><a class="clsMenu" href="/about/">About</a><br></td></tr><tr><td><a class="clsMenu" href="/download/"><img border="0" src="/img/plus.gif" WIDTH="9" HEIGHT="9"></a>&nbsp;</td><td colspan="4"><a class="clsMenu" href="/download/">Download</a><br></td></tr><tr><td><a class="clsMenu" href="/support/"><img border="0" src="/img/plus.gif" WIDTH="9" HEIGHT="9"></a>&nbsp;</td><td colspan="4"><a class="clsMenu" href="/support/">Support</a><br></td></tr><tr><td><a class="clsMenu" href="/account/"><img border="0" src="/img/plus.gif" WIDTH="9" HEIGHT="9"></a>&nbsp;</td><td colspan="4"><a class="clsMenu" href="/account/">Account&nbsp;Info</a><br></td></tr><tr><td><a class="clsMenu" href="/webmasters/"><img border="0" src="/img/plus.gif" WIDTH="9" HEIGHT="9"></a>&nbsp;</td><td colspan="4"><a class="clsMenu" href="/webmasters/">Webmasters</a><br></td></tr>
			</table>

			<br>
			<img src="img/trans.gif" height="100" width="1">
		</td>
		<td width="8" bgcolor="white"><img src="/img/trans.gif" height="8" width="8"></td>
		<td bgcolor="White" valign="top">
			<!-- main content -->

<%
dim strErrNo
if instr(Request.Querystring, ";") then
	strErrNo = Left(Request.Querystring, Instr(Request.Querystring, ";")-1)
end if
select case strErrNo
	case "404"
%>
<p class="req">The file or folder you requested was not found on the server.  Please use the links on 
the left to navigate to the file or folder you would like.</p>
<%
	case else
%>
<p class="req">An unknown error has occurred.  Please use the links on the lef tto navigate to the file or 
folder you would like.</p>
<%
end select
%>


		</td>
		<td width="10" bgcolor="white" background="img/trans.gif"></td>
	</tr>
</table>

<table width="100%" align="center" background="/img/bottom_wash.gif" cellpadding="0" cellspacing="0">
	<tr>
		<td width="10"><img src="/img/trans.gif" height="30" width="30"></td>
		<td align="right" valign="bottom">
			<font size="-2" color="Silver">© 2001, Line 2 Systems</font>
		</td>
		<td width="10"><img src="/img/trans.gif" width="10" height="10"></td>
	</tr>
</table>


</body>
</html>
