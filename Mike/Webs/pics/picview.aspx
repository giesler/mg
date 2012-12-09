<%@ Page language="c#" Codebehind="picview.aspx.cs" AutoEventWireup="false" Inherits="pics.picview" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="refresh" content="<%= HttpRefreshURL %>">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Zoom In">
		</pics:header>
		<form id="picview" method="post" runat="server">
			<p align="center">
				<asp:Label ID="lblTitle" Runat="server">[Title]</asp:Label>
				<br>
				<i>
					<asp:Label ID="lblPictureDate" Runat="server" Font-Size="smaller">[Date]</asp:Label>
				</i>
				<br>
				<table border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td runat="server" id="tdPicture" style="BORDER-RIGHT: silver thin solid; BORDER-TOP: silver thin solid; MARGIN: 2px; BORDER-LEFT: silver thin solid; BORDER-BOTTOM: silver thin solid">
						</td>
					</tr>
				</table>
			</p>
			<asp:Panel ID="pnlDescription" Runat="server" Width="100%">
				<P>
					<asp:Label id="lblPictureDesc" Runat="server">[Description]</asp:Label>
				</P>
			</asp:Panel>
			<asp:Panel ID="pnlPeople" Runat="server" Width="100%">
				<I>
					<asp:DataList id="dlPerson" Runat="server" Width="100%" RepeatLayout="Flow" RepeatDirection="Horizontal">
						<ItemTemplate>
							<asp:Label ID="lblPersonFullName" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>'></asp:Label>
						</ItemTemplate>
						<SeparatorTemplate>
							,
						</SeparatorTemplate>
					</asp:DataList>
				</I>
			</asp:Panel>
			<br>
			<asp:panel id="pnlPageControls" runat="server" Width="100%" DESIGNTIMEDRAGDROP="51" BackColor="Gray">
				<TABLE width="100%">
					<TR>
						<TD width="33%">
							<asp:HyperLink id="lnkPrevious" Runat="server" Visible="False">&lt;&lt; Previous</asp:HyperLink>
						</TD>
						<TD align="middle" width="34%">
							Picture
							<asp:Label id="lblPicture" Runat="server">[Picture]</asp:Label>
							&nbsp;of
							<asp:Label id="lblPictures" runat="server">[Pictures]</asp:Label>
						</TD>
						<TD align="right" width="33%">
							<asp:HyperLink id="lnkNext" Runat="server" Visible="False">Next &gt;&gt;</asp:HyperLink>
						</TD>
					</TR>
				</TABLE>
			</asp:panel>
			<P>
				<asp:hyperlink id="lnkReturn" Runat="server"></asp:hyperlink>
			</P>
		</form>
	</body>
</HTML>
