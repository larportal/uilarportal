<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="WhatsNew.aspx.cs" Inherits="LarpPortal.WhatsNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>What's New at LARP Portal</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div style="overflow-x: scroll;">
                        <asp:GridView ID="gvWhatsNewList" runat="server" AutoGenerateColumns="false" GridLines="None"
                            HeaderStyle-Wrap="false" CssClass="table table-responsive table-striped">
                            <Columns>
                                <asp:BoundField DataField="ReleaseDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="ModuleName" HeaderText="Module" />
                                <asp:BoundField DataField="BriefName" HeaderText="Name" />
                                <asp:BoundField DataField="ShortDescription" HeaderText="Description" />
                                <asp:HyperLinkField Text="Show Details" ControlStyle-CssClass="btn btn-primary" DataNavigateUrlFields="WhatsNewID"
                                        DataNavigateUrlFormatString="WhatsNewDetail.aspx?WhatsNewID={0}" Target="_blank" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>





























</asp:Content>
