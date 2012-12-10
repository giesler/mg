<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureDetails.aspx.cs"
    Inherits="pics.Controls.Mobile.PictureDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>pics.msn2.net</title>
    <link rel="Stylesheet" href="mobile.css" />
</head>
<body style="margin: 0px 0px 0px 0px; padding: 0px 0px 0px 0px">
    <form id="form1" runat="server">
    <div>
        <div class="title">
            <asp:Label runat="server" ID="title" />
        </div>
        <div class="description">
            <asp:Label runat="server" ID="description" />
        </div>
        <asp:Label runat="server" ID="dateTaken" />
        <asp:Panel runat="server" ID="content" />
        <div class="footer">
            <hr />
            <a href="default.aspx">Home</a> | <a href="javascript:history.go(-1)">Back</a>
        </div>
    </div>
    </form>
</body>
</html>
