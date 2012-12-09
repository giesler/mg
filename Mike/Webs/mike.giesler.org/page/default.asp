<html>

<head>
<title>Page Mike</title>



<style fprolloverstyle>A:hover {color: #FF0000; font-family: Arial}</style>




<meta name="Microsoft Theme" content="none, default"><meta name="Microsoft Border" content="none, default"></head>

<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">
<script LANGUAGE="JavaScript">
<!-- Hide from other browsers

function checkMessageLength(form,checkLength){
	if( checkMessageLength.arguments.length == 1) checkLength = true;
	if( checkLength == false ) return true;
	var count = countChars(form);
	var rc = true;
	if( count > charLimit ){
		alert("Your message exceeds "+charLimit+" characters.  Please revise your message.");
		form.message.focus();
		rc = false;
	} 
	return rc;
}

	function countChars(form){
		if( blegal ){
			var count = form.from.value.length + 
					form.subject.value.length + 
					form.message.value.length;
			form.count.value = count;
			return count;
		} else {
			return 0;
		}
	}

   function Validate(theForm) {
    if (theForm.MSSG.value.length > 200) {
     alert("Please reduce the message by " + (theForm.MSSG.value.length - 200) + " characters.");
     theForm.MSSG.focus();
     return (false);
    }
   }

function validate(form,checkLength){
	if( validate.arguments.length == 1) checkLength = true;
	if( form.message.value.length == 0 ){
		alert("You must enter a message.");
		form.message.focus();
		return false;
	}
	if( checkMessageLength(form,checkLength) ){
		return true;
	}
	return false;
}

//  -->
</script>

<p>
<br><br>
</p>

<table border="1" cellpadding="8" cellspacing="0" align="center" bgcolor="black">
	<tr>
		<td>Sorry, but I no longer have a pager.  Please <a href="../email/">email</a> me instead.</td>
	</tr>
</table>

</body>
</html>
