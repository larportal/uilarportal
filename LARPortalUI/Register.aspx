<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LarpPortal.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Google Tag Manager -->
    <script>(function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start':
                    new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
                    'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', 'GTM-5MQHDGS');</script>
    <!-- End Google Tag Manager -->
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

    <!-- MetisMenu CSS -->
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="/css/larportal.css" rel="stylesheet" />
    <link href="/css/nav.css" rel="stylesheet" />

    <!-- Custom Fonts -->
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />


    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="/Scripts/jquery-3.3.1.min.js"></script>

    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="/Scripts/bootstrap.js"></script>

    <%--Put the style sheets here. Any styles that are specific to the page will be in the section MainStyles.--%>
    <!-- Bootstrap Core CSS -->
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <!-- Metis Menu Plugin JavaScript -->
    <script src="/Scripts/metisMenu.min.js"></script>
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <!-- Custom Theme JavaScript -->
    <script src="/Scripts/sb-admin-2.js"></script>

    <script src="/Scripts/jquery.matchHeight.js"></script>

    <script>
        function EnableSignUp(cboxTOS) {
            var btnSignUp = document.getElementById("<%= btnSignUp.ClientID %>");

            if (cboxTOS.checked == true) {
                btnSignUp.disabled = false;
            }
            else {
                btnSignUp.disabled = true;
            }
        }
    </script>

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
    <!-- Google Tag Manager (noscript) -->
    <noscript><iframe src=https://www.googletagmanager.com/ns.html?id=GTM-5MQHDGS
    height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <!-- End Google Tag Manager (noscript) -->

    <form id="form" runat="server">
        <asp:ScriptManager ID="smManager" runat="server" />
        <div id="wrapper">
            <div class="row" style="padding-top: 10px;">
                <div class="col-md-4 col-md-offset-4 col-lg-4 col-lg-offset-4 col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Register For LARP Portal</h3>
                        </div>
                        <div class="panel-body">
                            <fieldset>
                                <div class="form-group">
                                    <label>Username</label>
                                    <asp:TextBox ID="txtNewUsername" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RFVNewUsername" runat="server" ErrorMessage="Username is required"
                                        ControlToValidate="txtNewUsername" Display="Dynamic" ForeColor="Red" />
                                </div>
                                <div class="form-group">
                                    <label>First Name</label>
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RFVFirstName" runat="server" ErrorMessage="First name is required"
                                        ControlToValidate="txtFirstName" Display="Dynamic" ForeColor="Red" />
                                </div>
                                <div class="form-group">
                                    <label>Last Name</label>
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RFVLastName" runat="server" ErrorMessage="Last name is required"
                                        ControlToValidate="txtLastName" Display="Dynamic" ForeColor="Red" />
                                </div>
                                <div class="form-group">
                                    <label>EMail Address</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                                    <asp:RegularExpressionValidator ID="REVEmailAddress" runat="server" SetFocusOnError="true" Display="Dynamic" 
                                        ControlToValidate="txtEmail" ErrorMessage="Must be a valid email address"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Must be a valid email address"
                                        ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red" />
                                </div>
                                <div class="form-group">
                                    <label>Password</label>
                                    <asp:Label ID="lblPasswordReqs" runat="server"><span class="glyphicon glyphicon-question-sign"></span></asp:Label>
                                    <asp:TextBox ID="txtPasswordNew" TextMode="Password" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Must enter a password"
                                        ControlToValidate="txtPasswordNew" Display="Dynamic" ForeColor="Red" />
                                </div>
                                <div class="form-group">
                                    <label>Confirm Password</label>
                                    <asp:TextBox ID="txtPasswordNewRetype" TextMode="Password" runat="server" CssClass="form-control" />
                                    <asp:CompareValidator runat="server" ID="cmpPasswords" ControlToValidate="txtPasswordNew" ControlToCompare="txtPasswordNewRetype" 
                                        Operator="Equal" Type="String" ErrorMessage="The passwords must be equal." Display="Dynamic" ForeColor="Red" />
                                </div>
                                <div class="form-group">
                                    <input type="checkbox" name="chkTermsOfUse" id="chkTermsOfUse" runat="server" onchange="EnableSignUp(this);" />
                                    <div class="btn-group">
                                        <label for="<%= chkTermsOfUse.ClientID%>" class="btn btn-default">
                                            <span class="glyphicon glyphicon-ok"></span>
                                            <span class="glyphicon glyphicon-unchecked"></span>
                                        </label>
                                        <label for="<%= chkTermsOfUse.ClientID%>" class="btn btn-default active">
                                            I agree to the <a href="#" data-toggle="modal" data-target="#terms">Terms of Use</a>
                                        </label>
                                    </div>
                                </div>
                            </fieldset>
                            <hr />
                            <asp:Label ID="lblSignUpErrors" runat="server" ForeColor="Red"></asp:Label>
                            <div class="row">
                                <div class="col-lg-6">
                                    <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                                </div>
                                <div class="col-lg-6 text-right">
                                    <asp:Button ID="btnSignUp" CssClass="btn btn-primary" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" />
                                </div>
                            </div>
                        </div>
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


        <div class="modal fade" id="terms" role="dialog">
            <div class="modal-dialog modal-lg">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3>Terms Of Use</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblTestOfUseMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12 no-no-gutters text-right">
                                <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
























    </form>
</body>
</html>
