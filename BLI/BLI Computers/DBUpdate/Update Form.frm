VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmUpdate 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Database Update"
   ClientHeight    =   840
   ClientLeft      =   810
   ClientTop       =   1575
   ClientWidth     =   5745
   Icon            =   "Update Form.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   840
   ScaleMode       =   0  'User
   ScaleWidth      =   5745
   Begin MSComctlLib.ProgressBar pbar 
      Height          =   190
      Left            =   120
      TabIndex        =   2
      Top             =   600
      Width           =   5535
      _ExtentX        =   9763
      _ExtentY        =   344
      _Version        =   393216
      Appearance      =   1
      Scrolling       =   1
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   495
      Left            =   5040
      Picture         =   "Update Form.frx":0442
      ScaleHeight     =   495
      ScaleWidth      =   615
      TabIndex        =   1
      Top             =   0
      Width           =   615
   End
   Begin VB.Timer Timer1 
      Left            =   4200
      Top             =   120
   End
   Begin VB.Label lblStatus 
      Caption         =   "Loading..."
      Height          =   375
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5535
   End
End
Attribute VB_Name = "frmUpdate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub Timer1_Timer()
End
End Sub


