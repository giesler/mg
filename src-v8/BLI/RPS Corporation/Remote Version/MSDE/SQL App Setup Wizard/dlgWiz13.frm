VERSION 5.00
Begin VB.Form dlgWiz13 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 13)"
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
         TabIndex        =   5
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   0
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnConfigure 
         Caption         =   "&Configure"
         Height          =   375
         Left            =   120
         TabIndex        =   7
         Top             =   120
         Width           =   2055
      End
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3600
         TabIndex        =   3
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   4800
         TabIndex        =   2
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
      Caption         =   "Configure System Databases for Auto Growth:"
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
      Width           =   3975
   End
End
Attribute VB_Name = "dlgWiz13"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 13: Configure System Databases for Auto Growth

Option Explicit

Dim mstrDetails As String
Dim mblnConfigured As Boolean


Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnConfigure_Click()
    Dim blnError As Boolean
    
    mstrDetails = ""
    blnError = False
    
    Me.MousePointer = vbHourglass
    SetSysDbAutoGrowth goDbServer, blnError, mstrDetails
    Me.MousePointer = vbDefault
    
    If blnError Then
        mblnConfigured = False
        mstrDetails = mstrDetails & _
            "Setup cannot continue.  Click 'Cancel' to quit."
    Else
        mblnConfigured = True
        mstrDetails = mstrDetails & _
            "The system databases have been configured for auto-growth.  Click 'Next' to proceed."
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz14.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz12.Show ' vbModal
End Sub
Private Sub Form_Load()
    mblnConfigured = False
    mstrDetails = _
        "Setup needs to configure the system databases on the local database server for auto-growth. " & _
        "Click on Configure to proceed."
End Sub

Private Sub Form_Activate()
    UpdateControls
    Me.btnConfigure.Visible = False
    btnConfigure_Click
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnConfigured Then
        btnConfigure.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnConfigure.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnConfigure.SetFocus
    End If
End Sub

