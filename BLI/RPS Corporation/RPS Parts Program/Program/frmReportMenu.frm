VERSION 5.00
Begin VB.Form frmReportMenu 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Report Menu"
   ClientHeight    =   2025
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4935
   Icon            =   "frmReportMenu.frx":0000
   LinkTopic       =   "Form1"
   MDIChild        =   -1  'True
   ScaleHeight     =   2025
   ScaleWidth      =   4935
   Begin VB.Frame fraContent 
      BorderStyle     =   0  'None
      Height          =   1695
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4575
      Begin VB.CommandButton cmdDealerParts 
         Caption         =   "&Dealer Parts"
         Height          =   495
         Left            =   0
         TabIndex        =   2
         Top             =   240
         Width           =   855
      End
      Begin VB.CommandButton cmdPriceListing 
         Caption         =   "&Price Listing"
         Height          =   495
         Left            =   0
         TabIndex        =   1
         Top             =   960
         Width           =   855
      End
      Begin VB.Label Label1 
         Caption         =   "Prints the Dealer Parts Price Listing with both dealer price and suggested list price."
         Height          =   495
         Left            =   1080
         TabIndex        =   4
         Top             =   240
         Width           =   3495
      End
      Begin VB.Label Label2 
         Caption         =   "Prints the Price Listing with only the suggested list price.  Also includes your company name at the top of the report."
         Height          =   615
         Left            =   1080
         TabIndex        =   3
         Top             =   960
         Width           =   3495
      End
   End
End
Attribute VB_Name = "frmReportMenu"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdDealerParts_Click()

dlDealerParts.Title = "Factory Cat Dealer Parts Listing"
dlDealerParts.Show

End Sub

Private Sub cmdPriceListing_Click()

Dim sTmp As String
sTmp = GetSetting(App.Title, "Options", "CompanyName", "RPS Corporation")
dlDealerPrice.Title = sTmp & " Price Listing"
dlDealerPrice.Show

End Sub

Private Sub Form_Resize()
If Me.WindowState <> vbMinimized Then
  Me.fraContent.Left = (Me.Width / 2) - (Me.fraContent.Width / 2)
  Me.fraContent.Top = (Me.Height / 2) - (Me.fraContent.Height / 2)
End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
Set dp = Nothing
End Sub
