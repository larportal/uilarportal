<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="SiteList.aspx.cs" Inherits="LarpPortal.Reports.SiteList" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="SiteListStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="SiteListScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="SiteListBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>PEL Answer Summary Report</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <label for="ddlCountryChoice">Country: </label>
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" />
                <label for="ddlState" style="padding-left: 10px;">State: </label>
                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" />
                <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" />
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" Visible="false" />
            </div>
        </div>
        <div class="margin10"></div>
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">LARP Portal Site List</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                        <asp:GridView ID="gvSites" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed">
                                            <Columns>
                                                <asp:BoundField DataField="Country" HeaderText="Country" />
                                                <asp:BoundField DataField="State" HeaderText="State" />
                                                <asp:BoundField DataField="City" HeaderText="City" />
                                                <asp:BoundField DataField="SiteName" HeaderText="Site" />
                                                <asp:BoundField DataField="Address1" HeaderText="Address" />
                                                <asp:BoundField DataField="PostalCode" HeaderText="Zip" />
                                                <asp:BoundField DataField="Phone" HeaderText="Phone Number" />
                                                <asp:BoundField DataField="Extension" HeaderText="Ext" />
                                                <asp:HyperLinkField HeaderText="Web Site" DataTextField="URL" DataNavigateUrlFields="URL"
                                                    DataNavigateUrlFormatString="http://{0}" Target="_blank" />
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
