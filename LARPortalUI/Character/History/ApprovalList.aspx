<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="ApprovalList.aspx.cs" Inherits="LarpPortal.Character.History.ApprovalList" %>

<asp:Content ID="ApprovalListStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="ApprovalListScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="ApprovalListBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character Histories</h1>
                </div>
            </div>
        </div>

        <asp:MultiView ID="mvPELs" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwPELs" runat="server">
                <div class="row" style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">
                    <%--                    <asp:Image ID="imgBlank1" runat="server" ImageUrl="~/img/blank.gif" Height="0" Width="25" />--%>
                    Character Name:
                    <asp:DropDownList ID="ddlCharacterName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCharacterName_SelectedIndexChanged" />
                    <asp:Image ID="imgBlank2" runat="server" ImageUrl="~/img/blank.gif" Height="0" Width="25" />
                    Status:
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        <asp:ListItem Text="No Filter" Value="" />
                        <asp:ListItem Text="Approved Only" Value="A" />
                        <asp:ListItem Text="Submitted Only" Value="S" />
                    </asp:DropDownList>
                </div>

                <div class="row">
                    <div class="col-sm-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">Character Histories</div>
                            <div class="panel-body">
                                <div style="max-height: 500px; overflow-y: auto;">
                                    <asp:GridView ID="gvHistoryList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvHistoryList_RowCommand" GridLines="None"
                                        CssClass="table table-striped table-hover table-condensed table-responsive" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="99%">
                                        <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="24pt" />
                                        <EmptyDataTemplate>
                                            There are no histories that meet your criteria.
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidCharacterID" runat="server" Value='<%# Eval("CharacterID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CampaignName" HeaderText="Campaign" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="PlayerName" HeaderText="Player Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="CharacterAKA" HeaderText="Character" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="ShortHistory" HeaderText="History" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="HistoryStatus" HeaderText="Status" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("CharacterID") %>' CommandName='View'
                                                        Text='View' CssClass="btn btn-primary btn-sm" Width="100px" />
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
            <asp:View ID="vwNoPELs" runat="server">
                <div class="row" style="padding-top: 30px; padding-left: 30px; color: red; font-weight: bold; font-size: 24pt">
                    There are no histories waiting to be processed.
                </div>
            </asp:View>
        </asp:MultiView>
    </div>

</asp:Content>
