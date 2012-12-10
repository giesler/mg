VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "Mscomctl.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "Tabctl32.ocx"
Begin VB.Form frmOLSettings 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Outlook Event Notification Settings"
   ClientHeight    =   3315
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5370
   Icon            =   "OLEvents Settings.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3315
   ScaleWidth      =   5370
   StartUpPosition =   3  'Windows Default
   Begin MSComctlLib.ImageList ilSmallReminders 
      Left            =   240
      Top             =   2880
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   3
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "OLEvents Settings.frx":0442
            Key             =   "Task"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "OLEvents Settings.frx":075C
            Key             =   "Mail Item"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "OLEvents Settings.frx":0A76
            Key             =   "Appointment"
         EndProperty
      EndProperty
   End
   Begin TabDlg.SSTab SSTab1 
      Height          =   2655
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   5175
      _ExtentX        =   9128
      _ExtentY        =   4683
      _Version        =   393216
      Style           =   1
      Tab             =   2
      TabHeight       =   520
      TabCaption(0)   =   "General"
      TabPicture(0)   =   "OLEvents Settings.frx":0D90
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "chkAutoMinimize"
      Tab(0).Control(1)=   "chkReminder"
      Tab(0).Control(2)=   "Picture2"
      Tab(0).Control(3)=   "Picture1"
      Tab(0).Control(4)=   "chkNewMail"
      Tab(0).Control(5)=   "chkAutoStart"
      Tab(0).ControlCount=   6
      TabCaption(1)   =   "Reminders"
      TabPicture(1)   =   "OLEvents Settings.frx":0DAC
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "cmdMoveDown"
      Tab(1).Control(1)=   "cmdMoveUp"
      Tab(1).Control(2)=   "txtEmailAddress"
      Tab(1).Control(3)=   "lvRemFields"
      Tab(1).Control(4)=   "tvReminders"
      Tab(1).Control(5)=   "Label1"
      Tab(1).ControlCount=   6
      TabCaption(2)   =   "New Mail"
      TabPicture(2)   =   "OLEvents Settings.frx":0DC8
      Tab(2).ControlEnabled=   -1  'True
      Tab(2).Control(0)=   "Label2"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).Control(1)=   "lvNewMail"
      Tab(2).Control(1).Enabled=   0   'False
      Tab(2).Control(2)=   "txtNewMailPrefix"
      Tab(2).Control(2).Enabled=   0   'False
      Tab(2).ControlCount=   3
      Begin VB.TextBox txtNewMailPrefix 
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   2160
         TabIndex        =   16
         ToolTipText     =   "Sets the email address the reminder will be sent to."
         Top             =   480
         Width           =   2775
      End
      Begin VB.CheckBox chkAutoMinimize 
         Caption         =   "&Minimize to traybar when program is launched"
         Height          =   255
         Left            =   -74880
         TabIndex        =   15
         Top             =   840
         Width           =   4815
      End
      Begin VB.CheckBox chkReminder 
         Caption         =   "&Send reminder notifications"
         Height          =   255
         Left            =   -74160
         TabIndex        =   14
         Top             =   1320
         Width           =   3855
      End
      Begin VB.PictureBox Picture2 
         BorderStyle     =   0  'None
         Height          =   495
         Left            =   -74880
         Picture         =   "OLEvents Settings.frx":0DE4
         ScaleHeight     =   495
         ScaleWidth      =   615
         TabIndex        =   13
         Top             =   1200
         Width           =   615
      End
      Begin VB.PictureBox Picture1 
         BorderStyle     =   0  'None
         Height          =   495
         Left            =   -74880
         Picture         =   "OLEvents Settings.frx":10EE
         ScaleHeight     =   495
         ScaleWidth      =   615
         TabIndex        =   12
         Top             =   1800
         Width           =   615
      End
      Begin VB.CheckBox chkNewMail 
         Caption         =   "&Announce new messages when they arrive"
         Height          =   255
         Left            =   -74160
         TabIndex        =   11
         Top             =   1920
         Width           =   3855
      End
      Begin VB.CommandButton cmdMoveDown 
         Caption         =   "&Down"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -70560
         TabIndex        =   9
         Top             =   1320
         Width           =   615
      End
      Begin VB.CommandButton cmdMoveUp 
         Caption         =   "&Up"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -70560
         TabIndex        =   8
         Top             =   960
         Width           =   615
      End
      Begin VB.TextBox txtEmailAddress 
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   -74040
         TabIndex        =   6
         ToolTipText     =   "Sets the email address the reminder will be sent to."
         Top             =   480
         Width           =   3975
      End
      Begin MSComctlLib.ListView lvRemFields 
         Height          =   1575
         Left            =   -72960
         TabIndex        =   5
         Top             =   840
         Width           =   2295
         _ExtentX        =   4048
         _ExtentY        =   2778
         View            =   1
         LabelEdit       =   1
         LabelWrap       =   -1  'True
         HideSelection   =   -1  'True
         Checkboxes      =   -1  'True
         _Version        =   393217
         ForeColor       =   -2147483640
         BackColor       =   -2147483643
         BorderStyle     =   1
         Appearance      =   1
         NumItems        =   0
      End
      Begin MSComctlLib.TreeView tvReminders 
         Height          =   1575
         Left            =   -74760
         TabIndex        =   4
         Top             =   840
         Width           =   1695
         _ExtentX        =   2990
         _ExtentY        =   2778
         _Version        =   393217
         Indentation     =   441
         LabelEdit       =   1
         Style           =   7
         Checkboxes      =   -1  'True
         ImageList       =   "ilSmallReminders"
         Appearance      =   1
      End
      Begin VB.CheckBox chkAutoStart 
         Caption         =   "Start watching for events when program is launched."
         Height          =   255
         Left            =   -74880
         TabIndex        =   3
         Top             =   480
         Width           =   4815
      End
      Begin MSComctlLib.ListView lvNewMail 
         Height          =   1575
         Left            =   240
         TabIndex        =   10
         Top             =   960
         Width           =   2535
         _ExtentX        =   4471
         _ExtentY        =   2778
         View            =   1
         LabelEdit       =   1
         LabelWrap       =   -1  'True
         HideSelection   =   -1  'True
         Checkboxes      =   -1  'True
         _Version        =   393217
         ForeColor       =   -2147483640
         BackColor       =   -2147483643
         BorderStyle     =   1
         Appearance      =   1
         NumItems        =   0
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "&Prefix mail messages with:"
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   120
         TabIndex        =   17
         Top             =   480
         Width           =   1935
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Email:"
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -74880
         TabIndex        =   7
         Top             =   480
         Width           =   735
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   4080
      TabIndex        =   1
      Top             =   2880
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2880
      TabIndex        =   0
      Top             =   2880
      Width           =   1095
   End
End
Attribute VB_Name = "frmOLSettings"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public blnCancel As Boolean
Private Const m_ModName = "frmOLSettings"

Private Sub cmdCancel_Click()
On Error GoTo cmdCancel_Err
LoadSettings
blnCancel = True
Me.Visible = False
Exit Sub

cmdCancel_Err:
ErrHand m_ModName, "cmdCancel", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub cmdOK_Click()
On Error GoTo cmdOK_Err

blnCancel = False
Me.Visible = False

'save settings
SaveSetting App.Title, "Settings", "Reminder", IIf(Me.chkReminder = 1, True, False)
blnReminder = IIf(Me.chkReminder = 1, True, False)
SaveSetting App.Title, "Settings", "Email Address", Me.txtEmailAddress
sEmailAddress = Me.txtEmailAddress
Dim i As Byte, fld As Byte
For i = 0 To UBound(arRems, 1)
  SaveSetting App.Title, arRems(i).sName, "Enabled", arRems(i).bSet
  For fld = 0 To UBound(arRems(i).flds, 1)
    SaveSetting App.Title, arRems(i).sName, arRems(i).flds(fld).sName & " Set", arRems(i).flds(fld).bSet
    SaveSetting App.Title, arRems(i).sName, arRems(i).flds(fld).sName & " Sort", arRems(i).flds(fld).iSort
    SaveSetting App.Title, arRems(i).sName, arRems(i).flds(fld).sName & " Prefix", arRems(i).flds(fld).sPrefix
  Next fld
Next i

SaveSetting App.Title, "New Mail", "Enabled", IIf(Me.chkNewMail = 1, True, False)

'save new mail settings
For i = 0 To UBound(arNewMail, 1)
  SaveSetting App.Title, "New Mail", arNewMail(i).sName & " Set", arNewMail(i).bSet
  SaveSetting App.Title, "New Mail", arNewMail(i).sName & " Sort", arNewMail(i).iSort
  SaveSetting App.Title, "New Mail", arNewMail(i).sName & " Prefix", arNewMail(i).sPrefix
Next i

SaveSetting App.Title, "Settings", "Email Address", Me.txtEmailAddress
blnNewMail = IIf(Me.chkNewMail = 1, True, False)

SaveSetting App.Title, "Settings", "Auto Start", IIf(Me.chkAutoStart = 1, True, False)
blnAutoStart = IIf(Me.chkAutoStart = 1, True, False)

SaveSetting App.Title, "Settings", "Auto Minimize", IIf(Me.chkAutoMinimize = 1, True, False)
blnAutoMinimize = IIf(Me.chkAutoMinimize = 1, True, False)

SaveSetting App.Title, "New Mail", "Prefix", Me.txtNewMailPrefix
sNewMailPrefix = Me.txtNewMailPrefix

Exit Sub
cmdOK_Err:
ErrHand m_ModName, "cmdOK", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub Form_Load()
On Error GoTo Form_Load_Err

Me.Left = Screen.Width - Me.Width - 100
Me.Top = Screen.Height - Me.Height - 800

Me.chkReminder = IIf(blnReminder, 1, 0)
Me.txtEmailAddress = sEmailAddress
Dim i As Integer, n As Node, li As ListItem
For i = 0 To UBound(arRems, 1)
  Set n = Me.tvReminders.Nodes.Add(, , , arRems(i).sName, arRems(i).sName)
  n.Checked = arRems(i).bSet
Next i

Me.chkNewMail = IIf(blnNewMail, 1, 0)
For i = 0 To UBound(arNewMail, 1)
  Set li = Me.lvNewMail.ListItems.Add(, , arNewMail(i).sName)
  li.Checked = arNewMail(i).bSet
Next i

Me.chkAutoStart = IIf(blnAutoStart, 1, 0)
Me.chkAutoMinimize = IIf(blnAutoMinimize, 1, 0)

Exit Sub
Form_Load_Err:
ErrHand m_ModName, "Form_Load", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub lvNewMail_DblClick()
On Error GoTo lvNewMail_DblClick_Err

Dim n As Node, li As ListItem, i As Byte, fld As Byte
Dim f As frmOLSetPrefix

Set li = Me.lvNewMail.SelectedItem

' find field
For fld = 0 To UBound(arNewMail, 1)
  If li.Text = arNewMail(fld).sName Then Exit For
Next fld

Set f = New frmOLSetPrefix
f.txtPrefix = arNewMail(fld).sPrefix
f.Left = Me.Left + Me.Width / 2 - f.Width / 2
f.Top = Me.Top + Me.Height / 2 - f.Height / 2
Pause f
If Not f.blnCancel Then
  arNewMail(fld).sPrefix = f.txtPrefix
End If
Set f = Nothing

Exit Sub
lvNewMail_DblClick_Err:
ErrHand m_ModName, "lvNewMail_DblClick", Err.Description, Err.Number
Exit Sub

End Sub

Private Sub lvNewMail_ItemCheck(ByVal Item As MSComctlLib.ListItem)
On Error GoTo lvNewMail_ItemCheck_Err

Dim li As ListItem, i As Byte
Set li = Item
For i = 0 To UBound(arNewMail, 1)
  If li.Text = arNewMail(i).sName Then Exit For
Next i

arNewMail(i).bSet = li.Checked

Exit Sub
lvNewMail_ItemCheck_Err:
ErrHand m_ModName, "lvNewMail_ItemCheck", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub lvRemFields_DblClick()
On Error GoTo lvRemFields_DblClick_Err

Dim n As Node, li As ListItem, i As Byte, fld As Byte
Dim f As frmOLSetPrefix

Set n = Me.tvReminders.SelectedItem
Set li = Me.lvRemFields.SelectedItem

' find array entry
For i = 0 To UBound(arRems, 1)
  If n.Text = arRems(i).sName Then Exit For
Next i

' find field
For fld = 0 To UBound(arRems(i).flds, 1)
  If li.Text = arRems(i).flds(fld).sName Then Exit For
Next fld

Set f = New frmOLSetPrefix
f.txtPrefix = arRems(i).flds(fld).sPrefix
f.Left = Me.Left + Me.Width / 2 - f.Width / 2
f.Top = Me.Top + Me.Height / 2 - f.Height / 2
Pause f
If Not f.blnCancel Then
  arRems(i).flds(fld).sPrefix = f.txtPrefix
End If
Set f = Nothing

Exit Sub
lvRemFields_DblClick_Err:
ErrHand m_ModName, "lvRemFields_DblClick", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub lvRemFields_ItemCheck(ByVal Item As MSComctlLib.ListItem)
On Error GoTo lvRemFields_ItemCheck_Err

Dim n As Node, li As ListItem, i As Byte, fld As Byte
Set n = Me.tvReminders.SelectedItem
Set li = Item

' find array entry
For i = 0 To UBound(arRems, 1)
  If n.Text = arRems(i).sName Then Exit For
Next i

' find field
For fld = 0 To UBound(arRems(i).flds, 1)
  If li.Text = arRems(i).flds(fld).sName Then Exit For
Next fld

arRems(i).flds(fld).bSet = li.Checked

Exit Sub
lvRemFields_ItemCheck_Err:
ErrHand m_ModName, "lvRemFields_ItemCheck", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub tvReminders_NodeCheck(ByVal Node As MSComctlLib.Node)
On Error GoTo tvReminders_NodeCheck_Err

Dim i As Byte
For i = 0 To UBound(arRems, 1)
  If Node.Text = arRems(i).sName Then Exit For
Next i
arRems(i).bSet = Node.Checked


Exit Sub
tvReminders_NodeCheck_Err:
ErrHand m_ModName, "tvReminders_NodeCheck", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub tvReminders_NodeClick(ByVal Node As MSComctlLib.Node)
On Error GoTo tvReminders_NodeClick_Err

Dim i As Byte, fld As Byte, li As ListItem

' find array entry
For i = 0 To UBound(arRems, 1)
  If Node.Text = arRems(i).sName Then Exit For
Next i

Me.lvRemFields.ListItems.Clear
For fld = 0 To UBound(arRems(i).flds, 1)
  Set li = Me.lvRemFields.ListItems.Add(, , arRems(i).flds(fld).sName)
  If arRems(i).flds(fld).bSet Then li.Checked = True
Next fld

Exit Sub
tvReminders_NodeClick_Err:
ErrHand m_ModName, "tvReminders_NodeClick", Err.Description, Err.Number
Exit Sub
End Sub
