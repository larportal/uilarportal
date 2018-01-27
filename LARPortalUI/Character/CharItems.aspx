<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharItems.aspx.cs" Inherits="LarpPortal.Character.CharItems" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharItemsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharItemsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    function openMessage() {
            $('#modalMessage').modal('show');
        }
</asp:Content>

<asp:Content ID="CharItemsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Character Items</h1>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-10 margin20">
                        <div class="input-group">
                            <CharSelector:Select ID="oCharSelect" runat="server" />
                        </div>
                    </div>
                    <div class="text-right">
                        <asp:Button ID="btnSaveTop" runat="server" Text="Save" CssClass="btn btn-lg btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-9">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">Costumes</div>
                                <div class="panel-body">
                                    <asp:TextBox ID="tbCostume" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    <asp:Label ID="lblCostume" runat="server" CssClass="form-control" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">Make-up</div>
                                <div class="panel-body">
                                    <asp:TextBox ID="tbMakeup" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    <asp:Label ID="lblMakeup" runat="server" CssClass="form-control" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">Accessories</div>
                                <div class="panel-body">
                                    <textarea rows="4" class="form-control" style="min-width: 100%"></textarea>
                                    <asp:TextBox ID="tbAccessories" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    <asp:Label ID="lblAccessories" runat="server" CssClass="form-control" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">Weapons and Armor</div>
                                <div class="panel-body">
                                    <asp:TextBox ID="tbWeapons" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    <asp:Label ID="lblWeapons" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">Other Items</div>
                                <div class="panel-body">
                                    <textarea rows="4" class="form-control" style="min-width: 100%"></textarea>
                                    <asp:TextBox ID="tbOtherItems" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    <asp:Label ID="lblOtherItems" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 margin20">
                    <div class="row" style="padding-left: 10px;" id="divAddPicture" runat="server">
                        <div class="panel-wrapper">
                            <div class="uploadFile col-xs-12">
                                <div class="row">
                                    <span class="input-group-btn">
                                        <asp:FileUpload ID="fuItem" runat="server" CssClass="btn btn-primary btn-sm btnFile col-sm-6" ToolTip="Here's where you specify the file name." />
                                        <span class="col-sm-1">&nbsp;</span>
                                        <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload File" OnClick="btnUpload_Click" />
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding-left: 10px;">
                        <div class="col-xs-12">
                            <div class="center-block pre-scrollable scroll-500" style="display: inline-block; overflow-y: auto;">
                                <asp:DataList ID="dlItems" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" OnDeleteCommand="dlItems_DeleteCommand">
                                    <AlternatingItemStyle BackColor="Transparent" />
                                    <ItemStyle BorderColor="Transparent" BorderWidth="20" />
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("PictureURL") %>' runat="server" Width="100" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Remove" CommandName="Delete" CommandArgument='<%# Eval("PictureID") %>' /></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <p class="text-right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-lg btn-primary" OnClick="btnSave_Click" />
                    </p>
                </div>
            </div>
        </div>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>

    <div class="modal fade" id="modalMessage" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3>Character Items</h3>
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
    </div>

    <!-- /#page-wrapper -->
</asp:Content>
