VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ntsec"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Public Function GetUserCollection(uCol As Collection, sError As String) As Boolean
On Error GoTo GetUserCollection_Err

GetUserCollection = False
Dim sCompName As String, iSize As Long
Dim lpBuff As String * 100
Dim lUsers As ListOfUserExt, i As Integer

GetComputerName lpBuff, 100
sCompName = Left(lpBuff, InStr(lpBuff, Chr(0)) - 1)

lUsers = LongEnumUsers(sCompName, sError)

'GetUserString = o.GetUserString(sString, sError)

For i = 1 To UBound(lUsers.List)
  uCol.Add lUsers.List(i).Name
Next i

GetUserCollection = True
Exit Function
GetUserCollection_Err:
sError = Err.Description & " (" & Err.Number & ": GetUserCollection)"
Exit Function

End Function

Public Function GetUserString(uString As String, sError As String) As Boolean
On Error GoTo GetUserString_Err

GetUserString = False
Dim sCompName As String, iSize As Long
Dim lpBuff As String * 100
Dim lUsers As ListOfUserExt, i As Integer

GetComputerName lpBuff, 100
sCompName = Left(lpBuff, InStr(lpBuff, Chr(0)) - 1)

lUsers = LongEnumUsers(sCompName, sError)

'GetUserString = o.GetUserString(sString, sError)

For i = 1 To UBound(lUsers.List)
  If i = 1 Then
    uString = lUsers.List(i).Name
  Else
    uString = uString & "|" & lUsers.List(i).Name
  End If
Next i

GetUserString = True
Exit Function
GetUserString_Err:
sError = Err.Description & " (" & Err.Number & ": GetUserString)"
Exit Function

End Function

Public Function GetUserStr() As String

GetUserStr = ""
Dim sCompName As String, iSize As Long, uString As String
Dim lpBuff As String * 100, sError As String
Dim lUsers As ListOfUserExt, i As Integer

GetComputerName lpBuff, 100
sCompName = Left(lpBuff, InStr(lpBuff, Chr(0)) - 1)

lUsers = LongEnumUsers(sCompName, sError)

'GetUserString = o.GetUserString(sString, sError)

For i = 1 To UBound(lUsers.List)
  If i = 1 Then
    uString = lUsers.List(i).Name
  Else
    uString = uString & "|" & lUsers.List(i).Name
  End If
Next i

GetUserStr = uString

End Function

