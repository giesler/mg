<%@ Control Language="c#" AutoEventWireup="false" Codebehind="_Featured.ascx.cs" Inherits="AMS_Welcome.AMSFeatured"%>
<%@ OutputCache Duration="60" VaryByParam="none" %>
<h4>
	<asp:label runat="server" id="lblFeaturedAdTitle">
		Feature Site Name
	</asp:label>
</h4>
<p>
	<a href="http://www.adultmediaswapper.com/featured/" target="_blank">
		<asp:image id="imgFeatured" runat="server">
		</asp:image>
		<br>
		<strong>Click here for a review of this site.</strong></a>
</p>
