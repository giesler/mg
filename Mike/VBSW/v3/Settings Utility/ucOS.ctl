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
         Style           =   2  'Dropdown List
         TabIndex        =   10
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
      Begin VB.ComboBox cmbNTMinServicePack 
         Height          =   315
         Left            =   2760
         Style           =   2  'Dropdown List
         TabIndex        =   7
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

Public Property Let NTMinVersion(vData As Single)
  txtNTMinVersion.Text = CStr(Format(vData, "0.00"))
End Property

Public Property Get NTMinVersion() As Single
  NTMinVersion = Val(txtNTMinVersion.Text)
End Property

Public Property Let NTMinServicePack(vData As String)
  If vData <> "" Then
    SetComboValue cmbNTMinServicePack, vData
  End If
End Property

Public Property Get NTMinServicePack() As String
  If cmbNTMinServicePack.ListIndex <> -1 Then
    NTMinServicePack = cmbNTMinServicePack.List(cmbNTMinServicePack.ListIndex)
  Else
    NTMinServicePack = ""
  End If
End Property

Public Property Let NTMaxVersion(vData As Single)
  txtNTMaxVersion.Text = CStr(Format(vData, "0.00"))
End Property

Public Property Get NTMaxVersion() As Single
  NTMaxVersion = Val(txtNTMaxVersion.Text)
End Property

Public Property Let NTMaxServicePack(vData As String)
  If vData <> "" Then
    SetComboValue cmbNTMaxServicePack, vData
  End If
End Property

Public Property Get NTMaxServicePack() As String
  If cmbNTMaxServicePack.ListIndex <> -1 Then
    NTMaxServicePack = cmbNTMaxServicePack.List(cmbNTMaxServicePack.ListIndex)
  Else
    NTMaxServicePack = ""
  End If
End Property

Public Function SetComboValue(cmb As ComboBox, str As String)

  Dim i As Integer
  For i = 0 To cmb.ListCount - 1
    If cmb.List(i) = str Then
      cmb.ListIndex = i
      Exit Function
    End If
  Next i
  
End Function

Private Sub chkWin9x_Click()

  If chkWin9x.Value And chkWin9x.Enabled Then
    chkWindows95.Enabled = True
    chkWindows98.Enabled = True
    chkWindowsMe.Enabled = True
    If chkWindows95.Value = 0 And chkWindows98.Value = 0 And chkWindowsMe.Value = 0 Then
      chkWindows95.Value = 1
      chkWindows98.Value = 1
      chkWindowsMe.Value = 1
    End If
  Else
    chkWindows95.Enabled = False
    chkWindows98.Enabled = False
    chkWindowsMe.Enabled = False
  End If

End Sub

Private Sub chkWinNT_Click()
  
  If chkWinNT.Value And chkWinNT.Enabled Then
    txtNTMinVersion.Enabled = True
    txtNTMaxVersion.Enabled = True
    cmbNTMinServicePack.Enabled = True
    cmbNTMaxServicePack.Enabled = True
    txtNTMinVersion.BackColor = vbWindowBackground
    txtNTMaxVersion.BackColor = vbWindowBackground
    cmbNTMinServicePack.BackColor = vbWindowBackground
    cmbNTMaxServicePack.BackColor = vbWindowBackground
  Else
    txtNTMinVersion.Enabled = False
    txtNTMaxVersion.Enabled = False
    cmbNTMinServicePack.Enabled = False
    cmbNTMaxServicePack.Enabled = False
    txtNTMinVersion.BackColor = vbButtonFace
    txtNTMaxVersion.BackColor = vbButtonFace
    cmbNTMinServicePack.BackColor = vbButtonFace
    cmbNTMaxServicePack.BackColor = vbButtonFace
  End If
  
End Sub

Private Sub UserControl_Initialize()
  
  cmbNTMinServicePack.AddItem ""
  cmbNTMinServicePack.AddItem "Service Pack 1"
  cmbNTMinServicePack.AddItem "Service Pack 2"
  cmbNTMinServicePack.AddItem "Service Pack 3"
  cmbNTMinServicePack.AddItem "Service Pack 4"
  cmbNTMinServicePack.AddItem "Service Pack 5"
  cmbNTMinServicePack.AddItem "Service Pack 6"
  
  cmbNTMaxServicePack.AddItem ""
  cmbNTMaxServicePack.AddItem "Service Pack 1"
  cmbNTMaxServicePack.AddItem "Service Pack 2"
  cmbNTMaxServicePack.AddItem "Service Pack 3"
  cmbNTMaxServicePack.AddItem "Service Pack 4"
  cmbNTMaxServicePack.AddItem "Service Pack 5"
  cmbNTMaxServicePack.AddItem "Service Pack 6"
  
  chkWin9x_Click
  chkWinNT_Click
  
End Sub

Public Sub Win9xEnable(enable As Boolean)
  chkWin9x.Enabled = enable
  If Not chkWin9x.Enabled Then
    chkWin9x.Value = 0
    chkWindows95.Value = 0
    chkWindows98.Value = 0
    chkWindowsMe.Value = 0
  End If
  chkWin9x_Click
End Sub

Public Sub WinNTEnable(enable As Boolean)
  chkWinNT.Enabled = enable
  chkWinNT_Click
End Sub

