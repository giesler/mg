VERSION 5.00
Begin VB.Form dlgWiz11 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 11)"
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
      Height          =   1935
      Left            =   120
      TabIndex        =   4
      Top             =   420
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1515
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   5
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnLocate 
      Caption         =   "&Locate"
      Height          =   375
      Left            =   120
      TabIndex        =   3
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
      Default         =   -1  'True
      Height          =   375
      Left            =   3600
      TabIndex        =   0
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Locate Service Pack Installation Executable:"
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
      TabIndex        =   6
      Top             =   60
      Width           =   5535
   End
End
Attribute VB_Name = "dlgWiz11"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Wizard Step 11: Locate Database SQL Server 7.0 SP 1 Installation Executable:
Option Explicit

Dim mblnLocated As Boolean
Dim mstrDetails As String
Dim mstrServicePackFileSpec As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnLocate_Click()
    ' Initialize dlgGetPath Variables
    gstrCurDrive = FindCd(goFileSystem)
    gstrCurPath = CurDir(gstrCurDrive)
    gblnPathChanged = False
       
'    dlgGetPath.Show vbModal
    gstrCurPath = App.Path & "\sql7sp2"
 '   If gblnPathChanged Then
        If Right(gstrCurPath, 1) = "\" Then
            mstrServicePackFileSpec = Left(gstrCurPath, Len(gstrCurPath) - 1) & goDbServers(glngSpType).SetupExeFileSpec
        Else
            mstrServicePackFileSpec = gstrCurPath & goDbServers(glngSpType).SetupExeFileSpec
        End If
        
        If goFileSystem.FileExists(mstrServicePackFileSpec) Then
            mblnLocated = True
            gstrServicePackFileSpec = mstrServicePackFileSpec
            gstrInstallImagePath = gstrCurPath
            mstrDetails = _
                "Service pack installation executable located at '" & gstrServicePackFileSpec & "'. " & _
                "Click 'Next' to proceed."
            
            WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
        Else
            mblnLocated = False
            mstrDetails = _
                "The path " & gstrCurPath & " does not contain a valid database server service pack installation executable. " & _
                "Click 'Locate' to try again, or press 'Cancel' to quit."
        End If
  '  End If
       
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Dim strFormName As String
    strFormName = Me.Name
    
    Unload Me
    If gblnSqlPreInstalled Then
        WriteLogMsg goLogFileHandle, strFormName & ": Service pack installation pre-requisites will be checked because database server was pre-installed. Proceeding with step 12."
        dlgWiz12.Show ' vbModal
        
    Else
        WriteLogMsg goLogFileHandle, strFormName & ": Skipping service pack installation pre-requisites because database server was not pre-installed.  Proceeding with step 16."
        dlgWiz16.Show ' vbModal
    End If
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz10.Show ' vbModal
End Sub

Private Sub Form_Activate()
    If mblnLocated Then
        btnNext.SetFocus
    Else
        btnLocate.SetFocus
    End If
    UpdateControls
    Me.btnLocate.Visible = False
    btnLocate_Click
    btnNext_Click
End Sub
Private Sub Form_Load()
    mblnLocated = False
    mstrDetails = ""
    mstrServicePackFileSpec = NOT_INITIALIZED
    
    mstrDetails = _
        "Click 'Locate' to specify the path to the " & _
        goDbServers(glngSpType).Description & _
        " installation executable named '" & _
        goDbServers(glngSpType).SetupExeFileSpec & "'."
    
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnLocated Then
        btnLocate.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
    Else
        btnLocate.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
    End If
End Sub
