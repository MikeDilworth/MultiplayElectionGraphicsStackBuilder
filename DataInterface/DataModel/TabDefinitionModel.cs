using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    class TabDefinitionModel
    {
        public class TabConfig
        {
            public string tabName;
            public bool showTab;
            public int[] outputEngine;
            public int tabType;
        }

        public class TabOutputDef
        {
            public int[] engine;
            public int[] sceneNo;
            public string[] sceneName;
            public string[] sceneName2;
            public string[] sceneType;
            public string engineSceneDef;

        }
    }
}
