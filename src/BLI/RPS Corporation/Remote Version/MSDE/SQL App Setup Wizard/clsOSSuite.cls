VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "OSSuite"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' clsOsSuite.cls class module provides data structure for storing information about  '
' different types of operating system suites handled by the installer.               '
' Defines object of type OSSuite used via OSuites collection.                        '
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit

'local variable(s) to hold property value(s)
Private mlngID As Long              'ID Associated with OS Suite.  Consistent with dwSuiteMask declarations.
Private mstrSuiteName As String    'Description of Suite

Public Property Let SuiteName(ByVal vData As Variant)
    mstrSuiteName = CStr(vData)
End Property

Public Property Get SuiteName() As Variant
    SuiteName = mstrSuiteName
End Property

Public Property Let ID(ByVal vData As Variant)
    Static blnAlreadySet As Boolean
    
    If Not blnAlreadySet Then
        If IsNumeric(vData) Then
            blnAlreadySet = True
            mlngID = CLng(vData)
        Else
            Err.Raise _
                Number:=vbObjectError + 32112, _
                Description:="ID Property Must Be Numeric"
        End If
    End If
End Property


Public Property Get ID() As Variant
Attribute ID.VB_UserMemId = 0
    ID = mlngID
End Property



