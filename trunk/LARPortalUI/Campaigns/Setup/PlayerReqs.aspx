<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="PlayerReqs.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.PlayerReqs" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsRequirementsStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CampaignsRequirementsScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="CampaignsRequirementsBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Requirements<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="margin20"></div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Requirements</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-3">
                                        <label for="<%=tbMembershipFee.ClientID %>">Membership Fee</label>
                                        <asp:TextBox ID="tbMembershipFee" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="<%= ddlFrequency %>">Frequency</label>
                                        <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-control" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-3">
                                        <label for="<%= tbMinAge %>">Minimum Age</label>
                                        <asp:TextBox ID="tbMinAge" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="<%= tbSuperAge.ClientID %>">Supervised Age</label>
                                        <asp:TextBox ID="tbSuperAge" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-6">
                                        <label for="<%= ddlWaiver.ClientID %>">Waivers</label>
                                        <asp:DropDownList ID="ddlWaiver" CssClass="form-control" runat="server" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 text-right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
