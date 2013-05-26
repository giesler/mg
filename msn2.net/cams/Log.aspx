<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Log.aspx.cs" Inherits="Log" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>cam log</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div style="float: right; padding-right: 3px">
        <a href="/">HOME</a> | 
        <asp:LinkButton runat="server" ID="showHideVideos" OnClick="showHideVideos_Click" Text="SHOW VIDEOS" /> | 
        <a href="Login.aspx">SIGN OUT</a>
    </div>
    <div st>
    
        <asp:Repeater ID="data" runat="server">
            <ItemTemplate>
                <div style="clear: both">
                <%# DataBinder.Eval(Container.DataItem, "Day") %>
                </div>
                <%# GetPictures(DataBinder.Eval(Container.DataItem, "Date")) %>
                <br />
            </ItemTemplate>
        </asp:Repeater>
        
    </div>
    </form>
</body>
</html>
