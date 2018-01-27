<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="HistorySearch.aspx.cs" Inherits="LarpPortal.Reports.HistorySearch" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="HistorySearchStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="HistorySearchScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="HistorySearchBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character History Keyword Search</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                <label for="<%= tbSearchString.ClientID %>" style="padding-left: 10px;">Search: </label>
                <asp:TextBox ID="tbSearchString" runat="server" CssClass="form-control" />
                <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" />
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" Visible="false" />
            </div>
        </div>
        <div class="divide10"></div>
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">History Keyword Search Results</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                        <asp:GridView ID="gvHistorySearch" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed">
                                            <Columns>
                                                <asp:BoundField DataField="CharacterAKA" HeaderText="Character" />
                                                <asp:BoundField DataField="Player" HeaderText="Player" />
                                                <asp:BoundField DataField="HistoryString" HeaderText="History String" HtmlEncode="false" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Redirecter" Text="View complete History" Target="_blank" />
                                                <%--                                                ControlStyle-Font-Size="X-Small" ItemStyle-Wrap="false"
                                                ControlStyle-ForeColor="DarkBlue" ControlStyle-Font-Underline="true" />--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
