VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "VTxtCallback"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Function SpeakingStarted()
    Debug.Print "Started"
    frmMain.btnSpeak.Caption = "&Stop"
End Function


Function SpeakingDone()
    Debug.Print "Stopped"
    frmMain.btnSpeak.Caption = "&Speak"
End Function



