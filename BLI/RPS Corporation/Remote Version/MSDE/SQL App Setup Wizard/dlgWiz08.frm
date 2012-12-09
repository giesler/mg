VERSION 5.00
Begin VB.Form dlgWiz08 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 8)"
   ClientHeight    =   3195
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6030
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3195
   ScaleWidth      =   6030
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txbCDKey 
      Height          =   285
      Left            =   1200
      TabIndex        =   0
      Top             =   600
      Width           =   4695
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   4
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3600
         TabIndex        =   2
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4800
         TabIndex        =   3
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2400
         TabIndex        =   1
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Specify CD Key:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   120
      Width           =   5775
   End
   Begin VB.Label Label1 
      Caption         =   "CD Key:"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   600
      Width           =   975
   End
End
Attribute VB_Name = "dlgWiz08"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Wizard Step 8: Obtain CD Key
Option Explicit

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    Dim blnError As Boolean
    VerifyCdKeyFormat txbCDKey.Text, blnError
    
    If Not blnError Then
        gstrCDKey = txbCDKey.Text
        WriteLogMsg goLogFileHandle, Me.Name & ": CD Key Set To '" & gstrCDKey & "'. "
        Unload Me
        dlgWiz09.Show ' vbModal
    Else
        MsgBox "Please enter a valid CD Key in the format 'xxxx-xxx-xxxxxxx-yyyy'.", vbExclamation + vbOKOnly, "Error"
        txbCDKey.SetFocus
    End If
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz07.Show ' vbModal
End Sub
Private Sub Form_Load()
    gstrCDKey = STR_NOT_INITIALIZED
End Sub

Private Sub txbCDKey_GotFocus()
    With txbCDKey
        .SelStart = 0
        .SelLength = Len(.Text)
    End With
End Sub
