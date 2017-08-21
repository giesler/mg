VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "CShellFolders"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'--------------------------------------------------------------------------------------------
'   Name:           CShellFolders (CSHELLFOLDERS.CLS)
'   Type:           API wrapper class
'   Description:    Simple class that returns filesystem locations of special folders
'                   within the shell
'
'   Author:         Klaus H. Probst [kprobst@vbbox.com]
'   URL:            http://www.vbbox.com/
'   Copyright:      This work is copyright © 1998-2001, Klaus H. Probst
'   Usage:          You may use this code as you see fit, provided that you assume all
'                   responsibilities for doing so.
'   Distribution:   If you intend to distribute the file(s) that make up this sample to
'                   any WWW site, online service, electronic bulletin board system (BBS),
'                   CD or any other electronic or physical media, you must notify me in
'                   advance to obtain my express permission.
'
'   Dependencies:
'
'       (none)      None if you use local declares. Otherwise add a module with the pertinent
'                   ones.
'
'
'--------------------------------------------------------------------------------------------
Option Explicit
DefLng A-Z

'// NOTE: Set this to something lower than 471 if your target machine does not
'// have the 4.71 version of SHELL32. Otherwise nothing will work and you'll get
'// an "entry point not found" error in the GetFolderLocation() method.
#Const IE_VERSION = 471

Public Enum cbSHSpecialFolders
    CSIDL_DESKTOP = &H0
    CSIDL_INTERNET = &H1
    CSIDL_PROGRAMS = &H2
    CSIDL_CONTROLS = &H3
    CSIDL_PRINTERS = &H4
    CSIDL_PERSONAL = &H5
    CSIDL_FAVORITES = &H6
    CSIDL_STARTUP = &H7
    CSIDL_RECENT = &H8
    CSIDL_SENDTO = &H9
    CSIDL_BITBUCKET = &HA
    CSIDL_STARTMENU = &HB
    CSIDL_DESKTOPDIRECTORY = &H10
    CSIDL_DRIVES = &H11
    CSIDL_NETWORK = &H12
    CSIDL_NETHOOD = &H13
    CSIDL_FONTS = &H14
    CSIDL_TEMPLATES = &H15
    CSIDL_COMMON_STARTMENU = &H16
    CSIDL_COMMON_PROGRAMS = &H17
    CSIDL_COMMON_STARTUP = &H18
    CSIDL_COMMON_DESKTOPDIRECTORY = &H19
    CSIDL_APPDATA = &H1A
    CSIDL_PRINTHOOD = &H1B
    CSIDL_ALTSTARTUP = &H1D                         '// DBCS
    CSIDL_COMMON_ALTSTARTUP = &H1E                  '// DBCS
    CSIDL_COMMON_FAVORITES = &H1F
    CSIDL_INTERNET_CACHE = &H20
    CSIDL_COOKIES = &H21
    CSIDL_HISTORY = &H22
End Enum

#If Not CB_NO_LOCAL_SYMBOLS Then

'// See? This is why it's good to upgrade =)
#If IE_VERSION < 471 Then
Private Declare Function SHGetSpecialFolderLocation Lib "shell32" (ByVal hwndOwner As Long, ByVal nFolder As Long, ByRef pidl As Long) As Long
Private Declare Function SHGetPathFromIDList Lib "shell32.dll" Alias "SHGetPathFromIDListA" (ByVal pidl As Long, ByVal pszPath As String) As Long
Private Declare Sub CoTaskMemFree Lib "OLE32.DLL" (ByVal pv As Long)
#Else
Private Declare Function SHGetSpecialFolderPath Lib "shell32" Alias "SHGetSpecialFolderPathA" (ByVal hwndOwner As Long, ByVal lpszPath As String, ByVal nFolder As Long, ByVal fCreate As Long) As Long
#End If

#End If

' ******************************************************************************
' Routine:            Function GetFolderLocation
' Purpose:            Returns the path to the specified shell folder
' Arguments:
'                     [in] Folder
'                     Enumerated ID of the required folder
'
' Returns:            The string with the path to the folder, or an empty string
'                     if an error ocurred.
' Notes:
'
' ******************************************************************************
Public Function GetFolderLocation(ByVal Folder As cbSHSpecialFolders) As String

#If (IE_VERSION < 471) Then
    
    Dim sBuffer As String
    Dim pidl As Long
    
    sBuffer = String$(260, 0)
    
    If (SHGetSpecialFolderLocation(0, Folder, pidl) = 0) Then
        If (SHGetPathFromIDList(pidl, sBuffer) <> 0) Then
            sBuffer = Left$(sBuffer, InStr(sBuffer, Chr$(0)) - 1)
            GetFolderLocation = sBuffer
        End If
        Call CoTaskMemFree(pidl)
    End If
    
#Else
    
    Dim sBuffer As String
    
    sBuffer = String$(260, 0)
    
    If (SHGetSpecialFolderPath(0, sBuffer, Folder, 0) <> 0) Then
        sBuffer = Left$(sBuffer, InStr(sBuffer, Chr$(0)) - 1)
        GetFolderLocation = sBuffer
    
    End If

#End If

End Function




