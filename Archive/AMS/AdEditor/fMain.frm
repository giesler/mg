VERSION 5.00
Object = "{EAB22AC0-30C1-11CF-A7EB-0000C05BAE0B}#1.1#0"; "shdocvw.dll"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form fMain 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Ad Editor"
   ClientHeight    =   6990
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   11310
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6990
   ScaleWidth      =   11310
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkEnabled 
      Caption         =   "&Enabled"
      Height          =   255
      Left            =   5760
      TabIndex        =   15
      Top             =   3840
      Width           =   1815
   End
   Begin VB.TextBox txtTitle 
      Height          =   285
      Left            =   3840
      TabIndex        =   1
      Top             =   240
      Width           =   6975
   End
   Begin VB.TextBox txtPriority 
      Height          =   285
      Left            =   9120
      TabIndex        =   5
      Text            =   "1"
      Top             =   3840
      Width           =   1575
   End
   Begin VB.CommandButton cmdDelete 
      Caption         =   "&Delete"
      Height          =   375
      Left            =   9360
      TabIndex        =   8
      Top             =   4200
      Width           =   1215
   End
   Begin VB.CommandButton cmdAddNew 
      Caption         =   "&Add As New"
      Height          =   375
      Left            =   8160
      TabIndex        =   7
      Top             =   4200
      Width           =   1095
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save"
      Height          =   375
      Left            =   6840
      TabIndex        =   6
      Top             =   4200
      Width           =   1215
   End
   Begin VB.CommandButton cmdUpdate 
      Caption         =   "&Update"
      Height          =   375
      Left            =   9840
      TabIndex        =   9
      Top             =   5040
      Width           =   1215
   End
   Begin VB.TextBox txtHTML 
      Height          =   2895
      Left            =   3120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   3
      Top             =   840
      Width           =   7815
   End
   Begin MSComctlLib.ListView lvAds 
      Height          =   6735
      Left            =   120
      TabIndex        =   11
      Top             =   120
      Width           =   2775
      _ExtentX        =   4895
      _ExtentY        =   11880
      View            =   3
      LabelEdit       =   1
      LabelWrap       =   -1  'True
      HideSelection   =   0   'False
      FullRowSelect   =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      NumItems        =   1
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "ID"
         Object.Width           =   3528
      EndProperty
   End
   Begin SHDocVwCtl.WebBrowser wb 
      Height          =   1335
      Left            =   3240
      TabIndex        =   10
      Top             =   5400
      Width           =   7935
      ExtentX         =   13996
      ExtentY         =   2355
      ViewMode        =   0
      Offline         =   0
      Silent          =   0
      RegisterAsBrowser=   0
      RegisterAsDropTarget=   1
      AutoArrange     =   0   'False
      NoClientEdge    =   0   'False
      AlignLeft       =   0   'False
      NoWebView       =   0   'False
      HideFileNames   =   0   'False
      SingleClick     =   0   'False
      SingleSelection =   0   'False
      NoFolders       =   0   'False
      Transparent     =   0   'False
      ViewID          =   "{0057D0E0-3573-11CF-AE69-08002B2E1262}"
      Location        =   "http:///"
   End
   Begin VB.Label lblID 
      Caption         =   "Label4"
      Height          =   255
      Left            =   3240
      TabIndex        =   14
      Top             =   3960
      Width           =   1935
   End
   Begin VB.Label Label3 
      Caption         =   "&Title:"
      Height          =   255
      Left            =   3240
      TabIndex        =   0
      Top             =   240
      Width           =   735
   End
   Begin VB.Label lblPri 
      Caption         =   "&Priority:"
      Height          =   255
      Left            =   7920
      TabIndex        =   4
      Top             =   3840
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Max size 468x80"
      Height          =   255
      Left            =   7080
      TabIndex        =   13
      Top             =   5160
      Width           =   2535
   End
   Begin VB.Label Label1 
      Caption         =   "Preview"
      Height          =   255
      Left            =   3360
      TabIndex        =   12
      Top             =   5160
      Width           =   3255
   End
   Begin VB.Label lblHTML 
      Caption         =   "&HTML"
      Height          =   255
      Left            =   3240
      TabIndex        =   2
      Top             =   600
      Width           =   2655
   End
End
Attribute VB_Name = "fMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdAddNew_Click()

    Dim strSQL As String
    
    strSQL = "insert into ad (adhtml, adtitle, priority, enabled) values ('"
    strSQL = strSQL & Replace(Replace(txtHTML.Text, vbCrLf, ""), "'", "''") & "', '"
    strSQL = strSQL & txtTitle.Text & "', " & txtPriority.Text & ", 0)"
    
    If Len(strSQL) > 970 Then
        MsgBox "Sorry, the ad length can only be 1000.  Ha.", vbExclamation
        Exit Sub
    End If
    
    gConn.Execute strSQL
    
    LoadAds
    chkEnabled.Value = 0
    

End Sub

Private Sub cmdDelete_Click()

    Dim strSQL As String
    
    If MsgBox("Are you sure you want to delete this current ad?", vbQuestion + vbYesNo) = vbYes Then
        strSQL = "delete from ad "
        strSQL = strSQL & "where adid = " & lvAds.SelectedItem.Tag
        gConn.Execute strSQL
        lvAds.ListItems.Remove lvAds.SelectedItem.Index
        LoadAds
    End If
    
End Sub

Private Sub cmdSave_Click()

    Dim strSQL As String
    
    If Len(txtTitle.Text) = 0 Then
        MsgBox "You must enter a title.", vbExclamation
    End If
    
    strSQL = "update ad set adhtml = '"
    strSQL = strSQL & Replace(Replace(txtHTML.Text, vbCrLf, ""), "'", "''") & "'"
    strSQL = strSQL & ", Priority = " & txtPriority.Text & " "
    strSQL = strSQL & ", AdTitle = '" & txtTitle.Text & "' "
    strSQL = strSQL & ", Enabled = '" & chkEnabled.Value & "' "
    strSQL = strSQL & "where adid = " & lvAds.SelectedItem.Tag
    
    If Len(strSQL) > 1000 Then
        MsgBox "Sorry, the ad length can only be 1000.  Ha.", vbExclamation
        Exit Sub
    End If
    
    gConn.Execute strSQL
    cmdUpdate_Click
    lvAds.SelectedItem.Text = Me.txtTitle.Text
    
End Sub

Private Sub cmdUpdate_Click()

    Dim strFilename As String, fso As FileSystemObject, tStream As TextStream
    Set fso = New FileSystemObject
    
    If fso.FileExists(App.Path & "\temp.html") Then
        fso.DeleteFile App.Path & "\temp.html"
    End If
    
    Set tStream = fso.OpenTextFile(App.Path & "\temp.html", ForAppending, True)
    tStream.Write (txtHTML.Text)
    tStream.Close

    wb.Navigate App.Path & "\temp.html"

End Sub

Private Sub Form_Load()

    Visible = True
    LoadAds
    wb.Width = Screen.TwipsPerPixelX * 468
    wb.Height = Screen.TwipsPerPixelY * 80
    
End Sub

Public Sub LoadAds()

    Dim rs As ADODB.Recordset
    Set rs = New ADODB.Recordset
    
    lvAds.ListItems.Clear
    
    rs.Open "Ad", gConn, adOpenForwardOnly, adLockReadOnly
    
    Do While Not rs.EOF
        Set li = lvAds.ListItems.Add(, , rs.Fields("AdTitle"))
        li.Tag = rs.Fields("AdID")
        rs.MoveNext
    Loop
    
    Set lvAds.SelectedItem = li
    lvAds_ItemClick li
        
End Sub

Private Sub lvAds_ItemClick(ByVal Item As MSComctlLib.ListItem)

    Dim rs As ADODB.Recordset, strTemp As String, intTemp As Integer
    Set rs = New ADODB.Recordset
    rs.Open "select * from Ad where AdID = " & Item.Tag, gConn, adOpenForwardOnly, adLockReadOnly
    
    strTemp = rs.Fields("AdHTML")
    strTemp = Replace(strTemp, vbCrLf, "")
    
    strTemp = Replace(strTemp, "<html>", "<html>" & vbCrLf)
    strTemp = Replace(strTemp, "<head>", "<head>" & vbCrLf)
    strTemp = Replace(strTemp, "</head>", "</head>" & vbCrLf)
    intTemp = InStr(strTemp, "<body")
    If intTemp > 0 Then
        intTemp = InStr(intTemp + 1, strTemp, ">")
        strTemp = Left(strTemp, intTemp) & vbCrLf & Mid(strTemp, intTemp + 1)
    End If
    strTemp = Replace(strTemp, "</body>", vbCrLf & "</body>" & vbCrLf)
    
    txtHTML.Text = strTemp
    txtPriority.Text = rs.Fields("Priority")
    txtTitle.Text = rs.Fields("AdTitle")
    lblID.Caption = "Ad ID: " & Item.Tag
    chkEnabled.Value = rs.Fields("Enabled")
    
    cmdUpdate_Click
    txtHTML.SetFocus
    
End Sub

