VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Begin VB.Form frmComponent 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Component"
   ClientHeight    =   4545
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7440
   LinkTopic       =   "Form2"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4545
   ScaleWidth      =   7440
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   5760
      TabIndex        =   25
      Top             =   4080
      Width           =   1335
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   24
      Top             =   4080
      Width           =   1335
   End
   Begin TabDlg.SSTab tb 
      Height          =   3855
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7215
      _ExtentX        =   12726
      _ExtentY        =   6800
      _Version        =   393216
      Style           =   1
      Tabs            =   9
      Tab             =   3
      TabsPerRow      =   9
      TabHeight       =   520
      TabCaption(0)   =   "General Settings"
      TabPicture(0)   =   "frmComponent.frx":0000
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "txtURL"
      Tab(0).Control(1)=   "fraCheckType"
      Tab(0).Control(2)=   "txtName"
      Tab(0).Control(3)=   "Label35"
      Tab(0).Control(4)=   "Label12"
      Tab(0).Control(5)=   "Label1"
      Tab(0).ControlCount=   6
      TabCaption(1)   =   "OS Reqs"
      TabPicture(1)   =   "frmComponent.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "Label7"
      Tab(1).Control(1)=   "Label8"
      Tab(1).Control(2)=   "Label9"
      Tab(1).Control(3)=   "Label10"
      Tab(1).Control(4)=   "Label34"
      Tab(1).Control(5)=   "txtOSVersionMin"
      Tab(1).Control(6)=   "txtOSVersionMax"
      Tab(1).Control(7)=   "chkOSVersionNT"
      Tab(1).Control(8)=   "chkOSVersion9x"
      Tab(1).ControlCount=   9
      TabCaption(2)   =   "Installation"
      TabPicture(2)   =   "frmComponent.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "Label13"
      Tab(2).Control(1)=   "Label6"
      Tab(2).Control(2)=   "Label5"
      Tab(2).Control(3)=   "Label4"
      Tab(2).Control(4)=   "Label3"
      Tab(2).Control(5)=   "Label2"
      Tab(2).Control(6)=   "Label37"
      Tab(2).Control(7)=   "cmbReboot"
      Tab(2).Control(8)=   "txtSetupTime"
      Tab(2).Control(9)=   "txtSetupCommandLine"
      Tab(2).Control(10)=   "txtSetupCommand"
      Tab(2).Control(11)=   "txtSetupMessage"
      Tab(2).Control(12)=   "cmdSetupBrowse"
      Tab(2).ControlCount=   13
      TabCaption(3)   =   "Dependencies"
      TabPicture(3)   =   "frmComponent.frx":0054
      Tab(3).ControlEnabled=   -1  'True
      Tab(3).Control(0)=   "Label11"
      Tab(3).Control(0).Enabled=   0   'False
      Tab(3).Control(1)=   "lvDependencies"
      Tab(3).Control(1).Enabled=   0   'False
      Tab(3).ControlCount=   2
      TabCaption(4)   =   "Notes"
      TabPicture(4)   =   "frmComponent.frx":0070
      Tab(4).ControlEnabled=   0   'False
      Tab(4).Control(0)=   "txtNotes"
      Tab(4).ControlCount=   1
      TabCaption(5)   =   "Reg Ver Check"
      TabPicture(5)   =   "frmComponent.frx":008C
      Tab(5).ControlEnabled=   0   'False
      Tab(5).Control(0)=   "Label21"
      Tab(5).Control(1)=   "Label22"
      Tab(5).Control(2)=   "Label23"
      Tab(5).Control(3)=   "Label24"
      Tab(5).Control(4)=   "Label25"
      Tab(5).Control(5)=   "txtRegVersionCheckKey"
      Tab(5).Control(6)=   "txtRegVersionCheckVersion"
      Tab(5).ControlCount=   7
      TabCaption(6)   =   "Reg Key Check"
      TabPicture(6)   =   "frmComponent.frx":00A8
      Tab(6).ControlEnabled=   0   'False
      Tab(6).Control(0)=   "Label27"
      Tab(6).Control(1)=   "Label28"
      Tab(6).Control(2)=   "Label29"
      Tab(6).Control(3)=   "Label30"
      Tab(6).Control(4)=   "txtRegKeyCheckKey"
      Tab(6).Control(5)=   "txtRegKeyCheckValue"
      Tab(6).ControlCount=   6
      TabCaption(7)   =   "NT Serv Pack Check"
      TabPicture(7)   =   "frmComponent.frx":00C4
      Tab(7).ControlEnabled=   0   'False
      Tab(7).Control(0)=   "Label31"
      Tab(7).Control(1)=   "Label32"
      Tab(7).Control(2)=   "Label33"
      Tab(7).Control(3)=   "Label36"
      Tab(7).Control(4)=   "txtNTServicePackCheckNumber"
      Tab(7).ControlCount=   5
      TabCaption(8)   =   "File Ver Check"
      TabPicture(8)   =   "frmComponent.frx":00E0
      Tab(8).ControlEnabled=   0   'False
      Tab(8).Control(0)=   "Label26"
      Tab(8).Control(1)=   "Label17"
      Tab(8).Control(2)=   "Label14"
      Tab(8).Control(3)=   "Label15"
      Tab(8).Control(4)=   "Label16"
      Tab(8).Control(5)=   "Label18"
      Tab(8).Control(6)=   "Label19"
      Tab(8).Control(7)=   "Label20"
      Tab(8).Control(8)=   "txtFileVersionCheckDLL"
      Tab(8).Control(9)=   "txtFileVersionCheckVersion"
      Tab(8).ControlCount=   10
      Begin VB.TextBox txtNotes 
         Height          =   3135
         Left            =   -74880
         MultiLine       =   -1  'True
         TabIndex        =   64
         Top             =   480
         Width           =   6855
      End
      Begin VB.TextBox txtFileVersionCheckVersion 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73440
         TabIndex        =   55
         ToolTipText     =   "Friendly name for prompts and such"
         Top             =   1200
         Width           =   1815
      End
      Begin VB.TextBox txtFileVersionCheckDLL 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73440
         TabIndex        =   54
         ToolTipText     =   "DLL name using variables below for path names."
         Top             =   840
         Width           =   4095
      End
      Begin VB.TextBox txtURL 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73560
         TabIndex        =   2
         ToolTipText     =   "URL to download this file"
         Top             =   1200
         Width           =   4575
      End
      Begin VB.CommandButton cmdSetupBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -69840
         TabIndex        =   13
         Top             =   1200
         Width           =   855
      End
      Begin VB.TextBox txtNTServicePackCheckNumber 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -72480
         TabIndex        =   22
         Top             =   840
         Width           =   2295
      End
      Begin VB.TextBox txtRegKeyCheckValue 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73560
         TabIndex        =   21
         Top             =   1800
         Width           =   4935
      End
      Begin VB.TextBox txtRegKeyCheckKey 
         CausesValidation=   0   'False
         Height          =   765
         Left            =   -73560
         MultiLine       =   -1  'True
         TabIndex        =   20
         ToolTipText     =   "Key to check"
         Top             =   960
         Width           =   4935
      End
      Begin VB.TextBox txtRegVersionCheckVersion 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73320
         TabIndex        =   19
         Top             =   1560
         Width           =   1815
      End
      Begin VB.TextBox txtRegVersionCheckKey 
         CausesValidation=   0   'False
         Height          =   525
         Left            =   -73320
         MultiLine       =   -1  'True
         TabIndex        =   18
         ToolTipText     =   "Key to check"
         Top             =   960
         Width           =   4335
      End
      Begin VB.TextBox txtSetupMessage 
         Height          =   285
         Left            =   -73680
         TabIndex        =   11
         ToolTipText     =   "Message to display while installing"
         Top             =   840
         Width           =   4695
      End
      Begin VB.TextBox txtSetupCommand 
         Height          =   285
         Left            =   -73680
         TabIndex        =   12
         ToolTipText     =   "Relative command line from VBSW program"
         Top             =   1200
         Width           =   3735
      End
      Begin VB.TextBox txtSetupCommandLine 
         Height          =   285
         Left            =   -73680
         TabIndex        =   14
         ToolTipText     =   "Command line for program (options, also specify EXE name)"
         Top             =   1560
         Width           =   4695
      End
      Begin VB.TextBox txtSetupTime 
         Height          =   285
         Left            =   -73680
         TabIndex        =   15
         ToolTipText     =   "Estimated time in secs to install"
         Top             =   1920
         Width           =   1095
      End
      Begin VB.ComboBox cmbReboot 
         Height          =   315
         Left            =   -73680
         Style           =   2  'Dropdown List
         TabIndex        =   16
         Top             =   2520
         Width           =   3135
      End
      Begin VB.Frame fraCheckType 
         Caption         =   "Component Check Type"
         Height          =   1455
         Left            =   -74760
         TabIndex        =   32
         Top             =   1920
         Width           =   6735
         Begin VB.OptionButton optComponentType 
            Caption         =   "No Check (Always install, depending on OS reqs only)"
            Height          =   255
            Index           =   4
            Left            =   240
            TabIndex        =   66
            ToolTipText     =   "Checks if specified service pack is installed"
            Top             =   1080
            Width           =   6255
         End
         Begin VB.OptionButton optComponentType 
            Caption         =   "NT Service Pack Check"
            Height          =   255
            Index           =   3
            Left            =   3360
            TabIndex        =   6
            ToolTipText     =   "Checks if specified service pack is installed"
            Top             =   720
            Width           =   3015
         End
         Begin VB.OptionButton optComponentType 
            Caption         =   "Registry Key Check"
            Height          =   255
            Index           =   2
            Left            =   3360
            TabIndex        =   5
            ToolTipText     =   "Checks if the specified registry key equals a value"
            Top             =   360
            Width           =   3015
         End
         Begin VB.OptionButton optComponentType 
            Caption         =   "Registry Version Check"
            Height          =   255
            Index           =   1
            Left            =   240
            TabIndex        =   4
            ToolTipText     =   "Checks if registry value >= specified verison string"
            Top             =   720
            Width           =   2895
         End
         Begin VB.OptionButton optComponentType 
            Caption         =   "File Version Check"
            Height          =   255
            Index           =   0
            Left            =   240
            TabIndex        =   3
            ToolTipText     =   "Checks if a file version is >= specified version"
            Top             =   360
            Width           =   2895
         End
      End
      Begin VB.CheckBox chkOSVersion9x 
         Caption         =   "Windows 9x (95, 98, Me)"
         Height          =   255
         Left            =   -73920
         TabIndex        =   10
         ToolTipText     =   "Check for component on Win9x"
         Top             =   2400
         Width           =   4455
      End
      Begin VB.CheckBox chkOSVersionNT 
         Caption         =   "Windows NT (NT4, 2000)"
         Height          =   255
         Left            =   -73920
         TabIndex        =   9
         ToolTipText     =   "Check for component on WinNT"
         Top             =   2040
         Width           =   4455
      End
      Begin VB.TextBox txtOSVersionMax 
         CausesValidation=   0   'False
         BeginProperty DataFormat 
            Type            =   0
            Format          =   "0"
            HaveTrueFalseNull=   0
            FirstDayOfWeek  =   0
            FirstWeekOfYear =   0
            LCID            =   1033
            SubFormatType   =   0
         EndProperty
         Height          =   285
         Left            =   -72480
         TabIndex        =   8
         ToolTipText     =   "Maximum OS version required to include component"
         Top             =   1320
         Width           =   975
      End
      Begin VB.TextBox txtOSVersionMin 
         CausesValidation=   0   'False
         BeginProperty DataFormat 
            Type            =   0
            Format          =   "0"
            HaveTrueFalseNull=   0
            FirstDayOfWeek  =   0
            FirstWeekOfYear =   0
            LCID            =   1033
            SubFormatType   =   0
         EndProperty
         Height          =   285
         Left            =   -72480
         TabIndex        =   7
         ToolTipText     =   "Minimum OS version required to include component"
         Top             =   960
         Width           =   975
      End
      Begin MSComctlLib.ListView lvDependencies 
         Height          =   2655
         Left            =   120
         TabIndex        =   17
         Top             =   960
         Width           =   6855
         _ExtentX        =   12091
         _ExtentY        =   4683
         View            =   3
         LabelEdit       =   1
         LabelWrap       =   -1  'True
         HideSelection   =   -1  'True
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
            Object.Width           =   8819
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   1
            Text            =   "ID"
            Object.Width           =   0
         EndProperty
      End
      Begin VB.TextBox txtName 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73560
         TabIndex        =   1
         ToolTipText     =   "Friendly name for prompts and such"
         Top             =   840
         Width           =   4575
      End
      Begin VB.Label Label37 
         Caption         =   "Estimated seconds to install"
         Height          =   255
         Left            =   -72480
         TabIndex        =   65
         Top             =   1920
         Width           =   2535
      End
      Begin VB.Label Label20 
         BackColor       =   &H80000018&
         Caption         =   "%CommonFilesPath% = c:\Program Files\Common Files"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74640
         TabIndex        =   63
         Top             =   3360
         Width           =   4455
      End
      Begin VB.Label Label19 
         BackColor       =   &H80000018&
         Caption         =   "%WinPath% = c:\windows on Win9x, c:\winnt on WinNT"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74640
         TabIndex        =   62
         Top             =   3120
         Width           =   4815
      End
      Begin VB.Label Label18 
         BackColor       =   &H80000018&
         Caption         =   "%WinSysPath% = c:\windows\system on 9x, c:\winnt\System32 on NT"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74640
         TabIndex        =   61
         Top             =   2880
         Width           =   5055
      End
      Begin VB.Label Label16 
         BackColor       =   &H80000018&
         Caption         =   "Specify the DLL and version to check"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   60
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label15 
         Caption         =   "DLL:"
         Height          =   255
         Left            =   -74280
         TabIndex        =   59
         Top             =   840
         Width           =   615
      End
      Begin VB.Label Label14 
         Caption         =   "Version:"
         Height          =   255
         Left            =   -74280
         TabIndex        =   58
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   1200
         Width           =   615
      End
      Begin VB.Label Label17 
         BackColor       =   &H80000018&
         Caption         =   $"frmComponent.frx":00FC
         ForeColor       =   &H80000017&
         Height          =   1335
         Left            =   -74880
         TabIndex        =   57
         Top             =   2400
         Width           =   6975
      End
      Begin VB.Label Label26 
         Caption         =   "Format: x.x.x.x"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   -1  'True
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -71520
         TabIndex        =   56
         Top             =   1200
         Width           =   2415
      End
      Begin VB.Label Label36 
         Alignment       =   2  'Center
         BackColor       =   &H80000018&
         Caption         =   "Service Pack x"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74160
         TabIndex        =   53
         Top             =   3360
         Width           =   4335
      End
      Begin VB.Label Label35 
         Caption         =   "URL:"
         Height          =   255
         Left            =   -74400
         TabIndex        =   52
         Top             =   1200
         Width           =   615
      End
      Begin VB.Label Label34 
         BackColor       =   &H80000018&
         Caption         =   "A value of 0 for both min and max will include this component."
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   -74880
         TabIndex        =   51
         Top             =   3240
         Width           =   6975
      End
      Begin VB.Label Label33 
         BackColor       =   &H80000018&
         Caption         =   $"frmComponent.frx":0183
         ForeColor       =   &H80000017&
         Height          =   1215
         Left            =   -74880
         TabIndex        =   50
         Top             =   2520
         Width           =   6975
      End
      Begin VB.Label Label32 
         BackColor       =   &H80000018&
         Caption         =   "Specify the full registry key and value to check (equality)"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   49
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label31 
         Caption         =   "Service Pack:"
         Height          =   255
         Left            =   -73680
         TabIndex        =   48
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   840
         Width           =   1095
      End
      Begin VB.Label Label30 
         BackColor       =   &H80000018&
         Caption         =   "Use the whole key, including HKEY_LOCAL_MACHINE, etc.  To query the 'default' value for a key, use a trailing \."
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   -74880
         TabIndex        =   47
         Top             =   3240
         Width           =   6975
      End
      Begin VB.Label Label29 
         BackColor       =   &H80000018&
         Caption         =   "Specify the full registry key and value to check (equality)"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   46
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label28 
         Caption         =   "Key:"
         Height          =   255
         Left            =   -74400
         TabIndex        =   45
         Top             =   960
         Width           =   615
      End
      Begin VB.Label Label27 
         Caption         =   "Value:"
         Height          =   255
         Left            =   -74400
         TabIndex        =   44
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   1800
         Width           =   615
      End
      Begin VB.Label Label25 
         Caption         =   "Format: x.x.x.x"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   -1  'True
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -71400
         TabIndex        =   43
         Top             =   1560
         Width           =   2415
      End
      Begin VB.Label Label24 
         BackColor       =   &H80000018&
         Caption         =   "Use the whole key, including HKEY_LOCAL_MACHINE, etc.  To query the 'default' value for a key, use a trailing \."
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   -74880
         TabIndex        =   42
         Top             =   3240
         Width           =   6975
      End
      Begin VB.Label Label23 
         BackColor       =   &H80000018&
         Caption         =   "Specify the full registry key and version to check"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   41
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label22 
         Caption         =   "Key:"
         Height          =   255
         Left            =   -74160
         TabIndex        =   40
         Top             =   960
         Width           =   615
      End
      Begin VB.Label Label21 
         Caption         =   "Version:"
         Height          =   255
         Left            =   -74160
         TabIndex        =   39
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   1560
         Width           =   615
      End
      Begin VB.Label Label2 
         Caption         =   "Message:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   38
         Top             =   840
         Width           =   855
      End
      Begin VB.Label Label3 
         Caption         =   "Command:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   37
         Top             =   1200
         Width           =   855
      End
      Begin VB.Label Label4 
         Caption         =   "CmdLine:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   36
         Top             =   1560
         Width           =   855
      End
      Begin VB.Label Label5 
         Caption         =   "Time:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   35
         Top             =   1920
         Width           =   855
      End
      Begin VB.Label Label6 
         Caption         =   "Reboot:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   34
         Top             =   2520
         Width           =   855
      End
      Begin VB.Label Label13 
         BackColor       =   &H80000018&
         Caption         =   "Installation settings for this component."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   33
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label12 
         BackColor       =   &H80000018&
         Caption         =   "General information about this component"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   31
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label11 
         BackColor       =   &H80000018&
         Caption         =   "Before installing this component, the following components must be installed/not be required based on OS"
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   120
         TabIndex        =   30
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label10 
         BackColor       =   &H80000018&
         Caption         =   "OS requirements for this component to be checked."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   29
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label9 
         BackColor       =   &H80000018&
         Caption         =   "Win95 = 4.00, WinNT = 4.00, Win98 = 4.10, WinMe = 4.90, Win2000 = 5.00"
         ForeColor       =   &H80000017&
         Height          =   735
         Left            =   -71040
         TabIndex        =   28
         Top             =   960
         Width           =   2535
      End
      Begin VB.Label Label8 
         Caption         =   "Min OS Version:"
         Height          =   255
         Left            =   -74040
         TabIndex        =   27
         Top             =   1320
         Width           =   1215
      End
      Begin VB.Label Label7 
         Caption         =   "Min OS Version:"
         Height          =   255
         Left            =   -74040
         TabIndex        =   26
         Top             =   960
         Width           =   1215
      End
      Begin VB.Label Label1 
         Caption         =   "Name:"
         Height          =   255
         Left            =   -74400
         TabIndex        =   23
         Top             =   840
         Width           =   615
      End
   End
End
Attribute VB_Name = "frmComponent"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private mobjComponent As CComponent
Private mblnDirty As Boolean

Public Property Set Component(pobjComponent As CComponent)
  Set mobjComponent = pobjComponent
End Property

Public Property Get Component() As CComponent
  Set Component = mobjComponent
End Property

Public Property Get Dirty() As Boolean
  Dirty = mblnDirty
End Property

Private Sub cmdCancel_Click()

  mblnDirty = False
  Me.Visible = False
  
End Sub

Private Sub cmdSave_Click()

  If ValidateSettings Then
    Save
    Me.Visible = False
  End If
  
End Sub

Private Sub cmdSetupBrowse_Click()

  Dim strFileName As String
  strFileName = GetFileName(mobjComponent.Settings.RootPathIDList, "Select an EXE to install this component", Me)
  If strFileName <> "" Then
    Me.txtSetupCommand = Mid(strFileName, Len(mobjComponent.Settings.RootPath) + 2)
    mblnDirty = True
  End If

End Sub

Private Sub Form_Load()

  ' load reboot types
  Me.cmbReboot.AddItem "No Reboot Required", 0
  Me.cmbReboot.AddItem "Batch Reboot", 1
  Me.cmbReboot.AddItem "Immediate Reboot", 2
  Me.tb.Tab = 0
  
End Sub

Private Sub optComponentType_Click(Index As Integer)

  ' First hide area specific tags
  Me.tb.TabVisible(5) = False
  Me.tb.TabVisible(6) = False
  Me.tb.TabVisible(7) = False
  Me.tb.TabVisible(8) = False
  Me.chkOSVersion9x.Enabled = True
  Me.chkOSVersionNT.Enabled = True
  Me.chkOSVersion9x.Value = IIf(mobjComponent.OSVersion9x, 1, 0)
  Me.chkOSVersionNT.Value = IIf(mobjComponent.OSVersionNT, 1, 0)
  
  Select Case Index
    Case 0        ' File version check
      Me.tb.TabVisible(8) = True
    Case 1        ' Registry version check
      Me.tb.TabVisible(5) = True
    Case 2        ' Registry key check
      Me.tb.TabVisible(6) = True
    Case 3        ' NT service pack check
      Me.tb.TabVisible(7) = True
      Me.chkOSVersion9x.Value = 0
      Me.chkOSVersion9x.Enabled = False
      Me.chkOSVersionNT.Value = 1
      Me.chkOSVersionNT.Enabled = False
      If Me.txtNTServicePackCheckNumber = "" Then
        Me.txtNTServicePackCheckNumber = "Service Pack x"
      End If
  End Select
    
End Sub

Private Sub txtID_Change()
  
  mblnDirty = True

End Sub

Public Sub Load()
  
  Me.txtName.Text = mobjComponent.Name
  Me.Caption = Me.txtName.Text
  
  ' load other components
  Dim objCompList As CComponents, objComp As CComponent, li As ListItem
  For Each objComp In mobjComponent.Settings.Components
    If objComp.ID <> mobjComponent.ID Then
      Set li = Me.lvDependencies.ListItems.Add(, , objComp.Name)
      li.SubItems(1) = objComp.ID
      If InStr(mobjComponent.Dependencies, " " & li.SubItems(1) & " ") > 0 Then
        li.Checked = True
      End If
    End If
  Next objComp
  
  Me.txtNotes = mobjComponent.Notes
  
  ' Installation tab
  Me.txtSetupMessage = mobjComponent.SetupMessage
  Me.txtSetupCommand = mobjComponent.SetupCommand
  Me.txtSetupCommandLine = mobjComponent.SetupCommandLine
  Me.txtSetupTime = CStr(mobjComponent.SetupTime)
  Me.optComponentType(mobjComponent.ComponentType) = True
  optComponentType_Click mobjComponent.ComponentType
  Me.cmbReboot.ListIndex = mobjComponent.RebootType
  Me.txtURL = mobjComponent.URL
  
  ' OS Tab
  Me.txtOSVersionMin = mobjComponent.OSVersionMin
  Me.txtOSVersionMax = mobjComponent.OSVersionMax
  Me.chkOSVersion9x = IIf(mobjComponent.OSVersion9x, 1, 0)
  Me.chkOSVersionNT = IIf(mobjComponent.OSVersionNT, 1, 0)
  
  ' File version check tab
  Me.txtFileVersionCheckDLL = mobjComponent.FileVersionCheckDLL
  Me.txtFileVersionCheckVersion = mobjComponent.FileVersionCheckVersion
  
  ' Reg verison check tab
  Me.txtRegVersionCheckKey = mobjComponent.RegVersionCheckKey
  Me.txtRegVersionCheckVersion = mobjComponent.RegVersionCheckVersion
  
  ' Reg value check tab
  Me.txtRegKeyCheckKey = mobjComponent.RegKeyCheckKey
  Me.txtRegKeyCheckValue = mobjComponent.RegKeyCheckValue
  
  ' NT service pack tab
  Me.txtNTServicePackCheckNumber = mobjComponent.NTServicePackCheckNumber

End Sub

Private Sub Save()
  
  Dim objComp As CComponent
  
  mobjComponent.Name = Me.txtName.Text
  mobjComponent.URL = Me.txtURL.Text
  
  mobjComponent.Notes = Me.txtNotes.Text
  
  ' Installation tab
  mobjComponent.SetupMessage = Me.txtSetupMessage
  mobjComponent.SetupCommand = Me.txtSetupCommand
  mobjComponent.SetupCommandLine = Me.txtSetupCommandLine
  mobjComponent.SetupTime = Val(Me.txtSetupTime)
  mobjComponent.RebootType = Me.cmbReboot.ListIndex
  If Me.optComponentType(0) Then
    mobjComponent.ComponentType = FileVersionCheck
  ElseIf Me.optComponentType(1) Then
    mobjComponent.ComponentType = RegVersionCheck
  ElseIf Me.optComponentType(2) Then
    mobjComponent.ComponentType = RegKeyCheck
  ElseIf Me.optComponentType(3) Then
    mobjComponent.ComponentType = NTServicePackCheck
  ElseIf Me.optComponentType(4) Then
    mobjComponent.ComponentType = NoCheck
  Else
    Err.Raise vbObjectError, "frmComponent.Save", "Invalid component type specified."
  End If
  
  ' OS Tab
  mobjComponent.OSVersionMin = Me.txtOSVersionMin
  mobjComponent.OSVersionMax = Me.txtOSVersionMax
  mobjComponent.OSVersion9x = IIf(Me.chkOSVersion9x = 1, True, False)
  mobjComponent.OSVersionNT = IIf(Me.chkOSVersionNT = 1, True, False)
  
  ' File Version check tab
  mobjComponent.FileVersionCheckDLL = Me.txtFileVersionCheckDLL
  mobjComponent.FileVersionCheckVersion = Me.txtFileVersionCheckVersion
  
  ' Reg Version check tab
  mobjComponent.RegVersionCheckKey = Me.txtRegVersionCheckKey
  mobjComponent.RegVersionCheckVersion = Me.txtRegVersionCheckVersion
  
  ' Reg Key check tab
  mobjComponent.RegKeyCheckKey = Me.txtRegKeyCheckKey
  mobjComponent.RegKeyCheckValue = Me.txtRegKeyCheckValue
  
  ' NT SP check tab
  mobjComponent.NTServicePackCheckNumber = Me.txtNTServicePackCheckNumber
  
  ' Dependencies
  Dim li As ListItem, strTemp As String
  For Each li In Me.lvDependencies.ListItems
    If li.Checked Then
      If strTemp <> "" Then strTemp = strTemp & " "
      strTemp = strTemp & li.SubItems(1)
    End If
  Next li
  mobjComponent.Dependencies = strTemp

End Sub

Private Function ValidateSettings() As Boolean
  
  ' check tab 0, basic stuff
  
  ' check tab 1, os stuff
  If Not IsNumeric(Me.txtOSVersionMax) Then
    MsgBox "Invalid OS max version specified.", vbExclamation
    Me.tb.Tab = 1
    Me.txtOSVersionMax.SetFocus
    Exit Function
  End If
  If Not IsNumeric(Me.txtOSVersionMin) Then
    MsgBox "Invalid OS min version specified.", vbExclamation
    Me.tb.Tab = 1
    Me.txtOSVersionMin.SetFocus
    Exit Function
  End If
  If Me.optComponentType(3) And IsNumeric(Me.txtOSVersionMax) And IsNumeric(Me.txtOSVersionMin) And (Me.txtOSVersionMax = 0 Or Me.txtOSVersionMin = 0) Then
    MsgBox "When checking an NT service pack, the OS version min and max must be set to a valid Windows NT version number.", vbExclamation
    Me.tb.Tab = 1
    Me.txtOSVersionMin.SetFocus
    Exit Function
  End If
  If Me.optComponentType(3) And InStr(Me.txtNTServicePackCheckNumber, "Service Pack") = 0 Then
    If MsgBox("You should specify the service pack in the format: 'Service Pack x'.  If you want to leave the string as is, click 'cancel'.", vbOKCancel + vbExclamation) = vbOK Then
      Exit Function
    End If
  End If
  
  ValidateSettings = True

End Function


Private Sub txtName_Change()
  
  mblnDirty = True

End Sub

