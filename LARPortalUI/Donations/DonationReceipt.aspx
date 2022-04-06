<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="DonationReceipt.aspx.cs" Inherits="LarpPortal.Donations.DonationReceipt" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="DonationReceiptStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .max-width-300 {
            max-width: 300px;
        }
    </style>
</asp:Content>

<asp:Content ID="DonationReceiptScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="DonationReceiptBody" ContentPlaceHolderID="MainBody" runat="server">

    <asp:HiddenField ID="hidScollPosition" runat="server" Value="" />

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Receive Donations</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="form-inline">

                    <div class="col-xs-3">
                        <label for="<%= ddlDonationStatus.ClientID %>">Status: </label>
                        <asp:DropDownList ID="ddlDonationStatus" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" OnSelectedIndexChanged="ddlDonationStatus_SelectedIndexChanged" />
                    </div>
                    <div class="col-xs-3">
                        <label for="<%= ddlEvent.ClientID %>">Event: </label>
                        <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" />
                    </div>
                    <asp:Panel ID="pnlPlayerDropdown" runat="server" Visible="true">
                        <div class="col-xs-3">
                            <label for="<%= ddlPlayer.ClientID %>">Player: </label>
                            <asp:DropDownList ID="ddlPlayer" runat="server" CssClass="from-control autoWidth" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayer_SelectedIndexChanged" />
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </div>
        <div class="divide10"></div>

        <div class="divide10"></div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Donation Receipts</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblDonationReceipts" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="pre-scrollable style=”max-height: 80vh" id="divRegs" >
                                
                                <asp:GridView ID="gvRegistrations" runat="server" OnRowDataBound="gvRegistrations_RowDataBound" OnRowCommand="gvRegistrations_RowCommand"
                                    OnRowEditing="gvRegistrations_RowEditing" OnRowUpdating="gvRegistrations_RowUpdating" OnRowCancelingEdit="gvRegistrations_RowCancelingEdit"
                                    AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false" CssClass="table table-striped table-hover table-condensed"
                                    AllowSorting="true" OnSorting="gvRegistrations_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidDonationClaimID" runat="server" Value='<%# Eval("DonationClaimID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidCampaignCPOpportunityID" runat="server" Value='<%# Eval("CampaignCPOpportunityID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidPlayerCPAuditID" runat="server" Value='<%# Eval("PlayerCPAuditID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidEventID" runat="server" Value='<%# Eval("EventID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Event Date" ItemStyle-Wrap="true" SortExpression="EventDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEventDate" runat="server" Text='<%# Eval("EventDate", "{0:MM/dd/yyyy}") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Player" ItemStyle-Wrap="true" SortExpression="Player">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlayerName" runat="server" Text='<%# Eval("Player") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Item" ItemStyle-Wrap="true" SortExpression="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Claimed" ItemStyle-Wrap="true" SortExpression="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyClaimed" runat="server" Text='<%# Eval("Quantity", "{0:0.00}") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Prev Recd" ItemStyle-Wrap="true" SortExpression="QuantityAccepted">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrevRecd" runat="server" Text='<%# Eval("QuantityAccepted", "{0:0.00}") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Player Comments" ItemStyle-Wrap="true" SortExpression="Player Comments">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlayerComments" runat="server" Text='<%# Eval("PlayerComments") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Staff Comments" SortExpression="StaffComments">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbStaffComments" runat="server" Text='<%# Eval("StaffComments") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Button ID="btn_Receive" runat="server" Text="Receive" CommandName="Receive" 
                                                    visible='<%# AlreadyReceived((Decimal)Eval("QuantityAccepted")) %>'
                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidApprovedStatus" runat="server" />
    </div>

    <script>
        function setScrollValue() {
            var divObj = $get('divRegs');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (obj) obj.value = divObj.scrollTop;
        }

        function pageLoad() {
            var divObj = $get('divRegs');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (divObj) divObj.scrollTop = obj.value;
        }
    </script>
</asp:Content>
