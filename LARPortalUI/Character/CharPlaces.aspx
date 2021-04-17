<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharPlaces.aspx.cs" Inherits="LarpPortal.Character.CharPlaces" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharPlacesStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharPlacesScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openDelete(PlaceID, PlaceDesc) {
            $('#modalDelete').modal('show');

            try {
                var hidDeletePlaceID = document.getElementById('<%= hidDeletePlaceID.ClientID %>');
                if (hidDeletePlaceID) {
                    hidDeletePlaceID.value = PlaceID;
                }

                var lblDeletePlaceMessage = document.getElementById('<%= lblDeletePlaceMessage.ClientID %>');
                if (lblDeletePlaceMessage)
                    if (PlaceDesc)
                        lblDeletePlaceMessage.innerText = "Are you sure you want to delete the place " + PlaceDesc + " ?";
                    else
                        lblDeletePlaceMessage.innerText = "Are you sure you want to delete this place ?";
            }
            catch (err) {
                alert(err);
            }
            return false;
        }
    </script>

</asp:Content>

<asp:Content ID="CharPlacesBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character Relationships</h1>
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
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Places</div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSkill" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-xs-3">
                                        <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Vertical" Height="550px">
                                            <asp:TreeView ID="tvCampaignPlaces" runat="server" SkipLinkText="" BorderColor="Black" BorderStyle="Solid" BorderWidth="0" ShowCheckBoxes="none"
                                                ShowLines="false" Font-Underline="false" CssClass="form-control" OnSelectedNodeChanged="tvCampaignPlaces_SelectedNodeChanged">
                                                <LevelStyles>
                                                    <asp:TreeNodeStyle Font-Underline="false" />
                                                </LevelStyles>
                                            </asp:TreeView>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-xs-9">
                                        <div class="lead">
                                            <b>Character Places</b>
                                        </div>
                                        <asp:GridView ID="gvPlaces" runat="server" AutoGenerateColumns="false" GridLines="none" CssClass="col-xs-12 table table-striped table-hover table-condensed"
                                            AlternatingRowStyle-BackColor="Linen" BorderColor="Black" BorderWidth="0px" BorderStyle="Solid"
                                            OnRowCommand="gvPlaces_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="PlaceName" HeaderText="Place Name" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem" />
                                                <asp:BoundField DataField="LocaleName" HeaderText="Locale" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem" />
                                                <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem" />
                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="16" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" CommandName="EditItem" Width="16px"
                                                            CommandArgument='<%# Eval("CharacterPlaceID") %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="16" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteItem"
                                                            Width="16px" Height="16"
                                                            OnClientClick='<%# string.Format("return openDelete({0}, \"{1}\");",
                                                                    Eval("CharacterPlaceID"), Eval("PlaceName")) %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <span style="color: red; font-weight: bold; font-size: 24px;">The character has no places defined.</span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <br />
                                        <asp:HiddenField ID="hidCharacterPlaceID" runat="server" />
                                        <asp:HiddenField ID="hidCampaignPlaceID" runat="server" />
                                        <asp:MultiView ID="mvAddingItems" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="vwNewItemButton" runat="server">
                                                <asp:Button ID="btnAddNonCampPlace" runat="server" Text="Add New Place" OnClick="btnAddNonCampPlace_Click" CssClass="btn btn-primary" />
                                            </asp:View>

                                            <asp:View ID="vwNonCampaignPlace" runat="server">
                                                <div class="row">
                                                    <div class="col-xs-12 text-center lead">
                                                        Add a new non-character relationship for this character.
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="form-group">
                                                            <div class="controls">
                                                                <div class="row margin20">
                                                                    <div class="col-xs-4">
                                                                        <label>Place Name</label>
                                                                        <asp:TextBox ID="tbNonCampPlaceName" runat="server" CssClass="form-control" />
                                                                    </div>
                                                                    <div class="col-xs-4">
                                                                        <label>Located In</label>
                                                                        <asp:DropDownList ID="ddlNonCampLocalePlaces" runat="server" CssClass="form-control" />
                                                                    </div>
                                                                    <div class="col-xs-4">
                                                                        <label>Player Comments</label>
                                                                        <asp:TextBox ID="tbNonCampPlayerComments" runat="server" Rows="6" TextMode="MultiLine" CssClass="form-control" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-xs-6">
                                                                        <asp:Button ID="btnNonCampCancel" runat="server" Text="Cancel" OnClick="btnCancelAdding_Click" CssClass="btn btn-primary" /></td>
                                                                    </div>
                                                                    <div class="col-xs-6 text-right">
                                                                        <asp:Button ID="btnNonCampSave" runat="server" Text="Save New Place" OnClick="btnNonCampSave_Click" CssClass="btn btn-primary" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:View>

                                            <asp:View ID="vwCampaignPlace" runat="server">
                                                <div class="row">
                                                    <div class="col-xs-12 text-center lead">
                                                        Add a campaign place to this character.<asp:HiddenField ID="hidExistingID" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="form-group">
                                                            <div class="controls">
                                                                <div class="row margin20">
                                                                    <div class="col-xs-4">
                                                                        <label>Place Name</label>
                                                                        <asp:TextBox ID="tbCampaignPlaceName" runat="server" CssClass="form-control" Enabled="false" />
                                                                    </div>
                                                                    <div class="col-xs-3">
                                                                        <label>Locale</label>
                                                                        <asp:TextBox ID="tbCampaignLocale" runat="server" CssClass="form-control" Enabled="false" />
                                                                        <asp:HiddenField ID="hidCampaignLocaleID" runat="server" />
                                                                    </div>
                                                                    <div class="col-xs-5">
                                                                        <label>Player Comments</label>
                                                                        <asp:TextBox ID="tbCampaignPlayerComments" runat="server" CssClass="form-control" Rows="6" TextMode="MultiLine" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-xs-6">
                                                                        <asp:Button ID="btnCampaignCancel" runat="server" Text="Cancel" OnClick="btnCancelAdding_Click" CssClass="btn btn-primary" />
                                                                    </div>
                                                                    <div class="col-xs-6 text-right">
                                                                        <asp:Button ID="btnCampaignSave" runat="server" Text="Save Existing Place" OnClick="btnCampaignSave_Click" CssClass="btn btn-primary" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:View>
                                        </asp:MultiView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div id="push"></div>
    </div>

    <div class="modal fade in" id="modalDelete" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3>Character Places - Delete</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblDeletePlaceMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-6 NoGutters text-left">
                            <asp:Button ID="btnDeleteCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-xs-6 NoGutters text-right">
                            <asp:HiddenField ID="hidDeletePlaceID" runat="server" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDelete_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
