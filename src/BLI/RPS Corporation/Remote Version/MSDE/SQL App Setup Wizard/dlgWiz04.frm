VERSION 5.00
Begin VB.Form dlgWiz04 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 4)"
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
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   2175
      Left            =   120
      TabIndex        =   5
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
         TabIndex        =   6
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnLocate 
      Caption         =   "&Locate"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   4
      Top             =   2640
      Width           =   2055
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
      Caption         =   "Locate Database Server Installation Image:"
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
      Width           =   3855
   End
End
Attribute VB_Name = "dlgWiz04"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 4: Locate Database Server Setup
Option Explicit

Dim mstrDetails As String
Dim mstrSetupExeFileSpec As String
Dim mblnLocated As Boolean

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnLocate_Click()
    ' Initialize dlgGetPath Variables
    gstrCurDrive = FindCd(goFileSystem)
    gstrCurPath = CurDir(gstrCurDrive)
    gblnPathChanged = False
       
    'dlgGetPath.Show vbModal
    gstrCurPath = App.Path & "\MSDE1"
    
'    If gblnPathChanged Then
        If Right(gstrCurPath, 1) = "\" Then
            mstrSetupExeFileSpec = Left(gstrCurPath, Len(gstrCurPath) - 1) & goDbServers(glngSqlType).SetupExeFileSpec
        Else
            mstrSetupExeFileSpec = gstrCurPath & goDbServers(glngSqlType).SetupExeFileSpec
        End If
        
        If goFileSystem.FileExists(mstrSetupExeFileSpec) Then
            gstrSetupExeFileSpec = mstrSetupExeFileSpec
            gstrInstallImagePath = gstrCurPath
            mstrDetails = _
                "Path to database server installation image located at '" & _
                gstrCurPath & "'. Installation executable located at '" & gstrSetupExeFileSpec & "'; "
            WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
            mstrDetails = mstrDetails & _
                "Click 'Next' to proceed."
            mblnLocated = True
        Else
            mstrDetails = _
                "The path " & gstrCurPath & " does not contain a valid database server installation image. " & _
                "Click 'Locate' to try again, or press 'Cancel' to quit."
        End If
'    End If
       
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz05.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    gstrSetupExeFileSpec = STR_NOT_INITIALIZED
    Unload Me
    dlgWiz03.Show ' vbModal
End Sub
Private Sub Form_Activate()
    UpdateControls
    Me.btnLocate.Visible = False
    btnLocate_Click
    btnNext_Click
End Sub
Private Sub Form_Load()
    mblnLocated = False
    
    Select Case glngSqlType
        Case SQL_STD_7_X86, SQL_ENT_7_X86, SQL_DESK_7_X86, MSDE_OFFICE_1_X86, SQL_DEV_7_X86, SQL_SBS_7_X86
            mstrDetails = "Click 'Locate' to specify the path to the root directory of your database server installation image"
        Case MSDE_1_X86
            mstrDetails = "Click 'Locate' to specify the path to your database server installation executable (" & _
                goDbServers(glngSqlType).SetupExeFileSpec & ")."
    End Select
End Sub
Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    If mblnLocated Then
        btnNext.Enabled = True
        btnPrev.Enabled = True
        btnLocate.Enabled = False
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnNext.Enabled = False
        btnPrev.Enabled = True
        btnLocate.Enabled = True
        btnCancel.Enabled = True
        btnLocate.SetFocus
    End If
End Sub

