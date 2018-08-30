using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class RaceBoardModel
    {
        public string state { get; set; }
        public string office { get; set; }
        public string cd { get; set; }
        public string pctsReporting { get; set; }
        public string mode { get; set; }
        public List<candidateData_RB> candData = new List<candidateData_RB>();

    }

    public class candidateData_RB
    {
        public string lastName { get; set; }
        public string votes { get; set; }
        public string percent { get; set; }
        public string party { get; set; }
        public string winner { get; set; }
        public string incumbent { get; set; }
        public string gain { get; set; }
        public string headshot { get; set; }
        public string FoxID { get; set; }
    }
}
