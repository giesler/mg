VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CComponents"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"No"
Attribute VB_Ext_KEY = "Collection" ,"CComponent"
Attribute VB_Ext_KEY = "Member0" ,"CComponent"
'local variable to hold collection
Private mCol As Collection

Public Function Add(pobjComponent As CComponent) As CComponent
    
    'set the properties passed into the method
    mCol.Add pobjComponent, pobjComponent.ID

    'return the object created
    Set Add = pobjComponent
    Set pobjComponent = Nothing

End Function

Public Property Get Item(vntIndexKey As Variant) As CComponent
Attribute Item.VB_UserMemId = 0
    'used when referencing an element in the collection
    'vntIndexKey contains either the Index or Key to the collection,
    'this is why it is declared as a Variant
    'Syntax: Set foo = x.Item(xyz) or Set foo = x.Item(5)
  'find item with this passed ID
  Dim objComp As CComponent
  For Each objComp In mCol
    If objComp.ID = vntIndexKey Then
      Set Item = objComp
      Exit Property
    End If
  Next objComp
  Set Item = Nothing
End Property

Public Property Get ActiveCount() As Long

  Dim i As Long, objComp As CComponent
  For Each objComp In mCol
    If Not objComp.Delete Then i = i + 1
  Next objComp
  ActiveCount = i
  
End Property


Public Property Get Count() As Long
    'used when retrieving the number of elements in the
    'collection. Syntax: Debug.Print x.Count
    Count = mCol.Count
End Property


Public Sub Remove(vntIndexKey As Variant)
    'used when removing an element from the collection
    'vntIndexKey contains either the Index or Key, which is why
    'it is declared as a Variant
    'Syntax: x.Remove(xyz)
  Dim objComp As CComponent
  For Each objComp In mCol
    If objComp.ID = vntIndexKey Then
      objComp.Delete = True
      Exit Sub
    End If
  Next objComp

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

Public Function Sort()
On Error Resume Next

Dim objCol As Collection, iMin As Integer
Dim objComp As CComponent
Set objCol = New Collection

While mCol.Count > 0
  ' find min value
  iMin = -1
  For Each objComp In mCol
    If iMin = -1 Then iMin = objComp.SortOrder
    If iMin > objComp.SortOrder Then
      iMin = objComp.SortOrder
    End If
  Next objComp
  ' now find that component
  For Each objComp In mCol
    If iMin = objComp.SortOrder Then
      Exit For
    End If
  Next objComp
  objCol.Add objComp, objComp.ID
  mCol.Remove objComp.ID
Wend

Set mCol = objCol

End Function
