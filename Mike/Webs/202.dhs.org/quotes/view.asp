<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Quotes"
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
Dim rst, strSQL, cnt, iCur, id, intStart, intEnd, intTotal
id = 0
intStart = 1
intEnd = 0
intTotal = 0
strSQL = "sp_QuoteList "
if Request("QuoteDateFrom") = "" then
	strSQL = strSQL + "null, "
else
	strSQL = strSQL + "'" + Request("QuoteDateFrom") + "', "
end if
if Request("QuoteDateTo") = "" then
	strSQL = strSQL + "null, "
else
	strSQL = strSQL + "'" + Request("QuoteDateTo") + "', "
end if
if Request("QuoteBy") = "" then
	strSQL = strSQL + "null"
else
	strSQL = strSQL + "'" + DQ(Request("QuoteBy")) + "'"
end if
set rst = Server.CreateObject("ADODB.Recordset")
rst.CursorLocation = adUseClient
rst.Open strSQL, cnWebDB, adOpenStatic, adLockReadOnly
rst.ActiveConnection = nothing

if Request("qty") = "All" then
	cnt = 10000000
elseif Request("qty") <> "" then
	cnt = CInt(Request("qty"))
else
	cnt = 10000000
end if

rst.MoveLast
intTotal = rst.RecordCount
rst.MoveFirst
if intEnd + cnt > intTotal then
	intEnd = intTotal
else
	intEnd = intEnd + cnt
end if

' go to sid if we have to
if Request("sid") <> "" then
	do while rst.Fields("QuoteID") <> CInt(Request("sid"))
		rst.MoveNext
		intStart = intStart + 1
		intEnd = intEnd + 1
	loop
end if

%>

<table class="clsNormalTable" align="center">
	<tr>
		<td>&nbsp;</td>
		<td align="center"><font class="clsNoteText">Quotes <%=intStart%> through <%=intEnd%> of <%=intTotal%><% if intTotal > intEnd then Response.Write("<br><i>Scroll to bottom for next page</i>")%></td>
		<td>&nbsp;</td>
	</tr>
</table>

<br>

<%

do while not rst.EOF And iCur < cnt
	iCur = iCur + 1
	ShowQuote rst.Fields("QuoteText"), rst.Fields("QuoteDate"), rst.Fields("QuoteLocation"), _
		rst.Fields("QuoteEnteredBy"), rst.Fields("QuoteBy"), rst.Fields("QuoteComment")
	rst.MoveNext
	Response.Write("<br>")
loop
if not rst.EOF then 
	id = rst.Fields("QuoteID")
else
	id = 0
end if
rst.Close
set rst = nothing

%>

<table border="0" cellpadding=4 align="center" width="450" class="clsNormalTable">
	<tr>
<% if Request("sid") <> "" then
			Response.Write("<td width=""150""><a href=""javascript:history.back()"">&lt; &lt; back</a></td>")
		else
			Response.Write("<td width=""150"">&nbsp;</td>")
 	 end if %>
		<td width="150" align="center"><a href="add.asp">Add Quote</a></td>
<% if Request("qty") <> "" and id <> 0 then %>
		<td width="150" align="right"><a href="view.asp?qty=<%=Request("qty")%>&sid=<%=id%>">next &gt; &gt;</a></td>
<% else %>
		<td width="150" align="center">&nbsp;</td>
<% end if %>
	</tr>
</table>

<br>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>