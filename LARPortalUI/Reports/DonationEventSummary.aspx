﻿<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="DonationEventSummary.aspx.cs" Inherits="LarpPortal.Reports.DonationEventSummary" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CheckInStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="CheckInScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function EventChange() {
            try {
                var ddlEvent = document.getElementById("<%= ddlEvent.ClientID %>");
                var btnRunReport = document.getElementById("<%= btnRunReport.ClientID %>");
                var btnExportExcel = document.getElementById("<%= btnExportExcel.ClientID %>");

                btnRunReport.style.display = "none";
                if (btnExportExcel)
                    btnExportExcel.style.display = "none";

                var EventDate = ddlEvent.options[ddlEvent.selectedIndex].value;

                if (EventDate != "") {
                    btnRunReport.style.display = "inline";
                }
            }
            catch (err) {
                return false;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="DonationBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Donation Event Summary Report</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                <label for="ddlEvent" runat="server" id="lblEvent" style="padding-left: 10px;">Event: </label>
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control" />
                <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" />
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" />
            </div>
        </div>
        <div class="margin10"></div>
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Donation Summary</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                        <asp:GridView ID="gvCheckList" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed">
                                            <Columns>
                                                <asp:BoundField DataField="EventDate" HeaderText="Event Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                                                <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                                                <asp:BoundField DataField="EventStatus" HeaderText="Event Status" />
                                                <asp:BoundField DataField="Category" HeaderText="Category" />
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />

                                                <asp:BoundField DataField="QtyNeeded" HeaderText="# Needed" />
                                                <asp:BoundField DataField="DateAdded" HeaderText="Date Added" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                                                <asp:BoundField DataField="NumberClaimed" HeaderText="# Claimed" />
                                                <asp:BoundField DataField="NumberReceived" HeaderText="# Received" />
                                                <asp:BoundField DataField="PercentClaimed" HeaderText="% Claimed" DataFormatString="{0:P2}" HtmlEncode="false" />

                                                <asp:BoundField DataField="PercentReceived" HeaderText="% Received" DataFormatString="{0:P2}" HtmlEncode="false" />
                                                <asp:BoundField DataField="Date100PercClaimed" HeaderText="Date 100% Claimed" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                                                <asp:BoundField DataField="StaffComments" HeaderText="Staff Notes" />
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
