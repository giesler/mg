VERSION 5.00
Object = "{E88121A0-9FA9-11CF-9D9F-00AA003A3AA3}#1.0#0"; "ZLIBTOOL.OCX"
Begin VB.Form frmRunZL 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Compressing/Expanding..."
   ClientHeight    =   1155
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   5460
   ControlBox      =   0   'False
   LinkTopic       =   "Form2"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1155
   ScaleWidth      =   5460
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdCancel 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4320
      TabIndex        =   1
      Top             =   720
      Width           =   975
   End
   Begin ZLIBTOOLLib.ZlibTool zlocx 
      Height          =   135
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5175
      _Version        =   65536
      _ExtentX        =   9128
      _ExtentY        =   238
      _StockProps     =   0
   End
   Begin VB.Timer tmrTime 
      Left            =   4080
      Top             =   120
   End
   Begin VB.Label lblRemainTime 
      Alignment       =   1  'Right Justify
      Caption         =   "0:00"
      Height          =   255
      Left            =   2400
      TabIndex        =   7
      Top             =   840
      Width           =   615
   End
   Begin VB.Label Label4 
      Caption         =   "Remaining Time:"
      Height          =   255
      Left            =   600
      TabIndex        =   6
      Top             =   840
      Width           =   1695
   End
   Begin VB.Label Label3 
      Caption         =   "Elapsed Time:"
      Height          =   255
      Left            =   600
      TabIndex        =   5
      Top             =   600
      Width           =   1695
   End
   Begin VB.Label Label2 
      Caption         =   "Percent Complete:"
      Height          =   255
      Left            =   600
      TabIndex        =   4
      Top             =   360
      Width           =   1695
   End
   Begin VB.Label lblStatus 
      Alignment       =   1  'Right Justify
      Caption         =   "0%"
      Height          =   255
      Left            =   2400
      TabIndex        =   3
      Top             =   360
      Width           =   615
   End
   Begin VB.Label lblElapsedTime 
      Alignment       =   1  'Right Justify
      Caption         =   "0:00"
      Height          =   255
      Left            =   2400
      TabIndex        =   2
      Top             =   600
      Width           =   615
   End
End
Attribute VB_Name = "frmRunZL"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public txtSourceFile As String
Public txtDestFile As String
Public bolCancel As Boolean
Public bolCompress As Boolean

Private intProgress As Byte
Private numSecs As Long

Private Sub cmdCancel_Click()
If MsgBox("Are you sure you want to cancel?", vbQuestion + vbYesNo) = vbYes Then
    bolCancel = True
    Me.zlocx.SetFocus
    Me.cmdCancel.Enabled = False
End If
End Sub

Private Sub Form_Load()
numSecs = 0
End Sub

Private Sub tmrTime_Timer()

numSecs = numSecs + 1
Me.lblElapsedTime.Caption = Int(numSecs / 60) & ":" & Format(Int(numSecs Mod 60), "00")
Dim tLeft As Integer

If intProgress <> 0 Then
    tLeft = (numSecs / intProgress) * (100 - intProgress)
    Me.lblRemainTime.Caption = Int(tLeft / 60) & ":" & Format(Int(tLeft Mod 60), "00")
End If

End Sub

Private Sub zlocx_Progress(ByVal percent_complete As Integer)

intProgress = percent_complete
Me.lblStatus.Caption = percent_complete & "%"
If bolCancel Then
    Me.zlocx.Abort
    Me.Visible = False
End If
If percent_complete = 100 Then
    Me.lblStatus.Caption = Me.zlocx.Status
    Me.Visible = False
End If

End Sub
