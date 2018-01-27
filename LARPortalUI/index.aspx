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

    <!-- Custom Fonts -->
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

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
            <div class="row">
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
                </div>
                <div class="col-lg-8 col-lg-pull-4 col-md-6">
                    <img src="images/PortalImageFade.jpg" alt="LARPortal Welcome Image" class="img-responsive" />
                </div>
            </div>
            <div class="margin40">&nbsp;</div>
        </div>
    </form>
    <!-- jQuery -->
    <script src="/Scripts/jquery-3.1.1.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="/Scripts/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="/Scripts/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="/Scripts/sb-admin-2.js"></script>
</body>
</html>
