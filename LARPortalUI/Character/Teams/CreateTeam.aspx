<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CreateTeam.aspx.cs" Inherits="LarpPortal.Character.Teams.CreateTeam" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CreateTeamsStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="CreateTeamsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    function openMessage() {
            $('#modalMessage').modal('show');
        }
</asp:Content>

<asp:Content ID="CreateTeamsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Create Team <Videos:PlayVideo runat="server" ID="playVideo" VideoID="5" /></h1>
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

        <div class="row">
            <div class="col-md-12">
                <div class="input-group">
                    <asp:TextBox ID="tbNewTeamName" runat="server" CssClass="form-control" />
                    <span class="input-group-btn">
                        <asp:Button ID="btnCreateTeam" runat="server" CssClass="btn btn-primary" Text="Create Team" OnClick="btnCreateTeam_Click" />
                    </span>
                </div>
            </div>
        </div>

        <div class="row" id="divAlreadyExists" runat="server">
            <div class="col-md-12">
                <asp:Label ID="lblAlreadyExists" runat="server" Text="* That team already exists" Font-Size="20px" Font-Bold="true" Font-Italic="true" CssClass="error text-danger" />
            </div>
        </div>


        <div class="divide30"></div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Current Teams</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-condensed">
                            <Columns>
                                <asp:BoundField DataField="TeamName" HeaderText="Team Name" />
                            </Columns>
                            <EmptyDataRowStyle ForeColor="Red" Font-Size="X-Large" />
                            <EmptyDataTemplate>
                                There are no teams for that campaign.
                            </EmptyDataTemplate>
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
                            <asp:Button ID="btnCloseMessage" runat="server" Text="Delete" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidNotificationEMail" runat="server" Value="" />
    <asp:HiddenField ID="hidCampaignID" runat="server" Value="" />
    <!-- /#page-wrapper -->
</asp:Content>

