using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LarpPortal.Classes;
using System.Net;
using System.Net.Mail;

namespace LarpPortal.Events
{
    public partial class EventPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Using session variable RegistrationID look up campaign, event, character
            int RegistrationID = 0;
            Session["OnlinePayment"] = "Y";
            if (Session["RegistrationID"] == null)
            {
                Session["RegistrationID"] = 0;  // 8529 Rick Madrigal registration for testing.  Remove this line when being called
            }
            int.TryParse(Session["RegistrationID"].ToString(), out RegistrationID);
            if (RegistrationID == 0)
            {
                ClosePage();
            }
            else
            {
                CalculateOrder();
                string strUserName = Session["UserName"].ToString();
                lblRegistrationText.Text = "<p>Pay for Madrigal with PayPal!</p><br /><br />";
                //lblRegistrationText.Text += "<p>As of 2011, Madrigal has changed our payment system so that there ";
                //lblRegistrationText.Text += "is no longer any Membership fee - insurance and overhead fees have ";
                //lblRegistrationText.Text += "been rolled into the event cost. This way, NPCs play for free!</p>";
                //lblRegistrationText.Text += "<p>This means that Adventure Weekends now cost $80, no matter when you register:</p><br />";
                lblFoodText.Text = "<br />At Camp Woodstock meal services are available directly through the site.<br>";
                lblFoodText.Text += "Go to  www.campwoodstock.org.<br>";
                lblFoodText.Text += "Click big orange REGISTER FOR CAMP button at the bottom.<br>";
                lblFoodText.Text += "First time fill out new user sign-in. Subsequent times just login.<br>";
                lblFoodText.Text += "Click Start application button under Weekend Group: Chimera<br>";
                lblFoodText.Text += "Click Continue button<br>";
                lblFoodText.Text += "If you are handling registration for multiple people, create each person under your account. It will take 2 adults and as many children as you want. Just list additional people as children.<br>";
                lblFoodText.Text += "When you’re done press Continue button.<br>";
                lblFoodText.Text += "Under each person’s tab select the meals they’re getting by clicking show details link.<br>";
                lblFoodText.Text += "When you’re done press Continue button.<br>";
                lblFoodText.Text += "Fill in forms for allergies if any (note: email additional allergies information is on the page)<br>";
                lblFoodText.Text += "Fill in your name and contact info (the person paying) and click Complete this Form button.<br>";
                lblFoodText.Text += "Choose your payment option (MasterCard, Visa, Discover or American Express).<br>";
                lblFoodText.Text += "Click Continue button.<br>";
                lblFoodText.Text += "Fill in credit card info.<br>";
                lblFoodText.Text += "Click Submit Application button.";
                //lblFoodText.Text = "<p>At the current campsite, meal services are available during Adventure Weekends, and may be paid for ahead of time or at check-in - ";
                //lblFoodText.Text += " please note that the camp staff cannot accept payment for meals during meal times:</p>";
                if (RegistrationID == 0)
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.open('close.html', '_self', null);", true);
                }
                else
                {
                    string lsRoutineName = "EventPayment.PageLoad";
                    string stStoredProc = "uspGetPaymentPageCode";
                    int RoleAlignmentID = 0;
                    string CharacterAKA = "";
                    string EventName = "";
                    string CampaignName = "";
                    int PageCodeID = 0;
                    int CampaignID = 0;
                    SortedList slParams = new SortedList();
                    slParams.Add("@RegistrationID", RegistrationID);
                    DataTable dtPaymentPageCode = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", strUserName, lsRoutineName);
                    if (dtPaymentPageCode.Rows.Count == 0)
                    {
                        Session["OnlinePayment"] = "N";
                        lblHeader.Text = "There are no LARP Portal online payment options for this campaign.  Please check the campaign website.";
                        lblHeader.Visible = true;
                        lblRegistrationText.Text = "";
                        HideOptions();
                    }
                    else
                    {
                        foreach (DataRow dRow in dtPaymentPageCode.Rows)
                        {
                            int.TryParse(dRow["PaymentPageID"].ToString(), out PageCodeID);
                            int.TryParse(dRow["RoleAlignmentID"].ToString(), out RoleAlignmentID);
                            int.TryParse(dRow["CampaignID"].ToString(), out CampaignID);
                            CharacterAKA = dRow["CharacterAKA"].ToString();
                            EventName = dRow["EventName"].ToString();
                            CampaignName = dRow["CampaignName"].ToString();

                            switch (RoleAlignmentID)
                            {
                                case 2:
                                    CharacterAKA = "NPC";
                                    break;
                                case 3:
                                    CharacterAKA = "Staff";
                                    break;
                                default:
                                    CharacterAKA = "Character-" + CharacterAKA;
                                    break;
                            }
                            hidItemName.Value = EventName + " - " + CharacterAKA;
                        }
                        GetPageCode(PageCodeID, CharacterAKA, EventName, CampaignName, RegistrationID, CampaignID, RoleAlignmentID);
                    }
                }
            }
        }

        protected void GetPageCode(int PaymentPageID, string CharacterAKA, string EventName, string CampaignName, int RegistrationID, int CampaignID, int RoleAlignmentID)
        {
            string lsRoutineName = "EventPayment.PageLoad.PaymentPageCode";
            string stStoredProc = "uspGetPaymentPageTypeCode";
            string strUserName = Session["UserName"].ToString();
            SortedList slParams = new SortedList();
            slParams.Add("@PaymentPageID", PaymentPageID);
            slParams.Add("@CharacterName", CharacterAKA);
            slParams.Add("@EventName", EventName);
            slParams.Add("@CampaignName", CampaignName);
            DataTable dtPaymentPageCode = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", strUserName, lsRoutineName);
            if (dtPaymentPageCode.Rows.Count == 0)
            {
                RedirectToCampaignSpecificURL(CampaignID, CharacterAKA, EventName, CampaignName, RoleAlignmentID);
            }
            else
            {
                string DescriptionKey;
                string PageCode;
                string ButtonName;
                bool ButtonVisible = false;
                foreach (DataRow dRow in dtPaymentPageCode.Rows)
                {
                    DescriptionKey = dRow["DescriptionKey"].ToString();
                    PageCode = dRow["PageCode"].ToString();
                    ButtonName = dRow["ButtonName"].ToString();
                    bool.TryParse(dRow["ButtonVisible"].ToString(), out ButtonVisible);
                    switch (DescriptionKey)
                    {
                        case "Header":
                            lblHeader.Text = PageCode;
                            lblHeader.Visible = true;
                            break;
                        case "Footer":
                            lblFooter.Text = PageCode;
                            lblFooter.Visible = true;
                            break;

                        default:

                            break;
                    }
                }
            }
        }

        protected void RedirectToCampaignSpecificURL(int CampaignID, string CharacterAKA, string EventName, string CampaignName, int RoleAlignmentID)
        {
            string lsRoutineName = "EventPayment.RedirectToCampaignSpecificURL";
            string stStoredProc = "uspGetPaymentPageRedirectURL";
            string strUserName = Session["UserName"].ToString();
            string PaymentPageURL = "";
            SortedList slParams = new SortedList();
            slParams.Add("@CampaignID", CampaignID);
            DataTable dtPaymentPageCode = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", strUserName, lsRoutineName);
            if (dtPaymentPageCode.Rows.Count != 0)
            {
                foreach (DataRow dRow in dtPaymentPageCode.Rows)
                {
                    Session["CharacterAKA"] = CharacterAKA;
                    Session["EventName"] = EventName;
                    Session["RoleAlignmentID"] = RoleAlignmentID;
                    PaymentPageURL = dRow["PaymentPageURL"].ToString();
                    Response.Redirect(PaymentPageURL);
                }
            }
            else
            {
                Session["OnlinePayment"] = "N";
                lblHeader.Text = "There are no LARP Portal online payment options for this campaign.  Please check the campaign website.";
                lblHeader.Visible = true;
                lblRegistrationText.Text = "";
                HideOptions();
            }
        }

        protected void HideOptions()
        {
            if (Session["OnlinePayment"].ToString() == "N")
            {
                lblFoodText.Visible = false;
                lblOrderTotalDisplay.Visible = false;
                lblOrderTotalSection.Visible = false;
                lblRegistrationText.Visible = false;
                btnCalculateOrder.Visible = false;
                btnPayPalTotal.Visible = false;
                lblPaymentNote.Visible = false;
                chkAllMeals.Visible = false;
                chkRegistration.Visible = false;
                chkSaturdayBrunch.Visible = false;
                chkSaturdayDinner.Visible = false;
                chkSundayBrunch.Visible = false;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClosePage();
        }

        protected void ClosePage()
        {
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }

        protected void btnCalculateOrder_Click(object sender, EventArgs e)
        {
            CalculateOrder();
        }

        protected void CalculateOrder()
        {
            if (chkRegistration.Checked == true || chkSaturdayBrunch.Checked == true || chkSaturdayDinner.Checked == true || chkSundayBrunch.Checked == true || chkAllMeals.Checked == true)
            {
                double OrderTotal = 0;
                string TotalSectionHTML;
                int CartCounter = 0;
                string strCartCounter = CartCounter.ToString();
                TotalSectionHTML = "<form></form><form action=\"http://www.paypal.com/cgi-bin/webscr\" method=\"post\">";
                TotalSectionHTML += "<input type=\"hidden\" name=\"cmd\" value=\"_cart\">";
                TotalSectionHTML += "<input type=\"hidden\" name=\"upload\" value=\"1\">";
                TotalSectionHTML += "<input type=\"hidden\" name=\"business\" value=\"rciccolini@gmail.com\">";
                TotalSectionHTML += "<input type=\"hidden\" name=\"currency_code\" value=\"USD\">";
                if (chkRegistration.Checked == true)
                {
                    CartCounter = CartCounter + 1;
                    strCartCounter = CartCounter.ToString();
                    TotalSectionHTML += "<input type=\"hidden\" name=\"item_name_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"" + hidItemName.Value + "\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"number_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"1\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"amount_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"80\">";
                    OrderTotal += 80;
                }
                if (chkSaturdayBrunch.Checked == true)
                {
                    CartCounter = CartCounter + 1;
                    strCartCounter = CartCounter.ToString();
                    TotalSectionHTML += "<input type=\"hidden\" name=\"item_name_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"Saturday Brunch\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"number_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"21\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"amount_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"9\">";
                    OrderTotal += 9;
                }
                if (chkSaturdayDinner.Checked == true)
                {
                    CartCounter = CartCounter + 1;
                    strCartCounter = CartCounter.ToString();
                    TotalSectionHTML += "<input type=\"hidden\" name=\"item_name_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"Saturday Dinner\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"number_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"22\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"amount_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"9\">";
                    OrderTotal += 9;
                }
                if (chkSundayBrunch.Checked == true)
                {
                    CartCounter = CartCounter + 1;
                    strCartCounter = CartCounter.ToString();
                    TotalSectionHTML += "<input type=\"hidden\" name=\"item_name_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"Sunday Brunch\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"number_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"23\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"amount_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"9\">";
                    OrderTotal += 9;
                }
                if (chkAllMeals.Checked == true)
                {
                    CartCounter = CartCounter + 1;
                    strCartCounter = CartCounter.ToString();
                    TotalSectionHTML += "<input type=\"hidden\" name=\"item_name_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"All three meals\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"number_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"25\">";
                    TotalSectionHTML += "<input type=\"hidden\" name=\"amount_";
                    TotalSectionHTML += strCartCounter;
                    TotalSectionHTML += "\" value=\"27\">";
                    OrderTotal += 27;
                }
                lblOrderTotalSection.Text = TotalSectionHTML;
                lblOrderTotalSection.Visible = true;
                lblOrderTotalDisplay.Text = "Your total is $" + OrderTotal.ToString();
                lblOrderTotalDisplay.Visible = true;
                btnPayPalTotal.Visible = true;
                lblPaymentNote.Visible = true;
            }
            else
            {
                string jsString = "alert('You need to select at least one option.');";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        "MyApplication",
                        jsString,
                        true);
            }
        }

        protected void btnPayPalTotal_Click(object sender, ImageClickEventArgs e)
        {

        }

    }
}