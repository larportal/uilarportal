<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="EventEdit.aspx.cs" Inherits="LarpPortal.Events.EventEdit" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="EventEditStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .ErrorDisplay {
            font-weight: bold;
            font-style: italic;
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="EventEditScripts" ContentPlaceHolderID="MainScripts" runat="server">

    <script type="text/javascript">
        function CalcDates() {
            var tbStartDateTime = document.getElementById("<%= tbStartDateTime.ClientID %>");
            var StartDate = new Date(tbStartDateTime.value);

            var tbCloseRegDateTime = document.getElementById("<%= tbCloseRegDateTime.ClientID %>");
            if (tbCloseRegDateTime) {
                var month = StartDate.getMonth() + 1; // Since getMonth() returns month from 0-11 not 1-12
                var year = StartDate.getFullYear();
                var day = StartDate.getDate();
                var dateStr = year + '-' + ('00' + month).slice(-2) + '-' + ('00' + day).slice(-2) + "T10:00";
                tbCloseRegDateTime.value = dateStr;
            }

            var InfoDueDate = parseInt(document.getElementById("<%= hidDaysToInfoSkillDeadlineDate.ClientID %>").value);
            if (!isNaN(InfoDueDate)) {
                var InfoSkillDate = new Date(StartDate);
                InfoSkillDate.setDate(InfoSkillDate.getDate() - InfoDueDate);
                var tbInfoDue = document.getElementById("<%= tbInfoSkillDue.ClientID %>");
                tbInfoDue.value = InfoSkillDate.toISOString().substring(0, 10);
            }

            var PreregOpenDays = parseInt(document.getElementById("<%= hidDaysToRegistrationOpenDate.ClientID %>").value);
            if (!isNaN(PreregOpenDays)) {
                var PreregOpenDate = new Date(StartDate);
                PreregOpenDate.setDate(PreregOpenDate.getDate() - PreregOpenDays);
                var month = PreregOpenDate.getMonth() + 1; // Since getMonth() returns month from 0-11 not 1-12
                var year = PreregOpenDate.getFullYear();
                var day = PreregOpenDate.getDate();
                var dateStr = year + '-' + ('00' + month).slice(-2) + '-' + ('00' + day).slice(-2) + "T10:00";
                PreregOpenDays.value = dateStr;
            }



<%--
                var tbOpenRegDate = document.getElementById("<%= tbOpenRegDateTime.ClientID %>");
                var t = PreregOpenDate.toISOString().substring(0, 10);
                alert(t);
                tbOpenRegDate.value = PreregOpenDate.toISOString().substring(0, 10) + "T00:00";
                alert(tbOpenRegDate.value);
            }--%>
            else
                alert("Couldn't find PreregOpenDate");

            var DaysToPELDeadlineDate = parseInt(document.getElementById("<%= hidDaysToPELDeadlineDate.ClientID %>").value);
            if (!isNaN(DaysToPELDeadlineDate)) {
                var PELDueDate = new Date(StartDate);
                PELDueDate.setDate(PELDueDate.getDate() + DaysToPELDeadlineDate + 3);
                var tbPELDue = document.getElementById("<%= tbPELDue.ClientID %>");
                tbPELDue.value = PELDueDate.toISOString().substring(0, 10);
            }

            var DaysToPreregistrationDeadline = parseInt(document.getElementById("<%= hidDaysToPreregistrationDeadline.ClientID %>").value);
            if (!isNaN(DaysToPreregistrationDeadline)) {
                var PreRegDate = new Date(StartDate);
                PreRegDate.setDate(PreRegDate.getDate() - DaysToPreregistrationDeadline);
                var tbPreReg = document.getElementById("<%= tbPreRegDeadline.ClientID %>");
                tbPreReg.value = PreRegDate.toISOString().substring(0, 10);
            }

            var DaysToPaymentDue = parseInt(document.getElementById("<%= hidDaysToPaymentDue.ClientID %>").value);
            if (!isNaN(DaysToPaymentDue)) {
                var PaymentDueDate = new Date(StartDate);
                PaymentDueDate.setDate(PaymentDueDate.getDate() - DaysToPaymentDue);
                var tbPaymentDue = document.getElementById("<%= tbPaymentDate.ClientID %>");
                tbPaymentDue.value = PaymentDueDate.toISOString().substring(0, 10);
            }

            // Why running client validate? Don't care if things are right. Have everything fill in and do the check on the submit.
            //Page_ClientValidate();
        }
        </script>
</asp:Content>
<asp:Content ID="EventEditBody" ContentPlaceHolderID="MainBody" runat="server">


    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Schedule An Event
                    </h1>
                </div>
            </div>
        </div>

        <div class="divide10"></div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Schedule An Event</div>
                    <div class="panel-body">
                        <%--                                <div class="pre-scrollable">--%>
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="<%# tbEventName.ClientID %>">Event Name:</label><asp:RequiredFieldValidator ID="rfvEventName" runat="server" ControlToValidate="tbEventName"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbEventName" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbStartDateTime.ClientID %>">Start Date/Time</label>
                                    <asp:TextBox ID="tbStartDateTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbEndDateTime.ClientID %>">End Date/Time</label>
                                    <asp:TextBox ID="tbEndDateTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbEventDescription %>">Event Description: </label>
                                    <asp:RequiredFieldValidator ID="rfvEventDescription"
                                        runat="server" ControlToValidate="tbEventDescription" CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbEventDescription" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <%--                                    <div class="col-lg-6 col-md-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%# tbStartDate.ClientID %>">Start Date/Time</label>
                                            <asp:TextBox ID="tbStartDate" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                                            <asp:TextBox ID="tbStartTime" runat="server" CssClass="form-control" Style="display: inline; margin-left: 10px; width:35%" TextMode="Time" />
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%# tbEndDate.ClientID %>">End Date/Time</label>
                                            <asp:TextBox ID="tbEndDate" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                                            <asp:TextBox ID="tbEndDate" runat="server" CssClass="form-control" style="display: inline" Width="60%" TextMode="Date" />
                                            <asp:TextBox ID="tbEndTime" runat="server" CssClass="form-control" Style="display: inline; margin-left: 10px; width:35%" TextMode="Time" />
                                            </div>
                                    </div>--%>
                        <%--                                    <div class="row col-lg-12 NoPadding">
                                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                                CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                            <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate" Display="Dynamic"
                                                CssClass="ErrorDisplay" Text="* Enter A Valid Date" Type="Date" Operator="DataTypeCheck" />
                                        </div>
                                        <div class="TableLabel col-lg-3">End Date/Time</div>
                                        <div class="col-lg-3 NoPadding">
                                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                                                CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate" Display="Dynamic"
                                                CssClass="ErrorDisplay" Text="* Enter A Valid Date" Type="Date" Operator="DataTypeCheck" />
                                        </div>
                                    </div>--%>
                        <div class="row">
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# ddlSiteList.ClientID %>">Site</label>
                                    <asp:RequiredFieldValidator ID="rfvSiteList" runat="server" ControlToValidate="ddlSiteList" InitialValue=""
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlSiteList" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbGameLocation.ClientID %>">In Game Location</label>
                                    <asp:RequiredFieldValidator ID="rfvGameLocation" runat="server" ControlToValidate="tbGameLocation"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbGameLocation" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label for="<%# tbMaxPCCount.ClientID %>">Max PC Count</label>
                                    <asp:TextBox ID="tbMaxPCCount" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvMaxPCCount" runat="server" ControlToValidate="tbMaxPCCount" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Integer" Operator="DataTypeCheck" />
                                    <asp:RequiredFieldValidator ID="rfvMaxPCCount" runat="server" ControlToValidate="tbMaxPCCount"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label for="<%# tbBaseNPCCount.ClientID %>">BaseNPC Count</label>
                                    <asp:TextBox ID="tbBaseNPCCount" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvBaseNPCCount" runat="server" ControlToValidate="tbBaseNPCCount" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Integer" Operator="DataTypeCheck" />
                                    <asp:RequiredFieldValidator ID="rfvBaseNPCCount" runat="server" ControlToValidate="tbBaseNPCCount"
                                        CssClass="ErrorDisplay" Text="* Enter Date" Display="Dynamic" />
                                </div>
                            </div>

                            <div class="col-md-3 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# ddlDefaultRegStatus.ClientID %>">Default Reg Status</label>
                                    <asp:RequiredFieldValidator ID="rvDefaultRegStatus" runat="server" ControlToValidate="ddlDefaultRegStatus" InitialValue="" CssClass="ErrorDisplay"
                                        Text="* Choose Def Reg Status" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlDefaultRegStatus" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="col-md-3 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# ddlAutoApproveWaitlist.ClientID %>">Auto Approve Waitlist</label>
                                    <asp:RequiredFieldValidator ID="rfvAutoApproveWaitlist" runat="server" ControlToValidate="ddlAutoApproveWaitlist" InitialValue=""
                                        CssClass="ErrorDisplay" Text="* Enter Value" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlAutoApproveWaitlist" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Choose Value" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="true" />
                                        <asp:ListItem Text="No" Value="false" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-4 col-md-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbOpenRegDateTime.ClientID %>">Open Reg Date</label>
                                    <asp:RequiredFieldValidator ID="rvOpenRegDate" runat="server" ControlToValidate="tbOpenRegDateTime"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbOpenRegDateTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbCloseRegDateTime.ClientID %>">Close Reg Date</label>
                                    <asp:RequiredFieldValidator ID="rfvCloseRegDate" runat="server" ControlToValidate="tbCloseRegDateTime"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbCloseRegDateTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-md-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbPreRegDeadline.ClientID %>">Pre Reg Deadline</label>
                                    <asp:RequiredFieldValidator ID="rfvPreRegDeadline" runat="server" ControlToValidate="tbPreRegDeadline"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbPreRegDeadline" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>

                            <div class="col-lg-2 col-md-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbPaymentDate.ClientID %>">Payment Date</label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbPaymentDate"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbPaymentDate" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbInfoSkillDue.ClientID %>">Info Skill Due</label>
                                    <asp:RequiredFieldValidator ID="rfvInfoSkillDue" runat="server" ControlToValidate="tbInfoSkillDue"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbInfoSkillDue" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbPELDue.ClientID %>">PEL Due</label>
                                    <asp:RequiredFieldValidator ID="rfvPELDue" runat="server" ControlToValidate="tbPELDue"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbPELDue" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-md-3 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# tbPreRegistrationPrice.ClientID %>">Pre Reg Price</label>
                                    <asp:RequiredFieldValidator ID="rfvPreRegistrationPrice" runat="server" ControlToValidate="tbPreRegistrationPrice"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:CompareValidator ID="cvPreRegistrationPrice" runat="server" ControlToValidate="tbPreRegistrationPrice" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Double" Operator="DataTypeCheck" />
                                    <asp:TextBox ID="tbPreRegistrationPrice" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="col-lg-2 col-md-3 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# tbRegPrice.ClientID %>">Reg Price</label>
                                    <asp:RequiredFieldValidator ID="rfvRegPrice" runat="server" ControlToValidate="tbRegPrice"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbRegPrice" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="col-lg-2 col-md-3 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# tbAtDoorPrice.ClientID %>">At Door Price</label>
                                    <asp:RequiredFieldValidator ID="rfvAtDoorPrice" runat="server" ControlToValidate="tbAtDoorPrice"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:CompareValidator ID="cvAtDoorPrice" runat="server" ControlToValidate="tbAtDoorPrice" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Double" Operator="DataTypeCheck" />
                                    <asp:TextBox ID="tbAtDoorPrice" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-md-4 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# ddlCapNearNotification.ClientID %>">Cap New Notification</label>
                                    <asp:RequiredFieldValidator ID="rfvCapNearNotification" runat="server" ControlToValidate="ddlCapNearNotification" InitialValue=""
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlCapNearNotification" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Choose Value" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="true" />
                                        <asp:ListItem Text="No" Value="false" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# tbCapThresholdNotification.ClientID %>">Cap Near Notification</label>
                                    <asp:CompareValidator ID="cvCapThresholdNotification" runat="server" ControlToValidate="tbCapThresholdNotification" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Integer" Operator="DataTypeCheck" />
                                    <asp:RequiredFieldValidator ID="rfvCapThresholdNotification" runat="server" ControlToValidate="tbCapThresholdNotification"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbCapThresholdNotification" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# tbOverrideRatio.ClientID %>">NPC Override Ratio</label>
                                    <asp:CompareValidator ID="cvOverrideRatio" runat="server" ControlToValidate="tbOverrideRatio" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Integer" Operator="DataTypeCheck" />
                                    <asp:RequiredFieldValidator ID="rfvOverrideRation" runat="server" ControlToValidate="tbOverrideRatio"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbOverrideRatio" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# ddlPCFoodService.ClientID %>">PC Food Service</label>
                                    <asp:RequiredFieldValidator ID="rfvPCFoodService" runat="server" ControlToValidate="ddlPCFoodService" InitialValue=""
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlPCFoodService" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Choose Value" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="true" />
                                        <asp:ListItem Text="No" Value="false" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-xs-6">
                                <div class="form-group">
                                    <label for="<%# ddlNPCFoodService.ClientID %>">NPC Food Service</label>
                                    <asp:RequiredFieldValidator ID="rfvNPCFoodService" runat="server" ControlToValidate="ddlNPCFoodService" InitialValue=""
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlNPCFoodService" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Choose Value" Value="" Selected="true" />
                                        <asp:ListItem Text="Yes" Value="true" />
                                        <asp:ListItem Text="No" Value="false" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbPaymentInstructions.ClientID %>">Payment Instructions</label>
                                    <asp:TextBox ID="tbPaymentInstructions" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <%--                        <div class="row">
                            <table>
                                <tr>
                                    <td>PC Stuff</td>
                                    <td>NPC Stuff</td>
                                    <td>Staff Stuff</td>
                                </tr>
                            </table>
                        </div>--%>

                        <div class="row">
                            <div class="col-xs-12">
                                <asp:DataList ID="dlPELTypes" runat="server" RepeatLayout="table" RepeatColumns="10" OnItemDataBound="dlPELTypes_ItemDataBound">
                                    <ItemStyle CssClass="autoWidth" VerticalAlign="top" />
                                    <ItemTemplate>
                                        <div style="margin-right: 10px;">
                                            <div class="panel panel-default">
                                                <div class="panel-heading"><%#Eval("TemplateTypeDescription") %> PELs</div>
                                                <div class="panel-body">
                                                    <asp:RadioButtonList ID="rblPELs" runat="server" RepeatLayout="flow"  BorderColor="Black" BorderStyle="none" BorderWidth="0" style="text-wrap: none;" />
                                                    <asp:HiddenField ID="hidEventPELID" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="text-right">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="margin20"></div>
    </div>
    <asp:HiddenField ID="hidEventID" runat="server" />
    <asp:HiddenField ID="hidCampaignID" runat="server" />
    <asp:HiddenField ID="hidDaysToPaymentDue" runat="server" />
    <asp:HiddenField ID="hidDaysToInfoSkillDeadlineDate" runat="server" />
    <asp:HiddenField ID="hidDaysToRegistrationOpenDate" runat="server" />
    <asp:HiddenField ID="hidDaysToPreregistrationDeadline" runat="server" />
    <asp:HiddenField ID="hidDaysToPELDeadlineDate" runat="server" />
    
</asp:Content>
