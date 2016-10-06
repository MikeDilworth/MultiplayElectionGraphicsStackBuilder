using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataInterface.DataModel;
using DataInterface.DataAccess;
using LogicLayer.Collections;

namespace GUILayer.Forms
{
    public partial class FrmSaveStack : Form
    {

        // Define the collection object for the list of available stacks
        private StacksCollection stacksCollection;
        BindingList<StackModel> stacks;

        
        // Read in database connection strings
        string GraphicsDBConnectionString = Properties.Settings.Default.GraphicsDBConnectionString;
        
        private Int32 stackId;
        public Int32 StackId { get; set; }

        //private string stackDescription;
        //public string StackDescription { get { return stackDescription; } set { StackDescription = stackDescription; } }
        public string StackDescription { get; set; }

        public FrmSaveStack(int stID, string stackDesc)
        {
            InitializeComponent();
            RefreshStacksList();

            txtStackID.Text = Convert.ToString(stID);
            txtStackDescription.Text = stackDesc;
            txtStackID.Focus();
        }
        #region Logger instantiation - uses reflection to get module name
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Logging & status setup
        // This method used to implement IAppender interface from log4net; to support custom appends to status strip
        public void DoAppend(log4net.Core.LoggingEvent loggingEvent)
        {
            // Set text on status bar only if logging level is DEBUG or ERROR
            if ((loggingEvent.Level.Name == "ERROR") | (loggingEvent.Level.Name == "DEBUG"))
            {
                //toolStripStatusLabel.BackColor = System.Drawing.Color.Red;
                //toolStripStatusLabel.Text = String.Format("Error Logging Message: {0}: {1}", loggingEvent.Level.Name, loggingEvent.MessageObject.ToString());
            }
            else
            {
                //toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
                //toolStripStatusLabel.Text = String.Format("Status Logging Message: {0}: {1}", loggingEvent.Level.Name, loggingEvent.MessageObject.ToString());
            }
        }

        // Handler to clear status bar message and reset color
        private void resetStatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
            //toolStripStatusLabel.Text = "Status Logging Message: Statusbar reset";
        }
        #endregion

        private void RefreshStacksList()
        {
            try
            {
                // Setup the available stacks collection
                this.stacksCollection = new StacksCollection();
                this.stacksCollection.MainDBConnectionString = GraphicsDBConnectionString;
                stacks = this.stacksCollection.GetStackCollection();

                // Setup the available stacks grid
                availableStacksGrid.AutoGenerateColumns = false;
                var availableStacksGridDataSource = new BindingSource(stacks, null);
                availableStacksGrid.DataSource = availableStacksGridDataSource;
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("FrmSaveStack Exception occurred during stacks list refresh: " + ex.Message);
                log.Debug("FrmSaveStack Exception occurred during stacks list refresh", ex);
            }
        }

        private void availableStacksGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            
            int stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            txtStackID.Text = Convert.ToString(availableStacksGrid[0, stackIndex].Value);
            txtStackDescription.Text = (string)availableStacksGrid[2, stackIndex].Value;
        }

        private void availableStacksGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            int stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            txtStackID.Text = Convert.ToString(availableStacksGrid[0, stackIndex].Value);
            txtStackDescription.Text = (string)availableStacksGrid[2, stackIndex].Value;
            
        }

        private void btnLoadStack_Click(object sender, EventArgs e)
        {
            //Will be replaced on save
            // check if txtStackID has a valid number entered
            bool inputOk = Int32.TryParse(txtStackID.Text, out stackId);
            if (inputOk == false)
                stackId = 0;
 
            // Check to see that a non-zero ID was specified if not in default mode; if so, alert operator and dump out
            if (stackId == 0)
            {
                MessageBox.Show("If the playlist is not being saved as the default stack, a non-zero Playlist ID must be specified.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check to see that a stack description was specified if not in default mode; if so, alert operator and dump out
            if ((txtStackDescription.Text.Trim() == string.Empty))
            {
                MessageBox.Show("If the playlist is not being saved as the default stack, a Playlist Description must be specified.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
                //Check if playlist already exists in database; if prompt for overwrite not checked, skip step
                try
                {
                    Boolean stackExists = stacksCollection.CheckIfStackExists_DB(stackId);
                    if (stackExists)
                    {
                        // Check to see if the sublist already exists in the database; if so, prompt for overwrite
                        DialogResult result1 =
                            MessageBox.Show(
                                "A playlist with the specified ID already exists in the database. Do you wish to proceed and overwrite it?",
                                "Confirmation",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 == DialogResult.Yes)
                        {
                            StackId = stackId;
                            StackDescription = txtStackDescription.Text.Trim();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                        }
                    }
                    else
                    {
                        StackId = stackId;
                        StackDescription = txtStackDescription.Text.Trim();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    // Log error
                    log.Error("frmMain Exception occurred: " + ex.Message);
                    log.Debug("frmMain Exception occurred", ex);
                }                        
        }

        private void txtStackID_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
