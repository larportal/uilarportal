<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="LarpPortal.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
        .space20 {
            height: 20px;
        }

        .staff h4 {
            text-transform: uppercase;
            margin-bottom: 3px;
            letter-spacing: .03em;
        }

        .staff span {
            display: block;
            color: #337ab7;
            font-style: italic;
        }

        .staff div {
            display: flex;
            vertical-align: top;
        }

        .staff p {
            color: #777;
            font-size: .95em;
            line-height: 1.5em;
        }

        .sidebar-widget {
            margin-bottom: 30px;
        }

            .sidebar-widget h3 {
                text-transform: uppercase;
                font-size: 16px;
                font-weight: 700;
                margin-bottom: 20px;
            }

            .sidebar-widget .list-unstyled {
                padding: 0;
                margin: 0;
            }

                .sidebar-widget .list-unstyled li {
                    padding-bottom: 7px;
                    vertical-align: middle;
                }

                    .sidebar-widget .list-unstyled li a {
                        font-size: 15px;
                        display: block;
                    }

                        .sidebar-widget .list-unstyled li a:before {
                            font-family: "FontAwesome";
                            content: "\f101";
                            margin-right: 10px;
                        }

        .side-event {
            padding-bottom: 10px;
        }

            .side-event .s-event-date {
                float: left;
                margin-right: 15px;
                text-align: center;
                background-color: #eee;
                padding-bottom: 3px;
            }

                .side-event .s-event-date span {
                    display: block;
                    padding: 3px 5px;
                    color: #fff;
                    background-color: #002949;
                    margin-bottom: 3px;
                }

            .side-event .s-event-content {
                overflow: hidden;
            }

                .side-event .s-event-content h5 {
                    margin: 0px;
                }

        @media (max-width: 767px) {
            #page-wrapper {
                padding: 0 0 30px 0;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Our Team</h1>
                </div>
            </div>
        </div>

        <!-- Team Members -->
        <div class="row">
            <div class="col-lg-1"></div>
            <div class="col-lg-10">
                <div class="row">
                    <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 margin30 staff">
                        <img src="images/Rick.jpg" class="img-thumbnail" style="max-width: 250px; max-height: 250px;" alt="" />
                        <h4>Rick Pierce</h4>
                        <span>System Architect and Lead Developer</span>
                        <p>Rick has applied his 30+ years’ experience both as a larper and as a programmer to develop LARP Portal, a logistics solution. He has played and staffed many campaigns in the New England area and knows firsthand how time consuming LARP logistics can be. Rick is also a musician and published composer in his spare time.</p>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 margin30 staff">
                        <img src="/images/JeffTie2.jpg" class="img-thumbnail" style="max-width: 250px; max-height: 250px;" alt="" />
                        <h4>Jeff Bradshaw</h4>
                        <span>Senior Developer and Technical Advisor</span>
                        <p>Jeff is a professional .Net/SQL developer with 25+ years&rsquo; experience. He has never larped but has learned the lingo rather quickly by supporting the entire development team. Jeff is also a professional chef and enjoys sci-fi/fantasy.</p>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 margin30 staff">
                        <img src="images/Monique.jpg" class="img-thumbnail" style="max-width: 250px; max-height: 250px;" alt="" />
                        <h4>Monique Pierce</h4>
                        <span>Product Manager</span>
                        <p>Monique has over 20 years&rsquo; experience managing process improvement and system development projects working for corporate America and as an entrepreneur. She has some logistics staffing experience though she is not a larper. Monique brings her system design and business skills to the team.</p>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 margin20 staff">
                        <h4>Contributors:</h4>
                        <p>We want to thank JJ McGill and Nicole Gouthro for their ongoing support, advice and testing and Lise Zanghetti for business management.</p>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 margin20 staff">
                        <h4>Initial Design and Development:</h4>
                        <p>Deep appreciation goes out to Paul Chanoine, Jack Voss, Matthew Botteon for countless hours in our early discussions about infrastructure, design, development and user support.</p>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 margin20 staff">
                        <h4>Advisory Board:</h4>
                        <p>Amanda Grow, Brent Desmarais, Erin Desmarais, Lisa Sants, Hillary Chapin, John Mangio, Michelle Mangio, Melissa Caruso, Mikka Goldberg, James Marston, Ann Brant, Sue Brassard, RJ Scott McKenzie, Melissa Caruso, Rob Ciccolini, and Trey Riley.</p>

                    </div>
                </div>
            </div>
        </div>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
