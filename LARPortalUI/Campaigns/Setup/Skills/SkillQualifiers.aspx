<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="SkillQualifiers.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Skills.SkillQualifiers" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <link href="/font-awesome/css/font-awesome.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="CharSkillsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script src="https://gitcdn.github.io/bootstrap-toggle/2.2.2/js/bootstrap-toggle.min.js"></script>

    <script type="text/javascript">
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "A")
                __doPostBack("", "");
        }

        function openMessage() {
            $('#modalMessage').modal('show');
        }

        function infoTypeChanged() {
            var ddlAddInfoType = document.getElementById("<%= ddlAddInfoType.ClientID %>");
            var btnAddNewValue = document.getElementById("<%= btnAddNewValue.ClientID %>");
            if (ddlAddInfoType) {
                var strUser = ddlAddInfoType.options[ddlAddInfoType.selectedIndex].value;
                var divDropDown = document.getElementById("<%= divDropDown.ClientID %>");
                if (strUser == "2") {
                    btnAddNewValue
                    divDropDown.style.display = "block";
                    btnAddNewValue.style.direction = "block";
                }
                else {
                    divDropDown.style.display = "none";
                    btnAddNewValue.style.display = "none";
                }
            }
        }

        function toggleItem(itemID) {
            $("#" + itemID).toggle();
        }

        function displayAddNewValue() {
            var btnSave = document.getElementById("<%= btnSave.ClientID %>");
            $(btnSave).toggle();
            var btnAddNewItem = document.getElementById("<%= btnAddNewValue.ClientID %>");
            $(btnAddNewItem).toggle();
            $("#divAddNewValue").toggle();
            //            document.getElementById(tbNewValue.ClientID).focus();
        }

        function cancelAddNewValue() {
            var btnSave = document.getElementById("<%= btnSave.ClientID %>");
            $(btnSave).toggle();
            var btnAddNewItem = document.getElementById("<%= btnAddNewValue.ClientID %>");
            $(btnAddNewItem).toggle();
            $("#divAddNewValue").toggle();
        }
    </script>
    <script type="text/javascript" src="~/script/bootstrap-toggle.min.js"></script>
    <link href="~/css/bootstrap-toggle.min.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="CharSkillsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character Skill Qualifiers</h1>
<%--                    <span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Skill qualifiers allow you customize a skill with either free form text or by selecting from a drop down list.">
                        <a href="#">What are skill qualifiers ?</a></span><br />--%>
                    <br />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:UpdatePanel ID="upSkill" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-xs-6 col-lg-4">
                                <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Auto" Height="450px">
                                    <asp:TreeView ID="tvDisplaySkills" runat="server" SkipLinkText="" BorderColor="Black" BorderStyle="Solid" BorderWidth="0"
                                        ShowLines="false" OnSelectedNodeChanged="tvDisplaySkills_SelectedNodeChanged" Font-Underline="false" CssClass="form-control"
                                        ShowCheckBoxes="None"
                                        SelectedNodeStyle-Font-Bold="true" EnableClientScript="false" LeafNodeStyle-CssClass="TreeItems" NodeStyle-CssClass="TreeItems">
                                        <LevelStyles>
                                            <asp:TreeNodeStyle Font-Underline="false" />
                                        </LevelStyles>
                                    </asp:TreeView>
                                </asp:Panel>
                            </div>
                            <div class="col-xs-6 col-lg-4" runat="server" id="divSkillItems">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:HiddenField ID="hidMasterSkillID" runat="server" />
                                        <label for="<%= lblSkillName.ClientID %>">Skill Name</label>
                                        <asp:Label ID="lblSkillName" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label for="<%= ddlAddInfoType.ClientID %>">Skill Qualifier Type</label>
                                        <asp:DropDownList ID="ddlAddInfoType" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="None" Value="0" Selected="True" />
                                            <asp:ListItem Text="TextBox" Value="1" />
                                            <asp:ListItem Text="Drop Down" Value="2" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-12 SliderBoxBorder">
                                        <div class="SliderSwitch">
                                            <label>
                                                <input type="checkbox" data-toggle="toggle" data-size="small" name="chkAllowChanges" id="chkAllowChanges" runat="server" />
                                                Allow changes after initial save</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-xs-12">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="row" runat="server" id="divDropDown">
                                    <div class="col-xs-12">
                                        <asp:GridView ID="gvDropDownItems" runat="server" GridLines="None" AutoGenerateColumns="false"
                                            OnRowDataBound="gvDropDownItems_RowDataBound" OnRowCommand="gvDropDownItems_RowCommand" OnRowDeleting="gvDropDownItems_RowDeleting"
                                            CssClass="table table-striped table-hover table-condensed" DataKeyNames="AddInfoValuesID">
                                            <RowStyle VerticalAlign="Middle" />
                                            <EmptyDataTemplate>There are no values defined.</EmptyDataTemplate>
                                            <EmptyDataRowStyle CssClass="text-center" />
                                            <Columns>
                                                <asp:BoundField DataField="DisplayValue" HeaderText="Value" ItemStyle-HorizontalAlign="Left" />
                                                <asp:ButtonField ButtonType="Link" Text="<i aria-hidden='true' class='fa fa-arrow-circle-up'></i>"
                                                    CommandName="Up" ControlStyle-CssClass="btn default btn-lg no-padding" ItemStyle-Width="20" />
                                                <asp:ButtonField ButtonType="Link" Text="<i aria-hidden='true' class='fa fa-arrow-circle-down'></i>"
                                                    CommandName="Down" ControlStyle-CssClass="btn default btn-lg no-padding" ItemStyle-Width="20" />
                                                <asp:ButtonField ButtonType="Link" Text="<i aria-hidden='true' class='fa fa-trash'></i>"
                                                    CommandName="Delete" ControlStyle-CssClass="btn default btn-lg no-padding" ItemStyle-Width="20" />
                                            </Columns>
                                        </asp:GridView>
                                        &nbsp;
                                    </div>
                                    <div class="col-xs-12" id="divAddNewValue" style="display: none;">
                                        <label for="<%= tbNewValue.ClientID %>">New Value:</label>
                                        <asp:TextBox ID="tbNewValue" runat="server" CssClass="form-control col-xs-8" />
                                        <p class="text-right" style="padding-top: 40px;">
                                            <asp:Button ID="btnNewValue" runat="server" CssClass="btn btn-primary" Text="Save New Value" OnClick="btnNewValue_Click" />
                                            <asp:Button ID="btnNewCancel" runat="server" CssClass="btn btn-danger" Text="Cancel New Value" OnClientClick="cancelAddNewValue(); return false;" />
                                        </p>
                                    </div>
                                    <div class="col-xs-12">
                                        <p class="text-right">
                                            <asp:HiddenField ID="hidNumItems" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <p class="col-xs-6">
                                            <asp:Button ID="btnAddNewValue" runat="server" CssClass="btn btn-primary" Text="Add New Item" OnClientClick="displayAddNewValue(); return false;" />
                                        </p>
                                        <p class="col-xs-6 text-right">
                                            <asp:HiddenField ID="hidCampaignSkillsID" runat="server" />
                                            <asp:HiddenField ID="hidCampaignSkillNodeID" runat="server" />
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save The Skill" OnClick="btnSave_Click" />
                                        </p>
                                    </div>
                                    <div class="modal fade in" id="modalMessage" role="dialog">
                            <div class="modal-dialog modal-lg">
                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h3>Campaign Skill Qualifier</h3>
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
                            <%--                        </div>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
