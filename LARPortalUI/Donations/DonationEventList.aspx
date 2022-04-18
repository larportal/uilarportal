<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="DonationEventList.aspx.cs" Inherits="LarpPortal.Donations.DonationEventList" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="MissingEventStyles" ContentPlaceHolderID="MainStyles" runat="Server">

    <style>
        .NoShadow {
            border: 0px solid #ccc !important;
            border-radius: 4px !important;
            -webkit-box-shadow: inset 0 0px 0px rgba(0, 0, 0, .075) !important;
            box-shadow: inset 0 0px 0px rgba(0, 0, 0, .075) !important;
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s !important;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s !important;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s !important;
        }
    </style>
</asp:Content>

<asp:Content ID="MissingEventScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>

<asp:Content ID="MissingEventBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>New Event Donations List</h1>
                </div>
            </div>
        </div>

        <div class="margin10"></div>

        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Event List</div>
                    <div class="panel-body">
                        <div class="container-fluid">
                            <div style="margin-right: 10px;" runat="server" id="divEventList">
                                <div class="row">
                                    <div class="TableLabel col-sm-5">
                                        <asp:Label ID="lblEventListLabel" runat="server" CssClass="form-control NoShadow">Event to Add New Donation List:</asp:Label>
                                    </div>
                                    <div class="col-sm-7 NoLeftPadding">
                                        <asp:DropDownList ID="ddlMissedEvents" runat="server" OnSelectedIndexChanged="ddlMissedEvents_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control autoWidth" />
                                    </div>
                                </div>
                            </div>

                            <div id="divNoEvents" runat="server">
                                <h1>There are no events without existing donations lists.</h1>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-xs-6">
                                <asp:Button ID="btnReturn" runat="server" Text="Return To Donation Setup List" CssClass="btn btn-primary" PostBackUrl="~/Donations/SetupDonations.aspx" />
                            </div>
                            <div class="col-xs-6 text-right">
                                <asp:Button ID="btnDonationsForEvent" runat="server" Text="Add Donations For Event" CssClass="btn btn-primary" OnClick="btnDonationsForEvent_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->

    <style type="text/css">
        .Padding5 {
            padding-top: 5px;
        }
    </style>


    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARP Portal Donations</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCloseMessage_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

