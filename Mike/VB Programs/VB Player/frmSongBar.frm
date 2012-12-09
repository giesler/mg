VERSION 5.00
Begin VB.Form frmSongBar 
   BackColor       =   &H80000004&
   BorderStyle     =   0  'None
   Caption         =   "3"
   ClientHeight    =   270
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4560
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   270
   ScaleWidth      =   4560
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.Label lblStop 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   ">"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00000000&
      Height          =   255
      Left            =   3840
      TabIndex        =   4
      ToolTipText     =   "Play / Stop"
      Top             =   25
      Width           =   135
   End
   Begin VB.Label lblNext 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   ">>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00000000&
      Height          =   255
      Left            =   3960
      TabIndex        =   3
      ToolTipText     =   "Next song"
      Top             =   25
      Width           =   255
   End
   Begin VB.Label lblPause 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "| |"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00000000&
      Height          =   255
      Left            =   3480
      TabIndex        =   2
      ToolTipText     =   "Play / Pause"
      Top             =   25
      Width           =   255
   End
   Begin VB.Label lblClose 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "x"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00000000&
      Height          =   255
      Left            =   4320
      TabIndex        =   1
      ToolTipText     =   "Exit VB Player"
      Top             =   25
      Width           =   135
   End
   Begin VB.Label lblCurSong 
      BackStyle       =   0  'Transparent
      Caption         =   "<Current Song>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00000000&
      Height          =   255
      Left            =   45
      TabIndex        =   0
      Top             =   25
      Width           =   3375
   End
End
Attribute VB_Name = "frmSongBar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit


Private Declare Function WindowFromPoint Lib "user32" (ByVal xPoint As Long, ByVal yPoint As Long) As Long
Private Declare Function ScreenToClient Lib "user32" (ByVal hWnd As Long, lpPoint As POINTAPI) As Long
Private Declare Function GetDC Lib "user32" (ByVal hWnd As Long) As Long
Private Declare Function ReleaseDC Lib "user32" (ByVal hWnd As Long, ByVal hDC As Long) As Long
Private Declare Function GetPixel Lib "gdi32" (ByVal hDC As Long, ByVal x As Long, ByVal y As Long) As Long
Private Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal dwRop As Long) As Long
Private Declare Function GetCursorPos Lib "user32" (lpPoint As POINTAPI) As Long

Private Type POINTAPI
   x As Long
   y As Long
End Type

Private Const CLR_INVALID = &HFFFFFFFF

' Scrollbar as "updown" constants
Private Const MIN_VALUE = 50
Private Const MAX_VALUE = 255
Private Const LG_CHANGE = 5
Private Const SM_CHANGE = 1

Private Const WM_SETCURSOR = &H20
Private Const WM_MOUSEACTIVATE = &H21
Private Const WM_CONTEXTMENU = &H7B
Private Const WM_DISPLAYCHANGE = &H7E
Private Const WM_RBUTTONUP = &H205

' Member vars...
Private m_Trans As CTranslucentForm

Private mblnMoving As Boolean
Private intX As Long, intY As Long

Private m_bdr As CFormBorder

Public Sub SetColor(intForeColor As Long, intBackColor As Long)

    Me.BackColor = intBackColor
    
    lblCurSong.ForeColor = intForeColor
    lblPause.ForeColor = intForeColor
    lblStop.ForeColor = intForeColor
    lblNext.ForeColor = intForeColor
    lblClose.ForeColor = intForeColor
    
    If trans.Mode = lwaColorKey Then m_Trans.ColorKey = BackColor

End Sub

Public Property Get trans() As CTranslucentForm
    Set trans = m_Trans
End Property

Public Property Get bdr() As CFormBorder
    Set bdr = m_bdr
End Property

Private Sub Form_Load()

    Dim intTemp1 As Long, intTemp2 As Long
    
    Set m_bdr = New CFormBorder
    Set m_bdr.Client = Me
    Set m_Trans = New CTranslucentForm
    m_Trans.hWnd = Me.hWnd
    
    Left = GetSetting("VBPlayer", Name, "Left", Left)
    Top = GetSetting("VBPlayer", Name, "Top", Top)
    
    intTemp1 = GetSetting("VBPlayer", Name, "ForeColor", lblCurSong.ForeColor)
    intTemp2 = GetSetting("VBPlayer", Name, "BackColor", Me.BackColor)
    SetColor intTemp1, intTemp2
    fMain.picSongBarForeColor.BackColor = intTemp1
    fMain.picSongBarBackColor.BackColor = intTemp2
    
    If Left > Screen.Width - Me.ScaleWidth Then
        Left = Screen.Width - Me.ScaleWidth
    End If
    
    If Top > Screen.Height - Me.ScaleHeight Then
        Height = Screen.Height - Me.ScaleHeight
    End If
    
    Call HookWindow(Me.hWnd, Me)
    
End Sub

Private Sub Form_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)

    Beep

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    SaveSetting "VBPlayer", Name, "Left", Left
    SaveSetting "VBPlayer", Name, "Top", Top
    
    SaveSetting "VBPlayer", Name, "ForeColor", lblCurSong.ForeColor
    SaveSetting "VBPlayer", Name, "BackColor", Me.BackColor

    Call UnhookWindow(Me.hWnd)

End Sub

Private Sub lblClose_Click()

    fMain.ExitApp
    
End Sub

Private Sub lblCurSong_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)

    intX = x
    intY = y
    mblnMoving = True
    
End Sub

Private Sub lblCurSong_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)

    If mblnMoving Then
        Move Left + x - intX, Top + y - intY, Width, Height
    End If

End Sub

Private Sub lblCurSong_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)

    mblnMoving = False

End Sub

Private Sub lblNext_Click()

    fMain.PlaySong QueueList
    
End Sub

Private Sub lblPause_Click()

    If fMain.wmp.PlayState = mpPlaying Then
        fMain.wmp.Pause
    ElseIf fMain.wmp.PlayState = mpPaused Then
        fMain.wmp.Play
    End If

End Sub

Private Sub lblStop_Click()
    
    If fMain.wmp.PlayState = mpPlaying Then
        fMain.wmp.Stop
        fMain.wmp.CurrentPosition = 0
    ElseIf fMain.wmp.PlayState = mpStopped Then
        fMain.wmp.Play
    End If

End Sub

Friend Function WindowProc(hWnd As Long, msg As Long, wp As Long, lp As Long) As Long
   Dim Result As Long
   
   Select Case msg
      Case WM_SETCURSOR
         Result = CallWindowProc(GetProp(hWnd, MHookMe.keyWndProc), hWnd, msg, wp, lp)
         
      Case WM_CONTEXTMENU
'         Text2.Text = "&h" & Right$("000000" & Hex(GetColor()), 6)
         Result = 0
         
      Case WM_RBUTTONUP
'         Text2.Text = "&h" & Right$("000000" & Hex(GetColor()), 6)
         Result = CallWindowProc(GetProp(hWnd, MHookMe.keyWndProc), hWnd, msg, wp, lp)
         
      Case WM_DISPLAYCHANGE
         ' Force refresh of layered window
         m_Trans.Mode = m_Trans.Mode
         Result = CallWindowProc(GetProp(hWnd, MHookMe.keyWndProc), hWnd, msg, wp, lp)
         
      Case Else
         ' Pass along to default window procedure.
         Result = CallWindowProc(GetProp(hWnd, MHookMe.keyWndProc), hWnd, msg, wp, lp)
         
   End Select
   
   ' Return desired result code to Windows.
   WindowProc = Result
End Function


Private Function GetColor() As Long
   Dim hWnd As Long
   Dim hDC As Long
   Dim pt As POINTAPI
   Dim nColor As Long
   
   ' Grab the color under the cursor.
   Call GetCursorPos(pt)
   hWnd = WindowFromPoint(pt.x, pt.y)
   hDC = GetDC(hWnd)
   Call ScreenToClient(hWnd, pt)
   nColor = GetPixel(hDC, pt.x, pt.y)
   If nColor = CLR_INVALID Then
      Call BitBlt(Me.hDC, 0, 0, 1, 1, hDC, pt.x, pt.y, vbSrcCopy)
      nColor = Me.Point(0, 0)
   End If
   Call ReleaseDC(hWnd, hDC)
   GetColor = nColor
End Function

