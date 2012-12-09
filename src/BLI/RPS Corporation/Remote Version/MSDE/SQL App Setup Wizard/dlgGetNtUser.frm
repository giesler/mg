VERSION 5.00
Begin VB.Form dlgGetNTUser 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Grant Access to NT Account"
   ClientHeight    =   1965
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   5310
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1965
   ScaleWidth      =   5310
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.ComboBox cmbRoleNames 
      Height          =   315
      Left            =   120
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   1560
      Width           =   3495
   End
   Begin VB.TextBox txbUserName 
      Height          =   285
      Left            =   120
      TabIndex        =   1
      Top             =   960
      Width           =   3495
   End
   Begin VB.TextBox txbDomain 
      Height          =   285
      Left            =   120
      TabIndex        =   0
      Top             =   360
      Width           =   3495
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   3720
      TabIndex        =   4
      Top             =   600
      Width           =   1455
   End
   Begin VB.CommandButton btnOk 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3720
      TabIndex        =   3
      Top             =   120
      Width           =   1455
   End
   Begin VB.Label Label3 
      Caption         =   "Database Security Role:"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   1320
      Width           =   3495
   End
   Begin VB.Label Label2 
      Caption         =   "Windows NT Account Name (User or Group):"
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   720
      Width           =   4335
   End
   Begin VB.Label Label1 
      Caption         =   "Windows NT Domain or Computer Name:"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   120
      Width           =   3135
   End
End
Attribute VB_Name = "dlgGetNTUser"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit
Option Base 1

Private Sub btnCancel_Click()
    Unload Me
End Sub

Private Sub btnOk_Click()
    If Len(txbDomain) = 0 Then
        MsgBox "Entry required for domain.", vbOKOnly + vbExclamation, "Error"
        txbDomain.SetFocus
        Exit Sub
    End If
    
    If Len(txbUserName) = 0 Then
        MsgBox "Entry required for user name.", vbOKOnly + vbExclamation, "Error"
        txbUserName.SetFocus
        Exit Sub
    End If
    
    gstrDomain = txbDomain.Text
    gstrAccount = txbUserName.Text
    gstrDbRole = cmbRoleNames.Text
    
    Unload Me
End Sub

Private Sub Form_Activate()
    Dim i As Integer
    
    cmbRoleNames.Clear
    
    For i = 1 To UBound(gstrDbRoles)
        cmbRoleNames.AddItem gstrDbRoles(i)
    Next i
    cmbRoleNames.Text = cmbRoleNames.List(0)
    
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDomain.Text = ""
    txbUserName.Text = ""
    txbDomain.SetFocus
End Sub


Private Sub txbDomain_GotFocus()
    txbDomain.SelStart = 0
    txbDomain.SelLength = Len(txbDomain.Text)
End Sub

Private Sub txbUserName_GotFocus()
    txbUserName.SelStart = 0
    txbUserName.SelLength = Len(txbUserName.Text)
End Sub

