<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="AssignRoles.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.AssignRoles" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="AssignRolesStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <link href="https://gitcdn.github.io/bootstrap-toggle/2.2.2/css/bootstrap-toggle.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="AssignRolesScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script src="https://gitcdn.github.io/bootstrap-toggle/2.2.2/js/bootstrap-toggle.min.js"></script>
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="AssignRolesBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Assign Roles</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-10 margin20">
                    <div class="input-group">
                        <div class="input-group noborder">
                            <label for="<%= ddlPlayerSelector.ClientID %>">Selected Player:</label>
                            <div class="row col-lg-12 row-flex-center">
                                <asp:DropDownList ID="ddlPlayerSelector" runat="server" AutoPostBack="true" Style="padding-right: 10px;" CssClass="form-control selectWidth"
                                    OnSelectedIndexChanged="ddlPlayerSelector_SelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="text-right">
                    <asp:Button ID="btnSaveTop" runat="server" Text="Save" CssClass="btn btn-lg btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Player Information</div>
                        <div class="panel-body">
                            <div class="container-fluid">
                                <div class="row">
<%--                                    <label for="<%= lblLoginName.ClientID %>" class="font-weight-bold">Profile: </label>
                                    <asp:Label ID="lblLoginName" runat="server" />--%>
                                    <label for="<%= lblPersonName.ClientID %>" class="font-weight-bold">Person Name: </label>
                                    <asp:Label ID="lblPersonName" runat="server" />
                                </div>

                                <div class="row" style="padding-top: 15px;">
                                    <asp:GridView ID="gvFullRoleList" runat="server" CssClass="table table-striped table-hover" GridLines="None"
                                        OnRowDataBound="gvFullRoleList_RowDataBound" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="PageDescription" HeaderText="Role" HtmlEncode="False" />
                                            <asp:BoundField DataField="RoleTier" HeaderText="Role Tier" />
                                            <asp:BoundField DataField="RoleID" HeaderText="RoleID" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input type="checkbox" data-toggle="toggle" data-size="small" runat="server" name="swHasRole" id="swHasRole" />
                                                    <%-- data-size="mini">--%>
                                                    <asp:HiddenField ID="hidRoleID" runat="server" Value='<%# Eval("RoleID") %>' />
                                                    <asp:HiddenField ID="hidPlayerHasRole" runat="server" Value='<%# Eval("PlayerHasRole") %>' />
                                                    <asp:HiddenField ID="hidCampaignPlayerRoleID" runat="server" Value='<%# Eval("CampaignPlayerRoleID") %>' />
                                                    <asp:HiddenField ID="hidCanAssign" runat="server" Value='<%# Eval("CanAssign") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

<%--                                <asp:GridView ID="gvRoleList" runat="server" CssClass="table table-striped table-bordered table-hover NarrowTable" RowStyle-CssClass="NarrowTable" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="RoleDesc" HeaderText="Roles You Have" ItemStyle-CssClass="NarrowTable" />
                                    </Columns>
                                </asp:GridView>--%>
                            </div>

<%--                            <asp:Repeater ID="rptRoles" runat="server">
                                <ItemTemplate>
                                    <%# Eval("DisplayGroup") %>
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>--%>

                            <div class="row">
                                <div class="col-md-12">
                                    <p class="text-right">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    </p>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="divide30"></div>
            <div id="push"></div>
        </div>
    </div>

    <div class="modal fade" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Info</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidCharacterID" runat="server" />
    <asp:HiddenField ID="hidActorDateProblems" runat="server" Value="" />
    <!-- /#page-wrapper -->
</asp:Content>
