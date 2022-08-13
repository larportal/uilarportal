<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="CharCard.aspx.cs" Inherits="LarpPortal.Character.CharCard" %>

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
                font-size: 10pt;
            }

            body
            {
                font-size: 10pt;
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
                font-size: 10pt;
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
            <asp:Table runat="server" ID="tblCharInfo">
                <asp:TableRow runat="server" VerticalAlign="top">
                    <asp:TableCell runat="server" ColumnSpan="6">
                        <asp:Label ID="lblCharName" runat="server" CssClass="HeaderLabel" />
                    </asp:TableCell>
                    <asp:TableCell runat="server" HorizontalAlign="Right" Width="300px" CssClass="hiddenOnPrint" RowSpan="4">
                        <asp:Button ID="printButton" runat="server" CssClass="PrintButton" Text="Print" OnClientClick="javascript:window.print();" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" CssClass="TableLabel">Common Name: </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblAKA" runat="server" /></asp:TableCell>
                    <asp:TableCell CssClass="TableLabel">Full Name: </asp:TableCell>
                    <asp:TableCell ColumnSpan="3">
                        <asp:Label ID="lblFullName" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" CssClass="TableLabel">Race: </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblRace" runat="server" /></asp:TableCell>
                    <asp:TableCell runat="server" CssClass="TableLabel">Birthplace: </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblOrigin" runat="server" /></asp:TableCell>
                    <asp:TableCell runat="server" CssClass="TableLabel">Player Name: </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblPlayerName" runat="server" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
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
                    <asp:BoundField DataField="CostString" ItemStyle-HorizontalAlign="Left" HeaderText="Cost" HeaderStyle-CssClass="LeftRightPadding" ItemStyle-CssClass="LeftRightPadding" />
                    <asp:BoundField DataField="FullDescription" ItemStyle-HorizontalAlign="Left" HeaderText="Complete Card Description" HtmlEncode="false" ItemStyle-CssClass="LeftRightPadding"
                        HeaderStyle-CssClass="LeftRightPadding" />
                    <asp:BoundField DataField="DisplaySkill" Visible="false" />
                    <asp:BoundField DataField="DisplayOrder" Visible="false" />
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
