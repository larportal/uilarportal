<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="JoinTeam.aspx.cs" Inherits="LarpPortal.Character.Teams.JoinTeam" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="JoinTeamStyles" runat="server" ContentPlaceHolderID="MainStyles">
    <style type="text/css">
        editArea {
            overflow: scroll;
            height: 425px;
        }

        div {
            border: 0px solid black;
        }

        .table th, .table td, .table {
            border-top: none !important;
            border-left: none !important;
            border-bottom: none !important;
            border-right: none !important;
        }

        .fixed-table-container {
            border: 0px;
            border-bottom: none !important;
        }
    </style>
</asp:Content>

<asp:Content ID="JoinTeamScripts" runat="server" ContentPlaceHolderID="MainScripts">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function closeMessage() {
            $('#modelMessage').hide();
        }

        function showNewAddendum() {
            document.getElementById('divNewAddendum').style.display = "block";
        }

        function hideNewAddendum() {
            document.getElementById('divNewAddendum').style.display = "none";
        }

    </script>
</asp:Content>

<asp:Content ID="JoinTeamBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Join a Team</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 margin20">
                <div class="input-group">
                    <CharSelector:Select ID="oCharSelect" runat="server" />
                </div>
            </div>
        </div>

        <div class="divide10"></div>

        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Teams</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvAvailable" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-condensed" ShowHeader="false"
                            OnRowCommand="gvAvailable_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="TeamName" HeaderText="Character" ItemStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="Message" HeaderText="" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-CssClass="text-right" ItemStyle-Width="250">
                                    <ItemTemplate>
                                        <asp:Button ID="btnJoin" runat="server" CssClass="btn btn-primary btn-sm" Text="Join Team"
                                            Visible='<%# Eval("Join").ToString() == "1" %>' CommandArgument='<%# Eval("TeamID") %>' CommandName="JoinTeam" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" Text="Accept"
                                            Visible='<%# Eval("Accept").ToString() == "1" %>' CommandArgument='<%# Eval("TeamID") %>' CommandName="AcceptInvite" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnDecline" runat="server" CssClass="btn btn-primary btn-sm" Text="Decline"
                                            Visible='<%# Eval("Accept").ToString() == "1" %>' CommandArgument='<%# Eval("TeamID") %>' CommandName="DeclineInvite" OnClientClick="setScrollValue();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Current Teams</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-condensed" GridLines="none"
                            ShowHeader="false" OnRowCommand="gvAvailable_RowCommand" Visible="true">
                            <Columns>
                                <asp:BoundField DataField="TeamName" HeaderText="Character" />
                                <asp:BoundField DataField="Message" HeaderText="" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField ItemStyle-Width="150" ItemStyle-HorizontalAlign="right">
                                    <ItemTemplate>
                                        <asp:Button ID="btnLeave" runat="server" CssClass="btn btn-primary btn-sm" Text="Leave"
                                            Visible='<%# Eval("DisplayLeave").ToString() == "1" %>' CommandArgument='<%# Eval("TeamID") %>' CommandName="LeaveTeam" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancel"
                                            Visible='<%# Eval("Requested").ToString() == "1" %>' CommandArgument='<%# Eval("TeamID") %>' CommandName="CancelRequest" OnClientClick="setScrollValue();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div id="push"></div>
    </div>

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3>Character Items</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 NoGutters text-right">
                            <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- /#page-wrapper -->
</asp:Content>
