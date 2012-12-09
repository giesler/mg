VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "Mscomctl.ocx"
Begin VB.Form frmStatus 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Please Wait..."
   ClientHeight    =   1005
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   3990
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1005
   ScaleWidth      =   3990
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin MSComctlLib.ProgressBar pb1 
      Height          =   165
      Left            =   120
      TabIndex        =   0
      Top             =   480
      Width           =   3735
      _ExtentX        =   6588
      _ExtentY        =   291
      _Version        =   393216
      Appearance      =   1
   End
   Begin VB.Label lblPercent 
      Alignment       =   2  'Center
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   720
      Width           =   3735
   End
   Begin VB.Label Label1 
      Caption         =   "Running database update..."
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   3735
   End
End
Attribute VB_Name = "frmStatus"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
