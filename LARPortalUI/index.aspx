<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LarpPortal.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>LARP Portal - Log In</title>

    <!-- Bootstrap Core CSS -->
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <!-- MetisMenu CSS -->
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="/css/sb-admin-2.css" rel="stylesheet" />

    <%-- By adding the ticks, it causes a number to be added which will force the style sheet to be reloaded. --%>
    <link href="/css/larportal.css?t=<%= DateTime.Now.Ticks %>" rel="stylesheet" />

    <!-- Custom Fonts -->
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <!-- jQuery -->
    <script src="/Scripts/jquery-3.3.1.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="/Scripts/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="/Scripts/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="/Scripts/sb-admin-2.js"></script>

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

    <style type="text/css">
        body {
            background: #fff;
        }

        #welcome h1 {
            color: #fff;
            font-size: 3.5em;
            text-align: center;
            letter-spacing: .03em;
        }

        #welcome p {
            color: #fff;
            font-size: 1.25em;
            text-align: center;
            letter-spacing: .03em;
        }

        /*.container {
            height: 991px;
            background: url(/images/PortalImageFade.jpg);
            background-size: cover;
        }*/

        .login-panel {
            margin-top: 12%;
        }

        .panel-default {
            border-color: #ddd;
        }

        .panel {
            margin-bottom: 20px;
            background-color: rgba(255,255,255,.75);
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
        }

        hr {
            margin-top: 0;
        }

        ul a {
            font-size: 16px;
        }

        .post-content {
            background: none repeat scroll 0 0 #FFFFFF;
            opacity: 0.5;
            top: 0;
            left: 0;
            position: absolute;
        }
    </style>

    <script type="text/javascript">
        function openActivationCode() {
            $('#modalActivationCode').modal('show');
        }

        function CheckActivationCode() {
            var hidActivateCode = document.getElementById('<%= hidActivateCode.ClientID %>');
            var tbActivationCode = document.getElementById("<%= tbActivationCode.ClientID %>");
            var divError = document.getElementById("divError");

            if ((hidActivateCode != null) &&
                (tbActivationCode != null)) {
                if (hidActivateCode.value == tbActivationCode.value) {
                    return true;
                }
                else {
                    tbActivationCode.value = "";
                    divError.style.display = 'block';
                    return false;
                }
            }
            tbActivationCode.value = "";
            divError.style.display = 'block';
            return false;
        }
    </script>



    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
	<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
	<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
<![endif]-->

</head>

<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <asp:MultiView ID="mvMainScreen" runat="server">
                <asp:View ID="vwLogin" runat="server">
                    <%--Normal login screen.--%> 
                    <div class="row" id="divMainScreen" runat="server">
                        <div class="col-lg-4 col-lg-push-8 col-md-6">
                            <div class="text-center">
                                <h1>LARP Portal</h1>
                                The Gateway to Managing Your LARPs
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Member Log In</h3>
                                </div>
                                <div class="panel-body">
                                    <fieldset>
                                        <div class="form-group">
                                            <label>Username</label>
                                            <asp:TextBox runat="server" ID="tbUserName" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label>Password</label>
                                            <asp:TextBox runat="server" ID="tbPassword" CssClass="form-control" TextMode="Password" />
                                        </div>
                                        <p>
                                            <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-block btn-success" OnClick="btnLogin_Click" />
                                        </p>
                                        <asp:Label ID="lblInvalidLogin" runat="server" ForeColor="Red" Visible="false"><p class="text-center lead">Invalid username and password</p></asp:Label>
                                        <p class="text-center"><a href="ForgotPassword.aspx">Forgot User ID/Password?</a></p>
                                    </fieldset>
                                    <hr />
                                    <ul class="list-inline text-center">
                                        <li>
                                            <asp:Button ID="btnGuest" runat="server" Text="Enter as a Guest" CssClass="btn btn-primary btn-sm" OnClick="btnGuest_Click" /></li>
                                        <li>
                                            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary btn-sm" OnClick="btnRegister_Click" /></li>
                                        <li>
                                            <asp:Button ID="btnContactUs" runat="server" Text="Contact Us" CssClass="btn btn-primary btn-sm" OnClick="btnContactUs_Click" /></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="text-center">
                                <h3><a href="/LearnMoreAboutLARPortal.aspx">What is LARP Portal ?</a></h3>
                            </div>
                        </div>
                        <div class="col-lg-8 col-lg-pull-4 col-md-6">
                            <img src="images/PortalImageFade.jpg" alt="LARPortal Welcome Image" class="img-responsive" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwActivate" runat="server">
                    <%--If need to ask for activation code, hide the member login and show just the logo.--%>
                    <div class="row" id="divLogoOnly" runat="server">
                        <div class="col-lg-8 col-lg-offset-2">
                            <img src="images/PortalImageFade.jpg" alt="LARPortal Welcome Image" class="img-responsive" />
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
            <div class="margin40">&nbsp;</div>
        </div>

        <div class="modal fade" id="modalActivationCode" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Account Activation Code</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            An activation code was included in you're welcome email that was sent to you when you registered for a LARP Portal account.
                        </p>
                        <span id="divError" style="display: none;">
                            <span class="text-danger">The code you entered was incorrect. Please try again.</span>
                            <br />
                        </span>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbActivationCode.ClientID %>">Activation Code</label>
                                    <asp:TextBox runat="server" ID="tbActivationCode" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-6 text-left">
                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                            </div>
                            <div class="col-xs-6 text-right">
                                <asp:Button ID="btnActivate" runat="server" Text="Activate" CssClass="btn btn-primary" OnClientClick="return CheckActivationCode();" OnClick="btnValidateAccount_Click" />
                            </div>
                            <asp:HiddenField ID="hidActivateCode" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
</script>
</body>
</html>
