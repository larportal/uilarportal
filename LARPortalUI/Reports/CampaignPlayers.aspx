<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CampaignPlayers.aspx.cs" Inherits="LarpPortal.Reports.CampaignPlayers" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignPlayersStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CampaignPlayersScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="CampaignPlayersBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Players</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" />
            </div>
        </div>
        <div class="divide10"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Campaign Players</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView ID="gvCampaignPlayers" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                        CssClass="table table-striped table-hover table-condensed">
                                        <Columns>
                                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                            <asp:BoundField DataField="Nickname" HeaderText="Nickname" />
                                            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                            <asp:TemplateField HeaderText="Email (click to send)">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hplEmail" runat="server" Font-Underline="true" Text='<%# Eval("EmailAddress") %>' NavigateUrl='<%# "mailto:" + Eval("EmailAddress") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PC" HeaderText="PC" />
                                            <asp:BoundField DataField="NPC" HeaderText="NPC" />
                                            <asp:BoundField DataField="Staff" HeaderText="Staff" />
                                            <asp:BoundField DataField="LastEventRegDate" HeaderText="Last Registered Event Date" />
                                            <asp:BoundField DataField="Role" HeaderText="Last Role" />
                                            <asp:BoundField DataField="CharacterPlayed" HeaderText="Character Played" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
