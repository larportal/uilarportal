<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="PointsAssign.aspx.cs" Inherits="LarpPortal.Points.PointsAssign" MaintainScrollPositionOnPostback="true" %>

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

        /*.funkyradio input[type="radio"]:hover:not(:checked) ~ label,
        .funkyradio input[type="checkbox"]:hover:not(:checked) ~ label {
            color: #888;
        }

            .funkyradio input[type="radio"]:hover:not(:checked) ~ label:before,
            .funkyradio input[type="checkbox"]:hover:not(:checked) ~ label:before {
                font-family: FontAwesome;
                content: '\2714';
                text-indent: .9em;
                color: #C2C2C2;
            }*/

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
                    <h1>Assign Character Points</h1>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlAssignHeader" runat="server" Visible="true">
            <div class="row">
                <div class="col-lg-11 col-md-10 col-sm-12 col-xs-12">
                    <div class="form-inline">
                        <div class="form-group">
                            <label for="<%= ddlAttendance.ClientID %>" class="control-label BottomSpacing">Attendance: </label>
                            <asp:DropDownList ID="ddlAttendance" runat="server" AutoPostBack="true" CssClass="form-control autoWidth BottomSpacing RightSpacing" OnSelectedIndexChanged="ddl_ReloadGrid" />
                        </div>
                        <div class="form-group">
                            <label for="<%= ddlEarnType.ClientID %>" class="control-label BottomSpacing">Earn Type: </label>
                            <asp:DropDownList ID="ddlEarnType" runat="server" AutoPostBack="true" CssClass="form-control autoWidth BottomSpacing RightSpacing" OnSelectedIndexChanged="ddl_ReloadGrid" />
                        </div>
                        <div class="form-group">
                            <label for="<%= ddlEarnReason.ClientID %>" class="control-label BottomSpacing">Earn Reason: </label>
                            <asp:DropDownList ID="ddlEarnReason" runat="server" AutoPostBack="true" CssClass="form-control autoWidth BottomSpacing RightSpacing" OnSelectedIndexChanged="ddl_ReloadGrid" />
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

        <asp:Panel ID="pnlAddHeader" runat="server" Visible="false">
            <div class="row">
                <div class="col-sm-12 text-right margin10">
                    <asp:Button ID="btnAssignExisting" runat="server" CssClass="btn btn-primary" Text="Assign View" ToolTip="Switch to assign view to assign existing point opportunities"
                        OnClick="btnAssignExisting_Click" />
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAssignExisting" runat="server">
            <div class="panel panel-default">
                <div class="panel-heading">Points Awaiting Assignment</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12">
                            <div class="pre-scrollable">
                                <asp:HiddenField ID="hidCampaignPlayerUserID" runat="server" />
                                <asp:GridView ID="gvPoints" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                    OnRowCancelingEdit="gvPoints_RowCancelingEdit" OnRowEditing="gvPoints_RowEditing" OnRowUpdating="gvPoints_RowUpdating"
                                    OnRowUpdated="gvPoints_RowUpdated" OnRowDeleting="gvPoints_RowDeleting" OnRowDataBound="gvPoints_RowDataBound"
                                    CssClass="table table-striped table-hover table-condensed">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="EventDate" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Event" DataField="EventName" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Player Name" ItemStyle-Wrap="true" DataField="PlayerName" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Character Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" DataField="CharacterAKA" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Earn Desc" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEarnDescription" runat="server" Text='<%# Eval("Description") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbEarnDescription" runat="server" Text='<%# Eval("Description") %>' CssClass="form-control"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCPValue" runat="server" Text='<%# Eval("CPValue") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCPValue" runat="server" Text='<%# Eval("CPValue") %>' CssClass="form-control"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Staff Comments">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStaffComments" runat="server" Visible="false" Text='<%# Eval("StaffComments") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbStaffComments" runat="server" Visible="true" Text='<%# Eval("StaffComments") %>' CssClass="form-control" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right" ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidReceiptDate" runat="server" Value='<%# Eval("ReceiptDate") %>' />
                                                <asp:HiddenField ID="hidReceivedBy" runat="server" Value='<%# Eval("ReceiversName") %>' />
                                                <asp:HiddenField ID="hidPointID" runat="server" Value='<%# Eval("CampaignCPOpportunityID") %>' />
                                                <asp:HiddenField ID="hidCampaignPlayer" runat="server" Value='<%# Eval("CampaignPlayerID") %>' />
                                                <asp:HiddenField ID="hidCharacterID" runat="server" Value='<%# Eval("CharacterID") %>' />
                                                <asp:HiddenField ID="hidEventID" runat="server" Value='<%# Eval("EventID") %>' />
                                                <asp:HiddenField ID="hidOpportunityNotes" runat="server" Value='<%# Eval("OpportunityNotes") %>' />
                                                <asp:HiddenField ID="hidExampleURL" runat="server" Value='<%# Eval("ExampleURL") %>' />
                                                <asp:HiddenField ID="hidCPOpportunityDefaultID" runat="server" Value='<%# Eval("CampaignCPOpportunityDefaultID") %>' />
                                                <asp:HiddenField ID="hidReasonID" runat="server" Value='<%# Eval("ReasonID") %>' />
                                                <asp:HiddenField ID="hidAddedByID" runat="server" Value='<%# Eval("AddedByID") %>' />
                                                <asp:HiddenField ID="hidCPAssignmentDate" runat="server" Value='<%# Eval("CPAssignmentDate") %>' />
                                                <asp:HiddenField ID="hidRole" runat="server" Value='<%# Eval("Role") %>' />
                                                <asp:HiddenField ID="hidNPCCampaignID" runat="server" Value='<%# Eval("NPCCampaignID") %>' />
                                                <asp:HiddenField ID="hidRegistrationID" runat="server" Value='<%# Eval("RegistrationID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-primary btn-sm" />
                                                <asp:Button ID="btnAssign" runat="server" CommandName="Update" Text="Assign" CssClass="btn btn-primary btn-sm" />
                                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-primary btn-sm" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="btnupdate2" runat="server" CommandName="Update" Text="Assign" CssClass="btn btn-primary btn-sm" />
                                                <asp:Button ID="btncancel2" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-primary btn-sm" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
                            <asp:Panel ID="pnlAddSourceCampaign" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlAddSourceCampaign.ClientID %>">Source Campaign:<%--B--%></label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList ID="ddlAddSourceCampaign" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlAddSourceCampaign_SelectedIndexChanged" />
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12">
                                            <asp:Label ID="lblAddMessage" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlAddOpportunityDefault" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlAddOpportunityDefaultID.ClientID %>">Earn Description:<%--C1--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlAddOpportunityDefaultID" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlAddOpportunityDefaultID_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlAddOpportunityDefaultC6" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlAddOpportunityDefaultIDC6 %>">Earn Description:<%--C6--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlAddOpportunityDefaultIDC6" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlAddOpportunityDefaultIDC6_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlCPDestinationD6" runat="server" Visible="false">
                                <%--D6 Choose destination for CP--%>
                            </asp:Panel>

                            <asp:Panel ID="pnlNPCCheckboxes" runat="server" Visible="false">
                                <%--E3, E4 or E6--%>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSourceEvent.ClientID %>">Event:</label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSourceEvent" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSourceEvent_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-lg-2"></div>
                                        <div class="col-lg-10">
                                            <div class="funkyradio">
                                                <div class="row">
                                                    <asp:CheckBox ID="chkNPCEvent" Text="NPC Event" runat="server" Checked="true" CssClass="col-lg-3 col-md-4 col-xs-6" />
                                                    <div class="col-lg-1 col-sm-2 col-xs-2" style="padding-left: 0px;">
                                                        <asp:TextBox ID="txtNPCEvent" runat="server" Text="1" CssClass="form-control" />
                                                    </div>
                                                    <div class="col-xs-4 no-padding" style="padding-left: 0px;">
                                                        <span class="form-control NoShadow" style="padding-left: 0px;">Points</span>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <asp:CheckBox ID="chkSetupCleanup" Text="Setup/Cleanup" runat="server" Checked="true" CssClass="col-lg-3 col-md-4 col-xs-6" />
                                                    <%--Conditional Value--%>
                                                    <div class="col-lg-1 col-sm-2 col-xs-2" style="padding-left: 0px;">
                                                        <asp:TextBox ID="txtSetupCleanup" runat="server" Text="0.5" CssClass="form-control" />
                                                    </div>
                                                    <div class="col-xs-4 no-padding" style="padding-left: 0px;">
                                                        <span class="form-control NoShadow" style="padding-left: 0px;">Points</span>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <asp:CheckBox ID="chkPEL" Text="PEL" runat="server" Checked="true" CssClass="col-lg-3 col-md-4 col-xs-6" />
                                                    <%--Conditional Value--%>
                                                    <div class="col-lg-1 col-sm-2 col-xs-2" style="padding-left: 0px;">
                                                        <asp:TextBox ID="txtPEL" runat="server" Text="0.5" CssClass="form-control" />
                                                    </div>
                                                    <div class="col-xs-4 no-padding" style="padding-left: 0px;">
                                                        <span class="form-control NoShadow" style="padding-left: 0px;">Points</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlCPDestinationD3" runat="server" Visible="false">
                                <%--D3 Choose event, Choose destination for CP--%>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlDestinationCampaign.ClientID %>">Send Points To:<%--D3--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlDestinationCampaign" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlDestinationCampaign_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlAddDonationCP" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlDonationTypes.ClientID %>">Donation Type:<%--F0--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlDonationTypes" runat="server" AutoPostBack="true" Enabled="true" CssClass="form-control autoWidth"
                                                OnSelectedIndexChanged="ddlDonationTypes_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Text="Donation"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Art"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Costume"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Effects"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Food"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Hats"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Labor"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Masks"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Miscellaneous"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Money"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Packets"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Prop"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Shields"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Water"></asp:ListItem>
                                                <asp:ListItem Text="Donation, Weapons"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= txtOpportunityNotes.ClientID %>">Donation Notes:<%--F0--%></label>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="txtOpportunityNotes" runat="server" CssClass="form-control"
                                                OnTextChanged="txtOpportunityNotes_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= txtReceiptDate.ClientID %>">Receipt Date:<%--F0--%></label>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="txtReceiptDate" runat="server" Width="150px" CssClass="form-control"
                                                OnTextChanged="txtReceiptDate_TextChanged"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txtReceiptDate" Format="MM/dd/yyyy" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= txtCPF0.ClientID %>">Points:<%--F0--%></label>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="txtCPF0" runat="server" Width="100px" Text="0" CssClass="form-control" OnTextChanged="txtCPF0_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSelectCharacterOrBankF0.ClientID %>">Character:<%--F0--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSelectCharacterOrBankF0" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectCharacterOrBankF0_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

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

                            <asp:Panel ID="pnlAddPCLocalCP" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSourceEventPC.ClientID %>">Event:</label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSourceEventPC" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSourceEventPC_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= txtCPF2.ClientID %>">Points:<%--F2--%></label>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="txtCPF2" runat="server" Width="100px" Text="0" CssClass="form-control autoWidth" OnTextChanged="txtCPF2_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSelectCharacterOrBankF2.ClientID %>">Character:<%--F2--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSelectCharacterOrBankF2" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectCharacterOrBankF2_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlAddNPCLocalCPStaying" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSelectCharacterOrBankF3.ClientID %>">Character:<%--F3--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSelectCharacterOrBankF3" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectCharacterOrBankF3_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlAddNPCLocalCPGoingToLARPPortalCampaign" runat="server" Visible="false">
                                <%--F4--%>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSelectCharacterOrBankF4.ClientID %>">Character:<%--F4--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSelectCharacterOrBankF4" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectCharacterOrBankF4_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlAddNPCLocalCPGoingToNonLARPPortalCampaign" runat="server" Visible="false">
                                <%--F5 Different from others. Set up email--%>
                            </asp:Panel>

                            <asp:Panel ID="pnlAddNPCIncoming" runat="server" Visible="false">
                                <%--F6--%>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-2 control-label" for="<%= ddlSelectCharacterOrBankF6.ClientID %>">Character:<%--F6--%></label>
                                        <div class="col-lg-10">
                                            <asp:DropDownList ID="ddlSelectCharacterOrBankF6" runat="server" CssClass="form-control autoWidth" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectCharacterOrBankF6_SelectedIndexChanged">
                                            </asp:DropDownList>
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
