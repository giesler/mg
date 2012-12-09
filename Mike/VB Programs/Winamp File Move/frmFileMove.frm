VERSION 5.00
Begin VB.Form frmFileMove 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Confirm File Actions"
   ClientHeight    =   2865
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4890
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2865
   ScaleWidth      =   4890
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   2400
      TabIndex        =   6
      Top             =   2400
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   1200
      TabIndex        =   5
      Top             =   2400
      Width           =   1095
   End
   Begin VB.CheckBox chkEnque 
      Caption         =   "&Enque in WinAmp"
      Height          =   255
      Left            =   840
      TabIndex        =   4
      Top             =   2040
      Value           =   1  'Checked
      Width           =   3855
   End
   Begin VB.CheckBox chkCopyFile 
      Caption         =   "&Copy file to MP3 share"
      Height          =   255
      Left            =   840
      TabIndex        =   3
      Top             =   1680
      Width           =   3735
   End
   Begin VB.CheckBox chkMoveFile 
      Caption         =   "&Move file to MP3 share"
      Height          =   255
      Left            =   840
      TabIndex        =   2
      Top             =   1320
      Value           =   1  'Checked
      Width           =   3735
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      Height          =   495
      Left            =   120
      Picture         =   "frmFileMove.frx":0000
      ScaleHeight     =   495
      ScaleWidth      =   495
      TabIndex        =   0
      Top             =   120
      Width           =   495
   End
   Begin VB.Label lblFilename 
      Caption         =   "<filename>"
      Height          =   495
      Left            =   1200
      TabIndex        =   8
      Top             =   720
      Width           =   3495
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "File:"
      Height          =   255
      Left            =   360
      TabIndex        =   7
      Top             =   720
      Width           =   615
   End
   Begin VB.Label Label1 
      Caption         =   "Are you sure you want to perform the actions checked below?"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   840
      TabIndex        =   1
      Top             =   120
      Width           =   3615
   End
End
Attribute VB_Name = "frmFileMove"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub chkCopyFile_Click()
Me.chkMoveFile.Value = 0
End Sub

Private Sub chkMoveFile_Click()
Me.chkCopyFile.Value = 0
End Sub

Private Sub cmdCancel_Click()
End
End Sub

Private Sub cmdOK_Click()
On Error GoTo cmdOK_Err

Dim sFile As String, fS As New frmStatus
sFile = Mid$(Me.lblFilename, InStrRev(Me.lblFilename, "\") + 1)

If Me.chkMoveFile Then
  fS.lblStatus.Caption = "Moving file..."
  fS.Left = Me.Left + (Me.Width / 2) - (fS.Width / 2)
  fS.Top = Me.Top + (Me.Height / 2) - (fS.Height / 2)
  fS.Show
  fS.Refresh
  FileCopy Me.lblFilename, "\\pdc\mp3s\" & sFile
  Kill Me.lblFilename
  fS.Hide
ElseIf Me.chkCopyFile Then
  fS.lblStatus.Caption = "Copying file..."
  fS.Left = Me.Left + (Me.Width / 2) - (fS.Width / 2)
  fS.Top = Me.Top + (Me.Height / 2) - (fS.Height / 2)
  fS.Show
  fS.Refresh
  FileCopy Me.lblFilename, "\\pdc\mp3s\" & sFile
  fS.Hide
End If

Set fS = Nothing

Shell "d:\program files\Winamp\Winamp.exe /ADD " & Chr(34) & "\\pdc\mp3s\" & sFile & Chr(34)
End

Exit Sub
cmdOK_Err:
MsgBox Err.Description & " (Error #" & Err.Number & ")", vbCritical
End
Exit Sub

End Sub

Private Sub Form_Load()

Me.lblFilename = Command

Dim sFile As String

If Left(Me.lblFilename, 1) = Chr(34) Then
  Me.lblFilename = Mid$(Me.lblFilename, 2)
End If
If Right(Me.lblFilename, 1) = Chr(34) Then
  Me.lblFilename = Left(Me.lblFilename, Len(Me.lblFilename) - 1)
End If

sFile = Mid$(Me.lblFilename, InStrRev(Me.lblFilename, "\") + 1)

If Not FileExists("d:\Program Files\Winamp\Winamp.exe") Then
  Me.chkEnque.Enabled = False
End If

If FileExists("\\pdc\mp3s\" & sFile) Then
  Me.chkCopyFile.Enabled = False
  Me.chkMoveFile.Enabled = False
  If Mid(Me.lblFilename, 2, 2) = ":\" Then
    If MsgBox("This file is already at the normal MP3 location.  Should I delete this copy?", vbQuestion + vbYesNo) = vbYes Then
      Kill Me.lblFilename
    End If
  End If
End If
End Sub

Function FileExists(ByVal strPathName As String) As Integer
On Error Resume Next

Dim intFileNum As Integer

'Remove any trailing directory separator character
If Right$(strPathName, 1) = "\" Then
  strPathName = Left$(strPathName, Len(strPathName) - 1)
End If

'Attempt to open the file, return value of this function is False
'if an error occurs on open, True otherwise
intFileNum = FreeFile
Open strPathName For Input As intFileNum
FileExists = IIf(Err, False, True)
Close intFileNum
Err.Clear

End Function

