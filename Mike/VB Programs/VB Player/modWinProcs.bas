Attribute VB_Name = "modWinProcs"
Option Explicit

'defWindowProc: Variable to hold the ID of the
'               default window message processing
'               procedure. Returned by SetWindowLong.
Public defWindowProc As Long

'isSubclassed: flag indicating that subclassing
'              has been done. Provides the means
'              to call the correct message-handler.
Public isSubclassed As Boolean

'Get/SetWindowLong messages
Public Const GWL_WNDPROC As Long = (-4)
Public Const GWL_HWNDPARENT As Long = (-8)
Public Const GWL_ID As Long = (-12)
Public Const GWL_STYLE As Long = (-16)
Public Const GWL_EXSTYLE As Long = (-20)
Public Const GWL_USERDATA As Long = (-21)

'general windows messages
Public Const WM_USER As Long = &H400
Public Const WM_MYHOOK As Long = WM_USER + 1
Public Const WM_NOTIFY As Long = &H4E
Public Const WM_COMMAND As Long = &H111
Public Const WM_CLOSE As Long = &H10

Public Declare Function SetForegroundWindow Lib "user32" _
   (ByVal hwnd As Long) As Long
   
Public Declare Function PostMessage Lib "user32" Alias "PostMessageA" _
   (ByVal hwnd As Long, _
    ByVal wMsg As Long, _
    ByVal wParam As Long, _
    lParam As Any) As Long
    
Public Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" _
   (ByVal hwnd As Long, _
    ByVal nIndex As Long, _
    ByVal dwNewLong As Any) As Long

Public Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" _
   (ByVal lpPrevWndFunc As Long, _
    ByVal hwnd As Long, _
    ByVal uMsg As Long, _
    ByVal wParam As Long, _
    ByVal lParam As Long) As Long
                            
                            
'our own window message procedure
Public Function WindowProc(ByVal hwnd As Long, _
                           ByVal uMsg As Long, _
                           ByVal wParam As Long, _
                           ByVal lParam As Long) As Long

  'window message procedure
  '
  'If the handle returned is to our form,
  'call a form-specific message handler to
  'deal with the tray notifications.  If it
  'is a general system message, pass it on to
  'the default window procedure.
  '
  'If its ours, we look at lParam for the
  'message generated, and react appropriately.
   On Error Resume Next
  
   Select Case hwnd
   
     'form-specific handler
      Case fMain.hwnd
         
         Select Case uMsg
          'check uMsg for the application-defined
          'identifier (NID.uID) assigned to the
          'systray icon in NOTIFYICONDATA (NID).
  
           'WM_MYHOOK was defined as the message sent
           'as the .uCallbackMessage member of
           'NOTIFYICONDATA the systray icon
            Case WM_MYHOOK
            
              'lParam is the value of the message
              'that generated the tray notification.
               Select Case lParam
                  Case WM_LBUTTONDOWN

                 'This assures that focus is restored to
                 'the form when the menu is closed. If the
                 'form is hidden, it (correctly) has no effect.
                  fMain.Show
                 fMain.Show
                 fMain.WindowState = vbNormal
                  Call SetForegroundWindow(fMain.hwnd)

                 'show the menu
               End Select
            
           'handle any other form messages by
           'passing to the default message proc
            Case Else
            
               WindowProc = CallWindowProc(defWindowProc, _
                                            hwnd, _
                                            uMsg, _
                                            wParam, _
                                            lParam)
               Exit Function
            
         End Select

     
     'this takes care of messages when the
     'handle specified is not that of the form
      Case Else
      
          WindowProc = CallWindowProc(defWindowProc, _
                                      hwnd, _
                                      uMsg, _
                                      wParam, _
                                      lParam)
   End Select
   
End Function


