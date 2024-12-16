<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUserDirections.aspx.cs" Inherits="LarpPortal.NewUserDirections" %>

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
    <title>Register LARP Portal - New User Directions</title>

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

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
		<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
	<![endif]-->

    <%--Put the scripts here. Any scripts that are specific to the page will be in the section MainScripts.--%>
</head>
<body>
    <!-- Google Tag Manager (noscript) -->
    <noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-5MQHDGS"
    height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <!-- End Google Tag Manager (noscript) -->

    <form id="form" runat="server">
        <asp:ScriptManager ID="smManager" runat="server" />
        <div id="wrapper">
            <div class="row" style="padding-top: 10px;">
                <div class="col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">LARP Portal New Account Directions</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12 text-center">
                                    Welcome to LARP Portal and thank you for signing up for a member account.  You will shortly be receiving an email with your account activation 
                                    key.  Return to the LARP Portal login screen and enter the username and password you selected.  You'll be prompted to enter the account activation
                                    key from the email.  Enter the key and click the login button to enter LARP Portal.
                                    <br /><br />
                                    If you don’t receive your email within a few minutes, <strong>DO NOT</strong> create a new account.
                                    <ul>
                                        <li>
                                            Check your spam filter.
                                        </li>
                                        <li>
                                            If you still don’t have it, send an email to <a href='mailto:support@larportal.com'>support@larportal.com</a>, and we will send you the email from a different account.
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 text-center">
                                    <a href="/index.aspx">Click here to return to the login page.</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
