VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Begin VB.Form frmMain 
   BackColor       =   &H80000004&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "VBSW Settings Utility"
   ClientHeight    =   5490
   ClientLeft      =   150
   ClientTop       =   720
   ClientWidth     =   6675
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5490
   ScaleWidth      =   6675
   StartUpPosition =   3  'Windows Default
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Align Bottom
      Height          =   255
      Left            =   0
      TabIndex        =   9
      Top             =   5235
      Width           =   6675
      _ExtentX        =   11774
      _ExtentY        =   450
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   3
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Object.Width           =   7056
            MinWidth        =   7056
         EndProperty
         BeginProperty Panel2 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Object.Width           =   2205
            MinWidth        =   2205
         EndProperty
         BeginProperty Panel3 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            Object.Width           =   2205
            MinWidth        =   2205
         EndProperty
      EndProperty
   End
   Begin TabDlg.SSTab tabs 
      Height          =   5055
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Visible         =   0   'False
      Width           =   6495
      _ExtentX        =   11456
      _ExtentY        =   8916
      _Version        =   393216
      Style           =   1
      Tabs            =   4
      TabsPerRow      =   4
      TabHeight       =   520
      Enabled         =   0   'False
      BackColor       =   -2147483644
      TabCaption(0)   =   "Program Settings"
      TabPicture(0)   =   "frmMain.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label2"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "Label1"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "Label5"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "Label3"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "Label8"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "Label38"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).Control(6)=   "Line1"
      Tab(0).Control(6).Enabled=   0   'False
      Tab(0).Control(7)=   "txtProgramName"
      Tab(0).Control(7).Enabled=   0   'False
      Tab(0).Control(8)=   "txtRootPath"
      Tab(0).Control(8).Enabled=   0   'False
      Tab(0).Control(9)=   "cmdSplashBrowse"
      Tab(0).Control(9).Enabled=   0   'False
      Tab(0).Control(10)=   "txtSplash"
      Tab(0).Control(10).Enabled=   0   'False
      Tab(0).ControlCount=   11
      TabCaption(1)   =   "Components"
      TabPicture(1)   =   "frmMain.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "Label10"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "cmdAdd"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "lvComponents"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "cmdEdit"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "cmdDelete"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "cmdDownload"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "cmdMoveUp"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).Control(7)=   "cmdMoveDown"
      Tab(1).Control(7).Enabled=   0   'False
      Tab(1).ControlCount=   8
      TabCaption(2)   =   "Buttons"
      TabPicture(2)   =   "frmMain.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "Label6"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).Control(1)=   "lvButtons"
      Tab(2).Control(1).Enabled=   0   'False
      Tab(2).Control(2)=   "cmdDeleteButton"
      Tab(2).Control(2).Enabled=   0   'False
      Tab(2).Control(3)=   "cmdEditButton"
      Tab(2).Control(3).Enabled=   0   'False
      Tab(2).Control(4)=   "cmdAddButton"
      Tab(2).Control(4).Enabled=   0   'False
      Tab(2).Control(5)=   "cmdSetAsDefault"
      Tab(2).Control(5).Enabled=   0   'False
      Tab(2).Control(6)=   "cmdSetAsCancel"
      Tab(2).Control(6).Enabled=   0   'False
      Tab(2).ControlCount=   7
      TabCaption(3)   =   "Program Options"
      TabPicture(3)   =   "frmMain.frx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "fraRebootOption"
      Tab(3).Control(1)=   "Frame2"
      Tab(3).ControlCount=   2
      Begin VB.CommandButton cmdSetAsCancel 
         Caption         =   "Set As Cancel"
         Height          =   375
         Left            =   -69960
         TabIndex        =   34
         Top             =   4080
         Width           =   1215
      End
      Begin VB.CommandButton cmdSetAsDefault 
         Caption         =   "Set As Default"
         Height          =   375
         Left            =   -69960
         TabIndex        =   33
         Top             =   3600
         Width           =   1215
      End
      Begin VB.Frame Frame2 
         Caption         =   " Dialog Options "
         Height          =   1335
         Left            =   -74760
         TabIndex        =   31
         Top             =   1560
         Width           =   6015
         Begin VB.TextBox txtSkipProgramName 
            Height          =   285
            Left            =   3720
            TabIndex        =   37
            Top             =   960
            Width           =   2175
         End
         Begin VB.OptionButton optDisplayType 
            Caption         =   "Skip splash bitmap only if program is named:"
            Height          =   255
            Index           =   2
            Left            =   120
            TabIndex        =   36
            Top             =   960
            Width           =   5655
         End
         Begin VB.OptionButton optDisplayType 
            Caption         =   "Skip splash bitmap, simply start program associated with 'Default' button"
            Height          =   255
            Index           =   1
            Left            =   120
            TabIndex        =   35
            Top             =   600
            Width           =   5655
         End
         Begin VB.OptionButton optDisplayType 
            Caption         =   "&Display splash bitmap with buttons"
            Height          =   255
            Index           =   0
            Left            =   120
            TabIndex        =   32
            Top             =   240
            Width           =   5655
         End
      End
      Begin VB.Frame fraRebootOption 
         Caption         =   " Reboot Option "
         Height          =   975
         Left            =   -74760
         TabIndex        =   27
         Top             =   480
         Width           =   6015
         Begin VB.TextBox txtRebootPromptSeconds 
            Height          =   285
            Left            =   2400
            TabIndex        =   30
            Top             =   240
            Width           =   375
         End
         Begin VB.OptionButton optRebootPromptType 
            Caption         =   "Display message box and wait for user to click 'Restart' or 'Don't Restart'"
            Height          =   255
            Index           =   1
            Left            =   120
            TabIndex        =   29
            Top             =   600
            Width           =   5655
         End
         Begin VB.OptionButton optRebootPromptType 
            Caption         =   "&Reboot automatically after              seconds"
            Height          =   255
            Index           =   0
            Left            =   120
            TabIndex        =   28
            Top             =   240
            Width           =   5655
         End
      End
      Begin VB.CommandButton cmdAddButton 
         Caption         =   "&Add"
         Height          =   375
         Left            =   -69960
         TabIndex        =   25
         Top             =   840
         Width           =   1215
      End
      Begin VB.CommandButton cmdEditButton 
         Caption         =   "&Edit"
         Height          =   375
         Left            =   -69960
         TabIndex        =   23
         Top             =   1320
         Width           =   1215
      End
      Begin VB.CommandButton cmdDeleteButton 
         Caption         =   "D&elete"
         Height          =   375
         Left            =   -69960
         TabIndex        =   22
         Top             =   1800
         Width           =   1215
      End
      Begin VB.TextBox txtSplash 
         Height          =   285
         Left            =   1320
         TabIndex        =   18
         ToolTipText     =   "Bitmap that will be loaded by autorun.exe"
         Top             =   1740
         Width           =   3975
      End
      Begin VB.CommandButton cmdSplashBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   5400
         TabIndex        =   17
         Top             =   1740
         Width           =   855
      End
      Begin VB.CommandButton cmdMoveDown 
         Caption         =   "Move &Down"
         Height          =   375
         Left            =   -69960
         TabIndex        =   14
         Top             =   4260
         Width           =   1215
      End
      Begin VB.CommandButton cmdMoveUp 
         Caption         =   "Move &Up"
         Height          =   375
         Left            =   -69960
         TabIndex        =   13
         Top             =   3780
         Width           =   1215
      End
      Begin VB.CommandButton cmdDownload 
         Caption         =   "Download"
         Height          =   375
         Left            =   -69960
         TabIndex        =   12
         Top             =   2700
         Width           =   1215
      End
      Begin VB.TextBox txtRootPath 
         BackColor       =   &H80000004&
         Height          =   285
         Left            =   1320
         Locked          =   -1  'True
         TabIndex        =   7
         TabStop         =   0   'False
         Top             =   1260
         Width           =   4935
      End
      Begin VB.TextBox txtProgramName 
         Height          =   285
         Left            =   1320
         TabIndex        =   5
         ToolTipText     =   "Program name that will appear in the title bar and message boxes"
         Top             =   900
         Width           =   4935
      End
      Begin VB.CommandButton cmdDelete 
         Caption         =   "D&elete"
         Height          =   375
         Left            =   -69960
         TabIndex        =   4
         Top             =   1800
         Width           =   1215
      End
      Begin VB.CommandButton cmdEdit 
         Caption         =   "&Edit"
         Height          =   375
         Left            =   -69960
         TabIndex        =   3
         Top             =   1320
         Width           =   1215
      End
      Begin MSComctlLib.ListView lvComponents 
         Height          =   3975
         Left            =   -74880
         TabIndex        =   1
         Top             =   720
         Width           =   4815
         _ExtentX        =   8493
         _ExtentY        =   7011
         View            =   3
         LabelEdit       =   1
         LabelWrap       =   -1  'True
         HideSelection   =   0   'False
         Checkboxes      =   -1  'True
         FullRowSelect   =   -1  'True
         _Version        =   393217
         ForeColor       =   -2147483640
         BackColor       =   -2147483643
         BorderStyle     =   1
         Appearance      =   1
         NumItems        =   2
         BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Text            =   "Name"
            Object.Width           =   7497
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   1
            Text            =   "ID"
            Object.Width           =   0
         EndProperty
      End
      Begin VB.CommandButton cmdAdd 
         Caption         =   "&Add"
         Height          =   375
         Left            =   -69960
         TabIndex        =   2
         Top             =   840
         Width           =   1215
      End
      Begin MSComctlLib.ListView lvButtons 
         Height          =   3975
         Left            =   -74880
         TabIndex        =   24
         Top             =   720
         Width           =   4815
         _ExtentX        =   8493
         _ExtentY        =   7011
         View            =   3
         LabelEdit       =   1
         LabelWrap       =   -1  'True
         HideSelection   =   0   'False
         Checkboxes      =   -1  'True
         FullRowSelect   =   -1  'True
         _Version        =   393217
         ForeColor       =   -2147483640
         BackColor       =   -2147483643
         BorderStyle     =   1
         Appearance      =   1
         NumItems        =   4
         BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Text            =   "Name"
            Object.Width           =   5292
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Alignment       =   2
            SubItemIndex    =   1
            Text            =   "Default"
            Object.Width           =   1235
         EndProperty
         BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Alignment       =   2
            SubItemIndex    =   2
            Text            =   "Cancel"
            Object.Width           =   1235
         EndProperty
         BeginProperty ColumnHeader(4) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   3
            Text            =   "ID"
            Object.Width           =   0
         EndProperty
      End
      Begin VB.Label Label6 
         BackColor       =   &H80000018&
         Caption         =   "The checked buttons below will be displayed on the autorun dialog."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   26
         Top             =   360
         Width           =   6255
      End
      Begin VB.Line Line1 
         BorderColor     =   &H00808080&
         X1              =   120
         X2              =   6240
         Y1              =   4620
         Y2              =   4620
      End
      Begin VB.Label Label38 
         Caption         =   "For more information/help, on the web go to http://giesler.org/vbsw."
         Height          =   255
         Left            =   240
         TabIndex        =   21
         Top             =   4740
         Width           =   6135
      End
      Begin VB.Label Label8 
         Caption         =   "The dialog will be resized to the size of the BMP specified."
         Height          =   255
         Left            =   1440
         TabIndex        =   20
         Top             =   2040
         Width           =   4335
      End
      Begin VB.Label Label3 
         Caption         =   "Background:"
         Height          =   255
         Left            =   240
         TabIndex        =   19
         Top             =   1740
         Width           =   1095
      End
      Begin VB.Label Label5 
         BackColor       =   &H80000018&
         Caption         =   "Basic program settings appear on this tab."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   120
         TabIndex        =   16
         Top             =   360
         Width           =   6255
      End
      Begin VB.Label Label10 
         BackColor       =   &H80000018&
         Caption         =   "The checked components below will be checked in the order listed."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   15
         Top             =   360
         Width           =   6255
      End
      Begin VB.Label Label1 
         Caption         =   "Root:"
         Height          =   255
         Left            =   240
         TabIndex        =   8
         Top             =   1260
         Width           =   1095
      End
      Begin VB.Label Label2 
         Caption         =   "Name:"
         Height          =   255
         Left            =   240
         TabIndex        =   6
         Top             =   900
         Width           =   855
      End
   End
   Begin VB.Frame Frame1 
      Height          =   1095
      Left            =   360
      TabIndex        =   10
      Top             =   1440
      Width           =   5895
      Begin VB.Label lblTip 
         Alignment       =   2  'Center
         Caption         =   "Select 'Open' or 'New' from the file menu to begin."
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   120
         TabIndex        =   11
         Top             =   480
         Width           =   5655
      End
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu cmdFileNew 
         Caption         =   "&New"
         Shortcut        =   ^N
      End
      Begin VB.Menu mnuFileOpen 
         Caption         =   "&Open"
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuFileSave 
         Caption         =   "&Save"
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuFileSaveAs 
         Caption         =   "Save &As..."
      End
      Begin VB.Menu mnuRecent 
         Caption         =   "-"
         Index           =   0
      End
      Begin VB.Menu mnuFileBlank1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFileExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About ..."
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private mobjSettings As CSettingsFile
Private mblnDirty As Boolean

Private Sub cmdAdd_Click()
  
  Dim li As ListItem, iTemp As Integer, objComponent As CComponent
  
  ' figure out ID for this component by looking through list
  For iTemp = 0 To 200
    If mobjSettings.Components("Component" + CStr(iTemp)) Is Nothing Then Exit For
  Next iTemp
  
  ' Init new components
  Set objComponent = New CComponent
  objComponent.Init
  objComponent.ID = "Component" + CStr(iTemp)
  objComponent.Name = "New Component " + CStr(iTemp)
  objComponent.Enabled = True
  Set objComponent.Settings = mobjSettings
  objComponent.SetAtMaxSortOrder
  
  ' Add to list
  Set li = Me.lvComponents.ListItems.Add(, , objComponent.Name)
  li.SubItems(1) = objComponent.ID
  li.Checked = objComponent.Enabled
  mobjSettings.Components.Add objComponent
  li.Selected = True
  mblnDirty = True
  
End Sub


Private Sub cmdAddButton_Click()
  
  Dim li As ListItem, objButton As CButton, iTemp As Integer
  
  ' figure out ID for this component by looking through list
  For iTemp = 0 To 200
    If mobjSettings.Buttons("Button" + CStr(iTemp)) Is Nothing Then Exit For
  Next iTemp
  
  ' Init new button
  Set objButton = New CButton
  objButton.Init
  objButton.ID = "Button" + CStr(iTemp)
  objButton.Name = "New Button " + CStr(iTemp)
  objButton.Enabled = True
  If mobjSettings.Buttons.ActiveCount = 0 Then
    objButton.Default = True
    objButton.Cancel = True
  End If
  
  Set objButton.Settings = mobjSettings
  Set li = Me.lvButtons.ListItems.Add(, , objButton.Name)
  If objButton.Default Then li.SubItems(1) = "x"
  If objButton.Cancel Then li.SubItems(2) = "x"
  li.SubItems(3) = objButton.ID
  li.Checked = objButton.Enabled
  mobjSettings.Buttons.Add objButton
  li.Selected = True
  mblnDirty = True
  
End Sub

Private Sub cmdDelete_Click()

  Dim li As ListItem
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a component to delete!", vbExclamation
    Exit Sub
  End If
  
  If MsgBox("Are you sure you want to remove the component '" + li.Text + "'?", vbQuestion + vbYesNo) = vbYes Then
    mobjSettings.Components(li.SubItems(1)).Delete = True
    Me.lvComponents.ListItems.Remove li.Index
  End If

End Sub

Private Sub cmdDeleteButton_Click()

  Dim li As ListItem, strId As String
  Set li = Me.lvButtons.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a button to delete!", vbExclamation
    Exit Sub
  End If
  
  If MsgBox("Are you sure you want to remove the button '" + li.Text + "'?", vbQuestion + vbYesNo) = vbYes Then
    ' remove button from list, mark deleted
    strId = li.SubItems(3)
    mobjSettings.Buttons(strId).Delete = True
    Me.lvButtons.ListItems.Remove li.Index
    
    ' check if default, if so, set first list item default if it exists
    If mobjSettings.Buttons.DefaultButton = strId Then
      If Me.lvButtons.ListItems.Count > 0 Then
        mobjSettings.Buttons(Me.lvButtons.ListItems(1).SubItems(3)).Default = True
      End If
    End If
  
    ' check if cancel, if so, set first list item cancel if it exists
    If mobjSettings.Buttons.CancelButton = strId Then
      If Me.lvButtons.ListItems.Count > 0 Then
        mobjSettings.Buttons(Me.lvButtons.ListItems(1).SubItems(3)).Cancel = True
      End If
    End If
    
    LoadButtons
  
  End If
  
End Sub

Private Sub cmdDownload_Click()

  Dim objComponent As CComponent, li As ListItem, rVal As Long
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a component to download!", vbExclamation
    Exit Sub
  End If
  Set objComponent = mobjSettings.Components(li.Text)
  If objComponent.URL = "" Then
    MsgBox "The component '" + li.Text + "' does not have a download URL set.", vbExclamation
    Exit Sub
  End If
'  rVal = URLDownloadToFile(0, objComponent.URL, mobjSettings.RootPath & "\" & objComponent.SetupCommand, 0, 0)
  ExecuteLink objComponent.URL
  Set objComponent = Nothing
  
End Sub

Private Sub cmdEdit_Click()

  Dim li As ListItem, fC As frmComponent, objComponent As CComponent
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a component to edit!", vbExclamation
    Exit Sub
  End If
  Set objComponent = mobjSettings.Components(li.SubItems(1))
  
  Set fC = New frmComponent
  Set fC.Component = objComponent
  fC.Load
  fC.Show vbModal, Me
  
  If fC.Dirty Then
    li.Text = objComponent.Name
    mblnDirty = True
  End If
  Set fC = Nothing
  
End Sub

Private Sub cmdEditButton_Click()

  Dim li As ListItem, fB As frmButton, objButton As CButton
  Set li = Me.lvButtons.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a button to edit!", vbExclamation
    Exit Sub
  End If
  Set objButton = mobjSettings.Buttons(li.SubItems(3))
  
  Set fB = New frmButton
  Set fB.Button = objButton
  fB.Load
  fB.Show vbModal, Me
  
  If fB.Dirty Then
    li.Text = objButton.Name
    mblnDirty = True
  End If
  Set fB = Nothing
  
End Sub

Private Sub cmdFileNew_Click()

  Dim sFoldername As String, lpIDList As Long

  Set mobjSettings = New CSettingsFile
  sFoldername = GetFolderName(lpIDList, "Select the folder where you would like to create a new VBSW project", Me)
  If sFoldername = "" Then Exit Sub

  If Dir(sFoldername & "\vbsw\", vbDirectory) = "" Then
    MkDir sFoldername & "\vbsw"
  End If
  
  If mobjSettings.Load(lpIDList, sFoldername) Then
    LoadSettings
  End If
  mblnDirty = True

End Sub


Private Sub cmdMoveDown_Click()

  Dim li As ListItem, iTemp As Integer, li2 As ListItem
  
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a list item!", vbExclamation
    Exit Sub
  End If
  
  If li.Index = Me.lvComponents.ListItems.Count Then
    MsgBox "You cannot move the bottom item down!", vbExclamation
    Exit Sub
  End If
  
  iTemp = mobjSettings.Components(li.Text).SortOrder
  Set li2 = Me.lvComponents.ListItems(li.Index + 1)
  mobjSettings.Components(li.Text).SortOrder = _
    mobjSettings.Components(li2.Text).SortOrder
  mobjSettings.Components(li2.Text).SortOrder = iTemp
  mobjSettings.Components.Sort
  iTemp = li.Index + 1
  LoadComponentList
  Me.lvComponents.ListItems(iTemp).Selected = True
  
End Sub

Private Sub cmdMoveUp_Click()

  Dim li As ListItem, iTemp As Integer, li2 As ListItem
  
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a list item!", vbExclamation
    Exit Sub
  End If
  
  If li.Index = 1 Then
    MsgBox "You cannot move the top item up!", vbExclamation
    Exit Sub
  End If
  
  iTemp = mobjSettings.Components(li.Text).SortOrder
  Set li2 = Me.lvComponents.ListItems(li.Index - 1)
  mobjSettings.Components(li.Text).SortOrder = _
    mobjSettings.Components(li2.Text).SortOrder
  mobjSettings.Components(li2.Text).SortOrder = iTemp
  mobjSettings.Components.Sort
  iTemp = li.Index - 1
  LoadComponentList
  Me.lvComponents.ListItems(iTemp).Selected = True
  
End Sub

Private Sub cmdSetAsCancel_Click()

  Dim li As ListItem, fB As frmButton, objButton As CButton
  Set li = Me.lvButtons.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a button to set as the cancel!", vbExclamation
    Exit Sub
  End If
  Set objButton = mobjSettings.Buttons(li.SubItems(3))
  
  Dim objTemp As CButton
  For Each objTemp In mobjSettings.Buttons
    objTemp.Cancel = False
  Next objTemp
  objButton.Cancel = True
  LoadButtons
  
End Sub

Private Sub cmdSetAsDefault_Click()

  Dim li As ListItem, fB As frmButton, objButton As CButton
  Set li = Me.lvButtons.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a button to set as the default!", vbExclamation
    Exit Sub
  End If
  Set objButton = mobjSettings.Buttons(li.SubItems(3))
  
  Dim objTemp As CButton
  For Each objTemp In mobjSettings.Buttons
    objTemp.Default = False
  Next objTemp
  objButton.Default = True
  LoadButtons
  
End Sub

Private Sub cmdSplashBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtSplash = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If
  
End Sub

Private Sub Form_Load()
  
  Dim strTemp As String, i As Integer
  
  Me.tabs.Tab = 0
  
'  If GetSetting("VBSWSettings", "Main", "RecentFile", "") = "" Then
    Me.mnuRecent(0).Visible = False
'  Else
'    Me.mnuRecent(0).Visible = True
'    Load Me.mnuRecent(1)
'    Me.mnuRecent(1).Visible = True
'    Me.mnuRecent(1).Caption = GetSetting("VBSWSettings", "Main", "RecentFile", "")
'  End If
  
  Me.Left = GetSetting("VBSWSettings", "Main", "FormLeft", Me.Left)
  Me.Top = GetSetting("VBSWSettings", "Main", "FormTop", Me.Top)
  
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  
  If mblnDirty Then
    If MsgBox("Do you want to save changes before exiting?", vbQuestion + vbYesNo) = vbYes Then
      SaveSettings
      mobjSettings.Save
    End If
  End If
  If Not (mobjSettings Is Nothing) Then
    SaveSetting "VBSWSettings", "Main", "RecentFile", mobjSettings.RootPath
    SaveSetting "VBSWSettings", "Main", "RecentFileIDList", mobjSettings.RootPathIDList
  End If
  
End Sub

Private Sub Form_Unload(Cancel As Integer)

  SaveSetting "VBSWSettings", "Main", "FormLeft", Me.Left
  SaveSetting "VBSWSettings", "Main", "FormTop", Me.Top

End Sub


Private Sub lvButtons_DblClick()

  cmdEditButton_Click
  
End Sub

Private Sub lvComponents_DblClick()
  
  cmdEdit_Click

End Sub

Private Sub lvComponents_ItemCheck(ByVal Item As MSComctlLib.ListItem)

  mobjSettings.Components(Item.Text).Enabled = Not mobjSettings.Components(Item.Text).Enabled

End Sub

Private Sub mnuFileExit_Click()

  Unload Me
  End

End Sub

Private Sub mnuFileOpen_Click()

  Dim sFoldername As String, lpIDList As Long

  Set mobjSettings = New CSettingsFile
  sFoldername = GetFolderName(lpIDList, "Select the folder containing the existing VBSW project (root folder; containing 'vbsw' folder)", Me)
  If sFoldername = "" Then Exit Sub
  If Not FileExists(sFoldername & "\vbsw\settings.ini") Then
    MsgBox "The file '" & sFoldername & "\vbsw\settings.ini' does not exist.", vbExclamation
    Exit Sub
  End If

  If mobjSettings.Load(lpIDList, sFoldername) Then
    LoadSettings
  End If

End Sub

Private Sub LoadSettings()

  Me.txtProgramName = mobjSettings.ProgramName
  Me.txtSplash = mobjSettings.Splash
  Me.txtRootPath = mobjSettings.RootPath
  
  Me.optDisplayType(mobjSettings.DisplayType).Value = True
  optDisplayType_Click mobjSettings.DisplayType
  Me.txtSkipProgramName.Text = mobjSettings.SkipProgramName
  
  Me.optRebootPromptType(mobjSettings.RebootPromptType).Value = True
  optRebootPromptType_Click mobjSettings.RebootPromptType
  Me.txtRebootPromptSeconds.Text = mobjSettings.RebootPromptSeconds
  
  LoadComponentList
  LoadButtons
  
  Me.tabs.Visible = True
  Me.tabs.Enabled = True

End Sub

Private Sub LoadComponentList()

  Dim varItem As CComponent, li As ListItem
  Me.lvComponents.ListItems.Clear
  For Each varItem In mobjSettings.Components
    If Not varItem.Delete Then
      Set li = Me.lvComponents.ListItems.Add(, , varItem.Name)
      li.SubItems(1) = varItem.ID
      li.Checked = varItem.Enabled
      li.ToolTipText = varItem.Notes
    End If
  Next varItem

End Sub

Private Sub LoadButtons()

  Dim objButton As CButton, li As ListItem
  Me.lvButtons.ListItems.Clear
  For Each objButton In mobjSettings.Buttons
    If Not objButton.Delete Then
      Set li = Me.lvButtons.ListItems.Add(, , objButton.Name)
      If objButton.Default Then li.SubItems(1) = "x"
      If objButton.Cancel Then li.SubItems(2) = "x"
      li.SubItems(3) = objButton.ID
      li.Checked = objButton.Enabled
    End If
  Next objButton

End Sub

Private Sub mnuFileSave_Click()

  SaveSettings
  mobjSettings.Save

  If mobjSettings.Load(mobjSettings.RootPathIDList, mobjSettings.RootPath) Then
    LoadSettings
  End If
  mblnDirty = False

End Sub

Private Sub mnuFileSaveAs_Click()

MsgBox "To save the current VBSW project as another project, simply close this program and copy the entire directory structure.", vbInformation

End Sub

Private Sub mnuHelpAbout_Click()

  Dim f As frmAbout
  Set f = New frmAbout
  f.Show vbModal, Me
  Set f = Nothing
  
End Sub

Private Sub mnuRecent_Click(Index As Integer)

  Dim sFoldername As String, sIDList As String
  If Index = 1 Then
    sFoldername = mnuRecent(Index).Caption
    If Not FileExists(sFoldername & "\vbsw\settings.ini") Then
      MsgBox "The file '" & sFoldername & "\vbsw\settings.ini' does not exist.", vbExclamation
      Exit Sub
    End If
  
    sIDList = GetSetting("VBSWSettings", "Main", "RecentFileIDList", "")
    
    Set mobjSettings = New CSettingsFile
    If mobjSettings.Load(CLng(sIDList), sFoldername) Then
      LoadSettings
    End If
  End If
  
End Sub

Private Sub optDisplayType_Click(Index As Integer)

  Me.txtSkipProgramName.Enabled = False
  Me.txtSkipProgramName.BackColor = vbMenuBar
  Select Case Index
    Case 2
      Me.txtSkipProgramName.Enabled = True
      Me.txtSkipProgramName.BackColor = vbWindowBackground
  End Select

End Sub

Private Sub optRebootPromptType_Click(Index As Integer)

  Me.txtRebootPromptSeconds.Enabled = False
  Me.txtRebootPromptSeconds.BackColor = vbMenuBar
  
  Select Case Index
    Case 0
      Me.txtRebootPromptSeconds.Enabled = True
      Me.txtRebootPromptSeconds.BackColor = vbWindowBackground
  End Select

End Sub

Private Sub txtProgramName_Change()

  mblnDirty = True

End Sub

Private Sub txtSplash_Change()

  mblnDirty = True

End Sub

Private Sub SaveSettings()
  
  mobjSettings.ProgramName = Me.txtProgramName
  mobjSettings.Splash = Me.txtSplash
  
  ' Reboot type options
  If Me.optRebootPromptType(0).Value Then
    mobjSettings.RebootPromptType = TimerReboot
  Else
    mobjSettings.RebootPromptType = ManualReboot
  End If
  mobjSettings.RebootPromptSeconds = Val(Me.txtRebootPromptSeconds.Text)
  
  ' Display type options
  If Me.optDisplayType(0).Value Then
    mobjSettings.DisplayType = NormalDisplay
  ElseIf Me.optDisplayType(1).Value Then
    mobjSettings.DisplayType = SkipSplashDisplay
  Else
    mobjSettings.DisplayType = ProgramNameDisplay
  End If
  mobjSettings.SkipProgramName = Me.txtSkipProgramName.Text



End Sub

