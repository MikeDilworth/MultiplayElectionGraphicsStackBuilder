using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class StateMetadataModel
    {
        public Int16 State_ID { get; set; }
        public string State_Abbv { get; set; }
        public string State_Name { get; set; }
        public Boolean IsBattlegroundState { get; set; }
        public Boolean IsAtLargeHouseState { get; set; }
        public DateTime State_PollClosingTime_DateTime { get; set; }
    }
}
