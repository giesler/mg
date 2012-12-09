VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "zl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
'set this to 0 to disable debug code in this class
#Const DebugMode = 0


#If DebugMode Then
    'local variable to hold the serialized class ID that was created in Class_Initialize
    Private mlClassDebugID As Long
#End If
'local variable(s) to hold property value(s)
Private mvarstrSourceFile As String 'local copy
Private mvarstrDestFile As String 'local copy
Private mvarbolCancelled As Boolean 'local copy
Private mvarbolSuccess As Boolean 'local copy

Public Function Expand(strSourceFile As String, strDestFile As String) As Boolean

Dim fE As New frmRunZL
fE.zlocx.InputFile = strSourceFile
fE.zlocx.OutputFile = strDestFile
fE.Caption = "Expanding..."
fE.Visible = True
fE.Refresh
fE.tmrTime.Interval = 1000
fE.zlocx.Decompress
While fE.Visible
    DoEvents
Wend
Expand = True


End Function

Public Function Compress(strSourceFile As String, strDestFile As String) As Boolean

Dim fE As New frmRunZL
fE.zlocx.InputFile = strSourceFile
fE.zlocx.OutputFile = strDestFile
fE.Caption = "Compressing..."
fE.Visible = True
fE.Refresh
fE.tmrTime.Interval = 1000
fE.zlocx.Compress
While fE.Visible
    DoEvents
Wend
Compress = True

End Function

Public Property Let bolSuccess(ByVal vData As Boolean)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.bolSuccess = 5
    mvarbolSuccess = vData
End Property


Public Property Get bolSuccess() As Boolean
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.bolSuccess
    bolSuccess = mvarbolSuccess
End Property



Public Property Let bolCancelled(ByVal vData As Boolean)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.bolCancelled = 5
    mvarbolCancelled = vData
End Property


Public Property Get bolCancelled() As Boolean
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.bolCancelled
    bolCancelled = mvarbolCancelled
End Property


Public Property Let strDestFile(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.strDestFile = 5
    mvarstrDestFile = vData
End Property


Public Property Get strDestFile() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.strDestFile
    strDestFile = mvarstrDestFile
End Property



Public Property Let strSourceFile(ByVal vData As String)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.strSourceFile = 5
    mvarstrSourceFile = vData
End Property


Public Property Get strSourceFile() As String
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.strSourceFile
    strSourceFile = mvarstrSourceFile
End Property

