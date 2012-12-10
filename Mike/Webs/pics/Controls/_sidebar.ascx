<%@ Control Language="c#" AutoEventWireup="false" Codebehind="_sidebar.ascx.cs" Inherits="pics.Controls.__sidebar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table width="100%" cellpadding="0" cellspacing="0" height="100%">
	<tr>
		<td style="POSITION: relative">
			<div style="Z-INDEX: 1; LEFT: 18px; POSITION: absolute; TOP: 350px">
				<img src="<%=HttpContext.Current.Request.ApplicationPath%>/Images/msn2needlewash.gif">
			</div>
			<div style="Z-INDEX: 2; POSITION: absolute">
				<asp:Panel Width="100%" Height="100%" Runat="server" ID="contentPanel">
				</asp:Panel>
			</div>
		</td>
	</tr>
</table>
