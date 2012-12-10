<%@ Page language="c#" Codebehind="WebForm1.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.WebForm1" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>WebForm1</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../msn2.css" type="text/css" rel="stylesheet">
		<LINK href="AuthStyles.css" type="text/css" rel="stylesheet">
	</head>
	<body MS_POSITIONING="GridLayout">
		<form id="WebForm1" method="post" runat="server">
			<picctls:ErrorMessagePanel id="pnlBadPassword" runat="server" title="Incorrect Password" visible="true"><B>
					The password you entered was not correct.</B> <BR>If you forgot your password, click 
<asp:HyperLink id="Hyperlink1" Runat="server">here</asp:HyperLink>&nbsp;to 
find out how to change it. <BR>
			</picctls:ErrorMessagePanel>
		</form>
	</body>
</html>
