<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUserSelectCampaign.aspx.cs" Inherits="LarpPortal.NewUserSelectCampaign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select A Campaign</title>
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
    <form id="frmMainForm" runat="server">
        <div role="form" class="form-horizontal">
            <div class="col-sm-12 NoPadding">
                <h1 class="col-sm-12 text-center">Welcome to LARP Portal</h1>
            </div>
            <div class="row" style="padding-top: 20px;">
                <div class="col-lg-4 col-lg-push-4 col-md-6 col-md-2 col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Select Starting Campaign</div>
                        <div class="panel-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <asp:Label ID="lblPageText" runat="server" Text=""></asp:Label>
                                    <asp:DropDownList ID="ddlCampaign" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCampaign_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="row">
                                    <br />
                                    <br />
                                    Select how you're going to participate in the campaign.<br />
                                    If you're going to staff the game, sign up as an NPC and notify the GM/owner.
                                </div>
                                <div class="row">
                                    <asp:CheckBoxList ID="cblRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblRole_SelectedIndexChanged">
                                        <asp:ListItem Text="PC" Value="PC"></asp:ListItem>
                                        <asp:ListItem Text="NPC" Value="NPC"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row"></div>
                <div class="row PrePostPadding">
                    <div class="col-sm-11">
                        <asp:HiddenField ID="hidRole" runat="server" />
                        <asp:HiddenField ID="hidCampaign" runat="server" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Visible="false" Text="Sign up" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
