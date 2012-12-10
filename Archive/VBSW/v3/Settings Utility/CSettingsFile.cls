VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CSettingsFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Attribute VB_Ext_KEY = "Member0" ,"CComponents"
Option Explicit

Private mstrRootPath As String
Private mlngRootPathIDList As Long
Private mstrProgramName As String
Private mstrSplash As String
Private mblnIsDirty As Boolean
Private mobjIni As CIni
Private mvarComponents As CComponents
Private mvarButtons As CButtons
Private mblnSingleInstance As Boolean
Public mstrSingleInstanceError As String
Private mstrAbortMutex As String
Private mblnHideTitleBar As Boolean
Private mblnEnableLogging As Boolean

Private mstrFilename As String

Public mstrConfirmSetupText As String

Public mblnWinNT As Boolean 'local copy
Public mblnWin9x As Boolean 'local copy
Public mblnWindows95 As Boolean
Public mblnWindows98 As Boolean
Public mblnWindowsMe As Boolean
Public mstrNTMinVersion As Single 'local copy
Public mstrNTMaxVersion As Single 'local copy
Public mstrNTMinServicePack As String
Public mstrNTMaxServicePack As String
Public mstrInvalidOSMessage As String

Public mstrOnOpenSound As String
Public mstrOnCloseSound As String

Public Enum DisplayTypeEnum
  NormalDisplay = 0
  SkipSplashDisplay
  ProgramNameDisplay
End Enum
Private mintDisplayType As DisplayTypeEnum
Private mstrSkipProgramName As String

Public Enum RebootPromptTypeEnum
  TimerReboot = 0
  ManualReboot
End Enum
Private mintRebootPromptType As RebootPromptTypeEnum
Private mintRebootPromptSeconds As Integer

Public Property Get Filename() As String
  Filename = mstrFilename
End Property

Public Property Let Filename(vData As String)
  mstrFilename = vData
End Property

Public Property Get RebootPromptType() As RebootPromptTypeEnum
  RebootPromptType = mintRebootPromptType
End Property

Public Property Let RebootPromptType(vData As RebootPromptTypeEnum)
  mintRebootPromptType = vData
End Property

Public Property Get RebootPromptSeconds() As Integer
  RebootPromptSeconds = mintRebootPromptSeconds
End Property

Public Property Let RebootPromptSeconds(vData As Integer)
  mintRebootPromptSeconds = vData
End Property

Public Property Get HideTitleBar() As Boolean
  HideTitleBar = mblnHideTitleBar
End Property

Public Property Let HideTitleBar(vData As Boolean)
  mblnHideTitleBar = vData
End Property

Public Property Get DisplayType() As DisplayTypeEnum
  DisplayType = mintDisplayType
End Property

Public Property Let DisplayType(vData As DisplayTypeEnum)
  mintDisplayType = vData
End Property

Public Property Get SkipProgramName() As String
  SkipProgramName = mstrSkipProgramName
End Property

Public Property Let SkipProgramName(vData As String)
  mstrSkipProgramName = vData
End Property

Public Property Get Components() As CComponents
    If mvarComponents Is Nothing Then
        Set mvarComponents = New CComponents
    End If

    Set Components = mvarComponents
End Property

Public Property Set Components(vData As CComponents)
    Set mvarComponents = vData
End Property

Public Property Get Buttons() As CButtons
  If mvarButtons Is Nothing Then
    Set mvarButtons = New CButtons
  End If
  Set Buttons = mvarButtons
End Property

Public Property Set Buttons(vData As CButtons)
  Set mvarButtons = vData
End Property

Private Sub Class_Initialize()
  mblnIsDirty = False
  Set mobjIni = New CIni
End Sub

Private Sub Class_Terminate()
  Set mvarComponents = Nothing
  Set mvarButtons = Nothing
  Set mobjIni = Nothing
End Sub

Public Property Let ProgramName(pstrIn As String)
  mstrProgramName = pstrIn
  mblnIsDirty = True
End Property

Public Property Get ProgramName() As String
  ProgramName = mstrProgramName
End Property

Public Property Let Splash(pstrIn As String)
  mstrSplash = pstrIn
  mblnIsDirty = True
End Property

Public Property Get Splash() As String
  Splash = mstrSplash
End Property

Public Property Get RootPathIDList() As Long
  RootPathIDList = mlngRootPathIDList
End Property

Public Property Get RootPath() As String
  RootPath = mstrRootPath
End Property

Public Property Let SingleInstance(vData As Boolean)
  mblnSingleInstance = vData
End Property

Public Property Get SingleInstance() As Boolean
  SingleInstance = mblnSingleInstance
End Property

Public Property Let EnableLogging(vData As Boolean)
  mblnEnableLogging = vData
End Property

Public Property Get EnableLogging() As Boolean
  EnableLogging = mblnEnableLogging
End Property


Public Property Let AbortMutex(vData As String)
  mstrAbortMutex = vData
End Property

Public Property Get AbortMutex() As String
  AbortMutex = mstrAbortMutex
End Property

Public Property Get IsDirty() As Boolean
  IsDirty = mblnIsDirty
  mblnIsDirty = True
End Property

Public Function Load(plngRootPathIDList As Long, pstrRootPath As String, pstrFilename As String) As Boolean

  mstrFilename = pstrFilename
  
  Dim strTemp As String
  
  If Not (mvarComponents Is Nothing) Then Set mvarComponents = Nothing
  If Not (mvarButtons Is Nothing) Then Set mvarButtons = Nothing
  Set mvarComponents = New CComponents
  Set mvarButtons = New CButtons
  
  ' Set INI file stuff up
  mlngRootPathIDList = plngRootPathIDList
  mstrRootPath = pstrRootPath
  mobjIni.Filename = mstrRootPath & mstrFilename
  
  ' Get basic settings
  mobjIni.ReadValue "Settings", "ProgramName", mstrProgramName, "Install Assistant"
  mobjIni.ReadValue "Settings", "Splash", mstrSplash, "splash.bmp"
  mobjIni.ReadValue "Settings", "DisplayType", strTemp, "0"
  mintDisplayType = Val(strTemp)
  mobjIni.ReadValue "Settings", "SkipProgramName", mstrSkipProgramName, "setup.exe"
  mobjIni.ReadValue "Settings", "RebootPromptType", strTemp, "0"
  mintRebootPromptType = Val(strTemp)
  mobjIni.ReadValue "Settings", "RebootPromptSeconds", strTemp, "15"
  mintRebootPromptSeconds = Val(strTemp)
  mobjIni.ReadValue "Settings", "SingleInstance", strTemp, "0"
  mobjIni.ReadValue "Settings", "SingleInstanceError", mstrSingleInstanceError, "You can only run one copy of this program at a time."
  mblnSingleInstance = IIf(strTemp = "1", True, False)
  mobjIni.ReadValue "Settings", "AbortMutex", mstrAbortMutex, ""
  mobjIni.ReadValue "Settings", "HideTitleBar", strTemp, "0"
  mblnHideTitleBar = IIf(strTemp = "1", True, False)
  mobjIni.ReadValue "Settings", "EnableLogging", strTemp, "0"
  mblnEnableLogging = IIf(strTemp = "1", True, False)
  mobjIni.ReadValue "Settings", "OnOpenSound", mstrOnOpenSound, ""
  mobjIni.ReadValue "Settings", "OnCloseSound", mstrOnCloseSound, ""
  mobjIni.ReadValue "Settings", "ConfirmSetupText", mstrConfirmSetupText, "Would you like to run setup?"
 
  mobjIni.ReadBool "RequiredOS", "Win9x", mblnWin9x, True
  If mblnWin9x Then
    mobjIni.ReadBool "RequiredOS", "Windows95", mblnWindows95, True
    mobjIni.ReadBool "RequiredOS", "Windows98", mblnWindows98, True
    mobjIni.ReadBool "RequiredOS", "WindowsMe", mblnWindowsMe, True
  End If
 
  mobjIni.ReadBool "RequiredOS", "WinNT", mblnWinNT, True
  If mblnWinNT Then
    mobjIni.ReadSngValue "RequiredOS", "NTMinVersion", mstrNTMinVersion, 0
    mobjIni.ReadSngValue "RequiredOS", "NTMaxVersion", mstrNTMaxVersion, 0
    mobjIni.ReadValue "RequiredOS", "NTMinServicePack", mstrNTMinServicePack, ""
    mobjIni.ReadValue "RequiredOS", "NTMaxServicePack", mstrNTMaxServicePack, ""
  End If
  
  mobjIni.ReadValue "RequiredOS", "InvalidOSMessage", mstrInvalidOSMessage, "This program requires a newer version of Windows."
  
  ' Now go through components
  Dim vCompKeys() As String, vCompValues() As String, i As Integer
  Dim objComponent As CComponent
  If mobjIni.ReadSection("Components", vCompKeys(), vCompValues()) Then
    For i = LBound(vCompKeys) To UBound(vCompKeys)
      ' check to see if component is active
      Set objComponent = New CComponent
      objComponent.ID = vCompKeys(i)
      objComponent.SortOrder = i
      objComponent.Load mobjIni
      Set objComponent.Settings = Me
      mvarComponents.Add objComponent
    Next i
  End If
  
  ' Now go through buttons
  Dim objButton As CButton
  If mobjIni.ReadSection("Buttons", vCompKeys(), vCompValues()) Then
    For i = LBound(vCompKeys) To UBound(vCompKeys)
      ' check to see if component is active
      Set objButton = New CButton
      objButton.ID = vCompKeys(i)
      objButton.Load mobjIni
      Set objButton.Settings = Me
      mvarButtons.Add objButton
    Next i
  End If
  
  Load = True
  
End Function

Public Function Save() As Boolean

  ' make sure components are sorted right
  mobjIni.DeleteSection "Components"
  Components.Sort
  
  ' Save basic settings
  mobjIni.WriteValue "Settings", "-----", "PLEASE DO NOT EDIT THIS FILE DIRECTLY!  USE THE SETTINGS UTILITY PROVIDED WITH IA!  http://installassistant.com"
  mobjIni.WriteValue "Settings", "ProgramName", mstrProgramName
  mobjIni.WriteValue "Settings", "Splash", mstrSplash
  mobjIni.WriteValue "Settings", "IASettingsUtility", App.Major & "." & App.Minor & "." & App.Revision & ".0"
  mobjIni.WriteValue "Settings", "DisplayType", CStr(mintDisplayType)
  mobjIni.WriteValue "Settings", "SkipProgramName", mstrSkipProgramName
  mobjIni.WriteValue "Settings", "RebootPromptType", CStr(mintRebootPromptType)
  mobjIni.WriteValue "Settings", "RebootPromptSeconds", CStr(mintRebootPromptSeconds)
  mobjIni.WriteValue "Settings", "AbortMutex", mstrAbortMutex
  mobjIni.WriteValue "Settings", "SingleInstance", IIf(mblnSingleInstance, "1", "0")
  mobjIni.WriteValue "Settings", "SingleInstanceError", mstrSingleInstanceError
  mobjIni.WriteValue "Settings", "HideTitleBar", IIf(mblnHideTitleBar, "1", "0")
  mobjIni.WriteValue "Settings", "EnableLogging", IIf(mblnEnableLogging, "1", "0")
  mobjIni.WriteValue "Settings", "OnOpenSound", mstrOnOpenSound
  mobjIni.WriteValue "Settings", "OnCloseSound", mstrOnCloseSound
  mobjIni.WriteValue "Settings", "ConfirmSetupText", mstrConfirmSetupText
  
  mobjIni.WriteBool "RequiredOS", "Win9x", mblnWin9x
  If mblnWin9x Then
    mobjIni.WriteBool "RequiredOS", "Windows95", mblnWindows95
    mobjIni.WriteBool "RequiredOS", "Windows98", mblnWindows98
    mobjIni.WriteBool "RequiredOS", "WindowsMe", mblnWindowsMe
  End If
 
  mobjIni.WriteBool "RequiredOS", "WinNT", mblnWinNT
  If mblnWinNT Then
    mobjIni.WriteValue "RequiredOS", "NTMinVersion", Format(mstrNTMinVersion, "0.00")
    mobjIni.WriteValue "RequiredOS", "NTMaxVersion", Format(mstrNTMaxVersion, "0.00")
    mobjIni.WriteValue "RequiredOS", "NTMinServicePack", mstrNTMinServicePack
    mobjIni.WriteValue "RequiredOS", "NTMaxServicePack", mstrNTMaxServicePack
  End If
  
  mobjIni.WriteValue "RequiredOS", "InvalidOSMessage", mstrInvalidOSMessage
  
  'Now go through components
  Dim vCompKeys() As String, vCompValues() As String, i As Integer
  Dim objComponent As CComponent
  For Each objComponent In mvarComponents
    mobjIni.DeleteKey "Components", objComponent.ID
    mobjIni.DeleteSection objComponent.ID
    If Not objComponent.Delete Then
      mobjIni.WriteValue "Components", objComponent.ID, IIf(objComponent.Enabled, "1", "0")
      objComponent.Save mobjIni
    End If
  Next objComponent
  
  ' Now save buttons
  Dim objButton As CButton
  For Each objButton In Buttons
    mobjIni.DeleteKey "Buttons", objButton.ID
    mobjIni.DeleteSection objButton.ID
    If Not objButton.Delete Then
      mobjIni.WriteValue "Buttons", objButton.ID, IIf(objButton.Enabled, "1", "0")
      objButton.Save mobjIni
    End If
  Next objButton
  
  
  mblnIsDirty = False
  
End Function
