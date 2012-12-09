VERSION 5.00
Begin VB.Form dlgWiz06 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "DbInstallImage Wizard (Step 6)"
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
   Begin VB.CommandButton btnVerify 
      Caption         =   "&Verify Folder"
      Height          =   375
      Left            =   120
      TabIndex        =   5
      Top             =   2640
      Width           =   2055
   End
   Begin VB.Frame Frame2 
      Caption         =   "Destination Folder"
      Height          =   615
      Left            =   120
      TabIndex        =   0
      Top             =   360
      Width           =   5775
      Begin VB.TextBox txbPath 
         Height          =   285
         Left            =   120
         Locked          =   -1  'True
         TabIndex        =   1
         Text            =   "Text1"
         Top             =   240
         Width           =   5175
      End
      Begin VB.CommandButton btnBrowseImagePath 
         Caption         =   "..."
         Height          =   255
         Left            =   5400
         TabIndex        =   2
         Top             =   240
         Width           =   255
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   1455
      Left            =   120
      TabIndex        =   3
      Top             =   1080
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1095
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   4
         TabStop         =   0   'False
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnPrev 
      Caption         =   "<< &Previous"
      Height          =   375
      Left            =   2400
      TabIndex        =   6
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   8
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnNext 
      Caption         =   "&Next >>"
      Height          =   375
      Left            =   3600
      TabIndex        =   7
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Select Destination Folder for Installatin Image:"
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
      TabIndex        =   9
      Top             =   120
      Width           =   5775
   End
End
Attribute VB_Name = "dlgWiz06"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mstrDetails As String
Dim mblnVerified As Boolean
Dim mstrImageDrive As String

Private Sub btnBrowseImagePath_Click()
    gstrCurDrive = mstrImageDrive
    
    If goFileSystem.FolderExists(gstrImageFolder) Then
        gstrCurPath = gstrImageFolder
    Else
        gstrCurPath = mstrImageDrive & "\"
    End If
    
    gblnPathChanged = False
    
    dlgGetPath.Show vbModal
    
    If gblnPathChanged Then
        gstrImageFolder = gstrCurPath
        mstrImageDrive = CrackDrive(gstrImageFolder)
    End If
    
    UpdateControls
End Sub

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    Unload Me
    dlgWiz07.Show vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz05.Show vbModal
End Sub

Private Sub btnVerify_Click()
    Dim blnError As Boolean
    Dim lngDbSizeBytes As Long
    
    mstrDetails = ""
    
    blnError = False
    
    Me.MousePointer = vbHourglass
    
    VerifyFolder gstrImageFolder, goAppDb.lngAppDbBytesReq, blnError, mstrDetails
    
    Me.MousePointer = vbDefault
        
RefreshDisplay:
    If blnError Then
        mblnVerified = False
        mstrDetails = mstrDetails & _
            "Unable to verify folder '" & gstrImageFolder & "'; " & _
            "Choose another folder or click 'Cancel' to quit."
    Else
        mblnVerified = True
        mstrDetails = mstrDetails & _
            "The folder '" & gstrImageFolder & "' was verified; " & _
            "Click 'Next' to proceed."
    End If
    
    UpdateControls
End Sub

Private Sub Form_Load()
    mstrDetails = _
        "Select a destination folder for the application database installation image " & _
        "files, then click 'Verify Folder'."
    mblnVerified = False
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    txbPath.Text = gstrImageFolder
    
    If mblnVerified Then
        btnVerify.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnVerify.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnVerify.SetFocus
    End If
End Sub
