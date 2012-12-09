VERSION 5.00
Begin VB.Form frmDataLinkDialog 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Data Link Dialog"
   ClientHeight    =   2040
   ClientLeft      =   60
   ClientTop       =   330
   ClientWidth     =   5955
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2040
   ScaleWidth      =   5955
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdSaveAsUDL 
      Caption         =   "&Save as UDL"
      Height          =   375
      Left            =   3720
      TabIndex        =   3
      Top             =   1320
      Width           =   1455
   End
   Begin VB.CommandButton cmdChangeConString 
      Caption         =   "..."
      Height          =   375
      Left            =   5280
      TabIndex        =   2
      Top             =   360
      Width           =   495
   End
   Begin VB.TextBox txtConString 
      Height          =   855
      Left            =   1080
      MultiLine       =   -1  'True
      TabIndex        =   0
      Top             =   360
      Width           =   4095
   End
   Begin VB.Label Label1 
      Caption         =   "Connection String:"
      Height          =   495
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   975
   End
End
Attribute VB_Name = "frmDataLinkDialog"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdChangeConString_Click()

  Dim objDL As MSDASC.DataLinks
  Dim objCon As ADODB.Connection
  
  Set objDL = New MSDASC.DataLinks
  
  'Display data link dialog
  If Me.txtConString = "" Then
    Set objCon = objDL.PromptNew
    If objCon Is Nothing Then
      Set objDL = Nothing
      Exit Sub
    End If
  Else
    Set objCon = New ADODB.Connection
    objCon.ConnectionString = Me.txtConString
    If Not objDL.PromptEdit(objCon) Then
      Set objDL = Nothing
      Exit Sub
    End If
  End If
  
  Me.txtConString = objCon.ConnectionString
  
  Set objCon = Nothing
  Set objDL = Nothing
  
End Sub

Private Sub cmdSaveAsUDL_Click()

  If Me.txtConString = "" Then
    MsgBox "The connection string must be filled in.", vbExclamation
    Exit Sub
  End If
  
  Dim objDialog As Object, strTemp As String, bytTemp() As Byte
    
  Set objDialog = CreateObject("MSComDlg.CommonDialog.1")
  objDialog.Filter = "Data Link Files (*.udl) | *.udl"
  objDialog.ShowSave
  
  If objDialog.FileName <> "" Then
    Open objDialog.FileName For Binary As #1
    Put #1, , &HFEFF
    
    strTemp = "[oledb]" & vbCrLf
    bytTemp = strTemp
    Put #1, , bytTemp
    
    strTemp = "; Everything after this line is an OLE DB initstring" & vbCrLf
    ReDim bytTemp(Len(strTemp) * 2) As Byte
    bytTemp = strTemp
    Put #1, , bytTemp
  
    ReDim bytTemp(Len(Me.txtConString & vbCrLf) * 2) As Byte
    bytTemp = Me.txtConString & vbCrLf
    Put #1, , bytTemp
    
    Close #1
    
  End If
  
  Set objDialog = Nothing
  
End Sub
