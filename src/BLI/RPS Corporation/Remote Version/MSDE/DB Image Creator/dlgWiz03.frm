VERSION 5.00
Begin VB.Form dlgWiz03 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "DbInstallImage Wizard (Step 3)"
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
   Begin VB.ComboBox cmbAppDbList 
      Height          =   315
      Left            =   120
      Style           =   2  'Dropdown List
      TabIndex        =   6
      Top             =   480
      Width           =   5775
   End
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   1575
      Left            =   120
      TabIndex        =   0
      Top             =   960
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1215
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
      TabIndex        =   2
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   4
      Top             =   2640
      Width           =   1095
   End
   Begin VB.CommandButton btnNext 
      Caption         =   "&Next >>"
      Height          =   375
      Left            =   3600
      TabIndex        =   3
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Select An Application Database:"
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
      TabIndex        =   5
      Top             =   120
      Width           =   3015
   End
End
Attribute VB_Name = "dlgWiz03"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Option Base 1

Dim mstrDetails As String
Dim mblnError As Boolean
Dim mstrDbNames() As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnNext_Click()
    goAppDb.strAppDbName = cmbAppDbList.Text
    gstrImageFolder = App.Path & "\" & gstrImageFolderName & "\" & goAppDb.strAppDbName
    WriteLogMsg goLogFileHandle, Me.Name & ": User selected the '" & goAppDb.strAppDbName & "' application database."
    Unload Me
    dlgWiz04.Show vbModal
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz02.Show vbModal
End Sub

Private Sub Form_Activate()
    Dim AppDbNames() As String
    Dim strDbName As Variant
    
    Me.MousePointer = vbHourglass
    mblnError = False
    
    LoadAppDbNames goDbServer, AppDbNames, mblnError, mstrDetails
    
    If Not mblnError Then
        If UBound(AppDbNames) >= 1 Then
            mstrDetails = _
                "Select the application database which will be the source " & _
                "for your new application database installation image, then click " & _
                "'Next' to proceed."
            
            cmbAppDbList.Enabled = True
            
            For Each strDbName In AppDbNames
                cmbAppDbList.AddItem strDbName
            Next
            
            cmbAppDbList.ListIndex = 0
        Else
            mstrDetails = _
                "There are no application databases on the local database server. " & _
                "The wizard cannot continue. " & _
                "Click 'Next' to proceed."
        End If
    Else
        cmbAppDbList.Enabled = False
        
        mstrDetails = mstrDetails & _
            "Unable to load application database names.  Click 'Cancel' to quit."
        
        mblnError = True
    End If

    Me.MousePointer = vbDefault
    
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnError Then
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        btnCancel.SetFocus
    Else
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
    End If
End Sub
