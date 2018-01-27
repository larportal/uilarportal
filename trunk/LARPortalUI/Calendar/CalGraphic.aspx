<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="CalGraphic.aspx.cs" Inherits="LarpPortal.Calendar.CalGraphic" Trace="false" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CalGraphicStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .EventHeader {
            color: blue;
        }

        .cellHeader {
            border-left: 1px solid black;
            border-right: 1px solid black;
            border-top: 1px solid black;
            height: 20px;
            width: 12%;
            font-size: larger;
        }

        .cellContents {
            border-left: 1px solid black;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            height: 80px;
            width: 12%;
            text-align: left;
            vertical-align: top;
            padding-left: 2px;
            padding-right: 2px;
        }

        .cellColumnHeader {
            border-left: 1px solid black;
            border-right: 1px solid black;
            border-top: 1px solid black;
            height: 20px;
            width: 12%;
            font-size: larger;
            text-align: center;
            padding-left: 2px;
            padding-right: 2px;
        }

        .CalendarMonth {
            border-left: 1px solid black;
            border-right: 1px solid black;
            border-top: 1px solid black;
            height: 20px;
            /*width: 12%;*/
            font-size: larger;
            text-align: center;
            background-color: lightgray;
        }

        th, tr:nth-child(even) > td {
            background-color: #ffffff;
        }

        .Calendar {
            border: 1px solid black;
            width: 99%;
        }

        .tooltip-inner {
            -webkit-border-radius: 10px;
            -moz-border-radius: 10px;
            border-radius: 10px;
            margin-bottom: 0px;
            background-color: white;
            color: black;
            font-size: 14px;
            box-shadow: 5px 5px 10px #888888;
            width: auto;
            min-width: 200px;
            opacity: 1;
            filter: alpha(opacity=100);
            white-space: nowrap;
        }

        .eventHighlight {
            background-color: blue;
            color: white;
        }

        div {
            border: 0px solid black;
        }
    </style>
</asp:Content>

<asp:Content ID="CalGraphicScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function ScrollToDate() {
            location.hash = '#ScrollToDate';
        }
    </script>
</asp:Content>
<asp:Content ID="CalGraphicBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Events Calendar</h1>
                </div>
            </div>
        </div>
        <div class="divide10"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Campaign Calendar<asp:Label ID="lblCampaignList" runat="server" /></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:Table ID="tabCalendar" runat="server" CssClass="Calendar" />
                                    <asp:Label ID="lblCalendar" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
