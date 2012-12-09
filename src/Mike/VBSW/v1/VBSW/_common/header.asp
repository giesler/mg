
<% 

Dim sOffset, i
for i = 1 to iOffset
	sOffset = sOffset + "../"
next

if rq("Debug") = 1 then 
	Response.Cookies("eSAMI")("debug") = "True"
end if

Function PopupHelp(sArea, sID)

sHref = sOffset + "_common/popuphelp.asp?Area=" + sArea + "&id=" + sID
PopupHelp = "onClick=""window.open('" + sHref + "', 'info', 'width=350,height=300')"""

End Function


Function PopupHelp2(sArea, sID, iWidth, iHeight)

sHref = sOffset + "_common/popuphelp.asp?Area=" + sArea + "&id=" + sID
PopupHelp2 = "onClick=""window.open('" + sHref + "', 'info', 'width=" + CStr(iWidth) + ",height=" + CStr(iHeight) + "')"""

End Function

Function ChangeLocDisplay()

dim sTmp
sTmp = lt("LocationName") + "&nbsp;"
sTmp = sTmp + "<a href=""#"" onClick=""hWin=window.open ('"
sTmp = sTmp + sOffset + "_common/loc_popup.asp', 'info', 'width=180,height=320')"">"
sTmp = sTmp + "<font size=""-1"">(change)</font></a>"
ChangeLocDisplay = sTmp

End Function

%>

<html>
<head>
<title>VBSW: <%=sTitle%></title>
<% if Request.QueryString("print") <> "1" then ' hides stuff for print only view %>
<link rel="stylesheet" type="text/css" href="<%=sOffset%>_common/styles.css">
<% else %>
<link rel="stylesheet" type="text/css" href="<%=sOffset%>_common/PrintStyles.css">
<% end if %>

<script LANGUAGE="JavaScript"><!-- // hide from older browsers

function OpenPopup(sTitle, sURL, iHeight, iWidth) {
	window.open(sURL, sTitle, "directories=no,height=300,width=300,toolbar=no,resizable=no;scrollbars=no;status=no", true);
}

function HandleMouseover()
{
	var oFrom = event.fromElement;
	var eSrc = window.event.srcElement;
	if ("LABEL" == eSrc.tagName.toUpperCase())
	{
		eSrc.style.color = "#003399";
	}
}

function HandleMouseout(eSrc)
{
	var eSrc = window.event.srcElement;
	if ("LABEL" == eSrc.tagName.toUpperCase())
	{
		eSrc.style.color = "";
	}
}

function PreloadImage(img) {
	var a = new Image();
	a.src = img;
	return a;
}

document.onmouseover = HandleMouseover;
document.onmouseout = HandleMouseout;
//--> </script>
</head>

<% if Request.QueryString("print") <> "1" then ' hides stuff for print only view %>

<body class="clsAll" BGCOLOR="white" text="#000000" link="#660000" vlink="#003399" alink="#999933" <%=sOnLoad%> topMargin="0" leftMargin="0" rightMargin="0" bottomMargin="0" marginheight="0" marginwidth="0">

<!-- outer full screen table -->
<table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td>

<!-- Title table -->
<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" class="clsHeader">
	<tr>
		<td width="34" rowSpan="2"><img src="<%=sOffset%>_common/image/WinInstIcon.gif" WIDTH="34" HEIGHT="34"></td>
		<td align="center"><font size="+2"><b><%rw(sTitle)%><% if sSubTitle <> "" then rw("<font size=+1><br>" + sSubTitle + "</font>")%></b></font></td>
		<td width="34" rowSpan="2"><img src="<%=sOffset%>_common/image/vbicon.gif" WIDTH="34" HEIGHT="35"></td>
	</tr><tr>
		<td align="center"><font size="-1"><a href="<%=sOffset%>default.asp">Home</a> | <a href="<%=sOffset%>download">Download</a> | <a href="<%=sOffset%>docs">Documentation</a> | <a href="<%=sOffset%>feedback">Feedback / Help</a> | <a href="<%=sOffset%>about">About</a></font></td>
	</tr>
</table>

<!-- main table-->
<table border="0" cellpadding="10" cellspacing="0" width="100%" height="100%">
	<tr>
		<td>
			<!-- BEGIN CONTENT HERE -->

<% 
' BELOW CONTENT FOR PRINT ONLY VIEW
else %>

<body <%=sOnLoad%> topMargin="0" leftMargin="0" rightMargin="0" bottomMargin="0" marginheight="0" marginwidth="0">

<table border="0" width="100%" cellpadding="0" cellspacing="0">
	<tr>
		<td width="33%"><img src="<%=sOffset%>/_common/sami_mini.gif" height="37" width="50"></td>
		<td width="34%" align="center"><b><%=sTitle%></b></td>
		<td width="33%" align="right"><font size="-1"><%=dl("Organization", "Name", "OrganizationID = " + Request.Cookies("eSAMI")("OrganizationID"))%> <br>
			<%=dl("Organization", "Division", "OrganizationID = " + Request.Cookies("eSAMI")("OrganizationID"))%></font></td>
	</tr>
</table>	
<hr noshade>
<%
 end if %>