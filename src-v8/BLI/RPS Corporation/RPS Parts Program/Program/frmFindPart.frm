VERSION 5.00
Begin VB.Form frmFindPart 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Find Part"
   ClientHeight    =   1665
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3675
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1665
   ScaleWidth      =   3675
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.TextBox txtFind 
      Height          =   285
      Left            =   1080
      TabIndex        =   6
      Top             =   825
      Width           =   2415
   End
   Begin VB.ComboBox comMatch 
      Height          =   315
      ItemData        =   "frmFindPart.frx":0000
      Left            =   1080
      List            =   "frmFindPart.frx":000D
      Style           =   2  'Dropdown List
      TabIndex        =   4
      Top             =   480
      Width           =   2415
   End
   Begin VB.ComboBox comField 
      Height          =   315
      ItemData        =   "frmFindPart.frx":0041
      Left            =   1080
      List            =   "frmFindPart.frx":004B
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   120
      Width           =   2415
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   2160
      TabIndex        =   1
      Top             =   1200
      Width           =   855
   End
   Begin VB.CommandButton cmdFind 
      Caption         =   "&Find Next"
      Default         =   -1  'True
      Height          =   375
      Left            =   840
      TabIndex        =   0
      Top             =   1200
      Width           =   1095
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      Caption         =   "Find What:"
      Height          =   255
      Left            =   -120
      TabIndex        =   7
      Top             =   840
      Width           =   1095
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "Match:"
      Height          =   255
      Left            =   -120
      TabIndex        =   5
      Top             =   510
      Width           =   1095
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "Field:"
      Height          =   255
      Left            =   -120
      TabIndex        =   3
      Top             =   150
      Width           =   1095
   End
End
Attribute VB_Name = "frmFindPart"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public bCancel As Boolean

Private Sub cmdCancel_Click()
bCancel = True
Me.Visible = False
End Sub

