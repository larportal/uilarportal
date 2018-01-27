using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LarpPortal.Classes
{
    /// <summary>
    /// Struct to store the error message if it can't connect to the database.
    /// </summary>
    public struct ErrorMessages
    {
        /// <summary>
        /// Name of the machine that the error happened on.
        /// </summary>
        public string MachineErrorHappenedOn;
        /// <summary>
        /// Where the error happened.
        /// </summary>
        public string ErrorLocation;
        /// <summary>
        /// The actual text of the error message.
        /// </summary>
        public string ErrorMessage;
        /// <summary>
        /// The type of error (SQL, General, ...)
        /// </summary>
        public string ErrorType;
        /// <summary>
        /// When the error happened.
        /// </summary>
        public DateTime WhenErrorHappened;
    }

    /// <summary>
    /// Class for misc. error routines.
    /// </summary>
    public class ErrorRoutines
    {
        /// <summary>
        /// Formats an Exception to make it printable (web printable)
        /// </summary>
        /// <param name="pvsComputerName">Name of computer it happened on.</param>
        /// <param name="pvsErrorType">Type of error.</param>
        /// <param name="pvException">The actual exception that happened.</param>
        /// <param name="pvsLocation">The location where the error happened.</param>
        /// <returns>The formatted string of the error.</returns>
        public string FormatError(string pvsErrorType, Exception pvException, string pvsLocation)
        {
            // First make the error string. We will always do this.
            string lsErrorText = "";

            try
            {
                lsErrorText = "General Error generated on : " + pvsLocation + "<br><br>";
                lsErrorText += "Location of Error: " + pvsLocation + "<br><br>";
                if ( pvException != null )
                    lsErrorText += "Error: " + pvException.ToString() + "<br><br>";
                else
                    lsErrorText += "Error: pvException is null" + "<br><br>";

                if ( pvException.Message != null )
                    lsErrorText += "Message: " + pvException.Message.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                else
                    lsErrorText += "Message: pvException.Message is null" + "<br><br>";

                if (pvException.Source != null)
                    lsErrorText += "Source: " + pvException.Source.ToString(CultureInfo.CurrentCulture) + "<br>" + "<br>";
                if (pvException.TargetSite != null)
                    lsErrorText += "TargetSite: " + pvException.TargetSite.ToString() + "<br><br>";
                if (pvException.StackTrace != null)
                    lsErrorText += "StackTrace: " + pvException.StackTrace.ToString(CultureInfo.CurrentCulture) + "<br><br>";

                if (pvException.InnerException != null)
                {
                    if ( pvException.InnerException != null )
                        lsErrorText += "Message: " + pvException.InnerException.ToString() + "<br>" + "<br>";
                    if (pvException.InnerException.Source != null)
                        lsErrorText += "Source: " + pvException.InnerException.Source.ToString(CultureInfo.CurrentCulture) + "<br>" + "<br>";
                    if (pvException.InnerException.TargetSite != null)
                        lsErrorText += "TargetSite: " + pvException.InnerException.TargetSite.ToString() + "<br>" + "<br>";
                    if (pvException.InnerException.StackTrace != null)
                        lsErrorText += "StackTrace: " + pvException.InnerException.StackTrace.ToString(CultureInfo.CurrentCulture) + "<br>" + "<br>";
                }
            }
            catch
            {
                // Want to catch the error but write out as much as we can.
            }

            lsErrorText = lsErrorText.Replace(Environment.NewLine, "<br>");

            return lsErrorText;
        }

        /// <summary>
		/// Formats a SQL Exception to make it printable (web printable)
		/// </summary>
        /// <param name="pvsComputerName">Name of computer it happened on.</param>
        /// <param name="pvsErrorType">Type of error.</param>
        /// <param name="pvSQLException">The actual exception that happened.</param>
        /// <param name="pvsLocation">The location where the error happened.</param>
        /// <returns>The formatted string of the error.</returns>
        public string FormatError(string pvsErrorType, SqlException pvSQLException, string pvsLocation)
        {
            string lsErrorText = "";

            lsErrorText = "SQL Error generated at " + pvsLocation + "<br><br>";

            foreach (SqlError SQLExcept in pvSQLException.Errors)
            {
                lsErrorText += "Location of Error: " + pvsLocation + "<br><br>";
                lsErrorText += "Error: " + SQLExcept.ToString() + "<br><br>";
                lsErrorText += "Line Number: " + SQLExcept.LineNumber.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "Message: " + SQLExcept.Message.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "Class: " + SQLExcept.Class.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "Number: " + SQLExcept.Number.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "Procedure: " + SQLExcept.Procedure.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "Server: " + SQLExcept.Server.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "Source: " + SQLExcept.Source.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "State: " + SQLExcept.State.ToString(CultureInfo.CurrentCulture) + "<br><br>";
                lsErrorText += "StackTrace: " + pvSQLException.StackTrace.ToString(CultureInfo.CurrentCulture) + "<br><br>";
            }
            return lsErrorText;
        }

        /// <summary>
        /// Formats the SQL command to make it printable.
        /// </summary>
        /// <param name="lcmdSQL">Command to format.</param>
        /// <returns>A string containing the information about the SQL command.</returns>
        public string FormatSQLCmd ( SqlCommand lcmdSQL )
        {
            string lsErrorText = "";

            if (lcmdSQL != null)
            {
                lsErrorText = "The SQL Command was:<br>" +
                    "Command Text: " + lcmdSQL.CommandText + "<br>" +
                    "Command Type: " + lcmdSQL.CommandType + "<br>";
                if (lcmdSQL.Parameters.Count == 0)
                    lsErrorText += "No Parameters<br><br>";
                else
                {
                    lsErrorText += "Parameters:<br>";
                    foreach (SqlParameter lParam in lcmdSQL.Parameters)
                    {
                        lsErrorText += lParam.ParameterName + " = " + lParam.Value + "<br>";
                    }
                }
                lsErrorText += "<br><br>";
                lsErrorText += "Connection String: " + lcmdSQL.Connection.ConnectionString + "<br><br>";
            }

            return lsErrorText;
        }
    }
}
