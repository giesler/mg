Attribute VB_Name = "APIDeclare"
Option Explicit

Public Const WM_USER = &H400
Public Const TB_SETSTYLE = WM_USER + 56
Public Const TB_GETSTYLE = WM_USER + 57
Public Const TBSTYLE_FLAT = &H800


Public Const LVM_FIRST As Long = &H1000
Public Const LVM_SETEXTENDEDLISTVIEWSTYLE As Long = (LVM_FIRST + 54)
Public Const LVM_GETEXTENDEDLISTVIEWSTYLE As Long = (LVM_FIRST + 55)
Public Const LVIF_TEXT = &H1
Public Const LVIF_STATE = &H8
Public Const LVM_SETITEMSTATE = (LVM_FIRST + 43)
Public Const LVM_GETITEMSTATE As Long = (LVM_FIRST + 44)
Public Const LVM_GETITEMTEXT As Long = (LVM_FIRST + 45)
Public Const LVM_GETHEADER As Long = (LVM_FIRST + 31)
Public Const LVM_FINDITEM As Long = (LVM_FIRST + 13)
Public Const LVM_SORTITEMS As Long = (LVM_FIRST + 48)

Public Const LVIS_STATEIMAGEMASK As Long = &HF000

Public Const LVFI_PARAM = 1

Public Const LVS_EX_GRIDLINES As Long = &H1
Public Const LVS_EX_CHECKBOXES As Long = &H4
Public Const LVS_EX_TRACKSELECT As Long = &H8
Public Const LVS_EX_FULLROWSELECT As Long = &H20
Public Const LVS_EX_HEADERDRAGDROP As Long = &H10

Public Const GWL_STYLE = (-16)
Public Const HDS_HOTTRACK = &H4

Public Const HDI_BITMAP = &H10
Public Const HDI_IMAGE = &H20
Public Const HDI_ORDER = &H80
Public Const HDI_FORMAT = &H4
Public Const HDI_TEXT = &H2
Public Const HDI_WIDTH = &H1
Public Const HDI_HEIGHT = HDI_WIDTH

Public Const HDF_LEFT = 0
Public Const HDF_RIGHT = 1
Public Const HDF_IMAGE = &H800
Public Const HDF_BITMAP_ON_RIGHT = &H1000
Public Const HDF_BITMAP = &H2000
Public Const HDF_STRING = &H4000

Public Const HDM_FIRST = &H1200
Public Const HDM_SETITEM = (HDM_FIRST + 4)


Public objFind As LV_FINDINFO
Public objItem As LV_ITEM
Public objIndex As Integer

'variable to hold the sort order (ascending or descending)
Public sOrder As Boolean

Public Type POINT
    x As Long
    y As Long
End Type

Public Type LV_FINDINFO
    Flags As Long
    psz As String
    lParam As Long
    pt As POINT
    vkDirection As Long
End Type

Public Type HD_ITEM
   mask        As Long
   cxy         As Long
   pszText     As String
   hbm         As Long
   cchTextMax  As Long
   fmt         As Long
   lParam      As Long
   iImage      As Long
   iOrder      As Long
End Type

Public Type LV_ITEM
    mask As Long
    iItem As Long
    iSubItem As Long
    state As Long
    stateMask As Long
    pszText As String
    cchTextMax As Long
    iImage As Long
    lParam As Long
    iIndent As Long
End Type

Public Type RECT
   Left        As Long
   Top         As Long
   Right       As Long
   Bottom      As Long
End Type

Public Declare Function SendMessageLong Lib "user32" Alias "SendMessageA" _
        (ByVal hWnd As Long, _
         ByVal Msg As Long, _
         ByVal wParam As Long, _
         ByVal lParam As Long) As Long

Public Declare Function SendMessageAny Lib "user32" Alias "SendMessageA" _
        (ByVal hWnd As Long, _
         ByVal wMsg As Long, _
         ByVal wParam As Long, _
         lParam As Any) As Long
   
Public Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" _
        (ByVal hWnd As Long, _
         ByVal nIndex As Long) As Long
  
Public Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" _
        (ByVal hWnd As Long, _
         ByVal nIndex As Long, _
         ByVal dwNewLong As Long) As Long
   
Public Declare Function SetWindowPos Lib "user32" _
        (ByVal hWnd As Long, _
         ByVal hWndInsertAfter As Long, _
         ByVal x As Long, ByVal y As Long, _
         ByVal cx As Long, ByVal cy As Long, _
         ByVal wFlags As Long) As Long

Public Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" _
        (ByVal hWnd1 As Long, _
         ByVal hWnd2 As Long, _
         ByVal lpsz1 As String, _
         ByVal lpsz2 As String) As Long

Public Declare Sub CopyMem Lib "kernel32" Alias "RtlMoveMemory" _
        (pTo As Any, _
         uFrom As Any, _
         ByVal lSize As Long)

Public Declare Function lstrlenW Lib "kernel32" _
        (ByVal lpString As Long) As Long

Public Declare Function lstrcpyW Lib "kernel32" _
        (lpString1 As Byte, _
        ByVal lpString2 As Long) As Long

Public Declare Function lstrcpy Lib "kernel32" Alias "lstrcpyA" _
        (ByVal lpString1 As String, _
         ByVal lpString2 As Long) As Long
         
Public Declare Function lstrlen Lib "kernel32" Alias "lstrlenA" _
        (ByVal lpString As String) As Long

Public Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, nSize As Long) As Long

