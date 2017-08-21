VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CFormBorder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' *********************************************************************
'  Copyright ©1995-2000 Karl E. Peterson, All Rights Reserved
'  http://www.mvps.org/vb
' *********************************************************************
'  You are free to use this code within your own applications, but you
'  are expressly forbidden from selling or otherwise distributing this
'  source code without prior written consent.
' *********************************************************************
'  GENERAL USAGE NOTE:  Be sure to set the Client form with these
'  properties, in order to insure the toggles in this class work:
'   * BorderStyle:  2 - Sizable
'   * ControlBox:   True
'  You may freely change these and all other properties at runtime.
' *********************************************************************
Option Explicit

' Win32 APIs used to toggle border styles.
Private Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal hWnd As Long, ByVal nIndex As Long) As Long
Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal hWnd As Long, ByVal nIndex As Long, ByVal dwNewLong As Long) As Long
Private Declare Function SetWindowPos Lib "user32" (ByVal hWnd As Long, ByVal hWndInsertAfter As Long, ByVal X As Long, ByVal Y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long
Private Declare Function ShowWindow Lib "user32" (ByVal hWnd As Long, ByVal nCmdShow As Long) As Long
Private Declare Function LockWindowUpdate Lib "user32" (ByVal hWndLock As Long) As Long
   
' Win32 APIs used to automate drag and sysmenu support.
Private Declare Function GetSystemMenu Lib "user32" (ByVal hWnd As Long, ByVal revert As Long) As Long
Private Declare Function GetMenuItemID Lib "user32" (ByVal hMenu As Long, ByVal nPos As Long) As Long
Private Declare Function GetMenuItemCount Lib "user32" (ByVal hMenu As Long) As Long
Private Declare Function GetMenuItemInfo Lib "user32" Alias "GetMenuItemInfoA" (ByVal hMenu As Long, ByVal un As Long, ByVal b As Long, lpMenuItemInfo As MENUITEMINFO) As Long
Private Declare Function SetMenuItemInfo Lib "user32" Alias "SetMenuItemInfoA" (ByVal hMenu As Long, ByVal un As Long, ByVal bool As Long, lpcMenuItemInfo As MENUITEMINFO) As Long
Private Declare Function RemoveMenu Lib "user32" (ByVal hMenu As Long, ByVal nPosition As Long, ByVal wFlags As Long) As Long
Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
Private Declare Function ReleaseCapture Lib "user32" () As Long
Private Declare Function GetCursorPos Lib "user32" (lpPoint As POINTAPI) As Long

' Used to get menu information.
Private Type MENUITEMINFO
    cbSize As Long
    fMask As Long
    fType As Long
    fState As Long
    wID As Long
    hSubMenu As Long
    hbmpChecked As Long
    hbmpUnchecked As Long
    dwItemData As Long
    dwTypeData As String
    cch As Long
End Type

' Point type used to track cursor.
Private Type POINTAPI
   X As Long
   Y As Long
End Type

' Used to support captionless drag
Private Const WM_NCLBUTTONDOWN = &HA1
Private Const HTCAPTION = 2

' Undocumented message constant.
Private Const WM_GETSYSMENU = &H313

' Used to select which menu to remove.
Private Const MF_BYCOMMAND = &H0&
Private Const MF_BYPOSITION = &H400

' Toggles enabled state of menu item.
Private Const MF_ENABLED = &H0&
Private Const MF_GRAYED = &H1&
Private Const MF_DISABLED = &H2&

' Menu information constants.
Private Const MIIM_STATE As Long = &H1
Private Const MIIM_ID As Long = &H2
Private Const MIIM_SUBMENU As Long = &H4
Private Const MIIM_CHECKMARKS As Long = &H8
Private Const MIIM_TYPE As Long = &H10
Private Const MIIM_DATA As Long = &H20

' Used to get window style bits.
Private Const GWL_STYLE = (-16)
Private Const GWL_EXSTYLE = (-20)

' Style bits.
Private Const WS_MAXIMIZEBOX = &H10000
Private Const WS_MINIMIZEBOX = &H20000
Private Const WS_THICKFRAME = &H40000
Private Const WS_SYSMENU = &H80000
Private Const WS_CAPTION = &HC00000

' Extended Style bits.
Private Const WS_EX_TOPMOST = &H8
Private Const WS_EX_TOOLWINDOW = &H80
Private Const WS_EX_CONTEXTHELP = &H400
Private Const WS_EX_APPWINDOW = &H40000

' Force total redraw that shows new styles.
Private Const SWP_FRAMECHANGED = &H20
Private Const SWP_NOMOVE = &H2
Private Const SWP_NOZORDER = &H4
Private Const SWP_NOSIZE = &H1

' Used to toggle into topmost layer.
Private Const HWND_TOPMOST = -1
Private Const HWND_NOTOPMOST = -2

' System menu command values commonly used by VB.
Private Const SC_SIZE = &HF000&
Private Const SC_MOVE = &HF010&
Private Const SC_MINIMIZE = &HF020&
Private Const SC_MAXIMIZE = &HF030&
Private Const SC_CLOSE = &HF060&
Private Const SC_RESTORE = &HF120&

' Enumerated sysmenu items.
Public Enum SysMenuItems
   smRestore = SC_RESTORE
   smMove = SC_MOVE
   smSize = SC_SIZE
   smMinimize = SC_MINIMIZE
   smMaximize = SC_MAXIMIZE
   smClose = SC_CLOSE
End Enum

' References to client form.
Private WithEvents m_Client As Form
Attribute m_Client.VB_VarHelpID = -1
Private WithEvents m_MdiClient As MDIForm
Attribute m_MdiClient.VB_VarHelpID = -1
Private m_hWnd As Long

' Member variables
Private m_AutoSysMenu As Boolean
Private m_AutoDrag As Boolean

' ************************************************
'  Initialize/Terminate
' ************************************************
Private Sub Class_Initialize()
   '
End Sub

Private Sub Class_Terminate()
   '
End Sub

' ************************************************
'  Sunken Client Events
' ************************************************
Private Sub m_Client_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
   Call ClientMouseDown(Button, Shift, X, Y)
End Sub

Private Sub m_Client_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
   Call ClientMouseUp(Button, Shift, X, Y)
End Sub

Private Sub m_MdiClient_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
   Call ClientMouseDown(Button, Shift, X, Y)
End Sub

Private Sub m_MdiClient_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
   Call ClientMouseUp(Button, Shift, X, Y)
End Sub

Private Sub ClientMouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
   ' Automatically allow user to drag using
   ' any portion of form, not just titlebar,
   ' when user depresses left mousebutton.
   ' Useful for captionless forms.
   If Button = vbLeftButton Then
      If m_AutoDrag Then
         Call ReleaseCapture
         Call SendMessage(m_hWnd, WM_NCLBUTTONDOWN, HTCAPTION, ByVal 0&)
      End If
   End If
End Sub

Private Sub ClientMouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
   ' Automatically handle system menu display
   ' when user right-clicks anywhere on form.
   ' Useful for captionless forms.
   Dim pt As POINTAPI
   ' This is relative to the screen, so we can't
   ' use the coordinates passed in the event
   Call GetCursorPos(pt)
   If Button = vbRightButton Then
      If m_AutoSysMenu Then
         ' Thanks to Klaus Probst for this trick!
         ' http://www.vbbox.com/
         Call ShowSysMenu(pt.X, pt.Y)
      End If
   End If
End Sub

' ************************************************
'  Public Properties: Read/Write
' ************************************************
Public Property Let AutoDrag(ByVal Value As Boolean)
   ' Automatically allow user to drag using
   ' any portion of form, not just titlebar,
   ' when user depresses left mousebutton.
   ' Useful for captionless forms.
   m_AutoDrag = Value
End Property

Public Property Get AutoDrag() As Boolean
   AutoDrag = m_AutoDrag
End Property

Public Property Let AutoSysMenu(ByVal Value As Boolean)
   ' Automatically handle system menu display
   ' when user right-clicks anywhere on form.
   ' Useful for captionless forms.
   m_AutoSysMenu = True
End Property

Public Property Get AutoSysMenu() As Boolean
   AutoSysMenu = m_AutoSysMenu
End Property

Public Property Set Client(ByVal obj As Object)
   ' Clear cached handle.
   m_hWnd = 0
   
   ' Store object reference and handle to client.
   If TypeOf obj Is MDIForm Then
      Set m_Client = Nothing
      Set m_MdiClient = obj
      m_hWnd = m_MdiClient.hWnd
   ElseIf TypeOf obj Is Form Then
      Set m_MdiClient = Nothing
      Set m_Client = obj
      m_hWnd = m_Client.hWnd
   End If
End Property

Public Property Get Client() As Object
   ' Return reference to client.
   If Not m_Client Is Nothing Then
      Set Client = m_Client
   ElseIf Not m_MdiClient Is Nothing Then
      Set Client = m_MdiClient
   End If
End Property

Public Property Let ControlBox(ByVal Value As Boolean)
   ' Set WS_SYSMENU On or Off as requested.
   Call FlipBit(WS_SYSMENU, Value)
End Property

Public Property Get ControlBox() As Boolean
   ' Return value of WS_SYSMENU bit.
   ControlBox = CBool(Style And WS_SYSMENU)
End Property

Public Property Let MaxButton(ByVal Value As Boolean)
   ' Set WS_MAXIMIZEBOX On or Off as requested.
   Call FlipBit(WS_MAXIMIZEBOX, Value)
   Call EnableMenuItem(smMaximize, Value)
End Property

Public Property Get MaxButton() As Boolean
   ' Return value of WS_MAXIMIZEBOX bit.
   MaxButton = CBool(Style And WS_MAXIMIZEBOX)
End Property

Public Property Let MinButton(ByVal Value As Boolean)
   ' Set WS_MINIMIZEBOX On or Off as requested.
   Call FlipBit(WS_MINIMIZEBOX, Value)
   Call EnableMenuItem(smMinimize, Value)
End Property

Public Property Get MinButton() As Boolean
   ' Return value of WS_MINIMIZEBOX bit.
   MinButton = CBool(Style And WS_MINIMIZEBOX)
End Property

Public Property Let Moveable(ByVal Value As Boolean)
   ' Toggle SC_MOVE menu appropriately.
   Call EnableMenuItem(smMove, Value)
End Property

Public Property Get Moveable() As Boolean
   ' Return whether SC_MOVE menu is enabled.
   Moveable = Not CBool(GetMenuItemState( _
      GetSystemMenu(m_hWnd, False), _
      GetMenuItemPosition(smMove)))
End Property

Public Property Let Sizeable(ByVal Value As Boolean)
   ' Toggle SC_SIZE menu appropriately,
   ' or else it gets removed!
   Call EnableMenuItem(smSize, Value)
   ' Set WS_THICKFRAME On or Off as requested.
   Call FlipBit(WS_THICKFRAME, Value)
End Property

Public Property Get Sizeable() As Boolean
   ' Return value of WS_THICKFRAME bit.
   Sizeable = CBool(Style And WS_THICKFRAME)
End Property

Public Property Let ShowInTaskbar(ByVal Value As Boolean)
   ' Set WS_EX_APPWINDOW On or Off as requested.
   ' Toggling this value requires that we also toggle
   ' visibility, flipping the bit while invisible,
   ' forcing the taskbar to update on reshow.
   ' Using LockWindowUpdate prevents some flicker.
   Call LockWindowUpdate(m_hWnd)
   Call ShowWindow(m_hWnd, vbHide)
   Call FlipBitEx(WS_EX_APPWINDOW, Value)
   Call ShowWindow(m_hWnd, vbNormalFocus)
   Call LockWindowUpdate(0&)
End Property

Public Property Get ShowInTaskbar() As Boolean
   ' Return value of WS_EX_APPWINDOW bit.
   ShowInTaskbar = CBool(StyleEx And WS_EX_APPWINDOW)
End Property

Public Property Let Titlebar(ByVal Value As Boolean)
   ' Set WS_CAPTION On or Off as requested.
   Call FlipBit(WS_CAPTION, Value)
End Property

Public Property Get Titlebar() As Boolean
   ' Return value of WS_CAPTION bit.
   Titlebar = CBool(Style And WS_CAPTION)
End Property

Public Property Let ToolWindow(ByVal Value As Boolean)
   ' Set WS_EX_TOOLWINDOW On or Off as requested.
   Call FlipBitEx(WS_EX_TOOLWINDOW, Value)
End Property

Public Property Get ToolWindow() As Boolean
   ' Return value of WS_EX_TOOLWINDOW bit.
   ToolWindow = CBool(StyleEx And WS_EX_TOOLWINDOW)
End Property

Public Property Let TopMost(ByVal Value As Boolean)
   Const swpFlags = SWP_NOMOVE Or SWP_NOSIZE
   ' Unlike most style bits, WS_EX_TOPMOST must be
   ' set with SetWindowPos rather than SetWindowLong.
   If Value Then
      Call SetWindowPos(m_hWnd, HWND_TOPMOST, 0, 0, 0, 0, swpFlags)
   Else
      Call SetWindowPos(m_hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, swpFlags)
   End If
   ' Additional references on VB use of SetWindowPos...
   ' BUG: SetWindowPos API Does Not Set Topmost Window in VB
   ' -- http://support.microsoft.com/support/kb/articles/Q192/2/54.ASP
   ' FIX: TopMost Window Does Not Stay on Top in Design Environment
   ' -- http://support.microsoft.com/support/kb/articles/Q150/2/33.ASP
End Property

Public Property Get TopMost() As Boolean
   ' Return value of WS_EX_TOPMOST bit.
   TopMost = CBool(StyleEx And WS_EX_TOPMOST)
End Property

Public Property Let WhatsThisButton(ByVal Value As Boolean)
   ' Set WS_EX_CONTEXTHELP On or Off as requested.
   Call FlipBitEx(WS_EX_CONTEXTHELP, Value)
End Property

Public Property Get WhatsThisButton() As Boolean
   ' Return value of WS_EX_CONTEXTHELP bit.
   WhatsThisButton = CBool(StyleEx And WS_EX_CONTEXTHELP)
End Property

' ************************************************
'  Public Properties: Read-only
' ************************************************
Public Property Get hWnd() As Long
   hWnd = m_hWnd
End Property

' ************************************************
'  Public Methods
' ************************************************
Public Sub EnableMenuItem(ByVal MenuItem As SysMenuItems, Optional ByVal Enabled As Boolean = True)
   ' This routine is automatically called whenever the
   ' MinButton, MaxButton, or Movable properties are
   ' set.
   Dim hMenu As Long
   Dim nPosition As Long
   Dim uFlags As Long
   Dim mii As MENUITEMINFO
   Const HighBit As Long = &H8000&
   
   ' Retrieve handle to system menu.
   hMenu = GetSystemMenu(m_hWnd, False)
   
   ' Translate ID to position.
   nPosition = GetMenuItemPosition(MenuItem)
   If nPosition >= 0 Then
   
      ' Initialize structure.
      mii.cbSize = Len(mii)
      mii.fMask = MIIM_STATE Or MIIM_ID Or MIIM_DATA Or MIIM_TYPE
      mii.dwTypeData = String$(80, 0)
      mii.cch = Len(mii.dwTypeData)
      Call GetMenuItemInfo(hMenu, nPosition, MF_BYPOSITION, mii)
      
      ' Set appropriate state.
      If Enabled Then
         mii.fState = MF_ENABLED
      Else
         mii.fState = MF_GRAYED
      End If
   
      ' New ID uses highbit to signify that
      ' the menu item is enabled.
      If Enabled Then
         mii.wID = MenuItem
      Else
         mii.wID = MenuItem And Not HighBit
      End If
   
      ' Modify the menu!
      mii.fMask = MIIM_STATE Or MIIM_ID
      Call SetMenuItemInfo(hMenu, nPosition, MF_BYPOSITION, mii)
   End If
End Sub

Public Sub Redraw()
   ' Redraw window with new style.
   Const swpFlags As Long = _
      SWP_FRAMECHANGED Or SWP_NOMOVE Or _
      SWP_NOZORDER Or SWP_NOSIZE
   SetWindowPos m_hWnd, 0, 0, 0, 0, 0, swpFlags
End Sub

Public Sub RemoveMenuItem(ByVal MenuItem As SysMenuItems)
   Dim hMenu As Long
   
   ' Retrieve handle to system menu.
   hMenu = GetSystemMenu(m_hWnd, False)
   
   ' Special case processing...
   Select Case MenuItem
      Case smClose
         ' when removing the Close menu, also
         ' remove the separator over it
         RemoveMenu hMenu, _
            GetMenuItemPosition(MenuItem) - 1, _
            MF_BYPOSITION
         
      Case smMinimize
         ' Ensure buttons are consistent.
         Me.MinButton = False
         
      Case smMaximize
         ' Ensure buttons are consistent.
         Me.MaxButton = False
   End Select
   
   ' Remove requested entry.
   Call RemoveMenu(hMenu, MenuItem, MF_BYCOMMAND)
End Sub

Public Sub ShowSysMenu(ByVal X As Long, ByVal Y As Long)
   ' Must be in screen coordinates.
   Call SendMessage(m_hWnd, WM_GETSYSMENU, 0, ByVal MakeLong(Y, X))
End Sub

' ************************************************
'  Private Methods
' ************************************************
Private Function MakeLong(ByVal WordHi As Integer, ByVal WordLo As Integer) As Long
   ' High word is coerced to Long to allow it to
   ' overflow limits of multiplication which shifts
   ' it left.
   MakeLong = (CLng(WordHi) * &H10000) Or (WordLo And &HFFFF&)
End Function

Private Function Style(Optional ByVal NewBits As Long = 0) As Long
   ' Attempt to set new style bits.
   If NewBits Then
      Call SetWindowLong(m_hWnd, GWL_STYLE, NewBits)
   End If
   ' Retrieve current style bits.
   Style = GetWindowLong(m_hWnd, GWL_STYLE)
End Function

Private Function StyleEx(Optional ByVal NewBits As Long = 0) As Long
   ' Attempt to set new style bits.
   If NewBits Then
      Call SetWindowLong(m_hWnd, GWL_EXSTYLE, NewBits)
   End If
   ' Retrieve current style bits.
   StyleEx = GetWindowLong(m_hWnd, GWL_EXSTYLE)
End Function

Private Function FlipBit(ByVal Bit As Long, ByVal Value As Boolean) As Boolean
   Dim nStyle As Long
   
   ' Retrieve current style bits.
   nStyle = GetWindowLong(m_hWnd, GWL_STYLE)
   
   ' Attempt to set requested bit On or Off,
   ' and redraw
   If Value Then
      nStyle = nStyle Or Bit
   Else
      nStyle = nStyle And Not Bit
   End If
   Call SetWindowLong(m_hWnd, GWL_STYLE, nStyle)
   Call Redraw
   
   ' Return success code.
   FlipBit = (nStyle = GetWindowLong(m_hWnd, GWL_STYLE))
End Function

Private Function FlipBitEx(ByVal Bit As Long, ByVal Value As Boolean) As Boolean
   Dim nStyleEx As Long
   
   ' Retrieve current style bits.
   nStyleEx = GetWindowLong(m_hWnd, GWL_EXSTYLE)
   
   ' Attempt to set requested bit On or Off,
   ' and redraw.
   If Value Then
      nStyleEx = nStyleEx Or Bit
   Else
      nStyleEx = nStyleEx And Not Bit
   End If
   Call SetWindowLong(m_hWnd, GWL_EXSTYLE, nStyleEx)
   Call Redraw
   
   ' Return success code.
   FlipBitEx = (nStyleEx = GetWindowLong(m_hWnd, GWL_EXSTYLE))
End Function

Private Function GetMenuItemPosition(ByVal MenuItem As SysMenuItems) As Long
   Dim hMenu As Long
   Dim ID As Long
   Dim i As Long
   Const HighBit As Long = &H8000&
   
   ' Default to returning -1 in case of
   ' failure, since menu is 0-based.
   GetMenuItemPosition = -1
   
   ' Retrieve handle to system menu.
   hMenu = GetSystemMenu(m_hWnd, False)

   ' Loop through system menu, scanning
   ' for requested standard menu item.
   For i = 0 To GetMenuItemCount(hMenu) - 1
      ID = GetMenuItemID(hMenu, i)
      If ID = MenuItem Then
         ' Return position of normal
         ' enabled menu item.
         GetMenuItemPosition = i
         Exit For
      ElseIf ID = (MenuItem And Not HighBit) Then
         ' This item is disabled.
         ' Return position and alter
         ' MenuItem with new ID.
         MenuItem = ID
         GetMenuItemPosition = i
         Exit For
      End If
   Next i
End Function

Private Function GetMenuItemText(ByVal hMenu As Long, ByVal nPosition As Long) As String
   Dim mii As MENUITEMINFO
   
   ' Initialize structure.
   mii.cbSize = Len(mii)
   mii.fMask = MIIM_TYPE
   mii.dwTypeData = String$(80, 0)
   mii.cch = Len(mii.dwTypeData)
   Call GetMenuItemInfo(hMenu, nPosition, MF_BYPOSITION, mii)
   
   ' Return current menu text
   If mii.cch > 0 Then
      GetMenuItemText = Left$(mii.dwTypeData, mii.cch)
   End If
End Function

Private Function GetMenuItemState(ByVal hMenu As Long, ByVal nPosition As Long) As Long
   Dim mii As MENUITEMINFO
   
   ' Initialize structure.
   mii.cbSize = Len(mii)
   mii.fMask = MIIM_STATE
   Call GetMenuItemInfo(hMenu, nPosition, MF_BYPOSITION, mii)
   
   ' Return current state.
   GetMenuItemState = mii.fState
End Function
