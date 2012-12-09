VERSION 5.00
Begin VB.Form frmComCtl 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Database Update Program"
   ClientHeight    =   1515
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6720
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1515
   ScaleWidth      =   6720
   StartUpPosition =   2  'CenterScreen
   Begin VB.Timer tmrPause 
      Left            =   6240
      Top             =   1080
   End
   Begin VB.PictureBox Picture2 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   120
      Picture         =   "frmComCtl.frx":0000
      ScaleHeight     =   615
      ScaleWidth      =   495
      TabIndex        =   3
      Top             =   240
      Width           =   495
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Enabled         =   0   'False
      Height          =   375
      Left            =   3480
      TabIndex        =   2
      Top             =   1080
      Width           =   975
   End
   Begin VB.CommandButton cmdRestart 
      Caption         =   "&Restart"
      Enabled         =   0   'False
      Height          =   375
      Left            =   2400
      TabIndex        =   1
      Top             =   1080
      Width           =   975
   End
   Begin VB.Label Label1 
      Caption         =   $"frmComCtl.frx":0442
      Height          =   735
      Left            =   840
      TabIndex        =   0
      Top             =   120
      Width           =   5775
   End
End
Attribute VB_Name = "frmComCtl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdCancel_Click()

Dim msg As String

msg = "Are you sure you want to cancel?  If you cancel and attempt to open the database, you may receive error messages in the database."
If MsgBox(msg, vbQuestion Or vbYesNo) = vbYes Then
  End
End If

End Sub

Private Sub cmdRestart_Click()
On Error GoTo cmdRestart_Err

Dim result As Long
result = ExitWindowsEx(EWX_REBOOT, 0)
End

Exit Sub
cmdRestart_Err:
ErrHand Me.Name, "cmdRestart"
Exit Sub
End Sub

Private Sub Form_Activate()
If Not Me.cmdCancel.Enabled Then
  Me.tmrPause.Interval = 5000
End If
End Sub

Private Sub tmrPause_Timer()

If Not Me.cmdCancel.Enabled Then Me.cmdCancel.Enabled = True
If Not Me.cmdRestart.Enabled Then Me.cmdRestart.Enabled = True
Me.cmdRestart.SetFocus
Me.tmrPause.Interval = 0

End Sub
