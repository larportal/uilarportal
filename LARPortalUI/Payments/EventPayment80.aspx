<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventPayment80.aspx.cs" Inherits="LarpPortal.Payments.EventPayment80" %>

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
                            <h1 class="col-sm-12">Crossover - Event Registration Payment</h1>
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
                                            <asp:Label ID="lblPageText" runat="server">
                                                Crossover events are $80 if paid in advance, or $85 if you pay less than three weeks before the event. 
                                                You can pay through PayPal, or by cash or check at the door.<br /><br />
                                                We ask that if you pay in the last few days before an event, please use cash or check at the door, since there
                                                 is a delay before we can access PayPal funds to pay the site fee.<br /><br />
                                                If you need to make other arrangements, or have questions, please 
                                                <a href="https://sites.google.com/site/crossoverrules/home/contact-info">contact us by email.</a><br /><br />
                                                If you'd rather pay from the Crossover site <a href="https://sites.google.com/site/crossoverrules/home/event-payment">click here.</a><br /><br />
                                            </asp:Label>
                                            <asp:Label ID="lblEarlyOrLate" runat="server">Crossover Event (Paid in advance)</asp:Label>
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
                                                    <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="https://www.paypalobjects.com/webstatic/en_AU/i/buttons/btn_paywith_primary_s.png" PostBackUrl="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=PYM9SG4S63CX2" Visible="true" OnClick="btnPayPalTotal_Click" />--%>
                                                    <asp:Label ID="lblImageButton" runat="server"></asp:Label><br /><br /><br />
                                                    Food service payments can be made to Suhayma <a href="https://www.paypal.me/Suhayma" rel="nofollow">here.</a>
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
