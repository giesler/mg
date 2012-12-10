<%@ Page language="c#" Inherits="pics.EditMyInfo" Codebehind="EditMyInfo.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="PicWeb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#ffffff" vLink="#ffff99" aLink="#ffcc99" link="#ffff00" bgColor="#650d00" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">

			<picctls:Header id="header" runat="server" Text="Edit My Info" size="small"></picctls:Header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3">
					    <!-- sidebar -->
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top">
						<table class="logintable" cellpadding="5" cellspacing="0">
							<tr>
								<td colspan="2" bgcolor="gray"><b>Personal Info</b></td>
							</tr>
							<tr>
								<td>First Name:</td>
								<td><asp:TextBox ID="firstName" Runat="server"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Last Name:</td>
								<td><asp:TextBox ID="lastName" Runat="server"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Full Name:</td>
								<td><asp:TextBox ID="fullName" Runat="server"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Email:</td>
								<td><asp:TextBox ID="email" Runat="server"></asp:TextBox></td>
							</tr>
						</table>
						<p>
							Below is any contact information you want others to be able to see.
						</p>
						<asp:DataList ID="contactInfo" class="logintable" Runat="server">
							<HeaderStyle BackColor="gray"></HeaderStyle>
							<HeaderTemplate>
								<b>Contact Info</b>
							</HeaderTemplate>
							<ItemTemplate>
								<p><b><%# DataBinder.Eval(Container.DataItem, "Title") %></b></p>
								<p><%# DataBinder.Eval(Container.DataItem, "Description") %></p>
							</ItemTemplate>
						</asp:DataList>
					
					</td>
				</tr>
			</table>
	</body>
</HTML>
