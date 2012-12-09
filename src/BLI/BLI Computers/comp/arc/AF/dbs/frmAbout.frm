VERSION 5.00
Begin VB.Form frmAbout 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "About..."
   ClientHeight    =   2910
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4605
   ControlBox      =   0   'False
   Icon            =   "frmAbout.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2910
   ScaleWidth      =   4605
   StartUpPosition =   2  'CenterScreen
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   360
      Picture         =   "frmAbout.frx":0442
      ScaleHeight     =   615
      ScaleWidth      =   615
      TabIndex        =   6
      Top             =   1440
      Width           =   615
   End
   Begin VB.CommandButton cmdOK 
      Cancel          =   -1  'True
      Caption         =   "&Close"
      Default         =   -1  'True
      Height          =   375
      Left            =   1560
      TabIndex        =   2
      Top             =   2400
      Width           =   1455
   End
   Begin VB.Label Label5 
      Caption         =   "(c) 1999"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   2520
      Width           =   1215
   End
   Begin VB.Line Line1 
      X1              =   0
      X2              =   4560
      Y1              =   960
      Y2              =   960
   End
   Begin VB.Label Label4 
      Caption         =   "Email: database@blicomputers.com"
      Height          =   375
      Left            =   1080
      TabIndex        =   5
      Top             =   1680
      Width           =   2775
   End
   Begin VB.Label Label3 
      Caption         =   "Phone:  (414) 633-2411"
      Height          =   255
      Left            =   1080
      TabIndex        =   4
      Top             =   1440
      Width           =   2775
   End
   Begin VB.Label Label2 
      Caption         =   "For more information, contact BLI Computers, Inc."
      Height          =   255
      Left            =   360
      TabIndex        =   3
      Top             =   1080
      Width           =   3855
   End
   Begin VB.Label lblVersion 
      Alignment       =   1  'Right Justify
      Caption         =   "Version"
      Height          =   225
      Left            =   3000
      TabIndex        =   1
      Top             =   2520
      Width           =   1485
   End
   Begin VB.Label Label1 
      Caption         =   "This program is for synchronizing TargetPRO data files between All Fasteners and All Tool Sales."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   4455
   End
End
Attribute VB_Name = "frmAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdOK_Click()

End

End Sub

Private Sub Form_Load()
    lblVersion.Caption = "Version " & App.Major & "." & App.Minor & "." & App.Revision
End Sub
