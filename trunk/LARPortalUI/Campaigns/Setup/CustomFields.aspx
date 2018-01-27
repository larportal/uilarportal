<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CustomFields.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.CustomFields" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsCustomFieldsStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .form-group input[type="checkbox"] {
            display: none;
        }

            .form-group input[type="checkbox"] + .btn-group > label span {
                width: 20px;
            }

                .form-group input[type="checkbox"] + .btn-group > label span:first-child {
                    display: none;
                }

                .form-group input[type="checkbox"] + .btn-group > label span:last-child {
                    display: inline-block;
                }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:first-child {
                display: inline-block;
            }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:last-child {
                display: none;
            }
    </style>
</asp:Content>

<asp:Content ID="CampaignsCustomFieldsScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CampaignsCustomFieldsBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Custom Fields<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <%--        <div class="row">
            <div class="col-xs-12">
                //<CampSelector:Select ID="oCampSelect" runat="server" />
            </div>
        </div>--%>
        <div class="margin20"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Custom Fields</div>
                    <div class="panel-body">
                        <div class="form-inline">
                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <label for="<%= tbCustomField1.ClientID %>">Field 1: </label>
                                    <asp:TextBox ID="tbCustomField1" runat="server" CssClass="form-control" />
                                    <div class="form-group">
                                        <input type="checkbox" name="cbxUseField1" id="cbxUseField1" runat="server" />
                                        <div class="btn-group">
                                            <label for="<%= cbxUseField1.ClientID%>" class="btn btn-default">
                                                <span class="glyphicon glyphicon-ok"></span>
                                                <span class="glyphicon glyphicon-unchecked"></span>
                                            </label>
                                            <label for="<%= cbxUseField1.ClientID%>" class="btn btn-default active">
                                                Use Field 1
                                   
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <label for="<%= tbCustomField2.ClientID %>">Field 2: </label>
                                    <asp:TextBox ID="tbCustomField2" runat="server" CssClass="form-control" />
                                    <div class="form-group">
                                        <input type="checkbox" name="cbxUseField2" id="cbxUseField2" runat="server" />
                                        <div class="btn-group">
                                            <label for="<%= cbxUseField2.ClientID%>" class="btn btn-default">
                                                <span class="glyphicon glyphicon-ok"></span>
                                                <span class="glyphicon glyphicon-unchecked"></span>
                                            </label>
                                            <label for="<%= cbxUseField2.ClientID%>" class="btn btn-default active">
                                                Use Field 2
                                   
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <label for="<%= tbCustomField3.ClientID %>">Field 3: </label>
                                    <asp:TextBox ID="tbCustomField3" runat="server" CssClass="form-control" />
                                    <div class="form-group">
                                        <input type="checkbox" name="cbxUseField3" id="cbxUseField3" runat="server" />
                                        <div class="btn-group">
                                            <label for="<%= cbxUseField3.ClientID%>" class="btn btn-default">
                                                <span class="glyphicon glyphicon-ok"></span>
                                                <span class="glyphicon glyphicon-unchecked"></span>
                                            </label>
                                            <label for="<%= cbxUseField3.ClientID%>" class="btn btn-default active">
                                                Use Field 3
                                   
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <label for="<%= tbCustomField4.ClientID %>">Field 4: </label>
                                    <asp:TextBox ID="tbCustomField4" runat="server" CssClass="form-control" />
                                    <div class="form-group">
                                        <input type="checkbox" name="cbxUseField4" id="cbxUseField4" runat="server" />
                                        <div class="btn-group">
                                            <label for="<%= cbxUseField4.ClientID%>" class="btn btn-default">
                                                <span class="glyphicon glyphicon-ok"></span>
                                                <span class="glyphicon glyphicon-unchecked"></span>
                                            </label>
                                            <label for="<%= cbxUseField4.ClientID%>" class="btn btn-default active">
                                                Use Field 4
                                   
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <label for="<%= tbCustomField5.ClientID %>">Field 5: </label>
                                    <asp:TextBox ID="tbCustomField5" runat="server" CssClass="form-control" />
                                    <div class="form-group">
                                        <input type="checkbox" name="cbxUseField5" id="cbxUseField5" runat="server" />
                                        <div class="btn-group">
                                            <label for="<%= cbxUseField5.ClientID%>" class="btn btn-default">
                                                <span class="glyphicon glyphicon-ok"></span>
                                                <span class="glyphicon glyphicon-unchecked"></span>
                                            </label>
                                            <label for="<%= cbxUseField5.ClientID%>" class="btn btn-default active">
                                                Use Field 5
                                   
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-right">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
        <div id="push"></div>

        <div class="modal fade" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Web Page Description</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblmodalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
