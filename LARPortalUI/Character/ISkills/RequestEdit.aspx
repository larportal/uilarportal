<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="RequestEdit.aspx.cs" Inherits="LarpPortal.Character.ISkills.RequestEdit" %>

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
<%--        function postBackByObject() {
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                hidScrollPos.value = $get('<%=pnlTreeView.ClientID%>').scrollTop;
            }
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }--%>

<%--        function SaveValue() {
            var ddlAddValue = document.getElementById('<%= ddlAddValue2.ClientID %>');
            var strUser = ddlAddValue.options[ddlAddValue.selectedIndex].value;
            var hid = document.getElementById('<%= hidNewDropDownValue.ClientID %>');
            hid.value = strUser;
        }--%>

<%--        function scrollTree() {
            var pnlTreeView = document.getElementById('<%=pnlTreeView.ClientID%>');
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                pnlTreeView.scrollTop = hidScrollPos.value;
            }
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
        }--%>
    </script>
</asp:Content>

<asp:Content ID="CharSkillsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <table>
            <tr>
                <td>Event Name:</td>
                <td>
                    <asp:Label ID="lblEventName" runat="server" /></td>
                <td>Event Date:</td>
                <td>
                    <asp:Label ID="lblEventDate" runat="server" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Skill Name:</td>
                <td>
                    <asp:Label ID="lblSkillName" runat="server" /></td>
            </tr>
            <tr>
                <td>Request:</td>
            </tr>
            <tr>
                <td colspan="2">
                    <CKEditor:CKEditorControl ID="CKERequestText" BasePath="/ckeditor/" runat="server" Height="100px"></CKEditor:CKEditorControl>
                </td>
            </tr>
            <tr>
                <td><asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-btn-primary" OnClick="btnCancel_Click" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-btn-primary" OnCommand="btnSubmitSave_Command" CommandName="SUBMIT" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSave" Text="Save" runat="server" OnCommand="btnSubmitSave_Command" CommandName="SAVE" /></td>
            </tr>
        </table>
        <asp:HiddenField ID="hidRegID" runat="server" />
        <asp:HiddenField ID="hidNodeID" runat="server" />
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
</asp:Content>
