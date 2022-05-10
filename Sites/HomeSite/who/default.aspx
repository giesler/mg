<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="netwho" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>who's home</title>
    <link href="https://home.giesler.org/Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
    <link href="Styles.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodyStyle">
    <form id="form1" runat="server">
        <div class="headerLink" style="padding: 4px; border-bottom-color: silver;">
            <a href="/">home</a> |
            <a href="/cams/">cams</a> | 
            <a href="/services.aspx">services</a> |
            <a href="/who/" style="color: deepskyblue">who</a>
        </div>
        <div>
            <asp:Panel runat="server" ID="errorPanel" Visible="false" ForeColor="DarkRed">
                <asp:Label runat="server" ID="error" />
            </asp:Panel>
        </div>
        <asp:Panel runat="server" ID="panel">
            <div style="padding-left: 8px; padding-top: 4px">
                <asp:Repeater runat="server" ID="items">
                    <HeaderTemplate>
                        <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr style="<%#GetStyle(Eval("IsHome"))%>">
                            <td style="padding-bottom: 0px"><%#Eval("Name").ToString().Split('-')[0].Trim() %><%# GetApInfo(Eval("IsHome"), Eval("APName")) %></td>
                        </tr>
                        <tr style="color: gray">
                            <td style="font-size: small; padding-bottom: 6px">
                                <%#Eval("Name").ToString().Split('-')[1].Trim().ToLower() %> 
                                <%#GetTimeInfo(Eval("IsHome"), Eval("LastSeen"), Eval("ConnectionTime") ) %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
