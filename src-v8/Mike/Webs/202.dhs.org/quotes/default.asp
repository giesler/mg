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

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<br>

<form action="view.asp" method="POST">
<table align="center" cellpadding=10>
	<tr>
		<td valign="top"><font class="clsHeadingText"><a href="add.asp">Add</a></font><br>
			Add a new quote you heard or heard about. <a href="add.asp">&gt;&gt;</a></td>
	</tr><tr>
		<td valign="top"><font class="clsHeadingText"><a href="view.asp?qty=5">View</a></font><br>
			View the quotes that have been entered. <a href="view.asp">&gt;&gt;</a><br><br>
			<table class="clsNormalTable" align="center" cellpadding=4>
				<tr>
					<td>Show 
						<select name="qty">
							<option value="5">5</option>
							<option value="10">10</option>
							<option value="20">20</option>
							<option value="All">All</option>
						</select> per page.
					</td>
				</tr><tr>
					<td>Show quotes by: <input type="text" name="QuoteBy" size="10">
				</tr><tr>
					<td align="center"><input type="submit" value="View" class="clsButton"></td>
				</tr>
			</table>
			</td>
	</tr>
</table>
</form>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>