<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../../"
%>
<html>
<head>
<title>Adult Media Swapper - Tutorial</title>
<link rel="stylesheet" type="text/css" href="../../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../../_header.asp"-->

<h3>Quick Start Tutorial</h3>
 
<p> Welcome to Adult Media Swapper. This guide is designed to teach you 
how to operate AMS quickly. It is not intended to answer all your questions 
or cover every aspect of the program. Please see the full manual 
for detailed information. </p>

<p> When you run AMS for the first time you will be presented with the 
options menu. Here you can set your download path, what
files you want to share, password protect AMS, and set your 
connection speed. </p>

<p> <img border="0" src="option_general.gif" width="481" height="443" alt="Option Window">
<br><i>Options Menu</i></p>

<p> <strong>To get started immediately, set the following options:</strong><br><br>
Under the <u>Downloads</u> tab, select your <u>Internet Connection Speed</u>.<br><br>
Under the <u>Uploads</u> tab, set your <u>Shared Files Path</u> (optional - any existing 
pictures you have that you want to share with others.)<br></p>

<p> Next, you will be presented with the logon screen. Fill in your <u>Login ID</u> 
and <u>Password.</u> (If you do not have a Login ID, click the <u>Sign Up Here!</u> 
link to create one.) If you have a lot of files to share, 
it may take a few minutes to inventory and share them.</p>

<p> <img border="0" src="login.gif" width="446" height="250" alt="Login Screen">
<br><i>Login Screen</i></p>

<p> After you login, you will see the main application window.</p>

<p> <img border="0" src="main.gif" width="680" height="481" alt="Program"><br><i>Main 
Application Window</i></p>

<p> <strong>The main application window is divided into four key areas:</strong><br><br>
On the left is a tree view of your local drive, this is used for browsing your 
existing image collections.<br><br>
The top of the application is where advertising banners are displayed.<br><br>
The bottom is a status window, complete with an upload/download thumbnail viewer.<br><br>

The main window on the right has three functions: <br>
The first function is searching. 
When searches are performed, the matching thumbnails are shown here.<br>
The second function is the Completed Downloads screen. 
This shows pictures that have been downloaded, but have not yet been saved.<br>
The last function is the Saved Files viewer. 
This is a picture browser/viewer used to display your media collection.</p>

<p> The Search tab of the main window has a toolbar. This is where all your 
searching is controlled from.</p>

<p> <img border="0" src="search_toolbar.GIF" width="430" height="86" alt="Search Toolbar">
<br><i>Search Toolbar</i></p>

<p> <strong> The left set of icons on the toolbar are to create and execute new searches. The right
set of icons are to modify and execute saved searches.</strong></p>

<p> To view images right away, simply click on the left magnifying glass. 
<img border="0" src="Magnifying_Glass.gif" width="33" height="33" alt="Magnifying Glass Icon"> 
Since no search options have been set, this will return random images.</p>

<p> To do a detailed search, click on the left checkbox icon. 
<img border="0" src="checkbox.gif" width="33" height="31" alt="Checkbox Icon"> 
This will bring up the <U>Modify Query</U> window.</p>

<p> <img border="0" src="Modify_query.gif" width="384" height="340" alt="Modify Query Window">
<br><i>Modify Query Window</i></p>

<p> The <u>Modify Query</u> window has two main buttons: a <U>Modify Search</U> button and a 
<U>Modify Filter</U> button.<br><br>

The <U>Modify Search</U> button allows you to choose exactly what you want to search for. 
Note: choosing two options from two different fields will "and" the results together. 
Choosing two options from the same field will "or" them together.<br><br>

Example: Choosing <i>Setting = Outdoors</i> and <i>Gender = Female</i> will return images with 
people that are <b>both</b> outdoors and female. Choosing <i>Setting = Outdoors</i> and 
<i>Setting = Indoors</i> will return images with people being <b>either</b> outdoors or indoors.</p>

<p> <img border="0" src="Modify_search.gif" width="449" height="447" alt="Modify Search Window">
<br><i>Modify Search Window</i></p>

<p> The <u>Modify Filter</u> button is just the opposite. Any selections made here will 
result in pictures that match <b>not</b> being shown.<br><br>

Example: Choosing <i>Setting = Outdoors</i> from the <U>Modify Search</U> screen and 
<i>Gender = Female</i> from the <u>Modify Filter</u> screen means you want to see outdoor 
pictures of people who are <b>not</b> female. Using the filter allows you to create powerful 
queries with more control then just using the search screen.</p>

<p> When you are finished customizing your search, click "OK" until you are back at the main 
window. Now click on the left magnifying glass <img border="0" src="Magnifying_Glass.gif" 
width="33" height="33" alt="Magnifying Glass Icon"> to run your custom search. If no results 
are shown, it could be for one of three reasons:<br><br>

<b>There are matching images, but the server hasn't run across them yet.</b> The server will 
search through several hundred random pictures to try and find your matches. The server does 
not search the entire database each time. This prevents one narrowly defined search from 
taking up all of the server's resources. If you click the magnifying glass a few more times, 
the server may run across some matching results.<br><br>

<b>There are matching images, but the person sharing them isn't online.</b> The server keeps 
a list of everyone who is sharing a particular image. If that image matches your search, but 
no one is sharing it currently, the server won't return that result. If you try your search 
again at a different time you may get additional results.<br><br>

<b>There are no matching images.</b> AMS is a new program and all possible search combinations 
haven't been indexed yet. If you wait a few days or weeks and run your search again you may 
get additional results.</p>

<p> Assuming some images were returned, lets learn how to download, view, and save them.<br><br>

The images that matched your search show up as thumbnails in the <u>Search</u> window. If you 
see images that you like, simply double click them to add them to the download queue at bottom. 
<i>Note: the cheesy triangle graphic is used as a simulation for this tutorial. The actual 
picture's thumbnail is normally shown.</i></p>

<p> <img border="0" src="downloadqueue.gif" width="649" height="144" alt="Download/Upload Queue">
<br><i>Download/Upload Queue</i></p>

<p>AMS will automatically download the pictures you add to the queue in the background. When 
the thumbnail disappears from the download queue, the image has been downloaded to your 
computer. However, the image still hasn't been saved yet. Instead, AMS puts the image in 
temporary memory until you view it. Click on the <u>Completed Downloads</u> tab to see these 
images.</p>

<p> <img border="0" src="completed_downloads.gif" width="430" height="96" alt="Completed Downloads Tab">
<br><i>Completed Downloads Tab</i></p>

<p> The <u>Completed Downloads</u> tab will show all of the images that have been downloaded, 
but not yet saved to your computer. The thumbnails are shown on the left and the full sized 
image for a selected thumbnail is shown on the right. As you view each image, you have two 
buttons to choose from: <U>Save</U> and <U>Delete</U>.</p>

<p> <IMG SRC="Main_CompletedDownloads_Zoom.GIF" border="0" width="666" height="223" alt="Completed 
Download Screen"><br><i>Completed Download Screen</i></p>

<p> <U>Saved</U> images get moved to the default <u>Saved Files Path</u> you choose at the 
very beginning of this tutorial. <U>Deleted</U> images are permanently removed from your system. 
<b> Note: If you exit the program without saving the images in the Completed Downloads 
section, they will be lost.</b></p>

<p> The last thing you need to know about is the <U>Saved Files</U> tab. This is a simple 
image viewer and pretty self explanatory.</p>

<p> <img border="0" src="saved_files.gif" width="430" height="96" alt="Saved Files Tab"><br>
<i>Saved Files Tab</i></p>

<p><b>Enjoy!</b></p><br>





<!-- #include file="../../../_footer.asp"-->

</body>
</html>
