VERSION 5.00
Begin VB.Form fASPClean 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ASP Cleanup"
   ClientHeight    =   4785
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   8910
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4785
   ScaleWidth      =   8910
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkConfigFiles 
      Caption         =   "Delete config files (global.asa, web.config)"
      Height          =   255
      Left            =   1320
      TabIndex        =   6
      Top             =   600
      Value           =   1  'Checked
      Width           =   5775
   End
   Begin VB.CommandButton cmdCleanup 
      Caption         =   "&Cleanup"
      Enabled         =   0   'False
      Height          =   375
      Left            =   7080
      TabIndex        =   4
      Top             =   4320
      Width           =   1455
   End
   Begin VB.ListBox lstFiles 
      Height          =   3180
      Left            =   360
      TabIndex        =   3
      Top             =   960
      Width           =   8175
   End
   Begin VB.CommandButton cmdLoadFileList 
      Caption         =   "&Load"
      Height          =   375
      Left            =   7200
      TabIndex        =   2
      Top             =   240
      Width           =   1215
   End
   Begin VB.TextBox txtFolder 
      Height          =   285
      Left            =   1320
      TabIndex        =   0
      Top             =   240
      Width           =   3735
   End
   Begin VB.Label lblStatus 
      Height          =   255
      Left            =   480
      TabIndex        =   5
      Top             =   4320
      Width           =   5175
   End
   Begin VB.Label Label1 
      Caption         =   "Path:"
      Height          =   255
      Left            =   600
      TabIndex        =   1
      Top             =   240
      Width           =   1335
   End
End
Attribute VB_Name = "fASPClean"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim mFileCol As Collection
Dim fso As Scripting.FileSystemObject

Private Sub cmdCleanup_Click()
        
    Dim strFile As Variant

    lstFiles.SetFocus
    txtFolder.Enabled = False
    
    lblStatus.Caption = "Cleaning files..."
    For Each strFile In mFileCol
        CleanupFile CStr(strFile)
    Next strFile
    lblStatus.Caption = "Files cleaned!"
    
End Sub

Private Sub cmdLoadFileList_Click()

    Dim fld As Scripting.Folder
    Set fso = New Scripting.FileSystemObject
    Set mFileCol = New Collection
    
    If MsgBox("This will delete all _* directories and mark files writable in '" & txtFolder.Text & "'.  Are you sure you want to continue?", vbQuestion + vbYesNo) = vbNo Then Exit Sub
    
    Set fld = fso.GetFolder(txtFolder.Text)
    lstFiles.Clear
    lblStatus.Caption = "Loading file list..."
    LoadFileList fld, txtFolder.Text
    lblStatus.Caption = "File list loaded."
    cmdCleanup.Enabled = True

End Sub

Private Sub LoadFileList(fld As Folder, strRoot As String)

    Dim f As File, ch As Folder, strExt As String
    
    If Left(fld.Name, 1) = "_" Then
        fso.DeleteFolder fld.Path, True
        Exit Sub
    End If
    
    For Each f In fld.Files
        strExt = Mid(f.Path, InStrRev(f.Path, ".") + 1)

        If f.Attributes And ReadOnly Then
            f.Attributes = f.Attributes - ReadOnly
        End If
        If f.Name = "vssver.scc" Or f.Name = "_vti_inf.html" Or f.Name = "postinfo.html" Or f.Name = "mssccprj.scc" Then
            fso.DeleteFile f.Path
        ElseIf strExt = "pdb" Or strExt = "csproj" Then
            fso.DeleteFile f.Path
        ElseIf chkConfigFiles.Value = 1 And (f.Name = "global.asa" Or f.Name = "web.config") Then
            fso.DeleteFile f.Path
        Else
            If f.Attributes And ReadOnly Then
                f.Attributes = f.Attributes - ReadOnly
            End If
            lstFiles.AddItem f.Path
            mFileCol.Add f.Path
        End If
    Next f
    
    For Each ch In fld.SubFolders
        LoadFileList ch, strRoot
    Next ch

End Sub

Private Sub CleanupFile(strFile As String)

    Dim f As File, tstr As TextStream, strFileContents As String, strCurLine, blnInASPCode As Boolean, strExt As String
    Set f = fso.GetFile(strFile)
    strExt = Mid(strFile, InStrRev(strFile, ".") + 1)
    
    If strExt <> "aspx" And strExt <> "htm" And strExt <> "html" And strExt <> "asp" And strExt <> "ams" And strExt <> "xml" And strExt <> "ascx" Then Exit Sub
    
    Set tstr = f.OpenAsTextStream(ForReading)
    
    Do While Not tstr.AtEndOfStream
        strCurLine = tstr.ReadLine
        If blnInASPCode And InStr(strCurLine, "%>") = 0 Then
            strCurLine = Replace(strCurLine, vbTab, "")
            strFileContents = strFileContents & strCurLine & vbCrLf
        ElseIf InStr(strCurLine, "<%") > 0 And InStr(strCurLine, "%>") = 0 Then
            blnInASPCode = True
            strCurLine = Replace(strCurLine, vbTab, "")
            strFileContents = strFileContents & strCurLine & vbCrLf
        Else
            blnInASPCode = False
            strCurLine = Replace(strCurLine, vbTab, "")
            strFileContents = strFileContents & strCurLine
        End If
    Loop
    
    tstr.Close
    Set tstr = Nothing
    Set f = Nothing
    
    fso.DeleteFile strFile
    
    strFileContents = Replace(strFileContents, "default.asp", "index.ams")
    strFileContents = Replace(strFileContents, ".asp", ".ams")
    
    Set tstr = fso.CreateTextFile(strFile)
    
    tstr.Write strFileContents
    tstr.Close
    Set tstr = Nothing
    Set f = fso.GetFile(strFile)
    If f.Name = "default.asp" Then
        f.Name = "index.ams"
    ElseIf strExt = "asp" Then
        f.Name = Left(f.Name, InStr(f.Name, ".") - 1) & ".ams"
    End If
    Set f = Nothing
  
End Sub

