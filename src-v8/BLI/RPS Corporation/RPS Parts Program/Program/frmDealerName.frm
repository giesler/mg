VERSION 5.00
Begin VB.Form frmDealerName 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Dealer Parts Program"
   ClientHeight    =   1770
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5070
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1770
   ScaleWidth      =   5070
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   120
      Picture         =   "frmDealerName.frx":0000
      ScaleHeight     =   615
      ScaleWidth      =   555
      TabIndex        =   3
      Top             =   120
      Width           =   555
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3480
      TabIndex        =   2
      Top             =   1320
      Width           =   1215
   End
   Begin VB.TextBox txtCompanyName 
      Height          =   285
      Left            =   240
      TabIndex        =   1
      Top             =   840
      Width           =   4575
   End
   Begin VB.Label Label1 
      Caption         =   "Please enter your company name below, then click OK.  Your company name will be used on any reports that are printed."
      Height          =   735
      Left            =   840
      TabIndex        =   0
      Top             =   120
      Width           =   3855
   End
End
Attribute VB_Name = "frmDealerName"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdOK_Click()

If Me.txtCompanyName = "" Then
  MsgBox "You must enter your company name to continue.", vbExclamation
  Me.txtCompanyName.SetFocus
  Exit Sub
End If

SaveSetting App.Title, "Options", "CompanyName", Me.txtCompanyName
Me.Visible = False

End Sub
