VERSION 5.00
Begin VB.Form dlgWiz40 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard"
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
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame Frame2 
      Caption         =   "Details"
      Height          =   1935
      Left            =   120
      TabIndex        =   2
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
         TabIndex        =   3
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
      Begin VB.CommandButton btnFinish 
         Caption         =   "&Finish && Reboot"
         Default         =   -1  'True
         Enabled         =   0   'False
         Height          =   375
         Left            =   4560
         TabIndex        =   1
         Top             =   120
         Width           =   1335
      End
   End
   Begin VB.Label I 
      Caption         =   "Installation Complete:"
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
      TabIndex        =   4
      Top             =   120
      Width           =   3075
   End
End
Attribute VB_Name = "dlgWiz40"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 26: Installation Complete

Option Explicit

Dim mstrDetails As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnFinish_Click()
    Dim blnError As Boolean
    
    mstrDetails = ""
    FinishInstallation blnError, mstrDetails
    
    If gblnReboot Then
        RebootSystem blnError, mstrDetails
    End If
    
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    
    Unload Me
    End
End Sub

Private Sub Form_Load()
    mstrDetails = _
        "The following components are installed on the system: " & vbCrLf
    
    If glngDbServerInst = INSTALLED Then
        mstrDetails = mstrDetails & _
            "- " & goDbServers(glngSqlType).Description & vbCrLf
    End If
    
    If glngServicePackInst = INSTALLED Then
        mstrDetails = mstrDetails & _
            "- " & goDbServers(glngSpType).Description & vbCrLf
    End If
    
    If glngAppDbInst = INSTALLED Then
        mstrDetails = mstrDetails & _
            "- The '" & goDbApp.strAppDbName & "' application database " & vbCrLf
    End If
        
    If gblnReboot Then
        mstrDetails = mstrDetails & vbCrLf & _
            "The system will be rebooted." & vbCrLf
    End If
    
    mstrDetails = mstrDetails & vbCrLf & _
        "Click on 'Finish' to complete setup."
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    btnFinish.Enabled = True
    btnFinish.SetFocus
End Sub
