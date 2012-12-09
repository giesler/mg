
<object runat="server" progid="ADODB.Connection" id="cnWebDB"></object>

<%
' --------------------------------------------------------------------------------------
' Contains code that will be executed/included on EVERY page
'  everything should be declared in gDeclares.asp
' --------------------------------------------------------------------------------------
cnWebDB.Open Application("cnWebDB_ConnectionString")

' --------------------------------------------------------------------------------------
' Common Functions
' --------------------------------------------------------------------------------------

' ---------------------
' Misc 'Get' functions
' ---------------------

Function GetBodyTag()

	dim strTemp

	' see if there is any javascript specified for onLoad
	if mstrOnLoad = "" then
		GetBodyTag = gstrBodyTag
	else
		' check if onload already in tag
		if InStr(LCase(gstrBodyTag), "onload=") > 0 then
			GetBodyTag = Replace(gstrBodyTag, ");" + mstrOnLoad)
		else
			GetBodyTag = gstrBodyTag + " onLoad=""" + mstrOnLoad + """"
		end if
	end if

End Function


' --------------------- 
' Table Class functions
' ---------------------

dim mstrTableBody
mstrTableBody = "clsTableBodyOdd"

Function GetTTag()
	GetTTag = gstrTableTag + " align=""center"""
End Function

Function GetTBodyTag(intSwap)
	if intSwap <> 0 then
		if mstrTableBody = "clsTableBodyOdd" then
			mstrTableBody = "clsTableBodyEven"
		else
			mstrTableBody = "clsTableBodyOdd"
		end if
	end if
	GetTBodyTag = "class=""" + mstrTableBody + """ valign=""top"""
End Function

Function InitTBodyTag()
	mstrTableBody = "clsTableBodyOdd"
End Function

Function GetTHeaderTag()
	GetTHeaderTag = "class=""clsTableHeader"" align=""center"""
End Function

Function GetTFooterTag()
	GetTFooterTag = "class=""clsTableFooter"""
End Function

' ---------------------
' Logic functions
' ---------------------
Public Function iif(expression, truepart, falsepart)
	If expression Then
		iif = truepart
	Else
		iif = falsepart
	End if
End Function

'Read input from querystring then form
Public Function GetInput(name)
	GetInput = Request.QueryString(name)
	If GetInput<>"" Then Exit Function
	GetInput = Request.Form(name)
End Function

'Form an array from both form and querystring
Public Function ArrayFromInput(name)
	Dim x, y, formcount, qscount, var()
	formcount = Request.Form(name).Count
	qscount = Request.QueryString(name).Count
	If formcount > 0 Or qscount > 0 Then
		ReDim var(formcount + qscount)
		For x = 1 To formcount
			var(x) = Request.Form(name).Item(x)
		Next
		For x = 1 To qscount
			var(formcount + x) = Request.QueryString(name).Item(x)
		Next
	Else
		ReDim var(0)
	End If
	For x = 1 To UBound(var) - 1
		If var(x)<>-1 Then
			For y = x + 1 To UBound(var)
				If var(y) = var(x) Then
					var(y) = -1
				End If
			Next
		End If
	Next
	ArrayFromInput = var
End Function

'----------------------
'Utility Functions
'----------------------

Function NewEmailMsg(pstrOffset, pstrID, pstrEmail)

	Dim strTemp
	strTemp = "<a href=""" & pstrOffset & "email/new.asp"
	strTemp = strTemp & "?toid=" & pstrID
	strTemp = strTemp & "&refurl=" & Server.URLEncode(Request.ServerVariables("SCRIPT_NAME"))
	strTemp = strTemp & """>" & pstrEmail & "</a>"
	NewEmailMsg = strTemp

End Function

Function Nb(sIn)

	if Len(sIn) = 0 then
		Nb = "&nbsp;"
	else
		Nb = sIn
	end if

End Function

Function NW(sIn)

	dim i
	for i = 1 to len(sIn)
		if mid(sIn, i, 1) = vbCr then
			NW = NW & "<br>"
		elseif mid(sIn, i, 1) <> vbLf then
			NW = NW & mid(sIn, i, 1)
		end if
	next

End Function

Function DQ(sIn)

	Dim i
	for i = 1 to Len(sIn)
		if Mid(sIn, i, 1) = "'" then
			DQ = DQ + "''"
		else
			DQ = DQ + Mid(sIn, i, 1)
		end if
	next

End Function

Function HashText(pstrIn)

	dim i, hc
	pstrIn = LCase(pstrIn)
	hc = 12
	for i = 1 to len(pstrIn)
		hc = Asc(Mid(pstrIn, i, 1)) * 21 + hc
		if hc > 1000 then hc = hc Mod 1000
	next
	HashText = "hc:" + CStr(hc)
	
End Function

' Add a var to a querystring
Public Function AddQSVar(strQS, strName, strValue) 
  dim strHREF 
  strHREF = RemoveQSVar(strQS, strName) 
  if len(strHREF) > 0 then 
    if Right(strHREF,1)<>"&" and Right(strHREF,1)<>"?" Then strHREF = strHREF & "&" 
  end if 
  strHREF = strHREF & strName & "=" & Server.URLEncode(strValue) 
  AddQSVar = strHREF 
End Function 

' removes a variable from a querystring 
Function RemoveQSVar(sQS, sSV) 
  dim iStart, iEnd, sTemp 
  iStart = Instr(lcase(sQS), lcase(sSV) + "=") 
  if iStart = 0 then 
    RemoveQSVar = sQS 
    exit function 
  end if 
  iEnd = Instr(iStart, sQS, "&") 
  if iEnd = 0 then iEnd = Len(sQS) 
  sTemp = Left(sQS, iStart-1) + Mid(sQS, iEnd +1) 
  if Right(sTemp, 1) = "&" then sTemp = Left(sTemp, Len(sTemp)-1) 
  RemoveQSVar = sTemp 
End Function 

%>
