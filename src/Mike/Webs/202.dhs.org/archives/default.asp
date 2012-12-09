<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Archives"
mstrOffset		 = "../"
mstrArea			 = "archives"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->


<h3>Turner House - More Beer</h3>
<ul>
	<li><a href="morebeer.mp3">In MP3 format (133 kb)</a></li>
</ul>


<h3>Old Event Invites</h3>
<ul>
  <li>

<a href="iframe.asp?url=events/homecoming.html">Homecoming Week, October 14, 1999</a></li>
  <li>
<a href="iframe.asp?url=events/finals.html">Finals/Binge Night, May 14, 1999</a></li>
  <li>
<a href="iframe.asp?url=events/beeramid.html">Beeramid Night, May 1, 1999</a></li>
  <li>
<a href="iframe.asp?url=events/StarNight.html">Star Night, April 29, 1999</a></li>
  <li>
<a href="iframe.asp?url=events/ghettonight.html">Ghetto Night, April 22, 1999</a></li>
  <li>
<a href="iframe.asp?url=events/OldPeopleNight.html">Old People Night, April 15, 1999</a></li>
  <li>
<a href="iframe.asp?url=events/SummerNight.html">Summer Night, April 8, 1999</a><br></li></ul>

<h3>Blood Alcohol Content Calculator</h3>
<blockquote><a href="bac.asp">Calculate</a> your BAC level</blockquote>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>
