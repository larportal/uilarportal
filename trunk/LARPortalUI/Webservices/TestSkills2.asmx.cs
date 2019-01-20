using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace LarpPortal.Webservices
{
    /// <summary>
    /// CampaignInfo is a web service with assorted routines for getting information about campaigns.
    /// </summary>
    [WebService(Namespace = "http://www.LARPortal.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class TestSkill : System.Web.Services.WebService
    {
        /// <summary>
        /// Give a skill ID, this will return the string to display about the skill.
        /// </summary>
        /// <param name="SkillNodeID">The campaign ID to get the information about.</param>
        /// <returns>HTML formatted string about the skill that can be put in a div to display to the user.</returns>
        [WebMethod(Description = "Get the corresponding description for a skill by ID.")]
        public string GetSkillNodeInfo(int SkillNodeID)
        {
			string sCampaignInfo = "Nothing Returned.";
			try
			{
				DataSet dsResults = new DataSet();

				using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
				{
					using (SqlCommand Cmd = new SqlCommand("TestSkills.dbo.uspGetCampaignSkillByNodeID", Conn))
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
					sCampaignInfo = "<b>Skill Name: " + dRow["SkillName"].ToString() + "</b><br>Skill Description:" +
						dRow["SkillShortDescription"].ToString() + "<br><br>" +
						"Cost: ";

					sCampaignInfo += @"<span style=""color: " + dRow["DisplayColor"].ToString() + @""">";

					if (dRow["SkillCPCost"] != DBNull.Value)
						sCampaignInfo += dRow["SkillCPCost"].ToString();

					bool bDefault = false;
					if (bool.TryParse(dRow["DefaultPool"].ToString(), out bDefault))
						if (!bDefault)
							sCampaignInfo += " " + dRow["PoolDescription"].ToString();

					sCampaignInfo += "</span>";
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
						int.TryParse(dGroupRow["PrerequisiteGroupID"].ToString(), out iGroupID);
						int.TryParse(dGroupRow["NumGroupSkillsRequired"].ToString(), out iNumItems);

						DataView dvGroupSkills = new DataView(dsResults.Tables[2], "PrerequisiteGroupID = " + iGroupID.ToString(), "DisplayOrder", DataViewRowState.CurrentRows);
						if (dvGroupSkills.Count > 0)
						{
							sCampaignInfo += "<br><br>";
							for (int i = 0; i < dvGroupSkills.Count; i++)
							{
								if (i == 0)
									sCampaignInfo += "You have to have " + iNumItems.ToString() + " of the following skills to purchase this: " + dvGroupSkills[i]["SkillName"].ToString();
								else
									sCampaignInfo += ", " + dvGroupSkills[i]["SkillName"].ToString();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				sCampaignInfo = ex.Message;
			}
            return sCampaignInfo;
        }
    }
}
