<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LarpPortal.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">



<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />
    <title>Register LARP Portal</title>

    <%--Put the style sheets here. Any styles that are specific to the page will be in the section MainStyles.--%>
    <!-- Bootstrap Core CSS -->
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <!-- MetisMenu CSS -->
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="/css/larportal.css" rel="stylesheet" />
    <link href="/css/nav.css" rel="stylesheet" />

    <!-- Custom Fonts -->
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />


    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="/Scripts/jquery-3.1.1.min.js"></script>

    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="/Scripts/bootstrap.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="/Scripts/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="/Scripts/sb-admin-2.js"></script>

    <script src="/Scripts/jquery.matchHeight.js"></script>


    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
		<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
	<![endif]-->

    <%--Put the scripts here. Any scripts that are specific to the page will be in the section MainScripts.--%>

    <script>
        $(document).ready(function () {
            $('.equal-height-panels .box').matchHeight();
        });
    </script>
</head>

<body>
    <form id="form" runat="server">
        <asp:ScriptManager ID="smManager" runat="server" />
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Register For LARP Portal</h3>
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <div class="form-group">
                                <label>Username</label>
                                <asp:TextBox ID="txtNewUserName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RFVNewUsername" runat="server" ErrorMessage="Username is required"
                                    ControlToValidate="txtNewUsername" ValidationGroup="SignUpGroup" Display="Dynamic" ForeColor="Red" />
                            </div>
                            <div class="form-group">
                                <label>First Name</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RFVFirstName" runat="server" ErrorMessage="First name is required"
                                    ControlToValidate="txtFirstName" ValidationGroup="SignUpGroup" Display="Dynamic" ForeColor="Red" />
                            </div>
                            <div class="form-group">
                                <label>Last Name</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RFVLastName" runat="server" ErrorMessage="Last name is required"
                                    ControlToValidate="txtLastName" ValidationGroup="SignUpGroup" Display="Dynamic" ForeColor="Red" />
                            </div>
                            <div class="form-group">
                                <label>EMail Address</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                                <asp:RegularExpressionValidator ID="REVEmailAddress" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtEmail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group">
                                <label>Password</label>
                                <asp:Label ID="lblPasswordReqs" runat="server" ToolTip=""></asp:Label>
                                <asp:TextBox ID="txtPasswordNew" TextMode="Password" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtPasswordNew_TextChanged" />
                            </div>
                            <div class="form-group">
                                <label>Confirm Password</label>
                                <asp:TextBox ID="txtPasswordNewRetype" TextMode="Password" runat="server" CssClass="form-control" OnTextChanged="txtPasswordNewRetype_TextChanged" />
                            </div>
                            <div class="form-group">
                                <label class="checkbox">
                                    <asp:CheckBox ID="chkTermsOfUse" CssClass="checkbox" runat="server" OnCheckedChanged="chkTermsOfUse_CheckedChanged" AutoPostBack="true" />
                                    I agree to the
                                    <asp:HyperLink ID="TermsOfUse" runat="server" NavigateUrl="~/TermsOfUse.aspx" Target="_blank">Terms Of Use</asp:HyperLink>
                                </label>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnSignUp" CssClass="btn btn-primary" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" />
                            </div>
                            <asp:Label ID="lblEmailFailed" runat="server" ForeColor="Red"></asp:Label>
                        </fieldset>
                        <hr />
                        <ul class="list-inline text-center">
                            <li><a href="http://www.darrinscottstudios.com/LARPortal/login.html#" class="btn btn-primary">Enter as a Guest</a></li>
                            <li><a href="http://www.darrinscottstudios.com/LARPortal/login.html#" class="btn btn-primary">Register</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <%--        <h2 class="form-signin-heading">Sign Up for LARP Portal</h2>
        <div class="form-signin" role="form">
            <div>
                <asp:Label ID="lblSignUpErrors" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>--%>
    </form>
</body>
</html>
