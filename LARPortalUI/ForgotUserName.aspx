<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotUserName.aspx.cs" Inherits="LarpPortal.ForgotUserName" %>

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

    <link href="/css/larportal.css" rel="stylesheet" />
    <style type="text/css">
        body {
            background: #fff;
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
        <div id="wrapper">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-lg-6 col-lg-offset-3">
                        <div class="header-background-image">
                            <h1>LARP Portal Forgot Username</h1>
                        </div>
                    </div>
                </div>
                <div class="divide10"></div>
                <asp:MultiView ID="mvInfoRequest" runat="server">
                    <asp:View ID="vwInfoRequest" runat="server">
                        <div class="row">
                            <div class="col-lg-6 col-lg-offset-3 col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Your Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label for="<%= txtEmailAddress.ClientID %>">EMail Address:</label>
                                                    <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" Font-Bold="true" Font-Italic="true" 
                                                        ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtEmailAddress" />
                                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-lg-12 text-center" id="divInvalid" runat="server">
                                                <h3 class="text-center text-danger">This email address is not associated with a LARP Portal account.<br />Please click 'Sign Up' to create an account.
                                                </h3>
                                                <asp:Button ID="btnSignUp" runat="server" CssClass="btn btn-primary" Text="Sign Up" OnClick="btnSignUp_Click" CausesValidation="true" />
                                                <br />
                                            </div>
                                            <div class="col-lg-12 text-right">
                                                <asp:Button ID="btnGetUsername" runat="server" CssClass="btn btn-primary" Text="Get Username" OnClick="btnGetUsername_Click" CausesValidation="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwSentEmail" runat="server">
                        <div class="row">
                            <div class="col-lg-6 col-lg-offset-3 col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Email Sent
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-12 text-center">
                                                <asp:Label ID="lblMessage" runat="server" />
                                                <br />
                                                Click <u><asp:HyperLink ID="hlLoginPage" runat="server" NavigateUrl="~/index.aspx" Text="here" /></u> to log in.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwIssue" runat="server">
                        <div class="row">
                            <div class="col-lg-6 col-lg-offset-3 col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        There was an issue.
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-12 text-center">
                                                There was an issue retrieving your information. Please contact us at support@larportal.com for assistance.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </form>
</body>
</html>
