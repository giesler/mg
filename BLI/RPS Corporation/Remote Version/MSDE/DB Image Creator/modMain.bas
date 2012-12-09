Attribute VB_Name = "modMain"
'''''''''''''''''''''''''''''''''''''''''''''''''''''''
' DbInstallImage Wizard Application version 1.0       '
' L. Roger Doherty                                    '
' Microsoft Corporation                               '
' October 1999                                        '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This source code is freely distributable and may be '
' used in whole or part without restriction.          '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT
' WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED,
' INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
' OF MERCHANTABILITY AND/OR FITNESS FOR A  PARTICULAR
' PURPOSE.
'
' Copyright (C) 1999 Microsoft Corporation, All rights reserved
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' This application is used to create an application database installation
' image for an arbitrary user-specified database.  This image contains both
' a file-based and script-based installation image, as well as a SqlAppSetupWiz.ini
' file which can be used to perform an unattended installation of the application
' database on a workstation or server.

Option Explicit

' Operating system type constants
Public Enum OsType
    OS_NOT_INITIALIZED = 0& ' Indicates that a variable has not yet been initialized yet.
    WIN_95_OLD              ' Any Win95 or Win98 release that doesn't have DCOM95 enabled
    WIN_95_98               ' Win95 or Win98 with DCOM95 enabled
    NT_OLD                  ' Any Winnt version that isn't 4.0 SP4 or greater
    NT_40_WKS               ' NT 4.0 Workstation
    NT_40_SRV               ' NT 4.0 Server
    NT_40_SRV_ENT           ' NT 4.0 Enterprise Edition
    NT_40_SRV_TRM           ' NT 4.0 Terminal Server Edition
    WIN_2K_PRO              ' Windows 2000 Profesional
    WIN_2K_SRV              ' Windows 2000 Server
    WIN_2K_SRV_ADV          ' Windows 2000 Advanced Server
    WIN_2K_SRV_DC           ' Windows 2000 Data Center Server
End Enum

' Database server type constants used to identify particular configurations in DBServers collection

Public Enum SQLDMO_PACKAGE_TYPE
    SQLDMO_Unknown = 0&
    SQLDMO_DESKTOP
    SQLDMO_STANDARD
    SQLDMO_ENTERPRISE
    SQLDMO_MSDE
End Enum

Public Enum DbServerType
    SQL_UNKNOWN = SQLDMO_Unknown        ' Unknown SQL Server release.   Not a valid package type.
    SQL_DESK_7_X86 = SQLDMO_DESKTOP     ' SQL Server 7.0 Desktop Edition. Valid package type.
    SQL_STD_7_X86 = SQLDMO_STANDARD     ' SQL Server 7.0 Standard Edition. Valid package type.
    SQL_ENT_7_X86 = SQLDMO_ENTERPRISE   ' SQL Server 7.0 Enterprise Edition. Valid package type.
    MSDE_1_X86 = SQLDMO_MSDE            ' MSDE 1.0. Valid package type.
    MSDE_OFFICE_1_X86 = 5&              ' MSDE 1.0 (Office Variant). Not a valid package type.
    SQL_DEV_7_X86 = 6&                  ' SQL Server 7.0 Developers Edition.  Not a valid package type.
    SQL_SBS_7_X86 = 7&                  ' SQL Server 7.0 Small Business Server Edition.  Not a valid package type.
    SQL_7_SP1_NT_X86 = 8&               ' SQL Server 7.0 SP1 for Windows NT and Windows 2000
    SQL_7_SP1_9X_X86 = 9&               ' SQL Server 7.0 SP1 for Windows 95 and Windows 98
    SQL_PRE_7 = 10&                     ' SQL Server 4.21, 6.0 or 6.5 Release. Not a valid package type.
    SQL_7_BETA = 11&                    ' Beta build of SQL Server 7.0. Not a valid package type.
    SQL_POST_7 = 12&                    ' Post SQL Server 7.0 Release. Not a valid package type.
    SQL_NOT_INITIALIZED = 13&           ' Indicates that a variable has not been initialized yet.  Not a valid package type.
End Enum

' Miscellaneous constants
Public Const NOT_INITIALIZED = 0
Public Const STR_NOT_INITIALIZED = "NOTINITIALIZED"

' Public Constants for Windows Scripting Host
Public Enum FolderSpec
    WINDOWSFOLDER = 0&
    SYSTEMFOLDER
    TEMPORARYFOLDER
End Enum

Public Enum DriveType
    Unknown = 0&
    Removable
    Fixed
    Remote
    CDRom
    RamDisk
End Enum

Public Enum FileAttributes
    Normal = 0&
    ReadOnly = 1&
    Hidden = 2&
    System = 4&
    VOLUME = 8&
    DIRECTORY = 16&
    Archive = 32&
    Alias = 64&
    COMPRESSED = 128&
End Enum

Public Enum IOMODE
    FORREADING = 1&
    FORAPPENDING = 8&
End Enum

Public Enum FILE_FORMAT
    TRISTATEUSEDEFAULT = -2&
    TristateTrue = -1&
    TRISTATEFALSE = 0&
End Enum

' Application database types and contants
Public Type AppDb
    lngAppDbSortOrderId As Long ' Sort order required for compatibility with file based application database installation image
    lngAppDbUnicodeLocaleId As Long ' Unicode locale id required for compatibility with file based application database installation image
    lngAppDbUnicodeCompStyle As Long ' Unicode comparison style required for compatibility with file based application database installation image
    lngAppDbSqlMajorVer As Long ' Database server major version number required for compatiblity with application database
    lngAppDbSqlMinorVer As Long ' Database server minor version number required for compatibility with application database
    lngAppDbSqlBuildNum As Long ' Database server build number required for compatibility with applicaiton database
    lngAppDbCsdVer As Long ' Database server service pack level required for compatibility with application database
    lngAppDbBytesReq As Long ' Data storage requirements in bytes for application database
    strAppDbName As String ' Default database name for application database, can be changed at install time
    strAppDbVer As String ' Version number of application database
    strAppDbVerSp As String ' Name of stored procedure used to retrieve the version number of the application database
    strAppDbOrg As String ' Company or individual who created application database
    dtAppDbDate As Date ' Release date of application database
    strAppDbDesc As String ' Description of application database
End Type

' Application Settings Contants
Public Const INI_FILE = "DbInstallImage.ini"                ' INI file with settings
Public Const APP_SETTINGS_SECTION = "Application Settings"  ' INI_FILE section containing generic application settings
Public Const APPDB_SECTION = "AppDb Settings"               ' INI_FILE section containing settings for the application database to be installed

' Global Object Variables
Public goFileSystem As Object               ' Used to access the local file system
Public goWScriptShell As Object             ' Used to access the local registry
Public goDbServer As Object                 ' Used to access the local database server
Public goLogFileHandle As Object            ' Handle to log file
Public goAppDb As AppDb                     ' Used to describe the application database

' Application Settings Global Variables
Public gstrTempDir As String                ' Points to local temp directory
Public gstrLogFile As String                ' Name of DbInstallImage Log File
Public gstrImageFolderName As String        ' Name of the folder for storing installation images
Public gstrAppDbImageIniFileName As String  ' Name of Application Database Image Settings ini file
Public gblnUnicodeAppDbFiles As Boolean     ' Indicates whether DbInstallImage will generate ANSI or Unicode DDL, load list and BCP files

' Global Session Variables
Public glngOsType As OsType                 ' Indicates the operating system detected
Public glngSqlType As DbServerType          ' Indicates the type of database server detected or installed
Public gblnSqlPreInstalled As Boolean       ' Indicates whether SQL Server is pre-installed
Public gblnDmoConnected As Boolean          ' Indicates whether goDbServer object is already connect to local db server as a member of serveradmin role
Public gstrSqlSaPwd As String               ' Used to store the sa password for connecting to SQL Server on Windows 95 / 98
Public gstrImageFolder As String            ' Indicates the directory for the installation image
Public gblnScriptUsersAndRoles As Boolean   ' Specifies whether users and roles should be included in DDL script
Public gblnScriptLogins As Boolean          ' Specifies whether logins should be included in DDL script
Public gblnScriptPermissions As Boolean     ' Specifies whether permissions should be included in DDL script
Public gblnScriptIndexes As Boolean         ' Specifies whether indexes and roles should be included in DDL script
Public gblnScriptTriggers As Boolean        ' Specifies whether triggers and roles should be included in DDL script
Public gblnScriptDRI As Boolean             ' Specifies whether DRI should be included in DDL script
Public gstrLoadTables() As String           ' Lists the tables that the wizard will create a bcp data file for
Public glngLoadTableCount As Long           ' Count of items in gstrLoadTables
Public gblnAppDbPropsConfigured As Boolean  ' Indicuates whether application database properties have been configured or not

' Global Variables and contants specific to use of dlgGetPath
Public gstrCurDrive As String
Public gstrCurPath As String
Public gblnPathChanged As Boolean ' Flag which indicates whether the current path changed in dlgGetFile


Public Sub Main()
    Dim blnError As Boolean
    Dim strMessage As String
    
    If App.PrevInstance Then
        MsgBox "An instance of DbInstallImage is already running.", vbCritical + vbOKOnly, "Error"
        blnError = True
        Exit Sub
    End If
    
    blnError = False
    
    InitVariables blnError
    
    If blnError Then
        Exit Sub
    End If
    
    OpenLogFile blnError
    
    If blnError Then
        Exit Sub
    End If
    
    WriteLogMsg goLogFileHandle, "DbInstallImage Version " & Str(App.Major)
    WriteLogMsg goLogFileHandle, "Session Started."
    
    ' Detect Operating System Information
    strMessage = ""
    CheckOs blnError, strMessage
    
    WriteLogMsg goLogFileHandle, strMessage
    
    If blnError Then
        ShowErrorDialog "Unable to detect operating system configuration.", 0, ""
        Exit Sub
    End If
    
    Select Case glngOsType
        Case WIN_95_98, NT_40_WKS, NT_40_SRV, NT_40_SRV_ENT, NT_40_SRV_TRM, WIN_2K_PRO, WIN_2K_SRV, WIN_2K_SRV_ADV, WIN_2K_SRV_DC
            ' Supported operating system, continue
        
        Case Else
            ShowErrorDialog "You are running an unsupported operating system.", 0, ""
            WriteLogMsg goLogFileHandle, "Unsupported OS.  Terminating."
            Exit Sub
    End Select
    
    ' Detect SQL Server Information
    strMessage = ""
    CheckSql glngSqlType, goAppDb, blnError, strMessage
    
    WriteLogMsg goLogFileHandle, strMessage
    
    If blnError Then
        ShowErrorDialog "You are running an unsupported database server.", 0, ""
        WriteLogMsg goLogFileHandle, "Unsupported database server. Terminating."
        Exit Sub
    End If
        
    ' Start Wizard
    dlgWiz01.Show vbModal
    
    ' All Forms Unloaded, Time to go...
    WriteLogMsg goLogFileHandle, "Session Ending."
    CleanUp
End Sub

Public Sub InitVariables(blnError As Boolean)
    Dim oFolder As Object
    Dim strMessage As String
    
    blnError = False
        
    On Error Resume Next
    
    Set goFileSystem = CreateObject("Scripting.FileSystemObject")
    
    If Err.Number <> 0 Then
        blnError = True
        ShowErrorDialog "Unable to initialize the Windows Scripting Host.  Check to make sure it is installed.", Err.Number, Err.Description
        Exit Sub
    End If
    
    Set goWScriptShell = CreateObject("WScript.Shell")
    
    If Err.Number <> 0 Then
        blnError = True
        ShowErrorDialog "Unable to initialize the Windows Scripting Host.  Check to make sure it is installed.", Err.Number, Err.Description
        Exit Sub
    End If
    
    Set goDbServer = CreateObject("SQLDMO.SQLServer")
    
    If Err.Number <> 0 Then
        blnError = True
        ShowErrorDialog "Unable to initialize SQL-DMO.  Check to make sure a local database server is installed.", Err.Number, Err.Description
        Exit Sub
    End If
    
    On Error GoTo 0
    
    With goAppDb
        .lngAppDbSortOrderId = 0
        .lngAppDbUnicodeLocaleId = 0
        .lngAppDbUnicodeCompStyle = 0
        .lngAppDbCsdVer = 0
        .lngAppDbBytesReq = 0
        .strAppDbName = ""
        .strAppDbVer = ""
        .strAppDbVerSp = STR_NOT_INITIALIZED
        .strAppDbOrg = ""
        .dtAppDbDate = Now()
        .strAppDbDesc = ""
    End With
        
    ' Get the Temp Path
    Set oFolder = goFileSystem.GetSpecialFolder(TEMPORARYFOLDER)
    gstrTempDir = oFolder.Path
    
    ' Get Application Settings
    LoadAppSettings blnError
    
    If blnError Then
        ShowErrorDialog "Unable to load application settings from '" & INI_FILE & "'. Setup cannot continue.", 0, ""
        blnError = True
        Exit Sub
    End If
    
    ' Initialize Session Variables
    glngOsType = OS_NOT_INITIALIZED
    glngSqlType = SQL_NOT_INITIALIZED
    gblnDmoConnected = False
    gblnSqlPreInstalled = False
    gstrSqlSaPwd = STR_NOT_INITIALIZED
    gstrImageFolder = STR_NOT_INITIALIZED
    gblnScriptUsersAndRoles = True
    gblnScriptLogins = False
    gblnScriptPermissions = True
    gblnScriptIndexes = True
    gblnScriptTriggers = True
    gblnScriptDRI = True
    glngLoadTableCount = 0
    gblnAppDbPropsConfigured = False
End Sub
Public Sub LoadAppSettings(blnError As Boolean)
    Dim strValueRead As String
    Dim strIniFileSpec As String
    
    strIniFileSpec = App.Path & "\" & INI_FILE
    
    blnError = False
    
    ' Load gstrLogFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "LogFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrLogFile = strValueRead
    
    ' Load gstrImageFolderName
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "ImageFolderName")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrImageFolderName = strValueRead

    ' Load goAppDb.strAppDbVerSp
    strValueRead = ReadIni(App.Path & "\" & INI_FILE, APP_SETTINGS_SECTION, "AppDbVerSp")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    goAppDb.strAppDbVerSp = strValueRead
    
    ' Load gstrAppDbImageIniFileName
    strValueRead = ReadIni(App.Path & "\" & INI_FILE, APP_SETTINGS_SECTION, "AppDbImageIniFileName")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrAppDbImageIniFileName = strValueRead
    
    ' Load gblnUnicodeAppDbFiles
    strValueRead = ReadIni(App.Path & "\" & INI_FILE, APP_SETTINGS_SECTION, "UnicodeAppDbFiles")
    
    Select Case strValueRead
        Case STR_NOT_INITIALIZED
            blnError = True
            Exit Sub
        Case "True"
            gblnUnicodeAppDbFiles = True
        Case "False"
            gblnUnicodeAppDbFiles = False
        Case Else
            blnError = True
            Exit Sub
    End Select
End Sub

Sub OpenLogFile(blnError As Boolean)
    Dim strLogFileLongName As String
    
    blnError = False
    
    strLogFileLongName = gstrTempDir & "\" & gstrLogFile
    
    On Error GoTo WSHError
    
    If goFileSystem.FileExists(strLogFileLongName) Then
        goFileSystem.DeleteFile strLogFileLongName
    End If
    
    Set goLogFileHandle = goFileSystem.CreateTextFile(strLogFileLongName, True, False)
    
    Exit Sub
    
WSHError:
    ShowErrorDialog "Windows Scripting Host Error. The log file cannot be initialized.  Check to make sure that there is space available for the Windows TEMP directory.", Err.Number, Err.Description
    Err.Clear
    blnError = True
    Exit Sub
End Sub

Public Sub ShowErrorDialog(strMessage As String, lngErrNum, strErrDesc)
    MsgBox strMessage & vbCrLf & "Number: " & Str(lngErrNum) & vbCrLf & "Description: " & strErrDesc, vbCritical + vbOKOnly, "Error"
End Sub

Public Sub WriteLogMsg(oLogFileHandle As Object, strMessage As String)
    Dim strOutputLine As String
    
    strOutputLine = FormatDateTime(Now(), vbGeneralDate) & ": " & strMessage
    
    On Error Resume Next
    oLogFileHandle.WriteLine strOutputLine
    
    If Err.Number <> 0 Then
        ShowErrorDialog "Unable to write the following message to the log file:" & vbCrLf & strMessage, Err.Number, Err.Description
        Err.Clear
    End If
    On Error GoTo 0
End Sub

Sub CleanUp()
    goLogFileHandle.Close
        
    Set goLogFileHandle = Nothing
    Set goFileSystem = Nothing
End Sub

Sub Quit(Optional bConfirm As Boolean = False)
    Dim intResponse As Integer
    Dim objForm As Form
    
    If bConfirm Then
        intResponse = MsgBox("Are you sure you want to cancel?", vbYesNo + vbQuestion, "Confirmation")
    
        If intResponse = vbNo Then
            Exit Sub
        End If
    End If
    
    WriteLogMsg goLogFileHandle, "User cancelled wizard."
    
    For Each objForm In Forms
        Unload objForm
    Next
End Sub

Public Sub ConnectUsingDmo(oDbServer As Object, blnError As Boolean, strMessage As String)
    blnError = False
    
    InitializeSqlDmo oDbServer, blnError, strMessage
    
    If Not blnError Then
        StartDbServer oDbServer, blnError, strMessage
    End If
    
    If Not blnError Then
        CheckSysAdminRole oDbServer, blnError, strMessage
    End If
End Sub
Public Function ReadRegValue(strValueName As String) As Variant
    On Error Resume Next
    
    ReadRegValue = Empty
    ReadRegValue = goWScriptShell.RegRead(strValueName)
            
    If Err.Number > 0 Then
        Err.Clear
    End If
End Function

Sub CheckOs(blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim strOs As String
    
    blnError = False

    ' Check for Windows NT
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\Windows NT\CurrentVersion\CurrentVersion")
    
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
        strOs = CStr(varRegValue)
        strMessage = strMessage & _
            "Windows NT Version Detected: " & strOs & "; "
    
        Select Case strOs
            Case "4.0"
                CheckNt40Version blnError, strMessage
            
            Case "5.0"
                CheckWin2kVersion blnError, strMessage
                
            Case "3.51", "3.5" Or "3.1"
                glngOsType = NT_OLD
                
            Case Else
                glngOsType = NOT_INITIALIZED
        End Select
    Else
        ' Else check for Windows 95/98
        CheckWin9598Version blnError, strMessage
        
        If blnError Then
            strMessage = strMessage & _
                "The Windows Scripting Host cannot read the sytem registry; "
        End If
    End If
End Sub

Public Sub CheckNt40Version(blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim lngCsdLevel As Long
    Dim strProductType As String
    Dim strProductSuite As String
    
    ' Check Service Pack Level
    varRegValue = ReadRegValue("HKLM\System\CurrentControlSet\Control\Windows\CSDVersion")
    
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbLong Then
        lngCsdLevel = CLng(Hex(varRegValue))
        
        strMessage = strMessage & _
            "Service Pack Level Detected: " & LTrim(Str(lngCsdLevel)) & "; "
            
        If lngCsdLevel >= 400 Then
            ' Check the product type
            varRegValue = ReadRegValue("HKLM\System\CurrentControlSet\Control\ProductOptions\ProductType")
            If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
                strProductType = CStr(varRegValue)
                strMessage = strMessage & _
                    "Windows NT ProductType Detected: " & strProductType & "; "
                Select Case strProductType
                    Case "WinNT"
                        glngOsType = NT_40_WKS
                        Exit Sub
                    Case "LanmanNT", "ServerNT"
                        glngOsType = NT_40_SRV
                        
                        ' Check for Product Suite
                        varRegValue = ReadRegValue("HKLM\System\CurrentControlSet\Control\ProductOptions\ProductSuite")
                        If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
                            strProductSuite = CStr(varRegValue)
                            strMessage = strMessage & _
                                "ProductSuite Detected: " & strProductSuite & "; "
                            Select Case strProductSuite
                                Case "Enterprise"
                                    glngOsType = NT_40_SRV_ENT
                                    Exit Sub
                                Case "Terminal Server"
                                    glngOsType = NT_40_SRV_TRM
                                    Exit Sub
                            End Select
                        Else
                            ' Leave glngOsType set to NT_40_SRV since product suite wasn't detected
                            strMessage = strMessage & _
                                "Setup was unable to detect a ProductSuite; "
                            Exit Sub
                        End If
                End Select
            Else
                glngOsType = NOT_INITIALIZED
                strMessage = strMessage & _
                    "Setup was unable to detect ProductType; "
                Exit Sub
            End If
        Else
            glngOsType = NT_OLD
            Exit Sub
        End If
    Else
        glngOsType = NOT_INITIALIZED
        blnError = True
        strMessage = strMessage & _
            "Unable to detect Windows NT service pack level; "
        Exit Sub
    End If
End Sub

Public Sub CheckWin9598Version(blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim strOs As String
    Dim strRegValue As String
    
    blnError = False
    
    ' Check for Windows 95 or Windows 98
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\Windows\CurrentVersion\Version")
        
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
        strOs = CStr(varRegValue)
        strMessage = strMessage & _
            "Windows Version Detected: " & strOs & "; "
    
        If (strOs = "Windows 95") Or (strOs = "Windows 98") Then
            ' Check for DCOM95
            varRegValue = ReadRegValue("HKLM\Software\Microsoft\OLE\EnableDCOM")
            
            If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
                strRegValue = CStr(varRegValue)
                
                If strRegValue = "Y" Then
                    glngOsType = WIN_95_98
                
                    strMessage = strMessage & _
                        "DCOM95 is enabled; "
                    Exit Sub
                Else
                    glngOsType = WIN_95_OLD
                    strMessage = strMessage & _
                        "DCOM95 is not enabled; "
                    Exit Sub
                End If
            Else
                glngOsType = WIN_95_OLD
                strMessage = strMessage & _
                    "DCOM95 is not enabled; "
                Exit Sub
            End If
        Else
            glngOsType = NOT_INITIALIZED
            blnError = True
            strMessage = strMessage & _
                "Setup could not determine what version of Windows is installed; "
            Exit Sub
        End If
    Else
        glngOsType = NOT_INITIALIZED
        blnError = True
        strMessage = strMessage & _
            "Setup could not determine what version of Windows is installed; "
        Exit Sub
    End If
End Sub



Sub CrackSqlBuild(strSqlVer As String, lngSqlMajorVer As Long, lngSqlMinorVer As Long, lngSqlBuildNum As Long)
    Dim strTemp As String
    Dim lngTemp As Long
    
    lngSqlMajorVer = -1
    lngSqlMinorVer = -1
    lngSqlBuildNum = -1
    
    ' Crack Major Version
    If IsNumeric(CLng(Left(strSqlVer, InStr(1, strSqlVer, ".") - 1))) Then
        lngTemp = CLng(Left(strSqlVer, InStr(1, strSqlVer, ".") - 1))
        If lngTemp >= 0 Then
            lngSqlMajorVer = lngTemp
        End If
    End If
    
    ' Crack Minor Version
    strTemp = Mid(strSqlVer, InStr(1, strSqlVer, ".") + 1, Len(strSqlVer))
    
    If Len(strTemp) > 0 And IsNumeric(CLng(Left(strTemp, InStr(1, strTemp, ".") - 1))) Then
        lngTemp = CLng(Left(strTemp, InStr(1, strTemp, ".") - 1))
        If lngTemp >= 0 Then
            lngSqlMinorVer = lngTemp
        End If
    End If

    ' Crack Build Number
    If IsNumeric(CLng(Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp)))) Then
        lngTemp = CLng(Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp)))
        If lngTemp >= 0 Then
            lngSqlBuildNum = CLng(Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp)))
        End If
    End If
End Sub

Sub CheckSql(lngSqlType As DbServerType, oAppDb As AppDb, blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim strSqlVer As String
    Dim lngSqlMajorVer As Long
    Dim lngSqlMinorVer As Long
    Dim lngSqlBuildNum As Long
    Dim lngSqlCsdVersion As Long
    
    blnError = False
    
    ' Look for SQL Server 4.21 installation
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\SQLServer\Server\CurrentVersion\CurrentVersion")
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
        strSqlVer = CStr(varRegValue)
        strMessage = strMessage & _
            "SQL Server Build Detected: " & strSqlVer & "; "
        
        Select Case strSqlVer
            Case "NT 4.2"
                lngSqlType = SQL_PRE_7
                Exit Sub
            Case Else
                lngSqlType = SQL_UNKNOWN
                Exit Sub
        End Select
    End If
    
    ' Look for SQL Server 6.0, 6.5, 7.0 or MSDE 1.0 installation
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\MSSQLServer\CurrentVersion\CurrentVersion")
    
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
        strSqlVer = CStr(varRegValue)
    
        CrackSqlBuild strSqlVer, lngSqlMajorVer, lngSqlMinorVer, lngSqlBuildNum
        
        With oAppDb
            .lngAppDbSqlMajorVer = lngSqlMajorVer
            .lngAppDbSqlMinorVer = lngSqlMinorVer
            .lngAppDbSqlBuildNum = lngSqlBuildNum
        End With
        
        If (lngSqlMajorVer <> -1) And (lngSqlMinorVer <> -1) And (lngSqlBuildNum <> -1) Then
            strMessage = strMessage & _
                "SQL Server or MSDE Build Detected: " & LTrim(Str(lngSqlMajorVer)) & "." & LTrim(Str(lngSqlMinorVer)) & "." & LTrim(Str(lngSqlBuildNum)) & "; "
            gblnSqlPreInstalled = True
            
            Select Case lngSqlMajorVer
                Case 6
                    lngSqlType = SQL_PRE_7
                    Exit Sub
                Case 7
                    Select Case lngSqlMinorVer
                        Case 0
                            If (lngSqlBuildNum >= 623) Then
                                ' Only check database server type for RTM 7.0 builds, this may change later.
                                ' Didn't check explicitly for 623 due to invalid build number of 677 reported
                                ' for some RTM MSDE installations.
                                
                                blnError = False
                                                            
                                ' Check for SQL Server Service Pack Installation
                                varRegValue = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\MSSQLServer\CurrentVersion\CSDVersionNumber")
                            
                                If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbLong Then
                                    oAppDb.lngAppDbCsdVer = CLng(Hex(varRegValue))
                                    strMessage = strMessage & _
                                        "SQL Server Service Pack Version Detected is '" & CStr(oAppDb.lngAppDbCsdVer) & "'; "
                                End If
                            Else
                                lngSqlType = SQL_7_BETA
                                strMessage = strMessage & _
                                    "A pre-release version of SQL Server was detected; "
                                Exit Sub
                            End If
                        Case Is > 0
                            lngSqlType = SQL_POST_7
                            Exit Sub
                    End Select
                    
                Case Is > 7
                    lngSqlType = SQL_POST_7
                    strMessage = strMessage & _
                        "The installed version of SQL Server is a post-SQL Server 7.0 release; "
                    Exit Sub
                
                Case Else
                    blnError = True
                    lngSqlType = SQL_UNKNOWN
                    strMessage = strMessage & _
                        "Setup was unable to determine what build of SQL Server is currently installed; "
                    Exit Sub
            End Select
            
        Else
            blnError = True
            lngSqlType = SQL_UNKNOWN
            strMessage = strMessage & _
                "Setup was unable to determine what build of SQL Server is currently installed; "
        End If
    Else
        lngSqlType = SQL_NOT_INITIALIZED
    End If
End Sub

Public Sub VerifyFolder(strPath As String, lngBytesReq As Long, blnError As Boolean, strMessage As String)
    Dim oDrive As Object
    blnError = False
    
    On Error GoTo WSHError
    
    If Not goFileSystem.FolderExists(strPath) Then
        strMessage = strMessage & _
            "The folder named '" & strPath & "' does not exist; "
        
        CreatePath strPath, blnError, strMessage
        
        If Not blnError Then
            strMessage = strMessage & _
                "Created folder named '" & strPath & "'; "
        Else
            Exit Sub
        End If
    End If
    
    If lngBytesReq > 0 Then
        Set oDrive = goFileSystem.GetDrive(CrackDrive(strPath))
        
        With oDrive
            strMessage = strMessage & _
                "Drive '" & CrackDrive(strPath) & "' has '" & .AvailableSpace & "' bytes available; " & _
                "'" & lngBytesReq & "' bytes are required; "
                
            If .AvailableSpace < lngBytesReq Then
                blnError = True
                strMessage = strMessage & _
                    "Insufficient space on drive '" & CrackDrive(strPath) & "'; "
            End If
        End With
    End If
    
    Exit Sub
WSHError:
    blnError = True
    strMessage = strMessage & WSHError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Private Function WSHError(lngErr As Long, strDesc As String) As String
    WSHError = _
        "The following WSH runtime error was encoutered: " & _
        CStr(Err.Number) & "-" & CStr(Err.Description) & "; "
End Function

Function CrackDrive(strPath As String) As String
    If Len(strPath) > 2 Then
        CrackDrive = Left(strPath, 2)
    Else
        CrackDrive = ""
    End If
    
    ' Test for valid drive
    If CrackDrive <> "" Then
        If Not goFileSystem.DriveExists(CrackDrive) Then
            CrackDrive = ""
        End If
    End If
End Function

Sub CreatePath(strFolder As String, blnError As Boolean, strMessage As String)
    Dim strLeftDir As String
    Dim j As Integer
    Dim slashcount As Integer
    
    blnError = False
    
    On Error GoTo WSHError
    
    slashcount = 0
    For j = 1 To Len(strFolder)
        If Mid(strFolder, j, 1) = "\" Then
            slashcount = slashcount + 1
            strLeftDir = Left(strFolder, j)
                    
            'Create a folder if it doesn't exist
            If (Not goFileSystem.FolderExists(strLeftDir)) Then
                goFileSystem.CreateFolder (strLeftDir)
            End If
        End If
    Next
    
    'Create a folder if it doesn't exist
    If (Not goFileSystem.FolderExists(strFolder)) Then
            goFileSystem.CreateFolder (strFolder)
    End If
    Exit Sub

WSHError:
    blnError = True
    strMessage = strMessage & WSHError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CleanFolder(strPath As String, blnError As Boolean, strMessage As String)
    Dim oFolder As Object
    Dim oFiles As Object
    Dim oFile As Object
    Dim oSubFolders As Object
    Dim oSubFolder As Object
    Dim intResponse As Integer
    
    blnError = False
    On Error GoTo WSHError
    
    Set oFolder = goFileSystem.GetFolder(strPath)
    Set oFiles = oFolder.Files
    
    If oFiles.Count > 0 Then
        intResponse = MsgBox("Preparing to delete " & CStr(oFiles.Count) & " files from the folder '" & strPath & "'.", vbOKCancel + vbInformation, "DbInstallImage")
        If intResponse <> vbOK Then
            blnError = True
            strMessage = strMessage & _
                "User chose not to delete existing files; "
            Exit Sub
        End If
        
        For Each oFile In oFiles
            WriteLogMsg goLogFileHandle, "Deleting file '" & oFile.Path & "'"
            oFile.Delete True
        Next
    End If
    
    Set oSubFolders = oFolder.SubFolders
        
    If oSubFolders.Count > 0 Then
        intResponse = MsgBox("Preparing to delete " & CStr(oSubFolders.Count) & " subfolders from the folder '" & strPath & "'.", vbOKCancel + vbInformation, "DbInstallImage")
        If intResponse <> vbOK Then
            blnError = True
            strMessage = strMessage & _
                "User chose not to delete existing subfolders; "
            Exit Sub
        End If
        
        For Each oSubFolder In oSubFolders
            WriteLogMsg goLogFileHandle, "Deleting folder '" & oSubFolder.Path & "'"
            oSubFolder.Delete True
        Next
    End If
        
    Exit Sub
WSHError:
    blnError = True
    strMessage = strMessage & WSHError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CreateLoadList(strPath As String, strDbName As String, blnUnicode As Boolean, blnError As Boolean, strMessage As String)
    Dim strFileSpec As String
    Dim strTableName As Variant
    Dim oTextFile As Object
    
    blnError = False
    strFileSpec = strPath & "\" & strDbName & "_Table_LoadList.TXT"
    
    On Error GoTo WSHError
        
    Set oTextFile = goFileSystem.CreateTextFile(strFileSpec, True, blnUnicode)
        
    For Each strTableName In gstrLoadTables
        oTextFile.WriteLine CStr(strTableName)
    Next
    
    oTextFile.Close
    
    Set oTextFile = Nothing
    
    strMessage = strMessage & _
        "Load list created in file '" & strFileSpec & "'; "
    Exit Sub

WSHError:
    blnError = True
    strMessage = strMessage & WSHError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub

Public Sub CopyFile(strSourceFileSpec As String, strDestFileSpec As String, blnError As Boolean, strMessage As String)
    blnError = False
    
    On Error GoTo WSHError

    goFileSystem.CopyFile strSourceFileSpec, strDestFileSpec, True
    
    Exit Sub

WSHError:
    blnError = True
    strMessage = strMessage & WSHError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub
Function ClearReadOnly(strFileName As String) As Boolean
    Dim oFile As Object
    
    ' Returns a boolean which indicates if there was an error or not
    
    On Error GoTo ClearReadOnly_End
    
    ' Get file
    Set oFile = goFileSystem.GetFile(strFileName)
    
    If oFile.Attributes And ReadOnly Then
        oFile.Attributes = oFile.Attributes - ReadOnly
    End If
    
    ClearReadOnly = False
    
    On Error GoTo 0
    Exit Function
    
ClearReadOnly_End:
    ClearReadOnly = True
    Exit Function
End Function

Public Sub CreateAppDbImageIni(strFileName As String, oAppDb As AppDb, blnError As Boolean, strMessage As String)
    Dim oTextFile As Object
    Dim strLine As String
    
    blnError = False
    On Error GoTo WSHError
    
    Set oTextFile = goFileSystem.CreateTextFile(strFileName, True, False)
    
    With oAppDb
        strLine = _
            "; " & vbCrLf & _
            "; Configuration file for " & .strAppDbName & " application database installation image " & vbCrLf & _
            "; Created by " & App.EXEName & " version " & CStr(App.Major) & vbCrLf & _
            "; Created on " & Format(Now(), "Short Date") & vbCrLf & _
            "; " & vbCrLf & _
            vbCrLf & _
            "[AppDb Settings]" & vbCrLf
        
        oTextFile.Write strLine
        
        strLine = "AppDbSortOrderId=" & CStr(.lngAppDbSortOrderId)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbUnicodeLocaleId=" & CStr(.lngAppDbUnicodeLocaleId)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbUnicodeCompStyle=" & CStr(.lngAppDbUnicodeCompStyle)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbSqlMajorVer=" & CStr(.lngAppDbSqlMajorVer)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbSqlMinorVer=" & CStr(.lngAppDbSqlMinorVer)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbSqlBuildNum=" & CStr(.lngAppDbSqlBuildNum)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbCsdVer=" & CStr(.lngAppDbCsdVer)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbBytesReq=" & CStr(.lngAppDbBytesReq)
        oTextFile.WriteLine strLine
        
        strLine = "AppDbName=" & .strAppDbName
        oTextFile.WriteLine strLine
        
        strLine = "AppDbVer=" & .strAppDbVer
        oTextFile.WriteLine strLine
        
        strLine = "AppDbVerSp=" & .strAppDbVerSp
        oTextFile.WriteLine strLine
        
        strLine = "AppDbOrg=" & .strAppDbOrg
        oTextFile.WriteLine strLine
        
        strLine = "AppDbDate=" & Format(.dtAppDbDate, "Short Date")
        oTextFile.WriteLine strLine
        
        strLine = "AppDbDesc=" & .strAppDbDesc
        oTextFile.WriteLine strLine
    End With

    Exit Sub
    
WSHError:
    blnError = True
    strMessage = strMessage & WSHError(Err.Number, Err.Description)
    Err.Clear
    Exit Sub
End Sub



