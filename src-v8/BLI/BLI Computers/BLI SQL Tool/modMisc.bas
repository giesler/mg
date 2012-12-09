Attribute VB_Name = "Module1"

Public Function HandleSQLError(sSQL As String, blnSkip As Boolean, iError As Integer, sDesc As String) As Boolean

HandleSQLError = False
Dim fE As frmError
Set fE = New frmError
fE.lblErrorNumber.Caption = iError
fE.lblErrorDescription.Caption = sDesc
fE.txtSQLStatement.Text = sSQL
fE.Visible = True
fE.Refresh
While fE.Visible
  DoEvents
Wend

HandleSQLError = fE.blnCancel
blnSkip = fE.blnSkip
sSQL = fE.txtSQLStatement.Text
Set fE = Nothing

End Function
