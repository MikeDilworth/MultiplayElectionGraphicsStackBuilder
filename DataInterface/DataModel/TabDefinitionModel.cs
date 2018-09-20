using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public  class TabDefinitionModel
    {
            public string tabName { get; set; }
            public bool showTab { get; set; }
            public bool[] outputEngine = new bool[4];
            public string engineSceneDef { get; set; }

            public List<TabOutputDef> TabOutput = new List<TabOutputDef>();
        
        
    }
    public class TabOutputDef
    {
        public int engine { get; set; }
        public string sceneCode { get; set; }
        public string sceneName { get; set; }
    }
}
