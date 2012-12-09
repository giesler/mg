VERSION 5.00
Begin VB.Form frmNewPort 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Change Printer Port"
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
   Begin VB.Label lblLabels 
      Caption         =   $"frmNewPort.frx":0000
      Height          =   975
      Index           =   0
      Left            =   105
      TabIndex        =   0
      Top             =   120
      Width           =   3720
   End
End
Attribute VB_Name = "frmNewPort"
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
    'Add the new port to registry
    INISaveSetting App.Title, "Settings", "Port", txtUserName.Text
    LoginSucceeded = True
    Me.Hide
    Status "Port changed to '" & txtUserName.Text & "'", False
End Sub

