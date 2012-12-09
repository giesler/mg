VERSION 5.00
Begin VB.Form frmSplash 
   BorderStyle     =   3  'Fixed Dialog
   ClientHeight    =   2835
   ClientLeft      =   45
   ClientTop       =   45
   ClientWidth     =   6780
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2835
   ScaleWidth      =   6780
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Visible         =   0   'False
   Begin VB.Frame fraMainFrame 
      Height          =   2790
      Left            =   45
      TabIndex        =   0
      Top             =   -15
      Width           =   6660
      Begin VB.PictureBox picTC 
         BorderStyle     =   0  'None
         Height          =   1095
         Left            =   1680
         Picture         =   "frmSplash.frx":0000
         ScaleHeight     =   1095
         ScaleWidth      =   3375
         TabIndex        =   4
         Top             =   240
         Width           =   3375
      End
      Begin VB.PictureBox picFC 
         BorderStyle     =   0  'None
         Height          =   975
         Left            =   1200
         Picture         =   "frmSplash.frx":14AF
         ScaleHeight     =   975
         ScaleWidth      =   4455
         TabIndex        =   5
         Top             =   240
         Width           =   4455
      End
      Begin VB.Label lblProductName 
         AutoSize        =   -1  'True
         Caption         =   "Dealer Parts Pricing"
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   29.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   675
         Left            =   600
         TabIndex        =   3
         Tag             =   "Product"
         Top             =   1200
         Width           =   5595
      End
      Begin VB.Label lblCompanyProduct 
         AutoSize        =   -1  'True
         Caption         =   "RPS Corporation"
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   15.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   360
         Left            =   120
         TabIndex        =   2
         Tag             =   "CompanyProduct"
         Top             =   2280
         Width           =   3255
      End
      Begin VB.Label lblVersion 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         Caption         =   "Version"
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   15.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   360
         Left            =   4110
         TabIndex        =   1
         Tag             =   "Version"
         Top             =   2280
         Width           =   2220
      End
   End
End
Attribute VB_Name = "frmSplash"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
If cComp = eFactoryCat Then
  Me.picFC.Visible = True
  Me.picTC.Visible = False
Else
  Me.picTC.Visible = True
  Me.picFC.Visible = False
End If
  lblVersion.Caption = "Version " & App.Major & "." & Format(App.Minor, "00")
End Sub

