<%@ Control Language="c#" Classname="pics.Controls.AutoTimer"  CompileWith="AutoTimer.ascx.cs" %>
<div id="progressBox" class="progressBox">
<div id="progressBar" class="progressBar"><img src="Images/blank.gif" height="3" width="1"></div>
</div>
<div id="progressText"></div>
<script language="javascript"><!--

/*
function AddHandler(element,strEvent,fnName) {
 ref='a'+new Date().valueOf()
 while (typeof element[ref]!='undefined')
     ref='a'+Number(ref.substr(1,100))+1
 element[ref]=element[strEvent]
element[strEvent]=new Function("this['"+ref+"']();"+fnName+"()")
}
*/
// need to find out how to check if an image is loaded

	var currentTime = 0;
	var active	= false;	
	var intervalId = 0;
	var navigateUrl = '<%=NavigateUrl%>';
	var seconds		= <%=Seconds%>;
	
	function WaitForLoad()	{
		
//		document.all.progressBar.style		= 'DISPLAY: none';
//		document.all.progressText.style		= 'DISPLAY: visible';

		if (!active)
		{
			active = true;
			currentTime = currentTime+ 1;
			
			progressBar.style.width			= progressBox.clientWidth * ((currentTime * 1.0) / (1.0 * seconds)) ;

			// See if we should go to next URL
			if (currentTime >= seconds)	{
				clearInterval(intervalId);
				
				document.location.href		= navigateUrl;
			}
				
			active = false;
				
		}

	}	
	
	intervalId = setInterval('WaitForLoad()', 1000);
	
//	AddHandler(window, 'OnLoad', 'WaitForLoad');
	
// --></script>
