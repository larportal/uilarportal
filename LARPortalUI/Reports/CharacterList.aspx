<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CharacterList.aspx.cs" Inherits="LarpPortal.Reports.CharacterList" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignCharactersStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CampaignCharactersScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="CampaignCharactersBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Characters</h1>
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
                    <div class="panel-heading">Campaign Characters</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView ID="gvCampaignCharacters" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                        CssClass="table table-striped table-hover table-condensed">
                                        <Columns>
                                            <asp:BoundField DataField="PlayerFirstName" HeaderText="Player First Name" />
                                            <asp:BoundField DataField="PlayerLastName" HeaderText="Last Name" />
                                            <asp:TemplateField HeaderText="Email (click to send)">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hplEmail" runat="server" Font-Underline="true" Text='<%# Eval("PlayerEmail") %>' NavigateUrl='<%# "mailto:" + Eval("PlayerEmail") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CharacterAKA" HeaderText="Character AKA" />
                                            <asp:BoundField DataField="CharacterType" HeaderText="Skill Set" />
                                            <asp:BoundField DataField="CharacterFirstName" HeaderText="Character First Name" />
                                            <asp:BoundField DataField="CharacterMiddleName" HeaderText="Middle Name" />
                                            <asp:BoundField DataField="CharacterLastName" HeaderText="Last Name" />
                                            <asp:BoundField DataField="WhereFrom" HeaderText="Where From" />
                                            <asp:BoundField DataField="CurrentHome" HeaderText="Current Home" />
                                            <asp:BoundField DataField="Team" HeaderText="Team" />
                                            <asp:BoundField DataField="TotalPoints" HeaderText="TotalPoints" />
                                            <asp:BoundField DataField="NumberDeaths" HeaderText="NumberDeaths" />
                                            <asp:BoundField DataField="NumberSkillSets" HeaderText="NumberSkillSets" />
                                            <asp:BoundField DataField="History" HeaderText="History?" />
                                            <asp:BoundField DataField="Relationships" HeaderText="Relationships" />
                                            <asp:BoundField DataField="Places" HeaderText="Places" />
                                            <asp:BoundField DataField="HiddenSkills" HeaderText="HiddenSkills" />
                                            <asp:BoundField DataField="Items" HeaderText="Items" />
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
