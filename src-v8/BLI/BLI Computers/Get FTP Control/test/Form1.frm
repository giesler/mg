VERSION 5.00
Object = "{8E9ADF5A-DD23-4062-BBF3-C61E2CD41407}#15.0#0"; "bliftpget.ocx"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3915
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6210
   LinkTopic       =   "Form1"
   ScaleHeight     =   3915
   ScaleWidth      =   6210
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Command1"
      Height          =   495
      Left            =   2160
      TabIndex        =   1
      Top             =   2520
      Width           =   1455
   End
   Begin BLIFTPGet.ucGetFTP ucGetFTP1 
      Height          =   1695
      Left            =   480
      TabIndex        =   0
      Top             =   360
      Width           =   4935
      _ExtentX        =   8705
      _ExtentY        =   2990
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()

Me.ucGetFTP1.SetDebug
Me.ucGetFTP1.StartTransfer "mgd", "anonymous", "gggg", "c:\temp.tmp", "cake.mp3"

End Sub
