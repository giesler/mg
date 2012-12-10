<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ajax.aspx.cs" Inherits="SLExpress.ajax" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Shopping List</title>
    <style type="text/css">
        #UpdateProgress1
        {
            width: 100px;
            background-color: Silver;
            bottom: 0%;
            left: 0px;
            position: absolute;
        }
    </style>
</head>
<body style="margin: 0px 0px 0px 0px; font-family: Tahoma">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />
    <div style="height:100%">
        <asp:UpdatePanel runat="server" UpdateMode="Always" RenderMode="Inline" ID="updatePanel">
            <ContentTemplate>
                <asp:Table ID="Table1" runat="server" Width="100%" Height="100%" BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell ID="TableCell2" runat="server" Width="20%" VerticalAlign="Top">
                            <asp:ListBox ID="storeList" runat="server" AutoPostBack="True" Rows="10" Font-Names="Tahoma"
                                Visible="false" Font-Size="Smaller"></asp:ListBox>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server" Width="80%" VerticalAlign="Top" Height="100%">
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="updatePanel" runat="server">
                                <ProgressTemplate>
                                    <asp:Label ID="Label1" runat="server" Font-Size="Smaller">
                                    loading...
                                    </asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:Panel runat="server" ID="defaultView" Visible="false" Height="168">
                                <asp:Panel runat="server" ID="addPanel">
                                    <asp:TextBox runat="server" ID="addItem" /><asp:Button runat="server" ID="addButton"
                                        Text="Add" />
                                    <asp:Button runat="server" ID="showBulkAdd" Text="Bulk Add" />
                                </asp:Panel>
                                <div style="height: 139; width: 100%; overflow: scroll; border: solid 1px Gray">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderStyle="None" BorderWidth="0px" CellPadding="4" ForeColor="Black" CellSpacing="0"
                                        GridLines="Vertical" ShowHeader="false" Width="100%">
                                        <FooterStyle BackColor="#CCCC99" />
                                        <RowStyle BackColor="White" Font-Size="8pt" />
                                        <Columns>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="itemLabel" Text='<%# Bind("ListItem") %>' />
                                                    <asp:HiddenField runat="server" ID="itemId" Value='<%# Bind("Id") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="itemText" Text='<%# Bind("ListItem") %>' />
                                                    <asp:HiddenField runat="server" ID="itemId" Value='<%# Bind("Id") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#E8F7F7" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="bulkAddPanel" Visible="false">
                                <div>
                                    <asp:TextBox TextMode="MultiLine" runat="server" Rows="8" MaxLength="1024" Columns="40"
                                        ID="bulkText" />
                                </div>
                                <div>
                                    <asp:Button runat="server" Text="Bulk Add" ID="bulkAdd" />
                                    <asp:Button runat="server" Text="Cancel" ID="cancel" />
                                </div>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
