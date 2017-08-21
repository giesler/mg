<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewComputer.aspx.cs" Inherits="AddNewComputer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Wake On LAN :. Add New Computer</title>
    <link rel="Stylesheet" href="css/default.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="formWrapper">
    
    
    	    <div class="formField text">
			    <div class="formLabel">
			      <label>Computer Display Name</label> 
			    </div>
			    <div class="formInputText">
			       <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="vldDisplayName" runat="server" ControlToValidate="txtDisplayName"
                        ErrorMessage="Enter Computer Display Name" SetFocusOnError="True"></asp:RequiredFieldValidator></div>
			    <div class="formErrorMsg">						    
			    </div>
			 </div>
			 
			  <div class="formField text">
			    <div class="formLabel">
			      <label>Hostname / Address</label> 
			    </div>
			    <div class="formInputText">
			        <asp:TextBox ID="txtHostnameOrAddress" runat="server" MaxLength="16"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtHostnameOrAddress"
                        ErrorMessage="Enter Hostname or Address" SetFocusOnError="True"></asp:RequiredFieldValidator></div>
			    <div class="formErrorMsg">						    
			    </div>
			 </div>
			 
			   <div class="formField text">			   
			       <asp:Button ID="btnSubmit" runat="server" Text="Add Computer" OnClick="btnSubmit_Click" />			   
			 </div>    
        
        
     </div>
    </form>
</body>
</html>
