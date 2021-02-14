<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="SkillSets.aspx.cs" Inherits="LarpPortal.Character.SkillSets" %>

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

        function openDeleteSkillSet() {
            $('#modalDeleteSkillSet').modal('show');
        }

        function changeBackColor(item) {
            item.style.background = "lightpink";
        }
    </script>
</asp:Content>

<asp:Content ID="CharCardOrderBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Character Skill Sets</h1>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-10 margin20 text-center">
                    <asp:Label ID="lblProblem" runat="server" Font-Bold="true" Font-Size="16pt" ForeColor="Red" />
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
                            <asp:GridView ID="gvSkills" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvSkills_RowDataBound"
                                HeaderStyle-Wrap="false" CssClass="table table-striped table-hover table-condensed">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidSkillID" runat="server" Value='<%# Eval("CharacterSkillSetID") %>' />
                                            <asp:HiddenField ID="hidCharacterID" runat="server" Value='<%# Eval("CharacterID") %>' />
                                            <asp:HiddenField ID="hidCampaignID" runat="server" Value='<%# Eval("CampaignID") %>' />
                                            <asp:HiddenField ID="hidSkillSetName" runat="server" Value='<%# Eval("CharacterSkillSetName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Character Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("CharacterNameCampaign") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Skill Set Name" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbSkillSetName" runat="server" Text='<%# Eval("CharacterSkillSetName") %>' Columns="45" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Skill Set Type" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidSkillSetTypeID" runat="server" Value='<%# Eval("CharacterSkillSetTypeID") %>' />
                                            <asp:DropDownList ID="ddlSkillSetType" runat="server" Width="200" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDuplicate" Text="* Duplicate Skill Name" Visible="false" Font-Bold="true" Font-Italic="true" Font-Size="12pt" />
                                            <asp:Label runat="server" ID="lblMultiplePrimary" Text="* Multiple Primary" Visible="false" Font-Bold="true" Font-Italic="true" Font-Size="12pt"   />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteSkillSet" runat="server" OnCommand="btnDeleteSkillSet_Command" 
                                                CommandName='<%# Eval("CharacterSkillSetName") %>'
                                                CommandArgument='<%# Eval("CharacterSkillSetID") %>' CssClass="btn btn-primary btn-sm" 
                                                Text="Delete Skill Set" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <span style="color: red; font-weight: bold; font-size: 24px;">The character has no skill sets defined.</span>
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

    <div class="modal fade" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">LARPortal Skill Sets Customization</h3>
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

    <div class="modal fade" id="modalDeleteSkillSet" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Skills Sets - Delete Skill Set</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblDeleteSkillSetMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-6 text-left">
                            <asp:Button ID="btnDeleteCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-xs-6 text-right">
                            <asp:HiddenField ID="hidDeleteSkillSetID" runat="server" />
                            <asp:Button ID="btnDeleteSkillSet" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteSkillSet_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <CharSelector:Select ID="oCharSelect" runat="server" />

  <!-- /#page-wrapper -->
</asp:Content>
