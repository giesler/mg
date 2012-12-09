Attribute VB_Name = "modUpdate"
Option Explicit

'Function declares
Declare Function GetCurrentProcessId Lib "kernel32" () As Long
Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal _
      hHandle As Long, ByVal dwMilliseconds As Long) As Long
Private Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long

Public Const gstrSEP_URLDIR$ = "/"                      ' Separator for dividing directories in URL addresses.
Public Const gstrSEP_DIR$ = "\"                         ' Directory separator character

'Global variable declares
Dim strDBDir As String
Dim intProcessID As Long
Dim strAccessDir As String
Dim strDBName As String
Dim flgMscomOnly As Boolean
Dim flgForceMscom As Boolean

Dim strUpdateFileDir As String
Dim fnTable() As String
Dim numFiles As Integer
Dim lngFileSizeTotal As Long
Dim lngFileSizeCopied As Long
Dim fu As frmUpdate

'Const Declarations
'Const ctNEWFILE = "\TPro.mde"
'Const ctCURFILE = "TargetPRO.mde"
'Const ctDBTITLE = "Access"
Private Const INFINITE = -1&        'used for waitshell function

' Reboot system code
Public Const EWX_REBOOT = 2
Public Declare Function ExitWindowsEx Lib "user32" (ByVal uFlags As Long, ByVal dwReserved As Long) As Long

Private Type OSVERSIONINFO 'for GetVersionEx API call
    dwOSVersionInfoSize As Long
    dwMajorVersion As Long
    dwMinorVersion As Long
    dwBuildNumber As Long
    dwPlatformId As Long
    szCSDVersion As String * 128
End Type

Private Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Private Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (lpVersionInformation As OSVERSIONINFO) As Long

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

Public Sub Main()
On Error GoTo Main_Err

'Check and split up command line
If Not (ParseCommandLine) Then End

'Read the list of files to update
If Not LoadSettings Then
  MsgBox "There was an error loading the settings file.", vbExclamation
  End
End If

'If we should just install mscomctl, then do that and end
If flgMscomOnly Then
  MscomInstall
End If

'Check for MSCOM control
Dim fC As New frmCCTest
fC.Visible = False
If Err <> 0 Then
  If InStr(Err.Description, "MSCOMCTL") > 0 Then
    MscomInstall
    End
  Else
    ErrHand "modUpdate", "Main"
    End
  End If
End If
Set fC = Nothing

Set fu = New frmUpdate
fu.pbar.Value = 0
fu.lblStatus = "Loading..."
fu.Visible = True

'Wait for access to close if there is a PID in command line
If intProcessID Then
  WaitForProcess (intProcessID)
End If
  
'Copy the file
If Not CopyFiles Then
  MsgBox "The file copy failed, so this program has been aborted.", vbCritical
  End
End If

'Check for mscomctl
On Error Resume Next
If flgForceMscom Then
  MscomInstall
End If

'Launch Access
If strAccessDir = "" Then
  fu.lblStatus.Caption = "The files have been updated."
  fu.Timer1.Interval = 5000
  Exit Sub
Else
  RunAccess
End If

End

Exit Sub

Main_Err:
If InStr(Err.Description, "MSCOMCTL") > 0 Then
  MscomInstall
Else
  ErrHand "modUpdate", "Main"
End If
Exit Sub

End Sub

'Splits the command line received, assuming it is:
' /d:'<directory path>' [/i:<process id>]
' Returns TRUE if success, FALSE otherwise
'
Public Function ParseCommandLine() As Integer

Dim intDirStart As Integer, intDirEnd As Integer
Dim intPIDStart As Integer, intPIDEnd As Integer
Dim intAcsStart As Integer, intAcsEnd As Integer
Dim intDBNStart As Integer, intDBNEnd As Integer
Dim strModCmd As String

ParseCommandLine = False

If InStr(Command, "/mscomctl") Then
  flgMscomOnly = True
  ParseCommandLine = True
  Exit Function
End If

If Len(Command) = 0 Or InStr(Command, "/?") > 0 Then
  Dim fI As New frmInfo
  Pause fI
  If fI.blnCancel Then Exit Function
  flgForceMscom = fI.chkInstallMSCom
  If flgForceMscom And fI.chkRunDBUpdate <> 1 Then
    ParseCommandLine = True
    flgMscomOnly = True
    Exit Function
  End If
  If fI.chkRunDBUpdate Then
    If Not GetFolderName(strDBDir, "Choose the local database directory", fu) Then
      Exit Function
    End If
  End If
  strModCmd = "/d:'" & strDBDir & "'"
Else
  strModCmd = Command
End If

'Figure out directory from command line
intDirStart = InStr(strModCmd, "/d:'") + 4
If intDirStart = 0 Then Exit Function
intDirEnd = InStr(intDirStart + 1, strModCmd, "'")
If intDirEnd = 0 Then intPIDEnd = Len(strModCmd) + 1
strDBDir = Trim(Mid$(strModCmd, intDirStart, intDirEnd - intDirStart))
If Right$(strDBDir, 1) <> "\" Then strDBDir = strDBDir & "\"

'Figure out Access directory from command line
intAcsStart = InStr(strModCmd, "/a:'") + 4
If intAcsStart = 4 Then
  strAccessDir = ""
Else
  intAcsEnd = InStr(intAcsStart + 1, strModCmd, "'")
  If intAcsEnd = 0 Then intPIDEnd = Len(strModCmd) + 1
  strAccessDir = Trim(Mid$(strModCmd, intAcsStart, intAcsEnd - intAcsStart))
End If

'Figure out database name from command line
intDBNStart = InStr(strModCmd, "/n:'") + 4
If intDBNStart = 4 Then
  strDBName = ""
Else
  intDBNEnd = InStr(intDBNStart + 1, strModCmd, "'")
  strDBName = Trim(Mid$(strModCmd, intDBNStart, intDBNEnd - intDBNStart))
End If

'Figure out process ID if there is one
intPIDStart = InStr(strModCmd, "/i:") + 3
If intPIDStart <> 0 Then
  intPIDEnd = InStr(intPIDStart + 1, strModCmd, " ")
  If intPIDEnd = 0 Then intPIDEnd = Len(strModCmd) + 1
  intProcessID = Val(Mid$(strModCmd, intPIDStart, intPIDEnd - intPIDStart))
Else
  intProcessID = 0
End If
ParseCommandLine = True

End Function

'
'Waits for the process specified by the passed value
' to close or a long time, then continues execution.
'
Public Sub WaitForProcess(intID As Long)
On Error Resume Next
Dim intTEMP As Long

fu.Show
fu.Refresh
For intTEMP = 1 To 10000
  On Error Resume Next
  Wait "Waiting for database to close...", 1
  AppActivate intID, False
  If Err = 5 Then Exit Sub
Next intTEMP
Exit Sub
Wait "Waiting for database to close...", 13

End Sub

Public Sub RunAccess()
On Error GoTo RunAccessErr
Dim retVal As Long

fu.lblStatus.Caption = "Restarting database..."
fu.Refresh

Dim txtLaunch As String

txtLaunch = strAccessDir & "msaccess.exe " & Chr$(34) & strDBDir & strDBName & Chr$(34) & " /cmd dbupdate"
retVal = Shell(txtLaunch, vbMaximizedFocus)
fu.Timer1.Interval = 4000

Exit Sub
RunAccessErr:
ErrHand "modUpdate", "RunAccess"
Exit Sub

End Sub

Public Function LoadSettings() As Boolean
On Error GoTo LoadSettings_Err

LoadSettings = False

If Not FileExists(App.Path & "\update.ini") Then
  MsgBox "The required INI file '" & App.Path & "\update.ini' was not found.", vbExclamation
  Exit Function
End If

' Get source directory
PrivIniRegister "General", App.Path & "\update.ini"
strUpdateFileDir = PrivGetString("SRCPath", "f:\database")

' Get file names
PrivIniRegister "Files", App.Path & "\update.ini"
numFiles = PrivGetSectEntriesEx(fnTable())

LoadSettings = True

Exit Function
LoadSettings_Err:
ErrHand "modUpdate", "LoadSettings"
Exit Function

End Function

Public Function CopyFiles() As Boolean
On Error GoTo CopyFiles_Err

Dim i As Integer, strFile As String
With fu
  .lblStatus.Caption = "Updating files..."
  .pbar.Value = 0
  .Show
  .Refresh
End With
 
For i = 0 To numFiles - 1
  strFile = strUpdateFileDir & "\" & fnTable(1, i)
  If Not FileExists(strFile) Then
    MsgBox "One of the required files, '" & strFile & "' was not found.  The update has been cancelled.", vbExclamation
    Exit Function
  End If
  lngFileSizeTotal = lngFileSizeTotal + FileLen(strFile)
Next i

For i = 0 To numFiles - 1
  If Not CopyFile(strUpdateFileDir & "\" & fnTable(1, i), strDBDir & "\" & fnTable(1, i), fu) Then
    MsgBox "The file '" & fnTable(1, i) & "' was not succesfully copied.", vbCritical
    Exit Function
  End If
Next i

CopyFiles = True

Exit Function
CopyFiles_Err:
ErrHand "modUpdate", "CopyFiles"
Exit Function

End Function


Function CopyFile(SourceName As String, DestName As String, f As frmUpdate) As Boolean
On Error GoTo CopyFile_Err

Const BufSize = 8192
Dim Buffer As String * BufSize, TempBuf As String, tVal As Single
Dim SourceF As Integer, DestF As Integer, i As Long, numAttempts As Integer

numAttempts = 0
  
If FileExists(DestName) Then Kill DestName
  
SourceF = FreeFile
Open SourceName For Binary As #SourceF
DestF = FreeFile
Open DestName For Binary As #DestF
For i = 1 To LOF(SourceF) \ BufSize
  Get #SourceF, , Buffer
  Put #DestF, , Buffer
  tVal = ((lngFileSizeCopied + (Loc(SourceF))) / lngFileSizeTotal) * 100
  fu.pbar.Value = IIf(tVal > 100, 100, tVal)
Next i
i = LOF(SourceF) Mod BufSize
If i > 0 Then
  Get #SourceF, , Buffer
  TempBuf = Left$(Buffer, i)
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
    If numAttempts < 6 Then
      numAttempts = numAttempts + 1
      Wait "Waiting for database to close...", 3
      Resume
    Else
      If MsgBox("The file '" & DestName & "' is in use.  Make sure you don't have another copy of the database open.  Click 'Retry' to attempt copying the file again.", vbRetryCancel + vbQuestion) = vbCancel Then
        Exit Function
      Else
        Resume
      End If
    End If
  Case Else
    MsgBox "There was an error copying the file '" & SourceName & "' to '" & DestName & "'.  Error #" & Err & ", " & Err.Description, vbCritical
    Close
    Exit Function
End Select

End Function

Public Function Wait(msg As String, waitSecs As Long) As Boolean
On Error GoTo Wait_Err

Wait = False

Dim fW As frmWait

Set fW = New frmWait
fW.Left = fu.Left + fu.Width / 2 - fW.Width / 2
fW.Top = fu.Top + fu.Height / 2 - fW.Height / 2
fW.lblMessage.Caption = msg
fW.timeDelaySecs = waitSecs * 1000
fW.timeDelayCur = 0
fW.tmrCancel.Interval = 100
fW.pbar.Value = 0
fW.Visible = True

While fW.Visible
  DoEvents
Wend

If Not fW.bolCancel Then Wait = True

Set fW = Nothing

Exit Function
Wait_Err:
ErrHand "modUpdate", "Wait"
End
Exit Function

End Function

Public Function ErrHand(Optional strLocation As String, Optional strFunction As String)

Select Case Err
  Case Else
    Dim sMsg As String
    sMsg = Err.Description & vbCrLf
    sMsg = sMsg & "Source: " & strLocation & "  (" & strFunction & ")" & vbCrLf
    sMsg = sMsg & "Number: " & Err.Number & " (" & Err.Source & ")"
    MsgBox sMsg, vbExclamation, "Error"
End Select

End Function

Public Function GetWindowsSysDir() As String
    Dim strBuf As String

    strBuf = Space$(255)
    '
    'Get the system directory and then trim the buffer to the exact length
    'returned and add a dir sep (backslash) if the API didn't return one
    '
    If GetSystemDirectory(strBuf, 255) Then
        GetWindowsSysDir = StringFromBuffer(strBuf)
        AddDirSep GetWindowsSysDir
    End If
End Function

Public Sub AddDirSep(strPathName As String)
    strPathName = RTrim$(strPathName)
    If Right$(strPathName, Len(gstrSEP_URLDIR)) <> gstrSEP_URLDIR Then
        If Right$(strPathName, Len(gstrSEP_DIR)) <> gstrSEP_DIR Then
            strPathName = strPathName & gstrSEP_DIR
        End If
    End If
End Sub


Public Function StringFromBuffer(Buffer As String) As String
    Dim nPos As Long

    nPos = InStr(Buffer, vbNullChar)
    If nPos > 0 Then
        StringFromBuffer = Left$(Buffer, nPos - 1)
    Else
        StringFromBuffer = Buffer
    End If
End Function



Public Function MscomInstall()
On Error GoTo MscomInstall_Err

Dim fCC As New frmComCtl, x As Double, sShell As String

If IsWindowsNT() Then
  If Not FileExists(GetWindowsSysDir & "rundll32.exe") Then
    MsgBox "The required file to register MSCOMCTL, rundll32.exe, could not be found.", vbExclamation
    End
    Exit Function
  End If
  sShell = GetWindowsSysDir & "rundll32.exe setupapi,InstallHinfSection DefaultInstall 132 "
Else
  If Not FileExists(GetWindowsDir & "rundll.exe") Then
    MsgBox "The required file to register MSCOMCTL, rundll.exe, could not be found.", vbExclamation
    End
    Exit Function
  End If
  sShell = GetWindowsDir & "rundll.exe setupx.dll,InstallHinfSection DefaultInstall 132 "
End If
sShell = sShell & strUpdateFileDir & "\mscomctl\mscomctl.inf"
x = Shell(sShell, vbNormalNoFocus)
WaitForSingleObject x, 15000
If Not fu Is Nothing Then fu.Visible = False
fCC.Visible = True
While fCC.Visible
  DoEvents
Wend
End
Exit Function

MscomInstall_Err:
ErrHand "modUpdate", "MscomInstall"
Exit Function
End Function

Public Function IsWindowsNT() As Boolean
Const dwMaskNT = &H2&
IsWindowsNT = (GetWinPlatform() And dwMaskNT)
End Function

'----------------------------------------------------------
' FUNCTION: GetWinPlatform
' Get the current windows platform.
' ---------------------------------------------------------
Public Function GetWinPlatform() As Long
  Dim osvi As OSVERSIONINFO

  osvi.dwOSVersionInfoSize = Len(osvi)
  If GetVersionEx(osvi) = 0 Then
    Exit Function
  End If
  GetWinPlatform = osvi.dwPlatformId
End Function

Public Function GetWindowsDir() As String
    Dim strBuf As String

    strBuf = Space$(255)
    '
    'Get the windows directory and then trim the buffer to the exact length
    'returned and add a dir sep (backslash) if the API didn't return one
    '
    If GetWindowsDirectory(strBuf, 255) Then
        GetWindowsDir = StringFromBuffer(strBuf)
        AddDirSep GetWindowsDir
    End If
End Function


Public Function Pause(f As Form)
If Not f.Visible Then f.Visible = True
While f.Visible
  DoEvents
Wend
End Function
