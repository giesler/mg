VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clOutlook"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Dim WithEvents myOlApp As Outlook.Application
Attribute myOlApp.VB_VarHelpID = -1
Private Const m_ModName = "clOutlook"

Sub Initialize_handler()
On Error GoTo Init_Handler_Err

Set myOlApp = CreateObject("Outlook.Application")

Exit Sub
Init_Handler_Err:
ErrHand m_ModName, "Init_Handler", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub myOlApp_NewMail()
On Error GoTo myOlApp_NewMail_Err

Status "New mail received"

Dim myExp As Outlook.Explorers, mi As Outlook.MailItem
Dim myFld As Outlook.MAPIFolder
Set myExp = myOlApp.Explorers
Set myFld = myOlApp.GetNamespace("MAPI").GetDefaultFolder(olFolderInbox)
Dim sMsg As String
For Each mi In myFld.Items
  If mi.UnRead Then
    If sMsg <> "" Then sMsg = sMsg & ", "
    sMsg = BuildNewMailMsg(mi)
  End If
Next mi
If sMsg <> "" Then
  sMsg = sNewMailPrefix & sMsg
  If oVoice Is Nothing Then
    Set oVoice = New VTxtAuto.VTxtAuto
    oVoice.Register App.Title, App.EXEName
    oVoice.Callback = "OutlookEvents.clVTxtCallback"
  End If
  oVoice.Speak sMsg, vtxtstr_READING
End If

Exit Sub
myOlApp_NewMail_Err:
ErrHand m_ModName, "NewMail", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub myOlApp_Reminder(ByVal Item As Object)
On Error GoTo myOlApp_Reminder_Err

Dim sMsg As String, mi As Outlook.MailItem
Dim ai As Outlook.AppointmentItem, ti As Outlook.TaskItem
Dim oMsg As Outlook.MailItem


If arRems(1).bSet And TypeName(Item) = "MailItem" Then
  Set mi = Item
  sMsg = "From: " & mi.SenderName & " | "
  sMsg = sMsg & "Date: " & mi.SentOn & " | "
  sMsg = sMsg & "Sbjt: " & mi.Subject & " | "
  sMsg = sMsg & "Body: " & Left(mi.Body, 100)
ElseIf arRems(0).bSet And TypeName(Item) = "AppointmentItem" Then
  sMsg = BuildApptMsg(Item)
ElseIf arRems(3).bSet And TypeName(Item) = "TaskItem" Then
  sMsg = BuildApptMsg(Item)
Else
  Exit Sub
End If

Set oMsg = myOlApp.CreateItem(olMailItem)

oMsg.To = sEmailAddress
oMsg.Body = sMsg
oMsg.Send

frmOlStatus.lblCurrentStatus.Caption = "Sent " & TypeName(Item) & " at " & Time
Set oMsg = Nothing

Exit Sub
myOlApp_Reminder_Err:
ErrHand m_ModName, "Reminder", Err.Description, Err.Number
Exit Sub

End Sub

Private Function BuildApptMsg(appt As Outlook.AppointmentItem) As String
On Error GoTo BuildApptMsg_Err

Dim iSort As Byte, iEl As Byte, flds() As tpRemField, sMsg As String
Dim sTmp As String

For iEl = 0 To UBound(arRems, 1)
  If arRems(iEl).sName = "Appointment" Then Exit For
Next iEl
flds = arRems(iEl).flds

iSort = 0
For iSort = 0 To UBound(flds, 1)
  'find the element at current isort
  For iEl = 0 To UBound(flds, 1)
    If flds(iEl).iSort = iSort Then Exit For
  Next iEl
  'now see if we want to include this part
  If flds(iEl).bSet Then
    Select Case flds(iEl).sName
      Case "Subject"
        sTmp = appt.Subject
      Case "Location"
        sTmp = appt.Location
      Case "Start Time"
        sTmp = appt.Start
      Case "Body"
        sTmp = Left(appt.Body, 100)
    End Select
    If sTmp <> "" Then
      If sMsg <> "" Then sMsg = sMsg & " | "
      sMsg = sMsg & flds(iEl).sPrefix & sTmp
    End If
  End If
Next iSort

BuildApptMsg = sMsg

Exit Function
BuildApptMsg_Err:
ErrHand m_ModName, "BuildApptMsg", Err.Description, Err.Number
Exit Function
End Function

Private Function BuildTaskMsg(tsk As Outlook.TaskItem) As String
On Error GoTo BuildTaskMsg_Err

Dim iSort As Byte, iEl As Byte, flds() As tpRemField, sMsg As String
Dim sTmp As String

For iEl = 0 To UBound(arRems, 1)
  If arRems(iEl).sName = "Task" Then Exit For
Next iEl
flds = arRems(iEl).flds

iSort = 0
For iSort = 0 To UBound(flds, 1)
  'find the element at current isort
  For iEl = 0 To UBound(flds, 1)
    If flds(iEl).iSort = iSort Then Exit For
  Next iEl
  'now see if we want to include this part
  If flds(iEl).bSet Then
    Select Case flds(iEl).sName
      Case "Subject"
        sTmp = tsk.Subject
      Case "Due Date"
        sTmp = tsk.DueDate
      Case "Body"
        sTmp = Left(tsk.Body, 100)
    End Select
    If sTmp <> "" Then
      If sMsg <> "" Then sMsg = sMsg & " | "
      sMsg = sMsg & flds(iEl).sPrefix & sTmp
    End If
  End If
Next iSort

BuildTaskMsg = sMsg

Exit Function
BuildTaskMsg_Err:
ErrHand m_ModName, "BuildTaskMsg", Err.Description, Err.Number
Exit Function
End Function

Private Function BuildMailMsg(ml As Outlook.MailItem) As String
On Error GoTo BuildMailMsg_Err

Dim iSort As Byte, iEl As Byte, flds() As tpRemField, sMsg As String
Dim sTmp As String

For iEl = 0 To UBound(arRems, 1)
  If arRems(iEl).sName = "Mail Item" Then Exit For
Next iEl
flds = arRems(iEl).flds

iSort = 0
For iSort = 0 To UBound(flds, 1)
  'find the element at current isort
  For iEl = 0 To UBound(flds, 1)
    If flds(iEl).iSort = iSort Then Exit For
  Next iEl
  'now see if we want to include this part
  If flds(iEl).bSet Then
    Select Case flds(iEl).sName
      Case "Subject"
        sTmp = ml.Subject
      Case "From"
        sTmp = ml.SenderName
      Case "Sent"
        sTmp = ml.Sent
      Case "Body"
        sTmp = Left(ml.Body, 100)
    End Select
    If sTmp <> "" Then
      If sMsg <> "" Then sMsg = sMsg & " | "
      sMsg = sMsg & flds(iEl).sPrefix & sTmp
    End If
  End If
Next iSort

BuildMailMsg = sMsg

Exit Function
BuildMailMsg_Err:
ErrHand m_ModName, "BuildMailMsg", Err.Description, Err.Number
Exit Function
End Function


Private Function BuildNewMailMsg(ml As Outlook.MailItem) As String
On Error GoTo BuildNewMailMsg_Err

Dim iSort As Byte, iEl As Byte, flds() As tpRemField, sMsg As String
Dim sTmp As String

iSort = 0
For iSort = 0 To UBound(arNewMail, 1)
  'find the element at current isort
  For iEl = 0 To UBound(arNewMail, 1)
    If arNewMail(iEl).iSort = iSort Then Exit For
  Next iEl
  'now see if we want to include this part
  If arNewMail(iEl).bSet Then
    Select Case arNewMail(iEl).sName
      Case "Subject"
        sTmp = ml.Subject
      Case "Sent"
        sTmp = ml.SentOn
      Case "From"
        sTmp = ml.SenderName
      Case "Body"
        sTmp = Left(ml.Body, 100)
    End Select
    If sTmp <> "" Then
      If sMsg <> "" Then sMsg = sMsg & ", "
      sMsg = sMsg & arNewMail(iEl).sPrefix & sTmp
    End If
  End If
Next iSort

BuildNewMailMsg = sMsg

Exit Function
BuildNewMailMsg_Err:
ErrHand m_ModName, "BuildNewMailMsg", Err.Description, Err.Number
Exit Function
End Function


