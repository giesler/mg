Attribute VB_Name = "modMain"
Option Explicit
Option Base 1

Public bAutoRun As Boolean

'''''''''''''''''''''''''''''''''''''''''''''''''''''''
' SQL App Setup Wizard Application version 1.1        '
' L. Roger Doherty                                    '
' Microsoft Corporation                               '
' April 2000                                          '
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

' This application can be used to perform an automated installation of any release of
' SQL Server 7.0 or MSDE 1.0, SQL Server 7.0 Service Pack 1, and an arbitrary application
' database.

'
' The Version 1.1 Update handles automated installation of SQL Server 7.0 Service Pack 2
'

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

Public Enum SQLDMO_PACKAGE_TYPE
    SQLDMO_Unknown = 0&
    SQLDMO_DESKTOP
    SQLDMO_STANDARD
    SQLDMO_ENTERPRISE
    SQLDMO_MSDE
End Enum

' Database server type constants used to identify particular configurations in DBServers collection

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
    SQL_7_SP2_NT_X86 = 10&              ' SQL Server 7.0 SP1 for Windows NT and Windows 2000
    SQL_7_SP2_9X_X86 = 11&              ' SQL Server 7.0 SP1 for Windows 95 and Windows 98
    SQL_PRE_7 = 12&                     ' SQL Server 4.21, 6.0 or 6.5 Release. Not a valid package type.
    SQL_7_BETA = 13&                    ' Beta build of SQL Server 7.0. Not a valid package type.
    SQL_POST_7 = 14&                    ' Post SQL Server 7.0 Release. Not a valid package type.
    SQL_NOT_INITIALIZED = 15&           ' Indicates that a variable has not been initialized yet.  Not a valid package type.
End Enum

' Software installation state constants
Public Enum InstallState
    INSTALL_STATE_NOT_INITIALIZED = 0&
    NOT_INSTALLED
    INSTALLED
End Enum

' Application database installation image type constants
Public Enum AppDbImageType
    APPDBIMAGETYPE_NOT_INITIALIZED = 0&
    FILE_IMAGE
    SCRIPT_IMAGE
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

' Global Session Variables

Public goDbApp As AppDb
Public glngOsType As OsType ' Indicates the operating system detected
Public gblnBuiltinAdminsMember As Boolean ' Indicates whether the current user is a member of BUILTIN\Administrators
Public gstrWinDir As String ' Location of Windows directory
Public gstrTempDir As String ' Location of Windows temp directory
Public glngIeInstalled As InstallState ' Indicates whether IE 4.01 SP1 or later is installed
Public glngSqlType As DbServerType ' Indicates the type of database server detected or installed
Public glngSpType As DbServerType ' Indicates the type of service pack to install
Public gblnReboot As Boolean ' Indicates whether a reboot is required after database server and/or service pack instalation
Public glngSqlMajorVer As Long ' Database server major version number detected
Public glngSqlMinorVer As Long ' Database server minor version number detected
Public glngSqlBuildNum As Long ' Database server build number detected
Public glngSqlCsdVer As Long ' Indicates the SQL Server service pack version number detected
Public gblnSqlPreInstalled As Boolean ' Indicates whether SQL Server was pre-installed on the system
Public gstrSqlSaPwd As String ' Used to store the sa password for connecting to SQL Server on Windows 95 / 98
Public gblnDmoConnected ' Indicates whether goDbServer object is already connect to local db server as a member of serveradmin role
Public gstrSetupExeFileSpec As String ' Path and file name of SQL Server or MSDE setup program
Public gstrInstallImagePath As String ' Path of SQL Server, MSDE or service pack installation image
Public gstrProgramPath As String ' Install location for database server program files
Public gstrDataPath As String ' Install location for database server data files
Public gstrAppDbDestFolder As String ' Folder name underneath gstrDataPath for storing application database data files
Public gstrUserName As String ' User name for installation
Public gstrOrg As String ' Organizaton for installation
Public gstrCDKey As String ' CD Key for installation
Public glngDbServerInst As InstallState ' Install status for database server
Public gstrServicePackFileSpec As String ' Path and file name of database server service pack installation executable
Public glngServicePackInst As InstallState ' Install status for database server service pack
Public glngAppDbInstallType As AppDbImageType ' Indicates the type of application database installation image that will be used
Public gstrAppDbName As String ' Name of application database
Public gstrAppDbSourceFolder As String ' Path for installation image files for application database
Public gstrAppDbIniFileSpec As String ' File spec for app db install image ini file
Public glngAppDbInst As InstallState ' Install status for application database
Public gblnJobServerStarted As Boolean ' Service status for job server
Public gstrBackupJobFile As String ' File name of backup job file
Public gstrStartupJobFile As String ' File name of startup job file
Public gstrJobServerStartupSpScript As String ' File name of script file with sp ddl for auto-starting job server on Win95 / Win98 platform
Public gstrJobServerStartupSpName As String ' Name of stored procedure used to start the job server on Win95

' Global Object Variables
Public goFileSystem As Object ' Used to access the local file system
Public goLogFileHandle As Object ' Log file handle
Public goDbServers As DbServers ' Collection of database server configurations
Public goOSSuites As OSSuites ' Collection of Win 2K operating system suite definitions

' Application Settings Contants
Public Const INI_FILE = "SqlAppSetupWiz.ini" ' INI file with settings
Public Const APPDB_SECTION = "AppDb Settings" ' INI_FILE section containing settings for the application database being installed
Public Const APP_SETTINGS_SECTION = "Application Settings" ' INI_FILE section containing generic application settings

' Application Settings Variables
Public gstrLogFile As String ' Name of application log file
Public gstrDbServerFile As String ' Name of database server list file
Public gstrOsSuiteFile As String ' Name of operating system suite list file
Public gstrSqlSetupIniFileSpec As String ' Used to verify SQL Server package type of a distribution image
Public gstrSqlSetupIniSection As String ' INI file section for key which contains package type
Public gstrSqlSetupIniKey As String ' INI file key which contains SQL package type
Public gstrSetupLogFile As String ' InstallShield setup log file
Public gstrSetupIssFile As String ' InstallShield unattended setup file
Public gstrSetupLogStatusSection As String ' Section used to report status information from an InstallShield installation session
Public gstrSetupLogCompletedKey As String ' Key used to determine if an installation was completed
Public gstrSetupLogRebootRequiredKey As String  ' Key in gstrSetupLogStatusSection used to determine whether a reboot is required after an InstallShield installation session
Public gstrSetupLogResponseResultSection As String ' Section used to report exit codes from an installshield installation executable
Public gstrSetupLogResultCodeKey As String ' Key in gstrSetupLogResponseResultSection used to detect failed MSDE installation
Public gstrSetupLogDetailFile As String ' SQL Server specific InstallShield log file used to diagnose failed installations
Public gstrDefaultAppDbImageFolderName As String ' Default folder name used for database installation images
Public gstrAppDbImageIniFileName As String ' INI file name for database installation image settings
Public gblnUnicodeAppDbFiles As Boolean ' Indicates whether SqlAppSetupWiz will open DDL, load list and BCP files as ANSI or Unicode files
Public glngCurSqlCsdVer As Long ' Most recent SQL Server 7.0 Service Pack CSD version

' Global Variables and contants specific to use of dlgGetPath
Public gstrCurDrive As String
Public gstrCurPath As String
Public gblnPathChanged As Boolean ' Flag which indicates whether the current path changed in dlgGetFile

' Global Variables and contants specific to use of dlgGetNTUser and dlgGetSqlUser
Public gstrDomain As String
Public gstrAccount As String
Public gstrDbRoles() As String
Public gstrDbRole As String
Public gstrPassword As String

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
    TristateFalse = 0&
End Enum

Public dCleanup As dlgCleanup

Private Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long

Sub Main()
    Dim blnError As Boolean
    
    If App.PrevInstance Then
        MsgBox "An instance of SqlAppSetupWiz is already running.", vbCritical + vbOKOnly, "Error"
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
    
    WriteLogMsg goLogFileHandle, "SQL Setup App Wizard Setup Version " & Str(App.Major)
    WriteLogMsg goLogFileHandle, "Session Started."
    
    Set dCleanup = New dlgCleanup
    dCleanup.Visible = False
    
    ' Start Wizard
    dlgWiz01.Show 'vbModal
    
End Sub

Sub InitVariables(blnError As Boolean)
    Dim oFolder As Object
    blnError = False
    
    ' Create Objects
    On Error Resume Next
            
    Set goFileSystem = CreateObject("Scripting.FileSystemObject")
        
    If Err.Number <> 0 Then
        ShowErrorDialog "Windows Scripting Host Error. The wizard cannot continue.  Check to make sure you have the Windows Scripting Host installed.", Err.Number, Err.Description
        Err.Clear
        blnError = True
        Exit Sub
    End If
    
    On Error GoTo 0
        
    ' Get the System Path
    Set oFolder = goFileSystem.GetSpecialFolder(WINDOWSFOLDER)
    gstrWinDir = oFolder.Path
        
    ' Get the Temp Path
    Set oFolder = goFileSystem.GetSpecialFolder(TEMPORARYFOLDER)
    gstrTempDir = oFolder.Path
    
    ' Get Application Settings
    LoadAppSettings blnError
    
    If blnError Then
        ShowErrorDialog "Unable to load application settings from '" & INI_FILE & "'. SETUP CANNOT CONTINUE.", 0, ""
        blnError = True
        Exit Sub
    End If
    
    ' Load db servers
    Set goDbServers = New DbServers
    goDbServers.Load (App.Path & "\" & gstrDbServerFile)
    
    If goDbServers.Count = 0 Then
        ShowErrorDialog "Missing or corrupted " & gstrDbServerFile & " file. Setup cannot continue.", 0, ""
        blnError = True
        Exit Sub
    End If
    
    ' Load os suites
    Set goOSSuites = New OSSuites
    goOSSuites.Load (App.Path & "\" & gstrOsSuiteFile)
    
    If goOSSuites.Count = 0 Then
        ShowErrorDialog "Missing or corrupted " & gstrDbServerFile & " file. Setup cannot continue.", 0, ""
        blnError = True
        Exit Sub
    End If
        
    ' Load Application Database Settings
'    LoadAppDbSettings goDbApp, blnError
'
'    If blnError Then
'        ShowErrorDialog "Unable to load database application settings from '" & INI_FILE & "'. SETUP CANNOT CONTINUE.", 0, ""
'        blnError = True
'        Exit Sub
'    End If
'
    ' Initialize Session Variables
    glngOsType = OS_NOT_INITIALIZED
    gblnBuiltinAdminsMember = False
    glngIeInstalled = INSTALL_STATE_NOT_INITIALIZED
    glngSqlType = SQL_NOT_INITIALIZED
    glngSpType = SQL_NOT_INITIALIZED
    gblnReboot = False
    glngSqlMajorVer = NOT_INITIALIZED
    glngSqlMinorVer = NOT_INITIALIZED
    glngSqlBuildNum = NOT_INITIALIZED
    glngSqlCsdVer = NOT_INITIALIZED
    gblnSqlPreInstalled = False
    gstrSqlSaPwd = STR_NOT_INITIALIZED
    gblnDmoConnected = False
    gstrSetupExeFileSpec = STR_NOT_INITIALIZED
    gstrInstallImagePath = STR_NOT_INITIALIZED
    gstrProgramPath = STR_NOT_INITIALIZED
    gstrDataPath = STR_NOT_INITIALIZED
    gstrAppDbDestFolder = goDbApp.strAppDbName
    gstrUserName = STR_NOT_INITIALIZED
    gstrOrg = STR_NOT_INITIALIZED
    gstrCDKey = STR_NOT_INITIALIZED
    glngDbServerInst = INSTALL_STATE_NOT_INITIALIZED
    gstrServicePackFileSpec = STR_NOT_INITIALIZED
    glngServicePackInst = INSTALL_STATE_NOT_INITIALIZED
    glngAppDbInstallType = FILE_IMAGE
    gstrAppDbName = STR_NOT_INITIALIZED
    glngAppDbInst = INSTALL_STATE_NOT_INITIALIZED
    gblnJobServerStarted = False
    gstrAppDbSourceFolder = STR_NOT_INITIALIZED
    gstrAppDbIniFileSpec = STR_NOT_INITIALIZED
End Sub

Sub OpenLogFile(blnError As Boolean)
    blnError = False
    Dim strLogFileLongName As String
    
    strLogFileLongName = gstrTempDir & "\" & gstrLogFile
    
    On Error Resume Next
    
    If goFileSystem.FileExists(strLogFileLongName) Then
        goFileSystem.DeleteFile strLogFileLongName
        If Err.Number <> 0 Then
            ShowErrorDialog "Windows Scripting Host Error. The log file cannot be initialized.  Check to make sure that there is space available for the Windows TEMP directory.", Err.Number, Err.Description
            Err.Clear
            blnError = True
        End If
    End If
    
    If Not blnError Then
        Set goLogFileHandle = goFileSystem.CreateTextFile(strLogFileLongName, True, False)
        If Err.Number <> 0 Then
            ShowErrorDialog "Windows Scripting Host Error. The log file cannot be initialized.  Check to make sure that there is space available for the Windows TEMP directory.", Err.Number, Err.Description
            Err.Clear
            blnError = True
        End If
    End If
    
    On Error GoTo 0
End Sub
Sub Quit(Optional bConfirm As Boolean = False)
    Dim intResponse
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

Sub WriteLogMsg(oLogFileHandle As Object, strMessage As String)
    Dim strOutputLine As String
    
    strOutputLine = FormatDateTime(Now(), vbGeneralDate) & ": " & strMessage
    
    On Error Resume Next
    goLogFileHandle.WriteLine strOutputLine
    
    If Err.Number <> 0 Then
        MsgBox "Unable to write the following message to the log file:" & vbCrLf & strMessage, vbCritical + vbOKOnly, "Error"
        Err.Clear
    End If
    On Error GoTo 0
End Sub

Sub ShowErrorDialog(strMessage As String, lngErrNum, strErrDesc)
    MsgBox strMessage & vbCrLf & "Number: " & Str(lngErrNum) & vbCrLf & "Description: " & strErrDesc, vbCritical + vbOKOnly, "Error"
End Sub

Sub CleanUp()
    goLogFileHandle.Close
        
    Set goLogFileHandle = Nothing
    Set goFileSystem = Nothing
End Sub


Function FindCd(goFileSystem As Object) As String
    ' Returns drive letter of first CD-ROM Drive found
    Dim oDrive As Object
    
    FindCd = "C:"
    
    For Each oDrive In goFileSystem.Drives
        If oDrive.DriveType = 4 Then ' CD-ROM
            FindCd = oDrive.DriveLetter & ":"
        End If
    Next
End Function


Sub CrackIeBuild(strIeVer As String, lngIeMajorVer As Long, lngIeMinorVer As Long, lngIeBuildNum As Long, lngIeSubBuildNum As Long)
    Dim strTemp As String
    Dim lngTemp As Long
    
    lngIeMajorVer = -1
    lngIeMinorVer = -1
    lngIeBuildNum = -1
    lngIeSubBuildNum = -1
    
    ' Crack Major Version
    If IsNumeric(CLng(Left(strIeVer, InStr(1, strIeVer, ".") - 1))) Then
        lngTemp = CLng(Left(strIeVer, InStr(1, strIeVer, ".") - 1))
        If lngTemp >= 0 Then
            lngIeMajorVer = lngTemp
        End If
    End If
    
    ' Crack Minor Version
    strTemp = Mid(strIeVer, InStr(1, strIeVer, ".") + 1, Len(strIeVer))
    
    If Len(strTemp) > 0 And IsNumeric(CLng(Left(strTemp, InStr(1, strTemp, ".") - 1))) Then
        lngTemp = CLng(Left(strTemp, InStr(1, strTemp, ".") - 1))
        If lngTemp >= 0 Then
            lngIeMinorVer = lngTemp
        End If
    End If

    ' Crack Build Number
    strTemp = Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp))
    
    If Len(strTemp) > 0 And IsNumeric(CLng(Left(strTemp, InStr(1, strTemp, ".") - 1))) Then
        lngTemp = CLng(Left(strTemp, InStr(1, strTemp, ".") - 1))
        If lngTemp >= 0 Then
            lngIeBuildNum = lngTemp
        End If
    End If
    
    ' Crack Sub-Build Number
    If IsNumeric(CLng(Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp)))) Then
        lngTemp = CLng(Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp)))
        If lngTemp >= 0 Then
            lngIeSubBuildNum = CLng(Mid(strTemp, InStr(1, strTemp, ".") + 1, Len(strTemp)))
        End If
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
Sub CheckForInstalledComponents(blnError As Boolean, strMessage As String)
    blnError = False
    strMessage = ""
    
    glngOsType = OS_NOT_INITIALIZED
    glngIeInstalled = NOT_INITIALIZED
    glngSqlType = SQL_NOT_INITIALIZED
    glngSqlCsdVer = NOT_INITIALIZED
    gblnBuiltinAdminsMember = False
    
    CheckOs blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    CheckIE blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    CheckSql blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    ' Determine final status
    
    Select Case glngOsType
        Case WIN_95_OLD
            strMessage = strMessage & _
                "Setup cannot continue because DCOM95 is not enabled on your Windows 95/98 installation. " & _
                "Install DCOM95 and re-run Setup."
            Exit Sub
        Case WIN_95_98
            strMessage = strMessage & _
                "Windows 95 / 98 is installed with necessary pre-requisites; "
        Case NT_OLD
            strMessage = strMessage & _
                "Setup cannot continue because the version of Windows NT installed on your system is not supported. " & _
                "Please upgrade your Windows NT installation to version 4.0, service pack 4 or later."
            Exit Sub
        Case NT_40_WKS, NT_40_SRV, NT_40_SRV_ENT, NT_40_SRV_TRM
            CheckGroupMembership "BUILTIN", "Administrators", gblnBuiltinAdminsMember, blnError, strMessage
            
            If blnError Then
                Exit Sub
            End If
            
            If gblnBuiltinAdminsMember Then
                strMessage = strMessage & _
                    "Windows NT 4.0 is installed with necessary pre-requisites; "
            Else
                strMessage = strMessage & _
                    "The current user does not have sufficient priveleges to run setup. " & _
                    "Add the current user's account to 'BUILTIN\Administrators' then re-run setup. "
                Exit Sub
            End If
            
        Case WIN_2K_PRO, WIN_2K_SRV, WIN_2K_SRV_ADV, WIN_2K_SRV_DC
            CheckGroupMembership "BUILTIN", "Administrators", gblnBuiltinAdminsMember, blnError, strMessage
            
            If blnError Then
                Exit Sub
            End If
            
            If gblnBuiltinAdminsMember Then
                strMessage = strMessage & _
                    "Windows 2000 is installed; "
            Else
                strMessage = strMessage & _
                    "The current user does not have sufficient priveleges to run setup. " & _
                    "Add the current user's account to 'BUILTIN\Administrators' then re-run setup. "
                Exit Sub
            End If
            
        Case Else
            strMessage = strMessage & _
                "Setup cannot continue because the version of Windows installed on the " & _
                "system does not meet the necessary pre-requisites, which are: " & _
                "Windows 95/98 with DCOM95 installed, Windows NT 4.0 SP4 or later, or Windows 2000; " & _
                "Please address this problem and re-run setup."
            Exit Sub
    End Select
        
    Select Case glngIeInstalled
        Case INSTALLED
            strMessage = strMessage & _
                "Internet Explorer 4.01 SP1 or later is installed; "
        Case NOT_INSTALLED Or NOT_INITIALIZED
            strMessage = strMessage & _
                "Internet Explorer 4.01 SP1 or later is not installed, only MSDE installations will be supported; "
    End Select
    
    Select Case glngSqlType
        Case SQL_PRE_7, SQL_7_BETA
            strMessage = strMessage & _
                "Setup cannot continue because the version of SQL Server installed on the system is not supported. " & _
                "Please upgrade to SQL Server 7.0 and re-run Setup."
        Case MSDE_OFFICE_1_X86, SQL_STD_7_X86, SQL_ENT_7_X86, MSDE_1_X86, SQL_DESK_7_X86
            gblnSqlPreInstalled = True
            strMessage = strMessage & _
                "Setup will skip database server installation because a database server is already installed."
        Case SQL_POST_7
            strMessage = strMessage & _
                "Setup will skip database server installation because a post-SQL Server 7.0 database server is already installed."
        Case SQL_UNKNOWN
            strMessage = strMessage & _
                "Setup cannot continue because it was unable determine what build of SQL Server is installed." & _
                "Please remove SQL Server from the system and re-run Setup."
        Case SQL_NOT_INITIALIZED
            gblnSqlPreInstalled = False
            gstrSqlSaPwd = ""
            strMessage = strMessage & _
                "Setup will proceed with database server installation because SQL Server is not installed."
    End Select
End Sub

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

Sub CheckIE(blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim strIeVer As String
    Dim lngIeMajorVer As Long
    Dim lngIeMinorVer As Long
    Dim lngIeBuildNum As Long
    Dim lngIeSubBuildNum As Long
    
    blnError = False
    
    ' Check for IE 4.0 SP1 or Later
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\Internet Explorer\Version")
    
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
        strIeVer = CStr(varRegValue)
                
        CrackIeBuild strIeVer, lngIeMajorVer, lngIeMinorVer, lngIeBuildNum, lngIeSubBuildNum
        
        If (lngIeMajorVer <> -1) And (lngIeMinorVer <> -1) And (lngIeBuildNum <> -1) And (lngIeSubBuildNum <> -1) Then
            strMessage = strMessage & _
                "Internet Explorer Build Detected: " & LTrim(Str(lngIeMajorVer)) & "." & LTrim(Str(lngIeMinorVer)) & "." & LTrim(Str(lngIeBuildNum)) & "." & LTrim(Str(lngIeSubBuildNum)) & "; "
                    
            If lngIeMajorVer > 4 Then
                glngIeInstalled = INSTALLED
            Else
                ' Check for IE 4.01 SP1 (build 4.72.3110.8) or later
                If (lngIeMajorVer = 4) And (lngIeMinorVer = 72) And (lngIeBuildNum >= 3110) Then
                    Select Case lngIeBuildNum
                        Case 3110
                            If lngIeSubBuildNum >= 8 Then
                                glngIeInstalled = INSTALLED
                            Else
                                glngIeInstalled = NOT_INSTALLED
                            End If
                        Case Is > 3110
                            glngIeInstalled = INSTALLED
                    End Select
                Else
                    glngIeInstalled = NOT_INSTALLED
                    strMessage = strMessage & _
                        "Internet Explorer 4.01 SP1 or later is not installed; "
                End If
            End If
        Else
            glngIeInstalled = NOT_INSTALLED
            strMessage = strMessage & _
                "Setup could not detect the build number for Internet Explorer; "
        End If
    Else
        glngIeInstalled = NOT_INSTALLED
        strMessage = strMessage & _
            "Internet Explorer is not installed; "
    End If
End Sub

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

Function CrackPath(strPath As String) As String
    Dim intPos As Integer
    
    If Len(strPath) > 2 Then
        intPos = InStrRev(strPath, "\")
        If intPos = 0 Then
            CrackPath = ""
        Else
            CrackPath = Mid(strPath, 1, intPos)
        End If
    Else
        CrackPath = CurDir(Left(strPath, 2))
    End If
    
    ' Test for valid path
    If CrackPath <> "" Then
        If Not goFileSystem.FolderExists(CrackPath) Then
            CrackPath = CurDir(Left(strPath, 2))
        End If
    End If
End Function

Function CrackFile(strPath As String) As String
    Dim intPos As Integer
    Dim oFolder As Object
    Dim oFile As Object
    Dim strPathOnly As String
    
    If Len(strPath) > 3 Then
        intPos = InStrRev(strPath, "\")
        If intPos = 0 Then
            CrackFile = ""
        Else
            CrackFile = Right(strPath, Len(strPath) - intPos)
            strPathOnly = Mid(strPath, 1, intPos)
        End If
        
        ' Test for valid file
        If CrackFile <> "" And Not goFileSystem.FileExists(strPath) Then
            ' File does not exist so check for valid file name
            Set oFolder = goFileSystem.GetFolder(strPathOnly)
            
            On Error GoTo CrackFileError
            
            Set oFile = oFolder.CreateTextFile(CrackFile, False, True)
            Set oFile = Nothing
            Set oFolder = Nothing
            goFileSystem.DeleteFile (strPath)
            
            On Error GoTo 0
        End If
    End If
    Exit Function
    
CrackFileError:
    On Error GoTo 0
    CrackFile = ""
    Exit Function
End Function
Sub CheckSql(blnError As Boolean, strMessage As String)
    Dim varRegValue As Variant
    Dim strSqlVer As String
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
                glngSqlType = SQL_PRE_7
                Exit Sub
            Case Else
                glngSqlType = SQL_UNKNOWN
                Exit Sub
        End Select
    End If
    
    ' Look for SQL Server 6.0, 6.5, 7.0 or MSDE 1.0 installation
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\MSSQLServer\CurrentVersion\CurrentVersion")
    
    If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbString Then
        strSqlVer = CStr(varRegValue)
    
        CrackSqlBuild strSqlVer, glngSqlMajorVer, glngSqlMinorVer, glngSqlBuildNum
        
        If (glngSqlMajorVer <> -1) And (glngSqlMinorVer <> -1) And (glngSqlBuildNum <> -1) Then
            strMessage = strMessage & _
                "SQL Server or MSDE Build Detected: " & LTrim(Str(glngSqlMajorVer)) & "." & LTrim(Str(glngSqlMinorVer)) & "." & LTrim(Str(glngSqlBuildNum)) & "; "
            gblnSqlPreInstalled = True
            
            Select Case glngSqlMajorVer
                Case 6
                    glngSqlType = SQL_PRE_7
                    Exit Sub
                Case 7
                    Select Case glngSqlMinorVer
                        Case 0
                            If (glngSqlBuildNum >= 623) Then
                                ' Only check database server type for RTM 7.0 builds, this may change later.
                                ' Didn't check explicitly for 623 due to invalid build number of 677 reported
                                ' for some RTM MSDE installations.
                                
                                blnError = False
                                GetSqlPkgType goDbServer, glngSqlType, blnError, strMessage
                                If blnError Then
                                    glngSqlType = SQL_UNKNOWN
                                    Exit Sub
                                End If
                                
                                strMessage = strMessage & _
                                    "Database server package type '" & goDbServers(glngSqlType).Description & "' detected; "
                                                            
                                ' Check for SQL Server Service Pack Installation
                                varRegValue = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\MSSQLServer\CurrentVersion\CSDVersionNumber")
                            
                                If Not IsEmpty(varRegValue) And VarType(varRegValue) = vbLong Then
                                    glngSqlCsdVer = CLng(Hex(varRegValue))
                                    strMessage = strMessage & _
                                        "SQL Server Service Pack Version Detected is '" & CStr(glngSqlCsdVer) & "'; "
                                Else
                                    glngSqlCsdVer = NOT_INITIALIZED
                                End If
                            Else
                                glngSqlType = SQL_7_BETA
                                strMessage = strMessage & _
                                    "A pre-release version of SQL Server was detected; "
                                Exit Sub
                            End If
                        Case Is > 0
                            glngSqlType = SQL_POST_7
                            Exit Sub
                    End Select
                    
                Case Is > 7
                    glngSqlType = SQL_POST_7
                    strMessage = strMessage & _
                        "The installed version of SQL Server is a post-SQL Server 7.0 release; "
                    Exit Sub
                
                Case Else
                    blnError = True
                    glngSqlType = SQL_UNKNOWN
                    strMessage = strMessage & _
                        "Setup was unable to determine what build of SQL Server is currently installed; "
                    Exit Sub
            End Select
            
        Else
            blnError = True
            glngSqlType = SQL_UNKNOWN
            strMessage = strMessage & _
                "Setup was unable to determine what build of SQL Server is currently installed; "
        End If
    Else
        glngSqlType = SQL_NOT_INITIALIZED
    End If
End Sub


Public Sub PrepareSetupIni(lngDbServerType As DbServerType, strSetupIssFileSpec As String, strMessage As String, blnError As Boolean)
    blnError = False
    
    On Error GoTo FileSystemError
    
    ' Check to make sure source file exists
    If Not goFileSystem.FileExists(App.Path & "\" & goDbServers(lngDbServerType).SetupFile) Then
        blnError = True
        strMessage = "Setup cannot continue because the setup initialization file " & goDbServers(lngDbServerType).SetupFile & " is missing."
        Exit Sub
    End If
    
    strSetupIssFileSpec = gstrWinDir & "\" & gstrSetupIssFile
    
    ' Check for existence of an old setup.iss file, if exists, delete
    If goFileSystem.FileExists(strSetupIssFileSpec) Then
        goFileSystem.DeleteFile (strSetupIssFileSpec)
    End If
    
    ' Copy new setup.iss file to windows directory
    goFileSystem.CopyFile App.Path & "\" & goDbServers(lngDbServerType).SetupFile, strSetupIssFileSpec, True
    
    ' Remove read-only attribute
    blnError = ClearReadOnly(strSetupIssFileSpec)
    
    If blnError Then
        blnError = True
        strMessage = _
            "Setup encountered an error while trying to clear the read-only attribute on the file '" & strSetupIssFileSpec & "'; "
        Exit Sub
    End If

    Select Case lngDbServerType
        Case SQL_7_SP1_NT_X86, SQL_7_SP1_9X_X86, SQL_7_SP2_NT_X86, SQL_7_SP2_9X_X86
            ' No ISS modifications necessary, do nothing
        Case Else
            ' Set the user name and organization in the temporary setup.iss file
            WriteIni strSetupIssFileSpec, "SdRegisterUser-0", "szName", gstrUserName, blnError
            
            If blnError Then
                blnError = True
                strMessage = _
                    "Setup cannot continue because of an error involving the temporary file " & strSetupIssFileSpec & "."
                Err.Clear
                On Error GoTo 0
                Exit Sub
            Else
                strMessage = strMessage & _
                    "The 'szName' key in '" & strSetupIssFileSpec & "' was set to '" & gstrUserName & "'; "
            End If
            
            WriteIni strSetupIssFileSpec, "SdRegisterUser-0", "szCompany", gstrOrg, blnError
            
            If blnError Then
                blnError = True
                strMessage = _
                    "Setup cannot continue because of an error involving the temporary file " & strSetupIssFileSpec & "."
                Err.Clear
                On Error GoTo 0
                Exit Sub
            Else
                strMessage = strMessage & _
                    "The 'szCompany' key in '" & strSetupIssFileSpec & "' was set to '" & gstrOrg & "'; "
            End If
            
            ' Set the program and data directories in the temporary setup.iss file
            WriteIni strSetupIssFileSpec, "SetupTypeSQL-0", "SzDir", gstrProgramPath, blnError
            
            If blnError Then
                blnError = True
                strMessage = _
                    "Setup cannot continue because of an error involving the temporary file " & strSetupIssFileSpec & "."
                Err.Clear
                On Error GoTo 0
                Exit Sub
            Else
                strMessage = strMessage & _
                    "The 'SzDir' key in '" & strSetupIssFileSpec & "' was set to '" & gstrProgramPath & "'; "
            End If
            
            WriteIni strSetupIssFileSpec, "SetupTypeSQL-0", "SzDataDir", gstrDataPath, blnError
            
            If blnError Then
                blnError = True
                strMessage = _
                    "Setup cannot continue because of an error involving the temporary file " & strSetupIssFileSpec & "."
                Err.Clear
                On Error GoTo 0
                Exit Sub
            Else
                strMessage = strMessage & _
                    "The 'SzDataDir' key in '" & strSetupIssFileSpec & "' was set to '" & gstrDataPath & "'; "
            End If
            
            Select Case lngDbServerType
                Case SQL_STD_7_X86, SQL_ENT_7_X86, SQL_DESK_7_X86
                    ' Set the CD Key in the temporary setup.iss file
                    WriteIni strSetupIssFileSpec, "CDKEYDialog-0", "svCdkey", gstrCDKey, blnError
                    
                    If blnError Then
                        blnError = True
                        strMessage = _
                            "Setup cannot continue because of an error involving the temporary file " & strSetupIssFileSpec & "."
                        Err.Clear
                        On Error GoTo 0
                        Exit Sub
                    Else
                        strMessage = strMessage & _
                            "The 'svCdkey' key in '" & strSetupIssFileSpec & "' was set to '" & gstrCDKey & "'; "
                    End If
                    
                Case Else
                    ' No CD Key Required, do nothing
            End Select
    End Select
    
    Exit Sub

FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub
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
            If MsgBox("Ignore fact that OS version not found?", vbQuestion + vbYesNo) = vbYes Then
              glngOsType = WIN_95_98
              blnError = False
            End If
            Exit Sub
        End If
    Else
        glngOsType = NOT_INITIALIZED
        blnError = True
        strMessage = strMessage & _
            "Setup could not determine what version of Windows is installed; "
        If MsgBox("Ignore fact that OS version not found?", vbQuestion + vbYesNo) = vbYes Then
          glngOsType = WIN_95_98
          blnError = False
        End If
        Exit Sub
    End If
End Sub

Public Function ReadRegValue(strValueName As String) As Variant
    Dim oWScriptShell As Object
        
    On Error Resume Next
    Set oWScriptShell = CreateObject("WScript.Shell")

    ReadRegValue = Empty
    ReadRegValue = oWScriptShell.RegRead(strValueName)
            
    If Err.Number > 0 Then
        Err.Clear
    End If
    
    On Error GoTo 0
    
    Set oWScriptShell = Nothing
End Function

Public Function GetWindowsSysDir() As String
    Dim strBuf As String

    strBuf = Space$(255)
    '
    'Get the system directory and then trim the buffer to the exact length
    'returned and add a dir sep (backslash) if the API didn't return one
    '
  If GetSystemDirectory(strBuf, 255) Then
    GetWindowsSysDir = Left(strBuf, InStr(strBuf, vbNullChar) - 1)
  End If
End Function


Public Sub WriteRegValue(strValueName As String, strValue As String, blnError As Boolean)
    
    Dim oWScriptShell As Object
    
    blnError = False
    On Error Resume Next
    
    Set oWScriptShell = CreateObject("WScript.Shell")
    
    oWScriptShell.RegWrite strValueName, strValue
            
    If Err.Number > 0 Then
        Err.Clear
        blnError = True
    End If
    
    Set oWScriptShell = Nothing
End Sub
Public Sub VerifyInstallImage(blnVerified As Boolean, strMessage As String)
    Dim strSetupIniData As String
    Dim oServerFile As Object
    Dim strServerFileSpec As String
    Dim strIniPath As String
    
    If Right(gstrInstallImagePath, 1) = "\" Then
        strIniPath = Left(gstrInstallImagePath, Len(gstrInstallImagePath) - 1)
    Else
        strIniPath = gstrInstallImagePath
    End If
    
    Select Case glngSqlType
        Case SQL_STD_7_X86, SQL_ENT_7_X86, SQL_DESK_7_X86, MSDE_OFFICE_1_X86, SQL_DEV_7_X86, SQL_SBS_7_X86, SQL_POST_7
            ' Valid package types for this routine, continue
        Case Else
            blnVerified = False
            strMessage = strMessage & "Invalid package type in VerifyInstallImage; "
            Exit Sub
    End Select
    
    Select Case glngSqlType
        Case SQL_STD_7_X86, SQL_ENT_7_X86, SQL_DEV_7_X86
            '
            ' Step 1: Verify Pacakge Type in gstrSqlSetupIniFileSpec for SQL Server 7.0 images
            '
            
            strSetupIniData = ReadIni(strIniPath & gstrSqlSetupIniFileSpec, gstrSqlSetupIniSection, gstrSqlSetupIniKey)
            
            If strSetupIniData = STR_NOT_INITIALIZED Then
                blnVerified = False
                strMessage = "The file " & gstrSqlSetupIniFileSpec & " is missing or corrupted;"
                WriteLogMsg goLogFileHandle, strMessage
                Exit Sub
            Else
                strMessage = "The value '" & strSetupIniData & "' was read from '" & gstrSqlSetupIniFileSpec & "' section '" & gstrSqlSetupIniSection & "' key '" & gstrSqlSetupIniKey & "'; "
                
                Select Case glngSqlType
                    Case SQL_DEV_7_X86, SQL_STD_7_X86, SQL_ENT_7_X86
                        If strSetupIniData = goDbServers(glngSqlType).SetupIniData Then
                            blnVerified = True
                        Else
                            blnVerified = False
                        End If
                        
                    Case SQL_DESK_7_X86
                        If (strSetupIniData = goDbServers(SQL_STD_7_X86).SetupIniData) Or (strSetupIniData = goDbServers(SQL_ENT_7_X86).SetupIniData) Then
                            blnVerified = True
                        Else
                            blnVerified = False
                        End If
                End Select
                        
                If blnVerified Then
                    strMessage = strMessage & _
                        "The installation image is compatible with SQL Server 7.0 package type '" & goDbServers(glngSqlType).Description & "'; "
                Else
                    strMessage = strMessage & _
                       "The installation image supplied is of package type " & strSetupIniData & _
                       " which is incompatible with an installation of " & goDbServers(glngSqlType).Description & "; "
                    Exit Sub
                End If
            End If
        
        Case MSDE_OFFICE_1_X86, SQL_SBS_7_X86, SQL_DESK_7_X86
            ' gstrSqlSetupIniFileSpec does not exist for MSDE_1_OFFICE_X86 and SQL_SBS_7_X86 images so do nothing
                        
        Case SQL_POST_7
            blnVerified = True
            strMessage = strMessage & _
                "Skipping verification check because no rules are defined for releases of SQL Server after version 7.0;"
            Exit Sub
    End Select

    '
    ' Step 2: Verify file properties of database server executable
    '
    
    If Right(gstrInstallImagePath, 1) = "\" Then
        strServerFileSpec = Left(gstrInstallImagePath, 2) & goDbServers(glngSqlType).ServerFileSpec
    Else
        strServerFileSpec = gstrInstallImagePath & goDbServers(glngSqlType).ServerFileSpec
    End If
    
    If Not goFileSystem.FileExists(strServerFileSpec) Then
        blnVerified = False
        strMessage = strMessage & _
            "Unable to locate server executable '" & strServerFileSpec & "'; "
        Exit Sub
    End If
    
    Set oServerFile = goFileSystem.GetFile(strServerFileSpec)
    
    If oServerFile.Size = goDbServers(glngSqlType).ServerSizeBytes Then
        blnVerified = True
        strMessage = strMessage & _
            "File size verified for server executable; "
    Else
        blnVerified = False
        strMessage = strMessage & _
            "File size of " & CStr(oServerFile.Size) & " bytes does not match expected size of " & CStr(goDbServers(glngSqlType).ServerSizeBytes) & " bytes; "
    End If
End Sub


Public Sub VerifyCdKeyFormat(strCdKey As String, blnError As Boolean)
    Dim strTemp As String
    Dim strKeyPart1 As String
    Dim strKeyPart2 As String
    Dim strKeyPart3 As String
    Dim strKeyPart4 As String
    Dim lngPos As Long
    
    If Len(strCdKey) <> 23 Then
        blnError = True
        Exit Sub
    End If
    
    strTemp = strCdKey
    blnError = False
    
    ' Key Part 1
    lngPos = InStr(1, strTemp, "-")
    
    If lngPos = 0 Then
        blnError = True
        Exit Sub
    End If
    
    strKeyPart1 = Left(strTemp, lngPos - 1)
    
    If Len(strKeyPart1) <> 5 Or Not IsNumeric(strKeyPart1) Then
        blnError = True
        Exit Sub
    End If
    
    ' Key Part 2
    strTemp = Mid(strTemp, lngPos + 1, Len(strTemp))
    lngPos = InStr(1, strTemp, "-")
    
    If lngPos = 0 Then
        blnError = True
        Exit Sub
    End If
    
    strKeyPart2 = Left(strTemp, lngPos - 1)
    
    If Len(strKeyPart2) <> 3 Or Not IsNumeric(strKeyPart2) Then
        blnError = True
        Exit Sub
    End If
    
    ' Key Part 3
    strTemp = Mid(strTemp, lngPos + 1, Len(strTemp))
    lngPos = InStr(1, strTemp, "-")
    
    If lngPos = 0 Then
        blnError = True
        Exit Sub
    End If
    
    strKeyPart3 = Left(strTemp, lngPos - 1)
    
    If Len(strKeyPart3) <> 7 Or Not IsNumeric(strKeyPart3) Then
        blnError = True
        Exit Sub
    End If
    
    ' Key Part 4
    strTemp = Mid(strTemp, lngPos + 1, Len(strTemp))
    strKeyPart4 = strTemp
    
    If Len(strKeyPart4) <> 5 Or Not IsNumeric(strKeyPart4) Then
        blnError = True
    End If
End Sub

Sub CreateTempFolder(strTempFolder As String, blnError As Boolean)
    blnError = False
    On Error GoTo CreateTempFolderError
    
    Do While True
        strTempFolder = gstrTempDir & "\" & goFileSystem.GetTempName
        If Not goFileSystem.FolderExists(strTempFolder) Then
            Exit Do
        End If
    Loop
    
    goFileSystem.CreateFolder (strTempFolder)
    
    On Error GoTo 0
    Exit Sub

CreateTempFolderError:
    blnError = True
    On Error GoTo 0
    
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

Function GetDataPathFromReg() As String
    Dim varRegValue As Variant
    
    varRegValue = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\Setup\SQLDataRoot")

    If IsEmpty(varRegValue) Or VarType(varRegValue) <> vbString Then
        GetDataPathFromReg = STR_NOT_INITIALIZED
    End If

    GetDataPathFromReg = CStr(varRegValue)
End Function

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

Public Sub CreateDataFileDirectory(strDataFileDir As String, strFolderName As String, blnError As Boolean, strMessage As String)
    blnError = False
    
    If gstrDataPath = STR_NOT_INITIALIZED Then
        gstrDataPath = GetDataPathFromReg()
        
        If gstrDataPath = STR_NOT_INITIALIZED Then
            blnError = True
            strMessage = strMessage & _
                "Unable to retrieve data directory from registry; "
            Exit Sub
        End If
    End If
    
    On Error GoTo FileSystemError
    
    strDataFileDir = gstrDataPath & "\Data"
    
    If Not goFileSystem.FolderExists(strDataFileDir) Then
        goFileSystem.CreateFolder (strDataFileDir)
        strMessage = strMessage & _
            "Setup created the data file directory named '" & strDataFileDir & "'; "
    End If
    
    strDataFileDir = gstrDataPath & "\Data\" & strFolderName
    
    If Not goFileSystem.FolderExists(strDataFileDir) Then
        goFileSystem.CreateFolder (strDataFileDir)
        strMessage = strMessage & _
            "Setup created the data file directory named '" & strDataFileDir & "'; "
    End If
    
    Exit Sub

FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub
End Sub

Public Sub GenerateUniqueDataFileNames(strDbName As String, strDataFileDir As String, strDataFileName As String, strLogFileName As String, blnError As Boolean, strMessage As String)
    Dim I As Integer
    Dim strNewDbName As String
    
    blnError = False
    
    I = 0
    strDataFileName = strDbName & "_Data.MDF"
    strLogFileName = strDbName & "_Log.LDF"
    
    On Error GoTo FileSystemError
    
    Do While True
        I = I + 1
        
        With goFileSystem
            If .FileExists(strDataFileDir & "\" & strDataFileName) Or .FileExists(strDataFileDir & "\" & strLogFileName) Then
                strDataFileName = strDbName & "_" & CStr(I) & "_Data.MDF"
                strLogFileName = strDbName & "_" & CStr(I) & "_Log.LDF"
            Else
                Exit Do
            End If
        End With
    Loop
    
    Exit Sub
    
FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub
End Sub

Public Sub LoadAppDbSettings(strIniFileSpec As String, oDbApp As AppDb, blnError As Boolean, strMessage As String)
    Dim strValueRead As String
    
    blnError = False
    
    With oDbApp
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbSortOrderId")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbSortOrderId = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbUnicodeLocaleId")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbUnicodeLocaleId = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbUnicodeCompStyle")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbUnicodeCompStyle = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbSqlMajorVer")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbSqlMajorVer = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbSqlMinorVer")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbSqlMinorVer = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbSqlBuildNum")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbSqlBuildNum = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbCsdVer")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbCsdVer = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbBytesReq")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .lngAppDbBytesReq = CLng(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbName")
        
        If strValueRead = STR_NOT_INITIALIZED Then
            blnError = True
            Exit Sub
        End If
        
        .strAppDbName = strValueRead
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbVer")
        
        If strValueRead = STR_NOT_INITIALIZED Then
            blnError = True
            Exit Sub
        End If
        
        .strAppDbVer = strValueRead
    
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbVerSp")
        
        If strValueRead = STR_NOT_INITIALIZED Then
            blnError = True
            Exit Sub
        End If
        
        .strAppDbVerSp = strValueRead
    
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbOrg")
        
        If strValueRead = STR_NOT_INITIALIZED Then
            blnError = True
            Exit Sub
        End If
        
        .strAppDbOrg = strValueRead
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbDate")
        
        If strValueRead = STR_NOT_INITIALIZED Or Not IsDate(strValueRead) Then
            blnError = True
            Exit Sub
        End If
        
        .dtAppDbDate = CDate(strValueRead)
        
        strValueRead = ReadIni(strIniFileSpec, APPDB_SECTION, "AppDbDesc")
        
        If strValueRead = STR_NOT_INITIALIZED Then
            blnError = True
            Exit Sub
        End If
        
        .strAppDbDesc = strValueRead
    End With
End Sub

Public Sub LocateDbFiles(strDbName As String, strFolder As String, blnFound As Boolean, blnError As Boolean, strMessage As String)
    Dim strDataFileDir As String
    Dim strDataFileSpec As String
    Dim strLogFileSpec As String
    
    blnError = False
    blnFound = False
    
    CreateDataFileDirectory strDataFileDir, strFolder, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    strDataFileSpec = strDataFileDir & "\" & strDbName & "_Data.MDF"
    strLogFileSpec = strDataFileDir & "\" & strDbName & "_Log.LDF"
    
    If goFileSystem.FileExists(strDataFileSpec) Then
        blnFound = True
        strMessage = strMessage & _
            "The data file named '" & strDataFileSpec & "' was found; "
    End If
    
    If goFileSystem.FileExists(strLogFileSpec) Then
        blnFound = True
        strMessage = strMessage & _
            "The log file named '" & strLogFileSpec & "' was found; "
    End If
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
    
    ' Load gstrDbServerFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "DbServerFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrDbServerFile = strValueRead
    
    ' Load gstrOsSuiteFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "OsSuiteFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrOsSuiteFile = strValueRead
    
    ' Load gstrSqlSetupIniFileSpec
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SqlSetupIniFileSpec")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSqlSetupIniFileSpec = strValueRead
    
    ' Load gstrSqlSetupIniSection
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SqlSetupIniSection")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSqlSetupIniSection = strValueRead
    
    ' Load gstrSqlSetupIniKey
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SqlSetupIniKey")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSqlSetupIniKey = strValueRead
    
    ' Load gstrSetupLogFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogFile = strValueRead
    
    ' Load gstrSetupIssFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupIssFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupIssFile = strValueRead
    
    ' Load gstrSetupLogStatusSection
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogStatusSection")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogStatusSection = strValueRead
    
    ' Load gstrSetupLogCompletedKey
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogCompletedKey")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogCompletedKey = strValueRead
    
    ' Load gstrSetupLogRebootRequiredKey
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogRebootRequiredKey")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogRebootRequiredKey = strValueRead
    
    ' Load gstrSetupLogResponseResultSection
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogResponseResultSection")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogResponseResultSection = strValueRead
    
    ' Load gstrSetupLogResultCodeKey
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogResultCodeKey")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogResultCodeKey = strValueRead
    
    ' Load gstrSetupLogDetailFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "SetupLogDetailFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrSetupLogDetailFile = strValueRead

    ' Load gstrBackupJobFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "BackupJobFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrBackupJobFile = strValueRead
    
    ' Load gstrStartupJobFile
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "StartupJobFile")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrStartupJobFile = strValueRead
    
    ' Load gstrJobServerStartupSpScript
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "JobServerStartupSpScript")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrJobServerStartupSpScript = strValueRead
    
    ' Load gstrJobServerStartupSpName
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "JobServerStartupSpName")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrJobServerStartupSpName = strValueRead
    
    ' Load gstrDefaultAppDbImageFolderName
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "DefaultAppDbImageFolderName")
    
    If strValueRead = STR_NOT_INITIALIZED Then
        blnError = True
        Exit Sub
    End If
    
    gstrDefaultAppDbImageFolderName = strValueRead
    
    ' Load AppDbImageIniFileName
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "AppDbImageIniFileName")
    
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
    
    ' Load glngCurSqlCsdVer
    strValueRead = ReadIni(strIniFileSpec, APP_SETTINGS_SECTION, "CurSqlCsdVer")
    
    If strValueRead = STR_NOT_INITIALIZED Or Not IsNumeric(strValueRead) Then
        blnError = True
        Exit Sub
    End If
    
    glngCurSqlCsdVer = CLng(strValueRead)
End Sub

Public Sub InstallJobFile(strJobFileName As String, strJobFileSourceSpec As String, strJobFileDestSpec As String, blnError As Boolean, strMessage As String)
    Dim strDataPath As String
    
    blnError = False
    
    strDataPath = GetDataPathFromReg
    
    If strDataPath = STR_NOT_INITIALIZED Then
        blnError = True
        strMessage = strMessage & _
            "Unable to retrieve data path from registry; "
        Exit Sub
    End If
    
    strJobFileDestSpec = strDataPath & "\JOBS\" & strJobFileName
    
    On Error GoTo FileSystemError
    
    With goFileSystem
        If Not .FileExists(strJobFileSourceSpec) Then
            strMessage = strMessage & _
                "The file '" & strJobFileSourceSpec & "' does not exist."
            blnError = True
            Exit Sub
        End If
        
        If Not .FolderExists(strDataPath & "\JOBS") Then
            .CreateFolder (strDataPath & "\JOBS")
        End If
        
        If .FileExists(strJobFileDestSpec) Then
            .DeleteFile strJobFileDestSpec
            strMessage = strMessage & _
                "Deleted existing file '" & strJobFileDestSpec & "'; "
        End If
        
        .CopyFile strJobFileSourceSpec, strJobFileDestSpec, True
        strMessage = strMessage & _
            "Copied the file '" & strJobFileSourceSpec & "' to '" & strJobFileDestSpec & "'; "
            
        blnError = ClearReadOnly(strJobFileDestSpec)
        
        If blnError Then
            strMessage = strMessage & _
                "Error clearing read only attribute on file '" & strJobFileDestSpec & "'; "
            Exit Sub
        End If
    End With
    
    Exit Sub
    
FileSystemError:
    blnError = True
    strMessage = strMessage & _
        "File system error number '" & CStr(Err.Number) & "' and description '" & Err.Description & "; "
    Err.Clear
    Exit Sub
End Sub

Public Sub CreateAutoStartRegEntry(strJobFileSpec As String, blnError As Boolean, strMessage As String)
    Dim strValueName As String
    Dim strValue As String
    
    ' this routine adds an entry to the Win95/98 registry which executes a script
    ' upon startup
    
    blnError = False
    
    strValueName = "HKLM\Software\Microsoft\Windows\CurrentVersion\Run\StartDbServices"
    strValue = "WSCRIPT """ & strJobFileSpec & """"
    
    WriteRegValue strValueName, strValue, blnError
    
    If Not blnError Then
        strMessage = strMessage & _
            "The '" & strValueName & "' key was added to the registry with the value '" & strValue & "'; "
    End If
End Sub

Public Sub FinishInstallation(blnError As Boolean, strMessage As String)
    Dim strValueName As String
    Dim strValue As String
    
    ' this routine writes the name of the application database to the registry
    ' so that applications do not have to hard code database names
    blnError = False
    
    strValueName = "HKLM\Software\SqlAppSetupWiz\" & goDbApp.strAppDbName & "\AppDbName"
    strValue = gstrAppDbName
    
    WriteRegValue strValueName, strValue, blnError
    
    If Not blnError Then
        strMessage = strMessage & _
            "The '" & strValueName & "' key was added to the registry with the value '" & strValue & "'; "
    End If
End Sub

Function CopyFile(SourceName As String, DestName As String, fu As dlgProgressBar) As Boolean
On Error GoTo CopyFile_Err

Const BufSize = 8192
Dim Buffer As String * BufSize, TempBuf As String, tVal As Single
Dim SourceF As Integer, DestF As Integer, I As Long, numAttempts As Integer

numAttempts = 0
  
If FileExists(DestName) Then Kill DestName
Dim lngFileSizeTotal As Long, lngFileSizeCopied As Long
lngFileSizeTotal = FileLen(SourceName)

SourceF = FreeFile
Open SourceName For Binary As #SourceF
DestF = FreeFile
Open DestName For Binary As #DestF
For I = 1 To LOF(SourceF) \ BufSize
  Get #SourceF, , Buffer
  Put #DestF, , Buffer
  tVal = ((lngFileSizeCopied + (Loc(SourceF))) / lngFileSizeTotal) * 100
  fu.pbar.Value = IIf(tVal > 100, 100, tVal)
Next I
I = LOF(SourceF) Mod BufSize
If I > 0 Then
  Get #SourceF, , Buffer
  TempBuf = LeftB(Buffer, I)
  Put #DestF, , TempBuf
  tVal = ((lngFileSizeCopied + (Loc(SourceF))) / lngFileSizeTotal) * 100
  fu.pbar.Value = IIf(tVal > 100, 100, tVal)
End If
lngFileSizeCopied = lngFileSizeCopied + LOF(SourceF)
Close #SourceF
Close #DestF
CopyFile = True


Exit Function

CopyFile_Err:
Select Case Err
  Case 70, 75    ' permission denied, because file in use, file access error
      If MsgBox("The file '" & DestName & "' is in use.  Make sure you don't have another copy of the database open.  Click 'Retry' to attempt copying the file again.", vbRetryCancel + vbQuestion) = vbCancel Then
        Exit Function
      Else
        Resume
      End If
  Case Else
    MsgBox "There was an error copying the file '" & SourceName & "' to '" & DestName & "'.  Error #" & Err & ", " & Err.Description, vbCritical
    Close #SourceF
    Close #DestF
    Exit Function
End Select

End Function

Function FileExists(ByVal strPathName As String) As Integer
On Error Resume Next

Dim intFileNum As Integer

'Remove any trailing directory separator character
If Right$(strPathName, 1) = "\" Then
  strPathName = Left$(strPathName, Len(strPathName) - 1)
End If

'Attempt to open the file, return value of this function is False
'if an error occurs on open, True otherwise
intFileNum = FreeFile
Open strPathName For Input As intFileNum
FileExists = IIf(Err, False, True)
Close intFileNum
Err.Clear

End Function

