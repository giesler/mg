VERSION 5.00
Begin VB.Form dlgWiz14 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 14)"
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
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   4
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnBackup 
         Caption         =   "&Backup"
         Height          =   375
         Left            =   120
         TabIndex        =   0
         Top             =   120
         Width           =   2055
      End
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
      Caption         =   "Backup System Databases:"
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
      Width           =   3975
   End
End
Attribute VB_Name = "dlgWiz14"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 14: Backup System Databases
Option Explicit
Dim mblnBackupComplete As Boolean
Dim mstrDetails As String

Private Sub btnBackup_Click()
    Dim blnError As Boolean
    Dim oDb As Object
    
    blnError = False
    mblnBackupComplete = False
    mstrDetails = ""
    
    Me.MousePointer = vbHourglass
    
    btnBackup.Enabled = False
    btnPrev.Enabled = False
    btnNext.Enabled = False
    btnCancel.Enabled = False
    
    For Each oDb In goDbServer.Databases
        If oDb.SystemObject Then
            Select Case UCase(oDb.Name)
                Case "TEMPDB", "MODEL"
                    ' No Need to back up these databases, do nothing
                Case Else
                    BackupDatabase goDbServer, oDb.Name, blnError, mstrDetails
                    
                    If blnError Then
                        Exit For
                    End If
            End Select
        End If
    Next oDb
    
    Me.MousePointer = vbDefault

    If blnError Then
        mblnBackupComplete = False
        mstrDetails = mstrDetails & "Setup cannot continue.  Click 'Cancel' to Quit."
    Else
        mblnBackupComplete = True
        mstrDetails = mstrDetails & "System databases were backed up successfully.  Click 'Next' to proceed."
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    UpdateControls
End Sub

Private Sub btnCancel_Click()
    WriteLogMsg goLogFileHandle, mstrDetails
    Quit True
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz15.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz13.Show ' vbModal
End Sub
Private Sub Form_Load()
    mstrDetails = ""
    mblnBackupComplete = False
    
    mstrDetails = mstrDetails & _
        "Setup needs to backup the system databases on the local database server. " & vbCrLf & vbCrLf & _
        "Please wait..."
End Sub
Private Sub Form_Activate()
    UpdateControls
    Me.btnBackup.Visible = False
    Me.Refresh
    btnBackup_Click
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnBackupComplete Then
        btnBackup.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnBackup.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnBackup.SetFocus
    End If
End Sub

