<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="SystemSecurity.aspx.cs" Inherits="LarpPortal.Profile.SystemSecurity" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="SystemSecurityStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>

<asp:Content ID="SystemSecurityScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="SystemSecurityBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Security</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">System Security Profile</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <h3>Personal Information</h3>
                            </div>
                        </div>
                        <%--      This is commented out because we may want it at a later date.
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= lblFirstName.ClientID %>">First Name:</label>
                                    <asp:Label ID="lblFirstName" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= lblMiddleName.ClientID %>">Middle Name:</label>
                                    <asp:Label ID="lblMiddleName" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= lblLastName.ClientID %>">Last Name:</label>
                                    <asp:Label ID="lblLastName" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= lblUserName.ClientID %>">Username:</label>
                                    <asp:Label ID="lblUserName" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= lblNickName.ClientID %>">Nick Name:</label>
                                    <asp:Label ID="lblNickName" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-4">&nbsp;</div>
                        </div>--%>
                        <div class="row">
                            <div class="col-md-4 col-sm-12">
                                <div class="form-group">
                                    <label for="<%= tbPassword.ClientID %>">Original Password:</label>
                                    <asp:TextBox ID="tbOrigPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12">
                                <div class="form-group">
                                    <label for="<%= tbPassword.ClientID %>">New Password:</label>
                                    <div class="input-group col-md-12">
                                        <asp:TextBox ID="tbPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                        <span class="input-group-addon">
                                            <asp:Label ID="lblPasswordReqs" runat="server" CssClass="fa fa-lg fa-question-circle-o" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12">
                                <div class="form-group">
                                    <label for="<%= tbPasswordConfirm %>">Confirm Password:</label>
                                    <div class="input-group col-md-12">
                                        <asp:TextBox ID="tbPasswordConfirm" runat="server" CssClass="form-control" TextMode="Password" />
                                        <span class="input-group-addon">
                                            <asp:Label ID="lblPasswordConfirmReqs" runat="server" CssClass="fa fa-lg fa-question-circle-o" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divErrorPasswords" runat="server">
                            <div class="col-xs-12">
                                <asp:Label ID="lblErrorPasswords" runat="server" CssClass="alert alert-danger text-center" />
                            </div>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-md-12">
                                <h3>Security Questions:</h3>
                                <p>Please select and answer one or more security questions for use in future email and password validation.</p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox ID="tbQuestion1" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox ID="tbAnswer1" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divErrorQuestion1" runat="server">
                            <div class="col-xs-12">
                                <asp:Label ID="lblErrorQuestion1" runat="server" CssClass="alert alert-danger text-center" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox ID="tbQuestion2" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox ID="tbAnswer2" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divErrorQuestion2" runat="server">
                            <div class="col-xs-12">
                                <asp:Label ID="lblErrorQuestion2" runat="server" CssClass="alert alert-danger text-center" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox ID="tbQuestion3" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox ID="tbAnswer3" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divErrorQuestion3" runat="server">
                            <div class="col-xs-12">
                                <asp:Label ID="lblErrorQuestion3" runat="server" CssClass="alert alert-danger text-center" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <p class="text-right">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                </p>
            </div>
        </div>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>

    <div class="modal fade" id="modalAddress" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">System Security</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="lblModalMessage" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="row">
                        <div class="col-sm-12 text-right">
                            <asp:Button ID="btnCloseModal" runat="server" Text="Save" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidOrigPassword" runat="server" />
    <!-- /#page-wrapper -->
</asp:Content>
