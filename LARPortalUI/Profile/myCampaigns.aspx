<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="myCampaigns.aspx.cs" Inherits="LarpPortal.Profile.myCampaigns" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="myCampaignsStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .ErrorDisplay {
            font-weight: bold;
            font-style: italic;
            font-size: large;
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="myCampaignsScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function openErrorMessage() {
            $('#modalErrorMessage').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="myCampaignsBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Select Campaigns To Display</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="text-right">
                    <div class="col-xs-12">
                        <asp:Button ID="btnSaveTop" runat="server" Text="Save" CssClass="btn btn-lg btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Player Information</div>
                        <div class="panel-body">
                            <div class="container-fluid">
                                <div class="row" style="padding-top: 15px;">
                                    <asp:GridView ID="gvCampaigns" runat="server" CssClass="table table-striped table-hover" GridLines="None"
                                        OnRowDataBound="gvCampaigns_RowDataBound" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="CampaignName" HeaderText="Campaign Name" HtmlEncode="False" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input type="checkbox" data-toggle="toggle" data-size="small" runat="server" name="swDisplayCampaign" id="swDisplayCampaign" />
                                                    <asp:HiddenField id="hidCampaignPlayerID" runat="server" Value='<%# Eval("CampaignPlayerID") %>' />
                                                    <asp:HiddenField ID="hidCampaignID" runat="server" Value='<%# Eval("CampaignID") %>' />
                                                    <asp:HiddenField ID="hidDisplay" runat="server" Value='<%# Eval("UserDisplayMyCampaigns") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <p class="text-right">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    </p>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="divide30"></div>
            <div id="push"></div>
        </div>
    </div>

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Profile myCampaign Setup</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
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

    <div class="modal fade in" id="modalErrorMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Profile myCampaign Setup - Error</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label CssClass="ErrorDisplay" ID="lblErrorMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Button ID="btnCloseErrorMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
