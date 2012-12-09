<%@ Page language="c#" Codebehind="Logout.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.Logout" %>
<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Logout" ShowUserInfo="false">
		</pics:header>
		<form runat="server" id="Login" method="post">
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
										You have been logged out.
									</p>
									<p>
										To log in again, click <a href="../">here</a>.
									</p>
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
