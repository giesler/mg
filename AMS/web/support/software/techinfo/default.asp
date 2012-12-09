<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../../"
%>
<html>
<head>
<title>Adult Media Swapper - Technical Information</title>
<link rel="stylesheet" type="text/css" href="../../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../../_header.asp"-->

<h3>Technical Information</h3>
 
<p>
<i>How does AMS work?</i>
</p>

<p>
AMS is a part client-server, part peer-to-peer application that allows for detailed adult media searching.
</p>

<p>
<i>How can I block AMS from my network?</i>
</p>

<p>
AMS logs in and submits queries over TCP port 8881. The easiest way of blocking AMS in your network is to 
prevent outgoing traffic on port 8881, as well as blocking the DNS address *.adultmediaswapper.com. This will prevent any AMS client from being able to operate. 
</p>

<p><br><br></p>

<p>
AMS is made from four distinct pieces:
</p>

<ol>
	<li>AMS Client (Also referred to as media server.)</li>
	<li>Index Server</li>
	<li>Database Server</li>
	<li>Banner Server</li>
</ol>

<p>
The AMS client is what runs on your computer. Its client functions include submitting queries to the index 
server and receiving results, displaying banners, and contacting other media servers to request thumbnails and pictures. its server functions include 
uploading locally created indexes to the index server, and serving thumbnails and media to other clients that request them. 
</p>

<p>
The index server is what performs all the searches. It receives a query from a client that it then runs against 
its list of all files. When it finds matches, it returns to the client the location of the computer hosting that file. It is then the client's 
responsibility to retrieve that file.
</p>

<p>
The database server stores the master index of all files. The index servers pull an updated list from the 
database server periodically. The database server also stores user accounts and performance logs.
</p>

<p>
The banner server is in charge of serving relevant advertising to the clients. It uses some logic to decide what 
advertisement to show. If you just did a search for Asian women, chances are you will see a banner featuring an Asian website. We randomize results slightly to 
insure that all of our sponsor's banners are shown.
</p>

<!-- #include file="../../../_footer.asp"-->

</body>
</html>
