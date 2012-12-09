Attribute VB_Name = "modGetFolderName"
'***************************************************************
'Windows API/Global Declarations for: Implementing the Browse For
'     Folders Dialog
'***************************************************************
Option Explicit

Private Const BIF_RETURNONLYFSDIRS = 1
Private Const BIF_DONTGOBELOWDOMAIN = 2
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

Public Function GetFolderName(sFolderName As String, sMsg As String, f As Form) As Boolean

Dim lpIDList As Long
Dim sBuffer As String
Dim tBrowseInfo As BrowseInfo

With tBrowseInfo
  'Fill the BROWSEINFO structure with the needed data
  .hwndOwner = frmInfo.hWnd
  'title message to be displayed in the Browse dialog.
  .lpszTitle = lstrcat(sMsg, "")
  'the type of folder to return.
  .ulFlags = BIF_RETURNONLYFSDIRS + BIF_DONTGOBELOWDOMAIN
End With

'show the browse folder dialog
lpIDList = SHBrowseForFolder(tBrowseInfo)

If (lpIDList) Then
  sBuffer = Space(MAX_PATH)
  SHGetPathFromIDList lpIDList, sBuffer
  sFolderName = Left(sBuffer, InStr(sBuffer, vbNullChar) - 1)
  GetFolderName = True
Else
  GetFolderName = False
End If
      
End Function



