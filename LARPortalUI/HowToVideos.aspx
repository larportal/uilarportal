<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="HowToVideos.aspx.cs" Inherits="LarpPortal.HowToVideos" %>

<asp:Content ID="WhatIsLARPingStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="WhatIsLARPingScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="WhatIsLARPingBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>How To Videos</h1>
                    </div>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="row">
                    <h2>Character</h2>
                    <asp:GridView ID="gvChar" runat="server" AutoGenerateColumns="false" GridLines="None"
                        CssClass="table table-striped table-hover" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                        <Columns>
                            <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="col-xs-12">
                <div class="row">
                    <h2>Events</h2>
                    <asp:GridView ID="gvEvent" runat="server" AutoGenerateColumns="false" GridLines="None"
                        CssClass="table table-striped table-hover" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                        <Columns>
                            <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="col-xs-12">
                <div class="row">
                    <h2>Player</h2>
                    <asp:GridView ID="gvPlayer" runat="server" AutoGenerateColumns="false" GridLines="None"
                        CssClass="table table-striped table-hover" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                        <Columns>
                            <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="col-xs-12">
                <div class="row">
                    <h2>Points             <Videos:PlayVideo runat="server" ID="hlVideos" VideoID="1" />
</h2>
                    <asp:GridView ID="gvPoints" runat="server" AutoGenerateColumns="false" GridLines="None"
                        CssClass="table table-striped table-hover" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                        <Columns>
                            <asp:HyperLinkField DataTextField="Name" DataNavigateUrlFields="YouTubeLink" HeaderText="Video Link" Target="_blank" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>




        </div>
        <div>
        </div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
