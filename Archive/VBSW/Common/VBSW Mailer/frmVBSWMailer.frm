VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "tabctl32.ocx"
Begin VB.Form frmVBSWMailer 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "VBSW Mailer"
   ClientHeight    =   4905
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6570
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4905
   ScaleWidth      =   6570
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "&Close"
      Height          =   375
      Left            =   5040
      TabIndex        =   17
      Top             =   4440
      Width           =   1095
   End
   Begin VB.CommandButton cmdSend 
      Caption         =   "&Send"
      Height          =   375
      Left            =   3840
      TabIndex        =   16
      Top             =   4440
      Width           =   1095
   End
   Begin TabDlg.SSTab tabStrip 
      Height          =   4215
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6135
      _ExtentX        =   10821
      _ExtentY        =   7435
      _Version        =   393216
      Style           =   1
      Tabs            =   4
      TabsPerRow      =   5
      TabHeight       =   520
      TabCaption(0)   =   "Mail List Recipients"
      TabPicture(0)   =   "frmVBSWMailer.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label1"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "Label2"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "Label4"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "txtConnectionString"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "cmdConnectionString"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "txtSQL"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).Control(6)=   "txtEmailField"
      Tab(0).Control(6).Enabled=   0   'False
      Tab(0).ControlCount=   7
      TabCaption(1)   =   "Message"
      TabPicture(1)   =   "frmVBSWMailer.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "Label5"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "Label6"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "Label7"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "Label8"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "txtFrom"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "txtSubject"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "txtBody"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).Control(7)=   "cmbBodyType"
      Tab(1).Control(7).Enabled=   0   'False
      Tab(1).ControlCount=   8
      TabCaption(2)   =   "Settings"
      TabPicture(2)   =   "frmVBSWMailer.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "Label3(0)"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).Control(1)=   "Label3(1)"
      Tab(2).Control(1).Enabled=   0   'False
      Tab(2).Control(2)=   "txtSMTPServer"
      Tab(2).Control(2).Enabled=   0   'False
      Tab(2).Control(3)=   "txtReplaceString"
      Tab(2).Control(3).Enabled=   0   'False
      Tab(2).ControlCount=   4
      TabCaption(3)   =   "Preview Recipients"
      TabPicture(3)   =   "frmVBSWMailer.frx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "lblRecCount"
      Tab(3).Control(0).Enabled=   0   'False
      Tab(3).Control(1)=   "lvPreview"
      Tab(3).Control(1).Enabled=   0   'False
      Tab(3).ControlCount=   2
      Begin VB.TextBox txtReplaceString 
         Height          =   285
         Left            =   -73680
         TabIndex        =   22
         Top             =   840
         Width           =   4575
      End
      Begin VB.TextBox txtEmailField 
         Height          =   285
         Left            =   1320
         TabIndex        =   19
         Top             =   2280
         Width           =   4215
      End
      Begin MSComctlLib.ListView lvPreview 
         Height          =   3375
         Left            =   -74880
         TabIndex        =   18
         Top             =   480
         Width           =   5895
         _ExtentX        =   10398
         _ExtentY        =   5953
         View            =   3
         LabelEdit       =   1
         LabelWrap       =   -1  'True
         HideSelection   =   -1  'True
         _Version        =   393217
         ForeColor       =   -2147483640
         BackColor       =   -2147483643
         BorderStyle     =   1
         Appearance      =   1
         NumItems        =   0
      End
      Begin VB.TextBox txtSMTPServer 
         Height          =   285
         Left            =   -73680
         TabIndex        =   14
         Top             =   480
         Width           =   4575
      End
      Begin VB.ComboBox cmbBodyType 
         Height          =   315
         ItemData        =   "frmVBSWMailer.frx":0070
         Left            =   -74040
         List            =   "frmVBSWMailer.frx":007A
         Style           =   2  'Dropdown List
         TabIndex        =   12
         Top             =   3720
         Width           =   4935
      End
      Begin VB.TextBox txtBody 
         Height          =   2445
         Left            =   -74040
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   10
         Top             =   1200
         Width           =   4935
      End
      Begin VB.TextBox txtSubject 
         Height          =   285
         Left            =   -74040
         TabIndex        =   8
         Top             =   840
         Width           =   4935
      End
      Begin VB.TextBox txtFrom 
         Height          =   285
         Left            =   -74040
         TabIndex        =   6
         Top             =   480
         Width           =   4935
      End
      Begin VB.TextBox txtSQL 
         Height          =   975
         Left            =   1320
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   4
         Top             =   1200
         Width           =   4215
      End
      Begin VB.CommandButton cmdConnectionString 
         Caption         =   "..."
         Height          =   375
         Left            =   5640
         TabIndex        =   3
         Top             =   480
         Width           =   375
      End
      Begin VB.TextBox txtConnectionString 
         Height          =   615
         Left            =   1320
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   1
         Top             =   480
         Width           =   4215
      End
      Begin VB.Label Label3 
         Caption         =   "Replace:"
         Height          =   255
         Index           =   1
         Left            =   -74880
         TabIndex        =   23
         Top             =   840
         Width           =   1215
      End
      Begin VB.Label lblRecCount 
         Height          =   255
         Left            =   -74760
         TabIndex        =   21
         Top             =   3840
         Width           =   5535
      End
      Begin VB.Label Label4 
         Caption         =   "Email Field:"
         Height          =   255
         Left            =   120
         TabIndex        =   20
         Top             =   2280
         Width           =   975
      End
      Begin VB.Label Label3 
         Caption         =   "SMTP Server:"
         Height          =   255
         Index           =   0
         Left            =   -74880
         TabIndex        =   15
         Top             =   480
         Width           =   1215
      End
      Begin VB.Label Label8 
         Caption         =   "Format:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   13
         Top             =   3720
         Width           =   735
      End
      Begin VB.Label Label7 
         Caption         =   "Body:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   11
         Top             =   1200
         Width           =   855
      End
      Begin VB.Label Label6 
         Caption         =   "Subject:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   9
         Top             =   840
         Width           =   855
      End
      Begin VB.Label Label5 
         Caption         =   "From:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   7
         Top             =   480
         Width           =   855
      End
      Begin VB.Label Label2 
         Caption         =   "SQL:"
         Height          =   615
         Left            =   120
         TabIndex        =   5
         Top             =   1200
         Width           =   975
      End
      Begin VB.Label Label1 
         Caption         =   "Connection String:"
         Height          =   495
         Left            =   120
         TabIndex        =   2
         Top             =   480
         Width           =   975
      End
   End
End
Attribute VB_Name = "frmVBSWMailer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdClose_Click()

  Unload Me
  
End Sub

Private Sub cmdConnectionString_Click()

  Dim objDL As MSDASC.DataLinks
  Dim objCon As ADODB.Connection
  
  Set objDL = New MSDASC.DataLinks
  
  'Display data link dialog
  If Me.txtConnectionString = "" Then
    Set objCon = objDL.PromptNew
    If objCon Is Nothing Then
      Set objDL = Nothing
      Exit Sub
    End If
  Else
    Set objCon = New ADODB.Connection
    objCon.ConnectionString = Me.txtConnectionString
    If Not objDL.PromptEdit(objCon) Then
      Set objDL = Nothing
      Exit Sub
    End If
  End If
  
  Me.txtConnectionString = objCon.ConnectionString
  
  Set objCon = Nothing
  Set objDL = Nothing
  
End Sub

Private Sub cmdSend_Click()

  If txtConnectionString.Text = "" Then
    MsgBox "You must specify the connection string!", vbExclamation
    tabStrip.Tab = 0
    cmdConnectionString.SetFocus
    Exit Sub
  End If
  
  If txtSQL.Text = "" Then
    MsgBox "You must specify the SQL statement!", vbExclamation
    tabStrip.Tab = 0
    txtSQL.SetFocus
    Exit Sub
  End If
  
  If txtEmailField.Text = "" Then
    MsgBox "You must specify the email field name!", vbExclamation
    tabStrip.Tab = 0
    txtEmailField.SetFocus
    Exit Sub
  End If
  
  If txtFrom.Text = "" Then
    MsgBox "You must specify who the email is from!", vbExclamation
    tabStrip.Tab = 1
    txtFrom.SetFocus
    Exit Sub
  End If
  
  If txtSubject.Text = "" Then
    MsgBox "You must specify a subject!", vbExclamation
    tabStrip.Tab = 1
    txtSubject.SetFocus
    Exit Sub
  End If
  
  If Me.txtBody.Text = "" Then
    MsgBox "You must specify a message body!", vbExclamation
    tabStrip.Tab = 1
    Me.txtBody.SetFocus
    Exit Sub
  End If
  
  If cmbBodyType.Text = "" Then
    MsgBox "You must specify a body type!", vbExclamation
    tabStrip.Tab = 1
    cmbBodyType.SetFocus
    Exit Sub
  End If
  
  If txtSMTPServer.Text = "" Then
    MsgBox "You must specify the SMTP server!", vbExclamation
    tabStrip.Tab = 2
    txtSMTPServer.SetFocus
    Exit Sub
  End If
  
  If MsgBox("Are you sure you want to send the email?", vbQuestion + vbYesNo) = vbNo Then Exit Sub
  
  Dim fStat As frmStatus, cn As ADODB.Connection, rs As ADODB.Recordset
  Dim strTempBody As String, intCurRec As Long
  Dim objSMTP As BLIMAILLib.SMTP
  
  Set fStat = New frmStatus
  Set cn = New ADODB.Connection
  Set rs = New ADODB.Recordset
  Set objSMTP = New BLIMAILLib.SMTP
  
  cn.Open txtConnectionString.Text
  rs.Open txtSQL.Text, cn, adOpenStatic, adLockReadOnly
  rs.MoveLast
  rs.MoveFirst
  
  If MsgBox("You will be sending this message to " & rs.RecordCount & " recipients.  Are you sure this is okay?", vbOKCancel + vbQuestion) = vbCancel Then
    GoTo CloseObjects
  End If
  
  
  fStat.pbar.Max = rs.RecordCount
  fStat.lblStatus.Caption = "Sending mail..."
  fStat.Left = Me.Left + (Me.Width / 2) - fStat.Width / 2
  fStat.Top = Me.Top + (Me.Height / 2) - fStat.Height / 2
  fStat.Visible = True
  fStat.Refresh
  
  Dim i As Long
  
  Do While Not rs.EOF
    If Len(rs.Fields(txtEmailField.Text)) > 0 Then
      strTempBody = Replace(txtBody.Text, txtReplaceString.Text, rs.Fields(txtEmailField.Text))
      objSMTP.SendMail txtSMTPServer.Text, _
                        txtFrom.Text, _
                        rs.Fields(txtEmailField.Text), _
                        txtSubject.Text, _
                        strTempBody, _
                        cmbBodyType.Text
    End If
    intCurRec = intCurRec + 1
    fStat.pbar.Value = intCurRec
    fStat.Refresh
    rs.MoveNext
  Loop
  
CloseObjects:
  fStat.Visible = False
  rs.Close
  cn.Close
  Set rs = Nothing
  Set cn = Nothing
  Unload fStat
  Set fStat = Nothing

End Sub

Private Sub Form_Load()

  ' Load things from registry
  Me.Left = GetSetting("VBSWMailer", "Main", "Left", Me.Left)
  Me.Top = GetSetting("VBSWMailer", "Main", "Top", Me.Top)
  Me.Height = GetSetting("VBSWMailer", "Main", "Height", Me.Height)
  Me.Width = GetSetting("VBSWMailer", "Main", "Width", Me.Width)
  Me.txtConnectionString.Text = GetSetting("VBSWMailer", "Main", "ConnectionString", "")
  Me.txtSQL.Text = GetSetting("VBSWMailer", "Main", "SQL", "")
  Me.txtSMTPServer.Text = GetSetting("VBSWMailer", "Main", "SMTPServer", "")
  Me.txtFrom.Text = GetSetting("VBSWMailer", "Main", "From", "")
  Me.txtEmailField.Text = GetSetting("VBSWMailer", "Main", "EmailField", "")
  Me.txtReplaceString.Text = GetSetting("VBSWMailer", "Main", "ReplaceString", "")
  
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

  If MsgBox("Are you sure you want to exit?", vbQuestion + vbYesNo) = vbNo Then
    Cancel = True
  End If
  
End Sub

Private Sub Form_Unload(Cancel As Integer)

  ' Save things to registry
  SaveSetting "VBSWMailer", "Main", "Left", Me.Left
  SaveSetting "VBSWMailer", "Main", "Top", Me.Top
  SaveSetting "VBSWMailer", "Main", "Height", Me.Height
  SaveSetting "VBSWMailer", "Main", "Width", Me.Width
  SaveSetting "VBSWMailer", "Main", "ConnectionString", Me.txtConnectionString.Text
  SaveSetting "VBSWMailer", "Main", "SQL", Me.txtSQL.Text
  SaveSetting "VBSWMailer", "Main", "SMTPServer", Me.txtSMTPServer.Text
  SaveSetting "VBSWMailer", "Main", "From", Me.txtFrom.Text
  SaveSetting "VBSWMailer", "Main", "EmailField", Me.txtEmailField.Text
  SaveSetting "VBSWMailer", "Main", "ReplaceString", Me.txtReplaceString.Text

End Sub

Private Sub tabStrip_Click(PreviousTab As Integer)

  If Me.txtConnectionString.Text = "" Then
    MsgBox "You must specify a connection before previewing the list of recipients!", vbExclamation
    tabStrip.Tab = 0
    txtConnectionString.SetFocus
    Exit Sub
  End If
  If txtSQL.Text = "" Then
    MsgBox "You must specify an SQL string before previewing the list of recipients!", vbExclamation
    tabStrip.Tab = 0
    txtSQL.SetFocus
    Exit Sub
  End If
  
  Dim cn As ADODB.Connection, rs As ADODB.Recordset
  Dim fld As Field, li As ListItem, i As Integer, cnt As Long
  
  Set cn = New ADODB.Connection
  Set rs = New ADODB.Recordset
  cn.Open Me.txtConnectionString
  rs.Open Me.txtSQL.Text, cn, adOpenForwardOnly, adLockReadOnly
    
  Me.lvPreview.ColumnHeaders.Clear
  Me.lvPreview.ListItems.Clear
  
  For Each fld In rs.Fields
    Me.lvPreview.ColumnHeaders.Add , , fld.Name
  Next fld
  
  Do While Not rs.EOF
    Set li = Me.lvPreview.ListItems.Add(, , rs.Fields(0))
    For i = 1 To rs.Fields.Count - 1
      If IsNull(rs.Fields(i)) Then
        li.SubItems(i) = ""
      Else
        li.SubItems(i) = rs.Fields(i)
      End If
    Next i
    cnt = cnt + 1
    rs.MoveNext
  Loop
  
  Me.lblRecCount.Caption = "There are " & cnt & " recipients."
  
  rs.Close
  cn.Close
  Set rs = Nothing
  Set cn = Nothing
  

End Sub
