VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "Clipboard speaker"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton btnExit 
      Caption         =   "E&xit"
      Height          =   495
      Left            =   3360
      TabIndex        =   3
      Top             =   2640
      Width           =   1215
   End
   Begin VB.CommandButton btnSpeak 
      Caption         =   "&Speak"
      Default         =   -1  'True
      Height          =   495
      Left            =   120
      TabIndex        =   2
      Top             =   2640
      Width           =   1215
   End
   Begin VB.Timer tmrSpeak 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   2280
      Top             =   2640
   End
   Begin VB.TextBox txtTextToSpeak 
      Height          =   2295
      Left            =   0
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   1
      Text            =   "frmMain.frx":0000
      Top             =   240
      Width           =   4695
   End
   Begin VB.CheckBox chkClipboardMonitor 
      Caption         =   "&Monitor clipboard"
      Height          =   255
      Left            =   1440
      TabIndex        =   0
      Top             =   2760
      Width           =   1815
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "Clipboard contents"
      Height          =   255
      Left            =   0
      TabIndex        =   4
      Top             =   0
      Width           =   4695
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

'voice object
Dim oVoice As VTxtAuto.VTxtAuto

'looping constraint
Dim bTimerLoop As Boolean

'toggle for clipboard monitoring
Dim bMonitorClipboard As Boolean

Private Sub btnExit_Click()

    'clean up and get out of here
    Unload Me
    End

End Sub


Private Sub btnSpeak_Click()

    If bMonitorClipboard Then
        'speak or stop speaking, depending on state
        If oVoice.IsSpeaking Or bTimerLoop Then
            'disable all constraint and redo button caption
            bTimerLoop = False
            tmrSpeak.Enabled = False
            oVoice.StopSpeaking
        Else
            'indicate we're speaking and start
            tmrSpeak.Enabled = True
        End If
    Else
        'speak or stop speaking, depending on state
        If oVoice.IsSpeaking Then
            'disable all constraint and redo button caption
            oVoice.StopSpeaking
        Else
            'indicate we're speaking and start SPEAKING
            oVoice.Speak txtTextToSpeak, vtxtst_READING
        End If
    End If

End Sub


Private Sub chkClipboardMonitor_Click()
    
    bMonitorClipboard = CBool(chkClipboardMonitor.Value)

End Sub

Private Sub Form_Load()

    'get instance of Voice object and register it
    Set oVoice = New VTxtAuto.VTxtAuto
    oVoice.Register App.Title, App.EXEName
    oVoice.Callback = "Speak2Me.VTxtCallback"
    
    'greet user
    oVoice.Speak txtTextToSpeak, vtxtst_READING

    'set for non looping speech
    bTimerLoop = False
    
End Sub


Private Sub Form_Unload(Cancel As Integer)

    'stop any speaking and clean up
    oVoice.StopSpeaking
    Set oVoice = Nothing

End Sub


Private Sub tmrSpeak_Timer()
Dim sText As String

    'exit if already speaking, to prevent overlap
    If oVoice.IsSpeaking Then
        Exit Sub
    End If
    
    'get clipboard text and exit if not changed
    sText = Clipboard.GetText
    If sText = txtTextToSpeak Then
        Exit Sub
    End If

    'set up text and looping constraint
    txtTextToSpeak = sText
    bTimerLoop = True
    
    'speak to user
    oVoice.Speak txtTextToSpeak, vtxtst_READING

End Sub



