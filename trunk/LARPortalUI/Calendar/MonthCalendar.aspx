<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="MonthCalendar.aspx.cs" Inherits="LarpPortal.Calendar.MonthCalendar" Trace="false" MaintainScrollPositionOnPostback="true" %>

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
    <link href="../Content/fullcalendar.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="CalGraphicScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function ScrollToDate() {
            location.hash = '#ScrollToDate';
        }
    </script>
    <%--    <script src="../Scripts/metisMenu.min.js"></script>
    <script src="../Scripts/moment.js"></script>
    <script src="/Scripts/jquery-3.3.1.min.js"></script>--%>
    <script src="../Scripts/moment.js"></script>
    <script src="../Scripts/fullcalendar/fullcalendar.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('#<%= calendar.ClientID %>').fullCalendar({
                height: 500,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                },
                textEscape: true,
                defaultDate: '<%= DateTime.Today.ToShortDateString() %>',
                navLinks: true, // can click day/week names to navigate views
                editable: true,
                eventLimit: false, // allow "more" link when too many events
                events: [
                   <%= eventstring %>
                ],
                eventRender: function (event, element) {
                    $(element).attr("data-original-title", event.tooltip)
                    $(element).attr("data-html", "true")
                    $(element).tooltip({ container: "body" })
                }
            });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
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
                            <div class="col-sm-12 col-xs-12">
                                <div id='calendar' runat="server"></div>
                                <h1 id="h2NoEvents" runat="server" class="text-center"><b>This campaign has no events.</b></h1>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .fc-day-grid-event > .fc-content {
            white-space: normal;
        }
    </style>



</asp:Content>
