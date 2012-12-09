
<%

'open connection
if cnn is nothing then
  dim cnn
  set cnn = Server.CreateObject("ADODB.Connection")
  cnn.open "Provider=SQLOLEDB.1;Password=202_login;Persist Security Info=True;User   ID=202_login;Data Source=pdc;Use Procedure for Prepare=1;Auto Translate=True;Packet   Size=4096;Workstation ID=202Web"
end if
%>
