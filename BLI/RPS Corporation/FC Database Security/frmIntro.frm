VERSION 5.00
Begin VB.Form frmIntro 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Factory Cat Database Security"
   ClientHeight    =   4500
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6435
   Icon            =   "frmIntro.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4500
   ScaleWidth      =   6435
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkAdvanced 
      Caption         =   "Adjust advanced &settings"
      Height          =   255
      Left            =   3120
      TabIndex        =   1
      Top             =   3600
      Width           =   2655
   End
   Begin VB.CommandButton cmdAbout 
      Caption         =   "&About"
      Height          =   375
      Left            =   120
      TabIndex        =   2
      Top             =   4080
      Width           =   975
   End
   Begin VB.PictureBox Picture2 
      BackColor       =   &H80000005&
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   720
      Picture         =   "frmIntro.frx":08CA
      ScaleHeight     =   615
      ScaleWidth      =   615
      TabIndex        =   11
      Top             =   3120
      Width           =   615
   End
   Begin VB.CommandButton cmdBack 
      Caption         =   "< &Back"
      Enabled         =   0   'False
      Height          =   375
      Left            =   3240
      TabIndex        =   3
      Top             =   4080
      Width           =   975
   End
   Begin VB.Frame fraLine 
      Height          =   30
      Left            =   -120
      TabIndex        =   10
      Top             =   3960
      Width           =   6615
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "&Next >"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   4
      Top             =   4080
      Width           =   975
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   5400
      TabIndex        =   5
      Top             =   4080
      Width           =   975
   End
   Begin VB.TextBox txtPassword 
      Height          =   285
      IMEMode         =   3  'DISABLE
      Left            =   2280
      PasswordChar    =   "*"
      TabIndex        =   0
      Top             =   2160
      Width           =   2535
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   1575
      Left            =   120
      Picture         =   "frmIntro.frx":1194
      ScaleHeight     =   1575
      ScaleWidth      =   1695
      TabIndex        =   7
      Top             =   120
      Width           =   1695
   End
   Begin VB.Label Label3 
      Caption         =   "Enter the administrator password to continue, then click 'Next'."
      Height          =   495
      Left            =   2280
      TabIndex        =   9
      Top             =   1680
      Width           =   2415
   End
   Begin VB.Label Label2 
      Caption         =   "This program allows you to change security settings for the Factory Cat database."
      Height          =   615
      Left            =   2280
      TabIndex        =   8
      Top             =   840
      Width           =   3135
   End
   Begin VB.Label Label1 
      Caption         =   "Factory Cat Database Security"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   13.5
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   2280
      TabIndex        =   6
      Top             =   120
      Width           =   4095
   End
   Begin VB.Shape Shape1 
      BackStyle       =   1  'Opaque
      BorderStyle     =   0  'Transparent
      FillColor       =   &H80000005&
      Height          =   3975
      Left            =   0
      Top             =   0
      Width           =   2055
   End
End
Attribute VB_Name = "frmIntro"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean

Private Sub cmdAbout_Click()
Load frmAbout
frmAbout.Show 1, Me
End Sub

Private Sub cmdCancel_Click()
Me.Visible = False
End Sub

Private Sub cmdNext_Click()
If LCase(Me.txtPassword) <> "bonehead" And LCase(Me.txtPassword) <> "bone" Then
  MsgBox "You do not have permission to use this program.", vbExclamation
  Me.Visible = False
Else
  blnCancel = False
  Me.Visible = False
End If
End Sub

Private Sub Form_Load()
blnCancel = True
End Sub
