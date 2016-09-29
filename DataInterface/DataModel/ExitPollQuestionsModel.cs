using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class ExitPollQuestionsModel
    {
        //Specific to exit polls
        public string mxID { get; set; }
        public string fullMxLabel { get; set; }
        public string shortMxLabel { get; set; }
        public string FNCMxLabel { get; set; }
        public Int16 stateNum { get; set; }
        public string state { get; set; }
        public string stateName { get; set; }
        public string shortName { get; set; }
        public string electionType { get; set; }
        public string office { get; set; }
        public Int16 CD { get; set; }
        public Int16 numRows { get; set; }
        public Int16 numColumns { get; set; }
        public Boolean baseQuestion { get; set; }
        public Boolean rowQuestion { get; set; }
        public Int16 rowNum { get; set; }
        public string rowLabel { get; set; }
        public string rowText { get; set; }
        public string FNCRowQuestion { get; set; }
        public Int32 subsetID { get; set; }
        public string subsetName { get; set; }
        public string suffix { get; set; }
        public string headerText1 { get; set; }
        public string headerText2 { get; set; }
        public string listBoxDescription { get; set; }
        public string questionType { get; set; }
        public string stateOffice { get; set; }
        
    }
}
