Attribute VB_Name = "Module1"

Public Function OpenWaitForm(txtStatus As String)
On Error Resume Next

Load frmStatus

frmStatus.lblStatus.Caption = txtStatus
frmStatus.Show
frmStatus.Refresh

End Function

Public Function CloseWaitForm()
On Error Resume Next

Unload frmStatus

End Function

'-----------------------------------------------------------
' FUNCTION: FileExists
' Determines whether the specified file exists
'
' IN: [strPathName] - file to check for
'
' Returns: True if file exists, False otherwise
'-----------------------------------------------------------
'
Function FileExists(ByVal strPathName As String) As Integer
On Error GoTo FileExists_Err

    Dim intFileNum As Integer

    '
    'Remove any trailing directory separator character
    '
    If Right$(strPathName, 1) = gstrSEP_DIR Then
        strPathName = Left$(strPathName, Len(strPathName) - 1)
    End If

    '
    'Attempt to open the file, return value of this function is False
    'if an error occurs on open, True otherwise
    '
    intFileNum = FreeFile
    Open strPathName For Input As intFileNum

    FileExists = IIf(Err, False, True)

    Close intFileNum

    Err = 0
    Exit Function

FileExists_Err:
Select Case Err
    Case 53         'file not found/dne
        FileExists = False
        Exit Function
    Case Else
        ErrHand "modMisc", "FileExists"
        FileExists = False
        Exit Function
End Select

End Function

Public Function ErrHand(Optional strLocation As String, Optional strFunction As String)

Dim strMsg As String, strMsg2 As String

If Err = 0 Then
    strMsg = "There was no error passed to the error handling function." & vbCr
    If Not IsNull(strLocation) Then strMsg = strMsg & "Location: " & strLocation
    If Not IsNull(strFunction) Then strMsg = strMsg & " (" & strFunction & ")"
    strMsg = strMsg & vbCr
    MsgBox strMsg, vbExclamation
Else
    strMsg = "There was an error in a database procedure just called.  The error is logged to c:\dberr.log." & vbCr
    If Not IsNull(strLocation) Then strMsg2 = strMsg2 & "Location: " & strLocation
    If Not IsNull(strFunction) Then strMsg2 = strMsg2 & "(" & strFunction & ")"
    strMsg2 = strMsg2 & vbCr
    strMsg2 = strMsg2 & "Error Info: (" & Err.Number & ") " & Err.Description
    strMsg = strMsg & strMsg2 & vbCr
    CloseWaitForm
    Open "c:\dberr.txt" For Append As 42
    Print #42, Date & " - " & Time & ", UserID: " & glCurUserID
    Print #42, strMsg2
    Print #42, "---------------------------------"
    Close 42
    MsgBox strMsg, vbCritical
End If

End Function
