using System;

namespace LarpPortal.Classes
{
    [Serializable()]
    public class cSkillPool
    {
        public cSkillPool()
        {
            PoolID = -1;
            TotalPoints = 0.0;
            DefaultPool = false;
            PoolDescription = "";
            PoolDisplayColor = "";
            RecordStatus = RecordStatuses.Active;
        }
        public int PoolID { get; set; }
        public double TotalPoints { get; set; }
        public bool DefaultPool { get; set; }
        public string PoolDescription { get; set; }
        public string PoolDisplayColor { get; set; }
        public RecordStatuses RecordStatus { get; set; }
    }
}
