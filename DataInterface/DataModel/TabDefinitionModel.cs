using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public  class TabDefinitionModel
    {
            public static string tabName { get; set; }
            public static bool showTab { get; set; }
            public static bool[] outputEngine = new bool[4];
            public static string engineSceneDef { get; set; }
            public static List<TabOutputDef> TabOutput = new List<TabOutputDef>();
        
        public class TabOutputDef
        {
            public int engine { get; set; }
            public string sceneCode { get; set; }
            public string sceneName { get; set; }
        }
    }
}
