VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmWait 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Please wait..."
   ClientHeight    =   720
   ClientLeft      =   5355
   ClientTop       =   4770
   ClientWidth     =   3885
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   720
   ScaleWidth      =   3885
   ShowInTaskbar   =   0   'False
   Begin MSComctlLib.ProgressBar pbar 
      Height          =   150
      Left            =   120
      TabIndex        =   2
      Top             =   480
      Width           =   3735
      _ExtentX        =   6588
      _ExtentY        =   265
      _Version        =   393216
      Appearance      =   1
      Scrolling       =   1
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Default         =   -1  'True
      Height          =   375
      Left            =   1320
      TabIndex        =   1
      Top             =   1200
      Visible         =   0   'False
      Width           =   1455
   End
   Begin VB.Timer tmrCancel 
      Left            =   360
      Top             =   1200
   End
   Begin VB.Label lblMessage 
      Caption         =   "#-Message"
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3615
   End
End
Attribute VB_Name = "frmWait"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public timeDelaySecs As Long
Public timeDelayCur As Long
Public bolCancel As Boolean

Private Sub cmdCancel_Click()
    
If MsgBox("Are you sure you want to stop waiting?", vbQuestion + vbYesNo) = vbYes Then
  Me.tmrCancel.Interval = 0
  Me.Visible = False
  Me.bolCancel = True
End If

End Sub

Private Sub Form_Load()

If Not IsNull(prtName) And prtName <> "" Then
    Me.Caption = Me.Caption & "  (" & prtName & ")"
End If

End Sub

Private Sub tmrCancel_Timer()

timeDelayCur = timeDelayCur + Me.tmrCancel.Interval

Me.pbar.Value = (timeDelayCur / timeDelaySecs) * 100

If timeDelayCur >= timeDelaySecs Then
    Me.bolCancel = False
    Me.Visible = False
    Me.tmrCancel.Interval = 0
End If

End Sub
