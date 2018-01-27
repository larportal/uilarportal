<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Contacts" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsContactsStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .form-group input[type="checkbox"] {
            display: none;
        }

            .form-group input[type="checkbox"] + .btn-group > label span {
                width: 20px;
            }

                .form-group input[type="checkbox"] + .btn-group > label span:first-child {
                    display: none;
                }

                .form-group input[type="checkbox"] + .btn-group > label span:last-child {
                    display: inline-block;
                }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:first-child {
                display: inline-block;
            }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:last-child {
                display: none;
            }
    </style>
</asp:Content>

<asp:Content ID="CampaignsContactsScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CampaignsContactsBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Information<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <%--        <div class="row">
            <div class="col-xs-12">
                //<CampSelector:Select ID="oCampSelect" runat="server" />
            </div>
        </div>--%>
        <div class="margin20"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Contacts</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12 margin10">
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbInfoRequestEmail.ClientID %>">Campagin Info Email: </label>
                                            <asp:TextBox ID="tbInfoRequestEmail" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkInfoRequestEmail" id="chkInfoRequestEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkInfoRequestEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkInfoRequestEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label>Campaign URL: </label>
                                            <asp:TextBox ID="tbCampaignURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbCharacterHistoryEmail.ClientID %>">Character History Email: </label>
                                            <asp:TextBox ID="tbCharacterHistoryEmail" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkCharacterHistoryEmail" id="chkCharacterHistoryEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkCharacterHistoryEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkCharacterHistoryEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbCharacterHistoryURL.ClientID %>">Character History URL: </label>
                                            <asp:TextBox ID="tbCharacterHistoryURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbCharacterNotificationEmail.ClientID %>">Character Notification Email: </label>
                                            <asp:TextBox ID="tbCharacterNotificationEmail" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkCharacterNotificationEmail" id="chkCharacterNotificationEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkCharacterNotificationEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkCharacterNotificationEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbCharacterGeneratorURL.ClientID %>">Character Generator URL: </label>
                                            <asp:TextBox ID="tbCharacterGeneratorURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbCPEmail.ClientID %>">CP Notification Email: </label>
                                            <asp:TextBox ID="tbCPEmail" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="col-xs-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkCPEmail" id="chkCPEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkCPEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkCPEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbRulesURL.ClientID %>">Rules URL: </label>
                                            <asp:TextBox ID="tbRulesURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbInfoRequestEmail.ClientID %>">Info Skill Email: </label>
                                            <asp:TextBox ID="tbInfoSkillEmail" runat="server" CssClass="form-control" />

                                        </div>
                                    </div>
                                    <div class="col-xs-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkInfoSkillEmail" id="chkInfoSkillEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkInfoSkillEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkInfoSkillEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbInfoSkillEmail.ClientID %>">Info Skill URL: </label>
                                            <asp:TextBox ID="tbInfoSkillURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbJoinRequestEmail.ClientID %>">Join Request Email: </label>
                                            <asp:TextBox ID="tbJoinRequestEmail" runat="server" CssClass="form-control" />

                                        </div>
                                    </div>
                                    <div class="col-xs-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkJoinRequestEmail" id="chkJoinRequestEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkJoinRequestEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkJoinRequestEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbJoinURL.ClientID %>">Join Request URL: </label>
                                            <asp:TextBox ID="tbJoinURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbPELNotificationEmail.ClientID %>">PEL Notification Email: </label>
                                            <asp:TextBox ID="tbPELNotificationEmail" runat="server" CssClass="form-control" />

                                        </div>
                                    </div>
                                    <div class="col-xs-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkPELNotificationEmail" id="chkPELNotificationEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkPELNotificationEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkPELNotificationEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbJoinURL.ClientID %>">PEL Submission URL: </label>
                                            <asp:TextBox ID="tbPELSubmissionURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbProductionSkillEmail.ClientID %>">Production Skill Email: </label>
                                            <asp:TextBox ID="tbProductionSkillEmail" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="col-xs-2">
                                        <div class="form-group" style="margin-top: 23px;">
                                            <input type="checkbox" name="chkProductionSkillEmail" id="chkProductionSkillEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkProductionSkillEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkProductionSkillEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbProductionSkillURL.ClientID %>">Production Skill URL: </label>
                                            <asp:TextBox ID="tbProductionSkillURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbRegistrationNotificationEmail.ClientID %>">Registration Notification Email: </label>
                                            <asp:TextBox ID="tbRegistrationNotificationEmail" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="col-xs-2">
                                        <div class="form-group" style="padding-top: 23px;">
                                            <input type="checkbox" name="chkREgistrationNotificationEmail" id="chkREgistrationNotificationEmail" runat="server" />
                                            <div class="btn-group">
                                                <label for="<%= chkREgistrationNotificationEmail.ClientID%>" class="btn btn-default">
                                                    <span class="glyphicon glyphicon-ok"></span>
                                                    <span class="glyphicon glyphicon-unchecked"></span>
                                                </label>
                                                <label for="<%= chkREgistrationNotificationEmail.ClientID%>" class="btn btn-default active">
                                                    Show
                                   
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="form-group">
                                            <label for="<%= tbRegistrationURL.ClientID %>">Registration URL: </label>
                                            <asp:TextBox ID="tbRegistrationURL" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 text-right">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Web Page Description</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblmodalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>




    </div>
</asp:Content>
