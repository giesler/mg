<%

Function WeatherFunctions()
	dim strAcid 
	strAcid = "WAKI"
	if Request("AID") <> "" then strAcid = Request("AID")
%>

function WeatherMain(sAcid) {
	var sWPQ = 'WPQ2'; 
	eval('var oDiv = msnbcWea' + sWPQ); 
	eval('var oData = msnbcWeaData' + sWPQ); 
	oDiv.style.padding = "5px"; 
	if(sAcid == '') { 
		oDiv.innerHTML = 'Click here to select your city.'; 
	} else { 
		oData.onreadystatechange = ShowWeather; 
		oData.src = 'http://www.msnbc.com/m/chnk/d/weather_d_src.asp?acid=' + sAcid; 
	} 
} 

function ShowWeather() { 
	var sWPQ = 'WPQ2'; 
	eval('var oDiv = msnbcWea' + sWPQ); 
	try { 
		var oWea = new makeWeatherObj(); 
		oDiv.innerHTML = RenderWeather(oWea); 
	} catch(exception) { 
		if (exception.number = -2146823279) {
			// this means object is undefined - page is still loading
		} else {
			oDiv.innerHTML = "<br><br><font size=-1>This data is<br>temporarily unavailable.<br><br></font><br><br>"; 
		}
	} 
} 

function WeatherPers() { 
	var sReturn = self.showModalDialog('http://www.msnbc.com/m/dd/dd_pers.asp?wp=wea','','dialogHeight:150px;dialogWidth:300px;status:no;help:no;'); 
	self.location.reload(); 
} 

if(navigator.userAgent.indexOf("MSIE 5")>0 || navigator.userAgent.indexOf("MSIE 6") > 0) {
	WeatherMain('<%=strAcid%>');
} else {
	document.write('This webpart requires Microsoft Internet Explorer 5.x<br><font size=-2>You are using '+navigator.userAgent+'</font>');
}

<%
End Function
%>
