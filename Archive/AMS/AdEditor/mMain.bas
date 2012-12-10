Attribute VB_Name = "mMain"
Public gstrConnectionString As String
Public gcn As ADODB.Connection

Public Function gConn() As ADODB.Connection

    If gcn Is Nothing Then
        Set gcn = New ADODB.Connection
        gcn.Open gstrConnectionString
    End If
    
    Set gConn = gcn
    
End Function
