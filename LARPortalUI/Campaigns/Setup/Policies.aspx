<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Policies.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Policies" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsPoliciesStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CampaignsPoliciesScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CampaignsPoliciesBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Information<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <div class="margin20"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Policies</div>
                    <div class="panel-body">
                        <div class="row" style="padding-left: 10px; padding-right: 10px;">
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkAllowCharacterRebuilds" id="chkAllowCharacterRebuilds" runat="server" />
                                    <label for="<%= chkAllowCharacterRebuilds.ClientID %>">Allow Character Rebuilds</label>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkAllowCPDonation" id="chkAllowCPDonation" runat="server" />
                                    <label for="<%= chkAllowCPDonation.ClientID %>">Allow CP Donation</label>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkShareLocationUseNotes" id="chkShareLocationUseNotes" runat="server" />
                                    <label for="<%= chkShareLocationUseNotes.ClientID %>">Share Location Use Notes</label>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkNPCApprovalRequired" id="chkNPCApprovalRequired" runat="server" />
                                    <label for="<%= chkNPCApprovalRequired.ClientID %>">NPC Approval Required</label>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkUseCampaignCharacters" id="chkUseCampaignCharacters" runat="server" />
                                    <label for="<%= chkUseCampaignCharacters.ClientID %>">Use Campaign Characters</label>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkPCApprovalRequired" id="chkPCApprovalRequired" runat="server" />
                                    <label for="<%= chkPCApprovalRequired.ClientID %>">PC Approval Required</label>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 SliderBoxBorder">
                                <div class="SliderSwitch">
                                    <input type="checkbox" data-toggle="toggle" data-size="small" name="chkAllowAddInfo" id="chkAllowAddInfo" runat="server" />
                                    <label for="<%= chkAllowAddInfo.ClientID %>">Allow Additional Info</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= ddlPELApprovalLevel.ClientID %>">PEL Approval Level</label>
                                <asp:DropDownList ID="ddlPELApprovalLevel" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="1" Value="1" />
                                    <asp:ListItem Text="This is a long one" Value="2" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= ddlCharacterApprovalLevel.ClientID %>">Character Approval Level</label>
                                <asp:DropDownList ID="ddlCharacterApprovalLevel" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Something goes here" Value="1" />
                                    <asp:ListItem Text="More things go here." Value="2" />
                                </asp:DropDownList>
                            </div>

                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbEarliestCPApplicationYear.ClientID %>">Earliest Point Application Year</label>
                                <asp:TextBox ID="tbEarliestCPApplicationYear" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbEarliestCPApplicationYear" runat="server" Enabled="True" TargetControlID="tbEarliestCPApplicationYear" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbEventCharacterCap.ClientID %>">Event Character Cap</label>
                                <asp:TextBox ID="tbEventCharacterCap" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbEventCharacterCap" runat="server" Enabled="True" TargetControlID="tbEventCharacterCap" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbMaximumCPPerYear.ClientID %>">Max Points Per Year</label>
                                <asp:TextBox ID="tbMaximumCPPerYear" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbMaximumCPPerYear" runat="server" Enabled="True" TargetControlID="tbMaximumCPPerYear" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbTotalCharacterCap.ClientID %>">Total Character Cap</label>
                                <asp:TextBox ID="tbTotalCharacterCap" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbTotalCharacterCap" runat="server" Enabled="True" TargetControlID="tbTotalCharacterCap" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-6 col-md-12 col-xs-12">
                                <label for="<%= tbCrossCampaignPosting.ClientID %>">Cross Campaign Posting</label>
                                <asp:TextBox ID="tbCrossCampaignPosting" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbCrossCampaignPosting" runat="server" Enabled="True" TargetControlID="tbCrossCampaignPosting" FilterType="Numbers" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="col-xs-12">
                                <div class="text-right">
                                    <asp:Button ID="btnSaveRepeat" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveChanges_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="margin20"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">Policies</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 text-right">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
