using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
	public class cUserLastLoggedIn
	{
		/// <summary>
		/// UserID of user for the values.
		/// </summary>
		public int UserID { get; set; }
		/// <summary>
		/// Last page the user was on.
		/// </summary>
		public string LastLoggedInLocation { get; set; }
		/// <summary>
		/// Last campaign the user was connected to.
		/// </summary>
		public int? LastLoggedInCampaign { get; set; }
		/// <summary>
		/// Last character the user was connected to.
		/// </summary>
		public int? LastLoggedInCharacter { get; set; }
		/// <summary>
		/// Whether the character was a user's character or campaign character 
		/// </summary>
		public char? LastLoggedInMyCharOrCamp { get; set; }

		/// <summary>
		/// Constructor that initializes all of the values.
		/// </summary>
		public cUserLastLoggedIn()
		{
			LastLoggedInLocation = string.Empty;
			LastLoggedInCampaign = null;
			LastLoggedInCharacter = null;
			LastLoggedInMyCharOrCamp = null;
		}

		/// <summary>
		/// Load the last logged in value for the user. You can either set the value comming in or set it in the class.
		/// </summary>
		/// <param name="pviUserID">User ID to load. (Optional - If not filled in will load using the class variable.)</param>
		public void Load(int? pviUserID = null)
		{
			if (pviUserID.HasValue)
				UserID = pviUserID.Value;

			using (SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
			{
				DataTable dtUserInfo = new DataTable();

				Conn.Open();
				using (SqlCommand Cmd = new SqlCommand("select * from MDBUsers where UserID = @UserID", Conn))
				{
					Cmd.Parameters.AddWithValue("@UserID", UserID);
					SqlDataAdapter SDACmd = new SqlDataAdapter(Cmd);
					SDACmd.Fill(dtUserInfo);

					foreach (DataRow dRow in dtUserInfo.Rows)
					{
						LastLoggedInLocation = dRow["LastLoggedInLocation"].ToString();
						LastLoggedInCampaign = null;
						LastLoggedInCharacter = null;
						LastLoggedInMyCharOrCamp = null;





					}
				}
			}
		}
	}
}