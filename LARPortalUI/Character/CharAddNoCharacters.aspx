﻿<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharAddNoCharacters.aspx.cs" 
    Inherits="LarpPortal.Character.CharAddNoCharacters" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharAddStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>

<asp:Content ID="CharAddScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }

        function openCharAdd() {
            $('#modalCharSaved').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CharAddBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Create New Character</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
                <asp:MultiView ID="mvCharacterCreate" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwCreateCharacter" runat="server">
                        <asp:UpdatePanel ID="upJoinCampaign" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="row">
                                            <!-- Create a new character. -->
                                            <div class="form-check">
<%--                                                <asp:RadioButton ID="rbNewCharacter" runat="server" GroupName="CharacterRadios" CssClass="form-check-input"
                                                    AutoPostBack="true" OnCheckedChanged="RadioButtonCheckedChanged" />--%>
<%--                                                <label class="form-check-label" for='<%# rbNewCharacter.ClientID %>'>Create a new character.</label>--%>
                                                <asp:Panel ID="pnlAddCharacter" runat="server" Visible="true">
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label for='<%# ddlUserCampaigns.ClientID %>' class="control-label">Campaign</label>
                                                            <asp:DropDownList ID="ddlUserCampaigns" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlUserCampaigns_SelectedIndexChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6" runat="server" id="divPlayerType">
                                                        <div class="form-group">
                                                            <label for="ddlCharacterType" class="control-label">Character Type</label>
                                                            <asp:DropDownList ID="ddlCharacterType" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlCharacterType_SelectedIndexChanged">
                                                                <asp:ListItem Text="PC" Value="1" Selected="true" />
                                                                <asp:ListItem Text="NPC" Value="2" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6" runat="server" id="divPlayer">
                                                        <div class="form-group">
                                                            <label for="ddlPlayer" class="control-label">Player</label>
                                                            <asp:DropDownList ID="ddlPlayer" runat="server" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="rfvddlPlayer" runat="server" ControlToValidate="ddlPlayer" InitialValue=""
                                                                ErrorMessage="* Required" Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Character Name</label>
                                                            <asp:TextBox ID="tbCharacterName" runat="server" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="rfvCharacterName" runat="server" ControlToValidate="tbCharacterName" InitialValue=""
                                                                ErrorMessage="* Required" Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Font-Size="larger" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6" style="display: none;">
                                                        <div class="form-group">
                                                            <label>Skill Set Name</label>
                                                            <asp:TextBox ID="tbNewCharSkillSetName" runat="server" CssClass="form-control" />
<%--                                                            <asp:RequiredFieldValidator ID="rfNewCharSkillSetName" runat="server" ControlToValidate="tbNewCharSkillSetName" InitialValue=""
                                                                ErrorMessage="* Required" Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Font-Size="larger" />--%>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6" style="display: none;">
                                                        <div class="form-group">
                                                            <label>Skill Set Type</label>
                                                            <asp:DropDownList ID="ddlNewCharSkillSetType" runat="server" CssClass="form-control" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <div class="bg-classes">
                                                            <p class="bg-info text-center">The character name is the name by which your character is commonly known. You will be able to enter a different first, middle and last name after saving the screen.</p>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12 text-right">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 text-center" style="font-size: 16pt; color: red;">
                                        <asp:Label ID="lblMessage" runat="server" />
                                    </div>
                                </div>

                                <div class="modal fade in" id="modalCharSaved" role="dialog">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <a class="close" data-dismiss="modal">&times;</a>
                                                <h3 class="modal-title text-center">LARPortal Add Character</h3>
                                            </div>
                                            <div class="modal-body">
                                                <p>
                                                    <asp:Label ID="lblCharAdded" runat="server" />
                                                </p>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <asp:Button ID="btnCloseCharAdded" runat="server" Text="Close" CssClass="btn btn-primary" PostBackUrl="CharInfo.aspx" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="modal fade in" id="modalMessage" role="dialog">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <a class="close" data-dismiss="modal">&times;</a>
                                                <h3 class="modal-title text-center">LARPortal Add Character</h3>
                                            </div>
                                            <div class="modal-body">
                                                <p>
                                                    <asp:Label ID="lblmodalMessage" runat="server" Visible="true" />
                                                </p>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                        </div>
                    </asp:View>
                    <asp:View ID="vwNoCampaigns" runat="server">
                        <div class="row col-xs-12">
                            <h1 class="text-center">You are currently not a member of any campaigns.</h1>
                        </div>
                        <div class="row col-sm-12">
                            <h1 class="text-center">You cannot create a character until you have joined a campaign.</h1>
                        </div>

                        <div class="row col-sm-12">
                            <h1 class="text-center">Click <a href="../Campaigns/JoinACampaign.aspx" style="background-color: white;">here</a> to request to join a campaign.</h1>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="col-xs-2">
            </div>
        </div>
        <div id="push"></div>
    </div>

    <asp:HiddenField ID="hidNumCharacters" runat="server" />
<%--    <CharSelector:Select ID="oCharSelect" runat="server" />--%>
    <!-- /#page-wrapper -->
</asp:Content>
