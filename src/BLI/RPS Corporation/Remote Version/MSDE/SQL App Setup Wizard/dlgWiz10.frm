VERSION 5.00
Begin VB.Form dlgWiz10 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 10)"
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
   Begin VB.Frame Frame2 
      Caption         =   "Details"
      Height          =   1935
      Left            =   120
      TabIndex        =   5
      Top             =   480
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1515
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   6
         TabStop         =   0   'False
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   0
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnInstall 
         Caption         =   "&Install"
         Height          =   375
         Left            =   120
         TabIndex        =   1
         Top             =   120
         Width           =   2175
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2400
         TabIndex        =   2
         Top             =   120
         Width           =   1095
      End
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
   End
   Begin VB.Label Label2 
      Caption         =   "SQL Server 7.0 Service Pack Installation"
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
      TabIndex        =   7
      Top             =   120
      Width           =   3795
   End
End
Attribute VB_Name = "dlgWiz10"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Wizard Step 10: SQL Server 7.0 Service Pack 1 Installation

Option Explicit
Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit True
End Sub
Public Sub btnNext_Click()
    glngServicePackInst = NOT_INSTALLED
    WriteLogMsg goLogFileHandle, Me.Name & ": User skipped service pack installation."
    Unload Me
    dlgWiz17.Show ' vbModal
End Sub

Private Sub btnInstall_Click()
    Dim strFormName
    strFormName = Me.Name
    
    Unload Me
    If glngServicePackInst = INSTALLED Then
        WriteLogMsg goLogFileHandle, strFormName & ": Skipping service pack installation because service pack is already installed. Proceeding to step 17."
        dlgWiz17.Show ' vbModal
        Exit Sub
    Else
        WriteLogMsg goLogFileHandle, strFormName & ": Proceeding with service pack installation."
        dlgWiz11.Show ' vbModal
    End If
    
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz02.Show ' vbModal
End Sub


Private Sub Form_Load()
    If glngSqlCsdVer >= 200 Then
        glngServicePackInst = INSTALLED
        
        mstrDetails = _
            "Your database server has already been upgraded to service pack version " & CStr(glngSqlCsdVer) & ". " & _
            "No service pack installation is required.  Click 'Next' to proceed."
    Else
        
        Select Case glngOsType
            Case WIN_95_98
                glngSpType = SQL_7_SP2_9X_X86
            Case Else
                glngSpType = SQL_7_SP2_NT_X86
        End Select
    
        mstrDetails = _
            "Setup recommends that you update the database server installation with the latest service pack. " & _
            "Click 'Install' to proceed with service pack installation. " & _
            "Click 'Next' to skip service pack installation and procede. "
    End If
End Sub
Private Sub Form_Activate()
    UpdateControls
    btnInstall_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    btnInstall.Enabled = True
    btnPrev.Enabled = True
    btnNext.Enabled = True
    btnCancel.Enabled = True
    btnInstall.SetFocus
End Sub
