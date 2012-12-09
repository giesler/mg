<%

Function ShowQuote(pstrQuoteText, pstrQuoteDate, pstrQuoteLocation, pstrQuoteEnteredBy, pstrQuoteBy, pstrQuoteComment)

%>
<table width=600 border=0 align=center cellpadding=4 class="clsNormalTable">
	<tr>
		<td width=450><%=NW(Nb(pstrQuoteText))%></td>
		<td width=150 rowspan=2><font size=-2>
			<font class="clsNoteHead"><i>When?</i></font><br><%=Nb(pstrQuoteDate)%><br>
			<font class="clsNoteHead"><i>Where?</i></font><br><%=Nb(pstrQuoteLocation)%><br>
			<font class="clsNoteHead"><i>Witness?</i></font><br><%=Nb(pstrQuoteEnteredBy)%><br>
		</td>
	</tr><tr>
		<td width=400 align=right><i>- <%=pstrQuoteBy%></i></td>
	</tr>
<% if pstrQuoteComment <> "" then %>
	<tr>
		<td width=600 colspan=2><font class="clsNoteHead">
			<%=NW(pstrQuoteComment)%></font>
		</td>
	</tr>
<% end if %>
</table>

<%

End Function

%>
