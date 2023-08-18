<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="LarpPortal.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Google Tag Manager -->
    <script>(function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start':
                    new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
                    'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', 'GTM-5MQHDGS');</script>
    <!-- End Google Tag Manager -->

    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>LARP Portal - Log In</title>

    <!-- Bootstrap Core CSS -->
    <link href="/css/bootstrap.css" rel="stylesheet" />

    <!-- MetisMenu CSS -->
    <link href="/css/metisMenu.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="/css/sb-admin-2.css" rel="stylesheet" />

    <!-- Custom Fonts -->
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <link href="/css/larportal.css" rel="stylesheet" />
    <style type="text/css">
        body {
            background: #fff;
        }

        .hidden {
            display: none;
        }

        .visible {
            display: block;
        }

        .Required {
            color: red;
            font-style: italic;
        }
    </style>

    <script type="text/javascript">

        // Trying to make it so if you put in a question, the answer will say 'Required'. Same for if you put in the answer without the question.
        // If a question or answer is missing where other isn't will also disable the save button.

        function CheckSet1() {
            var tbQuestion1 = document.getElementById('<%=tbQuestion1.ClientID%>');
            var tbAnswer1 = document.getElementById('<%=tbAnswer1.ClientID%>');
            var lblQuestion1Req = document.getElementById('lblQuestion1Req');
            var lblAnswer1Req = document.getElementById('lblAnswer1Req');
            var btnSetQuestions = document.getElementById('<%= btnSetQuestions.ClientID %>');

            lblQuestion1Req.innerHTML = 'Question 1';
            lblAnswer1Req.innerHTML = 'Answer 1';
            btnSetQuestions.disabled = false;

            if ((tbQuestion1.value == '') &&
                (tbAnswer1.value != '')) {
                lblQuestion1Req.innerHTML = '<span class="Required">Question 1 * Required</span>';
                btnSetQuestions.disabled = true;
            }
            else if ((tbQuestion1.value != '') &&
                (tbAnswer1.value == '')) {
                lblAnswer1Req.innerHTML = '<span class="Required">Answer 1 * Required</span>';
                btnSetQuestions.disabled = true;
            }
        }

        function CheckSet2() {
            var tbQuestion2 = document.getElementById('<%=tbQuestion2.ClientID%>');
            var tbAnswer2 = document.getElementById('<%=tbAnswer2.ClientID%>');
            var lblQuestion2Req = document.getElementById('lblQuestion2Req');
            var lblAnswer2Req = document.getElementById('lblAnswer2Req');
            var btnSetQuestions = document.getElementById('<%= btnSetQuestions.ClientID %>');

            lblQuestion2Req.innerHTML = 'Question 2';
            lblAnswer2Req.innerHTML = 'Answer 2';
            btnSetQuestions.disabled = false;

            if ((tbQuestion2.value == '') &&
                (tbAnswer2.value != '')) {
                lblQuestion2Req.innerHTML = '<span class="Required">Question 2 * Required</span>';
                btnSetQuestions.disabled = true;
            }
            else if ((tbQuestion2.value != '') &&
                (tbAnswer2.value == '')) {
                lblAnswer2Req.innerHTML = '<span class="Required">Answer 2 * Required</span>';
                btnSetQuestions.disabled = true;
            }
        }

        function CheckSet3() {
            var tbQuestion3 = document.getElementById('<%=tbQuestion3.ClientID%>');
            var tbAnswer3 = document.getElementById('<%=tbAnswer3.ClientID%>');
            var lblQuestion3Req = document.getElementById('lblQuestion3Req');
            var lblAnswer3Req = document.getElementById('lblAnswer3Req');
            var btnSetQuestions = document.getElementById('<%= btnSetQuestions.ClientID %>');

            lblQuestion3Req.innerHTML = 'Question 3';
            lblAnswer3Req.innerHTML = 'Answer 3';
            btnSetQuestions.disabled = false;

            if ((tbQuestion3.value == '') &&
                (tbAnswer3.value != '')) {
                lblQuestion3Req.innerHTML = '<span class="Required">Question 3 * Required</span>';
                btnSetQuestions.disabled = true;
            }
            else if ((tbQuestion3.value != '') &&
                (tbAnswer3.value == '')) {
                lblAnswer3Req.innerHTML = '<span class="Required">Answer 3 * Required</span>';
                btnSetQuestions.disabled = true;
            }
        }
    </script>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
	<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
	<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
<![endif]-->

</head>

<body>
    <!-- Google Tag Manager (noscript) -->
    <noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-5MQHDGS"
    height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <!-- End Google Tag Manager (noscript) -->

    <form id="form1" runat="server">
        <div id="wrapper">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="header-background-image">
                            <h1>LARP Portal Forgot Username or Password?</h1>
                        </div>
                    </div>
                </div>
                <div class="divide10"></div>
                <asp:MultiView ID="mvInfoRequest" runat="server">
                    <asp:View ID="vwInfoRequest" runat="server">
                        <div class="row">
                            <div class="col-lg-6 col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Your Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label for="<%= txtEmailAddress.ClientID %>">EMail Address:</label>
                                                    <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ValidationGroup="GetPassword"
                                                        Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtEmailAddress" />
                                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="form-group">
                                                    <label for="<%= txtUsername.ClientID %>">Username:</label>
                                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ValidationGroup="GetPassword"
                                                        Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtUserName" />
                                                    <a href="/ForgotUserName.aspx" style="margin-left: 20px;">Forgot Username ?</a>
                                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="form-group">
                                                    <label for="<%= txtLastName.ClientID %>">Last Name:</label>
                                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ValidationGroup="GetPassword"
                                                        Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtLastName" />
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" ValidationGroup="GetPassword" />
                                                </div>
                                            </div>
                                            <div class="col-lg-12" id="divInvalid" runat="server">
                                                <h3 class="text-center text-danger">That username, email address, last name combination is not valid.  Try again.
                                                </h3>
                                            </div>
                                            <div class="col-lg-12 text-right">
                                                <asp:Button ID="btnGetPassword" runat="server" CssClass="btn btn-primary" Text="Get Password" OnClick="btnGetPassword_Click" CausesValidation="true" ValidationGroup="GetPassword" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Need assistance? Email support.
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label for="<%# txtSupportName.ClientID %>">Your name:</label>
                                                    <asp:RequiredFieldValidator ID="rfvSupportName" runat="server" ValidationGroup="GetSupport"
                                                        Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtSupportName" />
                                                    <asp:TextBox ID="txtSupportName" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="form-group">
                                                    <label for="<%# txtSupportEmail.ClientID %>">Your email address:</label>
                                                    <asp:RequiredFieldValidator ID="rfvSupportEmail" runat="server" ValidationGroup="GetSupport"
                                                        Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtSupportEmail" />
                                                    <asp:TextBox ID="txtSupportEmail" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="form-group">
                                                    <label for="<%# lblSupportSubject.ClientID %>">Subject:</label>
                                                    <asp:Label ID="lblSupportSubject" runat="server" CssClass="form-control" Text="Trouble with username / password." />
                                                </div>
                                                <div class="form-group">
                                                    <label for="<%# txtSupportBody.ClientID %>">Details about the issue:</label>
                                                    <asp:RequiredFieldValidator ID="rfvSupportBody" runat="server" ValidationGroup="GetSupport"
                                                        Font-Bold="true" Font-Italic="true" ForeColor="Red" Display="Dynamic" Text="* Required" ControlToValidate="txtSupportBody" />
                                                    <asp:TextBox ID="txtSupportBody" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" />
                                                </div>

                                                <div class="form-group">
                                                    <input type="checkbox" name="chkSupportCCMe2" id="chkSupportCCMe2" runat="server" />
                                                    <div class="btn-group">
                                                        <label for="<%= chkSupportCCMe2.ClientID%>" class="btn btn-default">
                                                            <span class="glyphicon glyphicon-ok"></span>
                                                            <span class="glyphicon glyphicon-unchecked"></span>
                                                        </label>
                                                        <label for="<%= chkSupportCCMe2.ClientID%>" class="btn btn-default active">
                                                            CC me on the support request email.
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-12 text-right">
                                                <asp:Label ID="lblSpamFilter" runat="server" text="*Type 'Done' when ready to send, then click the 'Send Email' button." ForeColor="Red"></asp:Label>
                                                <asp:TextBox ID="txtSpamFilter" runat="server" ></asp:TextBox>
                                                <asp:Button ID="btnSupportSendEmail" runat="server" OnClick="btnSupportSendEmail_Click" CssClass="btn btn-primary" Text="Send Email" ValidationGroup="GetSupport" />
                                                <asp:Label ID="lblSupportSentEmail" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwSecurityQuestions" runat="server">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">System Security Profile</div>
                                    <div class="panel-body">
                                        <asp:Label ID="lblSetQuestions" runat="server">
                                            It would appear you haven't set any security questions yet.  You have to have at least one.
                                            You can have up to three.  You decide how many and you decide the questions and the answers.
                                            If you forget your password you will have to answer all questions that you set so you decide how much security you want.<br /><br />
                                            Enter each security question or answer and then tab or enter to move on to the next field.
                                        </asp:Label><p></p>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label id="lblQuestion1Req" for="<%= tbQuestion1.ClientID %>">Question 1</label>
                                                    <asp:TextBox ID="tbQuestion1" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label id="lblAnswer1Req" for="<%= tbAnswer1.ClientID %>">Answer 1</label>
                                                    <asp:TextBox ID="tbAnswer1" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label id="lblQuestion2Req" for="<%= tbQuestion2.ClientID %>">Question 2</label>
                                                    <asp:TextBox ID="tbQuestion2" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label id="lblAnswer2Req" for="<%= tbAnswer2.ClientID %>">Answer 2</label>
                                                    <asp:TextBox ID="tbAnswer2" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label id="lblQuestion3Req" for="<%= tbQuestion3.ClientID %>">Question 3</label>
                                                    <asp:TextBox ID="tbQuestion3" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label id="lblAnswer3Req" for="<%= tbAnswer3.ClientID %>">Answer 3</label>
                                                    <asp:TextBox ID="tbAnswer3" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 text-right">
                                                <asp:Button ID="btnSetQuestions" runat="server" CssClass="btn btn-primary" Text="Reset Password" OnClick="btnSetQuestions_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwAnswerQuestions" runat="server">
                        <div class="row">
                            <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">Security Questions</div>
                                    <div class="panel-body">
                                        <asp:Label ID="Label1" runat="server">Please answer the question<asp:Label ID="lblAnswerQuestionS" runat="server" Text="s" />
                                            below to reset your password.
                                        </asp:Label>
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="form-group">
                                                    <asp:Label ID="lblUserQuestion1" runat="server" AssociatedControlID="tbUserAnswer1" />
                                                    <asp:TextBox ID="tbUserAnswer1" runat="server" CssClass="form-control" /><asp:HiddenField ID="hidUserAnswer1" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="divUserQuestion2">
                                            <div class="col-xs-12">
                                                <div class="form-group">
                                                    <asp:Label ID="lblUserQuestion2" runat="server" AssociatedControlID="tbUserAnswer2" />
                                                    <asp:TextBox ID="tbUserAnswer2" runat="server" CssClass="form-control" /><asp:HiddenField ID="hidUserAnswer2" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="divUserQuestion3">
                                            <div class="col-xs-12">
                                                <div class="form-group">
                                                    <asp:Label ID="lblUserQuestion3" runat="server" AssociatedControlID="tbUserAnswer3" />
                                                    <asp:TextBox ID="tbUserAnswer3" runat="server" CssClass="form-control" /><asp:HiddenField ID="hidUserAnswer3" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 text-right">
                                                <asp:Button ID="btnUserQuestions" runat="server" CssClass="btn btn-primary" Text="Reset Password" OnClick="btnUserQuestions_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>

                    <asp:View ID="vwSetPassword" runat="server">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">Reset Password</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <h3>Password requirements:</h3>
                                                LARP Portal login passwords must be at least 7 characters long and contain at least
                                                <br />
                                                1 uppercase letter, 1 lowercase letter, 1 number and 1 special character<br />
                                            </div>
                                        </div>
                                        <div class="row" style="padding-top: 20px;">
                                            <div class="col-md-4 col-md-offset-2 col-sm-12">
                                                <div class="form-group">
                                                    <label for="<%= tbPassword.ClientID %>">New Password:</label>
                                                    <div class="input-group col-md-12">
                                                        <asp:TextBox ID="tbPassword" runat="server" CssClass="form-control col-xs-12" TextMode="Password" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-12">
                                                <div class="form-group">
                                                    <label for="<%= tbPasswordConfirm %>">Confirm Password:</label><asp:CompareValidator ID="cvConfirmPassword" runat="server"
                                                        ControlToValidate="tbPasswordConfirm"
                                                        CssClass="ValidationError"
                                                        ControlToCompare="tbPassword"
                                                        ErrorMessage="Passswords Must Match"
                                                        ToolTip="Password must be the same" />

                                                    <div class="input-group col-md-12">
                                                        <asp:TextBox ID="tbPasswordConfirm" runat="server" CssClass="form-control" TextMode="Password" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="divErrorPasswords" runat="server" visible="false">
                                            <div class="col-xs-12">
                                                <asp:Label ID="lblErrorPasswords" runat="server" CssClass="alert alert-danger text-center" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <p class="text-right">
                                                    <asp:Button ID="btnSaveNewPassword" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveNewPassword_Click" />
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwFinalStep" runat="server">
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-lg-12 text-center">
                                Your password has been reset.  Click the ok button to return to the login screen.

                            </div>
                        </div>
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-lg-12 text-center">
                                <asp:Button ID="btnDone" runat="server" OnClick="btnDone_Click" Text="OK" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>

        <%--                                    <asp:Label ID="lblForgotUsername" runat="server">Forgot your username or new to the system?  Fill in the email address and click 'Forgot Username'</asp:Label>
                                    <asp:Button ID="btnForgotUsername" runat="server" OnClick="btnForgotUsername_Click" CssClass="btn btn-primary" Text="Forgot Username" /><br />--%>
        <%--                        <asp:Label ID="lblUsernameISEmail" runat="server"></asp:Label>
                        <asp:Button ID="btnGetPassword" runat="server" OnClick="btnGetPassword_Click" CssClass="btn btn-primary" Text="Get Password" />
                        <asp:Label ID="lblInvalidCombination" runat="server" ForeColor="Red">That username, email address, last name combination is not valid.  Click OK and try again.</asp:Label>
                        <p></p>
                        <asp:Button ID="btnInvalidCombination" runat="server" OnClick="btnInvalidCombination_Click" CssClass="btn btn-primary" Text="OK" />
                        <p></p>
                        <br />
                        <br />
                        <br />
                        <br />
                        <b>Need assistance? Email support.</b><p></p>
                        Your name:
                                    <asp:TextBox ID="txtSupportName" runat="server" AutoPostBack="true" OnTextChanged="txtSupportName_TextChanged"></asp:TextBox><p></p>
                        Your email address:
                                    <asp:TextBox ID="txtSupportEmail" runat="server" AutoPostBack="true" OnTextChanged="txtSupportEmail_TextChanged"></asp:TextBox><p></p>
                        Subject: Trouble with username / password<p></p>
                        Details about the issue:<p></p>
                        <p></p>
                        <p></p>
                        <p></p>
                        <asp:TextBox ID="txtSupportBody" runat="server" TextMode="MultiLine" Columns="50" Rows="5" AutoPostBack="true" OnTextChanged="txtSupportBody_TextChanged"></asp:TextBox><br />
                        <asp:CheckBox ID="chkSupportCCMe" runat="server" Checked="true" AutoPostBack="true" />
                        CC me on the support request email.<p></p>
                        <asp:Button ID="btnSupportSendEmail" runat="server" OnClick="btnSupportSendEmail_Click" CssClass="btn btn-primary" Text="Send Email" />
                        <asp:Label ID="lblSupportSentEmail" runat="server"></asp:Label>
                        </asp:Panel>
                                <asp:Panel ID="pnlVariables" runat="server">
                                    <asp:Label ID="Q1" runat="server"></asp:Label>
                                    <asp:Label ID="Q1Update" runat="server">0</asp:Label>
                                    <asp:Label ID="Q2" runat="server"></asp:Label>
                                    <asp:Label ID="Q2Update" runat="server">0</asp:Label>
                                    <asp:Label ID="Q3" runat="server"></asp:Label>
                                    <asp:Label ID="Q3Update" runat="server">0</asp:Label>
                                    <asp:Label ID="A1" runat="server"></asp:Label>
                                    <asp:Label ID="A1Update" runat="server">0</asp:Label>
                                    <asp:Label ID="A2" runat="server"></asp:Label>
                                    <asp:Label ID="A2Update" runat="server">0</asp:Label>
                                    <asp:Label ID="A3" runat="server"></asp:Label>
                                    <asp:Label ID="A3Update" runat="server">0</asp:Label>
                                    <asp:Label ID="UserSecurityID" runat="server"></asp:Label>
                                </asp:Panel>
                        <asp:Panel ID="pnlSetPasswords" runat="server">
                            <asp:Label ID="lblPasswordRequirements" runat="server">
                                            Password requirements:<br />
                                            LARP Portal login passwords must be at least 7 characters long and contain at least <br />
                                            1 uppercase letter, 1 lowercase letter, 1 number and 1 special character<br />
                            </asp:Label><p></p>
                            <asp:Label ID="lblNewPassword" runat="server">New Password: </asp:Label>
                            <asp:TextBox ID="txtNewPassword" runat="server" AutoPostBack="true" OnTextChanged="txtNewPassword_TextChanged"></asp:TextBox><p></p>
                            <asp:Label ID="lblNewPasswordRetype" runat="server">Retype New Password: </asp:Label>
                            <asp:TextBox ID="txtNewPasswordRetype" runat="server" AutoPostBack="true" OnTextChanged="txtNewPasswordRetype_TextChanged"></asp:TextBox><p></p>
                            <asp:Label ID="lblPasswordErrors" runat="server" ForeColor="Red"></asp:Label><p></p>
                            <asp:Button ID="btnSubmitPasswordChange" runat="server" OnClick="btnSubmitPasswordChange_Click" Text="Submit" />
                        </asp:Panel>
                        <asp:Panel ID="pnlFinalStep" runat="server">
                            <asp:Label ID="lblDone" runat="server">
                                            Your password has been reset.  Click the ok button to return to the login screen.
                            </asp:Label>
                            <asp:Button ID="btnDone" runat="server" OnClick="btnDone_Click" Text="OK" />
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        </div>
        </section>
        </div>
        </div>--%>

        <%-- When we go to update these values, they will always be in pairs so only need one 'update' variable. --%>
        <asp:HiddenField ID="hidQuestion1" runat="server" />
        <asp:HiddenField ID="hidAnswer1" runat="server" />
        <asp:HiddenField ID="hidUpdate1" runat="server" />
        <asp:HiddenField ID="hidQuestion2" runat="server" />
        <asp:HiddenField ID="hidAnswer2" runat="server" />
        <asp:HiddenField ID="hidUpdate2" runat="server" />
        <asp:HiddenField ID="hidQuestion3" runat="server" />
        <asp:HiddenField ID="hidAnswer3" runat="server" />
        <asp:HiddenField ID="hidUpdate3" runat="server" />
        <asp:HiddenField ID="hidUserSecurityID" runat="server" />
    </form>
</body>
</html>
