Attribute VB_Name = "NetUserApi"
Option Explicit

Public Const SHORT_LEVEL = 10&
Public Const EXTENDED_LEVEL = 3&

Declare Function NetUserEnum Lib "netapi32" _
        (lpServer As Any, ByVal Level As Long, _
         ByVal Filter As Long, lpBuffer As Long, _
         ByVal PrefMaxLen As Long, lpEntriesRead As Long, _
         lpTotalEntries As Long, lpResumeHandle As Long) As Long

Declare Function NetUserGetInfo Lib "netapi32" _
        (lpServer As Any, UserName As Byte, _
         ByVal Level As Long, lpBuffer As Long) As Long


Private Type USER_INFO_SHORT_API
   Name As Long
   Comment As Long
   UsrComment As Long
   FullName As Long
End Type

Type UserInfoShort
   Name As String
   Comment As String
   UsrComment As String
   FullName As String
End Type

Type ListOfUserShort
    Init As Boolean
    LastErr As Long
    List() As UserInfoShort
End Type


Private Type USER_INFO_EXT_API
   Name As Long
   Password As Long
   PasswordAge As Long
   Privilege As Long
   HomeDir As Long
   Comment As Long
   Flags As Long
   ScriptPath As Long
   AuthFlags As Long
   FullName As Long
   UserComment As Long
   Parms As Long
   Workstations As Long
   LastLogon As Long
   LastLogoff As Long
   AcctExpires As Long
   MaxStorage As Long
   UnitsPerWeek As Long
   LogonHours As Long
   BadPwCount As Long
   NumLogons As Long
   LogonServer As Long
   CountryCode As Long
   CodePage As Long
   UserID As Long
   PrimaryGroupID As Long
   Profile As Long
   HomeDirDrive As Long
   PasswordExpired As Long
End Type

Type UserInfoExt
   Name As String
   Password As String
   PasswordAge As String
   Privilege As Long
   HomeDir As String
   Comment As String
   Flags As Long
   NoChangePwd As Boolean
   NoExpirePwd As Boolean
   AccDisabled As Boolean
   AccLocked As Boolean
   ScriptPath As String
   AuthFlags As Long
   FullName As String
   UserComment As String
   Parms As String
   Workstations As String
   LastLogon As Date
   LastLogoff As Date
   AcctExpires As Date
   MaxStorage As Long
   UnitsPerWeek As Long
   LogonHours(0 To 20) As Byte
   BadPwCount As Long
   NumLogons As Long
   LogonServer As String
   CountryCode As Long
   CodePage As Long
   UserID As Long
   PrimaryGroupID As Long
   Profile As String
   HomeDirDrive As String
   PasswordExpired As Boolean
End Type

Type ListOfUserExt
    Init As Boolean
    LastErr As Long
    List() As UserInfoExt
End Type

Type AccountInfo
    UserInfo As UserInfoExt
    LocalGrp As ListOfGroup0
    GlobalGrp As ListOfGroup0
End Type

'Ritorna un elenco breve degli utenti di un server NT
Public Function ShortEnumUsers(Server As String) As ListOfUserShort
    Dim yServer() As Byte, lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long, nResume As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim lUserInfo As Long
    Dim lUserInfoPtr As Long
    Dim UserInfo As UserInfoShort
    Dim UserList As ListOfUserShort
    Dim tUserInfo As USER_INFO_SHORT_API
    
    yServer = MakeServerName(ByVal Server)
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        nRet = NetUserEnum(yServer(0), SHORT_LEVEL, 2, _
                           lUserInfo, PrefMaxLen, nRead, _
                           nTotal, nResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            UserList.Init = False
            UserList.LastErr = nRet
            NetError nRet
            Exit Do
        End If
                
        lUserInfoPtr = lUserInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tUserInfo, ByVal lUserInfoPtr, Len(tUserInfo)
            
            UserInfo.Name = PointerToStringW(tUserInfo.Name)
            UserInfo.FullName = PointerToStringW(tUserInfo.FullName)
            UserInfo.Comment = PointerToStringW(tUserInfo.Comment)
            i = i + 1
            ReDim Preserve UserList.List(1 To i) As UserInfoShort
            UserList.List(i) = UserInfo
            x = x + 1
                
            lUserInfoPtr = lUserInfoPtr + Len(tUserInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lUserInfo)
        UserList.Init = (x > 1)
    
    Loop
        
    ShortEnumUsers = UserList
    
End Function

'Ritorna un elenco breve degli utenti di un server NT
Public Function LongEnumUsers(Server As String, Optional sError As Variant) As ListOfUserExt
    Dim yServer() As Byte, lRetCode As Long
    Dim nRead As Long, nTotal As Long
    Dim nRet As Long, nResume As Long
    Dim PrefMaxLen As Long
    Dim i As Long, x As Long
    Dim lUserInfo As Long
    Dim lUserInfoPtr As Long
    Dim UserInfo As UserInfoExt
    Dim UserList As ListOfUserExt
    Dim tUserInfo As USER_INFO_EXT_API
    
    yServer = MakeServerName(ByVal Server)
    PrefMaxLen = 65536
        
    nRet = NERR_MoreData
    Do While (nRet = NERR_MoreData)
        nRet = NetUserEnum(yServer(0), EXTENDED_LEVEL, 2, _
                           lUserInfo, PrefMaxLen, nRead, _
                           nTotal, nResume)
        
        If (nRet <> NERR_Success And _
             nRet <> NERR_MoreData) Then
            UserList.Init = False
            UserList.LastErr = nRet
            NetError nRet, sError
            Exit Do
        End If
                
        lUserInfoPtr = lUserInfo
                
        x = 1
        Do While x <= nRead
            
            CopyMem tUserInfo, ByVal lUserInfoPtr, Len(tUserInfo)
            
            UserInfo.Name = PointerToStringW(tUserInfo.Name)
            UserInfo.Password = PointerToStringW(tUserInfo.Password)
            UserInfo.PasswordAge = Format(tUserInfo.PasswordAge / 86400, "0.0")
            UserInfo.Privilege = tUserInfo.Privilege
            UserInfo.HomeDir = PointerToStringW(tUserInfo.HomeDir)
            UserInfo.Comment = PointerToStringW(tUserInfo.Comment)
            UserInfo.Flags = tUserInfo.Flags
            UserInfo.NoChangePwd = CBool((tUserInfo.Flags Or USER_ACC_NOPWD_CHANGE) = tUserInfo.Flags)
            UserInfo.NoExpirePwd = CBool((tUserInfo.Flags Or USER_ACC_NOPWD_EXPIRE) = tUserInfo.Flags)
            UserInfo.AccDisabled = CBool((tUserInfo.Flags Or USER_ACC_DISABLED) = tUserInfo.Flags)
            UserInfo.AccLocked = CBool((tUserInfo.Flags Or USER_ACC_LOCKED) = tUserInfo.Flags)
            UserInfo.ScriptPath = PointerToStringW(tUserInfo.ScriptPath)
            UserInfo.AuthFlags = tUserInfo.AuthFlags
            UserInfo.FullName = PointerToStringW(tUserInfo.FullName)
            UserInfo.UserComment = PointerToStringW(tUserInfo.UserComment)
            UserInfo.Parms = PointerToStringW(tUserInfo.Parms)
            UserInfo.Workstations = PointerToStringW(tUserInfo.Workstations)
            UserInfo.LastLogon = NetTimeToVbTime(tUserInfo.LastLogon)
            UserInfo.LastLogoff = NetTimeToVbTime(tUserInfo.LastLogoff)
            If tUserInfo.AcctExpires = -1& Then
                UserInfo.AcctExpires = NetTimeToVbTime(0)
            Else
                UserInfo.AcctExpires = NetTimeToVbTime(tUserInfo.AcctExpires)
            End If
            UserInfo.MaxStorage = tUserInfo.MaxStorage
            UserInfo.UnitsPerWeek = tUserInfo.UnitsPerWeek
            CopyMem UserInfo.LogonHours(0), ByVal tUserInfo.LogonHours, 21
            UserInfo.BadPwCount = tUserInfo.BadPwCount
            UserInfo.NumLogons = tUserInfo.NumLogons
            UserInfo.LogonServer = PointerToStringW(tUserInfo.LogonServer)
            UserInfo.CountryCode = tUserInfo.CountryCode
            UserInfo.CodePage = tUserInfo.CodePage
            UserInfo.UserID = tUserInfo.UserID
            UserInfo.PrimaryGroupID = tUserInfo.PrimaryGroupID
            UserInfo.Profile = PointerToStringW(tUserInfo.Profile)
            UserInfo.HomeDirDrive = PointerToStringW(tUserInfo.HomeDirDrive)
            UserInfo.PasswordExpired = CBool(tUserInfo.PasswordExpired)
            
            
            i = i + 1
            ReDim Preserve UserList.List(1 To i) As UserInfoExt
            UserList.List(i) = UserInfo
            x = x + 1
                
            lUserInfoPtr = lUserInfoPtr + Len(tUserInfo)
             
        Loop
        
        lRetCode = NetApiBufferFree(lUserInfo)
        UserList.Init = (x > 1)
    
    Loop
        
    LongEnumUsers = UserList
    
End Function

Public Function GetUserInfo(UserName As String, Optional ServerName) As UserInfoExt
    Dim yUserName() As Byte
    Dim yServer() As Byte
    Dim nRet As Long
    Dim tUserInfo As USER_INFO_EXT_API
    Dim lUserInfo As Long
    Dim lUserInfoPtr As Long
    Dim UserInfo As UserInfoExt
    Dim lRetCode As Long
    
    If IsMissing(ServerName) Then ServerName = ""
    
    yServer = MakeServerName(ByVal ServerName)
    yUserName = UserName & vbNullChar
    nRet = NetUserGetInfo(yServer(0), yUserName(0), _
                          3, lUserInfo)
   
    If nRet = NERR_Success Then
        CopyMem tUserInfo, ByVal lUserInfo, Len(tUserInfo)
        UserInfo.Name = PointerToStringW(tUserInfo.Name)
        UserInfo.Password = PointerToStringW(tUserInfo.Password)
        UserInfo.PasswordAge = Format(tUserInfo.PasswordAge / 86400, "0.0")
        UserInfo.Privilege = tUserInfo.Privilege
        UserInfo.HomeDir = PointerToStringW(tUserInfo.HomeDir)
        UserInfo.Comment = PointerToStringW(tUserInfo.Comment)
        UserInfo.Flags = tUserInfo.Flags
        UserInfo.NoChangePwd = CBool((tUserInfo.Flags Or USER_ACC_NOPWD_CHANGE) = tUserInfo.Flags)
        UserInfo.NoExpirePwd = CBool((tUserInfo.Flags Or USER_ACC_NOPWD_EXPIRE) = tUserInfo.Flags)
        UserInfo.AccDisabled = CBool((tUserInfo.Flags Or USER_ACC_DISABLED) = tUserInfo.Flags)
        UserInfo.AccLocked = CBool((tUserInfo.Flags Or USER_ACC_LOCKED) = tUserInfo.Flags)
        UserInfo.ScriptPath = PointerToStringW(tUserInfo.ScriptPath)
        UserInfo.AuthFlags = tUserInfo.AuthFlags
        UserInfo.FullName = PointerToStringW(tUserInfo.FullName)
        UserInfo.UserComment = PointerToStringW(tUserInfo.UserComment)
        UserInfo.Parms = PointerToStringW(tUserInfo.Parms)
        UserInfo.Workstations = PointerToStringW(tUserInfo.Workstations)
        UserInfo.LastLogon = NetTimeToVbTime(tUserInfo.LastLogon)
        UserInfo.LastLogoff = NetTimeToVbTime(tUserInfo.LastLogoff)
        If tUserInfo.AcctExpires = -1& Then
            UserInfo.AcctExpires = NetTimeToVbTime(0)
        Else
            UserInfo.AcctExpires = NetTimeToVbTime(tUserInfo.AcctExpires)
        End If
        UserInfo.MaxStorage = tUserInfo.MaxStorage
        UserInfo.UnitsPerWeek = tUserInfo.UnitsPerWeek
        CopyMem UserInfo.LogonHours(0), ByVal tUserInfo.LogonHours, 21
        UserInfo.BadPwCount = tUserInfo.BadPwCount
        UserInfo.NumLogons = tUserInfo.NumLogons
        UserInfo.LogonServer = PointerToStringW(tUserInfo.LogonServer)
        UserInfo.CountryCode = tUserInfo.CountryCode
        UserInfo.CodePage = tUserInfo.CodePage
        UserInfo.UserID = tUserInfo.UserID
        UserInfo.PrimaryGroupID = tUserInfo.PrimaryGroupID
        UserInfo.Profile = PointerToStringW(tUserInfo.Profile)
        UserInfo.HomeDirDrive = PointerToStringW(tUserInfo.HomeDirDrive)
        UserInfo.PasswordExpired = CBool(tUserInfo.PasswordExpired)
    End If
    
    lRetCode = NetApiBufferFree(lUserInfo)
    
    GetUserInfo = UserInfo
End Function

