VERSION 5.00
Begin VB.Form frmDebug 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Debug Form"
   ClientHeight    =   3195
   ClientLeft      =   630
   ClientTop       =   330
   ClientWidth     =   6645
   Icon            =   "frmDebug.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3195
   ScaleWidth      =   6645
   Begin VB.TextBox txtDebugMessages 
      Height          =   2535
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Top             =   120
      Width           =   6375
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&Close && Cancel"
      Height          =   375
      Left            =   4920
      TabIndex        =   0
      Top             =   2760
      Width           =   1455
   End
   Begin VB.Label lblVersion 
      Caption         =   "Version x.xx.xx"
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   2880
      Width           =   2295
   End
End
Attribute VB_Name = "frmDebug"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdClose_Click()
End

End Sub

Private Sub Form_Load()
    lblVersion.Caption = "Version " & App.Major & "." & App.Minor & "." & App.Revision
End Sub
