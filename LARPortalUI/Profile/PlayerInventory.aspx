<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="PlayerInventory.aspx.cs" Inherits="LarpPortal.Profile.PlayerInventory" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PlayerInventoryStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style type="text/css">
        .TableTextBox {
            border: 1px solid black;
        }

        th, tr:nth-child(even) > td {
            background-color: #ffffff;
        }

        .CharInfoTable {
            border-collapse: collapse;
        }

            .CharInfoTable td {
                padding: 4px;
            }

        .checkbox label {
            padding-left: 0px !important;
        }

        .form-group input[type="checkbox"] {
            display: none;
        }

            .form-group input[type="checkbox"] + .btn-group > label span {
                width: 20px;
            }

                .form-group input[type="checkbox"] + .btn-group > label span:first-child {
                    display: none;
                }

                .form-group input[type="checkbox"] + .btn-group > label span:last-child {
                    display: inline-block;
                }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:first-child {
                display: inline-block;
            }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:last-child {
                display: none;
            }

        .wide-modal {
            min-width: 700px !important;
        }

        .modal-body div {
            border: 0px solid black;
        }
    </style>
</asp:Content>

<asp:Content ID="PlayerInventoryScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }
        function closeModal() {
            $('#myModal').hide();
        }

        function openItem(PlayerInventoryID, ItemName, Description, TypeID, Quantity, Size, Location, PowerNeeded, WillShare, Comments, ImageURL) {
            $('#modalItem').modal('show');

            var hidInventoryID = document.getElementById('<%= hidInventoryID.ClientID %>');
            if (hidInventoryID)
                hidInventoryID.value = PlayerInventoryID;

            var tbItemName = document.getElementById('<%= tbItemName.ClientID %>');
            if (tbItemName) {
                tbItemName.value = "";
                if (ItemName)
                    tbItemName.value = ItemName;
                tbItemName.focus();
            }

            var tbDescription = document.getElementById('<%= tbDescription.ClientID %>');
            if (tbDescription) {
                tbDescription.value = "";
                if (Description)
                    tbDescription.value = Description;
            }

            var ddlType = document.getElementById('<%= ddlType.ClientID %>');
            if (ddlType) {
                ddlType.options[0].selected = true;
                if (TypeID)
                    setSelectedValue(ddlType, TypeID);
            }

            var tbQuantity = document.getElementById('<%= tbQuantity.ClientID %>');
            if (tbQuantity) {
                tbQuantity.value = "";
                if (Quantity)
                    tbQuantity.value = Quantity;
            }

            var tbSize = document.getElementById('<%= tbSize.ClientID %>');
            if (tbSize) {
                tbSize.value = "";
                if (Size)
                    tbSize.value = Size;
            }

            var tbLocation = document.getElementById('<%= tbLocation.ClientID %>');
            if (tbLocation) {
                tbLocation.value = "";
                if (Location)
                    tbLocation.value = Location;
            }

            var ddlPowerNeeded = document.getElementById('<%= ddlPowerNeeded.ClientID %>');
            if (ddlPowerNeeded) {
                ddlPowerNeeded.options[0].selected = true;
                if (PowerNeeded)
                    setSelectedValue(ddlPowerNeeded, PowerNeeded);
            }

            var cbxWillShare = document.getElementById('<%= cbxWillShare.ClientID %>');
            if (cbxWillShare) {
                cbxWillShare.checked = false;
                if (WillShare)
                    if (WillShare == "True")
                        cbxWillShare.checked = true;
                    else
                        cbxWillShare.checked = false;
            }

            var imgPicture = document.getElementById('<%= imgPicture.ClientID %>');
            var btnRemovePicture = document.getElementById('<%= btnRemovePicture.ClientID %>');
            if (imgPicture)
                if (ImageURL) {
                    imgPicture.src = ImageURL;
                    btnRemovePicture.style.display = "";
                }
                else {
                    try {
                        btnRemovePicture.style.display = "none";
                        imgPicture.src = "http://via.placeholder.com/200x150";
                    }
                    catch (err) {
                        var t = err.message;
                    }
                }

            var tbComments = document.getElementById('<%= tbComments.ClientID %>');
            if (tbComments) {
                tbComments.value = "";
                if (Comments)
                    tbComments.value = Comments;
            }

            return false;
        }

        function openItemDelete(PlayerInventoryID, Description, Comments) {
            $('#modalItemDelete').modal('show');

            var hidDeleteInventoryID = document.getElementById('<%= hidDeleteInventoryID.ClientID %>');
            if (hidDeleteInventoryID)
                hidDeleteInventoryID.value = PlayerInventoryID;

            var lblDeleteDescription = document.getElementById('lblDeleteDescription');
            if (lblDeleteDescription)
                lblDeleteDescription.innerText = Description;

            var lblDeleteItemComments = document.getElementById('lblDeleteItemComments');
            if (lblDeleteItemComments)
                lblDeleteItemComments.innerText = Comments;

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

        function ClearImage() {
            try {
                var imgPicture = document.getElementById('<%= imgPicture.ClientID %>');
                imgPicture.src = "http://via.placeholder.com/200x150";

                var fuItemPicture = document.getElementById('<%= fuItemPicture.ClientID %>');
                fuItemPicture.innerHTML = fuItemPicture.innerHTML;
                //fuItemPicture.select();
                //var n = fuItemPicture.createTextRange();
                //n.execCommand('delete');
                fuItemPicture.focus();
            }
            catch (err) {
                var t = err.message;
            }
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="PlayerInventoryBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Player Inventory</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Player Inventory</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px;">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="false" GridLines="none"
                                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1" CssClass="table table-striped table-hover table-condensed">
                                                <Columns>
                                                    <asp:BoundField DataField="DescWithImage" HtmlEncode="false" ItemStyle-Width="30" HeaderText="" />
                                                    <asp:BoundField DataField="ItemName" HeaderText="Name" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                    <asp:BoundField DataField="InventoryTypeDesc" HeaderText="Type" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <asp:BoundField DataField="Size" HeaderText="Size" />
                                                    <asp:BoundField DataField="Location" HeaderText="Location" />
                                                    <asp:BoundField DataField="PowerNeeded" HeaderText="Power" />
                                                    <asp:BoundField DataField="WillShareImage" HeaderText="Will Share" HtmlEncode="false" ItemStyle-Width="20"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                                    <asp:BoundField DataField="PlayerComments" HeaderText="Comments" />
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px"
                                                                OnClientClick='<%# Eval("JavaScriptEdit") %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" Width="16"
                                                                OnClientClick='<%# Eval("JavaScriptDelete") %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 text-right">
                                            <asp:Button ID="btnAddInventoryItem" runat="server" Text="Add Item" CssClass="btn btn-primary"
                                                OnClientClick="openItem(-1, '', '', '', '', '', '', '', ''); return false;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalItem" role="dialog">
            <div class="modal-dialog wide-modal">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Item</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-8">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbItemName.ClientID %>">Name:</label>
                                            <asp:TextBox ID="tbItemName" runat="server" CssClass="form-control" TabIndex="1" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbDescription.ClientID %>">Description:</label>
                                            <asp:TextBox ID="tbDescription" runat="server" CssClass="form-control" TabIndex="2" />
                                        </div>
                                    </div>
                                    <div class="col-xs-12" style="margin-right: 15px;">
                                        <div class="form-group">
                                            <label for="<%= ddlType.ClientID %>">Type:</label>
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="3" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-4 text-center">
                                <asp:FileUpload ID="fuItemPicture" runat="server" CssClass="col-lg-12" />
                                <asp:Image ID="imgPicture" runat="server" CssClass="col-lg-12 PrePostPadding" ImageUrl="http://via.placeholder.com/200x150" TabIndex="-1" />
                                <asp:Button ID="btnRemovePicture" runat="server" CssClass="btn btn-primary" Text="Remove" TabIndex="-1" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <label for="<%= tbSize.ClientID %>">Size:</label>
                                    <asp:TextBox ID="tbSize" runat="server" CssClass="form-control" TabIndex="4" />
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <label for="<%= tbQuantity.ClientID %>">Quantity:</label>
                                    <asp:TextBox ID="tbQuantity" runat="server" CssClass="form-control" TabIndex="5" />
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <label for="<%= ddlPowerNeeded.ClientID %>">Power Needed:</label>
                                    <asp:DropDownList ID="ddlPowerNeeded" runat="server" CssClass="form-control" TabIndex="6">
                                        <asp:ListItem Text="No Power Needed" Value="No Power Needed" />
                                        <asp:ListItem Text="Batteries Required" Value="Batteries Required" />
                                        <asp:ListItem Text="Power Required" Value="Power Required" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-8">
                                <div class="form-group">
                                    <label for="<%= tbLocation.ClientID %>">Location:</label>
                                    <asp:TextBox ID="tbLocation" runat="server" CssClass="form-control" TabIndex="7" />
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="form-group pull-right" style="padding-top: 23px; margin-right: 0px;">
                                    <input type="checkbox" name="cbxWillShare" id="cbxWillShare" runat="server" tabindex="-1" />
                                    <div class="btn-group col-xs-12" style="padding-right: 0px;" tabindex="7">
                                        <label for="<%= cbxWillShare.ClientID %>" class="btn btn-default">
                                            <span class="glyphicon glyphicon-ok"></span>
                                            <span class="glyphicon glyphicon-unchecked"></span>
                                        </label>
                                        <label for="<%= cbxWillShare.ClientID %>" class="btn btn-default active">
                                            Will Share
                                   
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="form-group">
                                    <label for="<%= tbComments.ClientID %>">Comments:</label>
                                    <asp:TextBox ID="tbComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" TabIndex="8" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnCloseItem" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClientClick="return false;" TabIndex="9" />
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:HiddenField ID="hidInventoryID" runat="server" />
                                <asp:Button ID="btnSaveItem" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveItem_Click" TabIndex="10" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalItemDelete" role="dialog">
            <div class="modal-dialog wide-modal">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Item - Delete</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="form-group">
                                    <label class="col-xs-12">Description:</label>
                                    <span class="col-xs-12" id="lblDeleteDescription"></span>
                                    <label class="col-xs-12" style="padding-top: 20px;">Comments:</label>
                                    <span class="col-xs-12" id="lblDeleteItemComments"></span>
                                    <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <asp:Button ID="btnDeleteClose" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClientClick="return false;" />
                            </div>
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidDeleteInventoryID" runat="server" />
                            <asp:Button ID="btnDeleteItem" runat="server" Text="Delete Record" CssClass="btn btn-primary" OnClick="btnDeleteItem_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title text-center">Player Item</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblMessage" runat="server" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-primary">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidProfileID" runat="server" />
        <script>
            $('a[data-toggle="tooltip"]').tooltip({
                animated: 'fade',
                placement: 'bottom',
                html: true
            });
        </script>
        <div id="push"></div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

