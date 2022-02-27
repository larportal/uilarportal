<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="SetupDonations.aspx.cs" Inherits="LarpPortal.Donations.SetupDonations" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="EventsSetupEventStyles" ContentPlaceHolderID="MainStyles" runat="server">
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

<asp:Content ID="EventsSetupEventScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="EventsSetupEventBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Setup Event Donations</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="form-inline col-xs-12">
                <div class="pull-left">
                    <asp:CheckBox ID="cbDisplayOnlyOpenEvents" Text="Display Only Open Events" AutoPostBack="true" runat="server" />
                </div>
                <div class="pull-right">
                    <asp:Button ID="btnCreate" runat="server" Style="" Text='Create New Donation List' CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                </div>
            </div>
        </div>
        <div class="divide10"></div>

        <div class="row">
            <div class="col-md-12">
                <asp:MultiView ID="mvEventList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwEventList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Event Donation List</div>
                            <div class="panel-body">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="pre-scrollable" style="max-height: 65vh">
                                            <asp:GridView ID="gvEventList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvEventList_RowCommand" GridLines="None"
                                                CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid"
                                                OnRowDataBound="gvEventList_RowDataBound" BorderWidth="1">
                                                <Columns>
                                                    <asp:BoundField DataField="CampaignName" HeaderText="Campaign" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="EventName" HeaderText="Event Name" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="StartDateTime" HeaderText="Event Date" DataFormatString="{0: MM/dd/yyyy}"
                                                        ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="EventStatus" HeaderText="Event Status" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEdit" Width="100px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='EDIT'
                                                                Text='Edit' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlEventsNoDonations" runat="server"></asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnClone" Width="250px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='CLONE'
                                                                Text='Clone Donation List To Selected Event:' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnCancel" Width="100px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='CANCELLED'
                                                                Text='Cancel' CssClass="btn btn-primary btn-sm option-button" OnClientClick="return confirm('Clicking OK will cancel all unclaimed donations for this event. They can not be recovered. Click OK to continue the cancel process.');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnComplete" Width="100px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='COMPLETED'
                                                                Text='Complete' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnDelete" Width="100px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='DELETEEVENT'
                                                                Text='Delete' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                </div>
                            </div>
                        </div>
            </div>
            </asp:View>
                    <asp:View ID="vwNoEvents" runat="server">
                        <p>
                            <strong>There are no donation event lists for the campaign 
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
                    <h3 class="modal-title text-center">Setup Events</h3>
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
