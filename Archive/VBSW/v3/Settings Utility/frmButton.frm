VERSION 5.00
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Begin VB.Form frmButton 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Button Properties"
   ClientHeight    =   4560
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6405
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4560
   ScaleWidth      =   6405
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdSave 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3120
      TabIndex        =   11
      Top             =   4080
      Width           =   1335
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   4560
      TabIndex        =   10
      Top             =   4080
      Width           =   1335
   End
   Begin TabDlg.SSTab tb 
      Height          =   3855
      Left            =   120
      TabIndex        =   12
      Top             =   120
      Width           =   6150
      _ExtentX        =   10848
      _ExtentY        =   6800
      _Version        =   393216
      Style           =   1
      Tabs            =   4
      TabsPerRow      =   5
      TabHeight       =   520
      TabCaption(0)   =   "General Settings"
      TabPicture(0)   =   "frmButton.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label1"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "Label12"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "txtName"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "chkComponentCheck"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "fraDialogAction"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).ControlCount=   5
      TabCaption(1)   =   "Button Action"
      TabPicture(1)   =   "frmButton.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "txtFile"
      Tab(1).Control(1)=   "optButtonAction(3)"
      Tab(1).Control(2)=   "chkRestartPrompt"
      Tab(1).Control(3)=   "txtSetup"
      Tab(1).Control(4)=   "cmdSetupBrowse"
      Tab(1).Control(5)=   "txtCmdLine"
      Tab(1).Control(6)=   "txtURL"
      Tab(1).Control(7)=   "optButtonAction(2)"
      Tab(1).Control(8)=   "optButtonAction(1)"
      Tab(1).Control(9)=   "optButtonAction(0)"
      Tab(1).Control(10)=   "Label16"
      Tab(1).Control(11)=   "Label4"
      Tab(1).Control(12)=   "Label9"
      Tab(1).Control(13)=   "Label3"
      Tab(1).Control(14)=   "Label2"
      Tab(1).ControlCount=   15
      TabCaption(2)   =   "Images"
      TabPicture(2)   =   "frmButton.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "cmdMouseCursor"
      Tab(2).Control(1)=   "txtMouseCursor"
      Tab(2).Control(2)=   "txtTop"
      Tab(2).Control(3)=   "txtLeft"
      Tab(2).Control(4)=   "txtMouseClick"
      Tab(2).Control(5)=   "cmdMouseClickBrowse"
      Tab(2).Control(6)=   "txtMouseOver"
      Tab(2).Control(7)=   "cmdMouseOverBrowse"
      Tab(2).Control(8)=   "txtStandard"
      Tab(2).Control(9)=   "cmdStandardBrowse"
      Tab(2).Control(10)=   "Label20"
      Tab(2).Control(11)=   "Label6"
      Tab(2).Control(12)=   "Label31"
      Tab(2).Control(13)=   "Label29"
      Tab(2).Control(14)=   "Label19"
      Tab(2).Control(15)=   "Label18"
      Tab(2).Control(16)=   "Label17"
      Tab(2).Control(17)=   "Label13"
      Tab(2).Control(18)=   "Label5"
      Tab(2).Control(19)=   "Label7"
      Tab(2).ControlCount=   20
      TabCaption(3)   =   "Sounds"
      TabPicture(3)   =   "frmButton.frx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "txtMouseUp"
      Tab(3).Control(1)=   "cmdMouseUp"
      Tab(3).Control(2)=   "txtMouseDown"
      Tab(3).Control(3)=   "cmdMouseDown"
      Tab(3).Control(4)=   "txtMouseExit"
      Tab(3).Control(5)=   "cmdMouseExit"
      Tab(3).Control(6)=   "txtMouseEnter"
      Tab(3).Control(7)=   "cmdMouseEnter"
      Tab(3).Control(8)=   "Label15"
      Tab(3).Control(9)=   "Label14"
      Tab(3).Control(10)=   "Label11"
      Tab(3).Control(11)=   "Label10"
      Tab(3).Control(12)=   "Label8"
      Tab(3).ControlCount=   13
      Begin VB.CommandButton cmdMouseCursor 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70200
         TabIndex        =   57
         Top             =   2640
         Width           =   855
      End
      Begin VB.TextBox txtMouseCursor 
         Height          =   285
         Left            =   -72960
         TabIndex        =   56
         ToolTipText     =   "Mouse cursor image when mouse is over this button"
         Top             =   2640
         Width           =   2655
      End
      Begin VB.Frame fraDialogAction 
         Caption         =   " Splash Dialog "
         Height          =   1095
         Left            =   600
         TabIndex        =   52
         Top             =   1800
         Width           =   4695
         Begin VB.OptionButton optDialogAction 
            Caption         =   "Show when button action is completed"
            Height          =   255
            Index           =   2
            Left            =   120
            TabIndex        =   55
            Top             =   720
            Width           =   4215
         End
         Begin VB.OptionButton optDialogAction 
            Caption         =   "Show when button action is started"
            Height          =   255
            Index           =   1
            Left            =   120
            TabIndex        =   54
            Top             =   480
            Width           =   4335
         End
         Begin VB.OptionButton optDialogAction 
            Caption         =   "Do not show again when button action is complete"
            Height          =   255
            Index           =   0
            Left            =   120
            TabIndex        =   53
            Top             =   240
            Width           =   4095
         End
      End
      Begin VB.TextBox txtFile 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73680
         TabIndex        =   50
         ToolTipText     =   "File to open"
         Top             =   3120
         Width           =   3735
      End
      Begin VB.OptionButton optButtonAction 
         Caption         =   "Open a file (using ShellExecute)"
         Height          =   255
         Index           =   3
         Left            =   -74880
         TabIndex        =   49
         Top             =   2760
         Width           =   5655
      End
      Begin VB.TextBox txtMouseUp 
         Height          =   285
         Left            =   -73560
         TabIndex        =   47
         Top             =   1920
         Width           =   3375
      End
      Begin VB.CommandButton cmdMouseUp 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70080
         TabIndex        =   46
         Top             =   1920
         Width           =   855
      End
      Begin VB.TextBox txtMouseDown 
         Height          =   285
         Left            =   -73560
         TabIndex        =   44
         Top             =   1560
         Width           =   3375
      End
      Begin VB.CommandButton cmdMouseDown 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70080
         TabIndex        =   43
         Top             =   1560
         Width           =   855
      End
      Begin VB.TextBox txtMouseExit 
         Height          =   285
         Left            =   -73560
         TabIndex        =   41
         Top             =   1200
         Width           =   3375
      End
      Begin VB.CommandButton cmdMouseExit 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70080
         TabIndex        =   40
         Top             =   1200
         Width           =   855
      End
      Begin VB.TextBox txtMouseEnter 
         Height          =   285
         Left            =   -73560
         TabIndex        =   38
         Top             =   840
         Width           =   3375
      End
      Begin VB.CommandButton cmdMouseEnter 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70080
         TabIndex        =   37
         Top             =   840
         Width           =   855
      End
      Begin VB.TextBox txtTop 
         Height          =   285
         Left            =   -70680
         TabIndex        =   26
         ToolTipText     =   "Button placement from top of background"
         Top             =   1920
         Width           =   615
      End
      Begin VB.TextBox txtLeft 
         Height          =   285
         Left            =   -72360
         TabIndex        =   25
         ToolTipText     =   "Button placement from left side of background"
         Top             =   1920
         Width           =   615
      End
      Begin VB.TextBox txtMouseClick 
         Height          =   285
         Left            =   -72960
         TabIndex        =   24
         ToolTipText     =   "Mouse click image for this button"
         Top             =   1560
         Width           =   2655
      End
      Begin VB.CommandButton cmdMouseClickBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70200
         TabIndex        =   23
         Top             =   1560
         Width           =   855
      End
      Begin VB.TextBox txtMouseOver 
         Height          =   285
         Left            =   -72960
         TabIndex        =   22
         ToolTipText     =   "Mouse over image for this button"
         Top             =   1200
         Width           =   2655
      End
      Begin VB.CommandButton cmdMouseOverBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70200
         TabIndex        =   21
         Top             =   1200
         Width           =   855
      End
      Begin VB.TextBox txtStandard 
         Height          =   285
         Left            =   -72960
         TabIndex        =   20
         ToolTipText     =   "Standard button image"
         Top             =   840
         Width           =   2655
      End
      Begin VB.CommandButton cmdStandardBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -70200
         TabIndex        =   19
         Top             =   840
         Width           =   855
      End
      Begin VB.CheckBox chkRestartPrompt 
         Caption         =   "&Prompt to restart computer after installation completes"
         Height          =   255
         Left            =   -74640
         TabIndex        =   4
         Top             =   1680
         Width           =   5295
      End
      Begin VB.CheckBox chkComponentCheck 
         Caption         =   "&Run component check before doing button action"
         Height          =   255
         Left            =   360
         TabIndex        =   9
         ToolTipText     =   "Whether or not to check the component list before performing button action"
         Top             =   1320
         Width           =   5295
      End
      Begin VB.TextBox txtSetup 
         Height          =   285
         Left            =   -73680
         TabIndex        =   1
         ToolTipText     =   "Relative path to setup.exe or xxx.msi which will be run when VBSW completes"
         Top             =   1080
         Width           =   3735
      End
      Begin VB.CommandButton cmdSetupBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -69840
         TabIndex        =   2
         Top             =   1080
         Width           =   855
      End
      Begin VB.TextBox txtCmdLine 
         Height          =   285
         Left            =   -73680
         TabIndex        =   3
         ToolTipText     =   "Command line for setup.  Include setup.exe filename."
         Top             =   1380
         Width           =   3735
      End
      Begin VB.TextBox txtURL 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73680
         TabIndex        =   6
         ToolTipText     =   "URL to launch"
         Top             =   2400
         Width           =   3735
      End
      Begin VB.OptionButton optButtonAction 
         Caption         =   "&Cancel button - close program"
         Height          =   255
         Index           =   2
         Left            =   -74880
         TabIndex        =   7
         Top             =   3480
         Width           =   5655
      End
      Begin VB.OptionButton optButtonAction 
         Caption         =   "&Launch a URL (if a web browser can be found)"
         Height          =   255
         Index           =   1
         Left            =   -74880
         TabIndex        =   5
         Top             =   2040
         Width           =   5655
      End
      Begin VB.OptionButton optButtonAction 
         Caption         =   "&Run a program"
         Height          =   255
         Index           =   0
         Left            =   -74880
         TabIndex        =   0
         Top             =   720
         Width           =   5655
      End
      Begin VB.TextBox txtName 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   1200
         TabIndex        =   8
         ToolTipText     =   "Internal name - for reference and logging"
         Top             =   900
         Width           =   4575
      End
      Begin VB.Label Label20 
         Caption         =   "Mouse Cursor:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   58
         Top             =   2640
         Width           =   1815
      End
      Begin VB.Label Label16 
         Caption         =   "File:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   51
         Top             =   3120
         Width           =   615
      End
      Begin VB.Label Label15 
         Caption         =   "Mouse Up:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   48
         Top             =   1920
         Width           =   1815
      End
      Begin VB.Label Label14 
         Caption         =   "Mouse Down:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   45
         Top             =   1560
         Width           =   1815
      End
      Begin VB.Label Label11 
         Caption         =   "Mouse Exit:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   42
         Top             =   1200
         Width           =   1815
      End
      Begin VB.Label Label10 
         Caption         =   "Mouse Enter:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   39
         Top             =   840
         Width           =   1815
      End
      Begin VB.Label Label8 
         BackColor       =   &H80000018&
         Caption         =   "Sounds to be played corresponding to mouse events"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   36
         Top             =   360
         Width           =   5895
      End
      Begin VB.Label Label6 
         BackColor       =   &H80000018&
         Caption         =   "Images to be displayed for this button"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   35
         Top             =   360
         Width           =   5895
      End
      Begin VB.Label Label31 
         Caption         =   " px"
         Height          =   255
         Left            =   -70080
         TabIndex        =   34
         Top             =   1920
         Width           =   375
      End
      Begin VB.Label Label29 
         Caption         =   " px"
         Height          =   255
         Left            =   -71760
         TabIndex        =   33
         Top             =   1920
         Width           =   375
      End
      Begin VB.Label Label19 
         Caption         =   "Top:"
         Height          =   255
         Left            =   -71280
         TabIndex        =   32
         Top             =   1920
         Width           =   615
      End
      Begin VB.Label Label18 
         Caption         =   "Left:"
         Height          =   255
         Left            =   -72960
         TabIndex        =   31
         Top             =   1920
         Width           =   615
      End
      Begin VB.Label Label17 
         Caption         =   "Button Placement:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   30
         Top             =   1920
         Width           =   1695
      End
      Begin VB.Label Label13 
         Caption         =   "OnMouseClick Image:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   29
         Top             =   1560
         Width           =   1815
      End
      Begin VB.Label Label5 
         Caption         =   "OnMouseOver Image:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   28
         Top             =   1200
         Width           =   1815
      End
      Begin VB.Label Label7 
         Caption         =   "Standard Image:"
         Height          =   255
         Left            =   -74880
         TabIndex        =   27
         Top             =   840
         Width           =   1815
      End
      Begin VB.Label Label4 
         Caption         =   "Setup / MSI:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   18
         Top             =   1080
         Width           =   1095
      End
      Begin VB.Label Label9 
         Caption         =   "Cmd Line:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   17
         Top             =   1380
         Width           =   1095
      End
      Begin VB.Label Label3 
         Caption         =   "URL:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   16
         Top             =   2400
         Width           =   615
      End
      Begin VB.Label Label2 
         BackColor       =   &H80000018&
         Caption         =   "Settings for what action will be performed when the button is clicked"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   15
         Top             =   360
         Width           =   5895
      End
      Begin VB.Label Label12 
         BackColor       =   &H80000018&
         Caption         =   "General information about this button"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   120
         TabIndex        =   14
         Top             =   360
         Width           =   5895
      End
      Begin VB.Label Label1 
         Caption         =   "Name:"
         Height          =   255
         Left            =   360
         TabIndex        =   13
         Top             =   900
         Width           =   615
      End
   End
End
Attribute VB_Name = "frmButton"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private mobjButton As CButton
Private mblnDirty As Boolean

Public Property Set Button(pobjButton As CButton)
  Set mobjButton = pobjButton
End Property

Public Property Get Button() As CButton
  Set Button = mobjButton
End Property

Public Property Get Dirty() As Boolean
  Dirty = mblnDirty
End Property


Private Sub chkComponentCheck_Click()
  mblnDirty = True
End Sub

Private Sub cmdCancel_Click()

  mblnDirty = False
  Me.Visible = False
  
End Sub

Private Sub cmdMouseClickBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtMouseClick = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdMouseCursor_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the mouse cursor file", Me)
  If strFileName <> "" Then
    txtMouseCursor.Text = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdMouseDown_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the mouse down sound file", Me)
  If strFileName <> "" Then
    txtMouseDown.Text = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdMouseEnter_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the mouse enter sound file", Me)
  If strFileName <> "" Then
    txtMouseEnter.Text = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdMouseExit_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the mouse exit sound file", Me)
  If strFileName <> "" Then
    txtMouseExit.Text = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdMouseOverBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtMouseOver = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdMouseUp_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the mouse up sound file", Me)
  If strFileName <> "" Then
    txtMouseUp.Text = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdSave_Click()

  If ValidateSettings Then
    Save
    Me.Visible = False
  End If
  
End Sub

Private Sub cmdSetupBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtSetup = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub cmdStandardBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjButton.Settings.RootPathIDList, "Select the bitmap file to use in autorun.exe", Me)
  If strFileName <> "" Then
    Me.txtStandard = Mid(strFileName, Len(mobjButton.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub Form_Load()

  tb.Tab = 0
  
End Sub

Private Sub optButtonAction_Click(Index As Integer)

  Dim i As Integer
  
  Me.txtSetup.Enabled = False
  Me.txtSetup.BackColor = vbButtonFace
  Me.cmdSetupBrowse.Enabled = False
  Me.txtCmdLine.Enabled = False
  Me.txtCmdLine.BackColor = vbButtonFace
  Me.chkRestartPrompt.Enabled = False
  Me.txtURL.Enabled = False
  Me.txtURL.BackColor = vbButtonFace
  Me.txtFile.Enabled = False
  Me.txtFile.BackColor = vbButtonFace
  mblnDirty = True
  
  Select Case Index
    Case 0
      Me.txtSetup.Enabled = True
      Me.txtSetup.BackColor = vbWindowBackground
      Me.cmdSetupBrowse.Enabled = True
      Me.txtCmdLine.Enabled = True
      Me.txtCmdLine.BackColor = vbWindowBackground
      Me.chkRestartPrompt.Enabled = True
    Case 1
      Me.txtURL.Enabled = True
      Me.txtURL.BackColor = vbWindowBackground
    Case 2
    Case 3
      Me.txtFile.Enabled = True
      Me.txtFile.BackColor = vbWindowBackground
  End Select

End Sub

Private Sub txtCmdLine_Change()
  mblnDirty = True
End Sub

Private Sub txtLeft_Change()
  mblnDirty = True
End Sub

Private Sub txtMouseClick_Change()
  mblnDirty = True
End Sub

Private Sub txtMouseOver_Change()
  mblnDirty = True
End Sub

Private Sub txtName_Change()
  mblnDirty = True
End Sub

Private Sub txtSetup_Change()
  mblnDirty = True
End Sub

Private Sub txtStandard_Change()
  mblnDirty = True
End Sub

Private Sub txtTop_Change()
  mblnDirty = True
End Sub

Private Sub txtURL_Change()
  mblnDirty = True
End Sub


Public Sub Load()

    Me.txtName.Text = mobjButton.Name
    Me.chkComponentCheck.Value = IIf(mobjButton.ComponentCheck, 1, 0)
    optDialogAction(mobjButton.DialogAction).Value = True
    
    Me.txtStandard.Text = mobjButton.Standard
    Me.txtMouseClick.Text = mobjButton.MouseClick
    Me.txtMouseOver.Text = mobjButton.MouseOver
    Me.txtLeft.Text = CStr(mobjButton.Left)
    Me.txtTop.Text = CStr(mobjButton.Top)
    
    Me.optButtonAction(mobjButton.ButtonType).Value = True
    optButtonAction_Click mobjButton.ButtonType
    
    Me.txtSetup.Text = mobjButton.SetupCommand
    Me.txtCmdLine.Text = mobjButton.SetupCommandLine
    Me.chkRestartPrompt.Value = IIf(mobjButton.RestartPrompt, 1, 0)
    
    Me.txtURL.Text = mobjButton.URL
    Me.txtFile.Text = mobjButton.File
    
    txtMouseEnter.Text = mobjButton.MouseEnter
    txtMouseExit.Text = mobjButton.MouseExit
    txtMouseUp.Text = mobjButton.MouseUp
    txtMouseDown.Text = mobjButton.MouseDown
    
    txtMouseCursor.Text = mobjButton.MouseCursor
    
End Sub

Public Sub Save()

    If Not mblnDirty Then Exit Sub
    
    mobjButton.Name = Me.txtName
    mobjButton.ComponentCheck = IIf(Me.chkComponentCheck.Value = 1, True, False)
    If optDialogAction(0) Then
        mobjButton.DialogAction = DialogActionDoNotShow
    ElseIf optDialogAction(1) Then
        mobjButton.DialogAction = DialogActionShowImmediately
    Else
        mobjButton.DialogAction = DialogActionShowWhenActionComplete
    End If
    
    mobjButton.Standard = Me.txtStandard.Text
    mobjButton.MouseClick = Me.txtMouseClick.Text
    mobjButton.MouseOver = Me.txtMouseOver.Text
    mobjButton.Left = Val(Me.txtLeft.Text)
    mobjButton.Top = Val(Me.txtTop.Text)
    
    If Me.optButtonAction(0) = True Then
        mobjButton.ButtonType = RunProgram
        mobjButton.SetupCommand = Me.txtSetup
        mobjButton.SetupCommandLine = Me.txtCmdLine
        mobjButton.RestartPrompt = IIf(Me.chkRestartPrompt.Value = 1, True, False)
        mobjButton.URL = ""
        mobjButton.File = ""
    ElseIf Me.optButtonAction(1) = True Then
        mobjButton.ButtonType = LaunchURL
        mobjButton.SetupCommand = ""
        mobjButton.SetupCommandLine = ""
        mobjButton.URL = Me.txtURL.Text
        mobjButton.File = ""
    ElseIf Me.optButtonAction(2) Then
        mobjButton.ButtonType = CancelDialog
        mobjButton.SetupCommand = ""
        mobjButton.SetupCommandLine = ""
        mobjButton.URL = ""
        mobjButton.File = ""
    Else
        mobjButton.ButtonType = ShellExecuteItem
        mobjButton.SetupCommand = ""
        mobjButton.SetupCommandLine = ""
        mobjButton.URL = ""
        mobjButton.File = txtFile.Text
    End If
  
    mobjButton.MouseEnter = txtMouseEnter.Text
    mobjButton.MouseExit = txtMouseExit.Text
    mobjButton.MouseUp = txtMouseUp.Text
    mobjButton.MouseDown = txtMouseDown.Text
    
    mobjButton.MouseCursor = txtMouseCursor.Text
    
End Sub

Private Function ValidateSettings() As Boolean
  
  ValidateSettings = False

  If Me.txtName.Text = "" Then
    MsgBox "You must specify a name!", vbExclamation
    Me.tb.Tab = 0
    Me.txtName.SetFocus
    Exit Function
  End If
  
  If Me.optButtonAction(0).Value = True Then
    If Me.txtSetup.Text = "" Then
      MsgBox "You must specify a setup/msi file!", vbExclamation
      Me.tb.Tab = 1
      Me.txtSetup.SetFocus
      Exit Function
    End If
  ElseIf Me.optButtonAction(1).Value = True Then
    If Me.txtURL.Text = "" Then
      MsgBox "You must specify a URL!", vbExclamation
      Me.tb.Tab = 1
      Me.txtURL.SetFocus
      Exit Function
    End If
  End If
  
  
  
  
  ValidateSettings = True

End Function