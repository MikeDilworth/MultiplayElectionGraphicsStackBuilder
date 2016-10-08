using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataInterface.DataAccess;
using DataInterface.DataModel;
using DataInterface.Enums;
using System.Reflection;
using MSEInterface;

using LogicLayer.Collections;
using LogicLayer.GeneralDataProcessingFunctions;

namespace GUILayer.Forms
{
    public partial class frmLoadStack : Form
    {
        // Define the collection object for the list of available stacks
        private StacksCollection stacksCollection;
        BindingList<StackModel> stacks;
        
        // Read in database connection strings
        string GraphicsDBConnectionString = Properties.Settings.Default.GraphicsDBConnectionString;
        string ElectionsDBConnectionString = Properties.Settings.Default.ElectionsDBConnectionString;
        
        private Int32 stackIndex;
        public Int32 StackIndex { get { return stackIndex; } set { StackIndex = stackIndex; } }

        private Int32 stackID;
        public Int32 StackID { get { return stackID; } set { StackID = stackID; } }

        private string stackDesc;
        public string StackDesc { get { return stackDesc; } set { StackDesc = stackDesc; } }

        public Int32 StackCollectionCount { get; set; }

        //string topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
        //string currentShowName = Properties.Settings.Default.CurrentShowName;
        //string currentPlaylistName = Properties.Settings.Default.CurrentSelectedPlaylist;
        //string currentShowName = "MAIN_WALL";
        //string currentPlaylistName = "GRAPHICS";

        public frmLoadStack()
        {
            InitializeComponent();

            // Enable handling of function keys
            KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);

            RefreshStacksList();
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
                log.Error("FrmLoadStack Exception occurred during stacks list refresh: " + ex.Message);
                log.Debug("FrmLoadStack Exception occurred during stacks list refresh", ex);
            }
        }
        private void btnLoadStack_Click(object sender, EventArgs e)
        {
            LoadSelectedStack();
        }
        private void availableStacksGrid_DoubleClick(object sender, EventArgs e)
        {
            LoadSelectedStack();
        }
        // Method to load stack
        private void LoadSelectedStack()
        {
            try
            {
                if ((stackIndex >= 0) && (StackCollectionCount > 0))
                {
                    DialogResult result1 =
                        MessageBox.Show(
                            "Are you sure you want to clear the current contents of the stack and load the specified stack from the database?",
                            "Confirmation",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
                // No elements in stack, so don't prompt operator
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();                   
                }

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmLoadStacks Exception occurred: " + ex.Message);
                log.Debug("frmLoadStacks Exception occurred", ex);
            }
        }

        private void btnDeleteStack_Click(object sender, EventArgs e)
        {
            try
            {
                if (stacks.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show("Are you sure you want to delete the selected stack from the database?", "Confirmation",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 != DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmLoadStacks Exception occurred: " + ex.Message);
                log.Debug("frmLoadStacks Exception occurred", ex);
            }
        
        }

        private void availableStacksGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            stackID = Convert.ToInt32(availableStacksGrid[0, stackIndex].Value);
            stackDesc = (string)availableStacksGrid[2, stackIndex].Value;
            
        }

        private void availableStacksGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            stackID = Convert.ToInt32(availableStacksGrid[0, stackIndex].Value);
            stackDesc = (string)availableStacksGrid[2, stackIndex].Value;
            
        }

        private void availableStacksGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            stackID = Convert.ToInt32(availableStacksGrid[0, stackIndex].Value);
            stackDesc = (string)availableStacksGrid[2, stackIndex].Value;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        // Method to handle function keys
        private void KeyEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    if (e.Control == true)
                    {
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                    break;
                case Keys.L:
                    if (e.Control == true)
                        btnLoadStack_Click(sender, e);
                    break;
                case Keys.C:
                    if (e.Control == true)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                    break;
                case Keys.D:
                    if (e.Control == true)
                    {
                        btnDeleteStack_Click(sender, e);
                    }
                    break;
            }
        }
    }
}
