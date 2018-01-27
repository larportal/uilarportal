<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="WhatsNewDetail.aspx.cs" Inherits="LarpPortal.WhatsNewDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-12">
                    <div class="panel">
                        <div class="panel-header">
                            <h2>
                                <asp:Label ID="lblPanelHeader" runat="server"></asp:Label></h2>
                        </div>
                        <div class="panel-body">
                            <div class="panel-container">
                                <div class="row">
                                    <b>Module Name: </b>
                                    <asp:Label ID="lblModuleName" runat="server" />
                                </div>
                                <div class="row">
                                    <b>Release Name: </b>
                                    <asp:Label ID="lblReleaseName" runat="server" />
                                </div>
                                <div class="row">
                                    <b>Release Date: </b>
                                    <asp:Label ID="lblReleaseDate" runat="server" />
                                </div>
                                <div class="row">
                                    <b>Description: </b>
                                    <asp:Label ID="lblDescription" runat="server" />
                                </div>
                                <div class="row text-right">
                                    <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" CssClass="StandardButton" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
