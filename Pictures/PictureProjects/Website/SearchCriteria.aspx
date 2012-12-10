<%@ Page Language="c#" SmartNavigation="False" Inherits="pics.SearchCriteria" CodeBehind="SearchCriteria.aspx.cs" %>

<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>pics.msn2.net: Advanced Search</title>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="msn2.css" type="text/css" rel="stylesheet" />
</head>
<body style="padding: 0px">
    <form method="post" runat="server">
    <picctls:Header ID="header" runat="server" Size="small" Text="Picture Folders">
    </picctls:Header>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" align="left"
        border="0">
        <tr>
            <td class="msn2headerfade" colspan="4" height="3">
                <img height="3" src="images/blank.gif" alt="" />
            </td>
        </tr>
        <tr>
            <td class="msn2sidebar" valign="top" width="125" rowspan="3">
                <picctls:Sidebar ID="Sidebar1" runat="server">
							<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" Width="100%" visible="false">
								<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
							</picctls:ContentPanel>
                </picctls:Sidebar>
            </td>
        </tr>
        <tr style="height: 25px">
            <td>
                <table class="areaPanel" cellspacing="0" cellpadding="4" width="100%" border="0">
                    <tr>
                        <td valign="top" width="16">
                            <img src="Images/search.gif" width="16" height="16" alt="Search" />
                        </td>
                        <td valign="top">
                            <b>Advanced Search</b>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="msn2contentwindow" valign="top">
                <p>
                    You can specify how you would like to search for pictures below. Fill in any of
                    the fields below. For Name and Description you can enter a part of a name.
                </p>
                <p>
                    <asp:Label ID="noResults" runat="server" Visible="False" CssClass="err">There were no results found for your search below.  Please change your search and try again.<br /><br /></asp:Label>
                </p>
                <p>
                    <b>Picture Description</b>: You can search for pictures based on the picture description.
                </p>
                <blockquote>
                    <p>
                        Description:
                        <asp:TextBox ID="description" runat="server" Width="200px"></asp:TextBox>
                    </p>
                </blockquote>
                <hr noshade="noshade" />
                <p>
                    <b>Picture Date</b>: You can search for pictures based on the date of the picture.
                </p>
                <blockquote>
                    <table>
                        <tr>
                            <td align="right">
                                Between
                            </td>
                            <td>
                                <asp:TextBox ID="pictureDateStart" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="pictureDateStartBad" runat="server" Visible="False" CssClass="err">The start date entered must be in the format mm/dd/yy.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                and
                            </td>
                            <td>
                                <asp:TextBox ID="pictureDateEnd" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="pictureDateEndBad" runat="server" Visible="False" CssClass="err">The end date entered must be in the format mm/dd/yy.</asp:Label>
                            </td>
                        </tr>
                    </table>
                </blockquote>
                <hr noshade="noshade" />
                <p>
                    <b>People</b>: You can search for pictures based on the people in the picture. First,
                    enter any part of the person's name. Then click 'Find'. This will show anyone with
                    similiar names. Select the person or persons you would like to include, and click
                    'Add'. You can repeat the process to add as many names as you wish.
                </p>
                <picctls:PeopleSelector ID="peopleSelector" runat="server" ForeColor="Black">
                </picctls:PeopleSelector>
                <p>
                    <asp:RadioButtonList ID="personSearchOption" runat="server" Visible="True">
                        <asp:ListItem Value="0" Text="Search for pictures with one or more of the people listed above in the picture"
                            Selected="True" />
                        <asp:ListItem Value="1" Text="Search for pictures with all of the people listed above in the picture" />
                        <asp:ListItem Value="2" Text="Search for pictures with only the people listed above in the picture" />
                    </asp:RadioButtonList>
                </p>
                <hr noshade="noshade" />
                <asp:Button ID="search" runat="server" Text="Search" CssClass="btn" OnClick="search_Click" />
                <asp:Button ID="reset" runat="server" Text="Reset" CssClass="btn" OnClick="reset_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
