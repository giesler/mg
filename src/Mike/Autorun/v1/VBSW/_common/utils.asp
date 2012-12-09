<%

'conn string for sami demo db
dim sConn
sConn = "Provider=SQLOLEDB.1;Persist Security Info=True;"
sConn = sConn + "User ID=vbsw;Password=vbsw;"
'if Instr(Request.ServerVariables("SERVER_NAME"), "esami.samimro.com") > 0 then
	sConn = sConn + "Initial Catalog=vbsw;Data Source=(local);"
'else
'	sConn = sConn + "Initial Catalog=samidemo;Data Source=esami.samimro.com;"
'end if
sConn = sConn + "Use Procedure for Prepare=1;Auto Translate=True;"
sConn = sConn + "Packet Size=4096;Workstation ID=webserver;"
sConn = sConn + "Network Library=dbmssocn"

Function qt(sIn)

Dim i

for i = 1 to Len(sIn)
	if Mid(sIn, i, 1) = "'" then
		qt = qt + "'" + "'"
	else
		qt = qt + Mid(sIn, i, 1)
	end if
next

qt = "'" + qt + "'"

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

Function RunSQL(s)

dim cn
set cn = server.CreateObject("ADODB.Connection")
cn.open sConn
cn.Execute s
cn.Close
set cn = nothing

End Function

Function OpenRS(sSQL)

dim rs
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
OpenRS = rs

End Function

Function RemoveSecs(s)

dim sTmp
sTmp = Left(s, Instr(s, ":00 ")-1) + " " + mid(s, instr(s, ":00 ") + 4)
RemoveSecs = sTmp

End Function


Function Nz(vIn, vReplace)

if isnull(vIn) then
	Nz = vReplace
else
	Nz = vIn
end if

end function

Function nb(sIn)
if sIn = "" then 
	nb = "&nbsp;"
elseif isnull(sIn) then
	nb = "&nbsp;"
else
	nb = sin
end if
end function

Function FillDropDown(sSQL, sFieldID, sFieldValue)

dim rs, sOut
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
do while not rs.EOF
	sOut = sOut + "<option value=" + CStr(rs.Fields(sFieldID)) + ">"
	sOut = sOut + CStr(rs.Fields(sFieldValue))
	sOut = sOut + "</option>"
	rs.MoveNext
loop
rs.close
set rs = nothing
FillDropDown = sOut

End Function


Function CreateRadioButtons(sGroup, sSQL, sFieldID, sFieldValue)

dim rs, sOut, bSelected, sClass
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
bSelected = false
sClass = "clsWizBody1"
if rq(sFieldID) <> "" then			' we already set this value, simply display it
	sOut = "<tr class=""" + sClass + """><td valign=top colSpan=2>"
	rs.filter = sFieldID + " = " + rq(sFieldID)
	sOut = sOut + rs.fields(sfieldvalue)
	sOut = sOut + "<input type=""hidden"" name=""" + CStr(sGroup) + """ value=""" + rq(sFieldID) + """>"
	sOut = sOut + "</td></tr>"
else														' we do need buttons
	do while not rs.EOF
		if bSelected then
			sOut = sOut + "<tr class=""" + sClass + """><td valign=top><input type=""radio"" name=""" + CStr(sGroup) + """ value=""" + CStr(rs.Fields(sFieldID)) + """ id=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """></td>"
		else
			sOut = sOut + "<tr class=""" + sClass + """><td valign=top><input type=""radio"" name=""" + CStr(sGroup) + """ value=""" + CStr(rs.Fields(sFieldID)) + """ id=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """ checked></td>"
			bSelected = true
		end if
		sOut = sOut + "<td><label for=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """>" + CStr(rs.Fields(sFieldValue)) + "</label></td></tr>"
		sClass = ReverseClass(sClass)
		rs.MoveNext
	loop
end if
rs.close
set rs = nothing
CreateRadioButtons = sOut

End Function


Function CreateRadioButtons2(sGroup, sSQL, sFieldID, sFieldValue)

dim rs, sOut, bSelected
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
bSelected = false
sOut = "<tr><td><input type=""radio"" name=""" + CStr(sGroup) + """ value=""0"" id=""" + CStr(sGroup) + "0"" checked></td>"
sOut = sOut + "<td><label for=""" + sGroup + "0"">- All -</label></td>"
do while not rs.EOF
	sOut = sOut + "<tr><td valign=top><input type=""radio"" name=""" + CStr(sGroup) + """ value=""" + CStr(rs.Fields(sFieldID)) + """ id=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """></td>"
	sOut = sOut + "<td><label for=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """>" + CStr(rs.Fields(sFieldValue)) + "</label></td></tr>"
	rs.MoveNext
loop
rs.close
set rs = nothing
CreateRadioButtons2 = sOut

End Function


Function LookupText(sSQL)

dim rs
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
if rs.bof and rs.eof then
	LookupText = ""
else
	LookupText = nz(rs.Fields(0), "")
end if
rs.close
set rs = nothing

End Function

function rw(s)
	Response.Write s
end function

function rwbr(s)
	Response.Write s & "<br>"
end function

function rwp(s)
	Response.Write "<p>" & s & "</p>"
end function

function rr(s)
	Response.Redirect(s)
end function

function rq(s)
	rq = Request.QueryString(s)
end function

function rf(s)
	rf = Request.Form(s)
end function

function sh(s)
	sh = Server.HTMLEncode(s)
end function

function lt(s)
	if s = "OrganizationName" then
		lt = LookupText("select Name from Organization where OrganizationID = " + Request.Cookies("eSAMI")("OrganizationID"))
	elseif s = "LocationName" then
		lt = LookupText("select Division from Organization where OrganizationID = " + Request.Cookies("eSAMI")("OrganizationID"))
	elseif s = "EmployeeName" then
		lt = LookupText("select LastName + ', ' + FirstName from Employee where EmployeeID = " + rq("EmployeeID"))
	else
		lt = "Error in lt function!"
	end if
end function

function ReverseClass(sClass)
	if Right(sClass, 1) = "1" then
		ReverseClass = Left(sClass, Len(sClass) - 1) + "2"
	else
		ReverseClass = Left(sClass, Len(sClass) - 1) + "1"
	end if		
end function

function nbsp(n)
	dim i, str
	
	for i = 0 to n
		str = str & "&nbsp;"
	next
	Response.Write(str)
end function

Function rw(s)
	Response.Write(s)
End Function

function rf(s)
  rf = Request.Form(s)
end function

Function dl(sTable, sField, sWhere)

dim rs, sSQL
set rs = Server.CreateObject("ADODB.Recordset")
sSQL = "select [" + sField + "] from " + sTable + " where " + sWhere
rs.Open sSQL, sConn
if rs.BOF and rs.EOF then
	dl = ""
else
	dl = rs.Fields(sField)
end if
rs.close
set rs = nothing

End Function



Function CreateCheckBoxes(sGroup, sSQL, sFieldID, sFieldValue)

dim rs, sOut, bSelected, sClass
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
bSelected = false
sClass = "clsWizBody1"
if rq(sFieldID) <> "" then			' we already set this value, simply display it
	dim vArr, iCur
	vArr = split(rq(sFieldID), ",")
	sOut = "<tr class=""" + sClass + """><td valign=top colSpan=2>"
	sOut = sOut + "<input type=""hidden"" name=""" + CStr(sGroup) + """ value=""" + rq(sFieldID) + """>"	
	for iCur = 0 to UBound(vArr)	
		rs.filter = sFieldID + " = " + vArr(iCur)
		sOut = sOut + rs.fields(sfieldvalue)
		if iCur <> UBound(vArr) then sOut = sOut + " and <br>"
	next
	sOut = sOut + "</td></tr>"
else														' we do need buttons
	do while not rs.EOF
		if bSelected then
			sOut = sOut + "<tr class=""" + sClass + """><td valign=top><input type=""checkbox"" name=""" + CStr(sGroup) + """ value=""" + CStr(rs.Fields(sFieldID)) + """ id=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """></td>"
		else
			sOut = sOut + "<tr class=""" + sClass + """><td valign=top><input type=""checkbox"" name=""" + CStr(sGroup) + """ value=""" + CStr(rs.Fields(sFieldID)) + """ id=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """ checked></td>"
			bSelected = true
		end if
		sOut = sOut + "<td valign=top><label for=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """>" + CStr(rs.Fields(sFieldValue)) + "</label></td></tr>"
		rs.MoveNext
		sClass = ReverseClass(sClass)
	loop
end if
rs.close
set rs = nothing
CreateCheckBoxes = sOut

End Function


Function CreateCheckBoxes2(sGroup, sSQL, sFieldID, sFieldValue)

dim rs, sOut, bSelected, sClass
set rs = Server.CreateObject("ADODB.Recordset")
rs.Open sSQL, sConn
bSelected = false
sClass = "clsWizBody1"
if rq(sFieldID) <> "" then			' we already set this value, simply display it
	dim vArr, iCur
	vArr = split(rq(sFieldID), ",")
	sOut = "<tr class=""" + sClass + """><td valign=top colSpan=2>"
	sOut = sOut + "<input type=""hidden"" name=""" + CStr(sGroup) + """ value=""" + rq(sFieldID) + """>"	
	for iCur = 0 to UBound(vArr)	
		rs.filter = sFieldID + " = " + vArr(iCur)
		sOut = sOut + rs.fields(sfieldvalue)
		if iCur <> UBound(vArr) then sOut = sOut + " and <br>"
	next
	sOut = sOut + "</td></tr>"
else														' we do need buttons
	do while not rs.EOF
		sOut = sOut + "<tr class=""" + sClass + """><td valign=top><input type=""checkbox"" name=""" + CStr(sGroup) + """ value=""" + CStr(rs.Fields(sFieldID)) + """ id=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """></td>"
		sOut = sOut + "<td valign=top><label for=""" + CStr(sGroup) + CStr(rs.fields(sFieldID)) + """>" + CStr(rs.Fields(sFieldValue)) + "</label></td></tr>"
		rs.MoveNext
		sClass = ReverseClass(sClass)
	loop
end if
rs.close
set rs = nothing
CreateCheckBoxes2 = sOut

End Function

Function RemoveSV(sQS, sSV)
' removes a server variable from a querystring

dim iStart, iEnd, sTemp

iStart = Instr(sQS, sSV)
if iStart = 0 then 
	RemoveSV = sQS
	exit function
end if

iEnd = Instr(iStart, sQS, "&")
if iEnd = 0 then iEnd = Len(sQS)
sTemp = Left(sQS, iStart-1) + Mid(sQS, iEnd +1)
if Right(sTemp, 1) = "&" then sTemp = Left(sTemp, Len(sTemp)-1)
RemoveSV = sTemp

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

Function TailCut(sIn, sMatch)

dim i, sTmp

' make sure match is in string
if instr(sIn, sMatch) = 0 then
	TailCut = sIn
	exit function
end if

for i = len(sIn) to 0 step -1
	if mid(sIn, i, Len(sMatch)) = sMatch then
		TailCut = Mid(sIn, i+1)
		exit function
	end if
next

TailCut = sIn

End Function

Function bl(sIn, sOut)

if sIn = "" then 
	bl = sOut
else
	bl = sIn
end if

End Function

%>