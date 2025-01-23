<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HowToVideosExt.aspx.cs" Inherits="LarpPortal.HowToVideosExt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LARPortal How To Videos</title>
    <style>
        th {
            text-align: left;
            background-color: #eeeeee;
        }
        body {
            font-family: Arial, Helvetica, sans-serif;
            line-height: 1.42;
        }
        table {
            font-size: .8em;
        }
        td, th {
            padding: 5pt;
        }
        .gvHover:hover
        {
            background-color: #dddddd;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>LARPortal How To Videos</h1>

            <h2>Character</h2>
            <asp:GridView ID="gvChar" runat="server" AutoGenerateColumns="false" GridLines="Both" RowStyle-CssClass="gvHover"
                BorderColor="#dddddd" BorderStyle="Solid" BorderWidth="1">
                <Columns>
                    <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                </Columns>
            </asp:GridView>

            <h2>Events</h2>
            <asp:GridView ID="gvEvent" runat="server" AutoGenerateColumns="false" GridLines="Both" RowStyle-CssClass="gvHover"
                BorderColor="#dddddd" BorderStyle="Solid" BorderWidth="1">
                <Columns>
                    <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                </Columns>
            </asp:GridView>

            <h2>Player</h2>
            <asp:GridView ID="gvPlayer" runat="server" AutoGenerateColumns="false" GridLines="Both" RowStyle-CssClass="gvHover"
                BorderColor="#dddddd" BorderStyle="Solid" BorderWidth="1">
                <Columns>
                    <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                </Columns>
            </asp:GridView>

            <h2>Points</h2>
            <asp:GridView ID="gvPoints" runat="server" AutoGenerateColumns="false" GridLines="Both" RowStyle-CssClass="gvHover"
                BorderColor="#dddddd" BorderStyle="Solid" BorderWidth="1">
                <Columns>
                    <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
