VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "OSSuites"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' clsOsSuites.cls class module defines collection of OSSuite objects   '
' Populated from a text file via load method.                          '
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit

Private mcolOSSuites As New Collection



Public Sub Add(varOSSuite As Variant)
    Dim oOSSuite As New OSSuite
    
    If Not IsObject(varOSSuite) Or TypeName(varOSSuite) <> "OSSuite" Then
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="Invalid variable or object type."
    Else
        Set oOSSuite = varOSSuite
        mcolOSSuites.Add oOSSuite, CStr(oOSSuite.ID)
    End If
End Sub

Public Function Count() As Variant
    Count = mcolOSSuites.Count
End Function

Public Sub Remove(ByVal varId As Variant)
    mcolOSSuites.Remove varId
End Sub

Public Function Item(ByVal varId As Variant) As Variant
Attribute Item.VB_UserMemId = 0
    Dim lngId As String
    lngId = CLng(varId)
    Set Item = mcolOSSuites.Item(lngId)
End Function

Public Function IdExists(ByVal varId As Variant) As Variant
    Dim lngId As Long
    Dim oOSSuite As OSSuite
    
    lngId = CLng(varId)
    
    IdExists = False
    
    For Each oOSSuite In mcolOSSuites
        If oOSSuite.ID = lngId Then
            IdExists = True
            Exit For
        End If
    Next
End Function

Public Sub Load(ByVal varLoadFileSpec As Variant)
    Dim strFileSpec
    Dim oFileSystem As Object
    Dim oLoadFile As Object
    Dim strLineRead As String
    Dim oOSSuite As OSSuite
    Dim lngPos As Long
    
    ' Loads collection from tab delimited text file
    strFileSpec = CStr(varLoadFileSpec)
    
    On Error GoTo LoadError
    
    Set oFileSystem = CreateObject("Scripting.FileSystemObject")
    
    If Not oFileSystem.FileExists(strFileSpec) Then
        GoTo LoadError
    End If
    
    Set oLoadFile = oFileSystem.OpenTextFile(strFileSpec, FORREADING, False, TristateFalse)
    
    Do While Not oLoadFile.AtEndOfStream
        Set oOSSuite = New OSSuite
        
        strLineRead = oLoadFile.ReadLine
        
        With oOSSuite
            ' Populate property values
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 1 (Long)
            .ID = CLng(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            ' Field 2 (String)
            .SuiteName = strLineRead
            
            ' Append object to collection
            mcolOSSuites.Add oOSSuite, CStr(.ID)
        End With
        Set oOSSuite = Nothing
    Loop
    
    oLoadFile.Close
    
    Set oOSSuite = Nothing
    Set oLoadFile = Nothing
    Set oFileSystem = Nothing
    
    On Error GoTo 0
    Exit Sub
LoadError:
    Err.Clear
    Set mcolOSSuites = Nothing
    Set oOSSuite = Nothing
    Set oLoadFile = Nothing
    Set oFileSystem = Nothing
    
    On Error GoTo 0
    Exit Sub
End Sub

Public Function NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
   Set NewEnum = mcolOSSuites.[_NewEnum]
End Function




