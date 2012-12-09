VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmUpdate 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Database Update"
   ClientHeight    =   1050
   ClientLeft      =   810
   ClientTop       =   1575
   ClientWidth     =   5745
   Icon            =   "Update Form.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   1050
   ScaleWidth      =   5745
   Begin MSComctlLib.ProgressBar pbar 
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   5535
      _ExtentX        =   9763
      _ExtentY        =   450
      _Version        =   393216
      Appearance      =   1
      Scrolling       =   1
   End
   Begin VB.TextBox lblStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H8000000F&
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   120
      TabIndex        =   0
      TabStop         =   0   'False
      Text            =   "txtStatus"
      Top             =   120
      Width           =   5535
   End
   Begin VB.Timer Timer1 
      Left            =   4680
      Top             =   120
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


