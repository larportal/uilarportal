<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="ModifySkills.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Skills.ModifySkills" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharSkillsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "A")
                __doPostBack("", "");
        }

        //function Callback(result) {
        //    var outDiv = document.getElementById("outputDiv");
        //    outDiv.innerText = result;
        //}
        //function OnSuccessCall(response) {
        //    alert(response.d);
        //}

        //function OnErrorCall(response) {
        //    alert(response.status + " " + response.statusText);
        //}

        //function openMessageWithText(Msg) {
        //    parent.openMessageWithText(Msg);
        //}

        //function openErrorWithText(Msg) {
        //    parent.openErrorWithText(Msg);
        //}

        function openMessage() {
            $('#modalMessage').modal('show');
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
                <asp:UpdatePanel ID="upSkill" runat="server">
                    <ContentTemplate>
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
                                        <label for="<%= tbShortDescription.ClientID %>">Short Description</label>
                                        <asp:TextBox ID="tbShortDescription" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-xs-12">
                                        <label for="<%= tbLongDescription.ClientID %>">Long Description</label>
                                        <asp:TextBox ID="tbLongDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    </div>
                                    <div class="col-xs-12">
                                        <label for="<%= tbIncant.ClientID %>">Skill Incant</label>
                                        <asp:TextBox ID="tbIncant" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-lg-12">
                                        <div style="display: inline-block;">
                                            <asp:GridView ID="gvSkills" runat="server" CssClass="table table-striped table-hover table-condensed" GridLines="None"
                                                AutoGenerateColumns="false" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                                                <Columns>
                                                    <asp:BoundField DataField="DisplayValue" HeaderText="Skill Path In Tree" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Pool" HeaderText="Pool" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Cost" HeaderText="Cost" DataFormatString="{0:##0.0}" ItemStyle-HorizontalAlign="Right" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-4">
                                        <label for="<%= ddlPoolName.ClientID %>">Node Point Pool Name</label>
                                        <asp:DropDownList ID="ddlPoolName" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-xs-4" style="padding-left: 0px;">
                                        <label for="<%= tbSkillCost.ClientID %>">Node Skill Cost</label>
                                        <asp:TextBox ID="tbSkillCost" runat="server" CssClass="form-control" />
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

                        <div class="modal fade" id="modalMessage" role="dialog">
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
