VERSION 5.00
Begin VB.Form frmWait 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Please wait..."
   ClientHeight    =   675
   ClientLeft      =   5355
   ClientTop       =   4770
   ClientWidth     =   3885
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   675
   ScaleWidth      =   3885
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
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
   Begin VB.Shape boxInner 
      FillColor       =   &H8000000D&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   120
      Shape           =   4  'Rounded Rectangle
      Top             =   480
      Width           =   2415
   End
   Begin VB.Shape boxOuter 
      Height          =   135
      Left            =   120
      Shape           =   4  'Rounded Rectangle
      Top             =   480
      Width           =   3615
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
    
Me.tmrCancel.Interval = 0
Me.Visible = False
Me.bolCancel = True

End Sub

Private Sub Form_Load()

If Not IsNull(prtName) And prtName <> "" Then
    Me.Caption = Me.Caption & "  (" & prtName & ")"
End If

End Sub

Private Sub tmrCancel_Timer()

timeDelayCur = timeDelayCur + Me.tmrCancel.Interval

Me.boxInner.Width = Me.boxOuter.Width * (timeDelayCur / timeDelaySecs)

If timeDelayCur >= timeDelaySecs Then
    Me.bolCancel = False
    Me.Visible = False
    Me.tmrCancel.Interval = 0
End If

End Sub
