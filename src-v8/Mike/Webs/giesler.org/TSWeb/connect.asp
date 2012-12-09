<html>
<head>
<title> Terminal Services Connection to <%Response.Write Request.QueryString("Server")%></title>
</head>

<body>

<script language="vbscript" >
<!--
const FullScreenWarnTxt1 = "Your current security settings do not allow automatically switching to fullscreen mode."
const FullScreenWarnTxt2 = "You can use ctrl-alt-pause to toggle your terminal services session to fullscreen mode"
const FullScreenTitleTxt = "Terminal Services Connection "
Const ErrMsgText         = "Error connecting to terminal server: "

sub window_onLoad()
      If not "<%Response.Write Request.QueryString("Server")%>" = "" then
          srvName = "<%Response.Write Request.QueryString("Server")%>"
      else
          srvName = Document.location.hostname
      end if
      MsTsc.Server	 =  srvName
      Document.all.srvNameField.innerHtml = srvName
      MsTsc.Domain       =      "<%Response.Write Request.QueryString("Domain")%>"
      MsTsc.UserName     =      "<%Response.Write Request.QueryString("UserName")%>"
      if  "<% Response.Write Request.QueryString("FS") %>" = "1" then
         if MsTsc.SecuredSettingsEnabled then
            MsTsc.SecuredSettings.FullScreen = "<% Response.Write Request.QueryString("FS") %>" = "1"
         else
            msgbox (FullScreenWarnTxt1  & vbCrLf & FullScreenWarnTxt2 )
         end if
      end if
      
      MsTsc.FullScreenTitle = FullScreenTitleTxt & "(" & "<%Response.Write Request.QueryString("Server")%>" & ")"
      MsTsc.Connect()
end sub
-->
</script>
<center>
        <table>
        <tr>
        <OBJECT language="vbscript" ID="MsTsc"
        CLASSID="CLSID:1fb464c8-09bb-4017-a2f5-eb742f04392f"
        CODEBASE="mstscax.cab#version=5,0,2221,1"
        WIDTH=<% resWidth = Request.QueryString("rW")
            if  resWidth < 200 or resWidth > 1600 then
               resWidth = 800
            end if
            Response.Write resWidth %>
        HEIGHT=<% resHeight = Request.QueryString("rH")
            if  resHeight < 200 or resHeight > 1200 then
               resHeight = 600
            end if
            Response.Write resHeight %>>
        </OBJECT>
        </tr>
        <tr>
        <br>
        <font size="1" color="#000000" face="Verdana, Arial, Helvetica">
        You are logged in to <i><span id="srvNameField"></span></i></font><br>
        </tr>
        
<script language="VBScript">
<!--
sub MsTsc_OnDisconnected(disconnectCode)
   if not disconnectCode = 2 then
      msgbox ErrMsgText & MsTsc.Server
   end if
      'redirect back to login page
      Window.Navigate("default.htm")
end sub
-->
</script>
</table>
</center>
</body>
</html>
