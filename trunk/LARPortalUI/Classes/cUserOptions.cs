using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Xml.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace LarpPortal.Classes
{
	public class cUserOption
	{
		public int? RowID { get; set; }
		public string LoginUsername { get; set; }
		public string PageName { get; set; }
		public string ObjectName { get; set; }
		public string ObjectOption { get; set; }
		public string OptionValue { get; set; }
		public string Comments { get; set; }

		public cUserOption()
		{
			RowID = null;
			LoginUsername = "";
			PageName = "";
			ObjectName = "";
			ObjectOption = "";
			OptionValue = "";
			Comments = "";
		}

		public void LoadOptionValue(string sLoginUsername, string sPageName, string sObjectName, string sObjectOption)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@LoginUsername", sLoginUsername);
			sParams.Add("@PageName", sPageName);
			sParams.Add("@ObjectName", sObjectName);
			sParams.Add("@ObjectOption", sObjectOption);
			OptionValue = "";
			Comments = "";
			RowID = null;

			LoginUsername = sLoginUsername;
			PageName = sPageName;
			ObjectName = sObjectName;
			ObjectOption = sObjectOption;

			DataTable dtOptions = Classes.cUtilities.LoadDataTable("uspGetMDBUserOptions", sParams, "LARPortal", LoginUsername, lsRoutineName);

			foreach (DataRow dRow in dtOptions.Rows)
			{
				int iRowID;
				if (int.TryParse(dRow["RowID"].ToString(), out iRowID))
					RowID = iRowID;
				OptionValue = dRow["OptionValue"].ToString();
				Comments = dRow["Comments"].ToString();
			}
		}

		public void SaveOptionValue(string sLoginUsername, string sPageName, string sObjectName, string sObjectOption, string sOptionValue, string sComments)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@LoginUsername", sLoginUsername);
			sParams.Add("@PageName", sPageName);
			sParams.Add("@ObjectName", sObjectName);
			sParams.Add("@ObjectOption", sObjectOption);
			sParams.Add("@OptionValue", sOptionValue);
			sParams.Add("@Comments", sComments);

			LoginUsername = sLoginUsername;
			PageName = sPageName;
			ObjectName = sObjectName;
			ObjectOption = sObjectOption;

			DataTable dtOptions = Classes.cUtilities.LoadDataTable("uspInsUpdMDBUserOptions", sParams, "LARPortal", LoginUsername, lsRoutineName);

			foreach (DataRow dRow in dtOptions.Rows)
			{
				int iRowID;
				if (int.TryParse(dRow["RowID"].ToString(), out iRowID))
					RowID = iRowID;
				LoginUsername = dRow["LoginUsername"].ToString();
				PageName = dRow["PageName"].ToString();
				ObjectName = dRow["ObjectName"].ToString();
				ObjectOption = dRow["ObjectOtion"].ToString();
				OptionValue = dRow["OptionValue"].ToString();
				Comments = dRow["Comments"].ToString();
			}
		}

		public void SaveOptionValue()
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@LoginUsername", LoginUsername);
			sParams.Add("@PageName", PageName);
			sParams.Add("@ObjectName", ObjectName);
			sParams.Add("@ObjectOption", ObjectOption);
			sParams.Add("@OptionValue", OptionValue);
			sParams.Add("@Comments", Comments);

			DataTable dtOptions = Classes.cUtilities.LoadDataTable("uspInsUpdMDBUserOptions", sParams, "LARPortal", LoginUsername, lsRoutineName);

			foreach (DataRow dRow in dtOptions.Rows)
			{
				int iRowID;
				if (int.TryParse(dRow["RowID"].ToString(), out iRowID))
					RowID = iRowID;
				LoginUsername = dRow["LoginUsername"].ToString();
				PageName = dRow["PageName"].ToString();
				ObjectName = dRow["ObjectName"].ToString();
				ObjectOption = dRow["ObjectOption"].ToString();
				OptionValue = dRow["OptionValue"].ToString();
				Comments = dRow["Comments"].ToString();
			}
		}
	}




	public class cUserOptions
	{
		public List<cUserOption> UserOptionList { get; set; }

		public cUserOptions()
		{
			UserOptionList = new List<cUserOption>();
		}

		public void LoadUserOptions(string sLoginUsername, string sPageName, string sObjectName, string sObjectOption)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@LoginUsername", sLoginUsername);
			sParams.Add("@PageName", sPageName);
			if (!string.IsNullOrEmpty(sObjectName))
			{
				sParams.Add("@ObjectName", sObjectName);
				if (!string.IsNullOrEmpty(sObjectOption))
					sParams.Add("@ObjectOption", sObjectOption);
			}

			DataTable dtOptions = Classes.cUtilities.LoadDataTable("uspGetMDBUserOptions", sParams, "LARPortal", sLoginUsername, lsRoutineName);

			UserOptionList = new List<cUserOption>();

			foreach (DataRow dRow in dtOptions.Rows)
			{
				cUserOption SingleOption = new cUserOption();

				int iRowID;
				if (int.TryParse(dRow["RowID"].ToString(), out iRowID))
					SingleOption.RowID = iRowID;
				SingleOption.LoginUsername = dRow["LoginUsername"].ToString();
				SingleOption.PageName = dRow["PageName"].ToString();
				SingleOption.ObjectName = dRow["ObjectName"].ToString();
				SingleOption.ObjectOption = dRow["ObjectOption"].ToString();
				SingleOption.OptionValue = dRow["OptionValue"].ToString();
				SingleOption.Comments = dRow["Comments"].ToString();

				UserOptionList.Add(SingleOption);
			}
		}
	}
}