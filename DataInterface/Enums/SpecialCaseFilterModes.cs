using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.Enums
{
    using System.ComponentModel;

    // Board Modes  0 - Normal, 1 - Race Called, 2 - Just Called, 3 - Too Close To Call, 4 - Runoff, 5 - Race To Watch
    public enum SpecialCaseFilterModes : int
    {
        [Description("No Special Filters")]
        None = 0,

        [Description("Show Races from Battleground States Only")]
        Battleground_States_Only = 1,

        [Description("Show Races from the Next Poll Closing States Only")]
        Next_Poll_Closing_States_Only = 2,

        }
}
