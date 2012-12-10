<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="SLExpress.signin" %>

<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="Microsoft.Live" %>

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="http://js.live.net/4.1/loader.js"></script>
    <script type="text/javascript">
        function signInComplete(e) {
            if (e.get_resultCode() !== Microsoft.Live.AsyncResultCode.success) {
                alert("sign-in failed: " + e.get_resultCode());
                return;
            }
            if (e.get_resultCode() === Microsoft.Live.AsyncResultCode.success) {
                // Get the data context and load contacts.
                dataContext = Microsoft.Live.App.get_dataContext();
                dataContext.loadAll(Microsoft.Live.DataType.contacts, contactsLoaded);
            }
        }
        function contactsLoaded(dataLoadCompletedEventArgs) {
            $get('Name_list').options.length = 0; // Clear list of names.
            if (dataLoadCompletedEventArgs.get_resultCode() !== Microsoft.Live.AsyncResultCode.success) {
                alert("Contacts failed to load: " + dataLoadCompletedEventArgs.get_error().message);
                return;
            }

            contactsCollection = dataLoadCompletedEventArgs.get_data();
            for (var i = 0; i < contactsCollection.get_length(); i++) {
                var contact = contactsCollection.getItem(i);
                addOption($get('Name_list'), contact.get_formattedName(), contact.get_id());
            }
        }

        // Add items to the drop down list box.
        function addOption(selectbox, text, value) {
            var optn = document.createElement("OPTION");
            optn.text = text;
            optn.value = value;
            selectbox.options.add(optn);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <wl:app channel-url="<%=WebConfigurationManager.AppSettings["wl_wrap_channel_url"]%>"
        callback-url="<%=WebConfigurationManager.AppSettings["wl_wrap_client_callback"]%>?wl_session_id=<%=SessionId%>"
        client-id="<%=WebConfigurationManager.AppSettings["wl_wrap_client_id"]%>" 
        scope="WL_Profiles.View, WL_Contacts.View">
    </wl:app>
        <wl:signin onsignin="{{signInComplete}}" signedouttext="Sign in" theme="white" />
        <hr />
        <table>
            <tr>
                <td>
                    <wl:userinfo />
                    <br />
                    <label>
                        Contact list (by DisplayName):</label><br />
                    <select id="Name_list">
                        <option value="">Display Names</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

