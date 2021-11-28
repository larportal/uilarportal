<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="PELApprovalList.aspx.cs" Inherits="LarpPortal.PELs.PELApprovalList" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELApprovalListStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELApprovalListScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="PELApprovalListBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>PEL Approval List for <asp:Label ID="lblCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>

        <asp:MultiView ID="mvPELs" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwPELs" runat="server">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-inline">
                            <div class="form-group">
                                <label for="<%= ddlEventDate.ClientID %>" style="padding-left: 10px;">Event Date: </label>
                                <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEventDate_SelectedIndexChanged" />
                            </div>
                            <div class="form-group">
                                <label for="<%= ddlCharacterName.ClientID %>" style="padding-left: 10px;">Character Name: </label>
                                <asp:DropDownList ID="ddlCharacterName" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCharacterName_SelectedIndexChanged" />
                            </div>
                            <div class="form-group">
                                <label for="<%= ddlEventName.ClientID %>" style="padding-left: 10px;">Event Name: </label>
                                <asp:DropDownList ID="ddlEventName" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEventName_SelectedIndexChanged" />
                            </div>
                            <div class="form-group" style="padding-right: 10px;">
                                <label for="<%= ddlStatus.ClientID %>" style="padding-left: 10px;">PEL Status: </label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" />
                            </div>
                            <asp:Button ID="btnApproveAll" runat="server" Text="Approve All" CssClass="btn btn-primary" OnClick="btnApproveAll_Click" />
                        </div>
                    </div>
                </div>
                <div class="margin10"></div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Event List</div>
                            <div class="panel-body">
                                <div class="panel-container">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div class="pre-scrollable">
                                                <asp:GridView ID="gvPELList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvPELList_RowCommand" GridLines="None"
                                                    CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                                    Width="99%" AllowSorting="true" OnSorting="gvPELList_Sorting" OnRowDataBound="gvPELList_RowDataBound">
                                                    <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="24pt" />
                                                    <EmptyDataTemplate>
                                                        There are no PELs that meet your criteria.
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField DataField="EventStartDate" HeaderText="Event Date" ItemStyle-Wrap="false" DataFormatString="{0:MM/dd/yyyy}"
                                                            HeaderStyle-Wrap="false" SortExpression="EventStartDate" />
                                                        <asp:BoundField DataField="EventName" HeaderText="Event Name" ItemStyle-Wrap="false"
                                                            HeaderStyle-Wrap="false" SortExpression="EventName" />
                                                        <asp:BoundField DataField="PlayerFirstName" HeaderText="Player First Name" ItemStyle-Wrap="false"
                                                            HeaderStyle-Wrap="false" SortExpression="PlayerFirstName" />
                                                        <asp:BoundField DataField="PLayerLastName" HeaderText="Player Last Name" ItemStyle-Wrap="false"
                                                            HeaderStyle-Wrap="false" SortExpression="PlayerLastName" />
                                                        <asp:BoundField DataField="RoleAlignment" HeaderText="Role" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" 
                                                            SortExpression="RoleAlignment" />
                                                        <asp:BoundField DataField="CharacterAKA" HeaderText="Character" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" 
                                                            SortExpression="CharacterAKA" />
                                                        <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" HeaderStyle-Wrap="false" 
                                                            SortExpression="DateSubmitted" />
                                                        <asp:BoundField DataField="PELStatus" HeaderText="Status" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" 
                                                            SortExpression="PELStatus" />
                                                        <asp:TemplateField HeaderText="Addendum" SortExpression="Addendum" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgAddendum" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"AddendumImage") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Staff<br>Comments" SortExpression="StaffComments" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100"
                                                                HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgStaffComment" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"StaffCommentsImage") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnCommand" runat="server" CommandArgument='<%# Eval("RegistrationID") %>'
                                                                    CommandName='<%# Eval("ButtonText") %>Item' Text='View' CssClass="btn btn-primary ShortButton" />
                                                                <asp:HiddenField ID="hidPELId" runat="server" Value='<%# Eval("PELID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwNoPELs" runat="server">
                <div class="row">
                    <div class="col-xs-12 text-center">
                        <span style="color: red; font-weight: bold; font-size: 24pt">There are no PELs waiting to be processed.</span>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

