VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{C7EDAA3D-6A82-11D2-8DE3-00403394A7F2}#1.0#0"; "SYSTRAY.OCX"
Begin VB.Form frmStatus 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Barcode Print Client"
   ClientHeight    =   5520
   ClientLeft      =   150
   ClientTop       =   720
   ClientWidth     =   4725
   ControlBox      =   0   'False
   Icon            =   "frmStatus.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5520
   ScaleWidth      =   4725
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame Frame2 
      Height          =   30
      Left            =   0
      TabIndex        =   23
      Top             =   0
      Width           =   4815
   End
   Begin VB.CheckBox chkDebug 
      Caption         =   "De&bug Mode"
      Height          =   255
      Left            =   3240
      TabIndex        =   22
      Top             =   0
      Visible         =   0   'False
      Width           =   1455
   End
   Begin VB.CommandButton cmdSaveLog 
      Caption         =   "Save &Log"
      Height          =   255
      Left            =   3480
      TabIndex        =   21
      Top             =   3840
      Width           =   975
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   2040
      Top             =   840
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DefaultExt      =   "mdb"
      DialogTitle     =   "Find database..."
      Filter          =   "*.*"
   End
   Begin VB.Timer tmrCheck 
      Left            =   1440
      Top             =   840
   End
   Begin VB.Frame Frame1 
      Caption         =   "Settings"
      Height          =   2295
      Left            =   120
      TabIndex        =   5
      Top             =   1440
      Width           =   4335
      Begin VB.CommandButton cmdAFIData 
         Caption         =   "..."
         Height          =   315
         Left            =   3840
         TabIndex        =   19
         Top             =   1800
         Width           =   375
      End
      Begin VB.TextBox txtAFIData 
         BackColor       =   &H8000000F&
         Height          =   315
         Left            =   120
         Locked          =   -1  'True
         TabIndex        =   18
         Top             =   1800
         Width           =   3615
      End
      Begin VB.TextBox txtTime 
         Alignment       =   2  'Center
         BackColor       =   &H8000000F&
         Height          =   315
         Left            =   3000
         Locked          =   -1  'True
         TabIndex        =   16
         Top             =   1200
         Width           =   735
      End
      Begin VB.CommandButton cmdTime 
         Caption         =   "..."
         Height          =   315
         Left            =   3840
         TabIndex        =   15
         Top             =   1200
         Width           =   375
      End
      Begin VB.TextBox txtUNCPath 
         BackColor       =   &H8000000F&
         Height          =   315
         Left            =   120
         Locked          =   -1  'True
         TabIndex        =   13
         Top             =   1200
         Width           =   2175
      End
      Begin VB.CommandButton cmdUNCPath 
         Caption         =   "..."
         Height          =   315
         Left            =   2400
         TabIndex        =   12
         Top             =   1200
         Width           =   375
      End
      Begin VB.TextBox txtPort 
         BackColor       =   &H8000000F&
         Height          =   315
         Left            =   3000
         Locked          =   -1  'True
         TabIndex        =   10
         Top             =   600
         Width           =   735
      End
      Begin VB.CommandButton cmdPort 
         Caption         =   "..."
         Height          =   315
         Left            =   3840
         TabIndex        =   9
         Top             =   600
         Width           =   375
      End
      Begin VB.CommandButton cmdName 
         Appearance      =   0  'Flat
         Caption         =   "..."
         Height          =   315
         Left            =   2400
         TabIndex        =   7
         Top             =   600
         Width           =   375
      End
      Begin VB.TextBox txtName 
         BackColor       =   &H8000000F&
         Height          =   315
         Left            =   120
         Locked          =   -1  'True
         TabIndex        =   6
         Top             =   600
         Width           =   2175
      End
      Begin VB.Label lblAFIData 
         BackStyle       =   0  'Transparent
         Caption         =   "AFI Data database"
         Height          =   255
         Left            =   120
         TabIndex        =   20
         Top             =   1560
         Width           =   2655
      End
      Begin VB.Label Label6 
         BackStyle       =   0  'Transparent
         Caption         =   "Query Time"
         Height          =   255
         Left            =   3000
         TabIndex        =   17
         Top             =   960
         Width           =   855
      End
      Begin VB.Label Label5 
         BackStyle       =   0  'Transparent
         Caption         =   "UNC Path"
         Height          =   255
         Left            =   120
         TabIndex        =   14
         Top             =   960
         Width           =   2655
      End
      Begin VB.Label Label4 
         BackStyle       =   0  'Transparent
         Caption         =   "Com Port"
         Height          =   255
         Left            =   3000
         TabIndex        =   11
         Top             =   360
         Width           =   855
      End
      Begin VB.Label Label3 
         BackStyle       =   0  'Transparent
         Caption         =   "Name"
         Height          =   255
         Left            =   120
         TabIndex        =   8
         Top             =   360
         Width           =   1815
      End
   End
   Begin VB.TextBox txtLog 
      BackColor       =   &H8000000F&
      Height          =   1335
      Left            =   120
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   3
      Top             =   4080
      Width           =   4455
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Details >>"
      Height          =   375
      Left            =   3480
      TabIndex        =   2
      Top             =   840
      Width           =   975
   End
   Begin VB.TextBox txtStatus 
      BackColor       =   &H8000000F&
      BorderStyle     =   0  'None
      Height          =   285
      Left            =   720
      Locked          =   -1  'True
      TabIndex        =   1
      Text            =   "Loading..."
      Top             =   360
      Width           =   3615
   End
   Begin SysTrayCtl.cSysTray cSysTray1 
      Left            =   2760
      Top             =   720
      _ExtentX        =   900
      _ExtentY        =   900
      InTray          =   0   'False
      TrayIcon        =   "frmStatus.frx":0442
      TrayTip         =   "Barcode Spooler"
   End
   Begin VB.Label Label2 
      BackStyle       =   0  'Transparent
      Caption         =   "Event Log"
      Height          =   255
      Left            =   120
      TabIndex        =   4
      Top             =   3840
      Width           =   855
   End
   Begin VB.Image img_Busy 
      Height          =   480
      Left            =   840
      Picture         =   "frmStatus.frx":0894
      Top             =   1320
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image img_Idle 
      Height          =   480
      Left            =   120
      Picture         =   "frmStatus.frx":0CD6
      Top             =   1320
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image imgStatus 
      Height          =   480
      Left            =   120
      Picture         =   "frmStatus.frx":1118
      Top             =   120
      Width           =   480
   End
   Begin VB.Label Label1 
      Caption         =   "Status"
      Height          =   255
      Left            =   720
      TabIndex        =   0
      Top             =   120
      Width           =   1095
   End
   Begin VB.Menu mnuCommands 
      Caption         =   "&Commands"
      Begin VB.Menu mnuStartStop 
         Caption         =   "&Start/Stop Printer"
      End
      Begin VB.Menu mnuSendTest 
         Caption         =   "Send &Test Print"
      End
      Begin VB.Menu mnuShowHide 
         Caption         =   "Show/&Hide Monitor"
      End
      Begin VB.Menu mnuBar1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuAbout 
         Caption         =   "&About..."
      End
      Begin VB.Menu mnuBar2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit Spooler"
      End
   End
End
Attribute VB_Name = "frmStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub chkDebug_Click()

If chkDebug.Value = vbChecked Then
    bolDebug = True
Else
    bolDebug = False
End If

End Sub

Private Sub cmdAFIData_Click()

If bolActive Then
    MsgBox "Please stop the printer before changing settings.", vbExclamation
    Exit Sub
End If

With cdlg
    .DialogTitle = "Find AFI data database..."
    .FileName = prtAFIData
    .ShowOpen
End With

INISaveSetting App.Title, "Settings", "AFIData", cdlg.FileName
txtAFIData = cdlg.FileName
prtAFIData = txtAFIData

End Sub


Private Sub cmdName_Click()
    
    If bolActive Then
        MsgBox "Please stop the printer before changing settings.", vbExclamation
        Exit Sub
    End If
    
    frmNewName.Show 1
    If frmNewName.LoginSucceeded Then
        txtName.Text = frmNewName.txtUserName.Text
        prtName = txtName.Text
    End If
    Unload frmNewName
    
End Sub

Private Sub cmdPort_Click()

    If bolActive Then
        MsgBox "Please stop the printer before changing settings.", vbExclamation
        Exit Sub
    End If
    
    frmNewPort.Show 1
    If frmNewPort.LoginSucceeded Then
        txtPort.Text = frmNewPort.txtUserName
        prtPort = txtPort.Text
    End If
    Unload frmNewPort
    
End Sub

Private Sub cmdSaveLog_Click()

Open App.Path & "\Event Log.txt" For Output As 42
Print #42, Me.txtLog.Text
Close 42

MsgBox "The event log has been saved as '" & App.Path & "\Event Log.txt'.", vbInformation

End Sub

Private Sub cmdTime_Click()

    If bolActive Then
        MsgBox "Please stop the printer before changing settings.", vbExclamation
        Exit Sub
    End If
    
    frmNewTime.Show 1
    If frmNewTime.LoginSucceeded Then
        txtTime.Text = frmNewTime.txtUserName.Text
        prtTIme = txtTime.Text
    End If
    Unload frmNewTime
    
End Sub

Private Sub cmdUNCPath_Click()

    If bolActive Then
        MsgBox "Please stop the printer before changing settings.", vbExclamation
        Exit Sub
    End If
    
    frmNewUNCPath.Show 1
    If frmNewUNCPath.LoginSucceeded Then
        txtUNCPath.Text = frmNewUNCPath.txtUserName.Text
        prtUNCPath = Val(txtUNCPath.Text)
    End If
    Unload frmNewUNCPath
    
End Sub

Private Sub Command1_Click()
'Move the bottom of the form up/down, change button text

If Command1.Caption = "Details >>" Then
    Height = 6150
    Command1.Caption = "<< Details"
    INISaveSetting App.Title, "Settings", "Details", "True"
Else
    Height = 1900
    Command1.Caption = "Details >>"
    INISaveSetting App.Title, "Settings", "Details", "False"
End If

End Sub

Private Sub cSysTray1_MouseDblClick(Button As Integer, Id As Long)

If Me.Visible = True Then
    Me.Visible = False
Else
    Me.Visible = True
End If


End Sub

Private Sub cSysTray1_MouseMove(Id As Long)

frmStatus.cSysTray1.TrayTip = prtName & " on " & prtPort & ", Status: " & strLastMsgTime & ": " & Me.txtStatus

End Sub

Private Sub cSysTray1_MouseUp(Button As Integer, Id As Long)

    ' SetForegroundWindow and PostMessage (WM_USER) must wrap all popup menu's _
      in order to work correctely with the Notification Icons... _
      (* see KB article Q1357888 for more info *)
    SetForegroundWindow Me.hwnd                     ' Set current window as ForegroundWindow
    
    Select Case Button                              ' Track mouse clicks...
    Case vbRightButton
        Me.PopupMenu mnuCommands, vbPopupMenuRightButton  ' Popup memu...
    End Select
    
    PostMessage Me.hwnd, WM_USER, 0&, 0&            ' Update form...
    
End Sub

Private Sub Form_Load()
On Error GoTo Form_Load_Err

Me.Left = INIGetSetting(App.Title, "Settings", "MainLeft", 1000)
Me.Top = INIGetSetting(App.Title, "Settings", "MainTop", 1000)

If bolDebug Then
    chkDebug.Value = vbChecked
End If

cSysTray1.InTray = True

txtName.Text = prtName
txtPort.Text = prtPort
txtUNCPath.Text = prtUNCPath
txtTime.Text = prtTIme
txtAFIData.Text = prtAFIData

If Not InStr(Command, "/service") Then
    Me.Visible = True
End If

If bolSetup Then
    Height = 6150
    Command1.Caption = "<< Details"
    INISaveSetting App.Title, "Settings", "Details", "True"
    Me.Command1.Enabled = False
    Me.mnuStartStop.Enabled = False
    Me.mnuShowHide.Enabled = False
    Me.mnuSendTest.Enabled = False
    Me.txtStatus = "Setup Mode"
    MsgBox "After entering the settings, you must close and reopen the spooler program.", vbInformation
    Me.Caption = "Barcode Print Client Setup"
Else
    If INIGetSetting(App.Title, "Settings", "Details", "") = "True" Then
        Height = 6150
        Command1.Caption = "<< Details"
    Else
        Height = 1900
        Command1.Caption = "Details >>"
    End If
    Me.Caption = "Barcode Print Client: " & prtName
End If

Exit Sub
Form_Load_Err:
Select Case Err
    Case Else
        Status "Error #" & Err.Number & " loading form, " & Err.Description
        Exit Sub
End Select
End Sub

Private Sub Form_Unload(Cancel As Integer)
On Error GoTo QueryUnload_Error

If Me.WindowState <> vbMinimized And Me.Visible Then
    INISaveSetting App.Title, "Settings", "MainLeft", Me.Left
    INISaveSetting App.Title, "Settings", "MainTop", Me.Top
End If

If bolActive = False Then Exit Sub

If MsgBox("Are you sure you wish to stop services to this printer, " & prtName & "?", vbQuestion Or vbYesNo) = vbNo Then
    'Cancel unload
    Cancel = True
Else
    'Inform server of printer status
    If StopPrinter = False Then
        MsgBox "The printer could not shut down.", vbCritical
        Exit Sub
    End If
    ' remove temp files which allows for complete app uninstall
    If FileExists(App.Path & "\jobdone.txt") Then Kill App.Path & "\jobdone.txt"
    If FileExists(App.Path & "\sendjob.bat") Then Kill App.Path & "\sendjob.bat"
    End
End If

dbsLabels.Close

Exit Sub

QueryUnload_Error:
MsgBox "Error shutting down, " & Err.Number & ", " & Err.Description, vbCritical
Exit Sub
End Sub

Private Sub mnuAbout_Click()
frmAbout.Show
End Sub

Private Sub mnuExit_Click()
On Error Resume Next
Form_Unload (False)
End
End Sub

Private Sub mnuSendTest_Click()

If bolSetup Then
    MsgBox "A test print job can't be sent in setup mode.", vbCritical
    Exit Sub
End If

If Not bolActive Then
    MsgBox "The printer must be started to send a print job.", vbExclamation
    Exit Sub
End If

Dim strTest As String

strTest = Chr(2) + "L" + vbCrLf
strTest = strTest + "H20" + vbCrLf
strTest = strTest + "PF" + vbCrLf
strTest = strTest + "SF" + vbCrLf
strTest = strTest + "121111100000005BLI Comp" + vbCrLf
strTest = strTest + "10110000000020042" + vbCrLf
strTest = strTest + "1A3102800000090222" + vbCrLf
strTest = strTest + "121111100550005Test label" + vbCrLf
strTest = strTest + "11111110030000522222" + vbCrLf
strTest = strTest + "E" + vbCrLf

If SendPrintJob(strTest) = False Then
    MsgBox "The print job send failed.", vbExclamation
Else
    MsgBox "The print job send was succesfull.  A label should have printed...", vbInformation
End If


End Sub

Private Sub mnuShowHide_Click()
If Me.Visible = True Then
    Me.Visible = False
Else
    Me.Visible = True
End If
End Sub

Private Sub mnuStartStop_Click()

If bolActive Then
    If StopPrinter = False Then
        MsgBox "Unable to stop the printer.", vbCritical
    End If
Else
    If bolSetup Then
        MsgBox "You can't start the printer in setup mode.", vbCritical
        Exit Sub
    End If
    If StartPrinter = False Then
        MsgBox "Unable to start the printer.", vbCritical
    End If
End If

End Sub

Private Sub tmrCheck_Timer()

Spooler

End Sub
