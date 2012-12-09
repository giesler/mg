Attribute VB_Name = "modMain"
Public Declare Function ShellExecute Lib "shell32.dll" _
    Alias "ShellExecuteA" (ByVal hwnd As Long, _
    ByVal lpOperation As String, ByVal lpFile As String, _
    ByVal lpParameters As String, ByVal lpDirectory As String, _
    ByVal nShowCmd As Long) As Long

Public Declare Function URLDownloadToFile Lib "urlmon" Alias _
  "URLDownloadToFileA" (ByVal pCaller As Long, ByVal szURL As String, ByVal _
  szFileName As String, ByVal dwReserved As Long, ByVal lpfnCB As Long) As Long

'
'
Public Sub ExecuteLink(ByVal sLinkTo As String)
'
' Execute the passed Link
'
    On Error Resume Next
'
    Dim lRet As Long
    Dim lOldCursor As Long
'
    lOldCursor = Screen.MousePointer
'
    Screen.MousePointer = vbHourglass
'
    lRet = ShellExecute(0, "open", sLinkTo, "", vbNull, SW_SHOWNORMAL)
'
    If lRet >= 0 And lRet <= 0 Then
        Screen.MousePointer = vbDefault
        MsgBox "error Opening Link to " & sLinkTo & vbCrLf & _
             vbCrLf & Err.LastDllError, vbExclamation, "ExecuteLink"
    End If
    Screen.MousePointer = vbDefault
'
End Sub

Public Sub Main()

Randomize Timer

Load frmMain
frmMain.Show

End Sub
