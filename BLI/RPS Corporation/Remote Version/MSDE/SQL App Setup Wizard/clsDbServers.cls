VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "DbServers"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' clsDbServers.cls class module defines collection of DBServer objects '
' Populated from a text file via load method.                          '
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit

Private mcolDbServers As New Collection


Public Sub Add(varDbServer As Variant)
    Dim oDbServer As New DbServer
    
    If Not IsObject(varDbServer) Or TypeName(varDbServer) <> "DbServer" Then
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="Invalid variable or object type."
    Else
        Set oDbServer = varDbServer
        mcolDbServers.Add oDbServer, CStr(oDbServer.ID)
    End If
End Sub

Public Function Count() As Variant
    Count = mcolDbServers.Count
End Function

Public Sub Remove(ByVal varId As Variant)
    mcolDbServers.Remove varId
End Sub

Public Function Item(ByVal varId As Variant) As Variant
Attribute Item.VB_UserMemId = 0
    Dim lngId As String
    lngId = CLng(varId)
    Set Item = mcolDbServers.Item(lngId)
End Function

Public Function IdExists(ByVal varId As Variant) As Variant
    Dim lngId As Long
    Dim oDbServer As DbServer
    
    lngId = CLng(varId)
    
    IdExists = False
    
    For Each oDbServer In mcolDbServers
        If oDbServer.ID = lngId Then
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
    Dim oDbServer As DbServer
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
        Set oDbServer = New DbServer
        
        strLineRead = oLoadFile.ReadLine
        
        With oDbServer
            ' Populate property values
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 1 (Double)
            .ID = CLng(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 4 (String)
            .Description = Left(strLineRead, lngPos - 1)
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 5 (Double)
            .MajorVer = CLng(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 6 (Double)
            .MinorVer = CLng(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 7 (Double)
            .BuildNum = CLng(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 8 (Double)
            .CSDVersionNum = CLng(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 9 (String)
            .SetupExeFileSpec = Left(strLineRead, lngPos - 1)
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 10 (String)
            .SetupParms = Left(strLineRead, lngPos - 1)
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 11 (String)
            .SetupFile = Left(strLineRead, lngPos - 1)
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 12 (String)
            .SetupIniData = Left(strLineRead, lngPos - 1)
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 13 (String)
            .ServerFileSpec = Left(strLineRead, lngPos - 1)
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 14 (Double)
            .ServerSizeBytes = CDbl(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 15 (Double)
            .ProgramBytes = CDbl(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 16 (Double)
            .DataBytes = CDbl(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 17 (Double)
            .SysBytes = CDbl(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            lngPos = InStr(1, strLineRead, vbTab)
            ' Field 18 (Double)
            .TempBytes = CDbl(Left(strLineRead, lngPos - 1))
            
            strLineRead = Mid(strLineRead, lngPos + 1, Len(strLineRead))
            ' Field 19 (Double)
            .DbLimitBytes = CDbl(strLineRead)
            
            ' Append object to collection
            mcolDbServers.Add oDbServer, CStr(.ID)
        End With
        Set oDbServer = Nothing
    Loop
    
    oLoadFile.Close
    
    Set oDbServer = Nothing
    Set oLoadFile = Nothing
    Set oFileSystem = Nothing
    
    On Error GoTo 0
    Exit Sub
LoadError:
    Err.Clear
    Set mcolDbServers = Nothing
    Set oDbServer = Nothing
    Set oLoadFile = Nothing
    Set oFileSystem = Nothing
    
    On Error GoTo 0
    Exit Sub
End Sub


Public Function NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
   Set NewEnum = mcolDbServers.[_NewEnum]
End Function


