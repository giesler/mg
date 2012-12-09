Attribute VB_Name = "modWin32"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' modWin32.bas module isolates all Win32 related procedures from rest of application '
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit

'
' Declarations for process and thread related functions
'
Private Const DELETE = &H10000
Private Const READ_CONTROL = &H20000
Private Const WRITE_DAC = &H40000
Private Const WRITE_OWNER = &H80000
Private Const SYNCHRONIZE = &H100000

Private Const STANDARD_RIGHTS_REQUIRED = &HF0000
Private Const STANDARD_RIGHTS_READ = READ_CONTROL
Private Const STANDARD_RIGHTS_WRITE = READ_CONTROL
Private Const STANDARD_RIGHTS_EXECUTE = READ_CONTROL
Private Const STANDARD_RIGHTS_ALL = &H1F0000
Private Const SPECIFIC_RIGHTS_ALL = &HFFFF

Private Const ACCESS_SYSTEM_SECURITY = &H1000000

Private Enum dwProcDesiredAccess
    PROCESS_TERMINATE = &H1
    PROCESS_CREATE_THREAD = &H2
    PROCESS_SET_SESSIONID = &H4
    PROCESS_VM_OPERATION = &H8
    PROCESS_VM_READ = &H10
    PROCESS_VM_WRITE = &H20
    PROCESS_DUP_HANDLE = &H40
    PROCESS_CREATE_PROCESS = &H80
    PROCESS_SET_QUOTA = &H100
    PROCESS_SET_INFORMATION = &H200
    PROCESS_QUERY_INFORMATION = &H400
    PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or &HFFF
End Enum

Private Enum dwThreadDesiredAccess
    THREAD_TERMINATE = &H1
    THREAD_SUSPEND_RESUME = &H2
    THREAD_GET_CONTEXT = &H8
    THREAD_SET_CONTEXT = &H10
    THREAD_SET_INFORMATION = &H20
    THREAD_QUERY_INFORMATION = &H40
    THREAD_SET_THREAD_TOKEN = &H80
    THREAD_IMPERSONATE = &H100
    THREAD_DIRECT_IMPERSONATION = &H200
    THREAD_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or &H3FF
End Enum

Private Declare Function OpenProcess Lib "kernel32" ( _
    ByVal dwDesiredAccess As dwProcDesiredAccess, _
    ByVal bInheritHandle As Boolean, _
    ByVal dwProcessId As Long) As Long

Private Declare Function OpenThread Lib "kernel32" ( _
    ByVal dwDesiredAccess As dwThreadDesiredAccess, _
    ByVal bInheritHandle As Boolean, _
    ByVal dwThreadId As Long) As Long
    
Private Declare Function GetCurrentProcess Lib "kernel32" () As Long
Private Declare Function GetCurrentProcessId Lib "kernel32" () As Long
Private Declare Function GetCurrentThreadId Lib "kernel32" () As Long

Private Declare Function GetExitCodeProcess Lib "kernel32" ( _
    ByVal hProcess As Long, _
    lpExitCode As Long) As Long

Private Const STILL_ACTIVE = &H103

Private Declare Function CloseHandle Lib "kernel32" ( _
    ByVal hObject As Long) As Long

Private Declare Function GetLastError Lib "kernel32" () As Long

' Constants for ExitWindowsEx
Private Const EWX_LOGOFF = &H0
Private Const EWX_SHUTDOWN = &H1
Private Const EWX_REBOOT = &H2
Private Const EWX_FORCE = &H4
Private Const EWX_POWEROFF = &H8
Private Const EWX_FORCEIFHUNG = &H10
Private Const EWX_RESET = EWX_LOGOFF + EWX_FORCE + EWX_REBOOT

Private Declare Function ExitWindowsEx Lib "user32" ( _
    ByVal uFlags As Long, _
    ByVal Reserved As Long) As Long
    
'
' Declarations for Version related functions
'

Private Type OSVERSIONINFOEXA
    dwOSVersionInfoSize As Long
    dwMajorVersion As Long
    dwMinorVersion As Long
    dwBuildNumber As Long
    dwPlatformId As Long
    szCSDVersion(127) As Byte
    wServicePackMajor As Integer
    wServicePackMinor As Integer
    wSuiteMask As Integer
    wProductType As Byte
    wReserved As Byte
End Type

Private Declare Function GetVersionEx _
    Lib "kernel32" _
    Alias "GetVersionExA" ( _
    lpVersionInformation As OSVERSIONINFOEXA) As Integer
    
Private Declare Function VerifyVersionInfo _
    Lib "kernel32" _
    Alias "VerifyVersionInfoA" ( _
    ByRef lpVersionInformation As OSVERSIONINFOEXA, _
    ByVal dwTypeMask As Long, _
    ByVal dwlConditionMask As Currency) As Integer

Private Declare Function VerSetConditionMask Lib "kernel32" ( _
    ByVal dwlConditionMask As Currency, _
    ByVal dwTypeMask As Long, _
    ByVal dwConditionMask As Byte) As Currency
    
' dwTypeMask Defines

Private Enum dwTypeMask
    VER_MINORVERSION = &H1
    VER_MAJORVERSION = &H2
    VER_BUILDNUMBER = &H4
    VER_PLATFORMID = &H8
    VER_SERVICEPACKMINOR = &H10
    VER_SERVICEPACKMAJOR = &H20
    VER_SUITENAME = &H40
    VER_PRODUCT_TYPE = &H80
End Enum

Private Enum wProductType
    VER_NT_WORKSTATION = &H1
    VER_NT_DOMAIN_CONTROLLER = &H2
    VER_NT_SERVER = &H3
End Enum

Private Enum dwPlatformId
    VER_PLATFORM_WIN32s = 0&
    VER_PLATFORM_WIN32_WINDOWS
    VER_PLATFORM_WIN32_NT
End Enum

Private Enum wSuiteMask
    VER_SUITE_SMALLBUSINESS = &H1
    VER_SUITE_ENTERPRISE = &H2
    VER_SUITE_BACKOFFICE = &H4
    VER_SUITE_COMMUNICATIONS = &H8
    VER_SUITE_TERMINAL = &H10
    VER_SUITE_SMALLBUSINESS_RESTRICTED = &H20
    VER_SUITE_EMBEDDEDNT = &H40
    VER_SUITE_DATACENTER = &H80
    VER_SUITE_SINGLEUSERTS = &H100
End Enum

Private Enum dwConditionMask
    VER_EQUAL = 1&
    VER_GREATER
    VER_GREATER_EQUAL
    VER_LESS
    VER_LESS_EQUAL
    VER_AND
    VER_OR
End Enum


' Other Win32 Function Declarations
Private Declare Function WritePrivateProfileString Lib "kernel32" _
    Alias "WritePrivateProfileStringA" _
    (ByVal AppName As String, ByVal KeyName As String, ByVal keydefault As String, ByVal FileName As String) _
    As Long

Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" ( _
    ByVal AppName As String, _
    ByVal KeyName As String, _
    ByVal keydefault As String, _
    ByVal ReturnString As String, _
    ByVal NumBytes As Integer, _
    ByVal FileName As String) As Long

' WNet Function Declarations
Private Declare Function WNetConnectionDialog Lib "mpr.dll" ( _
    ByVal hwnd As Long, _
    ByVal dwType As Long) As Long

Private Const RESOURCETYPE_DISK = &H1

' Security Function declarations
Private Type SID_AND_ATTRIBUTES
  pSid As Long
  Attributes As Long
End Type

Private Type TOKEN_GROUPS
    GroupCount As Long
    GroupArray() As SID_AND_ATTRIBUTES
End Type

Private Type LUID
  LowPart As Long
  HighPart As Long
End Type

Private Type TOKEN_PRIVILEGES
  PrivilegeCount As Long
  pLuid As LUID
  Attributes As Long
End Type

Private Enum TOKEN_INFORMATION_CLASS
    TokenUser = 1&
    TokenGroups
    TokenPrivileges
    TokenOwner
    TokenPrimaryGroup
    TokenDefaultDacl
    TokenSource
    TokenType
    TokenImpersonationLevel
    TokenStatistics
    TokenRestrictedSids
    TokenSessionId
End Enum

Private Enum SID_NAME_USE
    SidTypeUser = 1&
    SidTypeGroup
    SidTypeDomain
    SidTypeAlias
    SidTypeWellKnownGroup
    SidTypeDeletedAccount
    SidTypeInvalid
    SidTypeUnknown
    SidTypeComputer
End Enum

Private Const TOKEN_ASSIGN_PRIMARY = &H1
Private Const TOKEN_DUPLICATE = &H2
Private Const TOKEN_IMPERSONATE = &H4
Private Const TOKEN_QUERY = &H8
Private Const TOKEN_QUERY_SOURCE = &H10
Private Const TOKEN_ADJUST_PRIVILEGES = &H20
Private Const TOKEN_ADJUST_GROUPS = &H40
Private Const TOKEN_ADJUST_DEFAULT = &H80
Private Const TOKEN_ADJUST_SESSIONID = &H100
Private Const TOKEN_ALL_ACCESS = _
    STANDARD_RIGHTS_REQUIRED Or _
    TOKEN_ASSIGN_PRIMARY Or _
    TOKEN_DUPLICATE Or _
    TOKEN_IMPERSONATE Or _
    TOKEN_QUERY Or _
    TOKEN_QUERY_SOURCE Or _
    TOKEN_ADJUST_PRIVILEGES Or _
    TOKEN_ADJUST_GROUPS Or _
    TOKEN_ADJUST_SESSIONID Or _
    TOKEN_ADJUST_DEFAULT
Private Const TOKEN_READ = _
    STANDARD_RIGHTS_READ Or _
    TOKEN_QUERY
Private Const TOKEN_WRITE = _
    STANDARD_RIGHTS_WRITE Or _
    TOKEN_ADJUST_PRIVILEGES Or _
    TOKEN_ADJUST_GROUPS Or _
    TOKEN_ADJUST_DEFAULT
Private Const TOKEN_EXECUTE = STANDARD_RIGHTS_EXECUTE

Private Declare Function OpenProcessToken Lib "advapi32" ( _
    ByVal ProcessHandle As Long, _
    ByVal DesiredAccess As Long, _
    ByRef TokenHandle As Long) As Long

Private Declare Function OpenThreadToken Lib "advapi32" ( _
    ByVal ThreadHandle As Long, _
    ByVal DesiredAccess As Long, _
    ByVal OpenAsSelf As Long, _
    ByRef TokenHandle As Long) As Long

Private Declare Function GetTokenInformation Lib "advapi32" ( _
    ByVal TokenHandle As Long, _
    ByVal TokenInformationClass As TOKEN_INFORMATION_CLASS, _
    ByRef TokenInformation As Long, _
    ByVal TokenInformationLength As Long, _
    ByRef ReturnLength As Long) As Long

Private Declare Function LookupAccountSidA Lib "advapi32" ( _
    ByVal lpSystemName As String, _
    ByVal Sid As Long, _
    ByVal Name As String, _
    ByRef cbName As Long, _
    ByVal DomainName As String, _
    ByRef cbDomainName As Long, _
    ByRef peUse As SID_NAME_USE) As Long
    
Private Declare Function AdjustTokenPrivileges Lib "advapi32" ( _
    ByVal TokenHandle As Long, _
    ByVal DisableAllPrivileges As Long, _
    ByRef NewState As TOKEN_PRIVILEGES, _
    ByVal BufferLength As Long, _
    ByRef PreviousState As TOKEN_PRIVILEGES, _
    ByRef ReturnLength As Long) As Long

Private Declare Function LookupPrivilegeValueA Lib "advapi32" ( _
  ByVal lpSystemName As String, _
  ByVal lpName As String, _
  ByRef lpLuid As LUID) As Long

Private Const SE_PRIVILEGE_ENABLED = &H2
Private Const SE_SHUTDOWN_NAME = "SeShutdownPrivilege"

Public Sub CheckGroupMembership(strDomainName As String, strGroupName As String, blnMember As Boolean, blnError As Boolean, strMessage As String)
    Dim I As Long
    Dim lngProcId As Long
    Dim lngProcHandle As Long
    Dim lngReturnCode As Long
    Dim lngTokenHandle As Long
    Dim Groups As TOKEN_GROUPS
    Dim lngTokenInfoBuff(10240) As Long
    Dim lngBuffTotalLength As Long
    Dim lngBuffOutLength As Long
    Dim strSidNameRet As String
    Dim lngSidNameSize As Long
    Dim strDomainNameRet As String
    Dim lngDomainNameSize As Long
    Dim lngSidUse As SID_NAME_USE
    Dim lngWin32Error As Long
    Dim lngSidIndex As Long
    Dim lngAttributesIndex As Long
    
    On Error GoTo Win32Error
    
    blnError = False
    blnMember = False
    
    ' Get a handle to the current process using the current ProcessId
    lngProcHandle = GetCurrentProcess()
    
    If lngProcHandle = 0 Then
        GoTo Win32Error
    End If
    
    ' Get a handle to the access token for the current process
    lngReturnCode = OpenProcessToken(lngProcHandle, TOKEN_QUERY, lngTokenHandle)
    
    If lngReturnCode = 0 Then
        GoTo Win32Error
    End If
    
    ' Retrieve group membership for access token
    lngBuffTotalLength = UBound(lngTokenInfoBuff) + 1
    lngReturnCode = GetTokenInformation(lngTokenHandle, TokenGroups, lngTokenInfoBuff(0), lngBuffTotalLength, lngBuffOutLength)
    
    If lngReturnCode = 0 Then
        GoTo Win32Error
    End If
    
    On Error GoTo 0
    
    ' Copy group sids and attributes from buffer into structure for ease of use
    Groups.GroupCount = lngTokenInfoBuff(0)
    ReDim Groups.GroupArray(Groups.GroupCount - 1)
    
    For I = 0 To (Groups.GroupCount - 1)
        lngSidIndex = (I * 2) + 1
        lngAttributesIndex = lngSidIndex + 1
        Groups.GroupArray(I).pSid = lngTokenInfoBuff(lngSidIndex)
        Groups.GroupArray(I).Attributes = lngTokenInfoBuff(lngAttributesIndex)
    Next I
    
    ' Retrive the name of each group and check for a match
    For I = 0 To (Groups.GroupCount - 1)
        strSidNameRet = String(200, Chr(0))
        lngSidNameSize = Len(strSidNameRet) + 1
        strDomainNameRet = String(200, Chr(0))
        lngDomainNameSize = Len(strDomainNameRet) + 1
        lngReturnCode = LookupAccountSidA(Chr(0), Groups.GroupArray(I).pSid, strSidNameRet, lngSidNameSize, strDomainNameRet, lngDomainNameSize, lngSidUse)
        
        If lngReturnCode <> 0 Then
            If Mid(strDomainNameRet, 1, lngDomainNameSize) = strDomainName And _
                Mid(strSidNameRet, 1, lngSidNameSize) = strGroupName And _
                (lngSidUse = SidTypeGroup Or SidTypeAlias Or SidTypeWellKnownGroup) Then
                blnMember = True
                strMessage = strMessage & _
                    "User is a member of '" & strDomainName & "\" & strGroupName & "' group; "
                Exit For
            End If
        End If
    Next I
    
    If Not blnMember Then
        strMessage = strMessage & _
            "User is not a member of '" & strDomainName & "\" & strGroupName & "' group; "
    End If
    
    Exit Sub

Win32Error:
    lngWin32Error = GetLastError()
    blnError = True
    strMessage = strMessage & _
        "CheckGroupMembership failed due to a Win32 Error '" & CStr(GetLastError) & "'; "
    Exit Sub
End Sub
Public Function ReadIni(strIniFileName As String, strSection As String, strKeyName As String) As String
    ' Adapted from code written by David Rehbein of Microsoft Consulting Services
    Dim strDefault As String
    Dim strReturning As String
    Dim intSize As Integer
    Dim lngRetCode As Long
    Dim strReturn As String
    
    On Error Resume Next
       
    ReadIni = STR_NOT_INITIALIZED
    
    strReturning = String(200, Chr(0))
    intSize = Len(strReturning) + 1
    
    lngRetCode = GetPrivateProfileString(strSection, strKeyName, STR_NOT_INITIALIZED, strReturning, Len(strReturning) + 1, strIniFileName)
    
    If Err.Number <> 0 Or (lngRetCode = 0) Then
        ReadIni = STR_NOT_INITIALIZED
        WriteLogMsg goLogFileHandle, "Error reading from INI file " & strIniFileName & ". Error: " & Str(Err.Number) & "; Description: " & Err.Description
        If Err.Number <> 0 Then
            Err.Clear
        End If
    Else
        ReadIni = Left(strReturning, lngRetCode)
    End If
    
    On Error GoTo 0
End Function
Public Sub WriteIni(strIniFileName As String, strSection As String, strKeyName As String, strValue As String, blnError As Boolean)
    ' Adapted from code written by David Rehbein of Microsoft Consulting Services
    Dim lngRetCode As Long
    
    On Error Resume Next
    blnError = False
    
    lngRetCode = WritePrivateProfileString(strSection, strKeyName, strValue, strIniFileName)
    
    If (Err.Number <> 0) Or (lngRetCode = 0) Then
        blnError = True
        WriteLogMsg goLogFileHandle, "Error writing to INI file " & strIniFileName & ". Error: " & Str(Err.Number) & "; Description: " & Err.Description
        If Err.Number <> 0 Then
            Err.Clear
        End If
    End If
    
    On Error GoTo 0
End Sub

Public Sub RunSetupSql(lngDbServerType As DbServerType, strSetupExeFileSpec As String, strSetupIni As String, strMessage As String, blnError As Boolean)
    ' Thanks to contributions from David Rehbein of MCS and Bruce McKinney author of "Hardcore Visual Basic"
    Dim strCommand As String
    Dim lngProcessId As Long
    Dim lngProcessHandle As Long
    Dim lngExitCode As Long
    Dim lngReturnCode As Long
    Dim strIniValue As String
    Dim lngResultCode As Long
    Dim lngRebootRequired As Long
    Dim lngCompleted As Long
    Dim strTemp As String
    Dim intPos As Integer
    
    ' Get rid of existing gstrSetupLogFile and gstrSetupLogDetailFile files
    
    If goFileSystem.FileExists(gstrWinDir & "\" & gstrSetupLogFile) Then
        goFileSystem.DeleteFile (gstrWinDir & "\" & gstrSetupLogFile)
    End If
    
    If goFileSystem.FileExists(gstrWinDir & "\" & gstrSetupLogDetailFile) Then
        goFileSystem.DeleteFile (gstrWinDir & "\" & gstrSetupLogDetailFile)
    End If
    
    ' Construct command line
    strCommand = strSetupExeFileSpec & " " & goDbServers(lngDbServerType).SetupParms & " " & Chr(34) & strSetupIni & Chr(34)
    strMessage = strMessage & _
        "Installation command line used was '" & strCommand & "'; "
    
    Select Case lngDbServerType
        Case SQL_7_SP1_9X_X86, SQL_7_SP2_9X_X86
            ' Insert sa password into command line parameters
            intPos = 0
            intPos = InStr(1, strCommand, "*")
            
            If intPos = 0 Then
                blnError = True
                strMessage = strMessage & _
                    "Unable to insert sa password into command string; "
                Exit Sub
            End If
            
            strTemp = Left(strCommand, intPos - 1)
            strTemp = strTemp & gstrSqlSaPwd
            
            intPos = InStrRev(strCommand, "*")
            
            strTemp = strTemp & Right(strCommand, Len(strCommand) - intPos)
            
            strCommand = strTemp
            
        Case Else
            ' Do nothing
    End Select
    
    ' Run Setup
    lngProcessId = Shell(strCommand, vbHide)
    
    ' Get process handle
    lngProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, lngProcessId)
    
    lngReturnCode = GetExitCodeProcess(lngProcessHandle, lngExitCode)
    Dim dP As dlgProgressBar
    Set dP = New dlgProgressBar
    dP.lMinutes = 8
    dP.Visible = True

    ' Wait While Setup is still running
    Do While lngExitCode = STILL_ACTIVE
        DoEvents
        lngReturnCode = GetExitCodeProcess(lngProcessHandle, lngExitCode)
    Loop
    dP.Visible = False
    Set dP = Nothing
    
    lngReturnCode = CloseHandle(lngProcessHandle)
    
    strMessage = strMessage & _
        "Installation executable terminated with an exit code of '" & CStr(lngExitCode) & "'; "
    
    ' Check Setup.Log to determine if SQL Server or MSDE is properly installed
    If Not goFileSystem.FileExists(gstrWinDir & "\" & gstrSetupLogFile) Then
        blnError = True
        strMessage = strMessage & _
            "Installation is assumed to have failed because the file '" & gstrSetupLogFile & "' could not be located; "
        Exit Sub
    End If
    
    ' Retrieve value of gstrSetupLogCompletedKey
    strIniValue = ReadIni(gstrWinDir & "\" & gstrSetupLogFile, gstrSetupLogStatusSection, gstrSetupLogCompletedKey)
    
    If (strIniValue = STR_NOT_INITIALIZED) Or (Not IsNumeric(strIniValue)) Then
        blnError = True
        strMessage = strMessage & _
            "Installation is assumed to have failed because the key named '" & gstrSetupLogCompletedKey & "' could not be located in '" & gstrSetupLogFile & "'; "
        Exit Sub
    End If
    
    lngCompleted = CLng(strIniValue)
    
    ' Retrieve value of gstrSetupLogRebootRequiredKey
    strIniValue = ReadIni(gstrWinDir & "\" & gstrSetupLogFile, gstrSetupLogStatusSection, gstrSetupLogRebootRequiredKey)
    
    If (strIniValue = STR_NOT_INITIALIZED) Or (Not IsNumeric(strIniValue)) Then
        blnError = True
        strMessage = strMessage & _
            "Installation is assumed to have failed because the key named '" & gstrSetupLogCompletedKey & "' could not be located in '" & gstrSetupLogFile & "'; "
        Exit Sub
    End If
    
    lngRebootRequired = CLng(strIniValue)
    
    ' Retrieve value of gstrSetupLogResultCodeKey
    strIniValue = ReadIni(gstrWinDir & "\" & gstrSetupLogFile, gstrSetupLogResponseResultSection, gstrSetupLogResultCodeKey)
    
    If (strIniValue = STR_NOT_INITIALIZED) Or (Not IsNumeric(strIniValue)) Then
        blnError = True
        strMessage = strMessage & _
            "Installation is assumed to have failed because the key named '" & gstrSetupLogResultCodeKey & "' could not be located in '" & gstrSetupLogFile & "'; "
        Exit Sub
    End If
    
    lngResultCode = CLng(strIniValue)
    
    If (lngResultCode <> 0) Or (lngCompleted <> 1) Then
        blnError = True
        strMessage = strMessage & _
            "Failed installation reported in '" & gstrSetupLogFile & " ', " & _
            "Completed = " & CStr(lngCompleted) & ", " & _
            "ResultCode = " & CStr(lngResultCode) & "; "
    Else
        blnError = False
        
        If lngRebootRequired = 1& Then
            gblnReboot = True
            strMessage = strMessage & _
                "Reboot reccomended; "
        End If
    End If
End Sub

Public Sub CheckWin2kVersion(blnError As Boolean, strMessage As String)
    Dim intRetCode As Integer
    Dim lngTypeMask1 As dwTypeMask
    Dim lngTypeMask2 As dwTypeMask
    Dim lngConditionMask As dwConditionMask
    Dim curConditionMask As Currency ' Note use of currency datatype is a hack to integrate with C DWORDLONG
    Dim OsviRead As OSVERSIONINFOEXA
    Dim OsviCompare As OSVERSIONINFOEXA
    Dim oOSSuite As OSSuite
    
    blnError = False
    
    '
    ' Step 1: Report operating system characteristics and set OS type
    '
    
    ' Initialize Osvi structures
    OsviRead.dwOSVersionInfoSize = Len(OsviRead)
    OsviCompare.dwOSVersionInfoSize = Len(OsviCompare)
    
    ' Retrive version information
    intRetCode = GetVersionEx(OsviRead)
    
    If intRetCode = 0 Then
        glngOsType = OS_NOT_INITIALIZED
        strMessage = strMessage & _
            "Unable to get version information from Windows; "
        Exit Sub
    End If
    
    ' Report Build Number
    strMessage = strMessage & _
        "Windows 2000 Build Number Detected: " & CStr(OsviRead.dwMajorVersion) & "." & _
        CStr(OsviRead.dwMinorVersion) & "." & CStr(OsviRead.dwBuildNumber) & "; "
    
    ' Report Service Pack Information
    If (OsviRead.wServicePackMajor) <> 0 Or (OsviRead.wServicePackMinor) <> 0 Then
        strMessage = strMessage & _
            "Windows 2000 Service Pack Detected: " & CStr(OsviRead.wServicePackMajor) & "." & _
            CStr(OsviRead.wServicePackMinor) & "; "
    End If
    
    ' Report Product Type
    Select Case OsviRead.wProductType
        Case VER_NT_WORKSTATION
            glngOsType = WIN_2K_PRO
            strMessage = strMessage & _
                "Product Type Detected: Windows 2000 Professional; "
        Case VER_NT_DOMAIN_CONTROLLER
            glngOsType = WIN_2K_SRV
            strMessage = strMessage & _
                "Product Type Detected: Windows 2000 Server (primary or backup domain controller); "
        Case VER_NT_SERVER
            glngOsType = WIN_2K_SRV
            strMessage = strMessage & _
                "Product Type Detected: Windows 2000 Server (stand-alone server); "
        Case Else
            glngOsType = OS_NOT_INITIALIZED
            strMessage = strMessage & _
                "Unknown Product Type Detected: " & CStr(OsviRead.wProductType) & "; "
            Exit Sub
    End Select
    
    ' Report Product Suite(s)
           
    For Each oOSSuite In goOSSuites
        ' Build the condition mask
        curConditionMask = 0
        lngTypeMask1 = VER_SUITENAME
        lngConditionMask = VER_OR
        curConditionMask = VerSetConditionMask(curConditionMask, lngTypeMask1, lngConditionMask)
        
        lngTypeMask2 = VER_SUITENAME
        OsviCompare.wSuiteMask = oOSSuite.ID
    
        intRetCode = VerifyVersionInfo(OsviCompare, lngTypeMask2, curConditionMask)
        
        If intRetCode <> 0 Then
            strMessage = strMessage & _
                "Product Suite Detected: " & oOSSuite.SuiteName & "; "
                
            Select Case oOSSuite.ID
                Case VER_SUITE_ENTERPRISE
                    glngOsType = WIN_2K_SRV_ADV
                Case VER_SUITE_DATACENTER
                    glngOsType = WIN_2K_SRV_DC
                Case VER_SUITE_BACKOFFICE, VER_SUITE_COMMUNICATIONS, VER_SUITE_TERMINAL
                    ' Do nothing, these are just components installed on system that are irrelevant
                Case Else
                    ' Leave OS Type set to WIN_2K_SRV
                    glngOsType = OS_NOT_INITIALIZED
                    strMessage = strMessage & _
                        "Unsupported Windows 2000 Product Suite; "
            End Select
        End If
    Next
    
    OsviCompare.wSuiteMask = &H0
    
    '
    ' Step 2: Verify System Requirements
    '
    
    ' Verify Windows 2000 RTM Build
    OsviCompare.dwMajorVersion = 5&
    OsviCompare.dwMinorVersion = 0&
    OsviCompare.dwBuildNumber = 2195&
        
    curConditionMask = 0
    lngTypeMask1 = VER_MAJORVERSION
    lngConditionMask = VER_EQUAL
    curConditionMask = VerSetConditionMask(curConditionMask, lngTypeMask1, lngConditionMask)
    
    lngTypeMask1 = VER_MINORVERSION
    lngConditionMask = VER_EQUAL
    curConditionMask = VerSetConditionMask(curConditionMask, lngTypeMask1, lngConditionMask)
    
    lngTypeMask1 = VER_BUILDNUMBER
    lngConditionMask = VER_GREATER_EQUAL
    curConditionMask = VerSetConditionMask(curConditionMask, lngTypeMask1, lngConditionMask)
    
    lngTypeMask2 = VER_MAJORVERSION Or VER_MINORVERSION Or VER_BUILDNUMBER
    
    intRetCode = VerifyVersionInfo(OsviCompare, lngTypeMask2, curConditionMask)
     
    If intRetCode = 0 Then
        glngOsType = OS_NOT_INITIALIZED
        strMessage = strMessage & _
            "Unsupported Windows 2000 build detected; "
        Exit Sub
    End If
    
    OsviCompare.dwMajorVersion = 0&
    OsviCompare.dwMinorVersion = 0&
    OsviCompare.dwBuildNumber = 0&
End Sub

Sub ShowConnectDialog(lngFormHandle As Long)
    Dim lngRetValue
    
    On Error Resume Next
    
    lngRetValue = WNetConnectionDialog(lngFormHandle, RESOURCETYPE_DISK)
    
    If Err.Number <> 0 Then
        Err.Clear
    End If
    
    On Error GoTo 0
End Sub

Public Sub VerifyInstallPackage(strInstPkgFileSpec As String, blnVerified As Boolean, strMessage As String)
    Dim oInstPkgFile As Object
    Dim strCommand As String
    Dim lngProcessId As Long
    Dim lngProcessHandle As Long
    Dim lngReturnCode As Long
    Dim lngExitCode As Long
        
    '
    ' Step 1: Use ChkTrust.EXE CryptoAPI tool to check the validity of the installation package
    '
    
    If Not goFileSystem.FileExists(strInstPkgFileSpec) Then
        blnVerified = False
        strMessage = strMessage & _
            "Unable to locate file '" & strInstPkgFileSpec & "'; "
        Exit Sub
    End If
    
    strCommand = """" & App.Path & "\CHKTRUST.EXE"" ""-q"" """ & strInstPkgFileSpec & """"
    
    ' Run Setup
    
    strMessage = strMessage & _
        "Running ChkTrust.exe with the following command line: " & strCommand & "; "
        
    lngProcessId = Shell(strCommand, vbHide)
    
    ' Get process handle
    lngProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, lngProcessId)
    
    lngReturnCode = GetExitCodeProcess(lngProcessHandle, lngExitCode)

    ' Wait While Setup is still running
    Do While lngExitCode = STILL_ACTIVE
        DoEvents
        lngReturnCode = GetExitCodeProcess(lngProcessHandle, lngExitCode)
    Loop

    lngReturnCode = CloseHandle(lngProcessHandle)
    
    strMessage = strMessage & _
        "ChkTrust.EXE terminated with an exit code of '" & CStr(lngExitCode) & "'; "
        
    If lngExitCode = 0 Then
        blnVerified = True
        strMessage = strMessage & _
            "Trust verified for file '" & strInstPkgFileSpec & "'; "
    Else
        blnVerified = False
        strMessage = strMessage & _
            "Trust not verified for file '" & strInstPkgFileSpec & "'; "
    End If
End Sub

Public Sub RebootSystem(blnError As Boolean, strMessage As String)
    Dim lngRetCode As Long
    Dim lngFlag As Long
    Dim lngUnused As Long
    Dim lngError As Long
    Dim lngProcHandle As Long
    Dim lngTokenHandle As Long
    Dim strSystemName As String
    Dim strPrivelegeName As String
    Dim LuidStruct As LUID
    Dim TokenPrivStruct As TOKEN_PRIVILEGES
    Dim UnusedTokenPrivStruct As TOKEN_PRIVILEGES
    Dim lngBufferNeeded As Long
    Dim lngWin32Error As Long
    
    ' On Error GoTo Win32Error
    
    If glngOsType <> WIN_95_98 Then
        ' Get a handle to the current Process
        lngProcHandle = GetCurrentProcess()
        
        If lngProcHandle = 0 Then
            GoTo Win32Error
        End If
        
        ' Get a handle to the access token for the current process
        lngRetCode = OpenProcessToken(lngProcHandle, (TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY), lngTokenHandle)
        
        If lngRetCode = 0 Then
            GoTo Win32Error
        End If
        
        ' Lookup the LUID for the SE_SHUTDOWN_NAME privelege
        lngRetCode = LookupPrivilegeValueA("", "SeShutdownPrivilege", LuidStruct)
        
        If lngRetCode = 0 Then
            GoTo Win32Error
        End If
        
        ' Build the TOKEN_PRIVELEGES structure
        With TokenPrivStruct
            .PrivilegeCount = 1
            .pLuid = LuidStruct
            .Attributes = SE_PRIVILEGE_ENABLED
        End With
                
        ' Adjust priveleges
        lngRetCode = AdjustTokenPrivileges(lngTokenHandle, False, TokenPrivStruct, Len(UnusedTokenPrivStruct), UnusedTokenPrivStruct, lngBufferNeeded)
        
        If lngRetCode = 0 Then
            GoTo Win32Error
        End If
    End If
    
    lngFlag = EWX_RESET
    lngRetCode = ExitWindowsEx((EWX_SHUTDOWN Or EWX_FORCE Or EWX_REBOOT), &HFFFF)
    
    If lngRetCode = 0 Then
        GoTo Win32Error
    Else
        blnError = False
        strMessage = strMessage & _
            "Restarting system; "
    End If

    Exit Sub
    
Win32Error:
    lngWin32Error = GetLastError()
    blnError = True
    strMessage = strMessage & _
        "RebootSystem failed due to a Win32 Error '" & CStr(GetLastError) & "'; "
    Exit Sub
End Sub

Public Sub RunProgramAndWait(strCommand, lngMinutes)

Dim lngProcessId As Long, lngExitCode As Long
Dim lngProcessHandle As Long, lngReturnCode As Long

' Run Setup
lngProcessId = Shell(strCommand, vbHide)
    
' Get process handle
lngProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, lngProcessId)
lngReturnCode = GetExitCodeProcess(lngProcessHandle, lngExitCode)
Dim dP As dlgProgressBar
Set dP = New dlgProgressBar
dP.lMinutes = lngMinutes
dP.Visible = True

' Wait While Setup is still running
Do While lngExitCode = STILL_ACTIVE
  DoEvents
  lngReturnCode = GetExitCodeProcess(lngProcessHandle, lngExitCode)
Loop
dP.Visible = False
Set dP = Nothing
    
lngReturnCode = CloseHandle(lngProcessHandle)

End Sub
