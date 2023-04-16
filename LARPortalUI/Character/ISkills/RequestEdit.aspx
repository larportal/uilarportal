<%@ Page Title="In-between Skills Request" Language="C#" MasterPageFile="~/LARPortal.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="RequestEdit.aspx.cs" Inherits="LarpPortal.Character.ISkills.RequestEdit" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

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
        function openRequestSaved() {
            $('#modalRequestSaved').modal('show');
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

    </script>
</asp:Content>

<asp:Content ID="CharSkillsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>In-Between Event Skills - Info</h1>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <b>Event Name:&nbsp;</b>
                    <asp:Label ID="lblEventName" runat="server" />
                    <b style="padding-left: 20px;">Event Date:&nbsp;</b>
                    <asp:Label ID="lblEventDate" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <b>Skill Name:</b>
                    <asp:Label ID="lblSkillName" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <b>Skill Description:</b>
                    <asp:Label ID="lblShortSkillDesc" runat="server" />
                    <button type="button" id="toggleButton" name="toggleButton" onclick="toggleSkillDesc();" class="btn btn-xs btn-info">Display Full Description</button>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Label ID="lblLongSkillDesc" runat="server" />
                </div>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="controls">
                            <label for="">Request</label>
                            <CKEditor:CKEditorControl ID="CKERequestText" BasePath="/ckeditor/" runat="server" Height="100px"></CKEditor:CKEditorControl>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-4">
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                </div>
                <div class="col-xs-8 text-right">
                    <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-success" OnCommand="btnSubmitSave_Command" CommandName="SUBMIT" />
                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-success" OnCommand="btnSubmitSave_Command" CommandName="SAVE" />
                </div>
            </div>
            <asp:Panel ID="pnlStaffResponse" runat="server" Visible="false">
                <div class="row" style="padding-top: 15px;">
                    <div class="col-xs-12">
                        <b>Staff Response:</b>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <asp:Literal ID="litStaffResponse" runat="server" Mode="Encode" />
                    </div>
                </div>
            </asp:Panel>
        </div>

        <div class="modal fade in" id="modalRequestSaved" role="dialog">
            <div class="modal-dialog modal-lg">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3>In-between Skills Request</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblmodalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12 NoGutters text-right">
                                <asp:Button ID="btnCloseMessage" runat="server" Text="Close" OnClick="btnCloseMessage_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidSkillRequestID" runat="server" />
        <asp:HiddenField ID="hidRegistrationID" runat="server" />
        <asp:HiddenField ID="hidSkillNodeID" runat="server" />
        <asp:HiddenField ID="hidViewOnly" runat="server" />
        <asp:HiddenField ID="hidSkillSetID" runat="server" />
        <asp:HiddenField ID="hidWhichDisplayed" runat="server" />
    </div>
</asp:Content>
