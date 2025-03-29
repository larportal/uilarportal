<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="DonationEventDetailByItem.aspx.cs" Inherits="LarpPortal.Reports.DonationEventDetailByItem" %>

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
                    <h1>Donation Event By Item</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
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
                        <div class="panel-heading">Donation Event Detail By Item</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                        <asp:GridView ID="gvCheckList" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed">
                                            <Columns>
                                                <asp:BoundField DataField="EventDate" HeaderText="Event Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                                <asp:BoundField DataField="Category" HeaderText="Category" />
                                                <asp:BoundField DataField="QtyNeeded" HeaderText="# Needed" />
                                                <asp:BoundField DataField="StaffComments" HeaderText="Staff Notes" />

                                                <asp:BoundField DataField="Player" HeaderText="Player" />
                                                <asp:BoundField DataField="NumberClaimed" HeaderText="# Claimed" />

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
