Attribute VB_Name = "modSysTray"
Option Explicit
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright ©1996-2001 VBnet, Randy Birch, All Rights Reserved.
' Some pages may also contain other copyrights by the author.
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' You are free to use this code within your own applications,
' but you are expressly forbidden from selling or otherwise
' distributing this source code without prior written consent.
' This includes both posting free demo projects made from this
' code as well as reproducing the code in text or html format.
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

'Required Public constants, types & declares
'for the Shell_Notify API method
Public Const NIM_ADD As Long = &H0
Public Const NIM_MODIFY As Long = &H1
Public Const NIM_DELETE As Long = &H2

Public Const NIF_ICON As Long = &H2     'adding an ICON
Public Const NIF_TIP As Long = &H4      'adding a TIP
Public Const NIF_MESSAGE As Long = &H1  'want return messages

'rodent constant we'll need for the callback
Public Const WM_LBUTTONDOWN As Long = &H201
Public Const WM_LBUTTONUP As Long = &H202
Public Const WM_LBUTTONDBLCLK As Long = &H203

Public Const WM_MBUTTONDOWN As Long = &H207
Public Const WM_MBUTTONUP As Long = &H208
Public Const WM_MBUTTONDBLCLK As Long = &H209

Public Const WM_RBUTTONDOWN As Long = &H204
Public Const WM_RBUTTONUP As Long = &H205
Public Const WM_RBUTTONDBLCLK As Long = &H206

'the actual workhorse
Type NOTIFYICONDATA
  cbSize As Long
  hwnd As Long
  uID As Long
  uFlags As Long
  uCallbackMessage As Long
  hIcon As Long
  szTip As String * 64
End Type

Public NID As NOTIFYICONDATA

Declare Function Shell_NotifyIcon Lib "shell32.dll" _
   Alias "Shell_NotifyIconA" _
   (ByVal dwMessage As Long, _
   lpData As NOTIFYICONDATA) As Long


