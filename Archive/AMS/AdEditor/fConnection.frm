VERSION 5.00
Begin VB.Form fConnection 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Ad Editor Connection"
   ClientHeight    =   2355
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5700
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2355
   ScaleWidth      =   5700
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   4440
      TabIndex        =   3
      Top             =   1800
      Width           =   975
   End
   Begin VB.CommandButton cmdChange 
      Caption         =   "&Change"
      Height          =   375
      Left            =   240
      TabIndex        =   2
      Top             =   1800
      Width           =   975
   End
   Begin VB.TextBox txtConnect 
      Height          =   1215
      Left            =   240
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   0
      Top             =   480
      Width           =   5175
   End
   Begin VB.Label lblConnect 
      Caption         =   "&Connect to..."
      Height          =   255
      Left            =   240
      TabIndex        =   1
      Top             =   240
      Width           =   3855
   End
End
Attribute VB_Name = "fConnection"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdChange_Click()

    Dim obj As MSDASC.DataLinks, cnTemp As ADODB.Connection
    Set cnTemp = New ADODB.Connection
    Set obj = New MSDASC.DataLinks
    obj.hWnd = hWnd
    
    If txtConnect.Text <> "" Then
        cnTemp.ConnectionString = txtConnect.Text
        If obj.PromptEdit(cnTemp) Then
            txtConnect.Text = cnTemp.ConnectionString
        End If
    Else
        txtConnect.Text = obj.PromptNew()
    End If
    
    Set cnTemp = Nothing
    Set obj = Nothing
    
End Sub

Private Sub cmdOK_Click()

    If txtConnect.Text = "" Then Exit Sub
    
    SaveSetting "AdEditor", "Main", "Connection", txtConnect.Text
    gstrConnectionString = txtConnect.Text
    
    Load fMain
    fMain.Show
    Unload Me
    
End Sub

Private Sub Form_Load()

    txtConnect.Text = GetSetting("AdEditor", "Main", "Connection", "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=xmcatalog;Data Source=amstest")
    
End Sub
