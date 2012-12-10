VERSION 5.00
Begin VB.UserControl ucOS 
   ClientHeight    =   3330
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   5025
   ScaleHeight     =   3330
   ScaleWidth      =   5025
   Begin VB.CheckBox chkWinNT 
      Caption         =   "Win NT/2000/XP"
      Height          =   255
      Left            =   240
      TabIndex        =   12
      Top             =   1800
      Width           =   1815
   End
   Begin VB.CheckBox chkWin9x 
      Caption         =   "Win 9x"
      Height          =   255
      Left            =   240
      TabIndex        =   11
      Top             =   120
      Width           =   1095
   End
   Begin VB.Frame Frame1 
      Height          =   1335
      Left            =   120
      TabIndex        =   4
      Top             =   1800
      Width           =   4695
      Begin VB.ComboBox cmbNTMaxServicePack 
         Height          =   315
         Left            =   2760
         TabIndex        =   10
         Text            =   "Combo1"
         Top             =   840
         Width           =   1695
      End
      Begin VB.TextBox txtNTMaxVersion 
         Height          =   285
         Left            =   1560
         TabIndex        =   9
         Top             =   840
         Width           =   975
      End
      Begin VB.ComboBox cmbNTMinSerivcePack 
         Height          =   315
         Left            =   2760
         TabIndex        =   7
         Text            =   "Combo1"
         Top             =   360
         Width           =   1695
      End
      Begin VB.TextBox txtNTMinVersion 
         Height          =   285
         Left            =   1560
         TabIndex        =   6
         Top             =   360
         Width           =   975
      End
      Begin VB.Label Label2 
         Caption         =   "Maximum version:"
         Height          =   255
         Left            =   120
         TabIndex        =   8
         Top             =   840
         Width           =   1575
      End
      Begin VB.Label Label1 
         Caption         =   "Minimum version:"
         Height          =   255
         Left            =   120
         TabIndex        =   5
         Top             =   360
         Width           =   1575
      End
   End
   Begin VB.Frame fraWin9x 
      Caption         =   " Win9x "
      Height          =   1575
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4695
      Begin VB.CheckBox chkWindowsMe 
         Caption         =   "Windows Me"
         Height          =   255
         Left            =   360
         TabIndex        =   3
         Top             =   1080
         Width           =   2655
      End
      Begin VB.CheckBox chkWindows98 
         Caption         =   "Windows 98, 98 SE"
         Height          =   255
         Left            =   360
         TabIndex        =   2
         Top             =   720
         Width           =   2655
      End
      Begin VB.CheckBox chkWindows95 
         Caption         =   "Windows 95"
         Height          =   255
         Left            =   360
         TabIndex        =   1
         Top             =   360
         Width           =   2655
      End
   End
End
Attribute VB_Name = "ucOS"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False

Public Property Let Win9x(vData As Boolean)
  chkWin9x.Value = IIf(vData, 1, 0)
  chkWin9x_Click
End Property

Public Property Get Win9x() As Boolean
  Win9x = IIf(chkWin9x.Value = 1, True, False)
End Property

Public Property Let Windows95(vData As Boolean)
  chkWindows95.Value = IIf(vData, 1, 0)
End Property

Public Property Get Windows95() As Boolean
  Windows95 = IIf(chkWindows95.Value = 1, True, False)
End Property

Public Property Let Windows98(vData As Boolean)
  chkWindows98.Value = IIf(vData, 1, 0)
End Property

Public Property Get Windows98() As Boolean
  Windows98 = IIf(chkWindows98.Value = 1, True, False)
End Property

Public Property Let WindowsMe(vData As Boolean)
  chkWindowsMe.Value = IIf(vData, 1, 0)
End Property

Public Property Get WindowsMe() As Boolean
  WindowsMe = IIf(chkWindowsMe.Value = 1, True, False)
End Property



Public Property Let WinNT(vData As Boolean)
  chkWinNT.Value = IIf(vData, 1, 0)
  chkWinNT_Click
End Property

Public Property Get WinNT() As Boolean
  WinNT = IIf(chkWinNT.Value = 1, True, False)
End Property

Public Property Let NTMinVersion(vData As String)
  txtNTMinVersion.Text = vData
End Property

Public Property Get NTMinVersion() As String
  NTMinVersion = txtNTMinVersion.Text
End Property

Public Property Let NTMinServicePack(vData As String)
  cmbNTMinSerivcePack.SelText = vData
End Property

Public Property Get NTMinServicePack() As String
  NTMinServicePack = cmbNTMinSerivcePack.SelText
End Property

Public Property Let NTMaxVersion(vData As String)
  txtNTMaxVersion.Text = vData
End Property

Public Property Get NTMaxVersion() As String
  NTMaxVersion = txtNTMaxVersion.Text
End Property

Public Property Let NTMaxServicePack(vData As String)
  cmbNTMaxSerivcePack.SelText = vData
End Property

Public Property Get NTMaxServicePack() As String
  NTMaxServicePack = cmbNTMaxSerivcePack.SelText
End Property



Private Sub chkWin9x_Click()

  If chkWin9x.Value Then
    chkWindows95.Enabled = True
    chkWindows98.Enabled = True
    chkWindowsMe.Enabled = True
  Else
    chkWindows95.Enabled = False
    chkWindows98.Enabled = False
    chkWindowsMe.Enabled = False
  End If

End Sub

Private Sub chkWinNT_Click()
  
  If chkWinNT.Value Then
    txtNTMinVersion.Enabled = True
    txtNTMaxVersion.Enabled = True
    cmbNTMinSerivcePack.Enabled = True
    cmbNTMaxServicePack.Enabled = True
  Else
    txtNTMinVersion.Enabled = False
    txtNTMaxVersion.Enabled = False
    cmbNTMinSerivcePack.Enabled = False
    cmbNTMaxServicePack.Enabled = False
  End If
  
End Sub

Private Sub UserControl_Initialize()
  
  cmbNTMinSerivcePack.AddItem ""
  cmbNTMinSerivcePack.AddItem "Service Pack 1"
  cmbNTMinSerivcePack.AddItem "Service Pack 2"
  cmbNTMinSerivcePack.AddItem "Service Pack 3"
  cmbNTMinSerivcePack.AddItem "Service Pack 4"
  cmbNTMinSerivcePack.AddItem "Service Pack 5"
  cmbNTMinSerivcePack.AddItem "Service Pack 6"
  
  cmbNTMaxSerivcePack.AddItem ""
  cmbNTMaxSerivcePack.AddItem "Service Pack 1"
  cmbNTMaxSerivcePack.AddItem "Service Pack 2"
  cmbNTMaxSerivcePack.AddItem "Service Pack 3"
  cmbNTMaxSerivcePack.AddItem "Service Pack 4"
  cmbNTMaxSerivcePack.AddItem "Service Pack 5"
  cmbNTMaxSerivcePack.AddItem "Service Pack 6"
  
End Sub
