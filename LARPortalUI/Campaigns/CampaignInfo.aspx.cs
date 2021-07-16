using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns
{
    public partial class CampaignInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Classes.cUser User = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
            }
            else
            {
            }
            Classes.cCampaignBase CampaignBase = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
            lblCampaignName.Text = CampaignBase.CampaignName;
            DateTime? dtStartDate = CampaignBase.StartDate;
            lblStarted.Text = string.Format("{0:MMM d, yyyy}", dtStartDate);
            DateTime? dtExpEndDate = CampaignBase.ProjectedEndDate;
            lblExpectedEnd.Text = string.Format("{0:MMM d, yyyy}", dtExpEndDate);
            lblNumEvents.Text = CampaignBase.ActualNumberOfEvents.ToString();
            lblGameSystem.Text = CampaignBase.GameSystemName;
            lblGenres.Text = CampaignBase.GenreList;
            lblStyle.Text = CampaignBase.StyleDescription;
            //lblTechLevel.Text = CampaignBase.TechLevelName;
            lblTechLevel.Text = CampaignBase.TechLevelList;
            lblSize.Text = CampaignBase.CampaignSizeRange;
            lblAvgNumEvents.Text = CampaignBase.ProjectedNumberOfEvents.ToString();
            DataTable dtContacts = new DataTable();
            dtContacts.Columns.Add("Label", typeof(string));
            dtContacts.Columns.Add("URL", typeof(string));
            dtContacts.Columns.Add("URLLabel", typeof(string));
            dtContacts.Columns.Add("EMail", typeof(string));
            dtContacts.Columns.Add("EMailLabel", typeof(string));

            DataRow NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Campaign Info";
            if (!string.IsNullOrEmpty(CampaignBase.URL))
            {
                NewRow["URL"] = CampaignBase.URL;
                NewRow["URLLabel"] = "Campaign URL";
            }
            if ((CampaignBase.ShowCampaignInfoEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.InfoRequestEmail)))
            {
                NewRow["EMail"] = CampaignBase.InfoRequestEmail;
                NewRow["EMailLabel"] = "Information EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Character";
            if (!string.IsNullOrEmpty(CampaignBase.CharacterGeneratorURL))
            {
                NewRow["URL"] = CampaignBase.CharacterGeneratorURL;
                NewRow["URLLabel"] = "Character URL";
            }
            if ((CampaignBase.ShowCharacterNotificationEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.CharacterNotificationEMail)))
            {
                NewRow["EMail"] = CampaignBase.CharacterNotificationEMail;
                NewRow["EMailLabel"] = "CharacterEMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Character History";
            if (!string.IsNullOrEmpty(CampaignBase.CharacterHistoryURL))
            {
                NewRow["URL"] = CampaignBase.CharacterHistoryURL;
                NewRow["URLLabel"] = "Character History URL";
            }
            if ((CampaignBase.ShowCharacterHistoryEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.CharacterHistoryNotificationEmail)))
            {
                NewRow["EMail"] = CampaignBase.CharacterHistoryNotificationEmail;
                NewRow["EmailLabel"] = "Character History EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Info Skills";
            if (!string.IsNullOrEmpty(CampaignBase.InfoSkillURL))
            {
                NewRow["URL"] = CampaignBase.InfoSkillURL;
                NewRow["URLLabel"] = "Info Skills";
            }
            if ((CampaignBase.ShowCampaignInfoEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.InfoSkillEMail)))
            {
                NewRow["EMail"] = CampaignBase.InfoSkillEMail;
                NewRow["EMailLabel"] = "Info Skills EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Production Skills";
            if (!string.IsNullOrEmpty(CampaignBase.ProductionSkillURL))
            {
                NewRow["URL"] = CampaignBase.ProductionSkillURL;
                NewRow["URLLabel"] = "Production Skills URL";
            }
            if ((CampaignBase.ShowProductionSkillEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.ProductionSkillEMail)))
            {
                NewRow["EMail"] = CampaignBase.ProductionSkillEMail;
                NewRow["EMailLabel"] = "Production Skills EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "PEL";
            if (!string.IsNullOrEmpty(CampaignBase.PELSubmissionURL))
            {
                NewRow["URL"] = CampaignBase.PELSubmissionURL;
                NewRow["URLLabel"] = "PEL URL";
            }
            if ((CampaignBase.ShowPELNotificationEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.PELNotificationEMail)))
            {
                NewRow["EMail"] = CampaignBase.PELNotificationEMail;
                NewRow["EMailLabel"] = "PEL EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Points";
            NewRow["URL"] = "";
            NewRow["URLLabel"] = "";
            if ((CampaignBase.ShowCPNotificationEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.CPNotificationEmail)))
            {
                NewRow["EMail"] = CampaignBase.CPNotificationEmail;
                NewRow["EMailLabel"] = "Points EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Sign-up";
            if (!string.IsNullOrEmpty(CampaignBase.JoinURL))
            {
                NewRow["URL"] = CampaignBase.JoinURL;
                NewRow["URLLabel"] = "Sign-up URL";
            }
            if ((CampaignBase.ShowJoinRequestEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.JoinRequestEmail)))
            {
                NewRow["EMail"] = CampaignBase.JoinRequestEmail;
                NewRow["EMailLabel"] = "Sign-up EMail";
            }
            dtContacts.Rows.Add(NewRow);

            NewRow = dtContacts.NewRow();
            NewRow["Label"] = "Registration";
            if (!string.IsNullOrEmpty(CampaignBase.RegistrationURL))
            {
                NewRow["URL"] = CampaignBase.RegistrationURL;
                NewRow["URLLabel"] = "Registration URL";
            }
            if ((CampaignBase.ShowRegistrationNotificationEmail) &&
                (!string.IsNullOrEmpty(CampaignBase.RegistrationNotificationEmail)))
            {
                NewRow["EMail"] = CampaignBase.RegistrationNotificationEmail;
                NewRow["EMailLabel"] = "Registration EMail";
            }
            dtContacts.Rows.Add(NewRow);

            gvContactInfo.DataSource = dtContacts;
            gvContactInfo.DataBind();

            lblHeaderCampaignName.Text = " - " + Master.CampaignName;

            //string c1 = CampaignBase.URL;    //CampaignGMURL
            //string c2 = "";
            //string c4 = "";
            //string c6 = "";
            //string c8 = "";
            //string c10 = "";
            //string c12 = "";
            //string c14 = "";
            //string c16 = "";
            //string c18 = "";

            //if (CampaignBase.ShowCampaignInfoEmail == true)
            //{
            //    c2 = CampaignBase.InfoRequestEmail;    //CampgaignGMEmail
            //}
            //string c3 = CampaignBase.CharacterGeneratorURL;   //CharacterURL
            //if (CampaignBase.ShowCharacterNotificationEmail == true)
            //{
            //    c4 = CampaignBase.CharacterNotificationEMail;    //CharacterEmail
            //}
            //string c5 = CampaignBase.CharacterHistoryURL;   //CharHistoryURl
            //if (CampaignBase.ShowCharacterHistoryEmail == true)
            //{
            //    c6 = CampaignBase.CharacterHistoryNotificationEmail;   //CharHistoryEmail
            //}
            //string c7 = CampaignBase.InfoSkillURL;   //InfoSkillsURL
            //if (CampaignBase.ShowInfoSkillEmail == true)
            //{
            //    c8 = CampaignBase.InfoSkillEMail;   //InfoSkillsEmail
            //}
            //string c9 = CampaignBase.ProductionSkillURL; //ProdSkillsURL
            //if (CampaignBase.ShowProductionSkillEmail == true)
            //{
            //    c10 = CampaignBase.ProductionSkillEMail; //ProdSkillsEmail
            //}
            //string c11 = CampaignBase.PELSubmissionURL;  //PELURL
            //if (CampaignBase.ShowPELNotificationEmail == true)
            //{
            //    c12 = CampaignBase.PELNotificationEMail;    //PELEmail
            //}
            //string c13 = "";
            //if (CampaignBase.ShowCPNotificationEmail == true)
            //{
            //    c14 = CampaignBase.CPNotificationEmail;
            //}
            //string c15 = CampaignBase.JoinURL;
            //if (CampaignBase.ShowJoinRequestEmail == true)
            //{
            //    c16 = CampaignBase.JoinRequestEmail;
            //}
            //string c17 = CampaignBase.RegistrationURL;
            //if (CampaignBase.ShowRegistrationNotificationEmail == true)
            //{
            //    c18 = CampaignBase.RegistrationNotificationEmail;
            //}
            // Registration Notification Email            

            //BuildContacts(c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17, c18);
            CampaignDescription.Text = CampaignBase.WebPageDescription.Replace("\n", "<br>");
            lblMembershipFee.Text = CampaignBase.MembershipFee.ToString() + "&nbsp;" + CampaignBase.MembershipFeeFrequency;
            lblMinimumAge.Text = CampaignBase.MinimumAge.ToString();
            lblSupervisedAge.Text = CampaignBase.MinimumAgeWithSupervision.ToString();
            lblWaiver1.Text = "Waiver source TBD"; //TODO-Rick-2 Find the two (or more) waivers - may have to allow for even more
            lblWaiver2.Text = "Waiver source 2 TBD";
            lblConsent.Text = "Consent TBD"; //TODO-Rick-3 Find out where the consent is stored (what is consent anyway?)
        }

        //private void BuildContacts(string c1, string c2, string c3, string c4, string c5, string c6, string c7, string c8, string c9, string c10, string c11, string c12,
        //    string c13, string c14, string c15, string c16, string c17, string c18)
        //{
        //    // Load My Campaigns selection
        //    string hrefline = "";
        //    string CampaignInfoCheck = (c1 + c2);
        //    if (CampaignInfoCheck != "")
        //    {
        //        CampaignInfoCheck = "True";
        //    }
        //    string CharacterInfoCheck = (c3 + c4);
        //    if (CharacterInfoCheck != "")
        //    {
        //        CharacterInfoCheck = "True";
        //    }
        //    string CharacterHistoryCheck = (c5 + c6);
        //    if (CharacterHistoryCheck != "")
        //    {
        //        CharacterHistoryCheck = "True";
        //    }
        //    string InfoSkillCheck = (c7 + c8);
        //    if (InfoSkillCheck != "")
        //    {
        //        InfoSkillCheck = "True";
        //    }
        //    string ProductionSkillCheck = (c9 + c10);
        //    if (ProductionSkillCheck != "")
        //    {
        //        ProductionSkillCheck = "True";
        //    }
        //    string PELCheck = (c11 + c12);
        //    if (PELCheck != "")
        //    {
        //        PELCheck = "True";
        //    }
        //    string CPNotificationCheck = (c13 + c14);
        //    if (CPNotificationCheck != "")
        //    {
        //        CPNotificationCheck = "True";
        //    }
        //    string JoinCheck = (c15 + c16);
        //    if (JoinCheck != "")
        //    {
        //        JoinCheck = "True";
        //    }
        //    string RegistrationCheck = (c17 + c18);
        //    if (RegistrationCheck != "")
        //    {
        //        RegistrationCheck = "True";
        //    }
        //    // Constant values
        //    string DoubleQuote = "\"";
        //    int liLinesNeeded = 63;
        //    DataTable ContactsTable = new DataTable();
        //    ContactsTable.Columns.Add("href_li");
        //    for (int i = 0; i <= liLinesNeeded; i++)
        //    {
        //        //build on case of i
        //        switch (i)
        //        {
        //            case 0:
        //                hrefline = "<ul class=" + DoubleQuote + "col-sm-12" + DoubleQuote + ">";
        //                hrefline = "<table>";
        //                break;
        //            case 1:
        //                if (CampaignInfoCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 2:
        //                if (CampaignInfoCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 3:
        //                //c1 = CampaignGMURL
        //                //c2 = CampgaignGMEmail
        //                if (CampaignInfoCheck == "True")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + ">Campaign Info</li>";
        //                    hrefline = "<td>Campaign Info</td><td>&nbsp;&nbsp;</td>";
        //                }
        //                break;
        //            case 4:
        //                if (c1 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + c1 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Campaign URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c1 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Campaign URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 5:
        //                if (c2 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + "mailto:" + c2 + DoubleQuote + ">Information Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c2 + DoubleQuote + ">Information Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 6:
        //                if (CampaignInfoCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 7:
        //                if (CampaignInfoCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "</tr>";
        //                }
        //                break;
        //            case 8:  // New row
        //                if (CharacterInfoCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 9:
        //                if (CharacterInfoCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 10:
        //                //c3 = CharacterURL
        //                //c4 = CharacterEmail
        //                if (CharacterInfoCheck == "True")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + ">Character</li>";
        //                    hrefline = "<td>Character</td><td></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 11:
        //                if (c3 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + c3 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Character URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c3 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Character URL</a></td>";
        //                }
        //                break;
        //            case 12:
        //                if (c4 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + "mailto:" + c4 + DoubleQuote + ">Character Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c4 + DoubleQuote + ">Character Email</a></td>";
        //                }
        //                break;
        //            case 13:
        //                if (CharacterInfoCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "</tr>";
        //                }
        //                break;
        //            case 14:  // New row
        //                if (CharacterHistoryCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 15:
        //                //c5 = CharHistoryURl
        //                //c6 = CharHistoryEmail
        //                if (CharacterHistoryCheck == "True")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + ">Character History</li>";
        //                    hrefline = "<td>Character History</td><td></td>";
        //                }
        //                break;
        //            case 16:
        //                if (c5 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + c5 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Character History URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c5 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Character History URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 17:
        //                if (c6 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + "mailto:" + c6 + DoubleQuote + ">Character History Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c6 + DoubleQuote + ">Character History Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 18:
        //                if (CharacterHistoryCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 19:
        //                if (CharacterHistoryCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "</tr>";
        //                }
        //                break;
        //            case 20:  // New row
        //                if (InfoSkillCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 21:
        //                if (InfoSkillCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 22:
        //                //c7 = InfoSkillsURL
        //                //c8 = InfoSkillsEmail
        //                if (InfoSkillCheck == "True")
        //                {
        //                    hrefline = "<li>Info Skills</li>";
        //                    hrefline = "<td>Info Skills</td><td></td>";
        //                }
        //                break;
        //            case 23:
        //                if (c7 != "")
        //                {
        //                    hrefline = "<li><a href=" + DoubleQuote + c7 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Info Skills URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c7 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Info Skills URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 24:
        //                if (c8 != "")
        //                {
        //                    hrefline = "<li><a href=" + DoubleQuote + "mailto:" + c8 + DoubleQuote + ">Info Skills Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c8 + DoubleQuote + ">Info Skills Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 25:
        //                if (InfoSkillCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "</tr>";
        //                }
        //                break;
        //            case 26:  // New row
        //                if (ProductionSkillCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 27:
        //                if (ProductionSkillCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 28:
        //                if (ProductionSkillCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 29:
        //                //c9 = ProdSkillsURL
        //                //c10 = ProdSkillsEmail
        //                if (ProductionSkillCheck == "True")
        //                {
        //                    hrefline = "<li>Production Skills</li>";
        //                    hrefline = "<td>Production Skills</td><td></td>";
        //                }
        //                break;
        //            case 30:
        //                if (c9 != "")
        //                {
        //                    hrefline = "<li><a href=" + DoubleQuote + c9 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Production Skills URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c9 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Production Skills URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 31:
        //                if (c10 != "")
        //                {
        //                    hrefline = "<li><a href=" + DoubleQuote + "mailto:" + c10 + DoubleQuote + ">Production Skills Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c10 + DoubleQuote + ">Production Skills Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 32:
        //                if (ProductionSkillCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "</tr>";
        //                }
        //                break;
        //            case 33:  // New row
        //                if (PELCheck == "True")
        //                    hrefline = "</li>";
        //                hrefline = "</tr>";
        //                break;
        //            case 34:
        //                if (PELCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 35:
        //                if (PELCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 36:
        //                //c11 = PELURL
        //                //c12 = PELEmail
        //                if (PELCheck == "True")
        //                {
        //                    hrefline = "<li>PEL</li>";
        //                    hrefline = "<td>PEL</td><td></td>";
        //                }
        //                break;
        //            case 37:
        //                if (c11 != "")
        //                {
        //                    hrefline = "<li><a href=" + DoubleQuote + c11 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">PEL URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c11 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">PEL URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 38:
        //                if (c12 != "")
        //                {
        //                    hrefline = "<li><a href=" + DoubleQuote + "mailto:" + c12 + DoubleQuote + ">PEL Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c12 + DoubleQuote + ">PEL Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 39:
        //                if (PELCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "</tr>";
        //                }
        //                break;
        //            case 40:
        //                if (PELCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "";
        //                }
        //                break;

        //            //New line 1 CP Notification
        //            case 41:
        //                if (CPNotificationCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 42:
        //                if (CPNotificationCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 43:
        //                //c13 = CP URL
        //                //c14 = CP Email
        //                if (CPNotificationCheck == "True")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + ">Points</li>";
        //                    hrefline = "<td>Points</td><td>&nbsp;&nbsp;</td>";
        //                }
        //                break;
        //            case 44:
        //                if (c13 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + c13 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Points URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c13 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Points URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 45:
        //                if (c14 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + "mailto:" + c14 + DoubleQuote + ">Points Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c14 + DoubleQuote + ">Points Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 46:
        //                if (CPNotificationCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 47:
        //                if (CPNotificationCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "</tr>";
        //                }
        //                break;


        //            //New line 2 Join
        //            case 48:
        //                if (JoinCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 49:
        //                if (JoinCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 50:
        //                //c15 = Join URL
        //                //c16 = Join Email
        //                if (JoinCheck == "True")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + ">Sign-up</li>";
        //                    hrefline = "<td>Sign-up</td><td>&nbsp;&nbsp;</td>";
        //                }
        //                break;
        //            case 51:
        //                if (c15 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + c15 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Sign-up URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c15 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Sign-up URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 52:
        //                if (c6 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + "mailto:" + c16 + DoubleQuote + ">Sign-up Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c16 + DoubleQuote + ">Sign-up Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 53:
        //                if (JoinCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 54:
        //                if (JoinCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "</tr>";
        //                }
        //                break;


        //            //New line 3 Registration
        //            case 55:
        //                if (RegistrationCheck == "True")
        //                {
        //                    hrefline = "<li>";
        //                    hrefline = "<tr>";
        //                }
        //                break;
        //            case 56:
        //                if (RegistrationCheck == "True")
        //                {
        //                    hrefline = "<ul class=" + DoubleQuote + "list-inline" + DoubleQuote + ">";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 57:
        //                //c17 = Registration URL
        //                //c18 = Registration Email
        //                if (RegistrationCheck == "True")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + ">Registration</li>";
        //                    hrefline = "<td>Registration</td><td>&nbsp;&nbsp;</td>";
        //                }
        //                break;
        //            case 58:
        //                if (c17 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + c17 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Registration URL</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + c17 + DoubleQuote + " target=" + DoubleQuote + "_blank" + DoubleQuote + ">Registration URL</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 59:
        //                if (c18 != "")
        //                {
        //                    hrefline = "<li class=" + DoubleQuote + "col=sm-6" + DoubleQuote + "><a href=" + DoubleQuote + "mailto:" + c18 + DoubleQuote + ">Registration Email</a></li>";
        //                    hrefline = "<td><a href=" + DoubleQuote + "mailto:" + c18 + DoubleQuote + ">Registration Email</a></td>";
        //                }
        //                else
        //                {
        //                    hrefline = "<td></td>";
        //                }
        //                break;
        //            case 60:
        //                if (RegistrationCheck == "True")
        //                {
        //                    hrefline = "</ul>";
        //                    hrefline = "";
        //                }
        //                break;
        //            case 61:
        //                if (RegistrationCheck == "True")
        //                {
        //                    hrefline = "</li>";
        //                    hrefline = "</tr>";
        //                }
        //                break;

        //            case 62:  // End
        //                hrefline = "</ul>";
        //                hrefline = "</table>";
        //                break;
        //        }
        //        DataRow ContactsRow = ContactsTable.NewRow();
        //        ContactsRow["href_li"] = hrefline;
        //        ContactsTable.Rows.Add(ContactsRow);
        //        menu_ul_contacts.DataSource = ContactsTable;
        //        menu_ul_contacts.DataBind();
        //        hrefline = "";
        //    }
        //}

        protected void gvContactInfo_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ebebeb'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
            }
        }
    }
}