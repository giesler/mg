VERSION 5.00
Begin VB.Form dlgWiz19 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 19)"
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
      Begin VB.CommandButton btnDetect 
         Caption         =   "&Detect"
         Height          =   375
         Left            =   120
         TabIndex        =   0
         Top             =   120
         Width           =   2055
      End
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Enabled         =   0   'False
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
         Enabled         =   0   'False
         Height          =   375
         Left            =   2400
         TabIndex        =   1
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Detect Character Set Configuration:"
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
      Width           =   4395
   End
End
Attribute VB_Name = "dlgWiz19"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 19: Detect Character Set and Sorting Configuration

Option Explicit

Dim mstrDetails As String
Dim mblnDetected As Boolean


Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnDetect_Click()
    Dim lngSortOrder As Long ' Sort order of installed database server
    Dim lngLocaleId As Long ' Unicode locale id of installed stabase server
    Dim lngCompStyle As Long ' Unicode comparison style of installed database server
    Dim blnError As Boolean
    
    mstrDetails = ""
    blnError = False
    
    Me.MousePointer = vbHourglass
    
    If Not blnError Then
        GetSortOrder goDbServer, blnError, lngSortOrder, mstrDetails
    End If
    
    If Not blnError Then
        GetUnicodeConfig goDbServer, blnError, lngLocaleId, lngCompStyle, mstrDetails
    End If
    
    If Not blnError Then
        If lngSortOrder = goDbApp.lngAppDbSortOrderId And lngLocaleId = goDbApp.lngAppDbUnicodeLocaleId And lngCompStyle = goDbApp.lngAppDbUnicodeCompStyle Then
            mstrDetails = mstrDetails & _
                "The character set configuration is compatible with the application database files. " & _
                "A file based installation image will be used to install the application database. "
            glngAppDbInstallType = FILE_IMAGE
        Else
            mstrDetails = mstrDetails & _
                "The character set configuration is not compatible with the application database files. " & _
                "A script based installation image will be used to install the application database. "
            glngAppDbInstallType = SCRIPT_IMAGE
        End If
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    
    If Not blnError Then
        mstrDetails = mstrDetails & _
            "Click 'Next' to proceed."
        mblnDetected = True
    Else
        mstrDetails = mstrDetails & _
            "Click 'Cancel' to quit."
        mblnDetected = False
    End If
    
    Me.MousePointer = vbDefault
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Unload Me
    dlgWiz20.Show ' vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz18.Show ' vbModal
End Sub

Private Sub Form_Load()
    Select Case glngAppDbInstallType
        Case APPDBIMAGETYPE_NOT_INITIALIZED, FILE_IMAGE
            mblnDetected = False
            mstrDetails = "Setup needs to detect the character set configuration of the database server. Click 'Detect' to continue."
        Case SCRIPT_IMAGE
            mblnDetected = True
            mstrDetails = "Character set configuration will not be checked because a script-based installation image is being used to install the database.  Click 'Next' to procede."
    End Select
End Sub
Private Sub Form_Activate()
    UpdateControls
    Me.btnDetect.Visible = False
    btnDetect_Click
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnDetected Then
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnDetect.Enabled = False
        btnNext.SetFocus
    Else
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnDetect.Enabled = True
        btnDetect.SetFocus
    End If
End Sub
