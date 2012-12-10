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

Public Property Get IsDirty() As Boolean
  IsDirty = mblnIsDirty
  mblnIsDirty = True
End Property

Public Function Load(plngRootPathIDList As Long, pstrRootPath As String) As Boolean

  Dim strTemp As String
  
  If Not (mvarComponents Is Nothing) Then Set mvarComponents = Nothing
  If Not (mvarButtons Is Nothing) Then Set mvarButtons = Nothing
  Set mvarComponents = New CComponents
  Set mvarButtons = New CButtons
  
  ' Set INI file stuff up
  mlngRootPathIDList = plngRootPathIDList
  mstrRootPath = pstrRootPath
  mobjIni.FileName = mstrRootPath & "\ia\settings.ini"
  
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
  mobjIni.WriteValue "Settings", "-----", "PLEASE DO NOT EDIT THIS FILE DIRECTLY!  USE THE SETTINGS UTILITY PROVIDED WITH IA!  http://vbsw.giesler.org/"
  mobjIni.WriteValue "Settings", "ProgramName", mstrProgramName
  mobjIni.WriteValue "Settings", "Splash", mstrSplash
  mobjIni.WriteValue "Settings", "IASettingsUtility", App.Major & "." & App.Minor & "." & App.Revision & ".0"
  mobjIni.WriteValue "Settings", "DisplayType", Val(mintDisplayType)
  mobjIni.WriteValue "Settings", "SkipProgramName", mstrSkipProgramName
  mobjIni.WriteValue "Settings", "RebootPromptType", CStr(mintRebootPromptType)
  mobjIni.WriteValue "Settings", "RebootPromptSeconds", CStr(mintRebootPromptSeconds)
  
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
