Attribute VB_Name = "modMisc"
Public Const NOERROR = 0

' Retrieves the IShellFolder interface for the desktop folder.
' Returns NOERROR if successful or an OLE-defined error
' result otherwise.
Declare Function SHGetDesktopFolder Lib "shell32" _
                                (ppshf As IShellFolder) As Long

' Returns an absolute pidl (relative to the desktop) from a valid
' file system path only (i.e. not from a display name).

'   hwndOwner - owner of any displayed msg boxes
'   sPath         - fully qualified path whose pidl is to be returned,
'                      can also be a virtual folder's fully qualified CLSID

' If successful, the path's pidl is returned, otherwise 0 is returned.
' (calling proc is responsible for freeing the pidl)

Public Function GetPIDLFromPath(hwndOwner As Long, _
                                            sPath As String) As Long
  Dim isfDesktop As IShellFolder
  Dim pchEaten As Long
  Dim pidl As Long

  If (SHGetDesktopFolder(isfDesktop) = NOERROR) Then
    If (isfDesktop.ParseDisplayName(hwndOwner, 0, _
                                            StrConv(sPath, vbUnicode), _
                                            pchEaten, _
                                            pidl, 0) = NOERROR) Then
      GetPIDLFromPath = pidl
    End If
  End If

End Function


