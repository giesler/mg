VERSION 5.00
Begin VB.Form dlgWiz22 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 22)"
   ClientHeight    =   3195
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6030
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3195
   ScaleWidth      =   6030
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame Frame2 
      Caption         =   "Details"
      Height          =   1695
      Left            =   120
      TabIndex        =   6
      Top             =   420
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1275
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   7
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   975
      Left            =   0
      TabIndex        =   5
      Top             =   2160
      Width           =   6015
      Begin VB.CheckBox chkUseNT 
         Caption         =   "Require trusted connections for new database server logins"
         Height          =   255
         Left            =   120
         TabIndex        =   4
         Top             =   120
         Value           =   1  'Checked
         Width           =   5775
      End
      Begin VB.CommandButton btnGrant 
         Caption         =   "&Grant Access to User"
         Height          =   375
         Left            =   120
         TabIndex        =   0
         Top             =   480
         Width           =   2055
      End
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3600
         TabIndex        =   2
         Top             =   480
         Width           =   1095
      End
      Begin VB.CommandButton btnCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4800
         TabIndex        =   3
         Top             =   480
         Width           =   1095
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2400
         TabIndex        =   1
         Top             =   480
         Width           =   1095
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Grant Access to Database:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   120
      Width           =   2415
   End
End
Attribute VB_Name = "dlgWiz22"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 22: Grant Access to Database

Option Explicit
Option Base 1

Dim mstrDetails As String
Dim mblnUsersAdded As Boolean
Dim mintNTAccountsAdded As Integer
Dim mintSqlAccountsAdded As Integer
Dim mintDbUsersAdded As Integer
Dim mblnRoleError As Boolean

Private Sub btnGrant_Click()
    gstrDomain = ""
    gstrAccount = ""
    gstrPassword = ""
    gstrDbRole = ""
    
'    If chkUseNT.Value = 1 Then
'        dlgGetNTUser.Show vbModal
'    Else
'        dlgGetSqlUser.Show vbModal
'    End If
    
'    If gstrAccount = "" Then
        ' Assume user cancelled
'        Exit Sub
'    End If
    
'    If chkUseNT.Value = 1 Then
'        GrantAccessToAccount gstrDomain & "\" & gstrAccount, gstrDbRole, True
'    Else
'        GrantAccessToAccount gstrAccount, gstrDbRole, False, gstrPassword
'    End If
    GrantAccessToAccount "fcuser", "fcgenuser", False, "fcuser"
    
    UpdateControls
End Sub

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    Unload Me
    
    dlgWiz23.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz17.Show ' vbModal
End Sub

Private Sub Form_Load()
    mblnUsersAdded = False
    mintNTAccountsAdded = 0
    mintSqlAccountsAdded = 0
    mintDbUsersAdded = 0
    mstrDetails = ""
    
    ' Load Up Array of Database Role Names
    mblnRoleError = False
    ReDim gstrDbRoles(1)
    GetDbRoleNames goDbServer, gstrAppDbName, gstrDbRoles(), mblnRoleError, mstrDetails
    
    If Not mblnRoleError Then
        mstrDetails = mstrDetails & _
            "Click 'Grant Access To User' to grant a user to access the application database; " & _
            "Click 'Next' to skip this step and procede."
    Else
        mstrDetails = mstrDetails & _
            "Unable to detect database security roles from application database; " & _
            "Setup cannot continue.  Click 'Cancel' to quit."
    End If
        
    'If glngOsType = WIN_95_98 Then
        chkUseNT.Value = 0
    'Else
    '    chkUseNT.Value = 1
    'End If
End Sub

Private Sub Form_Activate()
    UpdateControls
    Me.chkUseNT.Enabled = False
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    Select Case glngOsType
        Case WIN_95_98
            ' NT Authentication not supported on Win95 / Win 98
            chkUseNT.Enabled = False
        Case Else
            chkUseNT.Enabled = True
    End Select
    
    If mblnRoleError Then
        btnGrant.Enabled = False
        btnPrev.Enabled = False
        btnNext.Enabled = False
        btnCancel.Enabled = True
    End If
End Sub

Private Sub GrantAccessToAccount(strAccount As String, strDbRole As String, blnNTAccount As Boolean, Optional strPassword As String)
    Dim blnError As Boolean
    Dim blnAdded As Boolean
    
    '
    ' Step 1: Create Login
    '
    
    blnAdded = False
    
    AddLogin goDbServer, strAccount, blnNTAccount, blnAdded, blnError, mstrDetails
    
    If blnError Then
        MsgBox "Unable to grant database server access to the Account '" & strAccount & "\" & gstrAccount & "'", vbExclamation + vbOKOnly, "Error Adding Account"
        GoTo RefreshDisplay
    End If
    
    If blnAdded Then
        If blnNTAccount Then
            mintNTAccountsAdded = mintNTAccountsAdded + 1
        Else
            mintSqlAccountsAdded = mintSqlAccountsAdded + 1
        End If
    Else
        MsgBox "The  Account '" & strAccount & "' has already been granted access to the database server.", vbInformation + vbOKOnly, "Information"
    End If
    
    '
    ' Step 2: Create Database User
    '
    
    blnAdded = False
    
    AddDbUser goDbServer, gstrAppDbName, strAccount, blnAdded, blnError, mstrDetails
    
    If blnError Then
        MsgBox "Unable to grant database access to the account '" & strAccount & "'", vbExclamation + vbOKOnly, "Error Adding Database User"
        GoTo RefreshDisplay
    End If
    
    If blnAdded Then
        mintDbUsersAdded = mintDbUsersAdded + 1
    Else
        MsgBox "The account '" & gstrDomain & "\" & gstrAccount & "' has already been granted access to the database '" & gstrAppDbName & "'.", vbInformation + vbOKOnly, "Information"
    End If
    
    '
    ' Step 3: Add Database User to Role
    '
    
    If UCase(strDbRole) <> "PUBLIC" Then
        blnAdded = False
        AddUserToRole goDbServer, gstrAppDbName, strDbRole, strAccount, blnAdded, blnError, mstrDetails
        
        If blnError Then
            MsgBox "Unable to add the account '" & strAccount & _
                "' to the role '" & gstrDbRole & "' in the database '" & gstrAppDbName & "'.", vbExclamation + vbOKOnly, "Error Adding Role Member"
            GoTo RefreshDisplay
        End If
        
        If Not blnAdded Then
            MsgBox "The account '" & strAccount & "' is already a member of the role '" & gstrDbRole & "' in the database '" & gstrAppDbName & "'.", vbInformation + vbOKOnly, "Information"
        End If
    End If
    
RefreshDisplay:
    mblnUsersAdded = True
    mstrDetails = _
        CStr(mintNTAccountsAdded) & " NT logins and " & CStr(mintSqlAccountsAdded) & " SQL logins have been granted access to the database server; " & _
        CStr(mintDbUsersAdded) & " database users have been added to the application database; " & _
        "Click 'Grant Access to User' to grant access to addtional users, or click 'Next' to proceed."
End Sub
