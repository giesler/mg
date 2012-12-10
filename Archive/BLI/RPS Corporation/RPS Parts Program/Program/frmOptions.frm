VERSION 5.00
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Begin VB.Form frmOptions 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Options"
   ClientHeight    =   4920
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4800
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4920
   ScaleWidth      =   4800
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Tag             =   "Options"
   Begin TabDlg.SSTab tbOptions 
      Height          =   4215
      Left            =   120
      TabIndex        =   9
      Top             =   120
      Width           =   4575
      _ExtentX        =   8070
      _ExtentY        =   7435
      _Version        =   393216
      Style           =   1
      Tabs            =   2
      Tab             =   1
      TabHeight       =   520
      TabCaption(0)   =   "Settings"
      TabPicture(0)   =   "frmOptions.frx":0000
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "chkOldData"
      Tab(0).Control(1)=   "chkTip"
      Tab(0).Control(2)=   "txtCompanyName"
      Tab(0).Control(3)=   "Label1"
      Tab(0).ControlCount=   4
      TabCaption(1)   =   "Advanced"
      TabPicture(1)   =   "frmOptions.frx":001C
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "Label2"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "Label3"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "txtUpdateServer"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).ControlCount=   3
      Begin VB.TextBox txtUpdateServer 
         Height          =   285
         Left            =   360
         TabIndex        =   14
         Top             =   1560
         Width           =   3855
      End
      Begin VB.CheckBox chkOldData 
         Caption         =   "&Disable warning when data file is older than 6 months"
         Height          =   255
         Left            =   -74760
         TabIndex        =   13
         Top             =   1680
         Width           =   4215
      End
      Begin VB.CheckBox chkTip 
         Caption         =   "Show &tip when program starts"
         Height          =   255
         Left            =   -74760
         TabIndex        =   12
         Top             =   1320
         Width           =   4095
      End
      Begin VB.TextBox txtCompanyName 
         Height          =   285
         Left            =   -74760
         TabIndex        =   10
         Top             =   840
         Width           =   3855
      End
      Begin VB.Label Label3 
         Caption         =   $"frmOptions.frx":0038
         Height          =   735
         Left            =   240
         TabIndex        =   16
         Top             =   480
         Width           =   3855
      End
      Begin VB.Label Label2 
         Caption         =   "Update Server"
         Height          =   255
         Left            =   360
         TabIndex        =   15
         Top             =   1335
         Width           =   1335
      End
      Begin VB.Label Label1 
         Caption         =   "Company Name (Dealer Name)"
         Height          =   255
         Left            =   -74760
         TabIndex        =   11
         Top             =   615
         Width           =   3015
      End
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Height          =   375
      Left            =   1050
      TabIndex        =   0
      Tag             =   "OK"
      Top             =   4455
      Width           =   1095
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   2280
      TabIndex        =   1
      Tag             =   "Cancel"
      Top             =   4455
      Width           =   1095
   End
   Begin VB.CommandButton cmdApply 
      Caption         =   "&Apply"
      Height          =   375
      Left            =   3480
      TabIndex        =   2
      Tag             =   "&Apply"
      Top             =   4455
      Width           =   1095
   End
   Begin VB.PictureBox picOptions 
      BorderStyle     =   0  'None
      Height          =   3780
      Index           =   3
      Left            =   -20000
      ScaleHeight     =   3840.968
      ScaleMode       =   0  'User
      ScaleWidth      =   5745.64
      TabIndex        =   4
      TabStop         =   0   'False
      Top             =   480
      Width           =   5685
      Begin VB.Frame fraSample4 
         Caption         =   "Sample 4"
         Height          =   2022
         Left            =   505
         TabIndex        =   8
         Tag             =   "Sample 4"
         Top             =   502
         Width           =   2033
      End
   End
   Begin VB.PictureBox picOptions 
      BorderStyle     =   0  'None
      Height          =   3780
      Index           =   2
      Left            =   -20000
      ScaleHeight     =   3840.968
      ScaleMode       =   0  'User
      ScaleWidth      =   5745.64
      TabIndex        =   6
      TabStop         =   0   'False
      Top             =   480
      Width           =   5685
      Begin VB.Frame fraSample3 
         Caption         =   "Sample 3"
         Height          =   2022
         Left            =   406
         TabIndex        =   7
         Tag             =   "Sample 3"
         Top             =   403
         Width           =   2033
      End
   End
   Begin VB.PictureBox picOptions 
      BorderStyle     =   0  'None
      Height          =   3780
      Index           =   1
      Left            =   -20000
      ScaleHeight     =   3840.968
      ScaleMode       =   0  'User
      ScaleWidth      =   5745.64
      TabIndex        =   3
      TabStop         =   0   'False
      Top             =   480
      Width           =   5685
      Begin VB.Frame fraSample2 
         Caption         =   "Sample 2"
         Height          =   2022
         Left            =   307
         TabIndex        =   5
         Tag             =   "Sample 2"
         Top             =   305
         Width           =   2033
      End
   End
End
Attribute VB_Name = "frmOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private bDirty As Boolean

Private Sub chkOldData_Click()
bDirty = True
End Sub

Private Sub chkTip_Click()
bDirty = True
End Sub

Private Sub cmdApply_Click()

SaveSetting App.Title, "Options", "UpdateServer", Me.txtUpdateServer
SaveSetting App.Title, "Options", "CompanyName", Me.txtCompanyName
SaveSetting App.Title, "Options", "Hide Update Warning", Me.chkOldData
SaveSetting App.Title, "Options", "Show Tips at Startup", Me.chkTip
Me.cmdApply.Enabled = False
bDirty = False

End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOK_Click()
  
cmdApply_Click
Unload Me

End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
  Dim i As Integer
  i = tbOptions.Tab
  'handle ctrl+tab to move to the next tab
  If (Shift And 3) = 2 And KeyCode = vbKeyTab Then
    If i = tbOptions.Tabs - 1 Then
      'last tab so we need to wrap to tab 1
      tbOptions.Tab = 0
    Else
      'increment the tab
      tbOptions.Tab = i + 1
    End If
  ElseIf (Shift And 3) = 3 And KeyCode = vbKeyTab Then
    If i = 0 Then
      'last tab so we need to wrap to tab 1
      tbOptions.Tab = tbOptions.Tabs - 1
    Else
      'increment the tab
      tbOptions.Tab = i - 1
    End If
  End If
End Sub

Private Sub Form_Load()

Me.txtUpdateServer = GetSetting(App.Title, "Options", "UpdateServer", sUpdateServer)
Me.txtCompanyName = GetSetting(App.Title, "Options", "CompanyName", "")
Me.chkOldData = GetSetting(App.Title, "Options", "Hide Update Warning", 0)
Me.chkTip = GetSetting(App.Title, "Options", "Show Tips at Startup", 1)

End Sub

Private Sub txtCompanyName_Change()
bDirty = True
End Sub

Private Sub txtUpdateServer_Change()
bDirty = True
End Sub
