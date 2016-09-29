using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    // Class definition for stack element types
    public class MSEStackElementTypeModel
    {
        public Int16 MSE_Stack_Element_Type { get; set; }
        public string MSE_Stack_Element_Type_Description { get; set; }
        public string Element_Type_Scene_Path { get; set; }
        public string Element_Type_Template_ID { get; set; }
    }
}
