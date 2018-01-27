<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="FAQs.aspx.cs" Inherits="LarpPortal.FAQs" %>

<asp:Content ID="FAQsStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        @media (max-width: 767px) {
            #page-wrapper {
                padding: 0 0 30px 0;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="FAQsScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="FAQsBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>LARPortal FAQs</h1>
                </div>
            </div>
        </div>
        <!-- Intro Content -->
        <asp:Repeater ID="rptrFAQ" runat="server" OnItemDataBound="rptrFAQ_ItemDataBound">
            <HeaderTemplate>
                <div class="panel-group" id="MainAccordian">
            </HeaderTemplate>

            <ItemTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title"><a data-toggle="collapse" data-parent="#MainAccordian" href="#collapse<%# DataBinder.Eval(Container.DataItem,"FAQCategoryID") %>"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %></a></h4>
                    </div>
                    <div id="collapse<%# DataBinder.Eval(Container.DataItem,"FAQCategoryID") %>" class="panel-collapse collapse">
                        <div class="panel-body">
                            <div class="panel-group" id="accordion<%# DataBinder.Eval(Container.DataItem,"FAQCategoryID") %>">
                                <asp:Label ID="lblMorePanels" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>

            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>

    <div class="divide30"></div>
    <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
