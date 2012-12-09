VERSION 5.00
Begin VB.Form dlgWiz17 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "SQL App Setup Wizard (Step 17)"
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
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame Frame2 
      Caption         =   "Details"
      Height          =   1935
      Left            =   120
      TabIndex        =   0
      Top             =   420
      Width           =   5775
      Begin VB.TextBox txbDetails 
         Appearance      =   0  'Flat
         BackColor       =   &H80000004&
         BorderStyle     =   0  'None
         Height          =   1515
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   1
         TabStop         =   0   'False
         Top             =   240
         Width           =   5535
      End
   End
   Begin VB.Frame frameButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   0
      TabIndex        =   6
      Top             =   2520
      Width           =   6015
      Begin VB.CommandButton btnLocate 
         Caption         =   "&Locate"
         Height          =   375
         Left            =   120
         TabIndex        =   2
         Top             =   120
         Width           =   2055
      End
      Begin VB.CommandButton btnNext 
         Caption         =   "&Next >>"
         Default         =   -1  'True
         Height          =   375
         Left            =   3600
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
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton btnPrev 
         Caption         =   "<< &Previous"
         Height          =   375
         Left            =   2400
         TabIndex        =   3
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Locate Application Database Installation Image:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   120
      Width           =   4395
   End
End
Attribute VB_Name = "dlgWiz17"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Step 17: Locate Application Database Installation Image

Option Explicit

Dim mstrDetails As String
Dim mblnLocated As Boolean
Dim mstrAppDbIniFileSpec As String

Private Sub btnCancel_Click()
    Quit True
End Sub

Private Sub btnLocate_Click()
    Dim blnError As Boolean
    Dim strMessage As String
    
    blnError = False
    
    ' Initialize dlgGetPath Variables
    gstrCurDrive = CrackDrive(App.Path)
    gstrCurPath = App.Path & "\" & gstrDefaultAppDbImageFolderName
    
    If Not goFileSystem.FolderExists(gstrCurPath) Then
        gstrCurPath = App.Path
    End If
    
    gblnPathChanged = False
       
'    dlgGetPath.Show vbModal
    gstrCurPath = App.Path & "\fcdata"
        
 '   If gblnPathChanged Then
        If Right(gstrCurPath, 1) = "\" Then
            mstrAppDbIniFileSpec = Left(gstrCurPath, Len(gstrCurPath) - 1) & "\" & gstrAppDbImageIniFileName
        Else
            mstrAppDbIniFileSpec = gstrCurPath & "\" & gstrAppDbImageIniFileName
        End If
        
        If goFileSystem.FileExists(mstrAppDbIniFileSpec) Then
            gstrAppDbIniFileSpec = mstrAppDbIniFileSpec
            gstrAppDbSourceFolder = gstrCurPath
            
            LoadAppDbSettings gstrAppDbIniFileSpec, goDbApp, blnError, mstrDetails
            
            If blnError Then
                mstrDetails = _
                    "Unable to load application database settings from '" & gstrAppDbIniFileSpec & "'; " & _
                    "Click 'Locate' to specify a different application database installation image, " & _
                    "or click 'Cancel' to quit. "
                mblnLocated = False
                UpdateControls
                Exit Sub
            End If
            
            CheckSql blnError, mstrDetails
            
            glngAppDbInst = INSTALL_STATE_NOT_INITIALIZED
            glngAppDbInstallType = APPDBIMAGETYPE_NOT_INITIALIZED
            
            With goDbApp
                gstrAppDbName = .strAppDbName
            
                If blnError Then
                    mblnLocated = False
                    mstrDetails = mstrDetails & _
                        "Unable to detect database server configuration information; " & _
                        "Click 'Cancel' to quit.  :"
                    UpdateControls
                    Exit Sub
                End If
                
                mstrDetails = _
                    "Path to application database installation image located at '" & gstrCurPath & "'; " & _
                    "Application database installation image settings file lcoated at '" & gstrAppDbIniFileSpec & "'; " & vbCrLf & vbCrLf & _
                    "The following application database properties were detected: " & vbCrLf & _
                    "Database name - '" & .strAppDbName & "' " & vbCrLf & _
                    "Organization - '" & .strAppDbOrg & "' " & vbCrLf & _
                    "Date - '" & Format(.dtAppDbDate, "Short Date") & "' " & vbCrLf & _
                    "Description - '" & .strAppDbDesc & "' " & vbCrLf & vbCrLf
                
                ' Version Check
                If (glngSqlMajorVer >= .lngAppDbSqlMajorVer) And (glngSqlMinorVer >= .lngAppDbSqlMinorVer) And (glngSqlBuildNum >= .lngAppDbSqlBuildNum) Then
                    If glngSqlCsdVer >= .lngAppDbCsdVer Then
                        mstrDetails = mstrDetails & _
                           "The version and service pack level of the local database server " & _
                           "are compatible with the application database; "
                    Else
                        mstrDetails = mstrDetails & _
                            "The service pack level of the local database server is older than " & _
                            "the application database; You may wish to install service pack '" & _
                            CStr(.lngAppDbCsdVer) & "' before installing the application database; "
                    End If
                Else
                    mstrDetails = mstrDetails & _
                        "The local database server version is older than the application database; " & _
                        "You may wish to upgrade your database server to version '" & _
                        CStr(.lngAppDbSqlMajorVer) & "." & CStr(.lngAppDbSqlMinorVer) & "." & CStr(.lngAppDbSqlBuildNum) & "' " & _
                        "before installing the application database; "
                End If
            End With
            
            mstrDetails = mstrDetails & vbCrLf & vbCrLf & _
                "Click 'Next' to proceed."

            
            mblnLocated = True
        Else
            mstrDetails = _
                "The path " & gstrCurPath & " does not contain a valid application database installation image. " & _
                "Click 'Locate' to try again, or press 'Cancel' to quit."
        End If
'    End If
       
    UpdateControls
End Sub

Private Sub btnNext_Click()
    Dim strFormName As String
    
    strFormName = Me.Name
    WriteLogMsg goLogFileHandle, strFormName & ": " & mstrDetails
    
    Unload Me
    
    mstrDetails = ""
    
    If gblnSqlPreInstalled Then
        mstrDetails = _
            strFormName & ": Application database installation pre-requisites will be checked because the database server was pre-installed. Proceeding to step 18."
        WriteLogMsg goLogFileHandle, mstrDetails
        dlgWiz18.Show vbModal
    Else
        mstrDetails = _
            strFormName & ": Application database installation pre-requisites will not be checked because the database server was not pre-installed. Proceeding to step 21."
        WriteLogMsg goLogFileHandle, mstrDetails
        glngAppDbInstallType = FILE_IMAGE
        dlgWiz21.Show vbModal
    End If
End Sub

Private Sub btnPrev_Click()
    Unload Me
    dlgWiz02.Show ' vbModal
End Sub
Private Sub Form_Load()
    mblnLocated = False
    mstrDetails = _
        "Setup will now proceed with application database installation. " & _
        "Click 'Locate' to to specify the location of the application database installation " & _
        "image you wish to install. "
End Sub

Private Sub Form_Activate()
    UpdateControls
    Me.btnLocate.Visible = False
    btnLocate_Click
    btnNext_Click
End Sub

Private Sub UpdateControls()
    txbDetails.Text = mstrDetails
    
    If mblnLocated Then
        btnLocate.Enabled = False
        btnPrev.Enabled = True
        btnNext.Enabled = True
        btnCancel.Enabled = True
        btnNext.SetFocus
    Else
        btnLocate.Enabled = True
        btnPrev.Enabled = True
        btnNext.Enabled = False
        btnCancel.Enabled = True
        Me.btnLocate.Enabled = True
    End If
End Sub


