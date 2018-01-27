<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharSkills.aspx.cs" Inherits="LarpPortal.Character.CharSkills" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharSkillsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                var hiddenStatusFlag = document.getElementById('<%= hidScrollPos.ClientID%>');
                if (hiddenStatusFlag != null) {
                    hiddenStatusFlag.value = $get('<%=pnlTreeView.ClientID%>').scrollTop;
                }
                __doPostBack("", "");
            }
        }

        function ShowContent(d) {
            if (d.length < 1) { return; }
            var dd = document.getElementById(d);
            dd.style.display = "block";
        }

        function GetContent(d) {
            $.ajax({
                contentType: "application/json; charset=utf-8",
                data: "{'SkillNodeID':'" + d.toString() + "'}",
                url: "/Webservices/CampaignInfo.asmx/GetSkillNodeInfo",
                type: "POST",
                dataType: 'json',
                success: function (result) {
                    divDesc.innerHTML = result.d;
                }
            });
        }

        function Callback(result) {
            var outDiv = document.getElementById("outputDiv");
            outDiv.innerText = result;
        }
        function OnSuccessCall(response) {
            alert(response.d);
        }

        function OnErrorCall(response) {
            alert(response.status + " " + response.statusText);
        }

        function openMessageWithText(Msg) {
            parent.openMessageWithText(Msg);
        }

        function openErrorWithText(Msg) {
            parent.openErrorWithText(Msg);
        }

        function openMessage() {
            $('#modalMessage').modal('show');
        }

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
            <div class="col-sm-8 margin20">
                <div class="input-group">
                    <CharSelector:Select ID="oCharSelect" runat="server" />
                </div>
            </div>
            <div class="col-sm-4 text-right">
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Button ID="btnTopSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:CheckBox ID="cbxShowExclusions" runat="server" Text="Show Exclusions" AutoPostBack="true" OnCheckedChanged="cbxShowExclusions_CheckedChanged" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 bg-classes">
                <p class="bg-info text-center">Skill validation may take a few moments. Please wait while the screen refreshes <strong>BEFORE SAVING</strong>.</p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Character Skills</div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSkill" runat="server">
                            <ContentTemplate>
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-xs-5">
                                            <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Auto" Height="350px">
                                                <asp:TreeView ID="tvDisplaySkills" runat="server" SkipLinkText="" BorderColor="Black" BorderStyle="Solid" BorderWidth="0"
                                                    ShowLines="false" OnTreeNodeCheckChanged="tvSkills_TreeNodeCheckChanged" Font-Underline="false" CssClass="form-control" EnableClientScript="false"
                                                    LeafNodeStyle-CssClass="TreeItems" NodeStyle-CssClass="TreeItems">
                                                    <LevelStyles>
                                                        <asp:TreeNodeStyle Font-Underline="false" />
                                                    </LevelStyles>
                                                </asp:TreeView>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-5 col-sm-3">
                                            <asp:Panel ID="pnlDescription" runat="server" ScrollBars="Auto" Height="350px">
                                                <div id="divDesc"></div>
                                                <br />
                                                <asp:TextBox ID="tbPlayerComments" runat="server" Visible="false" />
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-2 col-sm-3">
                                            <asp:Panel ID="pnlCostList" runat="server" HorizontalAlign="right" CssClass="text-right" ScrollBars="Auto" Height="350px">
                                                <asp:GridView ID="gvCostList" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvCostList_RowDataBound" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="Skill" HeaderText="Skill" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="Cost" HeaderText="Cost" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                &nbsp;&nbsp;
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hidAllowCharacterRebuild" runat="server" Value="0" />

                                <asp:HiddenField ID="hidScrollPos" runat="server" />

                                <script type="text/javascript">
                                    var hiddenStatusFlag = document.getElementById('<%= hidScrollPos.ClientID%>');
                                    if (hiddenStatusFlag != null) {
                                        if (!isNaN(hiddenStatusFlag.value)) {
                                            $get('<%=pnlTreeView.ClientID%>').scrollTop = hiddenStatusFlag.value
                                            hiddenStatusFlag.value = "";
                                        }
                                    }
                                </script>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-8">
                <asp:Label ID="lblMessage" runat="server" />
                <asp:Label ID="lblSkillsLocked" runat="server" Text="Changes not allowed after Save" CssClass="SkillsLocked" Visible="false" />
            </div>
            <div class="col-md-4">
                <p class="text-right">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                </p>
            </div>
        </div>
        <div id="push"></div>
    </div>

    <div class="modal fade" id="modalMessage" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3>Manage Teams</h3>
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
    <!-- /#page-wrapper -->
</asp:Content>
