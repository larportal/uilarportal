<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="True" CodeBehind="DonationAdd.aspx.cs" Inherits="LarpPortal.Donations.DonationAdd" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="DonationEditStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .ErrorDisplay {
            font-weight: bold;
            font-style: italic;
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="DonationEditScripts" ContentPlaceHolderID="MainScripts" runat="server">

    <script type="text/javascript">

</script>
</asp:Content>
<asp:Content ID="EventEditBody" ContentPlaceHolderID="MainBody" runat="server">


    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Maintain Donations</h1>
                </div>
            </div>
        </div>

        <h2><asp:Label ID="lblEventInfo" runat="server" ></asp:Label></h2>

       <asp:Panel ID="pnlDonationAdd" runat="server" Visible="true">
            <%--Row to add a new donation item--%>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Add New Donation</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <label for="tbDescription">Description:</label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbDescription" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblDonationType" runat="server">Donation Type:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:DropDownList ID="ddlDonationType" runat="server" AutoPostBack="false" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblQuantityNeeded" runat="server">Qty Needed:</asp:Label>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="tbQuantityNeeded" runat="server" CssClass="form-control" Text="1" />
                                        <asp:RegularExpressionValidator ID="revQuantityNeeded" runat="server" ControlToValidate="tbQuantityNeeded" ErrorMessage="Numbers Only" ValidationExpression="\d+" />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblReward" runat="server" >Worth:</asp:Label>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="tbUnitReward" runat="server" CssClass="form-control" Text="1" />
                                        <asp:RegularExpressionValidator ID="revUnitReward" runat="server" ControlToValidate="tbUnitReward" ErrorMessage="Numbers Only" ValidationExpression="^[1-9]\d*(\.\d+)?$" />
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:Label ID="RU" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblReqdBy" runat="server">Reqd By Date:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbReqdBy" runat="server" CssClass="form-control" TextMode="DateTime" />
                                        <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbReqdBy" Format="MM/dd/yyyy" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblURL" runat="server">URL:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbURL" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblDonationComments" runat="server">Donation Comments:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbDonationComments" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="row">
                                        <div class="col-lg-2"><b><u>Ship To Address</u></b></div>
                                        <div class="col-lg-10">
                                            <asp:Button ID="btnFillSTDefault" runat="server" CssClass="btn btn-primary" Text="Fill Default Ship To Address" OnClick="btnFillSTDefault_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-2">
                                            <asp:Label ID="lblSTAdd1" runat="server">Address 1:</asp:Label>
                                        </div>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="tbSTAdd1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-2">
                                            <asp:Label ID="lblSTAdd2" runat="server">Address 2:</asp:Label>
                                        </div>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="tbSTAdd2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-2">
                                            <asp:Label ID="lblSTCity" runat="server">City State Zip:</asp:Label>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:TextBox ID="tbSTCity" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-1">
                                            <asp:TextBox ID="tbSTState" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-1">
                                            <asp:TextBox ID="tbSTZip" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblStaffComments" runat="server">Staff Comments:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbStaffComments" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="row">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <asp:Label ID="lblSTPhone" runat="server">Phone:</asp:Label>
                                            </div>
                                            <div class="col-lg-10">
                                                <asp:TextBox ID="tbSTPhone" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <asp:Label ID="lblSTEmail" runat="server">Email:</asp:Label>
                                            </div>
                                            <div class="col-lg-10">
                                                <asp:TextBox ID="tbSTEmail" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-1"></div>
                            <div class="col-lg-4">
                                <asp:Button ID="btnSaveAdd" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveAdd_Click" />
                            </div>
                            <div class="col-lg-4">
                                <asp:Button ID="btnCancelAdd" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancelAdd_Click" />
                            </div>
                            <div class="col-lg-3">
                                <asp:Button ID="btnReturn" runat="server" CssClass="btn btn-primary" Text="Return to Lists" OnClick="btnReturn_Click" />
                            </div>
                        </div>
                        <div class="row"><br /></div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDonationEdit" runat="server" Visible="false">
            <%--Row to edit an existing donation item--%>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Edit Donation</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <label for="tbDescriptionEdit">Description:</label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbDescriptionEdit" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblDonationTypeEdit" runat="server">Donation Type:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:DropDownList ID="ddlDonationTypeEdit" runat="server" AutoPostBack="false" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblQuantityNeededEdit" runat="server">Qty Needed:</asp:Label>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="tbQuantityNeededEdit" runat="server" CssClass="form-control" Text="1" />
                                        <%--<asp:RegularExpressionValidator ID="revQuantityNeededEdit" runat="server" ControlToValidate="tbQuantityNeededEdit" ErrorMessage="Numbers Only" ValidationExpression="\d+" />--%>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblRewardEdit" runat="server" >Worth:</asp:Label>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:TextBox ID="tbUnitRewardEdit" runat="server" CssClass="form-control" Text="1" />
                                        <%--<asp:RegularExpressionValidator ID="revUnitRewardEdit" runat="server" ControlToValidate="tbUnitRewardEdit" ErrorMessage="Numbers Only" ValidationExpression="^[1-9]\d*(\.\d+)?$" />--%>
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:Label ID="RUEdit" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblReqdByEdit" runat="server">Reqd By Date:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbReqdByEdit" runat="server" CssClass="form-control" TextMode="DateTime" />
                                        <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbReqdByEdit" Format="MM/dd/yyyy" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblURLEdit" runat="server">URL:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbURLEdit" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6"></div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="LabelDonationCommentsEdit" runat="server">Donation Comments:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbDonationCommentsEdit" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="row">
                                        <div class="col-lg-2"><b><u>Ship To Address</u></b></div>
                                        <div class="col-lg-10">
                                            <asp:Button ID="btnFillSTDefaultEdit" runat="server" CssClass="btn btn-primary" Text="Fill Default Ship To Address" OnClick="btnFillSTDefaultEdit_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-2">
                                            <asp:Label ID="lblSTAdd1Edit" runat="server">Address 1:</asp:Label>
                                        </div>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="tbSTAdd1Edit" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-2">
                                            <asp:Label ID="lblSTAdd2Edit" runat="server">Address 2:</asp:Label>
                                        </div>
                                        <div class="col-lg-10">
                                            <asp:TextBox ID="tbSTAdd2Edit" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-2">
                                            <asp:Label ID="lblSTCityEdit" runat="server">City State Zip:</asp:Label>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:TextBox ID="tbSTCityEdit" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-1">
                                            <asp:TextBox ID="tbSTStateEdit" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-1">
                                            <asp:TextBox ID="tbSTZipEdit" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-2">
                                        <asp:Label ID="lblStaffCommentsEdit" runat="server">Staff Comments:</asp:Label>
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="tbStaffCommentsEdit" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="row">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <asp:Label ID="lblSTPhoneEdit" runat="server">Phone:</asp:Label>
                                            </div>
                                            <div class="col-lg-10">
                                                <asp:TextBox ID="tbSTPhoneEdit" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <asp:Label ID="lblSTEmailEdit" runat="server">Email:</asp:Label>
                                            </div>
                                            <div class="col-lg-10">
                                                <asp:TextBox ID="tbSTEmailEdit" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-1">
                                
                            </div>
                            <div class="col-lg-4">
                                <asp:Button ID="btnSaveEdit" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveEdit_Click" />
                            </div>
                            <div class="col-lg-4">
                                <asp:Button ID="btnCancelEdit" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancelEdit_Click" />
                            </div>
                            <div class="col-lg-3">
                                <asp:Button ID="btnReturnEdit" runat="server" CssClass="btn btn-primary" Text="Return to Lists" OnClick="btnReturn_Click" />
                            </div>
                        </div>
                        <div class="row"><br /></div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDonationList" runat="server" Visible="true">
            <%--Row to add a new donation item--%>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Donation List</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvDonationList" runat="server" AutoGenerateColumns="false" GridLines="None" HeaderStyle-Wrap="false" 
                                OnRowCommand="gvDonationList_RowCommand"

                                OnRowDataBound = "gvDonationList_RowDataBound"
                                CssClass="table table-progress-bar-striped table-fc-state-hover table-table-condensed"
                                AllowSorting="true" OnSorting="gvDonationList_Sorting">
                                <RowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="DisplayWorth" HeaderText="Worth" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="DonationComments" HeaderText="Comments" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="QtyNeeded" HeaderText="Qty Needed" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="QuantityClaimed" HeaderText="Qty Claimed" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                                    <%--Edit and delete buttons--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" CommandArgument='<%# Eval("DonationID") %>' CommandName="CHANGE" Text='Edit' CssClass="btn btn-alert-primary btn-small option-Button" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" CommandArgument='<%# Eval("DonationID") %>' CommandName="REMOVE" OnClientClick="return confirm('Are you sure you want to delete this item?');" Text='Delete' CssClass="btn btn-alert-primary btn-small option-Button" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <asp:HiddenField ID="hidAdd1" runat="server" />
                <asp:HiddenField ID="hidAdd2" runat ="server" />
                <asp:HiddenField ID="hidCity" runat ="server" />
                <asp:HiddenField ID="hidState" runat ="server" />
                <asp:HiddenField ID="hidZip" runat ="server" />
                <asp:HiddenField ID="hidPhone" runat ="server" />
                <asp:HiddenField ID="hidEmail" runat ="server" />
                <asp:HiddenField ID="hidDefaultRewardUnitID" runat="server" />
                <asp:HiddenField ID="hidDefaultPoolDescription" runat="server" />
                <asp:HiddenField ID="hidStatusID" runat="server" />
                <asp:HiddenField ID="hidStatusDescription" runat="server" />
                <asp:HiddenField ID="hidDefaultAwardWhen" runat="server" />
                
            </div>
        </asp:Panel>

        <div class="margin20"></div>
    </div>
</asp:Content>
