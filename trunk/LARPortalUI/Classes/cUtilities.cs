using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.IO;
//using System.DirectoryServices;
using LarpPortal.Classes;
//using Excel;

// General comment: 7/19/2015 JBradshaw changed so if the routines that get a datatable/dataset get an error, it re-throws the error
//                            so the calling routine knows that the call failed.
//                  6/24/2016 JBradshaw Added WriteSQLAuditRecord.
//                  4/27/2017 JBradshaw Added ReplaceQuotes.

namespace LarpPortal.Classes
{
    [Serializable()]
    public class cUtilities
    {
        public cUtilities()
        {
        }

        public enum LoadDataTableCommandType
        {
            StoredProcedure,
            Text
        };

        /// <summary>
        /// Load a datatable from a stored procedure.
        /// </summary>
        /// <param name="strStoredProc">Stored procedure to call.</param>
        /// <param name="slParameters">SortedList of parameters.</param>
        /// <param name="strLConn">Name of the connection to use.</param>
        /// <param name="strUserName">User doing this.</param>
        /// <param name="strCallingMethod">What's the name of the method calling this.</param>
        /// <returns></returns>
        public static DataTable LoadDataTable(string strStoredProc, SortedList slParameters, string strLConn, string strUserName, string strCallingMethod)
        {
            return LoadDataTable(strStoredProc, slParameters, strLConn, strUserName, strCallingMethod, LoadDataTableCommandType.StoredProcedure);
        }

        /// <summary>
        /// Load a datatable from a stored procedure/SQL command.
        /// </summary>
        /// <param name="strStoredProc">Stored procedure/SQL command to run.</param>
        /// <param name="slParameters">SortedList of parameters.</param>
        /// <param name="strLConn">Name of the connection to use.</param>
        /// <param name="strUserName">User doing this.</param>
        /// <param name="strCallingMethod">What's the name of the method calling this.</param>
        /// <param name="strCommandType">use LoadDataTableCommandType to tell the type.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataTable LoadDataTable(string strStoredProc, SortedList slParameters, string strLConn, string strUserName, string strCallingMethod, LoadDataTableCommandType strCommandType)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            DataTable ldt = new DataTable();
            using (SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings[strLConn].ConnectionString))
            {
                using (SqlCommand lcmd = new SqlCommand(strStoredProc, lconn))
                {
                    if (strCommandType == LoadDataTableCommandType.Text)
                        lcmd.CommandType = CommandType.Text;
                    else
                        lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.CommandTimeout = 0;
                    lcmd.Connection = lconn;
                    if (slParameters.Count > 0)
                    {
                        for (int i = 0; i < slParameters.Count; i++)
                        {
                            // Original code
                            // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                            // New code - JLB (via Rick) 7/16/2016
                            SqlParameter NewParam = new SqlParameter();
                            NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                            if (slParameters.GetByIndex(i) != null)
                                NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                            lcmd.Parameters.Add(NewParam);
                            // End new code 7/16/2016
                        }
                    }
                    SqlDataAdapter ldsa = new SqlDataAdapter(lcmd);

                    try
                    {
                        lconn.Open();
                        ldsa.Fill(ldt);
                        WriteSQLAuditRecord(lconn, strStoredProc, slParameters, strUserName, strCallingMethod);
                    }
                    catch (SqlException exSQL)
                    {
                        // Write the exception to error log and then throw it again...
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(exSQL, lsRoutineName + ":" + strStoredProc, lcmd, strUserName + strCallingMethod);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName + strCallingMethod);
                        throw;
                    }
                }
            }
            return ldt;
        }


        //public static DataSet ReturnDataTableFromExcelWorksheet(string strSheetLocation, string strSheetName, string strUserName)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
        //    DataSet dsUnUpdated = new DataSet();
        //    try
        //    {
        //        IExcelDataReader iExcelDataReader = null;

        //        FileStream oStream = File.Open(strSheetLocation, FileMode.Open, FileAccess.Read);

        //        iExcelDataReader = ExcelReaderFactory.CreateBinaryReader(oStream);

        //        iExcelDataReader.IsFirstRowAsColumnNames = true;

        //        dsUnUpdated = iExcelDataReader.AsDataSet();

        //        iExcelDataReader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorAtServer lobjError = new ErrorAtServer();
        //        lobjError.ProcessError(ex, lsRoutineName, strUserName);
        //        throw;
        //    }
        //    return dsUnUpdated;
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static string ReturnStringFromSQL(string strStoredProc, string strReturnValue, SortedList slParameters, string strLConn, string strUserName, string strCallingMethod)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string strReturn = "";

            using (SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings[strLConn].ConnectionString))
            {
                using (SqlCommand lcmd = new SqlCommand(strStoredProc, lconn))
                {
                    lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.CommandTimeout = 0;
                    lcmd.Connection = lconn;
                    if (slParameters.Count > 0)
                    {
                        for (int i = 0; i < slParameters.Count; i++)
                        {
                            if (slParameters.GetKey(i).ToString().Trim().Substring(0, 2) == "dt")
                            {
                                if (slParameters.GetByIndex(i).ToString().Trim().Substring(0, 10) == "01/01/1900")
                                {
                                    lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), DBNull.Value));
                                }
                            }
                            else
                            {
                                // Original code
                                // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                                // New code - JLB (via Rick) 7/16/2016
                                SqlParameter NewParam = new SqlParameter();
                                NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                                if (slParameters.GetByIndex(i) != null)
                                    NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                                lcmd.Parameters.Add(NewParam);
                                // End new code 7/16/2016
                            }
                        }
                    }
                    SqlDataAdapter ldsa = new SqlDataAdapter(lcmd);
                    DataTable ldt = new DataTable();

                    try
                    {
                        lconn.Open();
                        ldsa.Fill(ldt);
                        WriteSQLAuditRecord(lconn, strStoredProc, slParameters, strUserName, strCallingMethod);
                        if (ldt.Rows.Count > 0)
                        {
                            strReturn = ldt.Rows[0][strReturnValue].ToString().Trim();
                        }
                    }
                    catch (SqlException exSQL)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(exSQL, lsRoutineName, lcmd, strUserName + strCallingMethod);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName + strCallingMethod);
                        throw;
                    }
                }
            }
            return strReturn;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void PerformNonQuery(string strStoredProc, SortedList slParameters, string strLConn, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            using (SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings[strLConn].ConnectionString))
            {
                using (SqlCommand lcmd = new SqlCommand(strStoredProc, lconn))
                {
                    lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.CommandTimeout = 0;
                    if (slParameters.Count > 0)
                    {
                        for (int i = 0; i < slParameters.Count; i++)
                        {
                            // Original code
                            // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                            // New code - JLB (via Rick) 7/16/2016
                            SqlParameter NewParam = new SqlParameter();
                            NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                            if (slParameters.GetByIndex(i) != null)
                                NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                            lcmd.Parameters.Add(NewParam);
                            // End new code 7/16/2016
                        }
                    }
                    try
                    {
                        lconn.Open();
                        lcmd.ExecuteNonQuery();
                        WriteSQLAuditRecord(lconn, strStoredProc, slParameters, strUserName, "");
                    }
                    catch (SqlException exSQL)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(exSQL, lsRoutineName, lcmd, strUserName);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName);
                        throw;
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static Boolean PerformNonQueryBoolean(string strStoredProc, SortedList slParameters, string strLConn, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            using (SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings[strLConn].ConnectionString))
            {
                using (SqlCommand lcmd = new SqlCommand(strStoredProc, lconn))
                {
                    lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.CommandTimeout = 0;
                    Boolean blnReturn = false;
                    if (slParameters.Count > 0)
                    {
                        for (int i = 0; i < slParameters.Count; i++)
                        {
                            // Original code
                            // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                            // New code - JLB (via Rick) 7/16/2016
                            SqlParameter NewParam = new SqlParameter();
                            NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                            if (slParameters.GetByIndex(i) != null)
                                NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                            lcmd.Parameters.Add(NewParam);
                            // End new code 7/16/2016
                        }
                    }
                    try
                    {
                        lconn.Open();
                        lcmd.ExecuteNonQuery();
                        WriteSQLAuditRecord(lconn, strStoredProc, slParameters, strUserName, "");
                        blnReturn = true;
                    }
                    catch (SqlException exSQL)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(exSQL, lsRoutineName, lcmd, strUserName);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName);
                        throw;
                    }
                    return blnReturn;
                }
            }
        }

        public static ArrayList SeperateStrings(string strString, string strSeperator, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            ArrayList alList = new ArrayList();

            strString = strString.Trim();
            int intLength = strString.Length;
            try
            {
                do
                {
                    int intIndex = 0;
                    intIndex = strString.IndexOf(strSeperator, StringComparison.CurrentCulture);
                    if (intIndex > 2)
                    {
                        alList.Add(strString.Substring(0, intIndex));
                    }
                    else
                    {
                        if (intIndex == -1)
                        {
                            alList.Add(strString);
                            break;
                        }
                    }
                    strString = strString.Remove(0, intIndex + 1);
                    strString = strString.Trim();
                    intLength = strString.Length;
                } while (intLength != 0);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName);
                throw;
            }
            return alList;
        }

        public static void LoadDropDownList(DropDownList ddlList, string strStoredProc, SortedList slParam, string strTextValue, string strDataValue, string strLConn, string strUserName, string strCallingRoutine)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            DataTable ldt = new DataTable();
            try
            {
                ldt = LoadDataTable(strStoredProc, slParam, strLConn, strUserName, lsRoutineName + '-' + strCallingRoutine);
                if (ldt.Rows.Count > 0)
                {
                    ddlList.DataSource = ldt;
                    ddlList.DataTextField = strTextValue;
                    ddlList.DataValueField = strDataValue;
                    ddlList.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName);
                throw;
            }
        }

        public static string Right(string strString, int length)
        {
            string strReturn = strString.Substring(strString.Length - length);
            return strReturn;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static Int32 ReturnIntFromSQL(string strStoredProc, string strReturnValue, SortedList slParameters, string strLConn, string strUserName, string strCallingMethod)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            using (SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings[strLConn].ConnectionString))
            {
                using (SqlCommand lcmd = new SqlCommand(strStoredProc, lconn))
                {
                    lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.CommandTimeout = 0;

                    try
                    {
                        if (slParameters.Count > 0)
                        {
                            for (int i = 0; i < slParameters.Count; i++)
                            {
                                if (slParameters.GetKey(i).ToString().Trim().Substring(0, 2) == "dt")
                                {
                                    if (slParameters.GetByIndex(i).ToString().Trim().Substring(0, 10) == "01/01/1900")
                                    {
                                        lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), DBNull.Value));
                                    }
                                }
                                else
                                {
                                    // Original code
                                    // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                                    // New code - JLB (via Rick) 7/16/2016
                                    SqlParameter NewParam = new SqlParameter();
                                    NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                                    if (slParameters.GetByIndex(i) != null)
                                        NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                                    lcmd.Parameters.Add(NewParam);
                                    // End new code 7/16/2016
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName + strCallingMethod);
                        throw;
                    }
                    SqlDataAdapter ldsa = new SqlDataAdapter(lcmd);
                    DataTable ldt = new DataTable();
                    String strReturn = "";
                    Int32 intReturn = 0;
                    try
                    {
                        long number1 = 0;
                        Boolean blnCanConvert = false;
                        lconn.Open();
                        ldsa.Fill(ldt);
                        WriteSQLAuditRecord(lconn, strStoredProc, slParameters, strUserName, strCallingMethod);

                        if (ldt.Rows.Count > 0)
                        {
                            strReturn = ldt.Rows[0][strReturnValue].ToString().Trim();
                        }
                        blnCanConvert = long.TryParse(strReturn, out number1);
                        if (blnCanConvert)
                        {
                            intReturn = Convert.ToInt32(strReturn);
                        }
                    }
                    catch (SqlException exSQL)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(exSQL, lsRoutineName, lcmd, strUserName + strCallingMethod);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName + strCallingMethod);
                        throw;
                    }

                    return intReturn;
                }
            }
        }

        public static void ShowAlertMessage(string error)
        {

            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {

                error = error.Replace("'", "\\'");

                ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", true);

            }

        }

        public static DateTime ParseStringToDateTime(string strPassed)
        {
            DateTime temp;
            DateTime dtReturn = DateTime.Now;
            if (DateTime.TryParse(strPassed, out temp))
            {
                dtReturn = temp;
            }

            return dtReturn;
        }

        public static Int32 ParseStringToInt32(string strPassed)
        {
            Int32 temp;
            Int32 intReturn = -1;
            if (Int32.TryParse(strPassed, out temp))
            {
                intReturn = temp;
            }

            return intReturn;

        }

        public static string FormatFileNameForSave(string strPassed)
        {

            string strReturn = strPassed.Replace(" ", "");
            strReturn = strReturn.Replace("(", "");
            strReturn = strReturn.Replace(")", "");
            strReturn = strReturn.Replace("#", "");
            strReturn = strReturn.Replace("-", "");
            strReturn = strReturn.Replace("_", "");
            strReturn = strReturn.Replace("!", "");
            strReturn = strReturn.Replace("@", "");
            strReturn = strReturn.Replace("$", "");
            strReturn = strReturn.Replace("%", "");
            strReturn = strReturn.Replace("^", "");
            strReturn = strReturn.Replace("&", "");
            strReturn = strReturn.Replace("*", "");
            strReturn = strReturn.Replace("{", "");
            strReturn = strReturn.Replace("}", "");
            strReturn = strReturn.Replace("|", "");
            strReturn = strReturn.Replace("[", "");
            strReturn = strReturn.Replace("]", "");
            strReturn = strReturn.Replace("'", "");
            strReturn = strReturn.Replace(@"""", "");
            return strReturn;

        }

        // JBradshaw 4/27/2017 Added handling of null datatypes.
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }


        //public static string replaceUNCPath(string strPath)
        //{
        //    string strReturn = strPath.ToUpper();
        //    string strSharesPath = ReturnDefaultValue("SharesPath", "");
        //    if (strReturn.IndexOf("TMC-ERPDPLY-1", StringComparison.CurrentCulture) > -1)
        //    {
        //        strReturn = @"/ECSMT/HTMLUPload/";
        //    }
        //    else
        //    {
        //        strReturn = strReturn.ToUpper().Replace(@"\\TMC-OFFICE-6\SHARES\", "/");
        //        strReturn = strReturn.ToUpper().Replace(@"\\\\TMC-OFFICE-6\\SHARES\\", "/");
        //        strReturn = strReturn.Replace(@"\", @"/");
        //    }
        //    return strReturn;
        //}

        //public static string ReturnDefaultValue(string strValue, string strUserName)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
        //    SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString);
        //    SqlCommand lcmd = new SqlCommand();
        //    lcmd.CommandText = "uspGetDefaultValues";
        //    lcmd.CommandType = CommandType.StoredProcedure;
        //    lcmd.CommandTimeout = 0;
        //    lcmd.Connection = lconn;
        //    lcmd.Parameters.Add(new SqlParameter("@strDefaultCode", strValue));
        //    SqlDataAdapter ldsa = new SqlDataAdapter(lcmd);
        //    DataTable ldt = new DataTable();
        //    String strReturn = "";
        //    try
        //    {
        //        lconn.Open();
        //        ldsa.Fill(ldt);
        //        if (ldt.Rows.Count > 0)
        //        {
        //            strReturn = ldt.Rows[0]["DefaultValue"].ToString().Trim();
        //        }
        //    }
        //    catch (SqlException exSQL)
        //    {
        //        ErrorAtServer lobjError = new ErrorAtServer();
        //        lobjError.ProcessError(exSQL, lsRoutineName, lcmd, strUserName + lsRoutineName);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorAtServer lobjError = new ErrorAtServer();
        //        lobjError.ProcessError(ex, lsRoutineName, strUserName + lsRoutineName);
        //        throw;
        //    }
        //    finally
        //    {
        //        lconn.Close();
        //    }
        //    return strReturn;
        //}

        //public static string replaceUNCPathForOrderEntry(string strPath)
        //{
        //    string strReturn = strPath.ToUpper();
        //    string strSharesPath = ReturnDefaultValue("SharesPath", "");
        //    if (strReturn.IndexOf("TMC-ERPDPLY-1", StringComparison.CurrentCulture) > -1)
        //    {
        //        strReturn = @"/ECSMT/HTMLUPload/";
        //    }
        //    else
        //    {
        //        strReturn = strReturn.ToUpper().Replace(@"\\TMC-OFFICE-6\SHARES\ORDERENTRY\", "/");
        //        strReturn = strReturn.ToUpper().Replace(@"\\\\TMC-OFFICE-6\\SHARES\\ORDERENTRY\\", "/");
        //        strReturn = strReturn.Replace(@"\", @"/");
        //    }
        //    return strReturn;
        //}

        public static Int16 ReplaceBooleanWithBit(Boolean blnValue)
        {
            Int16 intReturn = 1;
            if (blnValue)
            {
                intReturn = 1;
            }
            else
            {
                intReturn = 0;
            }

            return intReturn;
        }

        public static Boolean ReplaceBitWithBoolean(Int32 intBit)
        {
            Boolean blnReturn = false;
            if (intBit == 1)
            {
                blnReturn = true;
            }
            else
            {
                blnReturn = false;
            }

            return blnReturn;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataSet LoadDataSetWithOpenConnection(string strStoredProc, SortedList slParameters, SqlConnection Conn, string strUserName, string strCallingMethod)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SqlCommand lcmd = new SqlCommand(strStoredProc, Conn);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.CommandTimeout = 0;
            if (slParameters.Count > 0)
            {
                for (int i = 0; i < slParameters.Count; i++)
                {
                    // Original code
                    // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                    // New code - JLB (via Rick) 7/16/2016
                    SqlParameter NewParam = new SqlParameter();
                    NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                    if (slParameters.GetByIndex(i) != null)
                        NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                    lcmd.Parameters.Add(NewParam);
                    // End new code 7/16/2016
                }
            }
            SqlDataAdapter ldsa = new SqlDataAdapter(lcmd);
            DataSet lds = new DataSet();

            try
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                ldsa.Fill(lds);
            }
            catch (SqlException exSQL)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(exSQL, lsRoutineName + ":" + strStoredProc, lcmd, strUserName + strCallingMethod);
                throw;
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName + strCallingMethod);
                throw;
            }

            return lds;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataSet LoadDataSet(string strStoredProc, SortedList slParameters, string strLConn, string strUserName, string strCallingMethod)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            using (SqlConnection lconn = new SqlConnection(ConfigurationManager.ConnectionStrings[strLConn].ConnectionString))
            {
                using (SqlCommand lcmd = new SqlCommand(strStoredProc, lconn))
                {
                    lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.CommandTimeout = 0;

                    if (slParameters.Count > 0)
                    {
                        for (int i = 0; i < slParameters.Count; i++)
                        {
                            // Original code
                            // lcmd.Parameters.Add(new SqlParameter(slParameters.GetKey(i).ToString().Trim(), slParameters.GetByIndex(i).ToString().Trim()));
                            // New code - JLB (via Rick) 7/16/2016
                            SqlParameter NewParam = new SqlParameter();
                            NewParam.ParameterName = slParameters.GetKey(i).ToString().Trim();
                            if (slParameters.GetByIndex(i) != null)
                                NewParam.Value = slParameters.GetByIndex(i).ToString().Trim();
                            lcmd.Parameters.Add(NewParam);
                            // End new code 7/16/2016
                        }
                    }
                    SqlDataAdapter ldsa = new SqlDataAdapter(lcmd);
                    DataSet lds = new DataSet();

                    try
                    {
                        lconn.Open();
                        ldsa.Fill(lds);
                    }
                    catch (SqlException exSQL)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(exSQL, lsRoutineName + ":" + strStoredProc, lcmd, strUserName + strCallingMethod);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ErrorAtServer lobjError = new ErrorAtServer();
                        lobjError.ProcessError(ex, lsRoutineName, strUserName + strCallingMethod);
                        throw;
                    }

                    return lds;
                }
            }
        }
        /// <summary>
        /// Writes out an audit record of SQL that is performed.
        /// </summary>
        /// <param name="sConn">Open SQL connection.</param>
        /// <param name="strStoredProc">SP or Text that was run.</param>
        /// <param name="slParameters">Parameters that were passed in</param>
        /// <param name="strUserName">User Name</param>
        /// <param name="strCallingMethod">Where was this called from.</param>
        public static void WriteSQLAuditRecord(SqlConnection sConn, string strStoredProc, SortedList slParameters, string strUserName, string strCallingMethod)
        {
            try
            {
                if (sConn.State != ConnectionState.Open)
                    sConn.Open();

                using (SqlCommand CmduspInsSQLAudit = new SqlCommand("uspInsSQLAudit", sConn))
                {
                    CmduspInsSQLAudit.CommandType = CommandType.StoredProcedure;
                    CmduspInsSQLAudit.Parameters.AddWithValue("@SQLStmt", strStoredProc);
                    CmduspInsSQLAudit.Parameters.AddWithValue("@CallingUser", strUserName);
                    CmduspInsSQLAudit.Parameters.AddWithValue("@CallingMethod", strCallingMethod);

                    CmduspInsSQLAudit.ExecuteNonQuery();
                }
            }
            catch
            {
                // Not much to do so keep going.
            }
        }

        public static void WriteSQLAuditRecord(string strStoredProc, SortedList slParameters, string strLConn, string strUserName, string strCallingMethod)
        {
            using (SqlConnection conn = new SqlConnection(strLConn))
            {
                conn.Open();
                WriteSQLAuditRecord(conn, strStoredProc, slParameters, strUserName, strCallingMethod);
            }
        }

        public static string ReplaceQuotes(object sOrig)
        {
            return sOrig.ToString().Replace("'", "\'").Replace("\"", "\\\"");
        }
    }
}
