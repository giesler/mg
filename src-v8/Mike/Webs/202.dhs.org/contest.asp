<%@ Language=VBScript %>
<%
Dim sPageTitle, bLoginPage, iFolderLevels
iFolderLevels = 0
sPageTitle  = "202 Beverage Wall Contest"
bLoginPage = False
%>

<!--#INCLUDE VIRTUAL="/include/header.asp"-->

<%
if Request.Form("enter") = "on" then
	dim sMsg, oMsg, rst
	set rst = Server.CreateObject("ADODB.Recordset")
	set oMsg = Server.CreateObject("CDONTS.NewMail")
	rst.Open "select * from tblUser where userid = " & Request.Cookies("202")("UserID"),cnn
	oMsg.Subject = "Beverage Wall Contest"
	oMsg.BodyFormat = 0
	oMsg.MailFormat = 0
	oMsg.From = rst.Fields("UserName") & "<" & rst.Fields("UserEmail") & ">"
	oMsg.To = "Mike Giesler <giesler@cs.wisc.edu>"

	sMsg = "<!DOCTYPE HTML PUBLIC ""-//IETF//DTD HTML//EN"">" & vbCrLf
	sMsg = sMsg & "<html><head>"
	sMsg = sMsg & "<meta http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1"">"
	sMsg = sMsg & "<title>Beverage Wall Survey Results</title>"
	sMsg = sMsg & "</head><body>"
	sMsg = sMsg & "Beverage Wall Survey Results<br><br>"
	sMsg = sMsg & "<table border=1 cellpadding=5>"
	sMsg = sMsg & "<tr><td>Beverage</td><td>Place</td><td>Qty</td></tr>"
	sMsg = sMsg & "<tr><td>Barq's</td><td>" & Request.Form("BarqsPlace") & "</td><td>" & Request.Form("BarqsQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Budweiser</td><td>" & Request.Form("BudweiserPlace") & "</td><td>" & Request.Form("BudweiserQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Bud Light</td><td>" & Request.Form("BudLightPlace") & "</td><td>" & Request.Form("BudLightQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Coke</td><td>" & Request.Form("CokePlace") & "</td><td>" & Request.Form("CokeQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>High Life</td><td>" & Request.Form("HighLifePlace") & "</td><td>" & Request.Form("HighLifeQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Icehouse</td><td>" & Request.Form("IcehousePlace") & "</td><td>" & Request.Form("IcehouseQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Miller Lite</td><td>" & Request.Form("MillerLitePlace") & "</td><td>" & Request.Form("MillerLiteQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>MGD</td><td>" & Request.Form("MGDPlace") & "</td><td>" & Request.Form("MGDQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>MGD Lite</td><td>" & Request.Form("MGDLitePlace") & "</td><td>" & Request.Form("MGDLiteQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Mug</td><td>" & Request.Form("MugPlace") & "</td><td>" & Request.Form("MugQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Old Style</td><td>" & Request.Form("OldStylePlace") & "</td><td>" & Request.Form("OldStyleQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Pepsi</td><td>" & Request.Form("PepsiPlace") & "</td><td>" & Request.Form("PepsiQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Red Dog</td><td>" & Request.Form("RedDogPlace") & "</td><td>" & Request.Form("RedDogQty") & "</td></tr>"
	sMsg = sMsg & "<tr><td>Sprite</td><td>" & Request.Form("SpritePlace") & "</td><td>" & Request.Form("SpriteQty") & "</td></tr>"
	sMsg = sMsg & "</table><br>"
	sMsg = sMsg & "Beverage Choice: " & Request.Form("BeverageChoice") & "<br><br>"
	sMsg = sMsg & "<font size=-1>"
	sMsg = sMsg & "Host: " & Request.ServerVariables("REMOTE_ADDR") & vbCrLf
	sMsg = sMsg & "Client: " & Request.ServerVariables("") & vbCrLf
	sMsg = sMsg & "</font>"
	sMsg = sMsg & "</body></html>"
	oMsg.Body = sMsg
	oMsg.Send
	Response.Write("Your entry has been submitted.  Good Luck!<br><br>")
	Response.Write("<a href=""default.asp"">Return to home</a>")
else
%>

<table border=0 width=500 align="center" cellpadding=8 bgColor="black">
	<tr>
		<td>In the form below, enter your guess at the order of the number of cans in the Beverage Wall.  Enter numbers one through 14.  You can also guess the quantity if you'd like.  (There are 311 total.)<br>
		<font size=-1>Grand Prize:  a case of your beverage choice (something from the wall).<br>
		Other Prizes:  if you get a decent number of the quantities correct, you'll get a six pack.<br><br>
		Scoring:  One point per beverage in the right order.  There were several that were tied - those the order doesn't matter.</font></td>
	</tr>
</table>

<hr noshade>

<form action="contest.asp" method="post">
<input type="hidden" name="enter" value="on">

<table border=0 width=400 align="center" cellpadding=4 bgColor="black">
	<tr><td>Beverage</td><td>Place</td><td>Quantity</td></tr>

	<tr><td>Barq's</td><td><input type="text" size=2 name="BarqsPlace"></td><td><input type="text" size=3 name="BarqsQty"></td></tr>
	<tr><td>Budweiser</td><td><input type="text" size=2 name="BudweiserPlace"></td><td><input type="text" size=3 name="BudweiserQty"></td></tr>
	<tr><td>Bud Light</td><td><input type="text" size=2 name="BudLightPlace"></td><td><input type="text" size=3 name="BudLightQty"></td></tr>
	<tr><td>Coke</td><td><input type="text" size=2 name="CokePlace"></td><td><input type="text" size=3 name="CokeQty"></td></tr>
	<tr><td>High Life</td><td><input type="text" size=2 name="HighLifePlace"></td><td><input type="text" size=3 name="HighLifeQty"></td></tr>
	<tr><td>Icehouse</td><td><input type="text" size=2 name="IcehousePlace"></td><td><input type="text" size=3 name="IcehouseQty"></td></tr>
	<tr><td>Miller Lite</td><td><input type="text" size=2 name="MillerLitePlace"></td><td><input type="text" size=3 name="MillerLiteQty"></td></tr>
	<tr><td>MGD</td><td><input type="text" size=2 name="MGDPlace"></td><td><input type="text" size=3 name="MGDQty"></td></tr>
	<tr><td>MGD Lite</td><td><input type="text" size=2 name="MGDLitePlace"></td><td><input type="text" size=3 name="MGDLiteQty"></td></tr>
	<tr><td>Mug</td><td><input type="text" size=2 name="MugPlace"></td><td><input type="text" size=3 name="MugQty"></td></tr>
	<tr><td>Old Style</td><td><input type="text" size=2 name="OldStylePlace"></td><td><input type="text" size=3 name="OldStyleQty"></td></tr>
	<tr><td>Pepsi</td><td><input type="text" size=2 name="PepsiPlace"></td><td><input type="text" size=3 name="PepsiQty"></td></tr>
	<tr><td>Red Dog</td><td><input type="text" size=2 name="RedDogPlace"></td><td><input type="text" size=3 name="RedDogQty"></td></tr>
	<tr><td>Sprite</td><td><input type="text" size=2 name="SpritePlace"></td><td><input type="text" size=3 name="SpriteQty"></td></tr>
	<tr><td colSpan=3>&nbsp;</td></tr>
	<tr><td>Your beverage choice:</td><td colSpan=2>
		<SELECT id=BeverageChoice name=BeverageChoice>
			<option>Barq's</option><option>Budweiser</option><option>Bud Light</option>
			<option>Coke</option><option>High Life</option><option>Icehouse</option>
			<option>Miller Lite</option><option>MGD</option><option>MGD Light</option>
			<option>Mug</option><option>Old Style</option><option>Pepsi</option>
			<option>Red Dog</option><option>Sprite</option>
		</SELECT></td></tr>
</table>
<br>
<center>
<input type="submit" name="Submit" value="Submit">
<input type="button" value="Cancel" onClick="javascript:history.back()">
</center>
</form>

<%
end if
%>

<!--#INCLUDE VIRTUAL="/include/footer.asp"-->

