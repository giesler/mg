<%@ Language=VBScript %>
<% Response.Buffer = false %>
<HTML>
<HEAD>
<title>Popup Help</title>
<link rel="stylesheet" type="text/css" href="styles.css">
</HEAD>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">
<!-- #include file="utils.asp"-->

<%
dim rs, sSQL
set rs = server.CreateObject("ADODB.Recordset")

if rq("Area") = "drug" then
	sSQL = "select Description, Help from drug where DrugID = " + CStr(rq("id"))
elseif rq("Area") = "TestResultSub" then
	sSQL = "select tr.TestResult + ' - ' + trs.TestResultSubName, trs.Help from TestResultSub trs, TestResult tr where tr.TestResultID = trs.TestResultID and trs.TestResultSubID = " + CStr(rq("id"))
elseif rq("Area") = "Help" then
	sSQL = "select HelpTitle, HelpText from Help where HelpID = " + CStr(rq("id"))
elseif rq("Area") = "RegDoc" then
	sSQL = "select DocumentName, Description from RegulatoryDocumentType where TypeID = " + CStr(rq("ID"))
else
	rw("Didn't recognize " + rq("Area"))
end if

rs.open sSQL, sConn
%>
<table class="clsWiz" align="center" width="100%" height="100%">
	<tr class="clsWizTop" align="center" height=5><td><font size=-1><%=rs.Fields(0)%></font></td>
	<tr class="clsWizBody1"><td valign=top><font size=-1><%=rs.fields(1)%></font></td>
	<tr class=clsWizBottom height=5>
		<td align=center colSpan=2><input type="button" value="Close" onClick="javascript:window.close()" id=button1 name=button1>
	</tr>
</table>

<%
rs.Close
set rs = nothing
%>

</BODY>
</HTML>
