//////////////////////////////////////////////////////////////////////////////
// MAIN APPLICATION FORM
// Version 1.0.0
// M. Dilworth  Rev: 08/17/2016
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using DataInterface.DataModel;
using DataInterface.Enums;
using log4net.Appender;
using LogicLayer.Collections;
using LogicLayer.CommonClasses;
using MSEInterface;
using MSEInterface.Constants;
// Required for implementing logging to status bar

//
// NOTES:
//
// 1. Code modified 8/17/2016 to use new approach provided by Viz for managing groups and elements within groups.
//

namespace GUILayer.Forms
{
    // Implement IAppender interface from log4net
    public partial class frmMain : Form, IAppender
    {

        #region Globals
        Boolean nonNumberEntered = false;
        public static Boolean UseSimulatedTime = false;
        DateTime referenceTime = DateTime.MaxValue;
        #endregion

        #region Collection & binding list definitions
        /// <summary>
        /// Define classes for collections and logic
        /// </summary>

        // Define the binding list object for the list of available shows
        //BindingList<ShowObject> showNames;

        // Define the collection object for the list of available stacks
        private StacksCollection stacksCollection;
        BindingList<StackModel> stacks;

        // Define the collection object for the elements within a specified stack
        public StackElementsCollection stackElementsCollection;
        BindingList<StackElementModel> stackElements;

        // Define the collection object for the list of available races
        private AvailableRacesCollection availableRacesCollection;
        BindingList<AvailableRaceModel> availableRaces;

        // Define the collection object for the list of Referendums
        private ReferendumsCollection  referendumsCollection;
        BindingList<ReferendumModel> referendums;

        // Define the collection object for the list of Referendums data
        private ReferendumsDataCollection referendumsDataCollection;
        BindingList<ReferendumDataModel> referendumsData;

        // Define the collection object for the list of Exit Poll questions
        private ExitPollQuestionsCollection exitPollQuestionsCollection;
        BindingList<ExitPollQuestionsModel> exitPollQuestions;

        // Define the collection object for the list of Exit Poll questions
        private ExitPollDataCollection exitPollDataCollection;
        //BindingList<ExitPollDataCollection> exitPollData;

        // Define the collection for MSE stack element data types
        private MSEStackElementTypeCollection mseStackElementTypeCollection;
        BindingList<MSEStackElementTypeModel> mseStackElementTypes;

        // Define the collection used for storing candidate data for a specific race
        private RaceDataCollection raceDataCollection;
        BindingList<RaceDataModel> raceData;

        // Define the collection used for storing race preview data for multi-play
        private RacePreviewCollection racePreviewCollection;
        BindingList<RacePreviewModel> racePreview;

        // Define the collection used for storing state metadata for all 50 states + US
        private StateMetadataCollection stateMetadataCollection;
        BindingList<StateMetadataModel> stateMetadata;

        // Define the collection used for storing state metadata for all 50 states + US
        private GraphicsConceptsCollection  graphicsConceptsCollection;
        BindingList<GraphicsConceptsModel> graphicsConcepts;
        BindingList<GraphicsConceptsModel> graphicsConceptTypes;

        // Define the collection object for the list of available races
        private ApplicationSettingsFlagsCollection applicationSettingsFlagsCollection;
        BindingList<ApplicationSettingsFlagsModel> applicationFlags;
        
        internal static readonly XNamespace Atom = "http://www.w3.org/2005/Atom";

        // Instantiate MSE classes
        MANAGE_GROUPS group = new MANAGE_GROUPS();
        MANAGE_PLAYLISTS playlist = new MANAGE_PLAYLISTS();
        MANAGE_TEMPLATES template = new MANAGE_TEMPLATES();
        MANAGE_SHOWS show = new MANAGE_SHOWS();
        MANAGE_ELEMENTS element = new MANAGE_ELEMENTS();

        // Read in MSE settings from config file and set default directories and parameters
        Boolean mseEndpoint1_Enable = Properties.Settings.Default.MSEEndpoint1_Enable;
        string mseEndpoint1 = Properties.Settings.Default.MSEEndpoint1;
        Boolean mseEndpoint2_Enable = Properties.Settings.Default.MSEEndpoint2_Enable;
        string mseEndpoint2 = Properties.Settings.Default.MSEEndpoint1;
        string topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
        string masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.MasterPlaylistsDirectory;
        string profilesURI = Properties.Settings.Default.MSEEndpoint1 + "profiles";
        string currentShowName = Properties.Settings.Default.CurrentShowName;
        string currentPlaylistName = Properties.Settings.Default.CurrentSelectedPlaylist;

        // Read in database connection strings
        string GraphicsDBConnectionString = Properties.Settings.Default.GraphicsDBConnectionString;
        string ElectionsDBConnectionString = Properties.Settings.Default.ElectionsDBConnectionString;

        //Read in default Trio profile and channel
        string defaultTrioProfile = Properties.Settings.Default.DefaultTrioProfile;
        string defaultTrioChannel = Properties.Settings.Default.DefaultTrioChannel;

        string elementCollectionURIShow;
        string templateCollectionURIShow;
        string elementCollectionURIPlaylist;
        string templateModel;

        Int32 stackID;
        string stackDescription;

        Int16 ConceptID;
        string ConceptName;
        string TemplateName;

        private Boolean manualEPQuestions = false;
                        
        #endregion

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
                toolStripStatusLabel.BackColor = System.Drawing.Color.Red;
                toolStripStatusLabel.Text = String.Format("Error Logging Message: {0}: {1}", loggingEvent.Level.Name, loggingEvent.MessageObject.ToString());
            }
            else
            {
                toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
                toolStripStatusLabel.Text = String.Format("Status Logging Message: {0}: {1}", loggingEvent.Level.Name, loggingEvent.MessageObject.ToString());
            }
        }

        // Handler to clear status bar message and reset color
        private void resetStatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
            toolStripStatusLabel.Text = "Status Logging Message: Statusbar reset";
        }
        #endregion

        #region Main form init, activation & close

        /// <summary>
        /// Main form init, activation and close
        /// </summary>
        
        public frmMain()
        {
            InitializeComponent();

            try
            {
                // Set the default to show all for race filters, Auto for exit pool questions
                rbShowAll.BackColor = Color.Gold;
                rbShowAll.Checked = true;
                rbAll.BackColor = Color.Gold;
                rbAll.Checked = true;

                rbEPAuto.Checked = true;
                rbEPAuto.BackColor = Color.Gold;
                rbNone.Checked = true;
                rbNone.BackColor = Color.Gold;

                // Update status
                toolStripStatusLabel.Text = "Starting program initialization - loading data from SQL database.";

                // Query the graphics DB to get the list of already saved stacks
                RefreshStacksList();

                // Read in the list of available MSE stack element types
                RefreshMSEStackElementTypesList();

                // Query the elections DB to get the list of exit poll questions
                RefreshExitPollQuestions();

                // Query the elections DB to get the list of available races
                RefreshAvailableRacesList();

                //Query the elections DB to get the list of Referendums
                RefreshReferendums();

                // Query the elections DB to get application flags(option settings)
                RefreshApplicationFlags();

                // Init the stack elements collection
                CreateStackElementsCollection();

                // Init the race data collection
                CreateRaceDataCollection();

                // Init the state metadata collection
                CreateStateMetadataCollection();

                // Init the race preview collection
                CreateRacePreviewCollection();

                // Init Graphics Concepts Collection
                CreateGraphicsConceptsCollection();

                // Set current show label
                lblCurrentShow.Text = currentShowName;

                // Load the BOP Grid
                LoadBOPDGV();

                // Enable handling of function keys
                KeyPreview = true;
                this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);

                timerStatusUpdate.Enabled = true;

                TimeFunctions.ElectionsDBConnectionString = ElectionsDBConnectionString;
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred during program init: " + ex.Message);
                log.Debug("frmMain Exception occurred during program init", ex);
            }

            // Update status labels
            toolStripStatusLabel.Text = "Program initialization complete.";
            lblPlaylistName.Text = currentPlaylistName;
            lblTrioChannel.Text = defaultTrioChannel;
        }

        // Handler for main form load
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Log application start
            log.Info("Starting Stack Builder application");
        }

        // Handler for main menu program exit button click
        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Method for main form close - confirm with operator
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Are you sure you want to exit the Election Graphics Stack Builder Application?", "Warning",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        // Handler for main form closed - log info
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Log application quit
            log.Info("Quitting Stack Builder application");
        }
        #endregion

        #region General Form related methods
        // General update timer
        private void timerStatusUpdate_Tick(object sender, EventArgs e)
        {
            if (ApplicationSettingsFlagsCollection.UseSimulatedTime == true)
            {
                referenceTime = TimeFunctions.GetSimulatedTime();
                gbTime.Text = @"SIMULATED TIME";
            }
            else
            {
                referenceTime = DateTime.Now;
                gbTime.Text = @"ACTUAL TIME";
            }

            //label1.Text = Convert.ToString(referenceTime);
            label1.Text = String.Format("{0:h:mm:ss tt  MMM dd, yyyy}", referenceTime);

        }

        // Handler for change to main data mode select tab control
        private void dataModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataModeSelect.SelectedIndex == 0)
            {
                gbRCF.Visible = true;
                gbROF.Visible = true;
            }
            else
            {
                gbRCF.Visible = false;
                gbROF.Visible = false;
            }

            if (dataModeSelect.SelectedIndex == 1)
                gbExitPolls.Visible = true;
            else
                gbExitPolls.Visible = false;
        }

        #endregion 
        
        #region Utility functions
        // Refresh the list of available stacks for the grid

        private void RefreshStacksList()
        {
            try
            {
                // Setup the available stacks collection
                this.stacksCollection = new StacksCollection();
                this.stacksCollection.MainDBConnectionString = GraphicsDBConnectionString;
                stacks = this.stacksCollection.GetStackCollection();
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred during stacks list refresh: " + ex.Message);
                log.Debug("frmMain Exception occurred during stacks list refresh", ex);
            }
        }
        #endregion

        # region Data refresh functions
        // Read in the list of available MSE stack element types
        private void RefreshMSEStackElementTypesList()
        {
            try
            {
                this.mseStackElementTypeCollection = new MSEStackElementTypeCollection();
                this.mseStackElementTypeCollection.MainDBConnectionString = GraphicsDBConnectionString;
                mseStackElementTypes = this.mseStackElementTypeCollection.GetMSEStackElementTypeCollection();
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Refresh the list of available races for the races list
        private void RefreshAvailableRacesList()
        {
            try
            {
                // Setup the available races collection
                this.availableRacesCollection = new AvailableRacesCollection();
                this.availableRacesCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                availableRaces = this.availableRacesCollection.GetAvailableRaceCollection();

                // Setup the available races grid
                availableRacesGrid.AutoGenerateColumns = false;
                var availableRacesGridDataSource = new BindingSource(availableRaces, null);
                availableRacesGrid.DataSource = availableRacesGridDataSource;
                lblAvailRaceCnt.Text = "Available Races: " + Convert.ToString(availableRaces.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }
        // Refresh the Application settings from the Flags DB
        private void RefreshApplicationFlags()
        {
            try
            {
                // Setup the available races collection
                this.applicationSettingsFlagsCollection = new ApplicationSettingsFlagsCollection();
                this.applicationSettingsFlagsCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                applicationFlags = this.applicationSettingsFlagsCollection.GetFlagsCollection();

                ApplicationSettingsFlagsModel flags = null;
                flags = applicationFlags[0];
                UseSimulatedTime = flags.UseSimulatedElectionDayTime;
            }

            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Refresh the list of available races for the races list
        private void RefreshAvailableRacesListFiltered(string ofc, Int16 cStatus, Int16 scfm, BindingList<StateMetadataModel> stateMetadata)
        {
            try
            {
                // Setup the available races collection
                this.availableRacesCollection = new AvailableRacesCollection();
                this.availableRacesCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                availableRaces = this.availableRacesCollection.GetFilteredRaceCollection(ofc, cStatus, scfm, stateMetadata);

                // Setup the available races grid
                availableRacesGrid.AutoGenerateColumns = false;
                var availableRacesGridDataSource = new BindingSource(availableRaces, null);
                availableRacesGrid.DataSource = availableRacesGridDataSource;
                lblAvailRaceCnt.Text = "Available Races: " + Convert.ToString(availableRaces.Count);

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }
        // Refresh the list of exit polls for the list
        private void RefreshExitPollQuestions()
        {
            try
            {
                // Setup the exit polls collection
                this.exitPollQuestionsCollection = new ExitPollQuestionsCollection();
                this.exitPollQuestionsCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                exitPollQuestions = this.exitPollQuestionsCollection.GetExitPollQuestionsCollection(manualEPQuestions);

                // Setup the available exit polls grid
                availableExitPollsGrid.AutoGenerateColumns = false;
                var availableExitPollsGridDataSource = new BindingSource(exitPollQuestions, null);
                availableExitPollsGrid.DataSource = availableExitPollsGridDataSource;
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Refresh the list of referendums for the list
        private void RefreshReferendums()
        {
            try
            {
                // Setup the referendums collection
                this.referendumsCollection = new ReferendumsCollection();
                this.referendumsCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                referendums = this.referendumsCollection.GetReferendumsCollection();

                // Setup the available exit polls grid
                ReferendumsGrid.AutoGenerateColumns = false;
                var ReferendumsGridDataSource = new BindingSource(referendums, null);
                ReferendumsGrid.DataSource = ReferendumsGridDataSource;
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        #endregion

        #region Race data & preview functions
        /// <summary>
        /// RACE DATA & PREVIEW FUNCTIONS
        /// </summary>
        // Define the collection used for storing candidate data for a specific race
        private void CreateRaceDataCollection()
        {
            try
            {
                this.raceDataCollection = new RaceDataCollection();
                this.raceDataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                // Specify state ID = -1 => Don't query database for candidate data until requesting actual race data
                raceData = this.raceDataCollection.GetRaceDataCollection(-1, "P", 0, "G", 1);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Define the collection used for storing data strings to be sent to multi-play for preview purposes
        private void CreateRacePreviewCollection()
        {
            try
            {
                this.racePreviewCollection = new RacePreviewCollection();
                racePreview = racePreviewCollection.GetRacepPreviewCollection();
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Create the state metadata collection
        private void CreateStateMetadataCollection()
        {
            try
            {
                // Setup the master race collection & bind to grid
                this.stateMetadataCollection = new StateMetadataCollection();
                this.stateMetadataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                stateMetadata = this.stateMetadataCollection.GetStateMetadataCollection();
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Create the state metadata collection
        private void CreateGraphicsConceptsCollection()
        {
            try
            {
                // Setup the master race collection & bind to grid
                this.graphicsConceptsCollection = new GraphicsConceptsCollection();
                this.graphicsConceptsCollection.GraphicsDBConnectionString = GraphicsDBConnectionString;
                graphicsConceptTypes = this.graphicsConceptsCollection.GetGraphicsConceptTypesCollection();

                for (Int16 i = 0; i < graphicsConceptTypes.Count; i++)
                    cbGraphicConcept.Items.Add(graphicsConceptTypes[i].ConceptName);

                ConceptID = graphicsConceptTypes[0].ConceptID;
                ConceptName = graphicsConceptTypes[0].ConceptName;
                cbGraphicConcept.SelectedIndex = 0;
                cbGraphicConcept.Text = ConceptName;
                
                // Setup the master race collection & bind to grid
                //this.graphicsConceptsCollection = new GraphicsConceptsCollection();
                //this.graphicsConceptsCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                graphicsConcepts = this.graphicsConceptsCollection.GetGraphicsConceptsCollection();

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }
        #endregion

        #region Stack (group) creation & management functions
        /// <summary>
        /// STACK CREATION & MANAGEMENT FUNCTIONS
        /// </summary>
        // Function create the initial playlist elements collection for editing
        private void CreateStackElementsCollection()
        {
            try
            {
                // Setup the master race collection & bind to grid
                this.stackElementsCollection = new StackElementsCollection();
                this.stackElementsCollection.MainDBConnectionString = GraphicsDBConnectionString;
                stackElements = this.stackElementsCollection.GetStackElementsCollection(-1);

                // Setup the stacks grid
                stackGrid.AutoGenerateColumns = false;
                var stackGridDataSource = new BindingSource(stackElements, null);
                stackGrid.DataSource = stackGridDataSource;
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }
        #endregion

        #region General dialogs
        /// <summary>
        /// GENERAL DIALOGS
        /// </summary>
        // Launch About box
        private void miAboutBox_Click(object sender, EventArgs e)
        {
            try
            {
                Forms.frmAbout aboutBox = new Forms.frmAbout();

                // Show the dialog
                aboutBox.ShowDialog();
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Handler from main menu to launch show selection dialog
        private void miSelectDefaultShow_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            frmSelectShow selectShow = new frmSelectShow();
            dr = selectShow.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // Set the new show
                currentShowName = selectShow.selectedShow;
                lblCurrentShow.Text = currentShowName;

                // Write the new default show out to the config file
                Properties.Settings.Default.CurrentShowName = currentShowName;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region Stack operations (add, etc.)
        /// <summary>
        /// STACK OPERATIONS
        /// </summary>
        // Handler for delete stack button
        private void btnDeleteStack_Click(object sender, EventArgs e)
        {

        }

        // Handler for Add 1-Way race board button
        private void btnAddRace1Way_Click(object sender, EventArgs e)
        {
            Int16 seType = (short)StackElementTypes.Race_Board_1_Way;
            string seDescription = "Race Board (1-Way)";
            AddRaceBoardToStack(seType, seDescription);
        }

        // Handler for Add 2-Way race board button
        private void btnAddRace2Way_Click(object sender, EventArgs e)
        {
            // Call method to add 2-way board
            Add2WayBoard();
        }
        // Double-click on availble races grid defaults to 2-way board
        private void availableRacesGrid_DoubleClick(object sender, EventArgs e)
        {
            // Call method to add 2-way board
            Add2WayBoard();
        }
        // General method to add a 2-way race board
        private void Add2WayBoard()
        {
            Int16 seType = (short)StackElementTypes.Race_Board_2_Way;
            string seDescription = "Race Board (2-Way)";
            AddRaceBoardToStack(seType, seDescription);
        }

        // Handler for Add 3-Way race board button
        private void btnAddRace3Way_Click(object sender, EventArgs e)
        {
            Int16 seType = (short)StackElementTypes.Race_Board_3_Way;
            string seDescription = "Race Board (3-Way)";
            AddRaceBoardToStack(seType, seDescription);
        }

        private void btnAddRace4Way_Click(object sender, EventArgs e)
        {
            Int16 seType = (short)StackElementTypes.Race_Board_4_Way;
            string seDescription = "Race Board (4-Way)";
            AddRaceBoardToStack(seType, seDescription);    
        }

        // Generic method to add a race board to a stack
        private void AddRaceBoardToStack(Int16 stackElementType, string stackElementDescription)
        {
            try
            {
                // Instantiate new stack element model
                StackElementModel newStackElement = new StackElementModel();

                //Get the selected race list object
                int currentRaceIndex = availableRacesGrid.CurrentCell.RowIndex;
                AvailableRaceModel selectedRace = availableRacesCollection.GetRace(availableRaces, currentRaceIndex);

                Int32 stackID = 0;
                newStackElement.fkey_StackID = stackID;
                newStackElement.Stack_Element_ID = stackElements.Count;
                newStackElement.Stack_Element_Type = stackElementType;
                newStackElement.Stack_Element_Description = stackElementDescription;
                
                // Get the template ID for the specified element type
                newStackElement.Stack_Element_TemplateID = mseStackElementTypeCollection.GetMSEStackElementType(mseStackElementTypes, (short)StackElementTypes.Race_Board_3_Way).Element_Type_Template_ID;

                newStackElement.Election_Type = selectedRace.Election_Type;
                newStackElement.Office_Code = selectedRace.Race_Office;
                newStackElement.State_Number = selectedRace.State_Number;
                newStackElement.State_Mnemonic = selectedRace.State_Mnemonic;
                newStackElement.State_Name = selectedRace.State_Name;
                newStackElement.CD = selectedRace.CD;
                newStackElement.County_Number = 0;
                newStackElement.County_Name = "N/A";
                newStackElement.Listbox_Description = selectedRace.Race_Description;

                // Specific to race boards
                newStackElement.Race_ID = selectedRace.Race_ID;
                newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = selectedRace.Race_Office;
                newStackElement.Race_District = selectedRace.CD;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = selectedRace.Race_PollClosingTime;
                newStackElement.Race_UseAPRaceCall = selectedRace.Race_UseAPRaceCall;

                //Specific to exit polls - set to default values
                newStackElement.ExitPoll_mxID = 0;
                newStackElement.ExitPoll_BoardID = 0;
                newStackElement.ExitPoll_ShortMxLabel = string.Empty;
                newStackElement.ExitPoll_NumRows = 0;
                newStackElement.ExitPoll_xRow = 0;
                newStackElement.ExitPoll_BaseQuestion = false;
                newStackElement.ExitPoll_RowQuestion = false;
                newStackElement.ExitPoll_Subtitle = string.Empty;
                newStackElement.ExitPoll_Suffix = string.Empty;
                newStackElement.ExitPoll_HeaderText_1 = string.Empty;
                newStackElement.ExitPoll_HeaderText_2 = string.Empty;
                newStackElement.ExitPoll_SubsetName = string.Empty;
                newStackElement.ExitPoll_SubsetID = 0;

                // Add element
                stackElementsCollection.AppendStackElement(newStackElement);
                // Update stack entries count label
                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("Exception occurred while trying to add board to stack: " + ex.Message);
                log.Debug("Exception occurred while trying to add board to stack", ex);
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            Int16 seType = (short)StackElementTypes.Race_Board_2_Way;
            string seDescription = "Race Board (2-Way)";
            Int16 i = 0;
            foreach (DataGridViewRow rowNum in availableRacesGrid.Rows)
            {
                availableRacesGrid.CurrentCell = availableRacesGrid.Rows[i].Cells[0];
                AddRaceBoardToStack(seType, seDescription);
                i++;
            }
        }

        #endregion

        #region UI widget data validation methods
        /// <summary>
        /// UI widget data validation methods
        /// </summary>
        // Handler for key down event for Stack ID text box (to limit valid keys to number keys)
        private void txtStackID_KeyDown(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard. 
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad. 
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace. 
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed. 
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }
            //If shift key was pressed, it's not a number. 
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }
        }

        // Handler for key press event for Stack ID text box
        private void txtStackID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event. 
            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        // Method to handle function keys for race boards
        private void KeyEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    rbPresident.Checked = true;
                    break;
                case Keys.F2:
                    rbSenate.Checked = true;
                    break;
                case Keys.F3:
                    rbHouse.Checked = true;
                    break;
                case Keys.F4:
                    rbGovernor.Checked = true;
                    break;
                case Keys.F5:
                    rbShowAll.Checked = true;
                    break;
                case Keys.F6:
                    rbTCTC.Checked = true;
                    break;
                case Keys.F7:
                    rbJustCalled.Checked = true;
                    break;
                case Keys.F8:
                    rbCalled.Checked = true;
                    break;
                case Keys.F9:
                    rbAll.Checked = true;
                    break;
                case Keys.F10:
                    rbBattleground.Checked = true;
                    break;
                case Keys.F11:
                    rbPollClosing.Checked = true;
                    break;
                case Keys.A:
                    if (e.Control == true)
                        btnAddAll_Click(sender, e);
                    break;
                default:
                    rbShowAll.Checked = true;
                    break;
            }
        }


        #endregion

        #region Stack manipulation methods
        /// <summary>
        /// Stack manipulation methods
        /// </summary>
        // Handler for Save Stack button
        private void btnSaveStack_Click(object sender, EventArgs e)
        {
            try
            {
                if (stackElements.Count > 0)
                {
                    
                    DialogResult dr = new DialogResult();
                    FrmSaveStack saveStack = new FrmSaveStack(stackID ,stackDescription);
                    dr = saveStack.ShowDialog();
                    if (dr == DialogResult.OK)
                    {

                        // Instantiate a new top-level stack metadata model
                        StackModel stackMetadata = new StackModel();

                        stackID = saveStack.StackId;
                        stackDescription = saveStack.StackDescription;
                        stackMetadata.ixStackID = stackID;
                        stackMetadata.StackName = stackDescription;
                        
                        stackMetadata.StackType = 0;
                        stackMetadata.ShowName = currentShowName;
                        stackMetadata.ConceptID = ConceptID;
                        stackMetadata.ConceptName = ConceptName;
                        stackMetadata.Notes = "Not currently used";
                        stacksCollection.SaveStack(stackMetadata);

                        // Save out stack elements; specify stack ID, and set flag to delete existing elements before adding
                        stackElementsCollection.SaveStackElementsCollection(stackMetadata.ixStackID, true);


                        // Update stack entries count label & name label
                        txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                        txtStack.Text = stackDescription;
                    }
                }

                // Set status strip
                toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
                //toolStripStatusLabel.Text = "Status Logging Message: Stack successfully saved out to database";
                toolStripStatusLabel.Text = String.Format("Status Logging Message: Stack {0} saved out to database", stackID);
            

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Handler for move stack element up in stack order
        private void btnStackElementUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (stackElementsCollection.CollectionCount > 0)
                {
                    int currentRowIndex = stackGrid.CurrentRow.Index;
                    stackElementsCollection.MoveStackElementUp((short)stackGrid.CurrentRow.Index);
                    if (stackGrid.CurrentRow.Index > 0)
                    {
                        stackGrid.Rows[currentRowIndex - 1].Selected = true;
                        stackGrid.CurrentCell = stackGrid[0, currentRowIndex - 1];
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Handler for move stack element down in stack order
        private void btnStackElementDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (stackElementsCollection.CollectionCount > 0)
                {
                    stackElementsCollection.MoveStackElementDown((short)stackGrid.CurrentRow.Index);
                    if (stackGrid.CurrentRow.Index < stackGrid.RowCount - 1)
                    {
                        stackGrid.Rows[stackGrid.CurrentRow.Index + 1].Selected = true;
                        stackGrid.CurrentCell = stackGrid[0, stackGrid.CurrentRow.Index + 1];
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Handler for Clear Stack button
        private void btnClearStack_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (stackElements.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show("Are you sure you want to clear all entries from the stack?", "Confirmation",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 != DialogResult.Yes)
                    {
                        return;
                    }

                    // Operator didn't dump out, so proceed
                    if (stackGrid.RowCount > 0)
                    {
                        //Clear the collection
                        stackElements.Clear();
                    }
                    // Update stack entries count label
                    txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Handler for delete stack element button
        private void btnDeleteStackElement_Click(object sender, EventArgs e)
        {
            try
            {
                if (stackGrid.RowCount > 0)
                {
                    //Get the delete point
                    int currentStackIndex = stackGrid.CurrentCell.RowIndex;

                    //Delete the item from the collection
                    stackElements.RemoveAt(currentStackIndex);
                }
                // Update stack entries count label
                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Handler for Load Stack button
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
                
                //Refresh the list of available stacks
                RefreshStacksList();
                Int32 stackIndex = 0;
                
                DialogResult dr = new DialogResult();
                //frmCandidateSelect selectCand = new frmCandidateSelect();
                frmLoadStack selectStack = new frmLoadStack();
                dr = selectStack.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    // Set candidateID's
                    stackIndex = selectStack.StackIndex;
                    stackID = selectStack.StackID;
                    stackDescription = selectStack.StackDesc;

                    // Clear the collection
                    stackElements.Clear();

                    // Get the stack ID and load the selected collection
                    //t currentStackIndex = availableStacksGrid.CurrentCell.RowIndex;
                    int currentStackIndex = stackIndex;
                    StackModel selectedStack = stacksCollection.GetStackMetadata(stacks, currentStackIndex);
                    cbGraphicConcept.SelectedIndex = selectedStack.ConceptID - 1;

                    // Load the collection
                    stackElementsCollection.GetStackElementsCollection(selectedStack.ixStackID);
                    // Update stack entries count label
                    txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                    txtStack.Text = selectedStack.ixStackID  + " " + selectedStack.StackName;
                }
                else if (dr == DialogResult.Abort)
                {
                    stackIndex = selectStack.StackIndex;
                    if (stacks.Count > 0)
                    {
                       
                        // Operator didn't cancel out, so delete the stack
                        int currentStackIndex = stackIndex;

                        // Delete from database
                        StackModel selectedStack = stacksCollection.GetStackMetadata(stacks, currentStackIndex);
                        stacksCollection.DeleteStack(selectedStack.ixStackID);

                        // Delete from MSE
                        string groupSelfLink = string.Empty;

                        // Get playlists directory URI based on current show
                        string showPlaylistsDirectoryURI = show.GetPlaylistDirectoryFromShow(topLevelShowsDirectoryURI + "/", currentShowName);

                        // Check for a playlist (group) in the VDOM with the specified name & return the Alt link
                        // Delete the group so it can be re-created
                        string playlistDownLink = playlist.GetPlaylistDownLink(showPlaylistsDirectoryURI, currentPlaylistName);
                        if (playlistDownLink != string.Empty)
                        {
                            // Get the self link to the specified group
                            groupSelfLink = group.GetGroupSelfLink(playlistDownLink, selectedStack.StackName);

                            // Delete the group if it exists
                            if (groupSelfLink != string.Empty)
                            {
                                group.DeleteGroup(groupSelfLink);
                            }
                        }

                        //Refresh the list of available stacks
                        RefreshStacksList();
                    }
                }
                else if (dr == DialogResult.Yes)
                {
                    // Set candidateID's
                    stackIndex = selectStack.StackIndex;
                    Int32 selStackID = selectStack.StackID;
                    string selStackDesc = selectStack.StackDesc;

                    // Clear the collection
                    //stackElements.Clear();

                    // Get the stack ID and load the selected collection
                    int currentStackIndex = stackIndex;
                    StackModel selectedStack = stacksCollection.GetStackMetadata(stacks, currentStackIndex);

                    ActivateStack(selStackID, selStackDesc);
                    
                }

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }



        private void btnActivateStack_Click(object sender, EventArgs e)
        {
            try
            {
                //Int32 stackID = 0;
                //string stackDesc;
                if (stackElements.Count > 0)
                {
                    
                    DialogResult dr = new DialogResult();
                    FrmSaveStack saveStack = new FrmSaveStack(stackID, stackDescription);
                    dr = saveStack.ShowDialog();
                    if (dr == DialogResult.OK)
                    {

                        // Instantiate a new top-level stack metadata model
                        StackModel stackMetadata = new StackModel();

                        stackID = saveStack.StackId;
                        stackDescription  = saveStack.StackDescription;
                        stackMetadata.ixStackID = stackID;
                        stackMetadata.StackName = stackDescription;
                        
                        stackMetadata.StackType = 0;
                        stackMetadata.ShowName = currentShowName;
                        stackMetadata.ConceptID = ConceptID;
                        stackMetadata.ConceptName = ConceptName;
                        stackMetadata.Notes = "Not currently used";
                        stacksCollection.SaveStack(stackMetadata);

                        // Save out stack elements; specify stack ID, and set flag to delete existing elements before adding
                        stackElementsCollection.SaveStackElementsCollection(stackMetadata.ixStackID, true);


                        // Update stack entries count label & name label
                        txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                        txtStack.Text = stackID + " " + stackDescription;
                
                        ActivateStack(stackID, stackDescription);
                    }
                }

                // Set status strip
                toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
            //    toolStripStatusLabel.Text = "Status Logging Message: Stack saved out to database and activated";
                toolStripStatusLabel.Text = String.Format("Status Logging Message: Stack {0} saved out to database and activated", stackID);
            }
                    
            catch (Exception ex)
            {
                log.Debug("Exception occurred", ex);
                log.Error("Exception occurred while trying to save and activate group: " + ex.Message);
            }
        }
        /// <summary>
        /// Mathod to Activate Stack
        /// </summary>
        private void ActivateStack(int stack_ID, string stack_Description)
        {
            try
            {
                    
                    // MSE OPERATION
                    string groupSelfLink = string.Empty;

                    // Get playlists directory URI based on current show
                    string showPlaylistsDirectoryURI = show.GetPlaylistDirectoryFromShow(topLevelShowsDirectoryURI, currentShowName);

                    // Set the top-level group metadata to be saved out to the DB
                    // Instantiate a new top-level stack metadata model
                    StackModel stackMetadata = new StackModel();

                    // Set the top-level stack metadata & save out the stack to the DB; data will be updated if stack (group) already exists
                    stackMetadata.ixStackID = stack_ID;
                    stackMetadata.StackName = stack_Description;
                    stackMetadata.StackType = 0;
                    stackMetadata.ShowName = currentShowName;
                    stackMetadata.ConceptID = ConceptID;
                    stackMetadata.ConceptName = ConceptName;
                    stackMetadata.Notes = "Not currently used";
                    stacksCollection.SaveStack(stackMetadata);

                    // Iterate through the races in the stack to build the preview collection, then call methods to create group containing elements
                    // Clear out the existing race preview collection
                    racePreview.Clear();

                    string raceBoardTypeDescription = string.Empty;
                    Int16 candidatesToReturn = 0;
                    Int16 dataType = 0;
                    string BOPHeader = String.Empty;
                    
                    // Build the Race Preview collection - contains strings for each race in the group/stack
                    // Iterate through each race in the stack to build the race preview command strings collection
                    
                    for (int i = 0; i < stackElements.Count; ++i)
                    {

                
                        switch (stackElements[i].Stack_Element_Type)
                        {
                            case (Int16)StackElementTypes.Race_Board_1_Way:
                                raceBoardTypeDescription = "1-Way Board";                                                                                                                                  
                                candidatesToReturn = 1;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_1_Way_Select:
                                raceBoardTypeDescription = "1-Way Select Board";
                                candidatesToReturn = 1;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_2_Way:
                                raceBoardTypeDescription = "2-Way Board";
                                candidatesToReturn = 2;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_2_Way_Select:
                                raceBoardTypeDescription = "2-Way Select Board";
                                candidatesToReturn = 2;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_3_Way:
                                raceBoardTypeDescription = "3-Way Board";
                                candidatesToReturn = 3;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_3_Way_Select:
                                raceBoardTypeDescription = "3-Way Select Board";
                                candidatesToReturn = 3;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_4_Way:
                                raceBoardTypeDescription = "4-Way Board";
                                candidatesToReturn = 4;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Race_Board_4_Way_Select:
                                raceBoardTypeDescription = "4-Way Select Board";
                                candidatesToReturn = 4;
                                dataType = (Int16) DataTypes.Race_Boards;
                                break;

                            case (Int16)StackElementTypes.Exit_Poll_Full_Screen:
                                raceBoardTypeDescription = "Exit Polls";
                                dataType = (Int16)DataTypes.Exit_Polls;
                                break;

                            case (Int16)StackElementTypes.Balance_of_Power_House_Current:
                                raceBoardTypeDescription = "Balance of Power - House: Current";
                                BOPHeader = "HOUSE^CURRENT";
                                dataType = (Int16)DataTypes.Balance_of_Power;
                                break;

                            case (Int16)StackElementTypes.Balance_of_Power_Senate_Current:
                                raceBoardTypeDescription = "Balance of Power - Senate: Current";
                                BOPHeader = "SENATE^CURRENT";
                                dataType = (Int16)DataTypes.Balance_of_Power;
                                break;

                            case (Int16)StackElementTypes.Balance_of_Power_House_New:
                                raceBoardTypeDescription = "Balance of Power - House: New";
                                BOPHeader = "HOUSE^NEW";
                                dataType = (Int16)DataTypes.Balance_of_Power;
                                break;

                            case (Int16)StackElementTypes.Balance_of_Power_Senate_New:
                                raceBoardTypeDescription = "Balance of Power - Senate: New";
                                BOPHeader = "SENATE^NEW";
                                dataType = (Int16)DataTypes.Balance_of_Power;
                                break;

                            case (Int16)StackElementTypes.Balance_of_Power_House_Net_Gain:
                                raceBoardTypeDescription = "Balance of Power - House: Net Gain";
                                BOPHeader = "HOUSE^NET GAIN";
                                dataType = (Int16)DataTypes.Balance_of_Power;
                                break;

                            case (Int16)StackElementTypes.Balance_of_Power_Senate_Net_Gain:
                                raceBoardTypeDescription = "Balance of Power - Senate: Net Gain";
                                BOPHeader = "SENATE^NET GAIN";
                                dataType = (Int16)DataTypes.Balance_of_Power;
                                break;

                            case (Int16)StackElementTypes.Referendums:
                                raceBoardTypeDescription = "Referendums";
                                dataType = (Int16)DataTypes.Referendums;
                                break;

                        }

                        // Instantiate and set the values of a race preview element
                        RacePreviewModel newRacePreviewElement = new RacePreviewModel();

                        switch (dataType)
                        {
                            
                            case (Int16)DataTypes.Race_Boards:

                                // Request the race data for the element in the stack - updates raceData binding list
                                GetRaceData(stackElements[i].State_Number, stackElements[i].Race_Office, stackElements[i].CD, stackElements[i].Election_Type, candidatesToReturn);

                                // Check for data returned for race
                                if (raceData.Count > 0)
                                {
                                    // Instantiate and set the values of a race preview element
                                    //RacePreviewModel newRacePreviewElement = new RacePreviewModel();

                                    // Set the name of the element for the group
                                    newRacePreviewElement.Raceboard_Description = stackElements[i].Listbox_Description + " - " + raceBoardTypeDescription;
                                    //newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + stackElements[i].Listbox_Description;

                                    // Set FIELD_TYPE value - stack ID plus stack index
                                    newRacePreviewElement.Raceboard_Type_Field_Text = stackMetadata.ixStackID.ToString() + "|" + i.ToString();

                                    // Call method to assemble the race data into the required command string for the raceboards scene
                                    newRacePreviewElement.Raceboard_Preview_Field_Text = GetRacePreviewString(stackElements[i], candidatesToReturn);

                                    // Append the preview element to the race preview collection
                                    racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                }
                                break;

                            case (Int16)DataTypes.Exit_Polls:
                                
                                string epType = stackElements[i].Race_RecordType[0].ToString();
                                Int32 epID = stackElements[i].ExitPoll_mxID;
                                string st = stackElements[i].State_Mnemonic;
                                string ofc = stackElements[i].Office_Code;
                                Int32 jCde = stackElements[i].County_Number;
                                Int16 rowNum = stackElements[i].ExitPoll_xRow;
                                Int32 subID = stackElements[i].ExitPoll_SubsetID;
                                string eType = stackElements[i].Election_Type;
                                string q = stackElements[i].ExitPoll_ShortMxLabel;

                                StackElementModel stElement = new StackElementModel();
                                stElement = stackElements[i];

                                // Setup the referendums collection
                                var records = ExitPollDataCollection.GetExitPollDataCollection(ElectionsDBConnectionString, epType, epID, st, ofc, (short)jCde, rowNum, (short)subID, eType);

                                // Check for data returned for race
                                if (records.Count > 0)
                                {

                                    // Set the name of the element for the group
                                    newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + stackElements[i].Listbox_Description;

                                    // Set FIELD_TYPE value - stack ID plus stack index
                                    newRacePreviewElement.Raceboard_Type_Field_Text = stackMetadata.ixStackID.ToString() + "|" + i.ToString();

                                    // Call method to assemble the race data into the required command string for the raceboards scene
                                    newRacePreviewElement.Raceboard_Preview_Field_Text = GetExitPollPreviewString(records, stElement);

                                    // Append the preview element to the race preview collection
                                    racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                }
                                break;

                            case (Int16)DataTypes.Balance_of_Power:

                                
                                // Set the name of the element for the group
                                newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + stackElements[i].Listbox_Description;

                                // Set FIELD_TYPE value - stack ID plus stack index
                                newRacePreviewElement.Raceboard_Type_Field_Text = stackMetadata.ixStackID.ToString() + "|" + i;

                                // Call method to assemble the race data into the required command string for the raceboards scene
                                newRacePreviewElement.Raceboard_Preview_Field_Text = GetBOPPreviewString(stackElements[i], BOPHeader);
                                
                                // Append the preview element to the race preview collection
                                racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                
                                break;

                            case (Int16)DataTypes.Referendums:

                                // Setup the referendums collection
                                this.referendumsDataCollection  = new ReferendumsDataCollection();
                                this.referendumsDataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                                referendumsData = referendumsDataCollection.GetReferendumsDataCollection(stackElements[i].State_Number, stackElements[i].Race_Office);

                                ReferendumDataModel refData = new ReferendumDataModel();

                                // Check for data returned for race
                                if (referendumsData.Count > 0)
                                {

                                    refData = referendumsData[1];

                                    // Set the name of the element for the group
                                    newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + stackElements[i].Listbox_Description;

                                    // Set FIELD_TYPE value - stack ID plus stack index
                                    newRacePreviewElement.Raceboard_Type_Field_Text = stackMetadata.ixStackID.ToString() + "|" + i.ToString();

                                    // Call method to assemble the race data into the required command string for the raceboards scene
                                    newRacePreviewElement.Raceboard_Preview_Field_Text = GetReferendumPreviewString(refData);

                                    // Append the preview element to the race preview collection
                                    racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                }
                                break;
                        }

                    }

                    // MSE OPERATION - SAVE OUT THE GROUP W/STACK ELEMENTS
                    // Get playlists directory URI based on current show
                    showPlaylistsDirectoryURI = show.GetPlaylistDirectoryFromShow(topLevelShowsDirectoryURI, currentShowName);

                    // Get templates directory URI based on current show
                    string showTemplatesDirectoryURI = show.GetTemplateCollectionFromShow(topLevelShowsDirectoryURI, currentShowName);

                    // Check for a playlist in the VDOM with the specified name & return the Alt link; if the playlist doesn't exist, create it first
                    if (playlist.CheckIfPlaylistExists(showPlaylistsDirectoryURI, currentPlaylistName) == false)
                    {
                        playlist.CreatePlaylist(showPlaylistsDirectoryURI, currentPlaylistName);
                    }

                    // Check for a playlist in the VDOM with the specified name & return the Alt link
                    // Delete the group so it can be re-created
                    string playlistDownLink = playlist.GetPlaylistDownLink(showPlaylistsDirectoryURI, currentPlaylistName);
                    if (playlistDownLink != string.Empty)
                    {
                        // Get the self link to the specified group
                        groupSelfLink = group.GetGroupSelfLink(playlistDownLink, stackDescription);

                        // Delete the group if it exists
                        if (groupSelfLink != string.Empty)
                        {
                            group.DeleteGroup(groupSelfLink);
                        }

                        // Create the group
                        REST_RESPONSE restResponse = group.CreateGroup(playlistDownLink, stackDescription);

                        // Check for elements in collection and add to group
                        if (racePreview.Count > 0)
                        {
                            // Iterate through each element in the preview collection and add the element to the group
                            for (int i = 0; i < racePreview.Count; ++i)
                            {
                                // Get the element from the collection
                                RacePreviewModel racePreviewElement;
                                racePreviewElement = racePreview[i];

                                // Add the element to the group
                                //Get the info for the current race
                                StackElementModel selectedStackElement = stackElementsCollection.GetStackElement(stackElements, i);

                                //Set template ID
                                string templateID = selectedStackElement.Stack_Element_TemplateID;

                                //Set page number
                                string pageNumber = i.ToString();

                                //Gets the URI's for the given show
                                GET_URI getURI = new GET_URI();

                                //Get the show info
                                //Get the URI to the show elements collection
                                elementCollectionURIShow = show.GetElementCollectionFromShow(topLevelShowsDirectoryURI, currentShowName);

                                //Get the URI to the show templates collection
                                templateCollectionURIShow = show.GetTemplateCollectionFromShow(topLevelShowsDirectoryURI, currentShowName);

                                //Get the URI to the model for the specified template within the specified show
                                templateModel = template.GetTemplateElementModel(templateCollectionURIShow, templateID);

                                // Alert if template model not found
                                if (templateModel == null)
                                {
                                    // Log error
                                    log.Error("Could not resolve template model - template might not exist");
                                    log.Debug("Could not resolve template model - template might not exist");
                                }

                                //Get the URI to the currently-specified playlist                                
                                elementCollectionURIPlaylist = restResponse.downLink;

                                // Set the data values as name/value pairs
                                // Get the element from the collection
                                racePreviewElement = racePreview[i];

                                // Add the element to the group
                                // NOTE: Currently hard-wired for race boards - will need to be extended to support varying data types
                                Dictionary<string, string> nameValuePairs =
                                    new Dictionary<string, string> { { TemplateFieldNames.RaceBoard_Template_Preview_Field, racePreviewElement.Raceboard_Preview_Field_Text }, 
                                                                         { TemplateFieldNames.RaceBoard_Template_Type_Field, stackID.ToString() + "|" + pageNumber } };

                                // Instance the element management class
                                MANAGE_ELEMENTS element = new MANAGE_ELEMENTS();

                                // Create the new element
                                element.createNewElement(i.ToString() + ": " + racePreviewElement.Raceboard_Description, elementCollectionURIPlaylist, templateModel, nameValuePairs, defaultTrioChannel);
                            }
                        }
                    }

                    // SQL DB OPERATION - SAVE OUT THE STACK ELEMENTS
                    // Save out the stack elements out to the database; specify stack ID, and set flag to delete existing elements before adding
                    stackElementsCollection.SaveStackElementsCollection(stackMetadata.ixStackID, true);

                    // Cleanup once stack is saved out - refresh list and clear UI widgets
                    // Refresh the list of available stacks
                    //RefreshStacksList();

                    // Clear out the stack save text controls
                    //stackID = 0;
                    //stackDescription = string.Empty;

                    // Update stack entries count label
                    txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                }
            
            catch (Exception ex)
            {
                log.Debug("Exception occurred", ex);
                log.Error("Exception occurred while trying to save and activate group: " + ex.Message);
            }
        }
        #endregion

        #region Race data acquisition methods and Preview
        /// <summary>
        /// Race data acquisition methods
        /// </summary>
        // Method to get the race data for a specified race
        private BindingList<RaceDataModel> GetRaceData(Int16 stateNumber, string raceOffice, Int16 cd, string electionType, Int16 candidatesToReturn)
        {
            // Setup the master race collection & bind to grid
            //this.raceDataCollection = new RaceDataCollection();
            //this.raceDataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
            raceData = this.raceDataCollection.GetRaceDataCollection(stateNumber, raceOffice, cd, electionType, candidatesToReturn);

            return raceData;
        }

        // Method to return metadata for a specific state
        private StateMetadataModel GetStateMetadata(Int16 stateID)
        {
            StateMetadataModel selectedStateMetadata = null;
            if (stateMetadata.Count > 0)
            {
                selectedStateMetadata = stateMetadataCollection.GetStateMetadata(stateMetadata, stateID);
            }
            return selectedStateMetadata;
        }

        // Method to get the RACE_PREVIEW string for a specified race - references raceData binding list
        private string GetRacePreviewString(StackElementModel stackElement, Int16 candidatesToReturn)
        {
            // FoxIDs for all candidates separated by | then
            // ~ Board Mode ~ State Name ^ House CD # or County Name ^ Precincts Reporting ^ Race Description ^ 0
            // Then for each candidate:
            // Name ^ Party ID ^ Incumbent Status ^ Vote Count ^ Percent ^ Winner Status ^ Gain Status ^ Headshot Path
            // Separated by |
            // Board Modes  0 - Normal, 1 - Race Called, 2 - Just Called, 3 - Too Close To Call, 4 - Runoff, 55 - Race To Watch

            // Init
            //string previewField = string.Empty;
            string previewField = "!"; // Bang

            try
            {
                if (raceData.Count >= candidatesToReturn)
                {
                    // Build string of Fox IDs
                    for (int i = 0; i < candidatesToReturn; ++i)
                    {
                        if (i != 0)
                        {
                            previewField = previewField + "|";
                        }
                        previewField = previewField + raceData[i].FoxID;
                    }

                    //Ex: USGOV001769|USGOV000540~0~WISCONSIN^^^DEMOCRATIC PRIMARY^0~HILLARY CRINTON^1^0^ ^ %^0^0^O:\\business\\ObjectStore\\2016\\Q1\\clinton_hillary_official.png|BERNIE SANDERS^1^0^ ^ %^0^0^O:\\business\\ObjectStore\\2015\\Q3\\Sanders_Bernie_IVT_Sen.png
                    // Add race metadata
                    previewField = previewField + "~";
                    previewField = previewField + (Int16)BoardModes.Race_Board_Normal;
                    previewField = previewField + "~";
                    previewField = previewField + stackElement.State_Name.ToUpper().Trim();
                    previewField = previewField + "^^^";
                    // Add race descriptor
                    //Dem primary
                    if (stackElement.Election_Type == "D")
                    {
                        previewField = previewField + "DEMOCRATIC PRIMARY";
                    }
                    //Rep primary
                    else if (stackElement.Election_Type == "R")
                    {
                        previewField = previewField + "REPUBLICAN PRIMARY";
                    }
                    //Dem caucuses
                    else if (stackElement.Election_Type == "E")
                    {
                        previewField = previewField + "DEMOCRATIC CAUCUSES";
                    }
                    //Rep caucuses
                    else if (stackElement.Election_Type == "S")
                    {
                        previewField = previewField + "DEMOCRATIC CAUCUSES";
                    }
                    // Not a primary or caucus event - build string based on office type
                    else
                    {
                        if (stackElement.Race_Office == "P")
                        {
                            previewField = previewField + "President";
                        }
                        else if (stackElement.Race_Office == "G")
                        {
                            previewField = previewField + "Governor";
                        }
                        else if (stackElement.Race_Office == "L")
                        {
                            previewField = previewField + "Lt. Governor";
                        }
                        else if ((stackElement.Race_Office == "S") | (stackElement.Race_Office == "S2"))
                        {
                            previewField = previewField + "Senate";
                        }
                        else if (stackElement.Race_Office == "H")
                        {
                            if (GetStateMetadata(stackElement.State_Number).IsAtLargeHouseState)
                            {
                                previewField = previewField + "House At Large";
                            }
                            else
                            {
                                previewField = previewField + "U.S. House CD " + stackElement.Race_District.ToString();
                            }
                        }
                    }
                    previewField = previewField + "^0~";

                    // Add candidate data - only include name and headshot (no data for preview)
                    for (int i = 0; i < candidatesToReturn; ++i)
                    {
                        if (i != 0)
                        {
                            previewField = previewField + "|";
                        }
                        previewField = previewField + raceData[i].CandidateLastName.Trim();
                        previewField = previewField + "^";
                        // Add party ID
                        if (raceData[i].CandidatePartyID.ToUpper() == "REP")
                        {
                            previewField = previewField + "0";
                        }
                        else if (raceData[i].CandidatePartyID.ToUpper() == "DEM")
                        {
                            previewField = previewField + "1";
                        }
                        else
                        {
                            previewField = previewField + "2";
                        }
                        // Add blank fields for data
                        previewField = previewField + "^0^ ^ ^0^0^";
                        // Add headshot path
                        previewField = previewField + raceData[i].HeadshotPathFNC.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

            return previewField;
        }



        // Method to get the Balance of Power preview string
        private string GetBOPPreviewString(StackElementModel stackElement, string Header)
        {
            
            // Init
            string previewField = "!"; // Bang

            try
            {
            
                // ex HOUSE^CURRENT| ^ ^ | ^ ^ | ^ ^ |
                previewField = previewField + Header;
                previewField = previewField + "| ^ ^ | ^ ^ | ^ ^ |";

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

            return previewField;
        }

        // Method to get the Referendum string 
        private string GetReferendumPreviewString(ReferendumDataModel referendumData)
        {

            // Init
            string previewField = "!"; // Bang

            try
            {

                // ex State|Proposition|Proposition Name|Proposition Description|Checkstate|Yes Votes|Yes Info|No Votes|No Info|
                previewField = previewField + referendumData.StateName + "|"; 
                previewField = previewField + referendumData.PropRefID + "|"; 
                previewField = previewField + referendumData.Description + "|";
                previewField = previewField + referendumData.Detailtext + "|";
                previewField = previewField +  "0|"; //Checkstate
                previewField = previewField + " |"; //Yes_Num
                previewField = previewField + " |"; //Yes_Info
                previewField = previewField + " |"; //No_Num
                previewField = previewField + " |"; //Mo_Info
                
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

            return previewField;
        }

        // Method to get the Referendum string 
        private string GetExitPollPreviewString(BindingList<ExitPollDataModel> exitPollData, StackElementModel stackElement)
        {

            
            Int32 numResp = exitPollData.Count;
            
            // Init
            string previewField = "!"; // Bang
            string Title = "EXIT POLL";
            if (stackElement.ExitPoll_ShortMxLabel[0] == 'N')
                Title = "ENTRANCE POLL";
            if (stackElement.Election_Type == "E" || stackElement.Election_Type == "S")
                Title = "ENTRANCE POLL";
            string plusMinus = String.Empty;
            if (stackElement.ExitPoll_ShortMxLabel[0] == 'S')
                plusMinus = "+";
            if (stackElement.ExitPoll_ShortMxLabel[0] == 'W')
                plusMinus = "-";
            string Question = exitPollData[0].Question;
            if (stackElement.Election_Type[0].ToString() == "M")
                Question = stackElement.ExitPoll_ShortMxLabel;

            try
            {
                // ex # of Repomses|Title|Question|State|Response 1^Response2[^Response3][^Response4]|+/- Percent1^+/- Percent2[^+/- Percent3][^+/- Percent4]|
                previewField = previewField + numResp + "|";
                previewField = previewField + Title + "|";
                previewField = previewField + Question + "|";
                previewField = previewField + stackElement.State_Mnemonic + "|";

                for (Int16 i = 0; i < numResp; i++)
                {
                    if (i > 0)
                        previewField = previewField + "^";
                    previewField = previewField + exitPollData[i].rowLabel;
                }

                previewField = previewField + "||";

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

            return previewField;
        }


        #endregion

        #region Balance of power methods
        /// <summary>
        /// Methods to add selected Balance of Power to stack
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            AddBOP();
        }

        private void BOPdataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddBOP();
        }

        // Loads the BOPdataGridView
        private void LoadBOPDGV()
        {
            try
            {
                BOPtype bopType = new BOPtype();
                for (int i = 1; i <= 6; i++)
                {
                    switch (i)
                    {
                        case 1:
                            bopType.eType = (short) StackElementTypes.Balance_of_Power_House_Current;
                            bopType.branch = "HOUSE";
                            bopType.session = "CURRENT";
                            break;

                        case 2:
                            bopType.eType = (short)StackElementTypes.Balance_of_Power_Senate_Current;
                            bopType.branch = "SENATE";
                            bopType.session = "CURRENT";
                            break;

                        case 3:
                            bopType.eType = (short)StackElementTypes.Balance_of_Power_House_New;
                            bopType.branch = "HOUSE";
                            bopType.session = "NEW";
                            break;

                        case 4:
                            bopType.eType = (short)StackElementTypes.Balance_of_Power_Senate_New;
                            bopType.branch = "SENATE";
                            bopType.session = "NEW";
                            break;

                        case 5:
                            bopType.eType = (short)StackElementTypes.Balance_of_Power_House_Net_Gain;
                            bopType.branch = "HOUSE";
                            bopType.session = "NET GAIN";
                            break;

                        case 6:
                            bopType.eType = (short)StackElementTypes.Balance_of_Power_Senate_Net_Gain;
                            bopType.branch = "SENATE";
                            bopType.session = "NET GAIN";
                            break;
                    }

                    BOPdataGridView.Rows.Add(bopType.eType, bopType.branch, bopType.session);
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // General method to add Balance Of Power to the stack
        private void AddBOP()
        {
            try
            {
                // Instantiate new stack element model
                StackElementModel newStackElement = new StackElementModel();

                //Get the selected BOP grid object
                int currentBOPIndex = BOPdataGridView.CurrentCell.RowIndex;

                //Get the selected branch 
                string BOPoffice = (string)BOPdataGridView[1, currentBOPIndex].Value;
                BOPoffice = Convert.ToString(BOPoffice[0]);

                // Get the Stack Element Type
                Int16 eType = (short)BOPdataGridView[0, currentBOPIndex].Value;

                newStackElement.fkey_StackID = 0;
                newStackElement.Stack_Element_ID = stackElements.Count;
                newStackElement.Stack_Element_Type = eType;
                newStackElement.Stack_Element_Description = "Balance Of Power";
                // Get the template ID for the specified element type
                newStackElement.Stack_Element_TemplateID = mseStackElementTypeCollection.GetMSEStackElementType(mseStackElementTypes, eType).Element_Type_Template_ID;

                newStackElement.Election_Type = "G";
                newStackElement.Office_Code = BOPoffice;
                newStackElement.State_Number = 0;
                newStackElement.State_Mnemonic = string.Empty;
                newStackElement.State_Name = string.Empty;
                newStackElement.CD = 0;
                newStackElement.County_Number = 0;
                newStackElement.County_Name = string.Empty;
                newStackElement.Listbox_Description = (string)BOPdataGridView[1, currentBOPIndex].Value + ": " + BOPdataGridView[2, currentBOPIndex].Value;

                // Specific to race boards
                newStackElement.Race_ID = 0;
                newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = string.Empty;
                newStackElement.Race_District = 0;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = Convert.ToDateTime("2016 11 08");
                newStackElement.Race_UseAPRaceCall = false;

                //Specific to exit polls - set to default values
                newStackElement.ExitPoll_mxID = 0;
                newStackElement.ExitPoll_BoardID = 0;
                newStackElement.ExitPoll_ShortMxLabel = string.Empty;
                newStackElement.ExitPoll_NumRows = 0;
                newStackElement.ExitPoll_xRow = 0;
                newStackElement.ExitPoll_BaseQuestion = false;
                newStackElement.ExitPoll_RowQuestion = false;
                newStackElement.ExitPoll_Subtitle = string.Empty;
                newStackElement.ExitPoll_Suffix = string.Empty;
                newStackElement.ExitPoll_HeaderText_1 = string.Empty;
                newStackElement.ExitPoll_HeaderText_2 = string.Empty;
                newStackElement.ExitPoll_SubsetName = string.Empty;
                newStackElement.ExitPoll_SubsetID = 0;

                // Add element
                stackElementsCollection.AppendStackElement(newStackElement);
                // Update stack entries count label
                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }
        #endregion

        #region Exit Polls Methods
        /// <summary>
        /// Methods to add selected Exit Polls to stack
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            AddExitPoll();
        }

        private void availableExitPollsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddExitPoll();
        }

        private void AddExitPoll()
        {
            try
            {
                // Instantiate new stack element model
                StackElementModel newStackElement = new StackElementModel();

                //Get the selected race list object
                int currentPollIndex = availableExitPollsGrid.CurrentCell.RowIndex;
                ExitPollQuestionsModel selectedPoll = exitPollQuestionsCollection.GetExitPoll(exitPollQuestions, currentPollIndex);

                newStackElement.fkey_StackID = 0;
                newStackElement.Stack_Element_ID = stackElements.Count;
                newStackElement.Stack_Element_Type = (short)StackElementTypes.Exit_Poll_Full_Screen;
                newStackElement.Stack_Element_Description = "Exit Poll";
                // Get the template ID for the specified element type
                newStackElement.Stack_Element_TemplateID = mseStackElementTypeCollection.GetMSEStackElementType(mseStackElementTypes, (short)StackElementTypes.Exit_Poll_Full_Screen).Element_Type_Template_ID;

                newStackElement.Election_Type = selectedPoll.electionType;
                newStackElement.Office_Code = selectedPoll.office;
                newStackElement.State_Number = selectedPoll.stateNum;
                newStackElement.State_Mnemonic = selectedPoll.state;
                newStackElement.State_Name = selectedPoll.stateName;
                newStackElement.CD = selectedPoll.CD;
                newStackElement.County_Number = 0;
                newStackElement.County_Name = "N/A";
                newStackElement.Listbox_Description = selectedPoll.listBoxDescription;

                // Specific to race boards
                newStackElement.Race_ID = 0;
                newStackElement.Race_RecordType = selectedPoll.questionType;
                newStackElement.Race_Office = selectedPoll.office;
                newStackElement.Race_District = selectedPoll.CD;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = Convert.ToDateTime("11/8/2016");
                newStackElement.Race_UseAPRaceCall = false;

                //Specific to exit polls - set to default values
                newStackElement.ExitPoll_mxID = Convert.ToInt32(selectedPoll.mxID);
                newStackElement.ExitPoll_BoardID = 0;
                newStackElement.ExitPoll_ShortMxLabel = selectedPoll.shortMxLabel;
                newStackElement.ExitPoll_NumRows = selectedPoll.numRows;
                newStackElement.ExitPoll_xRow = selectedPoll.rowNum;
                newStackElement.ExitPoll_BaseQuestion = selectedPoll.baseQuestion;
                newStackElement.ExitPoll_RowQuestion = selectedPoll.rowQuestion;
                newStackElement.ExitPoll_Subtitle = string.Empty;
                newStackElement.ExitPoll_Suffix = string.Empty;
                newStackElement.ExitPoll_HeaderText_1 = string.Empty;
                newStackElement.ExitPoll_HeaderText_2 = string.Empty;
                newStackElement.ExitPoll_SubsetName = selectedPoll.subsetName;
                newStackElement.ExitPoll_SubsetID = selectedPoll.subsetID;

                // Add element
                stackElementsCollection.AppendStackElement(newStackElement);
                // Update stack entries count label
                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }


        #endregion

        #region Exit Poll Filters
        /// <summary>
        /// Exit Poll Questions radio button handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbEPMan_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEPMan.Checked == true)
                rbEPMan.BackColor = Color.Gold;
            else
                rbEPMan.BackColor = gbExitPolls.BackColor;
            manualEPQuestions = rbEPMan.Checked;
            RefreshExitPollQuestions();
        }

        private void rbEPAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEPAuto.Checked == true)
                rbEPAuto.BackColor = Color.Gold;
            else
                rbEPAuto.BackColor = gbExitPolls.BackColor;
            manualEPQuestions = rbEPMan.Checked;
            RefreshExitPollQuestions();
        }


        #endregion 

        #region Referendums
        private void button1_Click(object sender, EventArgs e)
        {
            Addreferendum();
        }

        private void ReferendumsGrid_DoubleClick(object sender, EventArgs e)
        {
            Addreferendum();
        }

        private void Addreferendum()
        {
            try
            {
                // Instantiate new stack element model
                StackElementModel newStackElement = new StackElementModel();

                //Get the selected race list object
                int currentReferendumIndex = ReferendumsGrid.CurrentCell.RowIndex;
                ReferendumModel selectedReferendum = referendumsCollection.GetReferendum(referendums, currentReferendumIndex);

                newStackElement.fkey_StackID = 0;
                newStackElement.Stack_Element_ID = stackElements.Count;
                newStackElement.Stack_Element_Type = (short)StackElementTypes.Referendums;
                newStackElement.Stack_Element_Description = "Referendums";
                // Get the template ID for the specified element type
                newStackElement.Stack_Element_TemplateID = mseStackElementTypeCollection.GetMSEStackElementType(mseStackElementTypes, (short)StackElementTypes.Referendums).Element_Type_Template_ID;

                newStackElement.Election_Type = selectedReferendum.race_ElectionType;
                newStackElement.Office_Code = selectedReferendum.race_OfficeCode;
                newStackElement.State_Number = selectedReferendum.state_ID;
                newStackElement.State_Mnemonic = selectedReferendum.state_Abbv;
                newStackElement.State_Name = selectedReferendum.state_Name;
                newStackElement.CD = 0;
                newStackElement.County_Number = 0;
                newStackElement.County_Name = "N/A";
                newStackElement.Listbox_Description = selectedReferendum.race_Description;

                // Specific to race boards
                newStackElement.Race_ID = 0;
                newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = selectedReferendum.race_OfficeCode;
                newStackElement.Race_District = 0;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = Convert.ToDateTime("11/8/2016");
                newStackElement.Race_UseAPRaceCall = false;

                //Specific to exit polls - set to default values
                newStackElement.ExitPoll_mxID = 0;
                newStackElement.ExitPoll_BoardID = 0;
                newStackElement.ExitPoll_ShortMxLabel = String.Empty;
                newStackElement.ExitPoll_NumRows = 0;
                newStackElement.ExitPoll_xRow = 0;
                newStackElement.ExitPoll_BaseQuestion = false;
                newStackElement.ExitPoll_RowQuestion = false;
                newStackElement.ExitPoll_Subtitle = string.Empty;
                newStackElement.ExitPoll_Suffix = string.Empty;
                newStackElement.ExitPoll_HeaderText_1 = string.Empty;
                newStackElement.ExitPoll_HeaderText_2 = string.Empty;
                newStackElement.ExitPoll_SubsetName = string.Empty;
                newStackElement.ExitPoll_SubsetID = 0;

                // Add element
                stackElementsCollection.AppendStackElement(newStackElement);
                // Update stack entries count label
                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }


        #endregion

        #region Race Filters
        /// <summary>
        /// Contains all the methods for loading the available races list with the filters applied
        /// </summary>

        // load the available races list with Presidential races only

        public Int16 callStatus;
        public string ofcID = "A";
        public Boolean battlegroundOnly = false;
        public Int16 specialFilters;
        private void rbPresident_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPresident.Checked == true)
                rbPresident.BackColor = Color.Gold;
            else
                rbPresident.BackColor = gbROF.BackColor;
            ofcID = "P";
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        // load the available races list with all races
        private void rbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbShowAll.Checked == true)
                rbShowAll.BackColor = Color.Gold;
            else
                rbShowAll.BackColor = gbROF.BackColor;
            ofcID = "A";
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        // load the available races list with Senate races only
        private void rbSenate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSenate.Checked == true)
                rbSenate.BackColor = Color.Gold;
            else
                rbSenate.BackColor = gbROF.BackColor;
            ofcID = "S";
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        // load the available races list with House races only
        private void rbHouse_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHouse.Checked == true)
                rbHouse.BackColor = Color.Gold;
            else
                rbHouse.BackColor = gbROF.BackColor;
            ofcID = "H";
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        // load the available races list with Governor races only
        private void rbGovernor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGovernor.Checked == true)
                rbGovernor.BackColor = Color.Gold;
            else
                rbGovernor.BackColor = gbROF.BackColor;
            ofcID = "G";
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }
        private void rbTCTC_Click(object sender, EventArgs e)
        {
            callStatus = (Int16)BoardModes.Race_Board_To_Close_To_Call;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbJustCalled_CheckedChanged(object sender, EventArgs e)
        {
            if (rbJustCalled.Checked == true)
                rbJustCalled.BackColor = Color.Gold;
            else
                rbJustCalled.BackColor = gbRCF.BackColor;
            callStatus = (Int16)BoardModes.Race_Board_Just_Called;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbCalled_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCalled.Checked == true)
                rbCalled.BackColor = Color.Gold;
            else
                rbCalled.BackColor = gbRCF.BackColor;
            callStatus = (Int16)BoardModes.Race_Board_Race_Called;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAll.Checked == true)
                rbAll.BackColor = Color.Gold;
            else
                rbAll.BackColor = gbRCF.BackColor;
            callStatus = (Int16)BoardModes.Race_Board_Normal;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbTCTC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTCTC.Checked == true)
                rbTCTC.BackColor = Color.Gold;
            else
                rbTCTC.BackColor = gbRCF.BackColor;
            callStatus = (Int16)BoardModes.Race_Board_To_Close_To_Call;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbBattleground_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBattleground.Checked == true)
                rbBattleground.BackColor = Color.Gold;
            else
                rbBattleground.BackColor = gbSpF.BackColor;
            battlegroundOnly = rbBattleground.Checked;
            specialFilters = (short)SpecialCaseFilterModes.Battleground_States_Only;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbPollClosing_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPollClosing.Checked == true)
                rbPollClosing.BackColor = Color.Gold;
            else
                rbPollClosing.BackColor = gbSpF.BackColor;
            specialFilters = (short)SpecialCaseFilterModes.Next_Poll_Closing_States_Only;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNone.Checked == true)
                rbNone.BackColor = Color.Gold;
            else
                rbNone.BackColor = gbSpF.BackColor;
            battlegroundOnly = false;
            specialFilters = (short)SpecialCaseFilterModes.None;
            RefreshAvailableRacesListFiltered(ofcID, callStatus, specialFilters, stateMetadata);

        }

        #endregion

        #region Add select boards
        // Add 1-way select board
        private void btnSelect1_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCandidate1 = 0;
                Int32 selectedCandidate2 = 0;
                Int32 selectedCandidate3 = 0;
                Int32 selectedCandidate4 = 0;
                string cand1Name = string.Empty;
                string cand2Name = string.Empty;
                string cand3Name = string.Empty;
                string cand4Name = string.Empty;
                Int16 numCand = 1;

                //Get the selected race list object
                int currentRaceIndex = availableRacesGrid.CurrentCell.RowIndex;
                AvailableRaceModel selectedRace = availableRacesCollection.GetRace(availableRaces, currentRaceIndex);

                string eType = selectedRace.Election_Type;
                string ofc = selectedRace.Race_Office;
                Int16 st = selectedRace.State_Number;
                string des = selectedRace.Race_Description;

                DialogResult dr = new DialogResult();
                //frmCandidateSelect selectCand = new frmCandidateSelect();
                FrmCandidateSelect selectCand = new FrmCandidateSelect(numCand, st, ofc, eType, des);
                dr = selectCand.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    // Set candidateID's

                    selectedCandidate1 = selectCand.Cand1;
                    cand1Name = selectCand.CandName1;
                    selectedCandidate2 = selectCand.Cand2;
                    cand2Name = selectCand.CandName2;
                    selectedCandidate3 = selectCand.Cand3;
                    cand3Name = selectCand.CandName3;
                    selectedCandidate4 = selectCand.Cand4;
                    cand4Name = selectCand.CandName4;
                    AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
                }

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Add 2-way select board
        private void btnSelect2_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCandidate1 = 0;
                Int32 selectedCandidate2 = 0;
                Int32 selectedCandidate3 = 0;
                Int32 selectedCandidate4 = 0;
                string cand1Name = string.Empty;
                string cand2Name = string.Empty;
                string cand3Name = string.Empty;
                string cand4Name = string.Empty;
                Int16 numCand = 2;

                //Get the selected race list object
                int currentRaceIndex = availableRacesGrid.CurrentCell.RowIndex;
                AvailableRaceModel selectedRace = availableRacesCollection.GetRace(availableRaces, currentRaceIndex);

                string eType = selectedRace.Election_Type;
                string ofc = selectedRace.Race_Office;
                Int16 st = selectedRace.State_Number;
                string des = selectedRace.Race_Description;

                DialogResult dr = new DialogResult();
                //frmCandidateSelect selectCand = new frmCandidateSelect();
                FrmCandidateSelect selectCand = new FrmCandidateSelect(numCand, st, ofc, eType, des);
                dr = selectCand.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    // Set candidateID's

                    selectedCandidate1 = selectCand.Cand1;
                    cand1Name = selectCand.CandName1;
                    selectedCandidate2 = selectCand.Cand2;
                    cand2Name = selectCand.CandName2;
                    selectedCandidate3 = selectCand.Cand3;
                    cand3Name = selectCand.CandName3;
                    selectedCandidate4 = selectCand.Cand4;
                    cand4Name = selectCand.CandName4;
                    AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
                }

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

        }

        // Add 3-way select board
        private void btnSelect3_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCandidate1 = 0;
                Int32 selectedCandidate2 = 0;
                Int32 selectedCandidate3 = 0;
                Int32 selectedCandidate4 = 0;
                string cand1Name = string.Empty;
                string cand2Name = string.Empty;
                string cand3Name = string.Empty;
                string cand4Name = string.Empty;
                Int16 numCand = 3;

                //Get the selected race list object
                int currentRaceIndex = availableRacesGrid.CurrentCell.RowIndex;
                AvailableRaceModel selectedRace = availableRacesCollection.GetRace(availableRaces, currentRaceIndex);

                string eType = selectedRace.Election_Type;
                string ofc = selectedRace.Race_Office;
                Int16 st = selectedRace.State_Number;
                string des = selectedRace.Race_Description;

                DialogResult dr = new DialogResult();
                //frmCandidateSelect selectCand = new frmCandidateSelect();
                FrmCandidateSelect selectCand = new FrmCandidateSelect(numCand, st, ofc, eType, des);
                dr = selectCand.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    // Set candidateID's

                    selectedCandidate1 = selectCand.Cand1;
                    cand1Name = selectCand.CandName1;
                    selectedCandidate2 = selectCand.Cand2;
                    cand2Name = selectCand.CandName2;
                    selectedCandidate3 = selectCand.Cand3;
                    cand3Name = selectCand.CandName3;
                    selectedCandidate4 = selectCand.Cand4;
                    cand4Name = selectCand.CandName4;
                    AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
                }

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }
        }

        // Add 4-way select board
        private void btnSelect4_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCandidate1 = 0;
                Int32 selectedCandidate2 = 0;
                Int32 selectedCandidate3 = 0;
                Int32 selectedCandidate4 = 0;
                string cand1Name = string.Empty;
                string cand2Name = string.Empty;
                string cand3Name = string.Empty;
                string cand4Name = string.Empty;
                Int16 numCand = 4;

                //Get the selected race list object
                int currentRaceIndex = availableRacesGrid.CurrentCell.RowIndex;
                AvailableRaceModel selectedRace = availableRacesCollection.GetRace(availableRaces, currentRaceIndex);

                string eType = selectedRace.Election_Type;
                string ofc = selectedRace.Race_Office;
                Int16 st = selectedRace.State_Number;
                string des = selectedRace.Race_Description;

                DialogResult dr = new DialogResult();
                //frmCandidateSelect selectCand = new frmCandidateSelect();
                FrmCandidateSelect selectCand = new FrmCandidateSelect(numCand, st, ofc, eType, des);
                dr = selectCand.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    // Set candidateID's

                    selectedCandidate1 = selectCand.Cand1;
                    cand1Name = selectCand.CandName1;
                    selectedCandidate2 = selectCand.Cand2;
                    cand2Name = selectCand.CandName2;
                    selectedCandidate3 = selectCand.Cand3;
                    cand3Name = selectCand.CandName3;
                    selectedCandidate4 = selectCand.Cand4;
                    cand4Name = selectCand.CandName4;
                    AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
                }

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

        }

        // Generic method to add a candidate select race board to the stack
        private void AddSelectRaceBoardToStack(Int16 numCand, Int32 cID1, Int32 cID2, Int32 cID3, Int32 cID4, string cand1Name, string cand2Name, string cand3Name, string cand4Name)
        {
            //Get the selected race list object
            int currentRaceIndex = availableRacesGrid.CurrentCell.RowIndex;
            AvailableRaceModel selectedRace = availableRacesCollection.GetRace(availableRaces, currentRaceIndex);

            try
            {
                string nameList = "(";

                for (Int16 i = 1; i <= numCand; i++)
                {
                    switch (i)
                    {
                        case 1:
                            nameList = nameList + cand1Name;
                            break;
                        case 2:
                            nameList = nameList + ", " + cand2Name;
                            break;
                        case 3:
                            nameList = nameList + ", " + cand3Name;
                            break;
                        case 4:
                            nameList = nameList + ", " + cand4Name;
                            break;
                    }
                }
                nameList = nameList + ")";

                int eType = numCand * 2;

                string eDesc = "Race Board (" + numCand + "-Way Select)"; 

                // Instantiate new stack element model
                StackElementModel newStackElement = new StackElementModel();

                newStackElement.fkey_StackID = 0;
                newStackElement.Stack_Element_ID = stackElements.Count;
                newStackElement.Stack_Element_Type = (short)eType;
                newStackElement.Stack_Element_Description = eDesc;
                
                // Get the template ID for the specified element type
                newStackElement.Stack_Element_TemplateID = mseStackElementTypeCollection.GetMSEStackElementType(mseStackElementTypes, (short)StackElementTypes.Race_Board_1_Way).Element_Type_Template_ID;

                newStackElement.Election_Type = selectedRace.Election_Type;
                newStackElement.Office_Code = selectedRace.Race_Office;
                newStackElement.State_Number = selectedRace.State_Number;
                newStackElement.State_Mnemonic = selectedRace.State_Mnemonic;
                newStackElement.State_Name = selectedRace.State_Name;
                newStackElement.CD = selectedRace.CD;
                newStackElement.County_Number = 0;
                newStackElement.County_Name = "N/A";
                newStackElement.Listbox_Description = selectedRace.Race_Description + "  " + nameList;

                // Specific to race boards
                newStackElement.Race_ID = selectedRace.Race_ID;
                newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = selectedRace.Race_Office;
                newStackElement.Race_District = selectedRace.CD;
                newStackElement.Race_CandidateID_1 = cID1;
                newStackElement.Race_CandidateID_2 = cID2;
                newStackElement.Race_CandidateID_3 = cID3;
                newStackElement.Race_CandidateID_4 = cID4;
                newStackElement.Race_PollClosingTime = selectedRace.Race_PollClosingTime;
                newStackElement.Race_UseAPRaceCall = selectedRace.Race_UseAPRaceCall;

                //Specific to exit polls - set to default values
                newStackElement.ExitPoll_mxID = 0;
                newStackElement.ExitPoll_BoardID = 0;
                newStackElement.ExitPoll_ShortMxLabel = string.Empty;
                newStackElement.ExitPoll_NumRows = 0;
                newStackElement.ExitPoll_xRow = 0;
                newStackElement.ExitPoll_BaseQuestion = false;
                newStackElement.ExitPoll_RowQuestion = false;
                newStackElement.ExitPoll_Subtitle = string.Empty;
                newStackElement.ExitPoll_Suffix = string.Empty;
                newStackElement.ExitPoll_HeaderText_1 = string.Empty;
                newStackElement.ExitPoll_HeaderText_2 = string.Empty;
                newStackElement.ExitPoll_SubsetName = string.Empty;
                newStackElement.ExitPoll_SubsetID = 0;

                // Add element
                stackElementsCollection.AppendStackElement(newStackElement);
                // Update stack entries count label
                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred: " + ex.Message);
                log.Debug("frmMain Exception occurred", ex);
            }

        }
        #endregion

        #region Methods for handling graphics concepts
        // Handler for Concept Change
        private void cbGraphicConcept_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConceptID = (short)(cbGraphicConcept.SelectedIndex + 1);
            ConceptName = graphicsConceptTypes[cbGraphicConcept.SelectedIndex].ConceptName;
                
        }
        
        // Look up Template Name from conceptID and BoardType
        private string GetTemplate(Int16 cID, Int16 boardType)
        {
            Int16 Indx = -1;
            string Template = "None";
            for (short  i = 0; i < graphicsConcepts.Count; i++)
            {
                if ((cID == graphicsConcepts[i].ConceptID) & (boardType == (short)graphicsConcepts[i].ElementTypeCode))
                {
                    Indx = i;
                    Template = graphicsConcepts[i].TemplateName;
                }
                
            }
            return Template;
        }
        #endregion 

        private void gbROF_Enter(object sender, EventArgs e)
        {

        }
    }
}
