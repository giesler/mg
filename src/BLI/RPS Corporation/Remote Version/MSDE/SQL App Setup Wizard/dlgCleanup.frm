VERSION 5.00
Begin VB.Form dlgCleanup 
   Caption         =   "Cleanup Form"
   ClientHeight    =   1260
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4380
   LinkTopic       =   "Form1"
   ScaleHeight     =   1260
   ScaleWidth      =   4380
   StartUpPosition =   3  'Windows Default
End
Attribute VB_Name = "dlgCleanup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Unload(Cancel As Integer)

    ' All Forms Unloaded, Time to go...
    WriteLogMsg goLogFileHandle, "Session Ending."
    CleanUp
    
End Sub
