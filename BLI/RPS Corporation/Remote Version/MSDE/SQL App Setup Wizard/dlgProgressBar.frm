VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form dlgProgressBar 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Status"
   ClientHeight    =   1020
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   5265
   ControlBox      =   0   'False
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1020
   ScaleWidth      =   5265
   StartUpPosition =   2  'CenterScreen
   Begin VB.Timer tmr 
      Interval        =   1000
      Left            =   3960
      Top             =   120
   End
   Begin MSComctlLib.ProgressBar pbar 
      Height          =   225
      Left            =   120
      TabIndex        =   0
      Top             =   600
      Width           =   5055
      _ExtentX        =   8916
      _ExtentY        =   397
      _Version        =   393216
      Appearance      =   1
   End
   Begin VB.Label lblStatus 
      Caption         =   "Please wait..."
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   240
      Width           =   4815
   End
End
Attribute VB_Name = "dlgProgressBar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public lMinutes As Long

Private curSeconds As Long

Private Sub tmr_Timer()

Dim totalSeconds As Long, curPosition As Long

If lMinutes = 0 Then lMinutes = 10
totalSeconds = lMinutes * 60

curSeconds = curSeconds + 1

curPosition = curSeconds / totalSeconds * 100
If curPosition > 100 Then curPosition = 100
Me.pbar.Value = curPosition
On Error Resume Next
Me.SetFocus

End Sub
