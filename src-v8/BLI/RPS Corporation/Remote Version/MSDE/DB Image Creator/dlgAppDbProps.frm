VERSION 5.00
Begin VB.Form dlgAppDbProps 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Configure Installation Image Properties"
   ClientHeight    =   5175
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6000
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5175
   ScaleWidth      =   6000
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txbAppDbDesc 
      Height          =   285
      Left            =   2280
      TabIndex        =   3
      Top             =   4800
      Width           =   2415
   End
   Begin VB.TextBox txbAppDbDate 
      Height          =   285
      Left            =   2280
      TabIndex        =   2
      Top             =   4440
      Width           =   2415
   End
   Begin VB.TextBox txbAppDbOrg 
      Height          =   285
      Left            =   2280
      TabIndex        =   1
      Top             =   4080
      Width           =   2415
   End
   Begin VB.TextBox txbAppDbVer 
      Height          =   285
      Left            =   2280
      TabIndex        =   0
      Top             =   3360
      Width           =   2415
   End
   Begin VB.CommandButton btnOk 
      Caption         =   "Ok"
      Default         =   -1  'True
      Height          =   375
      Left            =   4800
      TabIndex        =   4
      Top             =   120
      Width           =   1095
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4800
      TabIndex        =   5
      Top             =   600
      Width           =   1095
   End
   Begin VB.Label Label17 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbSqlBuildNum:"
      Height          =   255
      Left            =   120
      TabIndex        =   29
      Top             =   1920
      Width           =   2055
   End
   Begin VB.Label Label16 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbSqlMinorVer:"
      Height          =   255
      Left            =   120
      TabIndex        =   28
      Top             =   1560
      Width           =   2055
   End
   Begin VB.Label Label15 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbSqlMajorVer:"
      Height          =   255
      Left            =   120
      TabIndex        =   27
      Top             =   1200
      Width           =   2055
   End
   Begin VB.Label lblAppDbSqlBuildNum 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   26
      Top             =   1920
      Width           =   2415
   End
   Begin VB.Label lblAppDbSqlMinorVer 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   25
      Top             =   1560
      Width           =   2415
   End
   Begin VB.Label lblAppDbSqlMajorVer 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   24
      Top             =   1200
      Width           =   2415
   End
   Begin VB.Label lblAppDbVerSp 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   23
      Top             =   3720
      Width           =   2415
   End
   Begin VB.Label lblAppDbName 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   22
      Top             =   3000
      Width           =   2415
   End
   Begin VB.Label lblAppDbBytesReq 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   21
      Top             =   2640
      Width           =   2415
   End
   Begin VB.Label lblAppDbCsdVer 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   20
      Top             =   2280
      Width           =   2415
   End
   Begin VB.Label lblAppDbUnicodeCompStyle 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   19
      Top             =   840
      Width           =   2415
   End
   Begin VB.Label lblAppDbUnicodeLocaleId 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   18
      Top             =   480
      Width           =   2415
   End
   Begin VB.Label lblAppDbSortOrderId 
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   2280
      TabIndex        =   17
      Top             =   120
      Width           =   2415
   End
   Begin VB.Label Label11 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbDesc:"
      Height          =   255
      Left            =   120
      TabIndex        =   16
      Top             =   4800
      Width           =   2055
   End
   Begin VB.Label Label10 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbDate:"
      Height          =   255
      Left            =   120
      TabIndex        =   15
      Top             =   4440
      Width           =   2055
   End
   Begin VB.Label Label9 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbOrg:"
      Height          =   255
      Left            =   120
      TabIndex        =   14
      Top             =   4080
      Width           =   2055
   End
   Begin VB.Label Label8 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbVerSp:"
      Height          =   255
      Left            =   120
      TabIndex        =   13
      Top             =   3720
      Width           =   2055
   End
   Begin VB.Label Label7 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbVer:"
      Height          =   255
      Left            =   120
      TabIndex        =   12
      Top             =   3360
      Width           =   2055
   End
   Begin VB.Label Label6 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbName:"
      Height          =   255
      Left            =   120
      TabIndex        =   11
      Top             =   3000
      Width           =   2055
   End
   Begin VB.Label Label5 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbBytesReq:"
      Height          =   255
      Left            =   120
      TabIndex        =   10
      Top             =   2640
      Width           =   2055
   End
   Begin VB.Label Label4 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbCsdVer:"
      Height          =   255
      Left            =   120
      TabIndex        =   9
      Top             =   2280
      Width           =   2055
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbUnicodeCompStyle:"
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   840
      Width           =   2055
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbUnicodeLocaleId:"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   480
      Width           =   2055
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "AppDbSortOrderId:"
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   120
      Width           =   2055
   End
End
Attribute VB_Name = "dlgAppDbProps"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mblnError As Boolean
Dim mstrDetails As String

Private Sub btnCancel_Click()
    Unload Me
End Sub

Private Sub btnOk_Click()
    If Trim(txbAppDbVer.Text) = "" Then
        MsgBox "Value required for AppDbVer.", vbOKOnly + vbExclamation, "Value Required"
        txbAppDbVer.SetFocus
        Exit Sub
    End If

    If Trim(txbAppDbOrg.Text = "") Then
        MsgBox "Value required for AppDbOrg.", vbOKOnly + vbExclamation, "Value Required"
        txbAppDbOrg.SetFocus
        Exit Sub
    End If

    If Not IsDate(txbAppDbDate.Text) Then
        MsgBox "Date required for AppDbDate.", vbOKOnly + vbExclamation, "Date Required"
        txbAppDbOrg.SetFocus
        Exit Sub
    End If

    If Trim(txbAppDbDesc.Text = "") Then
        MsgBox "Value required for AppDbDesc.", vbOKOnly + vbExclamation, "Value Required"
        txbAppDbDesc.SetFocus
        Exit Sub
    End If

    If Len(txbAppDbDesc.Text) > 200 Then
        MsgBox "Maximum length for AppDbDesc is 200 characters.", vbOKOnly + vbExclamation, "Value Exceeds Maximum Length"
        txbAppDbDesc.SetFocus
        Exit Sub
    End If
    
    With goAppDb
        .strAppDbVer = txbAppDbVer.Text
        .strAppDbOrg = txbAppDbOrg.Text
        .dtAppDbDate = CDate(txbAppDbDate.Text)
        .strAppDbDesc = txbAppDbDesc.Text
    End With
    
    gblnAppDbPropsConfigured = True
    
    Unload Me
End Sub

Private Sub Form_Activate()
    Dim lngDbSizeBytes As Long
    Dim strAppDbVer As String
    
    Me.MousePointer = vbHourglass
    
    With goAppDb
        mstrDetails = _
            "The wizard has detected the following application database installation image properties: "
            
        ' Detect code page and sort order
        GetSortOrder goDbServer, mblnError, .lngAppDbSortOrderId, mstrDetails
        
        If mblnError Then
            GoTo ErrorDetected
        End If
        
        ' Detect unicode configuration
        GetUnicodeConfig goDbServer, mblnError, .lngAppDbUnicodeLocaleId, .lngAppDbUnicodeCompStyle, mstrDetails
        
        If mblnError Then
            GoTo ErrorDetected
        End If
        
        ' Detect Database Version Info
        GetDbVersion goDbServer, goAppDb, mblnError, mstrDetails
        
        If mblnError Then
            GoTo ErrorDetected
        End If
        
        If goAppDb.strAppDbVer <> "" Then
            mstrDetails = mstrDetails & _
                "Application database version string '" & .strAppDbVer & "' detected; "
        End If
        
        ' Calculate space requirements for installation image
        GetDbSize goDbServer, .strAppDbName, lngDbSizeBytes, mblnError, mstrDetails
        
        If mblnError Then
            GoTo ErrorDetected
        End If
        
        mstrDetails = mstrDetails & _
            "The database is '" & lngDbSizeBytes & "' bytes in size; "
        
        .lngAppDbBytesReq = lngDbSizeBytes * 2
        
        ' Set up dialog
        lblAppDbSortOrderId.Caption = CStr(.lngAppDbSortOrderId)
        lblAppDbUnicodeCompStyle.Caption = CStr(.lngAppDbUnicodeCompStyle)
        lblAppDbUnicodeLocaleId.Caption = CStr(.lngAppDbUnicodeLocaleId)
        lblAppDbSqlMajorVer.Caption = CStr(.lngAppDbSqlMajorVer)
        lblAppDbSqlMinorVer.Caption = CStr(.lngAppDbSqlMinorVer)
        lblAppDbSqlBuildNum.Caption = CStr(.lngAppDbSqlBuildNum)
        lblAppDbCsdVer.Caption = CStr(.lngAppDbCsdVer)
        lblAppDbBytesReq.Caption = CStr(.lngAppDbBytesReq)
        lblAppDbName.Caption = .strAppDbName
        txbAppDbVer.Text = .strAppDbVer
        lblAppDbVerSp.Caption = .strAppDbVerSp
        txbAppDbOrg.Text = .strAppDbOrg
        txbAppDbDate.Text = Format(.dtAppDbDate, "Short Date")
        txbAppDbDesc.Text = .strAppDbDesc
    End With
    Me.MousePointer = vbDefault
    
    txbAppDbVer.SetFocus
    Exit Sub

ErrorDetected:
    Me.MousePointer = vbDefault
    
    MsgBox mstrDetails, vbOKOnly + vbExclamation, "Error"
    WriteLogMsg goLogFileHandle, Me.Name & ": " & mstrDetails
    
    btnOk.Enabled = False
    btnCancel.SetFocus
    Exit Sub
End Sub

Private Sub txbAppDbDate_GotFocus()
    txbAppDbDate.SelStart = 0
    txbAppDbDate.SelLength = Len(txbAppDbDate.Text)
End Sub

Private Sub txbAppDbDesc_GotFocus()
    txbAppDbDesc.SelStart = 0
    txbAppDbDesc.SelLength = Len(txbAppDbDesc.Text)
End Sub

Private Sub txbAppDbOrg_GotFocus()
    txbAppDbOrg.SelStart = 0
    txbAppDbOrg.SelLength = Len(txbAppDbOrg.Text)
End Sub

Private Sub txbAppDbVer_GotFocus()
    txbAppDbVer.SelStart = 0
    txbAppDbVer.SelLength = Len(txbAppDbVer.Text)
End Sub
