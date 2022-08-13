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

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                SortedList sParams = new SortedList();
                sParams.Add("@StatusType", "IBSkillStaffComments");
                DataTable dtStatus = Classes.cUtilities.LoadDataTable("uspGetStatus", sParams, "LARPortal", Master.UserName, lsRoutineName);
                DataView dvStatus = new DataView(dtStatus, "", "Comments", DataViewRowState.CurrentRows);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((!IsPostBack) ||
                (_Reload))
            {
                CKResponse.Attributes.Add("placeholder", "Add comment.");
                if (Request.QueryString["SkillID"] != null)
                    hidRegistrationID.Value = Request.QueryString["SkillID"];
                else
                    Response.Redirect("StaffList.aspx", true);

                Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
                if (UserInfo.NickName.Length > 0)
                    hidAuthorName.Value = UserInfo.NickName + " " + UserInfo.LastName;
                else
                    hidAuthorName.Value = UserInfo.FirstName + " " + UserInfo.LastName;

                if (!IsPostBack)
                {
                    //  Every time somebody looks at this skill request, it will add a blank comment and will later be printed.
                    //  The stored procedure is smart enough that it will look at the last comment for the request and if it's the
                    //  same commenterID and the staffComments is blank, it won't add a duplicate record.
                    SortedList sViewParams = new SortedList();
                    sViewParams.Add("@RequestID", hidRegistrationID.Value);
                    sViewParams.Add("@CommenterID", Master.UserID);
                    sViewParams.Add("@StaffComments", "");

                    Classes.cUtilities.PerformNonQuery("uspInsUpdISkillStaffComments", sViewParams, "LARPortal", Master.UserName);
                }

                SortedList sParams = new SortedList();
                sParams.Add("@ISkillID", hidRegistrationID.Value);

                DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspGetSubmittedISkills", sParams, "LARPortal", Master.UserName, lsRoutineName);
                DataTable dtComments = new DataTable();

                if (dtSkillInfo.Rows.Count > 0)
                {
                    lblRequest.Text = dtSkillInfo.Rows[0]["RequestText"].ToString();
                    lblEventInfo.Text = dtSkillInfo.Rows[0]["EventName"].ToString();
                    lblCharName.Text = dtSkillInfo.Rows[0]["CharName"].ToString();
                    lblPlayerName.Text = dtSkillInfo.Rows[0]["PlayerName"].ToString();
                    CKResponse.Text = dtSkillInfo.Rows[0]["StaffResponse"].ToString();
                    if (dtSkillInfo.Rows[0]["AttachmentOrigFileName"].ToString().Length > 0)
                    {
                        ulFile.Visible = false;
                        lblFileName.Visible = true;
                        lblFileName.Text = dtSkillInfo.Rows[0]["AttachmentOrigFileName"].ToString();
                    }
                    else
                    {
                        ulFile.Visible = true;
                        lblFileName.Visible = false;
                    }

                    int iStaffStatusID = -1;
                    int.TryParse(dtSkillInfo.Rows[0]["StaffStatusID"].ToString(), out iStaffStatusID);
                    string sCurrentStatus = dtSkillInfo.Rows[0]["StaffStatus"].ToString();

                    sParams = new SortedList();
                    sParams.Add("@ISkillID", hidRegistrationID.Value);
                    dtComments = Classes.cUtilities.LoadDataTable("uspGetISkillStaffComments", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetISkillComments");

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
                        DataTable dtRequestStatus = cUtilities.LoadDataTable("uspGetStatus", sStatus, "LARPortal", Master.UserName, lsRoutineName + ".GetStatuses");
                        DataView dvRequestStatus = new DataView(dtRequestStatus, "", "Comments", DataViewRowState.CurrentRows);
                        ddlRequestStatus.DataSource = dvRequestStatus;
                        ddlRequestStatus.DataTextField = "StatusName";
                        ddlRequestStatus.DataValueField = "StatusID";
                        ddlRequestStatus.DataBind();

                        ddlRequestStatus.ClearSelection();
                        if (sCurrentStatus.Length == 0)
                        {
                            ddlRequestStatus.Items.Insert(0, new ListItem("...Select Status...", "-1"));
                            ddlRequestStatus.SelectedIndex = 0;
                        }
                        else
                        {
                            foreach (ListItem li in ddlRequestStatus.Items)
                            {
                                if (li.Value == iStaffStatusID.ToString())
                                {
                                    ddlRequestStatus.ClearSelection();
                                    li.Selected = true;
                                }
                            }
                            if (ddlRequestStatus.SelectedIndex < 0)
                            {
                                ddlRequestStatus.Items.Insert(0, new ListItem("...Select Status...", "<nothing>"));
                                ddlRequestStatus.SelectedIndex = 0;
                            }
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

        }

        //protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SortedList sAddComment = new SortedList();
            sAddComment.Add("@RequestID", hidRegistrationID.Value);
            sAddComment.Add("@CommenterID", Master.UserID);
            sAddComment.Add("@StaffComments", CKEditorComment.Text);

            Classes.cUtilities.PerformNonQuery("uspInsUpdISkillStaffComments", sAddComment, "LARPortal", Master.UserName);
            _Reload = true;
        }

        protected void btnSaveRequest_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@ISkillID", hidRegistrationID.Value);

            DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspGetSubmittedISkills", sParams, "LARPortal", Master.UserName, lsRoutineName);
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
                }
                    catch //(Exception ex)
                    {
                        //                    lblMessage.Text = ex.Message + "<br>" + ex.StackTrace;
                    }
                }

                sUpdate.Add("@ISkillRequestID", hidRegistrationID.Value);
                if (dRow["StaffStatusID"].ToString() != ddlRequestStatus.SelectedValue)
                    sUpdate.Add("@StaffStatusID", ddlRequestStatus.SelectedValue);
                if (dRow["RequestText"].ToString() != CKResponse.Text)
                    sUpdate.Add("@StaffResponse", CKResponse.Text);

                cUtilities.PerformNonQuery("uspInsUpdISkillRequestTable", sUpdate, "LARPortal", Master.UserName);
                _Reload = true;
            }
        }
    }
}













        //< div class= "row" >
 
        //     < div class= "col-xs-12" >
  
        //          < div class= "row" >
   
        //               < div class= "col-xs-10" >
    
        //                    < div class= "row" >
     
        //                         < div class= "xol-xs-10" >
      
        //                              < asp:Label ID = "lblEventInfo" runat="server" />
        //                    </div>
        //                    <div class= "col-xs-2" >
        //                        < div class= "row" style = "margin-top: 20px;" >
  
        //                              < div class= "col-xs-12" >
   
        //                                   < div class= "row" >
    
        //                                        < div class= "form-group" >
     
        //                                             < div class= "controls" >
      
        //                                                  < label for= "<%= lblRequest.ClientID %>" > Request </ label >
       
        //                                                   < asp:Label ID = "lblRequest" runat = "server" BorderWidth = "1" BorderColor = "Black" BorderStyle = "Solid" CssClass = "form-control col-xs-12" />
       
        //                                               </ div >
       
        //                                           </ div >
       
        //                                       </ div >
       
        //                                   </ div >
       
        //                               </ div >
       
        //                           </ div >
       
        //                       </ div >
       
        //                   </ div >
       
        //                   <% --                    < div class= "col-xs-2" >
         
        //                         < asp:Image ID = "imgPicture" runat="server" Width="100px" Height="100px" />
        //            </div>--%>
        //        </div>
        //    </div>





