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
Private mstrSavedID As String
'local variable(s) to hold property value(s)
Private mvarComponentType As ComponentTypeEnum 'local copy
Private mvarRebootType As RebootTypeEnum 'local copy
Private mstrSetupMessage As String 'local copy
Private mstrSetupCommand As String 'local copy
Private mstrSetupCommandLine As String 'local copy
Private mlngSetupTime As Long 'local copy
Private mstrOSVersionMin As Single 'local copy
Private mstrOSVersionMax As Single 'local copy
Private mblnOSVersionNT As Boolean 'local copy
Private mblnOSVersion9x As Boolean 'local copy
Private mstrFileVersionCheckDLL As String 'local copy
Private mstrFileVersionCheckVersion As String 'local copy
Private mstrRegVersionCheckKey As String 'local copy
Private mstrRegVersionCheckVersion As String 'local copy
Private mstrRegKeyCheckKey As String 'local copy
Private mstrRegKeyCheckValue As String 'local copy
Private mstrNTServicePackCheckNumber As String 'local copy
Private mvarSettings As CSettingsFile 'local copy
Private mstrUniqueID As String 'local copy
Private mstrURL As String 'local copy
Private mstrDependencies As String 'local copy
Private mintsortOrder As Integer 'local copy
Private mstrNotes As String 'local copy

Public Property Let Notes(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Notes = 5
    mstrNotes = vData
End Property


Public Property Get Notes() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Notes
    Notes = mstrNotes
End Property




Public Function SetAtMaxSortOrder()

  Dim iMax As Integer, objComp As CComponent
  iMax = -1
  For Each objComp In mvarSettings.CComponents
    If iMax = -1 Then iMax = objComp.SortOrder
    If iMax < objComp.SortOrder Then
      iMax = objComp.SortOrder
    End If
  Next objComp
  
  mintsortOrder = iMax + 1

End Function

Public Property Let SortOrder(ByVal vData As Integer)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.SortOrder = 5
    mintsortOrder = vData
End Property


Public Property Get SortOrder() As Integer
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.SortOrder
    SortOrder = mintsortOrder
End Property




Public Property Let Dependencies(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.Dependencies = 5
    mstrDependencies = Trim(vData)
End Property


Public Property Get Dependencies() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Dependencies
    Dependencies = " " & mstrDependencies & " "
End Property



Public Property Let URL(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.URL = 5
    mstrURL = vData
End Property


Public Property Get URL() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.URL
    URL = mstrURL
End Property



Public Property Let UniqueID(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.UniqueID = 5
    mstrUniqueID = vData
End Property


Public Property Get UniqueID() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.UniqueID
    UniqueID = mstrUniqueID
End Property



Public Property Set Settings(ByVal vData As CSettingsFile)
'used when assigning an Object to the property, on the left side of a Set statement.
'Syntax: Set x.Settings = Form1
    Set mvarSettings = vData
End Property


Public Property Get Settings() As CSettingsFile
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.Settings
    Set Settings = mvarSettings
End Property




Public Property Let NTServicePackCheckNumber(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.NTServicePackCheckNumber = 5
    mstrNTServicePackCheckNumber = vData
End Property


Public Property Get NTServicePackCheckNumber() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.NTServicePackCheckNumber
    NTServicePackCheckNumber = mstrNTServicePackCheckNumber
End Property



Public Property Let RegKeyCheckValue(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.RegKeyCheckValue = 5
    mstrRegKeyCheckValue = vData
End Property


Public Property Get RegKeyCheckValue() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.RegKeyCheckValue
    RegKeyCheckValue = mstrRegKeyCheckValue
End Property



Public Property Let RegKeyCheckKey(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.RegKeyCheckKey = 5
    mstrRegKeyCheckKey = vData
End Property


Public Property Get RegKeyCheckKey() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.RegKeyCheckKey
    RegKeyCheckKey = mstrRegKeyCheckKey
End Property



Public Property Let RegVersionCheckVersion(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.RegVersionCheckVersion = 5
    mstrRegVersionCheckVersion = vData
End Property


Public Property Get RegVersionCheckVersion() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.RegVersionCheckVersion
    RegVersionCheckVersion = mstrRegVersionCheckVersion
End Property



Public Property Let RegVersionCheckKey(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.RegVersionCheckKey = 5
    mstrRegVersionCheckKey = vData
End Property


Public Property Get RegVersionCheckKey() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.RegVersionCheckKey
    RegVersionCheckKey = mstrRegVersionCheckKey
End Property



Public Property Let FileVersionCheckVersion(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.FileVersionCheckVersion = 5
    mstrFileVersionCheckVersion = vData
End Property


Public Property Get FileVersionCheckVersion() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.FileVersionCheckVersion
    FileVersionCheckVersion = mstrFileVersionCheckVersion
End Property



Public Property Let FileVersionCheckDLL(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.FileVersionCheckDLL = 5
    mstrFileVersionCheckDLL = vData
End Property


Public Property Get FileVersionCheckDLL() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.FileVersionCheckDLL
    FileVersionCheckDLL = mstrFileVersionCheckDLL
End Property



Public Property Let OSVersion9x(ByVal vData As Boolean)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.OSVersion9x = 5
    mblnOSVersion9x = vData
End Property


Public Property Get OSVersion9x() As Boolean
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.OSVersion9x
    OSVersion9x = mblnOSVersion9x
End Property



Public Property Let OSVersionNT(ByVal vData As Boolean)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.OSVersionNT = 5
    mblnOSVersionNT = vData
End Property


Public Property Get OSVersionNT() As Boolean
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.OSVersionNT
    OSVersionNT = mblnOSVersionNT
End Property



Public Property Let OSVersionMax(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.OSVersionMax = 5
    mstrOSVersionMax = Format(vData, "0.00")
End Property


Public Property Get OSVersionMax() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.OSVersionMax
    OSVersionMax = Format(mstrOSVersionMax, "0.00")
End Property



Public Property Let OSVersionMin(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.OSVersionMin = 5
    mstrOSVersionMin = Format(vData, "0.00")
End Property


Public Property Get OSVersionMin() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.OSVersionMin
    OSVersionMin = Format(mstrOSVersionMin, "0.00")
End Property




Public Property Let SetupTime(ByVal vData As Long)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.SetupTime = 5
    mlngSetupTime = vData
End Property


Public Property Get SetupTime() As Long
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.SetupTime
    SetupTime = mlngSetupTime
End Property



Public Property Let SetupCommandLine(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.SetupCommandLine = 5
    mstrSetupCommandLine = vData
End Property


Public Property Get SetupCommandLine() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.SetupCommandLine
    SetupCommandLine = mstrSetupCommandLine
End Property



Public Property Let SetupCommand(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.SetupCommand = 5
    mstrSetupCommand = vData
End Property


Public Property Get SetupCommand() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.SetupCommand
    SetupCommand = mstrSetupCommand
End Property



Public Property Let SetupMessage(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.WaitMessage = 5
    mstrSetupMessage = vData
End Property


Public Property Get SetupMessage() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.WaitMessage
    SetupMessage = mstrSetupMessage
End Property




Public Property Let RebootType(ByVal vData As RebootTypeEnum)
'used when assigning an Object to the property, on the left side of a Set statement.
'Syntax: Set x.RebootType = Form1
    mvarRebootType = vData
End Property


Public Property Get RebootType() As RebootTypeEnum
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.RebootType
    RebootType = mvarRebootType
End Property



Public Property Let ComponentType(ByVal vData As ComponentTypeEnum)
'used when assigning an Object to the property, on the left side of a Set statement.
'Syntax: Set x.ComponentType = Form1
    mvarComponentType = vData
End Property


Public Property Get ComponentType() As ComponentTypeEnum
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.ComponentType
    ComponentType = mvarComponentType
End Property




Public Property Let SavedID(pstrIn As String)
  mstrSavedID = pstrIn
End Property

Public Property Get SavedID() As String
  SavedID = mstrSavedID
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

' OS Version info
pobjCIni.ReadValue mstrID, "OSVersionMin", strTemp, "0"
mstrOSVersionMin = Val(strTemp)
pobjCIni.ReadValue mstrID, "OSVersionMax", strTemp, "0"
mstrOSVersionMax = Val(strTemp)
pobjCIni.ReadValue mstrID, "OSVersionNT", strTemp, "1"
If strTemp = "1" Then mblnOSVersionNT = True Else mblnOSVersionNT = False
pobjCIni.ReadValue mstrID, "OSVersion9x", strTemp, "1"
If strTemp = "1" Then mblnOSVersion9x = True Else mblnOSVersion9x = False

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

' Dependencies
pobjCIni.ReadValue mstrID, "UniqueID", mstrUniqueID
pobjCIni.ReadValue mstrID, "Dependencies", mstrDependencies

End Function

Public Function Save(ByRef pobjCIni As CIni)

Dim strTemp As String

If Len(mstrID) = 0 Then
  Err.Raise vbObjectError, mstrModname & ".Load", "No component ID specified"
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

' OS Version info
pobjCIni.WriteValue mstrID, "OSVersionMin", Format(mstrOSVersionMin, "0.00")
pobjCIni.WriteValue mstrID, "OSVersionMax", Format(mstrOSVersionMax, "0.00")
pobjCIni.WriteValue mstrID, "OSVersionNT", IIf(mblnOSVersionNT, "1", "0")
pobjCIni.WriteValue mstrID, "OSVersion9x", IIf(mblnOSVersion9x, "1", "0")

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

' Dependencies
pobjCIni.WriteValue mstrID, "UniqueID", mstrUniqueID
pobjCIni.WriteValue mstrID, "Dependencies", mstrDependencies

End Function

Private Sub Class_Initialize()
UniqueID = CStr(Timer) & "-" & CStr(Rnd * 100)
End Sub
