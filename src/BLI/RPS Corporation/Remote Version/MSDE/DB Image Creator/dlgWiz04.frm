VERSION 5.00
Begin VB.Form dlgWiz04 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "DbInstallImage Wizard (Step 4)"
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
   Begin VB.CommandButton btnConfigure 
      Caption         =   "&Configure Properties"
      Height          =   375
      Left            =   120
      TabIndex        =   2
      Top             =   2640
      Width           =   1935
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
         TabStop         =   0   'False
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
      Caption         =   "Configure Installation Image Properties:"
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
      Width           =   5415
   End
End
Attribute VB_Name = "dlgWiz04"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mstrDetails As String
Dim mblnError As Boolean

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnConfigure_Click()
    dlgAppDbProps.Show vbModal
    
    If gblnAppDbPropsConfigured Then
        mstrDetails = _
            "Click 'Configure Properties' to adjust installation image properties, or click " & _
            "'Next' to proceed. "
    Else
        mstrDetails = _
           "Click 'Configure Properties' to configure the installation image properties. "
    End If
    
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Dim strMessage As String
    
    With goAppDb
        strMessage = _
            Me.Name & ": The following installation image properties were configured: " & _
            "AppDbSortOrderId - " & CStr(.lngAppDbSortOrderId) & "; " & _
            "AppDbUnicodeLocaleId - " & CStr(.lngAppDbUnicodeLocaleId) & "; " & _
            "AppDbUnicodeCompStyle - " & CStr(.lngAppDbUnicodeCompStyle) & "; " & _
            "AppDbCsdVer - " & CStr(.lngAppDbCsdVer) & "; " & _
            "AppDbBytesReq - " & CStr(.lngAppDbBytesReq) & "; " & _
            "AppDbName - " & CStr(.strAppDbName) & "; " & _
            "AppDbVer - " & CStr(.strAppDbVer) & "; " & _
            "AppDbVerSp - " & CStr(.strAppDbVerSp) & "; " & _
            "AppDbOrg - " & CStr(.strAppDbOrg) & "; " & _
            "AppDbDate - " & Format(.dtAppDbDate, "Short Date") & "; " & _
            "AppDbDesc - " & CStr(.strAppDbDesc) & "; "
    End With
    
    WriteLogMsg goLogFileHandle, strMessage
    Unload Me
    dlgWiz05.Show vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz03.Show vbModal
End Sub

Private Sub Form_Load()
    If gblnAppDbPropsConfigured Then
        mstrDetails = _
            "Click 'Configure Properties' to adjust installation image properties, or click " & _
            "'Next' to proceed. "
    Else
        mstrDetails = _
           "Click 'Configure Properties' to configure the installation image properties. "
    End If
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnError Then
        btnConfigure.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnCancel.SetFocus
        
        Exit Sub
    End If
    
    If gblnAppDbPropsConfigured Then
        btnConfigure.Enabled = True
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
