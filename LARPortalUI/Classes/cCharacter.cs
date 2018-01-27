using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LarpPortal.Classes
{
    public enum RecordStatuses
    {
        Active,
        Delete
    };

    public class cCharacter
    {
        public cCharacter()
        {
            CharacterID = -1;      // -1 Means that no character has been loaded. Will be a new character.
            TotalCP = 0;
            ProfilePicture = new cPicture();
            //            AllowCharacterRebuild = false;              // JBradshaw 7/10/2016 Request #1293
        }

        public override string ToString()
        {
            return FirstName + "Don't use this. Use something else.";
        }

        public int CharacterID { get; set; }
        public int CurrentUserID { get; set; }
        public int CharacterStatusID { get; set; }
        public int CharacterStatusDesc { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AKA { get; set; }
        public string Title { get; set; }
        public int CharacterType { get; set; }
        public int PlotLeadPerson { get; set; }
        public bool RulebookCharacter { get; set; }
        public string CharacterHistory { get; set; }
        public DateTime? DateHistorySubmitted { get; set; }          // Added JBradshaw 9/12/2015
        public DateTime? DateHistoryApproved { get; set; }           // Added JBradshaw 9/12/2015
        public string DateOfBirth { get; set; }
        public string WhereFrom { get; set; }
        public string CurrentHome { get; set; }
        public string CardPrintName { get; set; }
        public string HousingListName { get; set; }
        public DateTime? StartDate { get; set; }
        public string CharacterEmail { get; set; }
        public double TotalCP { get; set; }
        public string CharacterPhoto { get; set; }
        public string Costuming { get; set; }
        public string Weapons { get; set; }
        public string Accessories { get; set; }
        public string Items { get; set; }
        public string Treasure { get; set; }
        public string Makeup { get; set; }
        public string PlayerComments { get; set; }
        public string StaffComments { get; set; }
        public int ProfilePictureID { get; set; }
        public cPicture ProfilePicture { get; set; }
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public int CharacterSkillSetID { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
        public int PlayerID { get; set; }
        public string PlayerEMail { get; set; }
        public DateTime? LatestEventDate { get; set; }
        public bool VisibleToPCs { get; set; }
        public Boolean AllowCharacterRebuild
        {
            get
            {
                if (AllowCharacterRebuildToDate.HasValue)
                    if (AllowCharacterRebuildToDate >= DateTime.Now)
                        return true;
                return false;
            }
        }              // Added J.Bradshaw Request #1293
        public DateTime? AllowCharacterRebuildToDate { get; set; }    // Added J.Bradshaw  3/12/2017

        public List<cCharacterPlace> Places = new List<cCharacterPlace>();
        public List<cCharacterDeath> Deaths = new List<cCharacterDeath>();
        public List<cActor> Actors = new List<cActor>();
        public List<cRelationship> Relationships = new List<cRelationship>();
        public List<cPicture> Pictures = new List<cPicture>();
        public List<cCharacterSkill> CharacterSkills = new List<cCharacterSkill>();
        public List<cDescriptor> Descriptors = new List<cDescriptor>();
        public List<cCharacterItems> CharacterItems = new List<cCharacterItems>();

        public cRace Race = new cRace();
        public cCharacterType CharType = new cCharacterType();
        public cCharacterStatus Status = new cCharacterStatus();
        public List<cSkillPool> SkillPools = new List<cSkillPool>();
        public List<cTeamInfo> Teams = new List<cTeamInfo>();

        public int LoadCharacter(int CharacterIDToLoad)
        {
            int iNumCharacterRecords = 0;

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CharacterID", CharacterIDToLoad);

            DataSet dsCharacterInfo = new DataSet();
            dsCharacterInfo = cUtilities.LoadDataSet("uspGetCharacterInfo", sParam, "LARPortal", "GeneralUser", lsRoutineName);

            int iTemp;
            bool bTemp;
            DateTime dtTemp;
            double dTemp;

            Places = new List<cCharacterPlace>();
            Deaths = new List<cCharacterDeath>();
            Actors = new List<cActor>();
            Relationships = new List<cRelationship>();
            Pictures = new List<cPicture>();
            CharacterSkills = new List<cCharacterSkill>();
            Descriptors = new List<cDescriptor>();
            CharacterItems = new List<cCharacterItems>();

            Race = new cRace();
            CharType = new cCharacterType();
            Status = new cCharacterStatus();
            SkillPools = new List<cSkillPool>();
            Teams = new List<cTeamInfo>();

            DateTime? dtDateCharacterCreated = null;

            Dictionary<int, string> TableInfo = new Dictionary<int, string>();

            for (int i = 0; i < dsCharacterInfo.Tables.Count; i++)
                if (dsCharacterInfo.Tables[i].Rows.Count > 0)
                {
                    if (dsCharacterInfo.Tables[i].Columns["TableName"] != null)
                        TableInfo.Add(i, dsCharacterInfo.Tables[i].Rows[0]["TableName"].ToString());
                    else
                        TableInfo.Add(i, "Unknown");
                }
                else
                    TableInfo.Add(i, "No Rows");

            dsCharacterInfo.Tables[0].TableName = "CHCharacters";
            dsCharacterInfo.Tables[1].TableName = "CHCharacterActors";
            dsCharacterInfo.Tables[2].TableName = "CHCharacterSkillSets";
            dsCharacterInfo.Tables[3].TableName = "CHCharacterPlaces";
            dsCharacterInfo.Tables[4].TableName = "CHCharacterStaffComments";
            dsCharacterInfo.Tables[5].TableName = "CHCharacterRelationships";
            dsCharacterInfo.Tables[6].TableName = "CHCharacterPictures";
            dsCharacterInfo.Tables[7].TableName = "CHCharacterDeaths";
            dsCharacterInfo.Tables[8].TableName = "CHCampaignRaces";
            dsCharacterInfo.Tables[9].TableName = "CHCharacterTypes";
            dsCharacterInfo.Tables[10].TableName = "MDBStatus";
            dsCharacterInfo.Tables[11].TableName = "CurrentCharacterUser";
            dsCharacterInfo.Tables[12].TableName = "PlotUser";
            dsCharacterInfo.Tables[13].TableName = "CharacterSkills";
            dsCharacterInfo.Tables[14].TableName = "Descriptors";
            dsCharacterInfo.Tables[15].TableName = "CharacterItems";
            dsCharacterInfo.Tables[16].TableName = "ProfilePicture";
            dsCharacterInfo.Tables[17].TableName = "CampaignInfo";
            dsCharacterInfo.Tables[18].TableName = "CHCharacterSkillsPoints";
            dsCharacterInfo.Tables[19].TableName = "CMTeamMemberInfo";

            iNumCharacterRecords = dsCharacterInfo.Tables["CHCharacters"].Rows.Count;

            foreach (DataRow dRow in dsCharacterInfo.Tables["CHCharacters"].Rows)
            {
                if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                    CharacterID = iTemp;

                if (int.TryParse(dRow["CurrentUserID"].ToString(), out iTemp))
                    CurrentUserID = iTemp;

                if (int.TryParse(dRow["CharacterStatus"].ToString(), out iTemp))
                    CharacterStatusID = iTemp;

                FirstName = dRow["CharacterFirstName"].ToString();
                MiddleName = dRow["CharacterMiddleName"].ToString();
                LastName = dRow["CharacterLastName"].ToString();
                AKA = dRow["CharacterAKA"].ToString();
                Title = dRow["CharacterTitle"].ToString();

                if (int.TryParse(dRow["CharacterType"].ToString(), out iTemp))
                    CharacterType = iTemp;
                else
                    CharacterType = -1;

                if (int.TryParse(dRow["PlotLeadPerson"].ToString(), out iTemp))
                    PlotLeadPerson = iTemp;
                else
                    PlotLeadPerson = -1;

                if (bool.TryParse(dRow["RulebookCharacter"].ToString(), out bTemp))
                    RulebookCharacter = bTemp;
                else
                    RulebookCharacter = false;

                CharacterHistory = dRow["CharacterHistory"].ToString();
                DateOfBirth = dRow["DateOfBirth"].ToString();
                WhereFrom = dRow["WhereFrom"].ToString();
                CurrentHome = dRow["CurrentHome"].ToString();
                CardPrintName = dRow["CardPrintName"].ToString();
                HousingListName = dRow["HousingListName"].ToString();

                if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
                    StartDate = dtTemp;
                else
                    StartDate = null;

                if (DateTime.TryParse(dRow["DateHistorySubmitted"].ToString(), out dtTemp))
                    DateHistorySubmitted = dtTemp;
                else
                    DateHistorySubmitted = null;

                if (DateTime.TryParse(dRow["DateHistoryApproved"].ToString(), out dtTemp))
                    DateHistoryApproved = dtTemp;
                else
                    DateHistoryApproved = null;

                CharacterEmail = dRow["CharacterEmail"].ToString();

                if (double.TryParse(dRow["TotalCP"].ToString(), out dTemp))
                    TotalCP = dTemp;
                else
                    TotalCP = 0.0;

                CharacterPhoto = dRow["CharacterPhoto"].ToString();
                Costuming = dRow["Costuming"].ToString();

                Weapons = dRow["Weapons"].ToString();
                Accessories = dRow["Accessories"].ToString();
                Items = dRow["Items"].ToString();
                Treasure = dRow["Treasure"].ToString();
                Makeup = dRow["Makeup"].ToString();
                PlayerComments = dRow["PlayerComments"].ToString();
                StaffComments = dRow["Comments"].ToString();

                if (int.TryParse(dRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                CampaignName = dRow["CampaignName"].ToString();

                if (int.TryParse(dRow["CharacterSkillSetID"].ToString(), out iTemp))
                    CharacterSkillSetID = iTemp;

                if (int.TryParse(dRow["PrimaryTeamID"].ToString(), out iTemp))
                {
                    TeamID = iTemp;
                    TeamName = dRow["TeamName"].ToString();
                }
                else
                    TeamName = "";

                if (int.TryParse(dRow["PlayerID"].ToString(), out iTemp))
                {
                    PlayerID = iTemp;
                    PlayerName = dRow["PlayerName"].ToString();
                    PlayerEMail = dRow["PlayerEMail"].ToString();
                }

                if (DateTime.TryParse(dRow["LatestEventDate"].ToString(), out dtTemp))
                    LatestEventDate = dtTemp;
                else
                    LatestEventDate = null;

                if (bool.TryParse(dRow["VisibleToPCs"].ToString(), out bTemp))
                    VisibleToPCs = bTemp;
                else
                    VisibleToPCs = true;

                // J.Bradshaw  7/10/2016  Request #1293
                //if (bool.TryParse(dRow["AllowCharacterRebuild"].ToString(), out bTemp))
                //    AllowCharacterRebuild = bTemp;
                if (DateTime.TryParse(dRow["AllowSkillRebuildToDate"].ToString(), out dtTemp))
                    AllowCharacterRebuildToDate = dtTemp;

                if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                    dtDateCharacterCreated = dtTemp;
            }

            foreach (DataRow dItems in dsCharacterInfo.Tables["CharacterItems"].Rows)
            {
                int CharacterItemID;
                int CharacterID;
                int? ItemPictureID;

                if ((int.TryParse(dItems["CharacterItemID"].ToString(), out CharacterItemID)) &&
                     (int.TryParse(dItems["CharacterID"].ToString(), out CharacterID)))
                {
                    ItemPictureID = null;
                    if (!dItems.IsNull("ItemPictureID"))
                    {
                        if (int.TryParse(dItems["ItemPictureID"].ToString(), out iTemp))
                            ItemPictureID = iTemp;
                    }
                    cCharacterItems NewItem = new cCharacterItems(CharacterItemID, CharacterID, dItems["ItemDescription"].ToString(), ItemPictureID);
                    CharacterItems.Add(NewItem);
                }
            }

            foreach (DataRow dPicture in dsCharacterInfo.Tables["CHCharacterPictures"].Rows)
            {
                cPicture NewPicture = new cPicture()
                {
                    PictureID = -1,
                    PictureFileName = dPicture["PictureFileName"].ToString(),
                    CreatedBy = dPicture["CreatedBy"].ToString(),
                    RecordStatus = RecordStatuses.Active
                };

                string sPictureType = dPicture["PictureType"].ToString();
                NewPicture.PictureType = (cPicture.PictureTypes)Enum.Parse(typeof(cPicture.PictureTypes), sPictureType);

                if (int.TryParse(dPicture["MDBPictureID"].ToString(), out iTemp))
                    NewPicture.PictureID = iTemp;

                if (int.TryParse(dPicture["CharacterID"].ToString(), out iTemp))
                    NewPicture.CharacterID = iTemp;

                Pictures.Add(NewPicture);
            }

            foreach (DataRow dPlaces in dsCharacterInfo.Tables["CHCharacterPlaces"].Rows)
            {
                cCharacterPlace NewPlace = new cCharacterPlace()
                {
                    CharacterPlaceID = -1,
                    CharacterID = CharacterIDToLoad,
                    CampaignPlaceID = -1,
                    LocaleID = -1,
                    PlaceName = dPlaces["PlaceName"].ToString(),
                    Comments = dPlaces["PlayerComments"].ToString(),
                    RecordStatus = RecordStatuses.Active
                };

                if (int.TryParse(dPlaces["CharacterPlaceID"].ToString(), out iTemp))
                    NewPlace.CharacterPlaceID = iTemp;

                if (int.TryParse(dPlaces["CharacterID"].ToString(), out iTemp))
                    NewPlace.CharacterID = iTemp;

                if (int.TryParse(dPlaces["PlaceID"].ToString(), out iTemp))
                    NewPlace.CampaignPlaceID = iTemp;

                if (int.TryParse(dPlaces["LocaleID"].ToString(), out iTemp))
                    NewPlace.LocaleID = iTemp;

                Places.Add(NewPlace);
            }

            foreach (DataRow dDeaths in dsCharacterInfo.Tables["CHCharacterDeaths"].Rows)
            {
                cCharacterDeath NewDeath = new cCharacterDeath()
                {
                    CharacterDeathID = -1,
                    CharacterID = CharacterIDToLoad,
                    StaffComments = dDeaths["StaffComments"].ToString(),
                    Comments = dDeaths["Comments"].ToString(),
                    RecordStatus = RecordStatuses.Active
                };

                if (int.TryParse(dDeaths["CharacterDeathID"].ToString(), out iTemp))
                    NewDeath.CharacterDeathID = iTemp;

                if (bool.TryParse(dDeaths["DeathPermanent"].ToString(), out bTemp))
                    NewDeath.DeathPermanent = bTemp;

                if (DateTime.TryParse(dDeaths["DeathDate"].ToString(), out dtTemp))
                    NewDeath.DeathDate = dtTemp;

                Deaths.Add(NewDeath);
            }

            foreach (DataRow dActors in dsCharacterInfo.Tables["CHCharacterActors"].Rows)
            {
                cActor NewActor = new cActor()
                {
                    CharacterActorID = -1,
                    UserID = -1,
                    StartDate = null,
                    EndDate = null,
                    loginUserName = dActors["loginUserName"].ToString(),
                    ActorFirstName = dActors["FirstName"].ToString(),
                    ActorMiddleName = dActors["MiddleName"].ToString(),
                    ActorLastName = dActors["LastName"].ToString(),
                    ActorNickName = dActors["NickName"].ToString(),
                    StaffComments = dActors["StaffComments"].ToString(),
                    Comments = dActors["Comments"].ToString(),
                    RecordStatus = RecordStatuses.Active
                };

                NewActor.CharacterID = CharacterIDToLoad;

                if (int.TryParse(dActors["CharacterActorID"].ToString(), out iTemp))
                    NewActor.CharacterActorID = iTemp;

                if (int.TryParse(dActors["UserID"].ToString(), out iTemp))
                    NewActor.UserID = iTemp;

                if (DateTime.TryParse(dActors["StartDate"].ToString(), out dtTemp))
                    NewActor.StartDate = dtTemp;

                if (DateTime.TryParse(dActors["EndDate"].ToString(), out dtTemp))
                    NewActor.EndDate = dtTemp;

                Actors.Add(NewActor);
            }

            foreach (DataRow dRelationship in dsCharacterInfo.Tables["CHCharacterRelationships"].Rows)
            {
                cRelationship NewRelationship = new cRelationship()
                {
                    CharacterRelationshipID = -1,
                    CharacterID = CharacterIDToLoad,
                    Name = dRelationship["Name"].ToString(),
                    RelationDescription = dRelationship["RelationDescription"].ToString(),
                    OtherDescription = dRelationship["OtherDescription"].ToString(),
                    RelationTypeID = -1,
                    RelationCharacterID = -1,
                    PlayerAssignedStatus = -1,
                    ListedInHistory = false,
                    RulebookCharacter = false,
                    RulebookCharacterID = -1,
                    PlayerComments = dRelationship["PlayerComments"].ToString(),
                    HideFromPC = false,
                    StaffAssignedRelationCharacterID = -1,
                    StaffAssignedStatus = -1,
                    StaffComments = dRelationship["StaffComments"].ToString(),
                    Comments = dRelationship["Comments"].ToString(),
                    RecordStatus = RecordStatuses.Active
                };

                if (int.TryParse(dRelationship["CharacterRelationshipID"].ToString(), out iTemp))
                    NewRelationship.CharacterRelationshipID = iTemp;

                if (int.TryParse(dRelationship["RelationTypeID"].ToString(), out iTemp))
                    NewRelationship.RelationTypeID = iTemp;

                if (int.TryParse(dRelationship["RelationCharacterID"].ToString(), out iTemp))
                    NewRelationship.RelationCharacterID = iTemp;

                if (int.TryParse(dRelationship["PlayerAssignedStatus"].ToString(), out iTemp))
                    NewRelationship.PlayerAssignedStatus = iTemp;

                if (int.TryParse(dRelationship["RulebookCharacterID"].ToString(), out iTemp))
                    NewRelationship.RulebookCharacterID = iTemp;
                //       StaffAssignedRelationCharacterID
                if (int.TryParse(dRelationship["StaffAssignedRelationCharacterID"].ToString(), out iTemp))
                    NewRelationship.StaffAssignedRelationCharacterID = iTemp;

                if (int.TryParse(dRelationship["StaffAssignedStatus"].ToString(), out iTemp))
                    NewRelationship.StaffAssignedStatus = iTemp;

                if (bool.TryParse(dRelationship["ListedInHistory"].ToString(), out bTemp))
                    NewRelationship.ListedInHistory = bTemp;

                if (bool.TryParse(dRelationship["RulebookCharacter"].ToString(), out bTemp))
                    NewRelationship.RulebookCharacter = bTemp;

                if (bool.TryParse(dRelationship["HideFromPC"].ToString(), out bTemp))
                    NewRelationship.HideFromPC = bTemp;

                Relationships.Add(NewRelationship);
            }

            foreach (DataRow dRace in dsCharacterInfo.Tables["CHCampaignRaces"].Rows)
            {
                if (int.TryParse(dRace["CampaignRaceID"].ToString(), out iTemp))
                    Race.CampaignRaceID = iTemp;
                if (int.TryParse(dRace["GameSystemID"].ToString(), out iTemp))
                    Race.GameSystemID = iTemp;
                if (int.TryParse(dRace["CampaignID"].ToString(), out iTemp))
                    Race.CampaignID = iTemp;
                Race.RaceName = dRace["RaceName"].ToString();
                Race.SubRace = dRace["SubRace"].ToString();
                Race.Description = dRace["Description"].ToString();
                Race.MakeupRequirements = dRace["MakeupRequirements"].ToString();
                Race.Photo = dRace["Photo"].ToString();
                Race.Comments = dRace["Comments"].ToString();
            }

            foreach (DataRow dCharType in dsCharacterInfo.Tables["CHCharacterTypes"].Rows)
            {
                if (int.TryParse(dCharType["CharacterTypeID"].ToString(), out iTemp))
                    CharType.CharacterTypeID = iTemp;
                CharType.Description = dCharType["Description"].ToString();
                CharType.Comments = dCharType["Comments"].ToString();
            }

            foreach (DataRow dStatus in dsCharacterInfo.Tables["MDBStatus"].Rows)
            {
                if (int.TryParse(dStatus["StatusID"].ToString(), out iTemp))
                    Status.StatusID = iTemp;
                Status.StatusType = dStatus["StatusType"].ToString();
                Status.StatusName = dStatus["StatusName"].ToString();
                Status.Comments = dStatus["Comments"].ToString();
            }

            foreach (DataRow dSkill in dsCharacterInfo.Tables["CharacterSkills"].Rows)
            {
                cCharacterSkill NewSkill = new cCharacterSkill()
                {
                    CharacterSkillSetID = -1,
                    SkillSetName = dSkill["SkillSetName"].ToString(),
                    StatusName = dSkill["SkillName"].ToString(),
                    SkillSetTypeDescription = dSkill["SkillSetTypeDescription"].ToString(),
                    SkillName = dSkill["SkillName"].ToString(),
                    SkillShortDescription = dSkill["SkillShortDescription"].ToString(),
                    //                    SkillLongDescription = dSkill["SkillLongDescription"].ToString(),
                    CampaignSkillsStandardComments = dSkill["CampaignSkillsStandardComments"].ToString(),
                    //                    SkillTypeDescription = dSkill["SkillTypeDescription"].ToString(),
                    SkillTypeComments = dSkill["SkillTypeComments"].ToString(),
                    PlayerDescription = dSkill["PlayerDescription"].ToString(),
                    PlayerIncant = dSkill["PlayerIncant"].ToString(),
                    SkillCardDescription = dSkill["SkillCardDescription"].ToString(),
                    SkillCardIncant = dSkill["SkillIncant"].ToString()
                    //if (SuppressCampaignDescription != null)
                    //if (SuppressCampaignIncant != null)
                    // DisplayOnCard
                };


                //if (int.TryParse(dSkill["CharacterSkillsStandardID"].ToString(), out iTemp))
                //    NewSkill.CharacterSkillsStandardID = iTemp;

                if (int.TryParse(dSkill["CharacterID"].ToString(), out iTemp))
                    NewSkill.CharacterID = iTemp;

                if (int.TryParse(dSkill["CharacterSkillSetStatusID"].ToString(), out iTemp))
                    NewSkill.CharacterSkillSetStatusID = iTemp;

                if (int.TryParse(dSkill["StatusType"].ToString(), out iTemp))
                    NewSkill.StatusType = iTemp;

                //if (int.TryParse(dSkill["CharacterSkillSetTypeID"].ToString(), out iTemp))
                //    NewSkill.CharacterSkillSetTypeID = iTemp;

                if (int.TryParse(dSkill["CharacterSkillID"].ToString(), out iTemp))
                    NewSkill.CharacterSkillID = iTemp;

                if (int.TryParse(dSkill["CampaignSkillNodeID"].ToString(), out iTemp))
                    NewSkill.CampaignSkillNodeID = iTemp;

                //if (int.TryParse(dSkill["CampaignSkillsStandardID"].ToString(), out iTemp))
                //    NewSkill.CampaignSkillsStandardID = iTemp;

                if (int.TryParse(dSkill["SkillTypeID"].ToString(), out iTemp))
                    NewSkill.SkillTypeID = iTemp;

                //                if (int.TryParse(dSkill["SkillHeaderTypeID"].ToString(), out iTemp))
                //                    NewSkill.SkillHeaderTypeID = iTemp;

                if (int.TryParse(dSkill["CharacterSkillSetID"].ToString(), out iTemp))
                    NewSkill.CharacterSkillSetID = iTemp;

                //if (int.TryParse(dSkill["HeaderAssociation"].ToString(), out iTemp))
                //    NewSkill.HeaderAssociation = iTemp;

                //if (int.TryParse(dSkill["SkillCostFixed"].ToString(), out iTemp))
                //    NewSkill.SkillCostFixed = iTemp;

                if (int.TryParse(dSkill["DisplayOrder"].ToString(), out iTemp))
                    NewSkill.DisplayOrder = iTemp;

                if (double.TryParse(dSkill["SkillCPCost"].ToString(), out dTemp))
                    NewSkill.SkillCPCost = dTemp;

                if (double.TryParse(dSkill["CPCostPaid"].ToString(), out dTemp))
                    NewSkill.CPCostPaid = dTemp;

                if (bool.TryParse(dSkill["CanBeUsedPassively"].ToString(), out bTemp))
                    NewSkill.CanBeUsedPassively = bTemp;

                if (bool.TryParse(dSkill["AllowPassiveUse"].ToString(), out bTemp))
                    NewSkill.AllowPassiveUse = bTemp;

                if (bool.TryParse(dSkill["OpenToAll"].ToString(), out bTemp))
                    NewSkill.OpenToAll = bTemp;

                if (bool.TryParse(dSkill["CardDisplayDescription"].ToString(), out bTemp))
                    NewSkill.CardDisplayDescription = bTemp;

                if (bool.TryParse(dSkill["CardDisplayIncant"].ToString(), out bTemp))
                    NewSkill.CardDisplayIncant = bTemp;

                if (bool.TryParse(dSkill["CardDisplayDescription"].ToString(), out bTemp))
                    NewSkill.CardDisplayDescription = bTemp;

                if (bool.TryParse(dSkill["CardDisplayIncant"].ToString(), out bTemp))
                    NewSkill.CardDisplayIncant = bTemp;


                NewSkill.RecordStatus = RecordStatuses.Active;

                CharacterSkills.Add(NewSkill);
            }

            foreach (DataRow dRow in dsCharacterInfo.Tables["Descriptors"].Rows)
            {
                cDescriptor NewDesc = new cDescriptor();
                NewDesc.SkillSetName = dRow["SkillSetName"].ToString();
                NewDesc.DescriptorValue = dRow["DescriptorValue"].ToString();
                NewDesc.CharacterDescriptor = dRow["CharacterDescriptor"].ToString();
                NewDesc.PlayerComments = dRow["PlayerComments"].ToString();

                int iValue;

                if (int.TryParse(dRow["CharacterSkillSetID"].ToString(), out iValue))
                    NewDesc.CharacterSkillSetID = iValue;
                else
                    NewDesc.CharacterSkillSetID = 0;

                if (int.TryParse(dRow["CharacterAttributesBasicID"].ToString(), out iValue))
                    NewDesc.CharacterAttributesBasicID = iValue;
                else
                    NewDesc.CharacterAttributesBasicID = 0;

                if (int.TryParse(dRow["CampaignAttributeStandardID"].ToString(), out iValue))
                    NewDesc.CampaignAttributeStandardID = iValue;
                else
                    NewDesc.CampaignAttributeStandardID = 0;

                if (int.TryParse(dRow["CampaignAttributeDescriptorID"].ToString(), out iValue))
                    NewDesc.CampaignAttributeDescriptorID = iValue;
                else
                    NewDesc.CampaignAttributeDescriptorID = 0;

                Descriptors.Add(NewDesc);
            }

            ProfilePicture = null;

            foreach (DataRow dRow in dsCharacterInfo.Tables["ProfilePicture"].Rows)
            {
                if (int.TryParse(dRow["MDBPictureID"].ToString(), out iTemp))
                {
                    ProfilePicture = new cPicture();
                    ProfilePicture.PictureID = iTemp;
                    ProfilePictureID = iTemp;
                    ProfilePicture.PictureFileName = dRow["PictureFileName"].ToString();
                    ProfilePicture.PictureType = cPicture.PictureTypes.Profile;
                    ProfilePicture.RecordStatus = RecordStatuses.Active;
                }
            }

            foreach (DataRow dRow in dsCharacterInfo.Tables["CHCharacterSkillsPoints"].Rows)
            {
                Classes.cSkillPool NewPool = new cSkillPool();
                NewPool.PoolDescription = dRow["PoolDescription"].ToString();
                NewPool.PoolDisplayColor = dRow["DisplayColor"].ToString();

                if (int.TryParse(dRow["PoolID"].ToString(), out iTemp))
                    NewPool.PoolID = iTemp;

                if (bool.TryParse(dRow["DefaultPool"].ToString(), out bTemp))
                    NewPool.DefaultPool = bTemp;
                else
                    NewPool.DefaultPool = false;

                if (double.TryParse(dRow["TotalPoints"].ToString(), out dTemp))
                    NewPool.TotalPoints = dTemp;
                else
                    NewPool.TotalPoints = 0.0;

                SkillPools.Add(NewPool);
            }

            // The only teams we want to do are teams where the person is an actual member.
            DataView dvTeams = new DataView(dsCharacterInfo.Tables["CMTeamMemberInfo"], "Approval or Member", "", DataViewRowState.CurrentRows);
            foreach (DataRowView dRow in dvTeams)
            {
                Classes.cTeamInfo NewTeam = new cTeamInfo();
                NewTeam.TeamName = dRow["TeamName"].ToString();
                NewTeam.RoleDescription = dRow["RoleDescription"].ToString();
                if (int.TryParse(dRow["TeamID"].ToString(), out iTemp))
                    NewTeam.TeamID = iTemp;
                if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                    NewTeam.CharacterID = iTemp;
                if (int.TryParse(dRow["RoleID"].ToString(), out iTemp))
                    NewTeam.RoleID = iTemp;
                if (bool.TryParse(dRow["Approval"].ToString(), out bTemp))
                    NewTeam.Approval = bTemp;
                if (bool.TryParse(dRow["Member"].ToString(), out bTemp))
                    NewTeam.Member = bTemp;
                if (bool.TryParse(dRow["Requested"].ToString(), out bTemp))
                    NewTeam.Requested = bTemp;
                if (bool.TryParse(dRow["Invited"].ToString(), out bTemp))
                    NewTeam.Invited = bTemp;
                Teams.Add(NewTeam);
            }

            return iNumCharacterRecords;
        }

        public string SaveCharacter(string sUserUpdating, int iUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string Timing;

            Timing = "Save character start: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");

            SortedList sParams = new SortedList();
            sParams.Add("@UserID", iUserID);
            sParams.Add("@CharacterID", CharacterID);
            sParams.Add("@CurrentUserID", CurrentUserID);
            sParams.Add("@CharacterStatus", CharacterStatusID);
            sParams.Add("@CharacterFirstName", FirstName);
            sParams.Add("@CharacterMiddleName", MiddleName);
            sParams.Add("@CharacterLastName", LastName);
            sParams.Add("@CharacterAKA", AKA);
            sParams.Add("@CharacterTitle", Title);
            sParams.Add("@CharacterRace", Race.CampaignRaceID);
            sParams.Add("@CharacterType", CharacterType);
            sParams.Add("@PlotLeadPerson", PlotLeadPerson);
            sParams.Add("@RulebookCharacter", RulebookCharacter);
            sParams.Add("@CharacterHistory", CharacterHistory);
            sParams.Add("@DateHistorySubmitted", DateHistorySubmitted);
            sParams.Add("@DateHistoryApproved", DateHistoryApproved);
            sParams.Add("@DateOfBirth", DateOfBirth);
            sParams.Add("@WhereFrom", WhereFrom);
            sParams.Add("@CurrentHome", CurrentHome);
            sParams.Add("@CardPrintName", CardPrintName);
            sParams.Add("@HousingListName", HousingListName);
            sParams.Add("@StartDate", StartDate);
            sParams.Add("@CharacterEmail", CharacterEmail);
            sParams.Add("@TotalCP", TotalCP);
            sParams.Add("@CharacterPhoto", CharacterPhoto);
            sParams.Add("@Costuming", Costuming);
            sParams.Add("@Weapons", Weapons);
            sParams.Add("@Accessories", Accessories);
            sParams.Add("@Items", Items);
            sParams.Add("@Treasure", Treasure);
            sParams.Add("@Makeup", Makeup);
            sParams.Add("@PlayerComments", PlayerComments);
            sParams.Add("@PrimaryTeamID", TeamID);
            sParams.Add("@VisibleToPCs", VisibleToPCs);
            sParams.Add("@Comments", StaffComments);

            DataTable dtCharInfo = cUtilities.LoadDataTable("uspInsUpdCHCharacters", sParams, "LARPortal", sUserUpdating, lsRoutineName + ".uspInsUpdCHCharacters");

            Timing += ", character record update done: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");

            sParams = new SortedList();

            sParams.Add("@UserID", iUserID);
            sParams.Add("@CharacterID", CharacterID);
            if (AllowCharacterRebuild)

                sParams.Add("@CharacterRebuildDate", AllowCharacterRebuildToDate);
            else
                sParams.Add("@ClearDate", 1);

            DataTable dtSkillSets = cUtilities.LoadDataTable("uspSetCharacterRebuildToDate", sParams, "LARPortal", sUserUpdating, lsRoutineName + ".uspSetCharacterRebuildToDate");

            foreach (cPicture Picture in Pictures)
            {
                Picture.CharacterID = CharacterID;
                if (Picture.RecordStatus == RecordStatuses.Delete)
                    Picture.Delete(sUserUpdating);
                else
                    Picture.Save(sUserUpdating);
            }

            if (ProfilePicture != null)
                if (ProfilePicture.RecordStatus == RecordStatuses.Delete)
                    ProfilePicture.Delete(sUserUpdating);
                else
                    ProfilePicture.Save(sUserUpdating);

            foreach (cCharacterSkill Skill in CharacterSkills)
            {
                Skill.CharacterSkillSetID = CharacterSkillSetID;
                if (Skill.RecordStatus == RecordStatuses.Delete)
                    Skill.Delete(sUserUpdating, iUserID);
                else
                    Skill.Save(sUserUpdating, iUserID);
            }

            Timing += ", skills update done: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");

            foreach (cDescriptor Desc in Descriptors)
            {
                Desc.CharacterSkillSetID = CharacterSkillSetID;
                if (Desc.RecordStatus == RecordStatuses.Delete)
                    Desc.Delete(sUserUpdating, iUserID);
                else
                    Desc.Save(sUserUpdating, iUserID);
            }

            foreach (cCharacterPlace Place in Places)
            {
                Place.CharacterID = CharacterID;
                Place.Save(iUserID);
            }

            foreach (cCharacterDeath Death in Deaths)
            {
                Death.Save(iUserID);
            }

            foreach (cActor Actor in Actors)
            {
                Actor.Save(iUserID);
            }

            foreach (cRelationship Relat in Relationships)
            {
                Relat.Save(sUserUpdating, iUserID);
            }

            Timing += ", character save done: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");

            return Timing;
        }
    }
}
