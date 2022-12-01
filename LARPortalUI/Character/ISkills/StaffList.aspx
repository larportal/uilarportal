<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="StaffList.aspx.cs" Inherits="LarpPortal.Character.ISkills.StaffList" %>

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
                    <h1>In-between Game Skills</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <asp:MultiView ID="mvISkillList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwISKillList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">In-Between Skills</div>
                            <div class="panel-body">
                                <div style="max-height: 500px; overflow-y: auto;">
                                    <asp:GridView ID="gvISkillList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvISkillList_RowCommand" GridLines="None"
                                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Outset" BorderWidth="1">
                                        <Columns>
                                            <asp:BoundField DataField="CharName" HeaderText="Character Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="EventName" HeaderText="Event Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="BreadcrumbsText" HeaderText="Skill" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="CharName" HeaderText="Character Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="StatusName" HeaderText="User Status" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="StaffStatus" HeaderText="Staff Status" />
                                            <asp:TemplateField ItemStyle-CssClass="text-right" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnCommand" runat="server" CommandArgument='<%# Eval("ISkillRequestID") %>' CommandName='<%# Eval("ButtonText") %>Item'
                                                        Text='<%# Eval("ButtonText") %>' CssClass="btn btn-primary btn-xs LeftRightPadding10" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwNoISkills" runat="server">
                        <p>
                            <strong>You do not have any open requests for the campaign
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

