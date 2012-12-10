VERSION 5.00
Begin VB.Form dlgWiz18 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 18)"
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
      Begin VB.CommandButton btnVerify 
         Caption         =   "&Verify"
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
      Caption         =   "Verify Database Name:"
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
Attribute VB_Name = "dlgWiz18"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

' Step 18: Verify Database Name
Option Explicit

Dim mblnVerified As Boolean
Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    Unload Me
    
    Select Case glngAppDbInst
        Case INSTALLED
            dlgWiz22.Show ' vbModal
        Case Else
            dlgWiz19.Show ' vbModal
    End Select
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz17.Show ' vbModal
End Sub

Private Sub btnVerify_Click()
    Dim blnError As Boolean
    Dim blnFound As Boolean
    Dim strDbVersion As String
    
    blnError = False
    mstrDetails = ""
    
    glngAppDbInst = INSTALL_STATE_NOT_INITIALIZED
    mblnVerified = False
    Me.MousePointer = vbHourglass
    
    '
    ' Step 1: Connect to database server if not already connected
    '
    
    If Not gblnDmoConnected Then
        ConnectUsingDmo goDbServer, blnError, mstrDetails
    
        If blnError Then
            mstrDetails = mstrDetails & _
                "Unable to connect to database server.  Click 'Verify' to try again, or click 'Cancel' to quit."
            GoTo RefreshDisplay
        End If
    End If
    
    '
    ' Step 2: Look for duplicate logical database name on database server
    '
    
    blnFound = False
    LocateDb goDbServer, gstrAppDbName, blnFound, blnError, mstrDetails
        
    If blnError Then
        mstrDetails = mstrDetails & _
            "Click 'Verify' to try again, or click 'Cancel' to quit."
        GoTo RefreshDisplay
    End If
        
    '
    ' Step 3: If duplicate logical database exists, check version
    '
    
    If blnFound Then
        strDbVersion = STR_NOT_INITIALIZED
        GetDbVersion goDbServer, gstrAppDbName, strDbVersion, blnError, mstrDetails
    
        If blnError Then
            mstrDetails = mstrDetails & _
                "Click 'Verify' to try again, or click 'Cancel' to quit."
            GoTo RefreshDisplay
        End If
    
        If strDbVersion <> STR_NOT_INITIALIZED Then
            mstrDetails = mstrDetails & _
                "Version number of database is '" & strDbVersion & "'; "
        End If
    
        If strDbVersion = goDbApp.strAppDbVer Then
            ' Versions match, application database was pre-installed
            
            mblnVerified = True
            glngAppDbInst = INSTALLED
            ' No need to check for data or log file conflicts, wrap it up
            GoTo RefreshDisplay
        Else
            ' Versions don't match, database name conflict detected
            mstrDetails = mstrDetails & _
                "Database name conflict detected; "
            
            ResolveDatabaseNameConflict blnError
            
            If blnError Then
                mstrDetails = mstrDetails & _
                    "Unable to resolve database name conflict; "
                GoTo RefreshDisplay
            Else
                mstrDetails = mstrDetails & _
                    "Database name conflict resolved; "
                ' So far so good, now proceed with data and log file conflict checks
            End If
        End If
    Else
        mstrDetails = mstrDetails & _
            "No database name conflicts detected; "
    End If
    
    '
    ' Step 4: Check for data and log file name conflicts
    '
    
    blnFound = False
    LocateDbFiles gstrAppDbName, gstrAppDbDestFolder, blnFound, blnError, mstrDetails
    
    If blnError Then
        GoTo RefreshDisplay
    End If
    
    If blnFound Then
        mstrDetails = mstrDetails & _
            "Data file name conflict detected; "
        
        ResolveDataFileConflict blnError
        
        If blnError Then
            mstrDetails = mstrDetails & _
                "Unable to resolve data file name conflict; "
            GoTo RefreshDisplay
        Else
            mstrDetails = mstrDetails & _
                "Data file name conflict resolved; "
                mblnVerified = True
                glngAppDbInst = NOT_INSTALLED
                GoTo RefreshDisplay
        End If
    Else
        mstrDetails = mstrDetails & _
            "No data file name conflicts detected; "
    End If
    
    mblnVerified = True
    glngAppDbInst = NOT_INSTALLED
    
RefreshDisplay:

    Me.MousePointer = vbDefault
    If mblnVerified Then
        Select Case glngAppDbInst
            Case INSTALLED
                mstrDetails = mstrDetails & _
                    "Application database installation will be skipped because the application database was pre-installed; "
            Case Else
                mstrDetails = mstrDetails & _
                    "Setup will procede with application database installation using the database name '" & gstrAppDbName & "'; "
        End Select
        
        mstrDetails = mstrDetails & _
            "Click 'Next' to procede."
    Else
        mstrDetails = mstrDetails & _
            "Click 'Verify' to try again, or click 'Cancel' to quit."
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    UpdateControls
End Sub
Private Sub Form_Load()
    mblnVerified = False
    
    mstrDetails = mstrDetails & _
        "Setup will examine the database server to verify the application database name; " & _
        "Click 'Verify' to proceed."
End Sub

Private Sub Form_Activate()
    UpdateControls
    Me.btnVerify.Visible = False
    btnVerify_Click
    btnNext_Click
    
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
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

Private Sub ResolveDatabaseNameConflict(blnError As Boolean)
    Dim intResponse As Integer
    Dim strBaseMessage As String
    Dim strPrompt As String
    Dim strTitle As String
    Dim strInput As String
    Dim blnFound As Boolean
    
    blnError = False
    strBaseMessage = _
        "A database name conflict was detected. " & vbCrLf & _
        "The name '" & gstrAppDbName & "' is already in use by an existing database. " & vbCrLf
    '
    ' Conflict Resolution Option 1: Rename Existing Database
    '
    
    strTitle = "Database Name Conflict Resolution Option 1: Rename Existing Database"
    strPrompt = strBaseMessage & _
        "Would you like to rename the existing database?"
        
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse = vbNo Then
        GoTo Option2
    End If
    
GetDatabaseName:
    ' Stay in loop until user provides a new non-unique name or cancels
    strInput = ""
    strPrompt = "Enter the new name for the existing database named '" & gstrAppDbName & "':"
    strInput = InputBox(strPrompt, strTitle)
    
    If strInput = "" Then
        ' User clicked cancel, goto next option
        GoTo Option2
    End If

    blnFound = False
    LocateDb goDbServer, strInput, blnFound, blnError, mstrDetails
    
    If blnError Then
        Exit Sub
    End If
    
    If blnFound Then
        strPrompt = "A database named '" & strInput & "' already exists.  Try another name."
        MsgBox strPrompt, vbOKOnly + vbExclamation
        GoTo GetDatabaseName
    Else
        ' Confirm user action
        strPrompt = _
            "Renaming a database can cause problems with applications that access that database." & vbCrLf & _
            "Are you sure you want to rename the database named '" & gstrAppDbName & "' to '" & strInput & "'?"
        intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
        
        If intResponse <> vbYes Then
            ' Give user another try at a different name
            GoTo GetDatabaseName
        End If
        
        RenameDb goDbServer, gstrAppDbName, strInput, blnError, mstrDetails
        
        ' Option 1 complete, return to calling procedure
        ' If Rename Failed, error flag is already set and calling procedure will know
        Exit Sub
    End If

Option2:

    '
    ' Conflict Resolution Option 2: Delete Existing Database
    '
    
    strTitle = "Database Name Conflict Resolution Option 2: Delete Existing Database"
    strPrompt = strBaseMessage & _
        "Would you like to delete the existing database?"
    
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse = vbNo Then
        GoTo Option3
    End If

    ' Confirm user action
    strPrompt = _
        "Deleting a database can cause applications which use that database to fail. " & vbCrLf & _
        "A backup of the database will be made prior to deletion. " & vbCrLf & _
        "Are you sure you want to delete the database named '" & gstrAppDbName & "'?"
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse <> vbYes Then
        ' Go on to next option
        GoTo Option3
    End If
    
    ' Backup database prior to deletion
    BackupDatabase goDbServer, gstrAppDbName, blnError, mstrDetails
    
    If blnError Then
        strPrompt = "Backup failed, database will not be deleted."
        MsgBox strPrompt, vbOKOnly + vbExclamation
        Exit Sub
    End If
    
    ' Delete Database
    DeleteDb goDbServer, gstrAppDbName, blnError, mstrDetails
    
    If blnError Then
        strPrompt = "Unable to delete the '" & gstrAppDbName & "' database."
        MsgBox strPrompt, vbOKOnly + vbExclamation, strTitle
    End If
    
    Exit Sub

Option3:

    '
    ' Conflict Resolution Option 3: Use a different name for application database being installed
    '

    strTitle = "Database Name Conflict Resolution Option 3: Use a New Database Name"
    strPrompt = strBaseMessage & _
        "Would you like to use a different name for the application database to be installed?"
        
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse = vbNo Then
        ' Conflict not resolved so set error flag
        blnError = True
        Exit Sub
    End If
    
GetDatabaseName2:
    ' Stay in loop until user provides a new non-unique name or cancels
    strInput = ""
    strPrompt = "Enter the new name for the application database to be installed:"
    strInput = InputBox(strPrompt, strTitle)
    
    If strInput = "" Then
        ' User clicked cancel, conflict not resolved so set error flag
        blnError = True
        Exit Sub
    End If

    blnFound = False
    LocateDb goDbServer, strInput, blnFound, blnError, mstrDetails
    
    If blnError Then
        Exit Sub
    End If
    
    If blnFound Then
        strPrompt = "A database named '" & strInput & "' already exists.  Try another name."
        MsgBox strPrompt, vbOKOnly + vbExclamation
        GoTo GetDatabaseName2
    Else
        ' Confirm user action
        strPrompt = "Are you sure you want to use the name '" & strInput & "' for the application database to be installed?"
        intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
        
        If intResponse <> vbYes Then
            ' Give user another try at a different name
            GoTo GetDatabaseName2
        End If
        
        ' Reset the current application database name to the new name
        mstrDetails = mstrDetails & _
            "User decided to use a different name for the appplication database to be installed; "
        gstrAppDbName = strInput
        
        ' Option 3 complete, return to calling procedure
        Exit Sub
    End If
End Sub

Private Sub ResolveDataFileConflict(blnError As Boolean)
    Dim intResponse As Integer
    Dim strBaseMessage As String
    Dim strPrompt As String
    Dim strTitle As String
    Dim strInput As String
    Dim strDataFileDir As String
    Dim strDataFileSpec As String
    Dim strLogFileSpec As String
    Dim strNewDataFileName As String
    Dim strNewLogFileName As String
    Dim blnFound As Boolean
    Dim oDrive As Object
    Dim oFile As Object
    Dim strRootDataDir As String
    Dim strNewAppDbFolder As String
    Dim lngPos As Long
    
    blnError = False
    
    ' Get Data Root First
    strRootDataDir = GetDataPathFromReg & "\Data"
    
    If strRootDataDir = STR_NOT_INITIALIZED Then
        mstrDetails = mstrDetails & _
            "Problem obtaining data directory from registry; "
        blnError = True
        Exit Sub
    End If
    
    ' Now Get Full Data Path for Application Data Files
    CreateDataFileDirectory strDataFileDir, gstrAppDbDestFolder, blnError, mstrDetails
    
    If blnError Then
        Exit Sub
    End If
    
    strDataFileSpec = strDataFileDir & "\" & gstrAppDbName & "_Data.MDF"
    strLogFileSpec = strDataFileDir & "\" & gstrAppDbName & "_Log.LDF"
    
    strBaseMessage = _
        "A file name conflict was detected. " & vbCrLf
        
    If goFileSystem.FileExists(strDataFileSpec) Then
        strBaseMessage = strBaseMessage & _
            "A data file named '" & strDataFileSpec & "' already exists." & vbCrLf
    End If
    
    If goFileSystem.FileExists(strLogFileSpec) Then
        strBaseMessage = strBaseMessage & _
            "A log file named '" & strLogFileSpec & "' already exists." & vbCrLf
    End If
    
    strBaseMessage = strBaseMessage & _
        "This conflict must be resolved before setup can continue." & vbCrLf
        
    '
    ' Conflict Resolution Option 1: Create a new application database folder
    '
    
    strTitle = "File Name Conflict Resolution Option 1: Create New Folder"
    strPrompt = strBaseMessage & _
        "Would you like to create a new application database folder to resolve the conflict?"
        
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse = vbNo Then
        GoTo Option2
    End If
    
GetFolderName:
    ' Keep looping until user provides a good folder name
    gstrCurDrive = CrackDrive(strDataFileDir)
    gstrCurPath = strDataFileDir
    gblnPathChanged = False
    
    dlgGetPath.Show vbModal
    
    If Not gblnPathChanged Then
        strPrompt = "You must specify a new folder to store the application data files.  Do you want to try again?"
        
        intResponse = MsgBox(strPrompt, vbYesNo + vbExclamation, strTitle)
        
        If intResponse = vbYes Then
            GoTo GetFolderName
        Else
            GoTo Option2
        End If
    End If
    
    ' Check to make sure the new folder is not the root data path
    If UCase(gstrCurPath) = UCase(strRootDataDir) Then
        strPrompt = _
            "Please specify a folder beneath '" & strRootDataDir & "'."
        
        intResponse = MsgBox(strPrompt, vbOKOnly + vbExclamation, strTitle)
        GoTo GetFolderName
    End If
    
    ' Check to make sure the new folder is in the current data path
    If UCase(Left(gstrCurPath, Len(strRootDataDir))) <> UCase(strRootDataDir) Then
        strPrompt = _
            "The folder '" & gstrCurPath & "' is not in the current data path." & vbCrLf & _
            "Please specify a folder contained in '" & strRootDataDir & "'."
        
        intResponse = MsgBox(strPrompt, vbOKOnly + vbExclamation, strTitle)
        GoTo GetFolderName
    End If
    
    ' Check to make sure the folder is valid
    If Not goFileSystem.FolderExists(gstrCurPath) Then
        strPrompt = "There was a problem accessing the folder '" & gstrCurPath & "'. " & vbCrLf & _
            "Do you want to try again?"
        
        intResponse = MsgBox(strPrompt, vbYesNo + vbExclamation, strTitle)
        
        If intResponse = vbYes Then
            GoTo GetFolderName
        Else
            GoTo Option2
        End If
    End If
    
    ' Build new AppDb folder name
    lngPos = InStrRev(gstrCurPath, "\")
    
    If lngPos = 0 Or lngPos = Null Then
        blnError = True
        mstrDetails = mstrDetails & _
            "Problem parsing path '" & gstrCurPath & "'; "
        Exit Sub
    End If
    
    strNewAppDbFolder = Mid(gstrCurPath, lngPos + 1, Len(gstrCurPath))
    
    ' Check new AppDbFolder
    CreateDataFileDirectory strDataFileDir, strNewAppDbFolder, blnError, mstrDetails
    
    If blnError Then
        Exit Sub
    End If
    
    ' Check for file conflicts in new folder
    blnFound = False
    LocateDbFiles gstrAppDbName, strNewAppDbFolder, blnFound, blnError, mstrDetails
    
    If blnFound Then
        strPrompt = _
            "File name conflicts were encountered in the folder '" & strDataFileDir & "'." & vbCrLf & _
            "Please try a different folder."
        
        intResponse = MsgBox(strPrompt, vbOKOnly + vbExclamation, strTitle)
        
        ' Reset the data file directory and try again
        CreateDataFileDirectory strDataFileDir, gstrAppDbDestFolder, blnError, mstrDetails
        If blnError Then
            Exit Sub
        End If
        GoTo GetFolderName
    End If
    
    ' Set data file folder to new folder
    gstrAppDbDestFolder = strNewAppDbFolder
    mstrDetails = mstrDetails & _
        "Application database installation will procede in the folder '" & strDataFileDir & "'; "
    Exit Sub
    
Option2:

    '
    ' Conflict Resolution Option 2: Rename Existing Data and / or Log File
    '
    
    strTitle = "File Name Conflict Resolution Option 2: Rename File(s)"
    strPrompt = strBaseMessage & _
        "Would you like to rename the existing data and / or log files?"
        
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse = vbNo Then
        GoTo Option3
    End If
    
    GenerateUniqueDataFileNames gstrAppDbName, strDataFileDir, strNewDataFileName, strNewLogFileName, blnError, mstrDetails

    If blnError Then
        Exit Sub
    End If
    
    On Error GoTo FileSystemError
    
    ' Set error flag in case of logic error
    blnError = True
    
    If goFileSystem.FileExists(strDataFileSpec) Then
        Set oFile = goFileSystem.GetFile(strDataFileSpec)
        oFile.Name = strNewDataFileName
        mstrDetails = mstrDetails & _
            "The data file '" & strDataFileSpec & "' was renamed to '" & strNewDataFileName & "'; "
        blnError = False
    End If
    
    If goFileSystem.FileExists(strLogFileSpec) Then
        Set oFile = goFileSystem.GetFile(strLogFileSpec)
        oFile.Name = strNewLogFileName
        mstrDetails = mstrDetails & _
            "The log file '" & strLogFileSpec & "' was renamed to '" & strNewLogFileName & "'; "
        blnError = False
    End If
    
    On Error GoTo 0
    
    If blnError Then
        mstrDetails = mstrDetails & _
            "No files were renamed due to a logic error; "
    End If
    
    Exit Sub
    
Option3:
    
    '
    ' Conflict Resolution Option 3: Delete Existing Data and / or Log File
    '
    
    strTitle = "File Name Conflict Resolution Option 3: Delete File(s)"
    strPrompt = strBaseMessage & _
        "Would you like to delete the existing data and / or log files?"
        
    intResponse = MsgBox(strPrompt, vbYesNo + vbQuestion, strTitle)
    
    If intResponse = vbNo Then
        blnError = True
        Exit Sub
    End If
    
    strPrompt = _
        "File deletion is permanent.  Do you wish to procede?"
        
    intResponse = MsgBox(strPrompt, vbYesNo + vbExclamation, strTitle)
    
    If intResponse <> vbYes Then
        ' Conflict not resolved so set error flag
        blnError = True
        Exit Sub
    End If
    
    ' Set error flag to check for logic error
    blnError = True
    
    On Error GoTo FileSystemError
    
    If goFileSystem.FileExists(strDataFileSpec) Then
        goFileSystem.DeleteFile strDataFileSpec, True
        blnError = False
    End If
    
    If goFileSystem.FileExists(strLogFileSpec) Then
        goFileSystem.DeleteFile strLogFileSpec, True
        blnError = False
    End If
    
    On Error GoTo 0
    
    If blnError Then
        mstrDetails = mstrDetails & _
            "No files were deleted due to a logic error; "
    End If
    
    Exit Sub
    
FileSystemError:
    blnError = True
    mstrDetails = mstrDetails & _
        Err.Description
    Exit Sub
End Sub

