<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="LarpPortal.Reports.ReportList" EnableEventValidation="false" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="ReportListStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="ReportListScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="ReportListBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Reports</h1>
                </div>
            </div>
        </div>
        <div class="divide10"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Reports for
                        <asp:Label ID="lblCampaignName" runat="server" /></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView ID="gvReportsList" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="PageNameURL"
                                        OnRowCommand="gvReportsList_RowCommand" OnRowCreated="gvReportsList_RowCreated"
                                        CssClass="table table-striped table-hover" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                                        <Columns>
                                            <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="ReportName" HeaderText="Report Name" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="ReportDescription" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
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
