VERSION 5.00
Object = "{0ECD9B60-23AA-11D0-B351-00A0C9055D8E}#6.0#0"; "MSHFLXGD.OCX"
Begin VB.Form frmPartCards 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Part Cards"
   ClientHeight    =   3885
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6375
   Icon            =   "frmPartsListing.frx":0000
   LinkTopic       =   "Form1"
   MDIChild        =   -1  'True
   ScaleHeight     =   3885
   ScaleWidth      =   6375
   Begin VB.CommandButton cmdFind 
      Caption         =   "&Find Next"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   18
      Top             =   3240
      Width           =   1095
   End
   Begin VB.ComboBox comField 
      Height          =   315
      ItemData        =   "frmPartsListing.frx":030A
      Left            =   4560
      List            =   "frmPartsListing.frx":0314
      Style           =   2  'Dropdown List
      TabIndex        =   17
      Top             =   2160
      Width           =   1455
   End
   Begin VB.ComboBox comMatch 
      Height          =   315
      ItemData        =   "frmPartsListing.frx":0331
      Left            =   4560
      List            =   "frmPartsListing.frx":033E
      Style           =   2  'Dropdown List
      TabIndex        =   16
      Top             =   2520
      Width           =   1455
   End
   Begin VB.TextBox txtFind 
      Height          =   285
      Left            =   4560
      TabIndex        =   15
      Top             =   2865
      Width           =   1455
   End
   Begin VB.TextBox txtUSADealerNet 
      DataField       =   "USADealerNet"
      BeginProperty DataFormat 
         Type            =   1
         Format          =   """$""#,##0.00"
         HaveTrueFalseNull=   0
         FirstDayOfWeek  =   0
         FirstWeekOfYear =   0
         LCID            =   1033
         SubFormatType   =   2
      EndProperty
      DataMember      =   "Parts"
      DataSource      =   "de1"
      Height          =   285
      Left            =   1200
      TabIndex        =   12
      Top             =   960
      Width           =   1320
   End
   Begin VB.CommandButton cmdMoveRec 
      Caption         =   ">>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   3
      Left            =   2280
      TabIndex        =   11
      ToolTipText     =   "Move to the last record"
      Top             =   3360
      Width           =   615
   End
   Begin VB.CommandButton cmdMoveRec 
      Caption         =   ">"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   2
      Left            =   1680
      TabIndex        =   10
      ToolTipText     =   "Move to the next record"
      Top             =   3360
      Width           =   615
   End
   Begin VB.CommandButton cmdMoveRec 
      Caption         =   "<"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   1
      Left            =   1080
      TabIndex        =   9
      ToolTipText     =   "Move the the previous record"
      Top             =   3360
      Width           =   615
   End
   Begin VB.CommandButton cmdMoveRec 
      Caption         =   "<<"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   0
      Left            =   480
      TabIndex        =   8
      ToolTipText     =   "Move to the first record"
      Top             =   3360
      Width           =   615
   End
   Begin VB.TextBox txtNote 
      DataField       =   "Note"
      DataMember      =   "Parts"
      DataSource      =   "de1"
      Height          =   285
      Left            =   1200
      TabIndex        =   6
      Top             =   1395
      Width           =   4215
   End
   Begin VB.TextBox txtUSASuggestedList 
      DataField       =   "USASuggestedList"
      BeginProperty DataFormat 
         Type            =   1
         Format          =   """$""#,##0.00"
         HaveTrueFalseNull=   0
         FirstDayOfWeek  =   0
         FirstWeekOfYear =   0
         LCID            =   1033
         SubFormatType   =   2
      EndProperty
      DataMember      =   "Parts"
      DataSource      =   "de1"
      Height          =   285
      Left            =   4080
      TabIndex        =   4
      Top             =   960
      Width           =   1305
   End
   Begin VB.TextBox txtRPSPartNum 
      DataField       =   "RPSPartNum"
      DataMember      =   "Parts"
      DataSource      =   "de1"
      Height          =   285
      Left            =   1200
      TabIndex        =   3
      Top             =   120
      Width           =   1695
   End
   Begin VB.TextBox txtPartName 
      DataField       =   "PartName"
      DataMember      =   "Parts"
      DataSource      =   "de1"
      Height          =   285
      Left            =   1200
      TabIndex        =   1
      Top             =   570
      Width           =   4215
   End
   Begin MSHierarchicalFlexGridLib.MSHFlexGrid MSHFlexGrid1 
      Bindings        =   "frmPartsListing.frx":0372
      Height          =   1440
      Left            =   120
      TabIndex        =   7
      Top             =   1800
      Width           =   3210
      _ExtentX        =   5662
      _ExtentY        =   2540
      _Version        =   393216
      Cols            =   3
      FixedCols       =   0
      AllowUserResizing=   3
      DataMember      =   "PartsModels"
      _NumberOfBands  =   1
      _Band(0).Cols   =   3
      _Band(0).GridLinesBand=   1
      _Band(0).TextStyleBand=   0
      _Band(0).TextStyleHeader=   0
      _Band(0)._NumMapCols=   4
      _Band(0)._MapCol(0)._Name=   "fkPartID"
      _Band(0)._MapCol(0)._RSIndex=   0
      _Band(0)._MapCol(0)._Alignment=   7
      _Band(0)._MapCol(0)._Hidden=   -1  'True
      _Band(0)._MapCol(1)._Name=   "Model"
      _Band(0)._MapCol(1)._RSIndex=   1
      _Band(0)._MapCol(2)._Name=   "Quantity"
      _Band(0)._MapCol(2)._RSIndex=   2
      _Band(0)._MapCol(2)._Alignment=   7
      _Band(0)._MapCol(3)._Name=   "Options"
      _Band(0)._MapCol(3)._RSIndex=   3
      _Band(0)._MapCol(3)._Alignment=   7
   End
   Begin VB.Label Label4 
      Alignment       =   2  'Center
      BackStyle       =   0  'Transparent
      Caption         =   "Find a Part"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H8000000E&
      Height          =   255
      Left            =   3480
      TabIndex        =   22
      Top             =   1920
      Width           =   2655
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      BackColor       =   &H8000000C&
      Caption         =   "Field:"
      ForeColor       =   &H8000000E&
      Height          =   255
      Left            =   3600
      TabIndex        =   21
      Top             =   2190
      Width           =   855
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      BackColor       =   &H8000000C&
      Caption         =   "Match:"
      ForeColor       =   &H8000000E&
      Height          =   255
      Left            =   3600
      TabIndex        =   20
      Top             =   2550
      Width           =   855
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      BackColor       =   &H8000000C&
      Caption         =   "Find What:"
      ForeColor       =   &H8000000E&
      Height          =   255
      Left            =   3600
      TabIndex        =   19
      Top             =   2880
      Width           =   855
   End
   Begin VB.Shape Shape1 
      BackColor       =   &H8000000C&
      BackStyle       =   1  'Opaque
      Height          =   1935
      Left            =   3480
      Top             =   1800
      Width           =   2655
   End
   Begin VB.Label lblFieldLabel 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Dealer Net:"
      Height          =   195
      Index           =   4
      Left            =   360
      TabIndex        =   14
      Top             =   1005
      Width           =   810
   End
   Begin VB.Label lblFieldLabel 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Suggested List:"
      Height          =   195
      Index           =   5
      Left            =   2760
      TabIndex        =   13
      Top             =   1005
      Width           =   1335
   End
   Begin VB.Label lblFieldLabel 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Note:"
      Height          =   255
      Index           =   6
      Left            =   -645
      TabIndex        =   5
      Top             =   1440
      Width           =   1815
   End
   Begin VB.Label lblFieldLabel 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "RPS Part Num:"
      Height          =   195
      Index           =   2
      Left            =   90
      TabIndex        =   2
      Top             =   165
      Width           =   1080
   End
   Begin VB.Label lblFieldLabel 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Part Name:"
      Height          =   195
      Index           =   1
      Left            =   375
      TabIndex        =   0
      Top             =   615
      Width           =   795
   End
End
Attribute VB_Name = "frmPartCards"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdFindPart_Click()

Dim fP As frmFindPart
Set fP = New frmFindPart
Pause fP
If fP.bCancel Then
  Set fP = Nothing
  Exit Sub
End If

Set fP = Nothing

End Sub

Private Sub cmdFind_Click()

If Me.txtFind = "" Then
  MsgBox "You must enter text to find!", vbExclamation
  Me.txtFind.SetFocus
  Exit Sub
End If

Dim txtFindCrit As String, bBOF As Boolean, bk As Variant
Dim rs As Recordset
Set rs = de1.rsParts.Clone
bk = de1.rsParts.Bookmark
rs.Bookmark = bk

Select Case Me.comField
  Case "RPS Part Num"
    txtFindCrit = "RPSPartNum"
  Case "Part Name"
    txtFindCrit = "PartName"
End Select

Select Case LCase(Me.comMatch)
  Case LCase("Start of field")
    txtFindCrit = txtFindCrit & " Like '" & Me.txtFind & "*'"
  Case LCase("Whole field")
    txtFindCrit = txtFindCrit & " = '" & Me.txtFind & "'"
  Case LCase("Any part of field")
    txtFindCrit = txtFindCrit & " Like '*" & Me.txtFind & "*'"
End Select

rs.Find txtFindCrit, 1, adSearchForward, de1.rsParts.Bookmark
If rs.EOF Then
  de1.rsParts.Bookmark = bk
  MsgBox "The selected text was not found.  If you weren't at the beginning of the records you may want to try searching again from the start.", vbExclamation
Else
  de1.rsParts.Bookmark = rs.Bookmark
End If
Set rs = Nothing

End Sub

Private Sub cmdMoveRec_Click(Index As Integer)

Select Case Index
  Case 0
    de1.rsParts.MoveFirst
  Case 1
    de1.rsParts.MovePrevious
  Case 2
    de1.rsParts.MoveNext
  Case 3
    de1.rsParts.MoveLast
End Select

End Sub

Private Sub Form_Load()

Me.Left = GetSetting(App.Title, "Window Positions", Me.Caption & " Left", 1000)
Me.Top = GetSetting(App.Title, "Window Positions", Me.Caption & " Top", 1000)
Me.WindowState = GetSetting(App.Title, "Window Positions", Me.Caption & " WindowState", 2)

Me.comField = "RPS Part Num"
Me.comMatch = "Start of field"

End Sub

Private Sub Form_Unload(Cancel As Integer)

If Me.WindowState <> vbMinimized Then
  SaveSetting App.Title, "Window Positions", Me.Caption & " Left", Me.Left
  SaveSetting App.Title, "Window Positions", Me.Caption & " Top", Me.Top
End If
SaveSetting App.Title, "Window Positions", Me.Caption & " WindowState", Me.WindowState

End Sub
