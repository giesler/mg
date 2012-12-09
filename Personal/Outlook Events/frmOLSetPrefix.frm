VERSION 5.00
Begin VB.Form frmOLSetPrefix 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Set Prefix"
   ClientHeight    =   855
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   3240
   ControlBox      =   0   'False
   Icon            =   "frmOLSetPrefix.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   855
   ScaleWidth      =   3240
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   350
      Left            =   2280
      TabIndex        =   3
      Top             =   480
      Width           =   855
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   350
      Left            =   1320
      TabIndex        =   2
      Top             =   480
      Width           =   855
   End
   Begin VB.TextBox txtPrefix 
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   1560
      TabIndex        =   0
      ToolTipText     =   "Sets the email address the reminder will be sent to."
      Top             =   120
      Width           =   1575
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "&Prefix text with:"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   1335
   End
End
Attribute VB_Name = "frmOLSetPrefix"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean

Private Sub cmdCancel_Click()
blnCancel = True
Me.Visible = False
End Sub

Private Sub cmdOK_Click()
blnCancel = False
Me.Visible = False
End Sub
