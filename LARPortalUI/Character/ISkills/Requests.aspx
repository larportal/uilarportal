<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="LarpPortal.Character.ISkills.Requests" %>

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
            <CharSelector:Select ID="oCharSelect" runat="server" />
        </div>
        <div class="row">
            <asp:Panel ID="pnlRequestInfo" runat="server" CssClass="col-lg-12">
                Request before next event.
                <div class="row col-12">
                    <asp:GridView ID="gvRegisteredEvents" runat="server" OnRowCommand="gvRegisteredEvents_RowCommand" AutoGenerateColumns="false" DataKeyNames="RegistrationID"
                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                            <asp:BoundField DataField="StartDateTime" HeaderText="Event Start Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Breadcrumbs" HeaderText="Skill" />
                            <asp:BoundField DataField="StatusName" HeaderText="Request Status" />
                            <asp:TemplateField ItemStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Button Text="View Status"  Visible='<%# Eval("StatusVisible").ToString() == "1" %>' runat="server"
                                        CommandName="View" CssClass="btn btn-success btn-sm" CommandArgument='<%#Eval("ISkillRequestID") %>' Width="100" />
                                </ItemTemplate>
                            </asp:TemplateField>
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
