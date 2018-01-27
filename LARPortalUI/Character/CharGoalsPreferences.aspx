<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharGoalsPreferences.aspx.cs" Inherits="LarpPortal.Character.CharGoalsPreferences" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharUserDefinedStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharUserDefinedScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CharUserDefinedBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Character Goals &amp; Preferences</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 margin20">
                    <div class="input-group">
                        <CharSelector:Select ID="oCharSelect" runat="server" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6" id="divUserDef1" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="lblUserDef1" runat="server" />
                        </div>
                        <div class="panel-body">
                            <asp:TextBox ID="tbUserField1" runat="server" Style="width: 100%" Rows="4" TextMode="MultiLine" CssClass="form-control" />
                            <asp:Label ID="lblUserField1" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="divUserDef2" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="lblUserDef2" runat="server" />
                        </div>
                        <div class="panel-body">
                            <asp:TextBox ID="tbUserField2" runat="server" Style="width: 100%" Rows="4" TextMode="MultiLine" CssClass="form-control" />
                            <asp:Label ID="lblUserField2" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="divUserDef3" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="lblUserDef3" runat="server" />
                        </div>
                        <div class="panel-body">
                            <asp:TextBox ID="tbUserField3" runat="server" Style="width: 100%" Rows="4" TextMode="MultiLine" CssClass="form-control" />
                            <asp:Label ID="lblUserField3" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="divUserDef4" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="lblUserDef4" runat="server" />
                        </div>
                        <div class="panel-body">
                            <asp:TextBox ID="tbUserField4" runat="server" Style="width: 100%" Rows="4" TextMode="MultiLine" CssClass="form-control" />
                            <asp:Label ID="lblUserField4" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="divUserDef5" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="lblUserDef5" runat="server" />
                        </div>
                        <div class="panel-body">
                            <asp:TextBox ID="tbUserField5" runat="server" Style="width: 100%" Rows="4" TextMode="MultiLine" CssClass="form-control" />
                            <asp:Label ID="lblUserField5" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

            <%--            <div class="col-md-6">
                <div class="pre-scrollable" style="height: 350px;">
                    <asp:TreeView ID="idTree" runat="server" CssClass="form-control table" BorderStyle="None">
                        <Nodes>
                            <asp:TreeNode Text="1st one" />
                            <asp:TreeNode Text="2nd one">
                                <asp:TreeNode Text="Here's another one" />
                                <asp:TreeNode Text="Done" />
                                <asp:TreeNode Text="2nd one">
                                    <asp:TreeNode Text="Here's another one" />
                                    <asp:TreeNode Text="Done">
                                        <asp:TreeNode Text="2nd one">
                                            <asp:TreeNode Text="Here's another one" />
                                            <asp:TreeNode Text="Done">
                                                <asp:TreeNode Text="2nd one">
                                                    <asp:TreeNode Text="Here's another one" />
                                                    <asp:TreeNode Text="Done">
                                                        <asp:TreeNode Text="2nd one">
                                                            <asp:TreeNode Text="Here's another one" />
                                                            <asp:TreeNode Text="Done">
                                                                <asp:TreeNode Text="2nd one">
                                                                    <asp:TreeNode Text="Here's another one" />
                                                                    <asp:TreeNode Text="Done">
                                                                        <asp:TreeNode Text="2nd one">
                                                                            <asp:TreeNode Text="Here's another one" />
                                                                            <asp:TreeNode Text="Done"></asp:TreeNode>
                                                                        </asp:TreeNode>
                                                                    </asp:TreeNode>
                                                                </asp:TreeNode>
                                                            </asp:TreeNode>
                                                        </asp:TreeNode>
                                                    </asp:TreeNode>
                                                </asp:TreeNode>
                                            </asp:TreeNode>
                                        </asp:TreeNode>
                                    </asp:TreeNode>
                                </asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </div>
            </div>
            --%>

            <div class="row">
                <div class="col-md-12">
                    <p class="text-right">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Changes" OnClick="btnSubmit_Click" />
                    </p>
                </div>
            </div>

        </div>
        <div class="divide30"></div>

        <div class="modal fade" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title text-center">LARPortal Character Goals &amp; Preferences</h3>
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
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
