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
    /// Class for operations related to top-level MSE Stacks
    /// </summary>
    public class StacksCollection
    {
        #region Logger instantiation - uses reflection to get module name
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties and Members
        public BindingList<StackModel> stacks;
        public string MainDBConnectionString { get; set; }
        #endregion

        #region Public Methods
        // Constructor - instantiates list collection
        public StacksCollection()
        {
            // Create list
            stacks = new BindingList<StackModel>();
        }

        /// <summary>
        /// Get the MSE Stack list from the SQL DB; clears out existing collection first
        /// </summary>
        public BindingList<StackModel> GetStackCollection(string StackNamePrefix)
        {
            DataTable dataTable;

            // Clear out the current collection
            stacks.Clear();

            try
            {
                StackAccess stackAccess = new StackAccess();
                stackAccess.MainDBConnectionString = MainDBConnectionString;
                dataTable = stackAccess.GetStacks();

                foreach (DataRow row in dataTable.Rows)
                {
                    string stackName = row["StackName"].ToString() ?? "";

                    // Check string length to ensure it's a multiplay stack with a netword ID prefix
                    if (stackName.Length >= 4)
                    {

                        var newStack = new StackModel()
                        {
                            ixStackID = Convert.ToDouble(row["ixStackID"] ?? 0),
                            StackName = stackName.Substring(4),
                            //StackName = row["StackName"].ToString() ?? "",
                            StackType = Convert.ToInt16(row["StackType"] ?? -1),
                            ShowName = row["ShowName"].ToString() ?? "",
                            ConceptID = Convert.ToInt16(row["ConceptID"] ?? 0),
                            ConceptName = row["ConceptName"].ToString() ?? "",
                            Notes = row["Notes"].ToString() ?? "",
                        };
                        // Added 02/27/2020 to filter by specified network
                        // Modified for 2020 General Election to only show stacks wuith StackTypeID =- 0 or 8 => Multi-play stacks; other stacks would be Stackmaster
                        if ((row["StackName"].ToString().Length >= 4) &&
                            ((row["StackName"].ToString().Substring(0, 4) ?? "") == (StackNamePrefix + "-")) &&
                            ((newStack.StackType == 0) || (newStack.StackType == 8)))
                        {
                            stacks.Add(newStack);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("StacksCollection Exception occurred: " + ex.Message);
                log.Debug("StacksCollection Exception occurred", ex);
            }

            // Return 
            return stacks;
        }


        /// <summary>
        /// Save the specified stack to the SQL DB; will merge in the metadata values if the stack already exists
        /// </summary>
        public void SaveStack(StackModel stackMetadata)
        {
            try
            {
                StackAccess stackAccess = new StackAccess();
                stackAccess.MainDBConnectionString = MainDBConnectionString;
                stackAccess.SaveStack(stackMetadata);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("StacksCollection Exception occurred: " + ex.Message);
                log.Debug("StacksCollection Exception occurred", ex);
            }
        }

        /// <summary>
        /// Delete the specified stack from the SQL DB; deleting top-level stack cascades to delete stack elements
        /// </summary>
        public void DeleteStack(Double stackID)
        {
            try
            {
                StackAccess stackAccess = new StackAccess();
                stackAccess.MainDBConnectionString = MainDBConnectionString;
                stackAccess.DeleteStack_DB(stackID);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("StacksCollection Exception occurred: " + ex.Message);
                log.Debug("StacksCollection Exception occurred", ex);
            }
        }

        /// <summary>
        /// Get the specified stack from the collection
        /// </summary>
        public StackModel GetStackMetadata(BindingList<StackModel> Stacks, int itemIndex)
        {
            StackModel stack = null;

            try
            {
                stack = Stacks[itemIndex];
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("StacksCollection Exception occurred: " + ex.Message);
                log.Debug("StacksCollection Exception occurred", ex);
            }

            return stack;
        }

        /// <summary>
        /// Check if the stack with the specified stack already exists in the SQL DB (checks by name)
        /// </summary>
        public Double CheckIfStackExists_DB(String stackName)
        {
            Double stackID = -1;
            try
            {
                StackAccess stackAccess = new StackAccess();
                stackAccess.MainDBConnectionString = MainDBConnectionString;
                stackID = stackAccess.CheckIfStackExists_DB(stackName);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("StacksCollection Exception occurred: " + ex.Message);
                log.Debug("StacksCollection Exception occurred", ex);
            }

            return stackID;
        }
        
        #endregion
    }
}
