using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;


//   Jbradshaw  6/19/2016  Changed over to new style of definitions for variables. No need for local variables anymore.
//                         Made changes required for user profiles.
//   JBradshaw  3/31/2017  Changes for LARP Resume. Also made the supporting tables loaded in the original SP so it's faster.

namespace LarpPortal.Classes
{
    public class cPlayer
    {
        public Int32 PictureID { get; set; }
        public Int32 UserID { get; set; }
        public string UserName { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public String AuthorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderStandared { get; set; }
        public string GenderOther { get; set; }
        public string EmergencyContactName { get; set; }
        public Int32 EmergencyContactPhoneID { get; set; }
        public string EmergencyContactPhone { get; set; }
        public Int32 MaxNumberOfEventsPerYear { get; set; }
        public string CPPreferenceDefault { get; set; }
        public Int32 CPDestinationDefault { get; set; }
        public Int32 PhotoPreference { get; set; }
        public string PhotoPreferenceDescription { get; set; }
        public cPicture Picture { get; set; }
        public string UserPhoto { get; set; }
        public Boolean SearchableProfile { get; set; }
        public string BackGroundKnowledge { get; set; }
        public string LinkedInURL { get; set; }
        public string Allergies { get; set; }               // JBradshaw 4/27/2017
        public string Comments { get; set; }
        public string LARPResumeComments { get; set; }
        public string ResumeComments { get; set; }
        public string MedicalComments { get; set; }         // JBradshaw 4/27/2017
        /// <summary>
        /// 0 to 100 max out at 100.
        /// </summary>
        private Int32 _RolePlayPercentage { get; set; }  // = 0; // 
        /// <summary>
        /// 0 to 100 max out at 100.
        /// </summary>
        private Int32 _CombatPercentage { get; set; }
        /// <summary>
        /// In days.
        /// </summary>
        public Int32 WriteUpLeadTimeNeeded { get; set; }
        /// <summary>
        /// In pages.
        /// </summary>
        public Int32 WriteUpLengthPreference { get; set; }
        public List<cPlayerInventory> PlayerInventoryItems = new List<cPlayerInventory>();
        public List<cPlayerLARPResume> PlayerLARPResumes = new List<cPlayerLARPResume>();
        public List<cPlayerOccasionExceptions> PlayerOccasionExceptions = new List<cPlayerOccasionExceptions>();
        public List<cPlayerSkill> PlayerSkills = new List<cPlayerSkill>();
        public List<cPlayerWaiver> PlayerWaivers = new List<cPlayerWaiver>();
        public List<cPlayerAffiliation> PlayerAffiliations = new List<cPlayerAffiliation>();
        public List<cPlayerMedical> PlayerMedical = new List<cPlayerMedical>();
        public List<cPlayerLimitation> PlayerLimitation = new List<cPlayerLimitation>();
        public DateTime DateAdded;
        public DateTime DateChanged;

        public Int32 RolePlayPercentage
        {
            get { return _RolePlayPercentage; }
            set
            {
                if (value > -1 && value < 101)
                {
                    if (value + _CombatPercentage != 100)
                    {
                        _RolePlayPercentage = value;
                        _CombatPercentage = 100 - _RolePlayPercentage;
                    }
                    else
                    { _RolePlayPercentage = value; }
                }
                else
                { _RolePlayPercentage = 0; }
            }
        }
        public Int32 CombatPercentage
        {
            get { return _CombatPercentage; }
            set
            {
                if (value > -1 && value < 101)
                {
                    if (value + _RolePlayPercentage != 100)
                    {
                        _CombatPercentage = value;
                        _RolePlayPercentage = 100 - _CombatPercentage;
                    }
                    else
                    { _CombatPercentage = value; }
                }
                else
                {
                    _CombatPercentage = 0;
                }
            }
        }

        public bool HasPicture
        {
            get
            {
                return PictureID > 0;        // If PictureID != -1 then we have a picture.
            }
        }

        private cPlayer()
        {       // JBradshaw  7/11/2016  Added to make sure the values are defined.
            Picture = new cPicture();
            UserID = -1;
            PlayerProfileID = -1;
            AuthorName = "";
            DateOfBirth = DateTime.Now;
            GenderStandared = "";
            GenderOther = "";
            EmergencyContactName = "";
            EmergencyContactPhoneID = -1;
            EmergencyContactPhone = "";
            MaxNumberOfEventsPerYear = 0;
            CPPreferenceDefault = "";
            CPDestinationDefault = -1;
            PhotoPreference = -1;
            PhotoPreferenceDescription = "";
            UserPhoto = "";
            SearchableProfile = false;
            _RolePlayPercentage = 0;
            _CombatPercentage = 0;
            WriteUpLeadTimeNeeded = 0;
            WriteUpLengthPreference = 0;
            BackGroundKnowledge = "";
            LinkedInURL = "";
            Allergies = "";
            Comments = "";
            LARPResumeComments = "";
            ResumeComments = "";
            MedicalComments = "";
            DateAdded = DateTime.Now;
            DateChanged = DateTime.Now;
        }

        public cPlayer(Int32 intUserId, string strUserName)
        {
            Picture = new cPicture();
            UserID = -1;
            PlayerProfileID = -1;
            AuthorName = "";
            DateOfBirth = DateTime.Now;
            GenderStandared = "";
            GenderOther = "";
            EmergencyContactName = "";
            EmergencyContactPhoneID = -1;
            EmergencyContactPhone = "";
            MaxNumberOfEventsPerYear = 0;
            CPPreferenceDefault = "";
            CPDestinationDefault = -1;
            PhotoPreference = -1;
            PhotoPreferenceDescription = "";
            UserPhoto = "";
            SearchableProfile = false;
            _RolePlayPercentage = 0;
            _CombatPercentage = 0;
            WriteUpLeadTimeNeeded = 0;
            WriteUpLengthPreference = 0;
            BackGroundKnowledge = "";
            LinkedInURL = "";
            Allergies = "";
            Comments = "";
            MedicalComments = "";
            DateAdded = DateTime.Now;
            DateChanged = DateTime.Now;

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            UserID = intUserId;
            UserName = strUserName;
            SortedList slParams = new SortedList();
            slParams.Add("@UserID", UserID);
            try
            {
                DataSet lds = cUtilities.LoadDataSet("uspGetPlayerProfileByUserID", slParams, "LARPortal", strUserName, lsRoutineName);
                if (lds.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = lds.Tables[0].Rows[0];
                    PlayerProfileID = dRow["PlayerProfileID"].ToString().ToInt32();
                    AuthorName = dRow["AuthorName"].ToString();
                    DateOfBirth = Convert.ToDateTime(dRow["DateOfBirth"].ToString());
                    GenderStandared = dRow["GenderStandard"].ToString();
                    GenderOther = dRow["GenderOther"].ToString();
                    EmergencyContactName = dRow["EmergencyContactName"].ToString();
                    EmergencyContactPhone = dRow["EmergencyContactPhone"].ToString();
                    MaxNumberOfEventsPerYear = dRow["MaxNumberEventsPerYear"].ToString().ToInt32();
                    CPPreferenceDefault = dRow["CPPreferenceDefault"].ToString();
                    CPDestinationDefault = dRow["CPDestinationDefault"].ToString().ToInt32();
                    PhotoPreference = dRow["PhotoPreference"].ToString().ToInt32();
                    UserPhoto = dRow["UserPhoto"].ToString();
                    int iTemp = 0;
                    if (int.TryParse(dRow["PlayerPictureID"].ToString(), out iTemp))
                        PictureID = iTemp;
                    else
                        Picture.PictureID = -1;

                    if (PictureID > 0)
                        Picture.Load(PictureID, intUserId.ToString());

                    SearchableProfile = dRow["SearchableProfile"].ToString().ToBoolean();
                    RolePlayPercentage = dRow["RoleplayPercentage"].ToString().ToInt32();
                    CombatPercentage = dRow["CombatPercentage"].ToString().ToInt32();
                    WriteUpLeadTimeNeeded = dRow["WriteUpLeadTimeNeeded"].ToString().ToInt32();
                    WriteUpLengthPreference = dRow["WriteUpLengthPreference"].ToString().ToInt32();
                    BackGroundKnowledge = dRow["BackgroundKnowledge"].ToString();
                    LinkedInURL = dRow["LinkedInURL"].ToString();
                    Allergies = dRow["Allergies"].ToString();
                    Comments = dRow["Comments"].ToString();
                    LARPResumeComments = dRow["LARPResumeComments"].ToString();
                    ResumeComments = dRow["ResumeComments"].ToString();
                    MedicalComments = dRow["MedicalComments"].ToString();
                    DateAdded = Convert.ToDateTime(dRow["DateAdded"].ToString());
                    DateChanged = Convert.ToDateTime(dRow["DateChanged"].ToString());

                    // Cleaning up the Phone Number.
                    EmergencyContactPhone = EmergencyContactPhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "");
                }
                LoadInventory(lds.Tables[1]);
                LoadLARPResumes(lds.Tables[2]);
                LoadOccasionExceptions(lds.Tables[3]);
                LoadSkills(lds.Tables[4]);
                LoadWaivers(lds.Tables[5]);
                LoadAffiliations(lds.Tables[6]);
                LoadMedical(lds.Tables[7]);
                LoadLimitations(lds.Tables[8]);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName + lsRoutineName);
            }
        }

        public Boolean Save()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean blnReturn = false;
            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@PlayerProfileID", PlayerProfileID);
                slParams.Add("@UserID", UserID);
                slParams.Add("@AuthorName", AuthorName);
                slParams.Add("@DateOfBirth", DateOfBirth);

                slParams.Add("@GenderStandard", GenderStandared);
                slParams.Add("@GenderOther", GenderOther);
                slParams.Add("@EmergencyContactName", EmergencyContactName);
                slParams.Add("@EmergencyContactPhone", EmergencyContactPhone);
                slParams.Add("@MaxNumberEventsPerYear", MaxNumberOfEventsPerYear);
                slParams.Add("@CPPreferenceDefault", CPPreferenceDefault);
                slParams.Add("@CPDestinationDefault", CPDestinationDefault);
                slParams.Add("@PhotoPreference", PhotoPreference);
                slParams.Add("@UserPhoto", UserPhoto);

                slParams.Add("@PlayerPictureID", PictureID);
                slParams.Add("@SearchableProfile", SearchableProfile);
                slParams.Add("@RoleplayPercentage", _RolePlayPercentage);
                slParams.Add("@CombatPercentage", _CombatPercentage);
                slParams.Add("@WriteUpLeadTimeNeeded", WriteUpLeadTimeNeeded);
                slParams.Add("@WriteUpLengthPreference", WriteUpLengthPreference);
                slParams.Add("@BackgroundKnowledge", BackGroundKnowledge);
                slParams.Add("@LinkedInURL", LinkedInURL);
                slParams.Add("@Allergies", Allergies);
                slParams.Add("@Comments", Comments);
                slParams.Add("@LARPResumeComments", LARPResumeComments);
                slParams.Add("@ResumeComments", ResumeComments);
                slParams.Add("@MedicalComments", MedicalComments);
                blnReturn = cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerProfiles", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                blnReturn = false;
            }
            return blnReturn;
        }

        private void LoadInventory(DataTable dtInventory)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtInventory.Rows)
                {
                    cPlayerInventory cItem = new cPlayerInventory
                    {
                        ItemName = ldr["ItemName"].ToString(),
                        Description = ldr["Description"].ToString(),
                        InventoryTypeDesc = ldr["InventoryTypeDescription"].ToString(),
                        Quantity = ldr["Quantity"].ToString(),
                        Size = ldr["Size"].ToString(),
                        PowerNeeded = ldr["PowerNeeded"].ToString(),
                        Location = ldr["Location"].ToString(),
                        InventoryNotes = ldr["InventoryNotes"].ToString(),
                        ImageURL = ldr["ImageURL"].ToString(),
                        PlayerComments = ldr["PlayerComments"].ToString(),
                        Comments = ldr["Comments"].ToString()
                    };

                    int iTemp;
                    bool bTemp;

                    if (int.TryParse(ldr["PlayerInventoryID"].ToString(), out iTemp))
                        cItem.PlayerInventoryID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cItem.PlayerProfileID = iTemp;
                    if (int.TryParse(ldr["InventoryTypeID"].ToString(), out iTemp))
                        cItem.InventoryTypeID = iTemp;
                    if (int.TryParse(ldr["ImageID"].ToString(), out iTemp))
                    {
                        cItem.ImageID = iTemp;
                        cItem.InvImage.Load(iTemp, "");
                        cItem.ImageURL = cItem.InvImage.PictureURL;
                    }

                    if (bool.TryParse(ldr["WillShare"].ToString(), out bTemp))
                        cItem.WillShare = bTemp;

                    PlayerInventoryItems.Add(cItem);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadLARPResumes(DataTable dtLARPResume)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtLARPResume.Rows)
                {
                    cPlayerLARPResume cAdd = new cPlayerLARPResume
                    {
                        GameSystem = ldr["GameSystem"].ToString(),
                        Campaign = ldr["Campaign"].ToString(),
                        AuthorGM = ldr["AuthorGM"].ToString(),
                        Style = ldr["StyleName"].ToString(),
                        Genre = ldr["GenreName"].ToString(),
                        Role = ldr["RoleDescription"].ToString(),
                        PlayerComments = ldr["PlayerComments"].ToString(),
                        Comments = ldr["Comments"].ToString(),
                        RecordStatus = RecordStatuses.Active
                    };

                    int iTemp;
                    DateTime dtTemp;
                    if (int.TryParse(ldr["PlayerLARPResumeID"].ToString(), out iTemp))
                        cAdd.PlayerLARPResumeID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cAdd.PlayerProfileID = iTemp;
                    if (int.TryParse(ldr["StyleID"].ToString(), out iTemp))
                        cAdd.StyleID = iTemp;
                    if (int.TryParse(ldr["GenreID"].ToString(), out iTemp))
                        cAdd.GenreID = iTemp;
                    if (int.TryParse(ldr["RoleID"].ToString(), out iTemp))
                        cAdd.RoleID = iTemp;
                    if (DateTime.TryParse(ldr["StartDate"].ToString(), out dtTemp))
                        cAdd.StartDate = dtTemp;
                    else
                        cAdd.StartDate = null;
                    if (DateTime.TryParse(ldr["EndDate"].ToString(), out dtTemp))
                        cAdd.EndDate = dtTemp;
                    else
                        cAdd.EndDate = null;

                    PlayerLARPResumes.Add(cAdd);
                }
            }

            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadOccasionExceptions(DataTable dtOccasionExcept)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtOccasionExcept.Rows)
                {
                    cPlayerOccasionExceptions cAdd =
                            new cPlayerOccasionExceptions
                            {
                                PlayerComments = ldr["PlayerComments"].ToString(),
                                Comments = ldr["Comments"].ToString(),
                                RecordStatus = RecordStatuses.Active
                            };

                    int iTemp;
                    if (int.TryParse(ldr["PlayerOccasionExceptionID"].ToString(), out iTemp))
                        cAdd.PlayerOccasionExceptionID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cAdd.PlayerProfileID = iTemp;
                    if (int.TryParse(ldr["CampaignID"].ToString(), out iTemp))
                        cAdd.CampaignID = iTemp;
                    if (int.TryParse(ldr["OccasionID"].ToString(), out iTemp))
                        cAdd.OccasionID = iTemp;

                    bool bTemp;
                    if (bool.TryParse(ldr["AttendPartial"].ToString(), out bTemp))
                        cAdd.AttendPartial = bTemp;

                    PlayerOccasionExceptions.Add(cAdd);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadSkills(DataTable dtSkills)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtSkills.Rows)
                {
                    cPlayerSkill cAdd = new
                        cPlayerSkill
                        {
                            SkillName = ldr["SkillName"].ToString(),
                            SkillLevel = ldr["SkillLevel"].ToString(),
                            PlayerComments = ldr["PlayerComments"].ToString(),
                            Comments = ldr["Comments"].ToString(),
                            RecordStatus = RecordStatuses.Active
                        };

                    int iTemp;
                    if (int.TryParse(ldr["PlayerSkillID"].ToString(), out iTemp))
                        cAdd.PlayerSkillID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cAdd.PlayerProfileID = iTemp;

                    PlayerSkills.Add(cAdd);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadWaivers(DataTable dtWaivers)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtWaivers.Rows)
                {
                    cPlayerWaiver oWaiver = new
                        cPlayerWaiver
                        {
                            WaiverImage = ldr["WaiverImage"].ToString(),
                            PlayerNotes = ldr["PlayerNotes"].ToString(),
                            StaffNotes = ldr["StaffNotes"].ToString(),
                            Comments = ldr["Comments"].ToString(),
                            WaiverText = ldr["WaiverText"].ToString(),
                            WaiverNotes = ldr["WaiverNotes"].ToString(),
                            WaiverComments = ldr["WaiverComments"].ToString(),
                            WaiverType = ldr["WaiverTypeDescription"].ToString(),
                            WaiverTypeComments = ldr["WaiverTypeComments"].ToString(),
                            WaiverStatus = ldr["WaiverStatus"].ToString(),
                            CampaignName = ldr["CampaignName"].ToString(),
                            RecordStatus = RecordStatuses.Active
                        };

                    DateTime dtTemp;

                    if (DateTime.TryParse(ldr["AcceptedDate"].ToString(), out dtTemp))
                        oWaiver.AcceptedDate = dtTemp;
                    else
                        oWaiver.AcceptedDate = null;
                    if (DateTime.TryParse(ldr["DeclinedDate"].ToString(), out dtTemp))
                        oWaiver.DeclinedDate = dtTemp;
                    else
                        oWaiver.DeclinedDate = null;
                    if (DateTime.TryParse(ldr["WaiverStartDate"].ToString(), out dtTemp))
                        oWaiver.WaiverStartDate = dtTemp;
                    else
                        oWaiver.WaiverStartDate = null;
                    if (DateTime.TryParse(ldr["WaiverEndDate"].ToString(), out dtTemp))
                        oWaiver.WaiverEndDate = dtTemp;
                    else
                        oWaiver.WaiverEndDate = null;
                    if (DateTime.TryParse(ldr["WaiverStatusDate"].ToString(), out dtTemp))
                        oWaiver.WaiverStatusDate = dtTemp;
                    else
                        oWaiver.WaiverStatusDate = null;

                    int iTemp;
                    if (int.TryParse(ldr["PlayerWaiverID"].ToString(), out iTemp))
                        oWaiver.PlayerWaiverID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        oWaiver.PlayerProfileID = iTemp;
                    if (int.TryParse(ldr["DeclineApprovedByID"].ToString(), out iTemp))
                        oWaiver.DeclineApprovedByID = iTemp;
                    else
                        oWaiver.DeclineApprovedByID = null;
                    if (int.TryParse(ldr["WaiverID"].ToString(), out iTemp))
                        oWaiver.WaiverID = iTemp;
                    if (int.TryParse(ldr["SourceID"].ToString(), out iTemp))
                        oWaiver.SourceID = iTemp;
                    else
                        oWaiver.SourceID = null;
                    if (int.TryParse(ldr["WaiverTypeID"].ToString(), out iTemp))
                        oWaiver.WaiverTypeID = iTemp;
                    else
                        oWaiver.WaiverTypeID = null;

                    bool bTemp;
                    if (bool.TryParse(ldr["RequiredToPlay"].ToString(), out bTemp))
                        oWaiver.RequiredToPlay = bTemp;
                    else
                        oWaiver.RequiredToPlay = false;

                    PlayerWaivers.Add(oWaiver);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadAffiliations(DataTable dtAffiliations)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtAffiliations.Rows)
                {
                    cPlayerAffiliation cAdd = new
                        cPlayerAffiliation
                    {
                        AffiliationName = ldr["AffiliationName"].ToString(),
                        AffiliationRole = ldr["AffiliationRole"].ToString(),
                        PlayerComments = ldr["PlayerComments"].ToString(),
                        Comments = ldr["Comments"].ToString(),
                        RecordStatus = RecordStatuses.Active
                    };
                    int iTemp;

                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cAdd.PlayerProfileID = iTemp;
                    if (int.TryParse(ldr["PlayerAffiliationID"].ToString(), out iTemp))
                        cAdd.PlayerAffiliationID = iTemp;

                    PlayerAffiliations.Add(cAdd);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadMedical(DataTable dtMedical)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtMedical.Rows)
                {
                    cPlayerMedical cAdd = new cPlayerMedical
                    {
                        Description = ldr["Description"].ToString(),
                        Medication = ldr["Medication"].ToString(),
                        Comments = ldr["Comments"].ToString(),
                        RecordStatus = RecordStatuses.Active
                    };

                    int iTemp;
                    if (int.TryParse(ldr["PlayerMedicalID"].ToString(), out iTemp))
                        cAdd.PlayerMedicalID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cAdd.PlayerProfileID = iTemp;

                    bool bTemp;
                    if (bool.TryParse(ldr["ShareInfo"].ToString(), out bTemp))
                        cAdd.ShareInfo = bTemp;
                    if (bool.TryParse(ldr["PrintOnCard"].ToString(), out bTemp))
                        cAdd.PrintOnCard = bTemp;

                    DateTime dtTemp;
                    if (DateTime.TryParse(ldr["StartDate"].ToString(), out dtTemp))
                        cAdd.StartDate = dtTemp;
                    if (DateTime.TryParse(ldr["EndDate"].ToString(), out dtTemp))
                        cAdd.EndDate = dtTemp;

                    PlayerMedical.Add(cAdd);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        private void LoadLimitations(DataTable dtLimitations)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                foreach (DataRow ldr in dtLimitations.Rows)
                {
                    cPlayerLimitation cAdd = new
                        cPlayerLimitation
                    {
                        Description = ldr["Description"].ToString(),
                        Comments = ldr["Comments"].ToString(),
                        RecordStatus = RecordStatuses.Active
                    };

                    int iTemp;
                    if (int.TryParse(ldr["PlayerLimitationID"].ToString(), out iTemp))
                        cAdd.PlayerLimitationID = iTemp;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        cAdd.PlayerProfileID = iTemp;

                    bool bTemp;
                    if (bool.TryParse(ldr["ShareInfo"].ToString(), out bTemp))
                        cAdd.ShareInfo = bTemp;
                    if (bool.TryParse(ldr["PrintOnCard"].ToString(), out bTemp))
                        cAdd.PrintOnCard = bTemp;

                    DateTime dtTemp;
                    if (DateTime.TryParse(ldr["StartDate"].ToString(), out dtTemp))
                        cAdd.StartDate = dtTemp;
                    if (DateTime.TryParse(ldr["EndDate"].ToString(), out dtTemp))
                        cAdd.EndDate = dtTemp;

                    PlayerLimitation.Add(cAdd);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }
    }
}