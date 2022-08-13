<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="StaffAnswerComment.aspx.cs" Inherits="LarpPortal.Character.ISkills.StaffAnswerComment" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="PELApproveStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELApproveScripts" ContentPlaceHolderID="MainScripts" runat="Server">

    <script>  
        function showhide() {
            var div = document.getElementById("divEnterComments");
            div.style.display = "block";
            return false;
        }
    </script>



</asp:Content>
<asp:Content ID="PELApproveBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="">
                    <h1>ISkill Header</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 col-lg-6">
                <div class="">
                    <div class="form-group">
                        <div class="controls">
                            <label for="<%= lblEventInfo.ClientID %>">Event Info</label>
                            <asp:Label ID="lblEventInfo" runat="server" CssClass="form-control col-xs-12" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-col-12 col-lg-6">
                <div class="">
                    <span class="alert-danger">This skill was purchased after the event. Dependng on the games rules you may or may not want to allow this request.</span>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-lg-6">
                <div class="">
                    <div class="form-group">
                        <div class="controls">
                            <label for="<%= lblCharName.ClientID %>">Character Name</label>
                            <asp:Label ID="lblCharName" runat="server" CssClass="form-control col-xs-12" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-lg-6">
                <div class="">
                    <div class="form-group">
                        <div class="controls">
                            <label for="<%= lblPlayerName.ClientID %>">Player Name</label>
                            <asp:Label ID="lblPlayerName" runat="server" CssClass="form-control col-xs-12" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <div class="form-group">
                    <div class="controls">
                        <label for="<%= lblRequest.ClientID %>">Request</label>
                        <asp:Label ID="lblRequest" runat="server" CssClass="form-control col-xs-12" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="padding-left: 15px; padding-bottom: 10px;">
        </div>
        <div class="row" style="padding-bottom: 10px;">
            <div class="col-lg-2 col-md-4 col-xs-12">
                <div class="form-group">
                    <label for="<%= ddlRequestStatus.ClientID %>">Request Status: </label>
                    <asp:DropDownList ID="ddlRequestStatus" runat="server" CssClass="form-control" />
                </div>
                Marking the request as <b>Complete</b> or <b>Delivered</b> means the user will be able to see the response.
            </div>
            <div class="col-lg-10 col-md-6 col-xs-12">
                <div class="form-group">
                    <label for="<%= CKResponse.ClientID %>">Staff Official Response: <i class="fa-solid fa-circle-question" title="The person will not see this until it has been marked completed."></i></label>
                    <CKEditor:CKEditorControl ID="CKResponse" BasePath="/ckeditor/" CssClass="form-control" runat="server" Height="100px"></CKEditor:CKEditorControl>
                </div>
                <asp:FileUpload ID="ulFile" runat="server" CssClass="form-control col-lg-6" />
                <asp:Label ID="lblFileName" runat="server" Visible="false" />
                <asp:Button ID="btnSaveRequest" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnSaveRequest_Click" Text="Save Request" />
            </div>
        </div>
        <asp:HiddenField ID="hidRegistrationID" runat="server" />
        <asp:HiddenField ID="hidPELID" runat="server" />

        <hr />
        <asp:Repeater ID="rptQuestions" runat="server">
            <ItemTemplate>
                <div class="row">
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
                            <label for="<%= CKEditorComment.ClientID %>">Enter the comments</label>
                            <CKEditor:CKEditorControl ID="CKEditorComment" BasePath="/ckeditor/" CssClass="form-control" runat="server" Height="100px"></CKEditor:CKEditorControl>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-lg-6 text-right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
                <br />
                <br />
            </div>
        </div>
        <asp:HiddenField ID="hidCampaignCPOpportunityDefaultID" runat="server" />
        <asp:HiddenField ID="hidReasonID" runat="server" />
        <asp:HiddenField ID="hidCampaignPlayerID" runat="server" />
        <asp:HiddenField ID="hidCharacterID" runat="server" />
        <asp:HiddenField ID="hidCampaignID" runat="server" />
        <asp:HiddenField ID="hidCharacterAKA" runat="server" />
        <asp:HiddenField ID="hidEventID" runat="server" />
        <asp:HiddenField ID="hidEventDesc" runat="server" />
        <asp:HiddenField ID="hidPELNotificationEMail" runat="server" />
        <asp:HiddenField ID="hidEventDate" runat="server" />
        <asp:HiddenField ID="hidPlayerName" runat="server" />
        <asp:HiddenField ID="hidSubmitDate" runat="server" />
        <asp:HiddenField ID="hidAuthorName" runat="server" />

        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

