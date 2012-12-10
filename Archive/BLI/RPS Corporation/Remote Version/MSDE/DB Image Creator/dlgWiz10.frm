VERSION 5.00
Begin VB.Form dlgWiz10 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "DbInstallImage Wizard (Step 10)"
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
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   2175
      Left            =   120
      TabIndex        =   2
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
         TabIndex        =   3
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.CommandButton btnFinish 
      Cancel          =   -1  'True
      Caption         =   "Finish"
      Height          =   375
      Left            =   4800
      TabIndex        =   0
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Wizard Complete:"
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
      TabIndex        =   1
      Top             =   120
      Width           =   3015
   End
End
Attribute VB_Name = "dlgWiz10"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim mstrDetails As String


Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnFinish_Click()
    Unload Me
End Sub

Private Sub Form_Load()
    mstrDetails = _
        "The DbInstallImage Wizard has successfully created an application database installation image " & _
        "for the application database named '" & goAppDb.strAppDbName & "' in the folder '" & _
        gstrImageFolder & "'. " & vbCrLf & vbCrLf & _
        "You may now use this image in conjunction with the 'SqlAppSetupWiz' " & _
        "installation wizard to deploy a database server and your application database on " & _
        "other servers and workstations." & vbCrLf & vbCrLf & _
        "Click 'Finish' to quit."
End Sub

Private Sub Form_Activate()
    UpdateControls
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    btnFinish.SetFocus
End Sub
