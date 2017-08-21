<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "About VBSW"
mstrOffset		 = "../"
mstrArea			 = "about"
%>

<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->


<h3>The Author</h3>
<p>
Written by Mike Giesler, <a href="http://mike.giesler.org">http://mike.giesler.org</a>.<br>
Thanks to Nick Nystrom for Win32 help and graphics and such...
</p>

<br>

<h3>Distribution</h3>
<p>
Although this program is freeware, I would really appreciate donations.  If you are using this 
program in a 'for profit' business, I'd hope you would seriously consider contributing.  
(It would help motivate me to continue development on future versions.)  
Any donations can be sent to:  Mike Giesler - 16541 Redmond Way, Redmond, WA  98052 - USA<br>
If you link to this site, please link to the 'Home' page.  
</p>

<br>

<h3>Legal Info</h3>
<p>
Below simply says use at your own risk.<br>
<font size=-2>Everything on this site is provided free of charge, with no warranties, 
express or implied. The author accepts no responsibility for any damages or 
problems caused by the software on this site. Technical support for the products 
on this site is currently provided free of charge, but any and all details may 
change without prior notice. The owner of this site reserves the right to make 
changes, removals, and improvements to the site and the products contained on it 
without notice.<br></font>
</p>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>