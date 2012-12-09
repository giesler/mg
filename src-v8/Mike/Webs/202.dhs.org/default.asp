<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Home Page"
mstrOffset		 = ""
%>
<!-- #include file="_common/gCommonCode.asp"-->
<!-- #include file="quotes/_quote.asp"-->

<html>
<head>
	<!--#include file="_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="_common/gPageHeader.asp"-->

<table width="100%" cellpadding="4" border="0">
	<tr>
		<td>
			<table align="center" border="0" cellpadding="4">
				<tr>
					<td valign="top" rowSpan="2"><a href="archives/"><img src="images/archives.gif" height="32" width="32" border="0"></a></td>
					<td valign="top"><font class="clsHeadingText"><a href="archives/">Archives</a></font></td>

					<td valign="top" rowSpan="2"><a href="people/"><img src="images/people.gif" height="32" width="32" border="0"></a></td>
					<td valign="top"><font class="clsHeadingText"><a href="people/">People</a></font></td>
				</tr><tr>
					<td valign="top">Old event web pages and the BAC calculator <a href="archives/default.asp">&gt;&gt;</a></td>
					<td valign="top">People of 202, including contact information <a href="people/default.asp">&gt;&gt;</a></td>
				</tr><tr>
					<td valign="top" rowSpan="2"><a href="email/"><img src="images/email.gif" height="32" width="32" border="0"></a></td>
					<td valign="top"><font class="clsHeadingText"><a href="email/">Email</a></font></td>

					<td valign="top" rowSpan="2"><a href="quotes/"><img src="images/quotes.gif" height="32" width="32" border="0"></a></td>
					<td valign="top"><font class="clsHeadingText"><a href="quotes/">Quotes</a></font></td>
				</tr><tr>
					<td valign="top">Send email to a 202 distribution list or select people <a href="email/default.asp">&gt;&gt;</a></td>
					<td valign="top">Quotes from 202 people.  You can add to them! <a href="quotes/default.asp">&gt;&gt;</a></td>
				</tr><tr>
					<td valign="top" rowSpan="2"><a href="pictures/"><img src="images/pictures.gif" height="32" width="32" border="0"></a></td>
					<td valign="top"><font class="clsHeadingText"><a href="pictures/">Pictures</a></font></td>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
				</tr><tr>
					<td valign="top">Pictures when 202 people got together <a href="pictures/default.asp">&gt;&gt;</a></td>
					<td valign="top">&nbsp;</td>
					<td valign="top">&nbsp;</td>
				</tr>
			</table>

			<hr noshade>

			<br>
<%
dim rst
set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "sp_QuoteRandom", cnWebDB, adOpenForwardOnly, adLockReadOnly
if not rst.bof and not rst.eof then
	ShowQuote rst.Fields("QuoteText"), rst.Fields("QuoteDate"), rst.Fields("QuoteLocation"), _
		rst.Fields("QuoteEnteredBy"), rst.Fields("QuoteBy"), rst.Fields("QuoteComment")
end if
rst.Close
set rst = Nothing
%>

		</td>
		<td valign="top">		
			<table class="clsNoteTable" width="200" align="right">
				<tr><td><center><font size="+1">What's New?</font></center><br>
					<b>Jan 13</b><br>Added pictures and quotes.  Fixed quote adding not working.  Sped up database connection stuff.  Disabled modifying list subscriptions and your email - due to moving - need something changed, email.<br><br>
					<b>Oct 1</b><br>Fixed a couple minor probs, added a random quote to the front page (try refreshing this page), and added quote counts in view quotes<br><br>
					<b>Sept 30</b><br>An updated look to the site, lots of new pictures, updated email section, new 'Change my Info' including list memberships, and other misc stuff.<br><br>
					<br><br><b><a href="new.asp">More &gt;&gt;</a></b>
				</td></tr>
			</table>
		</td>
	</tr>
</table>

<!--#include file="_common/gPageFooter.asp"-->

</body>
</html>
