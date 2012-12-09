Attribute VB_Name = "modUpdate"

'Function declares
Declare Function GetCurrentProcessId Lib "Kernel32" () As Long
Private Declare Function WaitForSingleObject Lib "Kernel32" (ByVal _
      hHandle As Long, ByVal dwMilliseconds As Long) As Long

'Global variable declares
Dim strDBDir As String
Dim intProcessID As Long
Dim strAccessDir As String
Dim strDBName As String

Dim strUpdateFileDir As String
Dim fnTable() As String
Dim numFiles As Integer

'Const Declarations
Const ctNEWFILE = "\Database.mde"
Const ctCURFILE = "Database.mde"
Const ctDBTITLE = "Access"
Private Const INFINITE = -1&        'used for waitshell function

Declare Sub Sleep Lib "Kernel32" (ByVal dwMilliseconds As Long)

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
    Dim intFileNum As Integer

    On Error Resume Next

    '
    'Remove any trailing directory separator character
    '
    If Right$(strPathName, 1) = gstrSEP_DIR Then
        strPathName = Left$(strPathName, Len(strPathName) - 1)
    End If

    '
    'Attempt to open the file, return value of this function is False
    'if an error occurs on open, True otherwise
    '
    intFileNum = FreeFile
    Open strPathName For Input As intFileNum

    FileExists = IIf(Err, False, True)

    Close intFileNum

    Err = 0
End Function

Public Sub Main()
On Error GoTo Main_Err

'Check and split up command line
If Not (ParseCommandLine) Then
    MsgBox "This update program must be started with command line options.  The format is: " & NWLN & NWLN & "<program path & name> /d:'<program directory>' /a:'<MS Access Directory>' [/i:<process id>]", vbExclamation
    End
End If

'Wait for access to close if there is a PID in command line
If intProcessID Then
    WaitForProcess (intProcessID)
End If

'Read the list of files to update
If Not LoadSettings Then
    MsgBox "There was an error loading the settings file.", vbExclamation
    End
    Exit Sub
End If

'Test the directory to copy to
If Not TestDir(strDBDir) Then
    MsgBox "There was an error setting up the directory '" & strDir & "'.", vbExclamation
    End
    Exit Sub
End If

frmUpdate.Refresh

'Copy the files
If Not CopyFiles Then
    MsgBox "The file copy failed, so this program has been aborted.", vbCritical
    End
    Exit Sub
End If

'Launch Access
If strAccessDir = "" Then
    MsgBox "The files have been updated.", vbInformation
Else
    RunAccess
End If

End

Exit Sub

Main_Err:
Select Case Err
    Case 75
        End
    Case Else
        MsgBox "There was an error in the main procedure.  Error #" & Err & ", " & Err.Description, vbCritical
        End
        Exit Sub
End Select

End Sub

Public Function UpdateFile(srcFile As String, dstFile As String) As Boolean
On Error GoTo UpdateFileErr

UpdateFile = False

Dim numAttempts As Integer
numAttemps = 0

'Check for new DB
If Not FileExists(srcFile) Then
    MsgBox "The source file '" & srcFile & "' was not found.", vbCritical
    Exit Function
End If

StartProc:

If FileExists(dstFile) Then Kill dstFile
FileCopy srcFile, dstFile

UpdateFile = True

Exit Function

UpdateFileErr:
Select Case Err
    Case 75, 70
        numAttempts = numAttempts + 1
        If numAttempts > 6 Then
            If MsgBox(dstFile & " is still open.  Please close " & dstFile & ", then click 'OK' below.", vbOKCancel + vbExclamation) = vbOK Then
                Resume
            Else
                MsgBox "This procedure cannot update " & dstFile & " while it is open.", vbCritical
                Exit Function
            End If
        End If
        frmUpdate.lblStatus.Text = "Holding 5 seconds... (" & numAttempts & ")"
        frmUpdate.Refresh
        Sleep (5000)
        frmUpdate.lblStatus.Text = "Re-attempting file copy..."
        frmUpdate.Refresh
        Resume
    Case Else
        If MsgBox("There was an error in the Update File procdure.  Error #" & Err & ", " & Err.Description & "  Would you like to retry?", vbQuestion + vbYesNo) = vbNo Then
            Exit Function
        End If
        numAttempts = numAttempts + 1
        If numAttempts > 6 Then
            If MsgBox(dstFile & " is still open.  Please close " & dstFile & ", then click 'OK' below.", vbOKCancel + vbExclamation) = vbOK Then
                Resume
            Else
                MsgBox "This procedure cannot update " & dstFile & " while it is open.", vbCritical
                Exit Function
            End If
        End If
        frmUpdate.lblStatus.Text = "Holding 5 seconds... (" & numAttempts & ")"
        frmUpdate.Refresh
        Sleep (5000)
        frmUpdate.lblStatus.Text = "Re-attempting file copy..."
        frmUpdate.Refresh
        Resume
End Select

End Function


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

If Len(Command) = 0 Then
    strDBDir = InputBox("Enter the directory the databases are stored in on the local computer: ", "Direcotry name", "c:\database")
    If Len(strDBDir) = 0 Then Exit Function
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

frmUpdate.lblStatus.Text = "Waiting for Access to close..."
frmUpdate.Show
frmUpdate.Refresh
For intTEMP = 1 To 10000
    On Error Resume Next
    AppActivate intID, False
    If Err = 5 Then Exit Sub
    DoEvents
Next intTEMP
Exit Sub
Sleep 3000

End Sub

Public Sub RunAccess()
On Error GoTo RunAccessErr
Dim retVal As Long

frmUpdate.lblStatus.Text = "Restarting Access..."
frmUpdate.Refresh

Dim txtLaunch As String
'If InStr(strAccessDir, "Runtime") > 0 Then

txtLaunch = strAccessDir & "msaccess.exe " & Chr$(34) & strDBDir & strDBName & Chr$(34)
retVal = Shell(txtLaunch, vbMaximizedFocus)
 'Else
'    MsgBox "Restarting the full version of Microsoft Access 95 is not supported in this version.", vbExclamation
'    retVal = Shell(strAccessDir & "msaccess.exe " & "" & strDBDir & ctCURFILE & "", vbMaximizedFocus)
'End If

frmUpdate.Timer1.Interval = 4000

Exit Sub

RunAccessErr:
Select Case Err
    Case Else
        MsgBox "Error in RunAccess procedure: " & Err & ", " & Err.Description, vbCritical
        Exit Sub
End Select

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

Dim fu As New frmUpdate, i As Integer
With fu
    .lblStatus.Text = "Updating files..."
    .pbar.Max = numFiles
    .pbar.Value = 0
    .Show
End With
    
For i = 0 To numFiles - 1

    If Not UpdateFile(strUpdateFileDir & "\" & fnTable(1, i), strDBDir & "\" & fnTable(1, i)) Then
        MsgBox "The file '" & fnTable(1, i) & "' was not succesfully copied.", vbCritical
        Exit Function
    End If
        
    fu.pbar.Value = i + 1
    fu.Refresh

Next i

CopyFiles = True

Exit Function
CopyFiles_Err:
ErrHand "modUpdate", "CopyFiles"
Exit Function

End Function

Public Function TestDir(strDir As String) As Boolean
On Error GoTo TestDir_Err

TestDir = False

Dim strTest As String
strTest = strDir
If Right$(strTest, 1) <> "\" Then
    strTest = strTest & "\"
End If

On Error Resume Next
Open strTest For Append As 99
If Err Then
    MkDir strDir
End If

Kill strTest
Close 99

TestDir = True

Exit Function
TestDir_Err:
ErrHand "modUpdate", "TestDir"
Exit Function


End Function
