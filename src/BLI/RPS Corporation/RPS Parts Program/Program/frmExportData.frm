VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmExportData 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Export Data"
   ClientHeight    =   3480
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4950
   Icon            =   "frmExportData.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   3480
   ScaleWidth      =   4950
   ShowInTaskbar   =   0   'False
   Begin VB.Frame fraOutputOptions 
      Caption         =   " Output Options "
      Height          =   735
      Left            =   120
      TabIndex        =   7
      Top             =   2160
      Width           =   4695
      Begin VB.CheckBox chkFirstRow 
         Caption         =   "First row contains field names"
         Height          =   255
         Left            =   240
         TabIndex        =   8
         Top             =   360
         Value           =   1  'Checked
         Width           =   3495
      End
      Begin VB.Label lblNoOptions 
         Caption         =   "There are no output options for the selected format."
         Height          =   255
         Left            =   240
         TabIndex        =   9
         Top             =   360
         Visible         =   0   'False
         Width           =   4215
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   " Export File Format "
      Height          =   1455
      Left            =   120
      TabIndex        =   2
      Top             =   600
      Width           =   4695
      Begin VB.OptionButton optExportFormat 
         Caption         =   "&Microsoft Access 2000 Database"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   5
         Top             =   1080
         Width           =   4215
      End
      Begin VB.OptionButton optExportFormat 
         Caption         =   "&Fixed Width text file"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   4
         Top             =   720
         Width           =   4215
      End
      Begin VB.OptionButton optExportFormat 
         Caption         =   "&CSV File - Comma Seperated Value"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   3
         Top             =   360
         Value           =   -1  'True
         Width           =   4215
      End
   End
   Begin MSComDlg.CommonDialog cd1 
      Left            =   120
      Top             =   3000
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   3240
      TabIndex        =   1
      Top             =   3000
      Width           =   1335
   End
   Begin VB.CommandButton cmdExport 
      Caption         =   "&Export"
      Default         =   -1  'True
      Height          =   375
      Left            =   1800
      TabIndex        =   0
      Top             =   3000
      Width           =   1335
   End
   Begin VB.Label Label1 
      Caption         =   "Select the export format you would like and click 'Export' below."
      Height          =   375
      Left            =   120
      TabIndex        =   6
      Top             =   120
      Width           =   5175
   End
End
Attribute VB_Name = "frmExportData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub ExportAccess()

Dim sMsg As String

sMsg = "In order to export to a Microsoft Access 2000 database, specify the location you would like to save the file to.  The file will have three tables, one with the date of the data, one with part information, and one with model information for each part."

If MsgBox(sMsg, vbQuestion + vbOKCancel) = vbCancel Then Exit Sub

Me.cd1.Filter = "Access Database (*.mdb)|*.mdb|All Files (*.*)|*.*"
Me.cd1.DefaultExt = "mdb"
Me.cd1.DialogTitle = "Save Database As?"
Me.cd1.Flags = cdlOFNOverwritePrompt
Me.cd1.ShowSave
If Me.cd1.FileName <> "" Then
  OpenWaitForm "Exporting..."
  If cComp = eFactoryCat Then
    FileCopy App.Path & "\rpsdata.mdb", Me.cd1.FileName
  Else
    FileCopy App.Path & "\rpsdata.mdb", Me.cd1.FileName
  End If
  CloseWaitForm
  MsgBox "The data has been copied to '" & Me.cd1.FileName & "'.", vbInformation
End If

End Sub

Private Sub ExportTextFile(bCSV As Boolean)

Dim rs As Recordset, ff As Integer, rsp As Integer
Dim bFirstRow As Boolean
If Me.chkFirstRow = 1 Then bFirstRow = True

If bCSV Then
  Me.cd1.Filter = "Comma Separated Value Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
  Me.cd1.DefaultExt = "csv"
Else
  Me.cd1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
  Me.cd1.DefaultExt = "txt"
End If
Me.cd1.DialogTitle = "Save File As?"
Me.cd1.Flags = cdlOFNOverwritePrompt
Me.cd1.ShowSave
If Me.cd1.FileName <> "" Then
  OpenWaitForm "Exporting..."
  On Error Resume Next
  Kill Me.cd1.FileName
  On Error GoTo 0
  Me.Refresh
  ff = FreeFile
  Open Me.cd1.FileName For Output As ff
  If de1.rsPartsOnly.State = adStateClosed Then de1.rsPartsOnly.Open
  Set rs = de1.rsPartsOnly
  rs.MoveFirst
  If bFirstRow Then
    If bCSV Then
      Write #ff, "RPS Part Num", "Part Name", "Dealer Net", _
        "Suggested List", "Note"
    Else
      Print #ff, "RPS Part Num", "Part Name", "Dealer Net", _
        "Suggested List", "Note"
    End If
  End If
  Do While Not rs.EOF
    If bCSV Then
      Write #ff, Nz(rs.Fields("RPS Part Num"), ""), _
        Nz(rs.Fields("Part Name"), ""), _
        Nz(rs.Fields("Dealer Net"), ""), _
        Nz(rs.Fields("Suggested List"), ""), _
        Nz(rs.Fields("Note"), "")
    Else
      Print #ff, Nz(rs.Fields("RPS Part Num"), ""), _
        Nz(rs.Fields("Part Name"), ""), _
        Nz(rs.Fields("Dealer Net"), ""), _
        Nz(rs.Fields("Suggested List"), ""), _
        Nz(rs.Fields("Note"), "")
    End If
    rs.MoveNext
  Loop
  Close ff
  Set rs = Nothing
  CloseWaitForm
  MsgBox "The CSV file has been saved to '" & Me.cd1.FileName & "'.", vbInformation
End If

End Sub


Private Sub cmdCancel_Click()
Unload Me
End Sub

Private Sub cmdExport_Click()

If Me.optExportFormat(0) Then
  ExportTextFile True
ElseIf Me.optExportFormat(1) Then
  ExportTextFile False
Else
  ExportAccess
End If
Unload Me

End Sub

Private Sub optExportFormat_Click(Index As Integer)
Select Case Index
  Case 0, 1
    Me.chkFirstRow.Visible = True
    Me.lblNoOptions.Visible = False
  Case 2
    Me.chkFirstRow.Visible = False
    Me.lblNoOptions.Visible = True
End Select
End Sub
