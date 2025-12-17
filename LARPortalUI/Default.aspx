<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_Default" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
        #quicklinks a:hover {
            display: block;
            cursor: pointer;
            /*color: #B1A158;*/
            color: #333;
            text-decoration: none;
        }

        h1 {
            font-size: 36px;
        }

        .box {
            padding: 20px 15px;
            background: #fff;
            border: 1px solid #e7e7e7;
            -webkit-box-shadow: 0 8px 6px -6px #ccc;
            -moz-box-shadow: 0 8px 6px -6px #ccc;
            box-shadow: 0 8px 6px -6px #ccc;
        }

            .box h4 {
                font-size: 1.5em;
            }

            .box p {
                font-size: 1.125em;
            }

        @media (max-width: 414px) {
            h1 {
                font-size: 26px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid" id="quicklinks">
            <div class="row">
                <div class="col-md-3">&nbsp;</div>
                <div class="col-md-6 text-center margin10">
                    <h1>Welcome to LARP Portal</h1>
                    <p class="sub-text">LARP Portal is an application that brings LARP players and campaigns together.</p>
                    <span class="center-line"></span>
                </div>
                <div class="col-md-3 text-right" style="margin-top: 20px;">
<%--                    <asp:LinkButton ID="lbtnDashboard" runat="server" OnClick="lbtnDashboard_Click"
                        Text="Switch to Dashboard" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-10 col-lg-offset-1 col-md-12 col-md-offset-0 margin20">
                    <div class="row">
                        <div class="col-lg-4"></div>
                        <div class="col-lg-4">
                            <p>
                                <asp:Button ID="btnLogin" runat="server" Text="Switch to Player Dashboard" CssClass="btn btn-block btn-success" OnClick="lbtnDashboard_Click" />
                            </p>
                        </div>
                        <div class="col-lg-4"></div>
                    </div>
                    <div class="row equal-height-panels">
                        <div class="col-md-4 text-center margin30">
                            <a href="/Campaigns/JoinACampaign.aspx">
                                <div class="box">
                                    <asp:Image ID="imgSignUpForCamp" runat="server" ImageUrl="~/images/SignUpForACampaign.png" />
                                    <h4>Sign Up for a Campaign</h4>
                                    <p>Search for campaigns to PC, NPC or Staff.</p>
                                </div>
                            </a>
                        </div>
                        <div class="col-md-4 text-center margin30">
                            <a href="/Character/CharInfo.aspx">
                                <div class="box">
                                    <asp:Image ID="imgUpdateMyChar" runat="server" ImageUrl="~/images/UpdateMyCharacter.png" />
                                    <h4>Update My Characters</h4>
                                    <p>Add or maintain your character.</p>
                                </div>
                            </a>
                        </div>
                        <div class="col-md-4 text-center margin30">
                            <a href="/Events/EventRegistration.aspx">
                                <div class="box">
                                    <asp:Image ID="imgRegisterForEvent" runat="server" ImageUrl="~/images/RegisterForAnEvent.png" />
                                    <h4>Register for an Event</h4>
                                    <p>Register for an event as a PC, NPC or Staff.</p>
                                </div>
                            </a>
                        </div>
                    </div>
                    <div class="row equal-height-panels">
                        <div class="col-md-4 text-center margin30">
                            <a href="/Profile/Profile.aspx">
                                <div class="box">
                                    <asp:Image ID="imgCreateATeam" runat="server" ImageUrl="~/images/CreateATeam.png" />
                                    <h4>Update My Profile</h4>
                                    <p>Update the information about yourself.</p>
                                </div>
                            </a>
                        </div>
                        <div class="col-md-4 text-center margin30">
                            <a href="/PELs/PELList.aspx">
                                <div class="box">
                                    <asp:Image ID="imgWriteMyPEL" runat="server" ImageUrl="~/images/WriteMyPEL.png" />
                                    <h4>Write My PEL</h4>
                                    <p>Write, edit, save and submit your new post event summary letters.</p>
                                </div>
                            </a>
                        </div>
                        <div class="col-md-4 text-center margin30">
                            <a href="/Points/MemberPointsView.aspx">
                                <div class="box">
                                    <asp:Image ID="imgMemberPoints" runat="server" ImageUrl="~/images/ViewMyPoints.png" />
                                    <h4>View My Points</h4>
                                    <p>View your earned, spent and banked points at a campaign level.</p>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row" style="">
                <div class="col-lg-10 col-lg-offset-1 col-md-12 col-md-offset-0">
                    <%--                    <h2 class="page-header">What is LARP Portal</h2>
                    <p>LARP Portal is an application that brings LARP players and campaigns together. It gives players a single interface to find new campaigns or manage all of their existing games whether they play, NPC or staff.  LARP Portal gives owners a tool to securely manage players, events and character logistics with the benefit of cross campaign functionality for shared calendars, character point exchange and reporting and analytics tools.</p>
                    <h4>New to Larping?</h4>
                    <p>LARP Portal is the perfect tool for you. It will provide you everything you need to choose a campaign, get connected, and have the time of your life. LARP Portal centralizes games and calendars and focuses on improving the player&rsquo;s experience. Start by searching for a game near you. For more about LARP Portal, log in as guest or sign up for new account from the log in screen.</p>
                    <h4>Experienced Player?</h4>
                    <p>Let LARP Portal simplify larping.  Players can build a profile to share with campaigns, set communication preferences and manage their CP across all their characters from their player CP bank account. Never wait for your CP again with auto deposit! Whether you PC , NPC, or staff-play  one shots, limited arc games or have a long term established character LARP postal pull everything together for you in a consistent streamlined system. For more about LARP Portal, log in as guest or sign up for new account from the log in screen.</p>
                    <h4>Current Campaign Owner?</h4>
                    <p>Are logistics burdening you down?  Do you need better tools?  Let LARP Portal provide one or more modules to fill in what is missing. By participating in LARP Portal you improve your player&rsquo;s experience and free up your staff&rsquo;s time. Even if you have your own logistics solutions, signing up for the FREE Basics  Level will allow your players to manage their CP exchanges systematically. For more about LARP Portal, log in as guest or sign up for new account from the log in screen.</p>
                    <h4>New or Future Campaign Owner?</h4>
                    <p>LARP Portal will help you market your game, manage your player base, plan events and support character build.  This tool is ideal for the new game and is scalable for one shots, limited run or long term games.   You control who has access to administrative functions by assigning roles and all functionality is available to the user through interfaces.  There is no reliance on a data base administrator needed and base functionality is offered at no charge to the campaign to support cross campaign functionality like calendars and cp exchange.  For more about LARP Portal, log in as guest and visit &ldquo;Larping&rdquo; or contact us at <a href="mailto:owner@larportal.com">owner@larportal.com</a>.</p>--%>

                    <h2><b>New to LARP Portal?</b></h2>
                    <h3>Helpful pointers:</h3>
                    <ul>
                        <li>You only need one account no matter how many games you play. Your access will adjust based on whether you PC, NPC, or Staff.</li>
                        <li>The <b>Home Page</b> offers quick access to things like updating your character, registering for events, and submitting Post Event Letters/Surveys.</li>
                        <li>You can create multiple characters in a campaign and different versions of the same character.</li>
                        <li>Remember to hit SAVE when making changes.</li>
                        <li>View <b>Points</b> earned for various contributions under the POINTS section.</li>
                        <li>The <b>Calendar</b> can be filtered to view all games or just your games.</li>
                        <li>Switching the <b>Home Page</b> to <b>Dashboard</b> view will show you everything in one consolidated view.</li>
                    </ul>
                    <h3>New Player Checklist</h3>
                    <p>We’ve included links to our YouTube tutorial videos for guidance where available.</p>
                    <table class="table table-striped table-hover">
                        <tr>
                            <th style="background-color: #337ab7; color: white">Module/Screen</th>
                            <th style="background-color: #337ab7; color: white">Description</th>
                        </tr>
                        <tr>
                            <td>PLAYER Profile</td>
                            <td>
                                <p>
                                    <Videos:PlayVideo runat="server" ID="playVideo" VideoID="9" videodescription="Complete your Player Profile" />
                                </p>
                                Fill in essential details like your address, phone numbers, pronouns, etc. (Required: Name, email, zip code, and DOB).</td>
                        </tr>
                        <tr>
                            <td>PLAYER Preferences</td>
                            <td>Set up notification preferences to stay informed via email or text
                            </td>
                        </tr>
                        <tr>
                            <td>PLAYER Medical Info</td>
                            <td>
                                <p>
                                    <Videos:PlayVideo runat="server" ID="playVideo2" VideoID="11" videodescription="Add any medical info" />
                                </p>
                                Fill in details like medical conditions or allergies for event staff reference.
                            </td>
                        </tr>
                        <tr>
                            <td>CHARACTER</td>
                            <td>
                                <p>
                                    <Videos:PlayVideo runat="server" ID="playVideo3" VideoID="1" videodescription="Start your character" />
                                </p>
                                Add your character header and details to register for events as a PC.
                            </td>
                        </tr>
                        <tr>
                            <td>CAMPAIGN Events</td>
                            <td>
                                <p>
                                    <Videos:PlayVideo runat="server" ID="playVideo4" VideoID="6" videodescription="Register for an event" />
                                </p>
                                Pick your role (PC, NPC, or Staff) and provide the necessary information for event participation.
                            </td>
                        </tr>
                    </table>




                </div>

            </div>
        </div>
        <div class="divide50"></div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
