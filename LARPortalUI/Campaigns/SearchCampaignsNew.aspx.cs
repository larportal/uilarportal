using LarpPortal.Classes;
using LarpPortal.Reports;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns
{
    public partial class SearchCampaignsNew : System.Web.UI.Page
    {
        public const int publicUserID = 2;
        public const string publicUserName = "Public";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //For testing purposes only. Remove when complete.
                //Session["UserID"] = 2;

                Session["DefaultCampaignLogoPath"] = "img/logo/";
                Session["DefaultCampaignLogoImage"] = "http://placehold.it/820x130";

                LoadDistanceFilter();
                LoadSortFilter();
                LoadFacetDropdownsOnce();
                RefreshSearchPage();
            }
        }

        private bool IsLoggedIn
        {
            get
            {
                return Session["UserID"] != null &&
                       !string.IsNullOrWhiteSpace(Session["UserID"].ToString());
            }
        }

        private int CurrentUserID
        {
            get
            {
                int userID;
                if (Session["UserID"] != null &&
                    int.TryParse(Session["UserID"].ToString(), out userID))
                    return userID;

                return 0;
            }
        }

        protected void rptCampaigns_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlGenericControl spnPC = (HtmlGenericControl)e.Item.FindControl("spnPC");
            HtmlGenericControl spnNPC = (HtmlGenericControl)e.Item.FindControl("spnNPC");
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            CheckBox chkPC = (CheckBox)e.Item.FindControl("chkPC");
            CheckBox chkNPC = (CheckBox)e.Item.FindControl("chkNPC");
            Button btnAddCampaign = (Button)e.Item.FindControl("btnAddCampaign");

            if (spnPC != null) spnPC.Visible = true;
            if (spnNPC != null) spnNPC.Visible = true;
            if (!IsLoggedIn)
                {
                    if (spnPC != null) spnPC.Visible = false;
                    if (spnNPC != null) spnNPC.Visible = false;
                    if (btnAddCampaign != null) btnAddCampaign.Visible = false;
                    return;
                }

            DataRowView drv = e.Item.DataItem as DataRowView;
            if (drv == null)
                return;

            int campaignID;
            if (!int.TryParse(drv["CampaignID"].ToString(), out campaignID))
                return;

            Classes.cPlayerRole role = new Classes.cPlayerRole();
            role.Load(CurrentUserID, 0, campaignID);

            bool isPC = string.Equals(role.IsPC, "true", StringComparison.OrdinalIgnoreCase);
            bool isNPC = string.Equals(role.IsNPC, "true", StringComparison.OrdinalIgnoreCase);

            if (chkPC != null)
            {
                chkPC.Checked = isPC;
                chkPC.Enabled = !isPC;   // already PC: checked but cannot uncheck
                chkPC.Visible = true;
            }

            if (chkNPC != null)
            {
                chkNPC.Checked = isNPC;
                chkNPC.Enabled = !isNPC; // already NPC: checked but cannot uncheck
                chkNPC.Visible = true;
            }

            if (btnAddCampaign != null)
            {
                bool canAddPC = !isPC;
                bool canAddNPC = !isNPC;

                btnAddCampaign.Visible = canAddPC || canAddNPC;
                btnAddCampaign.Enabled = false; // enabled by JavaScript when user checks an available box
                btnAddCampaign.Text = "Add Campaign";
            }
        }

        private int SafeInt(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : 0;
        }

        protected string SafeEval(object value, string fallback = "")
        {
            if (value == null || value == DBNull.Value)
                return fallback;

            return value.ToString();
        }

        private void BindDropdown(DropDownList ddl, DataTable dt, string selectValue = "0")
        {
            ddl.ClearSelection();
            ddl.SelectedIndex = -1;

            ddl.DataSource = null;
            ddl.Items.Clear();

            ddl.AppendDataBoundItems = true;
            ddl.Items.Add(new ListItem("-Select-", selectValue));

            if (dt != null)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
            }

            ddl.ClearSelection();

            ListItem selectItem = ddl.Items.FindByValue(selectValue);
            if (selectItem != null)
                selectItem.Selected = true;
        }

        private void SetSelectedValue(DropDownList ddl, string value)
        {
            if (value == null)
                value = "";

            ListItem li = ddl.Items.FindByValue(value);
            if (li != null)
                ddl.SelectedValue = value;
        }

        private void LoadSortFilter()
        {
            //ddlSort.Items.Clear();
            //ddlSort.Items.Add(new ListItem("Campaign Name", "CampaignName"));
            //ddlSort.Items.Add(new ListItem("State", "State"));
            //ddlSort.Items.Add(new ListItem("Distance", "Distance"));
            //ddlSort.Items.Add(new ListItem("Genre", "Genre"));
            //ddlSort.Items.Add(new ListItem("Style", "Style"));
            //ddlSort.Items.Add(new ListItem("Tech", "Tech"));
            //ddlSort.Items.Add(new ListItem("Size", "Size"));

            //ddlSort.SelectedValue = "CampaignName";
        }

        private void LoadDistanceFilter()
        {
            cCampaignSelection cCampaign = new cCampaignSelection();

            DataTable dtDistance = cCampaign.LoadRadius(publicUserID, "", 0, 0, 0, 0, 0, 0, "", 0);

            ddlDistanceFilter.Items.Clear();
            ddlDistanceFilter.Items.Add(new ListItem("-Select-", "0"));

            if (dtDistance != null)
            {
                foreach (DataRow dr in dtDistance.Rows)
                {
                    string text = "";
                    string value = "";

                    if (dtDistance.Columns.Contains("DistanceDescription"))
                        text = SafeEval(dr["DistanceDescription"]);
                    else if (dtDistance.Columns.Contains("MaximumDistance"))
                        text = SafeEval(dr["MaximumDistance"]);

                    if (dtDistance.Columns.Contains("DistanceID"))
                        value = SafeEval(dr["DistanceID"]);
                    else if (dtDistance.Columns.Contains("RadiusID"))
                        value = SafeEval(dr["RadiusID"]);

                    if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                        ddlDistanceFilter.Items.Add(new ListItem(text, value));
                }
            }

            ddlDistanceFilter.SelectedValue = "0";
        }

        private void RefreshSearchPage()
        {
            int campaignFilter = SafeInt(ddlNameFilter.SelectedValue);
            string stateFilter = ddlStateFilter.SelectedValue;
            int gameSystemFilter = SafeInt(ddlSystemFilter.SelectedValue);
            int genreFilter = SafeInt(ddlGenreFilter.SelectedValue);
            int styleFilter = SafeInt(ddlStyleFilter.SelectedValue);
            int techFilter = SafeInt(ddlTechFilter.SelectedValue);
            int sizeFilter = SafeInt(ddlSizeFilter.SelectedValue);
            int distanceFilter = SafeInt(ddlDistanceFilter.SelectedValue);

            string originalZipText = txtZipFilter.Text.Trim();
            string zipCode = NormalizePostalCode(originalZipText);
            string sortBy = "CampaignName";

            bool userEnteredZip = !string.IsNullOrWhiteSpace(originalZipText);
            bool postalCodeValid = false;

            if (!string.IsNullOrWhiteSpace(zipCode))
                postalCodeValid = PostalCodeExists(zipCode);

            lblZipError.Visible = false;
            lblZipError.Text = "";

            if (userEnteredZip && !postalCodeValid)
            {
                lblZipError.Text = "Postal code not found.";
                lblZipError.Visible = true;

                zipCode = "";
                distanceFilter = 0;

                if (ddlDistanceFilter.Items.FindByValue("0") != null)
                    ddlDistanceFilter.SelectedValue = "0";
            }
            else if (!userEnteredZip)
            {
                zipCode = "";
                distanceFilter = 0;

                if (ddlDistanceFilter.Items.FindByValue("0") != null)
                    ddlDistanceFilter.SelectedValue = "0";
            }

            cCampaignSelection cCampaign = new cCampaignSelection();

            DataSet ds = cCampaign.LoadCampaignSearchData(
                publicUserID,
                "",
                campaignFilter,
                stateFilter,
                gameSystemFilter,
                genreFilter,
                styleFilter,
                techFilter,
                sizeFilter,
                zipCode,
                distanceFilter,
                sortBy,
                CurrentPage,
                PageSize
            );

            int totalRows = 0;

            if (ds != null &&
                ds.Tables.Count > 0 &&
                ds.Tables[0].Rows.Count > 0 &&
                ds.Tables[0].Columns.Contains("TotalRows"))
            {
                int.TryParse(ds.Tables[0].Rows[0]["TotalRows"].ToString(), out totalRows);
            }

            int totalPages = totalRows == 0
                ? 1
                : (int)Math.Ceiling((double)totalRows / PageSize);

            if (CurrentPage > totalPages)
            {
                CurrentPage = totalPages;

                ds = cCampaign.LoadCampaignSearchData(
                    publicUserID,
                    "",
                    campaignFilter,
                    stateFilter,
                    gameSystemFilter,
                    genreFilter,
                    styleFilter,
                    techFilter,
                    sizeFilter,
                    zipCode,
                    distanceFilter,
                    sortBy,
                    CurrentPage,
                    PageSize
                );
            }

            lblPageInfo.Text = "Page " + CurrentPage.ToString() + " of " + totalPages.ToString();

            btnPreviousPage.Enabled = CurrentPage > 1;
            btnNextPage.Enabled = CurrentPage < totalPages;

            BindCampaigns(ds);

            RestoreSelections(
                campaignFilter,
                stateFilter,
                gameSystemFilter,
                genreFilter,
                styleFilter,
                techFilter,
                sizeFilter,
                distanceFilter,
                sortBy,
                userEnteredZip && !postalCodeValid ? originalZipText : zipCode
            );
        }

        private void LoadFacetDropdownsOnce()
        {
            cCampaignSelection cCampaign = new cCampaignSelection();

            DataSet ds = cCampaign.LoadCampaignSearchData(
                publicUserID,
                "",
                0,              // campaignFilter
                "",             // stateFilter
                0,              // gameSystemFilter
                0,              // genreFilter
                0,              // styleFilter
                0,              // techFilter
                0,              // sizeFilter
                "",             // zipCode
                0,              // distanceFilter
                "CampaignName", // sortBy
                1,              // pageNumber
                999999          // pageSize
            );

            if (ds == null || ds.Tables.Count < 8)
                return;

            BindDropdown(ddlNameFilter, ds.Tables[1], "0");
            BindDropdown(ddlStateFilter, ds.Tables[2], "");
            BindDropdown(ddlSystemFilter, ds.Tables[3], "0");
            BindDropdown(ddlGenreFilter, ds.Tables[4], "0");
            BindDropdown(ddlStyleFilter, ds.Tables[5], "0");
            BindDropdown(ddlTechFilter, ds.Tables[6], "0");
            BindDropdown(ddlSizeFilter, ds.Tables[7], "0");
        }

        private string NormalizePostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return "";

            postalCode = postalCode.Trim().ToUpper();

            // US ZIP: keep first 5 digits
            if (System.Text.RegularExpressions.Regex.IsMatch(postalCode, @"^\d{5}"))
                return postalCode.Substring(0, 5);

            // Canadian: A1A1A1 → A1A 1A1
            if (System.Text.RegularExpressions.Regex.IsMatch(postalCode, @"^[A-Z]\d[A-Z]\d[A-Z]\d$"))
                return postalCode.Insert(3, " ");

            // Already formatted Canadian
            if (System.Text.RegularExpressions.Regex.IsMatch(postalCode, @"^[A-Z]\d[A-Z] \d[A-Z]\d$"))
                return postalCode;

            return postalCode;
        }

        private void BindCampaigns(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                rptCampaigns.DataSource = ds.Tables[0];
                rptCampaigns.DataBind();
            }
            else
            {
                rptCampaigns.DataSource = null;
                rptCampaigns.DataBind();
            }
        }

        protected string RenderLink(object urlObj, string linkText, bool showNA = true)
        {
            string url = SafeEval(urlObj).Trim();

            if (string.IsNullOrWhiteSpace(url))
                return showNA ? $"{linkText} N/A" : linkText;

            if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                url = "https://" + url;

            return $"<a href='{url}' target='_blank' rel='noopener noreferrer'>{linkText}</a>";
        }

        protected string GetLogoPath(object logoUrl)
        {
            string file = SafeEval(logoUrl);

            if (string.IsNullOrWhiteSpace(file))
                return "";

            // If it's already a full URL, don't prepend
            if (file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return file;

            return "/img/logo/" + file;
        }

        protected string GetLogoStyle(object widthObj, object heightObj)
        {
            int boxWidth = 170;
            int boxHeight = 100;

            int imgWidth = SafeInt(SafeEval(widthObj));
            int imgHeight = SafeInt(SafeEval(heightObj));

            if (imgWidth <= 0) imgWidth = boxWidth;
            if (imgHeight <= 0) imgHeight = boxHeight;

            decimal imgRatio = (decimal)imgWidth / imgHeight;
            decimal boxRatio = (decimal)boxWidth / boxHeight;

            int finalWidth;
            int finalHeight;

            if (imgRatio > boxRatio)
            {
                // Image is wider than the box
                finalWidth = boxWidth;
                finalHeight = Convert.ToInt32(Math.Round(boxWidth / imgRatio, 0));
            }
            else
            {
                // Image is taller/narrower than the box
                finalHeight = boxHeight;
                finalWidth = Convert.ToInt32(Math.Round(boxHeight * imgRatio, 0));
            }

            return $"width:{finalWidth}px; height:{finalHeight}px; object-fit:contain; display:block;";
        }

        private void BindFacetDropdowns(DataSet ds)
        {
            if (ds == null)
                return;

            if (ds.Tables.Count > 1)
                BindDropdown(ddlNameFilter, ds.Tables[1], "0");

            if (ds.Tables.Count > 2)
                BindDropdown(ddlStateFilter, ds.Tables[2], "");

            if (ds.Tables.Count > 3)
                BindDropdown(ddlSystemFilter, ds.Tables[3], "0");

            if (ds.Tables.Count > 4)
                BindDropdown(ddlGenreFilter, ds.Tables[4], "0");

            if (ds.Tables.Count > 5)
                BindDropdown(ddlStyleFilter, ds.Tables[5], "0");

            if (ds.Tables.Count > 6)
                BindDropdown(ddlTechFilter, ds.Tables[6], "0");

            if (ds.Tables.Count > 7)
                BindDropdown(ddlSizeFilter, ds.Tables[7], "0");
        }

        private void RestoreSelections(
            int campaignFilter,
            string stateFilter,
            int gameSystemFilter,
            int genreFilter,
            int styleFilter,
            int techFilter,
            int sizeFilter,
            int distanceFilter,
            string sortBy,
            string zipCode)
        {
            SetSelectedValue(ddlNameFilter, campaignFilter.ToString());
            SetSelectedValue(ddlStateFilter, stateFilter);
            SetSelectedValue(ddlSystemFilter, gameSystemFilter.ToString());
            SetSelectedValue(ddlGenreFilter, genreFilter.ToString());
            SetSelectedValue(ddlStyleFilter, styleFilter.ToString());
            SetSelectedValue(ddlTechFilter, techFilter.ToString());
            SetSelectedValue(ddlSizeFilter, sizeFilter.ToString());
            SetSelectedValue(ddlDistanceFilter, distanceFilter.ToString());
            //SetSelectedValue(ddlSort, sortBy);
            txtZipFilter.Text = zipCode;
        }

        protected void rptCampaigns_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "AddCampaign")
                return;

            int campaignID;
            if (!int.TryParse(e.CommandArgument.ToString(), out campaignID))
                return;

            CheckBox chkPC = (CheckBox)e.Item.FindControl("chkPC");
            CheckBox chkNPC = (CheckBox)e.Item.FindControl("chkNPC");

            bool addedAny = false;

            if (chkPC != null && chkPC.Checked && chkPC.Enabled)
            {
                SignUpForSelectedRole(8, CurrentUserID, campaignID, 55);
                addedAny = true;
            }

            if (chkNPC != null && chkNPC.Checked && chkNPC.Enabled)
            {
                SignUpForSelectedRole(10, CurrentUserID, campaignID, 55);
                addedAny = true;
            }

            RefreshSearchPage();
        }

        protected void AddCampaignToUserList(int campaignID)
        {
            if (!IsLoggedIn || CurrentUserID <= 0)
                return;

            SignUpForSelectedRole(8, CurrentUserID, campaignID, 55);

        }


        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] == null ? 1 : (int)ViewState["CurrentPage"]; }
            set { ViewState["CurrentPage"] = value; }
        }

        private int PageSize
        {
            get { return 25; }
        }

        protected void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            RefreshSearchPage();
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            CurrentPage++;

            RefreshSearchPage();
        }


        protected void btnAddCampaign_Click(object sender, EventArgs e)
        {
        }

        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            hidBlink.Value = "";
            btnApplyFilters.CssClass = btnApplyFilters.CssClass.Replace("button-glow", "").Trim();

            CurrentPage = 1;
            RefreshSearchPage();
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            SetSelectedValue(ddlNameFilter, "0");
            SetSelectedValue(ddlStateFilter, "");
            SetSelectedValue(ddlSystemFilter, "0");
            SetSelectedValue(ddlGenreFilter, "0");
            SetSelectedValue(ddlStyleFilter, "0");
            SetSelectedValue(ddlTechFilter, "0");
            SetSelectedValue(ddlSizeFilter, "0");
            SetSelectedValue(ddlDistanceFilter, "0");

            txtZipFilter.Text = "";

            ddlDistanceFilter.Enabled = false;

            LoadFacetDropdownsOnce();

            CurrentPage = 1;
            RefreshSearchPage();
        }

        private bool PostalCodeExists(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return false;

            cCampaignSelection cCampaign = new cCampaignSelection();
            return cCampaign.PostalCodeExists(postalCode);
        }

        protected void SignUpForSelectedRole(int roleToSignUp, int userID, int campaignID, int statusID)
        {
            int campaignPlayerID = 0;

            Classes.cUserCampaign campaignPlayer = new Classes.cUserCampaign();
            campaignPlayer.Load(userID, campaignID);

            campaignPlayerID = campaignPlayer.CampaignPlayerID;

            if (campaignPlayerID == -1)
            {
                CreatePlayerInCampaign(userID, campaignID);

                campaignPlayer.Load(userID, campaignID);
                campaignPlayerID = campaignPlayer.CampaignPlayerID;
            }

            int roleAlignment = 2;

            if (roleToSignUp == 8)
                roleAlignment = 1;

            Classes.cPlayerRole playerRole = new Classes.cPlayerRole();
            playerRole.CampaignPlayerRoleID = -1;
            playerRole.CampaignPlayerID = campaignPlayerID;
            playerRole.RoleID = roleToSignUp;
            playerRole.RoleAlignmentID = roleAlignment;
            playerRole.Save(userID);

            Session["ReloadCampaigns"] = "Y";
        }

        protected void CreatePlayerInCampaign(int userID, int campaignID)
        {
            Classes.cUserCampaign userCampaign = new Classes.cUserCampaign();
            userCampaign.CampaignPlayerID = -1;
            userCampaign.CampaignID = campaignID;
            userCampaign.UserDisplayMyCampaigns = true;
            userCampaign.Save(userID);
        }

    }
}