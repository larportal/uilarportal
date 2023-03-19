<%@ Page Title="In-between Skills Requests" Language="C#" MasterPageFile="~/LARPortal.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="LarpPortal.Character.ISkills.Requests" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style type="text/css">
        .nopadding {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="CharSkillsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
</script>
</asp:Content>

<asp:Content ID="CharSkillsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
                <div class="">
                    <h1>In-between Skill Requests</h1>
            </div>
        </div>

        <div class="row">
            <CharSelector:Select ID="oCharSelect" runat="server" />
        </div>
        <div class="row" style="margin-top: 20px;">
            <asp:Panel ID="pnlRequestInfo" runat="server" CssClass="col-lg-12">
                <div class="row col-12">
                    <asp:GridView ID="gvRegisteredEvents" runat="server" OnRowCommand="gvRegisteredEvents_RowCommand" AutoGenerateColumns="false" DataKeyNames="RegistrationID"
                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                            <asp:BoundField DataField="StartDateTime" HeaderText="Event Start Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Breadcrumbs" HeaderText="Skill" />
                            <asp:BoundField DataField="CharacterStatus" HeaderText="Request Status" />
                            <asp:BoundField DataField="StaffStatus" HeaderText="Staff Status" />
                            <asp:TemplateField ItemStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Button Text='<%#Eval("ButtonText") %>' runat="server" CommandName='<%#Eval("ButtonText") %>'
                                        CssClass="btn btn-success btn-sm btn-" CommandArgument='<%#Eval("KeyValue") %>' Width="100" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
</script>
</asp:Content>
