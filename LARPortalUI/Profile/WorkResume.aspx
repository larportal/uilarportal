<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="WorkResume.aspx.cs" Inherits="LarpPortal.Profile.WorkResume" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="WorkResumeStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="WorkResumeScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModalMessage() {
            $('#ModalMessage').modal('show');
        }

        function openPlayerSkill(PlayerResumeID, SkillName, SkillLevel, PlayerComments) {
            $('#modalPlayerSkill').modal('show');

            var tbSkillName = document.getElementById('<%= tbSkillName.ClientID %>');
            if (tbSkillName) {
                tbSkillName.value = SkillName;
                tbSkillName.focus();
            }

            var hidPlayerResumeID = document.getElementById('<%= hidPlayerResumeID.ClientID %>');
            if (hidPlayerResumeID)
                hidPlayerResumeID.value = PlayerResumeID;

            var ddlSkillLevel = document.getElementById('<%= ddlSkillLevel.ClientID %>');
            if (ddlSkillLevel)
                setSelectedValue(ddlSkillLevel, SkillLevel);

            var tbSkillComments = document.getElementById('<%= tbSkillComments.ClientID %>');
            if (tbSkillComments)
                tbSkillComments.value = PlayerComments;

            return false;
        }

        function openDeletePlayerSkill(PlayerResumeID, SkillName, SkillLevel, PlayerComments) {
            $('#modalDeletePlayerSkill').modal('show');

            var lblDeletePlayerSkillName = document.getElementById('lblDeletePlayerSkillName');
            if (lblDeletePlayerSkillName)
                lblDeletePlayerSkillName.innerText = SkillName;

            var lblDeletePlayerSkillLevel = document.getElementById('lblDeletePlayerSkillLevel');
            if (lblDeletePlayerSkillLevel)
                lblDeletePlayerSkillLevel.innerText = SkillLevel;

            var lblDeletePlayerSkillComments = document.getElementById('lblDeletePlayerSkillComments');
            if (lblDeletePlayerSkillComments)
                lblDeletePlayerSkillComments.innerText = PlayerComments;

            var hidDeleteSkillID = document.getElementById('<%= hidDeleteSkillID.ClientID %>');
            if (hidDeleteSkillID)
                hidDeleteSkillID.value = PlayerResumeID;

            return false;
        }

        function openAffiliation(PlayerAffilID, AffilName, AffilRole, PlayerComments) {
            $('#modalPlayerAffil').modal('show');

            var tbAffiliationName = document.getElementById('<%= tbAffiliationName.ClientID %>');
            if (tbAffiliationName) {
                tbAffiliationName.value = AffilName;
                tbAffiliationName.focus();
            }

            var hidPlayerAffilID = document.getElementById('<%= hidPlayerAffilID.ClientID %>');
            if (hidPlayerAffilID)
                hidPlayerAffilID.value = PlayerAffilID;

            var tbAffiliationRole = document.getElementById('<%= tbAffiliationRole.ClientID %>');
            if (tbAffiliationRole)
                tbAffiliationRole.value = AffilRole;

            var tbAffiliationComments = document.getElementById('<%= tbAffiliationComments.ClientID %>');
            if (tbAffiliationComments)
                tbAffiliationComments.value = PlayerComments;

            return false;
        }

        function openDeleteAffiliation(PlayerAffilID, AffilName, AffilRole, PlayerComments) {
            $('#modalDeletePlayerAffil').modal('show');

            var lblDeletePlayerAffilName = document.getElementById('lblDeletePlayerAffilName');
            if (lblDeletePlayerAffilName)
                lblDeletePlayerAffilName.innerText = AffilName;

            var lblDeletePlayerAffilRole = document.getElementById('lblDeletePlayerAffilRole');
            if (lblDeletePlayerAffilRole)
                lblDeletePlayerAffilRole.innerText = AffilRole;

            var lblDeletePlayerAffilComments = document.getElementById('lblDeletePlayerAffilComments');
            if (lblDeletePlayerAffilComments)
                lblDeletePlayerAffilComments.innerText = PlayerComments;

            var hidDeleteAffilID = document.getElementById('<%= hidDeleteAffilID.ClientID %>');
            if (hidDeleteAffilID)
                hidDeleteAffilID.value = PlayerAffilID;

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
<asp:Content ID="WorkResumeBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Work Resume</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Profession Skills</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView runat="server" ID="gvSkills" AutoGenerateColumns="false" GridLines="None"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                        CssClass="table table-striped table-hover table-condensed">
                                        <Columns>
                                            <asp:BoundField DataField="SkillName" HeaderText="Professional Skill" />
                                            <asp:BoundField DataField="SkillLevel" HeaderText="Skill Level" />
                                            <asp:BoundField DataField="PlayerComments" HeaderText="Comments" />
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# string.Format("return openPlayerSkill({0}, \"{1}\", \"{2}\", \"{3}\"); return false;",
                                                            Eval("PlayerSkillID"), Eval("SkillName"), Eval("SkillLevel"), Eval("PlayerComments")) %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# string.Format("return openDeletePlayerSkill({0}, \"{1}\", \"{2}\", \"{3}\"); return false;",
                                                            Eval("PlayerSkillID"), Eval("SkillName"), Eval("SkillLevel"), Eval("PlayerComments")) %>'>
                                                        <i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i>
                                                    </asp:LinkButton>
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
                                    <asp:Button ID="btnAddResumeItem" runat="server" Text="Add Skill" CssClass="btn btn-primary"
                                        OnClientClick="return openPlayerSkill(-1, '', '', ''); return false;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Professional Affiliations</div>
                    <div class="panel-body">
                        <div class="panel-container" style="max-height: 500px;">
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:GridView runat="server" ID="gvAffiliations" AutoGenerateColumns="false" GridLines="None"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                        CssClass="table table-striped table-hover table-condensed">
                                        <Columns>
                                            <asp:BoundField DataField="AffiliationName" HeaderText="Organization Name & Affiliation" />
                                            <asp:BoundField DataField="AffiliationRole" HeaderText="Role" />
                                            <asp:BoundField DataField="PlayerComments" HeaderText="Comments" />
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# string.Format("return openAffiliation({0}, \"{1}\", \"{2}\", \"{3}\");",
                                                            Eval("PlayerAffiliationID"), Eval("AffiliationName"), Eval("AffiliationRole"), Eval("PlayerComments")) %>'>
                                                        <i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# string.Format("return openDeleteAffiliation({0}, \"{1}\", \"{2}\", \"{3}\");",
                                                            Eval("PlayerAffiliationID"), Eval("AffiliationName"), Eval("AffiliationRole"), Eval("PlayerComments")) %>'>
                                                        <i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 text-right">
                                    <asp:Button ID="btnAddAffiliation" runat="server" Text="Add Affiliation" CssClass="btn btn-primary"
                                        OnClientClick="return openAffiliation(-1, '', '', '');" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade in" id="ModalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Resume</h3>
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

        <div class="modal fade in" id="modalPlayerSkill" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Professional Skill</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="form-group">
                                        <div class="col-xs-6">
                                            <label for="tbSkillName">Skill Name:</label>
                                            <asp:TextBox ID="tbSkillName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="col-xs-6">
                                            <label for="ddlSkillLevel">Skill Level:</label>
                                            <asp:DropDownList ID="ddlSkillLevel" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="Beginner" Text="Beginner" />
                                                <asp:ListItem Value="Intermediate" Text="Intermediate" />
                                                <asp:ListItem Value="Expert" Text="Expert" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <label for="tbSkillComments">Comments:</label>
                                            <asp:TextBox ID="tbSkillComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-6 text-left">
                                        <asp:Button ID="btnClosePlayerSkill" runat="server" CssClass="btn btn-primary" Text="Close" />
                                    </div>
                                    <div class="col-sm-6 text-right">
                                        <asp:HiddenField ID="hidPlayerResumeID" runat="server" />
                                        <asp:Button ID="btnSaveSkill" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveSkill_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade in" id="modalDeletePlayerSkill" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Professional Skill - Delete</h3>
                    </div>

                    <div class="modal-body">
                        <div class="container-fluid" style="margin-left: -30px; margin-right: -30px">
                            <label class="col-xs-12">Skill Name:</label>
                            <span class="col-xs-12" id="lblDeletePlayerSkillName"></span>
                            <label class="col-xs-12" style="padding-top: 20px;">Skill Level:</label>
                            <span class="col-xs-12" id="lblDeletePlayerSkillLevel"></span>
                            <label class="col-xs-12" style="padding-top: 20px;">Comments:</label>
                            <span class="col-xs-12" id="lblDeletePlayerSkillComments"></span>
                            <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCancelDeleteSkill" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidDeleteSkillID" runat="server" />
                                <asp:Button ID="btnDeleteSkill" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteSkill_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="modalPlayerAffil" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Professional Affiliation</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="form-group">
                                        <div class="col-xs-12">
                                            <label for="tbAffiliation">Affiliation:</label>
                                            <asp:TextBox ID="tbAffiliationName" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <label for="tbAffiliationRole">Role:</label>
                                    <asp:TextBox ID="tbAffiliationRole" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <label for="tbAffilComments">Comments:</label>
                                    <asp:TextBox ID="tbAffiliationComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCloseAffil" runat="server" CssClass="btn btn-primary" Text="Close" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidPlayerAffilID" runat="server" />
                                <asp:Button ID="btnSaveAffiliation" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveAffiliation_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="modalDeletePlayerAffil" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Professional Affiliation - Delete</h3>
                    </div>

                    <div class="modal-body">
                        <div class="container-fluid" style="margin-left: -30px; margin-right: -30px">
                            <label class="col-xs-12">Affiliation:</label>
                            <span class="col-xs-12" id="lblDeletePlayerAffilName"></span>
                            <label class="col-xs-12" style="padding-top: 20px;">Role:</label>
                            <span class="col-xs-12" id="lblDeletePlayerAffilRole"></span>
                            <label class="col-xs-12" style="padding-top: 20px;">Comments:</label>
                            <span class="col-xs-12" id="lblDeletePlayerAffilComments"></span>
                            <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCancelDeleteAffil" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidDeleteAffilID" runat="server" />
                                <asp:Button ID="btnDeleteAffil" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteAffil_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidPlayerProfileID" runat="server" />
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

