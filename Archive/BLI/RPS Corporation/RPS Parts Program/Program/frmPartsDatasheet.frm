VERSION 5.00
Object = "{0ECD9B60-23AA-11D0-B351-00A0C9055D8E}#6.0#0"; "MSHFLXGD.OCX"
Begin VB.Form frmPartsDatasheet 
   Caption         =   "Parts Listing Datasheet"
   ClientHeight    =   3045
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5085
   Icon            =   "frmPartsDatasheet.frx":0000
   LinkTopic       =   "Form1"
   MDIChild        =   -1  'True
   ScaleHeight     =   203
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   339
   Begin MSHierarchicalFlexGridLib.MSHFlexGrid msh 
      Bindings        =   "frmPartsDatasheet.frx":030A
      Height          =   2175
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4095
      _ExtentX        =   7223
      _ExtentY        =   3836
      _Version        =   393216
      Rows            =   4
      Cols            =   6
      ScrollTrack     =   -1  'True
      AllowUserResizing=   1
      FormatString    =   "    |RPS Part Num|<Part Name                                   |>Dlr Net|>Sug List"
      DataMember      =   "PartsPretty"
      BandDisplay     =   1
      _NumberOfBands  =   2
      _Band(0).Cols   =   6
      _Band(0).GridLinesBand=   1
      _Band(0).TextStyleBand=   0
      _Band(0).TextStyleHeader=   0
      _Band(0)._NumMapCols=   7
      _Band(0)._MapCol(0)._Name=   "PartID"
      _Band(0)._MapCol(0)._RSIndex=   0
      _Band(0)._MapCol(0)._Alignment=   7
      _Band(0)._MapCol(0)._Hidden=   -1  'True
      _Band(0)._MapCol(1)._Name=   "RPSPartNum"
      _Band(0)._MapCol(1)._RSIndex=   1
      _Band(0)._MapCol(2)._Name=   "PartName"
      _Band(0)._MapCol(2)._RSIndex=   2
      _Band(0)._MapCol(3)._Name=   "RPSPNSort"
      _Band(0)._MapCol(3)._RSIndex=   3
      _Band(0)._MapCol(3)._Hidden=   -1  'True
      _Band(0)._MapCol(4)._Name=   "USADealerNet"
      _Band(0)._MapCol(4)._RSIndex=   4
      _Band(0)._MapCol(5)._Name=   "USASuggestedList"
      _Band(0)._MapCol(5)._RSIndex=   5
      _Band(0)._MapCol(5)._Alignment=   7
      _Band(0)._MapCol(6)._Name=   "Note"
      _Band(0)._MapCol(6)._RSIndex=   6
      _Band(1).BandIndent=   1
      _Band(1).Cols   =   3
      _Band(1).GridLinesBand=   1
      _Band(1).TextStyleBand=   0
      _Band(1).TextStyleHeader=   0
      _Band(1).ColHeader=   1
      _Band(1)._ParentBand=   0
      _Band(1)._NumMapCols=   4
      _Band(1)._MapCol(0)._Name=   "fkPartID"
      _Band(1)._MapCol(0)._RSIndex=   0
      _Band(1)._MapCol(0)._Alignment=   7
      _Band(1)._MapCol(0)._Hidden=   -1  'True
      _Band(1)._MapCol(1)._Name=   "Model"
      _Band(1)._MapCol(1)._RSIndex=   1
      _Band(1)._MapCol(2)._Name=   "Quantity"
      _Band(1)._MapCol(2)._RSIndex=   2
      _Band(1)._MapCol(2)._Alignment=   7
      _Band(1)._MapCol(3)._Name=   "Options"
      _Band(1)._MapCol(3)._RSIndex=   3
      _Band(1)._MapCol(3)._Alignment=   7
   End
End
Attribute VB_Name = "frmPartsDatasheet"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()

Me.Left = GetSetting(App.Title, "Window Positions", Me.Caption & " Left", Me.Left)
Me.Top = GetSetting(App.Title, "Window Positions", Me.Caption & " Top", Me.Top)
Me.Width = GetSetting(App.Title, "Window Positions", Me.Caption & " Width", Me.Width)
Me.Height = GetSetting(App.Title, "Window Positions", Me.Caption & " Height", Me.Height)

Me.msh.CollapseAll

Me.msh.FormatString = "    |<RPS Part Num|Part Name                                      |>Dlr Net      |>Sug List    |Note         "

End Sub

Private Sub Form_Resize()

If Me.WindowState <> vbMinimized Then
  If Me.Width < 200 Then Me.Width = 200
  If Me.Height < 200 Then Me.Height = 200

  Me.msh.Width = Me.ScaleWidth - (2 * Me.msh.Left)
  Me.msh.Height = Me.ScaleHeight - (2 * Me.msh.Top)
End If
Me.WindowState = GetSetting(App.Title, "Window Positions", Me.Caption & " WindowState", 2)

End Sub

Private Sub Form_Unload(Cancel As Integer)

If Me.WindowState <> vbMinimized Then
  SaveSetting App.Title, "Window Positions", Me.Caption & " Left", Me.Left
  SaveSetting App.Title, "Window Positions", Me.Caption & " Top", Me.Top
  SaveSetting App.Title, "Window Positions", Me.Caption & " Width", Me.Width
  SaveSetting App.Title, "Window Positions", Me.Caption & " Height", Me.Height
End If
SaveSetting App.Title, "Window Positions", Me.Caption & " WindowState", Me.WindowState

End Sub

