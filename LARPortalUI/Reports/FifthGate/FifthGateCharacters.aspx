<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="FifthGateCharacters.aspx.cs" Inherits="LarpPortal.Reports.FifthGate.FifthGateCharacters" %>

<asp:Content ID="FifthGateCharactersStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="FifthGateCharactersScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="FifthGateCharactersBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Fifth Gate Characters</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Fifth Gate Characters</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:Button ID="Button1" runat="server" Text=" Export to Excel " CssClass="btn btn-primary" OnClick="btnExportExcel_Click" />
                                            <asp:Button ID="Button2" runat="server" Text=" Export to csv " CssClass="btn btn-primary" OnClick="btnExportcsv_Click" />
                                        </div>
                                    </div>
                                    <div class="margin10"></div>
                                    <div style="max-height: 350px; overflow: auto;">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:Label ID="lblCharacters" runat="server">Build the table here programatically.  If you see this code the build failed.</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
