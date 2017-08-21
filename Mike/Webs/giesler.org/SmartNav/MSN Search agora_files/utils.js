var H_CONFIG='searchv3.ini';
var bSearch=true;
var H_BRAND='';
var H_FILTER='';
var H_TOPIC='';
var bScreen=false;

if( ( navigator.userAgent.indexOf("Nav") > 0 ) || ( navigator.userAgent.indexOf("Mozilla/4.5") > -1 ) ){
	var agent_isNS = 1;
} else {
	var agent_isNS = 0
}

if(navigator.userAgent.indexOf("Mac") > 0){
	var agent_isMac = 1;
} else {
	var agent_isMac = 0;
}

var agent_Major = parseInt(navigator.appVersion);

var agent_isAOL = 0;


function GoDefaultRedir(oAnchor, sForm)
{
	return GoRedir("/rider.asp?target=", oAnchor, sForm);
}

function GoRedir(sBase, oAnchor, sForm )
{
	var sURL = sBase + escape( oAnchor.href ) + "&FORM=" + sForm;
	
	// UNDONE: IE only, need browser detection here
	window.location.href = sURL;
	
	return false;
}

// Checkquery
// Determines whether form has proper submission criterion
//
// RETURNS: false if no value given for query param

function CheckMT(objMT, strID,qform)
{

	if (strID == "")
	{
		window.alert(notextalert);
		SetFocus(objMT);
		return false;
	}else{
	if ((hasspellchecking = "on") && (qform.origq.value == qform.q.value)) {
		qform.nosp.value = 1;

	}else{
		qform.nosp.value = 0;
	}
	return true;
	}
}	

// SetFocus()
// Given an element, it will set the focus

function SetFocus(objMT)
{
	objMT.focus();
	//document.forms[0].q.focus();
}

// rollover code for Info trans gifs...
//sniffs for ie3 or 4 and delivers the goods :)  image swaps for IE3 on Mac browser only)
//ignores netscape browsers
function toggleInfoGifs(p_obj)
{
	if((navigator.appVersion.charAt(0) >= 3) && (navigator.appName.indexOf("Explorer") > -1)){
		if(p_obj.src.indexOf("/images/infotransOFF.gif") > -1){
			p_obj.src="/images/infoTransON.gif"
		}else{
			p_obj.src="/images/infotransOFF.gif"
		}
	}
}

function newImage(arg) {
	if (document.images) {
		rslt = new Image();
		rslt.src = arg;
		return rslt;
	}
}

function changeImages() {  
	if (document.images && (preloadFlag == true)) {
		for (var i=0; i<changeImages.arguments.length; i+=2) {
			document[changeImages.arguments[i]].src = changeImages.arguments[i+1];
		}
	}
}

var preloadFlag = false;
function preloadImages() {
	if (document.images) {
		next_01_over = newImage("images/next_button_on.gif");
		prev_01_over = newImage("images/prev_button_on.gif");
		preloadFlag = true;
	}
}

if( navigator.appVersion.indexOf("4.")>=0) bScreen=true;

// Wrapper function to allow me to modify the variables in the Help call and not have to set global variables to do so
//
// fWrapHelp( IN v_bSearch, IN v_H_KEY, IN v_L_H_TEXT )
// WHERE
// v_bSearch = boolean value identifying whether this is a search in help or a topic
// v_H_KEY = if v_bSearch is false, then this is a topic, else it is a secret keyword
// v_L_H_TEXT = if v_bSearch is false, then it's ignored, else it is a localized string displayed in UI
function fWrapHelp( v_bSearch, v_H_KEY, v_L_H_TEXT )
{
	//Set each var and then call DoHelp()
	//If a Search in enabled, then we need a KEY and TEXT values, otherwise it's a topic and we just need TOPIC
	if( v_bSearch )
	{
		bSearch = v_bSearch;
		H_KEY = v_H_KEY;
		L_H_TEXT = v_L_H_TEXT;
	} else {
		H_TOPIC = v_H_KEY;
		bSearch = v_bSearch;
	}

	DoHelp();

	bSearch = true;
}




// ChangePage( obj )
// Modifies anchor to include query string
function ChangePage(objAnchor)
{
//alert("The anchor is\n" + objAnchor);
	var strQuery, intLoc, strPath;

	//Sanity check to be sure the value passed is an object
	if (typeof(objAnchor) == "object")
	{
		var strAnchor = objAnchor.href;
		strPath = window.location.pathname.substring( window.location.pathname.lastIndexOf( '/' ) + 1, window.location.pathname.length );
		//window.alert( "location.pathname = " + strPath );
		//determine if this page has valid controls to pass forward or not
		if( strPath == "default.asp" || strPath == "advanced.asp" || strPath == "results.asp" || strPath == "Preference.asp" || strPath== "searchclip.asp" || strPath == "searchclipcode.asp" || strPath == "searchclipTerms.asp" || strPath == "worldwide.asp" || strPath == "" )
		{
			if (strAnchor.indexOf("Preference.asp") < 0)
			{
				//window.alert( "Found an anchor object" );
				if (document.STWF){
					if( typeof(document.STWF.q ) == "object" )
					{
						//alert( "Found a q object" +document.STWF.q.value );
						document.STWF.cfg.value = "ADVANCED";
						document.STWF.submit();

						//write query string to originating anchor
						//if(navigator.appVersion.lastIndexOf("MSIE 3.") > -1 || navigator.appVersion.lastIndexOf("WebTV") > -1) // For all MSIE 3.0 versions
						//{
	//						strQuery = objAnchor.href + strQuery;
	//			      		window.top.location.href = strQuery;
						//} else {
							//strQuery = objAnchor.href + strQuery;
							//alert( "redirecting to " + strQuery );
						//	objAnchor.search = strQuery;
							//alert( "past the redirection" );
						//}
					}
				} else {
				self.location = "advanced.asp";
				}
			} else {
				window.top.location.href = strAnchor + "&newURL=" + escape(self.location);
			}
		}
		else 
		{
			//write query string to originating anchor
			if(navigator.appVersion.lastIndexOf("MSIE 3.") > -1 || navigator.appVersion.lastIndexOf("WebTV") > -1) // For all MSIE 3.0 versions
			{
				window.top.location.href = objAnchor.href + window.location.search;
			} else {
				if( objAnchor.search.length > 0 )
				{
					//alert( "you've already got a search " + objAnchor.search );
					//do nothing
				} else {
					//alert( "you come from a non-blessed page" + window.location.search );
					//objAnchor.search = window.location.search;
					window.top.location.href = objAnchor.href;
					 
				}
			}
		}
	}
}

function setCookie(key,value)
{
	var dc = document.cookie;
	var name = "msnsrch=";
	var nkey = key + "=";
	var newdc = name;
	if (dc.indexOf(name) > -1)
	{
		var flag = 0;
		dc = dc.substring(dc.indexOf(name) + name.length);
		var dce = dc.indexOf(';');
		if (dce > -1)
		{
			dc = dc.substring(0,dc.indexOf(";"));
		}
		// split the cookie at &
		var keys = dc.split("&");
		// loop through the existing keys
		for (var i = 0; i < keys.length; i++)
		{
			// if key exists
			if (keys[i].indexOf(nkey) > -1)
			{
				keys[i] = nkey + value;
				flag = 1;
			}
			newdc += keys[i];
			if (i < keys.length - 1)
			{
				newdc += "&";
			}
		}
		// if key does't exist 
		if (flag == 0)
		{
			newdc += "&" + nkey + value;
		}
	} else {
			newdc += nkey + value;
	}
	// date function
	var today = new Date();
	var expires = new Date(today.getTime() + 365 * 24 * 60 * 60 * 1000);
	newdc += ";PATH=/; expires=" + expires.toGMTString();
	// write cookie
	document.cookie = newdc;
}

function readCookie(key)
{
	var rkey = key + "=";
	var name = "msnsrch=";
	var value = "noValue";
	var dc = document.cookie;
	if (dc.indexOf(name) > -1)
	{
		dc = dc.substring(dc.indexOf(name) + name.length);
		var dce = dc.indexOf(";");
		if (dce > -1)
		{
			dc = dc.substring(0,dce);
		}
		var keys = dc.split("&");
		for (var i = 0; i < keys.length; i++)
		{
			if (keys[i].indexOf(rkey) > -1)
			{
				value = keys[i].substring(rkey.length);
			}
		}
	}
	
	return value;
}

function main()
{
// set the spell checking checkbox in qform.
var setSpelling = readCookie('nosp');
if (setSpelling != "1")
{
	if(document.STWF){
		if (document.STWF.spoff) {
			document.STWF.spoff.checked = true;
		}
	}

	if(document.STWF2){
		if (document.STWF2.spoff) {
			document.STWF2.spoff.checked = true;
		}
	}
}

if (document.STWF)
{
	SetFocus(document.STWF.q);
}

if (window.comppopup != null)
{
    if(document.all.item("prevImg")== null){
		comppopup();
    }
}

// preload images
	preloadImages();
}

function newUrl()
{
var strID = document.all.item("q").value;
if (strID == "")
	{
		window.alert(notextalert);
		SetFocus();
	}else{
		if (strID.indexOf("://") < 1)
		{
			strID = "http://" + strID;
		}
		self.location = strID;
	}
}

function tooltip()
{
	document.all.item("tips").style.display = "";
}

function GoMediaElem( oAnchor, sForm )
{
	var sURL = "/redir.asp?target=" + escape(oAnchor.href) + "&FORM=" + sForm;
	window.location.href = sURL;
	return false;
}
