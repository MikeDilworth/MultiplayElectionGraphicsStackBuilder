using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.Enums
{
    using System.ComponentModel;

    public enum StackElementTypes : int
    {
        [Description("Race Board (1-Way)")]
        Race_Board_1_Way = 1,

        [Description("Race Board (1-Way Select)")]
        Race_Board_1_Way_Select = 2,

        [Description("Race Board (2-Way)")]
        Race_Board_2_Way = 3,

        [Description("Race Board (2-Way Select)")]
        Race_Board_2_Way_Select = 4,

        [Description("Race Board (3-Way)")]
        Race_Board_3_Way = 5,

        [Description("Race Board (3-Way Select)")]
        Race_Board_3_Way_Select = 6,

        [Description("Race Board (4-Way)")]
        Race_Board_4_Way = 7,

        [Description("Race Board (4-Way Select)")]
        Race_Board_4_Way_Select = 8,

        [Description("Race Board (5-Way)")]
        Race_Board_5_Way = 9,

        [Description("Race Board (6-Way)")]
        Race_Board_6_Way = 10,

        [Description("Race Board (7-Way)")]
        Race_Board_7_Way = 11,

        [Description("Race Board (8-Way)")]
        Race_Board_8_Way = 12,

        [Description("Race Board (All Candidates)")]
        Race_Board_All_Candidates = 13,

        [Description("Exit Poll (Full-Screen)")]
        Exit_Poll_Full_Screen = 14,
        
        [Description("Balance of Power (House, Current)")]
        Balance_of_Power_House_Current = 15,

        [Description("Balance of Power (Senate, Current)")]
        Balance_of_Power_Senate_Current = 16,
        
        [Description("Balance of Power (House, New)")]
        Balance_of_Power_House_New = 17,
                
        [Description("Balance of Power (Senate, New)")]
        Balance_of_Power_Senate_New = 18,
    
        [Description("Balance of Power (House, Net CHange)")]
        Balance_of_Power_House_Net_Gain = 19,

        [Description("Balance of Power (Senate, Net Change)")]
        Balance_of_Power_Senate_Net_Gain = 20,
        
        [Description("Electoral College")]
        Electoral_College = 21,

        [Description("Balance of Power (House-Senate, Net Change)")]
        BalanceOfPowerHouseSenateNetChange = 22,

        [Description("Referendums")]
        Referendums = 23
    }
}
