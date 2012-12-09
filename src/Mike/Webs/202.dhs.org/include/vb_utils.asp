<%
Function QuoteText(sIn)

Dim i

for i = 1 to Len(sIn)
	if Mid(sIn, i, 1) = "'" then
		QuoteText = QuoteText + "'" + "'"
	else
		QuoteText = QuoteText + Mid(sIn, i, 1)
	end if
next

QuoteText = "'" + QuoteText + "'"

End Function

Function DQ(sIn)

Dim i

for i = 1 to Len(sIn)
	if Mid(sIn, i, 1) = """" then
		DQ = DQ + "&quot;"
	else
		DQ = DQ + Mid(sIn, i, 1)
	end if
next

End Function

'------------------------------------------
Function Nb(sIn)

if Len(sIn) = 0 then
	Nb = "&nbsp;"
else
	Nb = sIn
end if

End Function

Function IfBlankThen(sIn, sIn2)

if len(sIn) > 0 then
	IfBlankThen = sIn
else
	IfBlankThen = sIn2
end if

End Function

Function NewEmailMsg(sOffset, sID, sEmail)

Dim sOutput

sOutput = "<a href=""" & sOffset & "email/new.asp"
sOutput = sOutput & "?toid=" & sID
sOutput = sOutput & "&refurl=" & Server.URLEncode(Request.ServerVariables("SCRIPT_NAME"))
sOutput = sOutput & """>" & sEmail & "</a>"

NewEmailMsg = sOutput

End Function

Function NW(sIn)

for i = 1 to len(sIn)
	if mid(sIn, i, 1) = vbCr then
		NW = NW & "<br>"
	elseif mid(sIn, i, 1) <> vbLf then
		NW = NW & mid(sIn, i, 1)
	end if
next

End Function


Function IIf(bCond, sTrue, sFalse) 

if bCond then
	IIf = sTrue
else
	IIf = sFalse
end if

End Function

%>