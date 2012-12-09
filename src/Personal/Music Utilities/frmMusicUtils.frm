VERSION 5.00
Object = "{22D6F304-B0F6-11D0-94AB-0080C74C7E95}#1.0#0"; "msdxm.ocx"
Begin VB.Form frmMP3Util 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "MP3 Utility"
   ClientHeight    =   3330
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4980
   Icon            =   "frmMusicUtils.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3330
   ScaleWidth      =   4980
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdBrowse 
      Caption         =   "&Browse"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   3720
      TabIndex        =   1
      Top             =   1680
      Width           =   855
   End
   Begin VB.CheckBox chkPlayFile 
      Caption         =   "&Play file after added to library"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   360
      TabIndex        =   13
      Top             =   2520
      Width           =   4335
   End
   Begin VB.CheckBox chkDeleteLocal 
      Caption         =   "&Delete local copy after copy"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   360
      TabIndex        =   12
      Top             =   1920
      Value           =   1  'Checked
      Width           =   4335
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3720
      TabIndex        =   8
      Top             =   2880
      Width           =   975
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2640
      TabIndex        =   7
      Top             =   2880
      Width           =   975
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   120
      Picture         =   "frmMusicUtils.frx":030A
      ScaleHeight     =   495
      ScaleWidth      =   615
      TabIndex        =   4
      Top             =   120
      Width           =   615
   End
   Begin VB.CheckBox chkAddFile 
      Caption         =   "&Add file to Windows Media Library"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   2280
      Value           =   1  'Checked
      Width           =   4695
   End
   Begin VB.CheckBox chkCopyFile 
      Caption         =   "&Copy file to MP3 drive"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   1440
      Value           =   1  'Checked
      Width           =   4695
   End
   Begin MediaPlayerCtl.MediaPlayer objEmbeddedMP 
      Height          =   375
      Left            =   3840
      TabIndex        =   14
      Top             =   840
      Width           =   1095
      AudioStream     =   -1
      AutoSize        =   0   'False
      AutoStart       =   -1  'True
      AnimationAtStart=   -1  'True
      AllowScan       =   -1  'True
      AllowChangeDisplaySize=   -1  'True
      AutoRewind      =   0   'False
      Balance         =   0
      BaseURL         =   ""
      BufferingTime   =   5
      CaptioningID    =   ""
      ClickToPlay     =   -1  'True
      CursorType      =   0
      CurrentPosition =   -1
      CurrentMarker   =   0
      DefaultFrame    =   ""
      DisplayBackColor=   0
      DisplayForeColor=   16777215
      DisplayMode     =   0
      DisplaySize     =   4
      Enabled         =   -1  'True
      EnableContextMenu=   -1  'True
      EnablePositionControls=   -1  'True
      EnableFullScreenControls=   0   'False
      EnableTracker   =   -1  'True
      Filename        =   ""
      InvokeURLs      =   -1  'True
      Language        =   -1
      Mute            =   0   'False
      PlayCount       =   1
      PreviewMode     =   0   'False
      Rate            =   1
      SAMILang        =   ""
      SAMIStyle       =   ""
      SAMIFileName    =   ""
      SelectionStart  =   -1
      SelectionEnd    =   -1
      SendOpenStateChangeEvents=   -1  'True
      SendWarningEvents=   -1  'True
      SendErrorEvents =   -1  'True
      SendKeyboardEvents=   0   'False
      SendMouseClickEvents=   0   'False
      SendMouseMoveEvents=   0   'False
      SendPlayStateChangeEvents=   -1  'True
      ShowCaptioning  =   0   'False
      ShowControls    =   -1  'True
      ShowAudioControls=   -1  'True
      ShowDisplay     =   0   'False
      ShowGotoBar     =   0   'False
      ShowPositionControls=   -1  'True
      ShowStatusBar   =   0   'False
      ShowTracker     =   -1  'True
      TransparentAtStart=   0   'False
      VideoBorderWidth=   0
      VideoBorderColor=   0
      VideoBorder3D   =   0   'False
      Volume          =   -600
      WindowlessVideo =   0   'False
   End
   Begin VB.Label lblFile 
      Caption         =   "<filename>"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1800
      TabIndex        =   11
      Top             =   600
      Width           =   3135
   End
   Begin VB.Label Label3 
      Caption         =   "File:"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   960
      TabIndex        =   10
      Top             =   600
      Width           =   855
   End
   Begin VB.Label Label2 
      Caption         =   "Directory:"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   960
      TabIndex        =   9
      Top             =   360
      Width           =   855
   End
   Begin VB.Label lblDirectory 
      Caption         =   "<dir>"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1800
      TabIndex        =   6
      Top             =   360
      Width           =   3135
   End
   Begin VB.Label Label1 
      Caption         =   "What would you like to do with the following file?"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   840
      TabIndex        =   5
      Top             =   120
      Width           =   4215
   End
   Begin VB.Label lblCopyTo 
      Caption         =   "\\cheese\mp3s\"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   360
      TabIndex        =   0
      Top             =   1680
      Width           =   3135
   End
End
Attribute VB_Name = "frmMP3Util"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub chkAddFile_Click()

  Me.chkPlayFile.Enabled = Me.chkAddFile
  
End Sub

Private Sub chkCopyFile_Click()

  Me.chkDeleteLocal.Enabled = Me.chkCopyFile
  Me.cmdBrowse.Enabled = Me.chkCopyFile
  Me.lblCopyTo.Enabled = Me.chkCopyFile
  
End Sub

Private Sub cmdBrowse_Click()

  Dim strTemp As String
  strTemp = GetBrowseFolder("Choose the destination location.", Me.hWnd)
  If strTemp <> "" Then
    Me.lblCopyTo.Caption = strTemp
  End If
  
End Sub

Private Sub cmdCancel_Click()

  End
  
End Sub

Private Sub cmdOK_Click()

  Dim objWMP As WMPOCX.WMPOCX, objMedCol As WMPOCX.IWMPMediaCollection
  Dim objMed As WMPOCX.IWMPMedia, strDestFile As String
  
  Me.chkAddFile.Enabled = False
  Me.chkDeleteLocal.Enabled = False
  Me.lblCopyTo.Enabled = False
  Me.chkCopyFile.Enabled = False
  Me.cmdBrowse.Enabled = False
  Me.chkPlayFile.Enabled = False
  Me.cmdOK.Enabled = False
  Me.cmdCancel.Enabled = False
  Me.objEmbeddedMP.Stop
  Me.objEmbeddedMP.Enabled = False
  Me.Refresh
  
  If Me.chkCopyFile Then
    strDestFile = Me.lblCopyTo + Me.lblFile
    CopySourceToDest Me.lblDirectory + Me.lblFile, Me.lblCopyTo
    If Me.chkDeleteLocal Then
      Kill Me.lblDirectory + Me.lblFile
    End If
  Else
    strDestFile = Me.lblDirectory + Me.lblFile
  End If
  
  If Me.chkAddFile Then
    Set objWMP = New WMPOCX.WMPOCX
    Set objMedCol = objWMP.mediaCollection
    Set objMed = objMedCol.Add(strDestFile)
    If Me.chkPlayFile Then
      Shell "d:\Program Files\Windows Media Player\wmplayer.exe" + " /Play " + Chr(34) + strDestFile + Chr(34)
    End If
    Set objMed = Nothing
    Set objMedCol = Nothing
    Set objWMP = Nothing
  End If
    
  End
  
End Sub

Private Sub Form_Load()

  If Command = "" Then
    MsgBox "This program can only be started with a file.", vbExclamation
    End
  End If
  
  Me.lblDirectory.Caption = Left(Command, InStrRev(Command, "\"))
  Me.lblFile.Caption = Mid(Command, InStrRev(Command, "\") + 1)
  Me.objEmbeddedMP.autoStart = False
  Me.objEmbeddedMP.Open Command
  
End Sub
