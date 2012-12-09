VERSION 5.00
Object = "{22D6F304-B0F6-11D0-94AB-0080C74C7E95}#1.0#0"; "msdxm.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "TABCTL32.OCX"
Object = "{FE0065C0-1B7B-11CF-9D53-00AA003C9CB6}#1.1#0"; "COMCT232.OCX"
Begin VB.Form frmMain 
   Caption         =   "vbplayer"
   ClientHeight    =   5760
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   8490
   ClipControls    =   0   'False
   FillColor       =   &H8000000F&
   ForeColor       =   &H8000000F&
   LinkTopic       =   "Form1"
   ScaleHeight     =   5760
   ScaleWidth      =   8490
   StartUpPosition =   3  'Windows Default
   Begin TabDlg.SSTab tb1 
      Height          =   4215
      Left            =   120
      TabIndex        =   8
      Top             =   1440
      Width           =   8295
      _ExtentX        =   14631
      _ExtentY        =   7435
      _Version        =   393216
      Style           =   1
      Tabs            =   5
      TabsPerRow      =   5
      TabHeight       =   520
      TabCaption(0)   =   "Queue"
      TabPicture(0)   =   "Main.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "lvQueue"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "All Songs"
      TabPicture(1)   =   "Main.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "lv"
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "Fi&nd songs"
      TabPicture(2)   =   "Main.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "lvFind"
      Tab(2).Control(1)=   "cmdFind"
      Tab(2).Control(2)=   "txtFind"
      Tab(2).Control(3)=   "lblFind"
      Tab(2).ControlCount=   4
      TabCaption(3)   =   "Utilities"
      TabPicture(3)   =   "Main.frx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "cmdNewFileCheck"
      Tab(3).Control(1)=   "Label4"
      Tab(3).ControlCount=   2
      TabCaption(4)   =   "Options"
      TabPicture(4)   =   "Main.frx":0070
      Tab(4).ControlEnabled=   0   'False
      Tab(4).Control(0)=   "txtQueueCount"
      Tab(4).Control(1)=   "udQueueCount"
      Tab(4).Control(2)=   "Label1"
      Tab(4).Control(3)=   "Label2"
      Tab(4).ControlCount=   4
      Begin VB.TextBox txtQueueCount 
         Height          =   285
         Left            =   -73920
         TabIndex        =   21
         Top             =   600
         Width           =   495
      End
      Begin VB.CommandButton cmdNewFileCheck 
         Caption         =   "&New Files"
         Height          =   375
         Left            =   -74640
         TabIndex        =   17
         Top             =   600
         Width           =   1095
      End
      Begin MSComctlLib.ListView lvFind 
         Height          =   3015
         Left            =   -74760
         TabIndex        =   16
         Top             =   960
         Width           =   7815
         _ExtentX        =   13785
         _ExtentY        =   5318
         View            =   3
         LabelEdit       =   1
         MultiSelect     =   -1  'True
         LabelWrap       =   -1  'True
         HideSelection   =   0   'False
         FullRowSelect   =   -1  'True
         _Version        =   393217
         ForeColor       =   -2147483640
         BackColor       =   -2147483643
         BorderStyle     =   1
         Appearance      =   1
         NumItems        =   5
         BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Text            =   "ID"
            Object.Width           =   0
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   1
            Text            =   "Name"
            Object.Width           =   5292
         EndProperty
         BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   2
            Text            =   "Artist"
            Object.Width           =   5292
         EndProperty
         BeginProperty ColumnHeader(4) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   3
            Text            =   "Filename"
            Object.Width           =   0
         EndProperty
         BeginProperty ColumnHeader(5) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Alignment       =   1
            SubItemIndex    =   4
            Text            =   "Duration"
            Object.Width           =   1411
         EndProperty
      End
      Begin VB.CommandButton cmdFind 
         Caption         =   "F&ind"
         Height          =   375
         Left            =   -67800
         TabIndex        =   15
         Top             =   480
         Width           =   855
      End
      Begin VB.TextBox txtFind 
         Height          =   285
         Left            =   -74160
         TabIndex        =   14
         Top             =   480
         Width           =   6255
      End
      Begin MSComctlLib.ListView lv 
         Height          =   3495
         Left            =   -74760
         TabIndex        =   9
         Top             =   480
         Width           =   7815
         _ExtentX        =   13785
         _ExtentY        =   6165
         View            =   3
         LabelEdit       =   1
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
            Alignment       =   1
            SubItemIndex    =   4
            Text            =   "Duration"
            Object.Width           =   1764
         EndProperty
      End
      Begin MSComctlLib.ListView lvQueue 
         Height          =   3615
         Left            =   120
         TabIndex        =   10
         Top             =   480
         Width           =   7935
         _ExtentX        =   13996
         _ExtentY        =   6376
         SortKey         =   4
         View            =   3
         LabelEdit       =   1
         Sorted          =   -1  'True
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
         NumItems        =   6
         BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Text            =   "ID"
            Object.Width           =   0
         EndProperty
         BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   1
            Text            =   "Name"
            Object.Width           =   5292
         EndProperty
         BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            SubItemIndex    =   2
            Text            =   "Artist"
            Object.Width           =   5292
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
         BeginProperty ColumnHeader(6) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
            Alignment       =   1
            SubItemIndex    =   5
            Text            =   "Duration"
            Object.Width           =   1764
         EndProperty
      End
      Begin ComCtl2.UpDown udQueueCount 
         Height          =   285
         Left            =   -73425
         TabIndex        =   22
         Top             =   600
         Width           =   240
         _ExtentX        =   423
         _ExtentY        =   503
         _Version        =   327681
         Value           =   100
         BuddyControl    =   "txtQueueCount"
         BuddyDispid     =   196609
         OrigLeft        =   6720
         OrigTop         =   3720
         OrigRight       =   6960
         OrigBottom      =   3975
         Max             =   100
         Min             =   1
         SyncBuddy       =   -1  'True
         BuddyProperty   =   65547
         Enabled         =   -1  'True
      End
      Begin VB.Label Label1 
         Caption         =   "Queue"
         Height          =   255
         Left            =   -74640
         TabIndex        =   24
         Top             =   600
         Width           =   615
      End
      Begin VB.Label Label2 
         Caption         =   "songs"
         Height          =   255
         Left            =   -73080
         TabIndex        =   23
         Top             =   600
         Width           =   855
      End
      Begin VB.Label Label4 
         Caption         =   "Checks directory for new files not already in the song database."
         Height          =   615
         Left            =   -73320
         TabIndex        =   18
         Top             =   720
         Width           =   6375
      End
      Begin VB.Label lblFind 
         Caption         =   "Find:"
         Height          =   255
         Left            =   -74760
         TabIndex        =   13
         Top             =   480
         Width           =   495
      End
   End
   Begin VB.CommandButton cmdStop 
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
      Left            =   7440
      TabIndex        =   7
      Top             =   960
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
      Left            =   6360
      TabIndex        =   6
      Top             =   960
      Width           =   975
   End
   Begin VB.Timer tmrUpdateSlider 
      Interval        =   500
      Left            =   6960
      Top             =   3000
   End
   Begin VB.TextBox txtLog 
      Height          =   975
      Left            =   0
      MultiLine       =   -1  'True
      TabIndex        =   3
      Top             =   6840
      Visible         =   0   'False
      Width           =   6735
   End
   Begin MSComctlLib.Slider sldCurSong 
      Height          =   255
      Left            =   0
      TabIndex        =   2
      Top             =   1080
      Width           =   6255
      _ExtentX        =   11033
      _ExtentY        =   450
      _Version        =   393216
      LargeChange     =   10
      TickStyle       =   3
      TickFrequency   =   60
   End
   Begin MSComctlLib.Slider sldVolume 
      Height          =   255
      Left            =   6720
      TabIndex        =   11
      Top             =   240
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
   Begin VB.Label lblTime 
      Alignment       =   2  'Center
      Height          =   255
      Left            =   6360
      TabIndex        =   20
      Top             =   600
      Width           =   2055
   End
   Begin MediaPlayerCtl.MediaPlayer wmp2 
      Height          =   975
      Left            =   4800
      TabIndex        =   19
      Top             =   6240
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
      Left            =   6840
      TabIndex        =   12
      Top             =   0
      Width           =   1335
   End
   Begin VB.Label lblID 
      Height          =   255
      Left            =   6360
      TabIndex        =   5
      Top             =   840
      Visible         =   0   'False
      Width           =   735
   End
   Begin MediaPlayerCtl.MediaPlayer wmp 
      Height          =   975
      Left            =   2400
      TabIndex        =   4
      Top             =   6240
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
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   240
      TabIndex        =   1
      Top             =   240
      Width           =   6135
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
      Left            =   120
      TabIndex        =   0
      Top             =   600
      Width           =   6255
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuFileExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuShortcutMenus 
      Caption         =   "Shortcut Menus"
      Begin VB.Menu mnuQueue 
         Caption         =   "&Queue"
         Begin VB.Menu mnuQueuePlayNow 
            Caption         =   "&Play Now"
            Shortcut        =   ^P
         End
         Begin VB.Menu mnuQueueRemoveVote 
            Caption         =   "Remove (and vote)"
            Shortcut        =   ^V
         End
         Begin VB.Menu mnuQueueRemove 
            Caption         =   "&Remove (no vote)"
            Shortcut        =   ^R
         End
         Begin VB.Menu mnuQueueEditSong 
            Caption         =   "&Edit Song"
            Shortcut        =   ^E
         End
         Begin VB.Menu mnuQueueBlank1 
            Caption         =   "-"
         End
         Begin VB.Menu mnuQueueUp 
            Caption         =   "Move &Up"
            Shortcut        =   ^U
         End
         Begin VB.Menu mnuQueueDown 
            Caption         =   "Move &Down"
            Shortcut        =   ^D
         End
         Begin VB.Menu mnuQueueBlank2 
            Caption         =   "-"
         End
         Begin VB.Menu mnuQueueReset 
            Caption         =   "Reset &Queue"
            Shortcut        =   ^Q
         End
      End
      Begin VB.Menu mnuPlaylist 
         Caption         =   "&Playlist"
         Begin VB.Menu mnuPlaylistAddtoQueue 
            Caption         =   "&Add to Queue"
            Shortcut        =   ^A
         End
         Begin VB.Menu mnuPlaylistPlayNow 
            Caption         =   "&Play Now"
         End
         Begin VB.Menu mnuPlaylistEditSong 
            Caption         =   "&Edit Song"
         End
      End
      Begin VB.Menu mnuFind 
         Caption         =   "Find"
         Begin VB.Menu mnuFindAddtoQueue 
            Caption         =   "&Add to Queue"
         End
         Begin VB.Menu mnuFindPlayNow 
            Caption         =   "&Play Now"
         End
         Begin VB.Menu mnuFindEditSong 
            Caption         =   "&Edit Song"
         End
         Begin VB.Menu mnuFindBlank1 
            Caption         =   "-"
         End
         Begin VB.Menu mnuFindAddList 
            Caption         =   "Add List to Queue"
         End
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const m_Module = "Main"
Public fStatus As frmStatus

Private blnMovingWindow As Boolean
Private intStartX As Long, intStartY As Long

Public Enum ActiveList
    QueueList
    AllSongsList
    FindList
End Enum

Private Sub cmdFind_Click()
On Error GoTo errHandler

    Dim li As ListItem, strSQL As String, rs As ADODB.Recordset
    
    If txtFind.Text = "" Then
        MsgBox "You must enter something to find.", vbExclamation
        Exit Sub
    End If
    
    ' clear list and show a temp item
    lvFind.ListItems.Clear
    Set li = lvFind.ListItems.Add
    li.SubItems(1) = "Searching..."
    lvFind.Refresh
    lvFind.ListItems.Clear
    
    ' find data
    Set rs = New ADODB.Recordset
    strSQL = "select * from Media where Name like '%" & txtFind.Text & "%' or Artist like '%" & txtFind.Text & "%'"
    rs.Open strSQL, gConn, adOpenForwardOnly, adLockReadOnly
    
    Do While Not rs.EOF
        
        Set li = lvFind.ListItems.Add(, , rs.Fields("ID"))
        li.SubItems(1) = rs.Fields("Name")
        li.SubItems(2) = Nz(rs.Fields("Artist"), "")
        li.SubItems(3) = rs.Fields("Filename")
        li.SubItems(4) = rs.Fields("Duration")
        li.Tag = rs.Fields("ID")
        
        rs.MoveNext
    
    Loop
    
    If lvFind.ListItems.Count = 0 Then
        Set li = lvFind.ListItems.Add
        li.SubItems(1) = "No matches found."
    End If
    
    rs.Close
    Set rs = Nothing
    CloseStatus

Exit Sub
errHandler:
ErrHand m_Module, "Find"
Exit Sub
End Sub

Private Sub LoadSongs(ByRef fld As Folder, ByRef rs As ADODB.Recordset)

    Dim fl As File, subFld As Folder
    
    ' grab all files in this folder
    For Each fl In fld.Files
        ' add to collection
        With rs
            .AddNew
            .Fields("Filename") = fl.Path
            .Fields("UserID") = GetNTUserName
            .Update
        End With
    Next fl
        
    ' go through all folders in this folder
    For Each subFld In fld.SubFolders
        LoadSongs subFld, rs
    Next subFld

End Sub

Private Sub cmdNewFileCheck_Click()
On Error GoTo errHandler

    Dim fs As FileSystemObject, strRootPath As String, intCount As Long
    Dim fld As Folder, rs As ADODB.Recordset, rsAddList As ADODB.Recordset, strSQL As String
    
    strRootPath = InputBox("Enter the root path", "New File Check", "\\stan\music")
    
    If strRootPath = "" Then Exit Sub

    ' Load file list into temp table
    OpenStatus "Loading file list from " & strRootPath & "..."
    Set rs = New ADODB.Recordset
    Set fs = New FileSystemObject
    Set fld = fs.GetFolder(strRootPath)
    gConn.Execute "delete from tempfilelist where userid = '" & GetNTUserName() & "'"
    rs.Open "TempFileList", gConn, adOpenKeyset, adLockOptimistic
    LoadSongs fld, rs
    rs.Close
    Set fld = Nothing
    Set fs = Nothing
    CloseStatus
    
    ' open query to determine new songs
    OpenStatus "Adding new songs..."
    Set rsAddList = New ADODB.Recordset
    strSQL = "select * from tempfilelist where filename not in (select filename from media)"
    OpenStatus "Checking for new songs..."
    rsAddList.Open strSQL, gConn, adOpenForwardOnly, adLockReadOnly
    rs.Open "Media", gConn, adOpenKeyset, adLockOptimistic
    
    Dim strName As String, strArtist As String, strFile As String
    Do While Not rsAddList.EOF
    
        strFile = rsAddList.Fields("Filename")
        strFile = Mid(strFile, InStrRev(strFile, "\") + 1)
    
        ' Figure out artist and name from song title
        If InStr(strFile, "-") > 0 Then
            strArtist = Trim(Left(strFile, InStr(strFile, "-") - 1))
            strName = Trim(Mid(strFile, InStr(strFile, "-") + 1))
        Else
            strArtist = ""
            strName = strFile
        End If
    
        ' get rid of extension
        If Mid(strName, Len(strName) - 3, 1) = "." Then
            strName = Left(strName, Len(strName) - 4)
        End If
        
        ' get duration
        wmp2.FileName = rsAddList.Fields("Filename")
                
        ' Add to database
        With rs
            .AddNew
            .Fields("Name") = strName
            .Fields("Artist") = strArtist
            .Fields("Filename") = rsAddList.Fields("Filename")
            .Fields("Duration") = wmp2.Duration
            .Update
        End With
        
        rsAddList.MoveNext
    Loop
    CloseStatus
    
    If rsAddList.RecordCount > 0 Then
        LoadFromDB
        MsgBox "There were " & rsAddList.RecordCount & " songs added to the database.", vbInformation
    Else
        MsgBox "There were no new songs detected.", vbInformation
    End If
    
    rsAddList.Close
    rs.Close
    Set rsAddList = Nothing
    Set rs = Nothing

Exit Sub
errHandler:
ErrHand m_Module, "NewFileCheck"
Exit Sub
End Sub

Private Sub cmdPause_Click()

    If wmp.PlayState = mpPlaying Then
        wmp.Pause
    ElseIf wmp.PlayState = mpPaused Then
        wmp.Play
    End If
    
End Sub

Private Sub cmdStop_Click()

    If wmp.PlayState = mpPlaying Then
        wmp.Stop
        wmp.CurrentPosition = 0
    ElseIf wmp.PlayState = mpStopped Then
        wmp.Play
    End If

End Sub

Private Sub Form_Load()

    Set fStatus = New frmStatus
    tb1.Tab = 0
    mnuShortcutMenus.Visible = False
    mnuFile.Visible = False
    
    Left = GetSetting("VBPlayer", Name, "Left", Left)
    Top = GetSetting("VBPlayer", Name, "Top", Top)
    udQueueCount.Value = Val(GetSetting("VBPlayer", "Settings", "QueueCount", "5"))
    Height = 6150
    
    Show
    Refresh
    LoadFromDB
    
    sldVolume.Value = wmp.Volume + 10000

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    SaveSetting "VBPlayer", Name, "Left", Left
    SaveSetting "VBPlayer", Name, "Top", Top
    SaveSetting "VBPlayer", "Settings", "QueueCount", udQueueCount.Value
    
    Set fStatus = Nothing
    
    End
    
End Sub

Private Sub lblMin_Click()

    WindowState = vbMinimized
    
End Sub

Private Sub lblX_Click()
    End
End Sub

Private Sub lblName_DblClick()

    If lblID.Caption <> "" Then
        Dim fEdit As frmEditor
        Set fEdit = New frmEditor
        fEdit.LoadSong Val(lblID.Caption)
        fEdit.Show vbModal, Me
        Set fEdit = Nothing
    End If

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
       
    ' make sure there are songs we can use
    If lv.ListItems.Count = 0 Then Exit Sub
    
    While lvQueue.ListItems.Count < udQueueCount.Value
            
        i = Rnd() * lv.ListItems.Count
        Set liSrc = lv.ListItems(i)
        
        Set liDest = AddToQueue(liSrc)
        
    Wend
    
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
    liDest.SubItems(5) = liSrc.SubItems(4)
    liDest.SubItems(4) = strIndex
    
    Set AddToQueue = liDest
    
End Function

Private Sub lv_DblClick()

    mnuPlaylistAddtoQueue_Click
    
End Sub

Private Sub lv_KeyDown(KeyCode As Integer, Shift As Integer)

    If KeyCode = vbKeyA Then
        mnuPlaylistAddtoQueue_Click
    End If

End Sub

Private Sub lv_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)

    If Button = vbRightButton Then
        PopupMenu mnuPlaylist, , , , mnuPlaylistAddtoQueue
    End If
    
End Sub

Private Sub lvFind_DblClick()

    mnuFindAddtoQueue_Click

End Sub

Private Sub lvFind_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)

    If Button = vbRightButton Then
        PopupMenu mnuFind, , , , mnuFindAddtoQueue
    End If

End Sub

Private Sub lvQueue_DblClick()

    PlaySong QueueList, lvQueue.SelectedItem.Index

End Sub

Private Sub lvQueue_KeyDown(KeyCode As Integer, Shift As Integer)

    If KeyCode = vbKeyDelete Then
        mnuQueueRemoveVote_Click
    ElseIf KeyCode = vbKeyR Then
        mnuQueueRemove_Click
    ElseIf KeyCode = vbKeyV Then
        mnuQueueRemoveVote_Click
    ElseIf KeyCode = vbKeyP Then
        mnuQueuePlayNow_Click
    ElseIf KeyCode = vbKeyE Then
        mnuQueueEditSong_Click
    ElseIf KeyCode = vbKeyU Then
        mnuQueueUp_Click
    ElseIf KeyCode = vbKeyD Then
        mnuQueueDown_Click
    ElseIf KeyCode = vbKeyQ Then
        mnuQueueReset_Click
    End If
    
End Sub

Private Sub lvQueue_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)

    If Button = vbRightButton Then
        PopupMenu mnuQueue, , , , mnuQueuePlayNow
    End If

End Sub

Private Sub mnuFileExit_Click()

    Unload Me
    
End Sub

Private Sub mnuFindAddList_Click()

    Dim li As ListItem
    
    For Each li In lvFind.ListItems
        AddToQueue li
    Next li

End Sub

Private Sub mnuFindAddtoQueue_Click()

    Dim li As ListItem
    
    For Each li In lvFind.ListItems
        If li.Selected Then
            AddToQueue li
        End If
    Next li
    
End Sub

Private Sub mnuFindEditSong_Click()

    Dim li As ListItem, fEdit As frmEditor
    Set li = lvFind.SelectedItem
    If li Is Nothing Then Exit Sub
    Set fEdit = New frmEditor
    fEdit.LoadSong Val(li.Text)
    fEdit.Show vbModal, Me
    Set fEdit = Nothing

End Sub

Private Sub mnuFindPlayNow_Click()

    PlaySong FindList

End Sub

Private Sub mnuPlaylistAddtoQueue_Click()

    Dim li As ListItem
    
    For Each li In lv.ListItems
        If li.Selected Then
            AddToQueue li
        End If
    Next li

End Sub

Private Sub mnuPlaylistEditSong_Click()

    Dim li As ListItem, fEdit As frmEditor
    Set li = lv.SelectedItem
    If li Is Nothing Then Exit Sub
    Set fEdit = New frmEditor
    fEdit.LoadSong Val(li.Text)
    fEdit.Show vbModal, Me
    Set fEdit = Nothing

End Sub

Private Sub mnuPlaylistPlayNow_Click()

    PlaySong AllSongsList

End Sub

Private Sub mnuQueueDown_Click()

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

Private Sub mnuQueueEditSong_Click()

    Dim li As ListItem, fEdit As frmEditor
    Set li = lvQueue.SelectedItem
    If li Is Nothing Then Exit Sub
    Set fEdit = New frmEditor
    fEdit.LoadSong Val(li.Text)
    fEdit.Show vbModal, Me
    Set fEdit = Nothing
    
End Sub

Private Sub mnuQueuePlayNow_Click()
    
    PlaySong QueueList
    
End Sub

Private Sub mnuQueueRemove_Click()

    Dim li As ListItem, i As Integer, col As Collection
    Dim vID As Variant
    
    Set li = lvQueue.SelectedItem
    If li Is Nothing Then Exit Sub
    i = li.Index
    
    Set col = New Collection
    For Each li In lvQueue.ListItems
        If li.Selected Then col.Add li.Text, li.Text
    Next li
    
    For Each vID In col
        Set li = lvQueue.FindItem(vID)
        If Not li Is Nothing Then
            lvQueue.ListItems.Remove li.Index
        End If
    Next vID
    
    UpdateQueue

    lvQueue.SelectedItem = lvQueue.ListItems(i)

End Sub

Private Sub mnuQueueRemoveVote_Click()

    Dim li As ListItem, i As Integer, col As Collection
    Dim vID As Variant
    
    Set li = lvQueue.SelectedItem
    If li Is Nothing Then Exit Sub
    
    i = li.Index
    
    Set col = New Collection
    For Each li In lvQueue.ListItems
        If li.Selected Then col.Add li.Text, li.Text
    Next li
    
    For Each vID In col
        Set li = lvQueue.FindItem(vID)
        If Not li Is Nothing Then
            gConn.Execute "insert into History (mediaid, currentuser, datetime, vote) values (" & li.Text & ", '" & GetNTUserName() & "', '" & Now & "', -1.0)"
            lvQueue.ListItems.Remove li.Index
        End If
    Next vID
    
    UpdateQueue

    lvQueue.SelectedItem = lvQueue.ListItems(i)
    lvQueue.Refresh

End Sub

Private Sub mnuQueueReset_Click()

    lvQueue.ListItems.Clear
    UpdateQueue

End Sub

Private Sub mnuQueueUp_Click()

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
        lblTime.Caption = FormatTime(wmp.CurrentPosition) & " of " & FormatTime(wmp.Duration)
    ElseIf wmp.PlayState = mpPaused Then
        lblTime.Visible = Not lblTime.Visible
    Else
        sldCurSong.Value = 0
        lblTime.Caption = ""
    End If
    
End Sub

Private Sub txtFind_GotFocus()
    cmdFind.Default = True
End Sub

Private Sub txtFind_LostFocus()
    cmdFind.Default = False
End Sub

Private Sub udQueueCount_Change()

    UpdateQueue

End Sub

Private Sub wmp_EndOfStream(ByVal Result As Long)

    PlaySong QueueList
    
End Sub

Private Sub UpdateHistory()

    Dim dVote As Single
    
    If lblID.Caption = "" Then
        Exit Sub
    End If
    
    ' see if within 5% of end
    dVote = wmp.CurrentPosition / wmp.Duration
    If dVote > 0.95 Then
        gConn.Execute "update Media set PlayCount=PlayCount+1 where ID = " & lblID.Caption
        gConn.Execute "insert into History (mediaid, currentuser, datetime, vote) values (" & lblID.Caption & ", '" & GetNTUserName() & "', '" & Now & "', " & (dVote + 1) & ")"
    
    ' see if between 5% and 95%
    ElseIf dVote > 0.05 Then
        gConn.Execute "insert into History (mediaid, currentuser, datetime, vote) values (" & lblID.Caption & ", '" & GetNTUserName() & "', '" & Now & "', " & dVote & ")"
    End If

End Sub

Private Sub PlaySong(al As ActiveList, Optional intIndex As Long = 1)
On Error GoTo errHandler

    UpdateHistory

    Dim li As ListItem
    
    If lvQueue.ListItems.Count = 0 Then Exit Sub
    
    If al = QueueList Then
        Set li = lvQueue.ListItems(intIndex)
    ElseIf al = AllSongsList Then
        Set li = lv.SelectedItem
    ElseIf al = FindList Then
        Set li = lvFind.SelectedItem
    End If
    
    lblID.Caption = li.Text
    lblName.Caption = li.SubItems(1)
    lblArtist.Caption = li.SubItems(2)
    wmp.FileName = li.SubItems(3)
    wmp.Play
    sldCurSong.Max = wmp.Duration
    Caption = li.SubItems(1) & IIf(li.SubItems(2) <> "", " - " & li.SubItems(2), "")
    If al = QueueList Then
        lvQueue.ListItems.Remove li.Index
        UpdateQueue
    End If
    
Exit Sub
errHandler:
ErrHand m_Module, "PlayCurSong"
Exit Sub
End Sub

Private Sub wmp_PlayStateChange(ByVal OldState As Long, ByVal NewState As Long)

    If NewState = mpPaused Then
        cmdPause.Caption = "Play"
        cmdPause.Enabled = True
        cmdStop.Caption = "Stop"
        cmdStop.Enabled = False
    ElseIf NewState = mpPlaying Then
        cmdPause.Caption = "Pause"
        cmdPause.Enabled = True
        cmdStop.Caption = "Stop"
        cmdStop.Enabled = True
    ElseIf NewState = mpStopped Then
        cmdPause.Caption = "Pause"
        cmdPause.Enabled = False
        cmdStop.Caption = "Play"
        cmdStop.Enabled = True
    End If

End Sub

Private Sub wmp_PositionChange(ByVal oldPosition As Double, ByVal newPosition As Double)

    sldCurSong.Value = newPosition

End Sub

Private Sub AddToLog(strLog As String)

    txtLog.Text = txtLog.Text & vbCrLf & strLog
    txtLog.SelStart = Len(txtLog)
    
End Sub

Private Sub LoadFromDB()

    Dim rs As ADODB.Recordset, li As ListItem
    Set rs = New ADODB.Recordset
    
    OpenStatus "Loading..."
    rs.Open "select * from Media", gConn, adOpenForwardOnly, adLockReadOnly
    
    lv.ListItems.Clear
    lvQueue.ListItems.Clear
    lvFind.ListItems.Clear
    
    Do While Not rs.EOF
        
        Set li = lv.ListItems.Add(, , rs.Fields("ID"))
        li.SubItems(1) = rs.Fields("Name")
        li.SubItems(2) = Nz(rs.Fields("Artist"), "")
        li.SubItems(3) = rs.Fields("Filename")
        li.SubItems(4) = FormatTime(rs.Fields("Duration"))
        li.Tag = rs.Fields("ID")
        
        rs.MoveNext
    
    Loop
    
    rs.Close
    Set rs = Nothing
    CloseStatus
    
    UpdateQueue
    PlaySong QueueList

End Sub


Private Function FormatTime(intSecs As Long) As String

    FormatTime = Int(intSecs / 60) & ":" & Format(Int(intSecs Mod 60), "00")

End Function
