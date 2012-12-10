VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CComponent"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit

Private Const mstrModname = "CComponent"

Public Enum ComponentTypeEnum
  FileVersionCheck = 0
  RegVersionCheck
  RegKeyCheck
  NTServicePackCheck
  NoCheck
  NetFrameworkCheck
End Enum

Public Enum RebootTypeEnum
  NoReboot = 0
  BatchReboot
  ImmediateReboot
End Enum

Private mstrID As String
Private mstrName As String
Private mblnEnabled As Boolean
Private mblnDelete As Boolean

Private mvarComponentType As ComponentTypeEnum 'local copy
Private mvarRebootType As RebootTypeEnum 'local copy
Private mstrSetupMessage As String 'local copy
Private mstrSetupCommand As String 'local copy
Private mstrSetupCommandLine As String 'local copy
Private mlngSetupTime As Long 'local copy

Public mblnWinNT As Boolean 'local copy
Public mblnWin9x As Boolean 'local copy
Public mblnWindows95 As Boolean
Public mblnWindows98 As Boolean
Public mblnWindowsMe As Boolean
Public mstrNTMinVersion As Single 'local copy
Public mstrNTMaxVersion As Single 'local copy
Public mstrNTMinServicePack As String
Public mstrNTMaxServicePack As String

Private mstrFileVersionCheckDLL As String 'local copy
Private mstrFileVersionCheckVersion As String 'local copy
Private mstrRegVersionCheckKey As String 'local copy
Private mstrRegVersionCheckVersion As String 'local copy
Private mstrRegKeyCheckKey As String 'local copy
Private mstrRegKeyCheckValue As String 'local copy
Private mstrNTServicePackCheckNumber As String 'local copy
Private mvarSettings As CSettingsFile 'local copy

Private mstrNetFrameworkVersion As String

Private mstrURL As String 'local copy
Private mstrDependencies As String 'local copy
Private mintSortOrder As Integer 'local copy
Private mstrNotes As String 'local copy

Public Property Let Notes(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Notes = 5
    mstrNotes = Replace(vData, vbCrLf, "|||")
End Property


Public Property Get Notes() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Notes
    Notes = Replace(mstrNotes, "|||", vbCrLf)
End Property


Public Property Let NetFrameworkVersion(ByVal vData As String)
  mstrNetFrameworkVersion = vData
End Property

Public Property Get NetFrameworkVersion() As String
  NetFrameworkVersion = mstrNetFrameworkVersion
End Property


Public Function SetAtMaxSortOrder()

  Dim iMax As Integer, objComp As CComponent
  iMax = -1
  For Each objComp In mvarSettings.Components
    If iMax = -1 Then iMax = objComp.SortOrder
    If iMax < objComp.SortOrder Then
      iMax = objComp.SortOrder
    End If
  Next objComp
  
  mintSortOrder = iMax + 1

End Function

Public Property Let SortOrder(ByVal vData As Integer)
  mintSortOrder = vData
End Property

Public Property Get SortOrder() As Integer
  SortOrder = mintSortOrder
End Property

Public Property Let Dependencies(ByVal vData As String)
  mstrDependencies = Trim(vData)
End Property

Public Property Get Dependencies() As String
  Dependencies = " " & mstrDependencies & " "
End Property

Public Property Let URL(ByVal vData As String)
  mstrURL = vData
End Property

Public Property Get URL() As String
  URL = mstrURL
End Property

Public Property Set Settings(ByVal vData As CSettingsFile)
  Set mvarSettings = vData
End Property

Public Property Get Settings() As CSettingsFile
  Set Settings = mvarSettings
End Property

Public Property Let NTServicePackCheckNumber(ByVal vData As String)
  mstrNTServicePackCheckNumber = vData
End Property

Public Property Get NTServicePackCheckNumber() As String
  NTServicePackCheckNumber = mstrNTServicePackCheckNumber
End Property

Public Property Let RegKeyCheckValue(ByVal vData As String)
  mstrRegKeyCheckValue = vData
End Property

Public Property Get RegKeyCheckValue() As String
  RegKeyCheckValue = mstrRegKeyCheckValue
End Property

Public Property Let RegKeyCheckKey(ByVal vData As String)
  mstrRegKeyCheckKey = vData
End Property

Public Property Get RegKeyCheckKey() As String
  RegKeyCheckKey = mstrRegKeyCheckKey
End Property

Public Property Let RegVersionCheckVersion(ByVal vData As String)
  mstrRegVersionCheckVersion = vData
End Property

Public Property Get RegVersionCheckVersion() As String
  RegVersionCheckVersion = mstrRegVersionCheckVersion
End Property

Public Property Let RegVersionCheckKey(ByVal vData As String)
  mstrRegVersionCheckKey = vData
End Property

Public Property Get RegVersionCheckKey() As String
  RegVersionCheckKey = mstrRegVersionCheckKey
End Property

Public Property Let FileVersionCheckVersion(ByVal vData As String)
  mstrFileVersionCheckVersion = vData
End Property

Public Property Get FileVersionCheckVersion() As String
  FileVersionCheckVersion = mstrFileVersionCheckVersion
End Property

Public Property Let FileVersionCheckDLL(ByVal vData As String)
  mstrFileVersionCheckDLL = vData
End Property

Public Property Get FileVersionCheckDLL() As String
  FileVersionCheckDLL = mstrFileVersionCheckDLL
End Property

Public Property Let SetupTime(ByVal vData As Long)
  mlngSetupTime = vData
End Property

Public Property Get SetupTime() As Long
  SetupTime = mlngSetupTime
End Property

Public Property Let SetupCommandLine(ByVal vData As String)
  mstrSetupCommandLine = vData
End Property

Public Property Get SetupCommandLine() As String
  SetupCommandLine = mstrSetupCommandLine
End Property

Public Property Let SetupCommand(ByVal vData As String)
  mstrSetupCommand = vData
End Property

Public Property Get SetupCommand() As String
  SetupCommand = mstrSetupCommand
End Property

Public Property Let SetupMessage(ByVal vData As String)
  mstrSetupMessage = vData
End Property

Public Property Get SetupMessage() As String
  SetupMessage = mstrSetupMessage
End Property

Public Property Let RebootType(ByVal vData As RebootTypeEnum)
  mvarRebootType = vData
End Property

Public Property Get RebootType() As RebootTypeEnum
  RebootType = mvarRebootType
End Property

Public Property Let ComponentType(ByVal vData As ComponentTypeEnum)
  mvarComponentType = vData
End Property

Public Property Get ComponentType() As ComponentTypeEnum
  ComponentType = mvarComponentType
End Property

Public Property Let ID(pstrIn As String)
  mstrID = pstrIn
End Property

Public Property Get ID() As String
  ID = mstrID
End Property

Public Property Let Name(pstrIn As String)
  mstrName = pstrIn
End Property

Public Property Get Name() As String
  Name = mstrName
End Property

Public Property Let Enabled(pblnIn As Boolean)
  mblnEnabled = pblnIn
End Property

Public Property Get Enabled() As Boolean
  Enabled = mblnEnabled
End Property

Public Property Let Delete(pblnIn As Boolean)
  mblnDelete = pblnIn
End Property

Public Property Get Delete() As Boolean
  Delete = mblnDelete
End Property

Public Function Init()

  mstrID = "Comp-" & CStr(Timer) & "-" & CStr(Rnd * 100)
  mstrName = "<new button>"
  mblnEnabled = True

End Function


Public Function Load(ByRef pobjCIni As CIni)

  Dim strTemp As String
  
  If Len(mstrID) = 0 Then
    Err.Raise vbObjectError, mstrModname & ".Load", "No component ID specified"
    Exit Function
  End If
  
  ' check if enabled
  pobjCIni.ReadValue "Components", mstrID, strTemp, "1"
  If Val(strTemp) = "1" Then mblnEnabled = True
  
  pobjCIni.ReadValue mstrID, "Name", mstrName
  
  ' Component general information
  pobjCIni.ReadValue mstrID, "ComponentType", strTemp, "0"
  mvarComponentType = Val(strTemp)
  pobjCIni.ReadValue mstrID, "RebootType", strTemp, "0"
  mvarRebootType = Val(strTemp)
  pobjCIni.ReadValue mstrID, "SetupCommand", mstrSetupCommand, ""
  pobjCIni.ReadValue mstrID, "SetupCommandLine", mstrSetupCommandLine, ""
  pobjCIni.ReadValue mstrID, "SetupMessage", mstrSetupMessage, ""
  pobjCIni.ReadValue mstrID, "SetupTime", strTemp, "30"
  mlngSetupTime = Val(strTemp)
  pobjCIni.ReadValue mstrID, "URL", mstrURL
  pobjCIni.ReadValue mstrID, "Notes", mstrNotes
  
  pobjCIni.ReadBool mstrID, "Win9x", mblnWin9x, True
  If mblnWin9x Then
    pobjCIni.ReadBool mstrID, "Windows95", mblnWindows95, True
    pobjCIni.ReadBool mstrID, "Windows98", mblnWindows98, True
    pobjCIni.ReadBool mstrID, "WindowsMe", mblnWindowsMe, True
  End If
 
  pobjCIni.ReadBool mstrID, "WinNT", mblnWinNT, True
  If mblnWinNT Then
    pobjCIni.ReadSngValue mstrID, "NTMinVersion", mstrNTMinVersion, 0
    pobjCIni.ReadSngValue mstrID, "NTMaxVersion", mstrNTMaxVersion, 0
    pobjCIni.ReadValue mstrID, "NTMinServicePack", mstrNTMinServicePack, ""
    pobjCIni.ReadValue mstrID, "NTMaxServicePack", mstrNTMaxServicePack, ""
  End If
  
  ' File Version check info
  pobjCIni.ReadValue mstrID, "FileVersionCheckDLL", mstrFileVersionCheckDLL
  pobjCIni.ReadValue mstrID, "FileVersionCheckVersion", mstrFileVersionCheckVersion
  
  ' Registry Version check info
  pobjCIni.ReadValue mstrID, "RegVersionCheckKey", mstrRegVersionCheckKey
  pobjCIni.ReadValue mstrID, "RegVersionCheckVersion", mstrRegVersionCheckVersion
  
  ' Registry key check
  pobjCIni.ReadValue mstrID, "RegKeyCheckKey", mstrRegKeyCheckKey
  pobjCIni.ReadValue mstrID, "RegKeyCheckValue", mstrRegKeyCheckValue
  
  ' NT Service Pack check
  pobjCIni.ReadValue mstrID, "NTServicePackCheckNumber", mstrNTServicePackCheckNumber
  
  ' Net Framework check
  pobjCIni.ReadValue mstrID, "NetFrameworkCheckVersion", mstrNetFrameworkVersion
  
  ' Dependencies
  pobjCIni.ReadValue mstrID, "Dependencies", mstrDependencies

End Function

Public Function Save(ByRef pobjCIni As CIni)

  Dim strTemp As String
  
  If Len(mstrID) = 0 Then
    Err.Raise vbObjectError, mstrModname & ".Save", "No component ID specified"
    Exit Function
  End If
  
  pobjCIni.WriteValue mstrID, "Name", mstrName
  
  ' Component general information
  pobjCIni.WriteValue mstrID, "ComponentType", CStr(mvarComponentType)
  pobjCIni.WriteValue mstrID, "RebootType", CStr(mvarRebootType)
  pobjCIni.WriteValue mstrID, "SetupCommand", mstrSetupCommand
  pobjCIni.WriteValue mstrID, "SetupCommandLine", mstrSetupCommandLine
  pobjCIni.WriteValue mstrID, "SetupMessage", mstrSetupMessage
  pobjCIni.WriteValue mstrID, "SetupTime", CStr(mlngSetupTime)
  pobjCIni.WriteValue mstrID, "URL", mstrURL
  pobjCIni.WriteValue mstrID, "Notes", mstrNotes
  
  pobjCIni.WriteBool mstrID, "Win9x", mblnWin9x
  If mblnWin9x Then
    pobjCIni.WriteBool mstrID, "Windows95", mblnWindows95
    pobjCIni.WriteBool mstrID, "Windows98", mblnWindows98
    pobjCIni.WriteBool mstrID, "WindowsMe", mblnWindowsMe
  End If
 
  pobjCIni.WriteBool mstrID, "WinNT", mblnWinNT
  If mblnWinNT Then
    pobjCIni.WriteValue mstrID, "NTMinVersion", Format(mstrNTMinVersion, "0.00")
    pobjCIni.WriteValue mstrID, "NTMaxVersion", Format(mstrNTMaxVersion, "0.00")
    pobjCIni.WriteValue mstrID, "NTMinServicePack", mstrNTMinServicePack
    pobjCIni.WriteValue mstrID, "NTMaxServicePack", mstrNTMaxServicePack
  End If
 
  ' File Version check info
  pobjCIni.WriteValue mstrID, "FileVersionCheckDLL", mstrFileVersionCheckDLL
  pobjCIni.WriteValue mstrID, "FileVersionCheckVersion", mstrFileVersionCheckVersion
  
  ' Registry Version check info
  pobjCIni.WriteValue mstrID, "RegVersionCheckKey", mstrRegVersionCheckKey
  pobjCIni.WriteValue mstrID, "RegVersionCheckVersion", mstrRegVersionCheckVersion
  
  ' Registry key check
  pobjCIni.WriteValue mstrID, "RegKeyCheckKey", mstrRegKeyCheckKey
  pobjCIni.WriteValue mstrID, "RegKeyCheckValue", mstrRegKeyCheckValue
  
  ' NT Service Pack check
  pobjCIni.WriteValue mstrID, "NTServicePackCheckNumber", mstrNTServicePackCheckNumber
  
  ' Net framework check
  pobjCIni.WriteValue mstrID, "NetFrameworkCheckVersion", mstrNetFrameworkVersion
  
  ' Dependencies
  pobjCIni.WriteValue mstrID, "Dependencies", mstrDependencies

End Function
