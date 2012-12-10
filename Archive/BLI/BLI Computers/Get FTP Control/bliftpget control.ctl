VERSION 5.00
Object = "{248DD890-BB45-11CF-9ABC-0080C7E7B78D}#1.0#0"; "MSWINSCK.OCX"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.UserControl ucGetFTP 
   ClientHeight    =   1320
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4815
   PropertyPages   =   "bliftpget control.ctx":0000
   ScaleHeight     =   1320
   ScaleWidth      =   4815
   Begin VB.Timer tmrUpdateStatus 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   3000
      Top             =   2160
   End
   Begin MSComctlLib.ProgressBar ProgressBar1 
      Height          =   175
      Left            =   120
      TabIndex        =   0
      Top             =   100
      Width           =   4575
      _ExtentX        =   8070
      _ExtentY        =   291
      _Version        =   393216
      Appearance      =   1
      Scrolling       =   1
   End
   Begin MSWinsockLib.Winsock Winsock2 
      Left            =   2040
      Top             =   2160
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   393216
   End
   Begin MSWinsockLib.Winsock Winsock1 
      Left            =   2520
      Top             =   2160
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   393216
   End
   Begin VB.Label Label3 
      Caption         =   "Time Remaining:"
      Height          =   255
      Left            =   240
      TabIndex        =   6
      Top             =   840
      Width           =   1335
   End
   Begin VB.Label Label2 
      Caption         =   "Transfer Rate:"
      Height          =   255
      Left            =   240
      TabIndex        =   5
      Top             =   600
      Width           =   1335
   End
   Begin VB.Label Label1 
      Caption         =   "Progress:"
      Height          =   255
      Left            =   240
      TabIndex        =   4
      Top             =   360
      Width           =   1335
   End
   Begin VB.Label lblProgress 
      Height          =   255
      Left            =   1800
      TabIndex        =   3
      Top             =   360
      Width           =   2775
   End
   Begin VB.Label lblTransferRate 
      Height          =   255
      Left            =   1800
      TabIndex        =   2
      Top             =   600
      Width           =   2775
   End
   Begin VB.Label lblTimeRemaining 
      Height          =   375
      Left            =   1800
      TabIndex        =   1
      Top             =   840
      Width           =   2775
   End
End
Attribute VB_Name = "ucGetFTP"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "PropPageWizardRun" ,"Yes"

Private States(4) As Com  ' initialize the command/reply array
Private curState As Integer  ' tells where in the commucation process we are
Private iTotalData As Long  ' Total data to recieve
Private iCurrentData As Long  ' Current data recieved
Private iOldData As Long  ' a timer1 data value
Public sServer As String
Attribute sServer.VB_VarProcData = "Settings"
Attribute sServer.VB_VarDescription = "The server name or IP address to connect to."
Public sUserName As String
Attribute sUserName.VB_VarProcData = "Settings"
Attribute sUserName.VB_VarDescription = "User name to be used to log into the FTP server."
Public sPassword As String
Attribute sPassword.VB_VarProcData = "Settings;Text"
Attribute sPassword.VB_VarDescription = "Password to be used to log in to the remote server."
Public sLocalFile As String
Attribute sLocalFile.VB_VarProcData = "Settings"
Attribute sLocalFile.VB_VarDescription = "Name of the local file the remote file will be saved as."
Public sRemoteFile As String
Attribute sRemoteFile.VB_VarProcData = "Settings"
Attribute sRemoteFile.VB_VarDescription = "The name and location of the remote file."

Private flgDebug As Boolean
Private fDebug As frmDebug
Private flgBigFile As Boolean

Private iLocalFileNum As Long
Private iTimerTicks As Long
Private iTimerDataAmt As Long
Private dTransferRate As Double

Public Event TransferComplete()
Attribute TransferComplete.VB_Description = "Executes when the transfer in progress completes."
Public Event TransferCancelled()
Attribute TransferCancelled.VB_Description = "Executes when the transfer in progress is cancelled."
Public Event TransferFailure(sErrMsg As String)
Attribute TransferFailure.VB_Description = "Executes if there is a failure in the transfer."
Public Event ControlMessage(sMsg As String)

Public Function StartTransfer(Optional vServer, Optional vUserName, Optional vPassword, Optional vLocalFile, Optional vRemoteFile)
Attribute StartTransfer.VB_Description = "Begin the transfer operation."
On Error GoTo StartTransfer_Err
    
LogDebugMsg "StartTransfer called"

lblProgress.Caption = "Starting transfer..."

If Not IsMissing(vServer) Then sServer = vServer
If Not IsMissing(vUserName) Then sUserName = vUserName
If Not IsMissing(vPassword) Then sPassword = vPassword
If Not IsMissing(vLocalFile) Then sLocalFile = vLocalFile
If Not IsMissing(vRemoteFile) Then sRemoteFile = vRemoteFile

If sServer = "" Then
  LogDebugMsg "TransferFailure event raised"
  RaiseEvent TransferFailure("There was no server entered")
  Exit Function
End If
If sUserName = "" Then sUserName = "anonymous"
If sPassword = "" Then sPassword = "guest"
If sRemoteFile = "" Then
  LogDebugMsg "TransferFailure event raised - no remote file set"
  RaiseEvent TransferFailure("There was no remote file name entered.")
  Exit Function
End If
If sLocalFile = "" Then
  RaiseEvent TransferFailure("There was no local file name set.")
  Exit Function
End If
    
States(0).BackCode = "220" ' this is the welcome message from server
States(0).Command = "USER " + sUserName ' logges in.
States(1).BackCode = "331" ' "Username ok. Need password" from server
States(1).Command = "PASS " + sPassword ' send the password
States(2).BackCode = "230" ' "Access allowed" massage from server
States(2).Command = "TYPE I" ' Sets the type
States(3).BackCode = "200" ' "TYPE I OK" from server
States(3).Command = "PORT " ' Port command (enhanced features command button click."
States(4).BackCode = "200" ' On port OK
States(4).Command = "RETR " + sRemoteFile
Winsock1.Close
Winsock2.Close
iTimerTicks = 0

' Wait to make sure winsocks are closed before starting
LogDebugMsg "Closing winsocks..."
Do Until Winsock1.State = sckClosed And Winsock2.State = sckClosed
  DoEvents
Loop
LogDebugMsg "Winsocks closed."

Winsock1.RemoteHost = sServer
Winsock1.RemotePort = 21
Dim nr1 As Long
Dim nr2 As Long
Randomize Timer
nr1 = Int(Rnd * 126) + 1
nr2 = Int(Rnd * 255) + 1
Winsock2.LocalPort = (nr1 * 256) + nr2

Dim IP As String
IP = Winsock2.LocalIP '"128.104.54.95"  '
Do Until InStr(IP, ".") = 0
  IP = Left(IP, InStr(IP, ".") - 1) + "," + Right(IP, Len(IP) - InStr(IP, "."))
Loop

States(3).Command = "PORT " + IP + "," + Trim(Str(nr1)) + "," + Trim(Str(nr2))
Winsock2.Listen
LogDebugMsg "Winsock2 set to listen."
Winsock1.Connect
'tmrUpdateStatus.Interval = 1000
iLocalFileNum = FreeFile
Open sLocalFile For Output As #iLocalFileNum
LogDebugMsg "StartTransfer method done."

Exit Function
StartTransfer_Err:
RaiseEvent TransferFailure("Error in StartTransfer: (" & Err.Number & ") " & Err.Description)
Exit Function

End Function

Private Sub tmrUpdateStatus_Timer()
On Error GoTo tmrUpdateStatus_Err

Dim iDataLeft As Long, dSecsElapsed As Double
    
If Winsock1.State = sckClosed Then Exit Sub

iTimerTicks = iTimerTicks + 1
dSecsElapsed = iTimerTicks * (tmrUpdateStatus.Interval / 10000)
  
'LogDebugMsg "tmrUpdateStatus fire, secs: " & dSecsElapsed & " ticks: " & iTimerTicks

lblTransferRate = FormatSize((iCurrentData / 1024) / dSecsElapsed) & "/sec"
iDataLeft = iTotalData - iCurrentData
dTransferRate = iCurrentData / dSecsElapsed
iTimerDataAmt = iCurrentData
If iCurrentData > 0 Then
  lblTimeRemaining.Caption = FormatTime(iTotalData * dSecsElapsed / iCurrentData) & " remaining"
End If
If dSecsElapsed Mod 30 = 0 Then
  LogDebugMsg "Seconds elapsed: " & dSecsElapsed
End If

If dSecsElapsed Mod 10 = 0 Then
  Refresh
End If

Exit Sub
tmrUpdateStatus_Err:
RaiseEvent TransferFailure("tmrUpdateStatus: " & Err.Number & ", " & Err.Description)
Exit Sub
End Sub

Private Sub UserControl_Initialize()
ProgressBar1.Value = 0
lblProgress.Caption = ""
lblTransferRate.Caption = ""
lblTimeRemaining.Caption = ""
flgBigFile = False
Winsock1.Close
Winsock2.Close
End Sub

Private Sub UserControl_Terminate()
On Error GoTo UserControl_Terminate_Err
If Not (fDebug Is Nothing) Then
  fDebug.Visible = False
  Set fDebug = Nothing
End If

Exit Sub
UserControl_Terminate_Err:
RaiseEvent ControlMessage("UserControl_Terminate error: " & Err.Number & ", " & Err.Description)
Exit Sub
End Sub

Private Sub Winsock1_DataArrival(ByVal bytesTotal As Long)
' handles the ftp connection
On Error GoTo Winsock1_DA_Err

Dim tmpS As String
LogDebugMsg "State: " & curState & ": Winsock1_DataArrival (from remote host)"
Winsock1.GetData tmpS, , bytesTotal

If curState < 5 Then
  If Left(tmpS, 4) = "230-" Then
    'ignore this, this is a welcome message
  ElseIf Left(tmpS, 3) = States(curState).BackCode Then
    Winsock1.SendData States(curState).Command + Chr(13) + Chr(10)
    curState = curState + 1
  Else
    LogDebugMsg "Wsck1_DA (" & curState & "): String: " & tmpS
    LogDebugMsg "  Expected: " & States(curState).BackCode
    LogDebugMsg "  Received: " & tmpS
    LogDebugMsg "  Command : " & States(curState).Command
    RaiseEvent TransferFailure("Error! " + Left(tmpS, Len(tmpS) - 2))
  End If
ElseIf curState = 6 Then
  LogDebugMsg "Current data: " & iCurrentData
  LogDebugMsg "Total data: " & iTotalData
    Print #iLocalFileNum, tmpS;
'  If iCurrentData >= iTotalData Then
    tmrUpdateStatus.Enabled = False
    lblProgress.Caption = "Download complete."
    LogDebugMsg "TransferComplete event raised"
    Winsock2.Close
    LogDebugMsg "Closing open file number " & iLocalFileNum
    Close #iLocalFileNum
    RaiseEvent TransferComplete
'  End If
  
'  If lblProgress.Caption <> "Download complete." Then
'    lblProgress.Caption = "Download complete."
'    Winsock2.Close
'    LogDebugMsg "TransferComplete event raised"
'    Close #iLocalFileNum
'    RaiseEvent TransferComplete
'  End If
Else
  If Left(tmpS, 4) = "150 " Then
    iTotalData = Val(Right(tmpS, Len(tmpS) - InStr(tmpS, "(")))
    LogDebugMsg "enabling timer"
    tmrUpdateStatus.Enabled = True
    tmrUpdateStatus.Interval = 1000
  End If
  curState = curState + 1
End If

Exit Sub
Winsock1_DA_Err:
RaiseEvent TransferFailure("Winsock1_DA_Err: " & Err.Number & ", " & Err.Description)
Exit Sub

End Sub

Private Sub Winsock2_Close()
On Error GoTo Winsock2_Close_Err
' handles the data connection

Close #iLocalFileNum
Winsock1.Close

Exit Sub
Winsock2_Close_Err:
RaiseEvent ControlMessage("Winsock2_Close_Err: " & Err.Number & ", " & Err.Description)
Exit Sub
End Sub

Private Sub Winsock2_ConnectionRequest(ByVal requestID As Long)
On Error GoTo Winsock2_ConnectionReq_Err

LogDebugMsg "Winsock2_ConnectionRequest id = " & requestID

' make sure winsock is closed
Winsock2.Close
Do Until Winsock2.State = 0
  DoEvents
Loop

' now accept connection from remote server
Winsock2.Accept requestID
    
Exit Sub
Winsock2_ConnectionReq_Err:
RaiseEvent ControlMessage("Winsock2_ConnectReq_Err: " & Err.Number & ", " & Err.Description & " (reqid: " & requestID & ")")
Exit Sub
End Sub

Private Sub Winsock2_DataArrival(ByVal bytesTotal As Long)
On Error GoTo Winsock2_DA_Err:
' received data locally

Dim tmpS As String, dProgress As Double

'LogDebugMsg "Winsock2_DataArrival - local receive, state: " & Winsock2.State
If Winsock2.State <> 8 Then  'make sure connection isn't being closed
  Winsock2.GetData tmpS, , bytesTotal
  Print #iLocalFileNum, tmpS;
  iCurrentData = iCurrentData + Len(tmpS)

  dProgress = iCurrentData / iTotalData
  lblProgress.Caption = Format(dProgress, "0.0%") & "  (" & FormatSize(iCurrentData / 1024) & " / " & FormatSize(iTotalData / 1024) & ")"
  ProgressBar1.Value = dProgress * 100

  ' there is only one block, smaller then tmpS
  If iCurrentData = iTotalData Then
'  If Not flgBigFile Then
'    Winsock2.Close
'    tmrUpdateStatus.Enabled = False
'    lblProgress.Caption = "Download complete."
'    LogDebugMsg "TransferComplete event raised"
    Winsock2.Close
'    LogDebugMsg "Closing open file number " & iLocalFileNum
'    Close #iLocalFileNum
'    RaiseEvent TransferComplete
  End If
'  flgBigFile = True
End If
tmrUpdateStatus_Timer

Exit Sub
Winsock2_DA_Err:
RaiseEvent TransferFailure("Winsock2_DA_Err: " & Err.Number & ", " & Err.Description)
Exit Sub
End Sub

Public Function CancelTransfer() As Boolean
Attribute CancelTransfer.VB_Description = "Abort the current transfer"
On Error GoTo CancelTransfer_Err

CancelTransfer = False
Winsock1.Close
Winsock2.Close
iTimerTicks = 0

Do Until Winsock1.State = 0 And Winsock2.State = 0
  DoEvents
Loop

CancelTransfer = True
RaiseEvent TransferCancelled
Exit Function
CancelTransfer_Err:
RaiseEvent TransferFailure("Error cancelling, (" & Err.Number & "), " & Err.Description)
Exit Function
End Function

Public Function AboutBox()
Attribute AboutBox.VB_Description = "Information about control"
Attribute AboutBox.VB_UserMemId = -552
Dim fA As New frmAbout
fA.Show True
Set fA = Nothing
End Function

Public Function SetDebug()
flgDebug = True
Set fDebug = New frmDebug
fDebug.Visible = True
LogDebugMsg "Debugging enabled, control version " & App.Major & "." & App.Minor & "." & App.Revision
End Function


Public Function LogDebugMsg(sMsg As String)

If flgDebug Then
  fDebug.Text1.Text = fDebug.Text1 & sMsg & vbCrLf
  fDebug.Text1.SelStart = Len(fDebug.Text1.Text)
End If

End Function


