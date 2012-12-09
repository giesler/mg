VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmSelectUsers 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Factory Cat Database Security"
   ClientHeight    =   4500
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6435
   Icon            =   "frmSelectUsers.frx":0000
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
      Picture         =   "frmSelectUsers.frx":08CA
      ScaleHeight     =   615
      ScaleWidth      =   615
      TabIndex        =   7
      Top             =   120
      Width           =   615
   End
   Begin VB.CommandButton cmdBack 
      Caption         =   "< &Back"
      Height          =   375
      Left            =   3240
      TabIndex        =   1
      Top             =   4080
      Width           =   975
   End
   Begin VB.Frame fraLine 
      Height          =   30
      Left            =   -120
      TabIndex        =   6
      Top             =   3960
      Width           =   6615
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "&Next >"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   2
      Top             =   4080
      Width           =   975
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   5400
      TabIndex        =   3
      Top             =   4080
      Width           =   975
   End
   Begin MSComctlLib.ListView lvUsers 
      Height          =   2895
      Left            =   240
      TabIndex        =   0
      Top             =   960
      Width           =   6015
      _ExtentX        =   10610
      _ExtentY        =   5106
      View            =   2
      LabelEdit       =   1
      LabelWrap       =   -1  'True
      HideSelection   =   -1  'True
      Checkboxes      =   -1  'True
      _Version        =   393217
      SmallIcons      =   "ImageList1"
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      NumItems        =   1
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "User Name"
         Object.Width           =   7056
      EndProperty
   End
   Begin MSComctlLib.ImageList ImageList1 
      Left            =   120
      Top             =   3720
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   1
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmSelectUsers.frx":1194
            Key             =   "user"
         EndProperty
      EndProperty
   End
   Begin VB.Label Label2 
      BackStyle       =   0  'Transparent
      Caption         =   "If you only select one user, you will see that users current security."
      Height          =   255
      Left            =   480
      TabIndex        =   5
      Top             =   480
      Width           =   5775
   End
   Begin VB.Label Label1 
      BackStyle       =   0  'Transparent
      Caption         =   "Choose the users you want to modify permissions for."
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
      TabIndex        =   4
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
Attribute VB_Name = "frmSelectUsers"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean
Public blnBack As Boolean
Public sServer As String

Private Sub cmdBack_Click()
blnBack = True
Me.Visible = False
End Sub

Private Sub cmdCancel_Click()
blnCancel = True
Me.Visible = False
End Sub

Private Sub cmdNext_Click()

Dim i As Integer, bFlag As Boolean, iCheckCount As Integer
iCheckCount = 0
For i = 1 To Me.lvUsers.ListItems.Count
  If Me.lvUsers.ListItems.Item(i).Checked Then
    bFlag = True
    sSingleUserName = Me.lvUsers.ListItems.Item(i).Text
    iCheckCount = iCheckCount + 1
  End If
Next i
If iCheckCount = 1 Then
  blnSingleUser = True
End If
If Not bFlag Then
  MsgBox "You must select at least one user!", vbExclamation
  Exit Sub
End If
blnCancel = False
Me.Visible = False
End Sub

Private Sub Form_Load()
blnCancel = True
End Sub

