<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="PELAnswerSummary.aspx.cs" Inherits="LarpPortal.Reports.PELAnswerSummary" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PELAnswerSummaryStyles" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="PELAnswerSummaryScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="PELAnswerSummaryBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>PEL Answer Summary Report</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <%--                <CampSelector:Select ID="oCampSelect" runat="server" />--%>
                <label for="ddlEvent" runat="server" id="Label1" style="padding-left: 10px;">Event: </label>
                <asp:DropDownList ID="ddlEvent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" Visible="true" CssClass="form-control autoWidth" />
                <asp:Label ID="lblRole" runat="server" AssociatedControlID="ddlRole" Visible="false" Text="Player Type: " />
                <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" Visible="false" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" />
            </div>
        </div>
        <div class="margin10"></div>
        <div class="row">
            <div class="form-inline col-xs-12">
                <asp:Label ID="lblQuestion" runat="server" AssociatedControlID="ddlQuestion" Visible="false" Text="Question: " />
                <asp:DropDownList ID="ddlQuestion" runat="server" Visible="false" CssClass="form-control autoWidth" />
                <asp:Button ID="btnRunReport" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnRunReport_Click" />
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-primary" Text="Export To Excel" OnClick="btnExportExcel_Click" />
            </div>
        </div>
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Summary PEL Answers</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                        <asp:GridView ID="gvAnswers" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                            CssClass="table table-striped table-hover table-condensed">
                                            <Columns>
                                                <asp:BoundField DataField="Question" HeaderText="Question" />
                                                <asp:BoundField DataField="Answer" HeaderText="Answer" />
                                                <asp:BoundField DataField="CharacterAKA" HeaderText="Character" HeaderStyle-Wrap="false" />
                                                <asp:BoundField DataField="Player" HeaderText="Player" HeaderStyle-Wrap="false" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
