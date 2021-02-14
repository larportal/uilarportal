using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

namespace LarpPortal.Profile
{
    public partial class Profile : System.Web.UI.Page
    {
        public string PictureDirectory = "../Pictures";
        //public //string _UserName = "";
        //public int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //if (Session["Username"] != null)
            //    _UserName = Session["Username"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);

            tbFirstName.Attributes.Add("PlaceHolder", "First Name");
            tbMiddleName.Attributes.Add("PlaceHolder", "Middle Name");
            tbLastName.Attributes.Add("PlaceHolder", "Last Name");
            tbNickName.Attributes.Add("PlaceHolder", "Nick Name");
            //tbBirthPlace.Attributes.Add("PlaceHolder", "Birth Place");
            tbUserName.Attributes.Add("Placeholder", "User Name");
            tbPenName.Attributes.Add("Placeholder", "Pen Name");
            tbForumName.Attributes.Add("Placeholder", "Forum Name");

            ddlEnterPhoneType.Attributes.Add("onchange", "DisplayPhoneProvider(this);");
            btnClosePhoneNumber.Attributes.Add("data-dismiss", "modal");
            btnCancelDeletePhoneNumber.Attributes.Add("data-dismiss", "modal");
            btnCloseEnterEMail.Attributes.Add("data-dismiss", "modal");
            btnCancelDeleteEMail.Attributes.Add("data-dismiss", "modal");
            btnCloseEnterAddress.Attributes.Add("data-dismiss", "modal");
            btnCancelDeleteAddress.Attributes.Add("data-dismiss", "modal");

            if (!IsPostBack)
            {
                tbFirstName.Attributes.Add("Placeholder", "First Name");
                tbLastName.Attributes.Add("Placeholder", "Last Name");
                lblMessage.Text = "";

                SortedList sParam = new SortedList();
                DataTable dtPhoneTypes = cUtilities.LoadDataTable("uspGetPhoneTypes", sParam, "LARPortal", Master.UserName, lsRoutineName);
                DataView dvPhoneTypes = new DataView(dtPhoneTypes, "", "SortOrder", DataViewRowState.CurrentRows);
                ddlEnterPhoneType.DataSource = dtPhoneTypes;
                ddlEnterPhoneType.DataTextField = "PhoneType";
                ddlEnterPhoneType.DataValueField = "PhoneTypeID";
                ddlEnterPhoneType.DataBind();

                sParam = new SortedList();
                DataTable dtPhoneProvider = cUtilities.LoadDataTable("uspGetPhoneProviders", sParam, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPhoneProviders");
                ddlEnterProvider.DataSource = dtPhoneProvider;
                ddlEnterProvider.DataTextField = "ProviderName";
                ddlEnterProvider.DataValueField = "PhoneProviderID";
                ddlEnterProvider.DataBind();

                sParam = new SortedList();
                DataTable dtEMailTypes = cUtilities.LoadDataTable("uspGetEMailTypes", sParam, "LARPortal", Master.UserName, lsRoutineName + ".uspGetEMailTypes");
                ddlEnterEMailType.DataSource = dtEMailTypes;
                ddlEnterEMailType.DataTextField = "EMailType";
                ddlEnterEMailType.DataValueField = "EMailTypeID";
                ddlEnterEMailType.DataBind();

                sParam = new SortedList();
                DataTable dtAddressTypes = cUtilities.LoadDataTable("uspGetAddressTypes", sParam, "LARPortal", Master.UserName, lsRoutineName + ".uspGetAddressTypes");
                ddlEnterAddressType.DataSource = dtAddressTypes;
                ddlEnterAddressType.DataTextField = "AddressType";
                ddlEnterAddressType.DataValueField = "AddressTypeID";
                ddlEnterAddressType.DataBind();

                sParam = new SortedList();
                DataTable dtStateList = cUtilities.LoadDataTable("uspGetStateList", sParam, "LARPortal", Master.UserName, lsRoutineName + ".uspGetStateList");
                ddlEnterState.DataSource = dtStateList;
                ddlEnterState.DataTextField = "DisplayState";
                ddlEnterState.DataValueField = "StateID";
                ddlEnterState.DataBind();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            Classes.cUser Demography = null;
            Classes.cPlayer PLDemography = null;

            Demography = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
            PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);

            if (!IsPostBack)
            {
                string gen = PLDemography.GenderStandared.ToUpper();
                string othergen = PLDemography.GenderOther;

                string pict = PLDemography.UserPhoto;

				if (PLDemography.HasPicture)
				{
					imgPlayerImage.ImageUrl = PLDemography.Picture.PictureURL;
					ViewState["UserIDPicture"] = PLDemography.Picture.PictureID;
				}
				else
				{
					if (PLDemography.UserPhoto.Length > 0)
					{
						imgPlayerImage.ImageUrl = PLDemography.UserPhoto;
						ViewState.Remove("UserIDPicture");
					}
					else
					{
						imgPlayerImage.ImageUrl = "http://placehold.it/150x150";
						ViewState.Remove("UserIDPicture");
					}
				}

                string emergencyContactPhone = string.Empty;
                if (PLDemography.EmergencyContactPhone != null)
                {
                    emergencyContactPhone = PLDemography.EmergencyContactPhone;
                    Int32 iPhone;
                    if (Int32.TryParse(emergencyContactPhone.Replace("(", "").Replace(")", "").Replace("-", ""), out iPhone))
                    {
                        emergencyContactPhone = iPhone.ToString("(###)###-####");
                    }
                }

                tbFirstName.Text = Demography.FirstName;
                tbMiddleName.Text = Demography.MiddleName;
                tbLastName.Text = Demography.LastName;

                tbGenderOther.Style.Add("visibility", "hidden");
                tbGenderOther.Text = othergen;

                if (gen.Length > 0)
                {
                    if ("MFO".Contains(gen))
                        ddlGender.SelectedValue = gen;
                    if (gen == "O")
                        tbGenderOther.Style.Add("visibility", "visible");
                }

                tbBDMM.Value = PLDemography.DateOfBirth.Month.ToString();
                tbBDDD.Value = PLDemography.DateOfBirth.Day.ToString();
                tbBDYYYY.Value = PLDemography.DateOfBirth.Year.ToString();

                tbEmergencyName.Text = PLDemography.EmergencyContactName;
                tbEmergencyPhone.Text = emergencyContactPhone;
                tbUserName.Text = Master.UserName;
                tbNickName.Text = Demography.NickName;
                tbPenName.Text = PLDemography.AuthorName;
                tbForumName.Text = Demography.ForumUserName;

                ddlGender.Attributes.Add("onchange", "DisplaySexOther(this);");
            }

            //            lblErrorMessage1.Text = "";
            //            lblErrorMessage2.Text = "";
            //            btnSave1.Enabled = true;
            //            btnSave2.Enabled = true;

            hidNumOfPhones.Value = Demography.UserPhones.Count.ToString();
            gvPhoneNumbers.DataSource = Demography.UserPhones;
            gvPhoneNumbers.DataBind();

            hidNumOfEMails.Value = Demography.UserEmails.Count.ToString();
            gvEmails.DataSource = Demography.UserEmails;
            gvEmails.DataBind();

            hidNumOfAddresses.Value = Demography.UserAddresses.Count.ToString();
            gvAddresses.DataSource = Demography.UserAddresses;
            gvAddresses.DataBind();

            //if (gvEmails.Rows.Count == 0)
            //{
            //    if (gvEmails.Rows.Count == 0)
            //        lblErrorMessage1.Text += "* You must have at least one valid email address. ";
            //    lblErrorMessage1.Text = lblErrorMessage1.Text.Trim();
            //    btnSave1.Enabled = false;
            //}

            //lblErrorMessage2.Text = lblErrorMessage1.Text;
            //btnSave2.Enabled = btnSave1.Enabled;
        }

        protected void gvAddresses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (hidNumOfAddresses.Value == "1")
                {
                    LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                    if (lbtnDelete != null)
                        lbtnDelete.Attributes.Add("style", "display:none");
                }
            }
        }

        protected void btnSaveAddress_Click(object sender, EventArgs e)
        {
            int iAddressID;
            if (int.TryParse(hidAddressID.Value, out iAddressID))
            {
                Classes.cAddress UpdateAddress = new Classes.cAddress(iAddressID, Master.UserName, Master.UserID);
                UpdateAddress.AddressID = iAddressID;
                UpdateAddress.Address1 = tbEnterAddress1.Text;
                UpdateAddress.Address2 = tbEnterAddress2.Text;
                UpdateAddress.City = tbEnterCity.Text;
                UpdateAddress.StateID = ddlEnterState.SelectedValue;
                UpdateAddress.PostalCode = tbEnterZipCode.Text;
                UpdateAddress.Country = tbEnterCountry.Text;
                UpdateAddress.IsPrimary = cbxEnterAddressPrimary.Checked;

                int iTemp;
                if (int.TryParse(ddlEnterAddressType.SelectedValue, out iTemp))
                    UpdateAddress.AddressTypeID = iTemp;

                UpdateAddress.SaveUpdate(Master.UserID);
            }
        }

        protected void btnDeleteAddress_Click(object sender, EventArgs e)
        {
            int iAddressID;
            if (int.TryParse(hidDeleteAddressID.Value, out iAddressID))
            {
                Classes.cAddress UpdateAddress = new Classes.cAddress(iAddressID, Master.UserName, Master.UserID);
                UpdateAddress.SaveUpdate(Master.UserID, true);
            }
        }


        protected void gvPhoneNumbers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (hidNumOfPhones.Value == "1")
                {
                    LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                    if (lbtnDelete != null)
                        lbtnDelete.Attributes.Add("style", "display:none");
                }
            }
        }

        protected void btnSavePhone_Click(object sender, EventArgs e)
        {
            int iPhoneNumberID;
            if (int.TryParse(hidEnterPhoneID.Value, out iPhoneNumberID))
            {
                Classes.cPhone UpdatePhone = new Classes.cPhone(iPhoneNumberID, Master.UserID, Master.UserName);
                UpdatePhone.PhoneNumberID = iPhoneNumberID;
                UpdatePhone.AreaCode = tbEnterAreaCode.Text;
                UpdatePhone.PhoneNumber = tbEnterPhoneNumber.Text;
                UpdatePhone.Extension = tbEnterExtension.Text;
                UpdatePhone.IsPrimary = cbxEnterPrimary.Checked;

                int iTemp;
                if (int.TryParse(ddlEnterPhoneType.SelectedValue, out iTemp))
                    UpdatePhone.PhoneTypeID = iTemp;
                if (int.TryParse(ddlEnterProvider.SelectedValue, out iTemp))
                    UpdatePhone.ProviderID = iTemp;

                UpdatePhone.SaveUpdate(Master.UserID);
            }
        }

        protected void btnDeletePhone_Click(object sender, EventArgs e)
        {
            int iPhoneNumberID;
            if (int.TryParse(hidDeletePhoneID.Value, out iPhoneNumberID))
            {
                Classes.cPhone UpdatePhone = new Classes.cPhone(iPhoneNumberID, Master.UserID, Master.UserName);
                UpdatePhone.SaveUpdate(Master.UserID, true);
            }
        }

        protected void gvEmails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (hidNumOfEMails.Value == "1")
                {
                    LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                    if (lbtnDelete != null)
                        lbtnDelete.Attributes.Add("style", "display:none");
                }
            }
        }

        protected void btnSaveEMail_Click(object sender, EventArgs e)
        {
            int iEMailID;
            if (int.TryParse(hidEnterEMailID.Value, out iEMailID))
            {
                Classes.cEMail UpdateEMail = new Classes.cEMail(iEMailID, Master.UserName, Master.UserID);
                UpdateEMail.EMailID = iEMailID;
                UpdateEMail.EmailAddress = tbEnterEMailAddress.Text;

                int iTemp;
                if (int.TryParse(ddlEnterEMailType.SelectedValue, out iTemp))
                    UpdateEMail.EmailTypeID = iTemp;
                UpdateEMail.IsPrimary = cbxEnterEMailPrimary.Checked;

                UpdateEMail.SaveUpdate(Master.UserID);
            }
        }

        protected void btnDeleteEMail_Click(object sender, EventArgs e)
        {
            int iEMailID;
            if (int.TryParse(hidDeleteEMailID.Value, out iEMailID))
            {
                Classes.cEMail UpdateEMail = new Classes.cEMail(iEMailID, Master.UserName, Master.UserID);
                UpdateEMail.SaveUpdate(Master.UserID, true);
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            cUser Demography = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
            cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);

            Demography.FirstName = tbFirstName.Text.Trim();
            Demography.MiddleName = tbMiddleName.Text.Trim();
            Demography.LastName = tbLastName.Text.Trim();
            Demography.NickName = tbNickName.Text;
			Demography.ForumUserName = tbForumName.Text.Trim();
			PLDemography.EmergencyContactName = tbEmergencyName.Text.Trim();
			PLDemography.EmergencyContactPhone = tbEmergencyPhone.Text.Trim();

            if (ddlGender.SelectedIndex != -1)
                PLDemography.GenderStandared = ddlGender.SelectedValue;

            PLDemography.GenderOther = tbGenderOther.Text;

            if (string.IsNullOrWhiteSpace(tbUserName.Text)) //If left empty set back to original setting...They may not remember it....
            {
                tbUserName.Text = Demography.LoginName;
            }

            // 1 - No duplicate usernames allowed
            Classes.cLogin Login = new Classes.cLogin();
            Login.CheckForExistingUsername(tbUserName.Text);
            if (Login.MemberID != 0 && Login.MemberID != Demography.UserID)  // UserID is taken
            {
                lblMessage.Text = "This username is already in use.  Please select a different one.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                tbUserName.Focus();
                return;
            }
            else
            {
                Demography.LoginName = tbUserName.Text.Trim();
            }

            DateTime dtDOB;
            if (DateTime.TryParse(tbBDMM.Value + "/" + tbBDDD.Value + "/" + tbBDYYYY.Value, out dtDOB))
                PLDemography.DateOfBirth = dtDOB;
            else
            {
                lblMessage.Text = "Please enter a valid date";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                tbBDMM.Focus();
                return;
            }

            PLDemography.AuthorName = tbPenName.Text;
            Demography.ForumUserName = tbForumName.Text;

            PLDemography.EmergencyContactName = tbEmergencyName.Text;

            if (!cPhone.isValidPhoneNumber(tbEmergencyPhone.Text, 10))
            {
                lblMessage.Text = cPhone.ErrorDescription;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                tbEmergencyPhone.Focus();
                return;
            }
            else
                PLDemography.EmergencyContactPhone = tbEmergencyPhone.Text;

            PLDemography.PictureID = -1;

            if (ViewState["UserIDPicture"] != null)
            {
                int iTemp;
                if (int.TryParse(ViewState["UserIDPicture"].ToString(), out iTemp))
                    PLDemography.PictureID = iTemp;
                else
                    PLDemography.PictureID = -1;
                //PLDemography.UserPhoto = Session["dem_Img_Url"].ToString();
                //imgPlayerImage.ImageUrl = Session["dem_Img_Url"].ToString();
                //Session["dem_Img_Url"] = "";
                //Session.Remove("dem_Img_Id");

                //Classes.cPicture NewPicture = new Classes.cPicture();
                //int iPictureId =0;
                //if (Session["dem_Img_Id"] != null && Int32.TryParse(Session["dem_Img_Id"].ToString(), out iPictureId))
                //{
                //    //This code will be enabled once the stored procedure is created
                //    string userID = Session["UserID"].ToString();
                //    //NewPicture.Load(iPictureId, userID);
                //    //NewPicture.PictureFileName = NewPicture.PictureFileName.Replace("_2", "_1");
                //    //NewPicture.Save(userID);
                //    //Time to trash the old main picture with the picture in memory                    
                //    //PLDemography.UserPhoto = NewPicture.PictureFileName;                    
                //}
            }
            else
                PLDemography.PictureID = -1;

            Demography.Save();
            PLDemography.Save();
            //Session["Username"] = Demography.LoginName;

            lblMessage.Text = "Changes saved successfully.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }


		protected void btnSavePicture_Click(object sender, EventArgs e)
		{
			if (ulFile.HasFile)
			{
				try
				{
					string sUser = Session["LoginName"].ToString();
					Classes.cPicture NewPicture = new Classes.cPicture();
					NewPicture.PictureType = Classes.cPicture.PictureTypes.Profile;
					NewPicture.CreateNewPictureRecord(sUser);
					string sExtension = Path.GetExtension(ulFile.FileName);
					NewPicture.PictureFileName = "PL" + NewPicture.PictureID.ToString("D10") + sExtension;

					//int iCharacterID = 0;
					//int.TryParse(ViewState["CurrentCharacter"].ToString(), out iCharacterID);
					//NewPicture.CharacterID = iCharacterID;

					string LocalName = NewPicture.PictureLocalName;

					if (!Directory.Exists(Path.GetDirectoryName(NewPicture.PictureLocalName)))
						Directory.CreateDirectory(Path.GetDirectoryName(NewPicture.PictureLocalName));

					ulFile.SaveAs(NewPicture.PictureLocalName);
					NewPicture.Save(sUser);

					ViewState["UserIDPicture"] = NewPicture.PictureID;
					ViewState.Remove("PictureDeleted");

					imgPlayerImage.ImageUrl = NewPicture.PictureURL;
					imgPlayerImage.Visible = true;
					btnClearPicture.Visible = true;
				}
				catch (Exception ex)
				{
					lblMessage.Text = ex.Message + "<br>" + ex.StackTrace;
				}
			}
		}

		protected void btnClearPicture_Click(object sender, EventArgs e)
		{
			if (ViewState["UserIDPicture"] != null)
				ViewState.Remove("UserIDPicture");

			imgPlayerImage.Visible = false;
			btnClearPicture.Visible = false;

			//SortedList sParam = new SortedList();
			//sParam.Add("@CharacterID", Session["SelectedCharacter"].ToString());

			//Classes.cUtilities.LoadDataTable("uspClearCharacterProfilePicture", sParam, "LARPortal", Session["UserID"].ToString(), "CharInfo.btnClearPicture");
		}








    }
}