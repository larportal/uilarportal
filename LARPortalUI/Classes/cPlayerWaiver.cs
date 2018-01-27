using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;


namespace LarpPortal.Classes
{
    public class cPlayerWaiver
    {
        public int PlayerWaiverID { get; set; }
        public int PlayerProfileID { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public int? DeclineApprovedByID { get; set; }
        public string WaiverImage { get; set; }
        public string PlayerNotes { get; set; }
        public string StaffNotes { get; set; }
        public string Comments { get; set; }
        public int WaiverID { get; set; }
        public int? SourceID { get; set; }
        public string WaiverText { get; set; }
        public string WaiverNotes { get; set; }
        public bool? RequiredToPlay { get; set; }
        public DateTime? WaiverStartDate { get; set; }
        public DateTime? WaiverEndDate { get; set; }
        public string WaiverComments { get; set; }
        public int? WaiverTypeID { get; set; }
        public string WaiverType { get; set; }
        public string WaiverTypeComments { get; set; }
        public string CampaignName { get; set; }
        public string WaiverStatus { get; set; }
        public DateTime? WaiverStatusDate { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerWaiver()
        {
            PlayerWaiverID = -1;
            PlayerProfileID = -1;
            AcceptedDate = null;
            DeclinedDate = null;
            DeclineApprovedByID = null;
            WaiverImage = "";
            PlayerNotes = "";
            StaffNotes = "";
            Comments = "";
            WaiverID = -1;
            SourceID = null;
            WaiverText = "";
            WaiverNotes = "";
            RequiredToPlay = null;
            WaiverStartDate = null;
            WaiverEndDate = null;
            WaiverComments = "";
            WaiverTypeID = null;
            WaiverType = "";
            WaiverTypeComments = "";
            CampaignName = "";
            WaiverStatus = "";
            WaiverStatusDate = null;
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerWaiver(Int32 intPlayerWaiverID, string strUserName)
        {
            Load(intPlayerWaiverID, strUserName);
        }

        public void Load(Int32 intPlayerWaiverID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            PlayerProfileID = -1;
            AcceptedDate = null;
            DeclinedDate = null;
            DeclineApprovedByID = null;
            WaiverImage = "";
            PlayerNotes = "";
            StaffNotes = "";
            Comments = "";
            WaiverID = -1;
            SourceID = null;
            WaiverText = "";
            WaiverNotes = "";
            RequiredToPlay = null;
            WaiverStartDate = null;
            WaiverEndDate = null;
            WaiverComments = "";
            WaiverTypeID = null;
            WaiverType = "";
            WaiverTypeComments = "";
            CampaignName = "";
            WaiverStatus = "";
            WaiverStatusDate = null;
            RecordStatus = RecordStatuses.Active;

            PlayerWaiverID = intPlayerWaiverID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerWaiverID", PlayerWaiverID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerWaiverByID", slParams, "LARPortal", strUserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    DataRow ldr = ldt.Rows[0];

                    WaiverImage = ldr["WaiverImage"].ToString();
                    PlayerNotes = ldr["PlayerNotes"].ToString();
                    StaffNotes = ldr["StaffNotes"].ToString();
                    Comments = ldr["Comments"].ToString();
                    WaiverText = ldr["WaiverText"].ToString();
                    WaiverNotes = ldr["WaiverNotes"].ToString();
                    WaiverComments = ldr["WaiverComments"].ToString();
                    WaiverType = ldr["WaiverTypeDescription"].ToString();
                    WaiverTypeComments = ldr["WaiverTypeComments"].ToString();
                    WaiverStatus = ldr["WaiverStatus"].ToString();
                    CampaignName = ldr["CampaignName"].ToString();

                    DateTime dtTemp;

                    if (DateTime.TryParse(ldr["AcceptedDate"].ToString(), out dtTemp))
                        AcceptedDate = dtTemp;
                    else
                        AcceptedDate = null;
                    if (DateTime.TryParse(ldr["DeclinedDate"].ToString(), out dtTemp))
                        DeclinedDate = dtTemp;
                    else
                        DeclinedDate = null;
                    if (DateTime.TryParse(ldr["WaiverStartDate"].ToString(), out dtTemp))
                        WaiverStartDate = dtTemp;
                    else
                        WaiverStartDate = null;
                    if (DateTime.TryParse(ldr["WaiverEndDate"].ToString(), out dtTemp))
                        WaiverEndDate = dtTemp;
                    else
                        WaiverEndDate = null;
                    if (DateTime.TryParse(ldr["WaiverStatusDate"].ToString(), out dtTemp))
                        WaiverStatusDate = dtTemp;
                    else
                        WaiverStatusDate = null;

                    int iTemp;
                    if (int.TryParse(ldr["DeclineApprovedByID"].ToString(), out iTemp))
                        DeclineApprovedByID = iTemp;
                    else
                        DeclineApprovedByID = null;
                    if (int.TryParse(ldr["WaiverID"].ToString(), out iTemp))
                        WaiverID = iTemp;
                    if (int.TryParse(ldr["SourceID"].ToString(), out iTemp))
                        SourceID = iTemp;
                    else
                        SourceID = null;
                    if (int.TryParse(ldr["WaiverTypeID"].ToString(), out iTemp))
                        WaiverTypeID = iTemp;
                    else
                        WaiverTypeID = null;
                    if (int.TryParse(ldr["PlayerProfileID"].ToString(), out iTemp))
                        PlayerProfileID = iTemp;

                    bool bTemp;
                    if (bool.TryParse(ldr["RequiredToPlay"].ToString(), out bTemp))
                        RequiredToPlay = bTemp;
                    else
                        RequiredToPlay = false;
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName + lsRoutineName);
                throw;
            }
        }
        public void Save(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                if (RecordStatus == RecordStatuses.Delete)
                    Delete(UserName, UserID);
                else
                {
                    SortedList slParams = new SortedList();
                    slParams.Add("@UserID", UserID);
                    slParams.Add("@PlayerWaiverID", PlayerWaiverID);
                    slParams.Add("@WaiverID", WaiverID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    if (AcceptedDate.HasValue)
                        slParams.Add("@AcceptedDate", AcceptedDate.Value);
                    else
                        slParams.Add("@ClearAcceptedDate", 1);
                    if (DeclinedDate.HasValue)
                        slParams.Add("@DeclinedDate", DeclinedDate.Value);
                    else
                        slParams.Add("@ClearDeclinedDate", 1);

                    slParams.Add("@WaiverImage", WaiverImage);
                    if (DeclineApprovedByID.HasValue)
                        slParams.Add("@DeclineApprovedByID", DeclineApprovedByID.Value);
                    else
                        slParams.Add("@ClearDeclineApprovedByID", 1);
                    slParams.Add("@PlayerNotes", PlayerNotes);
                    slParams.Add("@StaffNotes", StaffNotes);
                    slParams.Add("@Comments", Comments);

                    cUtilities.PerformNonQuery("uspInsUpdPLPlayerWaivers", slParams, "LARPortal", UserName);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                throw;
            }
        }

        public void Delete(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", UserID);
                slParams.Add("@RecordID", PlayerWaiverID);
                cUtilities.PerformNonQuery("uspDelPLPlayerWaivers", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }
    }
}