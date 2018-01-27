using System.Collections;
using System.Data;
using System.Reflection;

namespace LarpPortal.Classes
{
    /// <summary>
    /// Class to check what permissions the user has to a specific page.
    /// </summary>
    public class cURLPermission
    {
        public bool PagePermission { get; set; }
        public string DefaultUnauthorizedURL { get; set; }

        public cURLPermission()
        {
            PagePermission = false;
            DefaultUnauthorizedURL = "/index.aspx";
        }

        /// <summary>
        /// See if the person has permissions to get to the page. If they do not, figure out what page they should be redirected to.
        /// </summary>
        /// <param name="URL">URL the person is trying to go to.</param>
        /// <param name="UserName">Only needed for the call to the database to keep track of who is calling it.</param>
        /// <param name="RoleString">The users role string which will have the roles the person has.</param>
        /// <returns>True if the can get to it, false if they can't.</returns>
        public bool GetURLPermissions(string URL, string UserName, string RoleString)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            int iTemp = 0;
            string URLPermissions = "";
            PagePermission = false;
            int URLPermissionID = 0;
            SortedList sParams = new SortedList();
            sParams.Add("@URL", URL);
            DataTable dtURLPermissions = cUtilities.LoadDataTable("uspGetURLPermissionsUI", sParams, "LARPortal", UserName, lsRoutineName + ".uspGetURLPermissions");
            foreach (DataRow drow in dtURLPermissions.Rows)
            {
                if (int.TryParse(drow["RoleID"].ToString(), out iTemp))
                {
                    URLPermissionID = iTemp;
                    if(iTemp == -1)  // We'll have a -1 entry for pages that anyone can get to
                    {
                        PagePermission = true;
                    }
                }

                DefaultUnauthorizedURL = drow["DefaultUnauthorizedURL"].ToString();
                URLPermissions = "/" + URLPermissionID.ToString() + "/";
                if(RoleString.Contains(URLPermissions))
                    PagePermission = true;
            }
            return PagePermission;
        }
    }
}