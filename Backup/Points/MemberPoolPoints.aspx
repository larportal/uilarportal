<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="MemberPoolPoints.aspx.cs" Inherits="LarpPortal.Points.MemberPoolPoints" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PointsAssignStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .BottomSpacing {
            margin-bottom: 10px !important;
        }

        .RightSpacing {
            margin-right: 10px !important;
        }

        .LabelSpacing {
            margin-top: 10px;
            margin-right: 10px;
        }

        .funkyradio div {
            /*clear: both;*/
            overflow: hidden;
        }

        .funkyradio label {
            width: 100%;
            border-radius: 3px;
            border: 1px solid #D1D3D4;
            font-weight: normal;
        }

        .funkyradio input[type="radio"]:empty,
        .funkyradio input[type="checkbox"]:empty {
            display: none;
        }

            .funkyradio input[type="radio"]:empty ~ label,
            .funkyradio input[type="checkbox"]:empty ~ label {
                position: relative;
                line-height: 2.5em;
                text-indent: 3.75em;
                margin-bottom: 1em;
                cursor: pointer;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
            }

                .funkyradio input[type="radio"]:empty ~ label:before,
                .funkyradio input[type="checkbox"]:empty ~ label:before {
                    position: absolute;
                    display: block;
                    top: 0;
                    bottom: 0;
                    left: 0;
                    content: '';
                    width: 2.5em;
                    background: #D1D3D4;
                    border-radius: 3px 0 0 3px;
                }

        .funkyradio input[type="radio"]:checked ~ label,
        .funkyradio input[type="checkbox"]:checked ~ label {
            color: #777;
        }

            .funkyradio input[type="radio"]:checked ~ label:before,
            .funkyradio input[type="checkbox"]:checked ~ label:before {
                font-family: FontAwesome;
                content: '\f046';
                font-size: larger;
                text-indent: 0.9em;
                color: #333;
                background-color: #ccc;
            }


        .funkyradio input[type="radio"]:not(:checked) ~ label:before,
        .funkyradio input[type="checkbox"]:not(:checked) ~ label:before {
            font-family: FontAwesome;
            content: '\f096';
            font-size: larger;
            text-indent: 0.9em;
            color: #333;
            background-color: #ccc;
        }

        .funkyradio input[type="radio"]:focus ~ label:before,
        .funkyradio input[type="checkbox"]:focus ~ label:before {
            box-shadow: 0 0 0 3px #999;
        }

        .funkyradio-default input[type="radio"]:checked ~ label:before,
        .funkyradio-default input[type="checkbox"]:checked ~ label:before {
            color: #333;
            background-color: #ccc;
        }

        .funkyradio-primary input[type="radio"]:checked ~ label:before,
        .funkyradio-primary input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #337ab7;
        }

        .funkyradio-success input[type="radio"]:checked ~ label:before,
        .funkyradio-success input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #5cb85c;
        }

        .funkyradio-danger input[type="radio"]:checked ~ label:before,
        .funkyradio-danger input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #d9534f;
        }

        .funkyradio-warning input[type="radio"]:checked ~ label:before,
        .funkyradio-warning input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #f0ad4e;
        }

        .funkyradio-info input[type="radio"]:checked ~ label:before,
        .funkyradio-info input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #5bc0de;
        }
    </style>
</asp:Content>
<asp:Content ID="PointsAssignScripts" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="PointsAssignBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Assign Character Pool/Bank Points</h1>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlAssignHeader" runat="server" Visible="true">
            <div class="row">
                <div class="col-lg-11 col-md-10 col-sm-12 col-xs-12">
                    <div class="form-inline">
                        <div class="form-group">
                            <%--<label for="<%= ddlPool.ClientID %>" class="control-label BottomSpacing">Pool: </label>--%>
                            <asp:DropDownList ID="ddlEarnType" runat="server" AutoPostBack="true" CssClass="form-control autoWidth BottomSpacing RightSpacing" OnSelectedIndexChanged="ddl_ReloadGrid" />
                        </div>
                        <div class="form-group">
                            <label for="<%= ddlPlayer.ClientID %>" class="control-label BottomSpacing">Player: </label>
                            <asp:DropDownList ID="ddlPlayer" runat="server" AutoPostBack="true" CssClass="form-control autoWidth BottomSpacing RightSpacing" OnSelectedIndexChanged="ddl_ReloadGrid" />
                        </div>
                        <div class="form-group">
                            <label for="<%= ddlCharacters.ClientID %>" class="control-label BottomSpacing">Character: </label>
                            <asp:DropDownList ID="ddlCharacters" runat="server" AutoPostBack="true" CssClass="form-control autoWidth BottomSpacing RightSpacing" OnSelectedIndexChanged="ddl_ReloadGrid" />
                        </div>
                    </div>
                </div>
                <div class="col-lg-1 col-md-2 col-sm-6 col-xs-6">
                    <div class="text-right">
                        <div class="row">
                            <asp:Button ID="btnAddNewOpportunity" runat="server" CssClass="btn btn-primary" Text="Add View" ToolTip="Switch to add view to add new points" OnClick="btnAddNewOpportunity_Click" />
                        </div>
                        <div class="margin10"></div>
                        <div class="row">
                            <asp:UpdatePanel ID="upnlAssignAll" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnAssignAll" runat="server" Visible="true" CssClass="btn btn-primary" Text="Assign All" OnClick="btnAssignAll_Click" />
                                    <asp:Label ID="lblAssignAll" runat="server" Visible="false" CssClass="margin10" Text="Please wait" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <%--Add new points panel--%>
        <asp:Panel ID="pnlAddNewCP" runat="server" Visible="false">
            <div class="panel panel-default">
                <div class="panel-heading">Add New Points</div>
                <div class="panel-body">
                    <div class="col-xs-12">
                        <div class="row">
                            <asp:HiddenField ID="hidInsertCampaignPlayerID" runat="server" />
                            <asp:HiddenField ID="hidInsertCharacterID" runat="server" />
                            <asp:HiddenField ID="hidInsertCampaignCPOpportunityDefaultID" runat="server" />
                            <asp:HiddenField ID="hidInsertCampaignCPOpportunityDefaultIDNPCEvent" runat="server" />
                            <asp:HiddenField ID="hidInsertCampaignCPOpportunityDefaultIDNPCSetup" runat="server" />
                            <asp:HiddenField ID="hidInsertCampaignCPOpportunityDefaultIDNPCPEL" runat="server" />
                            <asp:HiddenField ID="hidInsertEventID" runat="server" />
                            <asp:HiddenField ID="hidInsertCampaignID" runat="server" />
                            <asp:HiddenField ID="hidInsertDescription" runat="server" />
                            <asp:HiddenField ID="hidInsertDescriptionNPCEvent" runat="server" />
                            <asp:HiddenField ID="hidInsertDescriptionNPCSetup" runat="server" />
                            <asp:HiddenField ID="hidInsertDescriptionNPCPEL" runat="server" />
                            <asp:HiddenField ID="hidInsertDestinationCampaign" runat="server" />
                            <asp:HiddenField ID="hidInsertDestinationCampaignLPType" runat="server" />
                            <asp:HiddenField ID="hidInsertOpportunityNotes" runat="server" />
                            <asp:HiddenField ID="hidInsertExampleURL" runat="server" />
                            <asp:HiddenField ID="hidInsertReasonID" runat="server" />
                            <asp:HiddenField ID="hidInsertReasonIDNPCEvent" runat="server" />
                            <asp:HiddenField ID="hidInsertReasonIDNPCSetup" runat="server" />
                            <asp:HiddenField ID="hidInsertReasonIDNPCPEL" runat="server" />
                            <asp:HiddenField ID="hidInsertStatusID" runat="server" />
                            <asp:HiddenField ID="hidInsertAddedByID" runat="server" />
                            <asp:HiddenField ID="hidInsertCPValue" runat="server" />
                            <asp:HiddenField ID="hidInsertApprovedByID" runat="server" />
                            <asp:HiddenField ID="hidInsertReceiptDate" runat="server" />
                            <asp:HiddenField ID="hidInsertReceivedByID" runat="server" />
                            <asp:HiddenField ID="hidInsertCPAssignmentDate" runat="server" />
                            <asp:HiddenField ID="hidInsertStaffComments" runat="server" />
                            <asp:HiddenField ID="hidLastAddCPStep" runat="server" />
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-2 col-xs-5 control-label" for="<%= ddlCampaignPlayer.ClientID %>">Player:<%--A--%></label>
                                    <div class="col-xs-9">
                                        <asp:DropDownList ID="ddlCampaignPlayer" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCampaignPlayer_SelectedIndexChanged" />
                                    </div>
                                    <div class="col-lg-1 col-xs-2 text-right">
                                        <asp:Button ID="btnSaveNewOpportunity" runat="server" CssClass="btn btn-primary" Text="Save"
                                            OnClick="btnSaveNewOpportunity_Click" />
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlAddNonEventCP" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= txtCPF1.ClientID %>">Points:<%--F1--%></label>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="txtCPF1" runat="server" Width="100px" Text="0" CssClass="form-control" OnTextChanged="txtCPF1_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSelectCharacterOrBankF1.ClientID %>">Character:<%--F1--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSelectCharacterOrBankF1" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectCharacterOrBankF1_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlAddStaffComments" runat="server" Visible="true">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= txtStaffComments.ClientID %>">Staff Comments: </label>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="txtStaffComments" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlCharacterPointDisplay" runat="server" Visible="false">
            <asp:Label ID="lblHeader" runat="server" Font-Size="24px" Style="font-weight: 500" Text="" />
            <div class="panel panel-default">
                <div class="panel-heading">Total Points<asp:Label ID="lblGridHeader" runat="server"></asp:Label></div>
                <div class="panel-body">
                    <div class="col-xs-12">
                        <div class="row">
                            <div style="max-height: 500px; overflow-y: auto;">
                                <asp:GridView ID="gvPointsList" runat="server" AutoGenerateColumns="false" GridLines="None"
                                    CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                                    <RowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="ReceiptDate" HeaderText=" Earn Date" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="false"
                                            HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="FullDescription" HeaderText="Type" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="AdditionalNotes" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="CPAmount" HeaderText="Points" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="StatusName" HeaderText="Status" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="CPApprovedDate" HeaderText="Spend Date" DataFormatString="{0: MM/dd/yyyy}" ItemStyle-Wrap="true"
                                            HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="RecvFromCampaign" HeaderText="Earned At" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="OwningPlayer" HeaderText="Earned By" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="ReceivingCampaign" HeaderText="Spent At" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="Character" HeaderText="Spent On" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="ReceivingPlayer" HeaderText="Transfer To" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                        <asp:BoundField DataField="CPApprovedDate" HeaderText="Approved" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
