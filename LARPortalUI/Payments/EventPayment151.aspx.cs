using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Security.Principal;
using System.Web.UI;
using LarpPortal.Classes;
using LarpPortal.Webservices;
using Org.BouncyCastle.Asn1.X509;
using static System.Collections.Specialized.BitVector32;

namespace LarpPortal.Payments
{
    public partial class EventPayment151 : System.Web.UI.Page
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
                    string Player = "";
                    string strUserName = Session["UserName"].ToString();
                    int PageCodeID = 0;
                    int CampaignID = 0;
                    int PreviousEvents = 0;
                    DateTime EarlyPaymentDate;
                    int PayPalTotal = 0;
                    string PPItemName = "";

                    SortedList slParams = new SortedList();
                    slParams.Add("@RegistrationID", RegistrationID);
                    DataTable dtPaymentPageCode = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", strUserName, lsRoutineName);
                    if (dtPaymentPageCode.Rows.Count == 0)
                    {
                        Session["OnlinePayment"] = "N";
                    }
                    else
                    {
                        foreach (DataRow dRow in dtPaymentPageCode.Rows)
                        {
                            int.TryParse(dRow["PaymentPageID"].ToString(), out PageCodeID);
                            int.TryParse(dRow["RoleAlignmentID"].ToString(), out RoleAlignmentID);
                            int.TryParse(dRow["CampaignID"].ToString(), out CampaignID);
                            int.TryParse(dRow["PreviousEvents"].ToString(), out PreviousEvents);

                            CharacterAKA = dRow["CharacterAKA"].ToString();
                            EventName = dRow["EventName"].ToString();
                            CampaignName = dRow["CampaignName"].ToString();
                            Player = dRow["Player"].ToString();
                            DateTime.TryParse(dRow["EarlyPayment"].ToString(), out EarlyPaymentDate);

                            switch (RoleAlignmentID)
                            {
                                case 2:
                                    CharacterAKA = "NPC";
                                    pnlNPCFood.Visible = true;
                                    break;
                                case 3:
                                    CharacterAKA = "Staff";
                                    pnlNPCFood.Visible = true;
                                    break;
                                default:
                                    CharacterAKA = "Character: " + CharacterAKA;
                                    pnlPCFood.Visible = true;
                                    break;
                            }


                            hidItemName.Value = EventName + " - " + CharacterAKA;
                            lblPlayerEventCharacter.Text = "Player: " + Player + " - " + hidItemName.Value;

                            //Original
                            lblPayPalForm.Text = "<form class=\"paypalForm\" action=\"https://www.paypal.com/cgi-bin/webscr\" method=\"post\" target=\"_blank\" novalidate=\"novalidate\">";
                            lblPayPalForm.Text += "<input name=\"business\" type=\"hidden\" value=\"eric@ctfaire.com\" />";
                            lblPayPalForm.Text += "<input type=\"hidden\" name=\"upload\" value=\"1\">";

                            DateTime CurrDT = DateTime.Now;
                            if (CurrDT > EarlyPaymentDate)
                            {
                                PayPalTotal = 125;
                            }
                            else
                            {
                                if (PreviousEvents > 0)
                                {
                                    PayPalTotal = 95;
                                }
                                else
                                {
                                    PayPalTotal = 75;
                                    //PayPalTotal = 95; // Take this out when we have new players identified. Fixed 3/30/2024
                                    //PayPalTotal = 1;  // Take this line out for production. 
                                }
                            }

                            PPItemName = lblPlayerEventCharacter.Text + "($" + PayPalTotal.ToString() + ")";

                            switch (rblPCFoodChoice.SelectedValue)
                            {
                                case "1":
                                    PPItemName += " / Vegan Meal Plan ($30)";
                                    PayPalTotal += 30;
                                    break;
                                case "2":
                                    PPItemName += " / Meat Meal Plan ($30)";
                                    PayPalTotal += 30;
                                    break;
                                default:
                                    break;
                            }
                            WriteRegistrationPayment(PPItemName, PayPalTotal, RegistrationID);
                            lblPayPalForm.Text += "<input name=\"item_name\" type=\"hidden\" value=\"" + PPItemName + "\" />";
                            lblPayPalForm.Text += "<input name=\"amount\" type=\"hidden\" value=\"" + PayPalTotal.ToString() + "\" />";
                            lblPayPalForm.Text += "<input name=\"quantity\" type=\"hidden\" value=\"1\" />";

                            lblPayPalForm.Text += "<input name=\"bn\" type=\"hidden\" value=\"POWr_SP\" />";
                            lblPayPalForm.Text += "<input name=\"charset\" type=\"hidden\" value=\"UTF-8\" />";
                            lblPayPalForm.Text += "<input name=\"on0\" type=\"hidden\" value=\"Event\" />";
                            lblPayPalForm.Text += "<input name=\"currency_code\" type=\"hidden\" value=\"USD\" />";
                            lblPayPalForm.Text += "<input name=\"cmd\" type=\"hidden\" value=\"_xclick\" />";


                            lblPayPalForm.Text += "<input name=\"no_shipping\" type=\"hidden\" value=\"1\" />";
                            lblPayPalForm.Text += "<div class=\"submitButton fitText\" style=\"font-size: 14px;\"></div>";
                            lblPayPalForm.Text += "</form>";
                            lblImageButton.Text = "<input type=\"image\" name=\"ImageButton1\" id=\"ImageButton1\"";
                            lblImageButton.Text += " src=\"https://www.paypalobjects.com/webstatic/en_AU/i/buttons/btn_paywith_primary_s.png\"";
                            lblImageButton.Text += " onclick=\"javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;ImageButton1&quot;,";
                            lblImageButton.Text += "  &quot;&quot;, false, &quot;&quot;, &quot;https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&amp&quot;, ";
                            lblImageButton.Text += "  false, false))\" /><span id=\"lblImageButton\"></span>";

                        }
                    }
                }
            }
        }

        protected void btnPayPalTotal_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClosePage();
        }

        protected void ClosePage()
        {
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }

        protected void btnSaveNPCMeal_Click(object sender, EventArgs e)
        {
            string MealChoice = rblNPCFoodChoice.SelectedValue;
            int RegID = 0;
            if (Session["RegistrationID"] != null)
                int.TryParse(Session["RegistrationID"].ToString(), out RegID);
            WriteMealChoice(MealChoice, RegID);
            ClosePage();
        }

        protected void btnSavePCMeal_Click(object sender, EventArgs e)
        {
            int MealTotal = 0;
            int Payment = 0;
            string MealChoice = rblPCFoodChoice.SelectedValue;
            int RegID = 0;
            if (Session["RegistrationID"] != null)
                int.TryParse(Session["RegistrationID"].ToString(), out RegID);
            switch (MealChoice)
            {
                case "0":
                    MealTotal = 0;
                    Payment = 0;
                    break;
                case "1":
                    MealTotal = 30;
                    Payment = 30;
                    break;
                case "2":
                    MealTotal = 30;
                    Payment = 30;
                    break;
                default:
                    MealTotal = 30;
                    Payment = 30;
                    break;
            }
            WriteMealChoice(MealChoice, RegID);
            btnSavePCMeal.Visible = false;
            rblPCFoodChoice.Enabled = false;
            pnlPay.Visible = true;

        }

        protected void WriteMealChoice(string MealChoice, int RegID)
        {
            // This needs cleanup when we finalize it
            // Myth 31 = Vegan CampaignMealDefaultID / 32 = Meat CampaignMealDefaultID // MealChoice "1"
            // Myth 43 = Event 2418 EventMealID / 44 = Event 2418  EventMealID // MealChoice "2"
            // CMRegistrationMeals stores RegistrationID and EventMealID
            int MealChoiceNumber;
            int.TryParse(MealChoice, out MealChoiceNumber);

            SortedList sParam = new SortedList();
            sParam.Add("@RegistrationID", RegID);
            sParam.Add("@RegistrationMealsID", -1); // New RegistrationMeals record
            sParam.Add("@EventMealID", MealChoiceNumber);    // We'll let the stored proc figure it out for Myth (for now)
            try
            {
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                Classes.cUtilities.PerformNonQuery("uspInsUpdCMRegistrationMeals", sParam, "LARPortal", "");
            }
            catch
            {

            }
            finally
            {

            }
        }

        protected void WriteRegistrationPayment(string ItemName, int PaymentTotal, int RegID)
        {
            // This needs cleanup when we finalize it
            SortedList sParam = new SortedList();
            sParam.Add("@RegistrationID", RegID);
            sParam.Add("@PaymentTotal", PaymentTotal);
            sParam.Add("@PaymentTypeID", 1);    // PayPal = 1 Need to unhardcode this later
            try
            {
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                Classes.cUtilities.PerformNonQuery("uspUpdateRegistrationPayment", sParam, "LARPortal", "");
            }
            catch
            {

            }
            finally
            {

            }
        }

        protected void btnCancelRegistration_Click(object sender, EventArgs e)
        {

            int RegID = 0;
            if (Session["RegistrationID"] != null)
                int.TryParse(Session["RegistrationID"].ToString(), out RegID);
            if (RegID > 0)
            {
                // and the registration
                DeleteRegistration(RegID);
                ClosePage();
            }

        }

        protected void DeleteRegistration(int RegID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@RegistrationID", RegID);
            try
            {
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                Classes.cUtilities.PerformNonQuery("uspCancelRegistration", sParam, "LARPortal", "");
            }
            catch
            {

            }
            finally
            {

            }
        }

    }
}