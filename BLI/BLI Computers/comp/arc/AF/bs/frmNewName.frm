VERSION 5.00
Begin VB.Form frmNewName 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Change Printer Name"
   ClientHeight    =   2160
   ClientLeft      =   2835
   ClientTop       =   3480
   ClientWidth     =   5445
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1276.199
   ScaleMode       =   0  'User
   ScaleWidth      =   5112.56
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.TextBox txtUserName 
      Height          =   345
      Left            =   120
      TabIndex        =   1
      Top             =   1680
      Width           =   3765
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   390
      Left            =   4200
      TabIndex        =   2
      Top             =   120
      Width           =   1140
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   390
      Left            =   4200
      TabIndex        =   3
      Top             =   600
      Width           =   1140
   End
   Begin VB.Label Label1 
      Caption         =   $"frmNewName.frx":0000
      Height          =   855
      Left            =   120
      TabIndex        =   4
      Top             =   600
      Width           =   3735
   End
   Begin VB.Label lblLabels 
      Caption         =   "Enter a new name which will identify this printer on the network"
      Height          =   375
      Index           =   0
      Left            =   105
      TabIndex        =   0
      Top             =   120
      Width           =   3720
   End
End
Attribute VB_Name = "frmNewName"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public LoginSucceeded As Boolean

Private Sub cmdCancel_Click()
    LoginSucceeded = False
    Me.Hide
End Sub

Private Sub cmdOK_Click()
    'Add the new name to registry
    INISaveSetting App.Title, "Settings", "Name", txtUserName.Text
    LoginSucceeded = True
    Me.Hide
    Status "Printer name changed to '" & txtUserName.Text & "'", False
End Sub

