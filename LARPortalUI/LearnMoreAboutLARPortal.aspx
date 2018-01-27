<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="LearnMoreAboutLARPortal.aspx.cs" Inherits="LarpPortal.LearnMoreAboutLARPortal" %>

<asp:Content ID="LearnMoreAboutLARPortalStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="LearnMoreAboutLARPortalScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="LearnMoreAboutLARPortalBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>What is LARP Portal?</h1>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblWhatIsLARPortal" runat="server"></asp:Label>
        </div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

</asp:Content>
