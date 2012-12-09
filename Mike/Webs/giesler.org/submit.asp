<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>mike</title>
<style><!-- 
a 			{color:blue;  TEXT-DECORATION: none;} 
a:visited {color:blue;} 
a:hover 	{color:red; TEXT-DECORATION: none;}
--></style>
   
</head>

<body>

<%

dim oMsg
set oMsg = Server.CreateObject("CDONTS.NewMail")

oMsg.To = "Mike Giesler <mike@giesler.org>"
oMsg.From = Request.Form("from") & "<" & Request.Form("Email") & ">"
oMsg.Subject = Request.Form("subject")
oMsg.Body = Request.Form("message")
oMsg.Send
set omsg = nothing

%>

<table border="0" cellpadding="0" cellspacing="0" width="100%" height="99%">
  <tr>
    <td width="100%" valign="top" colspan="2">
      <div align="left">
        <table height="99%">
          <tbody>
            <tr>
              <td align="left" id="tableProps" vAlign="top" width="70"><a href="/"><img id="pagerrorImg" src="images/giesler.org.gif" border="0">
                </a>
              <td id="tablePropsWidth" width="400" align="left" vAlign="top">
                <h1 id="errortype" style="COLOR: #4e4e4e; FONT: 14pt/16pt verdana"><id id="Comment1"> giesler.org </id>  - Feedback
                </h1>
            <p>
                <id id="Comment2"><!--Probable causes:<--></id>&nbsp;&nbsp; <font style="COLOR: black; FONT: 9pt/12pt verdana">Thanks for your feedback!<br><br>Return to <a href="/">giesler.org</a>.</font></p> 
            <p>&nbsp;</p>
            <p>&nbsp;</p>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <p>&nbsp;</p>
      <p>&nbsp;</p></td>
  </tr>
  <tr>
    <td width="50%" valign="bottom" align="left">
    </td>
    <td width="50%" valign="bottom" align="right"><img border="0" src="images/w2k.gif">
    </td>
  </tr>
</table>

</body>

</html>
