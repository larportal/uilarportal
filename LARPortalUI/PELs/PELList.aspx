<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="PELList.aspx.cs" Inherits="LarpPortal.PELs.PELList" %>
<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELListStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELListScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="PELListBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                <h1>PEL List <Videos:PlayVideo runat="server" ID="playVideo" VideoID="8" /></h1>
</div>            </div>
        </div>

        <div class="row">
            <div class="col-sm-8 margin20">
                <div class="form-inline">
<%--                    <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                </div>
            </div>
            <div class="col-xs-4 text-right">
                <asp:Button ID="btnMissedEvent" runat="server" CssClass="btn btn-primary" Text="Missing Event?" OnClick="btnMissedEvent_Click" />
            </div>
        </div>

<%--        <div class="row">
            <asp:Label ID="lblCharInfo" runat="server" />
        </div>--%>
        <div class="row">
            <div class="col-xs-12">
                <asp:MultiView ID="mvPELList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPELList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Event List</div>
                            <div class="panel-body">
                                <div style="max-height: 500px; overflow-y: auto;">
                                    <asp:GridView ID="gvPELList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvPELList_RowCommand" GridLines="None"
                                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Outset" BorderWidth="1">
                                        <Columns>
                                            <asp:BoundField DataField="CampaignName" HeaderText="Campaign" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="EventStartDate" HeaderText="Event Date" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="CharacterAKA" HeaderText="Character" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="EventName" HeaderText="Event Name" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="EventDescription" HeaderText="Event Description" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="PELStatus" HeaderText="Status" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:TemplateField ItemStyle-CssClass="text-right">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnAddendum" runat="server" CommandName="Addendum" Text="Add Addendum" CssClass="btn btn-primary"
                                                        Visible='<%# Eval("DisplayAddendum") %>' CommandArgument='<%# Eval("RegistrationID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="text-right">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnCommand" runat="server" CommandArgument='<%# Eval("RegistrationID") %>' CommandName='<%# Eval("ButtonText") %>Item'
                                                        Text='<%# Eval("ButtonText") %>' CssClass="btn btn-primary" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwNoPELs" runat="server">
                        <p>
                            <strong>You do not have any open PELs for the campaign
                            <asp:Label ID="lblCampaignName" runat="server" />.
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
                        <h3 class="modal-title text-center">PELs</h3>
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

