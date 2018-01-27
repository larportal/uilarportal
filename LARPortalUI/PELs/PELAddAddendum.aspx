<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="PELAddAddendum.aspx.cs" Inherits="LarpPortal.PELs.PELAddAddendum" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELAddAddendumStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELAddAddendumScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="PELAddAddendumBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>PEL Add Addendum</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="lblEditMessage" runat="server" CssClass="lead" Visible="false" />

            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="lblEventInfo" runat="server" />
            </div>
        </div>

        <%--
                TODO There was a picture here but not at the moment.
                <asp:Image ID="imgPicture" runat="server" Width="100px" Height="100px" />

        --%>

        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-default">
                    <div class="panel-heading">PEL (Post Event Letter) Addendum</div>
                    <div class="panel-body">
                        <div class="panel-container">
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:TextBox ID="tbAddendum" runat="server" TextMode="MultiLine" Columns="100" Style="width: 100%"
                                        BorderColor="black" BorderStyle="Solid" BorderWidth="1" Rows="4" />
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <div class="col-xs-4">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                            </div>
                                            <div class="col-xs-4 text-center">
                                                <b>Everything below is read only.</b>
                                            </div>
                                            <div class="col-sm-4 text-right">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="margin10"></div>

        <div id="divQuestions" runat="server" class=" pre-scrollable">
            <asp:Repeater ID="rptAddendum" runat="server">
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
                    <div class="margin10"></div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Repeater ID="rptQuestions" runat="server">
                <ItemTemplate>

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">Question: <%# Eval("Question") %></div>
                                <div class="panel-body">
                                    <div class="panel-container">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:Label ID="lblAnswer" runat="server" Text='<%# Eval("Answer") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="margin10"></div>
                    <asp:HiddenField ID="hidQuestionID" runat="server" Value='<%# Eval("PELQuestionID") %>' />
                    <asp:HiddenField ID="hidAnswerID" runat="server" Value='<%# Eval("PELAnswerID") %>' />
                </ItemTemplate>
            </asp:Repeater>
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
        <asp:HiddenField ID="hidRegistrationID" runat="server" />
        <asp:HiddenField ID="hidPELID" runat="server" />

        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

