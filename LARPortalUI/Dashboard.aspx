<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LarpPortal.Dashboard" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .btn {
        }

        .btn-space {
            margin: 0 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-10">
                <h1>My LARP Dashboard
                </h1>
            </div>
            <div class="col-md-2" style="margin-top: 20px;">
                <asp:Button ID="btnHomeView" runat="server" Text="Switch to Home Screen" CssClass="btn btn-block btn-success" OnClick="btnHomeView_Click" />
<%--                    OnClick="lbtnClassicView_Click" />--%>
            </div>
            <hr />
            <div class="col-lg-8">
                <div class="row">
                    <div class="col-lg-12">
                        <h3 style="margin-top: 0px; padding-left: 0px;">Characters</h3>
                    </div>
                </div>
                <asp:GridView ID="gvCharacters" DataKeyNames="CharacterSkillSetID, CharacterID, CharacterAKA, CampaignID, CampaignName" runat="server"
                    AutoGenerateColumns="false" GridLines="None" CssClass="table table-striped table-hover" OnRowCommand="gvCharacters_RowCommand">
                    <HeaderStyle BackColor="#337ab7" ForeColor="white" />
                    <RowStyle Wrap="false" />
                    <Columns>
                        <asp:ButtonField ButtonType="Link" DataTextField="CharacterAKA" CommandName="CharAKA" HeaderText="Character" />
                        <asp:ButtonField ButtonType="Link" DataTextField="TotalCP" CommandName="TotalCP" HeaderText="Total Points" />
                        <asp:ButtonField ButtonType="Link" DataTextField="AvailCP" CommandName="AvailCP" HeaderText="Avail Points" />
                        <asp:ButtonField ButtonType="Link" DataTextField="HistoryStatus" CommandName="History" HeaderText="History" />
                        <asp:ButtonField ButtonType="Link" DataTextField="NumPeople" CommandName="NumPeople" HeaderText="People" />
                        <asp:ButtonField ButtonType="Link" DataTextField="NumPlaces" CommandName="NumPlaces" HeaderText="Places" />
                    </Columns>
                </asp:GridView>
                <div class="row">
                    <div class="col-lg-12">
                        <asp:HyperLink ID="hlAddCharacter" runat="server" NavigateUrl="/Character/CharAdd.aspx" Text="Add New Character" />
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="row">
                    <div class="col-lg-12 padding-left: 0px;">
                        <span style="margin-top: 0px; font-size: 24px;">Campaigns</span>
                        <asp:DropDownList ID="ddlCampaigns" runat="server" AutoPostBack="true" Style="margin-left: 20px;" OnSelectedIndexChanged="ddlCampaigns_SelectedIndexChanged" />
                    </div>
                </div>
                <asp:GridView ID="gvDonations" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvDonations_RowCommand" DataKeyNames="CampaignID"
                    CssClass="table table-striped table-hover table-sm">
                    <HeaderStyle BackColor="#337ab7" ForeColor="white" />
                    <RowStyle Wrap="false" />
                    <Columns>
                        <asp:BoundField DataField="CampaignName" HeaderText="Campaign" />
                        <asp:ButtonField ButtonType="Link" DataTextField="BankedPoints" CommandName="Banked" HeaderText="Banked Points" />
                        <asp:ButtonField ButtonType="Link" DataTextField="Donations" CommandName="Donations" HeaderText="Donations Avail" />
                    </Columns>
                </asp:GridView>
                <div class="row">
                    <div class="col-lg-12">
                        <asp:HyperLink ID="hlFindCampaign" runat="server" NavigateUrl="/Campaigns/JoinACampaign.aspx" Text="Find Campaigns" />
                    </div>
                </div>
            </div>
        </div>
        <hr style="height: 2px; margin-top: 5px; margin-bottom: 10px; margin-top: 10px; background-color: black; color: black;" />
        <div class="row">
            <div class="col-lg-12">
                <h3 style="margin-top: 0px;">Upcoming Events</h3>
            </div>
            <div class="col-lg-12">
                <asp:GridView ID="gvUpcomingEvents" runat="server" AutoGenerateColumns="false" GridLines="None"
                    DataKeyNames="CharacterID, SkillSetID, CampaignID, EventID"
                    OnRowCommand="gvUpcomingEvents_RowCommand" OnRowDataBound="gvUpcomingEvents_RowDataBound" CssClass="table table-striped table-hover">
                    <HeaderStyle BackColor="#337ab7" ForeColor="white" />
                    <RowStyle Wrap="false" />
                    <Columns>
                        <asp:ButtonField ButtonType="Link" DataTextField="StartDate" DataTextFormatString="{0:MM/dd/yyyy}" CommandName="Event" HeaderText="Start Date" />
                        <asp:BoundField DataField="CampaignName" HeaderText="Campaign" />
                        <asp:BoundField DataField="SiteName" HeaderText="Location" />
                        <asp:ButtonField ButtonType="Link" DataTextField="RegistrationStatus" CommandName="Status" HeaderText="Status" />
                        <asp:BoundField DataField="PT" HeaderText="Paid" />
                        <asp:TemplateField HeaderText="Role/Character">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidRoleChar" runat="server" Value='<%#Eval("RoleCharacter") %>' />
                                <asp:LinkButton runat="server" ID="lbRoleChar" Text='<%#Eval("RoleCharacter") %>' CommandName="Role"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                <asp:Label ID="lblRoleChar" runat="server" Text='<%#Eval("RoleCharacter") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PointsTo" HeaderText="Points To" />
                        <asp:TemplateField HeaderText="Info Skills Avail">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lbISAvail" Text='<%#Eval("ISAvail") %>' CommandName="ISAvail"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                <asp:Label ID="lblISAvail" runat="server" Text='<%#Eval("ISAvail") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        There are no events that meet that criteria.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <hr style="height: 2px; margin-top: 5px; margin-bottom: 10px; background-color: black; color: black;" />
        <div class="row">
            <div class="col-lg-12">
                <h3 style="margin-top: 0px;">Recent Events</h3>
            </div>
            <div class="col-lg-12">
                <asp:GridView ID="gvRecentEvents" runat="server" AutoGenerateColumns="false" GridLines="None"
                    DataKeyNames="CharacterID, SkillSetID, CampaignID, StartDate"
                    OnRowCommand="gvRecentEvents_RowCommand" OnRowDataBound="gvRecentEvents_RowDataBound" CssClass="table table-striped table-hover">
                    <HeaderStyle BackColor="#337ab7" ForeColor="white" />
                    <RowStyle Wrap="false" />
                    <Columns>
                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="CampaignName" HeaderText="Campaign" />
                        <asp:ButtonField ButtonType="Link" DataTextField="PELStatus" HeaderText="PEL" CommandName="PEL" />
                        <asp:BoundField DataField="Setup" HeaderText="Set-up/Clean-up" />
                        <asp:TemplateField HeaderText="Role/Character">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidRoleChar" runat="server" Value='<%#Eval("RoleCharacter") %>' />
                                <asp:LinkButton runat="server" ID="lbRoleChar" Text='<%#Eval("RoleCharacter") %>'
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="RoleChar" />
                                <asp:Label ID="lblRoleChar" runat="server" Text='<%#Eval("RoleCharacter") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PointsTo" HeaderText="Points To" />
                        <asp:BoundField DataField="TotalPoints" HeaderText="Points" />
                        <asp:TemplateField HeaderText="Info Skills Requested">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lbISReq" Text='<%#Eval("ISReqDisplay") %>'
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="ISReq" />
                                <asp:Label ID="lblISReq" runat="server" Text='<%#Eval("ISReqDisplay") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        There are no events that meet that criteria.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>



    </div>
</asp:Content>
