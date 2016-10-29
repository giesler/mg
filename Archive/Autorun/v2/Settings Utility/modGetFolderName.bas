Attribute VB_Name = "modGetFolderName"
'***************************************************************
'Windows API/Global Declarations for: Implementing the Browse For
'     Folders Dialog
'***************************************************************
Option Explicit

' consts are in shlobj.h
Private Const BIF_RETURNONLYFSDIRS = &H1
Private Const BIF_DONTGOBELOWDOMAIN = &H2
Private Const BIF_NEWDIALOGSTYLE = &H40
Private Const BIF_BROWSEINCLUDEFILES = &H4000
Private Const BIF_EDITBOX = &H10
Private Const BIF_RETURNFSANCESTORS = &H8
Private Const MAX_PATH = 260

Private Declare Function SHBrowseForFolder Lib "shell32" (lpbi As BrowseInfo) As Long
Private Declare Function SHGetPathFromIDList Lib "shell32" (ByVal pidList As Long, ByVal lpBuffer As String) As Long
Private Declare Function lstrcat Lib "kernel32" Alias "lstrcatA" (ByVal lpString1 As String, ByVal lpString2 As String) As Long
Private Type BrowseInfo
  hwndOwner      As Long
  pIDLRoot       As Long
  pszDisplayName As Long
  lpszTitle      As Long
  ulFlags        As Long
  lpfnCallback   As Long
  lParam         As Long
  iImage         As Long
End Type

Public Function GetFolderName(lpIDList As Long, sMsg As String, f As Form) As String

Dim sBuffer As String
Dim tBrowseInfo As BrowseInfo

With tBrowseInfo
  'Fill the BROWSEINFO structure with the needed data
  .hwndOwner = f.hwnd
  'title message to be displayed in the Browse dialog.
  .lpszTitle = lstrcat(sMsg, "")
  'the type of folder to return.
  .ulFlags = BIF_RETURNONLYFSDIRS Or BIF_NEWDIALOGSTYLE Or BIF_EDITBOX
End With

'show the browse folder dialog
lpIDList = SHBrowseForFolder(tBrowseInfo)

If (lpIDList) Then
  sBuffer = Space(MAX_PATH)
  SHGetPathFromIDList lpIDList, sBuffer
  GetFolderName = Left(sBuffer, InStr(sBuffer, vbNullChar) - 1)
Else
  GetFolderName = ""
End If
      
End Function

Public Function GetFileName(lpIDList As Long, sMsg As String, f As Form) As String

Dim sBuffer As String
Dim tBrowseInfo As BrowseInfo

With tBrowseInfo
  'Fill the BROWSEINFO structure with the needed data
  .hwndOwner = f.hwnd
  'title message to be displayed in the Browse dialog.
  .lpszTitle = lstrcat(sMsg, "")
  'the type of folder to return.
' changed  .ulFlags = BIF_BROWSEINCLUDEFILES Or BIF_RETURNONLYFSDIRS _
'    Or BIF_EDITBOX Or BIF_RETURNFSANCESTORS Or BIF_NEWDIALOGSTYLE
  .ulFlags = BIF_BROWSEINCLUDEFILES 'Or BIF_RETURNFSANCESTORS Or BIF_NEWDIALOGSTYLE '_
'    Or BIF_EDITBOX Or BIF_RETURNFSANCESTORS Or BIF_NEWDIALOGSTYLE
  'the root
  .pIDLRoot = lpIDList
End With

'show the browse folder dialog
lpIDList = SHBrowseForFolder(tBrowseInfo)

If (lpIDList) Then
  sBuffer = Space(MAX_PATH)
  SHGetPathFromIDList lpIDList, sBuffer
  GetFileName = Left(sBuffer, InStr(sBuffer, vbNullChar) - 1)
Else
  GetFileName = ""
End If

End Function

'-----------------------------------------------------------
' FUNCTION: FileExists
' Determines whether the specified file exists
'
' IN: [strPathName] - file to check for
'
' Returns: True if file exists, False otherwise
'-----------------------------------------------------------
'
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
