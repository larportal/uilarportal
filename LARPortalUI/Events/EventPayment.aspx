<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="EventPayment.aspx.cs" Inherits="LarpPortal.Events.EventPayment" %>

<asp:Content ID="EventPaymentStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="EventPaymentScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="EventPaymentBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Event Registration</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <asp:HiddenField ID="hidItemName" runat="server" />
            <asp:Label ID="lblHeader" runat="server" CssClass="form-control NoShadow"></asp:Label>
            <div class="row LeftRightPadding10">
                <asp:Label ID="lblRegistrationText" runat="server" Text="" CssClass="form-control NoShadow"></asp:Label>
            </div>
            <div class="row PrePostPadding">
                <asp:CheckBox ID="chkRegistration" runat="server" Text="Add registration payment ($100.00)" Checked="true" />
            </div>
            <div class="row PrePostPadding">
                <asp:Label ID="lblFoodTextOld" runat="server" Text="" Visible="false" CssClass="form-control NoShadow"></asp:Label>
            </div>
            <div class="row PrePostPadding">
                <asp:CheckBox ID="chkSaturdayBrunch" runat="server" Text="Saturday Brunch ($9.00)" Visible="false" />
            </div>
            <div class="row PrePostPadding">
                <asp:CheckBox ID="chkSaturdayDinner" runat="server" Text="Saturday Dinner ($9.00)" Visible="false" />
            </div>
            <div class="row PrePostPadding">
                <asp:CheckBox ID="chkSundayBrunch" runat="server" Text="Sunday Brunch ($9.00)" Visible="false" />
            </div>
            <div class="row PrePostPadding">
                <asp:CheckBox ID="chkAllMeals" runat="server" Text="All three meals ($27.00)" Visible="false" />
            </div>
            <div class="row PrePostPadding" runat="server">&nbsp;</div>
            <asp:Button ID="btnCalculateOrder" runat="server" CssClass="StandardButton" Text="Calculate Amount" Visible="false" OnClick="btnCalculateOrder_Click" />
            <div class="row PrePostPadding" runat="server">&nbsp;</div>
            <div class="row PrePostPadding" runat="server">
                <div>
                    <asp:Label ID="lblOrderTotalDisplay" runat="server" Visible="true" CssClass="form-control NoShadow"></asp:Label><br />
                </div>
                <div>
                    <asp:Label ID="lblOrderTotalSection" runat="server" Visible="true" CssClass="form-control NoShadow"></asp:Label>
                </div>
                <div>
                    <asp:ImageButton ID="btnPayPalTotal" runat="server" ImageUrl="https://www.paypalobjects.com/en_US/i/btn/btn_buynow_LG.gif" PostBackUrl="https://secure.paypal.com/cgi-bin/webscr" Visible="true" OnClick="btnPayPalTotal_Click" />
                </div>
                <div>
                    <asp:Label ID="lblPaymentNote" runat="server" Visible="true" CssClass="form-control NoShadow">NOTE: Payments will only reflect on your registration in LARP Portal once staff has applied the payment.  This usually happens the week before the event."</asp:Label>
                        <br /><br />
                        If there are issues with reaching PayPal through LARP Portal, use the PayPal option at the Madrigal site. <br />
                        <asp:HyperLink ID="hplCampaingURL" runat="server" NavigateUrl="https://madrigallarp.wordpress.com/payment-options/" Text="https://madrigallarp.wordpress.com/payment-options/" Target="_blank"></asp:HyperLink>
                </div>
                <div>
                    <asp:Label ID="lblClosePayPalForm" runat="server" Text="" CssClass="form-control NoShadow"></asp:Label>
                </div>
            </div>
            <div class="row PrePostPadding">
                <asp:Label ID="lblFoodText" runat="server" Text="" Visible="true" CssClass="form-control NoShadow"></asp:Label>
            </div>


            <asp:Label ID="lblFooter" runat="server" Visible="false" CssClass="form-control NoShadow"></asp:Label>

            <div class="row">&nbsp;</div>
            <div class="row">
                <div class="col-sm-11"></div>
                <div class="col-sm-1">
                    <asp:Button ID="btnClose" runat="server" CssClass="StandardButton" Text="Close" OnClick="btnClose_Click" />
                </div>
            </div>
            <div class="row"></div>
        </div>
    </div>
</asp:Content>

