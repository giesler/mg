<%@ Control Language="c#" AutoEventWireup="false" Codebehind="_Stats.ascx.cs" Inherits="AMS_Welcome.AMSStats"%>
<%@ OutputCache Duration="600" VaryByParam="none" %>
<asp:Repeater id="rpScores" runat="server">
	<HeaderTemplate>
		<table cellpadding="0" cellspacing="0">
			<tr>
				<td colspan="3">
					<p>
						Top Scores
					</p>
				</td>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td width="10">
				&nbsp;
			</td>
			<td>
				<p>
					<%# DataBinder.Eval(Container.DataItem, "place") %>
					.
				</p>
			</td>
			<td>
				<p>
					<%# DataBinder.Eval(Container.DataItem, "score") %>
				</p>
			</td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:Repeater>
<table cellpadding="0" cellspacing="0">
	<tr>
		<td colspan="2">
			<p>
				Total unique files:
			</p>
		</td>
	</tr>
	<tr>
		<td width="10">
			&nbsp;
		</td>
		<td>
			<p>
				<asp:label id="lblTotalUniqueFiles" runat="server">
					(unavailable)</asp:label>
			</p>
		</td>
	</tr>
</table>
<table width="100%">
	<tr>
		<td align="right">
			<font size="-2"><i>Updated&nbsp;
					<asp:label id="lblUpdateTime" runat="server">
					</asp:label>
					CST </i></font>
		</td>
	</tr>
</table>
