<html>
	<head>
		<title>Date Picker</title>
		<style>
		<!--
		.hdr  {background:gray;color:#FFFFFF;border-width:1px;border-color:black;border-style:solid}
		.hdrA {background:gray;color:#FFFFFF;border-width:1px;border-color:black;border-style:solid;cursor:hand;}
		.ndt  {position:absolute;width:19;height:19;}
		.bdt  {position:absolute;width:19;height:19;}
		.dt   {position:absolute;width:19;height:19;cursor:hand;}
		.sdt  {position:absolute;width:19;height:19;}
		 //-->
		</style>
	</head>

<body BGCOLOR="#FFFFFF" TEXT="#000000" LINK="#990000" VLINK="#990000" ALINK="#990000" onLoad="DoLoad()">
<basefont FACE="Arial,Helvetica,Geneva,Swiss,Sans Serif">
<table BORDER="0" CELLPADDING="0" CELLSPACING="1" style="border-width:2px;border-color:black;border-style:solid;font:8pt arial;background:white;">
<tr><td HEIGHT="20">
<table BORDER="0" CELLPADDING="0" CELLSPACING="1" HEIGHT="20" style="font: 8pt arial;"><tr>
<td WIDTH="16" ID="PrevDiv" CLASS="hdrA"><img ID="Prev" SRC="prev.gif" onClick="PC();" ALT="Show Previous Month" WIDTH="16" HEIGHT="16"></td>
<td WIDTH="101" ALIGN="MIDDLE" CLASS="hdr"><span ID="MonthTitle"></span>&nbsp;&nbsp;<span ID="YearTitle"></span></td>
<td WIDTH="16" ID="NextDiv" CLASS="hdrA"><img ID="Next" SRC="next.gif" onClick="NC();" ALT="Show Next Month" WIDTH="16" HEIGHT="16"></td>
</tr></table>
</td></tr>
<tr><td>
<table BORDER="0" CELLPADDING="0" CELLSPACING="0">
<tr><td><img ID="WeekImg" SRC="week0.gif" WIDTH="141" HEIGHT="20"></td></tr>
<tr><td HEIGHT="1" BGCOLOR="#000000"></td></tr>
<tr><td ALIGN="MIDDLE" STYLE="position:relative;">
<img ID="SelDate" CLASS="sdt" STYLE="display:none;" SRC="seldate.gif" WIDTH="19" HEIGHT="19">
<img ID="MonthImg" SRC="months/blank.gif" STYLE="position:relative;left:0;top:0;" WIDTH="141" HEIGHT="121">
<div ID="BKIMG1">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
<img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19"><img SRC="date.gif" WIDTH="19" HEIGHT="19">
</div>
<img ID="Today" CLASS="ndt" STYLE="display:none;" SRC="ring.gif" onClick="TC()" WIDTH="19" HEIGHT="19">
</td></tr>
<tr><td HEIGHT="1" BGCOLOR="#000000"></td></tr>
</table>
</td></tr>
<tr><td HEIGHT="20" ALIGN="MIDDLE"><a STYLE="color: #336699" HREF="javascript:CC()">Cancel</a></td></tr>
</table>

<script>
<!--
var g_fCalLoaded=false;
var da=document.all;
var wp=window.parent;
var cf=wp.document.all.CalFrame;
var bdc=da.BKIMG1.children;
var dMin;var dMax;
var XOff=2;var YOff=1;
var XSize=20;var YSize=20;
var g_dC=-1;var g_mC=-1;var g_yC=-1;
var g_dI=-1;var g_mI=-1;var g_yI=-1;

function DoLoad() {
	for(i=0;i<7;i++) {
		for(j=0;j<6;j++) {
			var t=j*7+i;
			bdc[t].day=t+1;
			bdc[t].onclick=BC;
			bdc[t].className="dt";
			bdc[t].style.left=da.MonthImg.offsetLeft+XOff+XSize*i-1;
			bdc[t].style.top=da.MonthImg.offsetTop+YOff+YSize*j;
		}
	}
}

function TC() {
	if(event.srcElement.className=="dt") {
		var dt=new Date();
		wp.SetDate(dt.getDate(),dt.getMonth()+1,dt.getFullYear());
		cf.style.display="none";
	}
	event.cancelBubble = true;
}

function BC() {
	if(event.srcElement.className=="dt") {
		var iDay = event.srcElement.day;
		iDay-=GetDOW(1,g_mC,g_yC);
		wp.SetDate(iDay,g_mC,g_yC);
		cf.style.display="none";
	}
	event.cancelBubble=true;
}

function CC() {
	cf.style.display="none";
}

function NC() {
	if(g_mC==12) 
		SetDate(g_dC,1,g_yC+1);
	else 
		SetDate(g_dC,g_mC+1,g_yC);
}

function PC() {
	if(g_mC==1) 
		SetDate(g_dC,12,g_yC-1);
	else 
		SetDate(g_dC,g_mC-1,g_yC);
}

function SetInputDate(day,month,year) {
	g_dI = day;g_mI = month;g_yI = year;
}

function FmtTitle(str) {
	var r=str.charAt(0);
	for(i=1;i<str.length;i++) 
		r=r+"&nbsp;"+str.charAt(i);
	return r;		
}

function SetMinMax(min,max) {
	dMin=min;
	dMax=max;
}

function SetDate(day, month, year) {
	da.WeekImg.src="week"+wp.GetDowStart()+".gif";
	da.MonthImg.src="months/w"+GetDOW(1,month,year)+"d"+GetMonthCount(month,year)+".gif";
	da.MonthTitle.innerHTML=FmtTitle(rgMN[month-1]);
	da.YearTitle.innerHTML=FmtTitle(year.toString());
	var dt=new Date();
	var s,n,v,d;

	d="none";
	if (month==dt.getMonth()+1&&year==dt.getFullYear()) {
		iBox=dt.getDate()+GetDOW(1,dt.getMonth()+1,dt.getFullYear())-1;
		if (ValidDate(dt.getDate(),dt.getMonth()+1,dt.getFullYear())) 
			n="dt";
		else 
			n="bdt";
		da.Today.className=n;
		da.Today.style.left=bdc[iBox].style.left;
		da.Today.style.top=bdc[iBox].style.top;
		d="block";
	}
	da.Today.style.display=d;

	d="none";
	if (-1!=g_dI&&month==g_mI&&year==g_yI) {
		iBox=g_dI+GetDOW(1,g_mI,g_yI)-1;
		da.SelDate.style.left=bdc[iBox].style.left;
		da.SelDate.style.top=bdc[iBox].style.top;
		d="block";
	}
	da.SelDate.style.display=d;

	if (year<dMin.getFullYear() || (year==dMin.getFullYear()&&month<=(dMin.getMonth()+1)) ) {
		n="hdr";v="hidden";
	} else {
		n="hdrA";
		v="visible";
	}
	da.PrevDiv.className=n;
	da.Prev.style.visibility=v;

	if (year>dMax.getFullYear() || (year==dMax.getFullYear()&&month>=(dMax.getMonth()+1)) ) {
		n="hdr";v="hidden";
	} else {
		n="hdrA";
		v="visible";
	}
	da.NextDiv.className=n;
	da.Next.style.visibility=v;

	var i=0;
	var iMin=GetDOW(1,month,year);
	var iMax=GetMonthCount(month,year)+GetDOW(1,month,year);

	for(;i<iMin;i++) {
		bdc[i].src="nodate.gif";
		bdc[i].className="ndt";
	}

	if (year<dMin.getFullYear() || (year==dMin.getFullYear()&&month<(dMin.getMonth()+1)) || year>dMax.getFullYear() || (year==dMax.getFullYear()&&month>(dMax.getMonth()+1)) ) {
		for (;i<iMax;i++) {
			bdc[i].src="baddate.gif";
			bdc[i].className="bdt";
		}
	} else if(month==(dMin.getMonth()+1)) {
		iBox=dMin.getDate()+GetDOW(1,dMin.getMonth()+1,dMin.getFullYear())-1;
		for (;i<iMax;i++) {
			if(i<iBox) {
				s="baddate.gif";n="bdt";
			} else {
				s="date.gif";n="dt";
			}
			bdc[i].src=s;bdc[i].className=n;
		}
	} else if(month==(dMax.getMonth()+1)) {
		iBox=dMax.getDate()+GetDOW(1,dMax.getMonth()+1,dMax.getFullYear())-1;
		for (;i<iMax;i++) {
			if (i<iBox+1) {
				s="date.gif";
				n="dt";
			} else {
				s="baddate.gif";
				n="bdt";
			}
			bdc[i].src=s;bdc[i].className=n;
		}
	} else {
		for (;i<iMax;i++) {
			bdc[i].src="date.gif";
			bdc[i].className="dt";
		}
	}

	for(;i<42;i++) {
		bdc[i].src="nodate.gif";
		bdc[i].className="ndt";
	}

	g_dC=day;
	g_mC=month;
	g_yC=year;
}

function ValidDate(day,month,year) {
	if (year<dMin.getFullYear() || (year==dMin.getFullYear()&&month<(dMin.getMonth()+1)) || (year==dMin.getFullYear()&&month==(dMin.getMonth()+1)&&day<dMin.getDate()) ) 
		return false;
	else if (year>dMax.getFullYear() || (year==dMax.getFullYear()&&month>(dMax.getMonth()+1)) || (year==dMax.getFullYear()&&month==(dMax.getMonth()+1)&&day>dMax.getDate()) ) 
		return false;
	else 
		return true;
}

function GetMonthCount(month,year) {
	var c=rgMC[month-1];
	if ((2==month)&&IsLeapYear(year)) 
		c++;
	return c;
}

function IsLeapYear(year) {
	return (0==year%4 && ((year%100!=0)||(year%400==0)) );
}

function GetDOW(day,month,year) {
	var dt=new Date(year,month-1,day);
	return (dt.getDay()+(7-wp.GetDowStart()))%7;
}

var rgMN=new Array(12);
rgMN[0]="JAN";
rgMN[1]="FEB";
rgMN[2]="MAR";
rgMN[3]="APR";
rgMN[4]="MAY";
rgMN[5]="JUN";
rgMN[6]="JUL";
rgMN[7]="AUG";
rgMN[8]="SEP";
rgMN[9]="OCT";
rgMN[10]="NOV";
rgMN[11]="DEC";

var rgMC=new Array(12);
rgMC[0]=31;
rgMC[1]=28;
rgMC[2]=31;
rgMC[3]=30;
rgMC[4]=31;
rgMC[5]=30;
rgMC[6]=31;
rgMC[7]=31;
rgMC[8]=30;
rgMC[9]=31;
rgMC[10]=30;
rgMC[11]=31;

g_fCalLoaded=true;

//-->
</script>

</body>
</html>
