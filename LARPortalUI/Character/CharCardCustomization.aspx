<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharCardCustomization.aspx.cs" Inherits="LarpPortal.Character.CharCardCustomization" 
        MaintainScrollPositionOnPostback="true" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharCardCustomStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style type="text/css">
        #charCardTable {
            font-size: .9em;
        }
    </style>
</asp:Content>
<asp:Content ID="CharCardCustomScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="CharCardCustomBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Character Card Customization</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-10 margin20">
                    <CharSelector:Select ID="oCharSelect" runat="server" />
                </div>
                <div class="col-md-2 text-right">
                    <asp:Button ID="btnSave_Top" runat="server" CssClass="btn btn-lg btn-primary" Text="Save Changes" OnClick="btnSave_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Character Card Customization</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvSkills" runat="server" AutoGenerateColumns="false" OnRowCancelingEdit="gvSkills_RowCancelingEdit" OnDataBound="gvSkills_DataBound"
                                OnRowEditing="gvSkills_RowEditing" OnRowUpdating="gvSkills_RowUpdating" OnRowDataBound="gvSkills_RowDataBound" GridLines="None"
                                HeaderStyle-Wrap="false" CssClass="table table-responsive table-striped">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidSkillID" runat="server" Value='<%# Eval("CharacterSkillID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Skill Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("SkillName") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("SkillName") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="right" ItemStyle-Wrap="false" ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <a href="#" data-toggle="tooltip" title="Should the standard description be displayed?">
                                                <asp:Image ID="imgDisplay" runat="server" ImageUrl='<%# Boolean.Parse(Eval("CardDisplayDescription").ToString()) ? "../../img/checkbox.png" : "../../img/uncheckbox.png" %>' ToolTip="Should the Skill Card Description Be Displayed" />
                                            </a>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbDisplayDesc" runat="server" Text="" CssClass="NoPadding" Checked='<%# Eval("CardDisplayDescription") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Skill Card Description" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSkillDesc" runat="server" Text='<%# Eval("SkillCardDescription") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="right" ItemStyle-Wrap="false" ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <a href="#" data-toggle="tooltip" title="Should the standard incant be displayed?">
                                                <asp:Image ID="imgIncant" runat="server" ImageUrl='<%# Boolean.Parse(Eval("CardDisplayIncant").ToString()) ? "../../img/checkbox.png" : "../../img/uncheckbox.png" %>' />
                                            </a>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbDisplayIncant" runat="server" Text="" CssClass="NoPadding" Checked='<%# Eval("CardDisplayIncant") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Skill Incant" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIncantDesc" runat="server" Text='<%# Eval("SkillIncant") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Card Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCardDesc" runat="server" Text='<%# Eval("PlayerDescription") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="tbPlayDesc" runat="server" Text='<%# Eval("PlayerDescription") %>' BorderColor="Black" BorderStyle="Solid" BorderWidth="1" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Card Incant">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCardIncant" runat="server" Text='<%# Eval("PlayerIncant") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="tbPlayIncant" runat="server" Text='<%# Eval("PlayerIncant") %>' BorderColor="Black" BorderStyle="Solid" BorderWidth="1" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-primary" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnupdate2" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-primary" />
                                            <asp:Button ID="btncancel2" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-primary" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <span style="color: red; font-weight: bold; font-size: 24px;">The character has no skills defined.</span>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <p class="text-right">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Changes" OnClick="btnSave_Click" />
                    </p>
                </div>
            </div>
        </div>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">LARPortal Character Card Customization</h3>
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

    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $('[data-toggle="popover"]').popover({
            placement: 'top',
            trigger: 'hover'
        });
    </script>

</asp:Content>
