using LarpPortal.Classes;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI;

namespace LarpPortal.Profile
{
    public partial class PlayerInventory : System.Web.UI.Page
    {
        //protected int _UserID = 0;
        //protected //string _UserName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //if (Session["Username"] != null)
            //    _UserName = Session["Username"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);

            if (!IsPostBack)
            {
                lblMessage.Text = "";
            }
            btnCloseItem.Attributes.Add("data-dismiss", "modal");
            btnRemovePicture.Attributes.Add("onClick", "ClearImage(); return false;");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            Classes.cPlayer PLPlayerInfo = new Classes.cPlayer(Master.UserID, Master.UserName);
            hidProfileID.Value = PLPlayerInfo.PlayerProfileID.ToString();

            DataTable InvItems = cUtilities.CreateDataTable(PLPlayerInfo.PlayerInventoryItems);

            if (InvItems.Columns["JavaScriptEdit"] == null)
                InvItems.Columns.Add(new DataColumn("JavaScriptEdit", typeof(string)));

            if (InvItems.Columns["JavaScriptDelete"] == null)
                InvItems.Columns.Add(new DataColumn("JavaScriptDelete", typeof(string)));

            if (InvItems.Columns["WillShareImage"] == null)
                InvItems.Columns.Add(new DataColumn("WillShareImage", typeof(string)));

            if (InvItems.Columns["DescWithImage"] == null)
                InvItems.Columns.Add(new DataColumn("DescWithImage", typeof(string)));

            foreach (DataRow dRow in InvItems.Rows)
            {
                string sImageURL = dRow["ImageURL"].ToString();
                if (sImageURL.StartsWith("~"))
                    sImageURL = sImageURL.Substring(1);

                dRow["JavaScriptEdit"] = string.Format("openItem({0}, \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\"); return false;",
                    dRow["PlayerInventoryID"].ToString(),
                    ReplaceQuotes(dRow["ItemName"]),
                    ReplaceQuotes(dRow["Description"]),
                    dRow["InventoryTypeID"].ToString(),
                    ReplaceQuotes(dRow["Quantity"]),
                    ReplaceQuotes(dRow["Size"]),
                    ReplaceQuotes(dRow["Location"]),
                    ReplaceQuotes(dRow["PowerNeeded"]),
                    ReplaceQuotes(dRow["WillShare"]),
                    ReplaceQuotes(dRow["PlayerComments"]),
                    ReplaceQuotes(sImageURL));

                dRow["JavaScriptDelete"] = string.Format("openItemDelete({0}, \"{1}\", \"{2}\", \"{3}\"); return false;",
                    dRow["PlayerInventoryID"].ToString(),
                    ReplaceQuotes(dRow["ItemName"]),
                    ReplaceQuotes(dRow["Description"]),
                    ReplaceQuotes(dRow["PlayerComments"]));


                bool bWillShare;
                if (bool.TryParse(dRow["WillShare"].ToString(), out bWillShare))
                    if (bWillShare)
                        dRow["WillShareImage"] = @"<span class=""glyphicon glyphicon-ok"" style=""width: 20px;""></span>";
                    else
                        dRow["WillShareImage"] = @"<span class=""glyphicon glyphicon-unchecked"" style=""width: 20px; text-align: center;""></span>";

                string sDescWithImage = @"<a data-toggle=""tooltip"" title=""<img src='";
                if (dRow["ImageURL"].ToString().StartsWith("~"))
                    sDescWithImage += dRow["ImageURL"].ToString().Substring(1);
                else
                    sDescWithImage += dRow["ImageURL"].ToString();

                sDescWithImage = sDescWithImage + @"' style='max-width: 400px;' />"">" +
                    @"<span class=""glyphicon glyphicon-picture""></span></a>";

                dRow["DescWithImage"] = sDescWithImage;
            }

            gvInventory.DataSource = InvItems;
            gvInventory.DataBind();

            if (!IsPostBack)
            {
                SortedList sParams = new SortedList();
                DataTable dtTypes = cUtilities.LoadDataTable("uspGetInventoryTypes", sParams, "LARPortal", Master.UserName, lsRoutineName);
                ddlType.DataTextField = "InventoryTypeDescription";
                ddlType.DataValueField = "InventoryTypeID";
                ddlType.DataSource = dtTypes;
                ddlType.DataBind();
            }
        }

        protected string ReplaceQuotes(object sOrig)
        {
            return sOrig.ToString().Replace("'", "\'").Replace("\"", "\\\"");
        }

        protected void btnDeleteItem_Click(object sender, EventArgs e)
        {
            int iDeleteInventoryID;

            if (int.TryParse(hidDeleteInventoryID.Value, out iDeleteInventoryID))
            {
                Classes.cPlayerInventory cInvItem = new cPlayerInventory(iDeleteInventoryID, Master.UserName, Master.UserID);
                cInvItem.RecordStatus = RecordStatuses.Delete;
                cInvItem.Delete(Master.UserName, Master.UserID);
            }
        }

        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            int iInventoryID;

            lblMessage.Text = "";
            try
            {
                if (int.TryParse(hidInventoryID.Value, out iInventoryID))
                {
                    int? iNewPictureID = null;
                    string sPictureURL = null;

                    if (fuItemPicture.HasFile)
                    {
                        System.Web.HttpPostedFile PostedFile = fuItemPicture.PostedFile;
                        //var postedFile = fuItemPicture.PostedFile;
                        int dataLength = PostedFile.ContentLength;
                        byte[] myData = new byte[dataLength];
                        PostedFile.InputStream.Read(myData, 0, dataLength);

                        Classes.cPicture NewPicture = new Classes.cPicture();
                        NewPicture.PictureType = Classes.cPicture.PictureTypes.Item;
                        string sExtension = Path.GetExtension(fuItemPicture.FileName);
                        NewPicture.CreateNewPictureRecord(cPicture.PictureTypes.Item, sExtension, Master.UserName);

                        int iTemp;
                        if (int.TryParse(hidProfileID.Value, out iTemp))
                            NewPicture.ProfileID = iTemp;

                        string LocalName = NewPicture.PictureLocalName;

                        if (!Directory.Exists(Path.GetDirectoryName(NewPicture.PictureLocalName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(NewPicture.PictureLocalName));

                        try
                        {
                            fuItemPicture.SaveAs(NewPicture.PictureLocalName);
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "While trying to save the file, got an error.<br>" + ex.Message + "<br>" +
                                ex.StackTrace + "<br>";
                            ErrorAtServer lobjError = new ErrorAtServer();
                            lobjError.ProcessError(ex, lsRoutineName + ".PicutreSaveAs", Master.UserName);
                        }
                        NewPicture.Save(Master.UserName);
                        sPictureURL = NewPicture.PictureURL;
                        iNewPictureID = NewPicture.PictureID;
                    }
                    else
                    {
                        // No file was sent
                    }

                    int iTypeID;
                    int.TryParse(ddlType.SelectedValue, out iTypeID);
                    Classes.cPlayerInventory cInvItem = new cPlayerInventory(iInventoryID, Master.UserName, Master.UserID);
                    cInvItem.ItemName = tbItemName.Text;
                    cInvItem.Description = tbDescription.Text;
                    cInvItem.InventoryTypeID = iTypeID;
                    cInvItem.Quantity = tbQuantity.Text;
                    cInvItem.Size = tbSize.Text;
                    cInvItem.Location = tbLocation.Text;
                    cInvItem.PowerNeeded = ddlPowerNeeded.SelectedValue;
                    cInvItem.WillShare = cbxWillShare.Checked;
                    cInvItem.PlayerComments = tbComments.Text;
                    if (iNewPictureID.HasValue)
                    {
                        cInvItem.ImageID = iNewPictureID.Value;
                        cInvItem.ImageURL = sPictureURL;
                    }

                    int iProfileID;
                    if (int.TryParse(hidProfileID.Value, out iProfileID))
                        cInvItem.PlayerProfileID = iProfileID;

                    cInvItem.RecordStatus = RecordStatuses.Active;
                    cInvItem.Save(Master.UserName, Master.UserID);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text += "There was a general error saving the file. The message was<br>" +
                    ex.Message + "<br>" +
                    ex.StackTrace + "<br>";
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, Master.UserName);
            }
            if (lblMessage.Text.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }
    }
}
