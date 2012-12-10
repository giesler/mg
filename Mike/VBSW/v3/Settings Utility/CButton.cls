VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CButton"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit

Private Const mstrModname = "CButton"

Public Enum ButtonTypeEnum
  RunProgram = 0
  LaunchURL
  CancelDialog
End Enum

Private mstrID As String
Private mstrName As String
Private mblnEnabled As Boolean
Private mblnDelete As Boolean
Private mblnComponentCheck As Boolean

Private mvarSettings As CSettingsFile

Private mvarButtonType As ButtonTypeEnum
Private mstrSetupCommand As String
Private mstrSetupCommandLine As String
Private mblnRestartPrompt As Boolean
Private mstrURL As String

Private mblnDefault As Boolean
Private mblnCancel As Boolean

'Images
Private mstrStandard As String
Private mstrMouseOver As String
Private mstrMouseClick As String
Private mintLeft As Integer
Private mintTop As Integer

' Sounds
Private mstrMouseEnter As String
Private mstrMouseExit As String
Private mstrMouseDown As String
Private mstrMouseUp As String

Public Property Let MouseEnter(ByVal vData As String)
    mstrMouseEnter = vData
End Property

Public Property Get MouseEnter() As String
    MouseEnter = mstrMouseEnter
End Property

Public Property Let MouseExit(ByVal vData As String)
    mstrMouseExit = vData
End Property

Public Property Get MouseExit() As String
    MouseExit = mstrMouseExit
End Property

Public Property Let MouseDown(ByVal vData As String)
    mstrMouseDown = vData
End Property

Public Property Get MouseDown() As String
    MouseDown = mstrMouseDown
End Property

Public Property Let MouseUp(ByVal vData As String)
    mstrMouseUp = vData
End Property

Public Property Get MouseUp() As String
    MouseUp = mstrMouseUp
End Property


Public Property Let Default(ByVal vData As Boolean)
  mblnDefault = vData
End Property

Public Property Get Default() As Boolean
  Default = mblnDefault
End Property


Public Property Let Cancel(ByVal vData As Boolean)
  mblnCancel = vData
End Property

Public Property Get Cancel() As Boolean
  Cancel = mblnCancel
End Property


Public Property Let RestartPrompt(ByVal vData As Boolean)
  mblnRestartPrompt = vData
End Property

Public Property Get RestartPrompt() As Boolean
  RestartPrompt = mblnRestartPrompt
End Property


Public Property Let ComponentCheck(ByVal vData As Boolean)
  mblnComponentCheck = vData
End Property

Public Property Get ComponentCheck() As Boolean
  ComponentCheck = mblnComponentCheck
End Property


Public Property Let Standard(ByVal vData As String)
  mstrStandard = vData
End Property

Public Property Get Standard() As String
  Standard = mstrStandard
End Property


Public Property Let MouseOver(ByVal vData As String)
  mstrMouseOver = vData
End Property

Public Property Get MouseOver() As String
  MouseOver = mstrMouseOver
End Property

Public Property Let MouseClick(ByVal vData As String)
  mstrMouseClick = vData
End Property

Public Property Get MouseClick() As String
  MouseClick = mstrMouseClick
End Property

Public Property Let Left(ByVal vData As Integer)
  mintLeft = vData
End Property

Public Property Get Left() As Integer
  Left = mintLeft
End Property

Public Property Let Top(ByVal vData As Integer)
  mintTop = vData
End Property

Public Property Get Top() As Integer
    Top = mintTop
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


Public Property Let ButtonType(ByVal vData As ButtonTypeEnum)
    mvarButtonType = vData
End Property

Public Property Get ButtonType() As ButtonTypeEnum
    ButtonType = mvarButtonType
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

  mstrID = "Button-" & CStr(Timer) & "-" & CStr(Rnd * 100)
  mstrName = "<new button>"
  mblnEnabled = True
  
End Function

Public Function Load(ByRef pobjCIni As CIni)

  Dim strTemp As String
  
  If Len(mstrID) = 0 Then
    mstrID = "Button-" & CStr(Timer) & "-" & CStr(Rnd * 100)
  End If
  
  ' check if enabled
  pobjCIni.ReadValue "Buttons", mstrID, strTemp, "1"
  If Val(strTemp) = "1" Then mblnEnabled = True
  
  pobjCIni.ReadValue mstrID, "Name", mstrName
  pobjCIni.ReadValue mstrID, "ComponentCheck", strTemp, "0"
  mblnComponentCheck = IIf(strTemp = "1", True, False)
  pobjCIni.ReadValue mstrID, "Default", strTemp, "0"
  mblnDefault = IIf(strTemp = "1", True, False)
  pobjCIni.ReadValue mstrID, "Cancel", strTemp, "0"
  mblnCancel = IIf(strTemp = "1", True, False)
  
  ' Button general information
  pobjCIni.ReadValue mstrID, "ButtonType", strTemp, "0"
  mvarButtonType = Val(strTemp)
  pobjCIni.ReadValue mstrID, "SetupCommand", mstrSetupCommand, ""
  pobjCIni.ReadValue mstrID, "SetupCommandLine", mstrSetupCommandLine, ""
  pobjCIni.ReadValue mstrID, "URL", mstrURL
  
  ' Button image information
  pobjCIni.ReadValue mstrID, "Standard", mstrStandard
  pobjCIni.ReadValue mstrID, "MouseOver", mstrMouseOver
  pobjCIni.ReadValue mstrID, "MouseClick", mstrMouseClick
  pobjCIni.ReadValue mstrID, "Left", strTemp, "0"
  mintLeft = Val(strTemp)
  pobjCIni.ReadValue mstrID, "Top", strTemp, "0"
  mintTop = Val(strTemp)
  pobjCIni.ReadValue mstrID, "RestartPrompt", strTemp, "0"
  mblnRestartPrompt = IIf(strTemp = "1", True, False)
  
  ' button sound information
  pobjCIni.ReadValue mstrID, "MouseEnter", mstrMouseEnter
  pobjCIni.ReadValue mstrID, "MouseExit", mstrMouseExit
  pobjCIni.ReadValue mstrID, "MouseDown", mstrMouseDown
  pobjCIni.ReadValue mstrID, "MouseUp", mstrMouseUp
  
End Function

Public Function Save(ByRef pobjCIni As CIni)
  
  Dim strTemp As String
  
  If Len(mstrID) = 0 Then
    Err.Raise vbObjectError, mstrModname & ".Load", "No component ID specified"
    Exit Function
  End If
  
  pobjCIni.WriteValue mstrID, "Name", mstrName
  pobjCIni.WriteValue mstrID, "ComponentCheck", IIf(mblnComponentCheck, "1", "0")
  pobjCIni.WriteValue mstrID, "Default", IIf(mblnDefault, "1", "0")
  pobjCIni.WriteValue mstrID, "Cancel", IIf(mblnCancel, "1", "0")
  
  ' Button image information
  pobjCIni.WriteValue mstrID, "Standard", mstrStandard
  pobjCIni.WriteValue mstrID, "MouseOver", mstrMouseOver
  pobjCIni.WriteValue mstrID, "MouseClick", mstrMouseClick
  pobjCIni.WriteValue mstrID, "Left", CStr(mintLeft)
  pobjCIni.WriteValue mstrID, "Top", CStr(mintTop)
  
  ' Button general information
  pobjCIni.WriteValue mstrID, "ButtonType", CStr(mvarButtonType)
  pobjCIni.WriteValue mstrID, "SetupCommand", mstrSetupCommand
  pobjCIni.WriteValue mstrID, "SetupCommandLine", mstrSetupCommandLine
  pobjCIni.WriteValue mstrID, "URL", mstrURL
  pobjCIni.WriteValue mstrID, "RestartPrompt", IIf(mblnRestartPrompt, "1", "0")
  
  ' Button sound information
  pobjCIni.WriteValue mstrID, "MouseEnter", mstrMouseEnter
  pobjCIni.WriteValue mstrID, "MouseExit", mstrMouseExit
  pobjCIni.WriteValue mstrID, "MouseDown", mstrMouseDown
  pobjCIni.WriteValue mstrID, "MouseUp", mstrMouseUp
  
End Function

