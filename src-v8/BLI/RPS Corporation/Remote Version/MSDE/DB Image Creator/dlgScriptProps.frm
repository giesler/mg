VERSION 5.00
Begin VB.Form dlgScriptProps 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Script Properties"
   ClientHeight    =   2505
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7185
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2505
   ScaleWidth      =   7185
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton btnOk 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   5880
      TabIndex        =   8
      Top             =   120
      Width           =   1215
   End
   Begin VB.CommandButton btnCancel 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   5880
      TabIndex        =   9
      Top             =   600
      Width           =   1215
   End
   Begin VB.Frame Frame2 
      Caption         =   "Table Scripting Options"
      Height          =   1095
      Left            =   120
      TabIndex        =   4
      Top             =   1320
      Width           =   5655
      Begin VB.CheckBox chkScriptDRI 
         Caption         =   "Script PRIMARY Keys, FOREIGN Keys, Defaults and Check Constraints"
         Height          =   255
         Left            =   120
         TabIndex        =   7
         Top             =   720
         Width           =   5415
      End
      Begin VB.CheckBox chkScriptTriggers 
         Caption         =   "Script Triggers"
         Height          =   255
         Left            =   120
         TabIndex        =   6
         Top             =   480
         Width           =   5295
      End
      Begin VB.CheckBox chkScriptIndexes 
         Caption         =   "Script Indexes"
         Height          =   255
         Left            =   120
         TabIndex        =   5
         Top             =   240
         Width           =   1815
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Security Scripting Options"
      Height          =   1095
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5655
      Begin VB.CheckBox chkScriptPermissions 
         Caption         =   "Script Permissions"
         Height          =   255
         Left            =   120
         TabIndex        =   3
         Top             =   720
         Width           =   2775
      End
      Begin VB.CheckBox chkScriptLogins 
         Caption         =   "Script SQL Server Logins"
         Height          =   255
         Left            =   120
         TabIndex        =   2
         Top             =   480
         Width           =   2175
      End
      Begin VB.CheckBox chkScriptUsersAndRoles 
         Caption         =   "Script Database Users and Database Roles"
         Height          =   255
         Left            =   120
         TabIndex        =   1
         Top             =   240
         Width           =   3495
      End
   End
End
Attribute VB_Name = "dlgScriptProps"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub btnCancel_Click()
    Unload Me
End Sub

Private Sub btnOk_Click()
    
    If chkScriptDRI.Value = 1 Then
        gblnScriptDRI = True
    Else
        gblnScriptDRI = False
    End If
    
    If chkScriptIndexes.Value = 1 Then
        gblnScriptIndexes = True
    Else
        gblnScriptIndexes = False
    End If
    
    If chkScriptLogins.Value = 1 Then
        gblnScriptLogins = True
    Else
        gblnScriptLogins = False
    End If
    
    If chkScriptPermissions.Value = 1 Then
        gblnScriptPermissions = True
    Else
        gblnScriptPermissions = False
    End If
    
    If chkScriptTriggers.Value Then
        gblnScriptTriggers = True
    Else
        gblnScriptTriggers = False
    End If
    
    If chkScriptUsersAndRoles.Value = 1 Then
        gblnScriptUsersAndRoles = True
    Else
        gblnScriptUsersAndRoles = False
    End If
    
    Unload Me
End Sub

Private Sub Form_Load()
    If gblnScriptDRI Then
        chkScriptDRI.Value = 1
    Else
        chkScriptDRI.Value = 0
    End If
    
    If gblnScriptIndexes Then
        chkScriptIndexes.Value = 1
    Else
        chkScriptIndexes.Value = 0
    End If
    
    If gblnScriptLogins Then
        chkScriptLogins.Value = 1
    Else
        chkScriptLogins.Value = 0
    End If
    
    If gblnScriptPermissions Then
        chkScriptPermissions.Value = 1
    Else
        chkScriptPermissions.Value = 0
    End If
    
    If gblnScriptTriggers Then
        chkScriptTriggers.Value = 1
    Else
        chkScriptTriggers.Value = 0
    End If
    
    If gblnScriptUsersAndRoles Then
        chkScriptUsersAndRoles.Value = 1
    Else
        chkScriptUsersAndRoles.Value = 0
    End If
End Sub
