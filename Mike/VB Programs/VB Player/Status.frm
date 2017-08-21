VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Begin VB.Form frmStatus 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Please Wait..."
   ClientHeight    =   420
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   3930
   ControlBox      =   0   'False
   LinkTopic       =   "Form2"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   420
   ScaleWidth      =   3930
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin MSComctlLib.ProgressBar pb 
      Height          =   150
      Left            =   120
      TabIndex        =   1
      Top             =   480
      Width           =   3735
      _ExtentX        =   6588
      _ExtentY        =   265
      _Version        =   393216
      Appearance      =   1
   End
   Begin VB.Label lblStatus 
      Caption         =   "Loading..."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3735
   End
End
Attribute VB_Name = "frmStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()

    Height = 750

End Sub

Public Sub EnableProgressBar(intMax As Long)

    pb.Max = intMax
    pb.Value = 0
    Height = 1020
    Refresh

End Sub

Public Sub DisableProgressBar()

    Height = 750
    
End Sub

Public Sub UpdateProgressBar(intValue As Long)

    pb.Value = intValue
    Refresh

End Sub
