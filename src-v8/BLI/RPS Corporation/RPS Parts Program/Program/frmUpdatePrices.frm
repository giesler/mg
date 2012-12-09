VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmUpdatePrices 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Price Update"
   ClientHeight    =   2415
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2415
   ScaleWidth      =   4680
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.Frame fraDetail 
      BorderStyle     =   0  'None
      Height          =   1575
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   4455
      Begin VB.Label Label4 
         Caption         =   $"frmUpdatePrices.frx":0000
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   975
         Left            =   240
         TabIndex        =   9
         Top             =   600
         Width           =   4095
      End
      Begin VB.Label Label3 
         Caption         =   "Click 'Update Prices' below to update your prices directly from RPS Corporation over the internet. "
         Height          =   735
         Left            =   240
         TabIndex        =   8
         Top             =   120
         Width           =   4095
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   495
      Left            =   2400
      TabIndex        =   1
      Top             =   1800
      Width           =   1575
   End
   Begin VB.CommandButton cmdUpdate 
      Caption         =   "&Update Prices"
      Height          =   495
      Left            =   600
      TabIndex        =   0
      Top             =   1800
      Width           =   1575
   End
   Begin VB.Frame fraProgress 
      BorderStyle     =   0  'None
      Height          =   1575
      Left            =   120
      TabIndex        =   3
      Top             =   120
      Width           =   4455
      Begin MSComctlLib.ProgressBar pbTotal 
         Height          =   255
         Left            =   240
         TabIndex        =   5
         Top             =   1200
         Width           =   4095
         _ExtentX        =   7223
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   1
      End
      Begin MSComctlLib.ProgressBar pbStep 
         Height          =   255
         Left            =   240
         TabIndex        =   4
         Top             =   480
         Width           =   4095
         _ExtentX        =   7223
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   1
      End
      Begin VB.Label Label2 
         Caption         =   "Total Progess"
         Height          =   255
         Left            =   360
         TabIndex        =   7
         Top             =   960
         Width           =   2535
      End
      Begin VB.Label Label1 
         Caption         =   "Current Step"
         Height          =   255
         Left            =   360
         TabIndex        =   6
         Top             =   240
         Width           =   2535
      End
   End
End
Attribute VB_Name = "frmUpdatePrices"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdCancel_Click()
Me.Visible = False
End Sub

Private Sub cmdUpdate_Click()
On Error GoTo cmdUpdate_Err

Dim f As Form, sRemotePartsTable As String, sRemoteModelsTable As String
Dim sServer As String

sServer = GetSetting(App.Title, "Options", "UpdateServer", sUpdateServer)

If cComp = eFactoryCat Then
  sRemotePartsTable = "tblFCDealerParts"
  sRemoteModelsTable = "tblFCDealerPartsModels"
Else
  sRemotePartsTable = "tblTCDealerParts"
  sRemoteModelsTable = "tblTCDealerPartsModels"
End If

For Each f In Forms
  If f.Name <> Me.Name And f.Name <> "frmMain" Then
    Unload f
  End If
Next f

Me.Refresh

Me.fraDetail.Visible = False
Me.pbTotal.Max = 13
Me.cmdUpdate.Enabled = False
Me.cmdCancel.Enabled = False

Dim cnLocal As Connection, cnRemote As Connection
Dim rsLocal As Recordset, rsRemote As Recordset, iCur As Long, iCnt As Long
Set cnLocal = New Connection
Set cnRemote = New Connection
cnLocal.Open de1.cnParts.ConnectionString
UpdateTotal 1
cnRemote.Open "Provider=SQLOLEDB.1;Password=fcrps;Persist Security Info=True;User ID=fcdealer;Data Source=" & sServer & ";Network Library=dbmssocn"
UpdateTotal 2

'check version
Dim sRemoteVersion As Double, sLocalVersion As Double
sLocalVersion = Val(App.Major & "." & Format(App.Minor, "00"))
sRemoteVersion = GetFieldValue(cnRemote, "tblReqVersion", "ReqVersion")
If sLocalVersion < sRemoteVersion Then
  MsgBox "A new version of the Dealer Parts Program is available.  Contact RPS to receive the new version.  The parts prices cannot be updated with this older version.", vbCritical
  GoTo ExitFunctionHere
End If
UpdateTotal 3

'check dates
Dim sLocalDate As String, sRemoteDate As String
Me.pbStep.Max = 2
sLocalDate = GetFieldValue(cnLocal, "tblDBInfo", "ListDate")
UpdateStep 1
sRemoteDate = GetFieldValue(cnRemote, "tblDBInfo", "ListDate")
UpdateStep 2
If CVDate(sLocalDate) >= CVDate(sRemoteDate) Then
  If MsgBox("You are already using the most up to date data available.  Would you like to contine to update anyways?", vbQuestion + vbYesNo) = vbNo Then
    Me.Refresh
    GoTo ExitFunctionHere
  End If
  Me.Refresh
  Me.Refresh
End If
Me.pbStep.Value = 0
UpdateTotal 4

With cnLocal
  .BeginTrans
  .Execute "delete * from tblDealerPartsModels"
  .Execute "delete * from tblDealerParts"
  UpdateTotal 5
  Set rsLocal = New Recordset
  rsLocal.Open "tblDealerParts", cnLocal, adOpenDynamic, adLockOptimistic
  UpdateTotal 6
  Set rsRemote = New Recordset
  rsRemote.Open sRemotePartsTable, cnRemote, adOpenForwardOnly, adLockReadOnly
  UpdateTotal 7
  With rsLocal
    Me.pbStep.Max = CountRecs(cnRemote, sRemotePartsTable)
    iCur = 0
    Do While Not rsRemote.EOF
      .AddNew
      .Fields("PartID") = rsRemote.Fields("PartID")
      .Fields("PartName") = rsRemote.Fields("PartName")
      .Fields("RPSPartNum") = rsRemote.Fields("RPSPartNum")
      .Fields("RPSPNSort") = rsRemote.Fields("RPSPNSort")
      .Fields("USADealerNet") = rsRemote.Fields("USADealerNet")
      .Fields("USASuggestedList") = rsRemote.Fields("USASuggestedList")
      .Fields("Note") = rsRemote.Fields("Note")
      .Update
      iCur = iCur + 1
      UpdateStep iCur
      rsRemote.MoveNext
    Loop
  End With 'rsLocal
  Me.pbStep.Value = 0
  UpdateTotal 8
  rsLocal.Close
  rsRemote.Close
  Set rsLocal = Nothing
  Set rsRemote = Nothing
  
  Set rsLocal = New Recordset
  rsLocal.Open "tblDealerPartsModels", cnLocal, adOpenDynamic, adLockOptimistic
  UpdateTotal 9
  Set rsRemote = New Recordset
  rsRemote.Open sRemoteModelsTable, cnRemote, adOpenForwardOnly, adLockReadOnly
  UpdateTotal 10
  With rsLocal
    Me.pbStep.Max = CountRecs(cnRemote, sRemoteModelsTable)
    iCur = 0
    Do While Not rsRemote.EOF
      .AddNew
      .Fields("PartModelID") = rsRemote.Fields("PartModelID")
      .Fields("fkPartID") = rsRemote.Fields("fkPartID")
      .Fields("Model") = rsRemote.Fields("Model")
      .Fields("Quantity") = rsRemote.Fields("Quantity")
      .Fields("Optional") = rsRemote.Fields("Optional")
      .Update
      iCur = iCur + 1
      UpdateStep iCur
      rsRemote.MoveNext
    Loop
  End With 'rsLocal
  UpdateTotal 11
  Me.pbStep.Value = 0
  .Execute "update tblDBInfo set ListDate = #" & sRemoteDate & "#"
  UpdateTotal 12
  .CommitTrans
  UpdateTotal 13
End With 'cnLocal

sLocalDate = GetFieldValue(cnLocal, "tblDBInfo", "ListDate")
MsgBox "The parts prices have been updated with prices from " & sLocalDate & ".", vbInformation


ExitFunctionHere:
cnLocal.Close
cnRemote.Close
Set cnLocal = Nothing
Set cnRemote = Nothing
Unload Me

Exit Sub
cmdUpdate_Err:
ErrHand Me.Name, "Update"
Exit Sub
End Sub

Sub UpdateStep(i As Long)

If i > Me.pbStep.Max Then i = Me.pbStep.Max
Me.pbStep.Value = i
Me.Refresh

End Sub

Sub UpdateTotal(i As Long)

If i > Me.pbTotal.Max Then i = Me.pbTotal.Max
Me.pbTotal.Value = i
Me.Refresh

End Sub

Private Function CountRecs(cn As Connection, sTable As String) As Long

Dim rs As Recordset
Set rs = New Recordset
rs.Open "select count(*) from " & sTable, cn, adOpenStatic, adLockReadOnly
CountRecs = rs.Fields(0)
rs.Close
Set rs = Nothing

End Function

Private Function GetFieldValue(cn As Connection, sTable As String, sField As String) As String

Dim rs As Recordset
Set rs = New Recordset
rs.Open "select " & sField & " from " & sTable, cn, adOpenStatic, adLockReadOnly
GetFieldValue = rs.Fields(sField)
rs.Close
Set rs = Nothing

End Function
