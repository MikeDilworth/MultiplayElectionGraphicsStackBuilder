using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.Enums
{
    using System.ComponentModel;

    // Board Modes  0 - Normal, 1 - Race Called, 2 - Just Called, 3 - Too Close To Call, 4 - Runoff, 5 - Race To Watch
    public enum GraphicsConcepts : int
    {
        [Description("32x9")]
        ThirtyTwo_By_Nine = 1,

        [Description("TopOfHour")]
        Top_Of_Hour = 2,

        [Description("16x9)")]
        Sixteen_By_Nine = 3,

        [Description("6-Way)")]
        Six_Way = 4,

        [Description("8-Way)")]
        Eight_Way = 5,

    }
}