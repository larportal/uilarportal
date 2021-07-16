<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="ClaimDonation.aspx.cs" Inherits="LarpPortal.Donations.ClaimDonation" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="ClaimDonationStyles" ContentPlaceHolderID="MainStyles" runat="Server">

    <style>
        .NoShadow {
            border: 0px solid #ccc !important;
            border-radius: 4px !important;
            -webkit-box-shadow: inset 0 0px 0px rgba(0, 0, 0, .075) !important;
            box-shadow: inset 0 0px 0px rgba(0, 0, 0, .075) !important;
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s !important;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s !important;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s !important;
        }
    </style>
</asp:Content>

<asp:Content ID="ClaimDonationScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>

<asp:Content ID="ClaimDonationBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Claim Donation</h1>
                </div>
            </div>
        </div>

        <div class="margin10"></div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Donation List</div>
                    <div class="panel-body">
                        <div class="container-fluid">
                            <div style="margin-right: 10px;" runat="server" id="divEventList">
                                <div class="row">
                                    <div class="TableLabel col-sm-2">
                                        <asp:Label ID="lblEvent" runat="server" CssClass="form-control NoShadow">Event:</asp:Label>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:Label ID="lbEventName" runat="server" CssClass="form-control NoShadow"></asp:Label>
                                    </div>
                                </div>
                                <div class="row Padding5">
                                    <div class="TableLabel col-sm-2">
                                        <span class="form-control NoShadow">Item:</span>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:Label ID="lblItem" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row Padding5" id="divPCStaff" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Value:</span>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:Label ID="lblValue" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row Padding5" id="divNPC" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Receiving Player:</span>
                                    </div>
                                    <div class="col-sm-4 NoLeftPadding">
                                        <asp:DropDownList ID="ddlReceivingPlayer" runat="server" AutoPostBack="true"  Style="padding-right: 10px;" CssClass="form-control selectWidth" OnSelectedIndexChanged="ddlReceivingPlayer_SelectedIndexChanged" />
                                    </div>
                                    <div class="col-sm-6 NoLeftPadding">
                                        <asp:HiddenField ID="hid1" runat="server" />
                                    </div>
                                </div>
                                <div class="row Padding5" id="divQtyClaim" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Quantity:</span>
                                    </div>
                                    <div class="col-sm-2 NoLeftPadding">
                                        <asp:DropDownList ID="ddlQtyClaim" runat="server" AutoPostBack="true"  Style="padding-right: 10px;" CssClass="form-control selectWidth" OnSelectedIndexChanged="ddlQtyClaim_SelectedIndexChanged" />
                                    </div>
                                    <div class="col-sm-8 NoLeftPadding">
                                        <asp:HiddenField ID="hidS" runat="server" />
                                    </div>
                                </div>
                                <div class="row Padding5" style="padding-right: 0px;" id="divSendOther" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Comments To Staff:</span>
                                    </div>
                                    <div class="col-sm-10 NoLeftPadding">
                                        <asp:TextBox ID="tbCommentsToStaff" runat="server" CssClass="col-lg-12 col-sm-12 form-control" Style="padding: 0px;" MaxLength="500" Rows="4" TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-xs-6">
                                <asp:Button ID="btnReturn" runat="server" Text="Cancel and Return To Donations" CssClass="btn btn-primary" PostBackUrl="~/Donations/DonationClaim.aspx" />
                            </div>
                            <div class="col-xs-6 text-right">
                                <asp:Button ID="btnRegisterForDonation" runat="server" Text="Donate" CssClass="btn btn-primary" OnClick="btnRegisterForDonation_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <asp:Panel ID="pnlDonationHistory" runat="server" Visible="true">
                    <asp:Label ID="lblHeader" runat="server" Font-Size="24px" Style="font-weight: 500" Text="" />
                    <div class="panel panel-default">
                        <div class="panel-heading">Previously Claimed<asp:Label ID="lblGridHeader" runat="server"></asp:Label></div>
                        <div class="panel-body">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div style="max-height: 500px; overflow-y: auto;">
                                        <asp:GridView ID="gvPreviouslyClaimed" runat="server" AutoGenerateColumns="false" GridLines="None"
                                            CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                            OnRowDataBound="gvPreviouslyClaimed_RowDataBound">
                                            <RowStyle BackColor="White" />
                                            <Columns>
                                                <asp:BoundField DataField="DateClaimed" HeaderText="Date Claimed" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="false"
                                                    HeaderStyle-Wrap="false" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Qty Claimed" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                                <asp:BoundField DataField="AwardedToPlayer" HeaderText="Award To" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                                <asp:BoundField DataField="Accepted" HeaderText="Accepted" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                                <asp:BoundField DataField="PlayerComments" HeaderText="Comments" ItemStyle-Wrap="true"
                                                    HeaderStyle-Wrap="false" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>


        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

    <style type="text/css">
        .Padding5 {
            padding-top: 5px;
        }
    </style>

</asp:Content>

