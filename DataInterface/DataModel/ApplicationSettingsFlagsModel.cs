using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class ApplicationSettingsFlagsModel
    {
        public Boolean PollClosingLockoutEnable { get; set; }
        public Boolean ZeroPrecinctsLockoutEnable { get; set; }
        public float ZeroPrecinctsLockoutThreshold { get; set; }
        public Boolean UseExpectedVoteIn { get; set; }
        public Boolean UseSimulatedElectionDayTime { get; set; }
        public Boolean UseAPCallsForTier2HouseRaces { get; set; }
        public Boolean UseTenthsOfPercent { get; set; }
        public Boolean OptionalFlag1 { get; set; }
        public Boolean OptionalFlag2 { get; set; }
        public Boolean OptionalFlag3 { get; set; }
        public Boolean OptionalFlag4 { get; set; }
        public Boolean OptionalFlag5 { get; set; }
        public float OptionalParameter1 { get; set; }
        public float OptionalParameter2 { get; set; }
        public float OptionalParameter3 { get; set; }
        public float OptionalParameter4 { get; set; }
        public float OptionalParameter5 { get; set; }
    }
}
