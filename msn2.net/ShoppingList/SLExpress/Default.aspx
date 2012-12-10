<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shopping List</title>
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="scirptManager" />
    <div style="height: 100%">
        <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/SLExpressControls.xap"
            Version="2.0" Width="100%" Height="100%" />
    </div>
    </form>
</body>
</html>
