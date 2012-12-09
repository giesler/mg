VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.MDIForm frmMain 
   BackColor       =   &H8000000C&
   Caption         =   "RPS Dealer Parts"
   ClientHeight    =   5130
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   6870
   LinkTopic       =   "MDIForm1"
   StartUpPosition =   3  'Windows Default
   Begin MSComctlLib.ImageList tbImages 
      Left            =   2880
      Top             =   1440
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   4
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmMain.frx":0000
            Key             =   ""
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmMain.frx":031A
            Key             =   ""
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmMain.frx":0634
            Key             =   ""
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmMain.frx":094E
            Key             =   ""
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.Toolbar tb1 
      Align           =   1  'Align Top
      Height          =   360
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   6870
      _ExtentX        =   12118
      _ExtentY        =   635
      ButtonWidth     =   2540
      ButtonHeight    =   582
      AllowCustomize  =   0   'False
      Wrappable       =   0   'False
      Appearance      =   1
      Style           =   1
      TextAlignment   =   1
      ImageList       =   "tbImages"
      _Version        =   393216
      BeginProperty Buttons {66833FE8-8583-11D1-B16A-00C0F0283628} 
         NumButtons      =   4
         BeginProperty Button1 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "&Part Cards"
            ImageIndex      =   1
         EndProperty
         BeginProperty Button2 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "Part &Listing"
            ImageIndex      =   2
         EndProperty
         BeginProperty Button3 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "&Report Menu"
            ImageIndex      =   3
         EndProperty
         BeginProperty Button4 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "&Export Data"
            ImageIndex      =   4
         EndProperty
      EndProperty
   End
   Begin MSComDlg.CommonDialog dlgCommonDialog 
      Left            =   1740
      Top             =   1350
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin MSComctlLib.StatusBar sbStatusBar 
      Align           =   2  'Align Bottom
      Height          =   270
      Left            =   0
      TabIndex        =   0
      Top             =   4860
      Width           =   6870
      _ExtentX        =   12118
      _ExtentY        =   476
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   3
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            AutoSize        =   1
            Object.Width           =   6482
            Text            =   "Status"
            TextSave        =   "Status"
         EndProperty
         BeginProperty Panel2 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   6
            AutoSize        =   2
            TextSave        =   "7/14/2000"
         EndProperty
         BeginProperty Panel3 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Style           =   5
            AutoSize        =   2
            TextSave        =   "5:50 PM"
         EndProperty
      EndProperty
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuFilePartsListing 
         Caption         =   "&Parts Info"
      End
      Begin VB.Menu mnuFilePartList 
         Caption         =   "Part &List"
      End
      Begin VB.Menu mnuFileReportsMenu 
         Caption         =   "&Reports"
      End
      Begin VB.Menu mnuFileExportData 
         Caption         =   "&Export Data"
      End
      Begin VB.Menu mnuFileBar0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFileExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuView 
      Caption         =   "&View"
      Begin VB.Menu mnuViewToolbar 
         Caption         =   "&Toolbar"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuViewStatusBar 
         Caption         =   "Status &Bar"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuViewBar0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuViewRefresh 
         Caption         =   "&Refresh"
      End
      Begin VB.Menu mnuViewOptions 
         Caption         =   "&Options..."
      End
   End
   Begin VB.Menu mnuTools 
      Caption         =   "&Tools"
      Begin VB.Menu mnuToolsUpdatePrices 
         Caption         =   "&Update Prices"
      End
   End
   Begin VB.Menu mnuWindow 
      Caption         =   "&Window"
      WindowList      =   -1  'True
      Begin VB.Menu mnuWindowCascade 
         Caption         =   "&Cascade"
      End
      Begin VB.Menu mnuWindowTileHorizontal 
         Caption         =   "Tile &Horizontal"
      End
      Begin VB.Menu mnuWindowTileVertical 
         Caption         =   "Tile &Vertical"
      End
      Begin VB.Menu mnuWindowArrangeIcons 
         Caption         =   "&Arrange Icons"
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      Begin VB.Menu mnuHelpTip 
         Caption         =   "&Tip of the Day"
      End
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About "
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub MDIForm_Load()
On Error GoTo MDIForm_Err

If cComp = eFactoryCat Then
  de1.cnParts.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Password="""";Data Source=" & App.Path & "\rpsdata.mdb;Persist Security Info=True"
ElseIf cComp = eTomCat Then
  de1.cnParts.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Password="""";Data Source=" & App.Path & "\rpsdata.mdb;Persist Security Info=True"
End If

Dim rs As Recordset, sDate As String
Set rs = New Recordset
rs.Open "tblDBInfo", de1.cnParts.ConnectionString, adOpenStatic, adLockReadOnly
sDate = rs.Fields(0)
rs.Close
Set rs = Nothing

Me.sbStatusBar.Panels(1).Text = "Parts list last updated " & sDate

Me.Left = GetSetting(App.Title, "Window Positions", "MainLeft", 1000)
Me.Top = GetSetting(App.Title, "Window Positions", "MainTop", 1000)
Me.Width = GetSetting(App.Title, "Window Positions", "MainWidth", 6500)
Me.Height = GetSetting(App.Title, "Window Positions", "MainHeight", 6500)

Me.Visible = True
Me.Show
Unload frmSplash

Dim sTmp
sTmp = GetSetting(App.Title, "Options", "CompanyName", "not set")
If sTmp = "not set" Then
  frmDealerName.Show vbModal, Me
End If

sTmp = GetSetting(App.Title, "Options", "Show Tips at Startup", 1)
If sTmp = 1 Then
  frmTip.Show vbModal, Me
End If

Dim fOW As frmOldDataWarning, bHide As Boolean

If DateDiff("m", CVDate(sDate), Date) > 6 Then
  bHide = GetSetting(App.Title, "Options", "Hide Update Warning", False)
  If Not bHide Then
    Set fOW = New frmOldDataWarning
    fOW.Show vbModal, Me
    If fOW.bUpdate Then
      Unload fOW
      frmUpdatePrices.Show vbModal, Me
    End If
  End If
End If

Exit Sub
MDIForm_Err:
ErrHand Me.Name, "MDIForm_Load"
Exit Sub
End Sub

Private Sub MDIForm_Unload(Cancel As Integer)
  If Me.WindowState <> vbMinimized Then
    SaveSetting App.Title, "Window Positions", "MainLeft", Me.Left
    SaveSetting App.Title, "Window Positions", "MainTop", Me.Top
    SaveSetting App.Title, "Window Positions", "MainWidth", Me.Width
    SaveSetting App.Title, "Window Positions", "MainHeight", Me.Height
  End If
End Sub

Private Sub mnuFileExportData_Click()
frmExportData.Show
frmExportData.SetFocus
End Sub

Private Sub mnuFilePartList_Click()
frmPartsDatasheet.Show
frmPartsDatasheet.SetFocus
End Sub

Private Sub mnuFilePartsListing_Click()
frmPartCards.Show
frmPartCards.SetFocus
End Sub

Private Sub mnuFileReportsMenu_Click()
frmReportMenu.Show
frmReportMenu.SetFocus
End Sub

Private Sub mnuHelpAbout_Click()
  frmAbout.Show vbModal, Me
End Sub

Private Sub mnuHelpTip_Click()
frmTip.Show vbModal, Me
End Sub

Private Sub mnuToolsUpdatePrices_Click()
  frmUpdatePrices.Show vbModal, Me

Dim rs As Recordset, sDate As String
Set rs = New Recordset
rs.Open "tblDBInfo", de1.cnParts.ConnectionString, adOpenStatic, adLockReadOnly
sDate = rs.Fields(0)
rs.Close
Set rs = Nothing

Me.sbStatusBar.Panels(1).Text = "Parts list last updated " & sDate

End Sub

Private Sub mnuWindowArrangeIcons_Click()
  Me.Arrange vbArrangeIcons
End Sub

Private Sub mnuWindowTileVertical_Click()
  Me.Arrange vbTileVertical
End Sub

Private Sub mnuWindowTileHorizontal_Click()
  Me.Arrange vbTileHorizontal
End Sub

Private Sub mnuWindowCascade_Click()
  Me.Arrange vbCascade
End Sub

Private Sub mnuViewOptions_Click()
  frmOptions.Show vbModal, Me
End Sub

Private Sub mnuViewRefresh_Click()
  'ToDo: Add 'mnuViewRefresh_Click' code.
  MsgBox "Add 'mnuViewRefresh_Click' code."
End Sub

Private Sub mnuViewStatusBar_Click()
  mnuViewStatusBar.Checked = Not mnuViewStatusBar.Checked
  sbStatusBar.Visible = mnuViewStatusBar.Checked
End Sub

Private Sub mnuViewToolbar_Click()
Me.mnuViewToolbar.Checked = Not Me.mnuViewToolbar.Checked
Me.tb1.Visible = Me.mnuViewToolbar.Checked
End Sub

Private Sub mnuFileExit_Click()
  'unload the form
  Unload Me

End Sub

Private Sub tb1_ButtonClick(ByVal Button As MSComctlLib.Button)

Select Case Button.Index
  Case 1
    mnuFilePartsListing_Click
  Case 2
    mnuFilePartList_Click
  Case 3
    mnuFileReportsMenu_Click
  Case 4
    mnuFileExportData_Click
End Select

End Sub
