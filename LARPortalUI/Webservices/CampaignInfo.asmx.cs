﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace LarpPortal.Webservices
{
    /// <summary>
    /// CampaignInfo is a web service with assorted routines for getting information about campaigns.
    /// </summary>
    [WebService(Namespace = "http://www.LARPortal.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class CampaignInfoService : System.Web.Services.WebService
    {
        /// <summary>
        /// Give a skill ID, this will return the string to display about the skill.
        /// </summary>
        /// <param name="SkillNodeID">The campaign ID to get the information about.</param>
        /// <returns>HTML formatted string about the skill that can be put in a div to display to the user.</returns>
        [WebMethod(Description = "Get the corresponding description for a skill by ID.")]
        public string GetSkillNodeInfo(int SkillNodeID)
        {
            string sCampaignInfo = "";
            DataSet dsResults = new DataSet();

            using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            {
                using (SqlCommand Cmd = new SqlCommand("uspGetCampaignSkillByNodeID", Conn))
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@SkillNodeID", SkillNodeID);
                    SqlDataAdapter SDAGetData = new SqlDataAdapter(Cmd);

                    Conn.Open();
                    SDAGetData.Fill(dsResults);
                }
            }
            foreach (DataRow dRow in dsResults.Tables[0].Rows)
            {
                // If you want to display different information about the skill, this is where you would change it.
                sCampaignInfo = "<b>" + dRow["SkillName"].ToString() + "</b><br>" +
                    dRow["SkillShortDescription"].ToString() + "<br><br>" +
                    "Cost: ";

                //if (dsResults.Tables.Count < 5)
                //{
                //sCampaignInfo += @"<span style=""color: " + dRow["DisplayColor"].ToString() + @""">";

                //if (dRow["SkillCPCost"] != DBNull.Value)
                //    sCampaignInfo += dRow["SkillCPCost"].ToString();

                //bool bDefault = false;
                //if (bool.TryParse(dRow["DefaultPool"].ToString(), out bDefault))
                //    if (!bDefault)
                //        sCampaignInfo += " " + dRow["PoolDescription"].ToString();

                //sCampaignInfo += "</span>";
                //}
                //else
                //{
                    if (dsResults.Tables[4].Rows.Count == 1)
                    {
                        sCampaignInfo += @"<span style=""color: " + dsResults.Tables[4].Rows[0]["DisplayColor"].ToString() + @""">";
                        if (dsResults.Tables[4].Rows[0]["SkillCPCost"] != DBNull.Value)
                            sCampaignInfo += dsResults.Tables[4].Rows[0]["SkillCPCost"].ToString();

                        bool bDefault = false;
                        if (bool.TryParse(dsResults.Tables[4].Rows[0]["DefaultPool"].ToString(), out bDefault))
                            if (!bDefault)
                                sCampaignInfo += " " + dsResults.Tables[4].Rows[0]["PoolDescription"].ToString();

                        sCampaignInfo += "</span>";
                    }
                    else
                    {
                        string sPaddingString = " style='padding-left: 5px; padding-right: 5px'";
                        string sPaddingRight = " style='padding-left: 5px; padding-right: 5px; text-align: right;'";
                        sCampaignInfo += "<table border=1><tr><th" + sPaddingString + ">Pool</th><th" + sPaddingString + ">Cost</th></tr>";
                        foreach (DataRow dCost in dsResults.Tables[4].Rows)
                        {
                            sCampaignInfo += "<tr>";
                            sCampaignInfo += "<td" + sPaddingString + ">";

                            sCampaignInfo += @"<span style=""color: " + dCost["DisplayColor"].ToString() + @""">";

                            //bool bDefault = false;
                            //if (bool.TryParse(dCost["DefaultPool"].ToString(), out bDefault))
                            //    if (!bDefault)
                            sCampaignInfo += " " + dCost["PoolDescription"].ToString();
                            sCampaignInfo += "</span></td><td" + sPaddingRight + " >";

                            sCampaignInfo += @"<span style=""color: " + dCost["DisplayColor"].ToString() + @""">";
                            if (dCost["SkillCPCost"] != DBNull.Value)
                                sCampaignInfo += dCost["SkillCPCost"].ToString();
                            sCampaignInfo += "</span></td>";
                            sCampaignInfo += "</tr>";
                        }
                        sCampaignInfo += "</table>";
                    }
                //}
                sCampaignInfo += "<br><br>" + dRow["SkillLongDescription"].ToString();
            }

            DataView dvSkills = new DataView(dsResults.Tables[1], "PrerequisiteSkillNodeID is not null and ExcludeFromPurchase = false", "", DataViewRowState.CurrentRows);
            DataTable dtSkillList = dvSkills.ToTable(true, "SkillName");

            if (dtSkillList.Rows.Count > 0)
            {
                sCampaignInfo += "<br><br>";
                DataView dvSortedFields = new DataView(dtSkillList, "", "SkillName", DataViewRowState.CurrentRows);
                for (int i = 0; i < dvSortedFields.Count; i++)
                {
                    if (i == 0)
                        sCampaignInfo += "This skill requires you already have " + dvSortedFields[i]["SkillName"].ToString();
                    else
                        sCampaignInfo += ", " + dvSortedFields[i]["SkillName"].ToString();
                }
            }

            dvSkills = new DataView(dsResults.Tables[1], "PrerequisiteSkillNodeID is not null and ExcludeFromPurchase = true", "", DataViewRowState.CurrentRows);
            dtSkillList = dvSkills.ToTable(true, "SkillName");

            if (dtSkillList.Rows.Count > 0)
            {
                sCampaignInfo += "<br><br>";
                DataView dvSortedFields = new DataView(dtSkillList, "", "SkillName", DataViewRowState.CurrentRows);
                for (int i = 0; i < dvSortedFields.Count; i++)
                {
                    if (i == 0)
                        sCampaignInfo += "You cannot buy this skill if you already have " + dvSortedFields[i]["SkillName"].ToString();
                    else
                        sCampaignInfo += ", " + dvSortedFields[i]["SkillName"].ToString();
                }
            }

            dvSkills = new DataView(dsResults.Tables[1], "PrerequisiteGroupID is not null", "", DataViewRowState.CurrentRows);
            if (dvSkills.Count > 0)
            {
                foreach (DataRowView dGroupRow in dvSkills)
                {
                    int iGroupID;
                    int iNumItems;
                    double iNumPoints;
                    int.TryParse(dGroupRow["PrerequisiteGroupID"].ToString(), out iGroupID);
                    int.TryParse(dGroupRow["NumGroupSkillsRequired"].ToString(), out iNumItems);
                    double.TryParse(dGroupRow["NumPointsRequired"].ToString(), out iNumPoints);

                    DataView dvGroupSkills = new DataView(dsResults.Tables[2], "PrerequisiteGroupID = " + iGroupID.ToString(), "DisplayOrder", DataViewRowState.CurrentRows);
                    if (dvGroupSkills.Count > 0)
                    {
                        sCampaignInfo += "<br><br>";
                        for (int i = 0; i < dvGroupSkills.Count; i++)
                        {
                            if (i == 0)
                            {
                                if (iNumItems > 0)
                                    sCampaignInfo += "You have to have " + iNumItems.ToString() + " of the following skills to purchase this: ";
                                else
                                    sCampaignInfo += "You have to spend " + iNumPoints.ToString() + " points on the following skills to purchase this: ";
                                sCampaignInfo += dvGroupSkills[i]["SkillName"].ToString();
                            }
                            else
                                sCampaignInfo += ", " + dvGroupSkills[i]["SkillName"].ToString();
                        }
                    }
                }
            }
            return sCampaignInfo;
        }



		[WebMethod(Description = "Get the values for the additional description.")]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string GetSkillValues(int CampaignSkillNodeID)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			Values RetVal = new Values { CampaignSkillNodeID = -1, DataType = "", CurrentValue = "" };

			SortedList sParams = new SortedList();
			sParams.Add("@CampaignSkillNodeID", CampaignSkillNodeID);
			DataTable dtValues = Classes.cUtilities.LoadDataTable("uspGetCharacterSkillAddValuesList", sParams, 
				"LARPortal", "WebService", lsRoutineName);

			if (dtValues.Rows.Count > 0)
			{
				int iTemp;
				if (int.TryParse(dtValues.Rows[0]["CampaignSkillNodeID"].ToString(), out iTemp))
					RetVal.CampaignSkillNodeID = iTemp;
				if (int.TryParse(dtValues.Rows[0]["AddInfoType"].ToString(), out iTemp))
				{
					if (iTemp == 1)
					{
						RetVal.DataType = "Text";
					}
					else if (iTemp == 2)
					{
						RetVal.DataType = "DropDown";
						List<string> AvailValues = new List<string>();
						DataView dvValue = new DataView(dtValues, "", "SortOrder", DataViewRowState.CurrentRows);
						foreach (DataRowView dv in dvValue)
						{
							AvailValues.Add(dv["DisplayValue"].ToString());
						}
						RetVal.AvailValues = AvailValues.ToArray();
					}
				}
			}

			JavaScriptSerializer js = new JavaScriptSerializer();
			string strJSON = js.Serialize(RetVal);
			return strJSON;
		}



        /// <summary>
        /// Give a skill ID, this will return the string to display about the skill.
        /// </summary>
        /// <param name="SkillNodeID">The campaign ID to get the information about.</param>
        /// <returns>HTML formatted string about the skill that can be put in a div to display to the user.</returns>
        [WebMethod(Description = "Get the corresponding description for a skill by ID.")]
        public string GetFormattedSkillNodeInfo(int SkillNodeID)
        {
            string sCampaignInfo = "";
            DataSet dsResults = new DataSet();

            using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            {
                using (SqlCommand Cmd = new SqlCommand("uspGetCampaignSkillByNodeIDFormatted", Conn))
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@SkillNodeID", SkillNodeID);
                    SqlDataAdapter SDAGetData = new SqlDataAdapter(Cmd);

                    Conn.Open();
                    SDAGetData.Fill(dsResults);
                }
            }
            foreach (DataRow dRow in dsResults.Tables[0].Rows)
            {
                sCampaignInfo = dRow[0].ToString();
                //// If you want to display different information about the skill, this is where you would change it.
                //sCampaignInfo = "<b>" + dRow["SkillName"].ToString() + "</b><br>" +
                //    dRow["SkillShortDescription"].ToString() + "<br><br>" +
                //    "Cost: ";

                //if (dsResults.Tables.Count < 5)
                //{
                //sCampaignInfo += @"<span style=""color: " + dRow["DisplayColor"].ToString() + @""">";

                //if (dRow["SkillCPCost"] != DBNull.Value)
                //    sCampaignInfo += dRow["SkillCPCost"].ToString();

                //bool bDefault = false;
                //if (bool.TryParse(dRow["DefaultPool"].ToString(), out bDefault))
                //    if (!bDefault)
                //        sCampaignInfo += " " + dRow["PoolDescription"].ToString();

                //sCampaignInfo += "</span>";
                //}
                //else
                //{



                //if (dsResults.Tables[4].Rows.Count == 1)
                //{
                //    sCampaignInfo += @"<span style=""color: " + dsResults.Tables[4].Rows[0]["DisplayColor"].ToString() + @""">";
                //    if (dsResults.Tables[4].Rows[0]["SkillCPCost"] != DBNull.Value)
                //        sCampaignInfo += dsResults.Tables[4].Rows[0]["SkillCPCost"].ToString();

                //    bool bDefault = false;
                //    if (bool.TryParse(dsResults.Tables[4].Rows[0]["DefaultPool"].ToString(), out bDefault))
                //        if (!bDefault)
                //            sCampaignInfo += " " + dsResults.Tables[4].Rows[0]["PoolDescription"].ToString();

                //    sCampaignInfo += "</span>";
                //}
                //else
                //{
                //    string sPaddingString = " style='padding-left: 5px; padding-right: 5px'";
                //    string sPaddingRight = " style='padding-left: 5px; padding-right: 5px; text-align: right;'";
                //    sCampaignInfo += "<table border=1><tr><th" + sPaddingString + ">Pool</th><th" + sPaddingString + ">Cost</th></tr>";
                //    foreach (DataRow dCost in dsResults.Tables[4].Rows)
                //    {
                //        sCampaignInfo += "<tr>";
                //        sCampaignInfo += "<td" + sPaddingString + ">";

                //        sCampaignInfo += @"<span style=""color: " + dCost["DisplayColor"].ToString() + @""">";

                //        //bool bDefault = false;
                //        //if (bool.TryParse(dCost["DefaultPool"].ToString(), out bDefault))
                //        //    if (!bDefault)
                //        sCampaignInfo += " " + dCost["PoolDescription"].ToString();
                //        sCampaignInfo += "</span></td><td" + sPaddingRight + " >";

                //        sCampaignInfo += @"<span style=""color: " + dCost["DisplayColor"].ToString() + @""">";
                //        if (dCost["SkillCPCost"] != DBNull.Value)
                //            sCampaignInfo += dCost["SkillCPCost"].ToString();
                //        sCampaignInfo += "</span></td>";
                //        sCampaignInfo += "</tr>";
                //    }
                //    sCampaignInfo += "</table>";
                //}
                //}






//                sCampaignInfo += "<br><br>" + dRow["SkillLongDescription"].ToString();
            }
            return sCampaignInfo;
        }
    }

    public class Values
	{
		public int CampaignSkillNodeID;
		public string DataType;
		public string CurrentValue;
		public string[] AvailValues;
	}
}
