VERSION 5.00
Begin VB.Form frmOldDataWarning 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Old Data Warning"
   ClientHeight    =   2100
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4740
   Icon            =   "frmOldDataWarning.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2100
   ScaleWidth      =   4740
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CheckBox chkDoNotShow 
      Caption         =   "&Do not show this warning message again"
      Height          =   255
      Left            =   720
      TabIndex        =   4
      Top             =   1800
      Width           =   3495
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&No, continue"
      Height          =   375
      Left            =   2520
      TabIndex        =   3
      Top             =   1320
      Width           =   1335
   End
   Begin VB.CommandButton cmdUpdate 
      Caption         =   "&Yes, update"
      Default         =   -1  'True
      Height          =   375
      Left            =   1080
      TabIndex        =   2
      Top             =   1320
      Width           =   1335
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   735
      Left            =   120
      Picture         =   "frmOldDataWarning.frx":0442
      ScaleHeight     =   735
      ScaleWidth      =   735
      TabIndex        =   0
      Top             =   240
      Width           =   735
   End
   Begin VB.Label Label1 
      Caption         =   $"frmOldDataWarning.frx":0884
      Height          =   855
      Left            =   840
      TabIndex        =   1
      Top             =   240
      Width           =   3735
   End
End
Attribute VB_Name = "frmOldDataWarning"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public bUpdate As Boolean

Private Sub cmdCancel_Click()
If Me.chkDoNotShow = 1 Then
  SaveSetting App.Title, "Options", "Hide Update Warning", True
End If
bUpdate = False
Me.Visible = False
End Sub

Private Sub cmdUpdate_Click()
If Me.chkDoNotShow = 1 Then
  SaveSetting App.Title, "Options", "Hide Update Warning", True
End If
bUpdate = True
Me.Visible = False
End Sub

