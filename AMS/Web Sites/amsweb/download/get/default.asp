<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset, strFile, strOnLoad, strSite, strAltSite
strOffset = "../../"
strSite = "http://download1.adultmediaswapper.com/"
strAltSite = "http://download.adultmediaswapper.com/"
strFile = "AMSInstall_070.exe"

if Request.Form("agree") = "n" then
	Response.Redirect("http://www.google.com")
elseif Request.Form("agree") = "y" then
	strOnLoad = "onLoad=""window.location.href='" & strSite & strFile & "'"";"
end if
%>
<html>
	<head>
		<title>Adult Media Swapper - Download</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<% if Request.Form("agree") = "" then %>
	<script language="javascript">
function checkform() {
	if (!f.agree(0).checked && ! f.agree(1).checked) {
		alert('You must agree before you can download AMS.');
		return false;
	}
	return true;
}
	</script>
	<% end if %>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19 <%=strOnLoad%>">
		<!-- #include file="../../_header.asp"-->
		<% if Request.Form("agree") = "" then %>
		<h3>
			Before you download the Adult Media Swapper application, you must read and 
			agree to the following disclaimer:
		</h3>
		<p>
			<ol>
				<li>
					You are at least 18 years of age or the minimum age required to view sexually 
					explicit material for your local jurisdiction, whichever is greater.</li>
				<li>
				You are not offended by sexually explicit images, video, audio, or other media 
				of a graphic nature.
				<li>
					Viewing and storing such material does not violate any applicable law, 
					regulation, or standard in your local jurisdiction.</li>
				<li>
					You acknowledge that Adult Media Swapper has no control over the media found 
					with our search engine, and that we make no guaranty as to the accuracy or 
					content of any media found.</li>
				<li>
					You agree to use the Adult Media Swapper application in an acceptable and legal 
					manner. You agree not to share any copyrighted works or content containing 
					beastiality, child pornography, or other illegal material under United States 
					laws.</li>
			</ol>
		</p>
		<p>
			The AMS version 0.70 download is approxiately 900kb. It will only take a few 
			minutes to download.
		</p>
		<form name="f" id="f" method="post" action="default.asp">
			<blockquote>
				<table bgcolor="#f0f0f0" cellpadding="3">
					<tr>
						<td>
							<input type="radio" name="agree" value="y" id="agreey">
						</td>
						<td class="clsText">
							<label for="agreey">I agree to these terms and conditions</label>
						</td>
					</tr>
					<tr>
						<td>
							<input type="radio" name="agree" value="n" id="agreen">
						</td>
						<td class="clsText">
							<label for="agreen">I do not agree to these terms and conditions</label>
						</td>
					</tr>
					<tr>
						<td colspan="2" align="right">
							<input type="submit" value="Download" onclick="return checkform()">
						</td>
					</tr>
				</table>
			</blockquote>
		</form>
		<% else %>
		<h3>
			Download
		</h3>
		<p>
			Your download should begin shortly. If it does not start automatically, click <a href="<%=strSite & strFile%>">
				here</a> to start your download.
		</p>
		<p>
			<a href="../../support/software/tutorial/">Click here</a> for a short tutorial 
			on using the Adult Media Swapper program.
		</p>
		<p>
			<font size="-2"><i>If your download does not start and the link above does not work, 
					try <a href="<%=strAltSite & strFile%>">this location</a>.</i> </font>
		</p>
		<% end if %>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
