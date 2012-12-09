VERSION 5.00
Begin VB.Form dlgWiz21 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 21)"
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
      TabIndex        =   4
      Top             =   420
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
         TabIndex        =   7
         Top             =   120
         Width           =   2055
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
         TabIndex        =   2
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
      Caption         =   "Install Application Database:"
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
      Width           =   3795
   End
End
Attribute VB_Name = "dlgWiz21"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 21: Install Application Database

Option Explicit
Dim mstrDetails As String
Dim mblnInstalled As Boolean

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnInstall_Click()
    Dim blnError As Boolean
    
    blnError = False
    mstrDetails = ""
    
    Me.btnInstall.Enabled = False
    txbDetails.Text = "Installing application database ."
    Me.Refresh
    Me.Refresh
    Me.MousePointer = vbHourglass
    
    If Not gblnDmoConnected Then
        ConnectUsingDmo goDbServer, blnError, mstrDetails
    End If
    
    If blnError Then
        mstrDetails = mstrDetails & _
            "Unabled to connect to local database server; "
        GoTo RefreshDisplay
    End If
    
    Select Case glngAppDbInstallType
        Case FILE_IMAGE, APPDBIMAGETYPE_NOT_INITIALIZED
            InstallDbFromFile goDbServer, gstrAppDbSourceFolder, goDbApp, gstrAppDbDestFolder, blnError, mstrDetails
        Case SCRIPT_IMAGE
            InstallDbFromScript goDbServer, gstrAppDbSourceFolder, goDbApp, gstrAppDbDestFolder, blnError, mstrDetails
    End Select
    
RefreshDisplay:
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    
    If blnError Then
        mblnInstalled = False
        glngAppDbInst = NOT_INSTALLED
        mstrDetails = mstrDetails & _
            "Cick 'Install' to re-try, or click 'Cancel' to quit."
    Else
        mblnInstalled = True
        glngAppDbInst = INSTALLED
        mstrDetails = mstrDetails & _
            "Click 'Next' to proceed."
        If bAutoRun Then btnNext_Click
    End If
    
    Me.MousePointer = vbDefault
    
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz22.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz20.Show ' vbModal
End Sub
Private Sub Form_Load()
    mblnInstalled = False
    glngAppDbInst = NOT_INITIALIZED
    
    mstrDetails = mstrDetails & _
        "Click 'Install' to proceed with application database installation." & vbCrLf & vbCrLf & "Installation of the database may take a few minutes."
        
End Sub

Private Sub Form_Activate()
    UpdateControls
    If bAutoRun Then btnInstall_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnInstalled Then
        btnInstall.Enabled = False
        btnPrev.Enabled = False
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnInstall.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
    End If
End Sub

