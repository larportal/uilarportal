<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Approve.aspx.cs" Inherits="LarpPortal.Character.History.Approve" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character History</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <%--                <div class="row col-lg-12" style="padding-left: 15px; padding-top: 10px;">--%>
                    <div class="col-xs-9">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblHeader" runat="server" Font-Size="24px" Style="font-weight: 500" Text="Character History" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblEditMessage" runat="server" Font-Size="18px" Style="font-weight: 500" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblCharacterInfo" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblPlayedBy" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-3 NoPadding text-right" style="padding-right: 5px;">
                        <asp:Image ID="imgPicture" runat="server" Width="100px" Height="100px" />
                    </div>
                </div>
            </div>
        </div>

        <div id="divQuestions" runat="server" style="max-height: 500px; overflow-y: auto;">
            <asp:Repeater ID="rptAddendum" runat="server" OnItemCommand="rptAddendum_ItemCommand" OnItemDataBound="rptAddendum_ItemDataBound">
                <ItemTemplate>
                    <div class="row" style="padding-left: 15px; margin-bottom: 20px; width: 100%;">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h2><%# Eval("Title") %></h2>
                                <div class="panel-body">
                                    <div class="col-xs-12">
                                        <%--                                        <div class="panel-container search-criteria" style="padding-bottom: 10px;">--%>
                                        <asp:Label ID="lblAddendum" runat="server" Text='<%# Eval("Addendum") %>' />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="padding-top: 5px;">
                            <asp:UpdatePanel ID="upComments" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr style="vertical-align: top;">
                                            <td>
                                                <asp:Button ID="btnAddStaffComment" runat="server" Text="Add Staff Only Comment" CommandName="EnterComment"
                                                    CommandArgument='<%# Eval("AddendumID") %>' CssClass="btn btn-primary" /></td>
                                            <td>
                                                <asp:Panel ID="pnlStaffCommentSection" runat="server" Visible="false" Style="vertical-align: top;">
                                                    <table>
                                                        <tr style="vertical-align: top;">
                                                            <td>
                                                                <asp:Image ID="imgStaffCommentProfilePicture" runat="server" Width="75" Height="75" /></td>
                                                            <td>
                                                                <asp:TextBox ID="tbNewStaffCommentAddendum" runat="server" TextMode="MultiLine" Rows="4" Columns="80" /></td>
                                                            <td>
                                                                <asp:Button ID="btnSaveNewStaffComment" runat="server" Text="Save" CssClass="btn btn-primary btn-sm option-button LeftRightPadding10" Width="100"
                                                                    CommandName="AddComment" CommandArgument='<%# Eval("AddendumID") %>' /></td>
                                                            <td>
                                                                <asp:Button ID="btnCancelStaffComment" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm option-button LeftRightPadding10" Width="100"
                                                                    CommandName="CancelComment" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <br />
                </ItemTemplate>
            </asp:Repeater>

            <div class="row" style="margin-right: 5px; margin-top: 20px;">
                <%-- style="padding-left: 15px; margin-bottom: 20px; width: 100%;">--%>
                <div class="col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Character History</div>
                        <div class="panel-body">
                            <%--                                <div class="panel-container search-criteria" style="padding-bottom: 10px;">--%>
                            <asp:Label ID="lblHistory" runat="server" />
                        </div>
                    </div>
                    <div style="padding-top: 5px;">
                        <asp:UpdatePanel ID="upComments" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr style="vertical-align: top;">
                                        <td>
                                            <asp:Button ID="btnAddComment" runat="server" Text="Add Staff Only Comment" OnClick="btnAddComment_Click" CssClass="btn btn-primary" /></td>
                                        <td>
                                            <asp:Panel ID="pnlCommentSection" runat="server" Visible="false" Style="vertical-align: top;">
                                                <table>
                                                    <tr style="vertical-align: top;">
                                                        <td>
                                                            <asp:Image ID="imgStaffPicture" runat="server" Width="75" Height="75" /></td>
                                                        <td>
                                                            <asp:TextBox ID="tbNewComment" runat="server" TextMode="MultiLine" Rows="4" Columns="80" /></td>
                                                        <td>
                                                            <asp:Button ID="btnSaveNewComment" runat="server" Text="Save" CssClass="btn btn-primary" Width="100" OnClick="btnSaveNewComment_Click" /></td>
                                                        <td>
                                                            <asp:Button ID="btnCancelComment" runat="server" Text="Cancel" CssClass="btn btn-primary" Width="100" OnClick="btnCancelComment_Click" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:DataList ID="dlComments" runat="server" AlternatingItemStyle-BackColor="linen">
                                                <HeaderTemplate>
                                                    <table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="vertical-align: top;" class="panel-body panel-container search-criteria">
                                                        <%--                                                        <td class="LeftRightPadding" style="padding-bottom: 5px;">
                                                            <asp:Image runat="server" Width="50" Height="50" ImageUrl='<%# Eval("UserPhoto") %>' /></td>--%>
                                                        <td class="LeftRightPadding" style="padding-bottom: 5px;">
                                                            <asp:Label runat="server" Text='<%# Eval("UserName") %>' Font-Bold="true" /></td>
                                                        <td class="LeftRightPadding" style="padding-bottom: 5px;">
                                                            <asp:Label runat="server" Text='<%# Eval("DateAdded") %>' /></td>
                                                        <td class="LeftRightPadding" style="padding-bottom: 5px;">
                                                            <asp:Label runat="server" Text='<%# Eval("StaffComments") %>' /></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <asp:Panel ID="pnlStaffComments" runat="server" Visible="true">
            <div class="row" style="padding-left: 15px; margin-bottom: 20px; width: 100%;">
                <div class="panel panel-default" style="padding-top: 0px; padding-bottom: 0px;">
                    <div class="panel-heading">CP Award</div>
                    <div class="panel-body">
                        <asp:MultiView ID="mvCPAwarded" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vwCPAwardedEntry" runat="server">
                                <table>
                                    <tr>
                                        <td>For completing this history, the person should be awarded </td>
                                        <td style="padding-left: 10px; padding-right: 10px;">
                                            <asp:TextBox ID="tbCPAwarded" runat="server" Columns="6" CssClass="form-control" Text="0.0" /></td>
                                        <td>CP.</td>
                                        <%--BorderColor="black" BorderStyle="Solid" BorderWidth="1" --%>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwCPAwardedDisplay" runat="server">
                                <asp:Label ID="lblCPAwarded" runat="server" />
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <div class="row col-sm-12" style="padding-left: 15px; margin-bottom: 20px; width: 100%; padding-right: 0px;">
            <div class="col-sm-4">
                <asp:Button ID="btnCancel" runat="server" Text="Return To History List" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
            </div>
            <div class="col-sm-8" style="text-align: right;">
                <asp:Button ID="btnReject" runat="server" Text="Needs Revision" ToolTip="By clicking this it will go back to the user for revisions."
                    CssClass="btn btn-primary" Width="150px" OnClick="btnReject_Click" />
                <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-primary" Width="150px" OnClick="btnApprove_Click" />
                <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="btn btn-primary" Width="150px" OnClick="btnDone_Click" />
            </div>
        </div>

        <div class="modal fade" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">LARPortal Character History Needs Revisions</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <CKEditor:CKEditorControl ID="ckHistory" runat="server" Height="410px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" Width="150px" CssClass="btn btn-primary" OnClick="btnSendMessage_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidCampaignID" runat="server" />
        <asp:HiddenField ID="hidCharacterAKA" runat="server" />
        <asp:HiddenField ID="hidEventID" runat="server" />
        <asp:HiddenField ID="hidCharacterID" runat="server" />
        <asp:HiddenField ID="hidEmail" runat="server" />
        <asp:HiddenField ID="hidNotificationEMail" runat="server" />
        <asp:HiddenField ID="hidCampaignCPOpportunityDefaultID" runat="server" />
        <asp:HiddenField ID="hidCampaignPlayerID" runat="server" />
        <asp:HiddenField ID="hidEventDesc" runat="server" />
        <asp:HiddenField ID="hidEventDate" runat="server" />
        <asp:HiddenField ID="hidSubmitDate" runat="server" />
        <asp:HiddenField ID="hidCampaignName" runat="server" />
        <asp:HiddenField ID="hidAuthorName" runat="server" />
    </div>
</asp:Content>
