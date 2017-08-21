VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 3  'UsesTransaction
END
Attribute VB_Name = "DBHelper"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

' Author: Mike Giesler
' Project: giesler.org Common Components
' Creation Date: Sept 21, 2000
' Purpose:
'       This class provides database support to all SAMI components.  It provides
'       connections, stored procedure execution, and SQL statement execution
'
' Change History:
' ID    Date        Method              Change Description

Private Const mstrModName As String = "GOCom.DBHelper"
Private mstrDataSource As String
Private mstrInitialCatalog As String
Private mstrProvider As String
Private mstrUserId As String
Private mstrPassword As String
Private mintConnectTimeout As Integer
Private mintCommandTimeout As Integer

Implements IObjectConstruct
Dim mstrDBServer As String
Dim mstrConnectionString As String

' Sets the connection string based on setting in activation
Private Sub IObjectConstruct_Construct(ByVal pobjConstruct As Object)
'eval construction string and find out what the admin wants us to do
' first character is 'split' character (ie, ',')
' can be in form: ,ConnectionString=<full connection string>
' or in form:     ,Server=<server name>
' or any comb of params:  ,Server=<server name>,Data Source=<ds>
  
  Dim arrParams, x As Integer, strConstructString As String
  Dim strSplit As String
  
  strConstructString = pobjConstruct.ConstructString
  If Len(pobjConstruct.ConstructString) = 0 Then Exit Sub
  strSplit = Left(strConstructString, 1)
  
  arrParams = Split(strConstructString, strSplit)
  For x = LBound(arrParams) To UBound(arrParams)
    Select Case UCase(GetKey(arrParams(x)))
      Case UCase("ConnectionString")
        mstrConnectionString = GetValue(arrParams(x))
      Case UCase("Data Source")
        mstrDataSource = GetValue(arrParams(x))
      Case UCase("Initial Catalog")
        mstrInitialCatalog = GetValue(arrParams(x))
      Case UCase("Provider")
        mstrProvider = GetValue(arrParams(x))
      Case UCase("User Id")
        mstrUserId = GetValue(arrParams(x))
      Case UCase("Password")
        mstrPassword = GetValue(arrParams(x))
      Case UCase("Connect Timeout")
        If IsNumeric(GetValue(arrParams(x))) Then
          mintConnectTimeout = Int(Val(GetValue(arrParams(x))))
        End If
      Case UCase("Command Timeout")
        If IsNumeric(GetValue(arrParams(x))) Then
          mintCommandTimeout = Int(Val(GetValue(arrParams(x))))
        End If
      End Select
  Next x
     
End Sub

' Returns connection string
Public Function GetConnectionString() As String
On Error GoTo errorHandler
  
  Dim strTmp As String
  
  If mstrConnectionString <> "" Then
    strTmp = mstrConnectionString
  Else
    If mstrDataSource = "" Then mstrDataSource = "giesler.org"
    If mstrInitialCatalog = "" Then mstrInitialCatalog = "webdb"
    If mstrProvider = "" Then mstrProvider = "SQLOLEDB"
    If mstrUserId = "" Then mstrUserId = "complus"
    If mstrPassword = "" Then mstrPassword = "blaheek3"
    If mintConnectTimeout = 0 Then mintConnectTimeout = 5
    If mintCommandTimeout = 0 Then mintCommandTimeout = 240
    
    'build connection string
    strTmp = "Provider=" + mstrProvider + ";"
    strTmp = strTmp + "Initial Catalog=" + mstrInitialCatalog + ";"
    strTmp = strTmp + "Data Source=" + mstrDataSource + ";"
    strTmp = strTmp + "User Id=" + mstrUserId + ";"
    strTmp = strTmp + "Password=" + mstrPassword + ";"
    strTmp = strTmp + "Connect Timeout=" + CStr(mintConnectTimeout) + ";"
    strTmp = strTmp + "Command Timeout=" + CStr(mintCommandTimeout)
  End If
  
  GetConnectionString = strTmp

Exit Function
errorHandler:
RaiseError mstrModName, "GetConnectionString"
Exit Function
End Function

' Runs a stored proc, and returns a recordset
Function RunSPReturnRS(ByVal pstrSP As String, ParamArray pvarParams() As Variant) As ADODB.Recordset
On Error GoTo errorHandler
      
  ' Create the ADO objects
  Dim rs As ADODB.Recordset, cmd As ADODB.Command
  Set rs = New ADODB.Recordset
  Set cmd = New ADODB.Command
      
  ' Init the ADO objects  & the stored proc parameters
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSP
  cmd.CommandType = adCmdStoredProc
      
  collectParams cmd, pvarParams
      
  ' Execute the query for readonly
  rs.CursorLocation = adUseClient
  rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
      
  ' Disconnect the recordset
  Set cmd.ActiveConnection = Nothing
  Set cmd = Nothing
  Set rs.ActiveConnection = Nothing
  
  ' Return the resultant recordset
  Set RunSPReturnRS = rs

Exit Function
errorHandler:
Set rs = Nothing
Set cmd = Nothing
RaiseError mstrModName, "RunSPReturnRS(" & pstrSP & ", ...)"
End Function

' RunSQLReturnRS differs from RunSPReturnRS only on the line that
' sets the cmd.CommandType = adCmdText vs. adCmdStoredProc
Function RunSQLReturnRS(ByVal pstrSP As String, ParamArray pvarParams() As Variant) As ADODB.Recordset
On Error GoTo errorHandler
      
  ' Set up Command and Connection objects
  Dim rs As ADODB.Recordset, cmd As ADODB.Command
  Set rs = New ADODB.Recordset
  Set cmd = New ADODB.Command
  
  'Run the procedure
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSP
  cmd.CommandType = adCmdText
      
  collectParams cmd, pvarParams
      
  rs.CursorLocation = adUseClient
  rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
      
  ' Disconnect the recordsets and cleanup
  Set cmd.ActiveConnection = Nothing
  Set cmd = Nothing
  Set rs.ActiveConnection = Nothing
      
  Set RunSQLReturnRS = rs

Exit Function
errorHandler:
Set rs = Nothing
Set cmd = Nothing
RaiseError mstrModName, "RunSQLReturnRS(" & pstrSP & ", ...)"
End Function

' Runs a stored procedure returning a read-write recordset
Function RunSPReturnRS_RW(ByVal pstrSP As String, ParamArray pvarParams() As Variant) As ADODB.Recordset
On Error GoTo errorHandler
      
  ' Create the ADO objects
  Dim rs As ADODB.Recordset, cmd As ADODB.Command
  Set rs = New ADODB.Recordset
  Set cmd = New ADODB.Command
      
  ' Init the ADO objects & the stored proc parameters
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSP
  cmd.CommandType = adCmdStoredProc
  collectParams cmd, pvarParams
  rs.CursorLocation = adUseClient
      
  ' Execute the query as an updatable recordset and stay connected
  rs.Open cmd, , adOpenDynamic, adLockBatchOptimistic
  Set cmd = Nothing
      
  ' Return and quit
  Set RunSPReturnRS_RW = rs

Exit Function
errorHandler:
Set rs = Nothing
Set cmd = Nothing
RaiseError mstrModName, "RunSPReturnRS_RW(" & pstrSP & ", ...)"
End Function

' Runs the specified SQL and returns a read-write recordset
Function RunSQLReturnRS_RW(ByVal pstrSQL As String, ParamArray pvarParams() As Variant) As ADODB.Recordset
On Error GoTo errorHandler
      
  ' Create the ADO objects
  Dim rs As ADODB.Recordset, cmd As ADODB.Command
  Set rs = New ADODB.Recordset
  Set cmd = New ADODB.Command
         
  ' Init the ADO objects & the stored proc parameters
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSQL
  cmd.CommandType = adCmdText
  collectParams cmd, pvarParams
  rs.CursorLocation = adUseClient
      
  ' Execute the query as an updatable recordset and stay connected
  rs.Open cmd, , adOpenDynamic, adLockBatchOptimistic
  Set cmd = Nothing
      
  ' Return and quit
  Set RunSQLReturnRS_RW = rs

Exit Function
errorHandler:
Set rs = Nothing
Set cmd = Nothing
RaiseError mstrModName, "RunSQLReturnRS_RW(" & pstrSQL & ", ...)"
End Function

' Run a stored proc
Function RunSP(ByVal pstrSP As String, ParamArray pvarParams() As Variant)
On Error GoTo errorHandler
  
  ' Create the ADO objects
  Dim cmd As ADODB.Command
  Set cmd = New ADODB.Command
      
  ' Init the ADO objects & the stored proc parameters
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSP
  cmd.CommandType = adCmdStoredProc
  collectParams cmd, pvarParams
      
  ' Execute the query without returning a recordset
  cmd.Execute , , adExecuteNoRecords
      
  ' Disconnect the recordset and clean up
  Set cmd.ActiveConnection = Nothing
  Set cmd = Nothing
    
Exit Function
errorHandler:
Set cmd = Nothing
RaiseError mstrModName, "RunSP(" & pstrSP & ", ...)"
End Function

' Run the SQL passed
Function RunSQL(ByVal pstrSQL As String, ParamArray pvarParams() As Variant)
On Error GoTo errorHandler
            
  ' Create the ADO objects
  Dim cmd As ADODB.Command
  Set cmd = New ADODB.Command
  
  ' Init the ADO objects & the stored proc parameters
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSQL
  cmd.CommandType = adCmdText
  collectParams cmd, pvarParams
      
  ' Execute the query without returning a recordset
  cmd.Execute , , adExecuteNoRecords
      
  ' Cleanup
  Set cmd.ActiveConnection = Nothing
  Set cmd = Nothing
  
Exit Function
errorHandler:
Set cmd = Nothing
RaiseError mstrModName, "RunSQL(" & pstrSQL & ", ...)"
End Function

' Run a stored proc that returns an int as last param
Function RunSPReturnInteger(ByVal pstrSP As String, ParamArray pvarParams() As Variant) As Long 'adInterger is really a VB Long
On Error GoTo errorHandler
      
  ' Create the ADO objects
  Dim cmd As ADODB.Command
  Set cmd = New ADODB.Command
      
  ' Init the ADO objects & the stored proc parameters
  cmd.ActiveConnection = GetConnectionString()
  cmd.CommandTimeout = mintCommandTimeout
  cmd.CommandText = pstrSP
  cmd.CommandType = adCmdStoredProc
  collectParams cmd, pvarParams
      
  ' Assume the last parameter is outgoing
  cmd.Parameters.Append cmd.CreateParameter("@retval", adInteger, adParamOutput, 4)
      
  ' Execute without a resulting recordset and pull out the "return value" parameter
  cmd.Execute ' , , adExecuteNoRecords
  RunSPReturnInteger = cmd.Parameters("@retval").Value
      
  ' Disconnect the recordset, and clean up
  Set cmd.ActiveConnection = Nothing
  Set cmd = Nothing
      
Exit Function
errorHandler:
Set cmd = Nothing
RaiseError mstrModName, "RunSPReturnInteger(" & pstrSP & ", ...)"
End Function

' Creates paramaters for the passed in array
Private Sub collectParams(ByRef cmd As ADODB.Command, ParamArray pvarParams() As Variant)
  
  Dim params As Variant, v As Variant
  Dim i As Integer, l As Integer, u As Integer
      
  params = pvarParams(0)
  For i = LBound(params) To UBound(params)
    l = LBound(params(i))
    u = UBound(params(i))
    ' Check for nulls.
    If u - l = 3 Then
      If VarType(params(i)(3)) = vbString Then
        v = IIf(params(i)(3) = "", Null, params(i)(3))
      Else
        v = params(i)(3)
      End If
      cmd.Parameters.Append cmd.CreateParameter(params(i)(0), params(i)(1), adParamInput, params(i)(2), v)
    Else
      CtxRaiseError mstrModName, "collectParams(...): incorrect # of parameters"
    End If
  Next i

End Sub

Public Function GetIdentifier(ByRef pstrTableName As String, ByRef pstrColumnName As String, _
                                ByRef pstrColumnValue As String) As Long
On Error GoTo errorHandler
                                
    Dim strQuery As String
    Dim rsTemp As ADODB.Recordset
    
    pstrTableName = Replace(pstrTableName, "'", "''")
    pstrColumnName = Replace(pstrColumnName, "'", "''")
    pstrColumnValue = Replace(pstrColumnValue, "'", "''")
    
    strQuery = " SELECT * from " & pstrTableName & _
                " WHERE " & pstrColumnName & " = '" & pstrColumnValue & "' "

    Set rsTemp = Me.RunSQLReturnRS(strQuery)
    
    If Not rsTemp.BOF And Not rsTemp.EOF Then
        GetIdentifier = rsTemp.Fields.Item(0)
    Else
        GetIdentifier = -1
    End If

    
Exit Function
errorHandler:
RaiseError mstrModName, "GetIdentifier"
End Function

