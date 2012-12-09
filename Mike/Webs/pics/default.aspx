<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="pics.Cdefault" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<LINK rel="stylesheet" type="text/css" href="msn2.css">
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures">
		</pics:header>
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
									Welcome to the MSN2 Pictures site.
								</p>
								<p>
									Over the next few months, expect to see more and more pictures added to this 
									site. <i>[I plan on adding all my pictures I have... eventually... -Mike]</i> And 
									if you run into any errors or have any problems using this site, let one of us 
									know. We plan on working on improving it to make it easier to use, and 
									suggestions are welcome.
								</p>
								<p>
									<STRONG>You can view pictures in one of two ways:</STRONG>
								</p>
								<UL>
									<li>
										<a href="categories.aspx">View by Category</a>
									- show all pictures sorted by category - similiar to an 'Album' type view.
									<LI>
										<a href="SearchCriteria.aspx">Search</a> - allows you to search for pictures by 
										date, description, or people in the picture</LI>
								</UL>
								<P>
								</P>
								<p>
									Any comments on this site or pictures on the site, send to <a href="mailto:mike@giesler.org">
										mike@giesler.org</a>.
								</p>
								<p>
									<STRONG>Recently added/updated categories... </STRONG>
								</p>
								<asp:Panel ID="recentPanel" Runat="server"></asp:Panel>
								<p>
									<strong>Random Picture</strong>
								</p>
								<asp:Panel ID="randomPicture" Runat="server"></asp:Panel>
							</td>
						</tr>
					</table>
					<!-- Begin footer -->
				</td>
			</tr>
		</table>
	</body>
</HTML>
