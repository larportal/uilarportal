<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="MemberPointsView.aspx.cs" Inherits="LarpPortal.Points.MemberPointsView" %>

<asp:Content ID="MemberPointsStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="MemberPointsListScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="MemberPointsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>View Character Points</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <asp:MultiView ID="mvPointList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPointList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Total Points</div>
                            <div class="panel-body">
                                <div style="max-height: 500px; overflow-y: auto;">
                                    <asp:GridView ID="gvPointsList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvPointsList_RowCommand" GridLines="None"
                                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Outset" BorderWidth="1"
                                        OnRowDataBound="gvPointsList_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ReceiptDate" HeaderText="Earn Date" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="FullDescription" HeaderText="Type" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="AdditionalNotes" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="CPAmount" HeaderText="Points" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="StatusName" HeaderText="Status" ItemStyle-Wrap="true" HeaderStyle-Wrap="true" />
                                            <asp:BoundField DataField="CPApprovedDate" HeaderText="Spend Date" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="RecvFromCampaign" HeaderText="Earned At" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="OwningPlayer" HeaderText="Earned By" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="ReceivingCampaign" HeaderText="Spent At" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="Character" HeaderText="Spent On" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="ReceivingPlayer" HeaderText="Transfer To" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="CPApprovedDate" HeaderText="Approved" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:TemplateField HeaderText="Apply To">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlCharacters" runat="server" Visible="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnApplyBanked" runat="server" Visible="true" CommandName="ApplyBanked" Text="Apply" CssClass="btn btn-primary"
                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidSentToCampaignPlayerID" runat="server" Value='<%# Eval("SentToCampaignPlayerID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidPlayerCPAuditID" runat="server" Value='<%# Eval("PlayerCPAuditID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidTotalCharacterCap" runat="server" Value='<%# Eval("TotalCharacterCap") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwPointlessList" runat="server">
                        <p>
                            <strong>You have not earned any points for this campaign.
                            <asp:Label ID="lblCharacterName" runat="server" />.
                            </strong>
                        </p>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>

        <div class="modal fade in" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Points</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblmodalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>

        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
