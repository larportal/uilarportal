<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharCardOrder.aspx.cs" Inherits="LarpPortal.Character.CharCardOrder" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharCardOrderStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style type="text/css">
        #charCardTable {
            font-size: .9em;
        }
    </style>
</asp:Content>

<asp:Content ID="CharCardOrderScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CharCardOrderBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Character Card Sort Order</h1>
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
                        <div class="panel-heading">Character Card Display Order</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvSkills" runat="server" AutoGenerateColumns="false" GridLines="None"
                                HeaderStyle-Wrap="false" CssClass="table table-striped table-hover table-condensed">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidSkillID" runat="server" Value='<%# Eval("CharacterSkillID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Skill Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("SkillName") %>' ToolTip='<%# Eval("DisplayOrder") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Skill Card Description" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSkillDesc" runat="server" Text='<%# Eval("SkillCardDescription") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sort Order">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="tbSortOrder" Text='<%# Eval("SortOrder") %>' CssClass="form-control" />
                                        </ItemTemplate>
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
    <!-- /#page-wrapper -->
</asp:Content>
