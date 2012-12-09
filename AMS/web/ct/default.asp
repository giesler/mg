<%@ Language=VBScript %>
<%
Option Explicit
dim cn
set cn = Server.CreateObject("ADODB.Connection")
cn.Open Application("cnString")
Response.Write(cn.Execute("sp_webgetad '" & Request.ServerVariables("REMOTE_ADDR") & "'").Fields(0))
cn.close
set cn = nothing
%>
