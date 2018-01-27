using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LarpPortal.Classes;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace LarpPortal.Classes
{
    public class cCampaignSelection
    {
        public int GameSystemID { get; set; }
        public string GameSystemName { get; set; }
        public string GameSystemURL { get; set; }
        public string GameSystemWebPageDescription { get; set; }

        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignProjectedEndDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public int CampaignActualNumberOfEvents { get; set; }
        public int CampaignGameSystemID { get; set; }
        public string CampaignGameSystemName { get; set; }
        public int CampaignMarketingSizeID { get; set; }
        public string CampaignMarketingSize { get; set; }
        public int CampaignStyleID { get; set; }
        public string CampaignStyle { get; set; }
        public int CampaignAddressID { get; set; }
        public string CampaignCity { get; set; }
        public string CampaignState { get; set; }
        public string CampaignZipCode { get; set; }
        public string CampaignWebPageDescription { get; set; }
        public string CampaignURL { get; set; }
        public string CampaignLogo { get; set; }
        public double CampaignMembershipFee { get; set; }
        public string CampaignMembershipFeeFrequency { get; set; }

        public int GenreID { get; set; }
        public string GenreName { get; set; }

        public int StyleID { get; set; }
        public string StyleName { get; set; }

        public int TechLevelID { get; set; }
        public string TechLevelName { get; set; }

        public int SizeID { get; set; }
        public string SizeRange { get; set; }
        public int SizeSortOrder { get; set; }

        public string ZipCode { get; set; }

        //public string EndDate { get; set; }
        //public int GameSystemFilter { get; set; }
        //public int CampaignFilter { get; set; }
        //public int GenreFilter { get; set; }
        //public int StyleFilter { get; set; }
        //public int TechLevelFilter { get; set; }
        //public int SizeFilter { get; set; }
        //public string ZipCodeFilter { get; set; }
        //public int RadiusFilter { get; set; }

        public DataTable LoadGameSystems(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetGameSystemsByName";
            string stCallingMethod = "cGameSystems.LoadGameSystemsByName";
            string strUsername = UserID.ToString();
            int iTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtGameSystems = new DataTable();
            dtGameSystems = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            foreach (DataRow cRow in dtGameSystems.Rows)
            {
                if (int.TryParse(cRow["GameSystemID"].ToString(), out iTemp))
                    GameSystemID = iTemp;
                GameSystemName = cRow["GameSystemName"].ToString();
            }
            return dtGameSystems;
        }

        public DataTable CampaignsByGameSystem(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetCampaignsByGameSystem";
            string stCallingMethod = "cGameSystems.CampaignsByGameSystem";
            string strUsername = UserID.ToString();
            int iTemp;
            SortedList slParameters = new SortedList();
            DataTable dtCampaigns = new DataTable();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            GameSystemID = GameSystemFilter;
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            foreach (DataRow cRow in dtCampaigns.Rows)
            {
                if (int.TryParse(cRow["CampaignID"].ToString(), out iTemp))
                    GameSystemID = iTemp;
                GameSystemName = cRow["CampaignName"].ToString();
            }
            return dtCampaigns;
        }

        public DataTable CampaignsByGenre(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetCampaignsByGenre";
            string stCallingMethod = "cGameSystems.CampaignsByGenre";
            string strUsername = UserID.ToString();
            int iTemp;
            SortedList slParameters = new SortedList();
            DataTable dtCampaigns = new DataTable();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            GenreID = GenreFilter;
            slParameters.Add("@GenreID", GenreID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            foreach (DataRow cRow in dtCampaigns.Rows)
            {
                if (int.TryParse(cRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                CampaignName = cRow["CampaignName"].ToString();
            }
            return dtCampaigns;
        }

        public DataTable CampaignsByTechLevel(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetCampaignsByTechLevel";
            string stCallingMethod = "cGameSystems.CampaignsByTechLevel";
            string strUsername = UserID.ToString();
            int iTemp;
            SortedList slParameters = new SortedList();
            DataTable dtCampaigns = new DataTable();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            TechLevelID = TechLevelFilter;
            slParameters.Add("@TechLevelID", TechLevelID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            foreach (DataRow cRow in dtCampaigns.Rows)
            {
                if (int.TryParse(cRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                CampaignName = cRow["CampaignName"].ToString();
            }
            return dtCampaigns;
        }

        public DataTable CampaignsBySize(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetCampaignsBySize";
            string stCallingMethod = "cGameSystems.CampaignsBySize";
            string strUsername = UserID.ToString();
            int iTemp;
            SortedList slParameters = new SortedList();
            DataTable dtCampaigns = new DataTable();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            SizeID = SizeFilter;
            slParameters.Add("@SizeID", SizeID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            foreach (DataRow cRow in dtCampaigns.Rows)
            {
                if (int.TryParse(cRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                CampaignName = cRow["CampaignName"].ToString();
            }
            return dtCampaigns;
        }

        public DataTable CampaignsByStyle(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetCampaignsByStyle";
            string stCallingMethod = "cGameSystems.CampaignsByStyle";
            string strUsername = UserID.ToString();
            int iTemp;
            SortedList slParameters = new SortedList();
            DataTable dtCampaigns = new DataTable();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            StyleID = StyleFilter;
            slParameters.Add("@StyleID", StyleID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            foreach (DataRow cRow in dtCampaigns.Rows)
            {
                if (int.TryParse(cRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                CampaignName = cRow["CampaignName"].ToString();
            }
            return dtCampaigns;
        }


        /// <summary>
        /// This will load a table of all campaigns by name
        /// EndDate is optional to include campaigns that have ended
        /// </summary>
        public DataTable LoadCampaigns(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetCampaignsByName";
            string stCallingMethod = "cCampaignSelections.LoadCampaigns";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtCampaigns = new DataTable();
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtCampaigns;
        }

        public DataTable LoadTechLevels(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetTechLevels";
            string stCallingMethod = "cCampaignSelections.LoadTechLevels";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtTechLevels = new DataTable();
            dtTechLevels = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtTechLevels;
        }

        public DataTable LoadStyles(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetStyles";
            string stCallingMethod = "cCampaignSelections.LoadStyles";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtStyles = new DataTable();
            dtStyles = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtStyles;
        }

        public DataTable LoadSizes(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetSizes";
            string stCallingMethod = "cCampaignSelections.LoadSizes";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtSizes = new DataTable();
            dtSizes = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtSizes;
        }

        public DataTable LoadRadius(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetRadii";
            string stCallingMethod = "cCampaignSelections.LoadRadius";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtRadii = new DataTable();
            dtRadii = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtRadii;
        }

        public DataTable LoadGenres(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCodeFilter, int RadiusFilter)
        {
            string stStoredProc = "uspGetGenres";
            string stCallingMethod = "cCampaignSelections.LoadGenres";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@GameSystemFilter", GameSystemFilter);
            slParameters.Add("@CampaignFilter", CampaignFilter);
            slParameters.Add("@GenreFilter", GenreFilter);
            slParameters.Add("@StyleFilter", StyleFilter);
            slParameters.Add("@TechLevelFilter", TechLevelFilter);
            slParameters.Add("@SizeFilter", SizeFilter);
            slParameters.Add("@ZipCode", ZipCodeFilter);
            slParameters.Add("@RadiusFilter", RadiusFilter);
            DataTable dtGenres = new DataTable();
            dtGenres = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtGenres;
        }

        public DataTable LoadPeriods(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCode, int RadiusFilter)
        {
            DataTable dtPeriods = new DataTable();
            return dtPeriods;
        }

        public DataTable LoadWeapons(int UserID, string EndDate, int GameSystemFilter, int CampaignFilter, int GenreFilter, int StyleFilter, int TechLevelFilter, int SizeFilter, string ZipCode, int RadiusFilter)
        {
            DataTable dtWeapons = new DataTable();
            return dtWeapons;
        }

        /// <summary>
        /// Get the list of campaigns using the filters.
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="GameSystemFilter"></param>
        /// <param name="CampaignFilter"></param>
        /// <param name="GenreFilter"></param>
        /// <param name="StyleFilter"></param>
        /// <param name="TechLevelFilter"></param>
        /// <param name="SizeFilter"></param>
        /// <param name="ZipCodeFilter"></param>
        /// <param name="RadiusFilter"></param>
        /// <returns>Table 0 - filtered campaigns, table 1 - list of genres</returns>
        public DataSet LoadFilteredCampaigns(string UserName, int? GameSystemFilter, int? CampaignFilter, int? GenreFilter, int? StyleFilter,
            int? TechLevelFilter, int? SizeFilter, string ZipCodeFilter, int? RadiusFilter)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //if (EndDate == "")
            //{
            //    EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            //}
            SortedList slParameters = new SortedList();
            if (GameSystemFilter.HasValue)
                slParameters.Add("@GameSystemFilter", GameSystemFilter.Value);
            if (CampaignFilter.HasValue)
                slParameters.Add("@CampaignFilter", CampaignFilter.Value);
            if (GenreFilter.HasValue)
                slParameters.Add("@GenreFilter", GenreFilter.Value);
            if (StyleFilter.HasValue)
                slParameters.Add("@StyleFilter", StyleFilter.Value);
            if (TechLevelFilter.HasValue)
                slParameters.Add("@TechLevelFilter", TechLevelFilter.Value);
            if (SizeFilter.HasValue)
                slParameters.Add("@SizeFilter", SizeFilter.Value);
            if ((ZipCodeFilter.Length == 5) &&
                (RadiusFilter.HasValue))
            {
                slParameters.Add("@ZipCode", ZipCodeFilter);
                slParameters.Add("@RadiusFilter", RadiusFilter.Value);
            }
            DataSet dsGenres = cUtilities.LoadDataSet("uspGetFilteredCampaigns", slParameters, "LARPortal", UserName, lsRoutineName + ".uspGetFilteredCampaigns");
            return dsGenres;
        }
    }
}