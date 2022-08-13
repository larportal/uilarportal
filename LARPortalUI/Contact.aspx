<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="LarpPortal.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
        @media (max-width: 767px) {
            #page-wrapper {
                padding: 0 0 30px 0;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="Server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Contact Us</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <h3><i class="fa fa-comment" aria-hidden="true"></i>Requests</h3>
                <p class="sub-text">If you would like to make a request, please email us and our business advisors will contact you shortly.</p>
                <p><strong>Request by Player:</strong> <a href="mailto:playerservices@larportal.com?subject=LARP Portal Request by Player">playerservices@larportal.com</a></p>
                <p><strong>Request by Campaign Owner:</strong> <a href="mailto:ownerservices@larportal.com?subject=LARP Portal Request by Campaign Owner">ownerservices@larportal.com</a></p>
                <p><strong>Request General Information:</strong> <a href="mailto:owner@larportal.com?subject=LARP Portal General Information Request">owner@larportal.com</a></p>
                <hr />
                <h3><i class="fa fa-support" aria-hidden="true"></i>Help Desk</h3>
                <p class="sub-text">If you would like to report a technical issue, please email us and our support team will get back to you shortly.</p>
                <p><strong>Report a Technical Issue:</strong> <a href="mailto:support@larportal.com ?subject=LARP Portal Report a Technical Issue">support@larportal.com</a></p>
                <hr />
                <h3><i class="fa fa-comments" aria-hidden="true"></i>Feedback</h3>
                <p class="sub-text">We love getting feedback from our users, please send us your suggestions.</p>
                <p><strong>Provide Feedback/Suggestions:</strong> <a href="mailto:feedback@larportal.com?subject=LARP Portal Feedback">feedback@larportal.com</a></p>
                <hr />
                <h3><i class="fa fa-toggle-off" aria-hidden="true"></i>Account Management</h3>
                <p class="sub-text">Please contact our sales team with any questions you have regarding your account.</p>
                <p><strong>Add Campaign:</strong> <a href="mailto:sales@larportal.com?subject=LARP Portal Add Campaign">sales@larportal.com</a></p>
                <p><strong>Upgrade Participation Level:</strong> <a href="mailto:sales@larportal.com?subject=LARP Portal Upgrade Participation Level">sales@larportal.com</a></p>
                <hr />
                <h3><i class="fa fa-question-circle" aria-hidden="true"></i>Billing Questions</h3>
                <p class="sub-text">Please contact our finance department with any questions you have in regards to billing.</p>
                <p><strong>Accounts Payable:</strong> <a href="mailto:finance@larportal.com?subject=LARP Portal Billing Question">finance@larportal.com</a></p>
                <hr />
            </div>
            <div class="col-md-4 col-md-offset-1">
                <h3 class="contact-form-heading">Where we're located</h3>
                <ul class="list-unstyled contact contact-info">
                    <li>
                        <p><strong>Address:</strong> Londonderry NH 03053</p>
                    </li>
                    <li>
                        <p><strong>Email Us:</strong> <a href="mailto:info@larportal.com">info@larportal.com</a></p>
                    </li>
                    <li>
                        <p><strong>Phone:</strong> TBD</p>
                    </li>
                </ul>
                <div class="divide10"></div>
                <h4>Get Social</h4>
                <ul class="list-inline social-1" style="text-align: left;">
                    <li><a href="https://www.facebook.com/larportal" target="_blank"><i class="fa fa-facebook-square fa-2x"></i></a></li>
                    <!--<li><a href="#" target="_blank"><i class="fa fa-twitter-square fa-2x"></i></a></li>-->
                </ul>
            </div>
        </div>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
