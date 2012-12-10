<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"
%>
<html>
	<head>
		<title>Adult Media Swapper - Known Issues</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			Known Issues
		</h3>
		<p>
			The following are the currently known issues with the AMS program:
			<ul>
				<li>
					<b>During each network event (login, query, file download, etc.), a single Windows 
						Handle is leaked, along with a small amount of memory.</b>
					<br>
					Solution: There is currently no workaround. The leak is not significant during 
					normal program use, however those running dedicated servers may need to 
					occasionally restart the program. In testing, a Windows 2000 Professional 
					computer was able to leak 150,000+ handles without any negetive affect aside 
					from increased memory load.</li>
			</ul>
		</p>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
