<%

Function WeatherFunctions()
	dim strAcid, strOut
	strAcid = "WAKI"
	if Request("AID") <> "" then strAcid = Request("AID")
	
	strOut = strOut & "function WeatherMain(sAcid) { " & vbCrLf
	strOut = strOut & "	var sWPQ = 'WPQ2'; " & vbCrLf
	strOut = strOut & "	eval('var oDiv = msnbcWea' + sWPQ); " & vbCrLf
	strOut = strOut & "	eval('var oData = msnbcWeaData' + sWPQ); " & vbCrLf
	strOut = strOut & "	oDiv.style.padding = ""5px""; " & vbCrLf
	strOut = strOut & "	if(sAcid == '') { " & vbCrLf
	strOut = strOut & "		oDiv.innerHTML = 'Click here to select your city.'; " & vbCrLf
	strOut = strOut & "	} else { " & vbCrLf
	strOut = strOut & "		oData.onreadystatechange = ShowWeather; " & vbCrLf
	strOut = strOut & "		oData.src = 'http://www.msnbc.com/m/chnk/d/weather_d_src.asp?acid=' + sAcid; " & vbCrLf
	strOut = strOut & "	} " & vbCrLf
	strOut = strOut & "} " & vbCrLf

	strOut = strOut & "function ShowWeather() { " & vbCrLf
	strOut = strOut & "	var sWPQ = 'WPQ2'; " & vbCrLf
	strOut = strOut & "	eval('var oDiv = msnbcWea' + sWPQ); " & vbCrLf
	strOut = strOut & "	try { " & vbCrLf
	strOut = strOut & "		var oWea = new makeWeatherObj(); " & vbCrLf
	strOut = strOut & "		oDiv.innerHTML = RenderWeather(oWea); " & vbCrLf
	strOut = strOut & "	} " & vbCrLf
	strOut = strOut & "	catch(exception) { " & vbCrLf
	strOut = strOut & "		if (exception.number = -2146823279) { " & vbCrLf
	strOut = strOut & "			// this means object is undefined - page is still loading " & vbCrLf
	strOut = strOut & "		} else { " & vbCrLf
	strOut = strOut & "			oDiv.innerHTML = '<br><br><font size=-1>This data is<br>temporarily unavailable.<br><br></font><br><br>';" & vbCrLf
	strOut = strOut & "		} " & vbCrLf
	strOut = strOut & "	} " & vbCrLf
	strOut = strOut & "} " & vbCrLf

	strOut = strOut & "function WeatherPers() { " & vbCrLf
	strOut = strOut & "	var sReturn = self.showModalDialog('http://www.msnbc.com/m/dd/dd_pers.asp?wp=wea','','dialogHeight:150px;dialogWidth:300px;status:no;help:no;'); " & vbCrLf
	strOut = strOut & "	self.location.reload(); " & vbCrLf
	strOut = strOut & "} " & vbCrLf

	strOut = strOut & "if(navigator.userAgent.indexOf(""MSIE 5"")>0 || navigator.userAgent.indexOf(""MSIE 6"") > 0) { " & vbCrLf
	strOut = strOut & "	WeatherMain('" + strAcid + "'); " & vbCrLf
	strOut = strOut & "} else { " & vbCrLf
	strOut = strOut & "	document.write('This webpart requires Microsoft Internet Explorer 5.x<br><font size=-2>You are using '+navigator.userAgent+'</font>'); " & vbCrLf
	strOut = strOut & "}" & vbCrLf

	Response.Write(strOut)
End Function
%>
