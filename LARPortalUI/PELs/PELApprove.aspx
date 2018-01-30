<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="PELApprove.aspx.cs" Inherits="LarpPortal.PELs.PELApprove" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELApproveStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELApproveScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="PELApproveBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>PEL Approve</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-xs-10">
                        <div class="row">
                            <asp:Label ID="lblEditMessage" runat="server" CssClass="lead" Visible="false" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblEventInfo" runat="server" />
                        </div>
                    </div>
                    <div class="col-xs-2">
                        <asp:Image ID="imgPicture" runat="server" Width="100px" Height="100px" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="padding-left: 15px; padding-bottom: 10px;">
        </div>
        <asp:HiddenField ID="hidRegistrationID" runat="server" />
        <asp:HiddenField ID="hidPELID" runat="server" />
        <div id="divQuestions" runat="server" style="max-height: 500px; overflow-y: auto; margin-right: 10px;">


            <asp:Repeater ID="rptAddendum" runat="server" OnItemCommand="rptAddendum_ItemCommand" OnItemDataBound="rptAddendum_ItemDataBound">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="panel panel-default">
                                <div class="panel-heading"><%# Eval("Title") %></div>
                                <div class="panel-body">
                                    <div class="panel-container">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:Label ID="lblAddendum" runat="server" Text='<%# Eval("Addendum") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="upComments" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-xs-3">
                                            <asp:Button ID="btnAddStaffComment" runat="server" Text="Add Staff Only Comment" CommandName="EnterComment"
                                                CommandArgument='<%# Eval("PELsAddendumID") %>' CssClass="btn btn-primary" />
                                        </div>
                                        <div class="col-xs-9">
                                            <asp:Panel ID="pnlStaffCommentSection" runat="server" Visible="false" Style="vertical-align: top;">
                                                <div class="form-inline">
                                                    <asp:Image ID="imgStaffCommentProfilePicture" runat="server" Width="75" Height="75" />
                                                    <asp:TextBox ID="tbNewStaffCommentAddendum" runat="server" TextMode="MultiLine" Rows="4" Columns="80" />
                                                    <asp:Button ID="btnSaveNewStaffComment" runat="server" Text="Save" CssClass="btn btn-primary"
                                                        CommandName="AddComment" CommandArgument='<%# Eval("PELsAddendumID") %>' />
                                                    <asp:Button ID="btnCancelStaffComment" runat="server" Text="Cancel" CssClass="btn btn-primary"
                                                        CommandName="CancelComment" />
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:DataList ID="dlStaffComments" runat="server" CssClass="table table-striped table-hover table-condensed">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="vertical-align: top;" class="panel-body panel-container search-criteria">
                                        <td>
                                            <asp:Image runat="server" Width="50" Height="50" ImageUrl='<%# Eval("UserPhoto") %>' /></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%# Eval("UserName") %>' Font-Bold="true" /></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%# Eval("DateAdded") %>' /></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%# Eval("StaffComments") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:DataList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="margin10"></div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Repeater ID="rptQuestions" runat="server" OnItemCommand="rptQuestions_ItemCommand" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="row" style="padding-left: 15px; margin-bottom: 20px; width: 100%;">
                        <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                            <div class="panelheader">
                                <h2>Question: <%# Eval("Question") %></h2>
                                <div class="panel-body">
                                    <div class="panel-container search-criteria" style="padding-bottom: 10px;">
                                        <asp:Label ID="lblAnswer" runat="server" Text='<%# Eval("Answer") %>' />
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
                                                <asp:Button ID="btnAddComment" runat="server" Text="Add Staff Only Comment" CommandName="EnterComment" CommandArgument='<%# Eval("PELAnswerID") %>'
                                                    CssClass="btn btn-primary" /></td>
                                            <td>
                                                <asp:Panel ID="pnlCommentSection" runat="server" Visible="false" Style="vertical-align: top;">
                                                    <table>
                                                        <tr style="vertical-align: top;">
                                                            <td>
                                                                <asp:Image ID="imgProfilePicture" runat="server" Width="75" Height="75" /></td>
                                                            <td>
                                                                <asp:TextBox ID="tbNewComment" runat="server" TextMode="MultiLine" Rows="4" Columns="80" /></td>
                                                            <td>
                                                                <asp:Button ID="btnSaveNewComment" runat="server" Text="Save" CssClass="btn btn-primary"
                                                                    CommandName="AddComment" CommandArgument='<%# Eval("PELAnswerID") %>' /></td>
                                                            <td>
                                                                <asp:Button ID="btnCancelComment" runat="server" Text="Cancel" CssClass="btn btn-primary" CommandName="CancelComment" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:DataList ID="dlComments" runat="server">
                                                    <HeaderTemplate>
                                                        <table class="table table-striped table-hover table-condensed">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="vertical-align: top;" class="panel-body panel-container search-criteria">
                                                            <td>
                                                                <asp:Image runat="server" Width="50" Height="50" ImageUrl='<%# Eval("UserPhoto") %>' /></td>
                                                            <td>
                                                                <asp:Label runat="server" Text='<%# Eval("UserName") %>' Font-Bold="true" /></td>
                                                            <td>
                                                                <asp:Label runat="server" Text='<%# Eval("DateAdded") %>' /></td>
                                                            <td>
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
                    <asp:HiddenField ID="hidQuestionID" runat="server" Value='<%# Eval("PELQuestionID") %>' />
                    <asp:HiddenField ID="hidAnswerID" runat="server" Value='<%# Eval("PELAnswerID") %>' />
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <br />

        <asp:Panel ID="pnlStaffComments" runat="server" Visible="true">
            <div class="row" style="padding-left: 15px; margin-bottom: 20px; width: 100%;">
                <div class="panel" style="padding-top: 0px; padding-bottom: 0px;">
                    <div class="panelheader">
                        <h2>CP Award</h2>
                        <div class="panel-body">
                            <div class="panel-container search-criteria" style="padding-bottom: 10px;">
                                <asp:MultiView ID="mvCPAwarded" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="vwCPAwardedEntry" runat="server">
                                        <table>
                                            <tr>
                                                <td>For completing this PEL, the person should be awarded </td>
                                                <td style="padding-left: 10px; padding-right: 10px;">
                                                    <asp:TextBox ID="tbCPAwarded" runat="server" Columns="6" BorderColor="black" BorderStyle="Solid" BorderWidth="1" Text="0.0" CssClass="form-control" /></td>
                                                <td>CP.</td>
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
                </div>
            </div>
        </asp:Panel>

        <div class="row">
            <div class="col-sm-4">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
            </div>
            <div class="col-sm-8 text-right">
                <asp:Button ID="btnSave" runat="server" Text="Approve" OnCommand="ProcessButton" CommandName="Approve" CssClass="btn btn-primary" />
            </div>
        </div>
        <br />
        <br />

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

