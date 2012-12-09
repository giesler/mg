VERSION 5.00
Begin VB.Form frmInfo 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Database Update Program"
   ClientHeight    =   2985
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5160
   Icon            =   "frmInfo.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2985
   ScaleWidth      =   5160
   StartUpPosition =   2  'CenterScreen
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   120
      Picture         =   "frmInfo.frx":0442
      ScaleHeight     =   615
      ScaleWidth      =   735
      TabIndex        =   6
      Top             =   240
      Width           =   735
   End
   Begin VB.CommandButton cmdContinue 
      Caption         =   "&Continue"
      Default         =   -1  'True
      Height          =   375
      Left            =   2280
      TabIndex        =   2
      Top             =   2520
      Width           =   1215
   End
   Begin VB.CommandButton cmdExit 
      Cancel          =   -1  'True
      Caption         =   "E&xit"
      Height          =   375
      Left            =   3600
      TabIndex        =   3
      Top             =   2520
      Width           =   1215
   End
   Begin VB.CheckBox chkInstallMSCom 
      Caption         =   "&Install MSCOMCTL"
      Height          =   255
      Left            =   1200
      TabIndex        =   1
      Top             =   2040
      Width           =   3855
   End
   Begin VB.CheckBox chkRunDBUpdate 
      Caption         =   "&Run database update (will prompt for local dir)"
      Height          =   255
      Left            =   1200
      TabIndex        =   0
      Top             =   1680
      Width           =   3855
   End
   Begin VB.Label Label2 
      Caption         =   "/d:'<database directory>' [/a:'<MS Access Directory>'] [/i:<process id>] [/mscomctl]"
      Height          =   495
      Left            =   1080
      TabIndex        =   5
      Top             =   1080
      Width           =   3975
   End
   Begin VB.Label Label1 
      Caption         =   $"frmInfo.frx":0884
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   975
      Left            =   960
      TabIndex        =   4
      Top             =   120
      Width           =   4095
   End
End
Attribute VB_Name = "frmInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean

Private Sub cmdContinue_Click()
blnCancel = False
Me.Visible = False
End Sub

Private Sub cmdExit_Click()
blnCancel = True
Me.Visible = False
End Sub
