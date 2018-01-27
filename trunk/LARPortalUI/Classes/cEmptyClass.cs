using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;


namespace LarpPortal.Classes
{
    public class cEmptyClass
    {

        private string _someValue = "";
        private DateTime? _aNullableDateValue = null; // putting a ? after the type when defining a field, property, or any variable allows that field to be null 

        public DateTime? ANullableDateValue
        {
            get { return _aNullableDateValue; }
            set { _aNullableDateValue = value; }
        }


        public string SomeValue
        {
            get { return _someValue; }
            set { _someValue = value; }
        }



        private cEmptyClass()
        {

        }

        public cEmptyClass(string strParameter1, Int32 intParameter2, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;


            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@Parameter1", strParameter1);
            slParams.Add("@Parameter2", intParameter2);

            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetSomeData", slParams, "DefaultSQLConnection", strUserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _someValue = ldt.Rows[0]["SomeValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, strUserName + lsRoutineName);
            }
        }
       

    }


}