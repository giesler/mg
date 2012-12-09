Attribute VB_Name = "Module1"
Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
Public Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long

Public oVoice As VTxtAuto.VTxtAuto

Public Type tpRemField
  sName As String
  bSet As Boolean
  sPrefix As String
  iSort As Byte
End Type

Public Type tpReminder
  sName As String
  bSet As Boolean
  flds() As tpRemField
End Type

Public Type tpNewMailField
  sName As String
  bSet As Boolean
  sPrefix As String
  iSort As Byte
End Type

Public arRems() As tpReminder
Public arNewMail() As tpNewMailField
Public blnNewMail As Boolean
Public blnReminder As Boolean
Public sEmailAddress As String
Public blnAutoStart As Boolean
Public blnAutoMinimize As Boolean
Public sNewMailPrefix As String

Public blnActive As Boolean
Public cl1 As clOutlook
Public clOE As OutlookEvents.clVTxtCallback

Private Const m_ModName = "modOutlookEvents"

Public Sub Main()
On Error GoTo Main_Err

LoadSettings

Load frmOlStatus
If blnAutoMinimize Then
  frmOlStatus.Visible = False
  frmOlStatus.mnuShowHide.Caption = "S&how"
Else
  frmOlStatus.Visible = True
  frmOlStatus.mnuShowHide.Caption = "&Hide"
End If

Exit Sub
Main_Err:
ErrHand m_ModName, "Main", Err.Description, Err.Number
End
Exit Sub
End Sub

Public Sub LoadSettings()
On Error GoTo LoadSettings_Err

ReDim arRems(2)
Dim tp As tpReminder, fd() As tpRemField

blnReminder = GetSetting(App.Title, "Settings", "Reminder", True)
arRems(0).sName = "Appointment"
arRems(0).bSet = GetSetting(App.Title, arRems(0).sName, "Enabled", True)
ReDim fd(3)
fd(0).sName = "Subject"
fd(0).bSet = GetSetting(App.Title, arRems(0).sName, fd(0).sName & " Set", True)
fd(0).iSort = GetSetting(App.Title, arRems(0).sName, fd(0).sName & " Sort", 0)
fd(0).sPrefix = GetSetting(App.Title, arRems(0).sName, fd(0).sName & " Prefix", "Sbjt: ")
fd(1).sName = "Location"
fd(1).bSet = GetSetting(App.Title, arRems(0).sName, fd(1).sName & " Set", True)
fd(1).iSort = GetSetting(App.Title, arRems(0).sName, fd(1).sName & " Sort", 1)
fd(1).sPrefix = GetSetting(App.Title, arRems(0).sName, fd(1).sName & " Prefix", "Loc: ")
fd(2).sName = "Start Time"
fd(2).bSet = GetSetting(App.Title, arRems(0).sName, fd(2).sName & " Set", True)
fd(2).iSort = GetSetting(App.Title, arRems(0).sName, fd(2).sName & " Sort", 2)
fd(2).sPrefix = GetSetting(App.Title, arRems(0).sName, fd(2).sName & " Prefix", "Time: ")
fd(3).sName = "Body"
fd(3).bSet = GetSetting(App.Title, arRems(0).sName, fd(3).sName & " Set", True)
fd(3).iSort = GetSetting(App.Title, arRems(0).sName, fd(3).sName & " Sort", 3)
fd(3).sPrefix = GetSetting(App.Title, arRems(0).sName, fd(3).sName & " Prefix", "Body: ")
arRems(0).flds = fd

arRems(1).sName = "Mail Item"
arRems(1).bSet = GetSetting(App.Title, arRems(1).sName, "Enabled", True)
ReDim fd(3)
fd(0).sName = "From"
fd(0).bSet = GetSetting(App.Title, arRems(1).sName, fd(0).sName & " Set", True)
fd(0).iSort = GetSetting(App.Title, arRems(1).sName, fd(0).sName & " Sort", 0)
fd(0).sPrefix = GetSetting(App.Title, arRems(1).sName, fd(0).sName & " Prefix", "From: ")
fd(1).sName = "Subject"
fd(1).bSet = GetSetting(App.Title, arRems(1).sName, fd(1).sName & " Set", True)
fd(1).iSort = GetSetting(App.Title, arRems(1).sName, fd(1).sName & " Sort", 1)
fd(1).sPrefix = GetSetting(App.Title, arRems(1).sName, fd(1).sName & " Prefix", "Sbjt: ")
fd(2).sName = "Sent Time"
fd(2).bSet = GetSetting(App.Title, arRems(1).sName, fd(2).sName & " Set", False)
fd(2).iSort = GetSetting(App.Title, arRems(1).sName, fd(2).sName & " Sort", 2)
fd(2).sPrefix = GetSetting(App.Title, arRems(1).sName, fd(2).sName & " Prefix", "Sent: ")
fd(3).sName = "Body"
fd(3).bSet = GetSetting(App.Title, arRems(1).sName, fd(3).sName & " Set", True)
fd(3).iSort = GetSetting(App.Title, arRems(1).sName, fd(3).sName & " Sort", 3)
fd(3).sPrefix = GetSetting(App.Title, arRems(1).sName, fd(3).sName & " Prefix", "Body: ")
arRems(1).flds = fd

arRems(2).sName = "Task"
arRems(2).bSet = GetSetting(App.Title, arRems(2).sName, "Enabled", True)
ReDim fd(2)
fd(0).sName = "Subject"
fd(0).bSet = GetSetting(App.Title, arRems(2).sName, fd(0).sName & " Set", True)
fd(0).iSort = GetSetting(App.Title, arRems(2).sName, fd(0).sName & " Sort", 0)
fd(0).sPrefix = GetSetting(App.Title, arRems(2).sName, fd(0).sName & " Prefix", "Sbjt: ")
fd(1).sName = "Due Date"
fd(1).bSet = GetSetting(App.Title, arRems(2).sName, fd(1).sName & " Set", True)
fd(1).iSort = GetSetting(App.Title, arRems(2).sName, fd(1).sName & " Sort", 1)
fd(1).sPrefix = GetSetting(App.Title, arRems(2).sName, fd(1).sName & " Prefix", "Due: ")
fd(2).sName = "Body"
fd(2).bSet = GetSetting(App.Title, arRems(2).sName, fd(2).sName & " Set", False)
fd(2).iSort = GetSetting(App.Title, arRems(2).sName, fd(2).sName & " Sort", 2)
fd(2).sPrefix = GetSetting(App.Title, arRems(2).sName, fd(2).sName & " Prefix", "Body: ")
arRems(2).flds = fd

ReDim arNewMail(3)

blnNewMail = GetSetting(App.Title, "New Mail", "Enabled", True)

arNewMail(0).sName = "From"
arNewMail(0).bSet = GetSetting(App.Title, "New Mail", arNewMail(0).sName & " Set", True)
arNewMail(0).sPrefix = GetSetting(App.Title, "New Mail", arNewMail(0).sName & " Prefix", "From ")
arNewMail(0).iSort = GetSetting(App.Title, "New Mail", arNewMail(0).sName & " Sort", 0)
arNewMail(1).sName = "Sent"
arNewMail(1).bSet = GetSetting(App.Title, "New Mail", arNewMail(1).sName & " Set", True)
arNewMail(1).sPrefix = GetSetting(App.Title, "New Mail", arNewMail(1).sName & " Prefix", "Sent ")
arNewMail(1).iSort = GetSetting(App.Title, "New Mail", arNewMail(1).sName & " Sort", 1)
arNewMail(2).sName = "Subject"
arNewMail(2).bSet = GetSetting(App.Title, "New Mail", arNewMail(2).sName & " Set", True)
arNewMail(2).sPrefix = GetSetting(App.Title, "New Mail", arNewMail(2).sName & " Prefix", "Subject: ")
arNewMail(2).iSort = GetSetting(App.Title, "New Mail", arNewMail(2).sName & " Sort", 2)
arNewMail(3).sName = "Body"
arNewMail(3).bSet = GetSetting(App.Title, "New Mail", arNewMail(3).sName & " Set", True)
arNewMail(3).sPrefix = GetSetting(App.Title, "New Mail", arNewMail(3).sName & " Prefix", "Body: ")
arNewMail(3).iSort = GetSetting(App.Title, "New Mail", arNewMail(3).sName & " Sort", 3)

sEmailAddress = GetSetting(App.Title, "Settings", "Email Address", "41477022xx@page.nextel.com")
blnAutoStart = GetSetting(App.Title, "Settings", "Auto Start", False)
blnAutoMinimize = GetSetting(App.Title, "Settings", "Auto Minimize", False)

sNewMailPrefix = GetSetting(App.Title, "New Mail", "Prefix", "New mail received.")

Exit Sub
LoadSettings_Err:
ErrHand m_ModName, "LoadSettings", Err.Description, Err.Number
Exit Sub

End Sub

Public Function Status(sMsg As String)
On Error GoTo Status_Err

frmOlStatus.lblCurrentStatus.Caption = sMsg

With frmOlStatus.txtStatus
  .SelStart = Len(.Text)
  .SelLength = 0
  .SelText = Format$(Now, "m/d h:Nn:Ss") & "  " & sMsg & vbCrLf
End With

Exit Function
Status_Err:
ErrHand m_ModName, "Status", Err.Description, Err.Number
Exit Function
End Function

Public Function ErrHand(sModule As String, sFunction As String, sError As String, iError As Long)
On Error Resume Next

Dim sMsg As String

Status "ERROR - " & sModule & ": " & sError
sMsg = "There was an unhandled error encountered.  The error information is below." & vbCrLf
sMsg = sMsg & "Module: " & sModule & ", Function: " & sFunction & vbCrLf
sMsg = sMsg & "Error #: " & iError & "; " & sError
MsgBox sMsg, vbCritical

End Function

Public Function Pause(f As Form)
f.Visible = True
While f.Visible
  DoEvents
Wend
End Function
