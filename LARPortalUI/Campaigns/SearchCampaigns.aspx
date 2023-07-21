<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="SearchCampaigns.aspx.cs" Inherits="LarpPortal.Campaigns.SearchCampaigns" %>

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
    <title>LARP Portal - The Gateway to Managing Your LARPs</title>

    <script src='https://kit.fontawesome.com/a076d05399.js'></script>

    <!-- MetisMenu CSS -->
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="/css/larportal.css" rel="stylesheet" />
    <link href="/css/nav.css" rel="stylesheet" />

    <!-- Custom Fonts -->
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <%--<script src="Scripts/jquery-3.3.1.intellisense.js"></script>--%>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="/Scripts/jquery-3.3.1.min.js"></script>

    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="/Scripts/popper.min.js"></script>
    <script src="/Scripts/bootstrap.js"></script>

    <%--Put the style sheets here. Any styles that are specific to the page will be in the section MainStyles.--%>
    <!-- Bootstrap Core CSS -->
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <!-- Metis Menu Plugin JavaScript -->
    <script src="/Scripts/metisMenu.min.js"></script>
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <style>
        .SliderSwitch {
            padding: 5px;
            margin-top: 5px;
            border: solid;
            border-width: 2px;
            border-color: lightgray;
            border-radius: 5px;
        }

        .SliderBoxBorder {
            padding-top: 3px;
            padding-bottom: 3px;
            margin-right: -10px;
        }
    </style>

    <!-- Custom Theme JavaScript -->
    <script src="/Scripts/sb-admin-2.js"></script>
    <script src="/Scripts/jquery.matchHeight.js"></script>


    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
		<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
	<![endif]-->
    <!-- For non-Retina (@1× display) iPhone, iPod Touch, and Android 2.1+ devices: -->
    <link rel="apple-touch-icon-precomposed" href="/images/apple-touch-icon-precomposed.png" />
    <!-- 57×57px -->
    <!-- For the iPad mini and the first- and second-generation iPad (@1× display) on iOS ≤ 6: -->
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="/images/apple-touch-icon-72x72-precomposed.png" />
    <!-- For the iPad mini and the first- and second-generation iPad (@1× display) on iOS ≥ 7: -->
    <link rel="apple-touch-icon-precomposed" sizes="76x76" href="/images/apple-touch-icon-76x76-precomposed.png" />
    <!-- For iPhone with @2× display running iOS ≤ 6: -->
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="/images/apple-touch-icon-114x114-precomposed.png" />
    <!-- For iPhone with @2× display running iOS ≥ 7: -->
    <link rel="apple-touch-icon-precomposed" sizes="120x120" href="/images/apple-touch-icon-120x120-precomposed.png" />
    <!-- For iPad with @2× display running iOS ≤ 6: -->
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="/images/apple-touch-icon-144x144-precomposed.png" />
    <!-- For iPad with @2× display running iOS ≥ 7: -->
    <link rel="apple-touch-icon-precomposed" sizes="152x152" href="/images/apple-touch-icon-152x152-precomposed.png" />
    <!-- For iPhone 6 Plus with @3× display: -->
    <link rel="apple-touch-icon-precomposed" sizes="180x180" href="/images/apple-touch-icon-180x180-precomposed.png" />
    <!-- For Chrome for Android: -->
    <link rel="icon" sizes="192x192" href="/images/apple-touch-icon-192x192.png" />

    <%--    <asp:ContentPlaceHolder ID="MainStyles" runat="server"></asp:ContentPlaceHolder>--%>

    <%--Put the scripts here. Any scripts that are specific to the page will be in the section MainScripts.--%>

    <script>
        $(document).ready(function () {
            $('.equal-height-panels .box').matchHeight();
        });
    </script>

    <script>
        var app = {
            loginname: '',
            characterid: '',
            campaignid: '',
            access: ''
        };


    </script>

    <%-- <asp:ContentPlaceHolder ID="MainScripts" runat="server"></asp:ContentPlaceHolder>--%>

    <%--<asp:Content ID="SearchCampaignsStyles" runat="server">
    <link href="../Content/jasny-bootstrap.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="SearchCampaignsScripts" runat="server">
    <script src="../Scripts/jasny-bootstrap.min.js"></script>
</asp:Content>--%>

    <%--<asp:Content ID="SearchCampaignsBody" ContentPlaceHolderID="MainBody" runat="server">--%>


    <!-- Bootstrap Toggle to make checkboxes into sliders.  -->
    <link href="/Content/bootstrap-toggle.min.css" rel="stylesheet" />
    <script src="/Scripts/bootstrap-toggle.min.js"></script>
</head>

<body runat="server" id="pageBody">
    <form id="form2" runat="server">



        <asp:ScriptManager ID="smManager" runat="server" />
        <div id="wrapper">
            <!-- Navigation -->
            <nav class="navbar navbar-custom navbar-static-top" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse"><span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
                    <!--<a class="navbar-brand" href="/Default.aspx"><img src="/images/Larp_logo.png" alt="LARP Portal"/></a>-->
                    <a href="#" class="navbar-left">
                        <img src="/images/LARPLogo.png" style="width: 70px; height: 70px; border: 10px solid transparent;" /></a>
                    <a class="navbar-brand" href="/Default.aspx" style="padding-top: 10px; padding-left: 5px;">LARP Portal <span class="tagline">The Gateway To Managing Your LARPs</span></a>
                </div>
            </nav>
        </div>





        <%--<div id="page-wrapper">--%>
        <%--        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaigns</h1>
                </div>
            </div>
        </div>--%>
        <div class="divide10"></div>
        <div class="row">

            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Search Campaigns
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-2 col-xs-12">
                                <div class="form-group">
                                    <label for="<%= ddlOrderBy.ClientID %>">Find By:</label>
                                    <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" CssClass="form-control">
                                        <asp:ListItem>Game System</asp:ListItem>
                                        <asp:ListItem>Campaign</asp:ListItem>
                                        <asp:ListItem>Genre</asp:ListItem>
                                        <asp:ListItem>Style</asp:ListItem>
                                        <asp:ListItem>Tech Level</asp:ListItem>
                                        <asp:ListItem>Size</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="margin10"></div>

                                <p><b>Filter By:</b> (Choose multiple options to narrow the search):</p>

                                <ul class="list-unstyled">
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkGameSystem" runat="server" AutoPostBack="true" OnCheckedChanged="chkGameSystem_CheckedChanged" />
                                                Game System:
                                            </label>
                                            <asp:DropDownList ID="ddlGameSystem" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlGameSystem_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkCampaign" runat="server" AutoPostBack="true" OnCheckedChanged="chkCampaign_CheckedChanged" />
                                                Campaign:
                                            </label>
                                            <asp:DropDownList ID="ddlCampaign" runat="server" Visible="false" OnSelectedIndexChanged="ddlCampaign_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkGenre" runat="server" AutoPostBack="true" OnCheckedChanged="chkGenre_CheckedChanged" />
                                                Genre:
                                            </label>
                                            <asp:DropDownList ID="ddlGenre" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlGenre_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkStyle" runat="server" AutoPostBack="true" OnCheckedChanged="chkStyle_CheckedChanged" />
                                                Style:
                                            </label>
                                            <asp:DropDownList ID="ddlStyle" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlStyle_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkTechLevel" runat="server" AutoPostBack="true" OnCheckedChanged="chkTechLevel_CheckedChanged" />
                                                Tech Level:
                                            </label>
                                            <asp:DropDownList ID="ddlTechLevel" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlTechLevel_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkSize" runat="server" AutoPostBack="true" OnCheckedChanged="chkSize_CheckedChanged" />
                                                Size:
                                            </label>
                                            <asp:DropDownList ID="ddlSize" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkZipCode" runat="server" AutoPostBack="true" OnCheckedChanged="chkZipCode_CheckedChanged" />
                                                Area / Zip Code:
                                            </label>
                                            <asp:TextBox ID="txtZipCode" runat="server" AutoPostBack="true" Visible="false" OnTextChanged="txtZipCode_TextChanged" />
                                            <asp:DropDownList ID="ddlMileRadius" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlMileRadius_SelectedIndexChanged" />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkEndedCampaigns" runat="server" AutoPostBack="true" OnCheckedChanged="chkEndedCampaigns_CheckedChanged" />
                                                Include campaigns that have ended
                                            </label>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="col-lg-10 col-xs-12">
                                <div class="row">
                                    <div class="col-lg-4">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                Search Campaigns
                                        <asp:Label ID="lblCampaignSearchBy" runat="server"> by Game System</asp:Label>
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Vertical" Height="300px">
                                                    <asp:TreeView ID="tvGameSystem" runat="server" Visible="true" ShowCheckBoxes="None" OnSelectedNodeChanged="tvGameSystem_SelectedNodeChanged" ExpandDepth="0"></asp:TreeView>
                                                    <asp:TreeView ID="tvCampaign" runat="server" Visible="false" OnSelectedNodeChanged="tvCampaign_SelectedNodeChanged" ExpandDepth="0"></asp:TreeView>
                                                    <asp:TreeView ID="tvGenre" runat="server" Visible="false" OnSelectedNodeChanged="tvGenre_SelectedNodeChanged" ExpandDepth="0"></asp:TreeView>
                                                    <asp:TreeView ID="tvStyle" runat="server" Visible="false" OnSelectedNodeChanged="tvStyle_SelectedNodeChanged" ExpandDepth="0"></asp:TreeView>
                                                    <asp:TreeView ID="tvTechLevel" runat="server" Visible="false" OnSelectedNodeChanged="tvTechLevel_SelectedNodeChanged" ExpandDepth="0"></asp:TreeView>
                                                    <asp:TreeView ID="tvSize" runat="server" Visible="false" OnSelectedNodeChanged="tvSize_SelectedNodeChanged" ExpandDepth="0"></asp:TreeView>
                                                </asp:Panel>
                                                <asp:GridView ID="gvCampaigns" runat="server" CssClass="col-xs-12"></asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="row">
                                            <asp:Panel ID="pnlImageURL" runat="server" CssClass="col-xs-12" Height="130" Width="820">
                                                <asp:Image ID="imgCampaignImage" runat="server" AlternateText="Game/Campaign Logo" ImageUrl="img/Logo/CM-1-Madrigal.jpg" />
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlCampaignName" runat="server" CssClass="col-xs-12" Height="45" Width="820">
                                                <asp:Label ID="lblCampaignName" runat="server" Text="Campaign Name" CssClass="panel-heading" Font-Size="XX-Large"></asp:Label>
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlCampaignURL" runat="server" CssClass="col-xs-12" Height="30" Width="820">
                                                <asp:HyperLink ID="hplLinkToSite" runat="server" NavigateUrl="." Target="_blank" Font-Underline="true" ></asp:HyperLink>
                                            </asp:Panel>
                                        </div>

                                        <%--<div class="row">
                                            <div class="col-xs-12">
                                                <asp:Panel ID="pnlOverview" runat="server">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            <asp:Label ID="lblGorC1" runat="server" />
                                                            Overview
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Label ID="lblCampaignOverview" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>--%>
                                        <div class="row">
                                            <div class="col-xs-6">
                                                <asp:Panel ID="pnlSelectors" runat="server">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            <asp:Label ID="lblGorC2" runat="server" />
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Table ID="tblSelectors" runat="server" Width="100%">
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblGameSystem1" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblGameSystem2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblGenre1" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblGenre2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblStyle1" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblStyle2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblTechLevel1" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblTechLevel2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblSize1" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblSize2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblLocation1" runat="server">Primary Location:</asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblLocation2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblEvent1" runat="server">Next Event:</asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblEvent2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblLastUpdated1" runat="server">Last Updated:</asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Left">
                                                                        <asp:Label ID="lblLastUpdated2" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableHeaderRow>
                                                            </asp:Table>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-xs-6">
                                                <asp:Panel ID="pnlSignUpForCampaign" runat="server">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            <asp:Label ID="lblSignUp" runat="server"></asp:Label>Add to Your Campaigns
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Table ID="tblAddCampaigns" runat="server" Width="100%">
                                                                <asp:TableRow>
                                                                    <asp:TableCell VerticalAlign="Top">
                                                                        Available Roles:<br />
                                                                        <asp:CheckBoxList ID="chkSignUp" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"></asp:CheckBoxList>
                                                                        <asp:Button ID="btnSignUpForCampaign" runat="server" CssClass="btn btn-primary" Visible="false" Text="Submit Request" OnClick="btnSignUpForCampaign_Click" />
                                                                        <asp:Label ID="lblSignUpMessage" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblCurrentCampaign" runat="server" Visible="false"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell VerticalAlign="Top">
                                                &nbsp;&nbsp;
                                                                    </asp:TableCell>
                                                                    <asp:TableCell VerticalAlign="Top">
                                                                        Current Roles:
                                                                            <asp:Repeater ID="listCurrentRoles" runat="server">
                                                                                <HeaderTemplate>
                                                                                    <div class="panel-container scroll-150">
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <%# Eval("RoleDescription")%><br />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    </div>
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>
                                                                        <asp:Label ID="lblCurrentRoles" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:Panel ID="pnlOverview" runat="server">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            <asp:Label ID="lblGorC1" runat="server" />
                                                            Overview
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:Label ID="lblCampaignOverview" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--</div>--%>
    </form>
</body>
<%--                <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
                <script>window.jQuery || document.write('<script src="js/vendor/jquery-1.10.2.min.js"><\/script>')</script>
                <script src="js/bootstrap/tab.js"></script>
                <script src="js/plugins.js"></script>
                <script src="js/main.js"></script>--%>

<!-- Google Analytics: change UA-XXXXX-X to be your site's ID and uncomment to use.
  <script>
  	(function(b,o,i,l,e,r){b.GoogleAnalyticsObject=l;b[l]||(b[l]=
  		function(){(b[l].q=b[l].q||[]).push(arguments)});b[l].l=+new Date;
  	e=o.createElement(i);r=o.getElementsByTagName(i)[0];
  	e.src='//www.google-analytics.com/analytics.js';
  	r.parentNode.insertBefore(e,r)}(window,document,'script','ga'));
  	ga('create','UA-XXXXX-X');ga('send','pageview');
  </script> -->

</html>
