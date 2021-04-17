<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="WaiversAndConsent.aspx.cs" Inherits="LarpPortal.Profile.WaiversAndConsent" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="WaiversAndConsentStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style type="text/css">
        .checkbox label {
            padding-left: 0px !important;
        }

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

<asp:Content ID="WaiversAndConsentScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModalMessage() {
            $('#ModalMessage').modal('show');
        }

        function DisagreeChecked(DisagreeCheckBox) {
            var cbxAgree = document.getElementById('<%= cbxAgree.ClientID %>');
            if (cbxAgree)
                if (DisagreeCheckBox.checked)
                    cbxAgree.checked = false;
        }

        function AgreeChecked(AgreeCheckBox) {
            var cbxDisagree = document.getElementById('<%= cbxDisagree.ClientID %>');
            if (cbxDisagree)
                if (AgreeCheckBox.checked)
                    cbxDisagree.checked = false;
        }

        function setSelectedValue(selectObj, valueToSet) {
            for (var i = 0; i < selectObj.options.length; i++) {
                if (selectObj.options[i].value == valueToSet) {
                    selectObj.options[i].selected = true;
                    return;
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="WaiversAndConsentBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Waivers &amp; Consents</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Waivers</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 125px; overflow-y: auto;">

                                    <%--                                    <div class="row" style="padding-left: 15px; padding-right: 15px; padding-bottom: 0px; margin-bottom: 0px; max-height: 125px; overflow-y: auto;">--%>
                                    <asp:GridView runat="server" ID="gvWaivers" AutoGenerateColumns="false" GridLines="None" DataKeyNames="PlayerWaiverID" OnRowCreated="gvWaivers_RowCreated" EnableViewState="false"
                                        CssClass="table table-striped table-hover table-condensed">
                                        <Columns>
                                            <asp:BoundField DataField="CampaignName" HeaderText="Campaign" />
                                            <asp:BoundField DataField="WaiverType" HeaderText="Waiver Type" />
                                            <asp:BoundField DataField="WaiverStatus" HeaderText="Status" />
                                            <asp:BoundField DataField="WaiverStatusDate" HeaderText="Status Date" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidPlayerWaiverID" runat="server" Value='<%# Eval("PlayerWaiverID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="col-xs-12" Font-Bold="true" Font-Size="XX-Large" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            There are no waivers you need to review.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-9">
                <div class="panel panel-default">
                    <div class="panel-heading">Waiver Text</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 150px; overflow-y: auto; overflow-x: hidden;">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:Label ID="lblWaiverText" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="col-xs-12 form-group" style="padding-left: 0px; margin-bottom: 0px;">
                    <input type="checkbox" name="cbxAgree" id="cbxAgree" runat="server" onclick="AgreeChecked(this);" />
                    <div class="btn-group col-xs-12">
                        <label for="<%= cbxAgree.ClientID%>" class="btn btn-default">
                            <span class="glyphicon glyphicon-ok"></span>
                            <span class="glyphicon glyphicon-unchecked"></span>
                        </label>
                        <label for="<%= cbxAgree.ClientID %>" class="btn btn-default active">
                            I Agree
                        </label>
                    </div>
                </div>
                <div class="col-xs-12 form-group" style="padding-left: 0px;">
                    <input type="checkbox" name="cbxDisagree" id="cbxDisagree" runat="server" onclick="DisagreeChecked(this);" />
                    <div class="btn-group col-xs-12">
                        <label for="<%= cbxDisagree.ClientID %>" class="btn btn-default">
                            <span class="glyphicon glyphicon-ok"></span>
                            <span class="glyphicon glyphicon-unchecked"></span>
                        </label>
                        <label for="<%= cbxDisagree.ClientID %>" class="btn btn-default active">
                            I do not Agree
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Player Comments</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:TextBox ID="tbWaiverComment" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
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
                <asp:Button ID="btnSaveComments" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveComments_Click" />
            </div>
        </div>
        <div class="modal fade in" id="ModalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title text-center">Player Medical Information</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblModalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer text-right">
                        <button type="button" data-dismiss="modal" class="btn btn-primary">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidPlayerWaiverID" runat="server" />
        <asp:HiddenField ID="hidRowSelected" runat="server" />
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

