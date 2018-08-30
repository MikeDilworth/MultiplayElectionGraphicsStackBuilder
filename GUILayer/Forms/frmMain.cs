//////////////////////////////////////////////////////////////////////////////
// MAIN APPLICATION FORM
// Version 1.0.0
// M. Dilworth  Rev: 08/17/2016
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using DataInterface.DataAccess;
using DataInterface.DataModel;
using DataInterface.Enums;
using log4net.Appender;
using LogicLayer.Collections;
using LogicLayer.CommonClasses;
using MSEInterface;
using MSEInterface.Constants;
using AsyncClientSocket;

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

        string elementCollectionURIShow;
        string templateCollectionURIShow;
        string elementCollectionURIPlaylist;
        string templateModel;
        // Parameters for current (working) stack
        // Specify stackID as double - will use encoded date/time string converted to float
        Double stackID = -1;
        string stackDescription;

        // For future use
        Boolean insertNext;
        Int32 insertPoint;

        // For no use what so ever
        ulong useless = 0;

        Int16 conceptID;
        string conceptName;

        private Boolean manualEPQuestions = false;
        public Boolean enableShowSelectControls = false;
        public List<EngineModel> vizEngines = new List<EngineModel>();
        public bool builderOnlyMode = false;

        //public static AsyncClientSocket.ClientSocket VizControl;
        public List<ClientSocket> vizClientSockets = new List<ClientSocket>();


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

        // Define the collection object for the elements within a specified working stack
        public StackElementsCollection stackElementsCollection;
        BindingList<StackElementModel> stackElements;

        // Define the collection object for the elements within a specified stack to be activated
        public StackElementsCollection activateStackElementsCollection;
        BindingList<StackElementModel> activateStackElements;

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
        //private ExitPollDataCollection exitPollDataCollection;
        //BindingList<ExitPollDataCollection> exitPollData;

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
        Boolean usingPrimaryMediaSequencer = true;
        Boolean mseEndpoint1_Enable = false;
        string mseEndpoint1 = string.Empty;
        Boolean mseEndpoint2_Enable = false;
        string mseEndpoint2 = string.Empty;
        string topLevelShowsDirectoryURI = string.Empty;
        string masterPlaylistsDirectoryURI = string.Empty;
        string profilesURI = string.Empty;
        string currentShowName = string.Empty;
        string currentPlaylistName = string.Empty;

        /*
        Boolean mseEndpoint1_Enable = Properties.Settings.Default.MSEEndpoint1_Enable;
        string mseEndpoint1 = Properties.Settings.Default.MSEEndpoint1;
        Boolean mseEndpoint2_Enable = Properties.Settings.Default.MSEEndpoint2_Enable;
        string mseEndpoint2 = Properties.Settings.Default.MSEEndpoint1;
        string topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
        string masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.MasterPlaylistsDirectory;
        string profilesURI = Properties.Settings.Default.MSEEndpoint1 + "profiles";
        string currentShowName = Properties.Settings.Default.CurrentShowName;
        string currentPlaylistName = Properties.Settings.Default.CurrentSelectedPlaylist;
        */

        // Read in database connection strings
        string GraphicsDBConnectionString = Properties.Settings.Default.GraphicsDBConnectionString;
        string ElectionsDBConnectionString = Properties.Settings.Default.ElectionsDBConnectionString;

        //Read in default Trio profile and channel
        string defaultTrioProfile = Properties.Settings.Default.DefaultTrioProfile;
        string defaultTrioChannel = Properties.Settings.Default.DefaultTrioChannel;                        
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

                // Set enable/disable state of tab pages
                if (Properties.Settings.Default.TabEnableRaces)
                {
                    tpRaces.Enabled = true;   
                }
                else
                {
                    tpRaces.Enabled = false;   
                }
                if (Properties.Settings.Default.TabEnableExitPolls)
                {
                    tpExitPolls.Enabled = true;
                }
                else
                {
                    tpExitPolls.Enabled = false;
                }
                if (Properties.Settings.Default.TabEnableBalanceOfPower)
                {
                    tpBalanceOfPower.Enabled = true;
                }
                else
                {
                    tpBalanceOfPower.Enabled = false;
                }
                if (Properties.Settings.Default.TabEnableReferendums)
                {
                    tpReferendums.Enabled = true;
                }
                else
                {
                    tpReferendums.Enabled = false;
                }

                // Setup show controls
                if (Properties.Settings.Default.EnableShowSelectControls)
                    enableShowSelectControls = true;
                else
                    enableShowSelectControls = false;
                if (enableShowSelectControls)
                {
                    miSelectDefaultShow.Enabled = true;
                }
                else
                {
                    miSelectDefaultShow.Enabled = false;
                }

                // Update status
                toolStripStatusLabel.Text = "Starting program initialization - loading data from SQL database.";

                // Query the graphics DB to get the list of already saved stacks
                RefreshStacksList();

                // Query the elections DB to get the list of exit poll questions
                RefreshExitPollQuestions();

                // Query the elections DB to get the list of available races
                RefreshAvailableRacesList();

                //Query the elections DB to get the list of Referendums
                RefreshReferendums();

                // Query the elections DB to get application flags(option settings)
                RefreshApplicationFlags();

                // Init the stack elements collections
                CreateStackElementsCollections();

                // Setup data binding for stacks grid
                stackGrid.AutoGenerateColumns = false;
                var stackGridDataSource = new BindingSource(stackElements, null);
                stackGrid.DataSource = stackGridDataSource;

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

                // Enable handling of function keys; setup method for function keys and assign delegate
                KeyPreview = true;
                this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);

                timerStatusUpdate.Enabled = true;

                // Set connection string for functions to get simulated time
                TimeFunctions.ElectionsDBConnectionString = ElectionsDBConnectionString;

                // Make entry into applications log
                ApplicationSettingsFlagsAccess applicationSettingsFlagsAccess = new ApplicationSettingsFlagsAccess();
                string ipAddress = string.Empty;
                string hostName = string.Empty;
                ipAddress = HostIPNameFunctions.GetLocalIPAddress();
                hostName = HostIPNameFunctions.GetHostName(ipAddress);
                lblIpAddress.Text = ipAddress;
                lblHostName.Text = hostName; 

                // Post application log entry
                applicationSettingsFlagsAccess.ElectionsDBConnectionString = ElectionsDBConnectionString;
                applicationSettingsFlagsAccess.PostApplicationLogEntry(
                    Properties.Settings.Default.ApplicationName,
                    Properties.Settings.Default.ApplicationName,
                    hostName,
                    ipAddress,
                    // Engines not used for this application
                    false,
                    "",
                    false,
                    "",
                    "Launched application",
                    Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version), 
                    Properties.Settings.Default.ApplicationID,
                    "",
                    System.DateTime.Now
                );

                // Set version number
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                this.Text = String.Format("Election Graphics Stack Builder Application  Version {0}", version);
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
            // Read in config settings - default to Media Sequencer #1
            mseEndpoint1 = Properties.Settings.Default.MSEEndpoint1;
            mseEndpoint2 = Properties.Settings.Default.MSEEndpoint2;
            mseEndpoint1_Enable = Properties.Settings.Default.MSEEndpoint1_Enable;
            mseEndpoint2_Enable = Properties.Settings.Default.MSEEndpoint2_Enable;
            topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
            masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.MasterPlaylistsDirectory;
            profilesURI = Properties.Settings.Default.MSEEndpoint1 + "profiles";
            currentShowName = Properties.Settings.Default.CurrentShowName;
            currentPlaylistName = Properties.Settings.Default.CurrentSelectedPlaylist;
            string sceneDescription = Properties.Settings.Default.Scene_Name;
            var useSceneName = Properties.Settings.Default[sceneDescription];
            builderOnlyMode = Properties.Settings.Default.builderOnly;

            this.Size = new Size(1462, 928);
            connectionPanel.Visible = false;
            listBox1.Visible = false;
            panel3.Size = new Size(648, 155);
            panel3.Location = new Point(8, 549);
            StackPanel.Location = new Point(3, 8);
            SaveActivatePanel.Location = new Point(424, 8);
            stackGrid.Size = new Size(648, 536);
            LockPanel.Visible = false;
            TakePanel.Visible = false;
            SaveActivatePanel.Visible = true;
            pnlUpDn.Location = new Point(676, 250);
            
            if (builderOnlyMode == false)
            {

                // Enlarge form
                this.Size = new Size(1462, 1061);
                connectionPanel.Visible = true;
                listBox1.Visible = true;
                stackGrid.Size = new Size(648, 468);
                panel3.Size = new Size(648, 221);
                panel3.Location = new Point(8, 483);
                StackPanel.Location = new Point(1, 78);
                LockPanel.Visible = true;
                TakePanel.Visible = true;
                SaveActivatePanel.Visible = false;
                SaveActivatePanel.Location = new Point(424, 3);
                btnSaveStack.Enabled = true;
                pnlUpDn.Location = new Point(676, 216);


                // get viz engine info

                int i = 0;
                bool done = false;
                string engineParam;
                var engineInfo = Properties.Settings.Default["Engine1_IPAddress"];

                // read engine info until no more engines found
                while (done == false)
                {
                    EngineModel viz = new EngineModel();
                    i++;
                    engineParam = $"Engine{i}_IPAddress";

                    try
                    {
                        engineInfo = Properties.Settings.Default[engineParam];
                        viz.IPAddress = (string)engineInfo;

                        engineParam = $"Engine{i}_Port";
                        engineInfo = Properties.Settings.Default[engineParam];
                        viz.Port = (int)engineInfo;

                        engineParam = $"Engine{i}_Enable";
                        engineInfo = Properties.Settings.Default[engineParam];
                        viz.enable = (bool)engineInfo;

                        viz.id = i;
                        viz.systemIP = System.Net.IPAddress.Parse(viz.IPAddress);


                        vizEngines.Add(viz);

                        vizClientSockets.Add(new ClientSocket(viz.systemIP, viz.Port));
                        vizClientSockets[i-1].DataReceived += vizDataReceived;
                        vizClientSockets[i-1].ConnectionStatusChanged += vizConnectionStatusChanged;


                        // set viz address labels
                        switch (i)
                        {
                            case 1:
                                gbIPlbl1.Text = "IP: " + viz.IPAddress;
                                gbPortlbl1.Text = "Port: " + viz.Port.ToString();
                                gbViz1.Visible = true;
                                gbViz1.Enabled = viz.enable;
                                break;

                            case 2:
                                gbIPlbl2.Text = "IP: " + viz.IPAddress;
                                gbPortlbl2.Text = "Port: " + viz.Port.ToString();
                                gbViz2.Visible = true;
                                gbViz2.Enabled = viz.enable;
                                break;

                            case 3:
                                gbIPlbl3.Text = "IP: " + viz.IPAddress;
                                gbPortlbl3.Text = "Port: " + viz.Port.ToString();
                                gbViz3.Visible = true;
                                gbViz3.Enabled = viz.enable;
                                break;

                            case 4:
                                gbIPlbl4.Text = "IP: " + viz.IPAddress;
                                gbPortlbl4.Text = "Port: " + viz.Port.ToString();
                                gbViz4.Visible = true;
                                gbViz4.Enabled = viz.enable;
                                break;

                            case 5:
                                gbIPlbl5.Text = "IP: " + viz.IPAddress;
                                gbPortlbl5.Text = "Port: " + viz.Port.ToString();
                                gbViz5.Visible = true;
                                gbViz5.Enabled = viz.enable;
                                break;

                            case 6:
                                gbIPlbl6.Text = "IP: " + viz.IPAddress;
                                gbPortlbl6.Text = "Port: " + viz.Port.ToString();
                                gbViz6.Visible = true;
                                gbViz6.Enabled = viz.enable;
                                break;

                        }

                        if (i == 6)
                            done = true;
                    }
                    catch
                    {
                        // Next engine not found
                        done = true;
                    }
                }

            }



            usingPrimaryMediaSequencer = true;

            // Log application start
            log.Info("Starting Stack Builder application");

            lblMediaSequencer.Text = "USING PRIMARY MEDIA SEQUENCER: " + Convert.ToString(Properties.Settings.Default.MSEEndpoint1);
            lblMediaSequencer.BackColor = System.Drawing.Color.White;
            usePrimaryMediaSequencerToolStripMenuItem.Checked = true;
            useBackupMediaSequencerToolStripMenuItem.Checked = false;
            
        }
        public void ConnectToVizEngines()
        {
            
            for (int i = 0; i < vizEngines.Count; i++)
            {
                // Connect to the ClientSocket; call-backs for connection status will indicate status of client sockets
                vizClientSockets[i].AutoReconnect = true;
                vizClientSockets[i].Connect();
            }
        }

        private void vizDataReceived(ClientSocket sender, byte[] data)
        {
            // receive the data and determine the type
            //string vizIP = sender.Ip;
            System.Net.IPAddress IP = sender.Ip;
            int port = sender.Port;
            int i = GetVizEngineNumber(IP, port);
            listBox1.Items.Add(System.Text.Encoding.Default.GetString(data));

        }

        public int GetVizEngineNumber(System.Net.IPAddress IP, int port)
        {
            for (int i = 0; i < vizEngines.Count; i++)
            {
                if (vizEngines[i].systemIP == IP && vizEngines[i].Port == port)
                {
                    return i;
                }
            }
            return -1;
        }

        // Handler for source & destination MSE connection status change
        public void vizConnectionStatusChanged(ClientSocket sender, ClientSocket.ConnectionStatus status)
        {

            System.Net.IPAddress IP = sender.Ip;
            int port = sender.Port;
            int i = GetVizEngineNumber(IP, port);

            // Set status
            if (status == ClientSocket.ConnectionStatus.Connected)
            {
                vizEngines[i].connected = true;
            }
            else
            {
                vizEngines[i].connected = false;
            }
            SetConnectionLED(i);
            // Send to log - DEBUG ONLY
            log.Debug($"Viz Engine {i + 1}: {status}");
        }

        public void SetConnectionLED(int i)
        {
            bool connected = vizEngines[i].connected;
            switch (i + 1)
            {
                case 1:
                    if (connected)
                        gbLEDOn1.Visible = true;
                    else
                        gbLEDOn1.Visible = false;
                    break;

                case 2:
                    if (connected)
                        gbLEDOn2.Visible = true;
                    else
                        gbLEDOn2.Visible = false;
                    break;

                case 3:
                    if (connected)
                        gbLEDOn3.Visible = true;
                    else
                        gbLEDOn3.Visible = false;
                    break;

                case 4:
                    if (connected)
                        gbLEDOn4.Visible = true;
                    else
                        gbLEDOn4.Visible = false;
                    break;

                case 5:
                    if (connected)
                        gbLEDOn5.Visible = true;
                    else
                        gbLEDOn5.Visible = false;
                    break;

                case 6:
                    if (connected)
                        gbLEDOn6.Visible = true;
                    else
                        gbLEDOn6.Visible = false;
                    break;

            }

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
            else
            {
                // Make entry into applications log
                ApplicationSettingsFlagsAccess applicationSettingsFlagsAccess = new ApplicationSettingsFlagsAccess();
                string ipAddress = string.Empty;
                string hostName = string.Empty;
                ipAddress = HostIPNameFunctions.GetLocalIPAddress();
                hostName = HostIPNameFunctions.GetHostName(ipAddress);
                lblIpAddress.Text = ipAddress;
                lblHostName.Text = hostName;

                // Post application log entry
                applicationSettingsFlagsAccess.ElectionsDBConnectionString = ElectionsDBConnectionString;
                applicationSettingsFlagsAccess.PostApplicationLogEntry(
                    Properties.Settings.Default.ApplicationName,
                    Properties.Settings.Default.ApplicationName,
                    hostName,
                    ipAddress,
                    // Engines not used for this application
                    false,
                    "",
                    false,
                    "",
                    "Closed application",
                    Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version),
                    Properties.Settings.Default.ApplicationID,
                    "",
                    System.DateTime.Now
                );               
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
            timeLabel.Text = String.Format("{0:h:mm:ss tt  MMM dd, yyyy}", referenceTime);

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

                // Set next poll closing label
                if (scfm == (short) SpecialCaseFilterModes.Next_Poll_Closing_States_Only)
                {
                    txtNextPollClosingTime.Text = Convert.ToString(this.availableRacesCollection.NextPollClosingTime);
                    txtNextPollClosingTimeHeader.Visible = true;
                    txtNextPollClosingTime.Visible = true;
                }
                else
                {
                    txtNextPollClosingTimeHeader.Visible = false;
                    txtNextPollClosingTime.Visible = false;
                    txtNextPollClosingTime.Text = "N/A";
                }

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

                // Setup the available referendums grid
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

        // Create the graphics concepts collection
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

                conceptID = graphicsConceptTypes[0].ConceptID;
                conceptName = graphicsConceptTypes[0].ConceptName;
                cbGraphicConcept.SelectedIndex = 0;
                cbGraphicConcept.Text = conceptName;
                
                // Setup the master race collection & bind to grid
                //this.graphicsConceptsCollection = new GraphicsConceptsCollection();
                //this.graphicsConceptsCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                graphicsConcepts = this.graphicsConceptsCollection.GetGraphicsConceptsCollection();

            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred while trying to create graphics concepts collection: " + ex.Message);
                log.Debug("frmMain Exception occurred while trying to create graphics concepts collection", ex);
            }
        }
        #endregion

        #region Stack (group) creation & management functions
        /// <summary>
        /// STACK CREATION & MANAGEMENT FUNCTIONS
        /// </summary>
        // Function create the initial playlist elements collection for editing
        private void CreateStackElementsCollections()
        {
            try
            {
                // Setup the master stack collection & bind to grid
                this.stackElementsCollection = new StackElementsCollection();
                this.stackElementsCollection.MainDBConnectionString = GraphicsDBConnectionString;
                stackElements = this.stackElementsCollection.GetStackElementsCollection(-1);

                // Setup the stacks grid
                stackGrid.AutoGenerateColumns = false;
                var stackGridDataSource = new BindingSource(stackElements, null);
                stackGrid.DataSource = stackGridDataSource;

                // Setup the stack collection used for loading to MSE only
                this.activateStackElementsCollection = new StackElementsCollection();
                this.activateStackElementsCollection.MainDBConnectionString = GraphicsDBConnectionString;
                activateStackElements = this.stackElementsCollection.GetStackElementsCollection(-1);

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
            string mseEndpoint = string.Empty;
            if (usingPrimaryMediaSequencer)
            {
                mseEndpoint = mseEndpoint1;
            }
            else
            {
                mseEndpoint = mseEndpoint2;
            }
            frmSelectShow selectShow = new frmSelectShow(mseEndpoint);
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

            if (insertNext == true)
            {
                AddRaceBoardToStack(seType, seDescription);
            }
            else
            {
            }
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
                
                // Get the template ID for the specified element type & concept ID
                newStackElement.Stack_Element_TemplateID = GetTemplate(conceptID, stackElementType);

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

        // Handler for insert button
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (stackGrid.CurrentCell.RowIndex < 0)
            {
                insertPoint = 0;
            }
            else
            {
                insertPoint = stackGrid.CurrentCell.RowIndex;
            }
            // Set flag
            insertNext = true;
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
                case Keys.F12:
                    rbNone.Checked = true;
                    break;
                case Keys.A:
                    if (e.Control == true)
                        btnAddAll_Click(sender, e);
                    break;
                // Check for 1 -> 4 way boards (horse-raced)
                case Keys.D1:
                    if (e.Control == true)
                        btnAddRace1Way_Click(sender, e);
                    else if (e.Alt == true)
                        btnSelect1_Click(sender, e);
                    break;
                case Keys.D2:
                    if (e.Control == true)
                        btnAddRace2Way_Click(sender, e);
                    else if (e.Alt == true)
                        btnSelect2_Click(sender, e);
                    break;
                case Keys.D3:
                    if (e.Control == true)
                        btnAddRace3Way_Click(sender, e);
                    else if (e.Alt == true)
                        btnSelect3_Click(sender, e);
                    break;
                case Keys.D4:
                    if (e.Control == true)
                        btnAddRace4Way_Click(sender, e);
                    else if (e.Alt == true)
                        btnSelect4_Click(sender, e);
                    break;
                // Stack operations
                case Keys.R:
                    if (e.Control == true)
                        btnLoadStack_Click(sender, e);
                    break;
                case Keys.S:
                    if (e.Control == true)
                        btnSaveActivateStack_Click(sender, e);
                    break;
                case Keys.D:
                    if (e.Control == true)
                        btnDeleteStackElement_Click(sender, e);
                    break;
                case Keys.C:
                    if (e.Control == true)
                        btnClearStack_Click_1(sender, e);
                    break;
                // Save to database only disabled for 2016 election
                //case Keys.O:
                //    if (e.Control == true)
                //        btnSaveStack_Click(sender, e);
                //    break;
                //default:
                //    rbShowAll.Checked = true;
                //    break;
            }
        }

        // Method to handle key press events (letters a-z as shortcuts to states in available races list)
        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for alpha character
            if (((e.KeyChar >= 'a') && (e.KeyChar <= 'z')) || ((e.KeyChar >= 'A') && (e.KeyChar <= 'Z')))
            {
                // Check for available races
                if (availableRaces.Count > 0)
                {
                    Boolean foundMatch = false;
                    Int16 searchIndex = 0;

                    do
                    {
                        AvailableRaceModel availableRace = null;
                        availableRace = availableRaces[searchIndex];
                        String stateName = availableRace.State_Mnemonic.Trim();
                        if ((stateName[0] == e.KeyChar) || (stateName[0] == Char.ToUpper(e.KeyChar)))
                        {
                            foundMatch = true;

                            availableRacesGrid.FirstDisplayedScrollingRowIndex = searchIndex;
                            availableRacesGrid.Refresh();

                            availableRacesGrid.CurrentCell = availableRacesGrid.Rows[searchIndex].Cells[0];

                            availableRacesGrid.Rows[searchIndex].Selected = true;
                        }
                        searchIndex++;
                    }
                    while ((foundMatch == false) && (searchIndex < availableRaces.Count));
                }
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
                        stackMetadata.ConceptID = conceptID;
                        stackMetadata.ConceptName = conceptName;
                        stackMetadata.Notes = "Not currently used";
                        stacksCollection.SaveStack(stackMetadata);

                        // Save out stack elements; specify stack ID, and set flag to delete existing elements before adding
                        stackElementsCollection.SaveStackElementsCollection(stackMetadata.ixStackID, true);


                        // Update stack entries count label & name label
                        txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                        txtStackName.Text = stackDescription;
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

                    // Clear out current stack settings
                    stackID = -1;
                    txtStackName.Text = "None Selected";

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

                // Clear out current settings if no entries left in stack
                if (stackElements.Count == 0)
                {
                    stackID = -1;
                    txtStackName.Text = "None Selected";
                }
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
                Int32 stackIndex = 0;

                // Setup dialog to load stack
                DialogResult dr = new DialogResult();               
                frmLoadStack loadStack = new frmLoadStack();

                loadStack.EnableShowControls = enableShowSelectControls;

                RefreshStacksList();

                dr = loadStack.ShowDialog();

                // Process result from dialog
                // Check for Load Stack operation
                if (dr == DialogResult.OK)
                {
                    // Set candidateID's
                    stackIndex = loadStack.StackIndex;
                    stackID = loadStack.StackID;
                    stackDescription = loadStack.StackDesc;

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
                    //txtStackName.Text = selectedStack.StackName + " [ID: " + Convert.ToString(selectedStack.ixStackID) + "]";
                    txtStackName.Text = selectedStack.StackName;
                }
                // Check for Delete Stack operation
                else if (dr == DialogResult.Ignore)
                {
                    Boolean okToGo = true;

                    stackIndex = loadStack.StackIndex;
                    if (stacks.Count > 0)
                    {
                        // Get the stack index from the dialog
                        int currentStackIndex = stackIndex;

                        // Get the metadata for the stack
                        StackModel selectedStack = stacksCollection.GetStackMetadata(stacks, currentStackIndex);

                        // Check if operator is trying the delete the currently loaded stack and prompt
                        if (selectedStack.StackName == stackDescription)
                        {
                            DialogResult result1 =
                                MessageBox.Show(
                                    "The stack you are deleting is currently loaded. If you proceed, the stack will also be cleared. Are you sure you want to proceed?",
                                    "Confirmation",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result1 == DialogResult.Yes)
                            {
                                // Clear the stack elements collection
                                if (stackGrid.RowCount > 0)
                                {
                                    //Clear the collection
                                    stackElements.Clear();
                                }

                                // Clear out current stack settings
                                stackID = -1;
                                txtStackName.Text = "None Selected";

                                // Update stack entries count label
                                txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);                             
                            }
                            else okToGo = false;
                        }

                        // Proceed as long as operater did not opt out
                        if (okToGo)
                        {
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
                        }
                    }
                }
                // Check for Activate Stack operation
                else if (dr == DialogResult.Yes)
                {
                    stackIndex = loadStack.StackIndex;
                    if (stacks.Count > 0)
                    {
                        stackIndex = loadStack.StackIndex;
                        Double stackID = loadStack.StackID;
                        String stackDescription = loadStack.StackDesc;

                        // Clear the collection
                        activateStackElements.Clear();

                        // Get the stack ID and load the selected collection
                        StackModel selectedActivateStack = stacksCollection.GetStackMetadata(stacks, stackIndex);

                        // Load the collection
                        activateStackElementsCollection.GetStackElementsCollection(selectedActivateStack.ixStackID);
                        activateStackElements = activateStackElementsCollection.stackElements;

                        // Activate the specified stack
                        ActivateStack(stackID, stackDescription, activateStackElementsCollection, activateStackElements);
                    }
                }
                //Refresh the list of available stacks
                RefreshStacksList();
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred during stack load: " + ex.Message);
                log.Debug("frmMain Exception occurred during stack load", ex);
            }
        }

        // Method for handing click on Save & Activate stack button
        private void btnSaveActivateStack_Click(object sender, EventArgs e)
        {
            try
            {
                if (stackElements.Count > 0)
                {
                    // Only display dialog if checkbox for prompt for info is checked
                    if (cbPromptForInfo.Checked == true)
                    {
                        DialogResult dr = new DialogResult();
                        FrmSaveStack saveStack = new FrmSaveStack(stackID, stackDescription);

                        saveStack.EnableShowControls = enableShowSelectControls;

                        dr = saveStack.ShowDialog();

                        // Will only get here if Prompt for Info checkbox is checked
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
                            stackMetadata.ConceptID = conceptID;
                            stackMetadata.ConceptName = conceptName;
                            stackMetadata.Notes = "Not currently used";

                            // Save out the top level metadata for the stack
                            stacksCollection.SaveStack(stackMetadata);

                            // Save out stack elements; specify stack ID, and set flag to delete existing elements before adding
                            stackElementsCollection.SaveStackElementsCollection(stackMetadata.ixStackID, true);

                            // Update stack entries count label & name label
                            txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                            //txtStackName.Text = stackDescription + " [ID: " + Convert.ToString(stackID) + "]";
                            txtStackName.Text = stackDescription;

                            // Call method to save stack out to MSE
                            ActivateStack(stackID, stackDescription, stackElementsCollection, stackElements);
                        }
                    }
                    else if ((cbPromptForInfo.Checked == false) && (stackID > 0))
                    {
                        // Instantiate a new top-level stack metadata model
                        StackModel stackMetadata = new StackModel();

                        stackMetadata.ixStackID = stackID;
                        stackMetadata.StackName = stackDescription;

                        stackMetadata.StackType = 0;
                        stackMetadata.ShowName = currentShowName;
                        stackMetadata.ConceptID = conceptID;
                        stackMetadata.ConceptName = conceptName;
                        stackMetadata.Notes = "Not currently used";

                        // Save out the top level metadata for the stack
                        stacksCollection.SaveStack(stackMetadata);

                        // Save out stack elements; specify stack ID, and set flag to delete existing elements before adding
                        stackElementsCollection.SaveStackElementsCollection(stackMetadata.ixStackID, true);

                        // Update stack entries count label & name label
                        //txtStackEntriesCount.Text = Convert.ToString(stackElements.Count);
                        //txtStackName.Text = stackDescription + " [ID: " + Convert.ToString(stackID) + "]";

                        // Call method to save stack out to MSE
                        ActivateStack(stackID, stackDescription, stackElementsCollection, stackElements);
                    }

                }

                // Set status strip
                toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
                // toolStripStatusLabel.Text = "Status Logging Message: Stack saved out to database and activated";
                toolStripStatusLabel.Text = String.Format("Status Logging Message: Stack \"{0}\" saved out to database and activated", stackDescription);
            }
                    
            catch (Exception ex)
            {
                log.Debug("Exception occurred", ex);
                log.Error("Exception occurred while trying to save and activate group: " + ex.Message);
            }
        }

        /// <summary>
        /// Mathod to activate the specified stack (can be working stack or selected stack)
        /// </summary>
        private void ActivateStack(Double stack_ID, string stack_Description, StackElementsCollection _stackElementsCollection, BindingList<StackElementModel> _stackElements)
        {
            try
            {    
                    // MSE OPERATION
                    string groupSelfLink = string.Empty;

                    // Get playlists directory URI based on current show
                    string showPlaylistsDirectoryURI = show.GetPlaylistDirectoryFromShow(topLevelShowsDirectoryURI, currentShowName);

                    // Iterate through the races in the stack to build the preview collection, then call methods to create group containing elements
                    // Clear out the existing race preview collection
                    racePreview.Clear();

                    string raceBoardTypeDescription = string.Empty;
                    Int16 candidatesToReturn = 0;
                    Int16 dataType = 0;
                    string BOPHeader = String.Empty;
                    
                    // Build the Race Preview collection - contains strings for each race in the group/stack
                    // Iterate through each race in the stack to build the race preview command strings collection                    
                    for (int i = 0; i < _stackElements.Count; ++i)
                    {
                        switch (_stackElements[i].Stack_Element_Type)
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
                                GetRaceData(_stackElements[i].State_Number, _stackElements[i].Race_Office, _stackElements[i].CD, _stackElements[i].Election_Type, candidatesToReturn);

                                // Check for data returned for race
                                if (raceData.Count > 0)
                                {
                                    // Instantiate and set the values of a race preview element
                                    //RacePreviewModel newRacePreviewElement = new RacePreviewModel();

                                    // Set the name of the element for the group
                                    newRacePreviewElement.Raceboard_Description = _stackElements[i].Listbox_Description + " - " + raceBoardTypeDescription;
                                    //newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + stackElements[i].Listbox_Description;

                                    // Set FIELD_TYPE value - stack ID plus stack index
                                    newRacePreviewElement.Raceboard_Type_Field_Text = stack_ID.ToString() + "|" + i.ToString();

                                    // Call method to assemble the race data into the required command string for the raceboards scene
                                    newRacePreviewElement.Raceboard_Preview_Field_Text = GetRacePreviewString(_stackElements[i], candidatesToReturn);

                                    // Append the preview element to the race preview collection
                                    racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                }
                                break;

                            case (Int16)DataTypes.Exit_Polls:

                                string epType = _stackElements[i].Race_RecordType[0].ToString();
                                Int32 epID = _stackElements[i].ExitPoll_mxID;
                                string st = _stackElements[i].State_Mnemonic;
                                string ofc = _stackElements[i].Office_Code;
                                Int32 jCde = _stackElements[i].County_Number;
                                Int16 rowNum = _stackElements[i].ExitPoll_xRow;
                                Int32 subID = _stackElements[i].ExitPoll_SubsetID;
                                string eType = _stackElements[i].Election_Type;
                                string q = _stackElements[i].ExitPoll_ShortMxLabel;

                                StackElementModel stElement = new StackElementModel();
                                stElement = _stackElements[i];

                                // Setup the referendums collection
                                var records = ExitPollDataCollection.GetExitPollDataCollection(ElectionsDBConnectionString, epType, epID, st, ofc, (short)jCde, rowNum, (short)subID, eType);

                                // Check for data returned for race
                                if (records.Count > 0)
                                {

                                    // Set the name of the element for the group
                                    newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + _stackElements[i].Listbox_Description;

                                    // Set FIELD_TYPE value - stack ID plus stack index
                                    newRacePreviewElement.Raceboard_Type_Field_Text = stack_ID.ToString() + "|" + i.ToString();

                                    // Call method to assemble the race data into the required command string for the raceboards scene
                                    newRacePreviewElement.Raceboard_Preview_Field_Text = GetExitPollPreviewString(records, stElement);

                                    // Append the preview element to the race preview collection
                                    racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                }
                                break;

                            case (Int16)DataTypes.Balance_of_Power:

                                
                                // Set the name of the element for the group
                                newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + _stackElements[i].Listbox_Description;

                                // Set FIELD_TYPE value - stack ID plus stack index
                                newRacePreviewElement.Raceboard_Type_Field_Text = stack_ID.ToString() + "|" + i;

                                // Call method to assemble the race data into the required command string for the raceboards scene
                                newRacePreviewElement.Raceboard_Preview_Field_Text = GetBOPPreviewString(_stackElements[i], BOPHeader);
                                
                                // Append the preview element to the race preview collection
                                racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                                
                                break;

                            case (Int16)DataTypes.Referendums:

                                // Setup the referendums collection
                                this.referendumsDataCollection  = new ReferendumsDataCollection();
                                this.referendumsDataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                                referendumsData = referendumsDataCollection.GetReferendumsDataCollection(_stackElements[i].State_Number, _stackElements[i].Race_Office);

                                ReferendumDataModel refData = new ReferendumDataModel();

                                // Check for data returned for race
                                if (referendumsData.Count > 0)
                                {

                                    refData = referendumsData[1];

                                    // Set the name of the element for the group
                                    newRacePreviewElement.Raceboard_Description = raceBoardTypeDescription + ": " + _stackElements[i].Listbox_Description;

                                    // Set FIELD_TYPE value - stack ID plus stack index
                                    newRacePreviewElement.Raceboard_Type_Field_Text = stack_ID.ToString() + "|" + i.ToString();

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

                    // Log if the URI could not be resolved
                    if (showPlaylistsDirectoryURI == string.Empty)
                    {
                        log.Error("Could not resolve Show Playlist Directory URI");
                        log.Debug("Could not resolve Show Playlist Directory URI");
                    }

                    // Get templates directory URI based on current show
                    string showTemplatesDirectoryURI = show.GetTemplateCollectionFromShow(topLevelShowsDirectoryURI, currentShowName);

                    // Log if the URI could not be resolved
                    if (showTemplatesDirectoryURI == string.Empty)
                    {
                        log.Error("Could not resolve Show Templates Directory URI");
                        log.Debug("Could not resolve Show Templates Directory URI");
                    }

                    // Check for a playlist in the VDOM with the specified name & return the Alt link; if the playlist doesn't exist, create it first
                    if (playlist.CheckIfPlaylistExists(showPlaylistsDirectoryURI, currentPlaylistName) == false)
                    {
                        playlist.CreatePlaylist(showPlaylistsDirectoryURI, currentPlaylistName);
                    }

                    // Check for a playlist in the VDOM with the specified name & return the Down link
                    // Delete the group so it can be re-created
                    string playlistDownLink = playlist.GetPlaylistDownLink(showPlaylistsDirectoryURI, currentPlaylistName);
                    if (playlistDownLink != string.Empty)
                    {
                        // Get the self link to the specified group
                        groupSelfLink = group.GetGroupSelfLink(playlistDownLink, stack_Description);

                        // Delete the group if it exists
                        if (groupSelfLink != string.Empty)
                        {
                            group.DeleteGroup(groupSelfLink);
                        }

                        // Create the group
                        REST_RESPONSE restResponse = group.CreateGroup(playlistDownLink, stack_Description);

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
                                StackElementModel selectedStackElement = _stackElementsCollection.GetStackElement(_stackElements, i);

                                //Set template ID
                                string templateID = selectedStackElement.Stack_Element_TemplateID;

                                //Set page number
                                string pageNumber = i.ToString();

                                //Gets the URI's for the given show
                                GET_URI getURI = new GET_URI();

                                //Get the show info
                                //Get the URI to the show elements collection
                                elementCollectionURIShow = show.GetElementCollectionFromShow(topLevelShowsDirectoryURI, currentShowName);

                                // Log if the URI could not be resolved
                                if (elementCollectionURIShow == string.Empty)
                                {
                                    log.Error("Could not resolve Show Elements Collection URI");
                                    log.Debug("Could not resolve Show Elements Collection URI");
                                }


                                //Get the URI to the show templates collection
                                templateCollectionURIShow = show.GetTemplateCollectionFromShow(topLevelShowsDirectoryURI, currentShowName);

                                // Log if the URI could not be resolved
                                if (templateCollectionURIShow == string.Empty)
                                {
                                    log.Error("Could not resolve Show Templates Collection URI");
                                    log.Debug("Could not resolve Show Templates Collection URI");
                                }

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

                                // Check for element collection URI for the specified playlist
                                if (elementCollectionURIPlaylist == null)
                                {                                
                                    log.Error("Could not resolve URI for specified playlist");
                                    log.Debug("Could not resolve URI for specified playlist");
                                }


                                // Set the data values as name/value pairs
                                // Get the element from the collection
                                racePreviewElement = racePreview[i];

                                // Add the element to the group
                                // NOTE: Currently hard-wired for race boards - will need to be extended to support varying data types
                                Dictionary<string, string> nameValuePairs =
                                    new Dictionary<string, string> { { TemplateFieldNames.RaceBoard_Template_Preview_Field, racePreviewElement.Raceboard_Preview_Field_Text }, 
                                                                         { TemplateFieldNames.RaceBoard_Template_Type_Field, stack_ID.ToString() + "|" + pageNumber } };

                                // Instance the element management class
                                MANAGE_ELEMENTS element = new MANAGE_ELEMENTS();

                                // Create the new element
                                element.createNewElement(i.ToString() + ": " + racePreviewElement.Raceboard_Description, elementCollectionURIPlaylist, templateModel, nameValuePairs, defaultTrioChannel);
                            }
                        }
                    }
                    // Log if the URI could not be resolved
                    else
                    {
                        log.Error("Could not resolve Playlist Down link");
                        log.Debug("Could not resolve Playlist Down link");
                    }
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
                        // Modified 10/13/2016 to support Libertarian candidates
                        else if (raceData[i].CandidatePartyID.ToUpper() == "LIB")
                        {
                            previewField = previewField + "3";
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
                newStackElement.Stack_Element_TemplateID = GetTemplate(conceptID, eType);

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
        private void btnAddExitPoll_Click(object sender, EventArgs e)
        {
            AddExitPoll();
        }
        private void availableExitPollsGrid_DoubleClick(object sender, EventArgs e)
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
                newStackElement.Stack_Element_TemplateID = GetTemplate(conceptID, (short)StackElementTypes.Exit_Poll_Full_Screen);

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

        // Method to add a referendum
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
                newStackElement.Stack_Element_TemplateID = GetTemplate(conceptID, (short)StackElementTypes.Referendums);
                //093016
                //mseStackElementTypeCollection.GetMSEStackElementType(mseStackElementTypes, (short)StackElementTypes.Referendums).Element_Type_Template_ID;

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

                // Only process if required number of candidates in race
                if (selectCand.candidatesFound)
                {
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
                        AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3,
                            selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
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

                // Calculate element type based on number of candidates
                int eType = numCand * 2;

                string eDesc = "Race Board (" + numCand + "-Way Select)"; 

                // Instantiate new stack element model
                StackElementModel newStackElement = new StackElementModel();

                newStackElement.fkey_StackID = 0;
                newStackElement.Stack_Element_ID = stackElements.Count;
                newStackElement.Stack_Element_Type = (short)eType;
                newStackElement.Stack_Element_Description = eDesc;
                
                // Get the template ID for the specified element type
                newStackElement.Stack_Element_TemplateID = GetTemplate(conceptID, newStackElement.Stack_Element_Type);

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
        // Handler for Graphics Concept dropdown selector change
        private void cbGraphicConcept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (graphicsConceptTypes.Count > 0)
            {
                // Set data members for specifying graphics concept
                conceptID = (short) (cbGraphicConcept.SelectedIndex + 1);
                conceptName = graphicsConceptTypes[cbGraphicConcept.SelectedIndex].ConceptName;

                // Re-assign templates to elements in grid
                if (stackElements.Count > 0)
                {
                    for (int i = 0; i < stackElements.Count; i++)
                    {
                        stackElements[i].Stack_Element_TemplateID = GetTemplate(conceptID,
                            stackElements[i].Stack_Element_Type);
                    }
                }
                // For focus to the grid to get it to repaint
                stackGrid.Refresh();
            }
        }
        
        // Look up Template Name from conceptID and BoardType - used when saving out various graphic element types
        private string GetTemplate(Int16 tempConceptID, Int16 tempElementType)
        {
            string Template = string.Empty;
            for (short  i = 0; i < graphicsConcepts.Count; i++)
            {
                if ((tempConceptID == graphicsConcepts[i].ConceptID) & (tempElementType == (short)graphicsConcepts[i].ElementTypeCode))
                {
                    Template = graphicsConcepts[i].TemplateName;
                }                
            }
            return Template;
        }

        // Method to color any rows in grid red where the template specified is not the default template for the
        // data type and cannot be changed.
        private void stackGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (GetStackGridHighlightEnableFlag((int) e.RowIndex))
            {
                stackGrid.Rows[e.RowIndex].Cells["TemplateID"].Style.BackColor = Color.Red;

            }
            else
            {
                stackGrid.Rows[e.RowIndex].Cells["TemplateID"].Style.BackColor = Color.White;
            }
        }
        
        // Method to determine if the stack grid entry template column should be highlighted to indicate
        // that the graphics concept for that entry cannot be changed to the overall concept selected.
        private Boolean GetStackGridHighlightEnableFlag(int rowIndex)
        {
            Boolean highlightEnable = false;

            // Call method to re-assign templates based on graphics concept selected
            if ((stackElements.Count > 0) && (graphicsConcepts.Count > 0))
            {
                // Loop through graphics concepts
                for (int j = 0; j < graphicsConcepts.Count; j++)
                {
                    if ((graphicsConcepts[j].ElementTypeCode == stackElements[rowIndex].Stack_Element_Type) &&
                       (graphicsConcepts[j].ConceptID == conceptID))
                    {
                        if ((graphicsConcepts[j].AllowConceptChange == false) &&
                            (graphicsConcepts[j].IsBaseConcept == false))
                        {
                            highlightEnable = true;                           
                        }
                        else
                        {
                            highlightEnable = false;
                        }
                    }
                }
            }
            return highlightEnable;
        }

        #endregion

        #region Methods for switching between media sequencers
        // Go to primary media sequencer
        private void usePrimaryMediaSequencerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usePrimaryMediaSequencerToolStripMenuItem.Checked == false)
            {
                if (mseEndpoint2_Enable)
                {
                    usingPrimaryMediaSequencer = false;
                    topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint2 + Properties.Settings.Default.TopLevelShowsDirectory;
                    masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint2 + Properties.Settings.Default.MasterPlaylistsDirectory;

                    lblMediaSequencer.Text = "USING BACKUP MEDIA SEQUENCER: " + mseEndpoint2;
                    lblMediaSequencer.BackColor = System.Drawing.Color.Yellow;
                    usePrimaryMediaSequencerToolStripMenuItem.Checked = false;
                    useBackupMediaSequencerToolStripMenuItem.Checked = true;
                }
            }
            else
            {
                if (mseEndpoint1_Enable)
                {
                    usingPrimaryMediaSequencer = true;
                    topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
                    masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.MasterPlaylistsDirectory;

                    lblMediaSequencer.Text = "USING PRIMARY MEDIA SEQUENCER: " + mseEndpoint1;
                    lblMediaSequencer.BackColor = System.Drawing.Color.White;
                    usePrimaryMediaSequencerToolStripMenuItem.Checked = true;
                    useBackupMediaSequencerToolStripMenuItem.Checked = false;
                }
            }
        }

        // Go to backup media sequencer
        private void useBackupMediaSequencerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (useBackupMediaSequencerToolStripMenuItem.Checked == false)
            {
                if (mseEndpoint1_Enable)
                {
                    usingPrimaryMediaSequencer = true;
                    topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.TopLevelShowsDirectory;
                    masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint1 + Properties.Settings.Default.MasterPlaylistsDirectory;

                    lblMediaSequencer.Text = "USING PRIMARY MEDIA SEQUENCER: " + mseEndpoint1;
                    lblMediaSequencer.BackColor = System.Drawing.Color.White;
                    usePrimaryMediaSequencerToolStripMenuItem.Checked = true;
                    useBackupMediaSequencerToolStripMenuItem.Checked = false;
                }

            }
            else
            {
                if (mseEndpoint2_Enable)
                {
                    usingPrimaryMediaSequencer = false;
                    topLevelShowsDirectoryURI = Properties.Settings.Default.MSEEndpoint2 + Properties.Settings.Default.TopLevelShowsDirectory;
                    masterPlaylistsDirectoryURI = Properties.Settings.Default.MSEEndpoint2 + Properties.Settings.Default.MasterPlaylistsDirectory;

                    lblMediaSequencer.Text = "USING BACKUP MEDIA SEQUENCER: " + mseEndpoint2;
                    lblMediaSequencer.BackColor = System.Drawing.Color.Yellow;
                    usePrimaryMediaSequencerToolStripMenuItem.Checked = false;
                    useBackupMediaSequencerToolStripMenuItem.Checked = true;
                }
            }
        }
        #endregion

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cbPromptForInfo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            if (stackGrid.Rows.Count > 0)
            {
                //RaceDataModel rd = new RaceDataModel();
                //DataTable rd = new DataTable();
                BindingList<RaceDataModel> rd = new BindingList<RaceDataModel>();

                //Get the selected race list object
                int currentRaceIndex = stackGrid.CurrentCell.RowIndex;
                short stateNumber = stackElements[currentRaceIndex].State_Number;
                short cd = stackElements[currentRaceIndex].CD;
                string raceOffice = stackElements[currentRaceIndex].Office_Code;
                string electionType = stackElements[currentRaceIndex].Election_Type;
                int ctr = (int)stackElements[currentRaceIndex].Stack_Element_Type;

                ctr = (ctr + 1) / 2 ;
                rd = GetRaceData (stateNumber, raceOffice, cd, electionType, (short)ctr);
                string outStr = GetRaceBoardMapkeyStr(rd);
                
            }
        }

        private void btnSaveStack_Click_1(object sender, EventArgs e)
        {

        }

        public string GetRaceBoardMapkeyStr(BindingList<RaceDataModel> raceData)
        {
            //Example of a 2 way raceboard with the Dem candidate winning and adding a gain

            //USGOV99991 ^ USGOV99992 ~state = New York; race = CD02; precincts = 10; office = house; racemode = 1 ~
            //name = candidate1; party = 0; incum = 0; vote = 3000; percent = 23.4; check = 0; gain = 0; imagePath = George_Bush | 
            //name = candidate2; party = 1; incum = 0; vote = 5000; percent = 33.4; check = 1; gain = 1; imagePath = barack_obama

            string mapKeyStr = "";

            RaceBoardModel raceBoardData = new RaceBoardModel();

            raceBoardData.state = raceData[0].StateName.Trim();
            raceBoardData.office = raceData[0].OfficeName.Trim();
            raceBoardData.cd = raceData[0].CD.ToString();
            raceBoardData.pctsReporting = raceData[0].PercentExpectedVote.ToString();
            bool called = raceData[0].RaceWinnerCalled;
            TimeSpan fifteenMinutes = 

            if (called)
            {
                DateTime callTime = raceData[0].RaceWinnerCallTime;
                DateTime pollClosingTime = raceData[0].RacePollClosingTime;
                DateTime timeNow = TimeFunctions.GetTime();
                if (callTime < pollClosingTime)
                    callTime = pollClosingTime;
                if ((timeNow - callTime) < 
            }


            for (int i = 0; i < raceData.Count; i++)
            {




            }
            return mapKeyStr;

        }
    }
}
