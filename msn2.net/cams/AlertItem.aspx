<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlertItem.aspx.cs" Inherits="AlertItem" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>cam alert</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style="float: right; padding-right: 3px">
        <asp:HyperLink runat="server" ID="previousLink">&lt;&lt;</asp:HyperLink> 
        <asp:HyperLink runat="server" ID="nextLink">&gt;&gt;</asp:HyperLink> &nbsp;
        <a href="default.aspx">HOME</a> | 
        <a href="Log.aspx">LOG</a> | 
        <a href="Login.aspx">SIGN OUT</a>
    </div>
    <asp:Label runat="server" ID="name" />
    <br />
    <asp:Image runat="server" ID="img" />
    </div>
    </form>
</body>
</html>
