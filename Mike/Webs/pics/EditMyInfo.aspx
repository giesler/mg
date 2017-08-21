<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="EditMyInfo.aspx.cs" AutoEventWireup="false" Inherits="pics.EditMyInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#ffffff" vLink="#ffff99" aLink="#ffcc99" link="#ffff00" bgColor="#650d00" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<!-- top table with MSN2 logo --><pics:header id="ctlHeader" header="Pictures" size="small" runat="server"></pics:header>
		<table cellSpacing="0" cellPadding="0" width="100%" align="left">
			<tr>
				<td>
					<!-- Main content -->
					<table width="100%">
						<tr>
							<td width="50">&nbsp;
							</td>
							<td align="left">
								<p></p>
								<p><b> Edit Personal Information</b>
								</p>
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
					<!-- Begin footer -->
				</td>
			</tr>
		</table>
	</body>
</HTML>
