VERSION 5.00
Begin VB.Form TimerForm 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "MSN2 Auto Runner"
   ClientHeight    =   2295
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   3465
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2295
   ScaleWidth      =   3465
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.ListBox List1 
      Height          =   1620
      Left            =   120
      TabIndex        =   1
      Top             =   480
      Width           =   3135
   End
   Begin VB.Timer Timer1 
      Left            =   1080
      Top             =   360
   End
   Begin VB.Label Label1 
      Caption         =   "A task is currently running, please wait..."
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3135
   End
End
Attribute VB_Name = "TimerForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Timer1_Timer()

  Timer1.Enabled = False
  Me.Visible = True
  
  ' Check for commands to run
  Dim sql As String, rs As Recordset, cmd As String
  sql = "p_GetCommands '" + computername + "'"
  Set rs = New Recordset
  rs.Open sql, GlobalConnection
  
  Do While Not rs.EOF
    
    cmd = rs.Fields("CommandText").Value
    
    List1.AddItem Command
    
    ' process it
    ProcessCommand cmd
    
    ' mark as done
    GlobalConnection.Execute "p_CompleteCommand " + CStr(rs.Fields("CommandId").Value)
        
    rs.MoveNext
  Loop
  
  rs.Close
  Set rs = Nothing
  
  Me.Visible = False
  Timer1.Enabled = True
  
End Sub

Private Sub ProcessCommand(message As String)

  If message = "" Then
  
    Exit Sub
    
  ElseIf message = "kill" Then
  
    End
  
  ElseIf message = "update" Then
  
    End
    
  Else
  
    Shell message, vbNormalNoFocus
  
  End If
  
  

End Sub
