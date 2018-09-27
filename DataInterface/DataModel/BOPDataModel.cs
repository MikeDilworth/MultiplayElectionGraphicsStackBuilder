using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class BOPDataModel
    {
        public Int16 BOPType { get; set; }
        public string Branch { get; set; }
        public string Session { get; set; }
        public Int16 Total { get; set; }
        public Int16 DemBaseline { get; set; }
        public Int16 DemCount { get; set; }
        public Int16 DemDelta { get; set; }
        public Int16 RepBaseline { get; set; }
        public Int16 RepCount { get; set; }
        public Int16 RepDelta { get; set; }
        public Int16 IndBaseline { get; set; }
        public Int16 IndCount { get; set; }
        public Int16 IndDelta { get; set; }
    }

}
