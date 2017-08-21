<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="image.aspx.cs" AutoEventWireup="false" Inherits="vbsw.image" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Image</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<uc1:Header id="Header1" runat="server"></uc1:Header>
		<p><a href="javascript:history.back();">&lt;&lt;&nbsp;Back<br><br>
		<asp:Image Runat="server" ID="zoomImage" BorderWidth="0"></asp:Image>
		</a></p>
		<br><br><br><br><br><br><br><br>
		<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
	</body>
</HTML>
