VERSION 5.00
Begin VB.Form dlgWiz02 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 2)"
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
   Begin VB.CommandButton btnSearch 
      Caption         =   "&Search"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   6
      Top             =   2640
      Width           =   2055
   End
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   2175
      Left            =   120
      TabIndex        =   4
      Top             =   360
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1815
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   5
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnPrev 
      Caption         =   "<< &Previous"
      Height          =   375
      Left            =   2400
      TabIndex        =   2
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   1
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnNext 
      Caption         =   "&Next >>"
      Height          =   375
      Left            =   3600
      TabIndex        =   0
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Search for Installed Components:"
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
      TabIndex        =   3
      Top             =   120
      Width           =   3015
   End
End
Attribute VB_Name = "dlgWiz02"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 2: Check for installed components

Option Explicit
Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnSearch_Click()
    Dim strMessage As String
    Dim blnError As Boolean
    
    blnError = False
    
    Me.MousePointer = vbHourglass
    CheckForInstalledComponents blnError, mstrDetails
    Me.MousePointer = vbDefault
    
    If blnError Then
        glngOsType = OS_NOT_INITIALIZED
        glngIeInstalled = INSTALL_STATE_NOT_INITIALIZED
        glngSqlType = SQL_NOT_INITIALIZED
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Dim strFormName As String
    strFormName = Me.Name
    
    Unload Me
    If glngSqlType = SQL_NOT_INITIALIZED Then
        WriteLogMsg goLogFileHandle, strFormName & ": Existing database server not detected. Proceeding with database server installation."
        dlgWiz03.Show ' vbModal
    Else
        glngDbServerInst = INSTALLED
        If glngSqlCsdVer >= glngCurSqlCsdVer Then
            glngServicePackInst = INSTALLED
            glngSpType = SQL_7_SP1_NT_X86
            WriteLogMsg goLogFileHandle, strFormName & ": Existing database server detected, service pack installed.  Skipping service pack installation.  Proceeding with step 17."
            dlgWiz17.Show ' vbModal
        Else
            WriteLogMsg goLogFileHandle, strFormName & ": Existing database server detected, service pack not installed.  Service pack installation will be performed.  Proceeding with step 10."
            dlgWiz10.Show ' vbModal
        End If
    End If
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz01.Show ' vbModal
End Sub

Private Sub Form_Activate()
  UpdateControls
  Me.btnSearch.Visible = False
  btnSearch_Click
  btnNext_Click
End Sub

Private Sub Form_Load()
    glngOsType = OS_NOT_INITIALIZED
    glngIeInstalled = INSTALL_STATE_NOT_INITIALIZED
    glngSqlType = SQL_NOT_INITIALIZED
    mstrDetails = "Setup must search for previously installed components.  Click 'Search' to proceed."

End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If glngOsType = OS_NOT_INITIALIZED And glngIeInstalled = INSTALL_STATE_NOT_INITIALIZED And glngSqlType = SQL_NOT_INITIALIZED Then
        ' First time through, enable search
        btnSearch.Enabled = True
        btnNext.Enabled = False
        btnPrev.Enabled = True
    '    btnSearch.SetFocus
        Exit Sub
    End If
    
    Select Case glngOsType
        Case WIN_95_98
            ' A supported operating system with the necessary prereqs is installed, allow user to move on
            btnSearch.Enabled = False
            btnNext.Enabled = True
            btnNext.SetFocus
        Case NT_40_WKS, NT_40_SRV, NT_40_SRV_ENT, NT_40_SRV_TRM, WIN_2K_PRO, WIN_2K_SRV, WIN_2K_SRV_ADV, WIN_2K_SRV_DC
            If gblnBuiltinAdminsMember Then
                ' A supported operating system with the necessary prereqs is installed, allow user to move on
                btnSearch.Enabled = False
                btnNext.Enabled = True
                btnNext.SetFocus
            Else
                btnSearch.Enabled = False
                btnNext.Enabled = False
                btnPrev.Enabled = False
                btnCancel.SetFocus
                Exit Sub
            End If
        Case WIN_95_OLD, NT_OLD
            ' An unsupported operating System Configuration is installed, don't allow user to move on
            btnSearch.Enabled = False
            btnNext.Enabled = False
            btnPrev.Enabled = False
            btnCancel.SetFocus
            Exit Sub
        Case OS_NOT_INITIALIZED
            ' Operating system configuration not known, don't allow user to move on
            btnSearch.Enabled = False
            btnNext.Enabled = False
            btnPrev.Enabled = False
            btnCancel.SetFocus
            Exit Sub
    End Select

    Select Case glngSqlType
        Case MSDE_OFFICE_1_X86, SQL_STD_7_X86, SQL_ENT_7_X86, MSDE_1_X86, SQL_DESK_7_X86, SQL_POST_7
            ' A supported version of SQL Server is installed, allow user to move on
            btnSearch.Enabled = False
            btnNext.Enabled = True
            btnPrev.Enabled = False
            btnNext.SetFocus
        Case SQL_PRE_7, SQL_7_BETA, SQL_UNKNOWN
            ' An unsupported version of SQL Server is installed, don't allow user to move on
            btnSearch.Enabled = False
            btnNext.Enabled = False
            btnPrev.Enabled = False
            btnCancel.SetFocus
            Exit Sub
        Case SQL_NOT_INITIALIZED
            ' No SQL Server installed, allow user to move on
            btnSearch.Enabled = False
            btnNext.Enabled = True
            btnPrev.Enabled = False
            btnNext.SetFocus
    End Select
End Sub

