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
											<p>
												Top Indexing Score:
												<asp:label id="lblTopIndexScore" runat="server">(unavailable)</asp:label>
												<br>
												Total unique files:
												<asp:label id="lblTotalUniqueFiles" runat="server">(unavailable)</asp:label>
											</p>
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
											<p>
												<font color="gray" size="-1"><i>July 20, 12:00 AM Central</i></font>
												<br>
												We had so many requests from users to see their current ranking in the contest 
												that we added a top score in the new 'AMS Stats' section on the left.
											</p>
											<p>
												<font color="gray" size="-1"><i>July 11, 9:00 PM Central</i></font>
												<br>
												We fixed a small bug with the score display on your left. Users behind 
												firewalls received a message 'Temporarily Unavailable'. Those user's scores now 
												accurately reflect any indexing done - no points were lost or not credited due 
												to this bug.
											</p>
											<p>
												<font color="gray" size="-1"><i>July 10, 12:00 AM Central</i></font>
												<br>
												A new AMS version was released. AMS version 0.70 includes many new features. We 
												are also announcing our new indexing contest - click <a href="http://adultmediaswapper.com/contest/" target="_blank">
													here</a> to see the details on the new contest.
											</p>
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
								<h4>
									<%=featureAdTitle%>
								</h4>
								<p>
									<a href="http://www.adultmediaswapper.com/featured/" target="_blank"><img src="<%=featureAdImageURL%>" border="0">
										<br>
										<strong>Click here for a review of this site.</strong></a>
								</p>
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
