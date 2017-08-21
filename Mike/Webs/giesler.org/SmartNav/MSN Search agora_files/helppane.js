var h_win
var iTopicTimer
var Topic_URL
var help_w, help_h, help_l, help_t, fudgeW
var bResize = true
var NoMax = 0
H_VER='1.5'
//H_CONFIG='searchv3.ini';
function GetClientURL()
{
	return escape(document.location.protocol + "//" + document.location.hostname)
	
}
function GetClientParams()
{
	return escape(document.location.search)
}

function setCurrentDomain(){
	selfURL = document.domain //selfURL.substring(i1,i2)
	var URLsegments = selfURL.split(".")
	if (URLsegments.length > 1)
	{
		stEndSeg = URLsegments[URLsegments.length-1]
		if  (stEndSeg== "com" || stEndSeg == "net")
		{ document.domain = (URLsegments[URLsegments.length-2] + "." + URLsegments[URLsegments.length-1])}
	}
}


function reMax()
{
	if (bScreen)
	{
		SniffSizing()
		if(agent_isNS && (screen.availWidth != window.innerWidth) && agent_Major < 5)
		{
			top.window.resizeTo(screen.availWidth -(window.outerWidth - window.innerWidth), screen.availHeight - (window.outerHeight-window.innerHeight))
			top.window.moveTo(0,0)		
		}
		else
		{
			if(agent_isAOL==false || agent_isMac==true)
			{
				top.window.resizeTo(screen.availWidth, screen.availHeight - startbarH - aoltoolbar)
				top.window.moveTo(0,(startbarH * mac_heightoffset)+aoltoolbar-1)
			}
		}
	}
}
function SniffSizing()
{
			fudgeW = 0
			if (agent_isMac==true)
			{
				if (agent_Major == 4)
				{
					fudgeW = 7
					mac_heightoffset = 1.7
					startbarH = (screen.height-screen.availHeight)
					startbarW = (screen.width-screen.availWidth)
				}
				else
				{
					fudgeW = 2
					mac_heightoffset=0
					startbarH = 0//(screen.height-screen.availHeight)
					startbarW = (screen.width - screen.availWidth)
				}
				if(agent_isAOL==true)
				{
					WIDTH = 248
					aoltoolbar = 70
				}
				else
				{
					if(agent_Major==4)
					{
						WIDTH = 230
					}
					else
					{
						WIDTH = 230
					}
					aoltoolbar=0
				}
			}
			else
			{
				mac_heightoffset = 0
				startbarH = 0
				startbarW = 0
				if(screen.width <= 800)
				{
					WIDTH = 180
				}
				else
				{
					WIDTH = 230
				}
				if(agent_isAOL==true)
				{
					aoltoolbar = 150
				}
				else
				{
					aoltoolbar=0
				}
			}
}

function SizeforHelp()
{
		if(agent_isNS && (screen.availWidth - WIDTH != window.innerWidth) && agent_Major < 5)
		{
			top.window.resizeTo(screen.availWidth - WIDTH - (window.outerWidth - window.innerWidth), screen.availHeight - (window.outerHeight-window.innerHeight))
			top.window.moveTo(0,(startbarH * mac_heightoffset)+aoltoolbar)		
		}
		else
		{
			help_h=help_h - startbarH - aoltoolbar
			if(agent_isAOL==false || agent_isMac==true)
			{
				top.window.resizeTo(screen.availWidth - WIDTH - (startbarW*fudgeW), screen.availHeight - startbarH-aoltoolbar)
				top.window.moveTo(0,(startbarH * mac_heightoffset)+aoltoolbar)
			}
		}

}
function DoHelp(bLocTestWindows)
{
	//document.domain = getCurrentDomain()
	var domaincheck = document.domain
	//bSearch is set in calling page
	Topic_URL="INI="+H_CONFIG+"&H_APP="+escape(L_H_APP)
	if (bSearch==true)
	{
		//do context sensative search
		Topic_URL = "?"+Topic_URL+"&SearchTerm="+escape(H_KEY)+"&S_Text="+escape(L_H_TEXT)
	}
	else
	{	
		//jump directly to topic
		Topic_URL = "?Topic="+H_TOPIC+"&"+Topic_URL
	}

	
	//name the window, so that topic from Help window can change URL of main app window.
	window.name="msnMain";
	
	if(agent_isAOL==false || agent_isMac==true)
	{
		//for Non AOL-WIN browsers
		myURL = H_URL_BASE+"/helpwindow.asp"+Topic_URL+"&BrandID="+H_BRAND+"&Filter="+H_FILTER+"&H_VER="+H_VER
	}
	else
	{
		//for AOL-WIN versions. AOL-WIN have problems displaying after redirection & don't resize well
		myURL = H_URL_BASE+"/frameset.asp"+Topic_URL+"&BrandID="+H_BRAND+"&Filter="+H_FILTER+"&H_VER="+H_VER
	}
myURL = myURL + "&NoMax="+NoMax + "&v1="+GetClientURL() + "&v2=" + GetClientParams()

var agent_MSNMajor = ''
var agent_MSNMinor = ''
var agent_isWebTV = (agent.indexOf('webtv')>0)
var agent_isMSNCompanion = (agent.indexOf('msn companion') >= 0)
	if (agent_isMSNCompanion == false)
	{
		if (agent.indexOf('msn ')>0) {
			version = version.substr(version.indexOf("msn ")) //get past msn and the space
			agent_MSNMajor = version.substring(version.indexOf(".")-1,version.indexOf("."))
			if (agent_MSNMajor >= 6)
			{
				agent_isMSN = true
				if (version.indexOf(0,";")>0)
					agent_MSNMinor = version.substring(version.indexOf(".")+1, version.indexOf(";"))
				else
					agent_MSNMinor = version.substring(version.indexOf(".")+1, version.indexOf(")"))
			}
			else
			{
				agent_isMSN = false
			}
		}
	}	
	if (agent_isMSN && agent_MSNMinor != '0b1')
	{
		
		if(screen.width <= 800)
			{
				WIDTH = 180
			}
			else
			{
				WIDTH = 230
			}
		window.external.showHelpPane(myURL, WIDTH)
		//h_win=window.open(myURL,"_help")

	}	
	else if (agent_isWebTV || agent_isMSNCompanion)
	{
		//webtv doesn't handle window.open very well, so just change the current windows href
		top.location.href=myURL
	}
	else
	{
		if (bLocTestWindows == null)
			windowName = '_help'
		else
			windowName = '_help'+Math.floor(Math.random()*100)

		//set defaults for Help window size for 3.x browsers
		help_w=230;help_h=450;help_l=640-help_w;help_t=0;
		//bScreen is set by calling page - bScreen means can do screen.availWidth stuff
		if(bScreen == true)
		{
		SniffSizing()
		if (NoMax == 0) SizeforHelp()
		help_w = WIDTH;
		help_h = screen.availHeight;
		help_l = screen.availWidth - WIDTH;

		w_dressing = "toolbar=0,status=0,menubar=0,width="+help_w+",height="+help_h+",left="+help_l+",top="+help_t+",resizable=1"

		}
		else
		{
			//3.x browsers
			w_dressing = "toolbar=0,status=0,menubar=0,width="+help_w+",height="+help_h+",,,resizable=1"
		}

		if(bScreen == true && agent_isNS == false && h_win != null && agent_isMac == false && agent_Major ==4)
		{
			if (H_URL_BASE.indexOf(domaincheck) > -1 && !(h_win.closed)) {
				h_win.name=''
				h_win.close()
			}
			else
			{
				h_win = null
			}
		}
		bResize = false
		h_win=window.open(myURL,windowName, w_dressing)
		if(agent_isNS && agent_isMac)
			h_win=window.open(myURL,windowName, w_dressing);
	}
}
//set the document.domain to a subdomain everytime .js file is loaded.
var ap = navigator.appName.toLowerCase()
var version = navigator.appVersion.toLowerCase()
var agent = navigator.userAgent.toLowerCase()
var agent_isMSN = (agent.indexOf('msn 6') >= 0)
var isNS = ap.indexOf('netscape') >= 0
var isver4 = version.substr(0,1) == '4'

