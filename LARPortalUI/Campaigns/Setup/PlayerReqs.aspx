﻿<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="PlayerReqs.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.PlayerReqs" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsRequirementsStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CampaignsRequirementsScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CampaignsRequirementsBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Requirements<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="margin20"></div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Requirements</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-3">
                                        <label for="<%=tbMembershipFee.ClientID %>">Membership Fee</label>
                                        <asp:TextBox ID="tbMembershipFee" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="<%= ddlFrequency %>">Frequency</label>
                                        <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Annual" Value="Annual" />
                                            <asp:ListItem Text="Event" Value="Event" />
                                            <asp:ListItem Text="None" Value="None" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-3">
                                        <label for="<%= tbMinAge %>">Minimum Age</label>
                                        <asp:TextBox ID="tbMinAge" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="<%= tbSuperAge.ClientID %>">Supervised Age</label>
                                        <asp:TextBox ID="tbSuperAge" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
<%--                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-6">
                                        <label for="<%= ddlWaiver.ClientID %>">Waivers</label>
                                        <asp:DropDownList ID="ddlWaiver" CssClass="form-control" runat="server" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 text-right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
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
