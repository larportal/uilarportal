<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CharacterRulebook.aspx.cs" Inherits="LarpPortal.Reports.CharacterRulebook" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharacterRulebookStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="CharacterRulebookScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="CharacterRulebookBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character Rulebook</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <label for="<%= ddlCharacter.ClientID %>" style="padding-left: 10px;">Characters:</label>
                <asp:DropDownList ID="ddlCharacter" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlCharacter_SelectedIndexChanged" AutoPostBack="true" />
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <label for="<%= ddlSkillSet.ClientID %>" id="lblSkillSet" runat="server" style="padding-left: 30px;">Skill Set:</label>
                <asp:DropDownList ID="ddlSkillSet" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlSkillSet_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:HyperLink ID="hlCharacterRulebookFinal" runat="server" NavigateUrl="/Reports/CharacterRulebookFinal.aspx" Target="_blank" Text="Display Character Rulebook" Visible="false"></asp:HyperLink>
                <%--<asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" Visible="false" />--%>
            </div>
        </div>

        <div class="margin10"></div>
        <asp:MultiView ID="mvReport" runat="server">
            <asp:View ID="vwRulebook" runat="server">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading"></div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                            <asp:GridView ID="gvRulebook" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                                CssClass="table table-striped table-hover table-condensed">
                                                <Columns>
                                                    <%--<asp:BoundField DataField="CharacterRulebook" HeaderText="Character Rulebook" />--%>
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
