<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="LarpPortal.Character.History.Edit" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="CharHistoryEditStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
        @media (max-width: 767px) {
            .text-right {
                text-align: inherit;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="CharHistoryEditScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function closeMessage() {
            $('#modelMessage').hide();
        }

        function showNewAddendum() {
            document.getElementById('divNewAddendum').style.display = "block";
        }

        function hideNewAddendum() {
            document.getElementById('divNewAddendum').style.display = "none";
        }

    </script>
</asp:Content>

<asp:Content ID="CharHistoryEditBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="">
                    <div class="header-background-image">
                        <h1>Character History</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-10 margin20">
                    <div class="input-group">
                        <CharSelector:Select ID="oCharSelect" runat="server" />
                    </div>
                </div>
                <div class="col-sm-2" style="padding-right: 0px;">
                    <p class="text-right">
                        <asp:Button ID="btnAddAddendum" runat="server" Text="Add Addendum" CssClass="btn btn-info" OnClientClick="showNewAddendum(); return false;" />
                    </p>
                </div>
            </div>

            <asp:Timer ID="Timer1" runat="server" Interval="300000" OnTick="Timer1_Tick"></asp:Timer>
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


            <div class="row">
                <div id="divNewAddendum" class="col-xs-12 margin20" style="display: none;">
                    <div class="row">
                        <div class="panel panel-default" style="padding-top: 0px; padding-bottom: 0px;">
                            <div class="panel-heading">Addendum</div>
                            <div class="panel-body">
                                <%--                            <div class="panel-container" style="padding-bottom: 10px;">--%>
                                <CKEditor:CKEditorControl ID="CKEAddendum" BasePath="/ckeditor/" runat="server" Height="100px"></CKEditor:CKEditorControl>
                                <%--                            </div>--%>
                            </div>
                        </div>
                        <div class="row" style="padding-bottom: 10px;">
                            <div class="col-lg-6">
                                <asp:Button ID="btnCancelAddendum" runat="server" Text="Cancel" CssClass="btn btn-info" OnClientClick="hideNewAddendum(); return false;" />
                            </div>
                            <div class="col-lg-6 text-right">
                                <asp:Button ID="btnSaveAddendum" runat="server" Text="Save" CssClass="btn btn-info" OnClick="btnSaveAddendum_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <asp:Repeater ID="rptAddendum" runat="server">
                        <ItemTemplate>
                            <div class="row" style="margin-bottom: 0px;">
                                <div class="panel panel-default" style="padding-top: 0px; padding-bottom: 0px;">
                                    <div class="panel-heading"><%# Eval("Title") %></div>
                                    <div class="panel-body">
                                        <div class="panel-container" style="padding-bottom: 10px;">
                                            <asp:Label ID="lblAddendum" runat="server" Text='<%# Eval("Addendum") %>' />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="row" style="margin-bottom: 20px;" runat="server" id="divHistory">
                <div class="panel panel-default" style="padding-top: 0px; padding-bottom: 0px;">
                    <div class="panel-heading">Character History</div>
                    <div class="panel-body">
                        <CKEditor:CKEditorControl ID="ckEditor" BasePath="/ckeditor/" runat="server" CssClass="col-xs-12"></CKEditor:CKEditorControl>
                        <asp:Label ID="lblHistory" runat="server" />
                    </div>
                </div>
            </div>

            <div class="row col-lg-12" id="divNoCharacter" runat="server">
                <h1>There is no character selected.</h1>
            </div>
        </div>
        <br />

        <div class="row text-right" style="margin-bottom: 20px;" runat="server" id="divSaveButtons">
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button ID="btnSubmit" runat="server" Text="Save And Submit" OnCommand="ProcessButton" CommandName="Submit" CssClass="btn btn-primary" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnCommand="ProcessButton" CommandName="Save" CssClass="btn btn-primary" Style="margin-left: 25px;" />
            </div>
        </div>
    </div>

    <div class="divide30"></div>
    <div id="push"></div>



    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Character History</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidNotificationEMail" runat="server" Value="" />

    <!-- /#page-wrapper -->
</asp:Content>
