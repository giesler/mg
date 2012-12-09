Attribute VB_Name = "modSqlDmo"
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' modSqlDmo.bas module isolates all SQL-DMO related procedures from rest of application '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit
Option Base 1

' Global Session Variables
Public goDbServer As Object ' Main SQLDMO.SQLServer object used to interact with local database server

Private Const SQLDMO_E_SVCALREADYRUNNING = 1056& ' SQL-DMO Error thrown when trying to start a server that is already started, not documented in SQL BOL

Private Enum SQLDMO_GROWTH_TYPE
    SQLDMOGrowth_MB = 0&
    SQLDMOGrowth_Percent = 1&
    SQLDMOGrowth_Invalid = 99&
End Enum

Private Enum SQLDMO_BACKUP_TYPE
    SQLDMOBackup_Database = 0&
    SQLDMOBackup_Differential = 1&
    SQLDMOBackup_Files = 2&
    SQLDMOBackup_Log = 3&
End Enum

Private Enum SQLDMO_DEVICE_TYPE
    SQLDMODevice_Unknown = 100&
    SQLDMODevice_DiskDump = 2&
    SQLDMODevice_FloppyADump = 3&
    SQLDMODevice_FloppyBDump = 4&
    SQLDMODevice_TapeDump = 5&
    SQLDMODevice_PipeDump = 6&
    SQLDMODevice_CDROM = 7&
End Enum

Private Enum SQLDMO_SECURITY_TYPE
    SQLDMOSecurity_Normal = 0&
    SQLDMOSecurity_Integrated
    SQLDMOSecurity_Mixed
End Enum

Private Enum SQLDMO_DATAFILE_TYPE
    SQLDMODataFile_CommaDelimitedChar = &H1
    SQLDMODataFile_Default = &H1
    SQLDMODataFile_TabDelimitedChar = &H2
    SQLDMODataFile_SpecialDelimitedChar = &H3
    SQLDMODataFile_NativeFormat = &H4
    SQLDMODataFile_UseFormatFile = &H5
End Enum

Private Enum SQLDMO_LOGIN_TYPE
    SQLDMOLogin_NTUser = 0&
    SQLDMOLogin_NTGroup
    SQLDMOLogin_Standard
End Enum

Private Enum SQLDMO_SVCSTATUS_TYPE
    SQLDMOSvc_Unknown = 0&
    SQLDMOSvc_Running
    SQLDMOSvc_Paused
    SQLDMOSvc_Stopped
    SQLDMOSvc_Starting
    SQLDMOSvc_Stopping
    SQLDMOSvc_Continuing
    SQLDMOSvc_Pausing
End Enum

Private Enum SQLDMO_COMPLETION_TYPE
    SQLDMOComp_Unknown = &H1000
    SQLDMOComp_None = &H0
    SQLDMOComp_Success = &H1
    SQLDMOComp_Failure = &H2
    SQLDMOComp_Always = &H3
    SQLDMOComp_All = &H6
End Enum

Private Enum SQLDMO_FREQUENCY_TYPE
    SQLDMOFreq_Unknown = &H0
    SQLDMOFreq_OneTime = &H1
    SQLDMOFreq_Daily = &H4
    SQLDMOFreq_Weekly = &H8
    SQLDMOFreq_Monthly = &H10
    SQLDMOFreq_MonthlyRelative = &H20
    SQLDMOFreq_Autostart = &H40
    SQLDMOFreq_OnIdle = &H80
    SQLDMOFreq_Valid = &HFF
End Enum

Private Enum SQLDMO_FREQSUB_TYPE
    SQLDMOFreqSub_Unknown = &H0
    SQLDMOFreqSub_Once = &H1
    SQLDMOFreqSub_Minute = &H4
    SQLDMOFreqSub_Hour = &H8
    SQLDMOFreqSub_Valid = &HD
End Enum

Public Sub InitializeSqlDmo(oDbServer As Object, blnError As Boolean, strMessage As String)
    On Error Resume Next
    
    Set oDbServer = Nothing
    Set oDbServer = CreateObject("SQLDMO.SQLServer")
    oDbServer.LoginTimeout = 15
    
    If Err.Number = 0 Then
        blnError = False
        strMessage = strMessage & "SQL-DMO was initialized; "
    Else
        blnError = True
        strMessage = strMessage & DMOError(Err.Number, Err.Description)
        Err.Clear
    End If
    
    On Error GoTo 0
End Sub
Public Sub StartDbServer(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim I As Long
    Dim lngRetVal As Long
    
    blnError = False
    
    On Error GoTo DMOError
    
    Select Case glngOsType
        Case WIN_95_98
            ' Use SQL Server Standard Authentication for Win95 or Win98
            If gblnSqlPreInstalled And (gstrSqlSaPwd = STR_NOT_INITIALIZED) Then
                dlgGetSaPwd.Show vbModal
                If gstrSqlSaPwd = STR_NOT_INITIALIZED Then
                    blnError = True
                    strMessage = strMessage & _
                        "Unable to start database server because user did not supply 'sa' password; "
                    Exit Sub
                End If
                strMessage = strMessage & _
                    "SQL Server authentication will be used, sa password obtained from user; "
            End If
        
            oDbServer.Start True, "(local)", "sa", gstrSqlSaPwd
            
        Case Else
            ' Use NT Authentication Connection for Winnt and Win2K
            oDbServer.Start True, "(local)"
    End Select
    
    I = -2147483647
    Do While oDbServer.Status <> SQLDMOSvc_Running
        I = I + 1
        If I = 2147483647 Then
            Err.Raise 65535, App.EXEName, "Error starting database server."
            GoTo DMOError
        End If
    Loop
        
    If oDbServer.Status <> SQLDMOSvc_Running Then
        blnError = True
        strMessage = strMessage & _
            "Unable to start the local database server; "
    Else
        strMessage = strMessage & _
            "The local database server was started successfully; "
    End If
        
    Exit Sub

DMOError:
    If Err.Number <> 0 Then
        If Err.Number = SQLDMO_E_SVCALREADYRUNNING Then
            Err.Clear
            strMessage = strMessage & _
                "The local database server was already started; "
            DbAdminConnect goDbServer, blnError, strMessage
            
        Else
            blnError = True
            strMessage = strMessage & DMOError(Err.Number, Err.Description)
            Err.Clear
        End If
    End If
    
    Exit Sub
End Sub

Public Sub DbAdminConnect(oDbServer As Object, blnError As Boolean, strMessage As String)
    On Error GoTo DMOError
    
    blnError = False
    
    ' Connect to the SQL Server
    Select Case glngOsType
        Case WIN_95_98
            ' Use SQL Server Standard Authentication for Win95 or Win98
            If gblnSqlPreInstalled And (gstrSqlSaPwd = STR_NOT_INITIALIZED) Then
                dlgGetSaPwd.Show vbModal
                strMessage = strMessage & _
                    "SQL Server authentication will be used, sa password obtained from user; "
            End If
        
            oDbServer.Connect "(local)", "sa", gstrSqlSaPwd
            
        Case Else
            ' Use NT Authentication Connection for Winnt and Win2K
            oDbServer.Connect
    End Select
   
    strMessage = strMessage & _
        "Connection established to local database server; "
    
    gblnDmoConnected = True
    
    On Error GoTo 0
    Exit Sub
DMOError:
    blnError = True
    gstrSqlSaPwd = STR_NOT_INITIALIZED
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CheckSysAdminRole(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim blnIsServerAdmin As Boolean
    
    On Error GoTo DMOError
    
    blnIsServerAdmin = oDbServer.Isserveradmin
    
    If oDbServer.Issysadmin Then
        blnError = False
        strMessage = strMessage & _
            "Connected user is a member of 'sysadmin' role on local database server; "
    Else
        blnError = True
        strMessage = strMessage & _
            "Connected user is not a member of 'sysadmin' role on local database server; "
    End If
    
    On Error GoTo 0
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub SetSysDbAutoGrowth(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim oDb As Object
    
    On Error GoTo DMOError
    
    blnError = False
    
    Set oDb = oDbServer.Databases("master")
    
    EnableAutoGrowth oDb, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    Set oDb = oDbServer.Databases("msdb")
    EnableAutoGrowth oDb, blnError, strMessage
    
    Set oDb = Nothing
    On Error GoTo 0
    
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub EnableAutoGrowth(oDb As Object, blnError As Boolean, strMessage As String)
    Dim oFileGroup As Object
    Dim oDbFile As Object
    Dim oLogFile As Object
    
    On Error GoTo DMOError
    
    blnError = False
    
    ' Configure database file for autogrowth
    For Each oFileGroup In oDb.FileGroups
        For Each oDbFile In oFileGroup.DBFiles
            With oDbFile
                strMessage = strMessage & _
                    "FileGrowthType property of database file '" & .Name & "' detected: " & CStr(.FileGrowthType) & "; "
                        
                strMessage = strMessage & _
                    "FileGrowth property of database file '" & .Name & "' detected: " & CStr(.FileGrowth) & "; "
                    
                strMessage = strMessage & _
                    "MaximumSize property of database file '" & .Name & "' detected: " & CStr(.MaximumSize) & "; "
                    
                Select Case .FileGrowthType
                    Case SQLDMOGrowth_MB
                        If .FileGrowth = 0 Then
                            .FileGrowth = 1
                            strMessage = strMessage & _
                                "FileGrowth property of database file '" & .Name & "' set to: " & CStr(.FileGrowth) & "; "
                        Else
                            strMessage = strMessage & _
                                "Autogrowth already enabled for database file '" & .Name & "'; "
                        End If
                
                    Case SQLDMOGrowth_Percent
                        If .FileGrowth = 0 Then
                            .FileGrowth = 10
                            strMessage = strMessage & _
                                "FileGrowth property of database file '" & .Name & "' set to: " & CStr(.FileGrowth) & "; "
                        Else
                            strMessage = strMessage & _
                                "Autogrowth already enabled for database file '" & .Name & "'; "
                        End If
                    
                    Case Else
                        .FileGrowthType = SQLDMOGrowth_Percent
                        strMessage = strMessage & _
                            "FileGrowthType property of database file '" & .Name & "' set to: " & CStr(.FileGrowthType) & "; "
                        .FileGrowth = 10
                        strMessage = strMessage & _
                            "FileGrowth property of database file '" & .Name & "' set to: " & CStr(.FileGrowth) & "; "
                End Select
                
                If .MaximumSize <> -1 Then
                    .MaximumSize = 0
                    strMessage = strMessage & _
                        "MaximumSize property of database file '" & .Name & "' set to: " & CStr(.MaximumSize) & "; "
                End If
            End With
        Next oDbFile
    Next oFileGroup
    
    Set oDbFile = Nothing
    Set oFileGroup = Nothing
    
    ' Configure transaction log for autogrowth
    For Each oLogFile In oDb.TransactionLog.LogFiles
        With oLogFile
            strMessage = strMessage & _
                "FileGrowthType property of log file '" & .Name & "' detected: " & CStr(.FileGrowthType) & "; "
                
            strMessage = strMessage & _
                "FileGrowth property of log file '" & .Name & "' detected: " & CStr(.FileGrowth) & "; "
            
            strMessage = strMessage & _
                "MaximumSize property of log file '" & .Name & "' detected: " & CStr(.MaximumSize) & "; "
            
            Select Case .FileGrowthType
                Case SQLDMOGrowth_MB
                        
                    If .FileGrowth = 0 Then
                        .FileGrowth = 1
                        strMessage = strMessage & _
                            "FileGrowth property of log file '" & .Name & "' set to: " & CStr(.FileGrowth) & "; "
                    Else
                        strMessage = strMessage & _
                            "Autogrowth already enabled for log file '" & .Name & "'; "
                    End If
            
                Case SQLDMOGrowth_Percent
                    If .FileGrowth = 0 Then
                        .FileGrowth = 10
                        strMessage = strMessage & _
                            "FileGrowth property of log file '" & .Name & "' set to: " & CStr(.FileGrowth) & "; "
                    Else
                        strMessage = strMessage & _
                            "Autogrowth already enabled for log file '" & .Name & "'; "
                    End If
                
                Case Else
                    .FileGrowthType = SQLDMOGrowth_Percent
                    strMessage = strMessage & _
                        "FileGrowthType property of log file '" & .Name & "' set to: " & CStr(.FileGrowthType) & "; "
                    .FileGrowth = 10
                    strMessage = strMessage & _
                        "FileGrowth property of log file '" & .Name & "' set to: " & CStr(.FileGrowth) & "; "
            End Select
            
            If .MaximumSize <> -1 Then
                .MaximumSize = 0
                strMessage = strMessage & _
                    "MaximumSize property of log file '" & .Name & "' set to: " & CStr(.MaximumSize) & "; "
            End If

        End With
    Next oLogFile
    
    Set oLogFile = Nothing
    
    On Error GoTo 0
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Private Function DMOError(lngErr As Long, strDesc As String) As String
    DMOError = _
        "The following SQL-DMO runtime error was encoutered: " & _
        CStr(Err.Number) & "-" & CStr(Err.Description) & "; "
End Function

Public Sub BackupDatabase(oDbServer As Object, strDbName As String, blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim strBackupDir As String
    Dim strBackupName As String
    Dim oBackupDevice As Object
    Dim oBackup As Object
    Dim oResults As Object
    Dim dblBackupSize As Double
    Dim dtBackupTime As Date
    Dim oDrive As Object
    Dim I As Integer
    Dim blnDeviceFound As Boolean
    
    On Error GoTo DMOError
    
    blnError = False
    
        '
    ' Step 1: Get location of SQL Server Backup Directory
    '
    
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\MSSQLServer\BackupDirectory")

    If IsEmpty(varRegValue) Or VarType(varRegValue) <> vbString Then
        blnError = True
        strMessage = strMessage & _
            "Unable to determine location of database server backup directory; "
        Exit Sub
    End If

    strBackupDir = CStr(varRegValue)

    '
    ' Step 2: Check for available space
    '
    
    Set oDrive = goFileSystem.GetDrive(CrackDrive(strBackupDir))
    
    If (oDrive.AvailableSpace / (1024 ^ 2)) < oDbServer.Databases(strDbName).Size Then
        blnError = True
        strMessage = strMessage & _
            "Insufficient space on drive '" & CrackDrive(strBackupDir) & "' to perform backup of database '" & strDbName & "'; "
        Exit Sub
    End If
    
    '
    ' Step 3: Create a Backup Device
    '
    
    ' Generate a unique backup device name
    strBackupName = App.EXEName & "_Backup_" & strDbName
    I = 0
    Do While True
        I = I + 1
        blnDeviceFound = False
        For Each oBackupDevice In oDbServer.BackupDevices
            If oBackupDevice.Name = strBackupName Then
                blnDeviceFound = True
                strBackupName = App.EXEName & "_Backup_" & strDbName & "_" & CStr(I)
                Exit For
            End If
        Next oBackupDevice
        
        If Not blnDeviceFound Then
            Exit Do
        End If
    Loop
        
    Set oBackupDevice = Nothing
    Set oBackupDevice = CreateObject("SQLDMO.BackupDevice")
    
    With oBackupDevice
        .Name = strBackupName
        .PhysicalLocation = strBackupDir & "\" & strBackupName & ".bak"
        .Type = SQLDMODevice_DiskDump
    End With
    
    oDbServer.BackupDevices.Add oBackupDevice
    
    strMessage = strMessage & _
        "Added new backup device named '" & oBackupDevice.PhysicalLocation & "'; "
    
    '
    ' Step 4: Backup Database to newly created device
    '
    
    Set oBackup = CreateObject("SQLDMO.Backup")
    
    With oBackup
        .Action = SQLDMOBackup_Database
        .Database = strDbName
        .Devices = strBackupName
        .Initialize = True
        .BackupSetName = strDbName & "_full"
        .BackupSetDescription = "Full backup of " & strDbName & " database initiated by " & App.EXEName & "."
        
        ' Initiate Backup
        .SQLBackup oDbServer
    End With
    
    Set oBackupDevice = oDbServer.BackupDevices(strBackupName)
    
    Set oResults = oBackupDevice.ReadBackupHeader
    
    With oResults
        dtBackupTime = .GetColumnDate(1, 19) - .GetColumnDate(1, 18)
        
        strMessage = strMessage & _
            "Backup of database '" & .GetColumnString(1, 10) & "' completed successfully; " & _
            "The backup size is " & CStr(.GetColumnDouble(1, 13)) & " bytes; " & _
            "The backup took " & Format(dtBackupTime, "Nn") & " minutes, " & Format(dtBackupTime, "Ss") & " seconds; "
    End With
    
    Set oDrive = Nothing
    Set oResults = Nothing
    Set oBackupDevice = Nothing
    Set oBackup = Nothing
    
    On Error GoTo 0
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub


Public Sub SetMixedSecurity(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim lngSecurityMode As Long
    
    blnError = False
    On Error GoTo DMOError
    
    lngSecurityMode = oDbServer.IntegratedSecurity.SecurityMode
    
    strMessage = strMessage & _
        "Security mode '" & CStr(lngSecurityMode) & "' detected; "
        
    Select Case lngSecurityMode
        Case SQLDMOSecurity_Mixed
            strMessage = strMessage & _
                "Security mode is already configured for mixed security; "
        Case SQLDMOSecurity_Normal, SQLDMOSecurity_Integrated
            strMessage = strMessage & _
                "Reconfiguring security mode to support mixed security; "
            
            oDbServer.IntegratedSecurity.SecurityMode = SQLDMOSecurity_Mixed
        Case Else
            blnError = True
            strMessage = strMessage & _
                "Unknown security mode; "
    End Select
    
    On Error GoTo 0
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub LocateDb(oDbServer As Object, strDbName As String, blnDbFound As Boolean, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    
    blnError = False
    blnDbFound = False
    
    On Error GoTo DMOError
    
    For Each oDatabase In oDbServer.Databases
        With oDatabase
            If UCase(.Name) = UCase(strDbName) Then
                blnDbFound = True
                strMessage = strMessage & _
                    "Database id '" & CStr(.ID) & "' located with name '" & strDbName & "'; "
                Exit For
            End If
        End With
    Next
    
    On Error GoTo 0
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub GetDbVersion(oDbServer As Object, strDbName As String, strDbVersion As String, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oSp As Object
    Dim oResults As Object
    
    Dim blnFound As Boolean
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    
    blnFound = False
    
    For Each oSp In oDatabase.StoredProcedures
        If oSp.Name = goDbApp.strAppDbVerSp Then
            blnFound = True
            Exit For
        End If
    Next
    
    If Not blnFound Then
        strMessage = strMessage & _
            "Stored procedure '" & goDbApp.strAppDbVerSp & "' not located; "
        strDbVersion = STR_NOT_INITIALIZED
        Exit Sub
    End If
    
    Set oResults = oDatabase.ExecuteWithResults(goDbApp.strAppDbVerSp)
    
    strDbVersion = oResults.GetColumnString(1, 1)
    
    On Error GoTo 0
    Exit Sub
            
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub InstallDbFromFile( _
    oDbServer As Object, _
    strAppDbSourceFolder As String, _
    oAppDb As AppDb, _
    strAppDbDestFolder As String, _
    blnError As Boolean, _
    strMessage As String)
    
    Dim strDataFileSourceSpec As String
    Dim strDataFileName As String
    Dim strLogFileName As String
    Dim strDataFileSpec As String
    Dim strLogFileSpec As String
    Dim strStatus As String
    Dim varRegValue As Variant
    Dim blnDbFound As Boolean
    Dim I As Integer
    
    blnError = False
    
    On Error GoTo FileSystemError
       
    '
    ' Step 1: Check for existance of application database data and log file
    '
    
    strDataFileSourceSpec = strAppDbSourceFolder & "\" & oAppDb.strAppDbName & "_Data.MDF"
    
    If Not goFileSystem.FileExists(strDataFileSourceSpec) Then
        blnError = True
        strMessage = strMessage & _
            "Setup was unable to locate application database data file named '" & strDataFileSourceSpec & "'; "
        Exit Sub
    End If
    
    '
    ' Step 2: Check for existance of data directory, if missing create it
    '
    
    CreateDataFileDirectory strAppDbDestFolder, oAppDb.strAppDbName, blnError, strMessage
    
    If blnError Then
        strMessage = strMessage & _
            "Unable to create data file directory named '" & strAppDbDestFolder & "'; "
        Exit Sub
    End If
   
    '
    ' Step 3: Generate unique file names for destination data and log files
    '
        
    GenerateUniqueDataFileNames oAppDb.strAppDbName, strAppDbDestFolder, strDataFileName, strLogFileName, blnError, strMessage
    
    If blnError Then
        strMessage = strMessage & _
            "Unable to generate unique data file names; "
        Exit Sub
    End If
    
    strDataFileSpec = strAppDbDestFolder & "\" & strDataFileName
    strLogFileSpec = strAppDbDestFolder & "\" & strLogFileName
    
    '
    ' Step 4: Copy data file to data directory and remove read-only attribute
    '
    
    Dim fu As dlgProgressBar
    Set fu = New dlgProgressBar
    fu.Visible = True
    fu.lblStatus = "Copying application database from CD..."
    fu.Refresh
    CopyFile strDataFileSourceSpec, strDataFileSpec, fu
    fu.Visible = False
    Set fu = Nothing
'    goFileSystem.CopyFile strDataFileSourceSpec, strDataFileSpec, True
        
    blnError = ClearReadOnly(strDataFileSpec)
    
    If blnError Then
        strMessage = strMessage & _
            "Setup encountered an error when trying to clear the read-only attribute of the file '" & strDataFileSpec & "'; "
        Exit Sub
    End If
    
    '
    ' Step 5: Attach data file and set database options
    '
    
    On Error GoTo DMOError
    
    strStatus = oDbServer.AttachDbWithSingleFile(oAppDb.strAppDbName, strDataFileSpec)
    
    blnDbFound = False
    LocateDb oDbServer, oAppDb.strAppDbName, blnDbFound, blnError, strMessage
    
    If blnError Then
        strMessage = strMessage & _
            "Setup encountered an error when trying to attach the data file '" & strDataFileName & "'; "
        Exit Sub
    End If
    
    If blnDbFound Then
        strMessage = strMessage & _
            "The database '" & oAppDb.strAppDbName & "' was successfully created from the data file '" & strDataFileSpec & "'; "
            
        SetDbOptions oDbServer, oAppDb.strAppDbName, blnError, strMessage
    Else
        blnError = True
        strMessage = strMessage & _
            "Setup encountered an error when trying to attach the data file '" & strDataFileSpec & "'; "
    End If
        
    ' run script to fix login
    Dim oDatabase As Object
    Set oDatabase = CreateObject("SQLDMO.Database")
    Set oDatabase = oDbServer.Databases(oAppDb.strAppDbName)    'oAppDb passed in
    oDatabase.executeimmediate ("sp_change_users_login 'auto_fix', 'fcuser'")
    oDatabase.executeimmediate ("sp_password NULL, 'fcuser', 'fcuser'")
    oDatabase.executeimmediate ("sp_password NULL, 'amdcamp', 'sa'")
    Set oDatabase = Nothing
    
    On Error GoTo 0
    Exit Sub
    
FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub

DMOError:
    blnError = True
    strMessage = "InstallDbFromFile() " & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub InstallDbFromScript( _
    oDbServer As Object, _
    strAppDbSourceFolder As String, _
    oAppDb As AppDb, _
    strAppDbDestFolder As String, _
    blnError As Boolean, _
    strMessage As String)
    
    Dim strDDLFileSpec As String
    Dim strLoadListFileSpec As String
    Dim strLoadList() As String
    Dim oTextFile As Object
    Dim strCommandBatch As String
    Dim strDataFileName As String
    Dim strLogFileName As String
    Dim strDataFileSpec As String
    Dim strLogFileSpec As String
    Dim oDatabase As Object
    Dim oDataFile As Object
    Dim oLogFile As Object
    Dim oTable As Object
    Dim blnFound As Boolean
    Dim strBcpFileSpec As String
    Dim strBcpErrFileSpec As String
    Dim strBcpLogFileSpec As String
    Dim I As Long
    Dim oBcp As Object
    Dim lngNumRows As Long
    
    blnError = False
    
    '
    ' Step 1: Validate parameters
    '
    
    strDDLFileSpec = strAppDbSourceFolder & "\" & oAppDb.strAppDbName & "_DDL_Script.SQL"
    
    If Not goFileSystem.FileExists(strDDLFileSpec) Then
        blnError = True
        strMessage = strMessage & _
            "Setup was unable to locate the application database DDL script named '" & strDDLFileSpec & "'; "
        Exit Sub
    End If
    
    strLoadListFileSpec = strAppDbSourceFolder & "\" & oAppDb.strAppDbName & "_Table_LoadList.TXT"
    
    If Not goFileSystem.FileExists(strLoadListFileSpec) Then
        blnError = True
        strMessage = strMessage & _
            "Setup was unable to locate the load list text file named '" & strLoadListFileSpec & "'; "
        Exit Sub
    End If
    
    '
    ' Step 2: Build array of tables to load
    '
    
    On Error GoTo FileSystemError
    
    If gblnUnicodeAppDbFiles Then
        Set oTextFile = goFileSystem.OpenTextFile(strLoadListFileSpec, FORREADING, False, TristateTrue)
    Else
        Set oTextFile = goFileSystem.OpenTextFile(strLoadListFileSpec, FORREADING, False, TristateFalse)
    End If
        
    I = 0
    Do While Not oTextFile.AtEndOfStream
        I = I + 1
        ReDim Preserve strLoadList(1 To I)
        strLoadList(I) = oTextFile.ReadLine
    Loop
    
    oTextFile.Close
    Set oTextFile = Nothing
    
    strMessage = strMessage & _
        "Load list created from file '" & strLoadListFileSpec & "'; " & _
        CStr(I) & " tables will be loaded; "
    
    '
    ' Step 3: Load DDL command batch
    '
    
    If gblnUnicodeAppDbFiles Then
        Set oTextFile = goFileSystem.OpenTextFile(strDDLFileSpec, FORREADING, False, TristateTrue)
    Else
        Set oTextFile = goFileSystem.OpenTextFile(strDDLFileSpec, FORREADING, False, TristateFalse)
    End If
    
    strCommandBatch = ""
    
    Do While Not oTextFile.AtEndOfStream
        strCommandBatch = strCommandBatch & oTextFile.ReadLine & vbCrLf
    Loop
    
    oTextFile.Close
    
    If strCommandBatch = "" Then
        blnError = True
        strMessage = strMessage & _
            "Invalid command batch; "
        Exit Sub
    End If
    
    '
    ' Step 4: check for data file directory and generate data file names
    '
    
    CreateDataFileDirectory strAppDbDestFolder, oAppDb.strAppDbName, blnError, strMessage
    
    If blnError Then
        strMessage = strMessage & _
            "Unable to create data file directory named '" & strAppDbDestFolder & "'; "
        Exit Sub
    End If
    
    GenerateUniqueDataFileNames oAppDb.strAppDbName, strAppDbDestFolder, strDataFileName, strLogFileName, blnError, strMessage
    
    If blnError Then
        strMessage = strMessage & _
            "Unable to generate unique data file names; "
        Exit Sub
    End If
    
    strDataFileSpec = strAppDbDestFolder & "\" & strDataFileName
    strLogFileSpec = strAppDbDestFolder & "\" & strLogFileName
    
    '
    ' Step 5: Create database
    '
    
    On Error GoTo DMOError
    
    Set oDatabase = CreateObject("SQLDMO.Database")
    Set oDataFile = CreateObject("SQLDMO.DBFile")
    Set oLogFile = CreateObject("SQLDMO.LogFile")
    
    oDatabase.Name = oAppDb.strAppDbName

    ' Define the PRIMARY data file.
    oDataFile.Name = oAppDb.strAppDbName & "_Data"
    oDataFile.PhysicalName = strDataFileSpec
    oDataFile.PrimaryFile = True

    ' Specify file growth in chunks of fixed size for all data files.
    oDataFile.FileGrowthType = SQLDMOGrowth_MB
    oDataFile.FileGrowth = 1
    oDatabase.FileGroups("PRIMARY").DBFiles.Add oDataFile

    ' Define the database transaction log.
    oLogFile.Name = oAppDb.strAppDbName & "_Log"
    oLogFile.PhysicalName = strLogFileSpec
    oDatabase.TransactionLog.LogFiles.Add oLogFile

    ' Create the database
    oDbServer.Databases.Add oDatabase
    Set oDatabase = oDbServer.Databases(oAppDb.strAppDbName)
    
    ' Set the database options
    SetDbOptions oDbServer, oAppDb.strAppDbName, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    '
    ' Step 6: Run DDL Script to create database objects
    '
    
    oDatabase.executeimmediate (strCommandBatch)
    
    '
    ' Step 7: Load Tables
    '
    
    Set oBcp = CreateObject("SQLDMO.BulkCopy")
    With oBcp
        .DataFileType = SQLDMODataFile_NativeFormat
        .MaximumErrorsBeforeAbort = 0&
        .IncludeIdentityValues = True
    End With

    ' Loop through each table specified in the load list
    For I = 1 To UBound(strLoadList)
        blnFound = False
        
        ' Make sure the table exists
        For Each oTable In oDatabase.Tables
            If UCase(oTable.Name) = UCase(strLoadList(I)) Then
                blnFound = True
                Exit For
            End If
        Next
        
        If Not blnFound Then
            blnError = True
            strMessage = strMessage & _
                "The table named '" & strLoadList(I) & "' in the load list file named '" & strLoadListFileSpec & "' does not exist in the database named '" & oAppDb.strAppDbName & "'; "
            Exit Sub
        End If
        
        ' Setup file specifications
        
        strBcpFileSpec = strAppDbSourceFolder & "\" & strLoadList(I) & ".BCP"
        strBcpErrFileSpec = gstrTempDir & "\" & strLoadList(I) & ".ERR"
        strBcpLogFileSpec = gstrTempDir & "\" & strLoadList(I) & ".LOG"
        
        On Error GoTo FileSystemError
        
        If Not goFileSystem.FileExists(strBcpFileSpec) Then
            blnError = True
            strMessage = strMessage & _
                "The bcp file named '" & strBcpFileSpec & "' could not be found; "
            Exit Sub
        End If
        
        If goFileSystem.FileExists(strBcpErrFileSpec) Then
            goFileSystem.DeleteFile (strBcpErrFileSpec)
        End If
        
        If goFileSystem.FileExists(strBcpLogFileSpec) Then
            goFileSystem.DeleteFile (strBcpLogFileSpec)
        End If
            
        On Error GoTo DMOError
        
        With oBcp
            .DataFilePath = strBcpFileSpec
            .ErrorFilePath = strBcpErrFileSpec
            .LogFilePath = strBcpLogFileSpec
        End With
        
        lngNumRows = oTable.ImportData(oBcp)
        
        strMessage = strMessage & _
            CStr(lngNumRows) & " rows were imported into the table named '" & strLoadList(I) & "' from the bcp file named '" & strBcpFileSpec & "'; "
        
        On Error GoTo FileSystemError
        
        If goFileSystem.FileExists(strBcpErrFileSpec) Then
            Set oTextFile = goFileSystem.GetFile(strBcpErrFileSpec)
            If oTextFile.Size > 0 Then
                blnError = True
                strMessage = strMessage & _
                    "Error occurred while loading data from the file named '" & strBcpFileSpec & "' into the table named " & _
                    strLoadList(I) & "', see '" & strBcpErrFileSpec & "' for details; "
                Exit Sub
            End If
            Set oTextFile = Nothing
        End If
        
        On Error GoTo DMOError
    Next I
    
    Exit Sub

FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub SetDbOptions(oDbServer As Object, strDatabaseName As String, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDatabaseName)
    
    ' Set database options
    With oDatabase.DBOption
        .TornPageDetection = True
        .TruncateLogOnCheckpoint = True
        .AutoClose = True
        .AutoShrink = True
    End With
    
    strMessage = strMessage & _
        "Database options set for database '" & strDatabaseName & "'; "
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub AddLogin(oDbServer As Object, strAccount As String, blnNTAccount As Boolean, blnLoginAdded As Boolean, blnError As Boolean, strMessage As String, Optional strPassword As String)
    Dim oLogin As Object
    blnError = False
    blnLoginAdded = False
    
    On Error GoTo DMOError
    
    For Each oLogin In oDbServer.Logins
        If UCase(oLogin.Name) = UCase(strAccount) Then
            ' Login already exists so get out
            Exit Sub
        End If
    Next
    
    Set oLogin = CreateObject("SQLDMO.Login")
    
    With oLogin
        If blnNTAccount Then
            .Type = SQLDMOLogin_NTUser
        Else
            .Type = SQLDMOLogin_Standard
        End If
        
        .Name = strAccount
        .Database = gstrAppDbName
    End With
    
    oDbServer.Logins.Add oLogin
    
    If Not blnNTAccount And strPassword <> "" Then
        Set oLogin = oDbServer.Logins(strAccount)
        oLogin.SetPassword "", strPassword
    End If
    
    blnLoginAdded = True
    WriteLogMsg goLogFileHandle, "The login '" & strAccount & "' was granted access to the database server."
     
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub AddDbUser(oDbServer As Object, strDbName As String, strLogin As String, blnUserAdded As Boolean, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oUser As Object
    
    blnError = False
    blnUserAdded = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    
    For Each oUser In oDatabase.Users
        If UCase(oUser.Login) = UCase(strLogin) Then
            ' User Already Exists in Database, don't add it
            Exit Sub
        End If
    Next
    
    Set oUser = CreateObject("SQLDMO.User")
    
    With oUser
        .Login = strLogin
    End With
    
    oDatabase.Users.Add oUser
    blnUserAdded = True
    
    WriteLogMsg goLogFileHandle, "The login '" & strLogin & "' was granted access to the database '" & strDbName & "';"
    
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    MsgBox strMessage
    Err.Clear
    Exit Sub
End Sub

Public Sub AddUserToRole(oDbServer As Object, strDbName As String, strRoleName As String, strUserName As String, blnUserAdded As Boolean, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oDbRole As Object
    Dim oResult As Object
    Dim I As Integer
    
    blnError = False
    blnUserAdded = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    Set oDbRole = oDatabase.DatabaseRoles(strRoleName)
    
    Set oResult = oDbRole.EnumDatabaseRoleMember
    
    For I = 1 To oResult.Rows
        If UCase(oResult.GetColumnString(I, 1)) = UCase(strUserName) Then
            ' User already role member so get out
            Exit Sub
        End If
    Next I
    
    oDbRole.AddMember (strUserName)
    blnUserAdded = True
    
    WriteLogMsg goLogFileHandle, "The user '" & strUserName & "' was added to the role '" & _
        strRoleName & "' in the database '" & strDbName & "';"
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub GetDbRoleNames(oDbServer As Object, strDbName As String, strRoleNames() As String, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oDbRole As Object
    Dim I As Integer
    
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    
    ' Add system defined public role to array
    I = 1
    ReDim Preserve strRoleNames(1 To I)
    strRoleNames(I) = "public"
    
    ' Add all user defined database roles to array
    For Each oDbRole In oDatabase.DatabaseRoles
        If Not oDbRole.IsFixedRole Then
            I = I + 1
            ReDim Preserve strRoleNames(1 To I)
            strRoleNames(I) = oDbRole.Name
        End If
    Next
        
    ' Add system defined db_owner role to array
    I = I + 1
    ReDim Preserve strRoleNames(1 To I)
    strRoleNames(I) = "db_owner"
    
    strMessage = strMessage & _
        CStr(I) & " database security roles were detected in application database; "
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub RenameDb(oDbServer As Object, strOldDbName As String, strNewDbName As String, blnError As Boolean, strMessage As String)
    Dim oDb As Object
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDb = oDbServer.Databases(strOldDbName)
    
    If Not oDb.SystemObject Then
        With oDb
            .DBOption.SingleUser = True
            .Name = strNewDbName
            .DBOption.SingleUser = False
        End With
        strMessage = strMessage & _
            "The database named '" & strOldDbName & "' was renamed to '" & strNewDbName & "';"
    Else
        blnError = True
        strMessage = strMessage & _
            "Could not rename the database '" & strOldDbName & "' because it is a system object; "
    End If
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub DeleteDb(oDbServer As Object, strDbName As String, blnError As Boolean, strMessage As String)
    Dim oDb As Object
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDb = oDbServer.Databases(strDbName)
    
    If Not oDb.SystemObject Then
        oDb.Remove
        strMessage = strMessage & _
            "The database named '" & strDbName & "' was deleted;"
    Else
        blnError = True
        strMessage = strMessage & _
            "Could not delete the database '" & strDbName & "' because it is a system object; "
    End If
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub ConfigServerForAutoTuning(oDbServer As Object, blnError As Boolean, strMessage As String)
    ' Ensures all performance and resource allocation related server configuration options
    ' are configured for auto tuning
    Dim blnReconfigured As Boolean
    Dim strOption As String
    Dim lngValue As Long
    
    On Error GoTo DMOError
    
    blnError = False
    blnReconfigured = False
    
    With oDbServer.Configuration
        .ShowAdvancedOptions = True
    
        strOption = "affinity mask"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "allow updates"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "cost threshold for parallelism"
        lngValue = 5
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "cursor threshold"
        lngValue = -1
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "fill factor (%)"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "index create memory (KB)"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "language in cache"
        lngValue = 3
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "lightweight pooling"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "locks"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "max async IO"
        lngValue = 32
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "max degree of parallelism"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "max server memory (MB)"
        lngValue = 2147483647
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "max text repl size (B)"
        lngValue = 65536
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "max worker threads"
        lngValue = 255
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "max worker threads"
        lngValue = 255
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "min memory per query (KB)"
        lngValue = 1024
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "min server memory (MB)"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "network packet size (B)"
        lngValue = 4096
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "open objects"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "priority boost"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "query governor cost limit"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "query wait (s)"
        lngValue = -1
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "recovery interval (min)"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "resource timeout (s)"
        lngValue = 10
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "set working set size"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "spin counter"
        lngValue = 10000
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "time slice (ms)"
        lngValue = 100
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "user connections"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        strOption = "user connections"
        lngValue = 0
        SetServerOption oDbServer, strOption, lngValue, blnReconfigured, blnError, strMessage
        If blnError Then
            Exit Sub
        End If
        
        If blnReconfigured Then
            .ReconfigureCurrentValues
            strMessage = strMessage & _
                "The local database server was reconfigured; "
        End If
        
        .ShowAdvancedOptions = False
    End With
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub SetServerOption(oDbServer As Object, strOption As String, lngValue As Long, blnReconfigured As Boolean, blnError As Boolean, strMessage As String)
    ' Checks a database server configuration option to see if it is equal to a supplied value
    ' If it is not equal, the option is reconfigured.
    blnError = False
    On Error GoTo DMOError
    
    With oDbServer.Configuration
        If .ConfigValues(strOption).CurrentValue <> lngValue Then
            .ConfigValues(strOption).CurrentValue = lngValue
            blnReconfigured = True
            strMessage = strMessage & _
                "'" & strOption & "' config option set to '" & CStr(lngValue) & "'; "
        End If
    End With
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub GetServerOption(oDbServer As Object, strOption As String, lngValue As Long, blnError As Boolean, strMessage As String)
    ' Checks a database server configuration option to see if it is equal to a supplied value
    ' If it is not equal, the option is reconfigured.
    blnError = False
    On Error GoTo DMOError
    
    lngValue = oDbServer.Configuration.ConfigValues(strOption).CurrentValue
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Sub GetSortOrder(oDbServer As Object, blnError As Boolean, lngSortOrder As Long, strMessage As String)
    On Error GoTo DMOError
    
    blnError = False
    
    With oDbServer.Configuration
        .ShowAdvancedOptions = True
        
        GetServerOption oDbServer, "default sortorder id", lngSortOrder, blnError, strMessage
        
        .ShowAdvancedOptions = False
    End With
    
    On Error GoTo 0
        strMessage = strMessage & _
        "Sort Order '" & CStr(lngSortOrder) & "' detected; "
        
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub GetUnicodeConfig(oDbServer As Object, blnError As Boolean, lngLocaleId As Long, lngCompStyle As Long, strMessage As String)
    On Error GoTo DMOError
    
    blnError = False
    
    With oDbServer.Configuration
        .ShowAdvancedOptions = True
        
        GetServerOption oDbServer, "Unicode locale id", lngLocaleId, blnError, strMessage
        GetServerOption oDbServer, "Unicode comparison style", lngCompStyle, blnError, strMessage
        
        .ShowAdvancedOptions = False
    End With
    
    strMessage = strMessage & _
        "Unicode locale id '" & CStr(lngLocaleId) & "' detected; " & _
        "Unicode comparison style '" & CStr(lngCompStyle) & "' detected; "
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub StartJobServer(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim I As Long
    blnError = False
    
    On Error GoTo DMOError
    
    With oDbServer.JobServer
        If .Status = SQLDMOSvc_Running Then
            strMessage = strMessage & _
                "The job server was already started; "
        Else
            .Start
            
            I = -2147483647
            Do While .Status <> SQLDMOSvc_Running
                I = I + 1
                DoEvents
                If I = 2147483647 Then
                    Err.Raise 65535, App.EXEName, "Error starting job server."
                    GoTo DMOError
                End If
            Loop
            
            strMessage = strMessage & _
                "Job server started successfully; "
        End If
    End With
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub FindJob(oDbServer As Object, strJobName As String, blnFound As Boolean, blnError As Boolean, strMessage As String)
    Dim oJob As Object
    
    On Error GoTo DMOError
    
    blnError = False
    blnFound = False
    
    For Each oJob In oDbServer.JobServer.Jobs
        If oJob.Name = strJobName Then
            blnFound = True
            Exit For
        End If
    Next
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub RemoveJob(oDbServer As Object, strJobName As String, blnError As Boolean, strMessage As String)
    Dim oJob As Object
    
    On Error GoTo DMOError
    
    blnError = False
    
    oDbServer.JobServer.Jobs(strJobName).Remove
    
    strMessage = strMessage & _
        "The job '" & strJobName & "' was removed; "
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CreateDailyBackupJob( _
    oDbServer As Object, _
    strJobName As String, _
    strScriptFileSpec As String, _
    strDbName As String, _
    blnError As Boolean, _
    strMessage As String)
    
    ' Adds a new SQL Server Agent Job which runs an arbitrary script file using the
    ' windows scripting host
    
    Dim oJobSchedule As Object
    Dim oJobStep As Object
    Dim oJob As Object
    Dim oResults As Object
    
    blnError = False
    
    On Error GoTo DMOError
    
    '
    ' Step 1: Create a job schedule
    '
    
    Set oJobSchedule = CreateObject("SQLDMO.JobSchedule")
    
    With oJobSchedule
        .Name = "Daily Schedule"
        .Enabled = True
    End With
    
    With oJobSchedule.Schedule
        .FrequencyType = SQLDMOFreq_Daily
        .FrequencyInterval = 1
        .ActiveStartTimeOfDay = 233000
    End With
    
    '
    ' Step 2: Create a job step
    '
    
    Set oJobStep = CreateObject("SQLDMO.JobStep")
    
    With oJobStep
        .Name = "Step 1"
        .StepID = 1
        .SubSystem = "CmdExec"
        .DatabaseName = strDbName
        .CmdExecSuccessCode = 0
                
        ' Build command line
        .Command = "CSCRIPT """ & strScriptFileSpec & """" & " ""-D:" & strDbName & """"
        ' Assume script connects using Windows NT Authentication unless running Win95 / Win98
        If glngOsType = WIN_95_98 Then
            ' Append sa login id and password to command execution string when running on Win98
            ' Job properties can only be viewed / altered by members of sysadmin role so this is not a security breach
            ' Command string will no longer work if sa password changes
            .Command = .Command & " ""-U:sa"" ""-P:" & gstrSqlSaPwd & """"
        End If
    End With
    
    '
    ' Step 3: Create a job object
    '
    
    Set oJob = CreateObject("SQLDMO.Job")
    
    With oJob
        .Name = strJobName
        .Category = "Database Maintenance"
        .Description = _
            "Runs the script named '" & strScriptFileSpec & "'"
        .Enabled = True
        .EventLogLevel = SQLDMOComp_Always
        ' Make owner sa because jobs won't run with NT Authentication in a disconnected state
        .Owner = "sa"
    End With
        
    oDbServer.JobServer.Jobs.Add oJob
    
    '
    ' Step 4: Append job step / schedule to new job and define target server
    '
    
    Set oJob = oDbServer.JobServer.Jobs(strJobName)
    
    With oJob
        .JobSchedules.Add oJobSchedule
        .JobSteps.Add oJobStep
        
        .ApplyToTargetServer "(local)"
        strMessage = strMessage & _
            "Job id '" & .JobID & "' named '" & .Name & "' was added; "
    End With
    
        
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub DmoDisconnect(oDbServer As Object, blnError As Boolean, strMessage As String)
    blnError = False
    
    On Error GoTo DMOError
    
    If gblnDmoConnected Then
        oDbServer.DisConnect
        gblnDmoConnected = False
        strMessage = strMessage & _
            "Current user was disconnected from the database server; "
        
        Set oDbServer = Nothing
    Else
        strMessage = strMessage & _
            "There was no active connection to disconnect; "
    End If
    
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
    
End Sub
Public Sub GetSqlPkgType(oDbServer As Object, lngPackage As DbServerType, blnError As Boolean, strMessage As String)
    blnError = False
    
    '
    ' Step 1: Connect to database server
    '
    
    If Not gblnDmoConnected Then
        ConnectUsingDmo oDbServer, blnError, strMessage
        
        If blnError Then
            Exit Sub
        End If
    End If
    
    '
    ' Step 2: Check Package Type
    '
    
    On Error GoTo DMOError
    
    lngPackage = oDbServer.IsPackage
    
    If gblnDmoConnected Then
        DmoDisconnect oDbServer, blnError, strMessage
    End If
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub AutoStartServices(oDbServer As Object, blnError As Boolean, strMessage As String)
    blnError = False
    
    On Error GoTo DMOError
    
    With oDbServer
        .Registry.AutoStartServer = True
        .JobServer.AutoStart = True
    End With
    
    strMessage = strMessage & _
        "Support services configured for AutoStart; "
        
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub RunScript(oDbServer As Object, strScriptFileSpec As String, blnError As Boolean, strMessage As String)
    Dim oTextFile As Object
    Dim strCommandBatch As String
    
    blnError = False
    
    On Error GoTo FileSystemError
    
    '
    ' Step 1: Load DDL command batch
    '
    
    Set oTextFile = goFileSystem.OpenTextFile(strScriptFileSpec, FORREADING, False, TristateFalse)
    
    strCommandBatch = ""
    
    Do While Not oTextFile.AtEndOfStream
        strCommandBatch = strCommandBatch & oTextFile.ReadLine & vbCrLf
    Loop
    
    oTextFile.Close
    
    If strCommandBatch = "" Then
        blnError = True
        strMessage = strMessage & _
            "Invalid command batch; "
        Exit Sub
    End If
    
    On Error GoTo DMOError
    
    '
    ' Step 6: Run DDL Script to create database objects
    '
    
    oDbServer.executeimmediate (strCommandBatch)
    
    strMessage = strMessage & _
        "The script file '" & strScriptFileSpec & "' was executed successfully on the local database server; "
    
    Exit Sub
    
FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CreateAutoExecSp(oDbServer As Object, strSqlSaPwd As String, blnError As Boolean, strMessage As String)
    Dim oSp As Object
    Dim strSpName As String
    
    blnError = False
    strSpName = "Run_" & gstrJobServerStartupSpName
    
    On Error GoTo DMOError
    
    For Each oSp In oDbServer.Databases("master").StoredProcedures
        If oSp.Name = strSpName Then
            oSp.Remove
            strMessage = strMessage & _
                "Deleted existing stored procedure named '" & strSpName & "'; "
            Exit For
        End If
    Next
        
    Set oSp = Nothing
    Set oSp = CreateObject("SQLDMO.StoredProcedure")
    
    With oSp
        .Name = strSpName
        .Text = _
            " CREATE PROC " & strSpName & " WITH ENCRYPTION AS " & vbCrLf & _
            "   EXEC " & gstrJobServerStartupSpName & " " & IIf(strSqlSaPwd <> "", """" & strSqlSaPwd & """", "")
        .Startup = True
    End With
    
    oDbServer.Databases("master").StoredProcedures.Add oSp
    
    strMessage = strMessage & _
        "Created new autoexec stored procedure named '" & strSpName & "'; "
        
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub StopDbServices(oDbServer As Object, blnError As Boolean, strMessage As String)
    blnError = False
    
    On Error GoTo DMOError
    
    With oDbServer.JobServer
        If .Status = SQLDMOSvc_Running Then
            .Stop
            strMessage = strMessage & _
                "Job server was stopped; "
        End If
    End With
    
    With oDbServer
        If .Status = SQLDMOSvc_Running Then
            .Stop
            strMessage = strMessage & _
                "Database server was stopped; "
        End If
    End With
    
    Set oDbServer = Nothing
    gblnDmoConnected = False
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub
