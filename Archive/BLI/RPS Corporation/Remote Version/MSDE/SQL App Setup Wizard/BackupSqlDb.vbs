Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Filename: BackupSqlDb.vbs
'
' Author:   Roger Doherty (lrdohert@microsoft.com)
'
' Date:     8/19/1999
' Modified:
'
' Description: VB Script file which backs up a database using SQL-DMO.  
'              Intended for use with Sql Server Agent CmdExec subsystem.
'
' This file is part of the SQL / MSDE Delpoyment Toolkit
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT
' WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED,
' INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
' OF MERCHANTABILITY AND/OR FITNESS FOR A  PARTICULAR
' PURPOSE.
'
' Copyright (C) 1999 Microsoft Corporation, All rights reserved
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Dim gstrLogFile
Dim gstrSqlUid
Dim gstrSqlPwd
Dim gstrSqlDbName
Dim gblnTrusted
Dim gblnVerbose
Dim gblnBadArgument
Dim gblnHelp
Dim goFileSystem
Dim goWshShell
Dim goDbServer
Dim gstrLogDir

Const SCRIPTTITLE = "SQL/MSDE Deployment Toolkit - Application Database Backup"
Const ARG_PREFIX_LEN = 2        
Const FORREADING = 1
Const FORWRITING = 2
Const FORAPPENDING = 8

Set goFileSystem = CreateObject("Scripting.FileSystemObject")
Set goWshShell = WScript.CreateObject("Wscript.Shell")
	

Call Main


'''''''''''''''''''''''''''''''''''''''''''''''''
' Main
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub Main()
    InitScript
    
    GetCommandLineArgs
    
    Welcome
    
    BackupSqlDb
    
    CompleteScript True
End Sub


'''''''''''''''''''''''''''''''''''''''''''''''''
' InitScript()
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub InitScript()

    On Error Resume Next
    
    'Create the Log folder and File
    gstrLogDir = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\Setup\SQLDataRoot") & "\LOG"
    gstrLogFile = gstrLogDir & "\BackupSqlDbLog.txt"
    CreateLog gstrLogFile
    TestForError
   
    'Initialize session variables
    gblnTrusted = True
    gblnVerbose = False
    gblnHelp = False
	gstrSqlPwd = ""
	gstrSqlDbName = ""

    
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' GetCommandLineArges parses the command line
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub GetCommandLineArgs()
    Dim str
    
    On Error Resume Next
    
    gblnBadArgument = False 'Prime for success
    
    For Each str In WScript.Arguments
        Select Case UCase(Left(str, ARG_PREFIX_LEN))
            Case "-?", "/?"
                gblnHelp = True
				Exit Sub

            Case "-D", "/D" ' SQL Server password
                gstrSqlDbName = Trim(PruneOffPrefix(str))
            
            Case "-U", "/U" ' SQL Server Login ID default is sa
                gblnTrusted = False
                gstrSqlUid = Trim(PruneOffPrefix(str))
                    
            Case "-P", "/P" ' SQL Server password
                gstrSqlPwd = Trim(PruneOffPrefix(str))
                    
            Case "-V", "/V"
                gblnVerbose = True
                    
            Case Else
                gblnBadArgument = True
                Exit Sub
        End Select
    Next

	If gstrSqlDbName = "" Then
		gblnBadArgument = True
	End If
        
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' PruneOffPrefix
'
' Removes the -X: from a command-line option
'''''''''''''''''''''''''''''''''''''''''''''''''
Function PruneOffPrefix(str)

    If Len(str) > ARG_PREFIX_LEN Then
        ' Prune off the -D and the : character : a-dajoh
        PruneOffPrefix = Mid(str, ARG_PREFIX_LEN + 2)
    Else
        PruneOffPrefix = ""
    End If

End Function


'''''''''''''''''''''''''''''''''''''''''''''''''
' Welcome
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub Welcome()

    Dim strWelcome
    
    strWelcome = strWelcome & _
		"** BackupSqlDb.vbs Help Information **" & vbCrLf & vbCrLf & _
		"This script performs a backup on an arbitrary database on local database server" & vbCrLf & vbCrLf & _
		"Usage:" & vbCrLf & _
		"  CSCRIPT ""BackupSqlDb.vbs"" ""/D:database"" [""/U:login id""] [""/P:password""] [""/V""]" & vbCrLf & _
		"  OR" & vbCrLf & _
		"  CSCRIPT ""BackupSqlDb.vbs"" ""-?""" & vbCrLf & vbCrLf & _
		"Where: " & vbCrLf & _
		"  /? - Displays help." & vbCrLf & _
		"  /D:database - Required. Logical database name to be backed up." & vbCrLf & _
		"  /U:login ID - Optional. Standard authentication login id." & vbCrLf & _
		"  /P:password - Optional. Standard authentication password." & vbCrLf & _
		"  /V - Optional. Enables verbose mode." & vbCrLf & vbCrLf & _
		"Script returns 0 on successfull execution, non-zero for failed execution."

    If gblnBadArgument Then
        WScript.echo  "Invalid parameters.  Run script with /? option for help."
        WScript.Quit 1
    ElseIf gblnHelp Then
        WScript.echo strWelcome
        WScript.Quit 1
    Else
        'Time stamp this install process
        Trace "**************************BackupSqlDb.vbs*******************************"
        Trace "InitScript(): " & SCRIPTTITLE
    End If
    
End Sub



'''''''''''''''''''''''''''''''''''''''''''''''''
' CreateLog
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub CreateLog(strLogFile)

    Dim oFile
    Dim strLogEntry
    Dim strBkpLogFile
    Dim strPrefix
    Dim lPos
    Dim nCount
    
    On Error Resume Next

    'Create a folder if it doesn't exist
    If (Not goFileSystem.FolderExists(gstrLogDir)) Then
        CreatePath gstrLogDir
    End If
        
    'Create a log file if it doesn't already exist
    If Not goFileSystem.FileExists(strLogFile) Then
		'Create the new log file
		Set oFile = goFileSystem.CreateTextFile(strLogFile, True)
		oFile.Close
    End If
    
    If Err.Number <> 0 Then
        strLogEntry = "ERROR CreateLog(): " & Err.Description & ", " & Hex(Err.Number) & vbCrLf
        WScript.echo strLogEntry
        WScript.echo "Setup terminated prematurely!"
        WScript.Quit 1
    End If
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' WriteLogFile
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub WriteLogFile(strLogEntry)

    WriteFile gstrLogFile, strLogEntry, FORAPPENDING

End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' WriteFile
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub WriteFile(strFileName, strEntry, Mode)
    
    Dim oFile
    
    On Error Resume Next
    
    Set oFile = goFileSystem.OpenTextFile(strFileName, Mode, True)
    
    oFile.Write strEntry
    
    oFile.Close
    
    TestForError
  
End Sub


'''''''''''''''''''''''''''''''''''''''''''''''''
' CreatePath()
'
' This function recursively builds the full path out of the component folders.
' CreateFolder will fail when passed a folder which doesn't live in an already created path.
'''''''''''''''''''''''''''''''''''''''''''''''''

Sub CreatePath(strFolder)
	On Error Resume Next

	Dim strLeftDir
	Dim strDirExists
	Dim j
	Dim slashcount

	'Trace "CreateFolder(): Creating folder " & """" & strFolder & """" & "..."

	slashcount = 0
	For j = 1 To Len(strFolder)
		If Mid(strFolder, j, 1) = "\" Then
			slashcount = slashcount + 1
			strLeftDir = Left(strFolder, j)

			'Create a folder if it doesn't exist
			If (Not goFileSystem.FolderExists(strLeftDir)) Then
				goFileSystem.CreateFolder (strLeftDir)
				TestForError
			End If
		End If
	Next

	'Create a folder if it doesn't exist
	If (Not goFileSystem.FolderExists(strFolder)) Then
		goFileSystem.CreateFolder (strFolder)
		TestForError
	End If

End Sub


'''''''''''''''''''''''''''''''''''''''''''''''''
' CompleteScript
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub CompleteScript(bDone)

    If bDone Then
        Trace "CompleteScript(): Backup is completed!"
        If gblnVerbose Then
            WScript.echo "BackupSqlDb completed successfully."
        End If
        WScript.Quit 0
    Else
        Trace "CompleteScript(): Backup was not completed!"
        If gblnVerbose Then
            WScript.echo "BackupSqlDb did not complete successfully."
        End If
        
        WScript.Quit 1
    End If

End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' ReadRegValue()
'''''''''''''''''''''''''''''''''''''''''''''''''
Function ReadRegValue(strValueName)
    On Error Resume Next
    
    ReadRegValue = goWshShell.RegRead(strValueName)
    TestForError

End Function

'''''''''''''''''''''''''''''''''''''''''''''''''
' Trace()
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub Trace(strMsg)

    Dim strFormattedMsg
    Dim WshShell
    
    Set WshShell = WScript.CreateObject("Wscript.Shell")
    
    strFormattedMsg = Now & " : " & strMsg & vbCrLf
    
    If gblnVerbose = True Then
        WshShell.Popup strFormattedMsg, 2, SCRIPTTITLE, 0
    End If
    
    WriteLogFile strFormattedMsg
    
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' TestForError()
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub TestForError()

    If Err.Number <> 0 Then
        Trace "ERROR " & Err.Description & ", " & Hex(Err.Number)
        CompleteScript False
        Trace "ERROR BackupSqlDb terminated prematurely!"
        WScript.Quit 1
    End If
        
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''''
' BackupSqlDb()
'''''''''''''''''''''''''''''''''''''''''''''''''
Sub BackupSqlDb()
    ' This routine is useful for implementing a daily differential database backup strategy.
	' If no backup device is found, a new backup device is created.
	' The backup device is examined to determine how many backup sets it contains.
	' If the backup device contains 0 or more than 10 backups, the backup device file is initialized and a full database backup is performed.
	' If the backup device contains >= 1 and <= 9 backups, a differential backup is appended to the end of the backup device file

	Dim oDb
    Dim blnFound
    Dim strBackupDir
    Dim oDrive
    Dim strBackupName
    Dim oBackupDevice
    Dim oBackup
    Dim dtBackupTime
    Dim oResults
	Dim blnInitialize
	Dim lngNumBackups
    
    
    On Error Resume Next
    
    '
    ' Step 1: Connect to database server
    '
        
    
    Set goDbServer = CreateObject("SQLDMO.SqlServer")

    TestForError
    
    If gblnTrusted Then
        goDbServer.Start True, "(local)"
    Else
        goDbServer.Start True, "(local)", gstrSqlUid, gstrSqlPwd
    End If
    
    If Err.Number = 1056 Then
        ' SQL Server already started, just connect
        Err.Clear
        If gblnTrusted Then
            goDbServer.Connect
        Else
            goDbServer.Connect "(local)", gstrSqlUid, gstrSqlPwd
        End If
    End If
    
    TestForError
    
    '
    ' Step 2: Validate database name
    '
    
    blnFound = False
    
    For Each oDb In goDbServer.Databases
        If UCase(oDb.Name) = UCase(gstrSqlDbName) Then
            blnFound = True
            Exit For
        End If
    Next
    
    If Not blnFound Then
        Err.Raise vbObjectError + 50, , "BackupSqlDb(): Database '" & gstrSqlDbName & "' does not exist."
        TestForError
    End If
    
    '
    ' Step 3: Get location of backup directory
    '
    
    strBackupDir = ReadRegValue("HKLM\Software\Microsoft\MSSQLServer\MSSQLServer\BackupDirectory")

	strBackupDir = strBackupDir & "\" & gstrSqlDbName

    If Not goFileSystem.FolderExists(strBackupDir) Then
		CreatePath strBackupDir
    End If
    
    '
    ' Step 4: Check for sufficient space
    '
    
    Set oDrive = goFileSystem.GetDrive(Left(strBackupDir, 2))
    
    If (oDrive.AvailableSpace / (1024 ^ 2)) < oDb.Size Then
        Err.Raise vbObjectError + 50, , "BackupSqlDb(): Insufficient space on drive '" & Left(strBackupDir) & "' to perform backup of database '" & gstrSqlDbName & "'"
        TestForError
        Exit Sub
    End If
    
    '
    ' Step 5: Locate Backup Device
    '
       
    strBackupName = gstrSqlDbName & "_Backup_Device"
    blnFound = False
    
    For Each oBackupDevice In goDbServer.BackupDevices
        If oBackupDevice.Name = strBackupName Then
            blnFound = True
            Exit For
        End If
    Next 
    
    TestForError
        
    '
    ' Step 6: Determine backup strategy
    '
    
    If blnFound Then
		' Backup device was found, examine it to determine how many backups it contains
		Set oResults = oBackupDevice.ReadBackupHeader
		lngNumBackups = oResults.Rows
		TestForError

		If lngNumBackups >= 10 Then
			' Initialize backup file and perform full database backup on seventh day of the week
			blnInitialize = True
			Trace "BackupSqlDb(): The backup file will be initialized because " & Cstr(lngNumBackups) & " backup(s) exist in the backup file."
		Else
			' Append to existing backup file and perform differential database backup on all other days
			blnInitialize = False
			Trace "BackupSqlDb(): Differential backup will be appended to existing backup file because " & Cstr(lngNumBackups) & " backup(s) exist in the backup file."
		End If
	Else
		' Backup device not found.  Create it and assume this is the first backup
        Set oBackupDevice = Nothing
        Set oBackupDevice = CreateObject("SQLDMO.BackupDevice")
        TestForError
        
        With oBackupDevice
            .Name = strBackupName
            .PhysicalLocation = strBackupDir & "\" & strBackupName & ".bak"
            .Type = 2 ' SQLDMODevice_DiskDump
        End With
        
        goDbServer.BackupDevices.Add oBackupDevice
        TestForError
        
        Trace "BackupSqlDb(): Added new backup device named '" & oBackupDevice.PhysicalLocation & "'"

		' Initialize backup file and perform full database backup because this is the first backup
		blnInitialize = True
		Trace "BackupSqlDb(): The backup file will be initialized because this is the first backup."
    End If
    
	
	'
    ' Step 7: Backup Database to device
    '
    
	Set oBackup = CreateObject("SQLDMO.Backup")
	TestForError
    
    With oBackup
        .Database = gstrSqlDbName
        .Devices = strBackupName
        
        If blnInitialize Then
			.Initialize = True
			.Action = 0 ' SQLDMOBackup_Database
			.BackupSetName = gstrSqlDbName & "_set_1_full"
			.BackupSetDescription = "Full backup of " & gstrSqlDbName & " database initiated by BackupSqlDb.vbs."
        Else
            .Initialize = False
			.Action = 1 ' SQLDMOBackup_Incremental
			.BackupSetName = gstrSqlDbName & "_set_" & CStr(lngNumBackups + 1) & "_differential"
			.BackupSetDescription = "Differential backup of " & gstrSqlDbName & " database initiated by BackupSqlDb.vbs."
        End If
        
        ' Initiate Backup
        .SQLBackup goDbServer
    End With
    
    TestForError
    
    '
	' Step 8: Report results
	'

	Set oBackupDevice = Nothing
	Set oBackupDevice = goDbServer.BackupDevices(strBackupName)
    
    Set oResults = Nothing
	Set oResults = oBackupDevice.ReadBackupHeader
    
    With oResults
        dtBackupTime = .GetColumnDate(.Rows, 19) - .GetColumnDate(.Rows, 18)
        Trace "BackupSqlDb(): Backup of database '" & .GetColumnString(1, 10) & "' completed successfully; " & _
            "The backup size is " & CStr(.GetColumnDouble(1, 13)) & " bytes; " & _
            "The backup took " & CStr(DatePart("n", dtBackupTime)) & " minutes, " & CStr(DatePart("s", dtBackupTime)) & " seconds; " & _
            "The backup completed " & FormatDateTime(.GetColumnDate(.Rows, 19)) & "; "
    End With
End Sub

