Attribute VB_Name = "Global"
Option Explicit

'Public variable
Public DomainServer As ListOfServer
Public CurrentServer As String
Public Dummy As Variant


Public Function CrLf(Optional Count) As String
    Dim x As Integer, Text As String
    
    If IsMissing(Count) Then Count = 1
    For x = 1 To Count
        Text = Text & Chr(13) & Chr(10)
    Next
    CrLf = Text
    
End Function

Public Function CompareDates(ByVal lParam1 As Long, _
                             ByVal lParam2 As Long, _
                             ByVal hWnd As Long) As Long
    'CompareDates: This is the sorting routine that gets passed to the
    'ListView control to provide the comparison test for date values.
    'Compare returns:
    ' 0 = Less Than
    ' 1 = Equal
    ' 2 = Greater Than
    
    Dim dDate1 As Date
    Dim dDate2 As Date
    
    'Obtain the item names and dates corresponding to the  'input parameters
    dDate1 = ListView_GetItemDate(lParam1, hWnd)
    dDate2 = ListView_GetItemDate(lParam2, hWnd)
    
    'based on the Public variable sOrder set in the
    'columnheader click sub, sort the dates appropriately:
    Select Case sOrder
        Case True       'sort descending
            If dDate1 < dDate2 Then
                CompareDates = 0
            ElseIf dDate1 = dDate2 Then
                CompareDates = 1
            Else
                CompareDates = 2
            End If
        Case Else        'sort ascending
            If dDate1 > dDate2 Then
                CompareDates = 0
            ElseIf dDate1 = dDate2 Then
                CompareDates = 1
            Else
                CompareDates = 2
            End If
    End Select
        
End Function

Public Function CompareValues(ByVal lParam1 As Long, _
                              ByVal lParam2 As Long, _
                              ByVal hWnd As Long) As Long
    'CompareValues: This is the sorting routine that gets passed to the
    'ListView control to provide the comparison test for numeric values.
    'Compare returns:
    ' 0 = Less Than
    ' 1 = Equal
    ' 2 = Greater Than
    
    Dim val1 As Long
    Dim val2 As Long
    
    'Obtain the item names and values corresponding
    'to the input parameters
    val1 = ListView_GetItemValueStr(hWnd, lParam1)
    val2 = ListView_GetItemValueStr(hWnd, lParam2)
    'based on the Public variable sOrder set in the
    'columnheader click sub, sort the values appropriately:
    Select Case sOrder
        Case True           'sort descending
            If val1 < val2 Then
                CompareValues = 0
            ElseIf val1 = val2 Then
                CompareValues = 1
            Else
                CompareValues = 2
            End If
        Case Else              'sort ascending
            If val1 > val2 Then
                CompareValues = 0
            ElseIf val1 = val2 Then
                CompareValues = 1
            Else
                CompareValues = 2
            End If
    End Select

End Function

Public Function ListView_GetItemDate(lParam As Long, hWnd As Long) As Date
    Dim r As Long
    Dim hIndex As Long
    
    'Convert the input parameter to an index in the list view
    objFind.Flags = LVFI_PARAM
    objFind.lParam = lParam
    hIndex = SendMessageAny(hWnd, LVM_FINDITEM, -1, objFind)
    'Obtain the value of the specified list view item.
    'The objItem.iSubItem member is set to the index
    'of the column that is being retrieved.
    objItem.mask = LVIF_TEXT
    'objItem.iSubItem = 1
    objItem.iSubItem = objIndex
    objItem.pszText = Space$(32)
    objItem.cchTextMax = Len(objItem.pszText)
    'get the string at subitem 1
    r = SendMessageAny(hWnd, LVM_GETITEMTEXT, hIndex, objItem)
    'and convert it into a date and exit
    If r > 0 Then
        ListView_GetItemDate = CDate(Left$(objItem.pszText, r))
    End If
    
End Function

Public Function ListView_GetItemValueStr(hWnd As Long, lParam As Long) As Long
    Dim r As Long
    Dim hIndex As Long
    
    'Convert the input parameter to an index in the list view
    objFind.Flags = LVFI_PARAM
    objFind.lParam = lParam
    hIndex = SendMessageAny(hWnd, LVM_FINDITEM, -1, objFind)
    'Obtain the value of the specified list view item.
    'The objItem.iSubItem member is set to the index
    'of the column that is being retrieved.
    objItem.mask = LVIF_TEXT
    'objItem.iSubItem = 2
    objItem.iSubItem = objIndex
    objItem.pszText = Space$(32)
    objItem.cchTextMax = Len(objItem.pszText)
    'get the string at subitem 2
    r = SendMessageAny(hWnd, LVM_GETITEMTEXT, hIndex, objItem)
    'and convert it into a long
    If r > 0 Then
        ListView_GetItemValueStr = CLng(Left$(objItem.pszText, r))
    End If

End Function

Public Function ErrHand(Optional strLocation As String, Optional strFunction As String)

Select Case Err
  Case Else
    Dim sMsg As String
    sMsg = Err.Description & vbCrLf
    sMsg = sMsg & "Source: " & strLocation & "  (" & strFunction & ")" & vbCrLf
    sMsg = sMsg & "Number: " & Err.Number & " (" & Err.Source & ")"
    MsgBox sMsg, vbExclamation, "Error"
End Select

End Function


