VERSION 5.00
Begin VB.Form frmEditor 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Song Editor"
   ClientHeight    =   1815
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6090
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1815
   ScaleWidth      =   6090
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   375
      Left            =   4560
      TabIndex        =   7
      Top             =   1320
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3360
      TabIndex        =   6
      Top             =   1320
      Width           =   1095
   End
   Begin VB.TextBox txtFilename 
      BackColor       =   &H80000004&
      Height          =   285
      Left            =   1200
      Locked          =   -1  'True
      TabIndex        =   4
      TabStop         =   0   'False
      Top             =   840
      Width           =   4695
   End
   Begin VB.TextBox txtArtist 
      Height          =   285
      Left            =   1200
      TabIndex        =   2
      Top             =   480
      Width           =   4695
   End
   Begin VB.TextBox txtName 
      Height          =   285
      Left            =   1200
      TabIndex        =   0
      Top             =   120
      Width           =   4695
   End
   Begin VB.Label Label3 
      Caption         =   "Filename:"
      Height          =   255
      Left            =   240
      TabIndex        =   5
      Top             =   840
      Width           =   855
   End
   Begin VB.Label Label2 
      Caption         =   "Artist:"
      Height          =   255
      Left            =   240
      TabIndex        =   3
      Top             =   480
      Width           =   855
   End
   Begin VB.Label Label1 
      Caption         =   "Name:"
      Height          =   255
      Left            =   240
      TabIndex        =   1
      Top             =   120
      Width           =   855
   End
End
Attribute VB_Name = "frmEditor"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private mintID As Long

Private Sub cmdCancel_Click()

    Visible = False
    
End Sub

Private Sub cmdOK_Click()

    Dim rs As ADODB.Recordset, li As ListItem
    Set rs = New ADODB.Recordset
    rs.Open "select * from media where id = " & mintID, gConn, adOpenForwardOnly, adLockReadOnly
    
    rs.Fields("Name") = txtName.Text
    rs.Fields("Artist") = txtArtist.Text
    rs.Update
    rs.Close
    Set rs = Nothing

    ' update list sheets
    Set li = fMain.lv.FindItem(Str(mintID))
    If Not li Is Nothing Then
        li.SubItems(1) = txtName.Text
        li.SubItems(2) = txtArtist.Text
    End If
    
    Set li = fMain.lvFind.FindItem(Str(mintID))
    If Not li Is Nothing Then
        li.SubItems(1) = txtName.Text
        li.SubItems(2) = txtArtist.Text
    End If
    
    Set li = fMain.lvQueue.FindItem(Str(mintID))
    If Not li Is Nothing Then
        li.SubItems(1) = txtName.Text
        li.SubItems(2) = txtArtist.Text
    End If
    
    Visible = False
    
End Sub

Public Sub LoadSong(intID As Long)

    Dim rs As ADODB.Recordset
    Set rs = New ADODB.Recordset

    mintID = intID
    
    rs.Open "select * from media where id = " & intID, gConn, adOpenForwardOnly, adLockReadOnly
    
    txtName.Text = rs.Fields("Name")
    txtArtist.Text = rs.Fields("Artist")
    
    rs.Close
    Set rs = Nothing

End Sub
