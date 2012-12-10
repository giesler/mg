<%@ Page language="c#" Codebehind="about.aspx.cs" AutoEventWireup="false" Inherits="vbsw.about" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>about</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="ia.css">
	</HEAD>
	<body bgcolor="white" text="black" link="red" vlink="#ff0033" alink="aqua" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="download" method="post" runat="server">
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			<h3>The Author</h3>
			<p>
				Written by Mike Giesler, <a href="http://mike.giesler.org">http://mike.giesler.org</a>.<br>
				Thanks to Kevin L. for all the great feedback and testing!
			</p>
			<br>
			<h3>Postcard-ware</h3>
			<p>
				This program is free. However, if you'd like to encourage continued 
				development, and the fact you don't have to pay anything, send me a postcard. 
				Just a simple postcard is all - something from your area. Send to:
			</p>
			<blockquote> Mike Giesler<br>
				11111 109th Pl NE<br>
				Kirkland, WA 98033<br>
			</blockquote>
			<p>
				Other contributions are more then welcome to offset the cost of bandwidth, 
				domain registration, etc. Even simple <a href="feedback.aspx">feedback</a> on 
				what you like and don't like would be great.
			</p>
			<br>
			<h3>Legal Info / Disclaimer</h3>
			<P>
				Below simply says use at your own risk.<br>
				<font size="-2">Everything on this site is provided free of charge, with no 
					warranties, express or implied. The author accepts no responsibility for any 
					damages or problems caused by the software on this site. Technical support for 
					the products on this site is currently provided free of charge, but any and all 
					details may change without prior notice. The owner of this site reserves the 
					right to make changes, removals, and improvements to the site and the products 
					contained on it without notice. </font>
			</P>
			<P><FONT size="-2"></FONT>&nbsp;</P>
			<P><FONT size="-2">&nbsp;</P>
			<P>
				<br>
			</P>
			</FONT>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		</form>
	</body>
</HTML>
