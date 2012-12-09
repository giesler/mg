VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmSelectPermissions 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Factory Cat Database Security"
   ClientHeight    =   4500
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6435
   Icon            =   "frmSelectPermissions.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4500
   ScaleWidth      =   6435
   StartUpPosition =   2  'CenterScreen
   Begin VB.PictureBox Picture1 
      BackColor       =   &H80000005&
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   5640
      Picture         =   "frmSelectPermissions.frx":08CA
      ScaleHeight     =   615
      ScaleWidth      =   615
      TabIndex        =   12
      Top             =   120
      Width           =   615
   End
   Begin VB.CommandButton cmdBack 
      Caption         =   "< &Back"
      Height          =   375
      Left            =   3240
      TabIndex        =   6
      Top             =   4080
      Width           =   975
   End
   Begin VB.Frame fraLine 
      Height          =   30
      Left            =   -120
      TabIndex        =   11
      Top             =   3960
      Width           =   6615
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "&Next >"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   7
      Top             =   4080
      Width           =   975
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   5400
      TabIndex        =   8
      Top             =   4080
      Width           =   975
   End
   Begin VB.Frame Frame1 
      Caption         =   "&Access Type"
      Height          =   1935
      Left            =   4320
      TabIndex        =   1
      Top             =   960
      Width           =   2055
      Begin VB.OptionButton optAccessType 
         Caption         =   "Default &access"
         Height          =   255
         Index           =   3
         Left            =   120
         TabIndex        =   5
         ToolTipText     =   "Resets the selected node to no access level required."
         Top             =   1440
         Width           =   1375
      End
      Begin VB.OptionButton optAccessType 
         Caption         =   "&Read Only"
         Height          =   255
         Index           =   1
         Left            =   120
         TabIndex        =   3
         Top             =   720
         Width           =   1455
      End
      Begin VB.OptionButton optAccessType 
         Caption         =   "&Full Access"
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   2
         Top             =   360
         Width           =   1455
      End
      Begin VB.OptionButton optAccessType 
         Caption         =   "&Deny Access"
         Height          =   255
         Index           =   2
         Left            =   120
         TabIndex        =   4
         Top             =   1080
         Width           =   1335
      End
      Begin VB.Image Image4 
         Height          =   300
         Left            =   1560
         Picture         =   "frmSelectPermissions.frx":1194
         Stretch         =   -1  'True
         Top             =   1440
         Width           =   300
      End
      Begin VB.Image Image3 
         Height          =   300
         Left            =   1560
         Picture         =   "frmSelectPermissions.frx":15D6
         Stretch         =   -1  'True
         Top             =   720
         Width           =   300
      End
      Begin VB.Image Image2 
         Height          =   300
         Left            =   1560
         Picture         =   "frmSelectPermissions.frx":1A18
         Stretch         =   -1  'True
         Top             =   360
         Width           =   300
      End
      Begin VB.Image Image1 
         Height          =   300
         Left            =   1560
         Picture         =   "frmSelectPermissions.frx":1E5A
         Stretch         =   -1  'True
         Top             =   1080
         Width           =   300
      End
   End
   Begin MSComctlLib.ImageList ImageList1 
      Left            =   480
      Top             =   3840
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   5
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmSelectPermissions.frx":2724
            Key             =   "full"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmSelectPermissions.frx":2B76
            Key             =   "nothing2"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmSelectPermissions.frx":2FC8
            Key             =   "read"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmSelectPermissions.frx":341A
            Key             =   "nothing"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmSelectPermissions.frx":3574
            Key             =   "deny"
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.TreeView tvSwitchboard 
      Height          =   2895
      Left            =   120
      TabIndex        =   0
      Top             =   960
      Width           =   4095
      _ExtentX        =   7223
      _ExtentY        =   5106
      _Version        =   393217
      HideSelection   =   0   'False
      Indentation     =   441
      LabelEdit       =   1
      Style           =   7
      HotTracking     =   -1  'True
      ImageList       =   "ImageList1"
      Appearance      =   1
   End
   Begin VB.Label Label2 
      BackStyle       =   0  'Transparent
      Caption         =   "Anything you modify will have the appropriate icon added next to it."
      Height          =   255
      Left            =   480
      TabIndex        =   10
      Top             =   480
      Width           =   5775
   End
   Begin VB.Label Label1 
      BackStyle       =   0  'Transparent
      Caption         =   "Choose the permissions you would like to modify"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   9
      Top             =   120
      Width           =   6015
   End
   Begin VB.Shape Shape1 
      BackStyle       =   1  'Opaque
      BorderColor     =   &H80000005&
      Height          =   855
      Left            =   0
      Top             =   0
      Width           =   6495
   End
End
Attribute VB_Name = "frmSelectPermissions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean
Public blnBack As Boolean
Private blnTVSelect As Boolean

Private Sub cmdBack_Click()
blnBack = True
Me.Visible = False
End Sub

Private Sub cmdCancel_Click()
blnCancel = True
Me.Visible = False
End Sub

Private Sub cmdNext_Click()
blnCancel = False
Me.Visible = False
End Sub

Private Sub Form_Load()
blnCancel = True
Dim n As Node
OpenWaitForm "Loading menu items..."
Set n = AddSwitchboard(1, -1)
CloseWaitForm
n.Expanded = True
blnTVSelect = False
End Sub

Public Function AddSwitchboard(swid As Long, iLastID As Long, Optional pNode As Node) As Node
On Error GoTo AddSwitchboard_Err

Dim newpNode As Node, cNode As Node, i As Long
Dim r As New Recordset, sql As String
sql = "Select * From dbo.tblSwitchboard Where SwitchboardID = " & swid & " Order By ItemNumber"
r.Open sql, cnn, adOpenForwardOnly, adLockReadOnly

If iLastID = -1 Then iLastID = r!ID

'add parent node
If pNode Is Nothing Then
  Set newpNode = Me.tvSwitchboard.Nodes.Add(, , "id:" & iLastID, r!ItemText, "nothing")
Else
  Set newpNode = Me.tvSwitchboard.Nodes.Add(pNode, tvwChild, "id:" & iLastID, r!ItemText, "nothing")
End If
Set AddSwitchboard = newpNode

'add children nodes
r.MoveNext
Do While Not r.EOF
  If r!Command = 1 Then
    AddSwitchboard Val(r!Argument), r!ID, newpNode
  Else
    Set cNode = Me.tvSwitchboard.Nodes.Add(newpNode, tvwChild, "id:" & r!ID, r!ItemText, "nothing")
  End If
  r.MoveNext
Loop

r.Close
Set r = Nothing

Exit Function
AddSwitchboard_Err:
ErrHand Me.Name, "AddSwitchboard"
Exit Function
End Function

Private Sub optAccessType_Click(Index As Integer)
On Error GoTo optAccessType_Err

Dim cNode As Node
Set cNode = Me.tvSwitchboard.SelectedItem
If cNode Is Nothing Then
  MsgBox "You must select an area to change security settings.", vbExclamation
  Exit Sub
End If

' make sure we didn't just chose a node, ie we clicked an option
If Not blnTVSelect Then cNode.Tag = "updated"
Select Case Index
  Case 0
    cNode.Image = "full"
  Case 1
    cNode.Image = "read"
  Case 2
    cNode.Image = "deny"
  Case 3
    cNode.Image = "nothing"
End Select


Exit Sub
optAccessType_Err:
ErrHand Me.Name, "optAccessType"
Exit Sub

End Sub

Private Sub tvSwitchboard_Click()
On Error GoTo tvSwitchboard_Click_Err

Dim cNode As Node

Me.optAccessType.Item(0).Value = False
Me.optAccessType.Item(1).Value = False
Me.optAccessType.Item(2).Value = False
Me.optAccessType.Item(3).Value = False

Set cNode = Me.tvSwitchboard.SelectedItem

If cNode Is Nothing Then Exit Sub

blnTVSelect = True
Select Case cNode.Image
  Case "full"
    Me.optAccessType.Item(0).Value = True
  Case "read"
    Me.optAccessType.Item(1).Value = True
  Case "deny"
    Me.optAccessType.Item(2).Value = True
  Case Else
    Me.optAccessType.Item(3).Value = True
End Select
blnTVSelect = False

Exit Sub
tvSwitchboard_Click_Err:
ErrHand Me.Name, "tvSwitchboard_Click_Err"
Exit Sub
End Sub
