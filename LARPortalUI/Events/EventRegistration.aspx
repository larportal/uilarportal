<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="EventRegistration.aspx.cs" Inherits="LarpPortal.Events.EventRegistration" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .autoWidth {
            width: auto !important;
        }

        .TableLabel {
            font-weight: bold !important;
            text-align: right !important;
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

        .NoShadow {
            border: 0px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 0px 0px rgba(0, 0, 0, .075);
            box-shadow: inset 0 0px 0px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

        /*div {
            border: solid 1px black;
        }*/
    </style>
</asp:Content>
<asp:Content ID="EventRegistrationScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript" src="../Scripts/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="../Scripts/moment.min.js"></script>
    <script type="text/javascript">
        function ddl_changed(ddl) {
            var panel = document.getElementById("<%= divFullEventNo.ClientID %>");
            if (panel) {
                if (ddl.value == "Y")
                    panel.style.display = "none";
                else
                    panel.style.display = "block";
            }
        }

        function ddlSendToCampaign(ddl) {
            var panel = document.getElementById("<%= tbSendToCPOther.ClientID %>");
            if (panel) {
                if (ddl.value == "-1")
                    panel.style.display = "block";
                else
                    panel.style.display = "none";
            }
        }

        $(function () {
            $("#<%= tbArriveDate.ClientID %>").datepicker();
            $("#<%= tbDepartDate.ClientID %>").datepicker();
        });

        function openModal() {
            $('#myModal').modal('show');
        }
        function closeModal() {
            $('#myModal').hide();
        }

        function openPayPalWindow() {
            var win = window.open("/Events/EventPayment.aspx", "_blank");
            win.focus();
        }

        function openHousing() {
            var winh = window.open("/Events/ChooseHousing.aspx", "_blank");
            winh.focus
        }

        function CampaignSpecific() {
            alert("We got to CampaignSpecific");
<%--            var CampaignName = document.getElementById('<%= hidCampaignName.ClientID%>').value;
            alert(CampaignName);
            var dropdown = document.getElementById('<%= ddlRoles.ClientID %>');
            var selectedOption = dropdown.options[dropdown.selectedIndex];
            var selectedText = selectedOption.text;
            alert(selectedText);
            alert("no more");

            if (CampaignName == "Myth") {
                alert(CampaignName);
                if (selectedText == "PC") {
                    alert(selectedText);
                    var winh = window.open("/Events/ChooseHousing.aspx", "_blank");
                    win.focus();
                }
                else {
                    alert(selectedText);
                    var winp = window.open("/Events/EventPayment.aspx", "_blank");
                    win.focus();
                }
            }
            else {
                alert(CampaignName);
            }--%>
        }

        function enablePayNowButton(ddlId) {
            var PayPalID = document.getElementById('<%= hidPayPalTypeID.ClientID%>').value;
            var ddlPaymentChoice = document.getElementById(ddlId.id);
            var btnPayNow = document.getElementById('<%= btnPayNow.ClientID %>');

            if (btnPayNow)
                if (ddlPaymentChoice.value == PayPalID) {
                    btnPayNow.style.display = '';
                }
                else {
                    btnPayNow.style.display = 'none';
                }
        }

        function openNoPCChar() {
            $('#modalNoPCChar').modal('show');
        }
        function closeNoPCChar() {
            $('#modalNoPCChar').hide();
        }

    </script>
</asp:Content>

<asp:Content ID="EventRegistrationBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Event Registration</h1>
                </div>
            </div>
        </div>

        <asp:MultiView ID="mvEventInfo" runat="server">
            <asp:View ID="vwEventInfo" runat="server">
                <div class="row">
                    <div class="form-inline col-xs-12">
                        <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                        <label for="<%= ddlEventDate.ClientID %>" style="padding-left: 25px;">Events:</label>
                        <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEventDate_SelectedIndexChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="divide10"></div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Event Registration
                                <span class="small" style="font-weight: normal; padding-left: 25px;">
                                    <label for="<%= lblRegistrationStatus.ClientID %>">My Current Status:</label>
                                    <asp:Label ID="lblRegistrationStatus" runat="server" CssClass="" />
                                </span>
                                <span class="small" style="font-weight: normal; padding-left: 25px;">
                                    <label for="<%= lblPaymentStatus.ClientID %>">Payment Status:</label>
                                    <asp:Label ID="lblPaymentStatus" runat="server" Text="No payment" />
                                </span>
                            </div>

                            <div class="row">
                                <div class="panel-body">
                                    <div class="container-fluid">
                                        <div class="row">
                                            <div class="col-lg-1 col-md-2 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="ddlRoles">Role</label>
                                                    <asp:DropDownList ID="ddlRoles" runat="server" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" CssClass="form-control autoWidth" AutoPostBack="true">
                                                        <asp:ListItem Text="PC" Value="PC" />
                                                        <asp:ListItem Text="NPC" Value="NPC" />
                                                        <asp:ListItem Text="Staff" Value="Staff" />
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblRole" runat="server" CssClass="form-control NoShadow" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="lblPlayerName">Player</label>
                                                    <asp:Label ID="lblPlayerName" runat="server" CssClass="form-control" />
                                                    <asp:DropDownList ID="ddlPlayerName" runat="server" CssClass="form-control autoWidth" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayerName_SelectedIndexChanged" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="lblEMail">EMail</label>
                                                    <asp:Label ID="lblEMail" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6 col-xs-12" runat="server" id="divCharacters">
                                                <div class="form-group autoWidth">
                                                    <label for="">Character</label>
                                                    <asp:Label ID="lblSkillSet" runat="server" CssClass="form-control NoShadow autoWidth" />
                                                    <asp:DropDownList ID="ddlSkillSetID" runat="server" CssClass="form-control autoWidth" />
                                                </div>
                                                <div class="form-group autoWidth" id="divSendCPTo" runat="server">
                                                    <label for="ddlSendToCampaign">Send CP to</label>
                                                    <asp:DropDownList ID="ddlSendToCampaign" runat="server" CssClass="form-control autoWidth" />
                                                    <asp:RequiredFieldValidator ErrorMessage="* Required" ControlToValidate="ddlSendToCampaign"
                                                        InitialValue="0" runat="server" ForeColor="Red" />
                                                    <asp:TextBox ID="tbSendToCPOther" runat="server" CssClass="form-control" MaxLength="500" Style="display: none;" TextMode="MultiLine" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="ddlFullEvent">Full Event</label>
                                                    <asp:DropDownList ID="ddlFullEvent" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Yes" Value="Y" Selected="True" />
                                                        <asp:ListItem Text="No" Value="N" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <span class="" id="divFullEventNo" style="display: none;" runat="server">
                                                <span class="col-md-4 col-sm-6 col-xs-12">
                                                    <span class="row">
                                                        <span class="col-xs-6 NoPadding">
                                                            <label for="tbArriveDate">Arrival Date/Time</label>

                                                            <asp:TextBox ID="tbArriveDate" runat="server" CssClass="form-control" />
                                                            <asp:TextBox ID="tbArriveTime" runat="server" CssClass="form-control" TextMode="Time" />
                                                        </span>
                                                        <span class="col-xs-6 NoPadding">
                                                            <label for="tbDepartDate">Depart Date/Time</label>
                                                            <asp:TextBox ID="tbDepartDate" runat="server" CssClass="form-control" />
                                                            <asp:TextBox ID="tbDepartTime" runat="server" CssClass="form-control" TextMode="Time" />
                                                        </span>
                                                    </span>
                                                </span>
                                            </span>
                                            <div class="" runat="server" id="divTeams">
                                                <div class="col-md-4 col-sm-6 col-xs-12">
                                                    <%--                                                <div class="row">--%>
                                                    <div class="form-group">
                                                        <label for="<%= ddlTeams.ClientID %>">Team Name</label>
                                                        <asp:DropDownList ID="ddlTeams" runat="server" CssClass="form-control autoWidth">
                                                            <asp:ListItem Text="Team 1" Value="Team1" />
                                                            <asp:ListItem Text="Team 2" Value="Team2" />
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblNoTeams" runat="server" Text="No Teams" CssClass="form-control NoShadow" />
                                                    </div>
                                                    <%--                                                </div>--%>
                                                </div>
                                            </div>
                                            <div class="" runat="server" id="divHousePref">
                                                <div class="col-md-4 col-sm-6 col-xs-12">
                                                    <div class="form-group">
                                                        <label for="<%= tbReqstdHousing.ClientID %>">Housing Preferences</label>
                                                        <asp:TextBox ID="tbReqstdHousing" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lblReqstdHousing" runat="server" CssClass="form-control NoShadow" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="" runat="server" id="divHouseAssign">
                                                <div class="col-md-4 col-sm-6 col-xs-12">
                                                    <div class="form-group">
                                                        <label for="<%= lblAssignHousing.ClientID %>">Assigned Housing</label>
                                                        <asp:Label ID="lblAssignHousing" runat="server" CssClass="form-control NoShadow" Text="No Housing Assigned" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-12 col-xs-12">
                                                <div class="form-group">
                                                    <label>Meal Plan</label>
                                                    <div class="col xs-4">
                                                        <asp:CheckBoxList ID="cbxlMeals" runat="server" RepeatDirection="Horizontal" CssClass="col-xs-4" />
                                                        <asp:Label ID="lblNoFoodLabel" runat="server" CssClass="form-control NoShadow col-lg-12 no-padding" Text="This event has not been set up with food services." />
                                                    </div>
                                                </div>
                                            </div>
                                            <span class="col-md-4 col-sm-6 col-xs-12" runat="server" id="divPaymentInstructions">
                                                <div class="form-group">
                                                    <label for="<%= lblPaymentInstructions.ClientID %>">Payment Instructions</label>
                                                    <asp:Label ID="lblPaymentInstructions" runat="server" CssClass="col-lg-12 no-padding" />
                                                </div>
                                            </span>

                                            <div class="col-md-4 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="<%= ddlPaymentChoice.ClientID %>">Payment Choice</label>
                                                    <span class="form-inline">
                                                        <asp:DropDownList ID="ddlPaymentChoice" runat="server" CssClass="form-control autoWidth" />
                                                        <asp:Label ID="lblPaymentChoice" runat="server" CssClass="form-control NoShadow" />
                                                        <asp:Button ID="btnPayNow" runat="server" CssClass="btn btn-primary" Text="Pay Now" OnClientClick="openPayPalWindow(); return false;" />
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="<%= tbComments.ClientID %>">Comments</label>
                                                    <asp:TextBox ID="tbComments" runat="server" TextMode="MultiLine" CssClass="col-xs-11 NoPadding form-control" Rows="4" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnlButtons" runat="server" Visible="true">
                        <div class="row col-xs-12 text-center" style="padding-top: 20px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger MarginLeftRight" OnCommand="CancelReg" />
                            <asp:Button ID="btnChange" runat="server" Text="Change Reg" CssClass="btn btn-primary MarginLeftRight" OnCommand="Register" />
                            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary MarginLeftRight" OnCommand="Register" />
                            <asp:Button ID="btnRSVPNo" runat="server" Text="I Cannot Attend" CssClass="btn btn-danger MarginLeftRight" OnCommand="RSVPEvent" CommandName="RSVPNO" />
                            <asp:Button ID="btnRSVP" runat="server" Text="I Will Attend" CssClass="btn btn-primary MarginLeftRight" OnCommand="RSVPEvent" CommandName="RSVP" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlSpecialButtons" runat="server" Visible="false">
                        <div class="row col-xs-12 text-center" style="padding-top: 20px;">
                            <%--PCs - Housing goes to Food and Pay
                            NPCs/Staff - Skips housing and goes right to Food and Pay (which skips pay for NPCs/staff--%>
                            <asp:Button ID="btnHousing" runat="server" Text="Select Housing" Visible="false" CssClass="btn btn-primary MarginLeftRight" OnClientClick="openHousing();" OnClick="btnHousing_Click" />
                            <asp:Button ID="btnFoodPay" runat="server" Text="Select Food" Visible="false" CssClass="btn btn-primary MarginLeftRight" OnClientClick="openPayPalWindow();" OnClick="btnFoodPay_Click"/>
                        </div>
                    </asp:Panel>

                    <div class="row col-xs-12 text-center">
                        <asp:Label ID="lblWhyRSVP" runat="server" Visible="false" Text="Currently this event is not yet open for registration.<br />By letting the owners know whether you plan to attend an event you will help in planning.<br />" />
                        <asp:Label ID="lblAlreadyHappened" runat="server" Visible="false" Text="This event has already happened. You cannot change your registration after the event." />
                        <asp:Label ID="lblClosedToPC" runat="server" Visible="false" Text="Event has been closed to new PC registration.<br />" />
                        <asp:Label ID="lblPastEvent" runat="server" Text="Event  has already happened." />
                    </div>

                </div>
                <br />
                <div class="row col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Event Details</div>
                        <div class="panel-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Event Status:</span>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:Label ID="lblEventStatus" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Event Name:</span>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:Label ID="lblEventName" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Description:</span>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:Label ID="lblEventDescription" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">In Game Location:</span>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:Label ID="lblInGameLocation" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Start Date:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblEventStartDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">End Date:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblEventEndDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Site Location:</span>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:Label ID="lblSiteLocation" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Reg Open Date:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblEventOpenDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Reg Close Date:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblEventCloseDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Pre Reg Price:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblPreRegPrice" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Pre Reg:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblPreRegDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Reg Price:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblRegPrice" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Payment Due:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblPaymentDue" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">At Door Price:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblDoorPrice" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">Info Skill Due:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblInfoSkillDueDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        <span class="form-control NoShadow">PEL Due:</span>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="lblPELDueDate" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row" style="padding-top: 6px;">
                                    <div class="TableLabel col-xs-3">
                                        <span style="padding: 6px 12px; color: #555555;">Available:</span>
                                    </div>
                                    <div class="col-xs-9" style="padding-left: 25px;">
                                        <asp:Image ID="imgPCFoodService" runat="server" ImageUrl="~/img/delete.png" />
                                        PC Food Services
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        &nbsp;
                                    </div>
                                    <div class="col-xs-9" style="padding-left: 25px;">
                                        <asp:Image ID="imgNPCFoodService" runat="server" ImageUrl="~/img/delete.png" />
                                        NPC Food Services
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        &nbsp;
                                    </div>
                                    <div class="col-xs-9" style="padding-left: 25px;">
                                        <asp:Image ID="imgCookingAllowed" runat="server" ImageUrl="~/img/delete.png" />
                                        Cooking Allowed
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        &nbsp;
                                    </div>
                                    <div class="col-xs-9" style="padding-left: 25px;">
                                        <asp:Image ID="imgRefrigerator" runat="server" ImageUrl="~/img/delete.png" />
                                        Refigerator
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        &nbsp;
                                    </div>
                                    <div class="col-xs-9" style="padding-left: 25px;">
                                        <asp:Image ID="imgMenu" runat="server" ImageUrl="~/img/delete.png" />
                                        Menu Prices 
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="TableLabel col-xs-3">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwNoEvents" runat="server">
                <h1 class="text-center">There are no events for this campaign.</h1>
            </asp:View>
        </asp:MultiView>
        <div class="row">
            &nbsp;
        </div>
        <asp:HiddenField ID="hidRegistrationID" runat="server" />
        <asp:HiddenField ID="hidCharacterID" runat="server" />
        <asp:HiddenField ID="hidTeamMember" runat="server" Value="0" />
        <asp:HiddenField ID="hidRegisterOthers" runat="server" Value="" />
        <asp:HiddenField ID="hidLastPlayerRegister" runat="server" Value="" />
        <div class="row" style="padding-bottom: 25px;">&nbsp;</div>
    </div>

    <div class="col-xs-12 NoGutters" id="divNoEvents" runat="server">
        There are no upcoming events for this campaign.
    </div>
    <div class="col-xs-12 NoGutters" id="divNoPriv" runat="server">
        You do not have the privileges required to register for an event for this campaign.
    </div>

    <div class="modal fade in" id="myModal" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Registration</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblRegistrationMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalNoPCChar" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Registration</h3>
                </div>
                <div class="modal-body">
                    You do not have any PC Characters. In order to register for an event as a PC, you must first create one.<br />
                    <br />
                    Do you want to create a character now?
                </div>
                <div class="modal-footer">
                    <div class="text-left col-lg-6">
                        <asp:Button ID="btnStayAsNPC" runat="server" Text="Stay as NPC" CssClass="btn btn-primary" OnClick="btnStayAsNPC_Click" />
                    </div>
                    <div class="text-right col-lg-6">
                        <asp:Button ID="btnCreateACharacter" runat="server" Text="Create Character" CssClass="btn btn-primary" OnClick="btnCreateACharacter_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidCampaignName" runat="server" />
    <asp:HiddenField ID="hidCampaignEMail" runat="server" />
    <asp:HiddenField ID="hidCharAKA" runat="server" />
    <asp:HiddenField ID="hidPlayerEMail" runat="server" />
    <asp:HiddenField ID="hidRegistrationStatusID" runat="server" />
    <asp:HiddenField ID="hidPayPalTypeID" runat="server" />

    <asp:HiddenField ID="hidHasPCChar" runat="server" />
    <asp:HiddenField ID="hidRegOpen" runat="server" />
    <asp:HiddenField ID="hidRegClose" runat="server" />
    <asp:HiddenField ID="hidEventClose" runat="server" />
    <asp:HiddenField ID="hidPELClose" runat="server" />
    <asp:HiddenField ID="hidCurrentRegStatus" runat="server" />
    <asp:HiddenField ID="hidPlayerRegistering" runat="server" />

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker();
        });
    </script>
</asp:Content>
