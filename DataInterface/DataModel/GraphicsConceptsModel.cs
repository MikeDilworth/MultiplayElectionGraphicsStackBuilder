using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    public class GraphicsConceptsModel
    {
        public Int16 ConceptID { get; set; }
        public string ConceptName { get; set; }
        public Int16 ElementTypeCode { get; set; }
        public string ElementTypeDescription { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public Boolean IsBaseConcept { get; set; }
        public Boolean AllowConceptChange { get; set; }
    }

}