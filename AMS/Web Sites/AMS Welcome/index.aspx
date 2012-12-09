<%@ Register TagPrefix="AMSWelcome" TagName="AMSFeatured" Src="_Featured.ascx" %>
<%@ Register TagPrefix="AMSWelcome" TagName="AMSNews" Src="_News.ascx" %>
<%@ Register TagPrefix="AMSWelcome" TagName="AMSStats" Src="_Stats.ascx" %>
<%@ Page language="c#" Codebehind="index.aspx.cs" AutoEventWireup="false" Inherits="AMS_Welcome.index" EnableSessionState="False" enableViewState="False"%>
<!doctype html public "-//w3c//dtd html 4.0 transitional//en" >
<html>
	<head>
		<title>Adult Media Swapper</title>
		<link rel="stylesheet" type="text/css" href="ams.css">
	</head>
	<body topmargin="0" leftmargin="0" link="#e22000" vlink="#bf0400" alink="#ef1c19" rightmargin="0" bottommargin="0">
		<table width="100%" cellpadding="0" cellspacing="0" background="img/welcome_top_wash.gif" bgcolor="white" height="60">
			<tr>
				<td width="65" rowspan="2" height="60" valign="top">
					<a href="http://www.adultmediaswapper.com" target="_blank"><img src="img/welcome_ams_logo.gif" width="65" height="60" border="0"></a>
				</td>
				<td width="250" height="50">
					<img src="img/welcome_ams_header.gif" width="250" height="50">
				</td>
				<td height="50">
					<img src="img/trans.gif">
				</td>
			</tr>
			<tr>
				<td colspan="2" height="10" bgcolor="white">
					<img src="img/trans.gif" height="2" width="2">
				</td>
			</tr>
		</table>
		<table cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
			<tr>
				<td valign="top">
					<table width="100%" align="center" cellpadding="5" cellspacing="0" border="0" bgcolor="white">
						<tr>
							<td width="30%" valign="top">
								<table cellpadding="0" cellspacing="0" border="0" class="bt" width="100%">
									<tr>
										<td class="bth">
											Welcome <b>
												<asp:label id="lblUserName" runat="server"></asp:label>
											</b>
										</td>
									</tr>
									<tr>
										<td class="btc">
											<p>
												<asp:label id="lblSharing" runat="server"></asp:label>
												<br>
												<br>
												<a href="http://adultmediaswapper.com/contest/" target="_blank">Indexing Score</a>:&nbsp;
												<asp:label id="lblScore" runat="server"></asp:label>
												<br>
												&nbsp;&nbsp;&nbsp;<asp:label id="lblPlace" runat="server"></asp:label>
											</p>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td valign="bottom">
								<table cellpadding="0" cellspacing="0" border="0" class="bt" width="100%">
									<tr>
										<td class="bth">
											AMS Links
										</td>
									</tr>
									<tr>
										<td class="btc">
											<li>
												<a href="http://adultmediaswapper.com/support/software/tutorial/" target="_blank">Tutorial</a>
											<li>
												<a href="http://adultmediaswapper.com/support/software/faq/" target="_blank">FAQ</a></li>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td valign="bottom">
								<table cellpadding="0" cellspacing="0" border="0" class="bt" width="100%">
									<tr>
										<td class="bth">
											AMS Stats
										</td>
									</tr>
									<tr>
										<td class="btc">
											<amswelcome:amsstats runat="server" id="Amsstats1" PositionsToShow="3" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
				<td valign="top">
					<table width="100%" align="center" cellpadding="5" cellspacing="0" border="0" bgcolor="white" height="100%">
						<tr>
							<td valign="top">
								<table cellpadding="0" cellspacing="0" class="bt" width="100%">
									<tr>
										<td class="bth">
											News
										</td>
									</tr>
									<tr>
										<td class="btc">
											<amswelcome:amsnews runat="server" id="AMSNews1" NewsItems="4" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table width="100%" align="center" cellpadding="5" cellspacing="0" border="0" bgcolor="white">
			<tr>
				<td valign="top">
					<table cellpadding="0" cellspacing="0" class="bt" width="100%">
						<tr>
							<td class="bth">
								Featured Site
							</td>
						</tr>
						<tr>
							<td class="btc">
								<amswelcome:amsfeatured runat="server" id="AMSFeatured1" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table width="100%" align="center" background="img/bottom_wash.gif" cellpadding="0" cellspacing="0">
			<tr>
				<td width="10">
					<img src="img/trans.gif" width="10" height="30">
				</td>
				<td>
					<asp:hyperlink id="lnkRefresh" runat="server" forecolor="silver" font-size="8" font-underline="false">refresh page</asp:hyperlink>
				</td>
				<td align="right" valign="bottom">
					<font size="-2" color="silver">© 2001, Line 2 Systems</font>
				</td>
				<td width="10">
					<img src="img/trans.gif" width="10" height="30">
				</td>
			</tr>
		</table>
	</body>
</html>
