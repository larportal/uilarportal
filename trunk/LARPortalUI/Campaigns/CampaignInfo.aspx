<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CampaignInfo.aspx.cs" Inherits="LarpPortal.Campaigns.CampaignInfo" %>

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
                    <h1>Campaign Info<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Demographics</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <table>
                                    <tr>
                                        <td class="TableLabel">Name: </td>
                                        <td>
                                            <asp:Label ID="lblCampaignName" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Started: </td>
                                        <td>
                                            <asp:Label ID="lblStarted" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Exp. End Date: </td>
                                        <td>
                                            <asp:Label ID="lblExpectedEnd" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Number of Events: </td>
                                        <td>
                                            <asp:Label ID="lblNumEvents" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Game System: </td>
                                        <td>
                                            <asp:Label ID="lblGameSystem" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Genres: </td>
                                        <td>
                                            <asp:Label ID="lblGenres" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Style: </td>
                                        <td>
                                            <asp:Label ID="lblStyle" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Tech Level: </td>
                                        <td>
                                            <asp:Label ID="lblTechLevel" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Size: </td>
                                        <td>
                                            <asp:Label ID="lblSize" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Avg # Events: </td>
                                        <td>
                                            <asp:Label ID="lblAvgNumEvents" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">Contacts</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:GridView ID="gvContactInfo" runat="server" ShowHeader="false" AutoGenerateColumns="false" GridLines="none" OnRowCreated="gvContactInfo_RowCreated">
                                    <Columns>
                                        <asp:BoundField DataField="Label" ItemStyle-CssClass="LeftRightPadding10" />
                                        <asp:HyperLinkField DataNavigateUrlFields="URL" DataTextField="URLLabel" ItemStyle-CssClass="LeftRightPadding10" />
                                        <asp:HyperLinkField DataNavigateUrlFields="EMail" DataTextField="EMailLabel" ItemStyle-CssClass="LeftRightPadding10" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Setting Description</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div style="max-height: 180px; overflow-y: auto;">
                                    <asp:Label ID="CampaignDescription" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">Requirements</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <table>
                                    <tr>
                                        <td class="TableLabel">Membership Fee:</td>
                                        <td colspan="2">
                                            <asp:Label ID="lblMembershipFee" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Minimum Age:</td>
                                        <td>
                                            <asp:Label ID="lblMinimumAge" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Supervised Age:</td>
                                        <td>
                                            <asp:Label ID="lblSupervisedAge" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Waiver 1:</td>
                                        <td>
                                            <asp:Label ID="lblWaiver1" runat="server" /></td>
                                        <td>
                                            <asp:HyperLink ID="hlWaiver1" runat="server" Text="Waiver Link" CssClass="LeftRightPadding10" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Waiver 2:</td>
                                        <td>
                                            <asp:Label ID="lblWaiver2" runat="server" /></td>
                                        <td>
                                            <asp:HyperLink ID="hlWaiver2" runat="server" Text="Waiver Link" CssClass="LeftRightPadding10" /></td>
                                    </tr>
                                    <tr>
                                        <td class="TableLabel">Consent:</td>
                                        <td>
                                            <asp:Label ID="lblConsent" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
