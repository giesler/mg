VERSION 5.00
Begin VB.Form frmSQLTool 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "BLI SQL Tool"
   ClientHeight    =   1890
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4965
   Icon            =   "frmSQLTool.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1890
   ScaleWidth      =   4965
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdBrowse 
      Caption         =   "&Browse"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   4080
      TabIndex        =   1
      Top             =   600
      Width           =   855
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   495
      Left            =   120
      Picture         =   "frmSQLTool.frx":212A
      ScaleHeight     =   495
      ScaleWidth      =   615
      TabIndex        =   7
      Top             =   1320
      Width           =   615
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   3720
      TabIndex        =   4
      Top             =   1440
      Width           =   1095
   End
   Begin VB.CheckBox chkDeleteFile 
      Caption         =   "Rename .sql file when update completes (.sql.old)"
      Height          =   255
      Left            =   480
      TabIndex        =   2
      Top             =   960
      Value           =   1  'Checked
      Width           =   4095
   End
   Begin VB.TextBox txtFileName 
      Height          =   285
      Left            =   240
      TabIndex        =   0
      Top             =   600
      Width           =   3855
   End
   Begin VB.CommandButton cmdExecSQL 
      Caption         =   "&Run Update"
      Default         =   -1  'True
      Height          =   375
      Left            =   2280
      TabIndex        =   3
      Top             =   1440
      Width           =   1335
   End
   Begin VB.Label lblVer 
      Height          =   255
      Left            =   770
      TabIndex        =   8
      Top             =   1560
      Width           =   1455
   End
   Begin VB.Line Line1 
      BorderColor     =   &H80000011&
      X1              =   4920
      X2              =   0
      Y1              =   240
      Y2              =   240
   End
   Begin VB.Label lblConnection 
      ForeColor       =   &H80000011&
      Height          =   255
      Left            =   0
      TabIndex        =   6
      Top             =   0
      Width           =   4935
   End
   Begin VB.Label Label1 
      Caption         =   "Update file filename"
      Height          =   375
      Left            =   240
      TabIndex        =   5
      Top             =   360
      Width           =   4455
   End
End
Attribute VB_Name = "frmSQLTool"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim cnn As Connection

Private Sub cmdBrowse_Click()
On Error GoTo cmdBrowse_Err

Dim sFilt As String, msOf As MSA_OPENFILENAME

sFilt = MSA_ConvertFilterString("SQL Files|*.sql|All Files|*.*")
msOf.strFilter = sFilt
msOf.strDefaultExtension = "sql"
msOf.strDialogTitle = "Open an sql file"
msOf.strInitialDir = App.Path

If MSA_GetOpenFileName(msOf) Then
  Me.txtFileName = msOf.strFullPathReturned
End If
Me.cmdExecSQL.SetFocus

Exit Sub
cmdBrowse_Err:
MsgBox "Error #" & Err & ", " & Err.Description, vbExclamation
Exit Sub

End Sub

Private Sub cmdCancel_Click()
End
End Sub

Private Sub cmdExecSQL_Click()
On Error GoTo cmdExecSQL_Err

Dim sSQL As String, fn As Long, sTmp As String, fLen As Long
Dim fStat As frmStatus, iPrevPercent As Single, iVal As Integer, blnCancel As Boolean
Dim blnSkip As Boolean

Set fStat = New frmStatus
fStat.Left = Me.Left + (Me.Width / 2) - (fStat.Width / 2)
fStat.Top = Me.Top + (Me.Height / 2) - (fStat.Height / 2)
fStat.Visible = True
fStat.Refresh

fn = FreeFile
Open Me.txtFileName For Input As fn
fLen = FileLen(Me.txtFileName)

'Ignore header line
Line Input #fn, sTmp
cnn.BeginTrans
cnn.CommandTimeout = 240

Do While Not EOF(fn)
  Line Input #fn, sTmp
  iPrevPercent = fStat.pb1.Value
  iVal = Int((Loc(fn) * 128 / fLen) * 100)
  fStat.pb1.Value = IIf(iVal > 100, 100, iVal)
  If iPrevPercent <> fStat.pb1.Value Then
    fStat.lblPercent.Caption = fStat.pb1.Value & "%"
    fStat.Refresh
  End If
  If UCase(sTmp) = "GO" Then
    sSQL = Trim(sSQL)
    blnSkip = False
    On Error Resume Next
    blnSkip = True
    Do
      If Len(sSQL) > 0 Then cnn.Execute sSQL
      If Err.Number <> 0 Then
        blnCancel = HandleSQLError(sSQL, blnSkip, Err.Number, Err.Description)
        If blnCancel Then GoTo cmdExecSQL_Err
      End If
    Loop Until Not blnSkip
    On Error GoTo cmdExecSQL_Err
    sSQL = ""
  ElseIf Len(sTmp) = 0 Then
  ElseIf Left(sTmp, 2) = "/*" Then
  ElseIf InStr(UCase(sTmp), "BEGIN TRANSACTION") Then
  ElseIf InStr(UCase(sTmp), "COMMIT") Then
  Else
    sSQL = sSQL & vbCrLf & sTmp
  End If
Loop
Close fn
If Me.chkDeleteFile Then
  FileCopy Me.txtFileName, Me.txtFileName & ".old"
  Kill Me.txtFileName
End If

cnn.CommitTrans
Unload fStat
Set fStat = Nothing

MsgBox "The updates in '" & Me.txtFileName & "' were succesfully completed.", vbInformation
End

Exit Sub
cmdExecSQL_Err:
Unload fStat
Set fStat = Nothing
Open App.Path & "\sqltool.log" For Output As 42
Print #42, "Error: " & Err & ", " & Err.Description
Print #42, "Current SQL: " & sSQL
MsgBox "There was an error in the update: " & Err & ", " & Err.Description, vbExclamation
On Error Resume Next
Dim e As Error
For Each e In cnn.Errors
  Print #42, "ADO Error #" & e.Number & ", " & e.Description & ", " & e.Source
Next e
cnn.RollbackTrans
Close 42
Close fn
Exit Sub

End Sub

Private Sub Form_Load()
On Error GoTo Form_Load_Err

Me.lblVer.Caption = "Version " & App.Major & "." & Format(App.Minor, "00")
If App.Revision <> 0 Then
  Me.lblVer.Caption = Me.lblVer.Caption & "." & Format(App.Revision, "00")
End If

PrivIniRegister "SQLSettings", App.Path & "\sqltool.ini"

Dim sConn As String, sServer As String, sDatabase As String
Dim sUser As String, sPassword As String

sServer = PrivGetString("ServerName", "none")
If sServer = "none" Then PrivPutString "ServerName", "none"

sDatabase = PrivGetString("Database", "none")
If sDatabase = "none" Then PrivPutString "Database", "none"

sUser = PrivGetString("UserName", "none")
If sUser = "none" Then PrivPutString "UserName", "none"

sPassword = PrivGetString("Password", "none")
If sPassword = "none" Then PrivPutString "Password", "none"
If sPassword = "none" Then sPassword = ""

sConn = "Provider=SQLOLEDB;Data Source=" & sServer & ";"
sConn = sConn & "Initial Catalog=" & sDatabase & ";"
sConn = sConn & "User ID=" & sUser & ";"
sConn = sConn & "Password=" & sPassword & ";"

Set cnn = New Connection
cnn.Open sConn

Me.lblConnection.Caption = "Connected to " & sServer & ", database " & sDatabase
Me.txtFileName.Text = Replace(Command, """", "")

Exit Sub
Form_Load_Err:
MsgBox "Error loading, " & Err & ", " & Err.Description & vbCrLf & "Connection string: " & sConn, vbExclamation
Exit Sub

End Sub

Private Sub Form_Unload(Cancel As Integer)
On Error GoTo Form_Unload_Err

cnn.Close

Exit Sub
Form_Unload_Err:
MsgBox "Error unloading, " & Err & ", " & Err.Description, vbExclamation
Exit Sub

End Sub

