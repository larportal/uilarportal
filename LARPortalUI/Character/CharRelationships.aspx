<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharRelationships.aspx.cs" Inherits="LarpPortal.Character.CharRelationships" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharRelationshipsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharRelationshipsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function CheckForOther() {
            var RelSelect = document.getElementById("<%= ddlRelationship.ClientID %>");
            var selectedText = RelSelect.options[RelSelect.selectedIndex].text;
            var tbOther = document.getElementById("<%= tbOther.ClientID %>");

            if (selectedText == "Other")
                tbOther.style.display = "block";
            else
                tbOther.style.display = "none";
        }

        function CheckForOtherNonChar() {
            var RelSelect = document.getElementById("<%= ddlRelationshipNonChar.ClientID %>");
            var selectedText = RelSelect.options[RelSelect.selectedIndex].text;
            var tbOther = document.getElementById("<%= tbOtherNonChar.ClientID %>");

            if (selectedText == "Other")
                tbOther.style.display = "block";
            else
                tbOther.style.display = "none";
        }

        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function closeMessage() {
            $('#modelMessage').hide();
        }
    </script>
</asp:Content>

<asp:Content ID="CharRelationshipsBody" ContentPlaceHolderID="MainBody" runat="Server">
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
        <div class="col-md-12 bg-classes">
            <p class="bg-info text-center">Select an in game PC ro NPC character and the corresponding relationship from the lists provided - or enter the name of a person that is not on the list in the field provided.</p>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Relationships</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-3 pre-scrollable" runat="server" id="divList">
                                <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false" CssClass="col-xs-12 table table-striped table-hover table-condensed" DataKeyNames="CharacterID"
                                    AutoGenerateSelectButton="true" OnRowDataBound="gvList_RowDataBound" OnSelectedIndexChanged="gvList_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Character" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Larger">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCharacterAKA" runat="server" Text='<%# Eval("CharacterAKA") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-xs-9" style="padding-left: 10px;">
                                <div class="row">
                                    <asp:Panel ID="pnlRelation" runat="server" CssClass="col-xs-12">
                                        <div class="lead">
                                            <b>Character Relationships</b>
                                        </div>
                                        <asp:GridView ID="gvRelationships" runat="server" AutoGenerateColumns="false" GridLines="none" CssClass="col-xs-12 table table-striped table-hover table-condensed"
                                            AlternatingRowStyle-BackColor="Linen" BorderColor="Black" BorderWidth="1px" BorderStyle="none" OnRowCommand="gvRelationships_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="Name" HeaderText="Character Name" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem" />
                                                <asp:BoundField DataField="RelationDescription" HeaderText="Relationship" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem" />
                                                <asp:BoundField DataField="PlayerComments" HeaderText="Player Comments" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem" />
                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="16" ItemStyle-Wrap="false" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" CommandName="EditItem" Width="16px"
                                                            CommandArgument='<%# Eval("CharacterRelationshipID") %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="16" ItemStyle-Wrap="false" ItemStyle-CssClass="GridViewItem" HeaderStyle-CssClass="GridViewItem">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteItem" OnClientClick="if (!confirm('Are you sure you want to delete?')) return false;"
                                                            CommandArgument='<%# Eval("CharacterRelationshipID") %>' Width="16px" Height="16"><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <span style="color: red; font-weight: bold; font-size: 24px;">The character has no relationships defined.</span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>

                                <div class="row" style="padding-left: 20px;">
                                    <asp:HiddenField ID="hidRelateID" runat="server" />
                                    <asp:MultiView ID="mvAddingRelationship" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="vwNewRelateButton" runat="server">
                                            <asp:Button ID="btnAddNewRelate" runat="server" CssClass="btn btn-info" Text="Add Non-Character Relationship" OnClick="btnAddNewRelate_Click" />
                                        </asp:View>
                                        <asp:View ID="vwNewRelate" runat="server">
                                            <div class="row">
                                                <div class="col-xs-12 text-center lead">Add a new non-character relationship for this character.</div>
                                            </div>
                                            <div class="col-xs-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <div class="row">
                                                            <div class="col-xs-4">
                                                                <label>Name</label>
                                                                <asp:TextBox ID="tbCharacterName" runat="server" CssClass="form-control" />
                                                            </div>
                                                            <div class="col-xs-3">
                                                                <label>Relationship</label>
                                                                <asp:DropDownList ID="ddlRelationshipNonChar" runat="server" CssClass="form-control" />
                                                                <asp:TextBox ID="tbOtherNonChar" runat="server" CssClass="form-control" Style="padding-top: 10px;" />
                                                            </div>
                                                            <div class="col-xs-5">
                                                                <label>Player Comments</label>
                                                                <asp:TextBox ID="tbPlayerCommentsNonChar" runat="server" Rows="6" TextMode="MultiLine" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="row" style="padding-top: 10px;">
                                                            <div class="col-xs-6">
                                                                <asp:Button ID="btnCancelNonChar" CssClass="btn btn-info" runat="server" Text="Cancel" OnClick="btnCancelAdding_Click" />
                                                            </div>
                                                            <div class="col-xs-6 text-right">
                                                                <asp:Button ID="btnSaveNonChar" CssClass="btn btn-info" runat="server" Text="Save" OnClick="btnSaveNonChar_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                        </asp:View>

                                        <asp:View ID="vwExistingCharacter" runat="server">
                                            <div class="form-group">
                                                <div class="controls">
                                                    <div class="row">
                                                        <div class="col-xs-12 text-center">
                                                            <asp:Label ID="lblCharacter" runat="server" Font-Size="X-Large" Font-Bold="true" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-4">
                                                            <label>Relationship</label>
                                                            <asp:DropDownList ID="ddlRelationship" runat="server" CssClass="form-control" /><br />
                                                            <asp:TextBox ID="tbOther" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-xs-8">
                                                            <label>Player Comments</label>
                                                            <asp:TextBox ID="tbPlayerComments" runat="server" Rows="6" TextMode="MultiLine" CssClass="form-control" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding-top: 10px;">
                                                        <div class="col-xs-6">
                                                            <asp:Button ID="btnCancelAdding" runat="server" Text="Cancel" CssClass="btn btn-info" OnClick="btnCancelAdding_Click" />
                                                        </div>
                                                        <div class="col-xs-6 text-right">
                                                            <asp:Button ID="btnSaveExistingRelate" runat="server" Text="Save" CssClass="btn btn-info" OnClick="btnSaveExistingRelate_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:View>
                                    </asp:MultiView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--    <div class="row">
        <div class="col-md-12">
            <p class="text-right">
                <input type="submit" value="Save All Changes" class="btn btn-lg btn-primary">
            </p>
        </div>
    </div>--%>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Relationships</h3>
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
</asp:Content>
