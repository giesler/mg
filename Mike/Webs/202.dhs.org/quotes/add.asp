<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Quote Add Page"
mstrOffset		 = "../"
mstrArea			 = "quotes"
mstrOnLoad     = "document.frmAddQuote.QuoteDate.focus();"
%>
<!-- #include file="../_common/gCommonCode.asp"-->
<!-- #include file="_quote.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<!--#include file="../_common/cal/_calhead.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<br>

<center>
<form method="POST" action="submit.asp" id=frmAddQuote name=frmAddQuote>
<table border=0 align="middle" cellpadding=4 bgcolor=Black>
	<tr>
		<td>Date</td>
		<td><INPUT type="text" name="QuoteDate" value="<%=FormatDateTime(Date,vbShortDate)%>" class="clsInputBox" size=10>&nbsp;<A 
					href="javascript:ShowCalendar(document.frmAddQuote.imgTo, document.frmAddQuote.QuoteDate, null, -334, 0)" 
					onclick="event.cancelBubble=true;"><IMG
						align=top border=0 width=34 height=21 id=imgTo src="<%=mstrOffset%>_common/cal/calendar.gif" style="POSITION: relative"></A>
</td>
	</tr>
	<tr>
		<td>Who said it</td>
		<td><INPUT type="text" name=QuoteBy class="clsInputBox"></td>
	</tr>
	<tr>
		<td>Where was it</td>
		<td><INPUT type="text" name=QuoteLocation class="clsInputBox"></td>
	</tr>
	<tr>
		<td>Who heard it</td>
		<td><INPUT type="text" name=QuoteEnteredBy value="<%=Request.Cookies("UserLogin")%>" class="clsInputBox"></td>
	</tr>
	<tr>
		<td colspan=2>The Quote</td>
	</tr>
	<tr>
		<td colspan=2><TEXTAREA rows=4 cols=40 name=QuoteText class="clsInputBox"></TEXTAREA></td>
	</tr>
	<tr>
		<td colspan=2>Comments</td>
	</tr>
	<tr>
		<td colspan=2><TEXTAREA rows=4 cols=40 name=QuoteComment class="clsInputBox"></TEXTAREA></td>
	</tr>
	<tr>
		<td colspan=2 align="center">
			<input type="submit" value="Add Quote" class="clsButton">
			<input type="button" value="Cancel" class="clsButton" onClick="javascript:history.back()">
		</td>
	</tr>
</table>
</form>
</center>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>