<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="MasterSkills.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Skills.MasterSkills" %>
<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="MasterSkillsStyles" ContentPlaceHolderID="MainStyles" runat="server">
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

<asp:Content ID="MasterSkillsScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="MasterSkillsBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Setup Master Skills</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="form-inline col-xs-12">
                <div class="pull-right">
                    <%--<asp:CheckBox ID="cbDisplayOnlyOpenEvents" Text="Display Only Open Events" AutoPostBack="true" runat="server" />--%>
                    <asp:Button ID="btnCreate" runat="server" Style="" Text='Create New Skill' CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                </div>
            </div>
        </div>
        <div class="divide10"></div>

        <div class="row">
            <div class="col-md-12">
                <asp:MultiView ID="mvSkillList" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwSkillList" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Master Skill List</div>
                            <div class="panel-body">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="pre-scrollable">
                                            <asp:GridView ID="gvSkillList" runat="server" AutoGenerateColumns="false" OnRowCommand="gvSkillList_RowCommand" GridLines="None"
                                                CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid"
                                                OnRowDataBound="gvSkillList_RowDataBound" BorderWidth="1">
                                                <Columns>
                                                    <%--<asp:BoundField DataField="CampaignName" HeaderText="Campaign" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />--%>
                                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Name" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="SkillShortDescription" HeaderText="Short Description" HeaderStyle-Wrap="false" ItemStyle-Wrap="true" />
                                                    <%--<asp:BoundField DataField="StartDateTime" HeaderText="Event Start Date" DataFormatString="{0: MM/dd/yyyy hh:mm tt}"
                                                        ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="EndDateTime" HeaderText="Event End Date" DataFormatString="{0: MM/dd/yyyy hh:mm tt}"
                                                        ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="EventStatus" HeaderText="Event Status" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />--%>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEdit" Width="100px" runat="server" CommandArgument='<%# Eval("CampaignSkillsID") %>' CommandName='EDIT'
                                                                Text='Edit' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
<%--                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnCancel" Width="100px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='CANCELLED'
                                                                Text='Cancel' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnComplete" Width="100px" runat="server" CommandArgument='<%# Eval("EventID") %>' CommandName='COMPLETED'
                                                                Text='Complete' CssClass="btn btn-primary btn-sm option-button" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnDelete" Width="100px" runat="server" CommandArgument='<%# Eval("CampaignSkillsID") %>' CommandName='DELETESKILL'
                                                                Text='Delete' CssClass="btn btn-primary btn-sm option-button" />
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
                    <asp:View ID="vwNoSkills" runat="server">
                        <p>
                            <strong>There are no skills for the campaign 
                            <asp:Label ID="lblCampaignName" runat="server" />.
                            </strong>
                        </p>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>

        <div class="modal fade" id="modalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Setup Master Skills</h3>
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
