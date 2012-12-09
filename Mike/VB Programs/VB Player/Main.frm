VERSION 5.00
Object = "{22D6F304-B0F6-11D0-94AB-0080C74C7E95}#1.0#0"; "msdxm.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Begin VB.Form frmMain 
   BorderStyle     =   0  'None
   Caption         =   "vbplayer"
   ClientHeight    =   6825
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   9135
   FillColor       =   &H8000000F&
   ForeColor       =   &H8000000F&
   LinkTopic       =   "Form1"
   ScaleHeight     =   6825
   ScaleWidth      =   9135
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraBorderTop 
      Height          =   30
      Left            =   0
      TabIndex        =   27
      Top             =   240
      Width           =   9500
   End
   Begin MSComctlLib.Slider sldVolume 
      Height          =   255
      Left            =   6960
      TabIndex        =   21
      Top             =   6360
      Width           =   1695
      _ExtentX        =   2990
      _ExtentY        =   450
      _Version        =   393216
      LargeChange     =   200
      SmallChange     =   25
      Min             =   8000
      Max             =   10000
      SelStart        =   9000
      TickFrequency   =   200
      Value           =   9000
   End
   Begin VB.CommandButton cmdStopPlay 
      Caption         =   "Stop"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7800
      TabIndex        =   20
      Top             =   1080
      Width           =   975
   End
   Begin VB.CommandButton cmdPause 
      Caption         =   "Pause"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   6720
      TabIndex        =   19
      Top             =   1080
      Width           =   975
   End
   Begin VB.CommandButton cmdPlayNow 
      Caption         =   "Play &Now"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7080
      TabIndex        =   17
      Top             =   4440
      Width           =   1575
   End
   Begin VB.Timer tmrUpdateSlider 
      Interval        =   500
      Left            =   6840
      Top             =   360
   End
   Begin VB.CommandButton cmdQueueRemove 
      Caption         =   "&Remove"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7080
      TabIndex        =   15
      Top             =   2640
      Width           =   1575
   End
   Begin VB.CommandButton cmdQueueDown 
      Caption         =   "&Down"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7920
      TabIndex        =   14
      Top             =   2160
      Width           =   735
   End
   Begin VB.CommandButton cmdQueueUp 
      Caption         =   "&Up"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7080
      TabIndex        =   13
      Top             =   2160
      Width           =   735
   End
   Begin VB.CommandButton cmdAddToQueue 
      Caption         =   "&Add to Queue"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7080
      TabIndex        =   12
      Top             =   3960
      Width           =   1575
   End
   Begin VB.CommandButton cmdQueuePlay 
      BackColor       =   &H80000012&
      Caption         =   "&Play Now"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7080
      MaskColor       =   &H00000000&
      TabIndex        =   11
      Top             =   1680
      Width           =   1575
   End
   Begin VB.CommandButton cmdResetQueue 
      Caption         =   "Reset &Queue"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7080
      TabIndex        =   10
      Top             =   3120
      Width           =   1575
   End
   Begin VB.TextBox txtLog 
      Height          =   975
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   9
      Top             =   6840
      Visible         =   0   'False
      Width           =   6735
   End
   Begin MSComctlLib.Slider sldCurSong 
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   1080
      Width           =   6255
      _ExtentX        =   11033
      _ExtentY        =   450
      _Version        =   393216
      LargeChange     =   10
      TickStyle       =   3
      TickFrequency   =   60
   End
   Begin VB.CommandButton cmdLoadFromDB 
      Caption         =   "&Load from DB"
      Height          =   375
      Left            =   7920
      TabIndex        =   2
      Top             =   360
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.CommandButton cmdLoadPath 
      Caption         =   "&Load from Path"
      Height          =   375
      Left            =   6360
      TabIndex        =   1
      Top             =   360
      Visible         =   0   'False
      Width           =   1335
   End
   Begin MSComctlLib.ListView lv 
      Height          =   3255
      Left            =   120
      TabIndex        =   0
      Top             =   3480
      Width           =   6735
      _ExtentX        =   11880
      _ExtentY        =   5741
      View            =   3
      MultiSelect     =   -1  'True
      LabelWrap       =   -1  'True
      HideSelection   =   0   'False
      FullRowSelect   =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      NumItems        =   4
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "ID"
         Object.Width           =   0
      EndProperty
      BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   1
         Text            =   "Name"
         Object.Width           =   3528
      EndProperty
      BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   2
         Text            =   "Artist"
         Object.Width           =   3528
      EndProperty
      BeginProperty ColumnHeader(4) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   3
         Text            =   "Path"
         Object.Width           =   7056
      EndProperty
   End
   Begin MSComctlLib.ListView lvQueue 
      Height          =   1455
      Left            =   120
      TabIndex        =   4
      Top             =   1680
      Width           =   6735
      _ExtentX        =   11880
      _ExtentY        =   2566
      SortKey         =   4
      View            =   3
      Sorted          =   -1  'True
      LabelWrap       =   -1  'True
      HideSelection   =   0   'False
      FullRowSelect   =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      NumItems        =   5
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "ID"
         Object.Width           =   0
      EndProperty
      BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   1
         Text            =   "Name"
         Object.Width           =   3528
      EndProperty
      BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   2
         Text            =   "Artist"
         Object.Width           =   3528
      EndProperty
      BeginProperty ColumnHeader(4) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   3
         Text            =   "Path"
         Object.Width           =   0
      EndProperty
      BeginProperty ColumnHeader(5) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   4
         Text            =   "Index"
         Object.Width           =   0
      EndProperty
   End
   Begin VB.Frame fraTop 
      BackColor       =   &H80000007&
      BorderStyle     =   0  'None
      Height          =   255
      Left            =   0
      TabIndex        =   23
      Top             =   0
      Width           =   9135
      Begin VB.Label lblCaption 
         BackStyle       =   0  'Transparent
         Caption         =   "vbplayer"
         BeginProperty Font 
            Name            =   "Tahoma"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H8000000E&
         Height          =   255
         Left            =   120
         TabIndex        =   26
         Top             =   0
         Width           =   8535
      End
      Begin VB.Label lblMin 
         Alignment       =   2  'Center
         BackStyle       =   0  'Transparent
         Caption         =   "_"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H8000000E&
         Height          =   255
         Left            =   8760
         TabIndex        =   25
         Top             =   0
         Width           =   135
      End
      Begin VB.Label lblX 
         Alignment       =   2  'Center
         BackStyle       =   0  'Transparent
         Caption         =   "x"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H8000000E&
         Height          =   255
         Left            =   9000
         TabIndex        =   24
         Top             =   0
         Width           =   135
      End
   End
   Begin VB.Label Label3 
      Caption         =   "Volume"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   7080
      TabIndex        =   22
      Top             =   6120
      Width           =   1335
   End
   Begin VB.Label lblID 
      Height          =   255
      Left            =   6480
      TabIndex        =   18
      Top             =   840
      Visible         =   0   'False
      Width           =   735
   End
   Begin MediaPlayerCtl.MediaPlayer wmp 
      Height          =   975
      Left            =   6960
      TabIndex        =   16
      Top             =   6840
      Visible         =   0   'False
      Width           =   2055
      AudioStream     =   -1
      AutoSize        =   0   'False
      AutoStart       =   0   'False
      AnimationAtStart=   -1  'True
      AllowScan       =   -1  'True
      AllowChangeDisplaySize=   -1  'True
      AutoRewind      =   0   'False
      Balance         =   0
      BaseURL         =   ""
      BufferingTime   =   5
      CaptioningID    =   ""
      ClickToPlay     =   -1  'True
      CursorType      =   0
      CurrentPosition =   -1
      CurrentMarker   =   0
      DefaultFrame    =   ""
      DisplayBackColor=   0
      DisplayForeColor=   16777215
      DisplayMode     =   0
      DisplaySize     =   4
      Enabled         =   -1  'True
      EnableContextMenu=   -1  'True
      EnablePositionControls=   -1  'True
      EnableFullScreenControls=   0   'False
      EnableTracker   =   -1  'True
      Filename        =   ""
      InvokeURLs      =   -1  'True
      Language        =   -1
      Mute            =   0   'False
      PlayCount       =   1
      PreviewMode     =   0   'False
      Rate            =   1
      SAMILang        =   ""
      SAMIStyle       =   ""
      SAMIFileName    =   ""
      SelectionStart  =   -1
      SelectionEnd    =   -1
      SendOpenStateChangeEvents=   -1  'True
      SendWarningEvents=   -1  'True
      SendErrorEvents =   -1  'True
      SendKeyboardEvents=   0   'False
      SendMouseClickEvents=   0   'False
      SendMouseMoveEvents=   0   'False
      SendPlayStateChangeEvents=   -1  'True
      ShowCaptioning  =   0   'False
      ShowControls    =   0   'False
      ShowAudioControls=   0   'False
      ShowDisplay     =   0   'False
      ShowGotoBar     =   0   'False
      ShowPositionControls=   0   'False
      ShowStatusBar   =   0   'False
      ShowTracker     =   0   'False
      TransparentAtStart=   0   'False
      VideoBorderWidth=   0
      VideoBorderColor=   0
      VideoBorder3D   =   0   'False
      Volume          =   -600
      WindowlessVideo =   0   'False
   End
   Begin VB.Label lblArtist 
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   360
      TabIndex        =   7
      Top             =   360
      Width           =   5295
   End
   Begin VB.Label lblName 
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   240
      TabIndex        =   6
      Top             =   600
      Width           =   6015
   End
   Begin VB.Label Label2 
      Caption         =   "Play Queue"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   5
      Top             =   1440
      Width           =   2535
   End
   Begin VB.Label Label1 
      Caption         =   "All Songs"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   3
      Top             =   3240
      Width           =   2535
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private blnMovingWindow As Boolean
Private intStartX As Long, intStartY As Long

Private Sub cmdAddToQueue_Click()

    Dim li As ListItem
    
    For Each li In lv.ListItems
        If li.Selected Then
            AddToQueue li
        End If
    Next li
    
End Sub

Private Sub cmdLoadFromDB_Click()

    Dim rs As ADODB.Recordset, li As ListItem
    Set rs = New ADODB.Recordset
    
    OpenStatus "Loading..."
    rs.Open "select * from Media", gConn, adOpenForwardOnly, adLockReadOnly
    
    lv.ListItems.Clear
    
    Do While Not rs.EOF
        
        Set li = lv.ListItems.Add(, , rs.Fields("ID"))
        li.SubItems(1) = rs.Fields("Name")
        li.SubItems(2) = Nz(rs.Fields("Artist"), "")
        li.SubItems(3) = rs.Fields("Filename")
        li.Tag = rs.Fields("ID")
        
        rs.MoveNext
    
    Loop
    
    rs.Close
    Set rs = Nothing
    CloseStatus
    
    UpdateQueue
'    PlayCurSong
    
End Sub

Private Sub cmdLoadPath_Click()

    Dim fs As FileSystemObject, strRootPath As String, blnAddToDB As Boolean
    Dim fld As Folder, rs As ADODB.Recordset
    Set fs = New FileSystemObject
    
    strRootPath = InputBox("Enter the root path")
    blnAddToDB = IIf(MsgBox("Click 'Yes' to add songs to db, click 'No' to add songs to listview.", vbYesNo + vbQuestion) = vbYes, True, False)
    
    If strRootPath = "" Then Exit Sub

    Set fld = fs.GetFolder(strRootPath)
    If blnAddToDB Then
        Set rs = New ADODB.Recordset
        rs.Open "Media", gConn, adOpenKeyset, adLockOptimistic
    End If
    OpenStatus "Loading songs from " & strRootPath & "..."
    LoadSongs fld, rs
    CloseStatus
    
    If blnAddToDB Then
        rs.Close
        Set rs = Nothing
    End If
    Set fld = Nothing
    Set fs = Nothing

End Sub

Private Sub LoadSongs(ByRef fld As Folder, ByRef rs As ADODB.Recordset)

    Dim fl As File, li As ListItem, strSQL As String, subFld As Folder
    Dim strName As String, strArtist As String
    
    ' grab all files in this folder
    For Each fl In fld.Files
        If InStr(fl.Name, "-") > 0 Then
            strArtist = Trim(Left(fl.Name, InStr(fl.Name, "-") - 1))
            strName = Trim(Mid(fl.Name, InStr(fl.Name, "-") + 1))
        Else
            strArtist = ""
            strName = fl.Name
        End If
        
        ' get rid of extension
        If Mid(strName, Len(strName) - 3, 1) = "." Then
            strName = Left(strName, Len(strName) - 4)
        End If
        If rs Is Nothing Then
            Set li = Me.lv.ListItems.Add(, , strName)
            li.SubItems(1) = strArtist
            li.SubItems(2) = fl.Path
        Else
            With rs
                .AddNew
                .Fields("Name") = strName
                .Fields("Artist") = strArtist
                .Fields("Filename") = fl.Path
                .Update
            End With
        End If
    Next fl
    
    lv.Refresh
    
    ' go through all folders in this folder
    For Each subFld In fld.SubFolders
        LoadSongs subFld, rs
    Next subFld

End Sub

Private Sub cmdPause_Click()

    If wmp.PlayState = mpPlaying Then
        wmp.Pause
    ElseIf wmp.PlayState = mpPaused Then
        wmp.Play
    End If
    
End Sub

Private Sub cmdPlayNow_Click()

    PlayCurSong False
    
End Sub

Private Sub cmdQueueDown_Click()

    Dim li As ListItem, li2 As ListItem, strTemp As String
        
    Set li = lvQueue.SelectedItem
    If li Is Nothing Then Exit Sub
    If li.Index = lvQueue.ListItems.Count Then Exit Sub
    
    Set li2 = lvQueue.ListItems(li.Index + 1)
    
    strTemp = li.SubItems(4)
    li.SubItems(4) = li2.SubItems(4)
    li2.SubItems(4) = strTemp
    
    lvQueue.Sorted = True
    lvQueue.Refresh

End Sub

Private Sub cmdQueuePlay_Click()

    PlayCurSong
    
End Sub

Private Sub cmdQueueRemove_Click()

    Dim li As ListItem
    
    lvQueue.ListItems.Remove lvQueue.SelectedItem.Index
    
    UpdateQueue

End Sub

Private Sub cmdQueueUp_Click()

    Dim li As ListItem, li2 As ListItem, strTemp As String
        
    Set li = lvQueue.SelectedItem
    If li Is Nothing Then Exit Sub
    If li.Index = 1 Then Exit Sub
    
    Set li2 = lvQueue.ListItems(li.Index - 1)
    
    strTemp = li.SubItems(4)
    li.SubItems(4) = li2.SubItems(4)
    li2.SubItems(4) = strTemp
    
    lvQueue.Sorted = True
    lvQueue.Refresh
    
End Sub

Private Sub cmdResetQueue_Click()

    lvQueue.ListItems.Clear
    UpdateQueue
    
End Sub

Private Sub cmdStopPlay_Click()

    If wmp.PlayState = mpPlaying Then
        wmp.Stop
        wmp.CurrentPosition = 0
        cmdStopPlay.Caption = "Play"
    ElseIf wmp.PlayState = mpStopped Then
        wmp.Play
        cmdStopPlay.Caption = "Stop"
    End If
    
End Sub

Private Sub Form_Load()

    Left = GetSetting("VBPlayer", Name, "Left", Left)
    Top = GetSetting("VBPlayer", Name, "Top", Top)
    
    Show
    Refresh
    cmdLoadFromDB_Click
    
    sldVolume.Value = wmp.Volume + 10000

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    SaveSetting "VBPlayer", Name, "Left", Left
    SaveSetting "VBPlayer", Name, "Top", Top
    
    End
    
End Sub

Private Sub lblCaption_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)

    intStartX = X
    intStartY = Y
    blnMovingWindow = True
    
End Sub

Private Sub lblCaption_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)

    If blnMovingWindow Then
        Move Left - (intStartX - X), Top - (intStartY - Y)
    End If
    
End Sub

Private Sub lblCaption_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)

    blnMovingWindow = False
    
End Sub

Private Sub lblMin_Click()

    WindowState = vbMinimized
    
End Sub

Private Sub lblX_Click()
    End
End Sub

Private Sub lv_ColumnClick(ByVal ColumnHeader As MSComctlLib.ColumnHeader)

    If lv.Sorted And lv.SortKey = ColumnHeader.Index Then
        lv.SortOrder = IIf(lv.SortOrder = lvwAscending, lvwDescending, lvwAscending)
    Else
        lv.SortOrder = lvwAscending
        lv.SortKey = ColumnHeader.Index - 1
        lv.Sorted = True
    End If

End Sub

Private Sub UpdateQueue()

    Dim liDest As ListItem, liSrc As ListItem, i As Long
       
    While lvQueue.ListItems.Count < 5
            
        i = Rnd() * lv.ListItems.Count
        Set liSrc = lv.ListItems(i)
        
        Set liDest = AddToQueue(liSrc)
        
    Wend
    
    Set lvQueue.SelectedItem = lvQueue.ListItems(1)

End Sub

Private Function AddToQueue(liSrc As ListItem) As ListItem

    Dim liDest As ListItem, iMax As Integer, strIndex As String
    
    iMax = -1
    For Each liDest In lvQueue.ListItems
        If Val(liDest.SubItems(4)) > iMax Then iMax = Val(liDest.SubItems(4))
    Next liDest
    
    iMax = iMax + 1
    strIndex = String(5 - Len(Trim(Str(iMax))), "0") & Trim(Str(iMax))
    Set liDest = lvQueue.ListItems.Add(, , liSrc.Text)
    liDest.SubItems(1) = liSrc.SubItems(1)
    liDest.SubItems(2) = liSrc.SubItems(2)
    liDest.SubItems(3) = liSrc.SubItems(3)
    liDest.SubItems(4) = strIndex
    
    Set AddToQueue = liDest
    
End Function

Private Sub sldCurSong_Scroll()

    If wmp.PlayState = mpPlaying Then
        wmp.CurrentPosition = sldCurSong.Value
    End If

End Sub

Private Sub sldVolume_Scroll()

    wmp.Volume = sldVolume.Value - 10000
    
End Sub

Private Sub tmrUpdateSlider_Timer()

    If wmp.PlayState = mpPlaying Then
        sldCurSong.Value = wmp.CurrentPosition
    End If
    
End Sub

Private Sub wmp_EndOfStream(ByVal Result As Long)

    If lblID.Caption <> "" Then
        gConn.Execute "update Media set PlayCount=PlayCount+1 where ID = " & lblID.Caption
        gConn.Execute "insert into History (mediaid, currentuser, datetime) values (" & lblID.Caption & ", '" & GetNTUserName() & "', '" & Now & "')"
    End If
    
    PlayCurSong
    
End Sub

Private Sub PlayCurSong(Optional blnQueueSong As Boolean = True)

    Dim li As ListItem
    
    If lvQueue.ListItems.Count = 0 Then Exit Sub
    
    If blnQueueSong Then
        Set li = lvQueue.ListItems(1)
    Else
        Set li = lv.SelectedItem
    End If
    
    lblID.Caption = li.Text
    lblName.Caption = li.SubItems(1)
    lblArtist.Caption = li.SubItems(2)
    wmp.FileName = li.SubItems(3)
    wmp.Play
    cmdStopPlay.Caption = "Stop"
    sldCurSong.Max = wmp.Duration
    Caption = li.SubItems(1) & IIf(li.SubItems(2) <> "", " - " & li.SubItems(2), "")
    lblCaption.Caption = Caption
    If blnQueueSong Then
        lvQueue.ListItems.Remove li.Index
    End If
    
    UpdateQueue
    
End Sub

Private Sub wmp_PlayStateChange(ByVal OldState As Long, ByVal NewState As Long)

    AddToLog "PlayStateChange: Old: " & OldState & ", New: " & NewState

End Sub

Private Sub wmp_PositionChange(ByVal oldPosition As Double, ByVal newPosition As Double)

    sldCurSong.Value = newPosition

End Sub

Private Sub AddToLog(strLog As String)

    txtLog.Text = txtLog.Text & vbCrLf & strLog
    txtLog.SelStart = Len(txtLog)
    
End Sub
