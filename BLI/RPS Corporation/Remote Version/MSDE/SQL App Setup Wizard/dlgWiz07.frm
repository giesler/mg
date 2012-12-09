VERSION 5.00
Begin VB.Form dlgWiz07 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 7)"
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
   Begin VB.TextBox txbOrg 
      Height          =   285
      Left            =   1200
      TabIndex        =   1
      Top             =   1080
      Width           =   4695
   End
   Begin VB.TextBox txbUserName 
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
      TabIndex        =   5
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3600
         TabIndex        =   3
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4800
         TabIndex        =   4
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2400
         TabIndex        =   2
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Label Label3 
      Caption         =   "Organization:"
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   1080
      Width           =   975
   End
   Begin VB.Label Label1 
      Caption         =   "User Name:"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   600
      Width           =   975
   End
   Begin VB.Label Label2 
      Caption         =   "Specify User Name and Organization:"
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
End
Attribute VB_Name = "dlgWiz07"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Wizard Step 7: Get User and Organization Name
Option Explicit

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    If Len(txbUserName.Text) = 0 Or Len(txbOrg.Text) = 0 Then
        MsgBox "Please supply a user name and organization.", vbOKOnly + vbInformation, "Message"
        txbUserName.SetFocus
        Exit Sub
    End If
    
    gstrUserName = txbUserName.Text
    gstrOrg = txbOrg.Text
    
    WriteLogMsg goLogFileHandle, Me.Name & ": User name set to '" & gstrUserName & "'; Organization set to '" & gstrOrg & "'."
    Unload Me
    
    Select Case glngSqlType
        Case SQL_STD_7_X86, SQL_ENT_7_X86
            ' Get the CDKey
            dlgWiz08.Show ' vbModal
        Case MSDE_OFFICE_1_X86, MSDE_1_X86, SQL_DESK_7_X86, SQL_DEV_7_X86, SQL_SBS_7_X86, SQL_POST_7
            ' CDKey not needed, go straight to install
            dlgWiz09.Show ' vbModal
    End Select
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz06.Show ' vbModal
End Sub
Private Sub Form_Load()
    gstrUserName = STR_NOT_INITIALIZED
    gstrOrg = STR_NOT_INITIALIZED
End Sub
Private Sub Form_Activate()
    txbUserName.SetFocus
    Me.txbUserName.Text = "RPS Corporation"
    Me.txbOrg.Text = "RPS Corporation"
    btnNext_Click
End Sub

Private Sub txbOrg_GotFocus()
    txbOrg.SelStart = 0
    txbOrg.SelLength = Len(txbOrg.Text)
End Sub

Private Sub txbUserName_GotFocus()
    txbUserName.SelStart = 0
    txbUserName.SelLength = Len(txbUserName.Text)
End Sub
