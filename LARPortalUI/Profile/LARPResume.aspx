<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="LARPResume.aspx.cs" Inherits="LarpPortal.Profile.LARPResume" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="LARPResumeStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>
<asp:Content ID="LARPResumeScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModalMessage() {
            $('#ModalMessage').modal('show');
        }

        function openResumeItem(PlayerLARPResumeID, PlayerProfileID, GameSystem, Campaign, AuthorGM, StyleID, GenreID, RoleID, StartDate, EndDate, PlayerComments) {
            $('#modalResumeItem').modal('show');

            var tbStartDate = document.getElementById('<%= tbStartDate.ClientID %>');
            if (tbStartDate)
                tbStartDate.value = StartDate;

            var tbEndDate = document.getElementById('<%= tbEndDate.ClientID %>');
            if (tbEndDate)
                tbEndDate.value = EndDate;

            var tbGameSystem = document.getElementById('<%= tbGameSystem.ClientID %>');
            if (tbGameSystem)
                tbGameSystem.value = GameSystem;

            var tbCampaign = document.getElementById('<%= tbCampaign.ClientID %>');
            if (tbCampaign)
                tbCampaign.value = Campaign;

            var tbAuthorGM = document.getElementById('<%= tbAuthorGM.ClientID %>');
            if (tbAuthorGM)
                tbAuthorGM.value = AuthorGM;

            var hidPlayerResumeID = document.getElementById('<%= hidPlayerLARPResumeID.ClientID %>');
            if (hidPlayerResumeID)
                hidPlayerResumeID.value = PlayerLARPResumeID;

            var ddlStyle = document.getElementById('<%= ddlStyle.ClientID %>');
            setSelectedValue(ddlStyle, StyleID);

            var ddlGenre = document.getElementById('<%= ddlGenre.ClientID %>');
            setSelectedValue(ddlGenre, GenreID);

            var ddlRole = document.getElementById('<%= ddlRole.ClientID %>');
            setSelectedValue(ddlRole, RoleID);

            var tbComments = document.getElementById('<%= tbComments.ClientID %>');
            if (tbComments)
                tbComments.value = PlayerComments;

            return false;
        }

        function openDeleteResumeItem(PlayerLARPResumeID, GameSystem, Campaign, AuthorGM, StartDate, EndDate) {
            $('#modalDeleteResumeItem').modal('show');

            var hidDeleteResumeItemID = document.getElementById('<%= hidDeleteResumeItemID.ClientID %>');
            if (hidDeleteResumeItemID)
                hidDeleteResumeItemID.value = PlayerLARPResumeID;

            var lblDeleteMessage = document.getElementById('<%= lblDeleteMessage.ClientID %>');
            if (lblDeleteMessage) {
                lblDeleteMessage.innerHTML = "";
                if (GameSystem.length > 0)
                    lblDeleteMessage.innerHTML += "Game System: " + GameSystem + "<br>";
                if (Campaign.length > 0)
                    lblDeleteMessage.innerHTML += "Campaign: " + Campaign + "<br>";
                if (AuthorGM.length > 0)
                    lblDeleteMessage.innerHTML += "Author/GM: " + AuthorGM + "<br>";
                if (StartDate.length > 0)
                    lblDeleteMessage.innerHTML += "Start Date: " + StartDate + "<br>";
                if (EndDate.length > 0)
                    lblDeleteMessage.innerHTML += "End Date: " + EndDate + "<br>";

                lblDeleteMessage.innerHTML += "<br>Are you sure you want to delete this record ?";
            }
            return false;
        }

        function setSelectedValue(selectObj, valueToSet) {
            for (var i = 0; i < selectObj.options.length; i++) {
                if (selectObj.options[i].value == valueToSet) {
                    selectObj.options[i].selected = true;
                    return;
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="LARPResumeBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>LARP Resume</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Player LARP Resume</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView runat="server" ID="gvResumeItems" AutoGenerateColumns="false" GridLines="None"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                        CssClass="table table-striped table-hover table-condensed col-sm-12">
                                        <Columns>
                                            <asp:BoundField DataField="StartDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Start Date" />
                                            <asp:BoundField DataField="EndDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="End Date" />
                                            <asp:BoundField DataField="GameSystem" HeaderText="Game System" />
                                            <asp:BoundField DataField="Campaign" HeaderText="Campaign" />
                                            <asp:BoundField DataField="AuthorGM" HeaderText="Author GM" />
                                            <asp:BoundField DataField="Style" HeaderText="Style" />
                                            <asp:BoundField DataField="Genre" HeaderText="Genre" />
                                            <asp:BoundField DataField="Role" HeaderText="Role" />
                                            <asp:BoundField DataField="PlayerComments" HeaderText="Comments" />
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="36" ItemStyle-CssClass="text-right" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" ToolTip="Edit the item"
                                                        OnClientClick='<%# string.Format("return openResumeItem({0}, {1}, \"{2}\", \"{3}\", \"{4}\", {5}, {6}, {7}, \"{8:MM/dd/yyyy}\", \"{9:MM/dd/yyyy}\", \"{10}\");",
                                                            Eval("PlayerLARPResumeID"), Eval("PlayerProfileID"), Eval("GameSystem"), Eval("Campaign"), Eval("AuthorGM"), Eval("StyleID"),
                                                            Eval("GenreID"), Eval("RoleID"), Eval("StartDate"), Eval("EndDate"), 
                                                            Eval("PlayerComments")) %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" ToolTip="Delete the item"
                                                        OnClientClick='<%# string.Format("return openDeleteResumeItem({0}, \"{1}\", \"{2}\", \"{3}\", \"{4:d}\", \"{5:d}\");",
                                                            Eval("PlayerLARPResumeID"), Eval("GameSystem"), Eval("Campaign"), Eval("AuthorGM"), Eval("StartDate"), 
                                                            Eval("EndDate")) %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnAddResumeItem" runat="server" Text="Add Item" CssClass="btn btn-primary"
                                    OnClientClick="return openResumeItem(-1, '', '', '', '', '', '', '', '', '');" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Comments</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:TextBox ID="tbLARPResumeComments" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" />
                                        </div>
                                    </div>
                                    <div class="margin20"></div>
                                    <div class="row">
                                        <div class="col-xs-12 text-right">
                                            <asp:Button ID="btnSaveComments" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveComments_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="ModalMessage" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Resume Item</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblModalMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer text-right">
                        <button type="button" data-dismiss="modal" class="btn btn-primary">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="modalResumeItem" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Resume Item</h3>
                    </div>
                    <div class="modal-body">
                        <div class="margin10"></div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <label>Start Date:</label>
                                    <asp:TextBox ID="tbStartDate" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:CalendarExtender ID="ceStartMonthYear" runat="server" TargetControlID="tbStartDate" />
                                </div>
                                <div class="col-xs-6">
                                    <label>End Date:</label>
                                    <asp:TextBox ID="tbEndDate" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:CalendarExtender ID="ceEndMonthYear" runat="server" TargetControlID="tbEndDate" />
                                </div>
                            </div>
                        </div>
                        <div class="margin10"></div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <label>Game System:</label>
                                    <asp:TextBox ID="tbGameSystem" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-xs-6">
                                    <label>Campaign:</label>
                                    <asp:TextBox ID="tbCampaign" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="margin10"></div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <label>Author GM:</label>
                                    <asp:TextBox ID="tbAuthorGM" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-xs-6">
                                    <label>Style:</label>
                                    <asp:DropDownList ID="ddlStyle" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="margin10"></div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <label>Genre:</label>
                                    <asp:DropDownList ID="ddlGenre" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-xs-6">
                                    <label>Role:</label>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="margin10"></div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <label>Comments:</label>
                                    <asp:TextBox ID="tbComments" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-6 text-left">
                            <button type="button" data-dismiss="modal" class="btn btn-primary">Close</button>
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidPlayerLARPResumeID" runat="server" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade in" id="modalDeleteResumeItem" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Resume - Delete</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblDeleteMessage" runat="server" Text="" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-6 text-left">
                                <asp:Button ID="btnCancelDelete" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                            </div>
                            <div class="col-xs-6 text-right">
                                <asp:HiddenField ID="hidDeleteResumeItemID" runat="server" />
                                <asp:Button ID="btnDeleteResumeItem" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteResumeItem_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidPlayerProfileID" runat="server" />
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

