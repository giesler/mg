VERSION 5.00
Begin VB.Form dlgWiz06 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 6)"
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
      Height          =   975
      Left            =   120
      TabIndex        =   14
      Top             =   1560
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   615
         Left            =   240
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   15
         Top             =   240
         Width           =   5415
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Destination Folders"
      Height          =   1215
      Left            =   120
      TabIndex        =   0
      Top             =   360
      Width           =   5775
      Begin VB.CommandButton btnBrowseDataPath 
         Caption         =   "..."
         Height          =   255
         Left            =   5400
         TabIndex        =   2
         Top             =   540
         Width           =   255
      End
      Begin VB.CommandButton btnBrowseProgramPath 
         Caption         =   "..."
         Height          =   255
         Left            =   5400
         TabIndex        =   1
         Top             =   240
         Width           =   255
      End
      Begin VB.Label lblSysRoot 
         BorderStyle     =   1  'Fixed Single
         Height          =   255
         Left            =   1320
         TabIndex        =   13
         Top             =   840
         Width           =   3975
      End
      Begin VB.Label lblDataPath 
         BorderStyle     =   1  'Fixed Single
         Height          =   255
         Left            =   1320
         TabIndex        =   12
         Top             =   540
         Width           =   3975
      End
      Begin VB.Label lblProgramPath 
         BorderStyle     =   1  'Fixed Single
         Height          =   255
         Left            =   1320
         TabIndex        =   11
         Top             =   240
         Width           =   3975
      End
      Begin VB.Label Label5 
         Alignment       =   1  'Right Justify
         Caption         =   "System Root:"
         Height          =   255
         Left            =   120
         TabIndex        =   10
         Top             =   840
         Width           =   1095
      End
      Begin VB.Label Label4 
         Alignment       =   1  'Right Justify
         Caption         =   "Data Files:"
         Height          =   255
         Left            =   120
         TabIndex        =   9
         Top             =   540
         Width           =   1095
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         Caption         =   "Program Files:"
         Height          =   255
         Left            =   120
         TabIndex        =   8
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.CommandButton btnPrev 
      Caption         =   "<< &Previous"
      Height          =   375
      Left            =   2400
      TabIndex        =   4
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   6
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnNext 
      Caption         =   "&Next >>"
      Default         =   -1  'True
      Height          =   375
      Left            =   3600
      TabIndex        =   5
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnVerifyFolders 
      Caption         =   "&Verify Folders"
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   2640
      Width           =   2055
   End
   Begin VB.Label Label2 
      Caption         =   "Select Destination Folders:"
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
      Width           =   5775
   End
End
Attribute VB_Name = "dlgWiz06"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 6: Select Destination Folders
Option Explicit
Option Base 1

Private Type TargetDrive
    DriveId As String
    BytesAvailable As Double
    BytesRequired As Double
End Type

Dim mDriveArray(4) As TargetDrive

Dim mstrProgramDrive As String
Dim mstrDataDrive As String
Dim mstrSysDrive As String
Dim mstrDetails As String
Dim mblnVerified As Boolean


Private Sub btnBrowseDataPath_Click()
    gstrCurDrive = mstrDataDrive
    
    If goFileSystem.FolderExists(gstrDataPath) Then
        gstrCurPath = gstrDataPath
    Else
        gstrCurPath = mstrDataDrive & "\"
    End If
    
    gblnPathChanged = False
    
    dlgGetPath.Show vbModal
    
    If gblnPathChanged Then
        gstrDataPath = gstrCurPath
        mstrDataDrive = CrackDrive(gstrDataPath)
    End If
    
    UpdateControls
End Sub

Private Sub btnBrowseProgramPath_Click()
    gstrCurDrive = mstrProgramDrive
    
    If goFileSystem.FolderExists(gstrProgramPath) Then
        gstrCurPath = gstrProgramPath
    Else
        gstrCurPath = mstrProgramDrive & "\"
    End If
    
    gblnPathChanged = False
    
    dlgGetPath.Show vbModal
    
    If gblnPathChanged Then
        gstrProgramPath = gstrCurPath
        mstrProgramDrive = CrackDrive(gstrProgramPath)
    End If
    
    UpdateControls
End Sub

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub UpdateControls()
    lblProgramPath.Caption = gstrProgramPath
    lblDataPath.Caption = gstrDataPath
    lblSysRoot.Caption = gstrWinDir
    txbDetails.Text = mstrDetails
    
    If mblnVerified Then
        btnVerifyFolders.Enabled = False
        btnBrowseDataPath.Enabled = False
        btnBrowseProgramPath.Enabled = False
        btnNext.Enabled = True
        btnNext.SetFocus
    Else
        btnVerifyFolders.Enabled = True
        btnBrowseDataPath.Enabled = True
        btnBrowseProgramPath.Enabled = True
        btnNext.Enabled = False
        btnVerifyFolders.SetFocus
    End If
End Sub


Private Sub btnNext_Click()
    Unload Me
    dlgWiz07.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    gstrProgramPath = STR_NOT_INITIALIZED
    Unload Me
    dlgWiz05.Show ' vbModal
End Sub
Private Sub Form_Load()
    mblnVerified = False
    mstrDetails = "Choose folders, then click Verify Folders to proceed."
    
    If gstrProgramPath = STR_NOT_INITIALIZED Then
        mstrSysDrive = CrackDrive(gstrWinDir)
        mstrProgramDrive = mstrSysDrive
        mstrDataDrive = mstrSysDrive
        
        gstrProgramPath = mstrProgramDrive & "\Program Files\MSSQL7"
        gstrDataPath = mstrDataDrive & "\Program Files\MSSQL7"
    End If
End Sub
Private Sub Form_Activate()
    UpdateControls
    btnVerifyFolders_Click
    Me.btnVerifyFolders.Visible = False
    btnNext_Click
End Sub

Private Sub btnVerifyFolders_Click()
    Dim I As Integer
    Dim blnError As Boolean
    
    mblnVerified = False
    
    ' Initialize array
    For I = 1 To 4
        mDriveArray(I).DriveId = STR_NOT_INITIALIZED
    Next I
    
    mstrDetails = ""
    blnError = False
    
    CategorizeDrive gstrProgramPath, goDbServers(glngSqlType).ProgramBytes, mstrDetails, blnError
    
    If blnError Then
        WriteLogMsg goLogFileHandle, mstrDetails
        mblnVerified = False
        UpdateControls
        Exit Sub
    End If
    
    CategorizeDrive gstrDataPath, goDbServers(glngSqlType).DataBytes + goDbApp.lngAppDbBytesReq, mstrDetails, blnError
    
    If blnError Then
        WriteLogMsg goLogFileHandle, mstrDetails
        mblnVerified = False
        UpdateControls
        Exit Sub
    End If

    CategorizeDrive gstrWinDir, goDbServers(glngSqlType).SysBytes, mstrDetails, blnError
    
    If blnError Then
        WriteLogMsg goLogFileHandle, mstrDetails
        mblnVerified = False
        UpdateControls
        Exit Sub
    End If
    
    CategorizeDrive gstrTempDir, goDbServers(glngSqlType).TempBytes, mstrDetails, blnError
    
    If blnError Then
        WriteLogMsg goLogFileHandle, mstrDetails
        mblnVerified = False
        UpdateControls
        Exit Sub
    End If
    
    ' Check Available Space
    For I = 1 To 4
        If mDriveArray(I).DriveId <> STR_NOT_INITIALIZED Then
            mstrDetails = mstrDetails & _
                "Bytes available on " & mDriveArray(I).DriveId & " " & CStr(mDriveArray(I).BytesAvailable) & "; " & _
                "Bytes required on " & mDriveArray(I).DriveId & " " & CStr(mDriveArray(I).BytesRequired) & "; "

            If mDriveArray(I).BytesAvailable < mDriveArray(I).BytesRequired Then
                mblnVerified = False
                mstrDetails = mstrDetails & _
                    "Drive " & mDriveArray(I).DriveId & " does not have enough space " & _
                    "to proceed with installation.  Choose a different drive or Click 'Cancel'. "
                WriteLogMsg goLogFileHandle, mstrDetails
                UpdateControls
                Exit Sub
            End If
        End If
    Next I
    
    ' Verify Directories
    VerifyFolder gstrProgramPath, mstrDetails, blnError
    
    If blnError Then
        WriteLogMsg goLogFileHandle, mstrDetails
        mblnVerified = False
        UpdateControls
        Exit Sub
    End If
    
    VerifyFolder gstrDataPath, mstrDetails, blnError
    
    If blnError Then
        WriteLogMsg goLogFileHandle, mstrDetails
        mblnVerified = False
        UpdateControls
        Exit Sub
    End If
    
    mblnVerified = True
    
    mstrDetails = mstrDetails & _
        "Folders verified; "
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails

    UpdateControls
End Sub
Private Sub CategorizeDrive(strPath As String, lngBytesReq As Long, strMessage As String, blnError As Boolean)
    Dim I As Integer
    Dim oDrive As Object
    
    blnError = False
    strMessage = ""
    
    If Not goFileSystem.DriveExists(CrackDrive(strPath)) Then
        strMessage = "Drive '" & CrackDrive(strPath) & "' does not exist. Choose another drive."
        blnError = True
        Exit Sub
    End If
    
    Set oDrive = goFileSystem.GetDrive(CrackDrive(strPath))
    
    Select Case oDrive.DriveType
        Case Fixed, Removable
            ' Acceptable Drive Types, Continue
        Case Remote, CDRom, RamDisk
            ' Unacceptable Drive Types, Abort
            blnError = True
            strMessage = "Drive '" & CrackDrive(strPath) & "' is not valid for software installation. Choose another drive."
            Exit Sub
        Case Unknown
            ' Unknown Drive Type, Abort
            blnError = True
            strMessage = "Drive '" & CrackDrive(strPath) & "' is of an unknown type and is not valid for software installation. Choose another drive."
            Exit Sub
    End Select
    
    For I = 1 To 3
        If mDriveArray(I).DriveId = STR_NOT_INITIALIZED Then
            ' Initialize drive in array
            mDriveArray(I).DriveId = CrackDrive(strPath)
            mDriveArray(I).BytesAvailable = oDrive.AvailableSpace
            mDriveArray(I).BytesRequired = lngBytesReq
            Exit For
        Else
            ' Drive already initialized in array
            If mDriveArray(I).DriveId = CrackDrive(strPath) Then
                ' Increment BytesRequired for matching drive
                mDriveArray(I).BytesRequired = mDriveArray(I).BytesRequired + lngBytesReq
                Exit For
            End If
        End If
    Next I
    
    Set oDrive = Nothing
End Sub

Private Sub VerifyFolder(strPath As String, strMessage As String, blnError As Boolean)
    On Error Resume Next
    
    If Not goFileSystem.FolderExists(strPath) Then
        ' Directory doesn't exist, create it
        MkDir (gstrProgramPath)
        If Err.Number <> 0 Then
            blnError = True
            strMessage = strMessage & _
                "Error creating directory " & strPath & ". " & _
                "Error Number: " & CStr(Err.Number) & "; " & _
                "Error Description: " & CStr(Err.Description) & "; " & _
                "Setup cannot continue."
            Err.Clear
        Else
            blnError = False
            strMessage = strMessage & _
                "Successfully created directory " & strPath & "; "
        End If
    Else
        ' Directory exists everything is cool
        blnError = False
    End If
    
    On Error GoTo 0
End Sub
