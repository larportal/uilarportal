<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Demographics.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Demographics" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Information<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="margin20"></div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Campaign Demographics</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12 margin10">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <label for="<%= tbCampaignName.ClientID %>">Campaign Name</label>
                                                <asp:TextBox ID="tbCampaignName" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= tbLARPPortalType.ClientID %>">LARP Portal Type</label>
                                                <asp:TextBox ID="tbLARPPortalType" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <label for="<%= ddlGameSystem.ClientID %>">Game System</label>
                                                <asp:DropDownList ID="ddlGameSystem" runat="server" CssClass="form-control" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= ddlCampaignStatus.ClientID %>">Campaign Status</label>
                                                <asp:DropDownList ID="ddlCampaignStatus" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <label for="<%= tbOwner.ClientID %>">Owner</label>
                                                <asp:TextBox ID="tbOwner" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= tbDateStarted.ClientID %>">Date Started</label>
                                                <asp:TextBox ID="tbDateStarted" runat="server" CssClass="form-control" />
                                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateStarted" Format="MM/dd/yyyy" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:HiddenField ID="hidCampaignZip" runat="server" />
                                            </div>
                                            <div class="col-lg-3">
                                                <label for="<%= tbExpectedEndDate.ClientID %>">Exp.End Date</label>
                                                <asp:TextBox ID="tbExpectedEndDate" runat="server" CssClass="form-control" />
                                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbExpectedEndDate" Format="MM/dd/yyyy" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 text-right">
                    <asp:Button ID="btnSaveChanges" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveChanges_Click" /><%-- OnClick="btnSaveChanges_Click" />--%>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title text-center">LARPortal Character Info</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblmodalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary"  />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
