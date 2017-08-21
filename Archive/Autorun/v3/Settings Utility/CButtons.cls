VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CButtons"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Collection" ,"CButton"
Attribute VB_Ext_KEY = "Member0" ,"CButton"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
'local variable to hold collection
Private mCol As Collection

Public Function Add(pobjButton As CButton) As CButton

  mCol.Add pobjButton, pobjButton.ID
  Set Add = pobjButton

End Function

Public Property Get Item(vntIndexKey As Variant) As CButton
Attribute Item.VB_UserMemId = 0
    'used when referencing an element in the collection
    'vntIndexKey contains either the Index or Key to the collection,
    'this is why it is declared as a Variant
    'Syntax: Set foo = x.Item(xyz) or Set foo = x.Item(5)
  Dim objBtn As CButton
  For Each objBtn In mCol
    If objBtn.ID = vntIndexKey Then
      Set Item = objBtn
      Exit Property
    End If
  Next objBtn
  Set Item = Nothing
End Property

Public Property Get ActiveCount() As Long

  Dim i As Long, objBtn As CButton
  For Each objBtn In mCol
    If Not objBtn.Delete Then i = i + 1
  Next objBtn
  ActiveCount = i
  
End Property

Public Property Get Count() As Long
    'used when retrieving the number of elements in the
    'collection. Syntax: Debug.Print x.Count
    Count = mCol.Count
End Property

Public Property Get DefaultButton() As String

  Dim objBtn As CButton
  For Each objBtn In mCol
    If objBtn.Default Then
      DefaultButton = objBtn.ID
      Exit Property
    End If
  Next objBtn
  DefaultButton = ""
  
End Property

Public Property Get CancelButton() As String

  Dim objBtn As CButton
  For Each objBtn In mCol
    If objBtn.Cancel Then
      CancelButton = objBtn.ID
      Exit Property
    End If
  Next objBtn
  CancelButton = ""
  
End Property

Public Sub Remove(vntIndexKey As Variant)
    'used when removing an element from the collection
    'vntIndexKey contains either the Index or Key, which is why
    'it is declared as a Variant
    'Syntax: x.Remove(xyz)


    mCol.Remove vntIndexKey
End Sub


Public Property Get NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
    'this property allows you to enumerate
    'this collection with the For...Each syntax
    Set NewEnum = mCol.[_NewEnum]
End Property


Private Sub Class_Initialize()
    'creates the collection when this class is created
    Set mCol = New Collection
End Sub


Private Sub Class_Terminate()
    'destroys collection when this class is terminated
    Set mCol = Nothing
End Sub

