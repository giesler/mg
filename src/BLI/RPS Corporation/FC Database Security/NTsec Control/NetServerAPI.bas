Attribute VB_Name = "NetServerAPI"
Option Explicit

Declare Function NetServerEnum Lib "netapi32" _
        (lpServer As Any, ByVal lLevel As Long, vBuffer As Any, _
         lPreferedMaxLen As Long, lEntriesRead As Long, lTotalEntries As Long, _
         ByVal lServerType As Long, ByVal sDomain$, vResume As Any) As Long

Declare Function NetWkstaGetInfo Lib "netapi32" _
        (lpServer As Any, ByVal lLevel As Long, _
         vBuffer As Any) As Long
    
Declare Function NetServerGetInfo Lib "netapi32" _
        (lpServer As Any, ByVal lLevel As Long, _
         vBuffer As Any) As Long
    

Private Type WKSTA_INFO_API     'Level 101
    PlatformId As Long
    ComputerName As Long
    LanGroup As Long
    VerMajor As Long
    VerMinor As Long
    LanRoot As Long
End Type

Private Type WkstaInfo
    PlatformId As Long
    ComputerName As String
    LanGroup As String
    VerMajor As Long
    VerMinor As Long
    LanRoot As String
End Type
                    
                    
Private Type SERVER_INFO_API    'Level 101
    PlatformId As Long
    ServerName As Long
    Type As Long
    VerMajor As Long
    VerMinor As Long
    Comment As Long
End Type


Type ServerInfo
    PlatformId As Long
    ServerName As String
    Type As Long
    VerMajor As Long
    VerMinor As Long
    Comment As String
    Platform As String
    ServerType As Integer
    LanGroup As String
    LanRoot As String
End Type

Type ListOfServer
    Init As Boolean
    LastErr As Long
    List() As ServerInfo
End Type
                    
'Ritorna un elenco dei server
Public Function EnumServer(lServerType As Long) As ListOfServer
    Dim nRet As Long, x As Integer, i As Integer
    Dim lRetCode As Long
    Dim tServerInfo As SERVER_INFO_API
    Dim lServerInfo As Long
    Dim lServerInfoPtr As Long
    Dim ServerInfo As ServerInfo
    Dim lPreferedMaxLen As Long
    Dim lEntriesRead As Long
    Dim lTotalEntries As Long
    Dim sDomain As String
    Dim vResume As Variant
    Dim yServer() As Byte
    Dim SrvList As ListOfServer
    
    yServer = MakeServerName(ByVal "")
    lPreferedMaxLen = 65536

    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
            
        'Call NetServerEnum to get a list of Servers
        nRet = NetServerEnum(yServer(0), 101, lServerInfo, _
                             lPreferedMaxLen, lEntriesRead, _
                             lTotalEntries, lServerType, _
                             sDomain, vResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            SrvList.Init = False
            SrvList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        ' NetServerEnum Index is 1 based
        x = 1
        lServerInfoPtr = lServerInfo
                         
        Do While x <= lTotalEntries
            
            CopyMem tServerInfo, ByVal lServerInfoPtr, Len(tServerInfo)
            
            ServerInfo.Comment = PointerToStringW(tServerInfo.Comment)
            ServerInfo.ServerName = PointerToStringW(tServerInfo.ServerName)
            ServerInfo.Type = tServerInfo.Type
            ServerInfo.PlatformId = tServerInfo.PlatformId
            ServerInfo.VerMajor = tServerInfo.VerMajor
            ServerInfo.VerMinor = tServerInfo.VerMinor
            GetPlatform tServerInfo, ServerInfo
            i = i + 1
            ReDim Preserve SrvList.List(1 To i) As ServerInfo
            SrvList.List(i) = ServerInfo
            
            x = x + 1
                
            lServerInfoPtr = lServerInfoPtr + Len(tServerInfo)
             
        Loop
            
        lRetCode = NetApiBufferFree(lServerInfo)
        SrvList.Init = (x > 1)
    
    Loop
    
    EnumServer = SrvList
    
End Function

'Ritorna un elenco dei Domain Controller (PDC e BDC)
Public Function EnumDomainServer() As ListOfServer
    Dim nRet As Long, x As Integer, i As Integer
    Dim lRetCode As Long
    Dim tServerInfo As SERVER_INFO_API
    Dim lServerInfo As Long
    Dim lServerInfoPtr As Long
    Dim ServerInfo As ServerInfo
    Dim lPreferedMaxLen As Long
    Dim lEntriesRead As Long
    Dim lTotalEntries As Long
    Dim sDomain As String
    Dim vResume As Variant
    Dim yServer() As Byte
    Dim SrvList As ListOfServer
        
    yServer = MakeServerName(ByVal "")
    lPreferedMaxLen = 65536

    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
            
        'Call NetServerEnum to get a list of Servers
        nRet = NetServerEnum(yServer(0), 101, lServerInfo, _
                             lPreferedMaxLen, lEntriesRead, _
                             lTotalEntries, 24, _
                             sDomain, vResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            SrvList.Init = False
            SrvList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
        
        ' NetServerEnum Index is 1 based
        x = 1
        lServerInfoPtr = lServerInfo
                         
        Do While x <= lTotalEntries
            
            CopyMem tServerInfo, ByVal lServerInfoPtr, Len(tServerInfo)
            
            ServerInfo.Comment = PointerToStringW(tServerInfo.Comment)
            ServerInfo.ServerName = PointerToStringW(tServerInfo.ServerName)
            ServerInfo.Type = tServerInfo.Type
            ServerInfo.PlatformId = tServerInfo.PlatformId
            ServerInfo.VerMajor = tServerInfo.VerMajor
            ServerInfo.VerMinor = tServerInfo.VerMinor
            GetPlatform tServerInfo, ServerInfo
            i = i + 1
            ReDim Preserve SrvList.List(1 To i) As ServerInfo
            SrvList.List(i) = ServerInfo
            
            x = x + 1
                
            lServerInfoPtr = lServerInfoPtr + Len(tServerInfo)
             
        Loop
            
        lRetCode = NetApiBufferFree(lServerInfo)
        SrvList.Init = (x > 1)
    
    Loop
    
    EnumDomainServer = SrvList
    
End Function

'Ritorna un le informazioni di un Server/Workstation NT
Public Function GetServerInfo(Optional sName) As ServerInfo
    Dim nRet As Long
    Dim tWks As WKSTA_INFO_API
    Dim tSrv As SERVER_INFO_API
    Dim lWks As Long
    Dim lWksPtr As Long
    Dim WksInfo As ServerInfo
    Dim yServer() As Byte
    
    If IsMissing(sName) Then sName = ""
    yServer = MakeServerName(ByVal sName)
        
    nRet = NetWkstaGetInfo(yServer(0), 101, lWks)
    lWksPtr = lWks

    If nRet = NERR_Success Then
        CopyMem tWks, ByVal lWksPtr, Len(tWks)
        WksInfo.ServerName = PointerToStringW(tWks.ComputerName)
        WksInfo.VerMajor = tWks.VerMajor
        WksInfo.VerMinor = tWks.VerMinor
        WksInfo.PlatformId = tWks.PlatformId
        WksInfo.LanGroup = PointerToStringW(tWks.LanGroup)
        WksInfo.LanRoot = PointerToStringW(tWks.LanRoot)
        
        nRet = NetServerGetInfo(yServer(0), 101, lWks)
        If nRet = NERR_Success Then
            lWksPtr = lWks
            CopyMem tSrv, ByVal lWksPtr, Len(tSrv)
            WksInfo.Comment = PointerToStringW(tSrv.Comment)
            WksInfo.Type = tSrv.Type
            GetPlatform tSrv, WksInfo
        End If
    Else
        NetError nRet
    End If
    
    GetServerInfo = WksInfo
        
End Function

Private Sub GetPlatform(tWks As SERVER_INFO_API, SrvInfo As ServerInfo)
    Dim IsDomain As Boolean
    
    If tWks.PlatformId = 500 Then
        IsDomain = IsDomainServer(SrvInfo.ServerName)
        If IsDomain Then
            If tWks.VerMinor > 300000 Then
                SrvInfo.Platform = "NT Primary Domain"
                SrvInfo.ServerType = SRV_PRIMARY
            Else        'If tWks.VerMinor > 103000 Then
                SrvInfo.Platform = "NT Backup Domain"
                SrvInfo.ServerType = SRV_BACKUP
            End If
        Else
            If tWks.VerMinor > 100000 Then
                SrvInfo.Platform = "NT Server"
                SrvInfo.ServerType = SRV_SERVER
            Else
                SrvInfo.Platform = "NT WorkStation"
                SrvInfo.ServerType = SRV_NTWK
            End If
        End If
    Else
        If tWks.Type = 1 Then
            SrvInfo.Platform = "Windows 3.x"
            SrvInfo.ServerType = SRV_WIN3
        Else
            SrvInfo.Platform = "Windows 95/98"
            SrvInfo.ServerType = SRV_WIN95
        End If
    End If

End Sub

