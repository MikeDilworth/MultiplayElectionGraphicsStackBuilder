using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class ReferendumModel
    {
      public Int16 race_ID { get; set; }
      public string race_ElectionType { get; set; }
      public string race_Description { get; set; }
      public Int16 state_ID { get; set; }
      public string state_Abbv { get; set; }
      public string state_Name { get; set; }
      public Int16 race_District { get; set; }
      public string race_OfficeCode { get; set; }
      public Boolean race_WinnerCalled { get; set; }
      public Int32 race_WinnerCandidateID { get; set; }
    }
}
