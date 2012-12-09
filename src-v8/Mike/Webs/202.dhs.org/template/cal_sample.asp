<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gIncludes.asp"-->
<% 
' Page level variables
mstrPageTitle = "Template Page"
mstrArea      = "default"
mstrOffset		= "../"
%>

<OBJECT RUNAT=Server PROGID=SAMIWeb.Session ID=mobjSession> </OBJECT>

<HTML>
<HEAD>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<!--#include file="../_common/cal/_calhead.asp"-->
	<TITLE><%=mstrPageTitle%></TITLE>	
</HEAD>
<BODY <%=gstrBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<form name="frmSearch" id="frmSearch">
Date:<BR>

<input type="text" name="CollectionDateTo" size="8" value="<%=strCollectionDateTo%>">&nbsp;<A 
					href="javascript:ShowCalendar(document.frmSearch.imgTo, document.frmSearch.CollectionDateTo, null, -334, 0)" 
					onclick="event.cancelBubble=true;"><IMG
						align=top border=0 width=34 height=21 id=imgTo src="<%=mstrOffset%>_common/cal/calendar.gif" style="POSITION: relative">
				</A>
</form>		

<!--#include file="../_common/gPageFooter.asp"-->

</BODY>
</HTML>
