<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventPayment151.aspx.cs" Inherits="LarpPortal.Payments.EventPayment151" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Google Tag Manager -->
    <script>(function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start':
                    new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
                    'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', 'GTM-5MQHDGS');</script>
    <!-- End Google Tag Manager -->

    <title></title>
</head>
<body>
    <!-- Google Tag Manager (noscript) -->
    <noscript>
        <iframe src="https://www.googletagmanager.com/ns.html?id=GTM-5MQHDGS"
            height="0" width="0" style="display: none; visibility: hidden"></iframe>
    </noscript>
    <!-- End Google Tag Manager (noscript) -->

    <div class="contentArea">
        <aside></aside>
        <div class="mainContent tab-content col-lg-6 input-group">
            <section id="campaign-info" class="campaign-info tab-pane active">
                <form id="frmHidFields" runat="server">
                    <div role="form" class="form-horizontal">
                        <div class="col-sm-12 NoPadding">
                            <h1 class="col-sm-12">Myth - Event Registration</h1>
                        </div>

                        <asp:Panel ID="pnlNPCFood" runat="server" Visible="false">
                            <div>Select meal option - NPCs eat free</div>
                            <asp:RadioButtonList ID="rblNPCFoodChoice" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Selected="True" Value="0">I will provide my own meals.</asp:ListItem>
                                <asp:ListItem Value="1">I would like the Vegan option.</asp:ListItem>
                                <asp:ListItem Value="2">I would like the Meat option.</asp:ListItem>
                            </asp:RadioButtonList>
                            <br /><br />
                            <asp:Button ID="btnSaveNPCMeal" runat="server" Text="Save Meal Choice" OnClick="btnSaveNPCMeal_Click" />
                        </asp:Panel>

                        <asp:Panel ID="pnlPCFood" runat="server" Visible="false">
                            <div>Select meal option - All meals cost $30</div>
                            <asp:RadioButtonList ID="rblPCFoodChoice" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Selected="True" Value="0">I will provide my own meals.</asp:ListItem>
                                <asp:ListItem Value="1">I would like to purchase the Vegan option.</asp:ListItem>
                                <asp:ListItem Value="2">I would like to purchase the Meat option.</asp:ListItem>
                            </asp:RadioButtonList>
                            <br /><br />
                            <asp:Button ID="btnSavePCMeal" runat="server" Text="Save Meal Choice" OnClick="btnSavePCMeal_Click" />
                        </asp:Panel>

                        <asp:Panel ID="pnlPay" runat="server" Visible="false">
                            <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                                <div class="col-lg-12 NoPadding" style="padding-left: 15px;">
                                    <div class="panel NoPadding" style="padding-top: 0px; padding-bottom: 0px; min-height: 50px;">
                                        <div class="panelheader NoPadding">
                                            <h2>
                                                <asp:Label ID="lblPlayerEventCharacter" runat="server" Text="Player - Event - Character"></asp:Label></h2>
                                        </div>
                                        <div class="panel-body NoPadding">
                                            <div class="panel-container NoPadding">
                                                <asp:Label ID="lblPageText" runat="server">
                                                Myth events are $95.00, $75.00 if it's your first event. 
                                                You can pay through PayPal.<br /><br />
                                                Payment must be made at time of registration. <br /><br />
                                                Click payment button to pay now or Cancel Registration to register later.<br /><br />
                                                <div>
                                                    <asp:Button ID="btnCancelRegistration" runat="server" OnClick="btnCancelRegistration_Click" />
                                                </div>
                                                <%--<a href="https://paypal.me/eric@ctfaire.com/75">Pay through PayPal.</a><br />--%>
                                                </asp:Label>
                                                <%--<asp:Label ID="lblEarlyOrLate" runat="server">Myth Event</asp:Label>--%>
                                                <div class="paypalWrapper">
                                                    <div class="errors hidden checkoutErrors">
                                                        <div class="paypalButtonContainer ani_cta_none">
                                                            <div class="bgradient">
                                                                <div class="paypalButton">
                                                                    <asp:Label ID="lblPayPalForm" runat="server" Text="lblPayPalForm text here"></asp:Label>
                                                                    <%--<asp:TextBox ID="tbPayPalFormCode" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="row PrePostPadding" runat="server">&nbsp;</div>--%>
                                                <%--<asp:Button ID="btnCalculateOrder" runat="server" CssClass="StandardButton" Text="Calculate Amount" Visible="false" OnClick="btnCalculateOrder_Click" />--%>
                                                <%--<div class="row PrePostPadding" runat="server">&nbsp;</div>--%>
                                                <div class="row PrePostPadding" runat="server">
                                                    <div>
                                                        <asp:HiddenField ID="hidItemName" runat="server" />
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="https://www.paypalobjects.com/webstatic/en_AU/i/buttons/btn_paywith_primary_s.png" PostBackUrl="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=PYM9SG4S63CX2" Visible="true" OnClick="btnPayPalTotal_Click" />
                                                        <asp:Label ID="lblImageButton" runat="server" Visible="false"></asp:Label><br />
                                                        <br />
                                                        <br />
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                </form>
            </section>
        </div>
    </div>
</body>
</html>
