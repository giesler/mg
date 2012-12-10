Attribute VB_Name = "Global"
Option Explicit

Public Function ErrHand(Optional strLocation As String, Optional strFunction As String)

Select Case Err
  Case Else
    Dim sMsg As String
    sMsg = Err.Description & vbCrLf
    sMsg = sMsg & "Source: " & strLocation & "  (" & strFunction & ")" & vbCrLf
    sMsg = sMsg & "Number: " & Err.Number & " (" & Err.Source & ")"
    MsgBox sMsg, vbExclamation, "Error"
    CloseWaitForm
End Select

End Function


