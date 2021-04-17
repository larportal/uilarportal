<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Description.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Description" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsDescriptionStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="CampaignsDescriptionScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CampaignsDescriptionBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Description<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
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
                    <div class="panel-heading">Web Page Description</div>
                    <div class="panel-body">
                        <asp:TextBox ID="tbWebPageDescription" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="10" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-right">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
        <div id="push"></div>

        <div class="modal fade in" id="modalMessage" role="dialog">
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
