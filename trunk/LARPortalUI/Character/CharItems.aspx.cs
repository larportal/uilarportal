using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
    public partial class CharItems : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
        {
            oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;

            if (Session["PictureList"] == null)
                Session["PictureList"] = "";
            if (!IsPostBack)
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if (oCharSelect.CharacterID.HasValue)
            {
                tbCostume.Text = oCharSelect.CharacterInfo.Costuming;
                lblCostume.Text = oCharSelect.CharacterInfo.Costuming;
                tbWeapons.Text = oCharSelect.CharacterInfo.Weapons;
                lblWeapons.Text = oCharSelect.CharacterInfo.Weapons;
                tbMakeup.Text = oCharSelect.CharacterInfo.Makeup;
                lblMakeup.Text = oCharSelect.CharacterInfo.Makeup;
                tbAccessories.Text = oCharSelect.CharacterInfo.Accessories;
                lblAccessories.Text = oCharSelect.CharacterInfo.Accessories;
                tbOtherItems.Text = oCharSelect.CharacterInfo.Items;
                lblOtherItems.Text = oCharSelect.CharacterInfo.Items;

                if ((oCharSelect.CharacterInfo.CharacterType != 1) && (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters))
                {
                    btnSave.Enabled = false;
                    btnSaveTop.Enabled = false;

                    tbCostume.Visible = false;
                    lblCostume.Visible = true;
                    tbWeapons.Visible = false;
                    lblWeapons.Visible = true;
                    tbMakeup.Visible = false;
                    lblMakeup.Visible = true;
                    tbAccessories.Visible = false;
                    lblAccessories.Visible = true;
                    tbOtherItems.Visible = false;
                    lblOtherItems.Visible = true;
                    divAddPicture.Visible = false;
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSaveTop.Enabled = true;

                    tbCostume.Visible = true;
                    lblCostume.Visible = false;
                    tbWeapons.Visible = true;
                    lblWeapons.Visible = false;
                    tbMakeup.Visible = true;
                    lblMakeup.Visible = false;
                    tbAccessories.Visible = true;
                    lblAccessories.Visible = false;
                    tbOtherItems.Visible = true;
                    lblOtherItems.Visible = false;
                    divAddPicture.Visible = true;
                }

                DataTable dtPictures = new DataTable();
                dtPictures = Classes.cUtilities.CreateDataTable(oCharSelect.CharacterInfo.Pictures);
                Session["PictureList"] = oCharSelect.CharacterInfo.Pictures;

                string sFilter = "RecordStatus <> '" + ((int)Classes.RecordStatuses.Delete).ToString() + "' and " +
                    "PictureType = " + ((int)Classes.cPicture.PictureTypes.Item).ToString();
                DataView dvPictures = new DataView(dtPictures, sFilter, "", DataViewRowState.CurrentRows);
                dlItems.DataSource = dvPictures;
                dlItems.DataBind();
            }
            //ViewState["CurrentCharacter"] = oCharSelect.CharacterID.Value;
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fuItem.HasFile)
            {
                Classes.cPicture NewPicture = new Classes.cPicture();
                NewPicture.CreateNewPictureRecord(Master.UserName);
                NewPicture.PictureType = Classes.cPicture.PictureTypes.Item;
                string sExtension = Path.GetExtension(fuItem.FileName);
                string filename = "CI" + NewPicture.PictureID.ToString("D10") + sExtension;

                NewPicture.PictureFileName = filename;

                if (!Directory.Exists(Path.GetDirectoryName(NewPicture.PictureLocalName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(NewPicture.PictureLocalName));

                string FinalFileName = NewPicture.PictureLocalName; // Path.Combine(Server.MapPath(Path.GetDirectoryName(NewPicture.PictureLocalName)), filename);
                fuItem.SaveAs(FinalFileName);

                //int iTemp;
                //if (int.TryParse(ViewState["CurrentCharacter"].ToString(), out iTemp))
                //    NewPicture.CharacterID = iTemp;

                if (oCharSelect.CharacterID.HasValue)
                    NewPicture.CharacterID = oCharSelect.CharacterID.Value;

                NewPicture.Save(Master.UserName);

                List<Classes.cPicture> PictureList = new List<Classes.cPicture>();
                PictureList = Session["PictureList"] as List<Classes.cPicture>;
                PictureList.Add(NewPicture);
                Session["PictureList"] = PictureList;

                DataTable dtPictures = Classes.cUtilities.CreateDataTable(PictureList);

                //if ( dtPictures.Columns["PictureURL"] == null )
                //    dtPictures.Columns.Add(new DataColumn("PictureURL", typeof(string)));

                string sFilter = "RecordStatus <> '" + ((int)Classes.RecordStatuses.Delete).ToString() + "' and " +
                    "PictureType = " + ((int)Classes.cPicture.PictureTypes.Item).ToString();
                DataView dvPictures = new DataView(dtPictures, sFilter, "", DataViewRowState.CurrentRows);
                dlItems.DataSource = dvPictures;
                dlItems.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();
            if (oCharSelect.CharacterID.HasValue)
            {
                oCharSelect.CharacterInfo.Costuming = tbCostume.Text;
                oCharSelect.CharacterInfo.Makeup = tbMakeup.Text;
                oCharSelect.CharacterInfo.Weapons = tbWeapons.Text;
                oCharSelect.CharacterInfo.Items = tbOtherItems.Text;
                oCharSelect.CharacterInfo.Accessories = tbAccessories.Text;
                oCharSelect.CharacterInfo.Pictures = Session["PictureList"] as List<Classes.cPicture>;
                oCharSelect.CharacterInfo.SaveCharacter(Master.UserName, Master.UserID);
                lblmodalMessage.Text = "Character " + oCharSelect.CharacterInfo.AKA + " has been saved.";
                btnCloseMessage.Attributes.Add("data-dismiss", "modal");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
            }
        }

        protected void dlItems_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int PictureID;
            if (int.TryParse(e.CommandArgument.ToString(), out PictureID))
            {
                List<Classes.cPicture> PictureList = new List<Classes.cPicture>();
                PictureList = Session["PictureList"] as List<Classes.cPicture>;

                foreach (Classes.cPicture Pict in Items)
                {
                    if (Pict.PictureID == PictureID)
                        Pict.RecordStatus = Classes.RecordStatuses.Delete;
                }
                Session["PictureList"] = PictureList;
                DataTable dtPictures = Classes.cUtilities.CreateDataTable(PictureList);
                string sFilter = "RecordStatus <> '" + ((int)Classes.RecordStatuses.Delete).ToString() + "' and " +
                    "PictureType = " + ((int)Classes.cPicture.PictureTypes.Item).ToString();
                DataView dvPictures = new DataView(dtPictures, sFilter, "", DataViewRowState.CurrentRows);
                dlItems.DataSource = dvPictures;
                dlItems.DataBind();
            }
        }

        protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
        {
            if (oCharSelect.CharacterInfo != null)
            {
                if (oCharSelect.CharacterID.HasValue)
                {
                    Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
                    UserInfo.LastLoggedInCampaign = oCharSelect.CharacterInfo.CampaignID;
                    UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
                    UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
                    UserInfo.Save();
					Master.ChangeSelectedCampaign();
                }
            }
        }

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
		}
	}
}
