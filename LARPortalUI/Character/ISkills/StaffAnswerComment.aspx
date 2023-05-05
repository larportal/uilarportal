<%@ Page Title="In-between Skill Request Staff Edit" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="StaffAnswerComment.aspx.cs" Inherits="LarpPortal.Character.ISkills.StaffAnswerComment" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="PELApproveStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
        .glow-button {
            position: relative;
        }

            .glow-button::before {
                content: "";
                position: absolute;
                z-index: -1;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: inherit;
                filter: blur(15px);
                transition: 0.5s;
            }

            .glow-button:focus::before {
                filter: blur(1px);
            }

        .btn-lg {
            margin: 1em;
        }

        .radius {
            border-radius: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="PELApproveScripts" ContentPlaceHolderID="MainScripts" runat="Server">

    <script>  
        $.jQueryFunction = function () {
            $('#ctl00$MainBody$cbDisplayToUser').bootstrapToggle('toggle');
        };

        function showhide() {
            var div = document.getElementById("divEnterComments");
            div.style.display = "block";
            return false;
        }

        function reallyDelete() {
            $('#modalDeleteFile').modal('show');
        }

        function ChangeButton() {
            var cbDisplayToUser = document.getElementById("<%= cbDisplayStatusToUser.ClientID %>");
            cbDisplayToUser.checked = false;
        }

        function toggleSkillDesc() {
            var Long = document.getElementById("<%= lblLongSkillDesc.ClientID %>");
            var WhichDisplayed = document.getElementById("<%= hidWhichDisplayed.ClientID %>");
            var DisplayButton = document.getElementById("toggleButton");
            if (WhichDisplayed.value == "S") {
                Long.style.display = "block";
                WhichDisplayed.value = "L";
                DisplayButton.innerText = "Hide Long Description";
            }
            else {
                Long.style.display = "none";
                WhichDisplayed.value = "S";
                DisplayButton.innerText = "Display Long Description";
            }
            return false;
        }

        function openMessage() {
            $('#modalMessage').modal('show');
        }


    </script>



</asp:Content>
<asp:Content ID="PELApproveBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>In-between Info Skill Request - Staff Edit</h1>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-xs-12 col-lg-12">
                        <div class="row">
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblEventDate.ClientID %>">Event Date:</label>
                                <asp:Label ID="lblEventDate" runat="server" />
                            </div>
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblSkillName.ClientID %>">Last Event Date:</label>
                                <asp:Label ID="lblLastEventDate" runat="server" />
                            </div>
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblCharName.ClientID %>">Character Name:</label>
                                <asp:Label ID="lblCharName" runat="server" />
                            </div>
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblPlayerName.ClientID %>">Player Name:</label>
                                <asp:Label ID="lblPlayerName" runat="server" />
                            </div>
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblSkillName.ClientID %>">Skill Name:</label>
                                <asp:Label ID="lblSkillName" runat="server" />
                            </div>
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblSkillPurchaseDate.ClientID %>">Skill Purchase Date:</label>
                                <asp:Label ID="lblSkillPurchaseDate" runat="server" />
                            </div>
                            <div class="col-xs-12 col-lg-6">
                                <label for="<%= lblShortSkillDesc.ClientID %>">Skill Description:</label>
                                <asp:Label ID="lblShortSkillDesc" runat="server" />
                                <button type="button" id="toggleButton" name="toggleButton" onclick="toggleSkillDesc();" class="btn btn-xs btn-info">Display Full Description</button>
                            </div>
                            <div class="col-xs-12">
                                <asp:Label ID="lblLongSkillDesc" runat="server" />
                            </div>

                            <div class="col-lg-12 col-xs-12" style="padding-top: 25px;">
                                <div class="">
                                    <label for="<%= ddlAssignedTo.ClientID %>">Assigned To: </label>
                                    <asp:DropDownList ID="ddlAssignedTo" runat="server" />&nbsp;&nbsp;
                                    <label for="<%= ddlRequestStatus.ClientID %>">Request Status: </label>
                                    <asp:DropDownList ID="ddlRequestStatus" runat="server" CssClass="" />&nbsp;&nbsp;
                                    <asp:CheckBox ID="cbDisplayStatusToUser" runat="server" Text="Display Status To Player" />&nbsp;&nbsp;
                                    <asp:CheckBox ID="cbDisplayResponseToUser" runat="server" Text="Display Response To Player" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="padding-top: 10px;">
                    <div class="col-xs-12">
                        <b>Request:</b>
                        <asp:Label ID="lblRequest" runat="server" CssClass="" />
                    </div>
                </div>
                <asp:Panel ID="pnlPayment" runat="server" Visible="false">
                    <div class="col-xs-12 col-lg-12">
                        <div class="row">
                            <label for="<%= lblEventDate.ClientID %>">Payment (if applicable): </label>
                            <asp:Label ID="lblPayment" runat="server" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlCollaboratingNotes" runat="server" Visible="false">
                    <div class="row" style="padding-top: 10px;">
                        <div class="col-xs-12">
                            <b>Collaborating Notes: </b>
                            <asp:Label ID="lblCollaboratingNotes" runat="server" CssClass="" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlPlayerComments" runat="server" Visible="false">
                    <div class="row" style="padding-top: 10px;">
                        <div class="col-xs-12">
                            <b>Player Comments: </b>
                            <asp:Label ID="lblPlayerComments" runat="server" CssClass="" />
                        </div>
                    </div>
                </asp:Panel>

                <div class="row" style="padding-bottom: 10px; padding-top: 10px;">
                    <div class="col-lg-10 col-md-6 col-xs-12">
                        <div class="form-group">
                            <label for="<%= tbResp.ClientID %>">Staff Official Response: <i class="fa-solid fa-circle-question" title="The person will not see this until it has been marked completed."></i></label>
                            <asp:TextBox ID="tbResp" runat="server" TextMode="MultiLine" Rows="5" CssClass="col-xs-12" />
                            <%--                            <CKEditor:CKEditorControl ID="CKResponse" BasePath="/ckeditor/" CssClass="form-control" runat="server" Height="100px"></CKEditor:CKEditorControl>--%>
                        </div>
                        <div class="row" style="display: none;">
                            <div class="col-lg-6">
                                <asp:FileUpload ID="ulFile" runat="server" CssClass="form-control" />
                                <asp:HyperLink ID="hlFileName" runat="server" Visible="false" Target="_blank" />
                                <asp:Label ID="lblFileName" runat="server" Visible="false" />
                                <asp:Button ID="btnDeleteAttach" runat="server" CssClass="btn btn-warning btn-xs" OnClick="btnDeleteAttach_Click" OnClientClick="reallyDelete(); return false;" Text="Delete Attachment" />
                            </div>
                            <div class="col-lg-6">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <asp:Button ID="btnCancel" Text="Return To List" runat="server" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-lg-6">
                        <asp:Button ID="btnSaveRequest" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnSaveRequest_Click" Text="Save Request" />
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidRequestSkillID" runat="server" />
        <asp:HiddenField ID="hidPELID" runat="server" />

        <hr />
        <asp:Repeater ID="rptQuestions" runat="server">
            <ItemTemplate>
                <div class="row border">
                    <%# Eval("CommentHeader") %>
                    <asp:Label ID="lblAnswer" runat="server" Text='<%# Eval("StaffComments") %>' CssClass="form-control"
                        Visible='<%# (Eval("ShowComment").ToString() == "Y") %>' />
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <div class="row">
            <div class="col-lg-pull-12">
                <asp:Button ID="btnAddComment" runat="server" Text="Add Staff Only Comment" CommandName="EnterComment" CssClass="btn btn-primary" OnClientClick="showhide(); return false;" />
            </div>
        </div>

        <div id="divEnterComments" style="display: none;">
            <div class="row">
                <div class="col-12">
                    <div class="form-group">
                        <div class="controls">
                            <label for="<%= tbNewComment.ClientID %>">Enter the comments</label>
                            <asp:TextBox ID="tbNewComment" runat="server" TextMode="MultiLine" Rows="5" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <asp:Button ID="btnCancelModal" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancelModal_Click" />
                    </div>
                    <div class="col-lg-6 text-right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
                <br />
                <br />
            </div>
        </div>

        <asp:HiddenField ID="hidFileName" runat="server" />
        <asp:HiddenField ID="hidChangesMade" runat="server" />
        <asp:HiddenField ID="hidCampaignID" runat="server" />

        <div class="modal fade in" id="modalDeleteFile" role="dialog">
            <div class="modal-dialog modal-lg">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3>Character Items</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            Are you sure you want to delete the file
                            <asp:Label ID="lblFileToDelete" runat="server" />
                            ?
                        </p>
                        <p>
                            If you press yes, the file will be immediately deleted and there is no way to get it back.
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-6 no-gutters">
                                <asp:Button ID="btnYesDelete" runat="server" Text="Yes (Delete the file.)" CssClass="btn btn-danger" OnClick="btnYesDelete_Click" />
                            </div>
                            <div class="col-xs-6 no-gutters">
                                <asp:Button ID="btnNoDelete" runat="server" Text="No (Do not delete.)" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">In-between Info Skill Request</h3>
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





        <div id="push"></div>
    </div>
    <asp:HiddenField ID="hidWhichDisplayed" runat="server" />
    <!-- /#page-wrapper -->
</asp:Content>












<%--Style="border-radius: 4px;" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1" --%>
