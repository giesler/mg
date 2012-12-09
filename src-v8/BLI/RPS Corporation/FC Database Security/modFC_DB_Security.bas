Attribute VB_Name = "modFC_DB_Security"
Private Const m_ModName = "modFC_DB_Security"
Public cnn As Connection
Public fs As frmStatus
Public blnSingleUser As Boolean
Public sSingleUserName As String
Public Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" _
 (ByVal lpBuffer As String, nSize As Long) As Long
Public Declare Sub Sleep Lib "Kernel32" (ByVal dwMilliseconds As Long)

Public m_sDBServer As String
Public m_sDBName As String
Public m_sDBUserName As String
Public m_sDBUserPW As String
Public m_sULDomain As String
Public m_sULServer As String

Public Sub Pause(f As Form)
f.Visible = True
While f.Visible
  DoEvents
Wend
End Sub

Public Sub Main()
On Error GoTo Main_Err

Select Case LCase(CurrentUser)
  Case "irene", "mike", "mike giesler", "bli", "mikeg"
  Case Else
    MsgBox "Only Irene can run the Factory Cat Database Security Administrator.", vbCritical
    End
    Exit Sub
End Select

'Form/var declares
Dim fI As frmIntro
Dim fUList As frmSelectUsers
Dim fSec As frmSelectPermissions
Dim fRev As frmReviewChanges
Dim fSet As frmSettings
Set fs = New frmStatus

'Load settings
PrivIniRegister "Settings", App.Path & "\" & App.EXEName & ".ini"
m_sDBServer = PrivGetString("DBServer", "fcserver")
m_sDBName = PrivGetString("DBName", "fcdata")
m_sDBUserName = PrivGetString("DBUserName", "sa")
m_sDBUserPW = PrivGetString("DBUserPW", "")
m_sULDomain = PrivGetString("ULDomain", "fc")
m_sULServer = PrivGetString("ULServer", "fcserver")

'Display start screen
Intro:
If fI Is Nothing Then
  Set fI = New frmIntro
End If
Pause fI
If fI.blnCancel Then
  GoTo Prog_Exit
End If

'Display settings if needed
Settings:
If fI.chkAdvanced Then
  If fSet Is Nothing Then Set fSet = New frmSettings
  Pause fSet
  If fSet.blnBack Then
    fSet.blnBack = False
    GoTo Intro
  End If
  If fSet.blnCancel Then GoTo Prog_Exit
End If

'Open connection
If cnn Is Nothing Then
  Set cnn = New Connection
  OpenWaitForm "Connecting to server..."
  cnn.Open "Provider=MSDataShape.1;Persist Security Info=True;Data Source=" & m_sDBServer _
    & ";User ID=" & m_sDBUserName & ";Password=" & m_sDBUserPW & ";Initial Catalog=" & m_sDBName & ";Data Provider=SQLOLEDB.1"
  CloseWaitForm
End If

' See if we should do advanced stuff
If Not fSet Is Nothing Then
  'Load user list if needed
  'Reset table if needed
  If fSet.chkResetSecTable.Enabled And fSet.chkResetSecTable Then
    OpenWaitForm "Clearing security table..."
    cnn.Execute "DELETE FROM tblSecurity"
    fSet.chkResetSecTable.Enabled = False
    CloseWaitForm
  End If
End If

'Display user list screen
UserList:
If fUList Is Nothing Then
  Set fUList = New frmSelectUsers
End If
fUList.Visible = False
If fUList.sServer <> m_sULServer Then
  OpenWaitForm "Loading user list..."
  LoadUserList fUList
  CloseWaitForm
End If
Pause fUList
If fUList.blnBack Then
  fUList.blnBack = False
  If fI.chkAdvanced Then
    GoTo Settings
  Else
    GoTo Intro
  End If
End If
If fUList.blnCancel Then GoTo Prog_Exit

'Display security settings screen
SecuritySettings:
If fSec Is Nothing Then
  Set fSec = New frmSelectPermissions
  fSec.Caption = fSec.Caption
Else
  ResetTreePermissions fSec
End If
If blnSingleUser Then
  LoadSingleUserSecurity fSec
End If
Pause fSec
If fSec.blnBack Then
  fSec.blnBack = False
  GoTo UserList
End If
If fSec.blnCancel Then
  GoTo Prog_Exit
End If

'Display review dialog
ReviewChanges:
Set fRev = New frmReviewChanges
LoadUsersForReview fUList, fRev
LoadPermissionsForReview fSec, fRev
If fRev.lvPerm.ListItems.Count = 0 Then
  MsgBox "You must select permissions to change!", vbExclamation
  GoTo SecuritySettings
End If
Pause fRev
If fRev.blnBack Then
  fRev.blnBack = False
  GoTo SecuritySettings
End If
If fRev.blnCancel Then
  GoTo Prog_Exit
End If
  
If UpdatePermissions(fRev) = False Then
  MsgBox "Since there was a problem setting the security, no changes were saved.", vbInformation
  GoTo ReviewChanges
End If

Dim fD As New frmDone
Pause fD
If fD.chkRepeat Then
  Set fUList = Nothing
  Set fSec = Nothing
  Set fRev = Nothing
  GoTo UserList
End If

Prog_Exit:
If Not (cnn Is Nothing) Then
  If cnn.State = adStateOpen Then cnn.Close
  Set cnn = Nothing
End If

End

Exit Sub
Main_Err:
Select Case Err
  Case 339
    If InStr(Err.Description, "MSCOMCTL") > 0 Then
      MsgBox "A required file to display the user list is not installed on your computer.  A window will open when you click OK.  Right click the file 'MSCOMCTL' (it has a little yellow cog on it) and choose 'Install'.  Restart your computer, then run this program again.  This error should not appear again.", vbExclamation
      Shell "explorer.exe ""\\fcserver\database\mscomctl"""
      End
    Else
      ErrHand m_ModName, "Main"
    End If
  Case Else
    ErrHand m_ModName, "Main"
End Select
End
Exit Sub
End Sub

Public Function LoadUsersForReview(fUser As frmSelectUsers, fReview As frmReviewChanges)

Dim li As ListItem, nli As ListItem

fReview.lvUsers.ListItems.Clear
For i = 1 To fUser.lvUsers.ListItems.Count
  Set li = fUser.lvUsers.ListItems.Item(i)
  If li.Checked Then
    Set nli = fReview.lvUsers.ListItems.Add(, , li.Text, , "user")
  End If
Next i

End Function

Public Function LoadPermissionsForReview(fPerm As frmSelectPermissions, fReview As frmReviewChanges)

Dim n As Node
Set n = fPerm.tvSwitchboard.Nodes.Item(1).Root
fReview.lvPerm.ListItems.Clear
LoadPermsSubTree n, fReview

End Function

Private Function LoadPermsSubTree(n As Node, fRev As frmReviewChanges)

Dim li As ListItem, ch As Node, i As Integer, sImage As String

If n.Tag = "updated" Then
  Set li = fRev.lvPerm.ListItems.Add(, n.Key, n.FullPath, , n.Image)
End If

If n.Children > 0 Then
  Set ch = n.Child
  If ch.Children > 0 Then
    LoadPermsSubTree ch, fRev
  ElseIf ch.Tag = "updated" Then
    Set li = fRev.lvPerm.ListItems.Add(, ch.Key, ch.FullPath, , ch.Image)
  End If
  Do While ch.Index <> ch.LastSibling.Index
    Set ch = ch.Next
    If ch.Children > 0 Then
      LoadPermsSubTree ch, fRev
    ElseIf ch.Tag = "updated" Then
      Set li = fRev.lvPerm.ListItems.Add(, ch.Key, ch.Text, , ch.Image)
    End If
  Loop
End If

End Function

Public Function OpenWaitForm(sStatus As String)
fs.lblStatus.Caption = sStatus
fs.Visible = True
fs.Refresh
End Function

Public Function CloseWaitForm()
fs.Visible = False
End Function

Public Function UpdatePermissions(fRev As frmReviewChanges) As Boolean
On Error GoTo UpdatePermissions_Err

UpdatePermissions = False

Dim liUser As ListItem, liPerm As ListItem
Dim iUser As Integer, iPerm As Integer
Dim cnUpdate As Connection, sSQL As String
Set cnUpdate = New ADODB.Connection

OpenWaitForm "Updating permissions..."
cnUpdate.Open cnn.ConnectionString

cnUpdate.BeginTrans
For iUser = 1 To fRev.lvUsers.ListItems.Count
  Set liUser = fRev.lvUsers.ListItems(iUser)
  
  For iPerm = 1 To fRev.lvPerm.ListItems.Count
    Set liPerm = fRev.lvPerm.ListItems(iPerm)
    sSQL = "delete from dbo.tblSecurity where UserID = '" & liUser.Text & "'" _
      & " And SwID = " & Val(Mid(liPerm.Key, 4))
    cnUpdate.Execute sSQL
    Select Case liPerm.SmallIcon
      Case "full"
        sSQL = "insert into dbo.tblSecurity (UserID, SwID, AccessType) " _
          & "values ('" & liUser.Text & "', " & Val(Mid(liPerm.Key, 4)) & ", 2)"
        cnUpdate.Execute sSQL
      Case "read"
        sSQL = "insert into dbo.tblSecurity (UserID, SwID, AccessType) " _
          & "values ('" & liUser.Text & "', " & Val(Mid(liPerm.Key, 4)) & ", 1)"
        cnUpdate.Execute sSQL
      Case "deny"
        sSQL = "insert into dbo.tblSecurity (UserID, SwID, AccessType) " _
          & "values ('" & liUser.Text & "', " & Val(Mid(liPerm.Key, 4)) & ", 0)"
        cnUpdate.Execute sSQL
      Case "nothing"
        'ignore case, we don't want any permissions set here
    End Select
  Next iPerm
Next iUser

OpenWaitForm "Saving changes..."
cnUpdate.CommitTrans
cnUpdate.Close
Set cnUpdate = Nothing
UpdatePermissions = True
CloseWaitForm

Exit Function
UpdatePermissions_Err:
ErrHand "modFC_DB_Security", "UpdatePermissions"
On Error Resume Next
If cnUpdate.State = adStateOpen Then
  cnUpdate.RollbackTrans
End If
cnUpdate.Close
Set cnUpdate = Nothing
CloseWaitForm
Exit Function
End Function

Public Function ResetTreePermissions(fSec As frmSelectPermissions)

Dim n As Node
For Each n In fSec.tvSwitchboard.Nodes
  n.Image = "nothing"
Next n

End Function

Public Function LoadSingleUserSecurity(fSec As frmSelectPermissions)
On Error GoTo LoadSingleUserSecurity_Err

Dim sSQL As String, rs As Recordset, nNode As Node
OpenWaitForm "Loading security..."
sSQL = "select swid, accesstype from dbo.tblSecurity where userid = '" & sSingleUserName & "'"
Set rs = New Recordset
rs.Open sSQL, cnn, adOpenForwardOnly, adLockReadOnly
If Not rs.BOF Or Not rs.EOF Then
  Do While Not rs.EOF
    Set nNode = FindNodeWithKey(fSec.tvSwitchboard, "id:" & rs!swid)
    If Not (nNode Is Nothing) Then
      Select Case rs!AccessType
        Case 0
          nNode.Image = "deny"
        Case 1
          nNode.Image = "read"
        Case 2
          nNode.Image = "full"
      End Select
    End If
    rs.MoveNext
  Loop
End If
rs.Close
Set rs = Nothing
CloseWaitForm

Exit Function
LoadSingleUserSecurity_Err:
ErrHand "modFC_DB_Security", "LoadSingleUserSecurity"
CloseWaitForm
Exit Function

End Function

Private Function FindNodeWithKey(tv As TreeView, k As String) As Node

Dim n As Node

Set FindNodeWithKey = Nothing

For Each n In tv.Nodes
  If n.Key = k Then
    Set FindNodeWithKey = n
    Exit Function
  End If
Next n

End Function

Public Function CurrentUser()
On Error GoTo CurrentUser_Err

' Dimension variables
Dim lpBuff As String * 25
Dim Ret As Long, UserName As String

' Get the user name minus any trailing spaces found in the name.
Ret = GetUserName(lpBuff, 25)
UserName = Left(lpBuff, InStr(lpBuff, Chr(0)) - 1)

CurrentUser = Trim(UserName)

Exit Function
CurrentUser_Err:
ErrHand "modBasicFunctions", "CurrentUser"
Exit Function

End Function

Public Function LoadUserList(fSU As frmSelectUsers)
On Error GoTo LoadUserList_Err

Dim r As New Recordset, cmd As New ADODB.Command
Dim sUList As String, sName As String

cmd.CommandText = "spGetNTUserList"
cmd.CommandType = adCmdStoredProc
cmd.ActiveConnection = cnn
Set r = cmd.Execute

If r.EOF Then Exit Function

fSU.lvUsers.ListItems.Clear
sUList = r!uList
Do While InStr(sUList, "|") > 0
  sName = Left(sUList, InStr(sUList, "|") - 1)
  If InStr(sName, "USR") = 2 Or InStr(sName, "IWAM") Or InStr(sName, "SQL") Then
  Else
    fSU.lvUsers.ListItems.Add , , sName, , "user"
  End If
  sUList = Mid(sUList, InStr(sUList, "|") + 1)
Loop
If Len(sUList) > 0 Then
  sName = sUList
  If InStr(sName, "USR") = 2 Or InStr(sName, "IWAM") Or InStr(sName, "SQL") Then
  Else
    fSU.lvUsers.ListItems.Add , , sName, , "user"
  End If
End If

r.Close
Set r = Nothing

Exit Function
LoadUserList_Err:
ErrHand "modFC_DB_Security", "LoadUserList"
Exit Function

End Function
