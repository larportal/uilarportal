<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CharacterSkillStats.aspx.cs" Inherits="LarpPortal.Reports.CharacterSkillStats" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharacterSkillStatsStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="CharacterSkillStatsScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="CharacterSkillStatsBody" ContentPlaceHolderID="MainBody" runat="server">
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
                <label for="<%= ddlEventDate.ClientID %>" style="padding-left: 10px;">Events:</label>
                <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEventDate_SelectedIndexChanged" AutoPostBack="true" />
                <label for="<%= ddlReportType.ClientID %>" id="lblReportType" runat="server" style="padding-left: 10px;">Report Type:</label>
                <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Select Report Type" Value="" Selected="true" />
                    <asp:ListItem Text="Skill Count" Value="SkillCount" />
                    <asp:ListItem Text="Skill Detail" Value="SkillDetail" />
                    <asp:ListItem Text="Skill Type Count" Value="SkillTypeCount" />
                    <asp:ListItem Text="Skill Type Detail" Value="SkillTypeDetail" />
                </asp:DropDownList>
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" Visible="false" />
            </div>
        </div>

        <div class="margin10"></div>
        <asp:MultiView ID="mvReport" runat="server">
            <asp:View ID="vwSkillCount" runat="server">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Character Skill Count</div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                            <asp:GridView ID="gvSkillCount" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                                CssClass="table table-striped table-hover table-condensed">
                                                <Columns>
                                                    <asp:BoundField DataField="ParentName" HeaderText="Parent Name" />
                                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Name" />
                                                    <asp:BoundField DataField="HowMany" HeaderText="How Many?" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwSkillDetail" runat="server">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Character Skill Detail</div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                            <asp:GridView ID="gvSkillDetail" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                                CssClass="table table-striped table-hover table-condensed">
                                                <Columns>
                                                    <asp:BoundField DataField="ParentName" HeaderText="Parent Name" />
                                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Name" />
                                                    <asp:BoundField DataField="SkillType" HeaderText="Skill Type" />
                                                    <asp:BoundField DataField="Character" HeaderText="Character" />
                                                    <asp:BoundField DataField="Player" HeaderText="Player" />
                                                    <asp:BoundField DataField="DisplayOrder" HeaderText="DisplayOrder" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwSkillTypeCount" runat="server">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Character Skill Type Count</div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                            <asp:GridView ID="gvSkillTypeCount" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                                CssClass="table table-striped table-hover table-condensed">
                                                <Columns>
                                                    <asp:BoundField DataField="SkillType" HeaderText="Skill Type" />
                                                    <asp:BoundField DataField="HowMany" HeaderText="How Many?" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwSkillTypeDetail" runat="server">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Character Skill Type Detail</div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                            <asp:GridView ID="gvSkillTypeDetail" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                                CssClass="table table-striped table-hover table-condensed">
                                                <Columns>
                                                    <asp:BoundField DataField="SkillType" HeaderText="Skill Type" />
                                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Name" />
                                                    <asp:BoundField DataField="ParentSkill" HeaderText="Parent Skill" />
                                                    <asp:BoundField DataField="Character" HeaderText="Character" />
                                                    <asp:BoundField DataField="Player" HeaderText="Player" />
                                                    <asp:BoundField DataField="DisplayOrder" HeaderText="DisplayOrder" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwNoReport" runat="server">
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
