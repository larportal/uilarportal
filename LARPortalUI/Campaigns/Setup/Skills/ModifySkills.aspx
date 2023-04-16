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
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                </p>
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
