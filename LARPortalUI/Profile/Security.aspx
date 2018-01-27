<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Security.aspx.cs" Inherits="LarpPortal.Profile.Security" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="SecurityStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="SecurityScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="SeccurityBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Security</h1>
                </div>
                <div class="row">
                    <asp:Label ID="lblUsernameLabel" runat="server" CssClass="control-label col-sm-1">Username</asp:Label>
                    <div class="user-name col-sm-3">
                        <asp:TextBox ID="txtUsername" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label ID="lblFirstNameLabel" runat="server" CssClass="control-label col-sm-1">First</asp:Label>
                    <div class="first-name col-sm-2">
                        <label>First Name</label>
                        <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblMILabel" runat="server" CssClass="control-label col-sm-1">MI</asp:Label>
                    <div class="middle-initial col-sm-1">
                        <asp:TextBox ID="txtMI" runat="server" ReadOnly="true" />
                    </div>
                    <asp:Label ID="lblLastNameLabel" runat="server" CssClass="control-label col-sm-1">Last</asp:Label>
                    <div class="last-name col-sm-2">
                        <asp:TextBox ID="txtLastName" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <asp:Label ID="lblNickNameLabel" runat="server" CssClass="control-label col-sm-2">Nick Name</asp:Label>
                    <div class="nick-name col-sm-2">
                        <asp:TextBox ID="txtNickName" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="formGroup row">
                    <asp:Label ID="lblError" runat="server" Font-Bold="true" CssClass="control-label col-sm-4" />
                </div>
                <div class="formGroup row">
                    <asp:Label ID="lblPasswordReqs" runat="server" ToolTip=""></asp:Label>
                    <asp:Label ID="lblPassword" runat="server" CssClass="control-label col-sm-1">Password</asp:Label>
                    <div class="pass-word col-sm-3">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="formGroup row">
                    <asp:Label ID="lblPassword2" runat="server" CssClass="control-label col-sm-1">Password</asp:Label>
                    <div class="pass-word col-sm-3">
                        <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="security-questions">
                    <h2>Security Questions:</h2>
                    <div class="row">
                        <p class="col-sm-7">Please enter 1 or more security questions for use in future email and password validation</p>
                    </div>
                    <div class="formGroup row">
                        <asp:Label ID="lblErrorQuestions" runat="server" Font-Bold="true" CssClass="control-label col-sm-4" />
                    </div>
                    <div class="form-group row">
                        <label class="control-label col-sm-1">Question 1</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSecurityQuestion1" runat="server" CssClass="form-control" />
                        </div>
                        <label class="control-label col-sm-1">Answer 1</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSecurityAnswer1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="formGroup row">
                        <asp:Label ID="lblErrorQuestion2" runat="server" Font-Bold="true" CssClass="control-label col-sm-4" />
                    </div>
                    <div class="form-group row">
                        <label for="security-question-2" class="control-label col-sm-1">Q 2</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSecurityQuestion2" runat="server" CssClass="form-control" />
                        </div>
                        <label for="security-answer-1" class="control-label col-sm-1">A 2</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSecurityAnswer2" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="formGroup row">
                        <asp:Label ID="lblErrorQuestion3" runat="server" Font-Bold="true" CssClass="control-label col-sm-4" />
                    </div>
                    <div class="form-group row">
                        <label for="security-question-3" class="control-label col-sm-1">Q 3</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSecurityQuestion3" runat="server" CssClass="form-control" />
                        </div>
                        <label for="security-answer-1" class="control-label col-sm-1">A 3</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSecurityAnswer3" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2 col-sm-offset-10 text-right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <div id="push"></div>
    </div>









</asp:Content>

