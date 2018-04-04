<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="TermsOfUse.aspx.cs" Inherits="LarpPortal.TermsOfUse" %>

<asp:Content ID="TermsOfUseStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="TermsOfUseScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="TermsOfUseBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Terms of Use for Website</h1>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblTermsOfUse" runat="server"></asp:Label>
        </div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

</asp:Content>
