<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="DonationAdd.aspx.cs" Inherits="LarpPortal.Donations.DonationAdd" %>

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
        function openMessage() {
            $('#modalMessage').modal('show');
            return false;
        }

        function SetDefaults() {

            var ReqByDate = parseInt(document.getElementById("<%= hidRequiredByDate.ClientID %>").value);
            if (!isNaN(InfoDueDate)) {
                var InfoSkillDate = new Date(StartDate);
                InfoSkillDate.setDate(InfoSkillDate.getDate() - InfoDueDate);
                var tbInfoDue = document.getElementById("<%= tbInfoSkillDue.ClientID %>");
                tbInfoDue.value = InfoSkillDate.toISOString().substring(0, 10);
            }

            tbReqByDate
            tbShipTo1
            tbShipTo2
            tbShipToCity
            tbShipToState
            tbShipToZip
            tbShipToPhone
            var InfoDueDate = parseInt(document.getElementById("<%= hidDaysToInfoSkillDeadlineDate.ClientID %>").value);
            if (!isNaN(InfoDueDate)) {
                var InfoSkillDate = new Date(StartDate);
                InfoSkillDate.setDate(InfoSkillDate.getDate() - InfoDueDate);
                var tbInfoDue = document.getElementById("<%= tbInfoSkillDue.ClientID %>");
                tbInfoDue.value = InfoSkillDate.toISOString().substring(0, 10);
            }
        }
    </script>

</asp:Content>

<asp:Content ID="EventEditBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Add Donations</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-8">
                <label for="<%= ddlEventDate.ClientID %>">Events:</label>
                <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEventDate_SelectedIndexChanged" AutoPostBack="true" />
            </div>
            <div class="form-inline col-xs-4 text-right">
                <asp:Button ID="btnAddRecurring" runat="server" Text="Add Recurring Donations" CssClass="btn btn-primary" OnClick="btnAddRecurring_Click" />
                <asp:Button ID="btnAddNew" runat="server" Visible="false" Text="Add Single Donation" CssClass="btn btn-primary" OnClick="btnAddNew_Click" />
            </div>
            <div class="row"></div>
        </div>

        <div class="divide10"></div>

        <%--Need an event drop down {of upcoming events} - Same line switching buttons ( Add Recurring {default} / Add New )--%>

        <div class="row">
            <div class="col-sm-12 margin20">
                <asp:MultiView ID="mvDonations" runat="server" ActiveViewIndex="0">

                    <asp:View ID="vwDonationSingle" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Add a Donation</div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbDonationDescription %>">Donation Description: </label>
                                    <asp:RequiredFieldValidator ID="rfvDonationDescription"
                                        runat="server" ControlToValidate="tbDonationDescription" CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbDonationDescription" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="<%# ddlDonationType.ClientID %>">Donation Type:</label>
                                    <asp:RequiredFieldValidator ID="rvDefaultRegStatus" runat="server" ControlToValidate="ddlDonationType" InitialValue="" CssClass="ErrorDisplay"
                                        Text="* Choose Donation Type" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlDonationType" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbReqByDate.ClientID %>">Required By Date:</label>
                                    <asp:RequiredFieldValidator ID="rfvReqByDate" runat="server" ControlToValidate="tbReqByDate"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbReqByDate" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                            <div class="col-lg-2">
                                <div class="form-group">
                                    <label for="<%# tbQtyNeeded.ClientID %>">Quanity Needed:</label>
                                    <asp:TextBox ID="tbQtyNeeded" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvQtyNeeded" runat="server" ControlToValidate="tbQtyNeeded" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Double" Operator="DataTypeCheck" />
                                    <asp:RequiredFieldValidator ID="rfvQtyNeeded" runat="server" ControlToValidate="tbQtyNeeded"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <%--Each quantity needed is worth x RewardQty / Reward Units (eg - 2.5 / CP)--%>
                                    <label for="<%# tbRewardQty.ClientID %>">Each Worth:</label>
                                    <asp:TextBox ID="tbRewardQty" runat="server" Columns="4" MaxLength="4" CssClass="form-control" />
                                    <asp:CompareValidator ID="cvtRewardQty" runat="server" ControlToValidate="tbRewardQty" Display="Dynamic"
                                        CssClass="ErrorDisplay" Text="* Numbers Only" Type="Double" Operator="DataTypeCheck" />
                                    <asp:RequiredFieldValidator ID="rfvRewardQty" runat="server" ControlToValidate="tbRewardQty"
                                        CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label for="<%# ddlRewardUnit.ClientID %>">Reward Type:</label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRewardUnit" InitialValue="" CssClass="ErrorDisplay"
                                        Text="* Choose Type" Display="Dynamic" />
                                    <asp:DropDownList ID="ddlRewardUnit" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbURL %>">URL: </label>
                                    <asp:TextBox ID="tbURL" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="<%# tbDonationComments %>">Donation Comments: </label>
                                    <asp:TextBox ID="tbDonationComments" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="<%# tbStaffOnlyComments %>">Staff Only Comments: </label>
                                    <asp:TextBox ID="tbStaffOnlyComments" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbShipTo1.ClientID %>">Ship To Address 1:</label>
                                    <asp:TextBox ID="tbShipTo1" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbShipTo2.ClientID %>">Ship To Address 2:</label>
                                    <asp:TextBox ID="tbShipTo2" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbShipToCity.ClientID %>">Ship To City:</label>
                                    <asp:TextBox ID="tbShipToCity" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbShipToState.ClientID %>">Ship To State:</label>
                                    <asp:TextBox ID="tbShipToState" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbShipToZip.ClientID %>">Ship To Zip:</label>
                                    <asp:TextBox ID="tbShipToZip" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-4 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# tbShipToPhone.ClientID %>">Ship To Phone:</label>
                                    <asp:TextBox ID="tbShipToPhone" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

<%--<___> DonationID			int				Unchecked      Auto assign
<___> CampaignID				int				Unchecked      Master.CampaignID
<___> EventID					int				Checked        ddlEventDate.SelectedValue
<___> StatusID				    int				Unchecked      Auto assign based on permissions
<_x_> Description				nvarchar(MAX)	Unchecked      text box value
<___> Recurring				    bit				Unchecked      Auto assign based on view
<_x_> DonationTypeID			int				Unchecked      ddlDonationType.SelectedValue
<___> MaterialsReimbursable	    bit				Checked        Check box value
<___> ReimbuirseType			nvarchar(50)	Checked        ddlReimburseType.SelectedValue
<___> HoursAllowed			    numeric(18, 2)	Checked        text box value
<_x_> RewardQty				    numeric(18, 2)	Checked        text box value
<_x_> RewardUnit				nchar(10)		Checked        ddlRewardUnit.SelectedValue
<___> Worth					    nchar(10)		Checked        ??????????
<_x_> QtyNeeded				    numeric(18, 2)	Unchecked      text box value
<_x_> DonationComments		    nvarchar(MAX)	Checked        text box value
<_x_> URL						nvarchar(MAX)	Checked        text box value
<_x_> StaffComments			    nvarchar(MAX)	Checked        text box value
<___> HideDonators			    bit				Checked        check box value
<___> ExpirationDate			date			Checked        text box value (ajax date selector)
<___> CarryToNextEvent		    bit				Checked        check box value
<_x_> RequiredByDate			date			Checked        text box value (ajax date selector) Default to Event Start Date
<___> AwardWhen				    nchar(10)		Checked        ddlAwardWhen (default to campaign value)
<_x_> ShipToAdd1				nvarchar(50)	Checked        text box value (default to campaign value)
<_x_> ShipToAdd2				nvarchar(50)	Checked        text box value (default to campaign value)
<_x_> ShipToCity				nvarchar(25)	Checked        text box value (default to campaign value)
<_x_> ShipToState				nvarchar(25)	Checked        text box value (default to campaign value)
<_x_> ShipToPostalCode		    nvarchar(25)	Checked        text box value (default to campaign value)
<_x_> ShipToPhone				nvarchar(25)	Checked        text box value (default to campaign value)
<___> NotificationEmail		    nvarchar(100)	Checked        text box value (default to campaign value)
<___> AllowEventFee			    bit				Checked        check box value
<___> Comments				    nvarchar(MAX)	Checked        NULL
<___> DateAdded				    datetime		Unchecked      system default
<___> DateChanged				datetime		Unchecked      system default
<___> DateDeleted				datetime		Checked        NULL--%>
                    </asp:View>

                    <asp:View ID="vwDonationsRecurring" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">Add Recurring Donations</div>
                        </div>


                    </asp:View>

                </asp:MultiView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidEventID" runat="server" />
    <asp:HiddenField ID="hidEventDate" runat="server" />
    <asp:HiddenField ID="hidStatus" runat="server" />
    <asp:HiddenField ID="hidRequiredByDate" runat="server" />
    <asp:HiddenField ID="hidShip1" runat="server" />
    <asp:HiddenField ID="hidShip2" runat="server" />
    <asp:HiddenField ID="hidCity" runat="server" />
    <asp:HiddenField ID="hidState" runat="server" />
    <asp:HiddenField ID="hidZip" runat="server" />
    <asp:HiddenField ID="hidPhone" runat="server" />
</asp:Content>
