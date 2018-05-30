<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CharacterCardPrint.aspx.cs" Inherits="LarpPortal.Reports.CharacterCardPrint" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharacterCardPrintStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>

<asp:Content ID="CharacterCardPrintScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function CardsForChange() {
            try {
                var ddlCardsFor = document.getElementById("<%= ddlCardsFor.ClientID %>");
                var ddlEvent = document.getElementById("<%= ddlEvent.ClientID %>");
                var lblEvent = document.getElementById("<%= lblEvent.ClientID %>");
                var ddlCharacter = document.getElementById("<%= ddlCharacter.ClientID %>");
                var lblCharacter = document.getElementById("<%= lblCharacter.ClientID %>");
                var btnRunReport = document.getElementById("<%= btnRunReport.ClientID %>");

                ddlEvent.style.display = "none";
                lblEvent.style.display = "none";
                ddlCharacter.style.display = "none";
                lblCharacter.style.display = "none";
                btnRunReport.style.display = "none";

                var CardsForText = ddlCardsFor.options[ddlCardsFor.selectedIndex].text;

                if (CardsForText == "Campaign") {
                    btnRunReport.style.display = "inline";
                }
                else if (CardsForText == "Event") {
                    ddlEvent.style.display = "inline";
                    lblEvent.style.display = "inline";
                }
                else if (CardsForText == "Character") {
                    ddlCharacter.style.display = "inline";
                    lblCharacter.style.display = "inline";
                }
            }
            catch (err) {
                return false;
            }
            return false;
        }

        function EventChange() {
            try {
                var ddlEvent = document.getElementById("<%= ddlEvent.ClientID %>");
                var btnRunReport = document.getElementById("<%= btnRunReport.ClientID %>");

                btnRunReport.style.display = "none";

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

        function CharacterChange() {
            try {
                var ddlCharacter = document.getElementById("<%= ddlCharacter.ClientID %>");
                var btnRunReport = document.getElementById("<%= btnRunReport.ClientID %>");

                btnRunReport.style.display = "none";

                var CharacterID = ddlCharacter.options[ddlCharacter.selectedIndex].value;

                if (CharacterID != "") {
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
<asp:Content ID="CharacterCardPrintBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character Cards</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                <label for="<%= ddlCardsFor.ClientID %>" style="padding-left: 10px;">Print Cards For:</label>
                <asp:DropDownList ID="ddlCardsFor" runat="server" CssClass="form-control autoWidth">
                    <asp:ListItem Text="Select Report Type" Value="" Selected="true" />
                    <asp:ListItem Text="Campaign" Value="Campaign" />
                    <asp:ListItem Text="Event" Value="Event" />
                    <asp:ListItem Text="Character" Value="Character" />
                </asp:DropDownList>

                <label for="ddlEvent" runat="server" id="lblEvent" style="padding-left: 10px;">Event: </label>
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control" />

                <label for="ddlCharacter" runat="server" id="lblCharacter" style="padding-left: 10px;">Character</label>
                <asp:DropDownList ID="ddlCharacter" runat="server" CssClass="form-control" />

                <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" OnClientClick="target = '_blank';" />
            </div>
        </div>
    </div>
</asp:Content>
