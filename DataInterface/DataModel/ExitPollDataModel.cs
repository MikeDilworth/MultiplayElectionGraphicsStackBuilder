using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class ExitPollDataModel
    {
        //Specific to exit polls
        public string mxID { get; set; }
        public string state { get; set; }
        public string stateName { get; set; }
        public string office { get; set; }
        public Int16 CD { get; set; }
        public Int16 pct { get; set; }
        public string header { get; set; }
        public string rowLabel { get; set; }
        public string party { get; set; }
        public Int16 rowNum { get; set; }
        public string Question { get; set; }
        public Int16 subsetID { get; set; }
        public string subsetName { get; set; }
        public string electionType { get; set; }
        public Int16 candID { get; set; }
        public Int16 qseq { get; set; }
        public Int16 xrow { get; set; }
        public Int16 sweep { get; set; }
        
    }
}
