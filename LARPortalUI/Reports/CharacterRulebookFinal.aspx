<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="CharacterRulebookFinal.aspx.cs" Inherits="LarpPortal.Reports.CharacterRulebookFinal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        table td {
            word-wrap: break-word;
        }
        .TableLabel {
            font-weight: bold;
            text-align: right;
            padding-left: 10px;
            padding-right: 5px;
        }


        .LeftRightPadding {
            padding-left: 10px;
            padding-right: 10px;
        }

        .PrintButton {
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

            .PrintButton:hover {
                background-color: lightblue;
            }

        .HeaderLabel {
            font-size: 24px;
            font-weight: bold;
        }

        @media print {
            Label {
                font-size: 10pt;
            }

            body {
                font-size: 10pt;
            }

            .hiddenOnPrint {
                display: none;
            }

            .TableLabel {
                font-weight: bold;
                text-align: right;
                padding-left: 10px;
                padding-right: 5px;
                font-size: 10pt;
            }
        }

        Label {
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <asp:Table runat="server" ID="tblCharacterRulebook">
                <asp:TableRow runat="server" VerticalAlign="top">
                    <asp:TableCell runat="server" >
                        <asp:Label ID="lblCharName" runat="server" CssClass="HeaderLabel" />
                    </asp:TableCell>
                    <asp:TableCell runat="server" HorizontalAlign="Right"  CssClass="hiddenOnPrint" RowSpan="4">
                        <asp:Button ID="printButton" runat="server" CssClass="PrintButton" Text="Print" OnClientClick="javascript:window.print();" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:GridView ID="gvCharacterRulebook" runat="server" AutoGenerateColumns="false"  RowStyle-VerticalAlign="top" HeaderStyle-BackColor="LightGray" AlternatingRowStyle-BackColor="Linen">
                <Columns>
                    <asp:BoundField DataField="CharacterRulebook" HeaderText="Character Rulebook"  HeaderStyle-CssClass="LeftRightPadding" ItemStyle-CssClass="LeftRightPadding" ItemStyle-Wrap="true" HtmlEncode="false"/>
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
