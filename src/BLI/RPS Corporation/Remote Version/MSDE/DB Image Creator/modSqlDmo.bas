Attribute VB_Name = "modSqlDmo"
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' modSqlDmo.bas module isolates all SQL-DMO related procedures from rest of application '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit
Option Base 1

Private Const SQLDMO_E_SVCALREADYRUNNING = 1056& ' SQL-DMO Error thrown when trying to start a server that is already started, not documented in SQL BOL

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

Private Enum SQLDMO_SCRIPT_TYPE
    SQLDMOScript_Default = &H4
    SQLDMOScript_Drops = &H1
    SQLDMOScript_ObjectPermissions = &H2
    SQLDMOScript_PrimaryObject = &H4
    SQLDMOScript_ClusteredIndexes = &H8
    SQLDMOScript_Triggers = &H10
    SQLDMOScript_DatabasePermissions = &H20
    SQLDMOScript_Permissions = &H22
    SQLDMOScript_ToFileOnly = &H40
    SQLDMOScript_Bindings = &H80
    SQLDMOScript_AppendToFile = &H100
    SQLDMOScript_NoDRI = &H200
    SQLDMOScript_UDDTsToBaseType = &H400
    SQLDMOScript_IncludeIfNotExists = &H1000
    SQLDMOScript_NonClusteredIndexes = &H2000
    SQLDMOScript_Indexes = &H12008
    SQLDMOScript_Aliases = &H4000
    SQLDMOScript_NoCommandTerm = &H8000
    SQLDMOScript_DRIIndexes = &H10000
    SQLDMOScript_IncludeHeaders = &H20000
    SQLDMOScript_OwnerQualify = &H40000
    SQLDMOScript_TimestampToBinary = &H80000
    SQLDMOScript_SortedData = &H100000
    SQLDMOScript_SortedDataReorg = &H200000
    SQLDMOScript_TransferDefault = &H670FF
    SQLDMOScript_DRI_NonClustered = &H400000
    SQLDMOScript_DRI_Clustered = &H800000
    SQLDMOScript_DRI_Checks = &H1000000
    SQLDMOScript_DRI_Defaults = &H2000000
    SQLDMOScript_DRI_UniqueKeys = &H4000000
    SQLDMOScript_DRI_ForeignKeys = &H8000000
    SQLDMOScript_DRI_PrimaryKey = &H10000000
    SQLDMOScript_DRI_AllKeys = &H1C000000
    SQLDMOScript_DRI_AllConstraints = &H1F000000
    SQLDMOScript_DRI_All = &H1FC00000
    SQLDMOScript_DRIWithNoCheck = &H20000000
    SQLDMOScript_NoIdentity = &H40000000
    SQLDMOScript_UseQuotedIdentifiers = &H80000000
End Enum

Private Enum SQLDMO_SCRIPT2_TYPE
    SQLDMOScript2_Default = &H0
    SQLDMOScript2_AnsiPadding = &H1
    SQLDMOScript2_AnsiFile = &H2
    SQLDMOScript2_UnicodeFile = &H4
    SQLDMOScript2_NonStop = &H8
    SQLDMOScript2_NoFG = &H10
    SQLDMOScript2_MarkTriggers = &H20
    SQLDMOScript2_OnlyUserTriggers = &H40
    SQLDMOScript2_EncryptPWD = &H80
    SQLDMOScript2_SeparateXPs = &H100
    SQLDMOScript2_NoWhatIfIndexes = &H200
    SQLDMOScript2_AgentNotify = &H400
    SQLDMOScript2_AgentAlertJob = &H800
    SQLDMOScript2_FullTextIndex = &H80000
    SQLDMOScript2_LoginSID = &H100000
    SQLDMOScript2_FullTextCat = &H200000
End Enum

Private Enum SQLDMO_COPYDATA_TYPE
    SQLDMOCopyData_False = 0&
    SQLDMOCopyData_Replace
    SQLDMOCopyData_Append
End Enum

Private Enum SQLDMO_XFRSCRIPTMODE_TYPE
    SQLDMOXfrFile_Default = &H1
    SQLDMOXfrFile_SummaryFiles = &H1
    SQLDMOXfrFile_SingleFile = &H2
    SQLDMOXfrFile_SingleFilePerObject = &H4
    SQLDMOXfrFile_SingleSummaryFile = &H8
End Enum

Private Enum SQLDMO_DATAFILE_TYPE
    SQLDMODataFile_CommaDelimitedChar = &H1
    SQLDMODataFile_Default = &H1
    SQLDMODataFile_TabDelimitedChar = &H2
    SQLDMODataFile_SpecialDelimitedChar = &H3
    SQLDMODataFile_NativeFormat = &H4
    SQLDMODataFile_UseFormatFile = &H5
End Enum

Private Enum SQLDMO_DEPENDENCY_TYPE
    SQLDMODep_Parents = 0
    SQLDMODep_FullHierarchy = &H10000
    SQLDMODep_OrderDescending = &H20000
    SQLDMODep_Children = &H40000
    SQLDMODep_ReturnInputObject = &H80000
    SQLDMODep_FirstLevelOnly = &H100000
    SQLDMODep_DRIOnly = &H200000
    SQLDMODep_Valid = &H3F0000
End Enum

Private Enum SQLDMO_OBJECT_TYPE
    SQLDMOObj_Unknown = 16384
    SQLDMOObj_Application = 0

    ' Database objects, value is power(2, sysobjects.sysstat & 0x0f), plus UDDTs in 0.
    SQLDMOObj_UserDefinedDatatype = &H1
    SQLDMOObj_SystemTable = &H2
    SQLDMOObj_View = &H4
    SQLDMOObj_UserTable = &H8
    SQLDMOObj_StoredProcedure = &H10
    SQLDMOObj_Default = &H40
    SQLDMOObj_Rule = &H80
    SQLDMOObj_Trigger = &H100
    SQLDMOObj_AllDatabaseUserObjects = &H1FD
    SQLDMOObj_AllDatabaseObjects = &H1FF
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

Private Function DMOError(lngErr As Long, strDesc As String) As String
    DMOError = _
        "The following SQL-DMO runtime error was encoutered: " & _
        CStr(Err.Number) & "-" & CStr(Err.Description) & "; "
End Function

Public Sub StartDbServer(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim i As Long
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
    
    i = -2147483647
    Do While oDbServer.Status <> SQLDMOSvc_Running
        i = i + 1
        If i = 2147483647 Then
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

Public Sub CheckSysAdminRole(oDbServer As Object, blnError As Boolean, strMessage As String)
    Dim blnIsServerAdmin As Boolean
    
    On Error GoTo DMOError
    
    blnIsServerAdmin = oDbServer.Isserveradmin
    
    If oDbServer.Issysadmin Then
        blnError = False
        gblnDmoConnected = True
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

Public Sub LoadAppDbNames(oDbServer As Object, AppDbNames() As String, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim i As Integer
    
    blnError = False
    i = 0
    
    For Each oDatabase In oDbServer.Databases
        If Not oDatabase.SystemObject Then
            i = i + 1
            ReDim Preserve AppDbNames(i)
            AppDbNames(i) = oDatabase.Name
        End If
    Next
    
    If i = 0 Then
        blnError = True
        strMessage = "There are no application databases on the local database server."
    End If
    
    Exit Sub
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub GetDbSize(oDbServer As Object, strAppDbName As String, lngDbSizeBytes As Long, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strAppDbName)
    
    lngDbSizeBytes = oDatabase.Size * (1024 ^ 2)
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CreateDbInstallImage( _
    oDbServer As Object, _
    oAppDb As AppDb, _
    strPath As String, _
    strTableList() As String, _
    lngTableListCount As Long, _
    blnError As Boolean, _
    strMessage As String)
    
    Dim oDatabase As Object
    Dim oTransfer As Object
    Dim strScript As String
    Dim strScriptFileSpec As String
    Dim strTableName As Variant
    Dim oFileGroup As Object
    Dim oDbFile As Object
    Dim strFileSrcSpec As String
    Dim strFileDestSpec As String
    Dim blnDbFound As Boolean
    
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(oAppDb.strAppDbName)
    
    '
    ' Step 1: Create Script Based Application Database Installation Image
    '
    
    ' Configure script generation options
    
    Set oTransfer = CreateObject("SQLDMO.Transfer")
    
    With oTransfer
        .CopyAllObjects = True
        .IncludeDependencies = True
        .IncludeLogins = gblnScriptLogins
        .IncludeUsers = gblnScriptUsersAndRoles
        .CopyAllTriggers = gblnScriptTriggers
        .ScriptType = _
            SQLDMOScript_PrimaryObject Or _
            SQLDMOScript_Bindings Or _
            SQLDMOScript_ToFileOnly Or _
            SQLDMOScript_IncludeHeaders Or _
            SQLDMOScript_Aliases Or _
            SQLDMOScript_OwnerQualify
        
        If gblnScriptPermissions Then
            .ScriptType = .ScriptType Or SQLDMOScript_Permissions Or SQLDMOScript_DatabasePermissions
        End If
        
        If gblnScriptTriggers Then
            .ScriptType = .ScriptType Or SQLDMOScript_Triggers
        End If
        
        If gblnScriptDRI Then
            .ScriptType = .ScriptType Or SQLDMOScript_DRI_PrimaryKey Or SQLDMOScript_DRI_ForeignKeys Or SQLDMOScript_DRI_Defaults Or SQLDMOScript_DRI_Checks
        End If
       
       .Script2Type = SQLDMOScript2_NoFG
        
        If gblnUnicodeAppDbFiles Then
            .Script2Type = .Script2Type Or SQLDMOScript2_UnicodeFile
        Else
            .Script2Type = .Script2Type Or SQLDMOScript2_AnsiFile
        End If
    End With
    
    ' Create script
    strScriptFileSpec = strPath & "\" & oAppDb.strAppDbName & "_DDL_Script.SQL"
    oDatabase.ScriptTransfer oTransfer, SQLDMOXfrFile_SingleFile, strScriptFileSpec
    
    strMessage = strMessage & _
        "DDL script '" & strScriptFileSpec & "' generated for the '" & _
        oAppDb.strAppDbName & "' database; "
    
    ' Create table data files
    If lngTableListCount > 0 Then
        For Each strTableName In strTableList
            ExportTableData oDbServer, oAppDb.strAppDbName, CStr(strTableName), strPath, blnError, strMessage
            If blnError Then
                Exit Sub
            End If
        Next
    End If
    
    ' Create table load list
    CreateLoadList strPath, oAppDb.strAppDbName, gblnUnicodeAppDbFiles, blnError, strMessage
    
    strMessage = strMessage & _
        "The script-based application database installation image was created successfully; "
    
    '
    ' Step 2: Create file-based application database installation image
    '
    
    ' Check to make sure database has only one database file
    If oDatabase.FileGroups.Count > 1 Then
        strMessage = strMessage & _
            "The '" & oAppDb.strAppDbName & "' database has more than one file group; "
        blnError = True
        Exit Sub
    End If
    
    Set oFileGroup = oDatabase.FileGroups("PRIMARY")
    
    If oFileGroup.DbFiles.Count > 1 Then
        strMessage = strMessage & _
            "The '" & oAppDb.strAppDbName & "' database has more than one file group; "
        blnError = True
        Exit Sub
    End If
    
    ' Get filespec for database file
    Set oDbFile = oFileGroup.DbFiles(1)
    strFileSrcSpec = Trim(oDbFile.PhysicalName)
    strFileDestSpec = strPath & "\" & oAppDb.strAppDbName & "_Data.MDF"
    
    Set oFileGroup = Nothing
    Set oDbFile = Nothing
    Set oDatabase = Nothing
    
    Beep
    MsgBox "Please manually detach and reattach the database.", vbExclamation
    
    ' Detach database
    strMessage = strMessage & oDbServer.DetachDb(oAppDb.strAppDbName, True)
    
    Beep
    MsgBox "Check to make sure the db is detached.", vbExclamation
    
    blnDbFound = False
    LocateDb oDbServer, oAppDb.strAppDbName, blnDbFound, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    If blnDbFound Then
        blnError = True
        strMessage = strMessage & _
            "The '" & oAppDb.strAppDbName & "' could not be detached; " & _
            "The database may be in use by another process; "
        Exit Sub
    End If
    
    ' Copy data file to image path
    CopyFile strFileSrcSpec, strFileDestSpec, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    strMessage = strMessage & _
        "Successfully copied application database data file to '" & strFileDestSpec & "'; "
    
    ' Re-Attach Database
    
    strMessage = strMessage & _
        oDbServer.AttachDbWithSingleFile(oAppDb.strAppDbName, strFileSrcSpec)
    
    blnDbFound = False
    LocateDb oDbServer, oAppDb.strAppDbName, blnDbFound, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    If blnDbFound Then
        strMessage = strMessage & _
            "Successfully re-attached the '" & oAppDb.strAppDbName & "' database; "
    Else
        blnError = True
        strMessage = strMessage & _
            "The wizard was unable to re-attach '" & oAppDb.strAppDbName & "' database; "
        Exit Sub
    End If
    
    '
    ' Step 3: Create AppDbImage.ini file
    '
    
    strFileDestSpec = strPath & "\" & gstrAppDbImageIniFileName
    
    CreateAppDbImageIni strFileDestSpec, oAppDb, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    strMessage = strMessage & _
        "The installation image settings file '" & strFileDestSpec & "' was created; "
    
    Exit Sub

DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub LoadTableNames( _
    oDbServer As Object, _
    strDbName As String, _
    strTableNames() As String, _
    lngTableCount As Long, _
    blnError As Boolean, _
    strMessage As String)
    
    ' Loads array with table names for user tables with rows in specified database
    Dim oDatabase As Object
    Dim oTable As Object
    
    blnError = False
    lngTableCount = 0
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    
    For Each oTable In oDatabase.Tables
        If Not oTable.SystemObject And (oTable.Rows > 0) Then
            lngTableCount = lngTableCount + 1
            ReDim Preserve strTableNames(lngTableCount)
            strTableNames(lngTableCount) = oTable.Name
        End If
    Next
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub ExportTableData( _
    oDbServer As Object, _
    strAppDbName As String, _
    strTableName As String, _
    strPath As String, _
    blnError As Boolean, _
    strMessage As String)
    
    Dim oDatabase As Object
    Dim oTable As Object
    Dim oBcp As Object
    Dim strDataFileSpec As String
    Dim strErrorFileSpec As String
    Dim lngNumRows As Long
    
    blnError = False
    
    strDataFileSpec = strPath & "\" & strTableName & ".BCP"
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strAppDbName)
    Set oTable = oDatabase.Tables(strTableName)
    
    Set oBcp = CreateObject("SQLDMO.BulkCopy")
    With oBcp
        .DataFileType = SQLDMODataFile_NativeFormat
        .DataFilePath = strDataFileSpec
        .MaximumErrorsBeforeAbort = 0&
    End With

    lngNumRows = oTable.ExportData(oBcp)
    
    strMessage = strMessage & _
        CStr(lngNumRows) & " rows were exported to the file named '" & strDataFileSpec & "' from the table named '" & strTableName & "'; "
        
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
Public Sub GetDbVersion(oDbServer As Object, oAppDb As AppDb, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oSp As Object
    Dim oResults As Object
    
    Dim blnFound As Boolean
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(oAppDb.strAppDbName)
    
    blnFound = False
    
    For Each oSp In oDatabase.StoredProcedures
        If oSp.Name = oAppDb.strAppDbVerSp Then
            blnFound = True
            Exit For
        End If
    Next
    
    If Not blnFound Then
        strMessage = strMessage & _
            "Stored procedure '" & oAppDb.strAppDbVerSp & "' not located; "
        Exit Sub
    End If
    
    Set oResults = oDatabase.ExecuteWithResults(oAppDb.strAppDbVerSp)
    
    With oAppDb
        .strAppDbVer = oResults.GetColumnString(1, 1)
        .strAppDbOrg = oResults.GetColumnString(1, 2)
        .dtAppDbDate = oResults.GetColumnDate(1, 3)
        .strAppDbDesc = oResults.GetColumnString(1, 4)
    End With
        
    On Error GoTo 0
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

Public Sub SaveSettingsToDb(oDbServer As Object, oAppDb As AppDb, blnError As Boolean, strMessage As String)
    Dim oSp As Object
    Dim oDb As Object
    Dim blnFound As Boolean
    
    blnError = False
    On Error GoTo DMOError
    
    '
    ' Step 1: Find version sp, if located, drop it
    '
    
    blnFound = False
    LocateSp oDbServer, oAppDb.strAppDbName, oAppDb.strAppDbVerSp, blnFound, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    If blnFound Then
        RemoveSp oDbServer, oAppDb.strAppDbName, oAppDb.strAppDbVerSp, blnError, strMessage
        
        If blnError Then
            Exit Sub
        End If
    End If

    '
    ' Step 2: Create version sp
    '
    
    Set oSp = CreateObject("SQLDMO.StoredProcedure")
    
    With oAppDb
        oSp.Name = .strAppDbVerSp
        oSp.Text = _
            "CREATE PROCEDURE " & .strAppDbVerSp & " AS " & vbCrLf & _
            "  DECLARE @AppDbVer AS SMALLDATETIME " & vbCrLf & _
            "  SELECT @AppDbVer = '" & Format(.dtAppDbDate, "Short Date") & "' " & vbCrLf & _
            "  SELECT " & vbCrLf & _
            "    AppDbVer = '" & .strAppDbVer & "', " & vbCrLf & _
            "    AppDbOrg = '" & .strAppDbOrg & "', " & vbCrLf & _
            "    AppDbDate = @AppDbVer, " & vbCrLf & _
            "    AppDbDesc = '" & .strAppDbDesc & "' " & vbCrLf
    End With
    
    Set oDb = oDbServer.Databases(oAppDb.strAppDbName)
    
    oDb.StoredProcedures.Add oSp
    
    strMessage = strMessage & _
        "Created the '" & oAppDb.strAppDbName & ".." & oAppDb.strAppDbVerSp & "' stored procedure; "
    
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub LocateSp(oDbServer As Object, strDbName As String, strSpName As String, blnFound As Boolean, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oSp As Object
    
    blnError = False
    blnFound = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    
    For Each oSp In oDatabase.StoredProcedures
        If oSp.Name = strSpName Then
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

Public Sub RemoveSp(oDbServer As Object, strDbName As String, strSpName As String, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oSp As Object
    
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strDbName)
    Set oSp = oDatabase.StoredProcedures(strSpName)
    oSp.Remove
    
    strMessage = strMessage & _
        "Removed the '" & strDbName & ".." & strSpName & "' stored procedure; "
        
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CheckTableDependencies(oDbServer As Object, strAppDbName As String, strTables() As String, lngTableCount As Long, blnError As Boolean, strMessage As String)
    Dim oDatabase As Object
    Dim oTable As Object
    Dim oResults As Object
    Dim i As Long
    Dim x As Long
    Dim y As Long
    Dim lngType As SQLDMO_OBJECT_TYPE
    Dim strObjName As String
    Dim blnFound As Boolean
    
    blnError = False
    
    On Error GoTo DMOError
    
    Set oDatabase = oDbServer.Databases(strAppDbName)
    
    For i = 1 To lngTableCount
        Set oTable = oDatabase.Tables(strTables(i))
        Set oResults = oTable.EnumDependencies(SQLDMODep_Parents)
        
        With oResults
            .CurrentResultSet = 1
            If .Rows > 1 Then
                For x = 1 To .Rows
                    lngType = .GetColumnLong(x, 1)
                    strObjName = .GetColumnString(x, 2)
                    
                    If lngType = SQLDMOObj_UserTable Then
                        blnFound = False
                        
                        If i > 1 Then
                            For y = 1 To (i - 1)
                                If strTables(y) = strObjName Then
                                    blnFound = True
                                    Exit For
                                End If
                            Next y
                        End If
                        
                        If Not blnFound Then
                            blnError = True
                            strMessage = strMessage & _
                                "The '" & oTable.Name & "' table depends on the '" & strObjName & "' table; " & _
                                "Re-order the tables in the list so that the '" & strObjName & "' table is loaded first. "
                            Exit Sub
                        End If
                    End If
                Next x
            End If
        End With
    Next i
        
    Exit Sub
    
DMOError:
    blnError = True
    strMessage = strMessage & DMOError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

