VERSION 5.00
Begin VB.Form dlgWiz24 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 24)"
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
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   0
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnEnable 
         Caption         =   "&Enable"
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
      Caption         =   "Enable Daily Application Database Backups:"
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
      Width           =   4035
   End
End
Attribute VB_Name = "dlgWiz24"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mstrDetails As String
Dim mblnEnabled As Boolean

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnEnable_Click()
    Dim blnError As Boolean
    Dim blnFound As Boolean
    Dim strJobName As String
    Dim strJobFileDestSpec As String
    
    mstrDetails = ""
    blnError = False
    mblnEnabled = False
    
    Me.MousePointer = vbHourglass
    
    '
    ' Step 1: Start Job Server
    '
    
    If Not gblnJobServerStarted Then
        StartJobServer goDbServer, blnError, mstrDetails
    End If
    
    If blnError Then
        GoTo RefreshDisplay
    End If
    
    '
    ' Step 2: Check for existing job, if exists delete
    '
    
    blnFound = False
    strJobName = gstrAppDbName & "_Daily_Backup"
    FindJob goDbServer, strJobName, blnFound, blnError, mstrDetails
    
    If blnError Then
        GoTo RefreshDisplay
    End If
    
    If blnFound Then
        RemoveJob goDbServer, strJobName, blnError, mstrDetails
    End If
    
    If blnError Then
        GoTo RefreshDisplay
    End If
    
    '
    ' Step 3: Copy VBScript file into jobs folder
    '
    
    InstallJobFile gstrBackupJobFile, App.Path & "\" & gstrBackupJobFile, strJobFileDestSpec, blnError, mstrDetails
    
    If blnError Then
        GoTo RefreshDisplay
    End If
    
    '
    ' Step 4: Create New Job
    '
    
    CreateDailyBackupJob goDbServer, strJobName, strJobFileDestSpec, gstrAppDbName, blnError, mstrDetails
    
    If blnError Then
        GoTo RefreshDisplay
    End If
    
    mblnEnabled = True
    
RefreshDisplay:
    If mblnEnabled Then
        mstrDetails = mstrDetails & _
            "Daily backups of the database named '" & gstrAppDbName & "' will be performed; "
    Else
        mstrDetails = mstrDetails & _
            "Daily backups were not enabled; Click 'Enable' to try again. "
    End If
    
    mstrDetails = mstrDetails & _
        "Click 'Next' to proceed. "
        
    Me.MousePointer = vbDefault
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
        
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz25.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz23.Show ' vbModal
End Sub
Private Sub Form_Load()
    mblnEnabled = False
    mstrDetails = _
        "Click 'Enable' to automatically perform daily backups of the application database named '" & gstrAppDbName & "'; " & _
        "Click 'Next' to skip this step and proceed. "
End Sub
Private Sub Form_Activate()
    UpdateControls
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnEnabled Then
        btnEnable.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnEnable.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnEnable.SetFocus
    End If
End Sub
