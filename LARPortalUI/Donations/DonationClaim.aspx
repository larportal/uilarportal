<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="DonationClaim.aspx.cs" Inherits="LarpPortal.Donations.DonationClaim" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="DonationClaimStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>

<asp:Content ID="DonationClaimScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
            return false;
        }
    </script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript">

        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus.png");
        });

        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus.png");
            $(this).closest("tr").next().remove();
        });

        $(".expandall").live("click", function () {
            $("[src*=plus]").trigger("click");
        })

        $(".closeall").live("click", function () {
            $("[src*=minus]").trigger("click");
        })

    </script>

</asp:Content>

<asp:Content ID="DonationClaimBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Donations</h1>
                </div>
            </div>
        </div>

        <asp:MultiView ID="mvDonations" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwDonations" runat="server">
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
                            <asp:Panel ID="pnlPlayerDropdown" runat="server" Visible="false">
                                <div class="col-xs-3">
                                    <label for="<%= ddlPlayer.ClientID %>">Player: </label>
                                    <asp:DropDownList ID="ddlPlayer" runat="server" CssClass="from-control autoWidth" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayer_SelectedIndexChanged" />
                                </div>
                            </asp:Panel>
                            <div class="col-xs-3">
                                <input type="button" class="expandall" value="Expand All" />
                                <input type="button" class="closeall" value="Close All" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <asp:HiddenField ID="hidAllowPlayerToPlayerPoints" runat="server" />
                    <asp:HiddenField ID="hidShowDonationClaims" runat="server" />
                    <asp:HiddenField ID="hidDefaultAwardWhen" runat="server" />
                    <asp:HiddenField ID="hidMaxPointsPerEvent" runat="server" />
                    <asp:HiddenField ID="hidMaxItemsPerEvent" runat="server" />
                    <asp:HiddenField ID="hidCountTransfersAgainstMax" runat="server" />
                    <asp:HiddenField ID="hidCPOpportunityID" runat="server" />
                    <asp:HiddenField ID="hidCampaignCPOpportunityDefaultID" runat="server" />
                </div>

                <div class="margin10"></div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Donation List</div>
                            <div class="panel-body">
                                <div class="panel-container">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div class="pre-scrollable">
                                                <asp:GridView ID="gvDonationList" runat="server" AutoGenerateColumns="false"
                                                    OnRowCommand="gvDonationList_RowCommand" DataKeyNames="DonationID" GridLines="None"
                                                    CssClass="table table-striped table-hover table-condensed" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="1" Width="99%" OnRowDataBound="OnRowDataBound"
                                                    EnableModeValidation="true">
                                                    <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="24pt" />
                                                    <EmptyDataTemplate>
                                                        There are no Donations that meet your criteria.
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <img src="images/plus.png" alt="" style="cursor: pointer" />
                                                                <asp:Panel ID="pnlDetails" runat="server" Style="display: none">
                                                                    <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false"
                                                                        GridLines="None" CssClass="table table-striped table-hover table-condensed"
                                                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="99%"
                                                                        ShowHeader="false">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PlayerDonationText" HeaderText="" ItemStyle-Wrap="true" HeaderStyle-Wrap="true" />
                                                                            <asp:BoundField DataField="PlayerComments" HeaderText="" ItemStyle-Wrap="true" HeaderStyle-Wrap="true" />
                                                                            <asp:BoundField DataField="Accepted" HeaderText="" ItemStyle-Wrap="true" HeaderStyle-Wrap="true" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="EventDate" HeaderText="Event Date" ItemStyle-Wrap="true" DataFormatString="{0: MM/dd/yyyy}" />
                                                        <asp:BoundField DataField="Description" HeaderText="Item(*=Recurring)" ItemStyle-Wrap="true" />
                                                        <asp:BoundField DataField="DisplayWorth" HeaderText="Value" ItemStyle-Wrap="true" />
                                                        <asp:BoundField DataField="DisplayNeeded" HeaderText="Needed" ItemStyle-Wrap="true" />
                                                        <asp:BoundField DataField="DisplayReceived" HeaderText="Received" ItemStyle-Wrap="true" />
                                                        <asp:BoundField DataField="DonationComments" HeaderText="Comments" ItemStyle-Wrap="true" />
                                                        <asp:BoundField DataField="URLLink" HeaderText="Web Link" HtmlEncode="false" DataFormatString="<a target='_blank' href='{0}'>Go to site</a>" />
                                                        <asp:BoundField DataField="ShipToAddress" HeaderText="Ship To" ItemStyle-Wrap="true" />
                                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidDonationID" runat="server" Value='<%# Eval("DonationID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidWorth" runat="server" Value='<%# Eval("Worth") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidCampaignSkillPoolID" runat="server" Value='<%# Eval("CampaignSkillPoolID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidDefaultPool" runat="server" Value='<%# Eval("DefaultPool") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnDonateItem" runat="server" Visible="true" CommandName="DonateItem" Text="Donate" CssClass="vtn btn-primary" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
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
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
