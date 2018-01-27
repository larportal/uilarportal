using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

// This class is only used in the character class. It's really so we can keep track of what teams a character belongs to.

namespace LarpPortal.Classes
{
    [Serializable]
    public class cTeamInfo
    {
        public cTeamInfo()
        {
            TeamID = -1;
            TeamName = "";
            CharacterID = -1;
            RoleID = -1;
            RoleDescription = "";
            Approval = false;
            Member = false;
            Requested = false;
            Invited = false;
            PrimaryTeam = false;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "Not Yet Defined.";
        }

        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int CharacterID { get; set; }
        public int RoleID { get; set; }
        public string RoleDescription { get; set; }
        public Boolean Approval { get; set; }
        public Boolean Member { get; set; }
        public Boolean Requested { get; set; }
        public Boolean Invited { get; set; }
        public Boolean PrimaryTeam { get; set; }
        public RecordStatuses RecordStatus { get; set; }
    }
}