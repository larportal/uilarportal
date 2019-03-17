<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Medical.aspx.cs" Inherits="LarpPortal.Reports.Medical" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CheckInStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="CheckInScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function EventChange() {
            try {
                var ddlEvent = document.getElementById("<%= ddlEvent.ClientID %>");
                var btnRunReport = document.getElementById("<%= btnRunReport.ClientID %>");
                var btnExportExcel = document.getElementById("<%= btnExportExcel.ClientID %>");

                btnRunReport.style.display = "none";
                if (btnExportExcel)
                    btnExportExcel.style.display = "none";

                var EventDate = ddlEvent.options[ddlEvent.selectedIndex].value;

                if (EventDate != "") {
                    btnRunReport.style.display = "inline";
                }
            }
            catch (err) {
                return false;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="CheckInBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Medical Report</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                <label for="ddlEvent" runat="server" id="lblEvent" style="padding-left: 10px;">Event: </label>
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control" />
                <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" />
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" />
            </div>
        </div>
        <div class="margin10"></div>
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Medical Concerns</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                        <asp:GridView ID="gvCheckList" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed">
                                            <Columns>
                                                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                                <asp:BoundField DataField="CharacterAKA" HeaderText="Character" />
                                                <asp:BoundField DataField="EmergencyContact" HeaderText="Emergency Contact" />
                                                <asp:BoundField DataField="EmergencyPhone" HeaderText="Emergency Phone" />
                                                <asp:BoundField DataField="Allergies" HeaderText="Allergies" />
                                                <asp:BoundField DataField="MedicalCondition" HeaderText="Medical Condition" />
                                                <asp:BoundField DataField="ConditionStartDate" HeaderText="Condition Start Date" />
                                                <asp:BoundField DataField="ConditionEndDate" HeaderText="Condition End Date" />
                                                <asp:BoundField DataField="Limitations" HeaderText="Limitations" />
                                                <asp:BoundField DataField="LimitationStartDate" HeaderText="Limitation Start Date" />
                                                <asp:BoundField DataField="LimitationEndDate" HeaderText="Limitation End Date" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
