Attribute VB_Name = "MainModule"
Option Explicit

Private mstrConString As String
Private mcn As ADODB.Connection
Public fMain As frmMain

Public Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" _
 (ByVal lpBuffer As String, nSize As Long) As Long

Public Function GetNTUserName() As String

  ' Dimension variables
  Dim lpBuff As String * 25
  Dim ret As Long, strUserName As String
  
  ' Get the user name minus any trailing spaces found in the name.
  ret = GetUserName(lpBuff, 25)
  strUserName = Left(lpBuff, InStr(lpBuff, Chr(0)) - 1)
  
  GetNTUserName = Trim(strUserName)

End Function

Public Sub Main()

    Randomize Timer
    
    mstrConString = GetSetting("mp", "settings", "ConString", "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=cartman")
    
    Set fMain = New frmMain
    Load fMain
    fMain.Show
    
End Sub


Public Function gConn() As ADODB.Connection
    
    If mcn Is Nothing And mstrConString <> "" Then
        Set mcn = New ADODB.Connection
        mcn.Open mstrConString
    End If
    
    Set gConn = mcn
    
End Function


Public Sub OpenStatus(strStatus As String)

    frmStatus.lblStatus.Caption = strStatus
    frmStatus.Show 0, fMain
    frmStatus.Refresh

End Sub

Public Sub CloseStatus()

    frmStatus.Hide

End Sub

Public Function Nz(vIn, vReplace) As Variant

    If IsNull(vIn) Then
        Nz = vReplace
    Else
        Nz = vIn
    End If

End Function
