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
Private mstrSetup As String
Private mstrCmdLine As String
Private mblnIsDirty As Boolean
Private mobjIni As CIni
Private mvarCComponents As CComponents

Private mstrInstallStandard As String
Private mstrInstallMouseOver As String
Private mstrInstallMouseClick As String
Private mintInstallLeft As Integer
Private mintInstallTop As Integer

Private mstrCancelStandard As String
Private mstrCancelMouseOver As String
Private mstrCancelMouseClick As String
Private mintCancelLeft As Integer
Private mintCancelTop As Integer

Public Property Get CComponents() As CComponents
    If mvarCComponents Is Nothing Then
        Set mvarCComponents = New CComponents
    End If

    Set CComponents = mvarCComponents
End Property

Public Property Set CComponents(vData As CComponents)
    Set mvarCComponents = vData
End Property

Private Sub Class_Initialize()
  mblnIsDirty = False
  Set mobjIni = New CIni
End Sub

Private Sub Class_Terminate()
  Set mvarCComponents = Nothing
  Set mobjIni = Nothing
  Set mvarCComponents = Nothing
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

Public Property Let Setup(pstrIn As String)
  mstrSetup = pstrIn
  mblnIsDirty = True
End Property

Public Property Get Setup() As String
  Setup = mstrSetup
End Property

Public Property Let CmdLine(pstrIn As String)
  mstrCmdLine = pstrIn
  mblnIsDirty = True
End Property

Public Property Get CmdLine() As String
  CmdLine = mstrCmdLine
End Property

Public Property Let InstallStandard(pstrIn As String)
  mstrInstallStandard = pstrIn
  mblnIsDirty = True
End Property

Public Property Get InstallStandard() As String
  InstallStandard = mstrInstallStandard
End Property

Public Property Let InstallMouseOver(pstrIn As String)
  mstrInstallMouseOver = pstrIn
  mblnIsDirty = True
End Property

Public Property Get InstallMouseOver() As String
  InstallMouseOver = mstrInstallMouseOver
End Property

Public Property Let InstallMouseClick(pstrIn As String)
  mstrInstallMouseClick = pstrIn
  mblnIsDirty = True
End Property

Public Property Get InstallMouseClick() As String
  InstallMouseClick = mstrInstallMouseClick
End Property

Public Property Let InstallLeft(pintIn As Long)
  mintInstallLeft = pintIn
  mblnIsDirty = True
End Property

Public Property Get InstallLeft() As Long
  InstallLeft = mintInstallLeft
End Property

Public Property Let InstallTop(pintIn As Long)
  mintInstallTop = pintIn
  mblnIsDirty = True
End Property

Public Property Get InstallTop() As Long
  InstallTop = mintInstallTop
End Property

Public Property Let CancelStandard(pstrIn As String)
  mstrCancelStandard = pstrIn
  mblnIsDirty = True
End Property

Public Property Get CancelStandard() As String
  CancelStandard = mstrCancelStandard
End Property

Public Property Let CancelMouseOver(pstrIn As String)
  mstrCancelMouseOver = pstrIn
  mblnIsDirty = True
End Property

Public Property Get CancelMouseOver() As String
  CancelMouseOver = mstrCancelMouseOver
End Property

Public Property Let CancelMouseClick(pstrIn As String)
  mstrCancelMouseClick = pstrIn
  mblnIsDirty = True
End Property

Public Property Get CancelMouseClick() As String
  CancelMouseClick = mstrCancelMouseClick
End Property

Public Property Let CancelLeft(pintIn As Long)
  mintCancelLeft = pintIn
  mblnIsDirty = True
End Property

Public Property Get CancelLeft() As Long
  CancelLeft = mintCancelLeft
End Property

Public Property Let CancelTop(pintIn As Long)
  mintCancelTop = pintIn
  mblnIsDirty = True
End Property

Public Property Get CancelTop() As Long
  CancelTop = mintCancelTop
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

  If Not (mvarCComponents Is Nothing) Then Set mvarCComponents = Nothing
  Set mvarCComponents = New CComponents

  ' Set INI file stuff up
  mlngRootPathIDList = plngRootPathIDList
  mstrRootPath = pstrRootPath
  mobjIni.FileName = mstrRootPath & "\vbsw\settings.ini"
  
  ' Get basic settings
  mobjIni.ReadValue "Settings", "ProgramName", mstrProgramName, "VB Setup Wrapper"
  mobjIni.ReadValue "Settings", "Setup", mstrSetup, "setup\setup.exe"
  If mstrSetup = "setup\setup.exe" Then mstrCmdLine = "setup.exe"
  mobjIni.ReadValue "Settings", "CmdLine", mstrCmdLine, mstrCmdLine
  mobjIni.ReadValue "Settings", "Splash", mstrSplash, "splash.bmp"
  
  ' Get button settings
  mobjIni.ReadValue "Settings", "InstallStandard", mstrInstallStandard, ""
  mobjIni.ReadValue "Settings", "InstallMouseOver", mstrInstallMouseOver, ""
  mobjIni.ReadValue "Settings", "InstallMouseClick", mstrInstallMouseClick, ""
  mobjIni.ReadIntValue "Settings", "InstallLeft", mintInstallLeft, 0
  mobjIni.ReadIntValue "Settings", "InstallTop", mintInstallTop, 0
  mobjIni.ReadValue "Settings", "CancelStandard", mstrCancelStandard, ""
  mobjIni.ReadValue "Settings", "CancelMouseOver", mstrCancelMouseOver, ""
  mobjIni.ReadValue "Settings", "CancelMouseClick", mstrCancelMouseClick, ""
  mobjIni.ReadIntValue "Settings", "CancelLeft", mintCancelLeft, 0
  mobjIni.ReadIntValue "Settings", "CancelTop", mintCancelTop, 0
  
  ' Now go through components
  Dim vCompKeys() As String, vCompValues() As String, i As Integer
  Dim objComponent As CComponent
  If mobjIni.ReadSection("Components", vCompKeys(), vCompValues()) Then
    For i = LBound(vCompKeys) To UBound(vCompKeys)
      ' check to see if component is active
      Set objComponent = New CComponent
      objComponent.ID = vCompKeys(i)
      objComponent.SavedID = vCompKeys(i)
      objComponent.SortOrder = i
      objComponent.Load mobjIni
      Set objComponent.Settings = Me
      mvarCComponents.Add objComponent
    Next i
  End If
  Load = True
  
End Function

Public Function Save() As Boolean

  ' make sure components are sorted right
  mobjIni.DeleteSection "Components"
  CComponents.Sort
  
  ' Save basic settings
  mobjIni.WriteValue "Settings", "-----", "PLEASE DO NOT EDIT THIS FILE DIRECTLY!  USE THE SETTINGS UTILITY PROVIDED WITH VBSW!  http://giesler.org/vbsw"
  mobjIni.WriteValue "Settings", "ProgramName", mstrProgramName
  mobjIni.WriteValue "Settings", "Setup", mstrSetup
  mobjIni.WriteValue "Settings", "CmdLine", mstrCmdLine
  mobjIni.WriteValue "Settings", "Splash", mstrSplash
  mobjIni.WriteValue "Settings", "VBSWSettingsUtility", App.Major & "." & App.Minor & "." & App.Revision & ".0"
  
  ' Get button settings
  mobjIni.WriteValue "Settings", "InstallStandard", mstrInstallStandard
  mobjIni.WriteValue "Settings", "InstallMouseOver", mstrInstallMouseOver
  mobjIni.WriteValue "Settings", "InstallMouseClick", mstrInstallMouseClick
  mobjIni.WriteValue "Settings", "InstallLeft", CStr(mintInstallLeft)
  mobjIni.WriteValue "Settings", "InstallTop", CStr(mintInstallTop)
  mobjIni.WriteValue "Settings", "CancelStandard", mstrCancelStandard
  mobjIni.WriteValue "Settings", "CancelMouseOver", mstrCancelMouseOver
  mobjIni.WriteValue "Settings", "CancelMouseClick", mstrCancelMouseClick
  mobjIni.WriteValue "Settings", "CancelLeft", CStr(mintCancelLeft)
  mobjIni.WriteValue "Settings", "CancelTop", CStr(mintCancelTop)
  
  'Now go through components
  Dim vCompKeys() As String, vCompValues() As String, i As Integer
  Dim objComponent As CComponent
  For Each objComponent In CComponents
    If objComponent.Delete Then
      If objComponent.SavedID <> "" Then
        mobjIni.DeleteKey "Components", objComponent.SavedID
        mobjIni.DeleteSection objComponent.SavedID
      End If
    Else
      If Len(objComponent.SavedID) > 0 And objComponent.SavedID <> objComponent.ID Then
        mobjIni.DeleteKey "Components", objComponent.SavedID
      End If
      ' clear section to be safe
      mobjIni.DeleteSection objComponent.SavedID
      mobjIni.WriteValue "Components", objComponent.ID, IIf(objComponent.Enabled, "1", "0")
      objComponent.Save mobjIni
    End If
  Next objComponent
  
  mblnIsDirty = False
  
End Function
