<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="LarpPortal.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" Runat="Server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" Runat="Server">
  <div id="page-wrapper">
    <div class="container-fluid">
      <div class="row">
        <div class="col-md-12">
          <h1 class="page-header">Sign Up for LARP Portal</h1>
        </div>
      </div>
      <!-- Intro Content -->
      <div class="row">
        <div class="col-md-6 col-md-offset-3 col-sm-12 col-sm-offset-0">
          <form>
            <div class="form-group">
              <label for="userName">Username</label>
              <input type="text" class="form-control" id="userName" placeholder="Username">
            </div>
            <div class="form-group">
              <label for="firstName">First Name</label>
              <input type="text" class="form-control" id="firstName" placeholder="First Name">
            </div>
            <div class="form-group">
              <label for="lastName">Last Name</label>
              <input type="text" class="form-control" id="lastName" placeholder="Last Name">
            </div>
            <div class="form-group">
              <label for="email">Email Address</label>
              <input type="email" class="form-control" id="email" placeholder="Email Address">
            </div>
            <div class="form-group">
              <label for="password">Password</label>
              <input type="password" class="form-control" id="password" placeholder="Password">
            </div>
            <div class="form-group">
              <label for="repassword">Re-Password</label>
              <input type="password" class="form-control" id="repassword" placeholder="Password">
            </div>
            <div class="pull-left checkbox">
              <label>
                <input type="checkbox">
                Accept terms of use. </label>
            </div>
            <div class="pull-right">
              <button type="submit" class="btn btn-primary btn-lg">Register</button>
            </div>
            <div class="clearfix"></div>
          </form>
        </div>
      </div>
    </div>
    <div id="push"></div>
  </div>
  <!-- /#page-wrapper --> 
</asp:Content>
