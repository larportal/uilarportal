using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

using LarpPortal.Classes;

namespace LarpPortal.Classes
{
    [Serializable]
    public class cPicture
    {
        private string _URLDirectory = "~/img/";
        private string _RootDirectory = @"img\";


        /// <summary>
        /// Handles database interaction and figures out where the picture files should be saved.
        /// </summary>
        public cPicture()
        {
            PictureType = PictureTypes.Generic;
            RecordStatus = RecordStatuses.Active;
            PictureID = -1;
        }


        /// <summary>
        /// enum to tell what type of picture is to be saved. 
        /// This gets used to put the files in the correct sub directories.
        /// </summary>
        public enum PictureTypes
        {
            Generic,
            Logo,
            Profile,
            Player,
            Item
        };


        public override string ToString()
        {
            return "ID: " + PictureID.ToString() + "  " + PictureFileName;
        }

        public int PictureID { get; set; }
        public string PictureFileName { get; set; }
        public string CreatedBy { get; set; }
        public int CharacterID { get; set; }
        public int ProfileID { get; set; }
        public string Comments { get; set; }
        /// <summary>
        /// The type of picture it is (profile, logo, ...)
        /// </summary>
        public PictureTypes PictureType { get; set; }
        public string FileExtension { get; set; }
        public RecordStatuses RecordStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUploaded { get; set; }
        public DateTime DateDeleted { get; set; }

        public string PictureURL
        {
            get
            {
                return _URLDirectory + PictureType.ToString() + "/" + PictureFileName;
            }
        }


        /// <summary>
        /// Formats the file name to a URL.
        /// </summary>
        /// <returns>File name as a URL</returns>
        //public string PictureURL ()
        //{
        //    return _URLDirectory + PictureType.ToString() + "/" + PictureFileName;
        //}


        /// <summary>
        /// Formats the file name to a local file name including the directories.
        /// </summary>
        /// <returns>Local file name including subdirectories.</returns>
        public string PictureLocalName
        {
            get
            {
                return Path.Combine(HttpContext.Current.Server.MapPath("/"), _RootDirectory, PictureType.ToString(), PictureFileName);
            }
        }


        /// <summary>
        /// Save an picture record to the database.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        public void Save(string sUserUpdating)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Delete)
            {
                lsRoutineName += ".Delete";
                SortedList sParam = new SortedList();
                sParam.Add("@PictureID", PictureID);
                sParam.Add("@PictureName", PictureFileName);
                sParam.Add("@PictureType", PictureType.ToString());
                sParam.Add("@UserID", sUserUpdating);

                cUtilities.PerformNonQuery("uspDelMDBPictures", sParam, "LARPortal", sUserUpdating);
            }
            else
            {
                SortedList sParam = new SortedList();
                sParam.Add("@PictureID", PictureID);
                sParam.Add("@PictureFileName", PictureFileName);
                sParam.Add("@CharacterID", CharacterID);
                sParam.Add("@PictureType", PictureType.ToString());
                cUtilities.PerformNonQuery("uspMDBPictureUpdatePicture", sParam, "LARPortal", sUserUpdating);
            }
        }


        /// <summary>
        /// Load a picture record. 
        /// </summary>
        /// <param name="iPictureID">Picture ID to load.</param>
        /// <param name="sUserID">User ID loading picture.</param>
        public void Load(int iPictureID, string sUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (iPictureID > 0)
            {
                SortedList sParam = new SortedList();
                sParam.Add("@MDBPictureID", iPictureID.ToString());

                DataTable dtPicture = new DataTable();
                dtPicture = cUtilities.LoadDataTable("uspGetMDBPicture", sParam, "LARPortal", sUserID, lsRoutineName);

                foreach (DataRow dRow in dtPicture.Rows)
                {
                    int iTemp;
                    if (int.TryParse(dRow["MDBPictureID"].ToString(), out iTemp))
                        PictureID = iTemp;
                    PictureFileName = dRow["PictureFileName"].ToString();
                    PictureType = (PictureTypes) Enum.Parse(typeof(PictureTypes), dRow["PictureType"].ToString());

                    if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                        CharacterID = iTemp;

                    CreatedBy = dRow["CreatedBy"].ToString();

                    DateTime dtTemp;
                    if (DateTime.TryParse(dRow["DateCreated"].ToString(), out dtTemp))
                        DateCreated = dtTemp;

                    if (DateTime.TryParse(dRow["DateUploaded"].ToString(), out dtTemp))
                        DateUploaded = dtTemp;

                    if (DateTime.TryParse(dRow["DateDeleted"].ToString(), out dtTemp))
                        DateDeleted = dtTemp;
                }
            }
        }

        /// <summary>
        /// 'Reserves' a picture record. This will get you the next ID in the database. Use the
        /// id as part of the file name to keep it unique. When this is done, the record will be
        /// empty but can be updated.
        /// </summary>
        /// <param name="sUserID">User creating the new record.</param>
        public void CreateNewPictureRecord(string sUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CreatedBy", sUserID);

            DataTable dtNewPicture = new DataTable();
            dtNewPicture = cUtilities.LoadDataTable("uspMDBPicturesNewPicture", sParam, "LARPortal", sUserID, lsRoutineName);

            foreach (DataRow dRow in dtNewPicture.Rows)
            {
                int iTemp;
                if (int.TryParse(dRow[0].ToString(), out iTemp))
                    PictureID = iTemp;
            }
        }



        /// <summary>
        /// 'Reserves' a picture record. This will get you the next ID in the database. Use the
        /// id. Send in the picture type and file extension and this will also set the full file name.
        /// </summary>
        /// <param name="sUserID">User creating the new record.</param>
        /// <param name="PicType">The type of file it is - Item, Profile, ...</param>
        /// <param name="PicExtension">Extension of file that will be created.</param>
        public void CreateNewPictureRecord(cPicture.PictureTypes PicType, string PicExtension, string sUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CreatedBy", sUserID);
            sParam.Add("@PictureType", PicType.ToString());
            sParam.Add("@PictureExtension", PicExtension);
            DataTable dtNewPicture = new DataTable();
            dtNewPicture = cUtilities.LoadDataTable("uspMDBPicturesNewPicture", sParam, "LARPortal", sUserID, lsRoutineName);

            PictureType = PicType;
            FileExtension = PicExtension;
            int iTemp = 0;

            foreach (DataRow dRow in dtNewPicture.Rows)
            {
                if (int.TryParse(dRow[0].ToString(), out iTemp))
                    PictureID = iTemp;
            }
            PictureFileName = PictureType.ToString().ToUpper().Substring(0, 2) + iTemp.ToString("D10") + PicExtension;
        }


        /// <summary>
        /// Delete the picture. Set the ID before doing this.
        /// </summary>
        /// <param name="sUserID">User ID of person deleting it.</param>
        public void Delete(string sUserID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@PictureID", PictureID.ToString());
            sParam.Add("@UserID", sUserID);
            cUtilities.PerformNonQuery("uspDelMDBPictures", sParam, "LARPortal", sUserID);
        }


        /// <summary>
        /// Sample code on how to use this class.
        /// </summary>
        public void Documentation()
        {
            //// This assumes that there is a variable UserID (probably Session["LoginName"])
            //// ulFile is a file upload control that the person uses to upload the file.

            //Classes.cPicture NewPicture = new Classes.cPicture();
            //NewPicture.PictureType = Classes.cPicture.PictureTypes.Profile;

            //// CreateNewPictureRecord 'reserves' the next record. Get's the next record ID
            //// in the database so it can be used as part of the file name.
            //NewPicture.CreateNewPictureRecord(UserID);

            //// Get the extension. The file name must have the correct extension.
            //string sExtension = Path.GetExtension(ulFile.FileName);

            //// If you want to do a different file naming convention do it here.
            //// For the profile picture it's using CP so the file name is    CP0000000092.gif
            //NewPicture.PictureFileName = NewPicture.PictureID.ToString("D10") + sExtension;

            //// PictureLoadName is the local file name. It's where the local file name is to be saved.
            //string LocalName = NewPicture.PictureLocalName();

            //// Save the actual file.
            //ulFile.SaveAs(NewPicture.PictureLocalName());

            //// Save the picture record to the database.
            //NewPicture.Save(sUser);

            //// PictureURL has the URL of the file.
            //imgCharacterPicture.ImageUrl = NewPicture.PictureURL;
        }
    }
}