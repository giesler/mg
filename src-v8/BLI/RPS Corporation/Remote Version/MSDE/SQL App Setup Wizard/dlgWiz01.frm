VERSION 5.00
Begin VB.Form dlgWiz01 
   AutoRedraw      =   -1  'True
   BorderStyle     =   1  'Fixed Single
   Caption         =   "SQL App Setup Wizard (Step 1)"
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
   Visible         =   0   'False
   Begin VB.CheckBox chkAutorunInstall 
      Caption         =   "Automatically run installation"
      Height          =   255
      Left            =   360
      TabIndex        =   5
      Top             =   2640
      Value           =   1  'Checked
      Visible         =   0   'False
      Width           =   2895
   End
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   2175
      Left            =   120
      TabIndex        =   3
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
         TabIndex        =   4
         Top             =   240
         Width           =   5535
      End
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
      Caption         =   "Welcome to the SQL App Setup Wizard!"
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
      TabIndex        =   2
      Top             =   120
      Width           =   3735
   End
End
Attribute VB_Name = "dlgWiz01"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

' Step 1: Intro Screen

Option Explicit

Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit
End Sub

Private Sub btnNext_Click()
'  If Me.chkAutorunInstall = 1 Then
'    bAutoRun = True
'  Else
'    If MsgBox("You have disabled the automatic installation.  This is not recommened unless you have been told to do this.  Would you like to run the installation automatically?", vbYesNo + vbQuestion) = vbYes Then
      bAutoRun = False
'    End If
'  End If
    Unload Me
    dlgWiz02.Show 'vbModal
End Sub
Private Sub Form_Load()
    mstrDetails = _
        "This wizard will install a local database server and the RPS Corporation database on your system." & vbCrLf & _
        "The installation is automated, and will take anywhere from 5 to 25 minutes, depending on your system." & vbCrLf & vbCrLf & _
        "Click 'Next' to proceed, or 'Cancel' to quit."
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
End Sub
