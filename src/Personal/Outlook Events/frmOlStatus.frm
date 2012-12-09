VERSION 5.00
Object = "{C7EDAA3D-6A82-11D2-8DE3-00403394A7F2}#1.0#0"; "SysTray.ocx"
Begin VB.Form frmOlStatus 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Outlook Event Watcher"
   ClientHeight    =   3615
   ClientLeft      =   150
   ClientTop       =   720
   ClientWidth     =   3720
   Icon            =   "frmOlStatus.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3615
   ScaleWidth      =   3720
   StartUpPosition =   3  'Windows Default
   Begin SysTrayCtl.cSysTray cSysTray1 
      Left            =   1680
      Top             =   3000
      _ExtentX        =   900
      _ExtentY        =   900
      InTray          =   -1  'True
      TrayIcon        =   "frmOlStatus.frx":0442
      TrayTip         =   "Outlook Event Watcher"
   End
   Begin VB.CommandButton cmdDetails 
      Caption         =   "&Details >>"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   300
      Left            =   2640
      TabIndex        =   3
      Top             =   480
      Width           =   975
   End
   Begin VB.TextBox txtStatus 
      Height          =   1455
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   2
      Top             =   840
      Width           =   3495
   End
   Begin VB.Frame Frame1 
      Height          =   30
      Left            =   -240
      TabIndex        =   1
      Top             =   0
      Width           =   5000
   End
   Begin VB.Image imgStatus 
      Height          =   480
      Left            =   120
      Picture         =   "frmOlStatus.frx":0894
      Top             =   120
      Width           =   480
   End
   Begin VB.Image img_Idle 
      Height          =   480
      Left            =   240
      Picture         =   "frmOlStatus.frx":0CD6
      Top             =   3000
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image img_Busy 
      Height          =   480
      Left            =   960
      Picture         =   "frmOlStatus.frx":1118
      Top             =   3000
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Label lblCurrentStatus 
      Caption         =   "Idle."
      Height          =   495
      Left            =   720
      TabIndex        =   0
      Top             =   120
      Width           =   2895
   End
   Begin VB.Menu mnuCommands 
      Caption         =   "&Commands"
      Begin VB.Menu mnuStartStop 
         Caption         =   "S&tart"
      End
      Begin VB.Menu mnuSettings 
         Caption         =   "&Settings"
      End
      Begin VB.Menu mnuShowHide 
         Caption         =   "&Hide"
      End
      Begin VB.Menu mnuBlank1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
      End
   End
End
Attribute VB_Name = "frmOlStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Const m_ModName = "frmOLStatus"

Private Sub cmdDetails_Click()
On Error GoTo cmdDetails_Err

If Me.cmdDetails.Caption = "&Details >>" Then
  Me.Height = 3000
  Me.cmdDetails.Caption = "<< &Details"
Else
  Me.Height = 1500
  Me.cmdDetails.Caption = "&Details >>"
End If

Exit Sub
cmdDetails_Err:
ErrHand m_ModName, "cmdDetails_Click", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub cSysTray1_MouseDblClick(Button As Integer, Id As Long)
mnuShowHide_Click
End Sub

Private Sub cSysTray1_MouseUp(Button As Integer, Id As Long)
On Error GoTo cSysTray_MouseUp_Err

'Set current window as foreground window
SetForegroundWindow Me.hwnd
If Button = vbRightButton Then
  Me.PopupMenu mnuCommands, vbPopupMenuRightButton
End If

PostMessage Me.hwnd, 0&, 0&, 0&            ' Update form...

Exit Sub
cSysTray_MouseUp_Err:
ErrHand m_ModName, "cSysTray_MouseUp_Err", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub Form_Load()
On Error GoTo Form_Load_Err

If blnAutoStart Then mnuStartStop_Click
Me.Left = Val(GetSetting(App.Title, Me.Name, "Left", Me.Left))
Me.Top = Val(GetSetting(App.Title, Me.Name, "Top", Me.Top))

If GetSetting(App.Title, Me.Name, "Details", False) Then
  Me.cmdDetails.Caption = "<< &Details"
  Me.Height = 3000
Else
  Me.cmdDetails.Caption = "&Details >>"
  Me.Height = 1500
End If

Exit Sub
Form_Load_Err:
ErrHand m_ModName, "Form_Load", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
On Error GoTo Form_QueryUnload_Err

SaveSetting App.Title, Me.Name, "Left", Me.Left
SaveSetting App.Title, Me.Name, "Top", Me.Top
SaveSetting App.Title, Me.Name, "Details", IIf(Me.cmdDetails.Caption = "&Details >>", False, True)
If blnActive Then Set cl1 = Nothing
End

Exit Sub
Form_QueryUnload_Err:
ErrHand m_ModName, "Form_QueryUnload", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub mnuExit_Click()
On Error GoTo mnuExit_Err

Unload Me
End

Exit Sub
mnuExit_Err:
ErrHand m_ModName, "mnuExit_Click", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub mnuSettings_Click()
On Error GoTo mnuSettings_Err

Dim fS As New frmOLSettings

fS.Visible = True
While fS.Visible
  DoEvents
Wend
Set fS = Nothing

Exit Sub
mnuSettings_Err:
ErrHand m_ModName, "mnuSettings", Err.Description, Err.Number
Exit Sub
End Sub

Private Sub mnuShowHide_Click()
On Error Resume Next
If Me.Visible Then
  Me.Visible = False
  Me.mnuShowHide.Caption = "S&how"
Else
  Me.Visible = True
  Me.mnuShowHide.Caption = "&Hide"
End If
End Sub

Private Sub mnuStartStop_Click()
On Error GoTo mnuStartStop_Err

If blnActive Then
  Me.mnuStartStop.Enabled = False
  Status "Disconnecting from Outlook..."
  Set cl1 = Nothing
  Set Me.imgStatus.Picture = Me.img_Busy.Picture
  Status "Idle."
  blnActive = False
  Me.mnuStartStop.Enabled = True
  Me.mnuStartStop.Caption = "S&tart"
Else
  Me.mnuStartStop.Enabled = False
  Status "Connecting to Outlook..."
  Set cl1 = New clOutlook
  cl1.Initialize_handler
  Status "Waiting for reminders..."
  Set Me.imgStatus.Picture = Me.img_Idle.Picture
  blnActive = True
  Me.mnuStartStop.Enabled = True
  Me.mnuStartStop.Caption = "S&top"
End If

Exit Sub
mnuStartStop_Err:
ErrHand m_ModName, "mnuStartStop", Err.Description, Err.Number
Exit Sub
End Sub
