Attribute VB_Name = "modUpdate"

Option Explicit
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright ©1996-2000 VBnet, Randy Birch, All Rights Reserved.
' Some pages may also contain other copyrights by the author.
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' You are free to use this code within your own applications,
' but you are expressly forbidden from selling or otherwise
' distributing this source code without prior written consent.
' This includes both posting free demo projects made from this
' code as well as reproducing the code in text or html format.
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public fs As Scripting.FileSystemObject

Public Type SHFILEOPSTRUCT
   hWnd        As Long
   wFunc       As Long
   pFrom       As String
   pTo         As String
   fFlags      As Integer
   fAborted    As Boolean
   hNameMaps   As Long
   sProgress   As String
 End Type
  
Public Const FO_MOVE As Long = &H1
Public Const FO_COPY As Long = &H2
Public Const FO_DELETE As Long = &H3
Public Const FO_RENAME As Long = &H4

Public Const FOF_SILENT As Long = &H4
Public Const FOF_RENAMEONCOLLISION As Long = &H8
Public Const FOF_NOCONFIRMATION As Long = &H10
Public Const FOF_SIMPLEPROGRESS As Long = &H100
Public Const FOF_ALLOWUNDO As Long = &H40

Public Declare Function GetTempPath _
     Lib "Kernel32" Alias "GetTempPathA" _
    (ByVal nSize As Long, ByVal lpBuffer As String) As Long

Public Declare Function SHFileOperation _
    Lib "shell32.dll" Alias "SHFileOperationA" _
    (lpFileOp As SHFILEOPSTRUCT) As Long
  
'we'll use Brad's Browse For Folders Dialog code to
'enable the user to pick the source and destination folders.

Public Declare Function SHGetPathFromIDList _
    Lib "shell32.dll" Alias "SHGetPathFromIDListA" _
    (ByVal pidl As Long, ByVal pszPath As String) As Long
       
Public Declare Function SHGetSpecialFolderLocation _
    Lib "shell32.dll" _
    (ByVal hWndOwner As Long, _
     ByVal nFolder As Long, _
     pidl As Long) As Long
   
Public Declare Function SHBrowseForFolder _
    Lib "shell32.dll" Alias "SHBrowseForFolderA" _
    (lpBrowseInfo As BROWSEINFO) As Long
   
Public Type BROWSEINFO
   hOwner           As Long
   pidlRoot         As Long
   pszDisplayName   As String
   lpszTitle        As String
   ulFlags          As Long
   lpfn             As Long
   lParam           As Long
   iImage           As Long
End Type
   
Public Const ERROR_SUCCESS As Long = 0
Public Const CSIDL_DESKTOP As Long = &H0
Public Const BIF_RETURNONLYFSDIRS As Long = &H1
Public Const BIF_STATUSTEXT As Long = &H4
Public Const BIF_RETURNFSANCESTORS As Long = &H8
'--end block--'
Public Const OFN_ALLOWMULTISELECT As Long = &H200
Public Const OFN_CREATEPROMPT As Long = &H2000
Public Const OFN_ENABLEHOOK As Long = &H20
Public Const OFN_ENABLETEMPLATE As Long = &H40
Public Const OFN_ENABLETEMPLATEHANDLE As Long = &H80
Public Const OFN_EXPLORER As Long = &H80000
Public Const OFN_EXTENSIONDIFFERENT As Long = &H400
Public Const OFN_FILEMUSTEXIST As Long = &H1000
Public Const OFN_HIDEREADONLY As Long = &H4
Public Const OFN_LONGNAMES As Long = &H200000
Public Const OFN_NOCHANGEDIR As Long = &H8
Public Const OFN_NODEREFERENCELINKS As Long = &H100000
Public Const OFN_NOLONGNAMES As Long = &H40000
Public Const OFN_NONETWORKBUTTON As Long = &H20000
Public Const OFN_NOREADONLYRETURN As Long = &H8000
Public Const OFN_NOTESTFILECREATE As Long = &H10000
Public Const OFN_NOVALIDATE As Long = &H100
Public Const OFN_OVERWRITEPROMPT As Long = &H2
Public Const OFN_PATHMUSTEXIST As Long = &H800
Public Const OFN_READONLY As Long = &H1
Public Const OFN_SHAREAWARE As Long = &H4000
Public Const OFN_SHAREFALLTHROUGH As Long = 2
Public Const OFN_SHAREWARN As Long = 0
Public Const OFN_SHARENOWARN As Long = 1
Public Const OFN_SHOWHELP As Long = &H10
Public Const OFS_MAXPATHNAME As Long = 260

'OFS_FILE_OPEN_FLAGS and OFS_FILE_SAVE_FLAGS below
'are mine to save long statements; they're not
'a standard Win32 type.
Public Const OFS_FILE_OPEN_FLAGS = OFN_EXPLORER _
             Or OFN_LONGNAMES _
             Or OFN_CREATEPROMPT _
             Or OFN_NODEREFERENCELINKS

Public Const OFS_FILE_SAVE_FLAGS = OFN_EXPLORER _
             Or OFN_LONGNAMES _
             Or OFN_OVERWRITEPROMPT _
             Or OFN_HIDEREADONLY

Public Type OPENFILENAME
  nStructSize       As Long
  hWndOwner         As Long
  hInstance         As Long
  sFilter           As String
  sCustomFilter     As String
  nMaxCustFilter    As Long
  nFilterIndex      As Long
  sFile             As String
  nMaxFile          As Long
  sFileTitle        As String
  nMaxTitle         As Long
  sInitialDir       As String
  sDialogTitle      As String
  Flags             As Long
  nFileOffset       As Integer
  nFileExtension    As Integer
  sDefFileExt       As String
  nCustData         As Long
  fnHook            As Long
  sTemplateName     As String
End Type

Public OFN As OPENFILENAME

Public Declare Function GetOpenFileName Lib "comdlg32.dll" _
    Alias "GetOpenFileNameA" _
   (pOpenfilename As OPENFILENAME) As Long

Public Declare Function GetSaveFileName Lib "comdlg32.dll" _
   Alias "GetSaveFileNameA" _
  (pOpenfilename As OPENFILENAME) As Long

Public Declare Function GetShortPathName Lib "Kernel32" _
    Alias "GetShortPathNameA" _
   (ByVal lpszLongPath As String, _
    ByVal lpszShortPath As String, _
    ByVal cchBuffer As Long) As Long

Private Declare Function GetDesktopWindow Lib "User32" () As Long

Private Declare Function ShellExecute Lib "shell32.dll" _
    Alias "ShellExecuteA" _
   (ByVal hWnd As Long, ByVal lpOperation As String, _
    ByVal lpFile As String, ByVal lpParameters As String, _
    ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long
    
Private Const SW_SHOWNORMAL As Long = 1
Private Const SW_SHOWMAXIMIZED As Long = 3
Private Const SW_SHOWDEFAULT As Long = 10
Private Const SE_ERR_NOASSOC As Long = 31

Public Function CopySourceToDest(strSource As String, strDest As String) As Long

   Dim FOF_FLAGS As Long
   Dim SHFileOp As SHFILEOPSTRUCT
   
  'terminate the folder string with a pair of nulls
   strSource = strSource & Chr$(0) & Chr$(0)
  
  'determine the user's options selected
   FOF_FLAGS = FOF_NOCONFIRMATION
  
  'set up the options
   With SHFileOp
      .wFunc = FO_COPY
      .pFrom = strSource
      .pTo = strDest
      .fFlags = FOF_FLAGS
   End With
  
  'and perform the chosen copy or move operation
   CopySourceToDest = SHFileOperation(SHFileOp)

End Function

Public Function GetBrowseFolder(msg As String, hWnd As Long) As String

   Dim pidl As Long
   Dim pos As Integer
   Dim path As String
   Dim bi As BROWSEINFO
  
  'Fill the BROWSEINFO structure with the needed data,
  'show the browse dialog, and if the returned value
  'indicates success (1), retrieve the user's
  'selection contained in pidl
   With bi
      .hOwner = hWnd
      .pidlRoot = CSIDL_DESKTOP
      .lpszTitle = msg
      .ulFlags = BIF_RETURNONLYFSDIRS
   End With

   pidl = SHBrowseForFolder(bi)
 
   path = Space$(512)
     
   If SHGetPathFromIDList(ByVal pidl, ByVal path) = 1 Then
      pos = InStr(path, Chr$(0))
      GetBrowseFolder = Left(path, pos - 1)
   End If

End Function

Public Function FileOpenDialog(strTitle As String, strStartIn As String, hWnd As Long) As String

  'used in call setup
   Dim sFilters As String
   
  'used after call
   Dim pos As Long
   Dim buff As String
   Dim sLongname As String
   Dim sShortname As String

  'create a string of filters for the dialog
   sFilters = "All Files" & vbNullChar & "*.*" & vbNullChar & vbNullChar
 
 
   With OFN
      'size of the OFN structure
      .nStructSize = Len(OFN)
      'window owning the dialog
      .hWndOwner = hWnd
      'filters (patterns) for the dropdown combo
      .sFilter = sFilters
      'index to the initial filter
      .nFilterIndex = 0
      'default filename, plus additional padding
      'for the user's final selection(s). Must be
      'double-null terminated
      .sFile = vbNullChar & vbNullChar
      'the size of the buffer
      .nMaxFile = Len(.sFile)
      'default extension applied to
      'file if it has no extention
      .sDefFileExt = vbNullChar & vbNullChar
      'space for the file title if a single selection
      'made, double-null terminated, and its size
      .sFileTitle = vbNullChar & Space$(512) & vbNullChar & vbNullChar
      .nMaxTitle = Len(OFN.sFileTitle)
      'starting folder, double-null terminated
      .sInitialDir = strStartIn & vbNullChar & vbNullChar
      'the dialog title
      .sDialogTitle = strTitle
      'default open flags
      .Flags = OFS_FILE_OPEN_FLAGS
   End With
   
   
  'call the API
   If GetOpenFileName(OFN) = 0 Then
    
     'remove trailing pair of terminating nulls
     'and trim returned file string
      buff = Trim$(Left$(OFN.sFile, Len(OFN.sFile) - 2))
    
      FileOpenDialog = OFN.sFile
      
  End If

End Function

Public Function OpenURL(pstrURL As String) As Boolean

Dim hWndDesk As Long
Dim success As Long

OpenURL = False

'the desktop will be the
'default for error messages
hWndDesk = GetDesktopWindow()

'execute the passed operation
success = ShellExecute(hWndDesk, "Open", pstrURL, 0&, 0&, SW_SHOWNORMAL)

OpenURL = True

End Function

Public Function ClearReadOnly(pstrFileName As String)

Dim oFile As File

Set oFile = fs.GetFile(pstrFileName)
If oFile.Attributes And ReadOnly Then
  oFile.Attributes = oFile.Attributes - ReadOnly
End If

End Function
