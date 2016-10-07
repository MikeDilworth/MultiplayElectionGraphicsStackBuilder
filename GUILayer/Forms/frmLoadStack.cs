using System;
using System.ComponentModel;
using System.Windows.Forms;
using DataInterface.DataModel;
using LogicLayer.Collections;

namespace GUILayer.Forms
{
    public partial class FrmLoadStack : Form
    {
        // Define the collection object for the list of available stacks
        private StacksCollection _stacksCollection;
        private BindingList<StackModel> _stacks;

        
        // Read in database connection strings
        private string GraphicsDBConnectionString = Properties.Settings.Default.GraphicsDBConnectionString;
        private string _electionsDbConnectionString = Properties.Settings.Default.ElectionsDBConnectionString;
        
        private Int32 _stackIndex;
        public Int32 StackIndex { get { return _stackIndex; } set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("value");
            _stackIndex = value;
            StackIndex = _stackIndex;
        } }

        private Int32 _stackId;
        public Int32 StackId { get { return _stackId; } set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("value");
            _stackIndex = value;
            StackId = _stackId;
        } }

        private string _stackDesc;
        public string StackDesc { get { return _stackDesc; } set
        {
            if (value == null) throw new ArgumentNullException("value");
            GraphicsDBConnectionString = value;
            StackDesc = _stackDesc;
        } }

        public Int32 StackCollectionCount { get; set; }

        //string topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
        //string currentShowName = Properties.Settings.Default.CurrentShowName;
        //string currentPlaylistName = Properties.Settings.Default.CurrentSelectedPlaylist;

        public FrmLoadStack()
        {
            InitializeComponent();

            RefreshStacksList();

            // Enable handling of function keys
            KeyPreview = true;
            KeyUp += KeyEvent;
        }

        #region Logger instantiation - uses reflection to get module name
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        #endregion
        
        private void RefreshStacksList()
        {
            //try
            {
                // Setup the available stacks collection
                _stacksCollection = new StacksCollection {MainDBConnectionString = GraphicsDBConnectionString};
                _stacks = _stacksCollection.GetStackCollection();

                // Setup the available stacks grid
                availableStacksGrid.AutoGenerateColumns = false;
                var availableStacksGridDataSource = new BindingSource(_stacks, null);
                availableStacksGrid.DataSource = availableStacksGridDataSource;
            }
            //catch (Exception ex)
            //{
                // Log error
            //    log.Error("FrmLoadStack Exception occurred during stacks list refresh: " + ex.Message);
            //    log.Debug("FrmLoadStack Exception occurred during stacks list refresh", ex);
            //}
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
                if ((_stackIndex >= 0) && (StackCollectionCount > 0))
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
                    DialogResult = DialogResult.OK;
                    Close();                   
                }

            }
            catch (Exception ex)
            {
                // Log error
                Log.Error("frmLoadStacks Exception occurred: " + ex.Message);
                Log.Debug("frmLoadStacks Exception occurred", ex);
            }
        }

        // Handler for Delete Stack button
        private void btnDeleteStack_Click(object sender, EventArgs e)
        {
            try
            {
                if (_stacks.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show("Are you sure you want to delete the selected stack from the database?", "Confirmation",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 != DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Abort;
                        Close();
                    }

                }
            }
            catch (Exception ex)
            {
                // Log error
                Log.Error("frmLoadStacks Exception occurred: " + ex.Message);
                Log.Debug("frmLoadStacks Exception occurred", ex);
            }
        
        }

        // Handler for Cancel Stack Load button
        private void btnCancelStackLoad_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Handler for Activate Stack button
        private void btnActivateStack_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        // Handler for click on grid - change currently selected stack
        private void availableStacksGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            _stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            _stackId = Convert.ToInt32(availableStacksGrid[0, _stackIndex].Value);
            _stackDesc = (string)availableStacksGrid[2, _stackIndex].Value;            
        }

        // Handler for double-click on grid - change currently selected stack
        private void availableStacksGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            _stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            _stackId = Convert.ToInt32(availableStacksGrid[0, _stackIndex].Value);
            _stackDesc = (string)availableStacksGrid[2, _stackIndex].Value;            
        }

        // Handler for click on grid - change currently selected stack
        private void availableStacksGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Get the selected stack list object
            _stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            _stackId = Convert.ToInt32(availableStacksGrid[0, _stackIndex].Value);
            _stackDesc = (string)availableStacksGrid[2, _stackIndex].Value;        
        }

        // Handler for current cell change
        private void availableStacksGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            //Get the selected stack list object
            _stackIndex = availableStacksGrid.CurrentCell.RowIndex;
            _stackId = Convert.ToInt32(availableStacksGrid[0, _stackIndex].Value);
            _stackDesc = (string)availableStacksGrid[2, _stackIndex].Value;        
        }

        // Method to handle function keys
        private void KeyEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.L:
                    if (e.Control)
                        btnLoadStack_Click(sender, e);
                    break;
                case Keys.A:
                    if (e.Control)
                        btnActivateStack_Click(sender, e);
                    break;
                case Keys.D:
                    if (e.Control)
                        btnDeleteStack_Click(sender, e);
                    break;
                case Keys.C:
                    if (e.Control)
                        btnCancelStackLoad_Click(sender, e);
                    break;
            }
        }
    }
}
