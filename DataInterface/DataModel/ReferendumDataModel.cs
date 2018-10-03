using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class ReferendumDataModel
    {
        public string StateName { get; set; }
        public string MainTitle { get; set; }
        public string Detailtext { get; set; }
        public string Description { get; set; }
        public string PropRefID { get; set; }
        public Int32 VoteCount { get; set; }
        public Int32 TotalVotes { get; set; }
        public Int32 VotePct { get; set; }
        public string CandLastName { get; set; }
        public string cStat { get; set; }
        public Int32 PrecinctsPercent { get; set; }
        public Int32 PrecinctsReporting { get; set; }
        public Int32 TotalPrecincts { get; set; }
        public Boolean WinnerCalled { get; set; }
        public Int32 WinnerCandidateID { get; set; }

    }




}
