VERSION 5.00
Begin VB.Form dlgWiz09 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 9)"
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
   Begin VB.Timer ctlTimer 
      Enabled         =   0   'False
      Interval        =   5000
      Left            =   5460
      Top             =   60
   End
   Begin VB.CommandButton btnInstall 
      Caption         =   "&Install"
      Height          =   375
      Left            =   120
      TabIndex        =   6
      Top             =   2640
      Width           =   2055
   End
   Begin VB.Frame Frame2 
      Caption         =   "Details"
      Height          =   1935
      Left            =   120
      TabIndex        =   4
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
         TabIndex        =   5
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnPrev 
      Caption         =   "<< &Previous"
      Height          =   375
      Left            =   2400
      TabIndex        =   2
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   1
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnNext 
      Caption         =   "&Next >>"
      Default         =   -1  'True
      Height          =   375
      Left            =   3600
      TabIndex        =   0
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Database Server Installation:"
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
      TabIndex        =   3
      Top             =   120
      Width           =   2595
   End
End
Attribute VB_Name = "dlgWiz09"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 9: Install database server

Option Explicit
Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnInstall_Click()
    Dim blnError As Boolean
    Dim strTempIni As String
    Dim dtStart As Date
    Dim dtEnd As Date
    Dim dtInstallTime As Date
        
    blnError = False
    mstrDetails = ""
    
    ' Prepare setup.ini for installation
    PrepareSetupIni glngSqlType, strTempIni, mstrDetails, blnError
    
    If blnError Then
        ' Operation failed
        glngDbServerInst = NOT_INSTALLED
        UpdateControls
        Exit Sub
    End If
    
    Me.MousePointer = vbHourglass
    ctlTimer.Enabled = True
    txbDetails.Text = "Installing Database Server"
    
    btnInstall.Enabled = False
    btnPrev.Enabled = False
    btnNext.Enabled = False
    btnCancel.Enabled = False
    
    dtStart = Now
    
    RunSetupSql glngSqlType, gstrSetupExeFileSpec, strTempIni, mstrDetails, blnError
    
    dtEnd = Now
    dtInstallTime = dtEnd - dtStart
    
    ctlTimer.Enabled = False
    
    Me.MousePointer = vbDefault
    
    If blnError Then
        ' Install failed
        glngDbServerInst = NOT_INSTALLED
        mstrDetails = mstrDetails & _
            "Setup cannot continue.  Click 'Cancel' to Quit. "
        UpdateControls
    Else
        ' Install succeeded
        glngDbServerInst = INSTALLED
        mstrDetails = "Database server installation succeded; " & _
            "Installation took " & Format(dtInstallTime, "Nn") & " minutes, " & Format(dtInstallTime, "Ss") & " seconds; " & _
            "Click 'Next' to proceed." & _
            vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & mstrDetails
        goFileSystem.DeleteFile strTempIni, True
        UpdateControls
        If bAutoRun Then btnNext_Click
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
End Sub

Private Sub btnNext_Click()
    Unload Me
     
    dlgWiz10.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz08.Show ' vbModal
End Sub

Private Sub ctlTimer_Timer()
    txbDetails.Text = txbDetails.Text & " ."
End Sub
Private Sub Form_Load()
    If glngDbServerInst = NOT_INITIALIZED Then
        mstrDetails = "Click 'Install' to proceed with database server installation." & vbCrLf & vbCrLf
        mstrDetails = mstrDetails & "Installation will take 5 to 10 minutes."
    End If
End Sub
Private Sub Form_Activate()
    UpdateControls
  If bAutoRun Then btnInstall_Click
  
  
End Sub
Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    Select Case glngDbServerInst
        Case NOT_INITIALIZED
            btnCancel.Enabled = True
            btnNext.Enabled = False
            btnPrev.Enabled = True
            btnInstall.Enabled = True
            btnInstall.SetFocus
        
        Case INSTALLED
            btnCancel.Enabled = True
            btnNext.Enabled = True
            btnPrev.Enabled = False
            btnInstall.Enabled = False
            btnNext.SetFocus
        
        Case NOT_INSTALLED
            btnCancel.Enabled = True
            btnNext.Enabled = False
            btnPrev.Enabled = False
            btnInstall.Enabled = False
            btnCancel.SetFocus
    End Select
End Sub
