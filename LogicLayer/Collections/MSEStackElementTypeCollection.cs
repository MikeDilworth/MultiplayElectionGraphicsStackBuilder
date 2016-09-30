using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInterface.DataModel;
using DataInterface.DataAccess;
using System.Data.SqlClient;
using System.ComponentModel;


namespace LogicLayer.Collections
{
    /// <summary>
    /// Class for creating stack element types collection
    /// </summary>
    public class MSEStackElementTypeCollection
    {
        #region Logger instantiation - uses reflection to get module name
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties and Members
        public BindingList<MSEStackElementTypeModel> stackElementTypes;
        public string MainDBConnectionString { get; set; }
        #endregion

        #region Public Methods
        // Constructor - instantiates list collection
        public MSEStackElementTypeCollection()
        {
            // Create list
            stackElementTypes = new BindingList<MSEStackElementTypeModel>();
        }

        /// <summary>
        /// Get the MSE Stack Element Type list from the SQL DB; clears out existing collection first
        /// </summary>
        public BindingList<MSEStackElementTypeModel> GetMSEStackElementTypeCollection()
        {
            DataTable dataTable;

            // Clear out the current collection
            stackElementTypes.Clear();

            try
            {
                MSEStackElementTypeAccess stackElementTypeAccess = new MSEStackElementTypeAccess();
                stackElementTypeAccess.MainDBConnectionString = MainDBConnectionString;
                dataTable = stackElementTypeAccess.GetMSEStackElementTypes();

                foreach (DataRow row in dataTable.Rows)
                {
                    var newStackElementType = new MSEStackElementTypeModel()
                    {
                        MSE_Stack_Element_Type = Convert.ToInt16(row["MSE_Stack_Element_Type"] ?? 0),
                        MSE_Stack_Element_Type_Description = row["MSE_Stack_Element_Type_Description"].ToString() ?? "",
                        Element_Type_Scene_Path = row["Element_Type_Scene_Path"].ToString() ?? "",
                        Element_Type_Template_ID = row["Element_Type_Template_ID"].ToString() ?? "",
                    };
                    stackElementTypes.Add(newStackElementType);
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("MSEStackElementCollection Exception occurred: " + ex.Message);
                log.Debug("MSEStackElementCollection Exception occurred", ex);
            }
            
            // Return 
            return stackElementTypes;
        }

        /// <summary>
        /// Get the MSE Stack element type metadata for the specified type ID code
        /// </summary>
        public MSEStackElementTypeModel GetMSEStackElementType(BindingList<MSEStackElementTypeModel> MSEStackElementTypes, Int16 mseStackElementTypeValue)
        {
            MSEStackElementTypeModel mseStackElementType = null;

            try
            {
                // Find matching entry
                for (int i = 0; i < MSEStackElementTypes.Count; ++i)
                {
                    if (mseStackElementTypeValue == MSEStackElementTypes[i].MSE_Stack_Element_Type)
                    {
                        mseStackElementType = MSEStackElementTypes[i];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("MSEStackElementCollection Exception occurred: " + ex.Message);
                log.Debug("MSEStackElementCollection Exception occurred", ex);
            }

            return mseStackElementType;
        }
        #endregion
    }
}
