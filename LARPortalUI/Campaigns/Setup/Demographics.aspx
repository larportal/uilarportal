<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Demographics.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Demographics" %>
<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                <h1>Campaign Setup Information<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                    </div>
            </div>
        </div>
        <div class="row">
            <div class="jumbotron">
                Campaign Setup Information - Changes are currently disabled on this page
            </div>
            <div class="margin20"></div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Demographics</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <label for="<%= tbCampaignName.ClientID %>">Campaign Name</label>
                                                <asp:TextBox ID="tbCampaignName" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= tbLARPPortalType.ClientID %>">LARP Portal Type</label>
                                                <asp:TextBox ID="tbLARPPortalType" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <label for="<%= ddlGameSystem.ClientID %>">Game System</label>
                                                <asp:DropDownList ID="ddlGameSystem" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlGameSystem_SelectedIndexChanged" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= ddlCampaignStatus.ClientID %>">Campaign Status</label>
                                                <asp:DropDownList ID="ddlCampaignStatus" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCampaignStatus_SelectedIndexChanged" AutoPostBack="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <label for="<%= tbOwner.ClientID %>">Owner</label>
                                                <asp:TextBox ID="tbOwner" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= tbDateStarted.ClientID %>">Date Started</label>
                                                <asp:TextBox ID="tbDateStarted" runat="server" CssClass="form-control" />
                                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateStarted" Format="MM/dd/yyyy" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:HiddenField ID="hidCampaignZip" runat="server" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= tbExpectedEndDate.ClientID %>">Exp.End Date</label>
                                                <asp:TextBox ID="tbExpectedEndDate" runat="server" CssClass="form-control" />
                                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbExpectedEndDate" Format="MM/dd/yyyy" />
                                            </div>
                                        </div>
                                    </div>
                                    <%--                                        <div class="row PrePostPadding">
                                        <div class="TableLabel col-sm-3">
                                            Primary Site: 
                                        </div>
                                        <div class="col-sm-3 NoPadding">
                                            <asp:DropDownList ID="ddlPrimarySite" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPrimarySite_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="TableLabel col-lg-3" style="position: relative;">Actual End Date: </div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:TextBox ID="tbActualEndDate" runat="server" CssClass="TableTextBox" />
                                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbActualEndDate" Format="MM/dd/yyyy" />
                                        </div>
                                    </div>
                                    <div class="row PrePostPadding">
                                        <div class="TableLabel col-lg-3">
                                            Avg # Events / Yr:
                                        </div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:TextBox ID="tbAvgNoEvents" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="TableLabel col-lg-3">
                                            Exp.Total Events:
                                        </div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:TextBox ID="tbProjTotalNumEvents" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row PrePostPadding">
                                        <div class="TableLabel col-sm-3">
                                            Emergency Contact: 
                                        </div>
                                        <div class="col-sm-3 NoPadding">
                                            <asp:TextBox ID="tbEmergencyContact" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="TableLabel col-sm-3">
                                            Phone: 
                                        </div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:TextBox ID="tbEmergencyPhone" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row PrePostPadding">
                                        <div class="TableLabel col-sm-3">
                                            Style:
                                        </div>
                                        <div class="col-sm-3 NoPadding">
                                            <asp:DropDownList ID="ddlStyle" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative" OnSelectedIndexChanged="ddlStyle_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="TableLabel col-lg-3">
                                            Tech Level:
                                        </div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:DropDownList ID="ddlTechLevel" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative" OnSelectedIndexChanged="ddlTechLevel_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row PrePostPadding">
                                        <div class="TableLabel col-sm-3">
                                            Size:
                                        </div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:DropDownList ID="ddlSize" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row PrePostPadding">
                                        <div class="TableLabel col-sm-3">
                                            Genres:
                                        </div>
                                        <div class="col-sm-7 NoPadding">
                                            <asp:Label ID="lblGenres" runat="server" />
                                        </div>
                                        <div class="col-lg-2 NoPadding">
                                            <asp:Button ID="btnEditGenres" runat="server" CssClass="StandardButton" Text="Change Genres" OnClick="btnEditGenres_Click" />
                                        </div>
                                    </div>
                                    <div class="row PrePostPadding">
                                        <div class="TableLabel col-sm-3">
                                            Periods:
                                        </div>
                                        <div class="col-sm-7 NoPadding">
                                            <asp:Label ID="lblPeriods" runat="server" />
                                        </div>
                                        <div class="col-lg-2 NoPadding">
                                            <asp:Button ID="btnEditPeriods" runat="server" CssClass="StandardButton" Text="Change Periods" OnClick="btnEditPeriods_Click" />
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 text-right">
                    <asp:Button ID="btnSaveRepeat" runat="server" Text="Save" CssClass="btn btn-primary" /><%-- OnClick="btnSaveChanges_Click" />--%>
                </div>
            </div>
        </div>

        <%--            <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                <asp:Panel ID="pnlSites" runat="server" Visible="false">
                    <div class="col-lg-12 NoPadding">
                        <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                            <div class="panelheader NoPadding">
                                <h2>Campaign Sites</h2>
                                <div class="panel-body NoPadding">
                                    <div class="panel-container">
                                        <div class="row PrePostPadding">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-lg-11"></div>
                    <div class="col-lg-1">
                        <asp:Button ID="btnSaveSites" runat="server" Text="Save Sites" Visible="false" OnClick="btnSaveSites_Click" />
                    </div>
                </div>
            </div>
            <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                <asp:Panel ID="pnlEditGenres" runat="server" Visible="false">
                    <div class="col-lg-12 NoPadding">
                        <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                            <div class="panelheader NoPadding">
                                <h2>Campaign Genres</h2>
                                <div class="panel-body NoPadding">
                                    <div class="panel-container">
                                        <div class="row PrePostPadding">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-lg-11"></div>
                    <div class="col-lg-1">
                        <asp:Button ID="btnSaveGenres" runat="server" Text="Save Genres" Visible="false" OnClick="btnSaveGenres_Click" />
                    </div>
                </div>
            </div>
            <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                <asp:Panel ID="pnlEditPeriods" runat="server" Visible="false">
                    <div class="col-lg-12 NoPadding">
                        <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                            <div class="panelheader NoPadding">
                                <h2>Campaign Periods</h2>
                                <div class="panel-body NoPadding">
                                    <div class="panel-container">
                                        <div class="row PrePostPadding">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-lg-11"></div>
                    <div class="col-lg-1">
                        <asp:Button ID="btnSavePeriods" runat="server" Text="Save Periods" Visible="false" OnClick="btnSavePeriods_Click" />
                    </div>
                </div>
            </div>
        </div>--%>
    </div>



    <%--    <div class="mainContent tab-content col-lg-12 input-group">
        <section id="campaign-info" class="campaign-info tab-pane active">
            <div role="form" class="form-horizontal">
                <div class="col-lg-12 NoPadding">
                    <h1 class="col-lg-12">Campaign Setup Information - Changes are currently disabled on this page</h1>
                </div>
                <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                    <div class="row">
                        <div class="col-lg-11"></div>
                        <div class="col-lg-1">
                            <asp:Button ID="btnSaveChanges" runat="server" Visible="false" Text="Save" OnClick="btnSaveChanges_Click" />
                        </div>
                    </div>
                    <asp:Panel ID="pnlDemographics" runat="server">
                        <div class="col-lg-12 NoPadding">
                            <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                                <div class="panelheader NoPadding">
                                    <h2>Demographics</h2>
                                    <div class="panel-body NoPadding">
                                        <div class="panel-container">
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">Campaign Name: </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:Label ID="lblCampaignName" runat="server" Text="" />
                                                </div>
                                                <div class="TableLabel col-sm-3">LARP Portal Type:</div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:Label ID="lblLARPPortalType" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Game System: 
                                                </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:DropDownList ID="ddlGameSystem" CssClass="NoPadding" runat="server" OnSelectedIndexChanged="ddlGameSystem_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="TableLabel col-lg-3" style="position: relative;">Campaign Status: </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:DropDownList ID="ddlCampaignStatus" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative;" OnSelectedIndexChanged="ddlCampaignStatus_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Owner: 
                                                </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:Label ID="lblOwner" runat="server" />
                                                </div>
                                                <div class="TableLabel col-sm-3">
                                                    Date Started: 
                                                </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:TextBox ID="tbDateStarted" runat="server" CssClass="TableTextBox" />
                                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateStarted" Format="MM/dd/yyyy" />
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:HiddenField ID="hidCampaignZip" runat="server" />
                                                </div>
                                                <div class="TableLabel col-lg-3" style="position: relative;">Exp.End Date: </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:TextBox ID="tbExpectedEndDate" runat="server" CssClass="TableTextBox" />
                                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbExpectedEndDate" Format="MM/dd/yyyy" />
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Primary Site: 
                                                </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:DropDownList ID="ddlPrimarySite" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPrimarySite_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="TableLabel col-lg-3" style="position: relative;">Actual End Date: </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:TextBox ID="tbActualEndDate" runat="server" CssClass="TableTextBox" />
                                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbActualEndDate" Format="MM/dd/yyyy" />
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-lg-3">
                                                    Avg # Events / Yr:
                                                </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:TextBox ID="tbAvgNoEvents" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="TableLabel col-lg-3">
                                                    Exp.Total Events:
                                                </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:TextBox ID="tbProjTotalNumEvents" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Emergency Contact: 
                                                </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:TextBox ID="tbEmergencyContact" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="TableLabel col-sm-3">
                                                    Phone: 
                                                </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:TextBox ID="tbEmergencyPhone" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Style:
                                                </div>
                                                <div class="col-sm-3 NoPadding">
                                                    <asp:DropDownList ID="ddlStyle" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative" OnSelectedIndexChanged="ddlStyle_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                <div class="TableLabel col-lg-3">
                                                    Tech Level:
                                                </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:DropDownList ID="ddlTechLevel" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative" OnSelectedIndexChanged="ddlTechLevel_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Size:
                                                </div>
                                                <div class="col-lg-3 NoPadding">
                                                    <asp:DropDownList ID="ddlSize" CssClass="NoPadding" runat="server" Style="z-index: 500; position: relative" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Genres:
                                                </div>
                                                <div class="col-sm-7 NoPadding">
                                                    <asp:Label ID="lblGenres" runat="server" />
                                                </div>
                                                <div class="col-lg-2 NoPadding">
                                                    <asp:Button ID="btnEditGenres" runat="server" CssClass="StandardButton" Text="Change Genres" OnClick="btnEditGenres_Click" />
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding">
                                                <div class="TableLabel col-sm-3">
                                                    Periods:
                                                </div>
                                                <div class="col-sm-7 NoPadding">
                                                    <asp:Label ID="lblPeriods" runat="server" />
                                                </div>
                                                <div class="col-lg-2 NoPadding">
                                                    <asp:Button ID="btnEditPeriods" runat="server" CssClass="StandardButton" Text="Change Periods" OnClick="btnEditPeriods_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-lg-11"></div>
                        <div class="col-lg-1">
                            <asp:Button ID="btnSaveRepeat" runat="server" Text="Save" OnClick="btnSaveChanges_Click" />
                        </div>
                    </div>
                </div>

                <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                    <asp:Panel ID="pnlSites" runat="server" Visible="false">
                        <div class="col-lg-12 NoPadding">
                            <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                                <div class="panelheader NoPadding">
                                    <h2>Campaign Sites</h2>
                                    <div class="panel-body NoPadding">
                                        <div class="panel-container">
                                            <div class="row PrePostPadding">
 
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-lg-11"></div>
                        <div class="col-lg-1">
                            <asp:Button ID="btnSaveSites" runat="server" Text="Save Sites" Visible="false" OnClick="btnSaveSites_Click"/>
                        </div>
                    </div>
                </div>
                <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                    <asp:Panel ID="pnlEditGenres" runat="server" Visible="false">
                        <div class="col-lg-12 NoPadding">
                            <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                                <div class="panelheader NoPadding">
                                    <h2>Campaign Genres</h2>
                                    <div class="panel-body NoPadding">
                                        <div class="panel-container">
                                            <div class="row PrePostPadding">
 
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-lg-11"></div>
                        <div class="col-lg-1">
                            <asp:Button ID="btnSaveGenres" runat="server" Text="Save Genres" Visible="false" OnClick="btnSaveGenres_Click"/>
                        </div>
                    </div>
                </div>
                <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                    <asp:Panel ID="pnlEditPeriods" runat="server" Visible="false">
                        <div class="col-lg-12 NoPadding">
                            <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                                <div class="panelheader NoPadding">
                                    <h2>Campaign Periods</h2>
                                    <div class="panel-body NoPadding">
                                        <div class="panel-container">
                                            <div class="row PrePostPadding">
 
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-lg-11"></div>
                        <div class="col-lg-1">
                            <asp:Button ID="btnSavePeriods" runat="server" Text="Save Periods" Visible="false" OnClick="btnSavePeriods_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>--%>
</asp:Content>
