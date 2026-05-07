<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="SearchCampaignsNew.aspx.cs" Inherits="LarpPortal.Campaigns.SearchCampaignsNew" %>

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
        @keyframes glowing {
            0% {
                background-color: #337ab7;
                box-shadow: 0 0 3px #337ab7;
            }

            50% {
                background-color: #f0ad4e;
                box-shadow: 0 0 12px #f0ad4e;
            }

            100% {
                background-color: #337ab7;
                box-shadow: 0 0 3px #337ab7;
            }
        }

        .button-glow {
            animation: glowing 1300ms infinite;
            color: white !important;
            font-weight: bold;
        }

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

        .campaign-description {
            max-height: 130px;
            overflow: hidden;
            position: relative;
        }

        .campaign-description-wrapper {
            border-bottom: 1px solid #ccc;
            padding-bottom: 8px;
            margin-bottom: 8px;
        }

        .campaign-description.expanded-description {
            max-height: none;
        }

        .see-more-link {
            display: inline-block;
            margin-top: 5px;
            cursor: pointer;
        }
    </style>

    <!-- Custom Theme JavaScript -->
    <script src="/Scripts/sb-admin-2.js"></script>
    <script src="/Scripts/jquery.matchHeight.js"></script>
    <script>
        $(document).ready(function () {
            $("input[type=text]").on("input", function () {
                Blink();
            });
        });
        function Blink() {
            var btn = document.getElementById('<%= btnApplyFilters.ClientID %>');
            if (!btn) return;

            btn.classList.remove("button-glow");

            // Force browser to restart the animation
            void btn.offsetWidth;

            btn.classList.add("button-glow");

            var hidBlink = document.getElementById('<%= hidBlink.ClientID %>');
            if (hidBlink) {
                hidBlink.value = "Showing";
            }
        }

        function updateAddCampaignButton(chk) {
            var row = chk.closest(".campaign-row");
            if (!row) return;

            var pc = row.querySelector("input[id*='chkPC']");
            var npc = row.querySelector("input[id*='chkNPC']");
            var btn = row.querySelector("input[id*='btnAddCampaign']");

            if (!btn) return;

            var pcAvailableChecked = pc && !pc.disabled && pc.checked;
            var npcAvailableChecked = npc && !npc.disabled && npc.checked;

            btn.disabled = !(pcAvailableChecked || npcAvailableChecked);
        }

        function looksLikePostalCode(value) {
            if (!value) return false;

            value = value.trim().toUpperCase();

            // US ZIP or ZIP+4
            if (/^\d{5}(-\d{4})?$/.test(value)) return true;

            // Canadian postal code: A1A 1A1 or A1A1A1
            if (/^[A-Z]\d[A-Z]\s?\d[A-Z]\d$/.test(value)) return true;

            return false;
        }

        function updateDistanceState() {
            var zipBox = document.getElementById('<%= txtZipFilter.ClientID %>');
            var ddl = document.getElementById('<%= ddlDistanceFilter.ClientID %>');

            var isValidFormat = looksLikePostalCode(zipBox.value);

            if (isValidFormat) {
                ddl.disabled = false;
            } else {
                ddl.selectedIndex = 0;
                ddl.disabled = true;
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            var zipBox = document.getElementById('<%= txtZipFilter.ClientID %>');

            if (zipBox) {
                zipBox.addEventListener("input", updateDistanceState);
                zipBox.addEventListener("input", Blink);
            }

            var filterControls = [
        '<%= ddlNameFilter.ClientID %>',
        '<%= ddlStateFilter.ClientID %>',
        '<%= ddlDistanceFilter.ClientID %>',
        '<%= ddlSystemFilter.ClientID %>',
        '<%= ddlGenreFilter.ClientID %>',
        '<%= ddlStyleFilter.ClientID %>',
        '<%= ddlTechFilter.ClientID %>',
        '<%= ddlSizeFilter.ClientID %>'
            ];

            filterControls.forEach(function (id) {
                var ctl = document.getElementById(id);
                if (ctl) {
                    ctl.addEventListener("change", Blink);
                }
            });

            updateDistanceState();
        });
    </script>

    <script>
        function toggleDescription(link) {
            var desc = link.previousElementSibling;

            if (desc.classList.contains("expanded-description")) {
                desc.classList.remove("expanded-description");
                link.innerText = "See More";
            } else {
                desc.classList.add("expanded-description");
                link.innerText = "See Less";
            }
        }

        // Run after page load (works with UpdatePanel too)
        function initDescriptionToggles() {
            var wrappers = document.querySelectorAll(".campaign-description-wrapper");

            wrappers.forEach(function (w) {
                var desc = w.querySelector(".campaign-description");
                var link = w.querySelector(".see-more-link");
                if (!desc || !link) return;

                // If content doesn't overflow, hide the link
                if (desc.scrollHeight <= desc.clientHeight + 1) {
                    link.style.display = "none";
                } else {
                    link.style.display = "inline-block";
                }
            });
        }

        // Initial load
        document.addEventListener("DOMContentLoaded", initDescriptionToggles);

        // If you use UpdatePanel (ASP.NET AJAX), re-run after partial postback
        if (typeof Sys !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initDescriptionToggles);
        }
    </script>


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

    <!-- Bootstrap Toggle to make checkboxes into sliders.  -->
    <link href="/Content/bootstrap-toggle.min.css" rel="stylesheet" />
    <script src="/Scripts/bootstrap-toggle.min.js"></script>
</head>

<body runat="server" id="pageBodyNew">
    <form id="form2New" runat="server">

        <asp:ScriptManager ID="smManagerNew" runat="server" />
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

        <div class="row mb-2" style="padding-left: 5px;">
            <div class="col-xs-12" style="font-size: 20px;">
                <strong>Find a LARP</strong>
            </div>
        </div>

        <div style="height: 3px; background-color: darkblue; width: 100%; margin: 5px"></div>

        <div class="row mb-2" style="padding-left: 5px;">
            <div class="col-xs-12">
                <strong>FILTERS</strong>
                <asp:HiddenField ID="hidBlink" runat="server" />
            </div>
        </div>

        <!-- FILTER ROW 1 -->
        <div class="row mb-2" style="padding-left: 15px;">
            <div class="col-xs-2 form-group form-inline">
                <label for="ddlNameFilter">Name:&nbsp;</label>
                <asp:DropDownList ID="ddlNameFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-2 form-group form-inline">
                <label for="ddlStateFilter">State:&nbsp;</label>
                <asp:DropDownList ID="ddlStateFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="txtZipFilter">Zip:&nbsp;</label>
                <asp:TextBox ID="txtZipFilter"
                    runat="server"
                    CssClass="form-control"
                    Style="width: 75%;"
                    MaxLength="10"
                    placeholder="Enter zip"
                    oninput="Blink();" />
                <br />
                <asp:Label ID="lblZipError" runat="server" ForeColor="Red" Font-Size="Small" Visible="false" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="ddlDistanceFilter">Distance:&nbsp;</label>
                <asp:DropDownList ID="ddlDistanceFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-2 form-group form-inline">
                <label for="ddlSystemFilter">System:&nbsp;</label>
                <asp:DropDownList ID="ddlSystemFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>
        </div>

        <!-- FILTER ROW 2 -->
        <div class="row mb-2" style="padding-left: 15px;">
            <div class="col-xs-2 form-group form-inline">
                <label for="ddlGenreFilter">Genre:&nbsp;</label>
                <asp:DropDownList ID="ddlGenreFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-2 form-group form-inline">
                <label for="ddlStyleFilter">Style:&nbsp;</label>
                <asp:DropDownList ID="ddlStyleFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="ddlTechFilter">Tech:&nbsp;</label>
                <asp:DropDownList ID="ddlTechFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="ddlSizeFilter">Size:&nbsp;</label>
                <asp:DropDownList ID="ddlSizeFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-1 form-group">
                <asp:Button ID="btnClearAll"
                    runat="server"
                    CssClass="btn btn-primary btn-block"
                    Style="width: 90%;"
                    Text="Clear All"
                    ToolTip="Clear all filters"
                    OnClick="btnClearAll_Click" />
            </div>
            <div class="col-xs-1 form-group">
                <asp:Button ID="btnApplyFilters"
                    runat="server"
                    CssClass="btn btn-primary btn-block"
                    Style="width: 90%;"
                    Text="Apply Filters"
                    ToolTip="Apply all filters"
                    OnClick="btnApplyFilters_Click" />
            </div>

        </div>

        <!-- SORT ROW -->
        <%--        <div class="row mb-2" style="padding-left: 15px;">
            <div class="col-xs-2 form-group form-inline">
                <label for="ddlSort">Sort By:&nbsp;</label>
                <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>
        </div>--%>
        <div style="height: 3px; background-color: darkblue; width: 100%; margin: 5px"></div>

        <%-- Content Line --%>

        <asp:Repeater ID="rptCampaigns" runat="server" OnItemCommand="rptCampaigns_ItemCommand" OnItemDataBound="rptCampaigns_ItemDataBound">
            <ItemTemplate>

                <!-- SINGLE CAMPAIGN ROW -->
                <div class="row campaign-row" style="padding: 15px 0;">

                    <!-- LEFT COLUMN — Logo + City/State -->
                    <div class="col-sm-2 text-center" style="min-height: 190px;">

                        <div style="width: 170px; height: 100px; display: flex; align-items: center; justify-content: center; margin: 0 auto 4px auto;">
                            <asp:PlaceHolder ID="phLogo" runat="server"
                                Visible='<%# !string.IsNullOrWhiteSpace(SafeEval(Eval("LogoUrl"))) %>'>

                                <img src='<%# GetLogoPath(Eval("LogoUrl")) %>'
                                    style='<%# GetLogoStyle(Eval("CampaignLogoWidth"), Eval("CampaignLogoHeight")) %>'
                                    alt="Campaign Logo"
                                    onerror="this.style.display='none';" />

                            </asp:PlaceHolder>
                        </div>

                        <h4 style="margin-top: 4px;">
                            <strong><%# SafeEval(Eval("CampaignName")) %></strong>
                        </h4>

                        <h5 style="margin-top: 4px;">
                            <%# SafeEval(Eval("City")) %>, <%# SafeEval(Eval("State")) %>
                        </h5>

                    </div>

                    <!-- MIDDLE COLUMN — Description + Links + Events -->
                    <div class="col-sm-7">
                        <div class="campaign-description-wrapper">
                            <div class="campaign-description collapsed-description">
                                <%# SafeEval(Eval("Description")) %>
                            </div>
                            <a href="javascript:void(0);" class="see-more-link" onclick="toggleDescription(this);">See More</a>
                        </div>

                        <p>
                            <%# RenderLink(Eval("CampaignUrl"), "Website") %>&nbsp;&nbsp;
                            <%# RenderLink(Eval("RulesUrl"), "Rules") %>&nbsp;&nbsp;
                            <%# RenderLink(Eval("DiscordUrl"), "Discord") %>
                        </p>

                        <p>
                            <strong>Upcoming Events:</strong>
                            &nbsp;<%# Eval("Event1", "{0:MM/dd/yyyy}") %> &nbsp;&nbsp;
                            &nbsp;<%# Eval("Event2", "{0:MM/dd/yyyy}") %> &nbsp;&nbsp;
                            &nbsp;<%# Eval("Event3", "{0:MM/dd/yyyy}") %>
                        </p>
                    </div>

                    <!-- RIGHT COLUMN — Attributes + Checkboxes + Button -->
                    <div class="col-sm-3" style="line-height: 1.0">

                        <p><strong>Game System:</strong> <%# SafeEval(Eval("GameSystem")) %></p>
                        <p><strong>Genre:</strong> <%# SafeEval(Eval("Genre")) %></p>
                        <p><strong>Style:</strong> <%# SafeEval(Eval("Style")) %></p>
                        <p><strong>Tech Level:</strong> <%# SafeEval(Eval("TechLevel")) %></p>
                        <p><strong>Size:</strong> <%# SafeEval(Eval("Size")) %></p>
                        <p>
                            <strong>Primary Location:</strong>
                            <%# SafeEval(Eval("PrimaryLocation")) %>
                            <%# string.IsNullOrWhiteSpace(SafeEval(Eval("PrimaryCity")))
                                ? ""
                                : " - " + SafeEval(Eval("PrimaryCity")) + ", " + SafeEval(Eval("PrimaryState")) %>
                        </p>

                        <p>
                            <strong>Secondary Location:</strong>
                            <%# SafeEval(Eval("SecondaryLocation")) %>
                            <%# string.IsNullOrWhiteSpace(SafeEval(Eval("SecondaryCity")))
                                ? ""
                                : " - " + SafeEval(Eval("SecondaryCity")) + ", " + SafeEval(Eval("SecondaryState")) %>
                        </p>

                        <!-- PC / NPC Checkboxes -->
                        <div class="checkbox">
                            <span id="spnPC" runat="server">
                                <label>
                                    <asp:CheckBox ID="chkPC" runat="server" onclick="updateAddCampaignButton(this);" />
                                    PC
                                </label>
                            </span>

                            <span id="spnNPC" runat="server">
                                <label>
                                    <asp:CheckBox ID="chkNPC" runat="server" onclick="updateAddCampaignButton(this);" />
                                    NPC
                                </label>
                            </span>
                        </div>

                        <!-- Add Campaign Button -->
                        <asp:Button ID="btnAddCampaign"
                            runat="server"
                            CssClass="btn btn-primary btn-block"
                            Style="width: 30%;"
                            Text="Add Campaign"
                            CommandName="AddCampaign"
                            CommandArgument='<%# Eval("CampaignID") %>' />
                    </div>



                </div>

                <!-- DIVIDER BETWEEN CAMPAIGNS -->
                <hr style="border: 0; border-top: 1px solid #ccc; margin: 10px 0;" />

            </ItemTemplate>
        </asp:Repeater>

        <div class="row mt-3">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnPreviousPage" runat="server"
                    Text="Previous"
                    CssClass="btn btn-secondary"
                    OnClick="btnPreviousPage_Click" />

                <asp:Label ID="lblPageInfo" runat="server"
                    CssClass="mx-3" />

                <asp:Button ID="btnNextPage" runat="server"
                    Text="Next"
                    CssClass="btn btn-secondary"
                    OnClick="btnNextPage_Click" />
            </div>
        </div>

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
