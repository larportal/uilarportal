using System;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LarpPortal.Classes
{
    /// <summary>
    /// Processing error at a server. This trys to connect directly to the database.
    /// </summary>
    public class LogWriter
    {
        /// <summary>
        /// Write the message to the log table.
        /// </summary>
        /// <param name="pvsMessage">The message to write out.</param>
        /// <param name="pvsLocation">The location of the message.</param>
        /// <param name="pvsAddInfo">Any additional information to add to the record.</param>
        /// <param name="pvsSessionID">Unique Identifier to be able to group records.</param>
        public void AddLogMessage(string pvsMessage, string pvsLocation, string pvsAddInfo, string pvsSessionID)
        {
            if ( ConfigurationManager.AppSettings["WriteLogMessages"] != null )
                if (ConfigurationManager.AppSettings["WriteLogMessages"].ToUpper().StartsWith("Y"))
                {
                    using (SqlConnection ConnErrors = new SqlConnection(ConfigurationManager.ConnectionStrings["Audit"].ConnectionString))
                    {
                        using (SqlCommand lcmdAddLogMessage = new SqlCommand("uspSystemLogIns", ConnErrors))
                        {
                            try
                            {
                                ConnErrors.Open();

                                SqlCommand lcmdAddErrorMessage = new SqlCommand("uspSystemLogIns", ConnErrors);
                                lcmdAddErrorMessage.CommandType = CommandType.StoredProcedure;
                                lcmdAddErrorMessage.Parameters.AddWithValue("@Location", pvsLocation);
                                lcmdAddErrorMessage.Parameters.AddWithValue("@Message", pvsMessage);
                                lcmdAddErrorMessage.Parameters.AddWithValue("@AddInfo", pvsAddInfo);
                                lcmdAddErrorMessage.Parameters.AddWithValue("@SessionID", pvsSessionID);

                                lcmdAddErrorMessage.ExecuteNonQuery();
                            }
                            catch //(Exception ex)
                            {
                                // Not much we can do so just leave it.....
                            }
                        }
                    }
                }
        }
    }
}
