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
%>
<!-- #include file="../_common/gCommonCode.asp"-->
<!-- #include file="_quote.asp"-->
	
<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<%
dim strSQL

if Request.Form("confirmed") = "yes" then
	strSQL = "sp_QuoteAdd '" + DQ(Request("QuoteDate")) + "', "
	strSQL = strSQL + "'" + DQ(Request("QuoteBy")) + "', "
	strSQL = strSQL + "'" + DQ(Request("QuoteLocation")) + "', "
	strSQL = strSQL + "'" + DQ(Request("QuoteEnteredBy")) + "', "
	strSQL = strSQL + "'" + DQ(Request("QuoteText")) + "', "
	strSQL = strSQL + "'" + DQ(Request("QuoteComment")) + "'"
	cnWebDB.Execute strSQL
	
	Response.Write("<p>The quote below was succesfully added.</p><br>")
	ShowQuote Request("QuoteText"), Request("QuoteDate"), Request("QuoteLocation"), _
		Request("QuoteEnteredBy"), Request("QuoteBy"), Request("QuoteComment")
	Response.Write("<p><a href=""add.asp"">Add another quote</a> or return to ")
	Response.Write("<a href=""default.asp"">quote page</a></p>")
else

%>

<p>
The quote you entered will appear as shown below.  If it isn't right, click 'Back' 
to change it.  <b>Important</b>: You must click 'Add Quote' below to actually add the quote!<br>
</p>

<%
ShowQuote Request("QuoteText"), Request("QuoteDate"), Request("QuoteLocation"), _
	Request("QuoteEnteredBy"), Request("QuoteBy"), Request("QuoteComment")
%>



<form method="POST" action="submit.asp" id=form1 name=form1>
<input type="hidden" name="QuoteText" value="<%=Replace(Request.Form("QuoteText"), """", "&quot;")%>">
<input type="hidden" name="QuoteDate" value="<%=Request.Form("QuoteDate")%>">
<input type="hidden" name="QuoteLocation" value="<%=DQ(Request.Form("QuoteLocation"))%>">
<input type="hidden" name="QuoteEnteredBy" value="<%=DQ(Request.Form("QuoteEnteredBy"))%>">
<input type="hidden" name="QuoteBy" value="<%=DQ(Request.Form("QuoteBy"))%>">
<input type="hidden" name="QuoteComment" value="<%=DQ(Request.Form("QuoteComment"))%>">
<input type="hidden" name="confirmed" value="yes">
<table align="center" border=0>
<tr>
	<td><input type="submit" value="Add Quote" class="clsButton" id=submit1 name=submit1> <input type="button" value="Back" class="clsButton" onClick="javascript:history.back()" id=button1 name=button1>
</tr>
</table>
</form>

<%
end if
%>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>

