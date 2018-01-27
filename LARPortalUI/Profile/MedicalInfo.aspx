<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="MedicalInfo.aspx.cs" Inherits="LarpPortal.Profile.MedicalInfo" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="MedicalInfoStyles" ContentPlaceHolderID="MainStyles" runat="Server">
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

<asp:Content ID="MedicalInfoScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModalMessage() {
            $('#ModalMessage').modal('show');
        }

        function openMedical(PlayerMedicalID, Condition, Medications, ShareInfo, PrintOnCard, StartDate, EndDate) {
            $('#modalMedical').modal('show');

            var tbCondition = document.getElementById('<%= tbCondition.ClientID %>');
            if (tbCondition) {
                tbCondition.value = Condition;
                tbCondition.focus();
            }

            var tbMedication = document.getElementById('<%= tbMedication.ClientID %>');
            if (tbMedication)
                tbMedication.value = Medications;

            var hidPlayerMedicalID = document.getElementById('<%= hidPlayerMedicalID.ClientID %>');
            if (hidPlayerMedicalID)
                hidPlayerMedicalID.value = PlayerMedicalID;

            var cbxShareWithStaff = document.getElementById('<%= cbxShareWithStaff.ClientID %>');
            if (cbxShareWithStaff) {
                cbxShareWithStaff.checked = false;
                if (ShareInfo == "True")
                    cbxShareWithStaff.checked = true;
            }

            var cbxPrintOnCard = document.getElementById('<%= cbxPrintOnCard.ClientID %>');
            if (cbxPrintOnCard) {
                cbxPrintOnCard.checked = false;
                if (PrintOnCard == "True")
                    cbxPrintOnCard.checked = true;
            }

            var tbMedicalStartDate = document.getElementById('<%= tbMedicalStartDate.ClientID %>');
            if (tbMedicalStartDate) {
                tbMedicalStartDate.value = "";
                if (StartDate)
                    tbMedicalStartDate.value = StartDate;
            }

            var tbMedicalEndDate = document.getElementById('<%= tbMedicalEndDate.ClientID %>');
            if (tbMedicalEndDate) {
                tbMedicalEndDate.value = "";
                if (EndDate)
                    tbMedicalEndDate.value = EndDate;
            }

            return false;
        }

        function openMedicalDelete(PlayerMedicalID, Condition, Medications) {
            $('#modalMedicalDelete').modal('show');

            var lblDeleteCondition = document.getElementById('lblDeleteCondition');
            if (lblDeleteCondition)
                lblDeleteCondition.innerText = Condition;

            var lblDeleteMedication = document.getElementById('lblDeleteMedication');
            if (lblDeleteMedication)
                lblDeleteMedication.innerText = Medications;

            var hidDeleteMedicalID = document.getElementById('<%= hidDeleteMedicalID.ClientID %>');
            if (hidDeleteMedicalID)
                hidDeleteMedicalID.value = PlayerMedicalID;

            return false;
        }

        function openLimitations(PlayerLimitID, Description, ShareInfo, PrintOnCard, StartDate, EndDate) {
            $('#modalLimitation').modal('show');

            var tbLimitation = document.getElementById('<%= tbLimitation.ClientID %>');
            if (tbLimitation) {
                tbLimitation.value = Description;
                tbLimitation.focus();
            }

            var hidPlayerLimitID = document.getElementById('<%= hidPlayerLimitID.ClientID %>');
            if (hidPlayerLimitID)
                hidPlayerLimitID.value = PlayerLimitID;

            var cbxLimitShareWithStaff = document.getElementById('<%= cbxLimitShareWithStaff.ClientID %>');
            if (cbxLimitShareWithStaff) {
                cbxLimitShareWithStaff.checked = false;
                if (ShareInfo == "True")
                    cbxLimitShareWithStaff.checked = true;
            }

            var cbxLimitPrintOnCard = document.getElementById('<%= cbxLimitPrintOnCard.ClientID %>');
            if (cbxLimitPrintOnCard) {
                cbxLimitPrintOnCard.checked = false;
                if (PrintOnCard == "True")
                    cbxLimitPrintOnCard.checked = true;
            }

            var tbLimitationStartDate = document.getElementById('<%= tbLimitationStartDate.ClientID %>');
            if (tbLimitationStartDate) {
                tbLimitationStartDate.value = "";
                if (StartDate)
                    tbLimitationStartDate.value = StartDate;
            }

            var tbLimitationEndDate = document.getElementById('<%= tbLimitationEndDate.ClientID %>');
            if (tbLimitationEndDate) {
                tbLimitationEndDate.value = "";
                if (EndDate)
                    tbLimitationEndDate.value = EndDate;
            }

            return false;
        }

        function openLimitationsDelete(PlayerLimitID, Description) {
            $('#modalLimitationsDelete').modal('show');

            var lblDeletePlayerLimitation = document.getElementById('lblDeletePlayerLimitation');
            if (lblDeletePlayerLimitation)
                lblDeletePlayerLimitation.innerText = Description;

            var hidDeleteLimitID = document.getElementById('<%= hidDeleteLimitID.ClientID %>');
            if (hidDeleteLimitID)
                hidDeleteLimitID.value = PlayerLimitID;

            return false;
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
<asp:Content ID="MedicalInfoBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Medical Info</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Medical Conditions</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView runat="server" ID="gvMedical" AutoGenerateColumns="false" GridLines="None" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                        CssClass="table table-striped table-hover table-condensed col-sm-12">
                                        <Columns>
                                            <asp:BoundField DataField="Description" HeaderText="Condition" />
                                            <asp:BoundField DataField="Medication" HeaderText="Medication" />
                                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />

                                            <asp:TemplateField HeaderText="Show Staff" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" ItemStyle-Width="30px" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <a href="#" data-toggle="tooltip" title="Should this be displayed to staff ?">
                                                        <asp:Image ID="imgDisplayShowStaff" runat="server"
                                                            ImageUrl='<%# Boolean.Parse(Eval("ShareInfo").ToString()) ? "../img/checkbox.png" : "../img/uncheckbox.png" %>' ToolTip="Should this be displayed to staff ?" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" ItemStyle-Width="40px">
                                                <HeaderTemplate>
                                                    <div class="text-center">
                                                        Print<br />
                                                        On Card
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <a href="#" data-toggle="tooltip" title="Should this print on the card ?">
                                                        <asp:Image ID="imgDisplayPrintOnCard" runat="server"
                                                            ImageUrl='<%# Boolean.Parse(Eval("PrintOnCard").ToString()) ? "../img/checkbox.png" : "../img/uncheckbox.png" %>' ToolTip="Should this be displayed to staff ?" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# Eval("JavaScriptEdit") %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# Eval("JavaScriptDelete") %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="text-right">
                                    <div class="text-right">
                                        <asp:Button ID="btnOpenMedical" runat="server" Text="Add Condition" CssClass="btn btn-primary"
                                            OnClientClick='openMedical(-1, "", "", "", ""); return false;' />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Limitations</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView runat="server" ID="gvLimitations" AutoGenerateColumns="false" GridLines="None"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                        CssClass="table table-striped table-hover table-condensed col-sm-12">
                                        <Columns>
                                            <asp:BoundField DataField="Description" HeaderText="Description" />
                                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:TemplateField HeaderText="Show Staff" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" ItemStyle-Width="30px" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <a href="#" data-toggle="tooltip" title="Should this be displayed to staff ?">
                                                        <asp:Image ID="imgDisplayShowStaff" runat="server"
                                                            ImageUrl='<%# Boolean.Parse(Eval("ShareInfo").ToString()) ? "../img/checkbox.png" : "../img/uncheckbox.png" %>' ToolTip="Should this be displayed to staff ?" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="60px" HeaderText="&nbsp;&nbsp;&nbsp;Print&nbsp; On Card" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <a href="#" data-toggle="tooltip" title="Should this be displayed to staff ?">
                                                        <asp:Image ID="imgDisplayPrintOnCard" runat="server"
                                                            ImageUrl='<%# Boolean.Parse(Eval("PrintOnCard").ToString()) ? "../img/checkbox.png" : "../img/uncheckbox.png" %>' ToolTip="Should this be displayed to staff ?" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# Eval("JavaScriptEdit") %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# Eval("JavaScriptDelete") %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="text-right">
                                    <div class="text-right">
                                        <asp:Button ID="btnAddLimitation" runat="server" Text="Add Limitation" CssClass="btn btn-primary"
                                            OnClientClick='openLimitations(-1, "", "", "", "", ""); return false;' />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Allergies</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container">
                                    <asp:TextBox ID="tbAllergies" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Comments</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container">
                                    <asp:TextBox ID="tbMedicalComments" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 text-right">
                <asp:Button ID="btnSaveComments" runat="server" Text="Save Allergies and Comments" CssClass="btn btn-primary" OnClick="btnSaveComments_Click" />
            </div>
        </div>
        <div id="push"></div>

        <div class="modal fade" id="ModalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Medical Information</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblModalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-primary">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalMedical" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Medical Information</h3>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="tbCondition">Medical Condition</label>
                            <asp:TextBox ID="tbCondition" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="tbMedication">Medication</label>
                            <asp:TextBox ID="tbMedication" runat="server" CssClass="form-control" />
                        </div>
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label for="<%= tbMedicalStartDate.ClientID %>">Start Date</label>
                                <asp:TextBox ID="tbMedicalStartDate" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label for="<%= tbMedicalEndDate.ClientID %>">End Date</label>
                                <asp:TextBox ID="tbMedicalEndDate" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="row" style="padding-left: 0px;">
                            <div class="col-xs-6 form-group" style="padding-left: 0px;">
                                <input type="checkbox" name="cbxShareWithStaff" id="cbxShareWithStaff" runat="server" />
                                <div class="btn-group col-xs-12">
                                    <label for="<%= cbxShareWithStaff.ClientID%>" class="btn btn-default">
                                        <span class="glyphicon glyphicon-ok"></span>
                                        <span class="glyphicon glyphicon-unchecked"></span>
                                    </label>
                                    <label for="<%= cbxShareWithStaff.ClientID%>" class="btn btn-default active">
                                        Show Staff
                                   
                                    </label>
                                </div>
                            </div>
                            <div class="col-xs-6 form-group" style="padding-left: 0px;">
                                <input type="checkbox" name="cbxPrintOnCard" id="cbxPrintOnCard" runat="server" />
                                <div class="btn-group col-xs-12">
                                    <label for="<%= cbxPrintOnCard.ClientID %>" class="btn btn-default">
                                        <span class="glyphicon glyphicon-ok"></span>
                                        <span class="glyphicon glyphicon-unchecked"></span>
                                    </label>
                                    <label for="<%= cbxPrintOnCard.ClientID %>" class="btn btn-default active">
                                        Print On Card
                                   
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCloseMedical" runat="server" CssClass="btn btn-primary" Text="Close" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidPlayerMedicalID" runat="server" />
                                <asp:Button ID="btnSaveMedical" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveMedical_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalMedicalDelete" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Medical Information - Delete</h3>
                    </div>

                    <div class="modal-body">
                        <div class="container-fluid">
                            <label class="col-xs-12">Medical Condition</label>
                            <span class="col-xs-12" id="lblDeleteCondition"></span>
                            <label class="col-xs-12" style="padding-top: 20px;">Medication</label>
                            <span class="col-xs-12" id="lblDeleteMedication"></span>
                            <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCancelDeleteMedical" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidDeleteMedicalID" runat="server" />
                                <asp:Button ID="btnDeleteMedical" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteMedical_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalLimitation" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Limitations</h3>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="tbLimitation">Player Limitation</label>
                            <asp:TextBox ID="tbLimitation" runat="server" CssClass="form-control" />
                        </div>

                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label for="<%= tbLimitationStartDate.ClientID %>">Start Date</label>
                                <asp:TextBox ID="tbLimitationStartDate" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label for="<%= tbLimitationEndDate.ClientID %>">End Date</label>
                                <asp:TextBox ID="tbLimitationEndDate" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="row" style="padding-left: 0px;">
                            <div class="col-xs-6 form-group" style="padding-left: 0px;">
                                <input type="checkbox" name="cbxLimitShareWithStaff" id="cbxLimitShareWithStaff" runat="server" />
                                <div class="btn-group col-xs-12">
                                    <label for="<%= cbxLimitShareWithStaff.ClientID%>" class="btn btn-default">
                                        <span class="glyphicon glyphicon-ok"></span>
                                        <span class="glyphicon glyphicon-unchecked"></span>
                                    </label>
                                    <label for="<%= cbxLimitShareWithStaff.ClientID%>" class="btn btn-default active">
                                        Show Staff
                                   
                                    </label>
                                </div>
                            </div>
                            <div class="col-xs-6 form-group" style="padding-left: 0px;">
                                <input type="checkbox" name="cbxLimitPrintOnCard" id="cbxLimitPrintOnCard" runat="server" />
                                <div class="btn-group col-xs-12">
                                    <label for="<%= cbxLimitPrintOnCard.ClientID %>" class="btn btn-default">
                                        <span class="glyphicon glyphicon-ok"></span>
                                        <span class="glyphicon glyphicon-unchecked"></span>
                                    </label>
                                    <label for="<%= cbxLimitPrintOnCard.ClientID %>" class="btn btn-default active">
                                        Print On Card
                                   
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCloseLimit" runat="server" CssClass="btn btn-primary" Text="Close" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidPlayerLimitID" runat="server" />
                                <asp:Button ID="btnSaveLimitation" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveLimit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalLimitationsDelete" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Limitation - Delete</h3>
                    </div>

                    <div class="modal-body">
                        <div class="container-fluid">
                            <label class="col-xs-12">PlayerLimitation</label>
                            <span class="col-xs-12" id="lblDeletePlayerLimitation"></span>
                            <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCancelDeleteLimit" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidDeleteLimitID" runat="server" />
                                <asp:Button ID="btnDeleteLimit" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteLimit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidPlayerProfileID" runat="server" />

        <%--        <script type="text/javascript">
            $(function () {
                $('#<%= tbMedicalStartDate.ClientID %>').data("datetimepicker");
                $('#<%= tbMedicalEndDate.ClientID %>').datepicker();
                $('#<%= tbLimitationStartDate.ClientID %>').datepicker();
                $('#<%= tbLimitationEndDate.ClientID %>').datepicker();
            });
        </script>--%>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

