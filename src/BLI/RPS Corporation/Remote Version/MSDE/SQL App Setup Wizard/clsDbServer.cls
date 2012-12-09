VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "DbServer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' clsDbServer.cls class module provides data structure for storing information about '
' different types of database servers and service packs handled by the installer.    '
' Defines object of type DBServer used via DBServers collection.                     '
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Option Explicit

Private mlngID As Long ' Unique id used to uniquely identify a database server
Private mstrDescription As String ' Describes the database server
Private mlngMajorVer As Long ' Major version number of database server
Private mlngMinorVer As Long ' Minor version number of database server
Private mlngBuildNum As Long ' RTM Build number of database server, 0 indicates no RTM Build
Private mlngCsdVersionNum As Long ' Service pack version number of database server, 0 indicates no service pack required
Private mstrSetupExeFileSpec As String ' Path and file name of setup executable for database server
Private mstrSetupParms As String ' Command line parameters used for setup executable
Private mstrSetupFile As String ' Name of Setup Initialization File used for setup executable
Private mstrSetupIniData As String ' Value of "Data" Key in "SQL Server - SKUStr" Section in SQLSERVR.INI for database server
Private mstrServerFileSpec As String ' Path and file name of database server executable.  NA indicates not visible in installation image (e.g. MSDE)
Private mdblServerSizeBytes As Double ' Size in bytes of database server executable. NA indicates not visible in installation image (e.g. MSDE)
Private mdblProgramBytes As Double ' Disk space requirements in bytes for database server program files
Private mdblDataBytes As Double ' Disk space requirements in bytes for database server system database files
Private mdblSysBytes As Double ' Disk space requirements in bytes for system files installed with database server on Windows system drive/directory
Private mdblTempBytes As Double ' Disk space requirements in bytes for temporary storage required during installation
Private mdblDbLimitBytes As Double ' Disk space limitation for database storage in bytes, 0 indicates no limit


Public Property Get ID() As Variant
    ID = mlngID
End Property

Public Property Let ID(ByVal varNewId As Variant)
    Static blnAlreadySet As Boolean
    
    If Not blnAlreadySet Then
        If IsNumeric(varNewId) Then
            blnAlreadySet = True
            mlngID = CLng(varNewId)
        Else
            Err.Raise _
                Number:=vbObjectError + 32112, _
                Description:="ID Property Must Be Numeric"
        End If
    End If
End Property

Public Property Get Description() As Variant
    Description = mstrDescription
End Property

Public Property Let Description(ByVal varDescription As Variant)
    mstrDescription = CStr(varDescription)
End Property

Public Property Get MajorVer() As Variant
    MajorVer = mlngMajorVer
End Property

Public Property Let MajorVer(ByVal varMajorVer As Variant)
    If IsNumeric(varMajorVer) Then
        mlngMajorVer = CLng(varMajorVer)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="MajorVer Property Must Be Numeric"
    End If
End Property

Public Property Get MinorVer() As Variant
    MinorVer = mlngMinorVer
End Property

Public Property Let MinorVer(ByVal varMinorVer As Variant)
    If IsNumeric(varMinorVer) Then
        mlngMinorVer = CLng(varMinorVer)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="MinorVer Property Must Be Numeric"
    End If
End Property

Public Property Get BuildNum() As Variant
    BuildNum = mlngBuildNum
End Property
Public Property Let BuildNum(ByVal varData As Variant)
    If IsNumeric(varData) Then
        mlngBuildNum = CLng(varData)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="MinorVer Property Must Be Numeric"
    End If
End Property
Public Property Get CSDVersionNum() As Variant
    CSDVersionNum = mlngCsdVersionNum
End Property
Public Property Let CSDVersionNum(ByVal varData As Variant)
    If IsNumeric(varData) Then
        mlngCsdVersionNum = CLng(varData)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="CSDVersionNum Property Must Be Numeric"
    End If
End Property
Public Property Get SetupExeFileSpec() As Variant
    SetupExeFileSpec = mstrSetupExeFileSpec
End Property

Public Property Let SetupExeFileSpec(ByVal varData As Variant)
    mstrSetupExeFileSpec = CStr(varData)
End Property

Public Property Get SetupParms() As Variant
    SetupParms = mstrSetupParms
End Property

Public Property Let SetupParms(ByVal varSetupParms As Variant)
    mstrSetupParms = CStr(varSetupParms)
End Property

Public Property Get SetupFile() As Variant
    SetupFile = mstrSetupFile
End Property

Public Property Let SetupFile(ByVal varSetupFile As Variant)
    mstrSetupFile = CStr(varSetupFile)
End Property
Public Property Get SetupIniData() As Variant
    SetupIniData = mstrSetupIniData
End Property
Public Property Let SetupIniData(ByVal varSetupIniData As Variant)
    mstrSetupIniData = CStr(varSetupIniData)
End Property

Public Property Get ServerFileSpec() As Variant
    ServerFileSpec = mstrServerFileSpec
End Property

Public Property Let ServerFileSpec(ByVal varData As Variant)
    mstrServerFileSpec = CStr(varData)
End Property
Public Property Get ServerSizeBytes() As Variant
    ServerSizeBytes = mdblServerSizeBytes
End Property

Public Property Let ServerSizeBytes(ByVal varData As Variant)
    If IsNumeric(varData) Then
        mdblServerSizeBytes = CDbl(varData)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="ServerSizeBytes Property Must Be Numeric"
    End If
End Property
Public Property Get ProgramBytes() As Variant
    ProgramBytes = mdblProgramBytes
End Property

Public Property Let ProgramBytes(ByVal varProgramBytes As Variant)
    If IsNumeric(varProgramBytes) Then
        mdblProgramBytes = CDbl(varProgramBytes)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="ProgramBytes Property Must Be Numeric"
    End If
End Property
Public Property Get DataBytes() As Variant
    DataBytes = mdblDataBytes
End Property

Public Property Let DataBytes(ByVal varDataBytes As Variant)
    If IsNumeric(varDataBytes) Then
        mdblDataBytes = CDbl(varDataBytes)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="DataBytes Property Must Be Numeric"
    End If
End Property
Public Property Get SysBytes() As Variant
    SysBytes = mdblSysBytes
End Property

Public Property Let SysBytes(ByVal varSysBytes As Variant)
    If IsNumeric(varSysBytes) Then
        mdblSysBytes = CDbl(varSysBytes)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="SysBytes Property Must Be Numeric"
    End If
End Property

Public Property Get DbLimitBytes() As Variant
    DbLimitBytes = mdblDbLimitBytes
End Property

Public Property Let DbLimitBytes(ByVal varDbLimitBytes As Variant)
    If IsNumeric(varDbLimitBytes) Then
        mdblDbLimitBytes = CDbl(varDbLimitBytes)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="DbLimitBytes Property Must Be Numeric"
    End If
End Property

Public Property Get TempBytes() As Variant
    TempBytes = mdblTempBytes
End Property

Public Property Let TempBytes(ByVal varData As Variant)
    If IsNumeric(varData) Then
        mdblTempBytes = CDbl(varData)
    Else
        Err.Raise _
            Number:=vbObjectError + 32112, _
            Description:="TypeBytes Property Must Be Numeric"
    End If
End Property


