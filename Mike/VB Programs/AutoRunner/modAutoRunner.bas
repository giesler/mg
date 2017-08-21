Attribute VB_Name = "modAutoRunner"

Public Const ConnectionString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=webdb;Data Source=kyle;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=CHEF;Use Encryption for Data=False;Tag with column collation when possible=False"
Private cn As ADODB.Connection

Sub Main()

  ' Update row in SQL
  Dim sql As String
  sql = "p_RegisterComputer '" + computername + "'"
  GlobalConnection.Execute sql
  
  

End Sub

Public Function GlobalConnection() As ADODB.Connection

  ' connect if we haven't yet
  If cn Is Nothing Then
    cn = New Connection
    cn.Open ConnectionString
  End If
  
  ' connection again if conneciton lost
  If cn.State = adStateClosed Then
    cn.Open ConnectionString
  End If
  
  GlobalConnection = cn
  
End Function

