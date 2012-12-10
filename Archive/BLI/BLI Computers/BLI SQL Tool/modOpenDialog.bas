Attribute VB_Name = "modOpenDialog"
Option Explicit

Declare Function GetOpenFileName Lib "comdlg32.dll" Alias "GetOpenFileNameA" (pOpenfilename As OPENFILENAME) As Boolean
Declare Function GetSaveFileName Lib "comdlg32.dll" Alias "GetSaveFileNameA" (pOpenfilename As OPENFILENAME) As Boolean

Public Type MSA_OPENFILENAME
    ' Filter string used for the File Open dialog filters.
    ' Use MSA_CreateFilterString() to create this.
    ' Default = All Files, *.*
    strFilter As String
    ' Initial Filter to display.
    ' Default = 1.
    lngFilterIndex As Long
    ' Initial directory for the dialog to open in.
    ' Default = Current working directory.
    strInitialDir As String
    ' Initial file name to populate the dialog with.
    ' Default = "".
    strInitialFile As String
    strDialogTitle As String
    ' Default extension to append to file if user didn't specify one.
    ' Default = System Values (Open File, Save File).
    strDefaultExtension As String
    ' Flags (see constant list) to be used.
    ' Default = no flags.
    lngFlags As Long
    ' Full path of file picked.  On OpenFile, if the user picks a
    ' nonexistent file, only the text in the "File Name" box is returned.
    strFullPathReturned As String
    ' File name of file picked.
    strFileNameReturned As String
    ' Offset in full path (strFullPathReturned) where the file name
    ' (strFileNameReturned) begins.
    intFileOffset As Integer
    ' Offset in full path (strFullPathReturned) where the file extension begins.
    intFileExtension As Integer
End Type

Const ALLFILES = "All Files"

Type OPENFILENAME
    lStructSize As Long
    hwndOwner As Long
    hInstance As Long
    lpstrFilter As String
    lpstrCustomFilter As Long
    nMaxCustrFilter As Long
    nFilterIndex As Long
    lpstrFile As String
    nMaxFile As Long
    lpstrFileTitle As String
    nMaxFileTitle As Long
    lpstrInitialDir As String
    lpstrTitle As String
    flags As Long
    nFileOffset As Integer
    nFileExtension As Integer
    lpstrDefExt As String
    lCustrData As Long
    lpfnHook As Long
    lpTemplateName As Long
End Type

Const OFN_ALLOWMULTISELECT = &H200
Const OFN_CREATEPROMPT = &H2000
Const OFN_EXPLORER = &H80000
Const OFN_FILEMUSTEXIST = &H1000
Const OFN_HIDEREADONLY = &H4
Const OFN_NOCHANGEDIR = &H8
Const OFN_NODEREFERENCELINKS = &H100000
Const OFN_NONETWORKBUTTON = &H20000
Const OFN_NOREADONLYRETURN = &H8000
Const OFN_NOVALIDATE = &H100
Const OFN_OVERWRITEPROMPT = &H2
Const OFN_PATHMUSTEXIST = &H800
Const OFN_READONLY = &H1
Const OFN_SHOWHELP = &H10

Function MSA_CreateFilterString(ParamArray varFilt() As Variant) As String
' Creates a filter string from the passed in arguments.
' Returns "" if no args are passed in.
' Expects an even number of args (filter name, extension), but
' if an odd number is passed in, it appends *.*
    
    Dim strFilter As String
    Dim intRet As Integer
    Dim intNum As Integer

    intNum = UBound(varFilt)
    If (intNum <> -1) Then
        For intRet = 0 To intNum
            strFilter = strFilter & varFilt(intRet) & Chr$(0)
        Next
        If intNum Mod 2 = 0 Then
            strFilter = strFilter & "*.*" & Chr$(0)
        End If
        
        strFilter = strFilter & Chr$(0)
    Else
        strFilter = ""
    End If

    MSA_CreateFilterString = strFilter
End Function

Function MSA_ConvertFilterString(strFilterIn As String) As String
' Creates a filter string from a bar ("|") separated string.
' The string should pairs of filter|extension strings, i.e. "Access Databases|*.mdb|All Files|*.*"
' If no extensions exists for the last filter pair, *.* is added.
' This code will ignore any empty strings, i.e. "||" pairs.
' Returns "" if the strings passed in is empty.

    
    Dim strFilter As String
    Dim intNum As Integer, intPos As Integer, intLastPos As Integer

    strFilter = ""
    intNum = 0
    intPos = 1
    intLastPos = 1

    ' Add strings as long as we find bars.
    ' Ignore any empty strings (not allowed).
    Do
        intPos = InStr(intLastPos, strFilterIn, "|")
        If (intPos > intLastPos) Then
            strFilter = strFilter & Mid$(strFilterIn, intLastPos, intPos - intLastPos) & Chr$(0)
            intNum = intNum + 1
            intLastPos = intPos + 1
        ElseIf (intPos = intLastPos) Then
            intLastPos = intPos + 1
        End If
    Loop Until (intPos = 0)
        
    ' Get last string if it exists (assuming strFilterIn was not bar terminated).
    intPos = Len(strFilterIn)
    If (intPos >= intLastPos) Then
        strFilter = strFilter & Mid$(strFilterIn, intLastPos, intPos - intLastPos + 1) & Chr$(0)
        intNum = intNum + 1
    End If
    
    ' Add *.* if there's no extension for the last string.
    If intNum Mod 2 = 1 Then
        strFilter = strFilter & "*.*" & Chr$(0)
    End If
    
    ' Add terminating NULL if we have any filter.
    If strFilter <> "" Then
        strFilter = strFilter & Chr$(0)
    End If
    
    MSA_ConvertFilterString = strFilter
End Function

Public Function MSA_GetSaveFileName(msaof As MSA_OPENFILENAME) As Integer
' Opens the file save dialog.
    
    Dim of As OPENFILENAME
    Dim intRet As Integer

    MSAOF_to_OF msaof, of
    of.flags = of.flags Or OFN_HIDEREADONLY
    intRet = GetSaveFileName(of)
    If intRet Then
        OF_to_MSAOF of, msaof
    End If
    MSA_GetSaveFileName = intRet
End Function

Function MSA_SimpleGetSaveFileName() As String
' Opens the file save dialog with default values.

    Dim msaof As MSA_OPENFILENAME
    Dim intRet As Integer
    Dim strRet As String
    
    intRet = MSA_GetSaveFileName(msaof)
    If intRet Then
        strRet = msaof.strFullPathReturned
    End If
    
    MSA_SimpleGetSaveFileName = strRet
End Function

Public Function MSA_GetOpenFileName(msaof As MSA_OPENFILENAME) As Integer
' Opens the file open dialog.
    
    Dim of As OPENFILENAME
    Dim intRet As Integer

    MSAOF_to_OF msaof, of
    intRet = GetOpenFileName(of)
    If intRet Then
        OF_to_MSAOF of, msaof
    End If
    MSA_GetOpenFileName = intRet
End Function

Function MSA_SimpleGetOpenFileName() As String
' Opens the file open dialog with default values.

    Dim msaof As MSA_OPENFILENAME
    Dim intRet As Integer
    Dim strRet As String
    
    intRet = MSA_GetOpenFileName(msaof)
    If intRet Then
        strRet = msaof.strFullPathReturned
    End If
    
    MSA_SimpleGetOpenFileName = strRet
End Function

Private Sub OF_to_MSAOF(of As OPENFILENAME, msaof As MSA_OPENFILENAME)
' This sub converts from the win32 structure to the friendly MSAccess structure.
    
    msaof.strFullPathReturned = Left$(of.lpstrFile, InStr(of.lpstrFile, Chr$(0)))
    msaof.strFileNameReturned = of.lpstrFileTitle
    msaof.intFileOffset = of.nFileOffset
    msaof.intFileExtension = of.nFileExtension
End Sub


Private Sub MSAOF_to_OF(msaof As MSA_OPENFILENAME, of As OPENFILENAME)
' This sub converts from the friendly MSAccess structure to the win32 structure.
    
    Dim strFile As String * 512

    ' Initialize some parts of the structure.
    of.hInstance = 0
    of.lpstrCustomFilter = 0
    of.nMaxCustrFilter = 0
    of.lpfnHook = 0
    of.lpTemplateName = 0
    of.lCustrData = 0
    
    If msaof.strFilter = "" Then
        of.lpstrFilter = MSA_CreateFilterString(ALLFILES)
    Else
        of.lpstrFilter = msaof.strFilter
    End If
    of.nFilterIndex = msaof.lngFilterIndex
    
    of.lpstrFile = msaof.strInitialFile & String$(512 - Len(msaof.strInitialFile), 0)
    of.nMaxFile = 511

    of.lpstrFileTitle = String$(512, 0)
    of.nMaxFileTitle = 511

    of.lpstrTitle = msaof.strDialogTitle

    of.lpstrInitialDir = msaof.strInitialDir
    
    of.lpstrDefExt = msaof.strDefaultExtension

    of.flags = msaof.lngFlags
    
    of.lStructSize = Len(of)
End Sub

