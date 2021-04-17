<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="MissingEvent.aspx.cs" Inherits="LarpPortal.PELs.MissingEvent" %>

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
                    <h1>Post Event Registration</h1>
                </div>
            </div>
        </div>

        <%--        <div class="row">
            <div class="col-xs-12">
                <div class="form-inline">
                  //  <CampSelector:Select ID="oCampSelect" runat="server" />
                </div>
            </div>
        </div>--%>

        <div class="margin10"></div>

        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Event List</div>
                    <div class="panel-body">
                        <div class="container-fluid">
                            <div style="margin-right: 10px;" runat="server" id="divEventList">
                                <div class="row">
                                    <div class="TableLabel col-sm-2">
                                        <asp:Label ID="lblEventListLabel" runat="server" CssClass="form-control NoShadow">Event List:</asp:Label>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:DropDownList ID="ddlMissedEvents" runat="server" OnSelectedIndexChanged="ddlMissedEvents_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control autoWidth" />
                                    </div>
                                </div>
                                <div class="row Padding5">
                                    <div class="TableLabel col-sm-2">
                                        <span class="form-control NoShadow">Role:</span>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:DropDownList ID="ddlRoles" runat="server" CssClass="form-control autoWidth" />
                                        <asp:Label ID="lblRole" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row Padding5" id="divPCStaff" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Character:</span>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:DropDownList ID="ddlCharacterList" runat="server" CssClass="form-control autoWidth" />
                                        <asp:Label ID="lblCharacter" runat="server" CssClass="form-control NoShadow" />
                                    </div>
                                </div>
                                <div class="row Padding5" id="divNPC" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Send CP to:</span>
                                    </div>
                                    <div class="col-sm-9 NoLeftPadding">
                                        <asp:DropDownList ID="ddlSendToCampaign" runat="server" CssClass="form-control autoWidth" />
                                    </div>
                                </div>
                                <div class="row Padding5" style="padding-right: 0px;" id="divSendOther" runat="server">
                                    <div class="col-sm-2 TableLabel">
                                        <span class="form-control NoShadow">Comments To Staff:</span>
                                    </div>
                                    <div class="col-sm-10 NoLeftPadding">
                                        <asp:TextBox ID="tbSendToCPOther" runat="server" CssClass="col-lg-12 col-sm-12 form-control" Style="padding: 0px;" MaxLength="500" Rows="4" TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>

                            <div id="divNoEvents" runat="server">
                                <h1>There are no events that you have not registered for yet.</h1>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-xs-6">
                                <asp:Button ID="btnReturn" runat="server" Text="Return To PELs" CssClass="btn btn-primary" PostBackUrl="~/PELs/PELList.aspx" />
                            </div>
                            <div class="col-xs-6 text-right">
                                <asp:Button ID="btnRegisterForEvent" runat="server" Text="Register For Event" CssClass="btn btn-primary" OnClick="btnRegisterForEvent_Click" />
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
                    <h3 class="modal-title text-center">LARPortal Registration</h3>
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

