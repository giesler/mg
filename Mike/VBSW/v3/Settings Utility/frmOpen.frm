VERSION 5.00
Begin VB.Form frmOpen 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Open..."
   ClientHeight    =   2280
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   3990
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2280
   ScaleWidth      =   3990
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   2760
      TabIndex        =   2
      Top             =   1800
      Width           =   1095
   End
   Begin VB.ListBox lstFile 
      Height          =   1035
      Left            =   120
      TabIndex        =   1
      Top             =   600
      Width           =   3735
   End
   Begin VB.Label Label1 
      BackColor       =   &H80000018&
      Caption         =   "Select the INI file you would like to open:"
      ForeColor       =   &H80000017&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3735
   End
End
Attribute VB_Name = "frmOpen"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdOK_Click()
  Visible = False
End Sub

Public Function SelectedFile() As String
  
  Dim i As Integer
  
  For i = 0 To lstFile.ListCount - 1
    If lstFile.Selected(i) Then
      SelectedFile = lstFile.List(i)
      Exit Function
    End If
  Next i

End Function

Private Sub lstFile_DblClick()
  cmdOK_Click
End Sub