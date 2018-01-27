<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="FifthGateCharacterSkills.aspx.cs" Inherits="LarpPortal.Reports.FifthGate.FifthGateCharacterSkills" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Work Fifth Gate Character Skills</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Fifth Gate Character Skills</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container">
                                    <div style="max-height: 350px; overflow: auto;">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:Label ID="lblCharacterSkills" runat="server">Build the table here programatically.  If you see this code the build failed.</asp:Label>
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
