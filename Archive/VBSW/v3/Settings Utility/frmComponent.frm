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
      TabIndex        =   21
      Top             =   4080
      Width           =   1335
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   20
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
      Tabs            =   11
      TabsPerRow      =   11
      TabHeight       =   520
      TabCaption(0)   =   "General"
      TabPicture(0)   =   "frmComponent.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label1"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "Label12"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "Label35"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "txtName"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "fraCheckType"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "txtURL"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).ControlCount=   6
      TabCaption(1)   =   "OS Reqs"
      TabPicture(1)   =   "frmComponent.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "ucOS1"
      Tab(1).Control(1)=   "Label9"
      Tab(1).Control(2)=   "Label10"
      Tab(1).ControlCount=   3
      TabCaption(2)   =   "Installation"
      TabPicture(2)   =   "frmComponent.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "cmdSetupBrowse"
      Tab(2).Control(1)=   "txtSetupMessage"
      Tab(2).Control(2)=   "txtSetupCommand"
      Tab(2).Control(3)=   "txtSetupCommandLine"
      Tab(2).Control(4)=   "txtSetupTime"
      Tab(2).Control(5)=   "cmbReboot"
      Tab(2).Control(6)=   "Label37"
      Tab(2).Control(7)=   "Label2"
      Tab(2).Control(8)=   "Label3"
      Tab(2).Control(9)=   "Label4"
      Tab(2).Control(10)=   "Label5"
      Tab(2).Control(11)=   "Label6"
      Tab(2).Control(12)=   "Label13"
      Tab(2).ControlCount=   13
      TabCaption(3)   =   "Dependencies"
      TabPicture(3)   =   "frmComponent.frx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "lvDependencies"
      Tab(3).Control(1)=   "Label11"
      Tab(3).ControlCount=   2
      TabCaption(4)   =   "Includes"
      TabPicture(4)   =   "frmComponent.frx":0070
      Tab(4).ControlEnabled=   0   'False
      Tab(4).Control(0)=   "lvIncludes"
      Tab(4).Control(1)=   "Label7"
      Tab(4).ControlCount=   2
      TabCaption(5)   =   "Reg Ver Check"
      TabPicture(5)   =   "frmComponent.frx":008C
      Tab(5).ControlEnabled=   0   'False
      Tab(5).Control(0)=   "txtRegVersionCheckVersion"
      Tab(5).Control(1)=   "txtRegVersionCheckKey"
      Tab(5).Control(2)=   "Label25"
      Tab(5).Control(3)=   "Label24"
      Tab(5).Control(4)=   "Label23"
      Tab(5).Control(5)=   "Label22"
      Tab(5).Control(6)=   "Label21"
      Tab(5).ControlCount=   7
      TabCaption(6)   =   "Reg Key Check"
      TabPicture(6)   =   "frmComponent.frx":00A8
      Tab(6).ControlEnabled=   0   'False
      Tab(6).Control(0)=   "txtRegKeyCheckValue"
      Tab(6).Control(1)=   "txtRegKeyCheckKey"
      Tab(6).Control(2)=   "Label30"
      Tab(6).Control(3)=   "Label29"
      Tab(6).Control(4)=   "Label28"
      Tab(6).Control(5)=   "Label27"
      Tab(6).ControlCount=   6
      TabCaption(7)   =   "NT Serv Pack Check"
      TabPicture(7)   =   "frmComponent.frx":00C4
      Tab(7).ControlEnabled=   0   'False
      Tab(7).Control(0)=   "txtNTServicePackCheckNumber"
      Tab(7).Control(1)=   "Label36"
      Tab(7).Control(2)=   "Label33"
      Tab(7).Control(3)=   "Label32"
      Tab(7).Control(4)=   "Label31"
      Tab(7).ControlCount=   5
      TabCaption(8)   =   "File Ver Check"
      TabPicture(8)   =   "frmComponent.frx":00E0
      Tab(8).ControlEnabled=   0   'False
      Tab(8).Control(0)=   "txtFileVersionCheckVersion"
      Tab(8).Control(1)=   "txtFileVersionCheckDLL"
      Tab(8).Control(2)=   "Label20"
      Tab(8).Control(3)=   "Label19"
      Tab(8).Control(4)=   "Label18"
      Tab(8).Control(5)=   "Label16"
      Tab(8).Control(6)=   "Label15"
      Tab(8).Control(7)=   "Label14"
      Tab(8).Control(8)=   "Label17"
      Tab(8).Control(9)=   "Label26"
      Tab(8).ControlCount=   10
      TabCaption(9)   =   ".Net Framework"
      TabPicture(9)   =   "frmComponent.frx":00FC
      Tab(9).ControlEnabled=   0   'False
      Tab(9).Control(0)=   "txtNetFrameworkVersion"
      Tab(9).Control(1)=   "Label42"
      Tab(9).Control(2)=   "Label38"
      Tab(9).Control(3)=   "Label41"
      Tab(9).Control(4)=   "Label40"
      Tab(9).Control(5)=   "Label39"
      Tab(9).ControlCount=   6
      TabCaption(10)  =   "Notes"
      TabPicture(10)  =   "frmComponent.frx":0118
      Tab(10).ControlEnabled=   0   'False
      Tab(10).Control(0)=   "txtNotes"
      Tab(10).ControlCount=   1
      Begin VB.TextBox txtNotes 
         Height          =   3135
         Left            =   -74880
         MultiLine       =   -1  'True
         TabIndex        =   67
         Top             =   480
         Width           =   6855
      End
      Begin IASettings.ucOS ucOS1 
         Height          =   3135
         Left            =   -74880
         TabIndex        =   66
         Top             =   600
         Width           =   4935
         _ExtentX        =   9340
         _ExtentY        =   5530
      End
      Begin VB.TextBox txtNetFrameworkVersion 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -72480
         TabIndex        =   59
         Top             =   840
         Width           =   2295
      End
      Begin VB.TextBox txtFileVersionCheckVersion 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73440
         TabIndex        =   48
         ToolTipText     =   "Friendly name for prompts and such"
         Top             =   1200
         Width           =   1815
      End
      Begin VB.TextBox txtFileVersionCheckDLL 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73440
         TabIndex        =   47
         ToolTipText     =   "DLL name using variables below for path names."
         Top             =   840
         Width           =   4935
      End
      Begin VB.TextBox txtURL 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   840
         TabIndex        =   2
         ToolTipText     =   "URL to download this file"
         Top             =   1200
         Width           =   6135
      End
      Begin VB.CommandButton cmdSetupBrowse 
         Caption         =   "Browse"
         Height          =   285
         Left            =   -68880
         TabIndex        =   9
         Top             =   1200
         Width           =   855
      End
      Begin VB.TextBox txtNTServicePackCheckNumber 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -72480
         TabIndex        =   18
         Top             =   840
         Width           =   3015
      End
      Begin VB.TextBox txtRegKeyCheckValue 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73800
         TabIndex        =   17
         Top             =   1800
         Width           =   5655
      End
      Begin VB.TextBox txtRegKeyCheckKey 
         CausesValidation=   0   'False
         Height          =   765
         Left            =   -73800
         MultiLine       =   -1  'True
         TabIndex        =   16
         ToolTipText     =   "Key to check"
         Top             =   960
         Width           =   5655
      End
      Begin VB.TextBox txtRegVersionCheckVersion 
         CausesValidation=   0   'False
         Height          =   285
         Left            =   -73560
         TabIndex        =   15
         Top             =   1560
         Width           =   1815
      End
      Begin VB.TextBox txtRegVersionCheckKey 
         CausesValidation=   0   'False
         Height          =   525
         Left            =   -73560
         MultiLine       =   -1  'True
         TabIndex        =   14
         ToolTipText     =   "Key to check"
         Top             =   960
         Width           =   5055
      End
      Begin VB.TextBox txtSetupMessage 
         Height          =   285
         Left            =   -73680
         TabIndex        =   7
         ToolTipText     =   "Message to display while installing"
         Top             =   840
         Width           =   5655
      End
      Begin VB.TextBox txtSetupCommand 
         Height          =   285
         Left            =   -73680
         TabIndex        =   8
         ToolTipText     =   "Relative command line from VBSW program"
         Top             =   1200
         Width           =   4695
      End
      Begin VB.TextBox txtSetupCommandLine 
         Height          =   285
         Left            =   -73680
         TabIndex        =   10
         ToolTipText     =   "Command line for program (options, also specify EXE name)"
         Top             =   1560
         Width           =   5655
      End
      Begin VB.TextBox txtSetupTime 
         Height          =   285
         Left            =   -73680
         TabIndex        =   11
         ToolTipText     =   "Estimated time in secs to install"
         Top             =   1920
         Width           =   1095
      End
      Begin VB.ComboBox cmbReboot 
         Height          =   315
         Left            =   -73680
         Style           =   2  'Dropdown List
         TabIndex        =   12
         Top             =   2520
         Width           =   5655
      End
      Begin VB.Frame fraCheckType 
         Caption         =   "Component Check Type"
         Height          =   1815
         Left            =   240
         TabIndex        =   26
         Top             =   1800
         Width           =   6735
         Begin VB.OptionButton optComponentType 
            Caption         =   ".Net Framework version Check"
            Height          =   255
            Index           =   5
            Left            =   240
            TabIndex        =   65
            ToolTipText     =   "Checks if specified service pack is installed"
            Top             =   1080
            Width           =   2775
         End
         Begin VB.OptionButton optComponentType 
            Caption         =   "No Check (Always install, depending on OS reqs only)"
            Height          =   255
            Index           =   4
            Left            =   240
            TabIndex        =   58
            ToolTipText     =   "Checks if specified service pack is installed"
            Top             =   1440
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
      Begin MSComctlLib.ListView lvDependencies 
         Height          =   2655
         Left            =   -74880
         TabIndex        =   13
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
            Object.Width           =   9701
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
         Left            =   840
         TabIndex        =   1
         ToolTipText     =   "Friendly name for prompts and such"
         Top             =   840
         Width           =   6135
      End
      Begin MSComctlLib.ListView lvIncludes 
         Height          =   2655
         Left            =   -74880
         TabIndex        =   68
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
            Object.Width           =   9701
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   1
            Text            =   "ID"
            Object.Width           =   0
         EndProperty
      End
      Begin VB.Label Label7 
         BackColor       =   &H80000018&
         Caption         =   "If this component includes other items, select them here to prevent them from showing up on the list of components to install."
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   -74880
         TabIndex        =   69
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label9 
         BackColor       =   &H80000018&
         Caption         =   "WinNT = 4.00, Win2000 = 5.00, WinXP = 5.01, Ignore = 0.00"
         ForeColor       =   &H80000017&
         Height          =   855
         Left            =   -69840
         TabIndex        =   22
         Top             =   2880
         Width           =   1335
      End
      Begin VB.Label Label42 
         BackStyle       =   0  'Transparent
         Caption         =   "1.0.3705"
         Height          =   255
         Left            =   -73440
         TabIndex        =   64
         Top             =   3240
         Width           =   3135
      End
      Begin VB.Label Label38 
         BackStyle       =   0  'Transparent
         Caption         =   "For the .Net framework version 1, enter:"
         Height          =   255
         Left            =   -74040
         TabIndex        =   63
         Top             =   2880
         Width           =   4335
      End
      Begin VB.Label Label41 
         Caption         =   "Version:"
         Height          =   255
         Left            =   -73680
         TabIndex        =   62
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   840
         Width           =   1095
      End
      Begin VB.Label Label40 
         BackColor       =   &H80000018&
         Caption         =   "Specify the .Net Framework version you would like to check for."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   61
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label39 
         BackColor       =   &H80000018&
         Caption         =   "Enter the version of the .Net framework you would like to check for."
         ForeColor       =   &H80000017&
         Height          =   1215
         Left            =   -74880
         TabIndex        =   60
         Top             =   2520
         Width           =   6975
      End
      Begin VB.Label Label37 
         Caption         =   "Estimated seconds to install"
         Height          =   255
         Left            =   -72480
         TabIndex        =   57
         Top             =   1920
         Width           =   2535
      End
      Begin VB.Label Label20 
         BackColor       =   &H80000018&
         Caption         =   "%CommonFilesPath% = c:\Program Files\Common Files"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74640
         TabIndex        =   56
         Top             =   3360
         Width           =   4455
      End
      Begin VB.Label Label19 
         BackColor       =   &H80000018&
         Caption         =   "%WinPath% = c:\windows on Win9x, c:\winnt on WinNT"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74640
         TabIndex        =   55
         Top             =   3120
         Width           =   4815
      End
      Begin VB.Label Label18 
         BackColor       =   &H80000018&
         Caption         =   "%WinSysPath% = c:\windows\system on 9x, c:\winnt\System32 on NT"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74640
         TabIndex        =   54
         Top             =   2880
         Width           =   5055
      End
      Begin VB.Label Label16 
         BackColor       =   &H80000018&
         Caption         =   "Specify the DLL and version to check"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   53
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label15 
         Caption         =   "DLL:"
         Height          =   255
         Left            =   -74280
         TabIndex        =   52
         Top             =   840
         Width           =   615
      End
      Begin VB.Label Label14 
         Caption         =   "Version:"
         Height          =   255
         Left            =   -74280
         TabIndex        =   51
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   1200
         Width           =   615
      End
      Begin VB.Label Label17 
         BackColor       =   &H80000018&
         Caption         =   $"frmComponent.frx":0134
         ForeColor       =   &H80000017&
         Height          =   1335
         Left            =   -74880
         TabIndex        =   50
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
         TabIndex        =   49
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
         TabIndex        =   46
         Top             =   3360
         Width           =   4335
      End
      Begin VB.Label Label35 
         Caption         =   "URL:"
         Height          =   255
         Left            =   240
         TabIndex        =   45
         Top             =   1200
         Width           =   615
      End
      Begin VB.Label Label33 
         BackColor       =   &H80000018&
         Caption         =   $"frmComponent.frx":01BB
         ForeColor       =   &H80000017&
         Height          =   1215
         Left            =   -74880
         TabIndex        =   44
         Top             =   2520
         Width           =   6975
      End
      Begin VB.Label Label32 
         BackColor       =   &H80000018&
         Caption         =   "Specify the full registry key and value to check (equality)"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   43
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label31 
         Caption         =   "Service Pack:"
         Height          =   255
         Left            =   -73680
         TabIndex        =   42
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
         TabIndex        =   41
         Top             =   3240
         Width           =   6975
      End
      Begin VB.Label Label29 
         BackColor       =   &H80000018&
         Caption         =   "Specify the full registry key and value to check (equality)"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   40
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label28 
         Caption         =   "Key:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   39
         Top             =   960
         Width           =   615
      End
      Begin VB.Label Label27 
         Caption         =   "Value:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   38
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
         Left            =   -71640
         TabIndex        =   37
         Top             =   1560
         Width           =   2415
      End
      Begin VB.Label Label24 
         BackColor       =   &H80000018&
         Caption         =   "Use the whole key, including HKEY_LOCAL_MACHINE, etc.  To query the 'default' value for a key, use a trailing \."
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   -74880
         TabIndex        =   36
         Top             =   3240
         Width           =   6975
      End
      Begin VB.Label Label23 
         BackColor       =   &H80000018&
         Caption         =   "Specify the full registry key and version to check"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   35
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label22 
         Caption         =   "Key:"
         Height          =   255
         Left            =   -74400
         TabIndex        =   34
         Top             =   960
         Width           =   615
      End
      Begin VB.Label Label21 
         Caption         =   "Version:"
         Height          =   255
         Left            =   -74400
         TabIndex        =   33
         ToolTipText     =   "Complete version number, ie x.x.x.x"
         Top             =   1560
         Width           =   615
      End
      Begin VB.Label Label2 
         Caption         =   "Message:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   32
         Top             =   840
         Width           =   855
      End
      Begin VB.Label Label3 
         Caption         =   "Command:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   31
         Top             =   1200
         Width           =   855
      End
      Begin VB.Label Label4 
         Caption         =   "CmdLine:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   30
         Top             =   1560
         Width           =   855
      End
      Begin VB.Label Label5 
         Caption         =   "Time:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   29
         Top             =   1920
         Width           =   855
      End
      Begin VB.Label Label6 
         Caption         =   "Reboot:"
         Height          =   255
         Left            =   -74640
         TabIndex        =   28
         Top             =   2520
         Width           =   855
      End
      Begin VB.Label Label13 
         BackColor       =   &H80000018&
         Caption         =   "Installation settings for this component."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   27
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label12 
         BackColor       =   &H80000018&
         Caption         =   "General information about this component"
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   120
         TabIndex        =   25
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label11 
         BackColor       =   &H80000018&
         Caption         =   "Before installing this component, the following components must be installed/not be required based on OS"
         ForeColor       =   &H80000017&
         Height          =   495
         Left            =   -74880
         TabIndex        =   24
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label10 
         BackColor       =   &H80000018&
         Caption         =   "OS requirements for this component to be checked."
         ForeColor       =   &H80000017&
         Height          =   255
         Left            =   -74880
         TabIndex        =   23
         Top             =   360
         Width           =   6975
      End
      Begin VB.Label Label1 
         Caption         =   "Name:"
         Height          =   255
         Left            =   240
         TabIndex        =   19
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
  Me.tb.TabVisible(9) = False
  ucOS1.Win9xEnable True
  ucOS1.WinNTEnable True
  
  Select Case Index
    Case 0        ' File version check
      Me.tb.TabVisible(8) = True
    Case 1        ' Registry version check
      Me.tb.TabVisible(5) = True
    Case 2        ' Registry key check
      Me.tb.TabVisible(6) = True
    Case 3        ' NT service pack check
      Me.tb.TabVisible(7) = True
      ucOS1.Win9xEnable False
      ucOS1.WinNT = True
      If Me.txtNTServicePackCheckNumber = "" Then
        Me.txtNTServicePackCheckNumber = "Service Pack x"
      End If
    Case 5          ' .Net framework version
      Me.tb.TabVisible(9) = True
      If txtNetFrameworkVersion = "" Then
        txtNetFrameworkVersion = "1.0.3705"
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
    If objComp.ID <> mobjComponent.ID And Not objComp.Delete Then
      ' add to depends list
      Set li = Me.lvDependencies.ListItems.Add(, , objComp.Name)
      li.SubItems(1) = objComp.ID
      If InStr(mobjComponent.Dependencies, " " & li.SubItems(1) & " ") > 0 Then
        li.Checked = True
      End If
      ' add to include list
      Set li = Me.lvIncludes.ListItems.Add(, , objComp.Name)
      li.SubItems(1) = objComp.ID
      If InStr(mobjComponent.Includes, " " & li.SubItems(1) & " ") > 0 Then
        li.Checked = True
      End If
    End If
  Next objComp
  
  Me.txtNotes = mobjComponent.Notes
  
  ' OS Tab
  ucOS1.Win9x = mobjComponent.mblnWin9x
  If ucOS1.Win9x Then
    ucOS1.Windows95 = mobjComponent.mblnWindows95
    ucOS1.Windows98 = mobjComponent.mblnWindows98
    ucOS1.WindowsMe = mobjComponent.mblnWindowsMe
  End If
  
  ucOS1.WinNT = mobjComponent.mblnWinNT
  If ucOS1.WinNT Then
    ucOS1.NTMinVersion = mobjComponent.mstrNTMinVersion
    ucOS1.NTMinServicePack = mobjComponent.mstrNTMinServicePack
    ucOS1.NTMaxVersion = mobjComponent.mstrNTMaxVersion
    ucOS1.NTMaxServicePack = mobjComponent.mstrNTMaxServicePack
  End If
  
  ' Installation tab
  Me.txtSetupMessage = mobjComponent.SetupMessage
  Me.txtSetupCommand = mobjComponent.SetupCommand
  Me.txtSetupCommandLine = mobjComponent.SetupCommandLine
  Me.txtSetupTime = CStr(mobjComponent.SetupTime)
  Me.optComponentType(mobjComponent.ComponentType) = True
  optComponentType_Click mobjComponent.ComponentType
  Me.cmbReboot.ListIndex = mobjComponent.RebootType
  Me.txtURL = mobjComponent.URL
  
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

  ' Net framework tab
  Me.txtNetFrameworkVersion.Text = mobjComponent.NetFrameworkVersion
  
  
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
  ElseIf Me.optComponentType(5) Then
    mobjComponent.ComponentType = NetFrameworkCheck
  Else
    Err.Raise vbObjectError, "frmComponent.Save", "Invalid component type specified."
  End If
  
  ' OS Tab
  mobjComponent.mblnWin9x = ucOS1.Win9x
  If ucOS1.Win9x Then
    mobjComponent.mblnWindows95 = ucOS1.Windows95
    mobjComponent.mblnWindows98 = ucOS1.Windows98
    mobjComponent.mblnWindowsMe = ucOS1.WindowsMe
  End If
  
  mobjComponent.mblnWinNT = ucOS1.WinNT
  If ucOS1.WinNT Then
    mobjComponent.mstrNTMinVersion = ucOS1.NTMinVersion
    mobjComponent.mstrNTMinServicePack = ucOS1.NTMinServicePack
    mobjComponent.mstrNTMaxVersion = ucOS1.NTMaxVersion
    mobjComponent.mstrNTMaxServicePack = ucOS1.NTMaxServicePack
  End If
  
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
  
  ' Net framework tab
  mobjComponent.NetFrameworkVersion = txtNetFrameworkVersion.Text
  
  ' Dependencies
  Dim li As ListItem, strTemp As String
  For Each li In Me.lvDependencies.ListItems
    If li.Checked Then
      If strTemp <> "" Then strTemp = strTemp & " "
      strTemp = strTemp & li.SubItems(1)
    End If
  Next li
  mobjComponent.Dependencies = strTemp

  ' Includes
  strTemp = ""
  For Each li In Me.lvIncludes.ListItems
    If li.Checked Then
      If strTemp <> "" Then strTemp = strTemp & " "
      strTemp = strTemp & li.SubItems(1)
    End If
  Next li
  mobjComponent.Includes = strTemp

End Sub

Private Function ValidateSettings() As Boolean
  
  ' check tab 0, basic stuff
  
  ' check tab 1, os stuff
  If ucOS1.WinNT Then
    If Not IsNumeric(ucOS1.NTMaxVersion) Then
      MsgBox "Invalid OS max version specified.", vbExclamation
      Me.tb.Tab = 1
      ucOS1.SetFocus
      Exit Function
    End If
    If Not IsNumeric(ucOS1.NTMinVersion) Then
      MsgBox "Invalid OS min version specified.", vbExclamation
      Me.tb.Tab = 1
      ucOS1.SetFocus
      Exit Function
    End If
  End If
  
  If Me.optComponentType(3) And IsNumeric(ucOS1.NTMaxVersion) And IsNumeric(ucOS1.NTMinVersion) And (ucOS1.NTMaxVersion = 0 Or ucOS1.NTMinVersion = 0) Then
    MsgBox "When checking an NT service pack, the OS version min and max must be set to a valid Windows NT version number.", vbExclamation
    Me.tb.Tab = 1
    ucOS1.SetFocus
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

Private Sub txtNotes_GotFocus()
  cmdSave.Default = False
End Sub

Private Sub txtNotes_LostFocus()
  cmdSave.Default = True
End Sub
