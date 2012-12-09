Attribute VB_Name = "modWin32"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' modWin32.bas module isolates all Win32 related procedures from rest of application '
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit

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

' WNet Function Declarations
Private Declare Function WNetConnectionDialog Lib "mpr.dll" ( _
    ByVal hwnd As Long, _
    ByVal dwType As Long) As Long

Private Const RESOURCETYPE_DISK = &H1

Public Function ReadIni(strIniFileName As String, strSection As String, strKeyName As String) As String
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

Public Sub CheckWin2kVersion(blnError As Boolean, strMessage As String)
    Dim intRetCode As Integer
    Dim lngTypeMask1 As dwTypeMask
    Dim lngTypeMask2 As dwTypeMask
    Dim lngConditionMask As dwConditionMask
    Dim curConditionMask As Currency ' Note use of currency datatype is a hack to integrate with C DWORDLONG
    Dim OsviRead As OSVERSIONINFOEXA
    Dim OsviCompare As OSVERSIONINFOEXA
    
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
    
    OsviCompare.wSuiteMask = &H0
    
    '
    ' Step 2: Verify System Requirements
    '
    
    ' Verify Windows 2000 RC2 Build or Later
    OsviCompare.dwMajorVersion = 5&
    OsviCompare.dwMinorVersion = 0&
    OsviCompare.dwBuildNumber = 2128&
        
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

