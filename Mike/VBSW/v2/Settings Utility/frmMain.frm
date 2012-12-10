VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "tabctl32.ocx"
Begin VB.Form frmMain 
   BackColor       =   &H80000004&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "VBSW Settings Utility"
   ClientHeight    =   5460
   ClientLeft      =   150
   ClientTop       =   720
   ClientWidth     =   6675
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5460
   ScaleWidth      =   6675
   StartUpPosition =   3  'Windows Default
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Align Bottom
      Height          =   255
      Left            =   0
      TabIndex        =   12
      Top             =   5205
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
      TabsPerRow      =   4
      TabHeight       =   520
      Enabled         =   0   'False
      BackColor       =   -2147483644
      TabCaption(0)   =   "Program Settings"
      TabPicture(0)   =   "frmMain.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label2"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "Label4"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "Label1"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "Label5"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "Label3"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "Label8"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).Control(6)=   "Label38"
      Tab(0).Control(6).Enabled=   0   'False
      Tab(0).Control(7)=   "Line1"
      Tab(0).Control(7).Enabled=   0   'False
      Tab(0).Control(8)=   "Label9"
      Tab(0).Control(8).Enabled=   0   'False
      Tab(0).Control(9)=   "txtProgramName"
      Tab(0).Control(9).Enabled=   0   'False
      Tab(0).Control(10)=   "txtSetup"
      Tab(0).Control(10).Enabled=   0   'False
      Tab(0).Control(11)=   "txtRootPath"
      Tab(0).Control(11).Enabled=   0   'False
      Tab(0).Control(12)=   "cmdBrowseSetup"
      Tab(0).Control(12).Enabled=   0   'False
      Tab(0).Control(13)=   "cmdSplashBrowse"
      Tab(0).Control(13).Enabled=   0   'False
      Tab(0).Control(14)=   "txtSplash"
      Tab(0).Control(14).Enabled=   0   'False
      Tab(0).Control(15)=   "txtCmdLine"
      Tab(0).Control(15).Enabled=   0   'False
      Tab(0).ControlCount=   16
      TabCaption(1)   =   "Components"
      TabPicture(1)   =   "frmMain.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "cmdMoveDown"
      Tab(1).Control(1)=   "cmdMoveUp"
      Tab(1).Control(2)=   "cmdDownload"
      Tab(1).Control(3)=   "cmdDelete"
      Tab(1).Control(4)=   "cmdEdit"
      Tab(1).Control(5)=   "lvComponents"
      Tab(1).Control(6)=   "cmdAdd"
      Tab(1).Control(7)=   "Label10"
      Tab(1).ControlCount=   8
      TabCaption(2)   =   "Buttons"
      TabPicture(2)   =   "frmMain.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "Label6"
      Tab(2).Control(0).Enabled=   0   'False
      Tab(2).Control(1)=   "fraInstallButton"
      Tab(2).Control(1).Enabled=   0   'False
      Tab(2).Control(2)=   "fraCancelButton"
      Tab(2).Control(2).Enabled=   0   'False
      Tab(2).ControlCount=   3
      Begin VB.TextBox txtCmdLine 
         Height          =   285
         Left            =   1320
         TabIndex        =   60
         ToolTipText     =   "Command line for setup.  Include setup.exe filename."
         Top             =   1560
         Width           =   3975
      End
      Begin VB.Frame fraCancelButton 
         Caption         =   "Cancel Button "
         Height          =   1695
         Left            =   -74760
         TabIndex        =   39
         Top             =   2640
         Width           =   6015
         Begin VB.TextBox txtCancelTop 
            Height          =   285
            Left            =   4440
            TabIndex        =   47
            ToolTipText     =   "Button placement from top of background"
            Top             =   1320
            Width           =   615
         End
         Begin VB.TextBox txtCancelLeft 
            Height          =   285
            Left            =   2760
            TabIndex        =   46
            ToolTipText     =   "Button placement from left side of background"
            Top             =   1320
            Width           =   615
         End
         Begin VB.TextBox txtCancelMouseClick 
            Height          =   285
            Left            =   2160
            TabIndex        =   45
            ToolTipText     =   "Mouse Click image for Cancel button"
            Top             =   960
            Width           =   2655
         End
         Begin VB.CommandButton cmdCancelMouseClickBrowse 
            Caption         =   "Browse"
            Height          =   285
            Left            =   4920
            TabIndex        =   44
            Top             =   960
            Width           =   855
         End
         Begin VB.TextBox txtCancelMouseOver 
            Height          =   285
            Left            =   2160
            TabIndex        =   43
            ToolTipText     =   "Mouse Over image for Cancel button"
            Top             =   600
            Width           =   2655
         End
         Begin VB.CommandButton cmdCancelMouseOverBrowse 
            Caption         =   "Browse"
            Height          =   285
            Left            =   4920
            TabIndex        =   42
            Top             =   600
            Width           =   855
         End
         Begin VB.TextBox txtCancelStandard 
            Height          =   285
            Left            =   2160
            TabIndex        =   41
            ToolTipText     =   "Cancel button image"
            Top             =   240
            Width           =   2655
         End
         Begin VB.CommandButton cmdCancelStandardBrowse 
            Caption         =   "Browse"
            Height          =   285
            Left            =   4920
            TabIndex        =   40
            Top             =   240
            Width           =   855
         End
         Begin VB.Label Label35 
            Caption         =   " px"
            Height          =   255
            Left            =   5040
            TabIndex        =   57
            Top             =   1320
            Width           =   375
         End
         Begin VB.Label Label33 
            Caption         =   " px"
            Height          =   255
            Left            =   3360
            TabIndex        =   56
            Top             =   1320
            Width           =   375
         End
         Begin VB.Label Label20 
            Caption         =   "Top:"
            Height          =   255
            Left            =   3840
            TabIndex        =   53
            Top             =   1320
            Width           =   615
         End
         Begin VB.Label Label21 
            Caption         =   "Left:"
            Height          =   255
            Left            =   2160
            TabIndex        =   52
            Top             =   1320
            Width           =   615
         End
         Begin VB.Label Label22 
            Caption         =   "Button Placement:"
            Height          =   255
            Left            =   240
            TabIndex        =   51
            Top             =   1320
            Width           =   1695
         End
         Begin VB.Label Label26 
            Caption         =   "OnMouseClick Image:"
            Height          =   255
            Left            =   240
            TabIndex        =   50
            Top             =   960
            Width           =   1815
         End
         Begin VB.Label Label27 
            Caption         =   "OnMouseOver Image:"
            Height          =   255
            Left            =   240
            TabIndex        =   49
            Top             =   600
            Width           =   1815
         End
         Begin VB.Label Label28 
            Caption         =   "Standard Image:"
            Height          =   255
            Left            =   240
            TabIndex        =   48
            Top             =   240
            Width           =   1815
         End
      End
      Begin VB.TextBox txtSplash 
         Height          =   285
         Left            =   1320
         TabIndex        =   31
         ToolTipText     =   "Bitmap that will be loaded by autorun.exe"
         Top             =   2460
         Width           =   3975
      End
      Begin VB.CommandButton cmdSplashBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   5400
         TabIndex        =   30
         Top             =   2460
         Width           =   855
      End
      Begin VB.Frame fraInstallButton 
         Caption         =   " Install Button "
         Height          =   1695
         Left            =   -74760
         TabIndex        =   20
         Top             =   840
         Width           =   6015
         Begin VB.TextBox txtInstallTop 
            Height          =   285
            Left            =   4440
            TabIndex        =   37
            ToolTipText     =   "Button placement from top of background"
            Top             =   1320
            Width           =   615
         End
         Begin VB.TextBox txtInstallLeft 
            Height          =   285
            Left            =   2760
            TabIndex        =   34
            ToolTipText     =   "Button placement from left side of background"
            Top             =   1320
            Width           =   615
         End
         Begin VB.TextBox txtInstallMouseClick 
            Height          =   285
            Left            =   2160
            TabIndex        =   28
            ToolTipText     =   "Mouse Click image for Install button"
            Top             =   960
            Width           =   2655
         End
         Begin VB.CommandButton cmdInstallMouseClick 
            Caption         =   "Browse"
            Height          =   285
            Left            =   4920
            TabIndex        =   27
            Top             =   960
            Width           =   855
         End
         Begin VB.TextBox txtInstallMouseOver 
            Height          =   285
            Left            =   2160
            TabIndex        =   25
            ToolTipText     =   "Mouse Over image for Install button"
            Top             =   600
            Width           =   2655
         End
         Begin VB.CommandButton cmdInstallMouseOverBrowse 
            Caption         =   "Browse"
            Height          =   285
            Left            =   4920
            TabIndex        =   24
            Top             =   600
            Width           =   855
         End
         Begin VB.TextBox txtInstallStandard 
            Height          =   285
            Left            =   2160
            TabIndex        =   22
            ToolTipText     =   "Install button image"
            Top             =   240
            Width           =   2655
         End
         Begin VB.CommandButton cmdInstallStandardBrowse 
            Caption         =   "Browse"
            Height          =   285
            Left            =   4920
            TabIndex        =   21
            Top             =   240
            Width           =   855
         End
         Begin VB.Label Label31 
            Caption         =   " px"
            Height          =   255
            Left            =   5040
            TabIndex        =   55
            Top             =   1320
            Width           =   375
         End
         Begin VB.Label Label29 
            Caption         =   " px"
            Height          =   255
            Left            =   3360
            TabIndex        =   54
            Top             =   1320
            Width           =   375
         End
         Begin VB.Label Label19 
            Caption         =   "Top:"
            Height          =   255
            Left            =   3840
            TabIndex        =   38
            Top             =   1320
            Width           =   615
         End
         Begin VB.Label Label18 
            Caption         =   "Left:"
            Height          =   255
            Left            =   2160
            TabIndex        =   36
            Top             =   1320
            Width           =   615
         End
         Begin VB.Label Label17 
            Caption         =   "Button Placement:"
            Height          =   255
            Left            =   240
            TabIndex        =   35
            Top             =   1320
            Width           =   1695
         End
         Begin VB.Label Label13 
            Caption         =   "OnMouseClick Image:"
            Height          =   255
            Left            =   240
            TabIndex        =   29
            Top             =   960
            Width           =   1815
         End
         Begin VB.Label Label12 
            Caption         =   "OnMouseOver Image:"
            Height          =   255
            Left            =   240
            TabIndex        =   26
            Top             =   600
            Width           =   1815
         End
         Begin VB.Label Label7 
            Caption         =   "Standard Image:"
            Height          =   255
            Left            =   240
            TabIndex        =   23
            Top             =   240
            Width           =   1815
         End
      End
      Begin VB.CommandButton cmdMoveDown 
         Caption         =   "Move &Down"
         Height          =   375
         Left            =   -69960
         TabIndex        =   17
         Top             =   4260
         Width           =   1215
      End
      Begin VB.CommandButton cmdMoveUp 
         Caption         =   "Move &Up"
         Height          =   375
         Left            =   -69960
         TabIndex        =   16
         Top             =   3780
         Width           =   1215
      End
      Begin VB.CommandButton cmdDownload 
         Caption         =   "Download"
         Height          =   375
         Left            =   -69960
         TabIndex        =   15
         Top             =   2700
         Width           =   1215
      End
      Begin VB.CommandButton cmdBrowseSetup 
         Caption         =   "Browse"
         Height          =   285
         Left            =   5400
         TabIndex        =   7
         Top             =   1260
         Width           =   855
      End
      Begin VB.TextBox txtRootPath 
         BackColor       =   &H80000004&
         Height          =   285
         Left            =   1320
         Locked          =   -1  'True
         TabIndex        =   10
         TabStop         =   0   'False
         Top             =   1980
         Width           =   4935
      End
      Begin VB.TextBox txtSetup 
         Height          =   285
         Left            =   1320
         TabIndex        =   6
         ToolTipText     =   "Relative path to setup.exe or xxx.msi which will be run when VBSW completes"
         Top             =   1260
         Width           =   3975
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
         Top             =   1860
         Width           =   1215
      End
      Begin VB.CommandButton cmdEdit 
         Caption         =   "&Edit"
         Height          =   375
         Left            =   -69960
         TabIndex        =   3
         Top             =   1380
         Width           =   1215
      End
      Begin MSComctlLib.ListView lvComponents 
         Height          =   3855
         Left            =   -74880
         TabIndex        =   1
         Top             =   840
         Width           =   4815
         _ExtentX        =   8493
         _ExtentY        =   6800
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
            Text            =   "ID"
            Object.Width           =   2646
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   1
            Text            =   "Name"
            Object.Width           =   5733
         EndProperty
      End
      Begin VB.CommandButton cmdAdd 
         Caption         =   "&Add"
         Height          =   375
         Left            =   -69960
         TabIndex        =   2
         Top             =   900
         Width           =   1215
      End
      Begin VB.Label Label9 
         Caption         =   "Cmd Line:"
         Height          =   255
         Left            =   240
         TabIndex        =   61
         Top             =   1560
         Width           =   1095
      End
      Begin VB.Label Label6 
         BackColor       =   &H80000018&
         Caption         =   "Specify BMP images for the various button states below."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   59
         Top             =   480
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
         TabIndex        =   58
         Top             =   4740
         Width           =   6135
      End
      Begin VB.Label Label8 
         Caption         =   "The dialog will be resized to the size of the BMP specified."
         Height          =   255
         Left            =   1440
         TabIndex        =   33
         Top             =   2760
         Width           =   4335
      End
      Begin VB.Label Label3 
         Caption         =   "Background:"
         Height          =   255
         Left            =   240
         TabIndex        =   32
         Top             =   2460
         Width           =   1095
      End
      Begin VB.Label Label5 
         BackColor       =   &H80000018&
         Caption         =   "Basic program settings appear on this tab."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   120
         TabIndex        =   19
         Top             =   480
         Width           =   6255
      End
      Begin VB.Label Label10 
         BackColor       =   &H80000018&
         Caption         =   "The checked components below will be checked in the order listed."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   18
         Top             =   480
         Width           =   6255
      End
      Begin VB.Label Label1 
         Caption         =   "Root:"
         Height          =   255
         Left            =   240
         TabIndex        =   11
         Top             =   1980
         Width           =   1095
      End
      Begin VB.Label Label4 
         Caption         =   "Setup / MSI:"
         Height          =   255
         Left            =   240
         TabIndex        =   9
         Top             =   1260
         Width           =   1095
      End
      Begin VB.Label Label2 
         Caption         =   "Name:"
         Height          =   255
         Left            =   240
         TabIndex        =   8
         Top             =   900
         Width           =   855
      End
   End
   Begin VB.Frame Frame1 
      Height          =   1095
      Left            =   360
      TabIndex        =   13
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
         TabIndex        =   14
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
  For iTemp = 0 To 100
    Set li = Me.lvComponents.FindItem("Component" + CStr(iTemp))
    If li Is Nothing Then Exit For
  Next iTemp
  Set objComponent = New CComponent
  objComponent.ID = "Component" + CStr(iTemp)
  objComponent.Name = "New Component " + CStr(iTemp)
  objComponent.Enabled = True
  Set objComponent.Settings = mobjSettings
  objComponent.SetAtMaxSortOrder
  Set li = Me.lvComponents.ListItems.Add(, , objComponent.ID)
  li.SubItems(1) = objComponent.Name
  li.Checked = objComponent.Enabled
  mobjSettings.CComponents.Add objComponent
  li.Selected = True
  mblnDirty = True
  
End Sub

Private Sub cmdBrowseSetup_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the setup.exe or package.msi to launch when VBSW is complete", Me)
  If strFileName <> "" Then
    Me.txtSetup = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If
  
End Sub

Private Sub cmdCancelMouseClickBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtCancelMouseClick = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdCancelMouseOverBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtCancelMouseOver = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdCancelStandardBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtCancelStandard = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdDelete_Click()

  Dim li As ListItem
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a component to edit!", vbExclamation
    Exit Sub
  End If
  
  If MsgBox("Are you sure you want to remove the component '" + li.Text + "'?", vbQuestion + vbYesNo) = vbYes Then
    mobjSettings.CComponents(li.Text).Delete = True
    mobjSettings.CComponents(li.Text).ID = li.Tag & CStr(Timer)
    Me.lvComponents.ListItems.Remove li.Index
  End If

End Sub

Private Sub cmdDownload_Click()

  Dim objComponent As CComponent, li As ListItem, rVal As Long
  Set li = Me.lvComponents.SelectedItem
  If li Is Nothing Then
    MsgBox "You must select a component to download!", vbExclamation
    Exit Sub
  End If
  Set objComponent = mobjSettings.CComponents(li.Text)
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
  Set objComponent = mobjSettings.CComponents(li.Text)
  
  Set fC = New frmComponent
  Set fC.Component = objComponent
  fC.Load
  fC.Show vbModal, Me
  
  If fC.Dirty Then
    li.Text = objComponent.ID
    li.SubItems(1) = objComponent.Name
    mblnDirty = True
  End If
  Set fC = Nothing
  
End Sub

Private Sub cmdFileNew_Click()

  Dim sFoldername As String, lpIDList As Long

  Set mobjSettings = New CSettingsFile
  sFoldername = GetFolderName(lpIDList, "Select the folder where you would like to create a new VBSW project", Me)
  If sFoldername = "" Then Exit Sub

  If Dir(sFoldername & "\vbsw\") = "" Then
    MkDir sFoldername & "\vbsw"
  End If
  
  If mobjSettings.Load(lpIDList, sFoldername) Then
    LoadSettings
  End If
  mblnDirty = True

End Sub

Private Sub cmdInstallMouseClick_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtInstallMouseClick = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdInstallMouseOverBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtInstallMouseOver = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdInstallStandardBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjSettings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtInstallStandard = Mid(strFileName, Len(Me.txtRootPath) + 2)
    mblnDirty = True
  End If

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
  
  iTemp = mobjSettings.CComponents(li.Text).SortOrder
  Set li2 = Me.lvComponents.ListItems(li.Index + 1)
  mobjSettings.CComponents(li.Text).SortOrder = _
    mobjSettings.CComponents(li2.Text).SortOrder
  mobjSettings.CComponents(li2.Text).SortOrder = iTemp
  mobjSettings.CComponents.Sort
  iTemp = li.Index + 1
  LoadLV
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
  
  iTemp = mobjSettings.CComponents(li.Text).SortOrder
  Set li2 = Me.lvComponents.ListItems(li.Index - 1)
  mobjSettings.CComponents(li.Text).SortOrder = _
    mobjSettings.CComponents(li2.Text).SortOrder
  mobjSettings.CComponents(li2.Text).SortOrder = iTemp
  mobjSettings.CComponents.Sort
  iTemp = li.Index - 1
  LoadLV
  Me.lvComponents.ListItems(iTemp).Selected = True
  
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
  
  Me.Tabs.Tab = 0
  
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

Private Sub lvComponents_DblClick()
  
  cmdEdit_Click

End Sub

Private Sub lvComponents_ItemCheck(ByVal Item As MSComctlLib.ListItem)

  mobjSettings.CComponents(Item.Text).Enabled = Not mobjSettings.CComponents(Item.Text).Enabled

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
  Me.txtSetup = mobjSettings.Setup
  Me.txtCmdLine.Text = mobjSettings.CmdLine
  Me.txtRootPath = mobjSettings.RootPath
  
  ' Buttons
  Me.txtInstallStandard.Text = mobjSettings.InstallStandard
  Me.txtInstallMouseClick.Text = mobjSettings.InstallMouseClick
  Me.txtInstallMouseOver.Text = mobjSettings.InstallMouseOver
  Me.txtInstallLeft.Text = mobjSettings.InstallLeft
  Me.txtInstallTop.Text = mobjSettings.InstallTop
  
  Me.txtCancelStandard.Text = mobjSettings.CancelStandard
  Me.txtCancelMouseClick.Text = mobjSettings.CancelMouseClick
  Me.txtCancelMouseOver.Text = mobjSettings.CancelMouseOver
  Me.txtCancelLeft.Text = mobjSettings.CancelLeft
  Me.txtCancelTop.Text = mobjSettings.CancelTop
  
  LoadLV
  Me.Tabs.Visible = True
  Me.Tabs.Enabled = True

End Sub

Private Sub LoadLV()

  Dim varItem As CComponent, li As ListItem
  Me.lvComponents.ListItems.Clear
  For Each varItem In mobjSettings.CComponents
    Set li = Me.lvComponents.ListItems.Add(, , varItem.ID)
    li.SubItems(1) = varItem.Name
    li.Checked = varItem.Enabled
    li.ToolTipText = varItem.Notes
  Next varItem

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

Private Sub txtCancelHeight_Change()

  mblnDirty = True

End Sub

Private Sub txtCancelLeft_Change()

  mblnDirty = True

End Sub

Private Sub txtCancelMouseClick_Change()

  mblnDirty = True

End Sub

Private Sub txtCancelMouseOver_Change()

  mblnDirty = True

End Sub

Private Sub txtCancelStandard_Change()

  mblnDirty = True

End Sub

Private Sub txtCancelTop_Change()

  mblnDirty = True

End Sub

Private Sub txtCancelWidth_Change()

  mblnDirty = True

End Sub

Private Sub txtInstallHeight_Change()

  mblnDirty = True

End Sub

Private Sub txtInstallLeft_Change()

  mblnDirty = True

End Sub

Private Sub txtInstallMouseClick_Change()
  
  mblnDirty = True

End Sub

Private Sub txtInstallMouseOver_Change()

  mblnDirty = True

End Sub

Private Sub txtInstallStandard_Change()

  mblnDirty = True

End Sub

Private Sub txtInstallTop_Change()

  mblnDirty = True

End Sub

Private Sub txtInstallWidth_Change()

  mblnDirty = True

End Sub

Private Sub txtProgramName_Change()

  mblnDirty = True

End Sub

Private Sub txtSetup_Change()

  mblnDirty = True

End Sub

Private Sub txtSplash_Change()

  mblnDirty = True

End Sub

Private Sub SaveSettings()
  
  mobjSettings.ProgramName = Me.txtProgramName
  mobjSettings.Setup = Me.txtSetup
  mobjSettings.CmdLine = Me.txtCmdLine.Text
  mobjSettings.Splash = Me.txtSplash

  mobjSettings.InstallStandard = Me.txtInstallStandard.Text
  mobjSettings.InstallMouseClick = Me.txtInstallMouseClick.Text
  mobjSettings.InstallMouseOver = Me.txtInstallMouseOver.Text
  mobjSettings.InstallLeft = IIf(Me.txtInstallLeft.Text = "", 0, Me.txtInstallLeft.Text)
  mobjSettings.InstallTop = IIf(Me.txtInstallTop.Text = "", 0, Me.txtInstallTop.Text)

  mobjSettings.CancelStandard = Me.txtCancelStandard.Text
  mobjSettings.CancelMouseClick = Me.txtCancelMouseClick.Text
  mobjSettings.CancelMouseOver = Me.txtCancelMouseOver.Text
  mobjSettings.CancelLeft = IIf(Me.txtCancelLeft.Text = "", 0, Me.txtCancelLeft.Text)
  mobjSettings.CancelTop = IIf(Me.txtCancelTop.Text = "", 0, Me.txtCancelTop.Text)

End Sub

