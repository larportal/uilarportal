<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="LarpPortal.Profile.Roles" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PlayerRolesStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="PlayerRolesScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="PlayerRolesBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Player Roles for <asp:Label ID="lblCampName" runat="server" /></h1>
                </div>
            </div>
        </div>

<%--        <div class="row">
            <div class="form-inline col-xs-12">
                <label for="<%= ddlEventDate.ClientID %>" style="padding-left: 25px;">Events:</label>
                <asp:DropDownList ID="ddlEventDate" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEventDate_SelectedIndexChanged" AutoPostBack="true" />
            </div>
        </div>--%>
        <div class="margin10"></div>

        <asp:Repeater ID="rptRoles" runat="server" OnItemDataBound="rptRoles_ItemDataBound">
            <ItemTemplate>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%# Eval("DisplayGroup") %></div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <ul class="col-lg-12">
                                            <asp:Label ID="lblDesc" runat="server" />
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>



        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

