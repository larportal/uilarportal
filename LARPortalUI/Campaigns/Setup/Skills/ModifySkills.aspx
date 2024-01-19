<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="ModifySkills.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Skills.ModifySkills" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharSkillsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function postBackByObject() {
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                hidScrollPos.value = $get('<%=pnlTreeView.ClientID%>').scrollTop;
            }
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }

        function openMessage() {
            $('#modalMessage').modal('show');
        }

        function scrollTree() {
            var pnlTreeView = document.getElementById('<%=pnlTreeView.ClientID%>');
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                pnlTreeView.scrollTop = hidScrollPos.value;
            }
        }

        function openDeletePool(PoolName, PoolID, Cost) {
            $('#modalDeletePool').modal('show');

            var lblDeletePoolName = document.getElementById('<%= lblDeletePoolName.ClientID %>');
            lblDeletePoolName.innerHTML = PoolName;

            var lblDeleteCost = document.getElementById('<%= lblDeleteCost.ClientID %>');
            lblDeleteCost.innerHTML = Cost;

            var hidDeletePoolID = document.getElementById('<%= hidDeletePoolID.ClientID %>');
            hidDeletePoolID.value = PoolID;

            return false;
        }

        function closeDeletePool() {
            $('#modalDeletePool').hide();
        }

        function openAddPool() {
            $('#modalAddPool').modal('show');

            var ddlAddPoolName = document.getElementById('<%= ddlAddPoolName.ClientID %>');
            var hidAddPoolID = document.getElementById('<%= hidAddPoolID.ClientID %>');
            var tbAddPoolCost = document.getElementById('<%= tbAddPoolCost.ClientID %>');

            hidAddPoolID.value = -1;
            ddlAddPoolName.options[0].selected = true;

            return false;
        }

        function closeAddPool() {
            $('#modalAddPool').hide();
        }

        function openEditPool(PoolName, PoolID, Cost) {
            $('#modalEditPool').modal('show');

            var lblEditPoolName = document.getElementById('<%= lblEditPoolName.ClientID %>');
            var hidEditPoolID = document.getElementById('<%= hidEditPoolID.ClientID %>');
            var tbEditPoolCost = document.getElementById('<%= tbEditPoolCost.ClientID %>');

            lblEditPoolName.innerHTML = PoolName;
            hidEditPoolID.value = PoolID;
            tbEditPoolCost.value = Cost;

            return false;
        }

        function closeEditPool() {
            $('#modalEditPool').hide();
        }

    </script>
</asp:Content>

<asp:Content ID="CharSkillsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character Skills</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-xs-5 col-lg-3">
                        <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Auto" Height="450px">
                            <asp:TreeView ID="tvDisplaySkills" runat="server" SkipLinkText="" BorderColor="Black" BorderStyle="Solid" BorderWidth="0"
                                ShowLines="false" OnSelectedNodeChanged="tvDisplaySkills_SelectedNodeChanged" Font-Underline="false" CssClass="form-control"
                                SelectedNodeStyle-Font-Bold="true" EnableClientScript="false" LeafNodeStyle-CssClass="TreeItems" NodeStyle-CssClass="TreeItems">
                                <LevelStyles>
                                    <asp:TreeNodeStyle Font-Underline="false" />
                                </LevelStyles>
                            </asp:TreeView>
                        </asp:Panel>
                    </div>
                    <div class="col-xs-7 col-lg-9" runat="server" id="divSkillItems">
                        <span style="font-size: larger; font-weight: 700;">Master Skill Information</span>
                        <div class="col-xs-12" style="border: 1px solid black;">
                            <div class="row">
                                <div class="col-xs-4">
                                    <label for="<%= tbSkillName.ClientID %>">Skill Name</label>
                                    <asp:TextBox ID="tbSkillName" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbSkillName" runat="server" FilterType="Numbers, UppercaseLetters, LowercaseLetters, Custom"
                                        FilterMode="InvalidChars" InvalidChars="&lt;&gt;"
                                        TargetControlID="tbSkillName" />
                                </div>

                                <div class="col-xs-4">
                                    <label for="<%= ddlSkillType.ClientID %>">Skill Type</label>
                                    <asp:DropDownList ID="ddlSkillType" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-xs-12">
                                    <label for="<%= tbCardDescription.ClientID %>">Card Description</label>
                                    <asp:TextBox ID="tbCardDescription" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-xs-12">
                                    <label for="<%= CKShortDescription.ClientID %>">Short Description</label>
                                    <CKEditor:CKEditorControl ID="CKShortDescription" BasePath="/ckeditor/" runat="server" Height="100px"></CKEditor:CKEditorControl>
                                </div>
                                <div class="col-xs-12">
                                    <label for="<%= CKLongDescription.ClientID %>">Long Description</label>
                                    <CKEditor:CKEditorControl ID="CKLongDescription" BasePath="/ckeditor/" runat="server" Height="100px"></CKEditor:CKEditorControl>
                                </div>
                                <div class="col-xs-12">
                                    <label for="<%= tbIncant.ClientID %>">Skill Incant</label>
                                    <asp:TextBox ID="tbIncant" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <p class="text-right">
                                        <asp:HiddenField ID="hidCampaignSkillsID" runat="server" />
                                        <asp:HiddenField ID="hidCampaignSkillNodeID" runat="server" />
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Master Skill Info" OnClick="btnSave_Click" />
                                    </p>
                                </div>
                            </div>
                        </div>


                        <div class="col-xs-12" style="padding-top: 20px;">
                            <div class="row" style="font-size: larger; font-weight: 700;">Node Information</div>
                        </div>
                        <div class="col-xs-12" style="border: 1px solid black;">
                            <div class="row" style="">
                                <div class="col-xs-12">
                                    <label for="<%= swHasRole.ClientID %>">Open To All</label>
                                    <input type="checkbox" data-toggle="toggle" data-size="small" runat="server" name="swHasRole" id="swHasRole" />
                                    <label for="<%= swSuppressParent.ClientID %>">Suppress Parent</label>
                                    <input type="checkbox" data-toggle="toggle" data-size="small" runat="server" name="swSuppressParent" id="swSuppressParent" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    Skill Cost:
                                </div>
                            </div>
                            <div class="row">
                                <asp:GridView ID="gvPoolData" runat="server" AutoGenerateColumns="false" CssClass="table-border table-condensed table-striped col-xs-10"
                                    GridLines="None" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField DataField="CampaignSkillPoolID" />
                                        <asp:BoundField DataField="PoolDescription" HeaderText="Pool Description" />
                                        <asp:BoundField DataField="SkillCPCost" DataFormatString="{0:F2}" HeaderText="Cost" />
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="text-right" />
                                            <FooterStyle CssClass="text-right" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-default"
                                                    OnClientClick='<%# string.Format("return openEditPool(\"{0}\", {1}, \"{2:F2}\"); return false;", 
                                                                    Eval("PoolDescription"), Eval("CampaignSkillPoolID"), Eval("SkillCPCost")) %>' />
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-sm btn-default"
                                                    OnClientClick='<%# string.Format("return openDeletePool(\"{0}\", {1}, \"{2:F2}\"); return false;", 
                                                                    Eval("PoolDescription"), Eval("CampaignSkillPoolID"), Eval("SkillCPCost")) %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row">
                                <div class="col-xs-10 text-right no-gutters">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-sm btn-default" OnClientClick="return openAddPool(); return false;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-10">
                                    <p class="text-right">
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:Button ID="btn_SaveNode" runat="server" CssClass="btn btn-primary" Text="Save Node Info" OnClick="btn_SaveNode_Click" />
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade in" id="modalMessage" role="dialog">
                    <div class="modal-dialog modal-lg">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h3>Campaign Character Skills</h3>
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
            </div>
        </div>

        <div class="modal fade in" id="modalDeletePool" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title text-center">Delete Pool</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-sm-3 TableLabel">Pool Name:</div>
                            <div class="col-sm-9" style="padding-left: 0px;">
                                <asp:Label ID="lblDeletePoolName" runat="server" />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-sm-3 TableLabel">Cost:</div>
                            <div class="col-sm-9" style="padding-left: 0px;">
                                <asp:Label ID="lblDeleteCost" runat="server" />
                            </div>
                        </div>
                        <div class="row text-center" style="padding-top: 5px; font-size: larger; font-weight: 700;">
                            Are you sure you want to delete this cost?
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCancelDelete" OnClientClick="closeDelete(); return false;" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidDeletePoolID" runat="server" />
                                <asp:Button ID="btnDeleteCost" runat="server" Text="Delete Cost" CssClass="btn btn-danger" OnClick="btnDeleteCost_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="modal fade in" id="modalAddPool" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title text-center">Add Pool</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-sm-3 TableLabel">Pool Name:</div>
                            <div class="col-sm-9" style="padding-left: 0px;">
                                <asp:DropDownList ID="ddlAddPoolName" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-sm-3 TableLabel">Cost:</div>
                            <div class="col-sm-9" style="padding-left: 0px;">
                                <asp:TextBox ID="tbAddPoolCost" runat="server" CssClass="form-control" MaxLength="10" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnAddEditPoolCancel" OnClientClick="closeAdd(); return false;" runat="server" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidAddPoolID" runat="server" />
                                <asp:Button ID="btnAddEditPoolSave" runat="server" Text="Save Cost" CssClass="btn btn-default" OnClick="btnAddPoolSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="modal fade in" id="modalEditPool" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title text-center">Modify Pool</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-sm-3 TableLabel">Pool Name:</div>
                            <div class="col-sm-9" style="padding-left: 0px;">
                                <asp:Label ID="lblEditPoolName" runat="server" />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-sm-3 TableLabel">Cost:</div>
                            <div class="col-sm-9" style="padding-left: 0px;">
                                <asp:TextBox ID="tbEditPoolCost" runat="server" CssClass="form-control" MaxLength="10" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCancelEditPool" OnClientClick="closeEdit(); return false;" runat="server" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidEditPoolID" runat="server" />
                                <asp:Button ID="btnEditPoolSave" runat="server" Text="Save Cost" CssClass="btn btn-default" OnClick="btnEditPoolSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="push"></div>
    </div>
    <asp:HiddenField ID="hidScrollPos" runat="server" />

    <!-- /#page-wrapper -->

    <script type="text/javascript">
        // It is important to place this JavaScript code after ScriptManager1
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=pnlTreeView.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=pnlTreeView.ClientID%>').scrollLeft;
                yPos = $get('<%=pnlTreeView.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=pnlTreeView.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=pnlTreeView.ClientID%>').scrollLeft = xPos;
                $get('<%=pnlTreeView.ClientID%>').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>

</asp:Content>
