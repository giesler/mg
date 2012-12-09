VERSION 5.00
Begin VB.Form frmSongBar 
   BackColor       =   &H80000008&
   BorderStyle     =   0  'None
   Caption         =   "Form1"
   ClientHeight    =   255
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   255
   ScaleWidth      =   4680
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.Label lblClose 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "x"
      ForeColor       =   &H8000000E&
      Height          =   255
      Left            =   4320
      TabIndex        =   1
      Top             =   0
      Width           =   255
   End
   Begin VB.Label lblCurSong 
      BackStyle       =   0  'Transparent
      Caption         =   "<Current Song>"
      ForeColor       =   &H8000000E&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   3855
   End
End
Attribute VB_Name = "frmSongBar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private mblnMoving As Boolean

Private Sub Form_Load()

    Left = GetSetting(App.EXEName, Name, "Left", Left)
    Top = GetSetting(App.EXEName, Name, "Top", Top)
    
    If Left > Screen.Width - Me.ScaleWidth Then
        Left = Screen.Width - Me.ScaleWidth
    End If
    
    If Top > Screen.Height - Me.ScaleHeight Then
        Height = Screen.Height - Me.ScaleHeight
    End If
    
End Sub

Private Sub Form_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)

    If mblnMoving Then
        Move Left + X, Top + Y, Width, Height
    End If

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    SaveSetting App.EXEName, Name, "Left", Left
    SaveSetting App.EXEName, Name, "Top", Top

End Sub

Private Sub lblCurSong_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)

    mblnMoving = True

End Sub

Private Sub lblCurSong_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)

    mblnMoving = False
    
End Sub
