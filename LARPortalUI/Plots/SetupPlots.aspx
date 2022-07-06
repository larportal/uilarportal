<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="True" Inherits="LarpPortal.Plots.SetupPlots" Codebehind="SetupPlots.aspx.cs" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="SetupPlotStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .option-button {
            height: 100%;
        }

        table .btn {
            padding-top: 0px;
            padding-bottom: 0px;
        }
    </style>
</asp:Content>

<asp:Content ID="SetupPlotScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="SetupPlotBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Setup Plots</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="form-inline col-xs-12">
                <div class="pull-left">
                    <asp:CheckBox ID="cbDisplayOnlyOpenPlots" Text=" Display Only Open Plots" AutoPostBack="true" runat="server" OnCheckedChanged="cbDisplayOnlyOpenPlots_CheckedChanged" />
                </div>
                <div class="pull-right">
                    <asp:Button ID="btnCreate" runat="server" Style="" Text='Create New Plot' CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                </div>
            </div>
        </div>
        <div class="divide10"></div>

        <div class="row">
            <div class="col-md-12">
                <asp:MultiView ID="mvPlotList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPlotList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Plot List</div>
                            <div class="panel-body">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="pre-scrollable" style="max-height: 65vh">
                                            <asp:GridView ID="gvPlotList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvPlotList_RowCommand" GridLines="None"
                                                CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid"
                                                OnRowDataBound="gvPlotList_RowDataBound" BorderWidth="1">
                                                <Columns>
                                                    <asp:BoundField DataField="PlotCode" HeaderText="Plot Code" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="PlotName" HeaderText="Plot Name" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="Writer" HeaderText="Writer" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="Scope" HeaderText="Scope" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Wrap="false" />
                                                    <%--<asp:BoundField DataField="StartDateTime" HeaderText="Event Date" DataFormatString="{0: MM/dd/yyyy}"
                                                        ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />--%>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEdit" Width="100px" runat="server" CommandArgument='<%# Eval("PlotID") %>' CommandName='EDIT'
                                                                Text='Edit' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnCancel" Width="100px" runat="server" CommandArgument='<%# Eval("PlotID") %>' CommandName='CANCELLED'
                                                                Text='Cancel' CssClass="btn btn-primary btn-sm option-button" OnClientClick="return confirm('Clicking OK will cancel all unclaimed donations for this event. They can not be recovered. Click OK to continue the cancel process.');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnComplete" Width="100px" runat="server" CommandArgument='<%# Eval("PlotID") %>' CommandName='COMPLETED'
                                                                Text='Complete' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwNoPlots" runat="server">
                        <p>
                            <strong>There are no plots for the campaign 
                            <asp:Label ID="lblCampaignName" runat="server" />.
                            </strong>
                        </p>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>

        <div class="modal fade in" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Setup Plots</h3>
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
