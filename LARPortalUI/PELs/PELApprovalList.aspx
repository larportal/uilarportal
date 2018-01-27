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
                    <h1>PEL Approval List</h1>
                </div>
            </div>
        </div>

        <asp:MultiView ID="mvPELs" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwPELs" runat="server">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-inline">
                            <%--                            <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                            <div class="form-group">
                                <label for="<%= ddlEventDate.ClientID %>" style="padding-left: 10px;">Event Date: </label>
                                <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" />
                            </div>
                            <div class="form-group">
                                <label for="<%= ddlCharacterName.ClientID %>" style="padding-left: 10px;">Character Name: </label>
                                <asp:DropDownList ID="ddlCharacterName" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" />
                            </div>
                            <div class="form-group">
                                <label for="<%= ddlEventName.ClientID %>" style="padding-left: 10px;">Event Name: </label>
                                <asp:DropDownList ID="ddlEventName" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" />
                            </div>
                            <div class="form-group" style="padding-right: 10px;">
                                <label for="<%= ddlStatus.ClientID %>" style="padding-left: 10px;">PEL Status: </label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" />
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
                                                    CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="99%">
                                                    <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="24pt" />
                                                    <EmptyDataTemplate>
                                                        There are no PELs that meet your criteria.
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField DataField="CampaignName" HeaderText="Campaign" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="EventStartDate" HeaderText="Event Date" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="false"
                                                            HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="PlayerName" HeaderText="Player Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="RoleAlignment" HeaderText="Role" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="CharacterAKA" HeaderText="Character" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="EventName" HeaderText="Event" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="EventDescription" HeaderText="Event Description" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="PELStatus" HeaderText="Status" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnCommand" runat="server" CommandArgument='<%# Eval("RegistrationID") %>' CommandName='<%# Eval("ButtonText") %>Item'
                                                                    Text='View' CssClass="btn btn-primary" />
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

