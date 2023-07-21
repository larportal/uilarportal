<%@ Page Title="In-between Skills Staff List" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="StaffList.aspx.cs" Inherits="LarpPortal.Character.ISkills.StaffList" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELListStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PELListScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="PELListBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Information Skills</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <asp:MultiView ID="mvISkillList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwISKillList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">In-Between Skills</div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-10">
                                        <div class="form-inline">
                                            <div class="form-group">
                                                <label for="<%= ddlEventDate.ClientID %>" style="padding-left: 10px;">Event Date: </label>
                                                <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlFilterChanged_SelectedIndexChanged" />
                                                <asp:Label ID="lblEventDate" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="<%= ddlSkillName.ClientID %>" style="padding-left: 10px;">Skill Name: </label>
                                                <asp:DropDownList ID="ddlSkillName" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlFilterChanged_SelectedIndexChanged" />
                                                <asp:Label ID="lblSkillName" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="<%= ddlStaffStatus.ClientID %>" style="padding-left: 10px;">Staff Status: </label>
                                                <asp:DropDownList ID="ddlStaffStatus" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlFilterChanged_SelectedIndexChanged" />
                                                <asp:Label ID="lblStaffStatus" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="<%= ddlAssignedTo.ClientID %>" style="padding-left: 10px;">Assigned To: </label>
                                                <asp:DropDownList ID="ddlAssignedTo" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlFilterChanged_SelectedIndexChanged" />
                                                <asp:Label ID="lblAssignedTo" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="<%= ddlCharacterList.ClientID %>" style="padding-left: 10px;">Character: </label>
                                                <asp:DropDownList ID="ddlCharacterList" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlFilterChanged_SelectedIndexChanged" />
                                                <asp:Label ID="lblCharacter" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="<%= ddlSkillType.ClientID %>" style="padding-left: 10px;">Skill Type: </label>
                                                <asp:DropDownList ID="ddlSkillType" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlFilterChanged_SelectedIndexChanged" />
                                                <asp:Label ID="lblSkillType" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-2 text-right">
                                        <asp:Button ID="btnPassiveSkills" runat="server" Text="Add Passive Skills" CssClass="btn btn-primary btn-xs" OnClick="btnPassiveSkills_Click" />
                                    </div>
                                </div>
                                <div style="max-height: 500px; overflow-y: auto; padding-top: 10px;">
                                    <asp:GridView ID="gvIBSkillList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvIBSkillList_RowCommand" GridLines="None"
                                        OnSorting="gvIBSkillList_Sorting" AllowSorting="true"
                                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Outset" BorderWidth="1">
                                        <Columns>
                                            <asp:BoundField DataField="CharName" HeaderText="Character Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle"
                                                SortExpression="CharName" />
                                            <asp:BoundField DataField="EventDate" HeaderText="Event Date" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle"
                                                SortExpression="EventDate" DataFormatString="{0: MM/dd/yyyy}" />
                                            <asp:BoundField DataField="BreadcrumbsText" HeaderText="Skill" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle"
                                                SortExpression="BreadcrumbsText" />
                                            <asp:BoundField DataField="PlayerName" HeaderText="Player Name" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle"
                                                SortExpression="PlayerName" />
                                            <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ItemStyle-VerticalAlign="Middle"
                                                SortExpression="DateSubmitted" DataFormatString="{0: MM/dd/yyyy}" />
                                            <asp:BoundField DataField="StatusName" HeaderText="User Status" HeaderStyle-Wrap="false" SortExpression="StatusName" />
                                            <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" HeaderStyle-Wrap="false" SortExpression="AssignedToLastFirst" />
                                            <asp:BoundField DataField="StaffStatus" HeaderText="Staff Status" SortExpression="StaffStatus" />
                                            <asp:TemplateField ItemStyle-CssClass="text-right" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnCommand" runat="server" CommandArgument='<%# Eval("IBSkillRequestID") %>' CommandName='<%# Eval("ButtonText") %>Item'
                                                        Text='<%# Eval("ButtonText") %>' CssClass="btn btn-primary btn-xs LeftRightPadding10" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwNoISkills" runat="server">
                        <p>
                            <strong>You do not have any open requests for the campaign
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
                        <h3 class="modal-title text-center">PELs</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblmodalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>

        <div id="push"></div>
        <asp:HiddenField ID="hidEventID" runat="server" />
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

