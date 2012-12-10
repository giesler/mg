Attribute VB_Name = "modBarcodePrinter"
Option Explicit

Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
Public Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long

' variables
Public prtName As String
Public prtUNCPath As String
Public prtTIme As String
Public prtPort As String
Public prtAFIData As String
Public prtAFIIndex As Long

Public strLastMsgTime As String

Public bolMaxRecs As Boolean

' databases
Public dbsAFI As Database
Public dbsLabels As Database

' status info
Public bolActive As Boolean, bolSetup As Boolean, bolDebug As Boolean

Public Sub Main()
On Error GoTo Main_Err

' set up flag variables
If InStr(Command, "/debug") Then
    bolDebug = True
Else
    bolDebug = False
End If
bolActive = False
bolSetup = False

' set up local database
If Not FileExists(App.Path & "\bprint.mdb") Then
    MsgBox "A required file, bprint.mdb, was not found in the application's directory.  This program has been aborted.", vbCritical
    End
    Exit Sub
End If
Set dbsLabels = OpenDatabase(App.Path + "\bprint.mdb", False, False)


' LOAD PROGRAM SETTINGS
If INIGetSetting(App.Title, "Settings", "Setup", "notset") = "notset" Then
    ' program never run before
    MsgBox "Please set the printer configuration.", vbExclamation
    INISaveSetting App.Title, "Settings", "Setup", "SET"
    INISaveSetting App.Title, "Settings", "Details", "True"
    INISaveSetting App.Title, "Settings", "Name", "<printer name>"
    INISaveSetting App.Title, "Settings", "UNCPath", "<file name>"
    INISaveSetting App.Title, "Settings", "Time", "5"
    INISaveSetting App.Title, "Settings", "Port", "COM4"
    INISaveSetting App.Title, "Settings", "AFIData", "\\exchange1\database\data\AIMSdata.mdb"
    bolSetup = True
    frmStatus.Visible = True
Else
    'program run, start up printer
    prtName = Trim(INIGetSetting(App.Title, "Settings", "Name", "default"))
    prtUNCPath = Trim(INIGetSetting(App.Title, "Settings", "UNCPath", "default"))
    prtTIme = Trim(INIGetSetting(App.Title, "Settings", "Time", "5"))
    prtPort = INIGetSetting(App.Title, "Settings", "Port", "COM3")
    prtAFIData = INIGetSetting(App.Title, "Settings", "AFIData", "\\exchange1\database\data\AIMSdata.mdb")
    If StartPrinter = False Then
        If bolDebug Then Status "StartPrinter failed from function main call."
        bolActive = False
        bolSetup = True
        Exit Sub
    End If
End If


Exit Sub
Main_Err:
Select Case Err
    Case Else
        MsgBox "There was an error in the startup procedure for the Barcode Spooler.  Error #" & Err.Number & ", " & Err.Description, vbCritical
        Exit Sub
End Select

End Sub

Public Function StartPrinter() As Boolean
On Error GoTo StartPrinter_Err

StartPrinter = False

Dim rstPrinters As Recordset

' add name to network database to allow printing
Status "Starting printer..."
If Not FileExists(prtAFIData) Then
    MsgBox "The AFI data file, '" & prtAFIData & "' was not found.  Please find the correct file then try starting the printer again.", vbExclamation
    Status "The printer was not started.", True
    Exit Function
End If
Set dbsAFI = OpenDatabase(prtAFIData, False, False)
Set rstPrinters = dbsAFI.OpenRecordset("tblBarcodePrinters", dbOpenDynaset)

With rstPrinters
    .FindFirst "PrinterName = " & Chr(34) & prtName & Chr(34)
    If .NoMatch Then
        .AddNew
        !PrinterName = prtName
        !UNCPath = prtUNCPath
        prtAFIIndex = !apkPrinter
        .Update
    Else
        .Edit
        !UNCPath = prtUNCPath
        .Update
        prtAFIIndex = !apkPrinter
    End If
    .Close
End With

bolActive = True
frmStatus.tmrCheck.Interval = prtTIme * 1000
'frmStatus.cSysTray1.TrayIcon = App.Path & "\spoola.ico"
frmStatus.cSysTray1.InTray = True
frmStatus.cSysTray1.TrayTip = prtName & " on " & prtPort

Status "Printer started.", False
StartPrinter = True

Exit Function
StartPrinter_Err:
Select Case Err
    Case Else
        LogEvent "StartPrinter", Err.Number, Err.Description
        Status "Error starting printer."
        Exit Function
End Select

End Function

Public Function StopPrinter() As Boolean
On Error GoTo StopPrinter_Err

StopPrinter = False

frmStatus.tmrCheck.Interval = 0

Status "Removing printer from available list...", True
With dbsAFI
    .Execute "Delete * From tblBarcodePrinters Where apkPrinter = " & prtAFIIndex & ";"
    .Close
End With

bolActive = False
Status "Printer " & prtName & " stopped.", True
'frmStatus.cSysTray1.TrayIcon = App.Path & "\spooli.ico"
frmStatus.cSysTray1.InTray = True
StopPrinter = True

Exit Function
StopPrinter_Err:
Select Case Err
    Case Else
        LogEvent "StopPrinter", Err.Number, Err.Description
        Status "Error stopping printer."
        Exit Function
End Select

End Function

'Display status in the visible window
Public Sub Status(NewStatus As String, Optional IsBusy As Variant)
On Error Resume Next

'Set status line
frmStatus.txtStatus.Text = NewStatus

If Not IsMissing(IsBusy) Then
    'Do busy/idle indicator
    If IsBusy = True Then
        Set frmStatus.imgStatus.Picture = frmStatus.img_Busy.Picture
    Else
        Set frmStatus.imgStatus.Picture = frmStatus.img_Idle.Picture
    End If
End If

'Add to the log box
With frmStatus.txtLog
    .SelStart = Len(.Text)
    .SelLength = 0
    .SelText = Format$(Now, "m/d h:Nn:Ss") & "  " & NewStatus & Chr(13) & Chr(10)
End With

strLastMsgTime = Format$(Now, "h:Nn:Ss")

End Sub


Public Function CheckForJob() As Boolean
On Error GoTo CheckForJob_Err

If bolDebug Then
    Status "Checking for a job..."
    'LogEvent "CheckForJob", "Checking for a job..."
End If

CheckForJob = False
Dim tJob As Recordset

Set tJob = dbsLabels.OpenRecordset("tblJob", dbOpenSnapshot)
If Not tJob.BOF Or Not tJob.EOF Then
    With tJob
        .FindFirst "JobSent = True"
        If Not .NoMatch Then CheckForJob = True
    End With
End If

tJob.Close
Set tJob = Nothing

Exit Function
CheckForJob_Err:
Select Case Err
    Case Else
        LogEvent "CheckForJob", Err.Number, Err.Description
        Status "Error checking for a job."
        Exit Function
End Select

End Function

Public Function GetCurrentJobs() As String
On Error GoTo GetCurrentJobs_Err

Dim tJobs As Recordset, tLabels As Recordset
Dim strTmp As String, curJob As Long, strSQL As String, lbls As Long

Set tJobs = dbsLabels.OpenRecordset("Select * From tblJob Where (JobSent = True) Order By apkJob;", dbOpenDynaset)
With tJobs
    ' go through each job completely sent
' only do max one job
'    Do While Not .EOF
        curJob = !apkJob
        strSQL = "Select * From tblLabel Where (afkJob = " & curJob & ") Order By apkLabel;"
        ' select labels for current job
        Set tLabels = dbsLabels.OpenRecordset(strSQL, dbOpenSnapshot)
        tLabels.MoveLast
        lbls = tLabels.RecordCount
        If lbls = 50 Then bolMaxRecs = True
        tLabels.MoveFirst
        With tLabels
            Do While Not .EOF
                strTmp = strTmp + !strLabel + vbCrLf
                .MoveNext
            Loop
            .Close
            Set tLabels = Nothing
        End With
        ' done with current job, delete and move on
        .Delete
        .MoveNext
'    Loop
    .Close
End With
Set tJobs = Nothing

GetCurrentJobs = strTmp

Exit Function
GetCurrentJobs_Err:
Select Case Err
    Case Else
        LogEvent "GetCurrentJobs", Err.Number, Err.Description
        Status "Error retreiving current jobs."
        GetCurrentJobs = ""
        Exit Function
End Select

End Function

Public Function Spooler()
On Error GoTo Spooler_Err

Dim strCur As String

If bolActive = False Then Exit Function

If bolDebug Then Status "Checking for a new job..."

If CheckForJob Then
    Open App.Path & "\output.txt" For Output As 42
    strCur = GetCurrentJobs
    If strCur = "" Then Exit Function
    Print #42, strCur
    Close 42
    If SendPrintJob(strCur) = False Then
        Status "The print job sent was aborted."
    Else
        Status "The job was sent to the printer."
    End If
    If bolMaxRecs Then
        bolMaxRecs = False
        Status "Waiting 10 seconds for a large print job to complete..."
        Call Wait("Waiting for large print job to complete...", 10)
    End If
Else
    If bolDebug Then
        Status "No new jobs found."
    End If
End If

Exit Function
Spooler_Err:
Select Case Err
    Case Else
        LogEvent "Spooler", Err.Number, Err.Description
        Status "Error in spooler function."
        Exit Function
End Select

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
On Error GoTo FileExists_Err

    Dim intFileNum As Integer

    '
    'Remove any trailing directory separator character
    '
    If Right$(strPathName, 1) = "\" Then
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


Public Function SendPrintJob(strJob As String) As Boolean
On Error GoTo SendPrintJob_Err

SendPrintJob = False

Status "Starting print job..."

Dim fNum As Long, i As Long, fJob As Long, x As Long

' check for files, delete if necessary
If FileExists(App.Path & "\jobdone.txt") Then Kill App.Path & "\jobdone.txt"
If FileExists(App.Path & "\sendjob.bat") Then Kill App.Path & "\sendjob.bat"

' set up batch file
Dim strLine As String
fNum = FreeFile
Open App.Path & "\sendjob.bat" For Output As fNum
Print #fNum, "@echo off"
Print #fNum, "mode " & prtPort & ":9600,n,8,1"

' parse strJob into lines
Dim tmp As String, curPos As Long, curLineStart As Long
Dim curChar As String, curLine As String
curLineStart = 1
For curPos = 1 To Len(strJob)
    curChar = Mid$(strJob, curPos, 1)
    If curChar = vbLf Then
        tmp = "echo " & curLine & " > " & prtPort
        Print #fNum, tmp
        curLine = ""
    ElseIf curChar = vbCr Then
        ' ignore this character
    Else
        curLine = curLine + curChar
    End If
Next curPos

Print #fNum, "echo Spool done. (this is a temp file) > " & Chr(34) & App.Path & "\jobdone.txt" & Chr(34)
Close fNum

' now shell it out
If bolDebug Then
    x = Shell(Chr(34) & App.Path & "\prompt.pif" & Chr(34) & "  /k " & Chr(34) & App.Path & "\sendjob.bat" & Chr(34), vbNormalFocus)
'    x = Shell("command.com  /k " & Chr(34) & App.Path & "\sendjob.bat" & Chr(34), vbNormalFocus)
Else
    x = Shell(Chr(34) & App.Path & "\prompt.pif" & Chr(34) & "  /c " & Chr(34) & App.Path & "\sendjob.bat" & Chr(34), vbHide)
End If

' now wait up to 2 minutes
For i = 1 To 120
    Sleep (1000)        ' sleep one second
    DoEvents
    If FileExists(App.Path & "\jobdone.txt") Then
        Status "Print job sent."
        SendPrintJob = True
        Exit Function
    End If
Next i

Status "The print job timed out after two minutes."

Exit Function
SendPrintJob_Err:
Select Case Err
    Case Else
        LogEvent "SendPrintJob", Err.Number, Err.Description
        Status "Error in sending print job."
        Exit Function
End Select

End Function

Public Function LogEvent(sSection$, sDesc$, Optional iError, Optional bHalt)
On Error Resume Next

If iError = "" Then iError = 0
If bHalt = "" Then bHalt = False

If bolDebug Then
    MsgBox "Error logged in " & sSection & ", " & sDesc & ": " & Error(iError), vbCritical
End If

Dim tErr As Recordset
Set tErr = dbsLabels.OpenRecordset("tblLog", dbOpenDynaset)
With tErr
    .AddNew
    !LogTime = Now
    !LogSection = sSection
    !LogDescription = sDesc
    !LogErrorNum = iError
    .Update
    .Close
End With
Set tErr = Nothing

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

