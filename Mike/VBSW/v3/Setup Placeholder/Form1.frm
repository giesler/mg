VERSION 5.00
Begin VB.Form Form1 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Install Assistant"
   ClientHeight    =   2730
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   7365
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2730
   ScaleWidth      =   7365
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdExit 
      Cancel          =   -1  'True
      Caption         =   "E&xit"
      Default         =   -1  'True
      Height          =   375
      Left            =   6000
      TabIndex        =   3
      Top             =   2160
      Width           =   1095
   End
   Begin VB.Label lblCommandLine 
      Caption         =   "(blank)"
      Height          =   255
      Left            =   1680
      TabIndex        =   2
      Top             =   1080
      Width           =   4095
   End
   Begin VB.Label Label2 
      Caption         =   "Command line used:"
      Height          =   255
      Left            =   1200
      TabIndex        =   1
      Top             =   720
      Width           =   4335
   End
   Begin VB.Label Label1 
      Caption         =   "This program is simply a placeholder for your setup program."
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4335
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdExit_Click()

    End
    
End Sub

Private Sub Form_Load()

    If Command <> "" Then
        lblCommandLine.Caption = Command
    End If
    
End Sub
