VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Begin VB.Form frmDBOps 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Database Operations"
   ClientHeight    =   3330
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7215
   Icon            =   "frmDBOps.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3330
   ScaleWidth      =   7215
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdAbout 
      Caption         =   "&About..."
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   3000
      Width           =   855
   End
   Begin MSComDlg.CommonDialog cmdlg1 
      Left            =   2280
      Top             =   2880
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DialogTitle     =   "Find a database"
      Filter          =   "Access Databases (*.md?)|*.md?|All Files (*.*)|*.*"
   End
   Begin VB.CommandButton cmdExit 
      Cancel          =   -1  'True
      Caption         =   "E&xit"
      Height          =   375
      Left            =   5880
      TabIndex        =   0
      Top             =   2880
      Width           =   1215
   End
   Begin MSComDlg.CommonDialog cmdlg2 
      Left            =   2880
      Top             =   2880
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DefaultExt      =   "mdb"
      DialogTitle     =   "Find a database"
      Filter          =   "Access Databases (*.md?)|*.md?|All Files (*.*)|*.*"
   End
   Begin TabDlg.SSTab SSTab1 
      Height          =   2655
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   6975
      _ExtentX        =   12303
      _ExtentY        =   4683
      _Version        =   393216
      Style           =   1
      Tabs            =   5
      TabsPerRow      =   5
      TabHeight       =   520
      TabCaption(0)   =   "&Compact"
      TabPicture(0)   =   "frmDBOps.frx":0442
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label4"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "Label3"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "chkBackupCompact"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "cmdBrowseC1"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "cmdCompactDB"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "txtCompact"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).ControlCount=   6
      TabCaption(1)   =   "&Synchronize"
      TabPicture(1)   =   "frmDBOps.frx":045E
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "Label5"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "cmdSynchronize"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "chkBackupDBS2"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "chkBackupDBS1"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "chkCompDBS2"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "chkCompDBS1"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "txtDBS2"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).Control(7)=   "cmdBrowseDBS2"
      Tab(1).Control(7).Enabled=   0   'False
      Tab(1).Control(8)=   "txtDBS1"
      Tab(1).Control(8).Enabled=   0   'False
      Tab(1).Control(9)=   "cmdBrowseDBS1"
      Tab(1).Control(9).Enabled=   0   'False
      Tab(1).ControlCount=   10
      TabCaption(2)   =   "&Replicate"
      TabPicture(2)   =   "frmDBOps.frx":047A
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "Label1"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).Control(1)=   "cmdBrowseR1"
      Tab(2).Control(1).Enabled=   0   'False
      Tab(2).Control(2)=   "txtDBR1"
      Tab(2).Control(2).Enabled=   0   'False
      Tab(2).Control(3)=   "cmdBrowseR2"
      Tab(2).Control(3).Enabled=   0   'False
      Tab(2).Control(4)=   "txtDBR2"
      Tab(2).Control(4).Enabled=   0   'False
      Tab(2).Control(5)=   "chkCompactR1"
      Tab(2).Control(5).Enabled=   0   'False
      Tab(2).Control(6)=   "chkCompactR2"
      Tab(2).Control(6).Enabled=   0   'False
      Tab(2).Control(7)=   "cmdReplicate"
      Tab(2).Control(7).Enabled=   0   'False
      Tab(2).ControlCount=   8
      TabCaption(3)   =   "DB &Properties"
      TabPicture(3)   =   "frmDBOps.frx":0496
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "Label2"
      Tab(3).Control(0).Enabled=   0   'False
      Tab(3).Control(1)=   "lstProps"
      Tab(3).Control(1).Enabled=   0   'False
      Tab(3).Control(2)=   "cmdProperties"
      Tab(3).Control(2).Enabled=   0   'False
      Tab(3).Control(3)=   "txtDBP"
      Tab(3).Control(3).Enabled=   0   'False
      Tab(3).Control(4)=   "cmdBrowseP"
      Tab(3).Control(4).Enabled=   0   'False
      Tab(3).ControlCount=   5
      TabCaption(4)   =   "S&QL"
      TabPicture(4)   =   "frmDBOps.frx":04B2
      Tab(4).ControlEnabled=   0   'False
      Tab(4).ControlCount=   0
      Begin VB.TextBox txtCompact 
         Height          =   285
         Left            =   240
         TabIndex        =   26
         Top             =   840
         Width           =   5535
      End
      Begin VB.CommandButton cmdCompactDB 
         Caption         =   "&Compact"
         Height          =   375
         Left            =   5400
         TabIndex        =   25
         Top             =   2160
         Width           =   1335
      End
      Begin VB.CommandButton cmdBrowseC1 
         Caption         =   "&Browse"
         Height          =   285
         Left            =   5880
         TabIndex        =   24
         Top             =   840
         Width           =   855
      End
      Begin VB.CommandButton cmdBrowseDBS1 
         Caption         =   "&Browse"
         Height          =   285
         Left            =   -69120
         TabIndex        =   23
         Top             =   840
         Width           =   855
      End
      Begin VB.TextBox txtDBS1 
         Height          =   285
         Left            =   -74760
         TabIndex        =   22
         Top             =   840
         Width           =   5535
      End
      Begin VB.CommandButton cmdBrowseDBS2 
         Caption         =   "Bro&wse"
         Height          =   285
         Left            =   -69120
         TabIndex        =   21
         Top             =   1560
         Width           =   855
      End
      Begin VB.TextBox txtDBS2 
         Height          =   285
         Left            =   -74760
         TabIndex        =   20
         Top             =   1560
         Width           =   5535
      End
      Begin VB.CheckBox chkCompDBS1 
         Caption         =   "C&ompact after Synch"
         Height          =   255
         Left            =   -72000
         TabIndex        =   19
         Top             =   1200
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.CheckBox chkCompDBS2 
         Caption         =   "Co&mpact after Synch"
         Height          =   255
         Left            =   -72000
         TabIndex        =   18
         Top             =   1920
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.CheckBox chkBackupDBS1 
         Caption         =   "&Backup to <db>.md1"
         Height          =   255
         Left            =   -74160
         TabIndex        =   17
         Top             =   1200
         Value           =   1  'Checked
         Width           =   1935
      End
      Begin VB.CheckBox chkBackupDBS2 
         Caption         =   "Bac&kup to <db>.md1"
         Height          =   255
         Left            =   -74160
         TabIndex        =   16
         Top             =   1920
         Value           =   1  'Checked
         Width           =   1935
      End
      Begin VB.CommandButton cmdSynchronize 
         Caption         =   "&Synchronize"
         Height          =   375
         Left            =   -69600
         TabIndex        =   15
         Top             =   2160
         Width           =   1335
      End
      Begin VB.CheckBox chkBackupCompact 
         Caption         =   "B&ackup to <db>.md1"
         Height          =   255
         Left            =   840
         TabIndex        =   14
         Top             =   1200
         Value           =   1  'Checked
         Width           =   1935
      End
      Begin VB.CommandButton cmdReplicate 
         Caption         =   "R&eplicate"
         Height          =   375
         Left            =   -69600
         TabIndex        =   13
         Top             =   2160
         Width           =   1335
      End
      Begin VB.CheckBox chkCompactR2 
         Caption         =   "Co&mpact after Replication"
         Height          =   255
         Left            =   -72000
         TabIndex        =   12
         Top             =   1920
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.CheckBox chkCompactR1 
         Caption         =   "C&ompact after Replication"
         Height          =   255
         Left            =   -72000
         TabIndex        =   11
         Top             =   1200
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.TextBox txtDBR2 
         Height          =   285
         Left            =   -74760
         TabIndex        =   10
         Top             =   1560
         Width           =   5535
      End
      Begin VB.CommandButton cmdBrowseR2 
         Caption         =   "Save &As"
         Height          =   285
         Left            =   -69120
         TabIndex        =   9
         Top             =   1560
         Width           =   855
      End
      Begin VB.TextBox txtDBR1 
         Height          =   285
         Left            =   -74760
         TabIndex        =   8
         Top             =   840
         Width           =   5535
      End
      Begin VB.CommandButton cmdBrowseR1 
         Caption         =   "&Browse"
         Height          =   285
         Left            =   -69120
         TabIndex        =   7
         Top             =   840
         Width           =   855
      End
      Begin VB.CommandButton cmdBrowseP 
         Caption         =   "&Browse"
         Height          =   285
         Left            =   -69120
         TabIndex        =   6
         Top             =   840
         Width           =   855
      End
      Begin VB.TextBox txtDBP 
         Height          =   285
         Left            =   -74760
         TabIndex        =   5
         Top             =   840
         Width           =   5535
      End
      Begin VB.CommandButton cmdProperties 
         Caption         =   "Pr&operties"
         Height          =   375
         Left            =   -69600
         TabIndex        =   4
         Top             =   2160
         Width           =   1335
      End
      Begin VB.ListBox lstProps 
         Height          =   1230
         ItemData        =   "frmDBOps.frx":04CE
         Left            =   -74760
         List            =   "frmDBOps.frx":04D0
         TabIndex        =   3
         Top             =   1200
         Width           =   5055
      End
      Begin VB.Label Label3 
         Caption         =   "This is the ONLY tab that uses ADO 2.5 / JRO.  It will only work with Jet 5.0 databases (ie, Access 2000 databases)"
         ForeColor       =   &H8000000D&
         Height          =   375
         Left            =   480
         TabIndex        =   31
         Top             =   1680
         Width           =   5655
      End
      Begin VB.Label Label4 
         Caption         =   "Choose the database to compact using the browse button."
         Height          =   255
         Left            =   240
         TabIndex        =   30
         Top             =   480
         Width           =   4455
      End
      Begin VB.Label Label5 
         Caption         =   "Choose the two database files to synchronize below."
         Height          =   255
         Left            =   -74880
         TabIndex        =   29
         Top             =   480
         Width           =   4455
      End
      Begin VB.Label Label1 
         Caption         =   "Enter the database to create a replica from in the first box, and the new replica in the second."
         Height          =   255
         Left            =   -74880
         TabIndex        =   28
         Top             =   480
         Width           =   6735
      End
      Begin VB.Label Label2 
         Caption         =   "Enter the database to create a replica from in the first box, and the new replica in the second."
         Height          =   255
         Left            =   -74880
         TabIndex        =   27
         Top             =   480
         Width           =   6735
      End
   End
End
Attribute VB_Name = "frmDBOps"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private Sub cmdAbout_Click()

frmAbout.Show

End Sub

Private Sub cmdBrowseC1_Click()

cmdlg1.ShowOpen
Me.txtCompact.Text = cmdlg1.FileName

End Sub

Private Sub cmdBrowseDBS1_Click()

cmdlg1.ShowOpen
Me.txtDBS1.Text = cmdlg1.FileName

End Sub

Private Sub cmdBrowseDBS2_Click()

cmdlg1.ShowOpen
Me.txtDBS2.Text = cmdlg1.FileName

End Sub

Private Sub cmdBrowseP_Click()

cmdlg1.ShowOpen
Me.txtDBP.Text = cmdlg1.FileName

End Sub

Private Sub cmdBrowseR1_Click()

cmdlg1.ShowOpen
Me.txtDBR1.Text = cmdlg1.FileName

End Sub

Private Sub cmdBrowseR2_Click()

cmdlg2.ShowSave
Me.txtDBR2.Text = cmdlg2.FileName

End Sub

Private Sub cmdCompactDB_Click()
On Error GoTo cmdCompactDB_Err

Dim txtBkName As String

If MsgBox("Are you sure you want to compact '" & Me.txtCompact & "'?", vbYesNo + vbQuestion) = vbNo Then Exit Sub

OpenWaitForm "Compacting database..."

Dim jr As jro.JetEngine
Set jr = New jro.JetEngine

txtBkName = Left$(Me.txtCompact, InStr(Me.txtCompact, ".md") - 1) & ".md1"
If FileExists(txtBkName) Then
    Kill txtBkName
End If
Name Me.txtCompact As txtBkName

jr.CompactDatabase "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & txtBkName, _
  "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.txtCompact & ";Jet OLEDB:Engine Type=5"

If Me.chkBackupCompact <> 1 Then
  Kill txtBkName
End If
CloseWaitForm

MsgBox "The database '" & txtCompact & "' has been compacted.", vbInformation

Exit Sub

cmdCompactDB_Err:
Select Case Err
    Case Else
        MsgBox "cmdSynch Error #" & Err & ", " & Err.Description, vbCritical
        Open App.Path & "\error.log" For Append As 42
        Print #42, Now & " - " & "cmdSynch Error #" & Err & ", " & Err.Description
        Close 42
        CloseWaitForm
        Exit Sub
End Select


End Sub



Private Sub cmdExit_Click()

End

End Sub


Private Sub cmdProperties_Click()
On Error Resume Next

Dim txt As Variant


Dim dbs As Database, ctr As Container, doc As Document
Dim prp As Property

Set dbs = OpenDatabase(Me.txtDBP)

lstProps.Clear

With dbs

    Set ctr = .Containers("Databases")
    Set doc = ctr.Documents("UserDefined")

    With doc
    
        For Each prp In .Properties
            lstProps.AddItem Trim(prp.Name & " - " & Trim(prp.Value))
        Next prp
        
    End With

    .Close
End With


End Sub

Private Sub cmdReplicate_Click()
On Error GoTo cmdReplicate_Err

Dim dbs As Database, txtBkName As String

If Len(Me.txtDBR1) = 0 Then
    MsgBox "You must choose a database to replicate!", vbCritical
    Me.txtDBR1.SetFocus
    Exit Sub
ElseIf Len(Me.txtDBR2) = 0 Then
    MsgBox "You must enter a filename for the new replica!", vbCritical
    Me.txtDBR2.SetFocus
    Exit Sub
End If

If Not FileExists(Me.txtDBR1) Then
    MsgBox "The file '" & Me.txtDBR1 & "' does not exist.", vbCritical
    Exit Sub
ElseIf FileExists(Me.txtDBR2) Then
    If MsgBox("The file '" & Me.txtDBR2 & "' already exists.  Do you want to delete it?", vbQuestion + vbYesNo) = vbYes Then
        Kill Me.txtDBR2
    Else
        Exit Sub
    End If
End If

If MsgBox("Are you sure you want to make a replica of '" & Me.txtDBR1 & "' as '" & Me.txtDBR2 & "'?", vbQuestion + vbYesNo) = vbNo Then
    Exit Sub
End If

' Opens the replicable database
Set dbs = OpenDatabase(Me.txtDBR1)

' Sends data or structural changes to the replica.
OpenWaitForm "Replicating database..."
dbs.MakeReplica Me.txtDBR2, "Replica of " & Me.txtDBR1
CloseWaitForm
dbs.Close
Set dbs = Nothing

' Check whether to compact dbs
If Me.chkCompactR1 = 1 Then
    OpenWaitForm "Compacting database 1..."
    txtBkName = Left$(Me.txtDBR1, InStr(Me.txtDBR1, ".md") - 1) & ".mdx"
    If FileExists(txtBkName) Then
        Kill txtBkName
    End If
    DBEngine.CompactDatabase Me.txtDBR1, txtBkName
    Kill Me.txtDBR1
    Name txtBkName As Me.txtDBR1
    CloseWaitForm
End If
If Me.chkCompactR2 = 1 Then
    OpenWaitForm "Compacting database 2..."
    txtBkName = Left$(Me.txtDBR2, InStr(Me.txtDBR2, ".md") - 1) & ".mdx"
    If FileExists(txtBkName) Then
        Kill txtBkName
    End If
    DBEngine.CompactDatabase Me.txtDBR2, txtBkName
    Kill Me.txtDBR2
    Name txtBkName As Me.txtDBR2
    CloseWaitForm
End If

MsgBox "Database '" & Me.txtDBR1 & "' has been replicated to '" & Me.txtDBR2 & "'.", vbInformation

Exit Sub

cmdReplicate_Err:
Select Case Err
    Case Else
        ErrHand "frmDBOps", "cmdReplicate"
        CloseWaitForm
        Exit Sub
End Select

End Sub

Private Sub cmdSynchronize_Click()
On Error GoTo cmdSynch_Err

Dim dbs As Database, txtBkName As String

If Len(Me.txtDBS1) = 0 Then
    MsgBox "You must choose a database to use!", vbCritical
    Me.txtDBS1.SetFocus
    Exit Sub
ElseIf Len(Me.txtDBS2) = 0 Then
    MsgBox "You must choose a database to use!", vbCritical
    Me.txtDBS2.SetFocus
    Exit Sub
End If

If Not FileExists(Me.txtDBS1) Then
    MsgBox "The file '" & Me.txtDBS1 & "' does not exist.", vbCritical
    Exit Sub
ElseIf Not FileExists(Me.txtDBS2) Then
    MsgBox "The file '" & Me.txtDBS2 & "' does not exist.", vbCritical
    Exit Sub
End If

If MsgBox("Are you sure you want to synchronize '" & Me.txtDBS1 & "' with '" & Me.txtDBS2 & "'?", vbQuestion + vbYesNo) = vbNo Then
    Exit Sub
End If

'Check for backups...
If Me.chkBackupDBS1 = 1 Then
    OpenWaitForm "Backing up database 1..."
    txtBkName = Left$(Me.txtDBS1, InStr(Me.txtDBS1, ".md") - 1) & ".md1"
    If FileExists(txtBkName) Then
        Kill txtBkName
    End If
    FileCopy Me.txtDBS1, txtBkName
    CloseWaitForm
End If
If Me.chkBackupDBS2 = 1 Then
    OpenWaitForm "Backing up database 2..."
    txtBkName = Left$(Me.txtDBS2, InStr(Me.txtDBS2, ".md") - 1) & ".md1"
    If FileExists(txtBkName) Then
        Kill txtBkName
    End If
    FileCopy Me.txtDBS2, txtBkName
    CloseWaitForm
End If
    

' Opens the replicable database
Set dbs = OpenDatabase(Me.txtDBS1)

' Sends data or structural changes to the replica.
OpenWaitForm "Synching databases..."
dbs.Synchronize Me.txtDBS2, dbRepImpExpChanges
CloseWaitForm
dbs.Close

' Check whether to compact dbs
If Me.chkCompDBS1 = 1 Then
    OpenWaitForm "Compacting database 1..."
    txtBkName = Left$(Me.txtDBS1, InStr(Me.txtDBS1, ".md") - 1) & ".mdx"
    If FileExists(txtBkName) Then
        Kill txtBkName
    End If
    DBEngine.CompactDatabase Me.txtDBS1, txtBkName
    Kill Me.txtDBS1
    Name txtBkName As Me.txtDBS1
    CloseWaitForm
End If
If Me.chkCompDBS2 = 1 Then
    OpenWaitForm "Compacting database 2..."
    txtBkName = Left$(Me.txtDBS2, InStr(Me.txtDBS2, ".md") - 1) & ".mdx"
    If FileExists(txtBkName) Then
        Kill txtBkName
    End If
    DBEngine.CompactDatabase Me.txtDBS2, txtBkName
    Kill Me.txtDBS2
    Name txtBkName As Me.txtDBS2
    CloseWaitForm
End If

MsgBox "Databases '" & Me.txtDBS1 & "' and '" & Me.txtDBS2 & "' have been synchronized succesfully.", vbInformation

Exit Sub

cmdSynch_Err:
Select Case Err
    Case Else
        CloseWaitForm
        ErrHand "frmDBOps", "cmdSynchronize"
        Exit Sub
End Select

End Sub

Function DoubleSlash(txtIn As String) As String

Dim i As Integer
For i = 1 To Len(txtIn)
  If Mid$(txtIn, i, 1) = "\" Then
    DoubleSlash = DoubleSlash & "\\"
  Else
    DoubleSlash = DoubleSlash & Mid$(txtIn, i, 1)
  End If
Next i

End Function
