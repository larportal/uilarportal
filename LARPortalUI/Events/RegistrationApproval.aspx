<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="RegistrationApproval.aspx.cs" Inherits="LarpPortal.Events.RegistrationApproval" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="RegistrationApprovalStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .max-width-300 {
            max-width: 300px;
        }
    </style>
</asp:Content>

<asp:Content ID="RegistrationApprovalScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>

<asp:Content ID="RegistrationApprovalBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Event Registration</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="form-inline col-xs-12">
                <label for="<%= ddlEvent.ClientID %>">Events:</label>
                <%--style="padding-left: 25px;"--%>
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" AutoPostBack="true" />
                <div class="pull-right">
                    <asp:Button ID="btnApproveAll" runat="server" CssClass="btn btn-primary" Text="Approve All" OnClick="btnApproveAll_Click" />
                </div>
            </div>
        </div>
        <div class="divide10"></div>
        <div class="row">
            <div class="form-inline">
                <div class="col-xs-12">
                    <asp:Label ID="lblEventDates" runat="server" Text="Event Info" CssClass="input-control NoShadow autoWidth" />
                    <asp:Label ID="lblEventSite" runat="server" CssClass="input-control NoShadow LeftRightPadding10 autoWidth" />
                </div>
            </div>
        </div>
        <div class="divide10"></div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Registration Approval</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblRegistrationCounts" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 pre-scrollable">
                                <asp:GridView ID="gvRegistrations" runat="server" OnRowDataBound="gvRegistrations_RowDataBound" OnRowCommand="gvRegistrations_RowCommand"
                                    OnRowEditing="gvRegistrations_RowEditing" OnRowUpdating="gvRegistrations_RowUpdating" OnRowCancelingEdit="gvRegistrations_RowCancelingEdit"
                                    AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false" CssClass="table table-striped table-hover table-condensed" 
                                    AllowSorting="true" OnSorting="gvRegistrations_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidRegistrationID" runat="server" Value='<%# Eval("RegistrationID") %>' />
                                                <asp:HiddenField ID="hidRegStatusID" runat="server" Value='<%# Eval("RegistrationStatusID") %>' />
                                                <asp:HiddenField ID="hidCampaignHousingID" runat="server" Value='<%# Eval("CampaignHousingTypeID") %>' />
                                                <asp:HiddenField ID="hidPaymentTypeID" runat="server" Value='<%# Eval("EventPaymentTypeID") %>' />
                                                <asp:HiddenField ID="hidPaymentDate" runat="server" Value='<%# Eval("EventPaymentDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Player Name" ItemStyle-Wrap="true" SortExpression="PlayerName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlayerName" runat="server" Text='<%# Eval("PlayerName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Role" ItemStyle-Wrap="false" SortExpression="Role">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRole" runat="server" Text='<%# Eval("RoleAlignmentDescription") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Character Name" ItemStyle-Wrap="true" SortExpression="CharacterName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCharacterName" runat="server" Text='<%# Eval("CharacterName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Team Name" ItemStyle-Wrap="true" SortExpression="TeamName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTeamName" runat="server" Text='<%# Eval("TeamName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Housing" SortExpression="CampaignHousing">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHousing" runat="server" Text='<%# Eval("CampaignHousing") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Type" SortExpression="DisplayPayment">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("DisplayPayment") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPaymentType" runat="server" /><br />
                                                <asp:TextBox ID="tbPayment" runat="server" Text='<%# Eval("EventPaymentAmount", "{0:0.00}") %>' CssClass="form-control" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Date" SortExpression="EventPaymentDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEventPaymentDate" runat="server" Text='<%# Eval("EventPaymentDate", "{0:d}") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Calendar ID="calPaymentDate" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comments" ItemStyle-CssClass="max-width-300">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComments" runat="server" Text='<%# Eval("PlayerCommentsToStaff") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reg Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegStatus" runat="server" Text='<%# Eval("RegistrationStatus") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlRegStatus" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Button ID="btnApprove" runat="server" CommandName="Approve" Text="Approve" Width="100px" CssClass="btn btn-primary btn-sm"
                                                    CommandArgument='<%# Eval("RegistrationID") %>' Visible='<%# DataBinder.Eval(Container.DataItem,"DisplayEditButtons") %>' />
                                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" Width="100px" CssClass="btn btn-primary btn-sm"
                                                    Visible='<%# DataBinder.Eval(Container.DataItem,"DisplayEditButtons") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" Width="100px" CssClass="btn btn-primary btn-sm" />
                                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" Width="100px" CssClass="btn btn-primary btn-sm" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidApprovedStatus" runat="server" />
    </div>
</asp:Content>
