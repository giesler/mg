VERSION 5.00
Begin VB.Form frmStatus 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Factory Cat Database Security"
   ClientHeight    =   4500
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6390
   Icon            =   "frmStatus.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4500
   ScaleWidth      =   6390
   StartUpPosition =   2  'CenterScreen
   Begin VB.PictureBox Picture1 
      BackColor       =   &H80000005&
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   5640
      Picture         =   "frmStatus.frx":08CA
      ScaleHeight     =   615
      ScaleWidth      =   615
      TabIndex        =   6
      Top             =   120
      Width           =   615
   End
   Begin VB.CommandButton cmdBack 
      Caption         =   "&Back"
      Enabled         =   0   'False
      Height          =   375
      Left            =   3120
      TabIndex        =   5
      Top             =   4080
      Width           =   975
   End
   Begin VB.Frame fraLine 
      Height          =   30
      Left            =   -240
      TabIndex        =   4
      Top             =   3960
      Width           =   6615
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Enabled         =   0   'False
      Height          =   375
      Left            =   5280
      TabIndex        =   3
      Top             =   4080
      Width           =   975
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "&Next >"
      Default         =   -1  'True
      Enabled         =   0   'False
      Height          =   375
      Left            =   4200
      TabIndex        =   2
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblStatus 
      BackStyle       =   0  'Transparent
      Caption         =   "Please wait..."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   855
      Left            =   480
      TabIndex        =   7
      Top             =   1200
      Width           =   5415
   End
   Begin VB.Label Label2 
      BackStyle       =   0  'Transparent
      Height          =   255
      Left            =   480
      TabIndex        =   1
      Top             =   480
      Width           =   5775
   End
   Begin VB.Label Label1 
      BackStyle       =   0  'Transparent
      Caption         =   "Please wait..."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   6015
   End
   Begin VB.Shape Shape1 
      BackStyle       =   1  'Opaque
      BorderColor     =   &H80000005&
      Height          =   855
      Left            =   0
      Top             =   0
      Width           =   6495
   End
End
Attribute VB_Name = "frmStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean
Public cnn As Connection
Public blnBack As Boolean

Private Sub cmdBack_Click()
blnBack = True
Me.Visible = False
End Sub

Private Sub cmdCancel_Click()
blnCancel = True
Me.Visible = False
End Sub

Private Sub cmdNext_Click()
Me.Visible = False
End Sub

Private Sub Form_Load()
blnCancel = False
blnBack = False
End Sub
