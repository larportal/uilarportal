<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCampaignInfo.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.EditCampaignInfo" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="/Scripts/jquery-3.3.1.min.js"></script>

    <style>
        body {
            font-size: 12pt !important;
        }

        .bold {
            font-weight: bold;
        }

        th {
            text-align: right;
        }

        .button {
            background-color: #0727f5;
            -webkit-border-radius: 60px;
            border-radius: 60px;
            border: none;
            cursor: pointer;
            display: inline-block;
            font-family: sans-serif;
            font-size: 20px;
            padding: 5px 15px;
            text-align: center;
            text-decoration: none;
            color: white;
        }

        .smallbutton {
            background-color: #0727f5;
            -webkit-border-radius: 30px;
            border-radius: 30px;
            border: none;
            cursor: pointer;
            display: inline-block;
            font-family: sans-serif;
            font-size: 12px;
            padding: 5px 15px;
            text-align: center;
            text-decoration: none;
            color: white;
        }


        .button-glow {
            animation: glowing 1300ms infinite;
        }

        @keyframes glowing {
            0% {
                background-color: #0727f5;
                box-shadow: 0 0 5px #0727f5;
            }

            50% {
                background-color: #0319a6;
                box-shadow: 0 0 20px #0319a6;
            }

            100% {
                background-color: #041580;
                box-shadow: 0 0 5px #041580;
            }
        }

        .tabheader {
            font-size: 12pt;
            padding-bottom: 10pt;
            margin-bottom: 100pt;
        }

        ajax__tab .ajax__tab_tab {
            height: 30px; /*if you change it here*/
            margin: 0;
            padding: 10px 4px;
            border: 1px solid;
            border-bottom: 0px;
        }

        .ajax__tab .ajax__tab_body {
            top: 30px; /*change it here also*/
            border: 1px solid;
            border-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="smManager" runat="server" />

            <span style="font-size: x-large; font-weight: 700;">Campaign Setup</span>

            <table width="100%">
                <tr>
                    <td>Campaign Name: <asp:DropDownList ID="ddlCampaignList" runat="server" OnSelectedIndexChanged="ddlCampaignList_SelectedIndexChanged" AutoPostBack="true" />
                    </td>
                    <td align="right">
                        <asp:Button runat="server" ID="btnAddCampaign" CssClass="smallbutton" BackColor="Green" Text="Add Campaign" OnClick="btnAddCampaign_Click" /></td>
                </tr>
            </table>

            <ajaxToolkit:TabContainer ID="tbCampaignInfo" runat="server" ActiveTabIndex="0">
                <ajaxToolkit:TabPanel runat="server" ID="tbBasicInfo" Font-Size="12pt">
                    <HeaderTemplate>
                        <span class="tabheader">Basic Info</span>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <th>Campaign Name:</th>
                                <td>
                                    <asp:TextBox ID="tbCampaignName" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Campaign Start Date:</th>
                                <td colspan="3">
                                    <asp:TextBox ID="tbStartDate" runat="server" MaxLength="20" />
                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbStartDate" Format="MM/dd/yyyy" />
                                    &nbsp;&nbsp;Projected End Date: 
                                <asp:TextBox ID="tbEndDate" runat="server" MaxLength="20" />
                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbEndDate" Format="MM/dd/yyyy" />
                                </td>

                            </tr>
                            <tr>
                                <th>Game System:</th>
                                <td>
                                    <asp:DropDownList ID="ddlGameSystem" runat="server" onchange="javascript: Blink();" /></td>
                            </tr>
                            <tr>
                                <th>Marketing Campaign Size: </th>
                                <td>
                                    <asp:DropDownList ID="ddlMarketingSize" runat="server" onchange="javascript: Blink();" /></td>
                            </tr>
                            <tr>
                                <th>Primary Owner ID:</th>
                                <td>
                                    <asp:DropDownList ID="ddlPrimaryOwner" runat="server" onchange="javascript: Blink();" /></td>
                            </tr>
                            <tr>
                                <th>Primary Zip Code: </th>
                                <td>
                                    <asp:TextBox ID="tbPrimaryZipCode" runat="server" MaxLength="10" /></td>
                            </tr>
                            <tr>
                                <th>Web Page Description: </th>
                                <td>
                                    <asp:TextBox ID="tbWebDescription" runat="server" Columns="100" Rows="5" /></td>
                            </tr>
                            <tr>
                                <th>Campaign URL: </th>
                                <td>
                                    <asp:TextBox ID="tbURL" runat="server" Columns="100" /></td>
                            </tr>

                            <tr>
                                <th>Minimum Age: </th>
                                <td>
                                    <asp:TextBox ID="tbMinAge" runat="server" Columns="100" /></td>
                            </tr>
                            <tr>
                                <th>Min Age w/Supervision: </th>
                                <td>
                                    <asp:TextBox ID="tbMinAgeSuper" runat="server" Columns="100" /></td>
                            </tr>
                            <tr>
                                <th>Campaign Status:</th>
                                <td>
                                    <asp:DropDownList ID="ddlCampaignStatus" runat="server" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2">
                    <HeaderTemplate>
                        <span class="tabheader">Notification Emails</span>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <h2>Notification Emails</h2>
                        <table>
                            <tr>
                                <th>CharacterHistoryNotificationEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbCharHistNotification" runat="server" MaxLength="1000" Columns="100" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowCharacterHistoryEmail" runat="server" Text="Show Character History Email" /></td>

                            </tr>
                            <tr>
                                <th style="background-color: wheat;">CharacterNotificationEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbCharNotification" runat="server" MaxLength="1000" Columns="100" BackColor="Wheat" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowCharacterNotificationEmail" runat="server" Text="Show Character Notification Email" BackColor="Wheat" /></td>
                            </tr>
                            <tr>
                                <th>CPNotificationEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbCPNotification" runat="server" MaxLength="1000" Columns="100" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowCPNotificationEmail" runat="server" Text="Show CP Notification Email" /></td>
                            </tr>
                            <tr>
                                <th style="background-color: wheat;">InfoRequestEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbInfoRequest" runat="server" MaxLength="1000" Columns="100" BackColor="Wheat" /></td>
                            </tr>
                            <tr>
                                <th>InfoSkillEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbInfoSkill" runat="server" MaxLength="1000" Columns="100" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowInfoSkillEmail" runat="server" Text="Show Info Skill Email" /></td>
                            </tr>
                            <tr>
                                <th style="background-color: wheat;">JoinRequestEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbJoinRequest" runat="server" MaxLength="1000" Columns="100" BackColor="Wheat" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowJoinRequestEmail" runat="server" Text="Show Join Request Email" BackColor="Wheat" /></td>
                            </tr>
                            <tr>
                                <th>NotificationsEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbNotifications" runat="server" MaxLength="1000" Columns="100" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowCampaignInfoEmail" runat="server" Text="Show Campaign Info Email" /></td>
                            </tr>
                            <tr>
                                <th style="background-color: wheat;">PELNotificationEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbPELNotification" runat="server" MaxLength="1000" Columns="100" BackColor="Wheat" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowPELNotificationEmail" runat="server" Text="Show PEL Notification Email" BackColor="Wheat" /></td>
                            </tr>
                            <tr>
                                <th>ProductionSkillEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbProductionSkill" runat="server" MaxLength="1000" Columns="100" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowProductionSkillEmail" runat="server" Text="Show Production Skill Email" /></td>
                            </tr>
                            <tr>
                                <th style="background-color: wheat;">RegistrationNotificationEmail: </th>
                                <td>
                                    <asp:TextBox ID="tbRegNotif" runat="server" MaxLength="1000" Columns="100" BackColor="Wheat" /></td>
                                <td>
                                    <asp:CheckBox ID="cbShowRegistrationNotificationEmail" runat="server" Text="Show Registration Notification Email" BackColor="Wheat" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="tpTechLevels">
                    <HeaderTemplate>
                        <span class="tabheader">Tech/Weapon/Genre/Campaign Roles/Period</span>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <th style="text-align: center;">Tech Levels</th>
                                <td style="width: 10px;" rowspan="2"></td>
                                <th style="text-align: center;">Weapons</th>
                                <td style="width: 10px;" rowspan="2"></td>
                                <th style="text-align: center;">Genres</th>
                                <td style="width: 10px;" rowspan="2"></td>
                                <th style="text-align: center;">Campaign Roles</th>
                                <td style="width: 10px;" rowspan="2"></td>
                                <th style="text-align: center;">Periods</th>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblTechLevels" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblWeapons" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblGenre" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblCampaignRoles" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblPeriods" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="tpHousing">
                    <HeaderTemplate>
                        <span class="tabheader">Housing/Campaign Statuses/Payment Types</span>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <th style="text-align: center;">Housing Types</th>
                                <td style="width: 10px;" rowspan="2"></td>
<%--                                <th style="text-align: center;">Campaign Statuses</th>
                                <td style="width: 10px;" rowspan="2"></td>--%>
                                <th style="text-align: center;">Payment Types</th>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblHousingTypes" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
<%--                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblCampaignStatuses" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>--%>
                                <td style="vertical-align: top">
                                    <asp:CheckBoxList ID="cblPaymentTypes" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="tbUserDefined">
                    <HeaderTemplate>
                        <span class="tabheader">User Defined Fields/Pools</span>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="vertical-align: top">
                                    <table style="border: solid 1px;">
                                        <tr>
                                            <th colspan="4" style="text-align: center">User Defined Fields</th>
                                        </tr>
                                        <tr style="vertical-align: middle">
                                            <td>User defined field 1: </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbUserDef1" /></td>
                                            <td>Use&nbsp;&nbsp;&nbsp;<span id="lblUserDef1" runat="server">Label: </span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbUserDef1" MaxLength="500" Columns="25" /></td>
                                        </tr>
                                        <tr style="background: wheat;">
                                            <td>User defined field 2: </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbUserDef2" /></td>
                                            <td>Use&nbsp;&nbsp;&nbsp;<span id="lblUserDef2" runat="server">Label: </span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbUserDef2" MaxLength="500" Columns="25" BackColor="Wheat" /></td>
                                        </tr>
                                        <tr style="vertical-align: middle">
                                            <td>User defined field 3: </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbUserDef3" /></td>
                                            <td>Use&nbsp;&nbsp;&nbsp;<span id="lblUserDef3" runat="server">Label: </span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbUserDef3" MaxLength="500" Columns="25" /></td>
                                        </tr>
                                        <tr style="background: wheat;">
                                            <td>User defined field 4: </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbUserDef4" /></td>
                                            <td>Use&nbsp;&nbsp;&nbsp;<span id="lblUserDef4" runat="server">Label: </span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbUserDef4" MaxLength="500" Columns="25" BackColor="Wheat" /></td>
                                        </tr>
                                        <tr style="vertical-align: middle">
                                            <td>User defined field 5: </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbUserDef5" /></td>
                                            <td>Use&nbsp;&nbsp;&nbsp;<span id="lblUserDef5" runat="server">Label: </span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tbUserDef5" MaxLength="500" Columns="25" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvPoolData" runat="server" AutoGenerateColumns="false"
                                                    DataKeyNames="CampaignSkillPoolID"
                                                    OnRowEditing="gvPoolData_RowEditing" OnRowCancelingEdit="gvPoolData_RowCancelingEdit"
                                                    OnRowUpdating="gvPoolData_RowUpdating" OnRowDeleting="gvPoolData_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField DataField="CampaignSkillPoolID" ReadOnly="true" />
                                                        <asp:BoundField DataField="PoolDescription" HeaderText="Pool Description" />
                                                        <asp:BoundField DataField="DisplayColor" HeaderText="Display Color" />
                                                        <asp:CheckBoxField DataField="DefaultPool" HeaderText="Default Pool" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:CheckBoxField DataField="SuppressOnCard" HeaderText="Suppress On Card" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ShowDeleteButton="true" ButtonType="Button" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top;">
                                                <asp:Button ID="btnAddPool" runat="server" Text="Add Pool" OnClick="btnAddPool_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlAddPool" runat="server" Visible="false">
                                                    <table>
                                                        <tr>
                                                            <th>Pool Name:</th>
                                                            <td>
                                                                <asp:TextBox MaxLength="500" Columns="50" runat="server" ID="tbNewPoolName" /></td>
                                                        </tr>
                                                        <tr>
                                                            <th>Pool Color:</th>
                                                            <td>
                                                                <asp:TextBox MaxLength="500" Columns="50" runat="server" ID="tbNewPoolColor" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:CheckBox ID="cbSuppressOnCard" runat="server" Text="Suppress On Card" /></td>
                                                        </tr>
                                                        <tr>
                                                            <th>&nbsp;</th>
                                                            <td>
                                                                <asp:CheckBox ID="cbDefaultPool" runat="server" Text="Default Pool*" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>By checking default pool you will</td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>override the current default pool.</td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Button ID="btnAddPoolRecord" runat="server" Text="Add New Pool" OnClick="btnAddPoolRecord_Click" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel1">
                    <HeaderTemplate>
                        <span class="tabheader">CP Opp Defaults</span>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvOppDefaults" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="ReasonID" OnRowDataBound="gvOppDefaults_RowDataBound"
                                        OnRowEditing="gvOppDefaults_RowEditing" OnRowCancelingEdit="gvOppDefaults_RowCancelingEdit"
                                        OnRowUpdating="gvOppDefaults_RowUpdating">
                                        <Columns>
                                            <asp:CheckBoxField DataField="HasDefault" HeaderText="Enable" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Description" HeaderText="Reason" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Opportunity Description" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOpportunityDescription" runat="server" Text='<%# Eval("OpportunityDescription") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlDescriptions" runat="server" />
                                                    <asp:HiddenField ID="hidTypeID" runat="server" Value='<%# Eval("TypeID") %>' />
                                                    <asp:HiddenField ID="hidOrigDesc" runat="server" Value='<%# Eval("OrigDescription") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="CPValue" DataField="CPValue" DataFormatString="{0:F2}" 
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
                                            <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ButtonType="Button" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
            <br />

            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Button runat="server" ID="btnSaveInfo" CssClass="button" Text="Save Info" OnClick="btnSaveInfo_Click" /></td>
                </tr>
            </table>

        </div>
    </form>

    <input type="hidden" id="hidBlink" value="" />
    <script>
        $(document).ready(function () {
            $("input[type=text]").keypress(function () {
                var hidBlink = document.getElementById('hidBlink')
                if (hidBlink.value == "") {
                    var d = document.getElementById('<%= btnSaveInfo.ClientID %>');
                    d.className += " button-glow";
                    hidBlink.value = "Showing";
                }
            });
        });
        function Blink() {
            var hidBlink = document.getElementById('hidBlink')
            if (hidBlink.value == "") {
                var d = document.getElementById('<%= btnSaveInfo.ClientID %>');
                d.className += " button-glow";
                hidBlink.value = "Showing";
            }
        }

        function CheckBoxes() {
            var cbxUserDef1 = document.getElementById('<%= cbUserDef1.ClientID %>');
            var tbUserDef1 = document.getElementById('<%= tbUserDef1.ClientID %>');
            var lblUserDef1 = document.getElementById('<%= lblUserDef1.ClientID %>');
            if (cbxUserDef1.checked) {
                tbUserDef1.style.visibility = 'visible';
                lblUserDef1.style.visibility = 'visible';
            }
            else {
                tbUserDef1.style.visibility = 'hidden';
                lblUserDef1.style.visibility = 'hidden';
            }

            var cbxUserDef2 = document.getElementById('<%= cbUserDef2.ClientID %>');
            var tbUserDef2 = document.getElementById('<%= tbUserDef2.ClientID %>');
            var lblUserDef2 = document.getElementById('<%= lblUserDef2.ClientID %>');
            if (cbxUserDef2.checked) {
                tbUserDef2.style.visibility = 'visible';
                lblUserDef2.style.visibility = 'visible';
            }
            else {
                tbUserDef2.style.visibility = 'hidden';
                lblUserDef2.style.visibility = 'hidden';
            }

            var cbxUserDef3 = document.getElementById('<%= cbUserDef3.ClientID %>');
            var tbUserDef3 = document.getElementById('<%= tbUserDef3.ClientID %>');
            var lblUserDef3 = document.getElementById('<%= lblUserDef3.ClientID %>');
            if (cbxUserDef3.checked) {
                tbUserDef3.style.visibility = 'visible';
                lblUserDef3.style.visibility = 'visible';
            }
            else {
                tbUserDef3.style.visibility = 'hidden';
                lblUserDef3.style.visibility = 'hidden';
            }


            var cbxUserDef4 = document.getElementById('<%= cbUserDef4.ClientID %>');
            var tbUserDef4 = document.getElementById('<%= tbUserDef4.ClientID %>');
            var lblUserDef4 = document.getElementById('<%= lblUserDef4.ClientID %>');
            if (cbxUserDef4.checked) {
                tbUserDef4.style.visibility = 'visible';
                lblUserDef4.style.visibility = 'visible';
            }
            else {
                tbUserDef4.style.visibility = 'hidden';
                lblUserDef4.style.visibility = 'hidden';
            }


            var cbxUserDef5 = document.getElementById('<%= cbUserDef5.ClientID %>');
            var tbUserDef5 = document.getElementById('<%= tbUserDef5.ClientID %>');
            var lblUserDef5 = document.getElementById('<%= lblUserDef5.ClientID %>');
            if (cbxUserDef5.checked) {
                tbUserDef5.style.visibility = 'visible';
                lblUserDef5.style.visibility = 'visible';
            }
            else {
                tbUserDef5.style.visibility = 'hidden';
                lblUserDef5.style.visibility = 'hidden';
            }
            Blink();
        }
    </script>


</body>
</html>
