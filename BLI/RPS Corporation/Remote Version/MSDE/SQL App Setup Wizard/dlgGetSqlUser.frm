VERSION 5.00
Begin VB.Form dlgGetSqlUser 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Add SQL Account"
   ClientHeight    =   2550
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   5325
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2550
   ScaleWidth      =   5325
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.ComboBox cmbRoleNames 
      Height          =   315
      Left            =   120
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   2160
      Width           =   3495
   End
   Begin VB.TextBox txbPassword2 
      Height          =   285
      IMEMode         =   3  'DISABLE
      Left            =   120
      PasswordChar    =   "*"
      TabIndex        =   2
      Top             =   1560
      Width           =   3495
   End
   Begin VB.TextBox txbPassword 
      Height          =   285
      IMEMode         =   3  'DISABLE
      Left            =   120
      PasswordChar    =   "*"
      TabIndex        =   1
      Top             =   960
      Width           =   3495
   End
   Begin VB.TextBox txbAccount 
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
      TabIndex        =   5
      Top             =   600
      Width           =   1455
   End
   Begin VB.CommandButton btnOk 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3720
      TabIndex        =   4
      Top             =   120
      Width           =   1455
   End
   Begin VB.Label Label4 
      Caption         =   "Database Security Role:"
      Height          =   255
      Left            =   120
      TabIndex        =   9
      Top             =   1920
      Width           =   3495
   End
   Begin VB.Label Label3 
      Caption         =   "Re-Type Password:"
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   1320
      Width           =   3495
   End
   Begin VB.Label Label2 
      Caption         =   "Password:"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   720
      Width           =   3495
   End
   Begin VB.Label Label1 
      Caption         =   "Account Name:"
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   120
      Width           =   1335
   End
End
Attribute VB_Name = "dlgGetSqlUser"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit
Dim mstrDbRole As String

Private Sub btnCancel_Click()
    Unload Me
End Sub

Private Sub btnOk_Click()
    If Len(txbAccount) = 0 Then
        MsgBox "Entry required for account.", vbOKOnly + vbExclamation, "Error"
        txbAccount.SetFocus
        Exit Sub
    End If
    
    If txbPassword.Text <> txbPassword2.Text Then
        MsgBox "Passwords do not match, please re-enter.", vbExclamation + vbOKOnly, "Error"
        txbPassword.SetFocus
        Exit Sub
    End If
    
    gstrAccount = txbAccount.Text
    gstrPassword = txbPassword.Text
    gstrDbRole = cmbRoleNames.Text
    
    Unload Me
End Sub


Private Sub Form_Load()
    Dim i As Integer
    
    txbAccount.Text = ""
    txbPassword.Text = ""
    txbPassword2.Text = ""
    
    cmbRoleNames.Clear
    
    For i = 1 To UBound(gstrDbRoles)
        cmbRoleNames.AddItem gstrDbRoles(i)
    Next i
    
    cmbRoleNames.Text = cmbRoleNames.List(0)
    
End Sub

Private Sub txbAccount_GotFocus()
    txbAccount.SelStart = 0
    txbAccount.SelLength = Len(txbAccount.Text)
End Sub

Private Sub txbPassword_GotFocus()
    txbPassword.SelStart = 0
    txbPassword.SelLength = Len(txbPassword.Text)
End Sub

Private Sub txbPassword2_GotFocus()
    txbPassword2.SelStart = 0
    txbPassword2.SelLength = Len(txbPassword2.Text)
End Sub

