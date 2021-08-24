<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="LarpPortal.Profile.Profile" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PlayerProfileStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
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

        .profilePictureSize {
            max-height: 250px;
            max-width: 250px;
        }

        .paddingLeftRight {
            padding-left: 25px;
            padding-right: 25px;
        }

        .centerPicture {
            display: block;
            margin-left: auto;
            margin-right: auto;
        }
    </style>



</asp:Content>
<asp:Content ID="PlayerProfileScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }

        function DisplaySexOther(SexDropDownList) {
            var Gender = SexDropDownList.options[SexDropDownList.selectedIndex].value;
            if (Gender != null) {
                var OtherBox = document.getElementById('<%= tbGenderOther.ClientID %>');
                if (Gender == 'O') {
                    OtherBox.style.visibility = 'visible';
                    OtherBox.focus();
                }
                else
                    OtherBox.style.visibility = 'hidden';
            }
        }

        function DisplayPhoneProvider(ddlPhoneType) {
            var PhoneType = ddlPhoneType.options[ddlPhoneType.selectedIndex].text;
            if (PhoneType != null) {
                var ddlEnterProvider = document.getElementById('<%= ddlEnterProvider.ClientID %>');
                var lblEnterProvider = document.getElementById("lblEnterProvider");
                if (PhoneType.startsWith("Mo")) {
                    ddlEnterProvider.style.visibility = 'visible';
                    lblEnterProvider.style.visibility = 'visible';
                    ddlEnterProvider.focus();
                }
                else {
                    ddlEnterProvider.style.visibility = 'hidden';
                    lblEnterProvider.style.visibility = 'hidden';
                }
            }
        }

        function openPhoneModal(PhoneID, AreaCode, PhoneNumber, Extension, Primary, PhoneType, PhoneProvider) {
            $('#modalPhoneNumber').modal('show');
            try {
                var hidEnterPhoneID = document.getElementById('<%= hidEnterPhoneID.ClientID %>');
                if (hidEnterPhoneID) {
                    hidEnterPhoneID.value = "-1";
                    if (PhoneID)
                        hidEnterPhoneID.value = PhoneID;
                }

                var tbEnterAreaCode = document.getElementById('<%= tbEnterAreaCode.ClientID %>');
                if (tbEnterAreaCode) {
                    tbEnterAreaCode.value = "";
                    if (AreaCode)
                        tbEnterAreaCode.value = AreaCode;
                    tbEnterAreaCode.focus();
                }

                var tbEnterPhoneNumber = document.getElementById('<%= tbEnterPhoneNumber.ClientID %>');
                if (tbEnterPhoneNumber) {
                    tbEnterPhoneNumber.value = "";
                    if (PhoneNumber)
                        tbEnterPhoneNumber.value = PhoneNumber;
                }

                var tbEnterExtension = document.getElementById('<%= tbEnterExtension.ClientID %>');
                if (tbEnterExtension) {
                    tbEnterExtension.value = "";
                    if (Extension)
                        tbEnterExtension.value = Extension;
                }

                var cbxEnterPrimary = document.getElementById('<%= cbxEnterPrimary.ClientID %>');
                if (cbxEnterPrimary) {
                    cbxEnterPrimary.checked = false;
                    if (Primary == "True")
                        cbxEnterPrimary.checked = true;
                }

                var ddlEnterPhoneType = document.getElementById('<%= ddlEnterPhoneType.ClientID %>');
                if (ddlEnterPhoneType) {
                    ddlEnterPhoneType.options[0].selected = true
                    if (PhoneType)
                        setSelectedValue(ddlEnterPhoneType, PhoneType);
                }

                var ddlEnterProvider = document.getElementById('<%= ddlEnterProvider.ClientID %>');
                if (ddlEnterProvider) {
                    ddlEnterProvider.options[0].selected = true;
                    if (PhoneProvider)
                        setSelectedValue(ddlEnterProvider, PhoneProvider);
                }

                DisplayPhoneProvider(ddlEnterPhoneType)
            }
            catch (err) {
                var t = err.message;
            }
            return false;
        }

        function openPhoneDeleteModal(PhoneID, AreaCode, PhoneNumber) {
            $('#modalPhoneNumberDelete').modal('show');
            try {
                var hidDeletePhoneID = document.getElementById('<%= hidDeletePhoneID.ClientID %>');
                if (hidDeletePhoneID) {
                    hidDeletePhoneID.value = "-1";
                    if (PhoneID)
                        hidDeletePhoneID.value = PhoneID;
                }

                var lblDeletePhoneNumber = document.getElementById('lblDeletePhoneNumber');
                if (lblDeletePhoneNumber) {
                    lblDeletePhoneNumber.value = "";
                    lblDeletePhoneNumber.innerText = "(" + AreaCode + ") " + PhoneNumber.substring(0, 3) + "-" + PhoneNumber.substring(3, 7);
                }
            }
            catch (err) {
                var t = err.message;
            }

            return false;
        }


        function openEMailModal(EMailID, EMailAddress, EMailType, Primary) {
            $('#modalEMail').modal('show');
            try {
                var hidEnterEMailID = document.getElementById('<%= hidEnterEMailID.ClientID %>');
                if (hidEnterEMailID) {
                    hidEnterEMailID.value = "-1";
                    if (EMailID)
                        hidEnterEMailID.value = EMailID;
                }

                var tbEnterEMailAddress = document.getElementById('<%= tbEnterEMailAddress.ClientID %>');
                if (tbEnterEMailAddress) {
                    tbEnterEMailAddress.value = "";
                    if (EMailAddress)
                        tbEnterEMailAddress.value = EMailAddress;
                    tbEnterEMailAddress.focus();
                }

                var ddlEnterEMailType = document.getElementById('<%= ddlEnterEMailType.ClientID %>');
                if (ddlEnterEMailType) {
                    ddlEnterEMailType.options[0].selected = true
                    if (EMailType)
                        setSelectedValue(ddlEnterEMailType, EMailType);
                }

                var cbxEnterEMailPrimary = document.getElementById('<%= cbxEnterEMailPrimary.ClientID %>');
                if (cbxEnterEMailPrimary) {
                    cbxEnterEMailPrimary.checked = false;
                    if (Primary == "True")
                        cbxEnterEMailPrimary.checked = true;
                }
            }
            catch (err) {
                var t = err.message;
            }
            return false;
        }

        function openEmailDeleteModal(EMailID, EMail) {
            $('#modalEMailDelete').modal('show');
            try {
                var hidDeleteEMailID = document.getElementById('<%= hidDeleteEMailID.ClientID %>');
                if (hidDeleteEMailID) {
                    hidDeleteEMailID.value = "-1";
                    if (EMailID)
                        hidDeleteEMailID.value = EMailID;
                }

                var lblDeleteEMail = document.getElementById('lblDeleteEMail');
                if (lblDeleteEMail) {
                    lblDeleteEMail.value = "";
                    lblDeleteEMail.innerText = EMail;
                }
            }
            catch (err) {
                var t = err.message;
            }

            return false;
        }

        function openAddressModal(AddressID, Address1, Address2, City, StateID, PostalCode, Country, AddressType, Primary) {
            $('#modalAddress').modal('show');
            try {
                var hidAddressID = document.getElementById('<%= hidAddressID.ClientID %>');
                if (hidAddressID) {
                    hidAddressID.value = "-1";
                    if (AddressID)
                        hidAddressID.value = AddressID;
                }

                var tbEnterAddress1 = document.getElementById('<%= tbEnterAddress1.ClientID %>');
                if (tbEnterAddress1) {
                    tbEnterAddress1.value = "";
                    if (Address1)
                        tbEnterAddress1.value = Address1;
                    tbEnterAddress1.focus();
                }

                var tbEnterAddress2 = document.getElementById('<%= tbEnterAddress2.ClientID %>');
                if (tbEnterAddress2) {
                    tbEnterAddress2.value = "";
                    if (Address2)
                        tbEnterAddress2.value = Address2;
                }

                var tbEnterCity = document.getElementById('<%= tbEnterCity.ClientID %>');
                if (tbEnterCity) {
                    tbEnterCity.value = "";
                    if (City)
                        tbEnterCity.value = City;
                }

                var ddlEnterState = document.getElementById('<%= ddlEnterState.ClientID %>');
                if (ddlEnterState) {
                    ddlEnterState.options[0].selected = true
                    if (StateID)
                        setSelectedValue(ddlEnterState, StateID);
                }

                var tbEnterZipCode = document.getElementById('<%= tbEnterZipCode.ClientID %>');
                if (tbEnterZipCode) {
                    tbEnterZipCode.value = "";
                    if (PostalCode)
                        tbEnterZipCode.value = PostalCode;
                }

                var tbEnterCountry = document.getElementById('<%= tbEnterCountry.ClientID %>');
                if (tbEnterCountry) {
                    tbEnterCountry.value = "";
                    if (Country)
                        tbEnterCountry.value = Country;
                }

                var ddlEnterAddressType = document.getElementById('<%= ddlEnterAddressType.ClientID %>');
                if (ddlEnterAddressType) {
                    ddlEnterAddressType.options[0].selected = true
                    if (AddressType)
                        setSelectedValue(ddlEnterAddressType, AddressType);
                }

                var cbxEnterAddressPrimary = document.getElementById('<%= cbxEnterAddressPrimary.ClientID %>');
                if (cbxEnterAddressPrimary) {
                    cbxEnterAddressPrimary.checked = false;
                    if (Primary == "True")
                        cbxEnterAddressPrimary.checked = true;
                }
            }
            catch (err) {
                var t = err.message;
            }

            return false;
        }

        function openAddressDeleteModal(AddressID, Address1, Address2, City, StateID, PostalCode, Country, AddressType, Primary) {
            $('#modalAddressDelete').modal('show');
            try {
                var hidDeleteAddressID = document.getElementById('<%= hidDeleteAddressID.ClientID %>');
                if (hidDeleteAddressID) {
                    hidDeleteAddressID.value = "-1";
                    if (AddressID)
                        hidDeleteAddressID.value = AddressID;
                }

                var DisplayMessage = Address1 + "<br>";
                if (Address2.length > 0)
                    DisplayMessage += Address2 + "<br>";
                DisplayMessage += City + " " + StateID + ", " + PostalCode;

                var lblDeleteAddress = document.getElementById('lblDeleteAddress');
                if (lblDeleteAddress) {
                    lblDeleteAddress.value = "";
                    lblDeleteAddress.innerHTML = DisplayMessage;
                }
            }
            catch (err) {
                var t = err.message;
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
<asp:Content ID="PlayerProfileBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Profile</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">User Profile</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-8">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3>Personal Information</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbFirstName.ClientID %>">First Name:</label>
                                            <asp:TextBox ID="tbFirstName" runat="server" CssClass="form-control" TabIndex="1" />
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbMiddleName.ClientID %>">Middle Name:</label>
                                            <asp:TextBox ID="tbMiddleName" runat="server" CssClass="form-control" TabIndex="2" />
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbLastName.ClientID %>">Last Name:</label>
                                            <asp:TextBox ID="tbLastName" runat="server" CssClass="form-control" TabIndex="3" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= ddlGender.ClientID %>">Gender:</label>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-12">
                                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control" TabIndex="4">
                                                        <asp:ListItem Text="" Value="" />
                                                        <asp:ListItem Text="Male" Value="M" />
                                                        <asp:ListItem Text="Female" Value="F" />
                                                        <asp:ListItem Text="Other" Value="O" />
                                                    </asp:DropDownList>

                                                </div>
                                                <div class="col-md-6 col-sm-12">
                                                    <asp:TextBox ID="tbGenderOther" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="gender">Date of Birth:</label>
                                            <div class="inline">
                                                <input id="tbBDMM" type="text" runat="server" class="form-control" placeholder="MM" maxlength="2" tabindex="5" style="width: 3.5em;" />
                                                <input id="tbBDDD" type="text" runat="server" class="form-control" placeholder="DD" maxlength="2" tabindex="6" style="width: 3.5em;" />
                                                <input id="tbBDYYYY" type="text" runat="server" class="form-control" placeholder="YYYY" maxlength="4" tabindex="7" style="width: 4.5em;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbForumName.ClientID %>">Forum Name:</label>
                                            <asp:TextBox ID="tbForumName" runat="server" CssClass="form-control" TabIndex="8" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="<%= tbPenName.ClientID %>">Pronouns:</label>
                                            <asp:TextBox ID="tbPenName" runat="server" CssClass="form-control" TabIndex="9" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="<%= tbNickName.ClientID %>">Nick Name:</label>
                                            <asp:TextBox ID="tbNickName" runat="server" CssClass="form-control" TabIndex="10" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="<%= tbUserName.ClientID %>">User Name:</label>
                                            <asp:TextBox ID="tbUserName" runat="server" CssClass="form-control" TabIndex="11" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3>Emergency Contact Info</h3>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <asp:Image ID="imgPlayerImage" runat="server" CssClass="profilePictureSize centerPicture" />
                                </div>
                                <div class="row">
                                    To add a profile picture, use the buttons below.
                                </div>
                                <div class="row">
                                    <asp:FileUpload ID="ulFile" runat="server" CssClass="form-control" />
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-lg-6">
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary col-sm-12" OnClick="btnSavePicture_Click" />
                                    </div>
                                    <div class="col-lg-6 col-sm-12">
                                        <asp:Button ID="btnClearPicture" runat="server" Text="Clear Picture" CssClass="btn btn-primary col-sm-12" OnClick="btnClearPicture_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= tbEmergencyName.ClientID %>">Name:</label>
                                    <asp:TextBox ID="tbEmergencyName" runat="server" CssClass="form-control" TabIndex="13" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="<%= tbEmergencyPhone.ClientID %>">Phone Number:</label>
                                    <asp:TextBox ID="tbEmergencyPhone" runat="server" CssClass="form-control" TabIndex="14" />
                                </div>
                            </div>
                            <div class="col-md-4 text-right" style="padding-top: 23px;">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveProfile_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Addresses</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="false" GridLines="none"
                                    OnRowDataBound="gvAddresses_RowDataBound" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" CssClass="table table-bordered table-striped table-hover table-responsive">
                                    <RowStyle VerticalAlign="Bottom" />
                                    <Columns>
                                        <asp:BoundField DataField="Address1" HeaderText="Address 1" />
                                        <asp:BoundField DataField="Address2" HeaderText="Address 2" />
                                        <asp:BoundField DataField="City" HeaderText="City" />
                                        <asp:BoundField DataField="StateID" HeaderText="State" />
                                        <asp:BoundField DataField="PostalCode" HeaderText="Postal/Zip Code" />
                                        <asp:BoundField DataField="Country" HeaderText="Country" />
                                        <asp:BoundField DataField="AddressType" HeaderText="Address Type" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                        <asp:TemplateField HeaderText="Primary" ItemStyle-Width="30px" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("IsPrimary").ToString() == "True" ? "Yes" : "No" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right" ItemStyle-Width="46px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" ToolTip="Edit the address"
                                                    OnClientClick='<%# string.Format("return openAddressModal(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\"); return false;",
                                                        Eval("AddressID"), Eval("Address1"), Eval("Address2"), Eval("City"), Eval("StateID"), Eval("PostalCode"),
                                                        Eval("Country"), Eval("AddressType"), Eval("IsPrimary")) %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" ToolTip="Delete the address"
                                                    OnClientClick='<%# string.Format("return openAddressDeleteModal(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\"); return false;",
                                                        Eval("AddressID"), Eval("Address1"), Eval("Address2"), Eval("City"), Eval("StateID"), Eval("PostalCode"),
                                                        Eval("Country"), Eval("AddressType"), Eval("IsPrimary")) %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnAddAddress" runat="server" Text="Add Address" CssClass="btn btn-primary" OnClientClick="openAddressModal(); return false;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Phone Numbers</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:GridView ID="gvPhoneNumbers" runat="server" AutoGenerateColumns="false" GridLines="none"
                                    OnRowDataBound="gvPhoneNumbers_RowDataBound" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" CssClass="table table-bordered table-striped table-hover table-responsive">
                                    <RowStyle VerticalAlign="Middle" />
                                    <Columns>
                                        <asp:BoundField DataField="AreaCode" HeaderText="Area Code" />
                                        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                                        <asp:BoundField DataField="Extension" HeaderText="Extension" />
                                        <asp:BoundField DataField="PhoneType" HeaderText="Type" />
                                        <asp:TemplateField HeaderText="Primary" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("IsPrimary").ToString() == "True" ? "Yes" : "No" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Provider" HeaderText="Provider" ItemStyle-Wrap="false" HeaderStyle-CssClass="GridViewHeader" ItemStyle-CssClass="GridViewItem" />
                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right" ItemStyle-Width="46px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" ToolTip="Edit the address"
                                                    OnClientClick='<%# string.Format("return openPhoneModal(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\"); return false;",
                                                        Eval("PhoneNumberID"), Eval("AreaCode"), Eval("PhoneNumber"), Eval("Extension"), Eval("IsPrimary"), Eval("PhoneTypeID"),
                                                        Eval("ProviderID")) %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" ToolTip="Delete the address"
                                                    OnClientClick='<%# string.Format("return openPhoneDeleteModal(\"{0}\", \"{1}\", \"{2}\"); return false;",
                                                        Eval("PhoneNumberID"), Eval("AreaCode"), Eval("PhoneNumber")) %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnAddPhoneNumber" runat="server" Text="Add Phone Number" CssClass="btn btn-primary" OnClientClick="openPhoneModal(); return false;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Email Addresses</div>
                    <!-- /.panel-heading -->
                    <div class="panel-body">

                        <div class="row">
                            <div class="col-xs-12">

                                <asp:GridView ID="gvEmails" runat="server" AutoGenerateColumns="false" Width="100%" GridLines="none"
                                    OnRowDataBound="gvEmails_RowDataBound" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" CssClass="table table-bordered table-striped table-hover table-responsive">
                                    <RowStyle VerticalAlign="Middle" />
                                    <Columns>
                                        <asp:BoundField DataField="EMailAddress" HeaderText="Email Address" />
                                        <asp:BoundField DataField="EMailType" HeaderText="Type" ItemStyle-Wrap="false" />
                                        <asp:TemplateField HeaderText="Primary" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("IsPrimary").ToString() == "True" ? "Yes" : "No" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right" ItemStyle-Width="46px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" ToolTip="Edit the EMail"
                                                    OnClientClick='<%# string.Format("return openEMailModal(\"{0}\", \"{1}\", \"{2}\", \"{3}\"); return false;",
                                                        Eval("EMailID"), Eval("EmailAddress"), Eval("EmailTypeID"), Eval("IsPrimary")) %>'><i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" ToolTip="Delete the EMail"
                                                    OnClientClick='<%# string.Format("return openEmailDeleteModal(\"{0}\", \"{1}\"); return false;",
                                                        Eval("EMailID"), Eval("EMailAddress")) %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12 text-right">
                                <asp:Button ID="btnAddEmail" runat="server" Text="Add EMail" CssClass="btn btn-primary" OnClientClick='openEMailModal("-1"); return false;' />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--        <div class="row">
            <div class="col-lg-12">
                <p class="text-right">
                    <input type="submit" value="Save Profile" class="btn btn-lg btn-primary">
                </p>
            </div>
        </div>--%>
        <div id="push"></div>
    </div>



    <div class="modal fade in" id="modalPhoneNumber" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">Phone Number</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-3 form-group">
                            <label for="<%= tbEnterAreaCode.ClientID %>">Area Code</label>
                            <asp:TextBox ID="tbEnterAreaCode" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-lg-6 form-group">
                            <label for="<%= tbEnterPhoneNumber.ClientID %>">Phone Number</label>
                            <asp:TextBox ID="tbEnterPhoneNumber" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-lg-3 form-group">
                            <label for="<%= tbEnterExtension.ClientID %>">Extension</label>
                            <asp:TextBox ID="tbEnterExtension" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3 form-group" style="padding-top: 23px; padding-left: 0px; padding-right: 0px;">
                            <input type="checkbox" name="cbxEnterPrimary" id="cbxEnterPrimary" runat="server" />
                            <div class="btn-group col-xs-12" style="padding-right: 0px;">
                                <label for="<%= cbxEnterPrimary.ClientID%>" class="btn btn-default">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span class="glyphicon glyphicon-unchecked"></span>
                                </label>
                                <label for="<%= cbxEnterPrimary.ClientID%>" class="btn btn-default active">
                                    Primary
                                   
                                </label>
                            </div>
                        </div>
                        <div class="col-lg-3 form-group">
                            <label for="<%= ddlEnterPhoneType.ClientID %>">Phone Type</label>
                            <asp:DropDownList ID="ddlEnterPhoneType" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-xs-6 form-group">
                            <label id="lblEnterProvider" for="<%= ddlEnterProvider.ClientID %>">Provider</label>
                            <asp:DropDownList ID="ddlEnterProvider" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="col-sm-6 text-left">
                        <asp:Button ID="btnClosePhoneNumber" runat="server" CssClass="btn btn-primary" Text="Close" />
                    </div>
                    <div class="col-sm-6 text-right">
                        <asp:HiddenField ID="hidEnterPhoneID" runat="server" />
                        <asp:Button ID="btnSavePhone" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSavePhone_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalPhoneNumberDelete" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Phone Number - Delete</h3>
                </div>

                <div class="modal-body">
                    <div class="container-fluid" style="margin-left: -30px; margin-right: -30px">
                        <label class="col-xs-12">Phone Number</label>
                        <span class="col-xs-12" id="lblDeletePhoneNumber"></span>
                        <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="col-sm-6 text-left" style="padding-left: 0px;">
                        <asp:Button ID="btnCancelDeletePhoneNumber" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                    </div>
                    <div class="col-sm-6 text-right  padding-right-0">
                        <asp:HiddenField ID="hidDeletePhoneID" runat="server" />
                        <asp:Button ID="btnDeletePhone" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeletePhone_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade in" id="modalEMail" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">EMail</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label for="<%= tbEnterEMailAddress.ClientID %>">EMail Address</label>
                            <asp:TextBox ID="tbEnterEMailAddress" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 form-group">
                            <label for="<%= ddlEnterEMailType.ClientID %>">EMail Type</label>
                            <asp:DropDownList ID="ddlEnterEMailType" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3 form-group" style="padding-left: 0px; padding-right: 0px;">
                            <input type="checkbox" name="cbxEnterEMailPrimary" id="cbxEnterEMailPrimary" runat="server" />
                            <div class="btn-group col-xs-12" style="padding-right: 0px;">
                                <label for="<%= cbxEnterEMailPrimary.ClientID%>" class="btn btn-default">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span class="glyphicon glyphicon-unchecked"></span>
                                </label>
                                <label for="<%= cbxEnterEMailPrimary.ClientID%>" class="btn btn-default active">
                                    Primary
                                   
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="row">
                        <div class="col-sm-6 text-left">
                            <asp:Button ID="btnCloseEnterEMail" runat="server" CssClass="btn btn-primary" Text="Close" />
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidEnterEMailID" runat="server" />
                            <asp:Button ID="btnSaveEMail" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveEMail_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalEMailDelete" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">EMail - Delete</h3>
                </div>

                <div class="modal-body">
                    <div class="container-fluid" style="margin-left: -30px; margin-right: -30px">
                        <label class="col-xs-12">EMail Address</label>
                        <span class="col-xs-12" id="lblDeleteEMail"></span>
                        <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="row">
                        <div class="col-sm-6 text-left">
                            <asp:Button ID="btnCancelDeleteEMail" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidDeleteEMailID" runat="server" />
                            <asp:Button ID="btnDeleteEMail" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteEMail_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalAddress" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">Address</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label for="<%= tbEnterAddress1.ClientID %>">Address 1</label>
                            <asp:TextBox ID="tbEnterAddress1" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label for="<%= tbEnterAddress2.ClientID %>">Address 2</label>
                            <asp:TextBox ID="tbEnterAddress2" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-4 form-group">
                            <label for="<%= tbEnterCity.ClientID %>">City</label>
                            <asp:TextBox ID="tbEnterCity" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 form-group">
                            <label for="<%= ddlEnterState.ClientID %>">State</label>
                            <asp:DropDownList ID="ddlEnterState" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 form-group">
                            <label for="<%= tbEnterZipCode.ClientID %>">Zip Code</label>
                            <asp:TextBox ID="tbEnterZipCode" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 form-group">
                            <label for="<%= tbEnterCountry.ClientID %>">Country</label>
                            <asp:TextBox ID="tbEnterCountry" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 form-group">
                            <label for="<%= ddlEnterAddressType.ClientID %>">Type</label>
                            <asp:DropDownList ID="ddlEnterAddressType" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 form-group" style="padding-top: 23px; padding-left: 0px; padding-right: 0px;">
                            <input type="checkbox" name="cbxEnterAddressPrimary" id="cbxEnterAddressPrimary" runat="server" />
                            <div class="btn-group col-xs-12" style="padding-right: 0px;">
                                <label for="<%= cbxEnterAddressPrimary.ClientID%>" class="btn btn-default">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span class="glyphicon glyphicon-unchecked"></span>
                                </label>
                                <label for="<%= cbxEnterAddressPrimary.ClientID%>" class="btn btn-default active">
                                    Primary
                                   
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="row">
                        <div class="col-sm-6 text-left">
                            <asp:Button ID="btnCloseEnterAddress" runat="server" CssClass="btn btn-primary" Text="Close" />
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidAddressID" runat="server" />
                            <asp:Button ID="btnSaveAddress" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveAddress_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalAddressDelete" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Address - Delete</h3>
                </div>

                <div class="modal-body">
                    <div class="container-fluid" style="margin-left: -30px; margin-right: -30px">
                        <label class="col-xs-12">Address</label>
                        <span class="col-xs-12" id="lblDeleteAddress"></span>
                        <span class="col-xs-12" style="padding-top: 40px">Are you sure you want to delete this record ?</span>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="row">
                        <div class="col-sm-6 text-left">
                            <asp:Button ID="btnCancelDeleteAddress" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidDeleteAddressID" runat="server" />
                            <asp:Button ID="btnDeleteAddress" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteAddress_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="myModal" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Profile Demographics</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidNumOfPhones" runat="server" />
    <asp:HiddenField ID="hidNumOfEMails" runat="server" />
    <asp:HiddenField ID="hidNumOfAddresses" runat="server" />

    <!-- /#page-wrapper -->
</asp:Content>

