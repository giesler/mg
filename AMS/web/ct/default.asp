<%@ Language=VBScript %>
<%
Option Explicit
on error resume next
dim cn, strSQL
strSQL = "sp_webclickthrough '" & Request.QueryString("id") & "', "
strsql = strsql & "'" & Replace(Request.ServerVariables("HTTP_USER_AGENT"), "'", "''") & "', "
strsql = strsql & "'" & Replace(Request.ServerVariables("HTTP_REFERER"), "'", "''") & "', "
strsql = strsql & "'" & Replace(Request.ServerVariables("REMOTE_ADDR"), "'", "''") & "'"
set cn = Server.CreateObject("ADODB.Connection")
cn.Open Application("cnString")
cn.Execute strSQL
cn.close
set cn = nothing
Response.Redirect("../home/")
%>
