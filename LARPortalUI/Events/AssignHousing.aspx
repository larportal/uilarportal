<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="AssignHousing.aspx.cs" Inherits="LarpPortal.Events.AssignHousing" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="EventsAssignHousingStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .Events td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>

<asp:Content ID="EventsAssignHousingScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="EventsAssignHousingBody" ContentPlaceHolderID="MainBody" runat="server">

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
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" AutoPostBack="true" />
                <div class="pull-right">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="margin10"></div>
                <asp:Label ID="lblEventInfo" runat="server" />
                <div class="margin10"></div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <asp:UpdatePanel ID="upHousing" runat="server">
                    <ContentTemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">Event Registrations</div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="pre-scrollable" id="divRegs">
                                        <asp:GridView ID="gvRegistrations" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed table-responsive Events" AllowSorting="true" OnSorting="gvRegistrations_Sorting">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidRegistrationID" runat="server" Value='<%# Eval("RegistrationID") %>' />
                                                        <asp:HiddenField ID="hidRegStatusID" runat="server" Value='<%# Eval("RegistrationStatusID") %>' />
                                                        <asp:HiddenField ID="hidOrigHousing" runat="server" Value='<%# Eval("OrigHousing") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PlayerName" HeaderText="Player Name" ItemStyle-Wrap="true" SortExpression="PlayerName" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="Blue" />
                                                <asp:TemplateField HeaderText="Role" ItemStyle-Wrap="false" SortExpression="RoleAlignmentDescription" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="blue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("RoleAlignmentDescription") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Character Name" ItemStyle-Wrap="true" SortExpression="CharacterName" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="blue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCharacterName" runat="server" Text='<%# Eval("CharacterName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Team Name" ItemStyle-Wrap="true" SortExpression="TeamName" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="blue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTeamName" runat="server" Text='<%# Eval("TeamName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reg Status" SortExpression="RegistrationStatus" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="blue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegStatus" runat="server" Text='<%# Eval("RegistrationStatus") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlRegStatus" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Req Housing" SortExpression="ReqstdHousing" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="blue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHousing" runat="server" Text='<%# Eval("ReqstdHousing") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comments" SortExpression="PlayerCommentsToStaff" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="Blue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComments" runat="server" Text='<%# Eval("PlayerCommentsToStaff") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assigned Housing" SortExpression="AssignHousing" HeaderStyle-Font-Underline="true" HeaderStyle-ForeColor="blue">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tbAssignHousing" runat="server" Text='<%# Eval("AssignHousing") %>' CssClass="form-control" Width="150px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <div class="pull-right">
                    <asp:Button ID="btnBottomSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidApprovedStatus" runat="server" />

        <div class="modal fade in" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Event Housing</h3>
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

    <asp:HiddenField ID="hidScollPosition" runat="server" Value="" />

    <script type="text/javascript">
        function setScrollValue() {
            var divObj = $get('divRegs');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (obj) obj.value = divObj.scrollTop;
        }

        function pageLoad() {
            var divObj = $get('divRegs');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (divObj) divObj.scrollTop = obj.value;
        }
</script>

</asp:Content>
