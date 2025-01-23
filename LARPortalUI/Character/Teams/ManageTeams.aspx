<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="ManageTeams.aspx.cs" Inherits="LarpPortal.Character.Teams.ManageTeams" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="ManageTeamsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="ManageTeamsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function closeMessage() {
            $('#modalMessage').hide();
        }

        function openError() {
            $('#modalError').modal('show');
        }
        function closeError() {
            $('#modelError').hide();
        }
    </script>
</asp:Content>

<asp:Content ID="ManageTeamsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Manage Teams <Videos:PlayVideo runat="server" ID="playVideo" VideoID="5" /></h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-9 margin20">
                <div class="input-group">
                    <CharSelector:Select ID="oCharSelect" runat="server" />
                </div>
            </div>
            <div class="col-sm-3 text-right">
                <asp:Button ID="btnTopSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <b>Team: </b>
                <asp:DropDownList ID="ddlTeams" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlTeams_SelectedIndexChanged" />
                <asp:Label ID="lblTeamName" runat="server" Visible="false" />
            </div>
        </div>

        <div class="divide30"></div>
        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Available Members</div>
                    <div class="panel-body" style="max-height: 400px; overflow-y: auto;" id="divAvailable">
                        <asp:GridView ID="gvAvailable" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-condensed" GridLines="None"
                            OnRowCommand="gvAvailable_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="CharacterAKA" HeaderText="Character" />
                                <asp:BoundField DataField="Message" HeaderText="" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-CssClass="text-right">
                                    <ItemTemplate>
                                        <asp:Button ID="btnInvite" runat="server" CssClass="btn btn-primary btn-sm" Text="Invite" Width="100px"
                                            Visible='<%# Eval("DisplayInvite") == "1" %>' CommandArgument='<%# Eval("CharacterID") %>' CommandName="InviteChar" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnDeny" runat="server" CssClass="btn btn-primary btn-sm" Text="Deny" Width="100px"
                                            Visible='<%# Eval("DisplayApprove") == "1" %>' CommandArgument='<%# Eval("CharacterID") %>' CommandName="DenyChar" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-primary btn-sm" Text="Approve" Width="100px"
                                            Visible='<%# Eval("DisplayApprove") == "1" %>' CommandArgument='<%# Eval("CharacterID") %>' CommandName="ApproveChar" OnClientClick="setScrollValue();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Current Members</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-condensed" GridLines="none"
                            OnRowCommand="gvAvailable_RowCommand" RowStyle-CssClass="thNoBorder tdNoBorder" HeaderStyle-CssClass="thNoBorder tdNoBorder">
                            <Columns>
                                <asp:BoundField DataField="CharacterAKA" HeaderText="Character" ItemStyle-CssClass="col-lg-12" />
                                <asp:TemplateField HeaderText="Approver" ItemStyle-Width="80" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBoxApprover" runat="server" Checked='<%# Eval("Approval") %>' AutoPostBack="true" OnCheckedChanged="chkBoxApprover_CheckedChanged"
                                            Enabled='<%# Eval("DisplayCancelRevoke") == "0" %>' />
                                        <asp:HiddenField ID="hidCharacterID" runat="server" Value='<%# Eval("CharacterID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Message" HeaderText="" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="col-sm-12" ItemStyle-Wrap="false" />
                                <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-CssClass="text-right">
                                    <ItemTemplate>
                                        <asp:Button ID="btnAccept" runat="server" CssClass="btn btn-primary btn-sm" Text="Accept" Width="100px"
                                            Visible='<%# Eval("DisplayAccept") == "1" %>' CommandArgument='<%# Eval("CharacterID") %>' CommandName="ApproveChar" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnDeny" runat="server" CssClass="btn btn-primary btn-sm" Text="Deny" Width="100px"
                                            Visible='<%# Eval("DisplayAccept") == "1" %>' CommandArgument='<%# Eval("CharacterID") %>' CommandName="DenyChar" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnRevoke" runat="server" CssClass="btn btn-primary btn-sm" Text="Revoke" Width="100px"
                                            Visible='<%# Eval("DisplayRevoke") == "1" %>' CommandArgument='<%# Eval("CharacterID") %>' CommandName="RevokeChar" OnClientClick="setScrollValue();" />
                                        <asp:Button ID="btnRevokeDisabled" runat="server" CssClass="btn btn-default" BackColor="Gray" Width="100px" ToolTip="You must have at least one Approver."
                                            Text="Revoke" Visible='<%# Eval("DisplayCancelRevoke") == "1" %>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 text-right">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </div>
        </div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

    <asp:HiddenField ID="hidNotificationEMail" runat="server" Value="" />
    <asp:HiddenField ID="hidCampaignID" runat="server" Value="" />
    <asp:HiddenField ID="hidTeamID" runat="server" Value="" />
    <asp:HiddenField ID="hidScollPosition" runat="server" Value="" />

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3>Manage Teams</h3>
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

    <script>
        function setScrollValue() {
            var divObj = $get('divAvailable');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (obj) obj.value = divObj.scrollTop;
        }

        function pageLoad() {
            var divObj = $get('divAvailable');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (divObj) divObj.scrollTop = obj.value;
        }

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })

        //$(document).ready(function () {
        //    $('[data-toggle="tooltip"]').tooltip();
        //});

        //$('[data-toggle="popover"]').popover({
        //    placement: 'top',
        //    trigger: 'hover'
        //});

        $(document).ready(function () {
            $('.RevokeDisabled').tooltip({
                title: "You cannot revoke the only approver. Make another person an approver and you will be able to revoke this character.",
                html: true, offset: '0 0'
            });
        });
    </script>
</asp:Content>
