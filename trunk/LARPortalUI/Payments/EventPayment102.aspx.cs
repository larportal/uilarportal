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
    public partial class EventPayment102 : System.Web.UI.Page
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
                    SortedList slParams = new SortedList();
                    slParams.Add("@RegistrationID", RegistrationID);
                    DataTable dtPaymentPageCode = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", strUserName, lsRoutineName);
                    if (dtPaymentPageCode.Rows.Count == 0)
                    {
                        Session["OnlinePayment"] = "N";
                        //lblHeader.Text = "There are no LARP Portal online payment options for this campaign.  Please check the campaign website.";
                        //lblHeader.Visible = true;
                        //lblRegistrationText.Text = "";
                        //HideOptions();
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
                            //lblPayPalForm.Text += "<input name=\"business\" type=\"hidden\" value=\"owner@fifthgatelarp.com\" />";
                            lblPayPalForm.Text += "<input name=\"item_name\" type=\"hidden\" value=\"" + lblPlayerEventCharacter.Text + "\" />";
                            //lblPayPalForm.Text += "<input name=\"bn\" type=\"hidden\" value=\"POWr_SP\" />";
                            //lblPayPalForm.Text += "<input name=\"charset\" type=\"hidden\" value=\"UTF-8\" />";
                            //lblPayPalForm.Text += "<input name=\"currency_code\" type=\"hidden\" value=\"USD\" />";
                            //lblPayPalForm.Text += "<input name=\"cmd\" type=\"hidden\" value=\"_xclick\" />";
                            //lblPayPalForm.Text += "<input name=\"amount\" type=\"hidden\" value=\"80.00\" />";
                            //lblPayPalForm.Text += "<input name=\"no_shipping\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"undefined_quantity\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"quantity\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"return\" type=\"hidden\" value=\"https://www.powr.io/plugins/\" />";
                            //lblPayPalForm.Text += "<input name=\"rm\" type=\"hidden\" value=\"1\" />";
                            //lblPayPalForm.Text += "<input name=\"notify_url\" type=\"hidden\" value=\"https://www.powr.io/payment_notification/5262016\" />";
                            lblPayPalForm.Text += "<div class=\"submitButton fitText\" style=\"font-size: 14px;\">Pay With PayPal</div>";

                            lblPayPalForm.Text += "</form>";
                            //tbPayPalFormCode.Text = lblPayPalForm.Text;
                            //<form class="paypalForm" action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_blank" novalidate="novalidate">

                            //    <input name="business" type="hidden" value="owner@fifthgatelarp.com" />
                            //    <input name="item_name" type="hidden" value="Fifth Gate Event" />
                            //    <input name="bn" type="hidden" value="POWr_SP" />
                            //    <input name="charset" type="hidden" value="UTF-8" />
                            //    <input name="currency_code" type="hidden" value="USD" />
                            //    <input name="cmd" type="hidden" value="_xclick" />
                            //    <input name="amount" type="hidden" value="80.00" />
                            //    <input name="no_shipping" type="hidden" value="1" />
                            //    <input name="undefined_quantity" type="hidden" value="1" />
                            //    <input name="quantity" type="hidden" value="1" />
                            //    <input name="return" type="hidden" value="https://www.powr.io/plugins/" />
                            //    <input name="rm" type="hidden" value="1" />
                            //    <input name="notify_url" type="hidden" value="https://www.powr.io/payment_notification/5262016" />
                            //    <div class="submitButton fitText" style="font-size: 14px;">Pay With PayPal</div>
                            //</form>

                        }
                        //GetPageCode(PageCodeID, CharacterAKA, EventName, CampaignName, RegistrationID, CampaignID, RoleAlignmentID);
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