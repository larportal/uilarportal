<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="ChooseHousing.aspx.cs" Inherits="LarpPortal.Events.ChooseHousing" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="EventsAssignHousingStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .Events td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>

<asp:Content ID="EventsAssignHousingScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="EventsAssignHousingBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Choose Housing</h1>
                </div>
            </div>
        </div>

        <%--        <div class="row">
            <div class="form-inline col-xs-12">
                <label for="<%= ddlEvent.ClientID %>">Events:</label>
                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control autoWidth" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged" AutoPostBack="true" />
                <div class="pull-right">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-xs-12">
                <div class="margin10"></div>
                <asp:Label ID="lblEventInfo" runat="server" />
                <div class="margin10"></div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <asp:UpdatePanel ID="upHousing" runat="server">
                    <contenttemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">Housing Options</div>
                            <div class="panel-body">
                                <div class="row">
                                    <asp:GridView ID="gvHousingOptions" runat="server" OnRowCommand="gvHousingOptions_RowCommand" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false"
                                        CssClass="table table-striped table-hover table-condensed table-responsive Events">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="LocationID" runat="server" Value='<%# Eval("LocationID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LocationName" HeaderText="Location Name" />
                                            <asp:BoundField DataField="TotalBeds" HeaderText="Total Beds" ItemStyle-Wrap="true" />
                                            <asp:BoundField DataField="BedsAvail" HeaderText="Total Avail" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btn" Visible="" CommandArgument='<%#Eval("LocationID") %>' CommandName="Select"  Text="Select" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
            </div>
            </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </div>
    <div class="row">
        <div class="form-inline col-xs-12">
            <div class="pull-right">
                <asp:Button ID="btnBottomSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidApprovedStatus" runat="server" />

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close">&times;</button>
                    <h3 class="modal-title text-center">Event Housing</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" Text="Your housing choice has been saved." />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 text-right">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidRegID" runat="server" />
    <asp:HiddenField ID="hidScollPosition" runat="server" Value="" />

<%--    <script type="text/javascript">
        function setScrollValue() {
            var divObj = $get('divRegs');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (obj) obj.value = divObj.scrollTop;
        }

        function pageLoad() {
            var divObj = $get('divRegs');
            var obj = $get('<%= hidScollPosition.ClientID %>');
            if (divObj) divObj.scrollTop = obj.value;
        }
    </script>--%>

</asp:Content>