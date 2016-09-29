using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface.DataModel
{
    // Class used for storing strings required for a single element/page in an MSE group
    public class RacePreviewModel
    {
        public string Raceboard_Description { get; set; }
        public string Raceboard_Preview_Field_Text { get; set; }
        public string Raceboard_Type_Field_Text { get; set; }
    }
}
