VERSION 5.00
Begin VB.Form frmSettings 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Load &User List - replaces current user list with updated NT list"
   ClientHeight    =   4500
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6390
   Icon            =   "frmSettings.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4500
   ScaleWidth      =   6390
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkResetSecTable 
      Caption         =   "&Reset Security Table - clears all security info"
      Height          =   255
      Left            =   240
      TabIndex        =   22
      Top             =   3480
      Width           =   5655
   End
   Begin VB.Frame Frame2 
      Caption         =   "User List"
      Height          =   735
      Left            =   240
      TabIndex        =   21
      Top             =   2160
      Visible         =   0   'False
      Width           =   2535
      Begin VB.TextBox sULServer 
         Height          =   285
         Left            =   720
         TabIndex        =   9
         Top             =   360
         Width           =   1455
      End
      Begin VB.TextBox sULDomain 
         Height          =   285
         Left            =   2880
         TabIndex        =   11
         Top             =   360
         Width           =   1455
      End
      Begin VB.Label Label10 
         Caption         =   "Se&rver:"
         Height          =   255
         Left            =   120
         TabIndex        =   8
         Top             =   360
         Width           =   975
      End
      Begin VB.Label Label8 
         Caption         =   "&Domain:"
         Height          =   255
         Left            =   2280
         TabIndex        =   10
         Top             =   360
         Width           =   975
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Database Connection"
      Height          =   1095
      Left            =   240
      TabIndex        =   20
      Top             =   960
      Width           =   5775
      Begin VB.TextBox sDBUserPW 
         Height          =   285
         Left            =   4200
         TabIndex        =   7
         Top             =   720
         Width           =   1455
      End
      Begin VB.TextBox sDBUserName 
         Height          =   285
         Left            =   4200
         TabIndex        =   3
         Top             =   360
         Width           =   1455
      End
      Begin VB.TextBox sDBName 
         Height          =   285
         Left            =   1200
         TabIndex        =   5
         Top             =   720
         Width           =   1455
      End
      Begin VB.TextBox sDBServer 
         Height          =   285
         Left            =   1200
         TabIndex        =   1
         Top             =   360
         Width           =   1455
      End
      Begin VB.Label Label7 
         Caption         =   "&Password:"
         Height          =   255
         Left            =   3120
         TabIndex        =   6
         Top             =   720
         Width           =   975
      End
      Begin VB.Label Label6 
         Caption         =   "&UserID:"
         Height          =   255
         Left            =   3120
         TabIndex        =   2
         Top             =   360
         Width           =   975
      End
      Begin VB.Label Label5 
         Caption         =   "&Database:"
         Height          =   255
         Left            =   120
         TabIndex        =   4
         Top             =   720
         Width           =   975
      End
      Begin VB.Label Label4 
         Caption         =   "S&erver:"
         Height          =   255
         Left            =   120
         TabIndex        =   0
         Top             =   360
         Width           =   975
      End
   End
   Begin VB.CheckBox chkSaveChanges 
      Caption         =   "&Save Changes"
      Height          =   255
      Left            =   3960
      TabIndex        =   12
      Top             =   2160
      Width           =   2055
   End
   Begin VB.PictureBox Picture1 
      BackColor       =   &H80000005&
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   5640
      Picture         =   "frmSettings.frx":08CA
      ScaleHeight     =   615
      ScaleWidth      =   615
      TabIndex        =   19
      Top             =   120
      Width           =   615
   End
   Begin VB.CommandButton cmdBack 
      Caption         =   "< &Back"
      Height          =   375
      Left            =   3120
      TabIndex        =   13
      Top             =   4080
      Width           =   975
   End
   Begin VB.Frame fraLine 
      Height          =   30
      Left            =   -240
      TabIndex        =   18
      Top             =   3960
      Width           =   6615
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   5280
      TabIndex        =   15
      Top             =   4080
      Width           =   975
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "&Next >"
      Default         =   -1  'True
      Height          =   375
      Left            =   4200
      TabIndex        =   14
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label Label2 
      BackStyle       =   0  'Transparent
      Caption         =   "If you want to save changes, be sure to check 'Save Changes' below."
      Height          =   255
      Left            =   480
      TabIndex        =   17
      Top             =   480
      Width           =   5775
   End
   Begin VB.Label Label1 
      BackStyle       =   0  'Transparent
      Caption         =   "You can change advanced settings below."
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
      TabIndex        =   16
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
Attribute VB_Name = "frmSettings"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public blnCancel As Boolean
Public cnn As Connection
Public blnBack As Boolean

Private Sub cmdBack_Click()
blnBack = True
Me.Visible = False
End Sub

Private Sub cmdCancel_Click()
blnCancel = True
Me.Visible = False
End Sub

Private Sub cmdNext_Click()
If Me.chkResetSecTable.Enabled And Me.chkResetSecTable Then
  If MsgBox("Are you sure you want to continue and clear the existing security table?  All settings saved will be erased.  You cannot undo this action!  Click OK to continue or cancel to stop.", vbOKCancel + vbQuestion + vbDefaultButton2, "Warning!") = vbCancel Then Exit Sub
End If
If Me.chkSaveChanges Then
  PrivPutString "DBServer", Me.sDBServer
  PrivPutString "DBName", Me.sDBName
  PrivPutString "DBUserName", Me.sDBUserName
  PrivPutString "DBUserPW", Me.sDBUserPW
  PrivPutString "ULDomain", Me.sULDomain
  PrivPutString "ULServer", Me.sULServer
End If
m_sDBServer = Me.sDBServer
m_sDBName = Me.sDBName
m_sDBUserName = Me.sDBUserName
m_sDBUserPW = Me.sDBUserPW
m_sULDomain = Me.sULDomain
m_sULServer = Me.sULServer
Me.Visible = False
End Sub

Private Sub Form_Load()
blnCancel = False
blnBack = False
Me.sDBServer = m_sDBServer
Me.sDBName = m_sDBName
Me.sDBUserName = m_sDBUserName
Me.sDBUserPW = m_sDBUserPW
Me.sULDomain = m_sULDomain
Me.sULServer = m_sULServer
End Sub
