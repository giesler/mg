Attribute VB_Name = "NetAPI"
Option Explicit

Public Const SRV_TYPE_SERVER = &H2
Public Const SRV_TYPE_SQLSERVER = &H4
Public Const SRV_TYPE_NT_PDC = &H8
Public Const SRV_TYPE_NT_BDC = &H10
Public Const SRV_TYPE_PRINT = &H200
Public Const SRV_TYPE_NT = &H1000
Public Const SRV_TYPE_ALL = &HFFFF
Public Const SRV_TYPE_RAS = &H400

Public Const SRV_PRIMARY = 1
Public Const SRV_BACKUP = 2
Public Const SRV_SERVER = 3
Public Const SRV_NTWK = 4
Public Const SRV_WIN95 = 5
Public Const SRV_WIN3 = 6

Public Const USER_ACC_NOPWD_CHANGE = 577&
Public Const USER_ACC_NOPWD_EXPIRE = 66049
Public Const USER_ACC_DISABLED = 515&
Public Const USER_ACC_LOCKED = 529&

Public Const NERR_Success = 0&
Public Const NERR_Access_Denied = 5&
Public Const NERR_MoreData = 234&

Declare Function NetGetDCName Lib "netapi32" _
        (lpServer As Any, lpDomain As Any, _
         vBuffer As Any) As Long
                       
Declare Function NetMessageBufferSend Lib "netapi32" _
        (ByVal ServerName As String, _
         ByVal ToName As String, _
         ByVal FromName As String, _
         ByVal Message As String, _
         ByVal BufferLen As Long) As Long

Declare Function NetApiBufferFree Lib "netapi32" _
        (ByVal lBuffer&) As Long

'Ritorna il nome del Primay Domain Controller
Public Function GetPDCName() As String
    Dim lpBuffer As Long, nRet As Long
    Dim yServer() As Byte
    Dim sLocal As String
    
    yServer = MakeServerName(ByVal "")
    
    nRet = NetGetDCName(yServer(0), yServer(0), lpBuffer)
        
    If nRet = 0 Then
        sLocal = PointerToStringW(lpBuffer)
    End If
    
    If lpBuffer Then Call NetApiBufferFree(lpBuffer)
                
    GetPDCName = sLocal
    
End Function

'Ritorna il nome del Dominio NT
Public Function GetDomainName() As String
    Dim SrvInfo As ServerInfo
    
    SrvInfo = GetServerInfo()
    GetDomainName = SrvInfo.LanGroup
    
End Function

'Ritorna il nome del computer locale
Public Function GetLocalSystemName() As String
    Dim SrvInfo As ServerInfo
    
    SrvInfo = GetServerInfo()
    GetLocalSystemName = SrvInfo.ServerName

End Function

Public Function PointerToStringW(lpStringW As Long) As String
   Dim buffer() As Byte
   Dim nLen As Long
   
   If lpStringW Then
      nLen = lstrlenW(lpStringW) * 2
      If nLen Then
         ReDim buffer(0 To (nLen - 1)) As Byte
         CopyMem buffer(0), ByVal lpStringW, nLen
         PointerToStringW = buffer
      End If
   End If
End Function

Public Function MakeServerName(ByVal ServerName As String)
    Dim yServer() As Byte
        
    If ServerName <> "" Then
        If InStr(1, ServerName, "\\") = 0 Then
            ServerName = "\\" & ServerName
        End If
    End If
    
    yServer = ServerName & vbNullChar
    MakeServerName = yServer

End Function

Public Function NetError(nErr As Long, Optional Ret) As String
    Dim Msg As String
    
    If IsMissing(Ret) Then Ret = False
    
    Select Case nErr
        Case 5
            Msg = "Accesso Negato!"
        Case 53
            Msg = "Percorso di Rete errato!"
        Case 1722
            Msg = "Server non disponibile!"
        Case Else
            Msg = "Si è verificato un Errore (" & nErr & ") !"
    End Select
    
    If Not Ret Then
        Beep
        MsgBox Msg, vbCritical, "Net Error"
    Else
        NetError = Msg
    End If
    
End Function

Public Function NetTimeToVbTime(NetDate As Long) As Double
    Const BaseDate# = 25569   'DateSerial(1970, 1, 1)
    Const SecsPerDay# = 86400
    Dim Tmp As Double
   
    Tmp = BaseDate + (CDbl(NetDate) / SecsPerDay)
    If Tmp <> BaseDate Then
        NetTimeToVbTime = Tmp
    End If
    
End Function

Public Function TimeToTime(NetDate As Long) As String
    Const SecsPerDay = 86400
    Const SecsPerHour = 3600
    Const SecsPerMinute = 60
    
    Dim Rest As Long
    Dim d As Long, h As Long
    Dim m As Long
   
    d = Int((NetDate / SecsPerDay)) * 24
    Rest = (NetDate Mod SecsPerDay)
    h = Int(Rest / SecsPerHour)
    Rest = (Rest Mod SecsPerHour)
    m = Int(Rest / SecsPerMinute)
    Rest = (Rest Mod SecsPerMinute)
        
    TimeToTime = (d + h) & "." & Format(m, "00") & "." & _
                 Format(Rest, "00")
    
End Function

Public Function IsDomainServer(Server As String) As Boolean
    Dim x As Integer
    
    If DomainServer.Init Then
        For x = 1 To UBound(DomainServer.List)
            If Server = DomainServer.List(x).ServerName Then
                IsDomainServer = True
                Exit Function
            End If
        Next
    End If
    
End Function


