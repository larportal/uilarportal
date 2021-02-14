using System;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using LarpPortal.Classes;


namespace LarpPortal.Classes
{
    /// <summary>
    /// Processing error at a server. This trys to connect directly to the database.
    /// </summary>
    public class ErrorAtClient
    {
        /// <summary>
        /// Given the error, this will attempt to format it and save it into the database.
        /// </summary>
        /// <param name="pvsComputerName">The name of the computer where the error happened.</param>
        /// <param name="pvExceptionText">The actual exception that happened.</param>
        /// <param name="pvsLocation">The location of the error.</param>
        public void ProcessError ( string pvsComputerName, string pvExceptionText, string pvsLocation )
        {
            try
            {
                string lsErrorText = pvExceptionText;
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.HandleError("Client Error", pvExceptionText, pvsLocation, "", pvsComputerName);
            }
            catch
            {
                // Can't really do anything because if we can't connect to the home server, then we're screwed.
            }
        }

        /// <summary>
        /// Given the error, this will attempt to format it and save it into the database.
        /// </summary>
        /// <param name="pvsComputerName">The name of the computer where the error happened.</param>
        /// <param name="pvExceptionText">The actual exception that happened.</param>
        /// <param name="pvsLocation">The location of the error.</param>
        /// <param name="pvsAddInfo">Any additional info to be added to the record.</param>
        public void ProcessError(string pvsComputerName, string pvExceptionText, string pvsLocation, string pvsAddInfo)
        {
            try
            {
                string lsErrorText = pvExceptionText;
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.HandleError("Client Error", pvExceptionText, pvsLocation, pvsAddInfo, pvsComputerName);
            }
            catch
            {
                // Can't really do anything because if we can't connect to the home server, then we're screwed.
            }
        }

        /// <summary>
        /// Given the error, this will attempt to format it and save it into the database.
        /// </summary>
        /// <param name="pvsComputerName">The name of the computer where the error happened.</param>
        /// <param name="pvExceptionText">The actual exception that happened.</param>
        /// <param name="pvsLocation">The location of the error.</param>
        /// <param name="pvsAddInfo">Any additional info to be added to the record.</param>
        /// <param name="pvsApplicationVersion">Application version that caused the problem.</param>
        public void ProcessError(string pvsComputerName, string pvExceptionText, string pvsLocation, 
                string pvsAddInfo, string pvsApplicationVersion)
        {
            try
            {
                string lsErrorText = pvExceptionText;
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.HandleError("Client Error", pvExceptionText, pvsLocation, pvsAddInfo, pvsComputerName, pvsApplicationVersion);
            }
            catch
            {
                // Can't really do anything because if we can't connect to the home server, then we're screwed.
            }
        }




    }
}
