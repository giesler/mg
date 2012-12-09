VERSION 5.00
Begin VB.Form dlgWiz25 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 25)"
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
      Begin VB.CommandButton btnConfigure 
         Caption         =   "&Configure"
         Height          =   375
         Left            =   120
         TabIndex        =   7
         Top             =   120
         Width           =   2055
      End
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Enabled         =   0   'False
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
         Enabled         =   0   'False
         Height          =   375
         Left            =   2400
         TabIndex        =   1
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Configure Services for Auto-Start:"
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
      Width           =   3075
   End
End
Attribute VB_Name = "dlgWiz25"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
 ' Step 25: Configure Services for Auto Start
Option Explicit

Dim mstrDetails As String
Dim mblnConfigured As Boolean

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnConfigure_Click()
    Dim blnError As Boolean
    Dim strJobFileDestSpec As String
    
    blnError = False
    mstrDetails = ""
    
    Me.MousePointer = vbHourglass
    
    Select Case glngOsType
        Case WIN_95_98
            '
            ' Step 1: Copy VBScript file used to auto-start db server to system
            '
            
            InstallJobFile gstrStartupJobFile, App.Path & "\" & gstrStartupJobFile, strJobFileDestSpec, blnError, mstrDetails
            
            If blnError Then
                GoTo RefreshDisplay
            End If
            
            '
            ' Step 2: Modify registry to auto-start VBscript file
            '
            CreateAutoStartRegEntry strJobFileDestSpec, blnError, mstrDetails
            
            If blnError Then
                GoTo RefreshDisplay
            End If
            
            '
            ' Step 3: Create SqlAppSetupWiz_Start_Job_Server stored procedure
            '
            RunScript goDbServer, App.Path & "\" & gstrJobServerStartupSpScript, blnError, mstrDetails
            
            If blnError Then
                GoTo RefreshDisplay
            End If
            
            '
            ' Step 4: Create autoexec stored procedure that runs SqlAppSetupWiz_Start_Job_Server when DB Server starts
            '
            
            CreateAutoExecSp goDbServer, gstrSqlSaPwd, blnError, mstrDetails
            
            If blnError Then
                GoTo RefreshDisplay
            End If
            
        Case Else
            AutoStartServices goDbServer, blnError, mstrDetails
            
            If blnError Then
                GoTo RefreshDisplay
            End If
    End Select
    
    mblnConfigured = True
    
RefreshDisplay:

    Me.MousePointer = vbDefault
    
    If mblnConfigured Then
        mstrDetails = mstrDetails & _
            "Configuration complete, click 'Next' to proceed. "
    Else
        mstrDetails = mstrDetails & _
            "Setup encountered a problem configuring services; " & _
            "Click 'Configure' to try again, or click 'Cancel' to quit."
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz26.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz24.Show ' vbModal
End Sub

Private Sub Form_Activate()
    UpdateControls
    Me.btnConfigure.Visible = False
    btnConfigure_Click
    btnNext_Click
End Sub

Private Sub Form_Load()
    mblnConfigured = False
    mstrDetails = _
        "Setup needs to configure database server support services to start automatically; " & _
        "Click 'Configure' to continue."
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnConfigured Then
        btnConfigure.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnConfigure.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnConfigure.SetFocus
    End If
End Sub
