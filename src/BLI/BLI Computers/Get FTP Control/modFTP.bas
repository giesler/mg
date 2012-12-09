Attribute VB_Name = "Module1"
'***************************************************************
'Windows API/Global Declarations for :Get a file from a FTP serve
'     r using winsock.
'***************************************************************


Type Com ' the type to the array, that tells the proggie witch command should be send after a certain reply from the server.
    BackCode As String
    Command As String
    End Type

    

Public Function FormatTime(secs As Long) As String

Dim hours As Long, mins As Long

mins = secs / 60
secs = secs Mod 60
hours = mins / 60
mins = mins Mod 60

If hours > 0 Then
  FormatTime = hours & " hr " & mins & " min " & secs & " sec"
ElseIf mins > 0 Then
  FormatTime = mins & " min " & secs & " sec"
Else
  FormatTime = "Less then a minute"
End If

End Function

Public Function FormatSize(kb As Double) As String

Dim mb As Double

mb = kb / 1024
If kb > 1000 Then
  FormatSize = Format(mb, "0.00") & " MB"
Else
  FormatSize = Format(kb, "0.0") & " KB"
End If

End Function

