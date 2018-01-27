<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="WhatIsLARPing.aspx.cs" Inherits="LarpPortal.WhatIsLARPing" %>

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
                        <h1>What is LARPing</h1>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblWhatIsLARPing" runat="server"></asp:Label>
        </div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
