<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="EventDefaults.aspx.cs" Inherits="LarpPortal.Events.EventDefaults" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="EventDefaultsStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="EventDefaultsScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="EventDefaultsBody" ContentPlaceHolderID="MainBody" runat="server">



    <style>
        select {
            font-size: 12px;
        }

        .NoMargins {
            margin: 0px 0px 0px 0px;
        }

        .row {
            vertical-align: middle;
            padding-bottom: 2px;
        }
    </style>



    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Event Setup Campaign Defaults -
                        <asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>

        <div class="divide10"></div>

        <div class="row">
            <div class="col-lg-8 col-xs-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Event Defaults</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="form-group">
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbStartTime.ClientID %>">Start Time</label>
                                    <asp:TextBox ID="tbStartTime" runat="server" CssClass="form-control" TextMode="Time" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbEndTime.ClientID %>">End Time</label>
                                    <asp:TextBox ID="tbEndTime" runat="server" CssClass="form-control" TextMode="Time" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbOpenRegDate %>">Open Reg Date</label>
                                    <asp:TextBox ID="tbOpenRegDate" runat="server" Columns="16" MaxLength="16" CssClass="form-control" />
                                    <asp:RangeValidator ID="rvOpenRegDate" runat="server" ForeColor="Red" MaximumValue="999" MinimumValue="-999" Font-Bold="true"
                                        Font-Italic="true" Text="* Numbers Only" Type="Integer" ControlToValidate="tbOpenRegDate" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbOpenRegTime.ClientID %>">Open Reg Time</label>
                                    <asp:TextBox ID="tbOpenRegTime" runat="server" TextMode="Time" CssClass="form-control" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbMaxPCCount.ClientID %>">Max PC Count</label>
                                    <asp:TextBox ID="tbMaxPCCount" runat="server" Columns="4" MaxLength="4" CssClass="form-control" /><asp:RangeValidator ID="rvMaxPCCount" runat="server"
                                        ForeColor="Red" MaximumValue="999" MinimumValue="0" Font-Bold="true" Font-Italic="true" Text="* Numbers Only" Type="Integer"
                                        ControlToValidate="tbMaxPCCount" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbBaseNPCCount.ClientID %>">BaseNPC Count</label>
                                    <asp:TextBox ID="tbBaseNPCCount" runat="server" Columns="4" MaxLength="4" CssClass="form-control" /><asp:RangeValidator ID="rvBaseNPCCount" runat="server"
                                        ForeColor="Red" MaximumValue="999" MinimumValue="0" Font-Bold="true" Font-Italic="true" Text="* Numbers Only" Type="Integer"
                                        ControlToValidate="tbBaseNPCCount" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbOverrideRatio.ClientID %>">NPC Override Ratio</label>
                                    <asp:TextBox ID="tbOverrideRatio" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                    <asp:RangeValidator ID="rvOverrideRatio" runat="server" ForeColor="Red" MaximumValue="999" MinimumValue="-999" Font-Bold="true"
                                        Font-Italic="true" Text="* Numbers Only" Type="Integer" ControlToValidate="tbOverrideRatio" Display="Dynamic" />
                                </div>
                                <div class="col-xs-12">
                                    <label for="<%= ddlSiteList.ClientID %>">Primary Site</label>
                                    <asp:DropDownList ID="ddlSiteList" runat="server" CssClass="form-control" ToolTip="Select the default site location." />
                                </div>
                                <div class="col-lg-6 col-md-12 col-xs-12">
                                    <label for="<%= ddlDefaultRegStatus.ClientID %>">Default Reg Status</label>
                                    <asp:DropDownList ID="ddlDefaultRegStatus" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= ddlCapNearNotification.ClientID %>">Cap New Notification</label>
                                    <asp:DropDownList ID="ddlCapNearNotification" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="No default" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="Yes" />
                                        <asp:ListItem Text="No" Value="No" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbCapThresholdNotification.ClientID %>">Cap Near Notification</label>
                                    <asp:TextBox ID="tbCapThresholdNotification" runat="server" MaxLength="4" CssClass="form-control" />
                                    <asp:RangeValidator ID="rvCapThresholdNotification" runat="server" ForeColor="Red" MaximumValue="999" MinimumValue="-999"
                                        Font-Bold="true" Font-Italic="true" Text="* Numbers Only" Type="Integer" ControlToValidate="tbCapThresholdNotification" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbPaymentDate.ClientID %>">Payment Date</label>
                                    <asp:TextBox ID="tbPaymentDate" runat="server" MaxLength="16" CssClass="form-control" />
                                    <asp:RangeValidator ID="rvPaymentDate" runat="server" ForeColor="Red" MaximumValue="999" MinimumValue="-999"
                                        Font-Bold="true" Font-Italic="true" Text="* Numbers Only" Type="Integer" ControlToValidate="tbPaymentDate" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbPreRegDeadline.ClientID %>">Pre Reg Deadline</label>
                                    <asp:TextBox ID="tbPreRegDeadline" runat="server" MaxLength="16" CssClass="form-control" />
                                    <asp:RangeValidator ID="rvPreRegDeadline" runat="server" ForeColor="Red" MaximumValue="999" MinimumValue="-999" Font-Bold="true"
                                        Font-Italic="true" Text="* Numbers Only" Type="Integer" ControlToValidate="tbPreRegDeadline" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbInfoSkillDue.ClientID %>">Info Skill Due</label>
                                    <asp:TextBox ID="tbInfoSkillDue" runat="server" MaxLength="16" CssClass="form-control" /><asp:RangeValidator ID="rvInfoSkillDue" runat="server"
                                        ForeColor="Red" MaximumValue="999" MinimumValue="-999" Font-Bold="true" Font-Italic="true" Text="* Numbers Only" Type="Integer"
                                        ControlToValidate="tbInfoSkillDue" Style="margin-left: 10px;" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbPELDue.ClientID %>">PEL Due</label>
                                    <asp:TextBox ID="tbPELDue" runat="server" MaxLength="16" CssClass="form-control" /><asp:RangeValidator ID="rvPELDue" runat="server"
                                        ForeColor="Red" MaximumValue="999" MinimumValue="-999" Font-Bold="true" Font-Italic="true" Text="* Numbers Only" Type="Integer"
                                        ControlToValidate="tbPELDue" Style="margin-left: 10px;" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= ddlAutoApproveWaitlist.ClientID %>">Auto Approve Waitlist</label>
                                    <asp:DropDownList ID="ddlAutoApproveWaitlist" runat="server" Style="vertical-align: middle;" CssClass="form-control">
                                        <asp:ListItem Text="No default" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="Yes" />
                                        <asp:ListItem Text="No" Value="No" />
                                    </asp:DropDownList>
                                </div>

                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbRegPrice.ClientID %>">Reg Price</label>
                                    <asp:TextBox ID="tbRegPrice" runat="server" MaxLength="8" CssClass="form-control" />
                                    <asp:RegularExpressionValidator ID="reRegPrice" runat="server" ControlToValidate="tbRegPrice"
                                        ErrorMessage="* Enter Currency" ValidationExpression="^\d+(\.\d\d)?$" Font-Bold="true" Font-Italic="true" ForeColor="red" Display="Dynamic" />
                                </div>

                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbAtDoorPrice %>">At Door Price</label>
                                    <asp:TextBox ID="tbAtDoorPrice" runat="server" MaxLength="8" CssClass="form-control" />
                                    <asp:RegularExpressionValidator ID="reAtDoorPrice" runat="server" ControlToValidate="tbAtDoorPrice"
                                        ErrorMessage="* Enter Currency" ValidationExpression="^\d+(\.\d\d)?$" Font-Bold="true" Font-Italic="true" ForeColor="red" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= tbPreRegistrationPrice.ClientID %>">Pre Registration Price</label>
                                    <asp:TextBox ID="tbPreRegistrationPrice" runat="server" MaxLength="8" CssClass="form-control" />
                                    <asp:RegularExpressionValidator ID="rePreRegistrationPrice" runat="server" ControlToValidate="tbPreRegistrationPrice"
                                        ErrorMessage="* Enter Currency" ValidationExpression="^\d+(\.\d\d)?$" Font-Bold="true" Font-Italic="true" ForeColor="red" Display="Dynamic" />
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= ddlPCFoodService.ClientID %>">PC Food Service</label>
                                    <asp:DropDownList ID="ddlPCFoodService" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="No default" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="Yes" />
                                        <asp:ListItem Text="No" Value="No" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-3 col-md-6 col-xs-12">
                                    <label for="<%= ddlNPCFoodService.ClientID %>">NPC Food Service</label>
                                    <asp:DropDownList ID="ddlNPCFoodService" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="No default" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="Yes" />
                                        <asp:ListItem Text="No" Value="No" />
                                    </asp:DropDownList>
                                </div>

                                <div class="col-xs-12">
                                    <label for="<%= tbPaymentInstructions.ClientID %>">Payment Instructions</label>
                                    <asp:TextBox ID="tbPaymentInstructions" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="col-lg-4 col-xs-12">
                <%--                <div class="row">
                    <div class="col-lg-12 NoPadding text-left" style="font-size: 16px; padding-bottom: 20px;">
                        <b>Default PEL Selection</b>
                    </div>
                </div>--%>
                <asp:Repeater ID="rptPELTypes" runat="server" OnItemDataBound="rptPELTypes_ItemDataBound">
                    <ItemTemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">Default PEL Selection for <%#Eval("TemplateTypeDescription") %></div>
                            <div class="panel-body">
                                <asp:RadioButtonList ID="rblPELs" runat="server" RepeatLayout="flow" BorderStyle="None" BorderColor="Transparent" BorderWidth="0" />
                            </div>
                            <asp:HiddenField ID="hidTemplateTypeID" runat="server" Value='<%#Eval("PELTemplateTypeID") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 text-right">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a class="close" data-dismiss="modal">&times;</a>
                        <div class="modal-title text-center">LARPortal Registration</div>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblRegistrationMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidDefaultID" runat="server" />
    </div>

</asp:Content>
