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
    public class cPlayerInventory
    {
        public Int32 PlayerInventoryID { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public string Description { get; set; }
        public string ItemName { get; set; }
        public Int32 InventoryTypeID { get; set; }
        public string InventoryTypeDesc { get; set; }
        public string Quantity { get; set; }
        public string Size { get; set; }
        public string PowerNeeded { get; set; }
        public string Location { get; set; }
        public bool WillShare { get; set; }
        public string InventoryNotes { get; set; }
        public int ImageID { get; set; }
        public string ImageURL { get; set; }
        public cPicture InvImage { get; set; }
        public string PlayerComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerInventory()
        {
            PlayerInventoryID = -1;
            PlayerProfileID = -1;
            Description = "";
            ItemName = "";
            InventoryTypeID = -1;
            InventoryTypeDesc = "";
            Quantity = "";
            Size = "";
            PowerNeeded = "";
            Location = "";
            WillShare = false;
            InventoryNotes = "";
//            InventoryImage = "";
            InvImage = new cPicture();
            PlayerComments = "";
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerInventory(Int32 intPlayerInventoryID, string strUserName, Int32 intUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            PlayerInventoryID = intPlayerInventoryID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerInventoryID", PlayerInventoryID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerInventoryByID", slParams, "LARPortal", strUserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    ItemName = ldt.Rows[0]["ItemName"].ToString();
                    Description = ldt.Rows[0]["Description"].ToString();
                    Quantity = ldt.Rows[0]["Quantity"].ToString();
                    Size = ldt.Rows[0]["Size"].ToString();
                    PowerNeeded = ldt.Rows[0]["PowerNeeded"].ToString();
                    Location = ldt.Rows[0]["Location"].ToString();
                    InventoryNotes = ldt.Rows[0]["InventoryNotes"].ToString();
//                  InventoryImage = ldt.Rows[0]["InventoryImage"].ToString();
                    ImageURL = ldt.Rows[0]["ImageURL"].ToString();
                    PlayerComments = ldt.Rows[0]["PlayerComments"].ToString();
                    Comments = ldt.Rows[0]["Comments"].ToString();

                    int iTemp;
                    if (int.TryParse(ldt.Rows[0]["PlayerProfileID"].ToString(), out iTemp))
                        PlayerProfileID = iTemp;
                    if (int.TryParse(ldt.Rows[0]["InventoryTypeID"].ToString(), out iTemp))
                        InventoryTypeID = iTemp;
                    if (int.TryParse(ldt.Rows[0]["ImageID"].ToString(), out iTemp))
                        ImageID = iTemp;

                    if (int.TryParse(ldt.Rows[0]["ImageID"].ToString(), out iTemp))
                    {
                        ImageID = iTemp;
                        InvImage = new cPicture();
                        InvImage.Load(iTemp, strUserName);
                    }

                    bool bTemp;
                    if (bool.TryParse(ldt.Rows[0]["WillShare"].ToString(), out bTemp))
                        WillShare = bTemp;
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName + lsRoutineName);
            }
        }
        public Boolean Save(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean blnReturn = false;

            if (RecordStatus == RecordStatuses.Delete)
                Delete(UserName, UserID);
            else
            {
                try
                {
                    SortedList slParams = new SortedList();
                    slParams.Add("@UserID", UserID);
                    slParams.Add("@PlayerInventoryID", PlayerInventoryID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    slParams.Add("@ItemName", ItemName);
                    slParams.Add("@Description", Description);
                    slParams.Add("@InventoryTypeID", InventoryTypeID);
                    slParams.Add("@Quantity", Quantity);
                    slParams.Add("@Size", Size);
                    slParams.Add("@PowerNeeded", PowerNeeded);
                    slParams.Add("@Location", Location);
                    if (WillShare)
                        slParams.Add("@WillShare", 1);
                    else
                        slParams.Add("@WillShare", 0);
                    slParams.Add("@InventoryNotes", InventoryNotes);
                    slParams.Add("@InventoryImage", ImageURL);
                    slParams.Add("@ImageID", ImageID);
                    slParams.Add("@PlayerComments", PlayerComments);
                    slParams.Add("@Comments", Comments);
                    blnReturn = cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerInventory", slParams, "LARPortal", UserName);
                }
                catch (Exception ex)
                {
                    ErrorAtServer lobjError = new ErrorAtServer();
                    lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                    blnReturn = false;
                }
            }

            return blnReturn;
        }

        public void Delete(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Active)
                Save(UserName, UserID);
            else
            {
                try
                {
                    SortedList slParams = new SortedList();
                    slParams.Add("@UserID", UserID);
                    slParams.Add("@RecordID", PlayerInventoryID);
                    cUtilities.PerformNonQuery("uspDelPLPlayerInventory", slParams, "LARPortal", UserName);
                }
                catch (Exception ex)
                {
                    ErrorAtServer lobjError = new ErrorAtServer();
                    lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                }
            }
        }
    }
}