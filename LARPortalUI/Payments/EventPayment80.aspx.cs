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

namespace LarpPortal.Payments
{
    public partial class EventPayment80 : System.Web.UI.Page
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
                    DateTime EarlyPaymentDate;
                    bool EarlyPay = false;

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
                            CharacterAKA = dRow["CharacterAKA"].ToString();
                            EventName = dRow["EventName"].ToString();
                            CampaignName = dRow["CampaignName"].ToString();
                            Player = dRow["Player"].ToString();
                            DateTime.TryParse(dRow["EarlyPayment"].ToString(), out EarlyPaymentDate);
                            if(EarlyPaymentDate == null)
                            {
                                // Set EarlyPay = true
                                EarlyPay = true;
                            }
                            else
                            {
                                if (DateTime.Today > EarlyPaymentDate)
                                {
                                    EarlyPay = false;
                                }
                                else
                                {
                                    EarlyPay = true;
                                }
                            }

                            switch (RoleAlignmentID)
                            {
                                case 2:
                                    CharacterAKA = "NPC";
                                    break;
                                case 3:
                                    CharacterAKA = "Staff";
                                    break;
                                default:
                                    CharacterAKA = "Character: " + CharacterAKA;
                                    break;
                            }
                            hidItemName.Value = EventName + " - " + CharacterAKA;
                            lblPlayerEventCharacter.Text = "Player: " + Player + " - " + hidItemName.Value;

                            lblPayPalForm.Text = "<form class=\"paypalForm\" action=\"https://www.paypal.com/cgi-bin/webscr\" method=\"post\" target=\"_blank\" novalidate=\"novalidate\">";
                            lblPayPalForm.Text += "<input name=\"business\" type=\"hidden\" value=\"sebastian.sarr@gmail.com\" />";
                            lblPayPalForm.Text += "<input name=\"item_name\" type=\"hidden\" value=\"" + lblPlayerEventCharacter.Text + "\" />";
                            lblPayPalForm.Text += "<input name=\"bn\" type=\"hidden\" value=\"POWr_SP\" />";
                            lblPayPalForm.Text += "<input name=\"charset\" type=\"hidden\" value=\"UTF-8\" />";
                            lblPayPalForm.Text += "<input name=\"on0\" type=\"hidden\" value=\"Event\" />";
                            lblPayPalForm.Text += "<input name=\"currency_code\" type=\"hidden\" value=\"USD\" />";
                            lblPayPalForm.Text += "<input name=\"cmd\" type=\"hidden\" value=\"_xclick\" />";
                            if (EarlyPay == true)
                            {
                                lblPayPalForm.Text += "<input name=\"amount\" type=\"hidden\" value=\"80.00\" />";
                                lblEarlyOrLate.Text = "Crossover Event (Paid in advance)";
                            }
                            else
                            {
                                lblPayPalForm.Text += "<input name=\"amount\" type=\"hidden\" value=\"85.00\" />";
                                lblEarlyOrLate.Text = "Crossover Event (Past 3 Week Deadline)";
                            }
                            lblPayPalForm.Text += "<input name=\"no_shipping\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"undefined_quantity\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"quantity\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"return\" type=\"hidden\" value=\"https://www.powr.io/plugins/\" />";
                            //lblPayPalForm.Text += "<input name=\"rm\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"notify_url\" type=\"hidden\" value=\"https://www.powr.io/payment_notification/5262016\" />";
                            //lblPayPalForm.Text += "<div class=\"submitButton fitText\" style=\"font-size: 14px;\">Pay With PayPal</div>";
                            lblPayPalForm.Text += "<div class=\"submitButton fitText\" style=\"font-size: 14px;\"></div>";
                            lblPayPalForm.Text += "</form>";
                            //tbPayPalFormCode.Text = lblPayPalForm.Text;
                            lblImageButton.Text = "<input type=\"image\" name=\"ImageButton1\" id=\"ImageButton1\"";
                            lblImageButton.Text += " src=\"https://www.paypalobjects.com/webstatic/en_AU/i/buttons/btn_paywith_primary_s.png\"";
                            lblImageButton.Text += " onclick=\"javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;ImageButton1&quot;,";
                            //lblImageButton.Text += "  &quot;&quot;, false, &quot;&quot;, &quot;https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&amp;hosted_button_id=PYM9SG4S63CX2&quot;, ";
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

    }
}