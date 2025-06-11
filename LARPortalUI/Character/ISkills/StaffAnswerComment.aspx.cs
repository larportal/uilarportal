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


namespace LarpPortal.Character.ISkills
{
    public partial class StaffAnswerComment : System.Web.UI.Page
    {
        public bool _Reload = false;
        public bool TextBoxEnabled = true;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            btnCancel.Attributes.Add("data-dismiss", "modal");
            btnNoDelete.Attributes.Add("data-dismiss", "modal");

            if (!IsPostBack)
            {
                //SortedList sParams = new SortedList();
                //sParams.Add("@StatusType", "IBSkillStaffComments");
                //sParams.Add("@CampaignID", Master.CampaignID);
                //DataTable dtStatus = Classes.cUtilities.LoadDataTable("uspGetStatusForCampaign", sParams, "LARPortal", Master.UserName, lsRoutineName);
                //DataView dvStatus = new DataView(dtStatus, "", "Comments", DataViewRowState.CurrentRows);
                //ddlRequestStatus.DataSource = dvStatus;
                //ddlRequestStatus.Items.Insert(0, "...Select Status");
                //ddlRequestStatus.Attributes.Add("onchange", "ChangeButton();");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((!IsPostBack) ||
                (_Reload))
            {
                tbResp.Attributes.Add("placeholder", "Add comment.");
                if (Request.QueryString["RequestSkillID"] != null)
                    hidRequestSkillID.Value = Request.QueryString["RequestSkillID"];
                else
                    Response.Redirect("StaffList.aspx", true);

                if (!IsPostBack)
                {
                    //  Every time somebody looks at this skill request, it will add a blank comment and will later be printed.
                    //  The stored procedure is smart enough that it will look at the last comment for the request and if it's the
                    //  same commenterID and the staffComments is blank, it won't add a duplicate record.
                    SortedList sViewParams = new SortedList();
                    sViewParams.Add("@RequestID", hidRequestSkillID.Value);
                    sViewParams.Add("@CommenterID", Master.UserID);
                    sViewParams.Add("@StaffComments", "");

                    Classes.cUtilities.PerformNonQuery("uspInsUpdIBSkillStaffComments", sViewParams, "LARPortal", Master.UserName);
                }

                SortedList sParams = new SortedList();
                sParams.Add("@IBSkillID", hidRequestSkillID.Value);

                DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspGetSubmittedIBSkills", sParams, "LARPortal", Master.UserName, lsRoutineName);
                DataTable dtComments = new DataTable();

                ulFile.Visible = true;
                lblFileName.Visible = false;
                hlFileName.Visible = false;
                btnDeleteAttach.Visible = false;
                hidFileName.Value = "";
                lblFileToDelete.Text = "";

                if (dtSkillInfo.Rows.Count > 0)
                {
                    DataRow drSkillInfo = dtSkillInfo.Rows[0];
                    lblSkillName.Text = drSkillInfo["SkillName"].ToString();
                    lblShortSkillDesc.Text = drSkillInfo["SkillShortDescription"].ToString();
                    lblLongSkillDesc.Text = drSkillInfo["SkillLongDescription"].ToString();
                    lblLongSkillDesc.Attributes.Add("style", "display: none;");
                    hidWhichDisplayed.Value = "S";
                    lblRequest.Text = drSkillInfo["RequestText"].ToString();
                    if (drSkillInfo["Payment"].ToString().Length > 0)
                    {
                        lblPayment.Text = drSkillInfo["Payment"].ToString();
                        pnlPayment.Visible = true;
                    }
                    else
                        pnlPayment.Visible = false;
                    if (drSkillInfo["PlayerComments"].ToString().Length > 0)
                    {
                        lblPlayerComments.Text = drSkillInfo["PlayerComments"].ToString();
                        pnlPlayerComments.Visible = true;
                    }
                    else
                        pnlPlayerComments.Visible = false;
                    if (drSkillInfo["CollaboratingNotes"].ToString().Length > 0)
                    {
                        lblCollaboratingNotes.Text = drSkillInfo["CollaboratingNotes"].ToString();
                        pnlCollaboratingNotes.Visible = true;
                    }
                    else
                        pnlCollaboratingNotes.Visible = false;

                    DateTime dtDate = new DateTime();
                    lblEventDate.Text = "";
                    DateTime dtSkillPurchaseDate = new DateTime();
                    DateTime dtPrevEventDate = new DateTime();

                    if (DateTime.TryParse(drSkillInfo["EventDate"].ToString(), out dtDate))
                        lblEventDate.Text = String.Format("{0:MM/dd/yyyy}", dtDate);
                    if (DateTime.TryParse(drSkillInfo["DateSkillPurchased"].ToString(), out dtSkillPurchaseDate))
                        lblSkillPurchaseDate.Text = String.Format("{0:MM/dd/yyyy}", dtSkillPurchaseDate);
                    if (DateTime.TryParse(drSkillInfo["PrevRegEventDate"].ToString(), out dtPrevEventDate))
                        lblLastEventDate.Text = String.Format("{0:MM/dd/yyyy}", dtPrevEventDate);
                    //if (dtPrevEventDate < dtSkillPurchaseDate)
                    //    divAlertMess.Visible = true;
                    lblCharName.Text = drSkillInfo["CharName"].ToString();
                    lblPlayerName.Text = drSkillInfo["PlayerName"].ToString();
                    tbResp.Text = drSkillInfo["StaffResponse"].ToString();
                    hidCampaignID.Value = drSkillInfo["CampaignID"].ToString();
                    bool bChecked = false;
                    if (bool.TryParse(drSkillInfo["DisplayStaffStatusToPlayer"].ToString(), out bChecked))
                        cbDisplayStatusToUser.Checked = bChecked;
                    if (bool.TryParse(drSkillInfo["DisplayResponseToPlayer"].ToString(), out bChecked))
                        cbDisplayResponseToUser.Checked = bChecked;

                    int iFileID = 0;
                    if (!string.IsNullOrEmpty(drSkillInfo["FileID"].ToString()))
                    {
                        if (int.TryParse(drSkillInfo["FileID"].ToString(), out iFileID))
                        {
                            ulFile.Visible = false;
                            lblFileName.Visible = false;
                            btnDeleteAttach.Visible = true;
                            hlFileName.Visible = true;
                            hlFileName.Text = drSkillInfo["AttachmentOrigFileName"].ToString();
                            hidFileName.Value = hlFileName.Text;
                            lblFileToDelete.Text = hlFileName.Text;
                            cPicture FileToLoad = new cPicture();
                            FileToLoad.Load(iFileID, Master.UserName);
                            hlFileName.NavigateUrl = FileToLoad.PictureURL;
                        }
                    }


                    int iStaffStatusID = -1;
                    int.TryParse(drSkillInfo["StaffStatusID"].ToString(), out iStaffStatusID);
                    string sCurrentStatus = drSkillInfo["StaffStatus"].ToString();
                    string sCurrentAssignedTo = drSkillInfo["AssignedToID"].ToString();

                    int iiWhenToDeliverID = -1;
                    int.TryParse(drSkillInfo["WhenToDeliverID"].ToString(), out iiWhenToDeliverID);

                    sParams = new SortedList();
                    sParams.Add("@IBSkillID", hidRequestSkillID.Value);
                    dtComments = Classes.cUtilities.LoadDataTable("uspGetIBSkillStaffComments", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetISkillComments");


                    if (dtComments.Columns["CommentHeader"] == null)
                        dtComments.Columns.Add("CommentHeader", typeof(string));
                    if (dtComments.Columns["ShowComment"] == null)
                        dtComments.Columns.Add("ShowComment", typeof(string));

                    foreach (DataRow dRow in dtComments.Rows)
                    {
                        DateTime dtAdded;
                        DateTime.TryParse(dRow["DateAdded"].ToString(), out dtAdded);
                        if (dRow["StaffComments"].ToString().Length == 0)
                        {
                            //  If it's a blank comment - then it's really a view.
                            dRow["CommentHeader"] = dRow["FirstName"].ToString() + " " + dRow["LastName"].ToString() + " viewed request at " + dtAdded.ToString();
                            dRow["ShowComment"] = "N";
                        }
                        else
                        {
                            dRow["CommentHeader"] = "<h4>Comment from " + dRow["FirstName"].ToString() + " " + dRow["LastName"].ToString() + " at " + dtAdded.ToString() + "</h4>";
                            dRow["ShowComment"] = "Y";
                        }
                    }
                    DataView dvComments = new DataView(dtComments, "", "DateAdded desc", DataViewRowState.CurrentRows);
                    rptQuestions.DataSource = dvComments;
                    rptQuestions.DataBind();

                    if (!IsPostBack)
                    {
                        SortedList sStatus = new SortedList();
                        sStatus.Add("@StatusType", "IBSkillStaffComments");
                        sStatus.Add("@CampaignID", hidCampaignID.Value);
                        DataTable dtRequestStatus = cUtilities.LoadDataTable("uspGetStatusForCampaign", sStatus, "LARPortal", Master.UserName, lsRoutineName + ".GetStatuses");
                        DataView dvRequestStatus = new DataView(dtRequestStatus, "", "Comments", DataViewRowState.CurrentRows);
                        ddlRequestStatus.DataSource = dvRequestStatus;
                        ddlRequestStatus.DataTextField = "StatusName";
                        ddlRequestStatus.DataValueField = "StatusID";
                        ddlRequestStatus.DataBind();

                        SortedList sWhenDeliver = new SortedList();
                        sWhenDeliver.Add("@StatusType", "IBSkillWhenToDeliver");
                        DataTable dtWhenDeliver = cUtilities.LoadDataTable("uspGetStatus", sWhenDeliver, "LARPortal", Master.UserName, lsRoutineName + ".GetWhenToDeliver");
                        DataView dvWhenDeliver = new DataView(dtWhenDeliver, "", "Comments", DataViewRowState.CurrentRows);

                        ddlWhenToDeliver.DataSource = dvWhenDeliver;
                        ddlWhenToDeliver.DataTextField = "StatusName";
                        ddlWhenToDeliver.DataValueField = "StatusID";
                        ddlWhenToDeliver.DataBind();

                        bool bItemFound = false;

                        ddlRequestStatus.ClearSelection();
                        if (sCurrentStatus.Length == 0)
                        {
                            ddlRequestStatus.Items.Insert(0, new ListItem("...Select Status...", "-1"));
                            ddlRequestStatus.SelectedIndex = 0;
                        }
                        else
                        {
                            bItemFound = false;
                            ddlRequestStatus.SelectedIndex = 0;
                            foreach (ListItem li in ddlRequestStatus.Items)
                            {
                                if (li.Value == iStaffStatusID.ToString())
                                {
                                    ddlRequestStatus.ClearSelection();
                                    li.Selected = true;
                                    bItemFound = true;
                                }
                            }
                            if (!bItemFound)
                            {
                                ddlRequestStatus.Items.Insert(0, new ListItem("...Select Status...", "-1"));
                                ddlRequestStatus.SelectedIndex = 0;
                            }
                        }
                        ddlRequestStatus.Attributes.Add("onchange", "ChangeButton();");

                        bItemFound = false;
                        ddlWhenToDeliver.ClearSelection();
                        if (iiWhenToDeliverID > 0)
                        {
                            foreach (ListItem li in ddlWhenToDeliver.Items)
                            {
                                if (li.Value == iiWhenToDeliverID.ToString())
                                {
                                    ddlWhenToDeliver.ClearSelection();
                                    li.Selected = true;
                                    bItemFound = true;
                                }
                            }
                        }

                        if (!bItemFound)
                        {
                            ddlWhenToDeliver.Items.Insert(0, new ListItem("...Select When To Deliver...", "-1"));
                            ddlWhenToDeliver.SelectedIndex = 0;
                        }

                        SortedList sAvailAssigned = new SortedList();
                        sAvailAssigned.Add("@CampaignID", hidCampaignID.Value);
                        sAvailAssigned.Add("@RoleID", Classes.cConstants.Roles.Campaign.PLOT_4.Replace("/", ""));
                        DataTable dtAssignedTo = cUtilities.LoadDataTable("uspGetPlayersForRole", sAvailAssigned, "LARPortal", Master.UserName, lsRoutineName + ".GetStatuses");
                        DataView dvAssignedTo = new DataView(dtAssignedTo, "", "PlayerNameLastFirst", DataViewRowState.CurrentRows);
                        ddlAssignedTo.DataSource = dvAssignedTo;
                        ddlAssignedTo.DataTextField = "PlayerName";
                        ddlAssignedTo.DataValueField = "UserID";
                        ddlAssignedTo.DataBind();

                        ddlAssignedTo.ClearSelection();
                        ddlAssignedTo.Items.Insert(0, new ListItem("Not Assigned", "-1"));

                        bItemFound = false;
                        ddlAssignedTo.SelectedIndex = 0;
                        foreach (ListItem li in ddlAssignedTo.Items)
                        {
                            if (li.Value == sCurrentAssignedTo.ToString())
                            {
                                ddlAssignedTo.ClearSelection();
                                li.Selected = true;
                                bItemFound = true;
                            }
                        }
                        if (!bItemFound)
                        {
                            ddlAssignedTo.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        //protected void ProcessButton(object sender, CommandEventArgs e)
        //{
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StaffList.aspx", true);
        }

        //protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SortedList sAddComment = new SortedList();
            sAddComment.Add("@RequestID", hidRequestSkillID.Value);
            sAddComment.Add("@CommenterID", Master.UserID);
            sAddComment.Add("@StaffComments", tbNewComment.Text);

            Classes.cUtilities.PerformNonQuery("uspInsUpdIBSkillStaffComments", sAddComment, "LARPortal", Master.UserName);

            _Reload = true;
        }

        protected void btnSaveRequest_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@IBSkillID", hidRequestSkillID.Value);

            DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspGetSubmittedIBSkills", sParams, "LARPortal", Master.UserName, lsRoutineName);
            if (dtSkillInfo.Rows.Count > 0)
            {
                DataRow dRow = dtSkillInfo.Rows[0];
                SortedList sUpdate = new SortedList();

                if (ulFile.HasFile)
                {
                    try
                    {
                        Classes.cPicture NewPicture = new Classes.cPicture();
                        NewPicture.PictureType = Classes.cPicture.PictureTypes.IBSkill;
                        NewPicture.CreateNewPictureRecord(Master.UserName);
                        string sExtension = Path.GetExtension(ulFile.FileName);
                        NewPicture.PictureFileName = "IB" + NewPicture.PictureID.ToString("D10") + sExtension;

                        //NewPicture.CharacterID = oCharSelect.CharacterID.Value;

                        string LocalName = NewPicture.PictureLocalName;

                        if (!Directory.Exists(Path.GetDirectoryName(NewPicture.PictureLocalName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(NewPicture.PictureLocalName));

                        ulFile.SaveAs(NewPicture.PictureLocalName);
                        NewPicture.Save(Master.UserName);

                        sUpdate.Add("@AttachmentOrigFileName", ulFile.FileName);
                        sUpdate.Add("@AttachmentLocalFileName", LocalName);
                        sUpdate.Add("@FileID", NewPicture.PictureID);
                    }
                    catch //(Exception ex)
                    {
                        //                    lblMessage.Text = ex.Message + "<br>" + ex.StackTrace;
                    }
                }

                sUpdate.Add("@IBSkillRequestID", hidRequestSkillID.Value);
                if ((dRow["StaffStatusID"].ToString() != ddlRequestStatus.SelectedValue) &&
                    (ddlRequestStatus.SelectedValue != "-1"))
                    sUpdate.Add("@StaffStatusID", ddlRequestStatus.SelectedValue);
                if (dRow["RequestText"].ToString() != tbResp.Text)
                    sUpdate.Add("@StaffResponse", tbResp.Text);
                sUpdate.Add("@DisplayStaffStatusToPlayer", cbDisplayStatusToUser.Checked);
                sUpdate.Add("@DisplayResponseToPlayer", cbDisplayResponseToUser.Checked);
                if (ddlWhenToDeliver.SelectedValue != "-1")
                    sUpdate.Add("@WhenToDeliverID", ddlWhenToDeliver.SelectedValue);
                sUpdate.Add("@AssignedToID", ddlAssignedTo.SelectedValue);

                cUtilities.PerformNonQuery("uspInsUpdIBSkillRequest", sUpdate, "LARPortal", Master.UserName);
                _Reload = true;

                lblmodalMessage.Text = "The request has been saved.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
            }
        }

        protected void btnDeleteAttach_Click(object sender, EventArgs e)
        {

        }

        protected void btnYesDelete_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@IBSkillRequestID", hidRequestSkillID.Value);
            sParams.Add("@DeleteAttachment", "Y");

            Classes.cUtilities.PerformNonQuery("uspInsUpdIBSkillRequestTable", sParams, "LARPortal", Master.UserName);
            _Reload = true;
        }

        protected void btnCancelModal_Click(object sender, EventArgs e)
        {

        }

        protected void MasterPage_CampaignChanged(object sender, EventArgs e)
        {
            Response.Redirect("/default.aspx");
        }
    }
}

