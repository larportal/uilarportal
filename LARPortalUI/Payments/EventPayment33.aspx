<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventPayment33.aspx.cs" Inherits="LarpPortal.Payments.EventPayment33" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="contentArea">
        <aside></aside>
        <div class="mainContent tab-content col-lg-6 input-group">
            <section id="campaign-info" class="campaign-info tab-pane active">
                <form id="frmHidFields" runat="server">
                    <div role="form" class="form-horizontal">
                        <div class="col-sm-12 NoPadding">
                            <h1 class="col-sm-12">Fifth Gate - Event Registration Payment</h1>
                        </div>
                        <div class="row col-sm-12 NoPadding" style="padding-left: 25px;">
                            <div class="col-lg-12 NoPadding" style="padding-left: 15px;">
                                <div class="panel NoPadding" style="padding-top: 0px; padding-bottom: 0px; min-height: 50px;">
                                    <div class="panelheader NoPadding">
                                        <h2>
                                            <asp:Label ID="lblPlayerEventCharacter" runat="server" Text="Player - Event - Character"></asp:Label></h2>
                                    </div>
                                    <div class="panel-body NoPadding">
                                        <div class="panel-container NoPadding">
                                            <asp:Label ID="lblPageText" runat="server" Text="The event fee for Fifth Gate events is $80."></asp:Label>
                                            <div class="paypalWrapper">
                                                <div class="errors hidden checkoutErrors">
                                                    <div class="paypalButtonContainer ani_cta_none">
                                                        <div class="bgradient">
                                                            <div class="paypalButton">
                                                                <asp:Label ID="lblPayPalForm" runat="server" Text="lblPayPalForm text here"></asp:Label>
                                                                <%--<asp:TextBox ID="tbPayPalFormCode" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>--%>
                                                                <%--                                                                <form class="paypalForm" action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_blank" novalidate="novalidate">
                                                                    <input name="business" type="hidden" value="owner@fifthgatelarp.com" />
                                                                    <input name="item_name" type="hidden" value="Fifth Gate Event" />
                                                                    <input name="bn" type="hidden" value="POWr_SP" />
                                                                    <input name="charset" type="hidden" value="UTF-8" />
                                                                    <input name="currency_code" type="hidden" value="USD" />
                                                                    <input name="cmd" type="hidden" value="_xclick" />
                                                                    <input name="amount" type="hidden" value="80.00" />
                                                                    <input name="no_shipping" type="hidden" value="1" />
                                                                    <input name="undefined_quantity" type="hidden" value="1" />
                                                                    <input name="quantity" type="hidden" value="1" />
                                                                    <input name="return" type="hidden" value="https://www.powr.io/plugins/" />
                                                                    <input name="rm" type="hidden" value="1" />
                                                                    <input name="notify_url" type="hidden" value="https://www.powr.io/payment_notification/5262016" />
                                                                    <div class="submitButton fitText" style="font-size: 14px;">Pay With PayPal</div>
                                                                </form>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row PrePostPadding" runat="server">&nbsp;</div>
                                            <%--<asp:Button ID="btnCalculateOrder" runat="server" CssClass="StandardButton" Text="Calculate Amount" Visible="false" OnClick="btnCalculateOrder_Click" />--%>
                                            <div class="row PrePostPadding" runat="server">&nbsp;</div>
                                            <div class="row PrePostPadding" runat="server">
                                                <div>
                                                    <asp:HiddenField ID="hidItemName" runat="server" />
                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="https://www.paypalobjects.com/webstatic/en_AU/i/buttons/btn_paywith_primary_s.png" PostBackUrl="https://www.paypal.com/webapps/shoppingcart?flowlogging_id=4037632035743&mfid=1497214843749_4037632035743" Visible="true" OnClick="btnPayPalTotal_Click" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row PrePostPadding">&nbsp;</div>
                        <div class="row PrePostPadding">
                            <div class="col-sm-11"></div>
                            <div class="col-sm-1">
                                <asp:Button ID="Button1" runat="server" CssClass="StandardButton" Text="Close" OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </div>
                </form>
            </section>
        </div>
    </div>
</body>
</html>
