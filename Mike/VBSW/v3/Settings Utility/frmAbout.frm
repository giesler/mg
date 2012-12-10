VERSION 5.00
Begin VB.Form frmAbout 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "About IA Settings Program"
   ClientHeight    =   2010
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3945
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2010
   ScaleWidth      =   3945
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdOK 
      Cancel          =   -1  'True
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   2760
      TabIndex        =   1
      Top             =   1560
      Width           =   1095
   End
   Begin VB.Label Label7 
      Caption         =   "http://installassistant.com"
      ForeColor       =   &H00C00000&
      Height          =   255
      Left            =   240
      TabIndex        =   3
      Top             =   600
      Width           =   2415
   End
   Begin VB.Label lblVersion 
      Caption         =   "Version x.x.x"
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   1680
      Width           =   2415
   End
   Begin VB.Label lblAbout 
      Caption         =   "This program creates settings files for VBSW.  For more information on this program or VBSW, visit "
      Height          =   735
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   3495
   End
End
Attribute VB_Name = "frmAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdOK_Click()

  Me.Visible = False
  
End Sub

Private Sub Form_Load()

  Me.lblVersion.Caption = "Version " & App.Major & "." & App.Minor & "." & App.Revision
  
End Sub

Private Sub Label7_Click()
  ExecuteLink "http://installassistant.com"
End Sub
