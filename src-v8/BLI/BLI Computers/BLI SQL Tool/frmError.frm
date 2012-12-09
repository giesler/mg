VERSION 5.00
Begin VB.Form frmError 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "SQL Error"
   ClientHeight    =   4290
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   6075
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4290
   ScaleWidth      =   6075
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdRunStatement 
      Caption         =   "&Run Statement"
      Height          =   375
      Left            =   4080
      TabIndex        =   10
      Top             =   3840
      Width           =   1455
   End
   Begin VB.CommandButton cmdSkipStatement 
      Caption         =   "&Skip Statement"
      Height          =   375
      Left            =   2520
      TabIndex        =   9
      Top             =   3840
      Width           =   1455
   End
   Begin VB.TextBox txtSQLStatement 
      Height          =   1455
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   7
      Top             =   2280
      Width           =   5775
   End
   Begin VB.CommandButton cmdAdvanced 
      Caption         =   "&Advanced"
      Height          =   375
      Left            =   120
      TabIndex        =   6
      Top             =   1560
      Width           =   1455
   End
   Begin VB.CommandButton cmdCancelUpdate 
      Cancel          =   -1  'True
      Caption         =   "&Cancel Update"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   5
      Top             =   1560
      Width           =   1455
   End
   Begin VB.Label lblSQLStatement 
      Caption         =   "SQL Statement"
      Height          =   255
      Left            =   240
      TabIndex        =   8
      Top             =   2040
      Width           =   1695
   End
   Begin VB.Label lblErrorDescription 
      Caption         =   "<description>"
      Height          =   495
      Left            =   1560
      TabIndex        =   4
      Top             =   840
      Width           =   4455
   End
   Begin VB.Label lblErrorNumber 
      Caption         =   "<number>"
      Height          =   255
      Left            =   1560
      TabIndex        =   3
      Top             =   480
      Width           =   2055
   End
   Begin VB.Label Label3 
      Caption         =   "Description:"
      Height          =   255
      Left            =   360
      TabIndex        =   2
      Top             =   840
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Error Number:"
      Height          =   255
      Left            =   360
      TabIndex        =   1
      Top             =   480
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "There was an error in the database update.  Tell BLI the error information below."
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5895
   End
End
Attribute VB_Name = "frmError"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean
Public blnSkip As Boolean

Private Sub cmdAdvanced_Click()
Me.Height = Me.cmdSkipStatement.Top + Me.cmdSkipStatement.Height + 100
Me.cmdSkipStatement.SetFocus
Me.cmdAdvanced.Enabled = False
End Sub

Private Sub cmdCancelUpdate_Click()
blnCancel = True
blnSkip = False
Me.Visible = False
End Sub

Private Sub cmdRunStatement_Click()
Me.Visible = False
End Sub

Private Sub cmdSkipStatement_Click()
blnSkip = True
Me.Visible = False
End Sub

Private Sub Form_Load()
blnCancel = False
Me.Height = Me.cmdAdvanced.Top + Me.cmdAdvanced.Height + 50
End Sub
