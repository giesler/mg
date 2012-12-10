VERSION 5.00
Begin VB.Form dlgGetPath 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Choose Folder"
   ClientHeight    =   3720
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   7320
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3720
   ScaleWidth      =   7320
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.FileListBox ctlFiles 
      Height          =   3210
      Left            =   4560
      TabIndex        =   11
      Top             =   360
      Width           =   2655
   End
   Begin VB.CommandButton btn_Network 
      Caption         =   "&Network"
      Height          =   375
      Left            =   840
      TabIndex        =   10
      Top             =   2760
      Width           =   855
   End
   Begin VB.CommandButton btnNewFolder 
      Height          =   375
      Left            =   1320
      Picture         =   "dlgGetPath.frx":0000
      Style           =   1  'Graphical
      TabIndex        =   9
      TabStop         =   0   'False
      ToolTipText     =   "New Folder"
      Top             =   720
      Width           =   375
   End
   Begin VB.CommandButton btnFolderUp 
      Height          =   375
      Left            =   840
      Picture         =   "dlgGetPath.frx":04B6
      Style           =   1  'Graphical
      TabIndex        =   3
      TabStop         =   0   'False
      ToolTipText     =   "Up One Level"
      Top             =   720
      Width           =   375
   End
   Begin VB.TextBox txbPath 
      Height          =   285
      Left            =   120
      TabIndex        =   2
      Top             =   360
      Width           =   4215
   End
   Begin VB.DriveListBox ctlDrive 
      Height          =   315
      Left            =   120
      TabIndex        =   6
      Top             =   3240
      Width           =   2895
   End
   Begin VB.DirListBox ctlPath 
      Height          =   1440
      Left            =   120
      TabIndex        =   4
      Top             =   1200
      Width           =   4215
   End
   Begin VB.CommandButton CancelButton 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   3120
      TabIndex        =   1
      Top             =   3240
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "OK"
      Height          =   375
      Left            =   3120
      TabIndex        =   0
      Top             =   2760
      Width           =   1215
   End
   Begin VB.Label Label3 
      Caption         =   "Files:"
      Height          =   255
      Left            =   4560
      TabIndex        =   12
      Top             =   120
      Width           =   1095
   End
   Begin VB.Label Label5 
      Caption         =   "Path:"
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   120
      Width           =   735
   End
   Begin VB.Label Label1 
      Caption         =   "Folders:"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   720
      Width           =   615
   End
   Begin VB.Label Label2 
      Caption         =   "Drives:"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   2760
      Width           =   615
   End
End
Attribute VB_Name = "dlgGetPath"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim mintResponse As Integer
Dim mblnControlUpdated As Boolean

Private Sub btn_Network_Click()
    ShowConnectDialog Me.hwnd
    RefreshControls
End Sub

Private Sub btnFolderUp_Click()
    If Len(gstrCurPath) > 3 Then
        gstrCurPath = Left(gstrCurPath, InStrRev(gstrCurPath, "\"))
    End If
    RefreshControls
End Sub

Private Sub btnNewFolder_Click()
    Dim strResponse As String
    
    On Error Resume Next
btnNewFolderInput:

    strResponse = ""
    strResponse = InputBox("Enter folder name:", "Create New Folder")
    
    If strResponse <> "" Then
        If Len(gstrCurPath) > 3 Then
            MkDir (gstrCurPath & "\" & strResponse)
        Else
            MkDir (gstrCurPath & strResponse)
        End If
        
        If Err.Number <> 0 Then
            mintResponse = MsgBox("Invalid folder name.", vbOKOnly + vbExclamation, "Error")
            Err.Clear
            GoTo btnNewFolderInput
        End If
        
        On Error GoTo 0
        
        gstrCurDrive = Left(gstrCurPath, 2)
        
        If Len(gstrCurPath) > 3 Then
            gstrCurPath = gstrCurPath & "\" & strResponse
        Else
            gstrCurPath = gstrCurPath & strResponse
        End If
        
        gstrCurDrive = CrackDrive(gstrCurPath)
        
        RefreshControls
    End If
End Sub

Private Sub CancelButton_Click()
    gblnPathChanged = False
    Unload Me
End Sub

Private Sub ctlDrive_Change()
    gstrCurDrive = UCase(Left(ctlDrive.Drive, 2))
    RefreshControls
End Sub

Private Sub ctlPath_Change()
    gstrCurPath = ctlPath.Path
    gstrCurDrive = UCase(Left(gstrCurPath, 2))
    RefreshControls
End Sub

Private Sub Form_Load()
    RefreshControls
    gblnPathChanged = False
    mblnControlUpdated = False
End Sub

Private Sub RefreshControls()
    Dim i As Integer
    Dim bFound As Boolean
    Dim oDrive As Object
    
    ' Set Drive Control
    ctlDrive.Refresh
    If gstrCurDrive = "" Then
        gstrCurDrive = "C:"
    End If
    
    bFound = False
    
    Set oDrive = goFileSystem.GetDrive(gstrCurDrive)
    
    If Not oDrive.IsReady Then
        gstrCurDrive = "C:"
        gstrCurPath = CurDir(gstrCurDrive)
    End If
    
    For i = 0 To (ctlDrive.ListCount - 1)
        If UCase(Left(ctlDrive.List(i), 2)) = UCase(gstrCurDrive) Then
            ctlDrive.ListIndex = i
            bFound = True
            Exit For
        End If
    Next i
    
    If Not bFound Then
        i = 1
    End If
    
    gstrCurDrive = UCase(Left(ctlDrive.List(i), 2))
    
    ' Set Path Control
    ctlPath.Refresh
    If (gstrCurPath = "") Or (UCase(Left(gstrCurPath, 2)) <> UCase(gstrCurDrive)) Then
        gstrCurPath = CurDir(gstrCurDrive)
    End If
    
    ctlPath.Path = gstrCurPath
    ctlPath.Refresh
    
    
    ' Set Path Text Box
    txbPath.Text = gstrCurPath
    
    ' Set Files Control
    ctlFiles.Path = gstrCurPath
    ctlFiles.Refresh
    
    Set oDrive = Nothing
End Sub

Private Sub OKButton_Click()
    gblnPathChanged = True
    Unload Me
End Sub

Private Sub txbPath_Change()
    mblnControlUpdated = True
End Sub

Private Sub txbPath_GotFocus()
    txbPath.SelStart = 0
    txbPath.SelLength = Len(txbPath.Text)
End Sub

Private Sub txbPath_LostFocus()
    If mblnControlUpdated Then
        mblnControlUpdated = False
        
        If Not goFileSystem.FolderExists(txbPath.Text) Then
            mintResponse = MsgBox("Invalid Path. Please try again.", vbCritical + vbOKOnly, "Error")
            txbPath.Text = gstrCurPath
            txbPath.SelStart = 0
            txbPath.SelLength = Len(txbPath.Text)
            txbPath.SetFocus
        Else
            gstrCurPath = txbPath.Text
            gstrCurDrive = UCase(Left(gstrCurPath, 2))
            RefreshControls
        End If
    End If
End Sub
