Attribute VB_Name = "Module1"
Public Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Long, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long
Public Const SW_NORMAL = 1

Public Enum CompType
  eFactoryCat
  eTomCat
End Enum

Public cComp As CompType
Public Const sUpdateServer = "partsupdate.factorycat.com"
Public fMainForm As frmMain

Sub Main()
On Error Resume Next

  Dim i As Long, sIn As String
  i = FreeFile
  Open App.Path & "\type.ini" For Input As i
  Input #i, sIn
  Close i
  
  If Trim(sIn) = "FC" Then
    cComp = eFactoryCat
  ElseIf Trim(sIn) = "TC" Then
    cComp = eTomCat
  Else
    cComp = eFactoryCat
  End If

  frmSplash.Show
  frmSplash.Refresh
  
  Dim sTmp As String
  sTmp = GetSetting(App.Title, "Options", "UpdateServer", "not set")
  If sTmp = "not set" Then SaveSetting App.Title, "Options", "UpdateServer", sUpdateServer
  If sTmp = "bli.dhs.org" Then SaveSetting App.Title, "Options", "UpdateServer", sUpdateServer
  
  Set fMainForm = New frmMain
  Load fMainForm
  fMainForm.Caption = fMainForm.Caption
  
  fMainForm.Show
End Sub

Sub Pause(f As Form)
f.Visible = True
While f.Visible
  DoEvents
Wend
End Sub

Public Function Nz(v1 As Variant, v2 As Variant)

If IsNull(v1) Then
  Nz = v2
Else
  Nz = v1
End If

End Function

Public Function OpenWaitForm(sStatus As String)

frmStatus.Label1.Caption = sStatus
frmStatus.Visible = True
frmStatus.Refresh
frmStatus.SetFocus

End Function

Public Function CloseWaitForm()

frmStatus.Hide

End Function

Public Sub ErrHand(sSource, sSource2)

If Err = 0 Then Exit Sub

MsgBox "An error has occurred.  If the error continues, please contact support." & vbCrLf & Err.Description & vbCrLf & "Location: " & sSource & ": " & sSource2, vbCritical, "Error #" & Err.Number

Exit Sub

End Sub
