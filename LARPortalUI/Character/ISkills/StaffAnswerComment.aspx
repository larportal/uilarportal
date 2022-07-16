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
                <h1>ISkill Header</h1>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-xs-10">
                        <asp:Label ID="lblEventInfo" runat="server" />
                        <div class="col-xs-2">
                            <div class="row" style="margin-top: 20px;">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <div class="controls">
                                            <label for="<%= lblRequest.ClientID %>">Request</label>
                                            <asp:Label ID="lblRequest" runat="server" BorderWidth="1" BorderColor="Black" BorderStyle="Solid" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-2">
                        <asp:Image ID="imgPicture" runat="server" Width="100px" Height="100px" />
                    </div>
                </div>
            </div>
            <div class="row" style="padding-left: 15px; padding-bottom: 10px;">
                <%--            <div class="row">
                Staff Options
            </div>--%>
                <div class="row">
                    <div class="col-lg-2 col-xs-12">
                        <div class="form-group">
                            <label for="<%= ddlResponse.ClientID %>">Status:</label>
                            <asp:DropDownList ID="ddlResponse" runat="server" AutoPostBack="true" CssClass="form-control">
                                <asp:ListItem>No Response</asp:ListItem>
                                <asp:ListItem>Respond Immediately</asp:ListItem>
                                <asp:ListItem>Respond at event (staff will need to print response.)</asp:ListItem>
                                <asp:ListItem>Do Nothing</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidRegistrationID" runat="server" />
        <asp:HiddenField ID="hidPELID" runat="server" />

        <asp:Repeater ID="rptQuestions" runat="server">
            <ItemTemplate>
                <div class="row">
                    <h4>Comment from <%# Eval("FirstName") %> <%# Eval("LastName") %></h4>
                    <%--                    <div class="panel-body">
                        <div class="panel-container search-criteria" style="padding-bottom: 10px;">--%>
                    <asp:Label ID="lblAnswer" runat="server" Text='<%# Eval("StaffComments") %>' CssClass="form-control" />
                    <%--                        </div>
                    </div>--%>
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

