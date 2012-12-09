VERSION 5.00
Begin VB.Form dlgWiz12 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 12)"
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
      TabIndex        =   5
      Top             =   480
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
         TabIndex        =   6
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   4
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnVerify 
         Caption         =   "&Verify"
         Height          =   375
         Left            =   120
         TabIndex        =   0
         Top             =   120
         Width           =   2055
      End
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3600
         TabIndex        =   2
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4800
         TabIndex        =   3
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2400
         TabIndex        =   1
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Verify Administrative Privileges:"
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
      TabIndex        =   7
      Top             =   120
      Width           =   3315
   End
End
Attribute VB_Name = "dlgWiz12"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 12: Verify Administrative Privileges
Dim mstrDetails As String
Dim mblnTestPassed As Boolean

Option Explicit

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz13.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz11.Show ' vbModal
End Sub

Private Sub btnVerify_Click()
    Dim blnError As Boolean
    
    mstrDetails = ""
    
    blnError = False
    
    Me.MousePointer = vbHourglass
    
    If Not gblnDmoConnected Then
        ConnectUsingDmo goDbServer, blnError, mstrDetails
    End If
    
    Me.MousePointer = vbDefault
    
    If Not blnError Then
        mblnTestPassed = True
        gblnDmoConnected = True
        mstrDetails = mstrDetails & "Administrative privileges verified.  Click 'Next' to proceed."
    Else
        mblnTestPassed = False
        gblnDmoConnected = False
        mstrDetails = mstrDetails & "Setup cannot continue.  Click 'Verify' to re-try, or click 'Cancel' to quit."
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    
    UpdateControls
End Sub
Public Sub Form_Load()
    mblnTestPassed = False
    gblnDmoConnected = False
    
    mstrDetails = "Setup needs to verify that your user account has administrative privileges on the local database server."
End Sub

Private Sub Form_Activate()
    UpdateControls
    Me.btnVerify.Visible = False
    btnVerify_Click
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnTestPassed Then
        btnVerify.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnVerify.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnVerify.SetFocus
    End If
End Sub

