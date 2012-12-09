VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 1  'NoTransaction
END
Attribute VB_Name = "Version"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Const mstrModName = "GOCom.Version"

Public Function ConnectionString() As String
On Error GoTo errorHandler
    
  Dim objDbh As DBHelper
  Set objDbh = CreateObject("GOCom.DBHelper")
  ConnectionString = objDbh.GetConnectionString()
  Set objDbh = Nothing
  
Exit Function
errorHandler:
  CtxRaiseError mstrModName, "ConnectionString"
End Function

Public Function Version() As String
  
  Version = GetVersionNumber()

End Function

Public Function ComputerName() As String
    
  ComputerName = GetComputerName()

End Function





