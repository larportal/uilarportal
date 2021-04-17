<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="CharInfo.aspx.cs" Inherits="LarpPortal.Character.CharInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharInfoStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>

<asp:Content ID="CharInfoScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
        function ddlRebuildSetVisible() {
            var ddlAllowRebuild = document.getElementById("<%=ddlAllowRebuild.ClientID %>");
            var tbRebuildToDate = document.getElementById("<%=tbRebuildToDate.ClientID %>");
            tbRebuildToDate.style.display = "none";
            if (ddlAllowRebuild)
                if (ddlAllowRebuild.options[ddlAllowRebuild.selectedIndex].value == "Y") {
                    tbRebuildToDate.style.display = "inline";
                    //lblExpiresOn.style.display = "inline";
                }
            return false;
        }

        //  JBradshaw  7/11/2016    Request #1286     Changed over to bootstrap popup.
        function openMessage() {
            $('#modalMessage').modal('show');
        }
        function closeMessage() {
            $('#modelMessage').hide();
        }

        function openError() {
            $('#modalError').modal('show');
        }
        function closeError() {
            $('#modelError').hide();
        }

        function openDeath(CharacterDeathID, DeathDate, Permed, Comments, StaffComments) {
            $('#modalEditUpdateDeath').modal('show');

            var tbDeathDate = document.getElementById('<%= tbDeathDate.ClientID %>');
            tbDeathDate.value = DeathDate;

            var cbxPerm = document.getElementById('<%= cbxDeathPerm.ClientID %>');
            var txtPermed = Permed.charAt(0);
            if (txtPermed == 'T') {
                cbxDeathPerm.checked = true;
            }

            var tbComments = document.getElementById('<%= tbDeathComments.ClientID %>');
            if (Comments)
                tbComments.value = Comments;
            else
                tbComments.value = "";

            var tbStaffComments = document.getElementById('<%= tbDeathStaffComments.ClientID %>');
            if (StaffComments)
                tbStaffComments.value = StaffComments;
            else
                tbStaffComments.value = "";

            var hidDeathID = document.getElementById('<%= hidDeathID.ClientID %>');

            if (hidDeathID) {
                hidDeathID.value = CharacterDeathID;
            }

            return false;
        }
        function closeDeath() {
            $('#modalEditUpdateDeath').hide();
            return false;
        }

        function openActor(CharacterActorID, UserName, Comments, StaffComments, StartDate, EndDate) {
            $('#modalEditUpdateActor').modal('show');

            var tbActorStartDate = document.getElementById('<%= tbActorStartDate.ClientID %>');
            if (StartDate) {
                tbActorStartDate.value = StartDate;
            }
            else {
                tbActorStartDate.value = "";
            }

            var tbActorEndDate = document.getElementById('<%= tbActorEndDate.ClientID %>');
            if (EndDate) {
                tbActorEndDate.value = EndDate;
            }
            else {
                tbActorEndDate.value = "";
            }

            var tbComments = document.getElementById('<%= tbActorComments.ClientID %>');
            tbComments.value = Comments;

            var tbStaffComments = document.getElementById('<%= tbActorStaffComments.ClientID %>');
            tbStaffComments.value = StaffComments;

            var hidActorID = document.getElementById('<%= hidActorID.ClientID %>');

            var ddlActorPlayer = document.getElementById('<%= ddlActorName.ClientID %>');
            if (UserName)
                setSelectedValue(ddlActorPlayer, UserName);

            if (hidActorID) {
                hidActorID.value = CharacterActorID;
            }

            return false;
        }
        function closeActor() {
            $('#modalEditUpdateActor').hide();
            return false;
        }

        function openDeleteDeath(CharacterDeathID, DeathDate) {
            $('#modalDeleteDeath').modal('show');

            var hidDeleteDeathID = document.getElementById('<%= hidDeleteDeathID.ClientID %>');
            if (hidDeleteDeathID)
                hidDeleteDeathID.value = CharacterDeathID;

            var lblDeleteDeathMessage = document.getElementById('<%= lblDeleteDeathMessage.ClientID %>');
            if (lblDeleteDeathMessage)
                if (DeathDate)
                    lblDeleteDeathMessage.innerText = "Are you sure you want to delete death on " + DeathDate + " ?";
                else
                    lblDeleteDeathMessage.innerText = "Are you sure you want to delete this death ?";
            return false;
        }


        function openDeleteActor(CharacterActorID, ActorName) {
            $('#modalDeleteActor').modal('show');

            var hidDeleteActorID = document.getElementById('<%= hidDeleteActorID.ClientID %>');
            if (hidDeleteActorID)
                hidDeleteActorID.value = CharacterActorID;

            var lblDeleteActorMessage = document.getElementById('<%= lblDeleteActorMessage.ClientID %>');
            if (lblDeleteActorMessage)
                if (ActorName)
                    lblDeleteActorMessage.innerText = "Are you sure you want to delete actor " + ActorName + " ?";
                else
                    lblDeleteActorMessage.innerText = "Are you sure you want to delete this actor ?";
            return false;
        }

        function openDeleteDesc(DescID, Descriptor, DescValue) {
            $('#modalDeleteDesc').modal('show');

            var hidDescID = document.getElementById('<%= hidDescID.ClientID %>');
            if (hidDescID)
                hidDescID.value = DescID;

            var lblDeleteDescMessage = document.getElementById('<%= lblDeleteDescMessage.ClientID %>');
            if (lblDeleteDescMessage)
                lblDeleteDescMessage.innerText = "Are you sure you want to delete " + Descriptor + " - " + DescValue + " ?";
            return false;
        }

        function setSelectedValue(selectObj, valueToSet) {
            for (var i = 0; i < selectObj.options.length; i++) {
                if (selectObj.options[i].text == valueToSet) {
                    selectObj.options[i].selected = true;
                    return;
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="CharInfo" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="header-background-image">
                        <h1>Character Info</h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-10 margin20">
                    <div class="input-group">
                        <CharSelector:Select ID="oCharSelect" runat="server" />
                    </div>
                </div>
                <div class="text-right">
                    <asp:Button ID="btn_SaveTop" runat="server" Text="Save" CssClass="btn btn-lg btn-primary" OnClick="btnSave_Click" />
                </div>
                <%--                <div class="col-sm-4" id="campaignUpdate">
                    <ul class="list-inline">
                        <li><strong>Campaign:</strong> Madrigal</li>
                        <li><strong>Last Update:</strong> 5/25/2017</li>
                    </ul>
                </div>--%>
            </div>
            <div class="row">
                <div class="col-md-12 bg-classes">
                    <p class="bg-info text-center">Fill in information to describe your character. Some items are automatically updated after events.</p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Character Information</div>
                        <div class="panel-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <label for="">Character</label>
                                                        <div class="row">
                                                            <div class="col-md-4 col-xs-12">
                                                                <asp:TextBox ID="tbFirstName" runat="server" CssClass="form-control" TabIndex="1" />
                                                            </div>
                                                            <div class="col-md-4 col-xs-12">
                                                                <asp:TextBox ID="tbMiddleName" runat="server" CssClass="form-control" TabIndex="2" />
                                                            </div>
                                                            <div class="col-md-4 col-xs-12">
                                                                <asp:TextBox ID="tbLastName" runat="server" CssClass="form-control" TabIndex="3" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <label for="">AKA</label>
                                                        <asp:TextBox ID="tbAKA" runat="server" CssClass="form-control" TabIndex="4" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <div class="row">
                                                            <div class="col-xs-8" runat="server" id="divCharType">
                                                                <label for="">Type</label>
                                                                <asp:TextBox ID="tbCharType" runat="server" Enabled="false" Visible="false" CssClass="form-control" TabIndex="5" />
                                                                <asp:DropDownList ID="ddlCharType" runat="server" Visible="true" CssClass="form-control" TabIndex="5">
                                                                    <asp:ListItem Text="PC" Value="1" />
                                                                    <asp:ListItem Text="NPC" Value="2" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-xs-4" runat="server" id="divVisibleRelationships">
                                                                <label for="">Visible On Relationships</label>
                                                                <asp:DropDownList ID="ddlVisible" runat="server" Visible="true" CssClass="form-control">
                                                                    <asp:ListItem Text="Yes" Value="1" />
                                                                    <asp:ListItem Text="No" Value="0" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <div class="col-xs-4" style="padding-left: 0px;">
                                                            <label for="<%= tbDOB.ClientID %>">Date of Birth</label>
                                                            <asp:TextBox ID="tbDOB" runat="server" CssClass="form-control" TabIndex="6" />
                                                        </div>
                                                        <div class="col-xs-8" style="padding-right: 0px;" runat="server" id="divAllowRebuild">
                                                            <label for="<%= ddlAllowRebuild.ClientID %>">Allow Character Rebuild</label>
                                                            <div class="row">
                                                                <div class="col-xs-6">
                                                                    <asp:DropDownList ID="ddlAllowRebuild" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Text="No" Value="N" Selected="True" />
                                                                        <asp:ListItem Text="Yes" Value="Y" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-xs-6">
                                                                    <asp:TextBox ID="tbRebuildToDate" runat="server" Columns="10" MaxLength="10" CssClass="form-control" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">Birthplace</label>
                                                    <asp:TextBox ID="tbBirthPlace" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">Status</label>
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="form-control" Visible="false" TabIndex="8" />
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="8" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">Home</label>
                                                    <asp:TextBox ID="tbHome" runat="server" CssClass="form-control" TabIndex="9" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">Last Event</label>
                                                    <asp:TextBox ID="tbLastEvent" runat="server" ReadOnly="true" CssClass="form-control" TabIndex="-1" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">Primary Team</label>
                                                    <asp:TextBox ID="tbTeam" runat="server" CssClass="form-control" Visible="false" Enabled="false" TabIndex="-1" />
                                                    <asp:DropDownList ID="ddlTeamList" runat="server" CssClass="form-control" TabIndex="11" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for=""># of Deaths</label>
                                                    <asp:TextBox ID="tbNumOfDeaths" runat="server" CssClass="form-control" Enabled="false" TabIndex="-1" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">Race</label>
                                                    <asp:TextBox ID="tbRace" runat="server" CssClass="form-control" Visible="false" Enabled="false" TabIndex="-1" />
                                                    <asp:DropDownList ID="ddlRace" runat="server" CssClass="form-control" TabIndex="12" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="">DOD</label>
                                                    <asp:TextBox ID="tbDOD" runat="server" CssClass="form-control" Enabled="false" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="gutter">
                                            <div class="form-group">
                                                <p>
                                                    <asp:Image ID="imgCharacterPicture" runat="server" ImageUrl="~/images/profile-photo.png" AlternateText="Character Picture" CssClass="img-thumbnail" />
                                                </p>
                                                <p>To add a profile picture, use the buttons below.</p>
                                                <asp:FileUpload ID="ulFile" runat="server" CssClass="form-control" />
                                                <%--                                            <button class="btn btn-sm btn-default" type="submit" tabindex="15">Choose File</button>
                                            <button class="btn btn-sm btn-primary" type="submit" tabindex="16">Upload</button>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <asp:UpdatePanel ID="upDescriptors" runat="server">
                    <ContentTemplate>
                        <div class="col-md-6" runat="server" id="divNonCost">
                            <div class="panel panel-default">
                                <div class="panel-heading">Non Cost Character Descriptions</div>
                                <div class="panel-body">
                                    <h5>Select criteria that describes your character.</h5>
                                    <asp:GridView ID="gvDescriptors" runat="server" AutoGenerateColumns="false" GridLines="None"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1" DataKeyNames="CharacterAttributesBasicID"
                                        CssClass="table table-bordered table-striped table-responsive">
                                        <EmptyDataRowStyle BackColor="Transparent" />
                                        <EmptyDataTemplate>
                                            <%--                                    <div class="row">--%>
                                            <div class="text-center" style="background-color: transparent;">You have no descriptors selected. Please select from the boxes below.</div>
                                            <%--                                    </div>--%>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:BoundField DataField="CharacterDescriptor" HeaderText="Character Descriptor" />
                                            <asp:BoundField DataField="DescriptorValue" HeaderText="Value" />
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="40" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" ToolTip="Delete the item" CausesValidation="false" Width="16px"
                                                        OnClientClick='<%# string.Format("return openDeleteDesc({0}, \"{1}\", \"{2}\"); return false;",
                                                            Eval("CharacterAttributesBasicID"), Eval("CharacterDescriptor"), Eval("DescriptorValue")) %>'>
                                                        <i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <div class="input-group margin10 col-md-12" id="divCharDev" runat="server">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <label for="<%= ddlDescriptor.ClientID %>">Character Descriptor</label>
                                                        <asp:DropDownList ID="ddlDescriptor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDescriptor_SelectedIndexChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="controls">
                                                        <label for="<%= ddlName.ClientID %>">Name</label>
                                                        <asp:DropDownList ID="ddlName" runat="server" CssClass="form-control" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 text-right">
                                                <asp:Button ID="btnAddDesc" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddDesc_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-md-4" runat="server" id="divDeaths">
                    <div class="panel panel-default">
                        <div class="panel-heading">Character Deaths</div>
                        <div class="panel-body">
                            <asp:GridView runat="server" ID="gvDeaths" AutoGenerateColumns="false" GridLines="None" OnDataBound="gvDeaths_DataBound"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                DataKeyNames="CharacterDeathID" CssClass="table table-striped table-hover table-condensed col-sm-12">
                                <Columns>
                                    <asp:BoundField DataField="DeathDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date of Death" />
                                    <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                    <asp:CheckBoxField DataField="DeathPermanent" HeaderText="Permed" ReadOnly="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px" Height="16px"
                                                CommandArgument='<%# Eval("CharacterDeathID") %>'
                                                OnClientClick='<%# string.Format("return openDeath({0}, \"{1:MM/dd/yyyy}\", \"{2}\", \"{3}\", \"{4}\");", 
                                                                    Eval("CharacterDeathID"), Eval("DeathDate"), Eval("DeathPermanent"), Eval("Comments"), Eval("StaffComments")) %>'>
                                                <i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteDeathStaff"
                                                CommandArgument='<%# Eval("CharacterDeathID") %>' Width="16px" Height="16px"
                                                OnClientClick='<%# string.Format("return openDeleteDeath({0}, \"{1:MM/dd/yyyy}\"); return false;", 
                                                                    Eval("CharacterDeathID"), Eval("DeathDate")) %>'>
                                            <i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="col-sm-12 text-right" style="padding-left: 0px; padding-right: 0px;" runat="server" id="divAddDeath">
                                <asp:Button ID="btnAddNewDeath" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClientClick="return openDeath(-1, '', '', '');" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="divActors">
                    <div class="panel panel-default">
                        <div class="panel-heading">Character Actors</div>
                        <div class="panel-body">
                            <asp:GridView runat="server" ID="gvActors" AutoGenerateColumns="false" GridLines="None" OnDataBound="gvActors_DataBound"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                DataKeyNames="CharacterActorID" CssClass="table table-striped table-hover table-condensed col-sm-12">
                                <Columns>
                                    <asp:BoundField DataField="PlayerName" HeaderText="Player Name" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="StartDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Start Date" />
                                    <asp:BoundField DataField="EndDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="End Date" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblComments" runat="server" Text='<%# Eval("Comments") %>' ToolTip='<%# Eval("StaffComments") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>Comments</HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="false" Width="16px"
                                                OnClientClick='<%# string.Format("return openActor({0}, \"{1}\", \"{2}\", \"{3}\", \"{4:MM/dd/yyyy}\", \"{5:MM/dd/yyyy}\");",
                                                                    Eval("CharacterActorID"), Eval("PlayerName"), Eval("Comments"), Eval("StaffComments"), Eval("StartDate"), Eval("EndDate")) %>'>
                                                <i class="fa fa-pencil-square-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="false"
                                                Width="16px"
                                                OnClientClick='<%# string.Format("return openDeleteActor({0}, \"{1}\"); return false;",
                                                                    Eval("CharacterActorID"), Eval("loginUserName")) %>'><i class="fa fa-trash-o fa-lg fa-fw" aria-hidden="true"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="col-lg-8 col-xs-12">
                                <asp:Label ID="lblDateProblem" runat="server" Text="There is a problem with the dates." Font-Bold="true" Font-Italic="true" ForeColor="Red" />
                            </div>
                            <div class="col-sm-4 text-right" style="padding-left: 0px; padding-right: 0px;" runat="server" id="divAddActor">
                                <asp:Button ID="btnAddActor" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClientClick="return openActor(-1, '', '', '');" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-4" runat="server" id="divPlayer">
                    <div class="panel panel-default">
                        <div class="panel-heading">Played By</div>
                        <div class="panel-body">
                            <asp:Label ID="lblPlayer" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row col-sm-12" runat="server" id="divHiddenSkills">
                <div class="panel panel-default ">
                    <div class="panel-heading">Hidden Skills</div>
                    <div class="panel-body">
                        <asp:GridView runat="server" ID="gvHiddenSkillAccess" AutoGenerateColumns="false" GridLines="None"
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                            CssClass="table table-striped table-hover table-condensed col-sm-12">
                            <Columns>
                                <asp:BoundField DataField="SkillNamePath" HeaderText="Skill Path" HeaderStyle-Wrap="false" />
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" Width="70" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        Open Skill
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbxHasSkill" runat="server" Checked='<%# ((int)Eval("HasAccess") == 0) ? false : true %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hidCampaignSkillNodeID" runat="server" Value='<%# Eval("CampaignSkillNodeID") %>' />
                                        <asp:HiddenField ID="hidHadOriginally" runat="server" Value='<%# Eval("HasAccess") %>' />
                                        <asp:HiddenField ID="hidCampaignSkillAccessID" runat="server" Value='<%# Eval("CampaignSkillAccessID") %>' />
                                        <asp:HiddenField ID="hidSkillName" runat="server" Value='<%# Eval("SkillName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>




            <%--                <div class="col-sm-8" runat="server" id="divHiddenSkills">
                    <div class="panel panel-default">
                        <div class="panel-heading">Hidden Skill Access</div>
                        <div class="panel-body">
                            <asp:GridView runat="server" ID="gvHiddenSkillAccess" AutoGenerateColumns="false" GridLines="None"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
                                CssClass="table table-striped table-hover table-condensed col-sm-12">
                                <Columns>
                                    <asp:BoundField DataField="SkillName" HeaderText="Skill Path" HeaderStyle-Wrap="false" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Has Skill
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxHasSkill" runat="server" Checked='<%# Eval("AccesToSkill") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidCampaignSkillNodeID" runat="server" Value='<%# Eval("Comments") %>' />
                                            <asp:HiddenField ID="hisHadOriginally" runat="server" Value='<%# Eval("HasAccess") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>--%>
            </div>

            <div class="row" runat="server" id="divStaffComments">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Staff Comments</div>
                        <div class="panel-body">
                            <asp:TextBox ID="tbStaffComments" runat="server" TextMode="MultiLine" Rows="4" CssClass="col-sm-12" Height="75px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <p class="text-right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    </p>
                </div>
            </div>
        </div>
        <div class="divide30"></div>
        <div id="push"></div>
    </div>

    <!-- JBradshaw  7/11/2016    Request #1286     Changed over to bootstrap popup.  -->
    <div class="modal fade in" id="modalError" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Info</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalError" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCloseError" runat="server" Text="Close" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalDeleteDeath" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Info - Delete Death</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblDeleteDeathMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-6">
                            <asp:Button ID="btnDeleteDeathCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-xs-6 text-right">
                            <asp:HiddenField ID="hidDeleteDeathID" runat="server" />
                            <asp:Button ID="btnDeleteDeath" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteDeath_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalDeleteActor" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Info - Delete Actor</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblDeleteActorMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-6">
                            <asp:Button ID="btnDeleteActorCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-xs-6 text-right">
                            <asp:HiddenField ID="hidDeleteActorID" runat="server" />
                            <asp:Button ID="btnDeleteActor" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteActor_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalDeleteDesc" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Info - Delete Descriptor</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblDeleteDescMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-6 text-left">
                            <asp:Button ID="btnDeleteDescCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-xs-6 text-right">
                            <asp:HiddenField ID="hidDescID" runat="server" />
                            <asp:Button ID="btnDeleteDesc" runat="server" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteDesc_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">LARPortal Character Info</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Button ID="btnCloseMessage" runat="server" Text="Close" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalEditUpdateDeath" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Character Death</h3>
                </div>
                <div class="modal-body">
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3">Death Date:</div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:TextBox ID="tbDeathDate" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceDeathDate" runat="server" TargetControlID="tbDeathDate" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3">Perm:</div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:CheckBox ID="cbxDeathPerm" runat="server" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3">
                            Comments:<br />
                            (Visible<br />
                            to player)
                        </div>
                        <div class="col-sm-9" style="padding-left: 0px; margin-left: 0px;">
                            <asp:TextBox ID="tbDeathComments" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="500" Rows="5" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3">
                            Staff Comments:<br />
                            (Not visible to player)
                        </div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:TextBox ID="tbDeathStaffComments" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="500" Rows="5" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Button ID="btnCancelCharDeath" OnClientClick="closeDeath(); return false;" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidDeathID" runat="server" />
                            <asp:Button ID="btnSaveCharDeath" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveCharDeath_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalEditUpdateActor" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h3 class="modal-title text-center">Character Actor</h3>
                </div>
                <div class="modal-body">
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3 TableLabel">Actor Name:</div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:DropDownList ID="ddlActorName" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3 TableLabel">Start Date:</div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:TextBox ID="tbActorStartDate" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceActorStartDate" runat="server" TargetControlID="tbActorStartDate" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3 TableLabel">End Date:</div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:TextBox ID="tbActorEndDate" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceActorEndDate" runat="server" TargetControlID="tbActorEndDate" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3 TableLabel">
                            Comments:<br />
                            (Visible<br />
                            to player)
                        </div>
                        <div class="col-sm-9" style="padding-left: 0px; margin-left: 0px;">
                            <asp:TextBox ID="tbActorComments" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="500" Rows="5" />
                        </div>
                    </div>
                    <div class="row" style="padding-top: 5px;">
                        <div class="col-sm-3 TableLabel">
                            Staff Comments:<br />
                            (Not visible to player)
                        </div>
                        <div class="col-sm-9" style="padding-left: 0px;">
                            <asp:TextBox ID="tbActorStaffComments" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="500" Rows="5" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-lg-12 text-center">
                            <asp:RequiredFieldValidator ID="rfvActorRequired" runat="server" ControlToValidate="ddlActorName" InitialValue="-1" ErrorMessage="You must choose an actor.<br>"
                                Font-Bold="true" Font-Italic="true" Font-Size="20px" ForeColor="Red" Display="Dynamic" ValidationGroup="ActorEntry" />
                            <asp:RequiredFieldValidator ID="rvActorStartDate" runat="server" ControlToValidate="tbActorStartDate" InitialValue="" ErrorMessage="You must include a start date.<br>"
                                Font-Bold="true" Font-Italic="true" Font-Size="20px" ForeColor="Red" Display="Dynamic" ValidationGroup="ActorEntry" />
                            <asp:CompareValidator ID="cvActorStartEndDate" runat="server" ControlToValidate="tbActorStartDate" ControlToCompare="tbActorEndDate" Type="Date" Operator="LessThanEqual"
                                ErrorMessage="The start date must be less than the end date.<br>" Font-Bold="true" Font-Italic="true" Font-Size="20px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Button ID="btnCancelActor" OnClientClick="closeActor(); return false;" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" />
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:HiddenField ID="hidActorID" runat="server" />
                            <asp:Button ID="btnSaveActor" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveActor_Click" ValidationGroup="ActorEntry" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidSkillSetID" runat="server" />
    <asp:HiddenField ID="hidCharacterID" runat="server" />
    <asp:HiddenField ID="hidActorDateProblems" runat="server" Value="" />
    <!-- /#page-wrapper -->
</asp:Content>
