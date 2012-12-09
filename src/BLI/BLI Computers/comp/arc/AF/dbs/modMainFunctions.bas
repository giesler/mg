Attribute VB_Name = "modMainFunctions"
'Global variables
Option Explicit

Global DebugFlag As Boolean
Global sComp$                   'company name
Global fD As frmDebug

'INI File
Global strININame$              'INI file name
Global strINISection$           'INI section name
Const cINIFile = "dbsynch.ini"  'actual ini filename

'Logging database
Global Const cLogDB = "dbsynlog.mdb"   'logging database
Global dbLogDB As Database      'database to write to

'Pathname variables
' ALL FASTENERS
Public Const AFDBShare = "e:\database\data"      'database share
Public Const AFOrigName = "TPDataF.mdb"         'original database file name
Public Const AFCompactName = "TPDataFc.mdb"      ' compacted db name
Public Const AFCompressName = "TPDataF.zl"       ' compressed db name
Public Const AFATSDropPath = "e:\database\ats\drop"  'ATS drop path
Public Const AFATSPickPath = "e:\database\ats\pick"  'ATS pick path
Public Const AFTproMDE = "e:\database\tpro.mde"

' ALL TOOL SALES
Public Const ATSDBShare = "c:\database\data"        ' ATS database share
Public Const ATSOrigName = "TPDataT.mdb"            ' original database file name
Public Const ATSCompactName = "TPDataTc.mdb"        ' compacted file name
Public Const ATSCompressName = "TPDataT.zl"         ' compressed file name
Public Const ATSDropPath = "/database/ats/drop"     ' ATS drop path at AF
Public Const ATSPickPath = "/database/ats/pick"     ' ATS pick path at AF
Public Const ATSAFHost = "allfast.com"
Public Const ATSTproMDE = "c:\database\tpro.mde"
Public Const ATSVersionFile = "c:\database\version.txt"
Public Const ATSVersionPick = "/database"

Public CommandInt As String
Public Const BackupPath = "\backup"                 ' directory to backup to
Public Const intGenerations = 3                     ' # generations (< 10)

Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

Public Sub Main()
On Error GoTo Main_Err

'check command line for option telling to open in debug mode
If InStr(LCase(Command), "/debug") Then
    Set fD = New frmDebug
    fD.Show
    DebugFlag = True
Else
    DebugFlag = False
End If

If InStr(LCase(Command), "/log") Then
    Dim frmLog As Form
    Set frmLog = New frmSynchLog
    frmLog.Show
    Exit Sub
End If

If InStr(LCase(Command), "/zl") Then
   Set frmZL = New frmZL
   frmZL.Show
   Exit Sub
End If

'find out which company to use
If InStr(LCase(Command), "af") Then
    sComp = "AF"
    CommandInt = "cmd.exe"
ElseIf InStr(LCase(Command), "at") Then
    sComp = "ATS"
    CommandInt = "cmd.exe"
Else
    frmAbout.Show
    Exit Sub
End If

'Set system db
If Not FileExists(App.Path & "\system.mdw") Then
    LogEvent "Main", "system.mdw file missing from " & App.Path
    End
    Exit Sub
End If
DBEngine.SystemDB = App.Path & "\system.mdw"
DBEngine.DefaultUser = "Admin"

' Backup, then open dbsynch, then compress it
If FileExists(App.Path & "\dbsynlog.md2") Then
    Kill App.Path & "\dbsynlog.md2"
End If
Call FileCopy(App.Path & "\" & cLogDB, App.Path & "\dbsynlog.md2")
Call OpenLoggingDB
LogEvent "Main", "Start------------------------- v" & App.Major & "." & App.Minor & "." & App.Revision
Call CompressFile(App.Path & "\dbsynlog.md2", App.Path & "\dbsynlog.zl")

LogEvent "Main", "Running company " & sComp

Select Case sComp
    Case "AF"
        If AFSynch = False Then
            LogEvent "Main", "AFSynch failed."
        End If
    Case "ATS"
        If ATSSynch = False Then
            LogEvent "Main", "ATSSynch failed."
        End If
End Select

If DebugFlag Then
    MsgBox "Close the debug messages dialog to close.", vbExclamation
    fD.Show
    While fD.Visible = True
        DoEvents
    Wend
End If

End

Exit Sub

Main_Err:
LogEvent "Function Main", Err.Description, Err.Number, True
End
Exit Sub

End Sub


'
'Renames generations of the database - for backup
'
Public Function RenameGens() As Boolean
On Error GoTo RenameGens_Err

Dim sFileName$, iTemp%, sCurFileName$, sPartFN$, sNextFile$
Dim sBackupPath$, sPrevFileName$

RenameGens = False
LogEvent "RenameGens", "Renaming generations started."

'build compressed file name for current company
Select Case sComp
    Case "AF"
        sFileName = AFDBShare & "\" & AFCompressName
        sBackupPath = AFDBShare & BackupPath & "\"
    Case "ATS"
        sFileName = ATSDBShare & "\" & ATSCompressName
        sBackupPath = ATSDBShare & BackupPath & "\"
End Select

'take one char off to allow for number *** don't need with .zl extensions
'sPartFN = Left$(sFileName, Len(sFileName) - 1)

For iTemp = intGenerations To 0 Step -1
    sCurFileName = sPartFN & Trim(Str$(iTemp))
    If iTemp > 0 Then
        sNextFile = sPartFN & Trim(Str$(iTemp - 1))
    Else
        sNextFile = sPartFN & "_"
    End If
    If FileExists(sCurFileName) Then
        Kill sCurFileName
    End If
    If FileExists(sNextFile) Then
        Name sNextFile As sCurFileName
    End If
Next iTemp

LogEvent "RenameGens", "Renaming done."
RenameGens = True
Exit Function

RenameGens_Err:
LogEvent "RenameGens", Err.Description, Err.Number, True
Exit Function

End Function

'Forces users to log out of the passed database
Public Function ForceUserLogoff(strDBase$) As Boolean
On Error GoTo ForceUserLogoff_Err

LogEvent "ForceUserLogoff", "Function started"

ForceUserLogoff = False
Dim dbTempDB As Database, strSQL$

'Open database
Set dbTempDB = OpenDatabase(strDBase, False, False)
dbTempDB.Execute "Update tblUserCurrent SET KickFlag = True;"
dbTempDB.Close
LogEvent "ForceUserLogoff", "Function completed succesfully."
ForceUserLogoff = True

Exit Function

ForceUserLogoff_Err:
LogEvent "ForceUserLogoff", Err.Description, Err.Number, True
Exit Function

End Function

'
'Compacts the database name passed into the second name passed
'
Public Function CompactDB(strCurDBName$, strNewDBName$) As Boolean
On Error GoTo CompactDB_Err

CompactDB = False
LogEvent "CompactDB", "Beginning compaction of " & strCurDBName & " to " & strNewDBName
If FileExists(strNewDBName) Then Kill strNewDBName
DBEngine.CompactDatabase strCurDBName, strNewDBName
LogEvent "CompactDB", "Database compacted from " & strCurDBName & " to " & strNewDBName
CompactDB = True

Exit Function
CompactDB_Err:
LogEvent "CompactDB", Err.Description, Err.Number, True
Exit Function

End Function

'
'Uses compress.exe to compress the passed filename
'
Public Function CompressFile(strSourceFile$, strDestFile$) As Boolean
On Error GoTo CompressFile_Err

CompressFile = False
LogEvent "CompressFile", "Compressing " & strSourceFile & " to " & strDestFile

Dim zl1 As New zl, result As Boolean

If Not FileExists(strSourceFile) Then
   LogEvent "CompressFile", "Source file " & strSourceFile & " does not exist."
   Exit Function
ElseIf FileExists(strDestFile) Then
   LogEvent "CompressFile", "Dest file " & strDestFile & " already exists."
   Exit Function
End If

result = zl1.Compress(strSourceFile, strDestFile)

If Not result Then
   If zl1.bolCancelled Then
      LogEvent "CompressFile", "Compression cancelled.  Exiting."
      Exit Function
   Else
      LogEvent "CompressFile", "Compression failed.  Exiting."
      Exit Function
   End If
End If

LogEvent "CompressFile", "File compression completed succesfully."

Exit Function
CompressFile_Err:
LogEvent "CompressFile", Err.Description, Err.Number
Exit Function

End Function


'  Uses expand.exe to expand the passed file
Public Function ExpandFile(strSourceFile$, strDestFile$) As Boolean
On Error GoTo ExpandFile_Err

ExpandFile = False
LogEvent "ExpandFile", "Expanding " & strSourceFile & " to " & strDestFile

Dim zl1 As New zl, result As Boolean

If Not FileExists(strSourceFile) Then
   LogEvent "ExpandFile", "Source file " & strSourceFile & " does not exist."
   Exit Function
ElseIf FileExists(strDestFile) Then
   LogEvent "ExpandFile", "Dest file " & strDestFile & " already exists."
   Exit Function
End If

result = zl1.Expand(strSourceFile, strDestFile)

If Not result Then
   If zl1.bolCancelled Then
      LogEvent "ExpandFile", "Expanding cancelled.  Exiting."
      Exit Function
   Else
      LogEvent "ExpandFile", "Expanding failed.  Exiting."
      Exit Function
   End If
End If

LogEvent "ExpandFile", "File expansion completed succesfully."

Exit Function
ExpandFile_Err:
LogEvent "ExpandFile", Err.Description, Err.Number, True
Exit Function

End Function


'ATS
'Waits for flag file in ATS pick path
'
Public Function ATSWaitPickFlag()
On Error GoTo ATSWaitPickFlag_Err

Dim i%

LogEvent "ATSWaitPickFlag", "Waiting for pick flagfile."

For i = 1 To 42
    Sleep (60000 * 5)           'wait 5 minutes
    If FileExists(ATSPickPath & "pickflag.txt") Then
        Kill ATSPickPath & "pickflag.txt"
        LogEvent "ATSWaitPickFlag", "pickflag.txt detected."
        Exit Function
    End If
Next i

LogEvent "ATSWaitPickFlag", "Flag file not found after 3.5 hours.", 0, True
End

Exit Function

ATSWaitPickFlag_Err:
Select Case Err
    Case Else
        LogEvent "ATSWaitPickFlag", Err.Description, Err.Number, True
        End
        Exit Function
End Select

End Function

'AF
'Synchronizes the two databases
'
Public Function AFSynchDBs() As Boolean
On Error GoTo AFSynchDBs_Err

Dim dbs As Database, ws As Workspace, AFDBName$, ATSDBName$
AFSynchDBs = False

AFDBName = AFDBShare & "\" & AFOrigName
ATSDBName = AFATSDropPath & "\" & ATSOrigName
 
Set ws = DBEngine(0)
LogEvent "AFSynchDBs", "Synching open db " & AFDBName & " with name: " & ATSDBName

Set dbs = ws.OpenDatabase(AFDBName)

' Synchronize replicas (bidirectional exchange).
dbs.Synchronize ATSDBName, dbRepImpExpChanges
LogEvent "AFSynchDBs", "Synching complete."
dbs.Close

LogEvent "AFSynchDBs", "Databases have been synchronized."
AFSynchDBs = True

Exit Function

AFSynchDBs_Err:
Select Case Err
    Case 3197  'The Microsoft Jet database engine stopped the process because you and another user are attempting to change the same data at the same time.
        LogEvent "AFSynchDBs", "User conflict error", Err.Number
        Exit Function
    Case Else
        LogEvent "AFSynchDBs", Err.Description, Err.Number
        Exit Function
End Select

End Function

Public Function FTPGetFile(strDir As String, strFile As String, strDestFName As String) As Boolean
On Error GoTo FTPGetFile_Err

FTPGetFile = False

If FileExists(App.Path & "\fgetfile.tmp") Then
    Kill App.Path & "\fgetfile.tmp"
End If

Dim i As Integer, x As Long
i = FreeFile

' Create ftpget.bat file
Open App.Path & "\ftpget.bat" For Output As i
Print #i, "@echo off"
Print #i, "REM File created by dbsynch program automatically on " & Now
Print #i, "ftp -s:" & Chr(34) & App.Path & "\sftpget.cmd" & Chr(34)
Print #i, "echo File Done at " & Now & " > " & Chr(34) & App.Path & "\fgetfile.tmp" & Chr(34)

' Create sftpget.cmd script file for FTP
Close i
Open App.Path & "\sftpget.cmd" For Output As i
Print #i, "open " & ATSAFHost
Print #i, "ats"
Print #i, "ats"
Print #i, "cd " & strDir
Print #i, "binary"
Print #i, "get " & strFile & " " & Chr(34) & strDestFName & Chr(34)
Print #i, "quit"
Close i

If DebugFlag Then
    x = Shell(CommandInt & " /c " & Chr(34) & App.Path & "\ftpget.bat" & Chr(34), vbNormalFocus)
Else
    x = Shell(CommandInt & " /c " & Chr(34) & App.Path & "\ftpget.bat" & Chr(34), vbHide)
End If

For i = 1 To 60
    If Not Wait("Waiting to get file " & strFile & "... (" & i & "/60)", 120) Then
        Exit Function
    End If
    If FileExists(App.Path & "\fgetfile.tmp") Then
        Kill App.Path & "\fgetfile.tmp"
        If Not FileExists(strDestFName) Then
            LogEvent "FTPGetFile", "File receive failed on file " & strDestFName
            Exit Function
        ElseIf FileLen(strDestFName) < 10 Then
            LogEvent "FTPGetFile", "File length of " & strDestFName & " < 2"
            Exit Function
        End If
        LogEvent "FTPGetFile", "File " & strFile & " rcvd to " & strDestFName
        FTPGetFile = True
        Exit Function
    End If
Next i
       
LogEvent "FTPGetFile", "2 hours and no flag file.", 0, True

Exit Function
FTPGetFile_Err:
LogEvent "FTPGetFile", Err.Description, Err.Number, True
Exit Function

End Function

Public Function FTPPutFile(strDir As String, strFilePath As String, strFile As String) As Boolean
On Error GoTo FTPPutFile_Err

FTPPutFile = False

If FileExists(App.Path & "\fputfile.tmp") Then
    Kill App.Path & "\fputfile.tmp"
End If

Dim i As Integer, x As Long
i = FreeFile

' Create ftpput.bat file
Open App.Path & "\ftpput.bat" For Output As i
Print #i, "@echo off"
Print #i, "REM File created by dbsynch program automatically on " & Now
Print #i, "ftp -s:" & Chr(34) & App.Path & "\sftpput.cmd" & Chr(34)
Print #i, "echo File Done at " & Now & " > " & Chr(34) & App.Path & "\fputfile.tmp" & Chr(34)

' Create sftpput.cmd script file for FTP
Close i
Open App.Path & "\sftpput.cmd" For Output As i
Print #i, "open " & ATSAFHost
Print #i, "ats"
Print #i, "ats"
Print #i, "cd " & strDir
Print #i, "binary"
Print #i, "del " & strFile
Print #i, "put " & Chr(34) & strFilePath & "\" & strFile & Chr(34)
Print #i, "quit"
Close i

If DebugFlag Then
    x = Shell(CommandInt & " /c " & Chr(34) & App.Path & "\ftpput.bat" & Chr(34), vbNormalFocus)
Else
    x = Shell(CommandInt & " /c " & Chr(34) & App.Path & "\ftpput.bat" & Chr(34), vbHide)
End If

For i = 1 To 60
    If Not Wait("Waiting to put file " & strFile & "... (" & i & "/60)", 120) Then
        Exit Function
    End If
    If FileExists(App.Path & "\fputfile.tmp") Then
        Kill App.Path & "\fputfile.tmp"
        LogEvent "FTPPutFile", "File " & strFile & " sent to " & strDir
        FTPPutFile = True
        Exit Function
    End If
Next i
       
LogEvent "FTPPutFile", "2 hours and no flag file.", 0, True


Exit Function
FTPPutFile_Err:
Select Case Err
    Case Else
        LogEvent "FTPPutFile", Err.Description, Err.Number, True
        Exit Function
End Select

End Function


Public Function AFSynch() As Boolean
On Error GoTo AFSynch_Err

Dim i As Long

AFSynch = False
LogEvent "AFSynch", "Synch started."
            
'First compress local copy of TPro for ATS pickup
If Not CompressFile(AFTproMDE, AFATSPickPath & "\TPro.zl") Then
    LogEvent "AFSynch", "TPro compression failed, skipping."
End If

'Force all users to logoff
If Not ForceUserLogoff(AFDBShare & "\" & AFOrigName) Then
    LogEvent "AFSynch", "User logoff function failed, skipping."
End If

'Rename generations
If Not RenameGens Then
    LogEvent "AFSynch", "Rename backup gens failed, skipping."
End If
            
'Wait for users
If Not Wait("Waiting for users to logoff/be kicked...", 120) Then
    LogEvent "AFSynch", "User wait cancelled.  Exiting."
    Exit Function
End If

'Compact database
If Not CompactDB(AFDBShare & "\" & AFOrigName, AFDBShare & "\" & AFCompactName) Then
    LogEvent "AFSynch", "Database compaction failed, exiting."
    Exit Function
End If

'Make sure new compacted version exists, then rename back
If Not FileExists(AFDBShare & "\" & AFCompactName) Then
    LogEvent "AFSynch", "Compacted copy of database missing, exiting."
    Exit Function
Else
    LogEvent "AFSynch", "Copying compacted database back to original filename."
    If FileExists(AFDBShare & "\" & AFOrigName) Then Kill AFDBShare & "\" & AFOrigName
    FileCopy AFDBShare & "\" & AFCompactName, AFDBShare & "\" & AFOrigName
End If

'Wait for drop flag
For i = 1 To 60
    If Wait("Waiting for ATS drop flag file (" & i & "/60)", 120) = False Then
        LogEvent "AFSynch", "Waiting for drop flag file cancelled by user."
        Exit Function
    End If
    If FileExists(AFATSDropPath & "\dropflag.txt") Then
        Kill AFATSDropPath & "\dropflag.txt"
        LogEvent "AFSynch", "ATS Drop flag file detected."
        Exit For
    End If
    If i = 60 Then
        LogEvent "AFSynch", "2 hours and no ATS drop flag file detected."
        Exit Function
    End If
Next i

'Expand compressed dropped database
If ExpandFile(AFATSDropPath & "\" & ATSCompressName, AFATSDropPath & "\" & ATSOrigName) = False Then
    LogEvent "AFSynch", "ExpandFile " & AFATSDropPath & "\" & ATSCompressName & " to " & ATSOrigName & " failed, exiting."
    Exit Function
End If
            
'Synch dbs
If AFSynchDBs = False Then
    LogEvent "AFSynch", "AFSynchDBs failed, exiting..."
    Exit Function
End If

'Compress file for ATS to pick it up
If Not CompressFile(AFATSDropPath & "\" & ATSOrigName, AFATSPickPath & "\" & ATSCompressName) Then
    LogEvent "AFSynch", "Compress ATS pick database failed"
    Exit Function
End If

' create pick flag file
i = FreeFile
Open AFATSPickPath & "\pickflag.txt" For Output As i
Write #i, "Pick flag file created " & Now
Close #i
LogEvent "AFSynch", "Pick flag file created."

AFSynch = True

Exit Function
AFSynch_Err:
LogEvent "AFSynch", Err.Description, Err.Number, True
Exit Function

End Function



Public Function ATSSynch() As Boolean
On Error GoTo ATSSynch_Err

Dim i As Long

ATSSynch = False
LogEvent "ATSSynch", "Synch started."
LogEvent "ATSSynch", FreeDiskSpace("c:")

'First send copy of synch log to AF
If FTPPutFile(ATSDropPath, App.Path, "dbsynlog.zl") = False Then
    LogEvent "ATSSynch", "Synch log copy failed, skippping..."
End If

'Force all users to logoff
If Not ForceUserLogoff(ATSDBShare & "\" & ATSOrigName) Then
    LogEvent "ATSSynch", "User logoff function failed, skipping."
End If

'Rename generations
If Not RenameGens Then
    LogEvent "ATSSynch", "Rename backup gens failed, skipping."
End If
            
'Wait for users
If Not Wait("Waiting for users to logoff/be kicked...", 120) Then
    LogEvent "ATSSynch", "User wait cancelled.  Exiting."
    Exit Function
End If

'Compact database
If Not CompactDB(ATSDBShare & "\" & ATSOrigName, ATSDBShare & "\" & ATSCompactName) Then
    LogEvent "ATSSynch", "Database compaction failed, exiting."
    Exit Function
End If

'Make sure new compacted version exists, then rename back
If Not FileExists(ATSDBShare & "\" & ATSCompactName) Then
    LogEvent "ATSSynch", "Compacted copy of database missing, exiting."
    Exit Function
Else
    LogEvent "ATSSynch", "Copying compacted database back to original filename."
    If FileExists(ATSDBShare & "\" & ATSOrigName) Then Kill ATSDBShare & "\" & ATSOrigName
    FileCopy ATSDBShare & "\" & ATSCompactName, ATSDBShare & "\" & ATSOrigName
End If

'Compress file for ATS to pick it up
If Not CompressFile(ATSDBShare & "\" & ATSCompactName, ATSDBShare & "\" & ATSCompressName) Then
    LogEvent "ATSSynch", "Compress ATS pick database failed"
    Exit Function
End If

'Send compressed db to AF
If Not FTPPutFile(ATSDropPath, ATSDBShare, ATSCompressName) Then
    LogEvent "ATSSynch", "Database send to AF failed, exiting."
    Exit Function
End If
LogEvent "ATSSynch", "Database copied to " & ATSDropPath & "/" & ATSCompressName
i = FreeFile
Open App.Path & "\dropflag.txt" For Output As i
Print #i, "Drop file created " & Now
Close i
If Not FTPPutFile(ATSDropPath, App.Path, "dropflag.txt") Then
    LogEvent "ATSSynch", "Failed to send dropflag.txt"
    Exit Function
End If

'get copy of Tpro
If FileExists(ATSDBShare & "\TPro.zl") Then Kill ATSDBShare & "\TPro.zl"
If Not FTPGetFile(ATSPickPath, "TPro.zl", ATSDBShare & "\TPro.zl") Then
    LogEvent "AFSynch", "Failed to receive TPro.zl from " & ATSPickPath & ", skipping."
End If
If FileExists(ATSDBShare & "\TPro.zl") Then
    If ExpandFile(ATSDBShare & "\TPro.zl", ATSTproMDE) = False Then
        LogEvent "AFSynch", "Failed to expand Tpro.zl to TPro.mde, skipping"
    End If
End If

'get version file
If FileExists(ATSVersionFile) Then Kill ATSVersionFile
If Not FTPGetFile(ATSVersionPick, "\pickflag.txt", ATSVersionFile) Then
    LogEvent "AFSynch", "Failed to get version file, skipping."
End If


'Wait for pick flag
If FileExists(App.Path & "\pickflag.txt") Then Kill App.Path & "\pickflag.txt"
For i = 1 To 60
    If Wait("Waiting for AF pick flag file (" & i & "/60)", 120) = False Then
        LogEvent "ATSSynch", "Waiting for pick flag file cancelled by user."
        Exit Function
    End If
    Call FTPGetFile(ATSPickPath, "pickflag.txt", App.Path & "\pickflag.txt")
    If FileExists(App.Path & "\pickflag.txt") And FileLen(App.Path & "\pickflag.txt") > 0 Then
        LogEvent "ATSSynch", "Pick flag file detected."
        Exit For
    End If
    If i = 60 Then
        LogEvent "ATSSynch", "2 hours and no pick flag file detected."
        Exit Function
    End If
Next i

'get pick db
If FileExists(ATSDBShare & ATSCompressName) Then Kill ATSDBShare & ATSCompressName
If Not FTPGetFile(ATSPickPath, ATSCompressName, ATSDBShare & "\" & ATSCompressName) Then
    LogEvent "ATSSynch", "FTP pick failed, exiting."
    Exit Function
End If

'Expand compressed picked database
If ExpandFile(ATSDBShare & "\" & ATSCompressName, ATSDBShare & "\" & ATSOrigName) = False Then
    LogEvent "ATSSynch", "ExpandFile " & ATSDBShare & "\" & ATSCompressName & " to " & ATSOrigName & " failed, exiting."
    Exit Function
End If
            
ATSSynch = True

Exit Function
ATSSynch_Err:
LogEvent "ATSSynch", Err.Description, Err.Number, True
Exit Function

End Function

