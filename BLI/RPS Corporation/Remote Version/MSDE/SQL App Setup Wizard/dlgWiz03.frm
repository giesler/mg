VERSION 5.00
Begin VB.Form dlgWiz03 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 3)"
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
   Begin VB.Frame frmeButtons 
      BorderStyle     =   0  'None
      Height          =   735
      Left            =   120
      TabIndex        =   8
      Top             =   2400
      Width           =   5895
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3480
         TabIndex        =   10
         Top             =   240
         Width           =   1095
      End
      Begin VB.CommandButton btnCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4680
         TabIndex        =   11
         Top             =   240
         Width           =   1095
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2280
         TabIndex        =   9
         Top             =   240
         Width           =   1095
      End
   End
   Begin VB.Frame frmOptions 
      Caption         =   "Database Server &Type"
      Height          =   2055
      Left            =   120
      TabIndex        =   0
      Top             =   360
      Width           =   5775
      Begin VB.OptionButton btnSqlSBS 
         Caption         =   "SQL Server 7.0 Small Business Se&rver Edition"
         Height          =   255
         Left            =   120
         TabIndex        =   5
         Top             =   1200
         Width           =   4095
      End
      Begin VB.OptionButton btnSqlDev 
         Caption         =   "SQL Server 7.0 De&veloper Edition"
         Height          =   255
         Left            =   120
         TabIndex        =   4
         Top             =   960
         Width           =   2775
      End
      Begin VB.OptionButton btnMSDEOffice 
         Caption         =   "MSDE 1.0 (&Office 2000 Release)"
         Height          =   255
         Left            =   120
         TabIndex        =   7
         Top             =   1680
         Width           =   2895
      End
      Begin VB.OptionButton btnSqlEnt 
         Caption         =   "SQL Server 7.0 &Enterprise Edition"
         Height          =   255
         Left            =   120
         TabIndex        =   2
         Top             =   480
         Width           =   3255
      End
      Begin VB.OptionButton btnSqlStd 
         Caption         =   "SQL Server 7.0 &Standard Edition"
         Height          =   255
         Left            =   120
         TabIndex        =   1
         Top             =   240
         Width           =   4095
      End
      Begin VB.OptionButton btnSqlDesk 
         Caption         =   "SQL Server 7.0 &Desktop Edition"
         Height          =   255
         Left            =   120
         TabIndex        =   3
         Top             =   720
         Width           =   2775
      End
      Begin VB.OptionButton btnMSDE 
         Caption         =   "&MSDE 1.0"
         Height          =   255
         Left            =   120
         TabIndex        =   6
         Top             =   1440
         Width           =   2775
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Choose the type of database server you wish to install:"
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
      TabIndex        =   12
      Top             =   120
      Width           =   4815
   End
End
Attribute VB_Name = "dlgWiz03"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 3: choose database server type to install

Option Explicit

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    WriteLogMsg goLogFileHandle, Me.Name & ": User Selected Database Server Type: " & goDbServers(glngSqlType).Description
    Unload Me
    dlgWiz04.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz02.Show ' vbModal
End Sub

Private Sub btnSqlDev_Click()
    glngSqlType = SQL_DEV_7_X86
    UpdateControls
End Sub

Private Sub btnSqlSBS_Click()
    glngSqlType = SQL_SBS_7_X86
    UpdateControls
End Sub

Private Sub Form_Load()
    glngSqlType = SQL_NOT_INITIALIZED
End Sub
Private Sub Form_Activate()
    UpdateControls
    btnMSDE.Value = False
    btnMSDE_Click
    btnNext_Click
End Sub

Private Sub btnMSDEOffice_Click()
    glngSqlType = MSDE_OFFICE_1_X86
    UpdateControls
End Sub

Private Sub btnMSDE_Click()
    glngSqlType = MSDE_1_X86
    UpdateControls
End Sub

Private Sub btnSqlDesk_Click()
    glngSqlType = SQL_DESK_7_X86
    UpdateControls
End Sub

Private Sub btnSqlEnt_Click()
    glngSqlType = SQL_ENT_7_X86
    UpdateControls
End Sub

Private Sub btnSqlStd_Click()
    glngSqlType = SQL_STD_7_X86
    UpdateControls
End Sub

Private Sub UpdateControls()
    Dim oOptBtn As OptionButton
    Dim oOptBtnFocus As OptionButton
    Dim intLowTabIndex As Integer
    
    ' Check to see if one of the options was selected
    If glngSqlType = SQL_NOT_INITIALIZED Then
        btnNext.Enabled = False
        
        btnSqlStd.Enabled = False
        btnSqlEnt.Enabled = False
        btnSqlDesk.Enabled = False
        
        ' MSDE Installations are always supported
        btnMSDE.Enabled = True
        btnMSDEOffice.Enabled = True ' Not checking to see if O2K or OSE is installed, need to fix
        
        ' Enable / Disable installation options depending upon IE 4.01 SP1 dependency
        If glngIeInstalled = NOT_INSTALLED Then
            btnSqlStd.Enabled = False
            btnSqlEnt.Enabled = False
            btnSqlDesk.Enabled = False
            btnSqlDev.Enabled = False
            btnSqlSBS.Enabled = False
        Else
            ' Enable / Disable installation options depending upon OS type
            Select Case glngOsType
                Case WIN_95_98, NT_40_WKS, WIN_2K_PRO
                    btnSqlStd.Enabled = False
                    btnSqlEnt.Enabled = False
                    btnSqlDesk.Enabled = True
                    btnSqlDev.Enabled = False
                    btnSqlSBS.Enabled = False
                Case NT_40_SRV, NT_40_SRV_ENT, NT_40_SRV_TRM, WIN_2K_SRV, WIN_2K_SRV_ADV, WIN_2K_SRV_DC
                    btnSqlStd.Enabled = True
                    btnSqlEnt.Enabled = False
                    btnSqlDesk.Enabled = False
                    btnSqlDev.Enabled = True
                    btnSqlSBS.Enabled = True
                Case NT_40_SRV_ENT, WIN_2K_SRV_ADV, WIN_2K_SRV_DC
                    btnSqlStd.Enabled = True
                    btnSqlEnt.Enabled = True
                    btnSqlDesk.Enabled = False
                    btnSqlDev.Enabled = True
                    btnSqlSBS.Enabled = True
            End Select
        End If
        
        ' Uncheck everything
        btnSqlStd.Value = False
        btnSqlEnt.Value = False
        btnSqlDesk.Value = False
        btnSqlDev.Value = False
        btnSqlSBS.Value = False
        btnMSDE.Value = False
        btnMSDEOffice.Value = False
                
        If btnSqlStd.Enabled Then
            btnSqlStd.SetFocus
        ElseIf btnSqlEnt.Enabled Then
            btnSqlEnt.SetFocus
        ElseIf btnSqlDesk.Enabled Then
            btnSqlDesk.SetFocus
        ElseIf btnSqlDev.Enabled Then
            btnSqlDev.SetFocus
        ElseIf btnSqlSBS.Enabled Then
            btnSqlSBS.SetFocus
        ElseIf btnMSDE.Enabled Then
            btnMSDE.SetFocus
        ElseIf btnMSDEOffice.Enabled Then
            btnMSDEOffice.SetFocus
        End If
    Else
        btnNext.Enabled = True
        ' btnNext.SetFocus
    End If
End Sub
