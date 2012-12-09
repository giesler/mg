VERSION 5.00
Begin VB.Form frmMP3Util 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "MP3 Utility"
   ClientHeight    =   2895
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4980
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2895
   ScaleWidth      =   4980
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   3840
      TabIndex        =   8
      Top             =   2400
      Width           =   855
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   2880
      TabIndex        =   7
      Top             =   2400
      Width           =   855
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   495
      Left            =   120
      Picture         =   "frmMusicUtils.frx":0000
      ScaleHeight     =   495
      ScaleWidth      =   615
      TabIndex        =   4
      Top             =   120
      Width           =   615
   End
   Begin VB.CheckBox Check1 
      Caption         =   "&Add file to Windows Media Library"
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   1560
      Width           =   4695
   End
   Begin VB.CommandButton cmdBrowse 
      Caption         =   "&Browse"
      Height          =   255
      Left            =   3720
      TabIndex        =   1
      Top             =   1200
      Width           =   855
   End
   Begin VB.CheckBox chkCopyFile 
      Caption         =   "&Copy file to MP3 drive"
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   960
      Width           =   4695
   End
   Begin VB.Label lblFileName 
      Caption         =   "<filename>"
      Height          =   255
      Left            =   840
      TabIndex        =   6
      Top             =   360
      Width           =   4215
   End
   Begin VB.Label Label1 
      Caption         =   "What would you like to do with the following file?"
      Height          =   255
      Left            =   840
      TabIndex        =   5
      Top             =   120
      Width           =   4215
   End
   Begin VB.Label lblCopyTo 
      Caption         =   "\\cheese\mp3s"
      Height          =   255
      Left            =   360
      TabIndex        =   0
      Top             =   1200
      Width           =   3135
   End
End
Attribute VB_Name = "frmMP3Util"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()

  Dim m As WMPOCX.IWMPMediaCollection, txtOut As String
  Set m = New WMPOCX.WMPOCX
  Dim pl As WMPOCX.IWMPPlaylist
  Set pl = m.getByAuthor("Dave Matthews")
  Dim pli As WMPOCX.IWMPMedia
  Dim i As Integer
  
  For i = 0 To pl.Count - 1
    txtOut = txtOut + pl.Item(i).Name + vbCrLf
  Next i
  Me.Text1 = txtOut
  
  m.Add "d:\New MP3s\Bare Naked Ladies - Pinch Me.mp3"
  
  
End Sub
