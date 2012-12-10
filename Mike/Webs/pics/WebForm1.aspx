<%@ Page language="c#" Codebehind="WebForm1.aspx.cs" AutoEventWireup="false" Inherits="pics.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WebForm1</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="WebForm1" method="post" runat="server">
			<asp:Button id="setCDMode" style="Z-INDEX: 101; LEFT: 211px; POSITION: absolute; TOP: 89px" runat="server" Text="Set CD Mode"></asp:Button>
			<object id="editForm" classid="http:PicAdmin.dll#PicAdmin.EditPictureLink" height="100" width="200" VIEWASTEXT>
				<param name="PictureId" value="1646">
			</object>
		</form>
	</body>
</HTML>
