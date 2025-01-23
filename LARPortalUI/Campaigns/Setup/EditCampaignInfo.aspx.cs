using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns.Setup
{
    public partial class EditCampaignInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SortedList sParams = new SortedList();
                DataTable dtCampaigns = Classes.cUtilities.LoadDataTable("uspGetCampaigns", sParams, "LARPortal", "JB", "PageLoad");
                DataView dvCampaigns = new DataView(dtCampaigns, "DateDeleted is null", "CampaignName", DataViewRowState.CurrentRows);
                ddlCampaignList.DataSource = dvCampaigns;
                ddlCampaignList.DataTextField = "CampaignName";
                ddlCampaignList.DataValueField = "CampaignID";
                ddlCampaignList.DataBind();

                DataTable dtGameSystems = Classes.cUtilities.LoadDataTable("uspGetGameSystems", sParams, "LARPortal", "JB", "PageLoad");
                DataView dvGamesSystems = new DataView(dtGameSystems, "", "GameSystemName", DataViewRowState.CurrentRows);
                ddlGameSystem.DataSource = dvGamesSystems;
                ddlGameSystem.DataTextField = "GameSystemName";
                ddlGameSystem.DataValueField = "GameSystemID";
                ddlGameSystem.DataBind();
                ddlGameSystem.Items.Insert(0, new ListItem("Not Specified", "-1"));

                sParams = new SortedList();
                sParams.Add("@StatusType", "Campaign");
                DataTable dtCampaignStatus = Classes.cUtilities.LoadDataTable("uspGetStatus", sParams, "LARPortal", "JB", "PageLoad");
                DataView dvCampaignStatus = new DataView(dtCampaignStatus, "", "StatusName", DataViewRowState.CurrentRows);
                ddlCampaignStatus.DataSource = dvCampaignStatus;
                ddlCampaignStatus.DataTextField = "StatusName";
                ddlCampaignStatus.DataValueField = "StatusID";
                ddlCampaignStatus.DataBind();
                ddlCampaignStatus.Items.Insert(0, new ListItem("Not Specified", "-1"));

                sParams = new SortedList();
                sParams.Add("@SizeFilter", "0");
                DataTable dtSizes = Classes.cUtilities.LoadDataTable("uspGetSizes", sParams, "LARPortal", "JB", "PageLoad");
                DataView dvSizes = new DataView(dtSizes, "", "SortOrder", DataViewRowState.CurrentRows);
                ddlMarketingSize.DataSource = dvSizes;
                ddlMarketingSize.DataTextField = "CampaignSizeRange";
                ddlMarketingSize.DataValueField = "CampaignSizeID";
                ddlMarketingSize.DataBind();

                sParams = new SortedList();
                DataTable dtPELApproveLevel = Classes.cUtilities.LoadDataTable("uspGetPELApprovalLevel", sParams, "LARPortal", "JB", "PageLoad");
                ddlPELApproval.DataSource = dtPELApproveLevel;
                ddlPELApproval.DataTextField = "Description";
                ddlPELApproval.DataValueField = "ApprovalLevelID";
                ddlPELApproval.DataBind();

                sParams = new SortedList();
                DataTable dtCharHistApprovLevel = Classes.cUtilities.LoadDataTable("uspGetCharHistApprovLevel", sParams, "LARPortal", "JB", "PageLoad");
                ddlCharHistoryApproval.DataSource = dtCharHistApprovLevel;
                ddlCharHistoryApproval.DataTextField = "Description";
                ddlCharHistoryApproval.DataValueField = "ApprovalLevelID";
                ddlCharHistoryApproval.DataBind();
                ddlCharHistoryApproval.Items.Insert(0, new ListItem("Select Item...", "-1"));

                sParams = new SortedList();
                DataTable dtCancelPolicy = Classes.cUtilities.LoadDataTable("uspGetCancellationPolicy", sParams, "LARPortal", "JB", "PageLoad");
                DataView dvCancelPolicy = new DataView(dtCancelPolicy, "", "PolicyDescription", DataViewRowState.CurrentRows);
                ddlCancelPolicy.DataSource = dvCancelPolicy;
                ddlCancelPolicy.DataTextField = "PolicyDescription";
                ddlCancelPolicy.DataValueField = "PolicyID";
                ddlCancelPolicy.DataBind();


                sParams = new SortedList();
                sParams.Add("@StyleFilter", 0);
                DataTable dtCampaignStyles = Classes.cUtilities.LoadDataTable("uspGetStyles", sParams, "LARPortal", "JB", "PageLoad");
                ddlStyle.DataSource = dtCampaignStyles;
                ddlStyle.DataTextField = "StyleName";
                ddlStyle.DataValueField = "StyleID";
                ddlStyle.DataBind();

                sParams = new SortedList();
                DataTable dtPortalAccessTypes = Classes.cUtilities.LoadDataTable("uspGetPortalAccessTypes", sParams, "LARPortal", "JB", "PageLoad");
                ddlPortalAccessType.DataSource = dtPortalAccessTypes;
                ddlPortalAccessType.DataTextField = "Description";
                ddlPortalAccessType.DataValueField = "PortalAccessTypeID";
                ddlPortalAccessType.DataBind();

                sParams = new SortedList();
                DataTable dtDonationTimetable = Classes.cUtilities.LoadDataTable("uspGetDonationtimetable", sParams, "LARPortal", "JB", "PageLoad");
                ddlDonAwardWhen.DataSource = dtDonationTimetable;
                ddlDonAwardWhen.DataTextField = "TimetableDescription";
                ddlDonAwardWhen.DataValueField = "DonationTimeTableID";
                ddlDonAwardWhen.DataBind();

                using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
                {
                    Conn.Open();
                    string SQLUsers = "SELECT UserID, LoginUserName, LastName, FirstName, NickName, " +
                        "CASE WHEN LEN(ISNULL(NickName, '')) > 0 THEN NickName ELSE FirstName END AS PlayerAKA FROM MDBUsers where DateDeleted is null";
                    using (SqlCommand cmdGetUsers = new SqlCommand(SQLUsers, Conn))
                    {
                        cmdGetUsers.CommandType = CommandType.Text;
                        SqlDataAdapter SDAGetUsers = new SqlDataAdapter(cmdGetUsers);
                        DataTable dtUsers = new DataTable();
                        SDAGetUsers.Fill(dtUsers);
                        if (dtUsers.Columns["DisplayValue"] == null)
                        {
                            dtUsers.Columns.Add("DisplayValue", typeof(string), "PLayerAKA + ' ' + LastName + ' : ' + LoginUserName");
                        }
                        DataView dvUsers = new DataView(dtUsers, "LastName <> ''", "LastName, PlayerAKA, LoginUserName", DataViewRowState.CurrentRows);
                        ddlPrimaryOwner.DataSource = dvUsers;
                        ddlPrimaryOwner.DataTextField = "DisplayValue";
                        ddlPrimaryOwner.DataValueField = "UserID";
                        ddlPrimaryOwner.DataBind();
                    }
                    string SQLCPOppDesc = "select CampaignCPOpportunityTypeID, OpportunityTypeName, OpportunityDescription " +
                        "from CMCampaignCPOpportunityTypes where DateDeleted is NULL";
                    using (SqlCommand cmdGetOppDesc = new SqlCommand(SQLCPOppDesc, Conn))
                    {
                        cmdGetOppDesc.CommandType = CommandType.Text;
                        SqlDataAdapter SDAGetOppDesc = new SqlDataAdapter(cmdGetOppDesc);
                        DataTable dtOppDesc = new DataTable();
                        SDAGetOppDesc.Fill(dtOppDesc);
                        ViewState["OppDesc"] = dtOppDesc;
                    }
                }

                ddlCampaignList_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlCampaignList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", ddlCampaignList.SelectedValue);
            ClearAllObjects();
            DataSet dsCampaigns = Classes.cUtilities.LoadDataSet("uspGetCampaignByCampaignID", sParams, "LARPortal", "JB", "PageLoad");

            dsCampaigns.Tables[0].TableName = "CampaignInfo";
            dsCampaigns.Tables[1].TableName = "TechLevel";
            dsCampaigns.Tables[2].TableName = "WeaponLevel";
            dsCampaigns.Tables[3].TableName = "Genres";
            dsCampaigns.Tables[4].TableName = "CampaignRoles";
            dsCampaigns.Tables[5].TableName = "Periods";
            dsCampaigns.Tables[6].TableName = "HousingTypes";
            dsCampaigns.Tables[7].TableName = "CampaignStatuses";
            dsCampaigns.Tables[8].TableName = "PaymentTypes";
            dsCampaigns.Tables[9].TableName = "OpportunityDefaults";
            dsCampaigns.Tables[10].TableName = "Pools";
            dsCampaigns.Tables[11].TableName = "Donations";
            dsCampaigns.Tables[12].TableName = "Shares";

            ViewState["Pools"] = dsCampaigns.Tables["Pools"];

            BindPoolData();
            foreach (DataRow dr in dsCampaigns.Tables["CampaignInfo"].Rows)
            {
                tbCampaignName.Text = dr["CampaignName"].ToString();
                bool bFoundItem = false;

                DateTime dtTemp;
                if (DateTime.TryParse(dr["CampaignStartDate"].ToString(), out dtTemp))
                    tbStartDate.Text = dtTemp.ToString("MM/dd/yyyy");

                if (DateTime.TryParse(dr["ProjectedEndDate"].ToString(), out dtTemp))
                    tbEndDate.Text = dtTemp.ToString("MM/dd/yyyy");

                ddlGameSystem.ClearSelection();
                foreach (ListItem li in ddlGameSystem.Items)
                {
                    if (li.Value == dr["GameSystemID"].ToString())
                    {
                        ddlGameSystem.ClearSelection();
                        li.Selected = true;
                        bFoundItem = true;
                    }
                }
                if (!bFoundItem)
                    ddlGameSystem.SelectedIndex = 0;

                bFoundItem = false;
                ddlMarketingSize.ClearSelection();
                foreach (ListItem li in ddlMarketingSize.Items)
                {
                    if (li.Value == dr["MarketingCampaignSize"].ToString())
                    {
                        ddlMarketingSize.ClearSelection();
                        li.Selected = true;
                        bFoundItem = true;
                    }
                }
                if (!bFoundItem)
                    ddlMarketingSize.SelectedIndex = 0;

                bFoundItem = false;

                ListItem liOwner = new ListItem();
                string sOwnerID = dr["PrimaryOwnerID"].ToString();
                liOwner = ddlPrimaryOwner.Items.FindByValue(sOwnerID);
                if (liOwner != null)
                {
                    ddlPrimaryOwner.ClearSelection();
                    liOwner.Selected = true;
                }
                else
                {
                    liOwner = ddlPrimaryOwner.Items.FindByValue("2");            //  Ricks account.
                    ddlPrimaryOwner.ClearSelection();
                    liOwner.Selected = true;
                }

                tbPrimaryZipCode.Text = dr["PrimarySiteZipCode"].ToString();
                tbWebDescription.Text = dr["CampaignWebPageDescription"].ToString();
                tbURL.Text = dr["CampaignURL"].ToString();

                int iTemp;
                if (int.TryParse(dr["MinimumAge"].ToString(), out iTemp))
                    tbMinAge.Text = iTemp.ToString();
                if (int.TryParse(dr["MinimumAgeWithSupervision"].ToString(), out iTemp))
                    tbMinAgeSuper.Text = iTemp.ToString();

                //                string t = dr["CampDesc"].ToString();

                string sCampaignStatus = dr["StatusID"].ToString();
                bFoundItem = false;
                ddlCampaignStatus.ClearSelection();
                foreach (ListItem li in ddlCampaignStatus.Items)
                {
                    if (li.Value == sCampaignStatus)
                    {
                        ddlCampaignStatus.ClearSelection();
                        li.Selected = true;
                        bFoundItem = true;
                    }
                }
                if (!bFoundItem)
                {
                    ddlCampaignStatus.ClearSelection();
                    ddlCampaignStatus.SelectedIndex = 0;
                }

                tbProjectedNumEvents.Text = dr["ProjectedNumberOfEvents"].ToString();
                double dTemp;
                tbMembershipFee.Text = "";
                object o = dr["MembershipFee"].ToString();
                if (!string.IsNullOrEmpty(dr["MembershipFee"].ToString()))
                    if (double.TryParse(dr["MembershipFee"].ToString(), out dTemp))
                        tbMembershipFee.Text = string.Format("{0:0.00}", dTemp);
                tbMembershipFreq.Text = dr["MembershipFeeFrequency"].ToString();
                tbEmerContact.Text = dr["EmergencyEventContactInfo"].ToString();

                bool bTemp;

                cbPlayerApprovalReq.Checked = false;
                if (bool.TryParse(dr["PlayerApprovalRequired"].ToString(), out bTemp))
                    cbPlayerApprovalReq.Checked = bTemp;

                cbNPCApprovalReq.Checked = false;
                if (bool.TryParse(dr["NPCApprovalRequired"].ToString(), out bTemp))
                    cbNPCApprovalReq.Checked = bTemp;
                cbShareLocationUseNotes.Checked = false;

                tbMaxCPPerYear.Text = dr["MaximumCPPerYear"].ToString();
                cbAllowCPDonation.Checked = false;
                if (bool.TryParse(dr["AllowCPDonation"].ToString(), out bTemp))
                    cbAllowCPDonation.Checked = bTemp;

                bFoundItem = false;
                if (int.TryParse(dr["PELApprovalLevel"].ToString(), out iTemp))
                {
                    ddlPELApproval.ClearSelection();
                    foreach (ListItem lItem in ddlPELApproval.Items)
                    {
                        if (lItem.Value == iTemp.ToString())
                        {
                            ddlPELApproval.ClearSelection();
                            lItem.Selected = true;
                            bFoundItem = true;
                        }
                    }
                }

                if (!bFoundItem)
                {
                    ddlPELApproval.ClearSelection();
                    ddlPELApproval.Items[0].Selected = true;
                }

                bFoundItem = false;
                if (int.TryParse(dr["CharacterHistoryApprovalLevel"].ToString(), out iTemp))
                {
                    ddlCharHistoryApproval.ClearSelection();
                    foreach (ListItem lItem in ddlCharHistoryApproval.Items)
                    {
                        if (lItem.Value == iTemp.ToString())
                        {
                            ddlCharHistoryApproval.ClearSelection();
                            lItem.Selected = true;
                            bFoundItem = true;
                        }
                    }
                }

                if (!bFoundItem)
                {
                    ddlCharHistoryApproval.ClearSelection();
                    ddlCharHistoryApproval.Items[0].Selected = true;
                }

                tbCampaignRuleURL.Text = dr["CampaignRulesURL"].ToString();

                cbShareLocationUseNotes.Checked = false;
                if (bool.TryParse(dr["ShareLocationUseNotes"].ToString(), out bTemp))
                    cbShareLocationUseNotes.Checked = bTemp;

                tbEventCharCap.Text = "";
                if (int.TryParse(dr["EventCharacterCap"].ToString(), out iTemp))
                    tbEventCharCap.Text = iTemp.ToString();
                tbAnnualCharCap.Text = "";
                if (int.TryParse(dr["AnnualCharacterCap"].ToString(), out iTemp))
                    tbAnnualCharCap.Text = iTemp.ToString();
                tbTotalCharCap.Text = "";
                if (int.TryParse(dr["TotalCharacterCap"].ToString(), out iTemp))
                    tbTotalCharCap.Text = iTemp.ToString();

                //ddlCancelPolicy.SelectedIndex = 0;

                //foreach (ListItem li in ddlCancelPolicy.Items)
                //{
                //    if (li.Text == dr["CancellationPolicy"].ToString())
                //    {
                //        ddlCancelPolicy.ClearSelection();
                //        li.Selected = true;
                //    }
                //}
//                ddlCancelPolicy.Text = dr["CancellationPolicy"].ToString();

                if (int.TryParse(dr["EarliestCPApplicationYear"].ToString(), out iTemp))
                    tbEarliestCPAppYear.Text = iTemp.ToString();
                tbBillingFreq.Text = dr["BillingFrequency"].ToString();

                cbAllowCharRebuild.Checked = false;
                if (bool.TryParse(dr["AllowCharacterRebuild"].ToString(), out bTemp))
                    cbAllowCharRebuild.Checked = bTemp;

                tbCharHistNotification.Text = dr["CharacterHistoryNotificationEmail"].ToString();
                tbCharNotification.Text = dr["CharacterNotificationEmail"].ToString();
                tbCPNotification.Text = dr["CPNotificationEmail"].ToString();
                tbInfoRequest.Text = dr["InfoRequestEmail"].ToString();
                tbInfoSkill.Text = dr["InfoSkillEmail"].ToString();
                tbJoinRequest.Text = dr["JoinRequestEmail"].ToString();
                tbNotifications.Text = dr["NotificationsEmail"].ToString();
                tbPELNotification.Text = dr["PELNotificationEmail"].ToString();
                tbProductionSkill.Text = dr["ProductionSkillEmail"].ToString();
                tbRegNotif.Text = dr["RegistrationNotificationEmail"].ToString();


                cbShowCampaignInfoEmail.Checked = false;
                if (bool.TryParse(dr["ShowCampaignInfoEmail"].ToString(), out bTemp))
                    cbShowCampaignInfoEmail.Checked = bTemp;

                cbShowCharacterHistoryEmail.Checked = false;
                if (bool.TryParse(dr["ShowCharacterHistoryEmail"].ToString(), out bTemp))
                    cbShowCharacterHistoryEmail.Checked = bTemp;

                cbShowCharacterNotificationEmail.Checked = false;
                if (bool.TryParse(dr["ShowCharacterNotificationEmail"].ToString(), out bTemp))
                    cbShowCharacterNotificationEmail.Checked = bTemp;

                cbShowCPNotificationEmail.Checked = false;
                if (bool.TryParse(dr["ShowCPNotificationEmail"].ToString(), out bTemp))
                    cbShowCPNotificationEmail.Checked = bTemp;

                cbShowInfoSkillEmail.Checked = false;
                if (bool.TryParse(dr["ShowInfoSkillEmail"].ToString(), out bTemp))
                    cbShowInfoSkillEmail.Checked = bTemp;

                cbShowJoinRequestEmail.Checked = false;
                if (bool.TryParse(dr["ShowJoinRequestEmail"].ToString(), out bTemp))
                    cbShowJoinRequestEmail.Checked = bTemp;

                cbShowPELNotificationEmail.Checked = false;
                if (bool.TryParse(dr["ShowPELNotificationEmail"].ToString(), out bTemp))
                    cbShowPELNotificationEmail.Checked = bTemp;

                cbShowProductionSkillEmail.Checked = false;
                if (bool.TryParse(dr["ShowProductionSkillEmail"].ToString(), out bTemp))
                    cbShowProductionSkillEmail.Checked = bTemp;

                cbShowRegistrationNotificationEmail.Checked = false;
                if (bool.TryParse(dr["ShowRegistrationNotificationEmail"].ToString(), out bTemp))
                    cbShowRegistrationNotificationEmail.Checked = bTemp;

                //cbUserDef1.Attributes.Add("onchange", "javascript: CheckBoxes();");
                //tbUserDef1.Attributes.Add("onchange", "javascript: Blink();");

                //cbUserDef2.Attributes.Add("onchange", "javascript: CheckBoxes();");
                //tbUserDef2.Attributes.Add("onchange", "javascript: Blink();");

                //cbUserDef3.Attributes.Add("onchange", "javascript: CheckBoxes();");
                //tbUserDef3.Attributes.Add("onchange", "javascript: Blink();");

                //cbUserDef4.Attributes.Add("onchange", "javascript: CheckBoxes();");
                //tbUserDef4.Attributes.Add("onchange", "javascript: Blink();");

                //cbUserDef5.Attributes.Add("onchange", "javascript: CheckBoxes();");
                //tbUserDef5.Attributes.Add("onchange", "javascript: Blink();");

                if (dr["UseUserDefinedField1"].ToString() == "1")
                {
                    cbUserDef1.Checked = true;
                    lblUserDef1.Style.Add("visibility", "visible");
                    tbUserDef1.Style.Add("visibility", "visible");
                    tbUserDef1.Text = dr["UserDefinedField1"].ToString();
                }
                else
                {
                    cbUserDef1.Checked = false;
                    lblUserDef1.Style.Add("visibility", "hidden");
                    tbUserDef1.Style.Add("visibility", "hidden");
                    tbUserDef1.Text = dr["UserDefinedField1"].ToString();
                }

                if (dr["UseUserDefinedField2"].ToString() == "1")
                {
                    cbUserDef2.Checked = true;
                    lblUserDef2.Style.Add("visibility", "visible");
                    tbUserDef2.Style.Add("visibility", "visible");
                    tbUserDef2.Text = dr["UserDefinedField2"].ToString();
                }
                else
                {
                    cbUserDef2.Checked = false;
                    lblUserDef2.Style.Add("visibility", "hidden");
                    tbUserDef2.Style.Add("visibility", "hidden");
                    tbUserDef2.Text = dr["UserDefinedField2"].ToString();
                }

                if (dr["UseUserDefinedField3"].ToString() == "1")
                {
                    cbUserDef3.Checked = true;
                    lblUserDef3.Style.Add("visibility", "visible");
                    tbUserDef3.Style.Add("visibility", "visible");
                    tbUserDef3.Text = dr["UserDefinedField3"].ToString();
                }
                else
                {
                    cbUserDef3.Checked = false;
                    lblUserDef3.Style.Add("visibility", "hidden");
                    tbUserDef3.Style.Add("visibility", "hidden");
                    tbUserDef3.Text = dr["UserDefinedField3"].ToString();
                }

                if (dr["UseUserDefinedField4"].ToString() == "1")
                {
                    cbUserDef4.Checked = true;
                    lblUserDef4.Style.Add("visibility", "visible");
                    tbUserDef4.Style.Add("visibility", "visible");
                    tbUserDef4.Text = dr["UserDefinedField4"].ToString();
                }
                else
                {
                    cbUserDef4.Checked = false;
                    lblUserDef4.Style.Add("visibility", "hidden");
                    tbUserDef4.Style.Add("visibility", "hidden");
                    tbUserDef4.Text = dr["UserDefinedField4"].ToString();
                }

                if (dr["UseUserDefinedField5"].ToString() == "1")
                {
                    cbUserDef5.Checked = true;
                    lblUserDef5.Style.Add("visibility", "visible");
                    tbUserDef5.Style.Add("visibility", "visible");
                    tbUserDef5.Text = dr["UserDefinedField5"].ToString();
                }
                else
                {
                    cbUserDef5.Checked = false;
                    lblUserDef5.Style.Add("visibility", "hidden");
                    tbUserDef5.Style.Add("visibility", "hidden");
                    tbUserDef5.Text = dr["UserDefinedField5"].ToString();
                }

                bFoundItem = false;
                foreach (ListItem lItem in ddlStyle.Items)
                {
                    if (lItem.Value == dr["StyleID"].ToString())
                    {
                        ddlStyle.ClearSelection();
                        lItem.Selected = true;
                        bFoundItem = true;
                    }
                }
                if (!bFoundItem)
                {
                    ddlStyle.ClearSelection();
                    ddlStyle.Items[0].Selected = true;
                }

                bFoundItem = false;
                foreach (ListItem lItem in ddlPortalAccessType.Items)
                {
                    if (lItem.Value == dr["PortalAccessType"].ToString())
                    {
                        ddlPortalAccessType.ClearSelection();
                        lItem.Selected = true;
                        bFoundItem = true;
                    }
                }
                if (!bFoundItem)
                {
                    ddlPortalAccessType.ClearSelection();
                    ddlPortalAccessType.Items[0].Selected = true;
                }
            }

            cblTechLevels.DataSource = dsCampaigns.Tables["TechLevel"];
            cblTechLevels.DataValueField = "TechLevelID";
            cblTechLevels.DataTextField = "TechLevelName";
            cblTechLevels.DataBind();
            cblTechLevels.ClearSelection();
            DataView dvSelectedTech = new DataView(dsCampaigns.Tables["TechLevel"], "HasTechLevel = 1", "", DataViewRowState.CurrentRows);
            foreach (DataRowView dTech in dvSelectedTech)
            {
                cblTechLevels.Items.FindByValue(dTech["TechLevelID"].ToString()).Selected = true;
            }
            foreach (ListItem li in cblTechLevels.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            cblWeapons.DataSource = dsCampaigns.Tables["WeaponLevel"];
            cblWeapons.DataValueField = "WeaponID";
            cblWeapons.DataTextField = "WeaponName";
            cblWeapons.DataBind();
            cblWeapons.ClearSelection();
            DataView dvSelectedWeapon = new DataView(dsCampaigns.Tables["WeaponLevel"], "HasWeapon = 1", "", DataViewRowState.CurrentRows);
            foreach (DataRowView dWeapon in dvSelectedWeapon)
            {
                cblWeapons.Items.FindByValue(dWeapon["WeaponID"].ToString()).Selected = true;
            }
            foreach (ListItem li in cblWeapons.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            cblGenre.DataSource = dsCampaigns.Tables["Genres"];
            cblGenre.DataValueField = "GenreID";
            cblGenre.DataTextField = "GenreName";
            cblGenre.DataBind();
            cblGenre.ClearSelection();
            DataView dvSelectedGenre = new DataView(dsCampaigns.Tables["Genres"], "HasGenre = 1", "", DataViewRowState.CurrentRows);
            foreach (DataRowView dGenre in dvSelectedGenre)
            {
                cblGenre.Items.FindByValue(dGenre["GenreID"].ToString()).Selected = true;
            }
            foreach (ListItem li in cblGenre.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            DataView dvRoles = new DataView(dsCampaigns.Tables["CampaignRoles"], "", "RoleLevel desc", DataViewRowState.CurrentRows);
            cblCampaignRoles.DataSource = dvRoles;
            cblCampaignRoles.DataValueField = "RoleID";
            cblCampaignRoles.DataTextField = "RD";
            cblCampaignRoles.DataBind();
            cblCampaignRoles.ClearSelection();
            dvRoles.RowFilter = "HasRole = 1";
            foreach (DataRowView dRole in dvRoles)
            {
                cblCampaignRoles.Items.FindByValue(dRole["RoleID"].ToString()).Selected = true;
                cblCampaignRoles.Items.FindByValue(dRole["RoleID"].ToString()).Attributes["title"] = dRole["DisplayDescription"].ToString();
            }
            foreach (ListItem li in cblCampaignRoles.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            DataView dvPeriods = new DataView(dsCampaigns.Tables["Periods"], "", "SortOrder", DataViewRowState.CurrentRows);
            cblPeriods.DataSource = dvPeriods;
            cblPeriods.DataValueField = "PeriodID";
            cblPeriods.DataTextField = "PeriodName";
            cblPeriods.DataBind();
            cblPeriods.ClearSelection();
            dvPeriods.RowFilter = "HasPeriod = 1";
            foreach (DataRowView dRole in dvPeriods)
            {
                cblPeriods.Items.FindByValue(dRole["PeriodID"].ToString()).Selected = true;
            }
            foreach (ListItem li in cblPeriods.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            cblHousingTypes.DataSource = dsCampaigns.Tables["HousingTypes"];
            cblHousingTypes.DataValueField = "HousingTypeID";
            cblHousingTypes.DataTextField = "Description";
            cblHousingTypes.DataBind();
            cblHousingTypes.ClearSelection();
            DataView dvHousing = new DataView(dsCampaigns.Tables["HousingTypes"], "HasHousing = 1", "", DataViewRowState.CurrentRows);
            foreach (DataRowView dRole in dvHousing)
            {
                cblHousingTypes.Items.FindByValue(dRole["HousingTypeID"].ToString()).Selected = true;
            }
            foreach (ListItem li in cblHousingTypes.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            //cblCampaignStatuses.DataSource = dsCampaigns.Tables["CampaignStatuses"];
            //cblCampaignStatuses.DataValueField = "StatusID";
            //cblCampaignStatuses.DataTextField = "StatusName";
            //cblCampaignStatuses.DataBind();
            //cblCampaignStatuses.ClearSelection();
            //DataView dvCampaignStatuses = new DataView(dsCampaigns.Tables["CampaignStatuses"], "HasStatus = 1", "", DataViewRowState.CurrentRows);
            //foreach (DataRowView dRole in dvCampaignStatuses)
            //{
            //    cblCampaignStatuses.Items.FindByValue(dRole["StatusID"].ToString()).Selected = true;
            //}
            //foreach (ListItem li in cblCampaignStatuses.Items)
            //{
            //    li.Attributes.Add("onchange", "javascript: Blink();");
            //}

            cblPaymentTypes.DataSource = dsCampaigns.Tables["PaymentTypes"];
            cblPaymentTypes.DataValueField = "PaymentTypeID";
            cblPaymentTypes.DataTextField = "Description";
            cblPaymentTypes.DataBind();
            cblPaymentTypes.ClearSelection();
            DataView dvPaaymentTypes = new DataView(dsCampaigns.Tables["PaymentTypes"], "HasPaymentType = 1", "", DataViewRowState.CurrentRows);
            foreach (DataRowView dRole in dvPaaymentTypes)
            {
                cblPaymentTypes.Items.FindByValue(dRole["PaymentTypeID"].ToString()).Selected = true;
            }
            foreach (ListItem li in cblPaymentTypes.Items)
            {
                li.Attributes.Add("onchange", "javascript: Blink();");
            }

            ViewState["CampaignOppDefs"] = dsCampaigns.Tables["OpportunityDefaults"];
            gvOppDefaults.DataSource = (DataTable)ViewState["CampaignOppDefs"];
            gvOppDefaults.DataBind();

            cbDonShowDonationClaims.Checked = false;
            tbDonDefaultShipToAdd1.Text = "";
            tbDonDefaultShipToAdd2.Text = "";
            tbDonDefaultShipToCity.Text = "";
            tbDonDefaultShipToState.Text = "";
            tbDonDefaultShipToPostalCode.Text = "";
            tbDonDefaultShipToPhone.Text = "";
            cbDonAllowPlayerToPlayerPoints.Checked = false;
            tbDonDollarToPointValue.Text = "";
            tbDonHourToPointValue.Text = "";
            ddlDonAwardWhen.ClearSelection();
            if (ddlDonAwardWhen.Items.Count > 0)
                ddlDonAwardWhen.Items[0].Selected = true;
            tbDonDefaultEmail.Text = "";
            tbDonMaxPointsPerEvent.Text = "";
            tbDonMaxItemsPerEvent.Text = "";
            cbDonCountTransfersAgainstMax.Checked = false;
            hidCampaignDonationSettingsID.Value = "";

            foreach (DataRow dRow in dsCampaigns.Tables["Donations"].Rows)
            {
                bool bTemp;
                if ( bool.TryParse(dRow["ShowDonationClaims"].ToString(), out bTemp) )
                    cbDonShowDonationClaims.Checked = bTemp;
                tbDonDefaultShipToAdd1.Text = dRow["DefaultShipToAdd1"].ToString();
                tbDonDefaultShipToAdd2.Text = dRow["DefaultShipToAdd2"].ToString();
                tbDonDefaultShipToCity.Text = dRow["DefaultShipToCity"].ToString();
                tbDonDefaultShipToState.Text = dRow["DefaultShipToState"].ToString();
                tbDonDefaultShipToPostalCode.Text = dRow["DefaultShipToPostalCode"].ToString();
                tbDonDefaultShipToPhone.Text = dRow["DefaultShipToPhone"].ToString();
                if ( bool.TryParse(dRow["AllowPlayerToPlayerPoints"].ToString(), out bTemp) )
                    cbDonAllowPlayerToPlayerPoints.Checked = bTemp;
                double dTemp;
                int iTemp;
                if (!string.IsNullOrEmpty(dRow["CampaignDonationSettingsID"].ToString()))
                {
                    if (int.TryParse(dRow["CampaignDonationSettingsID"].ToString(), out iTemp))
                        hidCampaignDonationSettingsID.Value = iTemp.ToString();
                }
                if (double.TryParse(dRow["DefaultDollarToPointValue"].ToString(), out dTemp))
                    tbDonDollarToPointValue.Text = dTemp.ToString();
            }




//            gvShares.DataSource = dsCampaigns.Tables["Shares"];
//            gvShares.DataBind();
        }


        protected void btnSaveInfo_Click(object sender, EventArgs e)
        {
            int iCampaignID;
            DateTime dtTemp;
            if (!int.TryParse(ddlCampaignList.SelectedValue, out iCampaignID))
                return;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", ddlCampaignList.SelectedValue);
            sParams.Add("@CampaignName", tbCampaignName.Text);

            //sParams.Add("@ActualEndDate", tbActualEndDate.Text);
            //sParams.Add("@ActualNumberOfEvents", tbActualNumberOfEvents.Text);
            sParams.Add("@AllowCharacterRebuild", cbAllowCharRebuild.Checked);
            sParams.Add("@AllowCPDonation", cbAllowCPDonation.Checked);

            double dTemp;
            if (double.TryParse(tbAnnualCharCap.Text, out dTemp))
                sParams.Add("@AnnualCharacterCap", dTemp);
//            sParams.Add("@AnnualCharacterCap", tbAnnualCharCap.Text);
            sParams.Add("@BillingFrequency", tbBillingFreq.Text);
            sParams.Add("@BillTo", tbBillTo.Text);

            if (DateTime.TryParse(tbStartDate.Text, out dtTemp))
                sParams.Add("@CampaignStartDate", dtTemp);
            if (DateTime.TryParse(tbEndDate.Text, out dtTemp))
                sParams.Add("@CampaignEndDate", dtTemp);
            //sParams.Add("@CampaignAddress", XXX);
            sParams.Add("@CampaignRulesURL", tbCampaignRuleURL.Text);

            sParams.Add("@StatusID", ddlCampaignStatus.SelectedValue);

            sParams.Add("@CampaignURL", tbURL.Text);
            sParams.Add("@CampaignWebPageDescription", tbWebDescription.Text);
            //sParams.Add("@CancellationPolicy", ddlCancelPolicy.SelectedValue);
            //sParams.Add("@CharacterApprovalLevel", ???);
            //sParams.Add("@CampaignHistoryNotificationDeliveryPreferences", ???);

            sParams.Add("@CharacterHistoryApprovalLevel", ddlCharHistoryApproval.SelectedValue);

            //sParams.Add("@CharacterHistoryNotificationDeliveryPreferences", ????);
            sParams.Add("@CharacterHistoryNotificationEmail", tbCharHistNotification.Text);
            sParams.Add("@CharacterNotificationEmail", tbCharNotification.Text);
            //sParams.Add("@CPNotificationDeliveryPreferences", tbCPNotificationDeliveryPreferences.Text);
            sParams.Add("@CPNotificationEmail", tbCPNotification.Text);
            sParams.Add("@EarliestCPApplicationYear", tbEarliestCPAppYear.Text);
            sParams.Add("@EmergencyEventContactInfo", tbEmerContact.Text);
            sParams.Add("@EventCharacterCap", tbEventCharCap.Text);
            sParams.Add("@GameSystemID", ddlGameSystem.SelectedValue);
            sParams.Add("@InfoRequestEmail", tbInfoRequest.Text);
            //sParams.Add("@InfoSkillApprovalLevel", tbInfoSkill ???);
            sParams.Add("@InfoSkillEmail", tbInfoSkill.Text);
            //sParams.Add("@InfoSkillURL", tbInfoSkillURL.Text);
            sParams.Add("@JoinRequestEmail", tbJoinRequest.Text);
            sParams.Add("@MarketingCampaignSize", ddlMarketingSize.SelectedValue);
            sParams.Add("@MaximumCPPerYear", tbMaxCPPerYear.Text);
            if (tbMembershipFee.Text.Length > 0)
                sParams.Add("@MembershipFee", tbMembershipFee.Text);
            sParams.Add("@MembershipFeeFrequency", tbMembershipFreq.Text);
            sParams.Add("@MinimumAge", tbMinAge.Text);
            sParams.Add("@MinimumAgeWithSupervision", tbMinAgeSuper.Text);
            sParams.Add("@NPCApprovalRequired", cbNPCApprovalReq.Checked);
            sParams.Add("@NotificationsEmail", tbNotifications.Text);
            sParams.Add("@PELNotificationEmail", tbPELNotification.Text);
            sParams.Add("@PELApprovalLevel", ddlPELApproval.SelectedValue);
            sParams.Add("@PlayerApprovalRequired", cbPlayerApprovalReq.Checked);
            sParams.Add("@PortalAccessType", ddlPortalAccessType.SelectedValue);
            sParams.Add("@PrimaryOwnerID", ddlPrimaryOwner.SelectedValue);
            sParams.Add("@PrimarySiteZipCode", tbPrimaryZipCode.Text);
            sParams.Add("@ProductionSkillEmail", tbProductionSkill.Text);

            if (DateTime.TryParse(tbEndDate.Text, out dtTemp))
                sParams.Add("@ProjectedEndDate", dtTemp);
            //           sParams.Add("@ProjectedNumEvents", tbProjectedNumEvents.Text);
            //            sParams.Add("@RegistrationNotificationEmail", tbRegNotif.Text);
            //            sParams.Add("@ShareLocationUseNotes", cbShareLocationUseNotes.Checked);
            //            sParams.Add("@ShowCampaignInfoEmail", cbShowCampaignInfoEmail.Checked);
            //            sParams.Add("@ShowCharacterHistoryEmail", cbShowCharacterHistoryEmail.Checked);
            //            sParams.Add("@ShowCharacterNotificationEmail", cbShowCharacterNotificationEmail.Checked);
            //            sParams.Add("@ShowCPNotificationEmail", cbShowCPNotificationEmail.Checked);
            //            sParams.Add("@ShowInfoSkillEmail", cbShowInfoSkillEmail.Checked);
            //            sParams.Add("@ShowJoinRequestEmail", cbShowJoinRequestEmail.Checked);
            //            sParams.Add("@ShowPELNotificationEmail", cbShowPELNotificationEmail.Checked);
            //            sParams.Add("@ShowProductionSkillEmail", cbShowProductionSkillEmail.Checked);
            //            sParams.Add("@ShowRegistrationNotificationEmail", cbShowRegistrationNotificationEmail.Checked);
            //            sParams.Add("@StatusID", ddlCampaignStatus.SelectedValue);
            //            sParams.Add("@StyleID", ddlStyle.SelectedValue);
            sParams.Add("@TotalCharacterCap", tbTotalCharCap.Text);
            sParams.Add("@UserDefinedField1", tbUserDef1.Text);
            sParams.Add("@UseUserDefinedField1", cbUserDef1.Checked);
            sParams.Add("@UserDefinedField2", tbUserDef2.Text);
            sParams.Add("@UseUserDefinedField2", cbUserDef2.Checked);
            sParams.Add("@UserDefinedField3", tbUserDef3.Text);
            sParams.Add("@UseUserDefinedField3", cbUserDef3.Checked);
            sParams.Add("@UserDefinedField4", tbUserDef4.Text);
            sParams.Add("@UseUserDefinedField4", cbUserDef4.Checked);
            sParams.Add("@UserDefinedField5", tbUserDef5.Text);
            sParams.Add("@UseUserDefinedField5", cbUserDef5.Checked);

            sParams.Add("@UserID", -1);


            DataTable dtCampaignInfo = Classes.cUtilities.LoadDataTable("uspInsUpdCMCampaigns", sParams, "LARPortal", "sa", "EditCampaignInfo.aspx");

            iCampaignID = 0;
            foreach (DataRow dr in dtCampaignInfo.Rows)
            {
                int.TryParse(dr["CampaignID"].ToString(), out iCampaignID);
            }


            string sProcName = "";
            foreach (ListItem li in cblTechLevels.Items)
            {
                SortedList slTechLevel = new SortedList();
                slTechLevel.Add("@UserID", "-1");
                slTechLevel.Add("@CampaignID", iCampaignID);
                slTechLevel.Add("@TechLevelID", li.Value);
                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignTechLevels";
                else
                    sProcName = "uspDelCMCampaignTechLevels";

                Classes.cUtilities.PerformNonQuery(sProcName, slTechLevel, "LARPortal", "CampaignEdit");
            }

            sProcName = "";
            foreach (ListItem li in cblWeapons.Items)
            {
                SortedList slWeapon = new SortedList();
                slWeapon.Add("@UserID", "-1");
                slWeapon.Add("@CampaignID", iCampaignID);
                slWeapon.Add("@WeaponID", li.Value);
                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignWeapons";
                else
                    sProcName = "uspDelCMCampaignWeapons";

                Classes.cUtilities.PerformNonQuery(sProcName, slWeapon, "LARPortal", "CampaignEdit");
            }

            sProcName = "";
            foreach (ListItem li in cblGenre.Items)
            {
                SortedList sGenre = new SortedList();
                sGenre.Add("@UserID", "-1");
                sGenre.Add("@CampaignID", iCampaignID);
                sGenre.Add("@GenreID", li.Value);
                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignGenres";
                else
                    sProcName = "uspDelCMCampaignGenres";

                Classes.cUtilities.PerformNonQuery(sProcName, sGenre, "LARPortal", "CampaignEdit");
            }

            sProcName = "";
            foreach (ListItem li in cblCampaignRoles.Items)
            {
                SortedList sRoles = new SortedList();
                sRoles.Add("@UserID", "-1");
                sRoles.Add("@CampaignID", iCampaignID);
                sRoles.Add("@RoleID", li.Value);
                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignRoles";
                else
                    sProcName = "uspDelCMCampaignRoles";

                Classes.cUtilities.PerformNonQuery(sProcName, sRoles, "LARPortal", "CampaignEdit");
            }

            sProcName = "";
            foreach (ListItem li in cblPeriods.Items)
            {
                SortedList sPeriods = new SortedList();
                sPeriods.Add("@UserID", "-1");
                sPeriods.Add("@CampaignID", iCampaignID);
                sPeriods.Add("@PeriodID", li.Value);
                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignPeriods";
                else
                    sProcName = "uspDelCMCampaignPeriods";

                Classes.cUtilities.PerformNonQuery(sProcName, sPeriods, "LARPortal", "CampaignEdit");
            }

            sProcName = "";
            foreach (ListItem li in cblHousingTypes.Items)
            {
                SortedList sPeriods = new SortedList();
                sPeriods.Add("@UserID", "-1");
                sPeriods.Add("@CampaignID", iCampaignID);
                sPeriods.Add("@HousingTypeID", li.Value);
                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignHousingTypes";
                else
                    sProcName = "uspDelCMCampaignHousingTypes";

                Classes.cUtilities.PerformNonQuery(sProcName, sPeriods, "LARPortal", "CampaignEdit");
            }

            sProcName = "";
            foreach (ListItem li in cblPaymentTypes.Items)
            {
                SortedList sPeriods = new SortedList();
                sPeriods.Add("@UserID", "-1");
                sPeriods.Add("@CampaignID", iCampaignID);
                sPeriods.Add("@PaymentTypeID", li.Value);

                if (li.Selected)
                    sProcName = "uspInsUpdCMCampaignPaymentTypes";
                else
                    sProcName = "uspDelCMCampaignPaymentTypes";

                Classes.cUtilities.PerformNonQuery(sProcName, sPeriods, "LARPortal", "CampaignEdit");
            }


            DataTable dtPools = (DataTable)ViewState["Pools"];
            if (dtPools != null)
            {
                foreach (DataRow dRow in dtPools.Rows)
                {
                    SortedList sPools = new SortedList();
                    int CampaignSkillPoolID;
                    if (int.TryParse(dRow["CampaignSkillPoolID"].ToString(), out CampaignSkillPoolID))
                    {
                        //  If PoolID < 0, it means it's a new record.
                        if (CampaignSkillPoolID < 0)
                        {
                            CampaignSkillPoolID = -1;
                            if (dRow["Deleted"].ToString() == "1")
                                continue;
                        }
                        if (dRow["Deleted"].ToString().Equals("1"))
                        {
                            sPools = new SortedList();
                            sPools.Add("@CampaignSkillPoolID", CampaignSkillPoolID);
                            sPools.Add("@UserID", 2);

                            Classes.cUtilities.PerformNonQuery("uspDelCMCampaignSkillPools", sPools, "LARPortal", "EditCampaignInfo.aspx");
                        }
                        else
                        {
                            sPools = new SortedList();
                            sPools.Add("@CampaignSkillPoolsID", CampaignSkillPoolID);
                            sPools.Add("@CampaignID", iCampaignID);
                            sPools.Add("@PoolDescription", dRow["PoolDescription"].ToString());
                            sPools.Add("@DisplayColor", dRow["DisplayColor"].ToString());
                            bool bTemp;
                            if (bool.TryParse(dRow["DefaultPool"].ToString(), out bTemp))
                                sPools.Add("@DefaultPool", bTemp);
                            else
                                sPools.Add("@DefaultPool", 0);
                            if (bool.TryParse(dRow["SuppressOnCard"].ToString(), out bTemp))
                                sPools.Add("@SuppressOnCard", bTemp);
                            else
                                sPools.Add("@SuppressOnCard", 0);

                            Classes.cUtilities.PerformNonQuery("uspInsUpdCMCampaignSkillPools", sPools, "LARPortal", "EditCampaignInfo.aspx");
                        }
                    }
                }
            }

            if (ViewState["CampaignOppDefs"] != null)
            {

                DataTable dtCampaignOppDefs = (DataTable)ViewState["CampaignOppDefs"];
                foreach (DataRow dRow in dtCampaignOppDefs.Rows)
                {
                    SortedList sObjDefs = new SortedList();
                    string sObjProcName = "";
                    sObjDefs.Add("@CampaignID", iCampaignID);
                    sObjDefs.Add("@ReasonID", dRow["ReasonID"].ToString());
                    sObjDefs.Add("@UserID", 2);
                    bool bHasReason = false;
                    if (bool.TryParse(dRow["HasDefault"].ToString(), out bHasReason))
                    {
                        if (bHasReason)
                        {
                            sObjDefs.Add("@Description", dRow["Description"].ToString());
                            sObjDefs.Add("@OpportunityTypeID", dRow["TypeID"].ToString());
                            double dCPValue = 0.0;
                            if (double.TryParse(dRow["CPValue"].ToString(), out dCPValue))
                                sObjDefs.Add("@CPValue", dCPValue);
                            else
                                sObjDefs.Add("@CPValue", 0);
                            sObjProcName = "uspInsUpdCMCampaignCPOpportunityDefaults";
                        }
                        else
                            sObjProcName = "uspDelCMCampaignCPOpportunityDefaults";
                        Classes.cUtilities.PerformNonQuery(sObjProcName, sObjDefs, "LARPortal", "EditCampaignInfo.aspx");
                    }
                }
            }



            if (hidCampaignDonationSettingsID.Value.Length > 0)
            {
                SortedList sDonationParams = new SortedList();
                sDonationParams.Add("@UserID", 2);
                sDonationParams.Add("@CampaignDonationSettingsID", hidCampaignDonationSettingsID.Value);
                sDonationParams.Add("@CampaignID", iCampaignID);
                sDonationParams.Add("@ShowDonationClaims", cbDonShowDonationClaims.Checked);
                sDonationParams.Add("@DefaultShipToAdd1", tbDonDefaultShipToAdd1.Text);
                sDonationParams.Add("@DefaultShipToAdd2", tbDonDefaultShipToAdd2.Text);
                sDonationParams.Add("@DefaultShipToCity", tbDonDefaultShipToCity.Text);
                sDonationParams.Add("@DefaultShipToState", tbDonDefaultShipToState.Text);
                sDonationParams.Add("@DefaultShipToPostalCode", tbDonDefaultShipToPostalCode.Text);
                sDonationParams.Add("@DefaultShipToPhone", tbDonDefaultShipToPhone.Text);
                sDonationParams.Add("@AllowPlayerToPlayerPoints", cbDonAllowPlayerToPlayerPoints.Checked);
                //   sDonationParams.Add("@DefaultDollarToPointValue", 
                //@DefaultHourToPointValue NUMERIC(18,2) = NULL,
                //@DefaultAwardWhen INT = NULL,
                //                                 @DefaultNotificationEmail                         NVARCHAR(100) = NULL,
                //@MaxPointsPerEvent NUMERIC(18,2) = NULL,
                //@MaxItemsPerEvent NUMERIC(18,2) = NULL,
                //@CountTransfersAgainstMax BIT = NULL,
                //                                 @Comments                                         NVARCHAR(MAX) = NULL)
                Classes.cUtilities.PerformNonQuery("uspInsUpdCMCampaignDonationSettings", sDonationParams, "LARPortal", "EditCampaignDonations");
            }

            string jsString = "alert('The campaign has been updated.');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        "MyApplication",
                    jsString,
                        true);
            ddlCampaignList_SelectedIndexChanged(null, null);
        }


        // The RowEditing event is called when data editing has been requested by the user
        // The EditIndex property should be set to the row index to enter edit mode
        protected void gvPoolData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPoolData.EditIndex = e.NewEditIndex;
            BindPoolData();
        }

        // The RowCancelingEdit event is called when editing is canceled by the user
        // The EditIndex property should be set to -1 to exit edit mode
        protected void gvPoolData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPoolData.EditIndex = -1;
            BindPoolData();
        }

        // The RowUpdating event is called when the Update command is selected by the user
        // The EditIndex property should be set to -1 to exit edit mode
        protected void gvPoolData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int CampaignSkillPoolID = (int)e.Keys["CampaignSkillPoolID"];
            DataTable dtPools = (DataTable)ViewState["Pools"];
            DataView dvPools = new DataView(dtPools, "CampaignSkillPoolID = " + CampaignSkillPoolID.ToString(), "", DataViewRowState.CurrentRows);
            if (dvPools.Count > 0)
            {
                DropDownList ddlDisplayColor = gvPoolData.Rows[gvPoolData.EditIndex].FindControl("ddlDisplayColor") as DropDownList;
                if (ddlDisplayColor != null)
                {
                    dvPools[0]["DisplayColor"] = ddlDisplayColor.SelectedValue;
                }
                dvPools[0]["PoolDescription"] = e.NewValues["PoolDescription"].ToString();
//                dvPools[0]["DisplayColor"] = e.NewValues["DisplayColor"].ToString();
                dvPools[0]["DefaultPool"] = (bool)e.NewValues["DefaultPool"];
                dvPools[0]["SuppressOnCard"] = (bool)e.NewValues["SuppressOnCard"];
                if ((bool)e.NewValues["DefaultPool"] == true)
                {
                    dvPools = new DataView(dtPools, "CampaignSkillPoolID <> " + CampaignSkillPoolID.ToString(), "", DataViewRowState.CurrentRows);
                    foreach (DataRowView dOldRows in dvPools)
                        dOldRows["DefaultPool"] = (bool)false;
                }
            }

            ViewState["Pools"] = dtPools;

            gvPoolData.EditIndex = -1;
            BindPoolData();
        }

        // The RowDeleting event is called when the Delete command is selected by the user
        protected void gvPoolData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int CampaignSkillPoolID = (int)e.Keys["CampaignSkillPoolID"];
            DataTable dtPools = (DataTable)ViewState["Pools"];
            DataView dvPools = new DataView(dtPools, "CampaignSkillPoolID = " + CampaignSkillPoolID.ToString(), "", DataViewRowState.CurrentRows);
            if (dvPools.Count > 0)
            {
                dvPools[0]["Deleted"] = 1;
            }
            ViewState["Pools"] = dtPools;

            BindPoolData();
        }

        private void BindPoolData()
        {
            if (ViewState["Pools"] != null)
            {
                DataTable dtPools = (DataTable)ViewState["Pools"];
                DataView dvPools = new DataView(dtPools, "Deleted = 0", "", DataViewRowState.CurrentRows);
                if (dvPools.Count == 0)
                {
                    DataRow dNewRow = dtPools.NewRow();
                    dNewRow["CampaignSkillPoolID"] = -1000;
                    dNewRow["PoolDescription"] = "Universal";
                    dNewRow["DefaultPool"] = true;
                    dNewRow["DisplayColor"] = "Black";
                    dNewRow["SuppressOnCard"] = false;
                    dNewRow["Deleted"] = 0;
                    dtPools.Rows.Add(dNewRow);
                    ViewState["Pools"] = dtPools;
                    //                    if (dvPools.Count == 0) ;
                    dvPools = new DataView(dtPools, "Deleted = 0", "", DataViewRowState.CurrentRows);
                }
                gvPoolData.DataSource = dvPools;
                gvPoolData.DataBind();
            }

        }

        private void ClearAllObjects()
        {
            ListItem liItem = new ListItem();
            liItem = ddlCampaignList.Items.FindByValue("-1");

            tbCampaignName.Text = ""; ;
            tbStartDate.Text = "";
            tbEndDate.Text = "";
            ddlGameSystem.SelectedIndex = 0;
            ddlMarketingSize.SelectedIndex = 0;
            ddlPrimaryOwner.SelectedIndex = 0;
            ListItem liDefaultOwner = new ListItem();
            if ((liDefaultOwner = ddlPrimaryOwner.Items.FindByValue("2")) != null)
            {
                ddlPrimaryOwner.ClearSelection();
                liDefaultOwner.Selected = true;
            }

            tbPrimaryZipCode.Text = "";
            tbWebDescription.Text = "";
            tbURL.Text = "";
            tbMinAge.Text = "";
            tbMinAgeSuper.Text = "";
            tbCharHistNotification.Text = "";
            cbShowCharacterHistoryEmail.Checked = false;
            tbCharNotification.Text = "";
            cbShowCharacterNotificationEmail.Checked = false;
            tbCPNotification.Text = "";
            cbShowCPNotificationEmail.Checked = false;
            tbInfoRequest.Text = "";
            tbInfoSkill.Text = "";
            cbShowInfoSkillEmail.Checked = false;
            tbJoinRequest.Text = "";
            cbShowJoinRequestEmail.Checked = false;
            tbNotifications.Text = "";
            tbPELNotification.Text = "";
            cbShowPELNotificationEmail.Checked = false;
            tbProductionSkill.Text = "";
            cbShowProductionSkillEmail.Checked = false;
            tbRegNotif.Text = "";
            cbShowRegistrationNotificationEmail.Checked = false;
            cbShowCampaignInfoEmail.Checked = false;
            cblTechLevels.Items.Clear();
            cblWeapons.Items.Clear();
            cblGenre.Items.Clear();
            cblCampaignRoles.Items.Clear();
            cblPeriods.Items.Clear();
            cblHousingTypes.Items.Clear();
            //            cblCampaignStatuses.Items.Clear();
            cblPaymentTypes.Items.Clear();
            cbUserDef1.Checked = false;
            tbUserDef1.Text = "";
            cbUserDef2.Checked = false;
            tbUserDef2.Text = "";
            cbUserDef3.Checked = false;
            tbUserDef3.Text = "";
            cbUserDef4.Checked = false;
            tbUserDef4.Text = "";
            cbUserDef5.Checked = false;
            tbUserDef5.Text = "";
            ViewState.Remove("CampaignOppDefs");
            ViewState.Remove("Pools");
        }

        protected void btnAddCampaign_Click(object sender, EventArgs e)
        {
            ListItem liItem = new ListItem();
            liItem = ddlCampaignList.Items.FindByValue("-1");

            if (liItem == null)
                ddlCampaignList.Items.Add(new ListItem("New Campaign", "-1"));

            liItem = ddlCampaignList.Items.FindByValue("-1");
            ddlCampaignList.ClearSelection();
            liItem.Selected = true;
            ddlCampaignList_SelectedIndexChanged(null, null);
        }

        protected void btnAddPool_Click(object sender, EventArgs e)
        {
            tbNewPoolName.Text = "";
            ddlDisplayColor.SelectedIndex = 0;
            //tbNewPoolColor.Text = "";
            cbDefaultPool.Checked = false;
            cbSuppressOnCard.Checked = false;

            pnlAddPool.Visible = true;
        }

        protected void btnAddPoolRecord_Click(object sender, EventArgs e)
        {
            DataTable dtPools = (DataTable)ViewState["Pools"];
            var minPoolID = dtPools
                    .AsEnumerable()
                    .Min(r => r.Field<int>("CampaignSkillPoolID"));
            DataRow dNewRow = dtPools.NewRow();
            dNewRow["CampaignSkillPoolID"] = (minPoolID > 0 ? -1 : minPoolID - 1);
            dNewRow["PoolDescription"] = tbNewPoolName.Text;
            dNewRow["DefaultPool"] = cbDefaultPool.Checked;
            dNewRow["DisplayColor"] = ddlDisplayColor.SelectedValue;
            dNewRow["SuppressOnCard"] = cbSuppressOnCard.Checked;
            dNewRow["Deleted"] = 0;
            dtPools.Rows.Add(dNewRow);
            ViewState["Pools"] = dtPools;

            pnlAddPool.Visible = false;
            BindPoolData();
        }

        protected void gvOppDefaults_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvOppDefaults.EditIndex = e.NewEditIndex;
            if (ViewState["CampaignOppDefs"] != null)
            {
                DataTable dt = (DataTable)ViewState["CampaignOppDefs"];
                if (dt.Rows[e.NewEditIndex]["CPValue"] == DBNull.Value)
                    dt.Rows[e.NewEditIndex]["CPValue"] = 0.0;
                gvOppDefaults.DataSource = (DataTable)ViewState["CampaignOppDefs"];
                gvOppDefaults.DataBind();
            }
        }

        protected void gvOppDefaults_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvOppDefaults.EditIndex = -1;
            if (ViewState["CampaignOppDefs"] != null)
            {
                gvOppDefaults.DataSource = (DataTable)ViewState["CampaignOppDefs"];
                gvOppDefaults.DataBind();
            }
        }

        protected void gvOppDefaults_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //  Save the key value.
            int ReasonID = (int)e.Keys["ReasonID"];
            //  Save the checkbox.
            string HasDefault = e.NewValues["HasDefault"].ToString();

            //  To get the value of the drop down, we need to get the row from the gridview first.
            GridViewRow row = gvOppDefaults.Rows[e.RowIndex];
            //  Get drop down list.
            DropDownList ddlddlDescriptions = (DropDownList)row.FindControl("ddlDescriptions");
            HiddenField hidOrigDescription = (HiddenField)row.FindControl("OrigDescription");
            string Description = e.NewValues["Description"].ToString();
            if (Description.Length == 0)
                Description = hidOrigDescription.Value;

            //  Get the viewstate table so we can manipulate it.
            DataTable dtCampaignOppDefs = new DataTable();
            if (ViewState["CampaignOppDefs"] != null)
            {
                dtCampaignOppDefs = (DataTable)ViewState["CampaignOppDefs"];
                DataView dvFoundRow = new DataView(dtCampaignOppDefs, "ReasonID = " + ReasonID.ToString(), "", DataViewRowState.CurrentRows);
                if (dvFoundRow.Count > 0)
                {
                    //  Set the values in the found row.
                    dvFoundRow[0]["HasDefault"] = HasDefault;
                    dvFoundRow[0]["Description"] = Description;
                    dvFoundRow[0]["OpportunityDescription"] = ddlddlDescriptions.SelectedItem.Text;
                    dvFoundRow[0]["TypeID"] = ddlddlDescriptions.SelectedValue;
                    dvFoundRow[0]["CPValue"] = e.NewValues["CPValue"].ToString();

                    //  Save the updated values back to the viewstate.
                    ViewState["CampaignOppDefs"] = dtCampaignOppDefs;

                    //  Take viewstate out of edit mode.
                    gvOppDefaults.EditIndex = -1;
                    gvOppDefaults.DataSource = dtCampaignOppDefs;
                    gvOppDefaults.DataBind();
                }
            }
        }

        protected void gvOppDefaults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    //  Gridview is in edit mode.
                    DropDownList ddlDescriptions = (DropDownList)(e.Row.FindControl("ddlDescriptions"));
                    HiddenField hidTypeID = (HiddenField)(e.Row.FindControl("hidTypeID"));
                    if ((ddlDescriptions != null) &&
                        (hidTypeID != null))
                    {
                        //  Have both the drop down list plus the hidden field that is the currently selected value.
                        //  Bind the drop down list.
                        ddlDescriptions.DataSource = (DataTable)ViewState["OppDesc"];
                        ddlDescriptions.DataTextField = "OpportunityDescription";
                        ddlDescriptions.DataValueField = "CampaignCPOpportunityTypeID";
                        ddlDescriptions.DataBind();

                        //  Select the current value.
                        foreach (ListItem li in ddlDescriptions.Items)
                        {
                            if (li.Value == hidTypeID.Value)
                            {
                                ddlDescriptions.ClearSelection();
                                li.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        protected void gvPoolData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlDisplayColor = e.Row.FindControl("ddlDisplayColor") as DropDownList;
                    ddlDisplayColor.SelectedValue = DataBinder.Eval(e.Row.DataItem, "DisplayColor").ToString();
                }
            }
        }

        protected void gvShares_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvShares_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvShares_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvShares_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
    }
}
