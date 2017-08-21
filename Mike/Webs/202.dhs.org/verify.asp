
<%
Dim sPageTitle, bLoginPage, iFolderLevels
iFolderLevels = 0
sPageTitle  = "202 Login Verification Page"
bLoginPage = True

'Check for UserID, Password in form collection
if Request.Form("UserLogin") = "" then
	Response.Redirect("login.asp")
end if

dim cnn1
set cnn1 = Server.CreateObject("ADODB.Connection")
cnn1.open "Provider=SQLOLEDB.1;Password=202_login;Persist Security Info=True;User ID=202_login;Data Source=pdc;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=202Web"

Dim rst, refurl, cookie
set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "Select UserID, UserPW, UserLogin, UserDrunkEnable from tblUser Where Upper(UserLogin) = Upper('" & Request.Form("UserLogin") & "')",cnn1

if rst.BOF and rst.EOF then
	sMsg = "The user id '" + Request.Form("UserLogin") + "' was not found.<br>"
	sMsg = sMsg & "<a href=""login.asp"">Back to login page</a>"
elseif UCase(rst.Fields("UserPW")) <> UCase(Request.Form("UserPW")) then
	sMsg = "The password you entered is incorrect.<br>"
	sMsg = sMsg & "<a href=""login.asp"">Back to login page</a>"
else
	refurl = Request.Form("refurl")
	if refurl = "" then
		refurl = "menu.asp"
	end if
	Response.Cookies("202")("LoggedIn")= True
	Response.Cookies("202")("UserLogin") = rst.Fields("UserLogin")
	Response.Cookies("202")("UserDrunkEnable") = rst.Fields("UserDrunkEnable")
	Response.Cookies("202")("UserID") = rst.Fields("UserID")
	
	if Request.Form("SaveLogin") = "on" then
		Response.Cookies("202").Expires = "December 31, 2004"
	end if
	Response.Redirect(refurl)
	sMsg = "You succesfully logged in.<br>"
	sMsg = sMsg & "<a href=""menu.asp"">Home</a>"
end if

rst.Close

%>
<!--#INCLUDE VIRTUAL="/include/header.asp"-->

<%=sMsg%>

<!--#INCLUDE VIRTUAL="/include/footer.asp"-->
