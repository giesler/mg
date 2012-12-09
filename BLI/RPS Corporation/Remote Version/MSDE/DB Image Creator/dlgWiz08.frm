VERSION 5.00
Begin VB.Form dlgWiz08 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "DbInstallImage Wizard (Step 8)"
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
   Begin VB.CommandButton btnSelect 
      Caption         =   "&Select Tables"
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
      Caption         =   "Select Tables to Load:"
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
Attribute VB_Name = "dlgWiz08"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    Dim strTableName As Variant
    Dim strMessage As String
    
    strMessage = Me.Name & ": User selected the following " & CStr(glngLoadTableCount) & " tables to load: "
    
    For Each strTableName In gstrLoadTables
        strMessage = strMessage & strTableName & "; "
    Next
    
    WriteLogMsg goLogFileHandle, strMessage
    
    Unload Me
    dlgWiz09.Show vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz07.Show vbModal
End Sub

Private Sub btnSelect_Click()
    dlgTableSelect.Show vbModal
        
    mstrDetails = _
        CStr(glngLoadTableCount) & " tables have been selected. " & _
        "Click 'Next' to proceed."
    
    UpdateControls
End Sub

Private Sub Form_Load()
    mstrDetails = _
        "Click 'Select Tables' to generate data files to be included " & _
        "with your application database installation image.  These datafiles are used to load " & _
        "data into individual tables during a script-based application database installation. " & _
        "Click 'Next' to skip this step and proceed."
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    btnSelect.Enabled = True
    btnPrev.Enabled = True
    btnNext.Enabled = True
    btnCancel.Enabled = True
End Sub
