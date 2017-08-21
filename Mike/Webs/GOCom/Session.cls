VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 3  'UsesTransaction
END
Attribute VB_Name = "Session"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit

' Author: Mike Giesler
' Project: giesler.org Common Components
' Creation Date: Sept 21, 2000
' Purpose:
'       This class provides methods for supporting a web session like object.  It uses a
'         database for storage, so can be scaled across web servers.
'
' Usage:
'       Initially, you must create a new session:
'             mstrSessionId = objSession.CreateSession();
'       Then to use, you must follow the following format:
'             objSession.Load mstrUserId, mstrSessionId
'             objSession("ElementName") = "ElementValue"      'sets value
'             objSession("ElementName")                       'retreives value
'
' Change History:
' ID    Date        Method              Change Description
'
'
'

Implements ObjectControl
Implements IObjectConstruct

Private mCol As Collection
Private mguidSessionId As String
Private mobjDbh As GOCom.DBHelper
Private Const mstrModName As String = "GOCom.Session"
Private mblnLoaded As Boolean
Private mblnModified As Boolean

Private mintSessionMins As Integer

' receives params for this object
Private Sub IObjectConstruct_Construct(ByVal pobjConstruct As Object)
On Error GoTo errorHandler
  
  'Default values
  mintSessionMins = 20
  
  ' Check construct string for timeout setting
  Dim strTemp As String
  Dim vParams As Variant, i As Integer
  strTemp = pobjConstruct.ConstructString
  
  If Len(Trim(strTemp)) > 1 Then
    ' split into array, using first char as sep
    vParams = Split(Mid$(strTemp, 2), Left$(strTemp, 1))
    For i = LBound(vParams) To UBound(vParams)
      Select Case UCase(GetKey(vParams(i)))
        Case UCase("SessionTimeout")
          If IsNumeric(GetValue(vParams(i))) Then
            mintSessionMins = Int(Val(GetValue(vParams(i))))
          End If
      End Select
    Next i
  End If

Exit Sub
errorHandler:
  RaiseError mstrModName, "IObjectConstruct_Construct"
End Sub

' sets up object
Private Sub ObjectControl_Activate()
  
  ' Make sure construct was called, if not default settings
  If mintSessionMins = 0 Then mintSessionMins = 20
  
  Set mCol = New Collection
  Set mobjDbh = CreateObject("GOCom.DBHelper")

End Sub

Private Function ObjectControl_CanBePooled() As Boolean
  
  ObjectControl_CanBePooled = False

End Function

' saves session to database if it has been modified, then drops objects
Private Sub ObjectControl_Deactivate()

  If mblnModified Then SaveSession
  Set mCol = Nothing
  Set mobjDbh = Nothing

End Sub

' Load the specified session from the database
Public Sub Load(pstrUserID As String, pstrSessionId As String)
On Error GoTo errorHandler

  mguidSessionId = CStr(pstrSessionId)
  LoadSession
  mblnLoaded = True
  mblnModified = False

Exit Sub
errorHandler:
  CtxRaiseError mstrModName, "Load"
End Sub

' Function for the following syntax:  mobjSession("ItemName") = "ItemData"
Public Property Let Value(pvarKey As Variant, pvarData As Variant)
On Error Resume Next
    
  ' Check if loaded
  If Not mblnLoaded Then
    Err.Raise vbObjectError + 2, mstrModName & " PropertyLet Value", "Session not loaded."
    Exit Property
  End If
  
  ' See if we can find the element requested
  Dim objSE As SessionElement
  Set objSE = mCol(pvarKey)
  If Err Then
    ' couldn't find, add and set data to pvarData
    Set objSE = New SessionElement
    objSE.strKey = CStr(pvarKey)
    objSE.strData = CStr(pvarData)
    mCol.Add objSE, pvarKey
  Else
    ' found it; set it
    objSE.strData = pvarData
    objSE.blnDeleted = False
  End If
  Set objSE = Nothing
  
  ' flag that there has been a modification
  mblnModified = True

End Property

' Function for the following syntax:  varWhatever = mobjSession("ItemName")
Public Property Get Value(pvarKey As Variant) As Variant
Attribute Value.VB_UserMemId = 0
On Error Resume Next
  
  ' Check if loaded
  If Not mblnLoaded Then
    Err.Raise vbObjectError + 3, mstrModName & " PropertyGet Value", "Session not loaded."
    Exit Property
  End If
  
  ' See if we can find element requested, if so return it, if not return blank
  Dim objSE As SessionElement
  Set objSE = mCol(pvarKey)
  If Err Then
    Value = ""
  ElseIf objSE.blnDeleted Then
    Value = ""
  Else
    Value = objSE.strData
  End If
  Set objSE = Nothing

End Property

' Loads the session from the database
Private Sub LoadSession()
On Error GoTo LoadSession_Err
  
  Dim rsSession As Recordset
  Dim strSessionData As String, varSessionData As Variant, varSessionElements As Variant
  Dim i As Long, objSE As SessionElement, varLastUpdate As Variant
  
  ' Load from database sp
  Set rsSession = mobjDbh.RunSPReturnRS("sp_Session_Load", _
    mp("Session_ID", adVarChar, 50, mguidSessionId), _
    mp("Last_Update", adVarChar, 30, Now))
  
  ' check for valid rs
  If rsSession.BOF And rsSession.EOF Then
    Err.Raise vbObjectError + 1, mstrModName & " LoadSession()", "Invalid or expired session"
    Exit Sub
  End If
  varLastUpdate = rsSession.Fields("Last_Update")
  If IsNull(varLastUpdate) Then
    Err.Raise vbObjectError + 8, mstrModName & " LoadSession", "Invalid last updated date."
    Exit Sub
  End If
  
  ' check for expired session
  If DateDiff("n", CVDate(varLastUpdate), Now()) > mintSessionMins Then
    DeleteSession mguidSessionId
    Err.Raise vbObjectError + 9, mstrModName & " LoadSession", "Session expired."
    Exit Sub
  End If
  
  ' break up session field into collection, add elements
  varSessionData = rsSession.Fields("State_Variables")
  If Not IsNull(varSessionData) Then
    varSessionElements = Split(varSessionData, Chr(1))
    For i = 0 To UBound(varSessionElements) - 1 Step 2
      Set objSE = New SessionElement
      objSE.strKey = CStr(varSessionElements(i))
      objSE.strData = CStr(varSessionElements(i + 1))
      mCol.Add objSE, varSessionElements(i)
    Next i
  End If
  
  Set rsSession = Nothing

Exit Sub
LoadSession_Err:
  CtxRaiseError mstrModName, "LoadSession"
End Sub

' Saves the current sesion to disk
Private Sub SaveSession()
On Error GoTo errorHandler
  
  ' check if loaded
  If Not mblnLoaded Then
    Err.Raise vbObjectError + 4, mstrModName & " SaveSession", "Session not loaded."
    Exit Sub
  End If
  
  Dim varItm As Variant, strSessionData As String
  Dim objSE As SessionElement
  
  ' create session data string from collection
  For Each varItm In mCol
    Set objSE = varItm
    If Not objSE.blnDeleted Then
      strSessionData = strSessionData & objSE.strKey & Chr(1) & CStr(objSE.strData) & Chr(1)
    End If
  Next varItm
  
  ' save to database
  mobjDbh.RunSP "sp_Session_Save", _
      mp("Session_ID", adVarChar, 50, mguidSessionId), _
      mp("State_Variables", adVarChar, 2000, strSessionData), _
      mp("Last_Update", adVarChar, 30, Now)

Exit Sub
errorHandler:
  CtxRaiseError mstrModName, "SaveSession"
End Sub

' Gets a new GUID for a session
Public Function CreateSession() As String
On Error GoTo errorHandler
  
  Dim strSessionId As String
  
  ' create a guid and save to database
  strSessionId = CreateGUID()
  mobjDbh.RunSP "sp_Session_Create", _
    mp("Session_ID", adVarChar, 50, strSessionId), _
    mp("Last_Update", adDate, 8, Now)
  
  CreateSession = strSessionId

Exit Function
errorHandler:
  CtxRaiseError mstrModName, "CreateSession"
End Function

' Deletes a session
Public Sub DeleteSession(Optional pstrSessionId As Variant)
On Error GoTo errorHandler
  
  Dim strSessionId As String
  
  ' check which session id to use, param or mod var
  If IsMissing(pstrSessionId) Then
    If mguidSessionId = "" Then
      Exit Sub
    Else
      strSessionId = mguidSessionId
    End If
  Else
    strSessionId = pstrSessionId
  End If
  
  ' run delete sp
  mobjDbh.RunSP "sp_Session_Delete", mp("Session_Id", adVarChar, 50, strSessionId)
    
Exit Sub
errorHandler:
  CtxRaiseError mstrModName, "DeleteSession"
End Sub

' Retreive all session data
Public Property Get SessionData(pstrDelimiter As String) As String

  Dim varItm As Variant, strSessionData As String
  Dim objSE As SessionElement

  ' create session data string from collection
  For Each varItm In mCol
    Set objSE = varItm
    If Not objSE.blnDeleted Then
      strSessionData = strSessionData & objSE.strKey & "=" & CStr(objSE.strData) & pstrDelimiter
    End If
  Next varItm

  SessionData = strSessionData
  
End Property

' returns timeout value being used
Public Property Get Timeout() As Integer

  Timeout = mintSessionMins

End Property

' Removes vars from the session
Public Sub RemoveVars(pstrPrefix As String)
On Error GoTo errorHandler

  Dim varItm As Variant, objSE As SessionElement
  
  For Each varItm In mCol
    Set objSE = varItm
    If Left(objSE.strKey, Len(pstrPrefix)) = pstrPrefix Then
      objSE.blnDeleted = True
      mblnModified = True
    End If
  Next varItm
  
Exit Sub
errorHandler:
  CtxRaiseError mstrModName, "RemoveVars"
End Sub
