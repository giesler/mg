Attribute VB_Name = "NetGroupAPI"
Option Explicit

Declare Function NetGroupEnum Lib "netapi32" _
        (lpServer As Any, ByVal Level As Long, lpBuffer As Long, _
         ByVal MaxLen As Long, lpEntriesRead As Long, _
         lpTotalEntries As Long, vResume As Any) As Long
                       
Declare Function NetGroupGetInfo Lib "netapi32" _
        (lpServer As Any, lpGroup As Any, _
         ByVal Level As Long, lpBuffer As Long) As Long
                       
Declare Function NetLocalGroupEnum Lib "netapi32" _
        (lpServer As Any, ByVal Level As Long, lpBuffer As Long, _
         ByVal MaxLen As Long, lpEntriesRead As Long, _
         lpTotalEntries As Long, vResume As Any) As Long
                       
Declare Function NetGroupGetUsers Lib "netapi32" _
        (lpServer As Any, lpGroup As Any, ByVal Level As Long, _
         lpBuffer As Long, ByVal MaxLen As Long, lpEntriesRead As Long, _
         lpTotalEntries As Long, vResume As Any) As Long
                       
Declare Function NetLocalGroupGetMembers Lib "netapi32" _
        (lpServer As Any, lpLocalGroup As Any, ByVal Level As Long, _
         lpBuffer As Long, ByVal MaxLen As Long, lpEntriesRead As Long, _
         lpTotalEntries As Long, vResume As Any) As Long
                       
Declare Function NetUserGetGroups Lib "netapi32" _
        (lpServer As Any, UserName As Byte, _
         ByVal Level As Long, lpBuffer As Long, _
         ByVal PrefMaxLen As Long, lpEntriesRead As Long, _
         lpTotalEntries As Long) As Long
         
Declare Function NetUserGetLocalGroups Lib "netapi32" _
        (lpServer As Any, UserName As Byte, _
        ByVal Level As Long, ByVal Flags As Long, _
        lpBuffer As Long, ByVal MaxLen As Long, _
        lpEntriesRead As Long, _
        lpTotalEntries As Long) As Long


Private Type GROUP_INFO_0_API
    Name As Long
End Type

Type GroupInfo0
    Name As String
End Type

Type ListOfGroup0
    Init As Boolean
    LastErr As Long
    List() As GroupInfo0
End Type


Private Type GROUP_INFO_API
    Name As Long
    Comment As Long
End Type

Type GroupInfo
    Name As String
    Comment As String
End Type

Type ListOfGroup
    Init As Boolean
    LastErr As Long
    List() As GroupInfo
End Type


Private Type GROUP_INFO_2_API
   Name As Long
   Comment As Long
   GroupID As Long
   Attributes As Long
End Type


Private Type GLOBALGROUP_MEMBERS_INFO_API
    UserName As Long
    Attribute As Long
End Type

Private Type LOCALGROUP_MEMBERS_INFO_API
    Sid As Long
    Attribute As Long
    UserName As Long
End Type

Type GroupMemberInfo
    UserName As String
    Attribute As Long
End Type


Type ListOfMemberGroup
    Init As Boolean
    LastErr As Long
    List() As GroupMemberInfo
End Type

Private Const LG_INCLUDE_INDIRECT As Long = &H1&

'Ritorna un elenco dei gruppi globali di un Dominio NT
Public Function EnumGlobalGroups() As ListOfGroup
    Dim yServer() As Byte, lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long, nResume As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim tGroupInfo As GROUP_INFO_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    Dim GroupInfo As GroupInfo
    Dim GrpList As ListOfGroup
    
    yServer = MakeServerName(ByVal GetPDCName())
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        GrpList.Init = True
        nRet = NetGroupEnum(yServer(0), 1, lGroupInfo, _
                            PrefMaxLen, nRead, nTotal, _
                            nResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            GrpList.Init = False
            GrpList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        lGroupInfoPtr = lGroupInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
            
            GroupInfo.Name = PointerToStringW(tGroupInfo.Name)
            GroupInfo.Comment = PointerToStringW(tGroupInfo.Comment)
            i = i + 1
            ReDim Preserve GrpList.List(1 To i) As GroupInfo
            GrpList.List(i) = GroupInfo
            x = x + 1
                
            lGroupInfoPtr = lGroupInfoPtr + Len(tGroupInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lGroupInfo)
    Loop
        
    EnumGlobalGroups = GrpList
    
End Function

'Ritorna un elenco dei gruppi locali di un server NT
Public Function EnumLocalGroups(Server As String) As ListOfGroup
    Dim yServer() As Byte, lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long, nResume As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim tGroupInfo As GROUP_INFO_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    Dim GroupInfo As GroupInfo
    Dim GrpList As ListOfGroup
        
    yServer = MakeServerName(ByVal Server)
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        
        nRet = NetLocalGroupEnum(yServer(0), 1, lGroupInfo, _
                                 PrefMaxLen, nRead, nTotal, _
                                 nResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            GrpList.Init = False
            GrpList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        lGroupInfoPtr = lGroupInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
            
            GroupInfo.Name = PointerToStringW(tGroupInfo.Name)
            GroupInfo.Comment = PointerToStringW(tGroupInfo.Comment)
            i = i + 1
            ReDim Preserve GrpList.List(1 To i) As GroupInfo
            GrpList.List(i) = GroupInfo
            x = x + 1
                
            lGroupInfoPtr = lGroupInfoPtr + Len(tGroupInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lGroupInfo)
        GrpList.Init = (x > 1)
    
    Loop
        
    EnumLocalGroups = GrpList
    
End Function

'Ritorna un elenco dei gruppi locali di cui un utente fa parte
Public Function GetUserLocalGroups(Server As String, _
                                   User As String, _
                                   bIndirect As Boolean) As ListOfGroup0
    Dim yServer() As Byte
    Dim yUser() As Byte
    Dim lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long
    Dim PrefMaxLen As Long, Flags As Long
    Dim i As Long, x As Long
    Dim tGroupInfo As GROUP_INFO_0_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    Dim GroupInfo As GroupInfo0
    Dim GrpList As ListOfGroup0
        
    If bIndirect Then
        Flags = LG_INCLUDE_INDIRECT
    End If
    
    yServer = MakeServerName(ByVal Server)
    yUser = User & vbNullChar
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        
        nRet = NetUserGetLocalGroups(yServer(0), _
                                     yUser(0), _
                                     0, _
                                     Flags, _
                                     lGroupInfo, _
                                     PrefMaxLen, _
                                     nRead, _
                                     nTotal)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            GrpList.Init = False
            GrpList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        lGroupInfoPtr = lGroupInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
            
            GroupInfo.Name = PointerToStringW(tGroupInfo.Name)
            i = i + 1
            ReDim Preserve GrpList.List(1 To i) As GroupInfo0
            GrpList.List(i) = GroupInfo
            x = x + 1
                
            lGroupInfoPtr = lGroupInfoPtr + Len(tGroupInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lGroupInfo)
        GrpList.Init = (x > 1)
    
    Loop
        
    GetUserLocalGroups = GrpList
    
End Function

'Ritorna un elenco dei gruppi globali di cui un utente fa parte
Public Function GetUserGroups(Server As String, User As String) As ListOfGroup0
    Dim yServer() As Byte
    Dim yUser() As Byte
    Dim lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim tGroupInfo As GROUP_INFO_0_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    Dim GroupInfo As GroupInfo0
    Dim GrpList As ListOfGroup0
        
    yServer = MakeServerName(ByVal Server)
    yUser = User & vbNullChar
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        
        nRet = NetUserGetGroups(yServer(0), _
                                yUser(0), _
                                0, _
                                lGroupInfo, _
                                PrefMaxLen, _
                                nRead, _
                                nTotal)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            GrpList.Init = False
            GrpList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        lGroupInfoPtr = lGroupInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
            
            GroupInfo.Name = PointerToStringW(tGroupInfo.Name)
            i = i + 1
            ReDim Preserve GrpList.List(1 To i) As GroupInfo0
            GrpList.List(i) = GroupInfo
            x = x + 1
                
            lGroupInfoPtr = lGroupInfoPtr + Len(tGroupInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lGroupInfo)
        GrpList.Init = (x > 1)
    
    Loop
        
    GetUserGroups = GrpList
    
End Function

'Ritorna un elenco dei membri di un gruppo locale
Public Function LocalGroupsGetMember(Server As String, Group As String) As ListOfMemberGroup
    Dim yServer() As Byte
    Dim yGroup() As Byte
    Dim lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long, nResume As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim tGroupInfo As LOCALGROUP_MEMBERS_INFO_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    Dim GroupInfo As GroupMemberInfo
    Dim GrpList As ListOfMemberGroup
        
    yServer = MakeServerName(ByVal Server)
    yGroup = Group & vbNullChar
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        
        nRet = NetLocalGroupGetMembers(yServer(0), _
                                       yGroup(0), _
                                       1, _
                                       lGroupInfo, _
                                       PrefMaxLen, _
                                       nRead, _
                                       nTotal, _
                                       nResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            GrpList.Init = False
            GrpList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        lGroupInfoPtr = lGroupInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
            
            GroupInfo.UserName = PointerToStringW(tGroupInfo.UserName)
            GroupInfo.Attribute = tGroupInfo.Attribute
            i = i + 1
            ReDim Preserve GrpList.List(1 To i) As GroupMemberInfo
            GrpList.List(i) = GroupInfo
            x = x + 1
                
            lGroupInfoPtr = lGroupInfoPtr + Len(tGroupInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lGroupInfo)
        GrpList.Init = (x > 1)
    
    Loop
        
    LocalGroupsGetMember = GrpList
    
End Function

'Ritorna un elenco dei membri di un gruppo globale
Public Function GlobalGroupsGetMember(Server As String, Group As String) As ListOfMemberGroup
    Dim yServer() As Byte
    Dim yGroup() As Byte
    Dim lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long, nResume As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim tGroupInfo As GLOBALGROUP_MEMBERS_INFO_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    Dim GroupInfo As GroupMemberInfo
    Dim GrpList As ListOfMemberGroup
        
    yServer = MakeServerName(ByVal Server)
    yGroup = Group & vbNullChar
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        
        nRet = NetGroupGetUsers(yServer(0), _
                                yGroup(0), _
                                1, _
                                lGroupInfo, _
                                PrefMaxLen, _
                                nRead, _
                                nTotal, _
                                nResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            GrpList.Init = False
            GrpList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        lGroupInfoPtr = lGroupInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
            
            GroupInfo.UserName = PointerToStringW(tGroupInfo.UserName)
            'Non include i computer account
            If Right(GroupInfo.UserName, 1) <> "$" Then
                GroupInfo.Attribute = tGroupInfo.Attribute
                i = i + 1
                ReDim Preserve GrpList.List(1 To i) As GroupMemberInfo
                GrpList.List(i) = GroupInfo
            End If
            x = x + 1
                
            lGroupInfoPtr = lGroupInfoPtr + Len(tGroupInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lGroupInfo)
        GrpList.Init = (x > 1)
    
    Loop
        
    GlobalGroupsGetMember = GrpList
    
End Function

'Ritorna l'ID di un gruppo globale di un Dominio NT
Public Function GetGroupID(Group As String) As Long
    Dim yServer() As Byte
    Dim yGroup() As Byte
    Dim lRetCode As Long
    Dim nRet As Long
    Dim tGroupInfo As GROUP_INFO_2_API
    Dim lGroupInfo As Long
    Dim lGroupInfoPtr As Long
    
    yServer = MakeServerName(ByVal GetPDCName())
    yGroup = Group & vbNullChar
        
    nRet = NetGroupGetInfo(yServer(0), yGroup(0), 2, lGroupInfo)
        
    If nRet = NERR_Success Then
        lGroupInfoPtr = lGroupInfo
        CopyMem tGroupInfo, ByVal lGroupInfoPtr, Len(tGroupInfo)
        GetGroupID = tGroupInfo.GroupID
    Else
        NetError nRet
        GetGroupID = 0
    End If
                
    lRetCode = NetApiBufferFree(lGroupInfo)
    
End Function

