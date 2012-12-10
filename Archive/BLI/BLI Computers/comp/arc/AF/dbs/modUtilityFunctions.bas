Attribute VB_Name = "modUtilityFunctions"
Declare Function GetDiskFreeSpace Lib "kernel32" Alias "GetDiskFreeSpaceA" (ByVal lpRootPathName As String, lpSectorsPerCluster As Long, lpBytesPerSector As Long, lpNumberOfFreeClusters As Long, lpTtoalNumberOfClusters As Long) As Long

Public Type DiskInformation
    lpSectorsPerCluster As Long
    lpBytesPerSector As Long
    lpNumberOfFreeClusters As Long
    lpTotalNumberOfClusters As Long
End Type

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
On Error GoTo FileExists_Err

    Dim intFileNum As Integer

    'Remove any trailing directory separator character
    If Right$(strPathName, 1) = gstrSEP_DIR Then
        strPathName = Left$(strPathName, Len(strPathName) - 1)
    End If

    'Attempt to open the file, return value of this function is False
    'if an error occurs on open, True otherwise
    intFileNum = FreeFile
    Open strPathName For Input As intFileNum
    FileExists = IIf(Err, False, True)

    Close intFileNum
    Err = 0
    Exit Function

FileExists_Err:
Select Case Err
    Case 53         'file not found/dne
        FileExists = False
        Exit Function
    Case Else
        LogEvent "FileExists", Err.Description & " - " & strPathName, Err.Number, False
        FileExists = False
        Exit Function
End Select

End Function

'
'Logs the event passed
'
Public Function LogEvent(sSection$, sDesc$, Optional iError, Optional bHalt)
On Error Resume Next

If iError = "" Then iError = 0
If bHalt = "" Then bHalt = False

Dim strSQL$
Dim rstEL As Recordset
Set rstEL = dbLogDB.OpenRecordset("Tbl_EventLog", dbOpenDynaset)
With rstEL
    .AddNew
    !LogTime = Now
    !Section = sSection
    !Description = sDesc
    !ErrorNumber = iError
    !HaltFlag = hhalt
    .Update
    .Close
End With
Set rstEL = Nothing

If DebugFlag Then
    'Add to the log box
    With fD.txtDebugMessages
        .SelStart = Len(.Text)
        .SelLength = 0
        If iError = 0 Then
            .SelText = Format$(Now, "m/d Hh:Nn:Ss") & "  " & sSection & ", " & sDesc & Chr(13) & Chr(10)
        Else
            .SelText = Format$(Now, "m/d Hh:Nn:Ss") & "  " & sSection & ", " & sDesc & " (" & iError & ")" & Chr(13) & Chr(10)
        End If
    End With
End If


End Function


'
'Opens the logging database for output
'
Public Function OpenLoggingDB()
On Error GoTo OpenLoggingDB_Err

Dim sLogDB$, iTemp%, sWinDir$

'path to database
sLogDB = App.Path & "\" & cLogDB

'Open database for logging purposes
Set dbLogDB = DBEngine.Workspaces(0).OpenDatabase(sLogDB, False)

dbLogDB.Execute "Delete * From [Tbl_EventLog] Where LogTime < #" & Date - 21 & "#;"

Exit Function

OpenLoggingDB_Err:
Select Case Err
    Case Else
        Open App.Path & "\error.txt" For Output As 21
        Print #42, "Error in logging DB open: " & Err & ", " & Err.Description
        Close 21
        End
        Exit Function
End Select

End Function


Public Function Wait(msg As String, waitSecs As Long) As Boolean
On Error GoTo Wait_Err

Wait = False

Dim fW As frmWait

Set fW = New frmWait
fW.lblMessage.Caption = msg
fW.timeDelaySecs = waitSecs * 1000
fW.timeDelayCur = 0
fW.tmrCancel.Interval = 250
fW.boxInner.Width = 0
fW.Visible = True

While fW.Visible
    DoEvents
Wend

If Not fW.bolCancel Then Wait = True

Set fW = Nothing

Exit Function
Wait_Err:
LogEvent "Wait", Err.Description, Err, True
End
Exit Function

End Function

Public Sub DebugLog(sSection As String, sDesc As String)

'Add to the log box
With fD.txtDebugMessages
    .SelStart = Len(.Text)
    .SelLength = 0
    .SelText = Format$(Now, "General Date") & "  " & sSection & ", " & sDesc & Chr(13) & Chr(10)
End With

End Sub

Public Function FreeDiskSpace(lpRootPathName As String) As String

Dim info As DiskInformation
Dim lAnswer As Long
Dim lpSectorsPerCluster As Long
Dim lpBytesPerSector As Long
Dim lpNumberOfFreeClusters As Long
Dim lpTotalNumberOfClusters As Long
Dim lBytesPerCluster As Long
Dim lNumFreeBytes As Double
Dim sString As String

lpRootPathName = "c:\"
lAnswer = GetDiskFreeSpace(lpRootPathName, lpSectorsPerCluster, lpBytesPerSector, lpNumberOfFreeClusters, lpTotalNumberOfClusters)
lBytesPerCluster = lpSectorsPerCluster * lpBytesPerSector
lNumFreeBytes = lBytesPerCluster * lpNumberOfFreeClusters
sString = lpRootPathName & " free space = " & Format(((lNumFreeBytes / 1024) / 1024), "0.00") & "MB"
FreeDiskSpace = sString

End Function
