VERSION 5.00
Begin VB.Form dlgWiz07 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "DbInstallImage Wizard (Step 7)"
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
   Begin VB.CommandButton btnProperties 
      Caption         =   "&Script Properties"
      Height          =   375
      Left            =   120
      TabIndex        =   2
      Top             =   2640
      Width           =   2055
   End
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   2175
      Left            =   120
      TabIndex        =   0
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
         TabIndex        =   1
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnPrev 
      Caption         =   "<< &Previous"
      Height          =   375
      Left            =   2400
      TabIndex        =   3
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   5
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnNext 
      Caption         =   "&Next >>"
      Height          =   375
      Left            =   3600
      TabIndex        =   4
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Adjust Script Properties:"
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
      Top             =   120
      Width           =   3015
   End
End
Attribute VB_Name = "dlgWiz07"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mstrDetails

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    Dim strMessage As String
    
    strMessage = Me.Name & ": The following script options were enabled: " & _
        IIf(gblnScriptUsersAndRoles, "ScriptUsersAndRoles; ", "") & _
        IIf(gblnScriptLogins, "ScriptLogins; ", "") & _
        IIf(gblnScriptPermissions, "ScriptObjPermissions; ", "") & _
        IIf(gblnScriptIndexes, "ScriptIndexes; ", "") & _
        IIf(gblnScriptTriggers, "ScriptTriggers; ", "") & _
        IIf(gblnScriptDRI, "ScriptDRI; ", "")
    
    WriteLogMsg goLogFileHandle, strMessage

    Unload Me
    dlgWiz08.Show vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz06.Show vbModal
End Sub

Private Sub btnProperties_Click()
    dlgScriptProps.Show vbModal
    mstrDetails = _
        "Click 'Next' to proceed."
    UpdateControls
End Sub

Private Sub Form_Load()
    mstrDetails = _
        "Click 'Script Properties' to adjust the default settings for the format of data " & _
        "definition language scripts which will be generated as part of the " & _
        "application database installation image, or click 'Next' to proceed with " & _
        "the default settings."
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    btnProperties.Enabled = True
    btnPrev.Enabled = True
    btnNext.Enabled = True
    btnCancel.Enabled = True
    btnNext.SetFocus
End Sub
