<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Page language="c#" Codebehind="SearchRun.aspx.cs" AutoEventWireup="false" Inherits="pics.SearchRun" debug="true" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<%=RedirectHeader%>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<picctls:Header id="header" runat="server" size="small" Text="Searching..."></picctls:Header>
		<form id="Form1" method="post" runat="server">
			<table align="left" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>
						<!-- Main content -->
						<table width="100%">
							<tr>
								<td width="50">
									&nbsp;
								</td>
								<td align="left">
									<p>
									</p>
									<p>
									</p>
									<picctls:contentpanel id="welcomeMessage" runat="server" title="Welcome to the MSN2 pictures website!">
										<P>
											Please wait while we look through pictures...
										</P>
									</picctls:contentpanel>
									<P>
									</P>
								</td>
							</tr>
						</table>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
