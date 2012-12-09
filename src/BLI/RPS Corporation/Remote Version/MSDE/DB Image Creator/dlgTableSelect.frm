VERSION 5.00
Begin VB.Form dlgTableSelect 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Select Tables"
   ClientHeight    =   3195
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6030
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3195
   ScaleWidth      =   6030
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame Frame2 
      Caption         =   "Tables"
      Height          =   3015
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   2895
      Begin VB.CommandButton btnRefresh 
         Caption         =   "&Refresh"
         Height          =   375
         Left            =   120
         Picture         =   "dlgTableSelect.frx":0000
         TabIndex        =   2
         Top             =   2520
         Width           =   975
      End
      Begin VB.CommandButton btnSelAll 
         Caption         =   "&Select All"
         Height          =   375
         Left            =   1200
         TabIndex        =   3
         Top             =   2520
         Width           =   975
      End
      Begin VB.CommandButton btnUp 
         Appearance      =   0  'Flat
         BeginProperty Font 
            Name            =   "System"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   2280
         Picture         =   "dlgTableSelect.frx":0442
         Style           =   1  'Graphical
         TabIndex        =   4
         ToolTipText     =   "Move Up"
         Top             =   720
         Width           =   495
      End
      Begin VB.CommandButton btnDown 
         Appearance      =   0  'Flat
         Height          =   495
         Left            =   2280
         Picture         =   "dlgTableSelect.frx":0884
         Style           =   1  'Graphical
         TabIndex        =   5
         ToolTipText     =   "Move Down"
         Top             =   1440
         Width           =   495
      End
      Begin VB.ListBox lstTables 
         Height          =   2085
         Left            =   120
         Style           =   1  'Checkbox
         TabIndex        =   1
         Top             =   240
         Width           =   2055
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Details"
      Height          =   2295
      Left            =   3120
      TabIndex        =   6
      Top             =   120
      Width           =   2775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1935
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   7
         TabStop         =   0   'False
         Top             =   240
         Width           =   2535
      End
   End
   Begin VB.CommandButton btnCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4680
      TabIndex        =   9
      Top             =   2640
      Width           =   1215
   End
   Begin VB.CommandButton btnOk 
      Caption         =   "Ok"
      Default         =   -1  'True
      Height          =   375
      Left            =   3120
      TabIndex        =   8
      Top             =   2640
      Width           =   1215
   End
End
Attribute VB_Name = "dlgTableSelect"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Option Base 1

Dim mstrTables() As String
Dim mstrDetails As String
Dim mlngTableCount As Long
Dim mblnError As Boolean

Private Sub btnCancel_Click()
    Unload Me
End Sub


Private Sub btnOk_Click()
    Dim i As Long
    Dim blnError As Boolean
    
    blnError = False
    mstrDetails = ""
    
    With lstTables
        If .ListCount > 0 Then
            ReDim mstrTables(1)
            mlngTableCount = 0
    
            For i = 0 To (.ListCount - 1)
                If .Selected(i) Then
                    mlngTableCount = mlngTableCount + 1
                    ReDim Preserve mstrTables(mlngTableCount)
                    mstrTables(mlngTableCount) = .List(i)
                End If
            Next i
            
            Me.MousePointer = vbHourglass
            CheckTableDependencies goDbServer, goAppDb.strAppDbName, mstrTables, mlngTableCount, blnError, mstrDetails
            Me.MousePointer = vbDefault
            
            If blnError Then
                MsgBox "Dependency error.  See 'Details' for more information.", vbOKOnly + vbExclamation, "Dependency Error Detected"
                UpdateControls
                Exit Sub
            Else
                gstrLoadTables = mstrTables
                glngLoadTableCount = mlngTableCount
            End If
        End If
    End With
        
    Unload Me
End Sub

Private Sub btnRefresh_Click()
    Dim blnError As Boolean
    
    blnError = False
    RefreshTableList blnError, mstrDetails
    UpdateControls
    
End Sub

Private Sub btnSelAll_Click()
    Dim i As Long
    
    With lstTables
        For i = 0 To (.ListCount - 1)
            .Selected(i) = True
        Next i
        
        .ListIndex = 0
    End With
End Sub

Private Sub btnUp_Click()
    Dim strTableName As String
    Dim lngOldListIdx As Long
    Dim lngNewListIdx As String
    Dim blnSelected As Boolean
    
    With lstTables
        lngOldListIdx = .ListIndex
        blnSelected = .Selected(lngOldListIdx)
        Select Case lngOldListIdx
            Case -1, 0
                Exit Sub
            Case Else
                strTableName = .List(lngOldListIdx)
                lngNewListIdx = lngOldListIdx - 1
                .RemoveItem lngOldListIdx
                .AddItem strTableName, lngNewListIdx
                .Selected(lngNewListIdx) = blnSelected
                .ListIndex = lngNewListIdx
                .TopIndex = lngNewListIdx
        End Select
    End With
End Sub
Private Sub btnDown_Click()
    Dim strTableName As String
    Dim lngOldListIdx As Long
    Dim lngNewListIdx As String
    Dim blnSelected As Boolean
    
    With lstTables
        lngOldListIdx = .ListIndex
        blnSelected = .Selected(lngOldListIdx)
        Select Case lngOldListIdx
            Case -1, (.ListCount - 1)
                Exit Sub
            Case Else
                strTableName = .List(lngOldListIdx)
                lngNewListIdx = lngOldListIdx + 1
                .RemoveItem lngOldListIdx
                .AddItem strTableName, lngNewListIdx
                .Selected(lngNewListIdx) = blnSelected
                .ListIndex = lngNewListIdx
                .TopIndex = lngNewListIdx
        End Select
    End With
End Sub

Private Sub Form_Load()
    Dim strTableName As Variant
    Dim i As Long
    
    mblnError = False
    mstrDetails = ""
    lstTables.Clear
    
    If glngLoadTableCount = 0 Then
        RefreshTableList mblnError, mstrDetails
        
        If mblnError Then
            mstrDetails = mstrDetails & _
                "Click 'Cancel'."
            Exit Sub
        End If
    Else
        With lstTables
            For i = 0 To (glngLoadTableCount - 1)
                .AddItem gstrLoadTables(i + 1), i
                .Selected(i) = True
            Next
            
            .ListIndex = 0
        End With
        
    End If
    
    mstrDetails = _
        "Check each table you wish to load during script-based application database " & _
        "installation.  Use the up and down arrows " & _
        "to adjust loading order to avoid problems with referential integrity violations. " & _
        "Note that only tables which contain rows are listed. "
    
End Sub
Private Sub Form_Activate()
    UpdateControls
End Sub
Private Sub RefreshTableList(blnError As Boolean, strMessage As String)
    Dim strTableName As Variant
    
    blnError = False
    mlngTableCount = 0
    
    LoadTableNames goDbServer, goAppDb.strAppDbName, mstrTables, mlngTableCount, blnError, strMessage
    
    If blnError Then
        Exit Sub
    End If
    
    If mlngTableCount = 0 Then
        strMessage = strMessage & _
            "There are no user tables in the '" & goAppDb.strAppDbName & "' database; "
        blnError = True
        Exit Sub
    End If
    
    lstTables.Clear
    
    For Each strTableName In mstrTables
        lstTables.AddItem CStr(strTableName)
    Next
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    If Not mblnError Then
        btnOk.Enabled = True
        btnCancel.Enabled = True
        btnRefresh.Enabled = True
        btnUp.Enabled = True
        btnDown.Enabled = True
        btnOk.SetFocus
    Else
        btnOk.Enabled = False
        btnCancel.Enabled = True
        btnRefresh.Enabled = False
        btnUp.Enabled = False
        btnDown.Enabled = False
        btnCancel.SetFocus
    End If
End Sub
