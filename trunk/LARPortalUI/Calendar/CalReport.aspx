<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CalReport.aspx.cs" Inherits="LarpPortal.Calendar.CalReport" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CalReportStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CalReportScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="CalReportMain" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Events Calendar Report</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="form-inline">
                <div class="col-xs-12">
                    <label>Date Range: </label>
                    <asp:DropDownList ID="ddlEventDateRange" runat="server" CssClass="form-control autoWidth">
                        <asp:ListItem Value="1" Text="All scheduled"></asp:ListItem>
                        <%--                            <asp:ListItem Value="2" Text="Next 6 months"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Last 3 months"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Last 6 months"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Last 12 months"></asp:ListItem>--%>
                        <asp:ListItem Value="6" Text="All historical"></asp:ListItem>
                    </asp:DropDownList>
                    <label>Sort By: </label>
                    <asp:DropDownList ID="ddlOrderBy" runat="server" CssClass="form-control autoWidth">
                        <asp:ListItem Value="StartDate" Text="Event Date (ascending)"></asp:ListItem>
                        <%--<asp:ListItem Value="2" Text="Event Date (descending)"></asp:ListItem>--%>
                        <asp:ListItem Value="CampaignName, StartDate" Text="Campaign, Event Date"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" />
                    <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" Visible="false" />
                </div>
            </div>
        </div>
        <div class="margin10"></div>
        <asp:Panel ID="pnlReportOutput" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Event Calendar</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView ID="gvCalendar" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                        CssClass="table table-striped table-hover table-condensed">
                                        <Columns>
                                            <asp:BoundField DataField="CampaignName" HeaderText="Campaign" />
                                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                                            <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                                            <asp:BoundField DataField="EventName" HeaderText="Event Description" />
                                            <asp:BoundField DataField="SiteName" HeaderText="Location" />
                                            <asp:BoundField DataField="City" HeaderText="City" />
                                            <asp:BoundField DataField="StateID" HeaderText="State" />
                                            <asp:BoundField DataField="RoleDescription" HeaderText="Role" />
                                            <asp:BoundField DataField="RegistrationStatusName" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
