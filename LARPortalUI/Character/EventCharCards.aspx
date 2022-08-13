<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventCharCards.aspx.cs" Inherits="LarpPortal.Character.EventCharCards" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <style type="text/css">
        .TableLabel
        {
            font-weight: bold;
            text-align: right;
            padding-left: 10px;
            padding-right: 5px;
        }

        .LeftRightPadding
        {
            padding-left: 10px;
            padding-right: 10px;
        }

        .PrintButton
        {
            padding-left: 40px;
            padding-right: 40px;
            font-size: 48px;
            border-radius: 15px;
            background-color: darkblue;
            border-color: black;
            border-width: 1px;
            border-style: solid;
            color: white;
        }

            .PrintButton:hover
            {
                background-color: lightblue;
            }

        .HeaderLabel
        {
            font-size: 24px;
            font-weight: bold;
        }

        @media print
        {
            Label
            {
                font-size: 8pt;
            }

            body
            {
                font-size: 8pt;
            }

            .hiddenOnPrint
            {
                display: none;
            }

            .TableLabel
            {
                font-weight: bold;
                text-align: right;
                padding-left: 10px;
                padding-right: 5px;
                font-size: 8pt;
            }
        }

        Label
        {
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <!-- Google Tag Manager (noscript) -->
    <noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-5MQHDGS"
    height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <!-- End Google Tag Manager (noscript) -->

    <form id="form1" runat="server">
        <div>
            <asp:Repeater ID="rptrCharacter" runat="server" OnItemDataBound="rptrCharacter_ItemDataBound">
                <ItemTemplate>
                    <table border="0">
                        <tr style="vertical-align: top;">
                            <td colspan="6">
                                <asp:Label ID="lblCharName" runat="server" CssClass="HeaderLabel" /></td>
                            <td style="text-align: right; width: 300px;" class="hiddenOnPrint" rowspan="4">
                                <asp:Button ID="printButton" runat="server" CssClass="PrintButton" Text="Print" OnClientClick="javascript:window.print();" /></td>
                        </tr>
                        <tr>
                            <td class="TableLabel">Common Name: <asp:HiddenField ID="hidSkillSetID" runat="server" Value='<%# Eval("ID") %>' /></td>
                            <td>
                                <%# Eval("AKA") %>
                            </td>
                            <td class="TableLabel">Full Name: </td>
                            <td colspan="3">
                                <%# Eval("FullName") %>
                            </td>
                        </tr>
                        <tr>
                            <td class="TableLabel">Race: </td>
                            <td>
                                <asp:Label ID="lblRace" runat="server" />
                            </td>
                            <td class="TableLabel">Birthplace: </td>
                            <td>
                                <%# Eval("WhereFrom") %>
                            </td>
                            <td class="TableLabel">Player Name: </td>
                            <td>
                                <%# Eval("PlayerName") %>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="TableLabel">Total CP: </td>
                            <td>
                                <asp:Label ID="lblTotalCP" runat="server" /></td>
                            <td class="TableLabel">Total Spent: </td>
                            <td>
                                <asp:Label ID="lblCPSpent" runat="server" /></td>
                            <td class="TableLabel">Total Avail: </td>
                            <td>
                                <asp:Label ID="lblCPAvail" runat="server" /></td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:GridView ID="gvNonCost" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="LightGray" AlternatingRowStyle-BackColor="Linen">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Descriptor" HeaderStyle-CssClass="LeftRightPadding" ItemStyle-CssClass="LeftRightPadding" />
                            <asp:BoundField DataField="Value" HeaderText="Descriptor Value" HeaderStyle-CssClass="LeftRightPadding" ItemStyle-CssClass="LeftRightPadding" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                    <asp:GridView ID="gvSkills" runat="server" AutoGenerateColumns="false" RowStyle-VerticalAlign="top" HeaderStyle-BackColor="LightGray" AlternatingRowStyle-BackColor="Linen">
                        <Columns>
                            <asp:BoundField DataField="SkillName" HeaderText="Skill" HeaderStyle-CssClass="LeftRightPadding" ItemStyle-CssClass="LeftRightPadding" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="CPCostPaid" ItemStyle-HorizontalAlign="Right" HeaderText="Cost" HeaderStyle-CssClass="LeftRightPadding" ItemStyle-CssClass="LeftRightPadding"
                                DataFormatString="{0:0.00}" />
                            <asp:BoundField DataField="FullDescription" ItemStyle-HorizontalAlign="Left" HeaderText="Complete Card Description" HtmlEncode="false" ItemStyle-CssClass="LeftRightPadding"
                                HeaderStyle-CssClass="LeftRightPadding" />
                            <asp:BoundField DataField="DisplaySkill" Visible="false" />
                            <asp:BoundField DataField="DisplayOrder" Visible="false" />
                        </Columns>
                    </asp:GridView>
                    <p style="page-break-after: always" />
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
