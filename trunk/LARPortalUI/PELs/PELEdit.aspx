<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="PELEdit.aspx.cs" Inherits="LarpPortal.PELs.PELEdit" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELEditStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELEditScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="PELEditBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <asp:Timer ID="Timer1" runat="server" Interval="300000" OnTick="Timer1_Tick" />
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>PEL Edit</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-6">
                <asp:Label ID="lblEditMessage" runat="server" Font-Size="18px" Style="font-weight: 500" Visible="false" />
            </div>
            <div class="col-xs-6">
                <asp:Panel ID="pnlSaveReminder" runat="server" Visible="true">
                    <strong>Note: </strong>Remember to click save to save your PEL and submit when it's complete to submit it to staff.
                                <asp:Button ID="btnTopSave" runat="server" Text="Save" OnCommand="ProcessButton" CommandName="Save" CssClass="btn btn-primary" />
                </asp:Panel>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="lblEventInfo" runat="server" />
            </div>
        </div>

        <div class="row"></div>
        <asp:UpdatePanel ID="upAutoSave" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hidRegistrationID" runat="server" />
                <asp:HiddenField ID="hidPELID" runat="server" />
                <asp:HiddenField ID="hidPELTemplateID" runat="server" />
                <asp:HiddenField ID="hidTextBoxEnabled" runat="server" Value="1" />
                <asp:HiddenField ID="hidAutoSaveText" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>

        <div id="divQuestions" runat="server" style="max-height: 500px; overflow-y: auto;">
            <asp:Repeater ID="rptAddendum" runat="server">
                <ItemTemplate>
                    <div class="row" style="margin-bottom: 20px;">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%# Eval("Title") %></div>
                            <div class="panel-body">
                                <asp:Label ID="lblAddendum" runat="server" Text='<%# Eval("Addendum") %>' />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Repeater ID="rptQuestions" runat="server">
                <ItemTemplate>
                    <div class="row" style="margin-bottom: 20px;">
                        <div class="panel panel-default">
                            <div class="panel-heading">Question: <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>' /></div>
                            <div class="panel-body">
                                <asp:TextBox ID="tbAnswer" runat="server" Text='<%# Eval("Answer") %>' Columns="100" Style="width: 100%"
                                    BorderColor="black" BorderStyle="Solid" BorderWidth="1" TextMode="MultiLine" Rows="4"
                                    Visible="<%# TextBoxEnabled %>" />
                                <asp:Label ID="lblAnswer" runat="server" Text='<%# Eval("Answer") %>' Visible="<%# !(TextBoxEnabled) %>" />
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hidQuestionID" runat="server" Value='<%# Eval("PELQuestionID") %>' />
                    <asp:HiddenField ID="hidAnswerID" runat="server" Value='<%# Eval("PELAnswerID") %>' />
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="row">
            <div class="col-xs-4">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
            </div>
            <div class="col-sm-8" style="text-align: right;">
                <asp:Button ID="btnSubmit" runat="server" Text="Save And Submit" OnCommand="ProcessButton" CommandName="Submit" CssClass="btn btn-primary" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnCommand="ProcessButton" CommandName="Save" CssClass="btn btn-primary" Style="margin-left: 25px;" />
            </div>
        </div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

