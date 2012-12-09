<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 BAC Calculator"
mstrOffset		 = "../"
mstrArea			 = "archives"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<script>

function computeIt(form) {

var lbs = form.weight.options[form.weight.selectedIndex].value;
var numLbs = new Number(lbs);

var drinks = form.drinks.options[form.drinks.selectedIndex].value;
var numDrinks = new Number(drinks);

var alco = numDrinks*.045;
var alcohol = new Number(alco);

var hours = form.time.options[form.time.selectedIndex].value;
var numHours = new Number(hours);

var metabol = numHours*.012;
var metabolism = new Number(metabol);

if (form.gender.options[form.gender.selectedIndex].value == "male") {
	gendervalue = ".58";
	} else {
		gendervalue = ".49";
		}
var numSex = new Number(gendervalue);


var weight = numLbs/2.2046;
var lbs = new Number(weight);

var water = lbs*numSex;
var bodywater = new Number(water);

var millwater = bodywater*1000;
var milliliters = new Number(millwater);

var oz = 23.36/milliliters;
var ounces = new Number(oz);

var gram = ounces*.806;
var grams = new Number(gram);

var gramsmill = grams*100;
var bac0 = new Number(gramsmill);

var bac1 = bac0*alcohol;
var bac2 = new Number(bac1);

var bac3 = bac2-metabolism;
if (bac3 < 0) {
	var result = "0.0000";
}
var bac4 = bac3 + "";
var result = bac4.substring(0,6);

form.average.value = result;


}


</script>
<script language="javascript"><!--//

function CheckBrowser(){
        ns2 = "false";
        if ((navigator.appName.indexOf("Netscape") > -1)&&(parseInt(navigator.appVersion) < 3)) {
                ns2 = "true";
        }
        if (ns2 == "true"){
                document.boozestuff.baclevel.value = "This may fail in NS 2.0.";
                document.boozestuff.drinks.value = "Please Upgrade.";


                       } else {
                 document.boozestuff.baclevel.value = " ";
        }
}


function ValidateInput(checkVal){
        len = checkVal.length;
        for (i = 0; i<len; i++) {
        if (!((checkVal.substring(i, i+1)=="0") ||
                (checkVal.substring(i,i+1)=="1") ||
                (checkVal.substring(i,i+1)=="2") ||
                (checkVal.substring(i,i+1)=="3") ||
                (checkVal.substring(i,i+1)=="4") ||
                (checkVal.substring(i,i+1)=="5") ||
                (checkVal.substring(i,i+1)=="6") ||
                (checkVal.substring(i,i+1)=="7") ||
                (checkVal.substring(i,i+1)=="8") ||
                (checkVal.substring(i,i+1)=="9") ||
                (checkVal.substring(i,i+1)=="."))) {
                errorMsg = "You have entered an invalid number. Please enter a new amount and try again.";
                        return true;
                }
        }
        if (!((checkVal >= 0) & (checkVal < 10000))) {
                errorMsg = "Sorry we're not calculating for elephants.";
                return true;
        } else {
        return false;
        }
}


function doStuff(form)  {

var compactmpg = 31.5;
var midmpg = 22;
var bac = 0;
var numdrinks = document.boozestuff.drinks.value;
var pounds = document.boozestuff.weight.value;
var sex = (document.boozestuff.gender.options[document.boozestuff.gender.selectedIndex].value);
var hours = document.boozestuff.time.value;
var leveltemp = 0;

        if (ValidateInput(pounds)) {
            window.alert(errorMsg);
                distance = 0;
                }

        if (ValidateInput(numdrinks)) {
            window.alert(errorMsg);
                distance = 0;
                }

        if (ValidateInput(hours)) {
            window.alert(errorMsg);
                distance = 0;
                }


        if (sex == 1)  {
			if (pounds <= 87)  {
				bac = numdrinks * .0547;
				alert('Do your mommy and daddy know you drink?');
			} else if (pounds > 87 && pounds <= 112)  {
				bac = numdrinks * .0435;
			} else if (pounds > 112 && pounds <=137) {
				bac = numdrinks * .0346;
			} else if (pounds > 137 && pounds <=162) {
				bac = numdrinks * .0290;
			} else if (pounds > 162 && pounds <=187) {
				bac = numdrinks * .0250;
			} else if (pounds > 187 && pounds <=212) {
				bac = numdrinks * .0217;
			} else if (pounds > 212 && pounds <=237) {
				bac = numdrinks * .0195;
			} else if (pounds > 237) {
				bac = numdrinks * .0173;
            }
	}

        if (sex == 2)  {
			if (pounds <= 87)  {
				bac = numdrinks * .0607;
				alert('Do your mommy and daddy know you drink?');
			} else if (pounds > 87 && pounds <= 112)  {
				bac = numdrinks * .0507;
			} else if (pounds > 112 && pounds <=137) {
				bac = numdrinks * .0404;
			} else if (pounds > 137 && pounds <=162) {
				bac = numdrinks * .0338;
			} else if (pounds > 162 && pounds <=187) {
				bac = numdrinks * .0292;
			} else if (pounds > 187 && pounds <=212) {
				bac = numdrinks * .0253;
			} else if (pounds > 212 && pounds <=237) {
				bac = numdrinks * .0227;
			} else if (pounds > 237) {
				bac = numdrinks * .0202;
            }
                }
		leveltemp = bac - (hours * .015);

		if (leveltemp < 0)  {
			leveltemp = 0;
		}


        document.boozestuff.baclevel.value = (Math.round(1000 * leveltemp)/1000);

	bacval = document.boozestuff.baclevel.value;
	if ( bacval > 0.10 && bacval <= 0.20 ) {
		alert ('Your level is ' + bacval + '.  You are legally drunk.');
	} else if ( bacval > 0.20 && bacval <= 0.30 ) {
		alert ('Your level is ' + bacval + '.  You probably could not actually be entering these numbers.');
	} else if ( bacval > 0.30 && bacval <= 0.40 ) {
		alert ('Your level is ' + bacval + '.  Can anyone say DETOX?');
	} else if ( bacval > 0.40 && bacval <= 0.50 ) {
		alert ('Your level is ' + bacval + '.  You are dead.');
	} else if ( bacval > 0.50 ) {
		alert ('Your level is ' + bacval + '.  You are past dead.  More like alcohol-blood content...');
	}
  }



data = new Array

function State(total,alc,percent) {
	this.total=total
	this.alc=alc
	this.percent=percent
}

data[1] = new State("1113","462","41.5%")
data[2] = new State("87","47","54.0%")
data[3] = new State("1031","447","43.4%")
data[4] = new State("631","217","34.4%")
data[5] = new State("4192","1720","41.0%")
data[6] = new State("645","294","45.6%")
data[7] = new State("317","155","48.9%")
data[8] = new State("121","50","41.3%")
data[9] = new State("58","32","55.2%")
data[10] = new State("2805","1110","39.6%")
data[11] = new State("1488","522","35.1%")
data[12] = new State("130","64","49.2%")
data[13] = new State("262","89","34.0%")
data[14] = new State("1586","681","42.9%")
data[15] = new State("960","331","34.5%")
data[16] = new State("527","219","41.6%")
data[17] = new State("442","179","45%")
data[18] = new State("849","287","33.8%")
data[19] = new State("883","470","53.2%")
data[20] = new State("187","52","27.8%")
data[21] = new State("671","234","34.9%")
data[22] = new State("444","203","45.7%")
data[23] = new State("1530","616","43%")
data[24] = new State("597","265","44.4%")
data[25] = new State("868","361","41.6%")
data[26] = new State("1109","572","51.6%")
data[27] = new State("215","91","42.3%")
data[28] = new State("254","93","36.6%")
data[29] = new State("313","154","49.2%")
data[30] = new State("118","46","39.0%")
data[31] = new State("773","316","40.9%")
data[32] = new State("485","244","50.3%")
data[33] = new State("1674","542","32.4%")
data[34] = new State("1448","489","33.8%")
data[35] = new State("74","43","58.1%")
data[36] = new State("1366","440","32.2%")
data[37] = new State("669","251","37.5%")
data[38] = new State("572","237","41.4%")
data[39] = new State("1480","610","41.2%")
data[40] = new State("69","29","42.0%")
data[41] = new State("881","281","31.9%")
data[42] = new State("158","71","44.9%")
data[43] = new State("1259","512","40.7%")
data[44] = new State("3181","1782","56.0%")
data[45] = new State("326","86","26.4%")
data[46] = new State("106","44","41.5%")
data[47] = new State("900","358","39.8%")
data[48] = new State("653","317","48.5%")
data[49] = new State("376","160","42.6%")
data[50] = new State("745","317","42.6%")
data[51] = new State("170","83","48.8%")

function PickState(num) {
	if (num>0) {
		document.forms[0].total.value = data[num].total
		document.forms[0].alc.value = data[num].alc
		document.forms[0].percent.value = data[num].percent
	}
}



  //--></script>
<script
language="javascript">

<!--

function Checkitout(){

//      Gets Browser and Version

        var appver = "null";
        var browser = navigator.appName;
        var version = navigator.appVersion;
        if ((browser == "Netscape")) version = navigator.appVersion.substring(0, 3);
        if ((browser == "Microsoft Internet Explorer")) version = navigator.appVersion.substring(22, 25);

//      Gives AppVersion (appver) for Detect Strings

        if ((browser == "Microsoft Internet Explorer") && (version >= 3)) appver = "ie3+";
        if ((browser == "Netscape") && (version >= 3)) appver = "ns3+";
        if ((browser == "Netscape") && (version < 3)) appver = "ns2";


       if ((appver == "ie3+")) {
                return 0;
        }  else {
                return 1;
                }
}




function ShowStuffInWindow(DaURL) {
        var ItsTheWindow;
		if (Checkitout())  {
	        ItsTheWindow = window.open(DaURL,"himom","status,height=435,width=395,scrolling=no,resizable=no,toolbar=0");
		} else {
	        ItsTheWindow = window.open(DaURL,"himom","scrollbars=no,menubar=no,toolbar=no,links=no,status=no,width=395,height=435,resizable=no");
		}

		if (Checkitout()){

			ItsTheWindow.focus();

        }

}




//-->

</script>

<div align="center"><center>

<table border="0" cellpadding="5" cellspacing="0" width="98%" height="98%">
  <tr>
    <td width="100%" valign="middle" align="center"><div align="center"><center><table
    border="0" cellpadding="5" cellspacing="5">
      <tr>
        <td><form name="boozestuff" align="center">
          <div align="center"><center><table border="0" cellpadding="3" cellspacing="0">
            <tr>
              <td><table width="248" cellpadding="4" cellspacing="0" border="1" hspace="2"
              bgcolor="#C00000">
                <tr>
                  <td align="center" colspan="4" bgcolor="#000000" width="240"><font size="3"
                  face="geneva, arial, helvetica" color="ffffff"><b>Know Your Limit</b></font></td>
                </tr>
                <tr>
                  <td align="center" colspan="4" width="240"><table bgcolor="#FFF8D9" border="0"
                  cellspacing="4">
                    <tr>
                      <td align="center"><small><small><font face="geneva, arial, helvetica" color="#000000"><b>Enter
                      the information to find out your approximate blood-alcohol level.</b> </font></small></small></td>
                    </tr>
                  </table>
                  </td>
                </tr>
                <tr>
                  <td align="right" width="62"><font face="geneva, arial, helvetica" size="1"
                  color="#FFFFFF"><strong>Drinks:</strong></font></td>
                  <font size="-1"><td width="62"><input TYPE="text" name="drinks" size="4"></font></td>
                  <td width="62"><font face="geneva, arial, helvetica" size="1" color="#FFFFFF"><strong>Hours
                  spent drinking:</strong></font></td>
                  <font size="-1"><td width="62"><input TYPE="text" name="time" size="4"></font></td>
                </tr>
                <tr>
                  <td align="right" width="62"><font face="geneva, arial, helvetica" size="1"
                  color="#FFFFFF"><strong>Weight:</strong></font></td>
                  <font size="-1"><td width="62"><input TYPE="text" name="weight" size="4"></font></td>
                  <td width="62"><font face="geneva, arial, helvetica" size="1" color="#FFFFFF"><strong>Gender:</strong></font></td>
                  <font size="-1"><td width="62"><font face="geneva, arial, helvetica" color="#000000"
                  size="2"><select name="gender" size="1">
                    <option selected value="1">Male </option>
                    <option value="2">Female </option>
                  </select></font></font></td>
                </tr>
                <tr>
                  <td align="center" colspan="4" width="240"><font face="geneva, arial, helvetica"
                  color="#000000" size="2"><input TYPE="button" value="Calculate" onClick="doStuff()"></font></td>
                </tr>
                <tr>
                  <td align="center" colspan="4" bgcolor="#000000" width="240"><font
                  face="geneva, arial, helvetica" size="2" color="#FFFFFF"><b>Your BAC is:</b></font><font
                  face="geneva, arial, helvetica" color="#000000" size="-1"><br>
                  <input TYPE="text" name="baclevel" size="6"> </font></td>
                </tr>
              </table>
              </td>
            </tr>
          </table>
          </center></div>
        </form>
        </td>
      </tr>
    </table>
    </center></div></td>
  </tr>
</table>
</center></div>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>
