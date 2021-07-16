<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="JoinACampaign.aspx.cs" Inherits="LarpPortal.Campaigns.JoinACampaign" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="JoinACampaignStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <link href="../Content/jasny-bootstrap.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="JoinACampaignScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script src="../Scripts/jasny-bootstrap.min.js"></script>
</asp:Content>

<asp:Content ID="JoinACampaignBody" ContentPlaceHolderID="MainBody" runat="server">



    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaigns</h1>
                </div>
            </div>
        </div>
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
                                                <p>
                                                    <asp:HyperLink ID="hplLinkToSite" runat="server" NavigateUrl="." Target="_blank" Font-Underline="true"></asp:HyperLink>
                                                </p>
                                            </asp:Panel>
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
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


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
</asp:Content>
