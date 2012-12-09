Attribute VB_Name = "Helpers"
' This module provides helper function for all VB layers

Option Explicit

Private Declare Function GetComputerNameAPI Lib "kernel32" Alias "GetComputerNameA" _
    (ByVal lpBuffer As String, nSize As Long) As Long

Private Type GUID
  Data1 As Long
  Data2 As Integer
  Data3 As Integer
  Data4(7) As Byte
End Type

Private Declare Function CoCreateGuid Lib "OLE32.DLL" (pGuid As GUID) As Long

' mp is short for MakeParameter - does typesafe array creation for use with Run* functions
Public Function mp(ByVal pstrName As String, ByVal pvarType As ADODB.DataTypeEnum, _
  ByVal pintSize As Integer, ByVal pvarValue As Variant)

  mp = Array(pstrName, pvarType, pintSize, pvarValue)

End Function

'Converts a variant into a string. If the varaint is null, it is converted to the empty string.
Public Function ConvertToString(pvarIn As Variant) As String

  If IsNull(pvarIn) Then
    ConvertToString = ""
  Else
    ConvertToString = CStr(pvarIn)
  End If

End Function

'Converts any Null variant to null, otherwise it returns the existing value.
Public Function NullsToZero(pvarIn As Variant) As Variant

  If IsNull(pvarIn) Then
    NullsToZero = 0
  Else
    NullsToZero = pvarIn
  End If

End Function

'Change the transaction state, and raise an error.
Public Sub CtxRaiseError(pstrModule As String, pstrFunction As String)
    
'Set the default to disable transaction. Now unless someone does a SetComplete the transaction will abort.
'This is just like calling setabort, but has it doesn't destroy the Err object if we are in a transaction.
GetObjectContext.DisableCommit
    
'log the error to the event for later use.
'logError Err.Number, GetErrSourceDescription(module, functionName), Err.description, "from CtxRaiseError"
    
'Raise an error to indate there was a problem.
'This will indicate that no one should do a SetComplete unless they can handle this error.
Err.Raise Err.Number, GetErrSourceDescription(pstrModule, pstrFunction), Err.description

End Sub

'Raise an error without changing the transaction state.
Public Sub RaiseError(pstrModule As String, pstrFunction As String)
    
'log the error to the event for later use.
'logError Err.Number, GetErrSourceDescription(module, functionName), Err.description, "From RaiseError"
    
Err.Raise Err.Number, GetErrSourceDescription(pstrModule, pstrFunction), Err.description

End Sub

' Set or retrieve the name of the computer.
Function GetComputerName() As String

Dim strBuffer As String, lngLen As Long
        
strBuffer = Space(255 + 1)
lngLen = Len(strBuffer)
If CBool(GetComputerNameAPI(strBuffer, lngLen)) Then
  GetComputerName = Left$(strBuffer, lngLen)
Else
  GetComputerName = ""
End If

End Function

' Returns an error message like:  "[SAMICom.DBHelper] RunSP [on AppServer version 1.1.176]"
Private Function GetErrSourceDescription(pstrModName As String, pstrFunction As String) As String

  GetErrSourceDescription = Err.source & vbNewLine & "<br>" & "[" & pstrModName & "]  " & pstrFunction & _
      " [on " & GetComputerName() & " version " & GetVersionNumber() & "]"

End Function

'resturns the current DLL version number
Function GetVersionNumber() As String

  GetVersionNumber = App.Major & "." & App.Minor & "." & App.Revision

End Function

'function takes a string with an '=' in it and returns the left part
'GetKey("test=1") return test
Public Function GetKey(ByVal strItem As String) As String

  Dim i As Integer
  i = InStr(1, strItem, "=", vbTextCompare)
  If i > 0 Then
    GetKey = Left(strItem, i - 1)
  Else
    GetKey = ""
  End If

End Function

'function takes a string with an '=' in it and returns the right part
'GetValue("test=1") return 1
Public Function GetValue(ByVal strItem As String) As String

  Dim i As Integer
  i = InStr(1, strItem, "=", vbTextCompare)
  If i > 0 Then
    GetValue = Mid(strItem, 1 + i)
  Else
    GetValue = ""
  End If

End Function

'this procedure logs an error to the system application log
Private Sub logError(plngErrNum As Long, pstrSource As String, _
  pstrDescription As String, Optional pstrNotes As String = "")

App.LogEvent vbNewLine & "An error has occured in a SAMI Component." & vbNewLine & _
    "Error Number:" & plngErrNum & vbNewLine & vbNewLine & _
    "Description:" & vbNewLine & pstrDescription & vbNewLine & vbNewLine & _
    "Source: " & vbNewLine & pstrSource & vbNewLine & vbNewLine & _
    "Notes: " & vbNewLine & pstrNotes

End Sub

Public Function CreateGUID()

  Dim udtGUID As GUID, strTmp As String
  
  If (CoCreateGuid(udtGUID) = 0) Then
    strTmp = _
      String(8 - Len(Hex$(udtGUID.Data1)), "0") & Hex$(udtGUID.Data1) & _
      String(4 - Len(Hex$(udtGUID.Data2)), "0") & Hex$(udtGUID.Data2) & _
      String(4 - Len(Hex$(udtGUID.Data3)), "0") & Hex$(udtGUID.Data3) & _
      IIf((udtGUID.Data4(0) < &H10), "0", "") & Hex$(udtGUID.Data4(0)) & _
      IIf((udtGUID.Data4(1) < &H10), "0", "") & Hex$(udtGUID.Data4(1)) & _
      IIf((udtGUID.Data4(2) < &H10), "0", "") & Hex$(udtGUID.Data4(2)) & _
      IIf((udtGUID.Data4(3) < &H10), "0", "") & Hex$(udtGUID.Data4(3)) & _
      IIf((udtGUID.Data4(4) < &H10), "0", "") & Hex$(udtGUID.Data4(4)) & _
      IIf((udtGUID.Data4(5) < &H10), "0", "") & Hex$(udtGUID.Data4(5)) & _
      IIf((udtGUID.Data4(6) < &H10), "0", "") & Hex$(udtGUID.Data4(6)) & _
     IIf((udtGUID.Data4(7) < &H10), "0", "") & Hex$(udtGUID.Data4(7))
     CreateGUID = _
      "{" & _
      Mid(strTmp, 1, 8) & "-" & _
      Mid(strTmp, 9, 4) & "-" & _
      Mid(strTmp, 13, 4) & "-" & _
      Mid(strTmp, 17, 4) & "-" & _
      Mid(strTmp, 21, 12) & _
      "}"
  
  End If

End Function

' replaces blank and null and zero values with null for a sp
Public Function sn(Optional pvarIn As Variant) As Variant
  
  If IsMissing(pvarIn) Then
    sn = Null
  ElseIf IsNull(pvarIn) Then
    sn = Null
  ElseIf pvarIn = "" Then
    sn = Null
  ElseIf IsDate(pvarIn) Then
    If CVDate(pvarIn) < #1/1/1900# Then
      sn = Null
    Else
      sn = CVDate(pvarIn)
    End If
  ElseIf IsNumeric(pvarIn) Then
    If Val(pvarIn) = 0 Then
      sn = Null
    Else
      sn = Val(pvarIn)
    End If
  Else
    sn = pvarIn
  End If

End Function
