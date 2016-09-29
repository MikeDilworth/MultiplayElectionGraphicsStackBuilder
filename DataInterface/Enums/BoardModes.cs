using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.Enums
{
    using System.ComponentModel;

    // Board Modes  0 - Normal, 1 - Race Called, 2 - Just Called, 3 - Too Close To Call, 4 - Runoff, 5 - Race To Watch
    public enum BoardModes : int
    {
        [Description("Race Board (Normal)")]
        Race_Board_Normal = 0,

        [Description("Race Board (Race Called)")]
        Race_Board_Race_Called = 1,

        [Description("Race Board (Just Called)")]
        Race_Board_Just_Called = 2,

        [Description("Race Board (Too Close to Call)")]
        Race_Board_To_Close_To_Call = 3,

        [Description("Race Board (Runoff)")]
        Race_Board_Runoff = 4,

        [Description("Race Board (Race to Watch)")]
        Race_Board_Race_To_Watch = 5,
    }
}
