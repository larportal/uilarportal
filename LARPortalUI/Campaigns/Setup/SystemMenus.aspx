<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="SystemMenus.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.SystemMenus" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="SystemMenusStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .ErrorDisplay {
            font-weight: bold;
            font-style: italic;
            font-size: large;
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="SystemMenusScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function postBackByObject() {
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                hidScrollPos.value = $get('<%=pnlTreeView.ClientID%>').scrollTop;
            }
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }


        function scrollTree() {
            var pnlTreeView = document.getElementById('<%=pnlTreeView.ClientID%>');
            var hidScrollPos = document.getElementById('<%= hidScrollPos.ClientID%>');
            if (hidScrollPos != null) {
                pnlTreeView.scrollTop = hidScrollPos.value;
            }
        }


        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function openSaveMessage() {
            $('#modalSaveMessage').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="SystemMenusBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Select Menus To Display for <asp:Label ID="lblCampaignName" runat="server" /></h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="text-right">
                    <div class="col-xs-12">
                        <asp:Button ID="btnSaveTop" runat="server" Text="Save" CssClass="btn btn-lg btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">System Menus</div>
                        <div class="panel-body">
                            <div class="row" style="padding-top: 15px;">
                                <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Auto" Height="350px">
                                    <asp:TreeView ID="tvSystemMenus" runat="server" SkipLinkText="" BorderColor="Black" BorderStyle="Solid" BorderWidth="0"
                                        ShowLines="false" OnTreeNodeCheckChanged="tvSystemMenus_TreeNodeCheckChanged" Font-Underline="false" CssClass="form-control" EnableClientScript="false"
                                        LeafNodeStyle-CssClass="TreeItems" NodeStyle-CssClass="TreeItems">
                                        <LevelStyles>
                                            <asp:TreeNodeStyle Font-Underline="false" />
                                        </LevelStyles>
                                    </asp:TreeView>

                                </asp:Panel>

                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <p class="text-right">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divide30"></div>
            <div id="push"></div>
        </div>
    </div>

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
<%--                    <a class="close" data-dismiss="modal">&times;</a>--%>
                    <h3 class="modal-title text-center">Campaign Setup System Menus</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCloseMessage_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalSaveMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
<%--                    <a class="close" data-dismiss="modal">&times;</a>--%>
                    <h3 class="modal-title text-center">Campaign Setup System Menus</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblSaveMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Button ID="btnCloseSaveMessage" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCloseSaveMessage_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /#page-wrapper -->
    <asp:HiddenField runat="server" ID="hidScrollPos" />
</asp:Content>
