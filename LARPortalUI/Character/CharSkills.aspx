<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CharSkills.aspx.cs" Inherits="LarpPortal.Character.CharSkills" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style type="text/css">
        .nopadding {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }
    </style>
</asp:Content>
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

        function SaveValue() {
            var ddlAddValue = document.getElementById('<%= ddlAddValue2.ClientID %>');
            var strUser = ddlAddValue.options[ddlAddValue.selectedIndex].value;
            var hid = document.getElementById('<%= hidNewDropDownValue.ClientID %>');
            hid.value = strUser;
        }

        function ChangeValue(CampSkillNodeID, CurrentValue) {
            //                       alert(CampSkillNodeID);
            $.ajax({
                contentType: "application/json; charset=utf-8",
                data: "{'CampaignSkillNodeID':'" + CampSkillNodeID.toString() + "'}",
                url: "/Webservices/CampaignInfo.asmx/GetSkillValues",
                type: "POST",
                dataType: 'json',
                success: function (result) {
                    var ret = JSON.parse(result.d);
                    if (ret["DataType"] == "Text") {
                        var hidCampaignSkillNodeID = document.getElementById('<%= hidCampaignSkillNodeID.ClientID %>');
                        hidCampaignSkillNodeID.value = CampSkillNodeID.toString();
                        var lblOrigTextValue = document.getElementById('<%=lblOrigTextValue.ClientID%>')
                        lblOrigTextValue.innerText = CurrentValue;
                        var tbNewValue = document.getElementById('<%= tbNewValue.ClientID %>');
                        tbNewValue.value = ret["CurrentValue"];
                        $("#modalChangeTextValue").modal('show');
                    } else if (ret["DataType"] == "DropDown") {
                        var hidCampaignSkillNodeID = document.getElementById('<%= hidCampaignSkillNodeID.ClientID %>');
                        hidCampaignSkillNodeID.value = CampSkillNodeID.toString();
                        var lblOrigDropDownValue = document.getElementById('<%= lblOrigDropDownValue.ClientID %>');
                        lblOrigDropDownValue.innerText = CurrentValue;
                        var ddlAddValue = document.getElementById('<%= ddlAddValue2.ClientID %>');
                        var iSelectedIndex = 0;
                        for (var i = 0; i < ret["AvailValues"].length; i++) {
                            var opt = ret["AvailValues"][i];
                            var el = document.createElement("option");
                            el.textContent = opt;
                            el.value = opt;
                            if (opt == CurrentValue)
                                iSelectedIndex = i;
                            ddlAddValue.appendChild(el);
                        }
                        ddlAddValue.selectedIndex = iSelectedIndex;
                        $('#modalChangeDropDownValue').modal('show');
                    }
                }
            });
        }

        function scrollTree() {
            var pnlTreeView = document.getElementById('<%=pnlTreeView.ClientID%>');
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                pnlTreeView.scrollTop = hidScrollPos.value;
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

        function openTextValueChange() {
            $("#modalChangeTextValue").show();
        }

        function openPointIssue() {
            $("#modalPointIssue").show();
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
                                <asp:Label ID="lblPlace" runat="server" />
                                <asp:HiddenField ID="hidNewDropDownValue" runat="server" />
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

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3>Character Skills</h3>
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
        <asp:HiddenField ID="hidAutoBuyParentSkills" runat="server" />
    </div>


    <asp:HiddenField ID="hidCampaignSkillNodeID" runat="server" />

    <!--  Change the text additional value -->
    <div class="modal fade in" id="modalChangeTextValue" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 style="padding-top: 0px !important; padding-bottom: 0px !important; margin-top: 0px; margin-bottom: 0px;">Change Skill Qualifier</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    &nbsp;
                                </div>
                                <div class="col-sm-8">
                                    <div class="row">
                                        <div class="form-group col-sm-12">
                                            <div class="controls">
                                                <label for="<%= lblOrigTextValue.ClientID %>">Original Value:</label>
                                                <asp:Label ID="lblOrigTextValue" runat="server" CssClass="form-control NoShadow nopadding" />
                                            </div>
                                        </div>
                                        <div class="form-group col-sm-12">
                                            <div class="controls">
                                                <label for="<%= tbNewValue.ClientID %>">New Value:</label>
                                                <asp:TextBox ID="tbNewValue" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    &nbsp;
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 NoGutters text-right">
                            <asp:Button ID="btnCancelTextChanges" runat="server" Text="Cancel" CssClass="btn btn-danger" />&nbsp;&nbsp;
                            <asp:Button ID="btnSaveTextChanges" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSaveTextChanges_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--  Change the text additional value -->



    <!--  Change the text additional value -->
    <div class="modal fade in" id="modalChangeDropDownValue" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 style="padding-top: 0px !important; padding-bottom: 0px !important; margin-top: 0px; margin-bottom: 0px;">Change Skill Qualifier</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    &nbsp;
                                </div>
                                <div class="col-sm-8">
                                    <div class="row">
                                        <div class="form-group col-sm-12">
                                            <div class="controls">
                                                <label for="<%= lblOrigDropDownValue.ClientID %>">Original Value:</label>
                                                <asp:Label ID="lblOrigDropDownValue" runat="server" CssClass="form-control NoShadow nopadding" />
                                            </div>
                                        </div>
                                        <div class="form-group col-sm-12">
                                            <div class="controls">
                                                <label for="<%= tbNewValue.ClientID %>">New Value:</label>
                                                <select id="ddlAddValue2" runat="server" class="form-control"></select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    &nbsp;
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 NoGutters text-right">
                            <asp:Button ID="btnCancelDropDown" runat="server" Text="Cancel" CssClass="btn btn-danger" />&nbsp;&nbsp;
                            <asp:Button ID="btnSaveDropDownChanges" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClientClick="SaveValue();" OnClick="btnSaveDropDownChanges_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--  Change the drop down additional value -->




    <div class="modal fade in" id="modalPointIssue" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 style="padding-top: 0px !important; padding-bottom: 0px !important; margin-top: 0px; margin-bottom: 0px;">Issue with points</h3>
                </div>
                <div class="modal-body">
                    <p>
                        You appear to have a pool with a total negative point available.<br />
                        <br />
                        You cannot save a character who has negative points. Your current point totals are:
                        <asp:GridView ID="gvPoolTotals" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-striped" 
                                OnRowDataBound="gvPoolTotals_RowDataBound" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="PoolName" HeaderText="Pool Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TotalCP" HeaderText="Total Available Points" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CPSpent" HeaderText="Total Points Spent" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AvailablePoints" HeaderText="Available Points" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 NoGutters text-right">
                            <asp:Button ID="btnCloseCantSave" runat="server" Text="Cancel" CssClass="btn btn-danger" />
<%--                            <asp:Button ID="Button2" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClientClick="SaveValue();" OnClick="btnSaveDropDownChanges_Click" />--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>











    <!-- /#page-wrapper -->

    <script type="text/javascript">
        // It is important to place this JavaScript code after ScriptManager1
<%--        var xPos, yPos;
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
        }--%>

<%--var hdnScroll = document.getElementById('<%=hidScrollPos.ClientID%>');
var bigDiv = document.getElementById('<%=pnlTreeView.ClientID%>');
bigDiv.onscroll = function() {
     hdnScroll.value = bigDiv.scrollTop;
}

window.onload = function () { 
    bigDiv.scrollTop = hdnScroll.value;
}
--%>

        //prm.add_beginRequest(BeginRequestHandler);
        //prm.add_endRequest(EndRequestHandler);
    </script>

</asp:Content>
