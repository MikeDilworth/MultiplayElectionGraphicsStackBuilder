﻿//////////////////////////////////////////////////////////////////////////////
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
using System.Text;
using DataInterface.DataAccess;
using DataInterface.DataModel;
using DataInterface.Enums;
using log4net.Appender;
using LogicLayer.Collections;
using LogicLayer.CommonClasses;
using MSEInterface;
using MSEInterface.Constants;
using AsyncClientSocket;
using System.Data.SqlClient;
using System.IO;

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
        public static Boolean PollClosinglockout = false;
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
        Boolean insertNext = true;
        Int32 insertPoint;

        Int16 conceptID;
        string conceptName;

        private Boolean manualEPQuestions = false;
        public Boolean enableShowSelectControls = false;
        public List<EngineModel> vizEngines = new List<EngineModel>();
        public bool builderOnlyMode = false;
        public bool isNonPresidentialPrimary = false;
        public string Network = "";
        public string quot = "\"";
        public string term = "\0";

        public string RBSceneName = "_ELECTIONS/2018/FNC/MIDTERMS/FINALS/16X9_RACEBOARDS";
        public string RaceboardCmd = "";
        public int currentRaceIndex = -1;
        public bool stackLocked = false;
        public string computerName = string.Empty;
        public string configName = string.Empty;
        public string ipAddress = string.Empty;
        public string hostName = string.Empty;

        public List<TabDefinitionModel> tabConfig = new List<TabDefinitionModel>();

        public List<ClientSocket> vizClientSockets = new List<ClientSocket>();

        public string[] lastSceneLoaded = new string[4];
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
        private ReferendumsCollection referendumsCollection;
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
        private GraphicsConceptsCollection graphicsConceptsCollection;
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
                RefreshStacksList(Network);

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

                // Set version number
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                this.Text = String.Format("Election Graphics (Multi-Play) Stack Builder Application  Version {0}", version);

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

            // Get host IP
            ipAddress = HostIPNameFunctions.GetLocalIPAddress();
            hostName = HostIPNameFunctions.GetHostName(ipAddress);
            lblIpAddress.Text = ipAddress;
            lblHostName.Text = hostName;

            usingPrimaryMediaSequencer = true;

            // Log application start
            log.Info("Starting Stack Builder application");

            lblMediaSequencer.Text = "USING PRIMARY MEDIA SEQUENCER: " + Convert.ToString(Properties.Settings.Default.MSEEndpoint1);
            lblMediaSequencer.BackColor = System.Drawing.Color.White;
            usePrimaryMediaSequencerToolStripMenuItem.Checked = true;
            useBackupMediaSequencerToolStripMenuItem.Checked = false;

            // Load config & set network label
            LoadConfig();
            lblNetwork.Text = Network;
            lblConfig.Text = configName;
        }
        private void loadConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            string[] IPs = new string[4] { string.Empty, string.Empty, string.Empty, string.Empty };
            bool[] enables = new bool[4] { false, false, false, false };
            string applicationLogComments = string.Empty;

            //builderOnlyMode = Properties.Settings.Default.builderOnly;
            //Network = Properties.Settings.Default.Network;

            // get configuration info based on computer's IP Address 
            DataTable dtComp = new DataTable();
            string cmdStr = $"SELECT * FROM FE_Devices WHERE IP_Address = '{ipAddress}'";
            dtComp = GetDBData(cmdStr, ElectionsDBConnectionString);
            DataRow row;

            if (dtComp.Rows.Count > 0)
            {
                row = dtComp.Rows[0];
                computerName = row["Name"].ToString() ?? "";
                configName = row["Config1"].ToString() ?? "";
                if (configName == null)
                    configName = "DEFAULT";
            }
            else
                configName = "DEFAULT";

            // get tab enables and mode and network
            bool RBenable = false;
            bool VAenable = false;
            bool BOPenable = false;
            bool REFenable = false;
            builderOnlyMode = false;

            DataTable dtEnab = new DataTable();
            cmdStr = $"SELECT * FROM FE_Tabs WHERE Config = '{configName}'";
            dtEnab = GetDBData(cmdStr, ElectionsDBConnectionString);

            if (dtEnab.Rows.Count > 0)
            {
                row = dtEnab.Rows[0];
               RBenable = Convert.ToBoolean(row["RaceBoards"] ?? 0);
               VAenable = Convert.ToBoolean(row["VoterAnalysis"] ?? 0);
               BOPenable = Convert.ToBoolean(row["BOP"] ?? 0);
               REFenable = Convert.ToBoolean(row["Referendums"] ?? 0);
               builderOnlyMode = Convert.ToBoolean(row["StackBuildOnly"] ?? 0);
               Network = row["Network"].ToString() ?? "";

                applicationLogComments = $"{Network}; Config: {configName}; ";
            }

            this.Size = new Size(1462, 991);
            connectionPanel.Visible = false;
            enginePanel.Visible = false;
            listBox1.Visible = false;
            listBox2.Visible = false;
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
                this.Size = new Size(1462, 1165);

                lblConfig.Text = $"{configName}  {DateTime.Now}";
                lblNetwork.Text = Network;
                connectionPanel.Visible = true;
                enginePanel.Visible = true;
                listBox1.Visible = true;
                listBox2.Visible = true;
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
                if (RBenable)
                {
                    tpRaces.Enabled = true;
                }
                else
                {
                    tpRaces.Enabled = false;

                }
                if (VAenable)
                {
                    tpExitPolls.Enabled = true;
                }
                else
                {
                    tpExitPolls.Enabled = false;
                }
                if (BOPenable)
                {
                    tpBalanceOfPower.Enabled = true;
                }
                else
                {
                    tpBalanceOfPower.Enabled = false;
                }
                if (REFenable)
                {
                    tpReferendums.Enabled = true;
                }
                else
                {
                    tpReferendums.Enabled = false;
                }

                // get viz engine info
                cmdStr = $"SELECT * FROM FE_EngineDefs WHERE Config = '{configName}'";
                DataTable dtEng = new DataTable();
                dtEng = GetDBData(cmdStr, ElectionsDBConnectionString);
                if (dtEng.Rows.Count > 0)
                {
                    row = dtEng.Rows[0];

                    int i = 0;
                    bool done = false;
                    string engineParam;
                    var engineInfo = Properties.Settings.Default["Engine1_IPAddress"];
                    string engineData;
                    string engineStr;

                    // read engine info until no more engines found
                    while (done == false)
                    {
                        EngineModel viz = new EngineModel();
                        i++;
                        engineParam = $"Engine{i}_IPAddress";

                        try
                        {
                            engineStr = $"Eng{i}_";

                            engineData = row[engineStr + "Name"].ToString() ?? "";
                            if (engineData != null)
                            {


                                viz.EngineName = engineData;
                                viz.IPAddress = GetIP(engineData);
                                viz.Port = Convert.ToInt32(row[engineStr + "Port"] ?? 0);
                                viz.enable = Convert.ToBoolean(row[engineStr + "Enable"] ?? 0);
                                viz.id = i;
                                viz.systemIP = System.Net.IPAddress.Parse(viz.IPAddress);


                                /*
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
                                */

                                vizEngines.Add(viz);

                                vizClientSockets.Add(new ClientSocket(viz.systemIP, viz.Port));
                                vizClientSockets[i - 1].DataReceived += vizDataReceived;
                                vizClientSockets[i - 1].ConnectionStatusChanged += vizConnectionStatusChanged;


                                // set viz address labels
                                switch (i)
                                {
                                    case 1:
                                        gbNamelbl1.Text = viz.EngineName;
                                        gbIPlbl1.Text = "IP: " + viz.IPAddress;
                                        gbPortlbl1.Text = "Port: " + viz.Port.ToString();
                                        gbViz1.Visible = true;
                                        gbViz1.Enabled = viz.enable;
                                        gbEng1.Visible = true;
                                        gbEng1.Enabled = viz.enable;
                                        IPs[0] = viz.IPAddress;
                                        enables[0] = viz.enable;
                                        if (viz.enable)
                                            vizClientSockets[i - 1].Connect();
                                        break;

                                    case 2:
                                        gbNamelbl2.Text = viz.EngineName;
                                        gbIPlbl2.Text = "IP: " + viz.IPAddress;
                                        gbPortlbl2.Text = "Port: " + viz.Port.ToString();
                                        gbViz2.Visible = true;
                                        gbViz2.Enabled = viz.enable;
                                        gbEng2.Visible = true;
                                        gbEng2.Enabled = viz.enable;
                                        IPs[1] = viz.IPAddress;
                                        enables[1] = viz.enable;
                                        if (viz.enable)
                                            vizClientSockets[i - 1].Connect();
                                        break;

                                    case 3:
                                        gbNamelbl3.Text = viz.EngineName;
                                        gbIPlbl3.Text = "IP: " + viz.IPAddress;
                                        gbPortlbl3.Text = "Port: " + viz.Port.ToString();
                                        gbViz3.Visible = true;
                                        gbViz3.Enabled = viz.enable;
                                        gbEng3.Visible = true;
                                        gbEng3.Enabled = viz.enable;
                                        IPs[2] = viz.IPAddress;
                                        enables[2] = viz.enable;
                                        if (viz.enable)
                                            vizClientSockets[i - 1].Connect();
                                        break;

                                    case 4:
                                        gbNamelbl4.Text = viz.EngineName;
                                        gbIPlbl4.Text = "IP: " + viz.IPAddress;
                                        gbPortlbl4.Text = "Port: " + viz.Port.ToString();
                                        gbViz4.Visible = true;
                                        gbViz4.Enabled = viz.enable;
                                        gbEng4.Visible = true;
                                        gbEng4.Enabled = viz.enable;
                                        IPs[3] = viz.IPAddress;
                                        enables[3] = viz.enable;
                                        if (viz.enable)
                                            vizClientSockets[i - 1].Connect();
                                        break;

                                    case 5:
                                        gbIPlbl5.Text = "IP: " + viz.IPAddress;
                                        gbPortlbl5.Text = "Port: " + viz.Port.ToString();
                                        gbViz5.Visible = true;
                                        gbViz5.Enabled = viz.enable;
                                        if (viz.enable)
                                            vizClientSockets[i - 1].Connect();
                                        break;

                                    case 6:
                                        gbIPlbl6.Text = "IP: " + viz.IPAddress;
                                        gbPortlbl6.Text = "Port: " + viz.Port.ToString();
                                        gbViz6.Visible = true;
                                        gbViz6.Enabled = viz.enable;
                                        if (viz.enable)
                                            vizClientSockets[i - 1].Connect();
                                        break;

                                }

                                if (i == 4)
                                    done = true;
                            }
                        }
                        catch
                        {
                            // Next engine not found
                            done = true;
                        }
                    }
                }
                //ConnectToVizEngines();

            }

            // get tab enables and mode and network
            //cmdStr = $"SELECT * FROM FE_TabConfig WHERE Config = '{configName}'";
            DataTable dt = new DataTable();
            //dt = GetDBData(cmdStr, ElectionsDBConnectionString);
            string tabName;
            bool enab;

            for (int tabNo = 0; tabNo < 4; tabNo++)
            {
                switch (tabNo)
                {
                    case 0:
                        tabName = "RaceBoards";
                        enab = RBenable;
                        break;
                    case 1:
                        tabName = "VoterAnalysis";
                        enab = VAenable;
                        break;
                    case 2:
                        tabName = "BOP";
                        enab = BOPenable;
                        break;
                    case 3:
                        tabName = "Referendums";
                        enab = REFenable;
                        break;
                    default:
                        tabName = "RaceBoards";
                        enab = RBenable;
                        break;
                }


                cmdStr = $"SELECT * FROM FE_TabConfig WHERE Config = '{configName}' AND TabName = '{tabName}'";
                dt = GetDBData(cmdStr, ElectionsDBConnectionString);

                string tabN;
                TabDefinitionModel td = new TabDefinitionModel();
                List<TabOutputDef> tod = new List<TabOutputDef>();

                string sceneCode;
                string sceneName = "";
                int engine = 0;
                bool[] outEng = new bool[4] { false, false, false, false };

                if (dt.Rows.Count > 0)
                {
                    if (enab)
                        applicationLogComments += $"{tabName}:";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = dt.Rows[i];

                        tabN = row["TabName"].ToString() ?? "";
                        engine = Convert.ToInt32(row["EngineNumber"] ?? 0);
                        sceneCode = row["SceneCode"].ToString() ?? "";

                        TabOutputDef to = new TabOutputDef();
                        to.engine = engine;
                        to.sceneCode = sceneCode;
                        to.sceneName = GetScenePath(sceneCode);
                        tod.Add(to);
                        sceneName += $"{engine}: {GetSceneName(sceneCode)}; ";
                        outEng[engine - 1] = true;
                        applicationLogComments += $" {engine}: {sceneCode}";

                    }
                    td.tabName = tabName;
                    td.showTab = enab;
                    //td.outputEngine[engine - 1] = true;
                    td.outputEngine = outEng;
                    //td.engineSceneDef += $"{engine}: {sceneName}; ";
                    td.engineSceneDef = sceneName;
                    td.TabOutput.AddRange(tod);
                    tabConfig.Add(td);
                    if (enab)
                        applicationLogComments += $"; ";

                }
                else
                {
                    //TabOutputDef to = new TabOutputDef();
                    td.tabName = tabName;
                    td.showTab = enab;
                    td.engineSceneDef = "";
                    td.TabOutput.AddRange(tod);
                    tabConfig.Add(td);

                }

            }

            SetOutput(0);


            // Initial load all scenes
            for (int index = 0; index < 4; index++)
            {
                for (int i = 0; i < tabConfig[index].TabOutput.Count; i++)
                {
                    string scenename = tabConfig[index].TabOutput[i].sceneName;
                    int engine = tabConfig[index].TabOutput[i].engine;
                    if (vizEngines.Count > 0)
                    {
                        if (vizEngines[engine - 1].enable)
                        {
                            LoadScene(scenename, engine);
                        }
                    }
                }
            }

            // Make entry into applications log
            ApplicationSettingsFlagsAccess applicationSettingsFlagsAccess = new ApplicationSettingsFlagsAccess();

            // Post application log entry
            applicationSettingsFlagsAccess.ElectionsDBConnectionString = ElectionsDBConnectionString;
            applicationSettingsFlagsAccess.PostApplicationLogEntry(
                Properties.Settings.Default.ApplicationName,
                Properties.Settings.Default.ApplicationName,
                hostName,
                ipAddress,
                enables[0],
                IPs[0],
                enables[1],
                IPs[1],
                "Launched application",
                Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version),
                Properties.Settings.Default.ApplicationID,
                applicationLogComments,
                System.DateTime.Now
            //enables[2],
            //IPs[2],
            //enables[3],
            //IPs[3]

            );
        }

        public void ConnectToVizEngines()
        {

            for (int i = 0; i < vizEngines.Count; i++)
            {
                // Connect to the ClientSocket; call-backs for connection status will indicate status of client sockets
                if (vizEngines[i].enable)
                {
                    vizClientSockets[i].AutoReconnect = true;
                    vizClientSockets[i].Connect();
                }

            }
        }

        public string GetIP(string name)
        {
            DataTable dt = new DataTable();
            string cmdStr = $"SELECT * FROM FE_Devices WHERE Name = '{name}'";
            dt = GetDBData(cmdStr, ElectionsDBConnectionString);

            DataRow row = dt.Rows[0];
            return row["IP_Address"].ToString() ?? "";

        }
        public string GetSceneName(string sceneCode)
        {
            DataTable dt = new DataTable();
            string cmdStr = $"SELECT * FROM FE_Scenes WHERE SceneCode = '{sceneCode}'";
            dt = GetDBData(cmdStr, ElectionsDBConnectionString);
            string[] sceneNames;
            string[] strSeparator = new string[] { "/" };
            string sceneName = "";


            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string scenePath = row["ScenePath"].ToString() ?? "";
                sceneNames = scenePath.Split(strSeparator, StringSplitOptions.None);
                int i = sceneNames.Length;
                sceneName = sceneNames[i - 1];
                return sceneName;
                //return row["ScenePath"].ToString() ?? "";

            }
            else
                return "";
        }
        public string GetScenePath(string sceneCode)
        {
            DataTable dt = new DataTable();
            string cmdStr = $"SELECT * FROM FE_Scenes WHERE SceneCode = '{sceneCode}'";
            dt = GetDBData(cmdStr, ElectionsDBConnectionString);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string scenePath = row["ScenePath"].ToString() ?? "";
                return row["ScenePath"].ToString() ?? "";
            }
            else
                return "";
        }

        private void vizDataReceived(ClientSocket sender, byte[] data)
        {
            // receive the data and determine the type
            //string vizIP = sender.Ip;
            System.Net.IPAddress IP = sender.Ip;
            int port = sender.Port;
            int i = GetVizEngineNumber(IP, port);
            string resp = System.Text.Encoding.Default.GetString(data);
            //listBox1.Items.Add(resp);

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
            //log.Debug($"Viz Engine {i + 1}: {status}");
        }

        delegate void Invoker(int i);

        public void SetConnectionLED(int i)
        {

            if (this.InvokeRequired)
            {
                // Execute the same method, but this time on the GUI thread
                this.BeginInvoke(new Invoker(SetConnectionLED), i);

                // we return immedeately
                return;
            }


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
            DialogResult result1 = MessageBox.Show("Are you sure you want to exit the Election Graphics (Multi-Play) Stack Builder Application?", "Warning",
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

            if (!builderOnlyMode)
                SetOutput(dataModeSelect.SelectedIndex);
        }

        #endregion

        #region Utility functions
        // Refresh the list of available stacks for the grid

        private void RefreshStacksList(string stackNamePrefix)
        {
            try
            {
                // Setup the available stacks collection
                this.stacksCollection = new StacksCollection();
                this.stacksCollection.MainDBConnectionString = GraphicsDBConnectionString;
                stacks = this.stacksCollection.GetStackCollection(stackNamePrefix);
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Exception occurred during stacks list refresh: " + ex.Message);
                log.Debug("frmMain Exception occurred during stacks list refresh", ex);
            }
        }
        #endregion

        #region Data refresh functions
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
                // Setup the application flags
                this.applicationSettingsFlagsCollection = new ApplicationSettingsFlagsCollection();
                this.applicationSettingsFlagsCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
                applicationFlags = this.applicationSettingsFlagsCollection.GetFlagsCollection();

                ApplicationSettingsFlagsModel flags = null;
                flags = applicationFlags[0];
                UseSimulatedTime = flags.UseSimulatedElectionDayTime;
                PollClosinglockout = flags.PollClosingLockoutEnable;
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
                if (scfm == (short)SpecialCaseFilterModes.Next_Poll_Closing_States_Only)
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
                raceData = this.raceDataCollection.GetRaceDataCollection(-1, "P", 0, "G", 1, false, 0, 0, 0, 0);
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
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_1_Way;
                string seDescription = "Race Board (1-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                if (insertNext == true)
                {
                    AddRaceBoardToStack(seType, seDescription, seDataType);
                }
                else
                {
                }
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
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_2_Way;
                string seDescription = "Race Board (2-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }

        // Handler for Add 3-Way race board button
        private void btnAddRace3Way_Click(object sender, EventArgs e)
        {
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_3_Way;
                string seDescription = "Race Board (3-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }

        private void btnAddRace4Way_Click(object sender, EventArgs e)
        {
            // Check for correct graphics concept selected
            //if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            //{
            //    MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
            //        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex != (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select the 32x9 (4-10) graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_4_Way;
                string seDescription = "Race Board (4-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }

        // Handler for add Top 5 Candidates board button
        private void btnAddRace5Way_Click(object sender, EventArgs e)
        {
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex != (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select the 32x9 (4-10) graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_5_Way;
                string seDescription = "Race Board (5-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }
  
        // Handler for add Top 6 Candidates board button
        private void btnAddRace6Way_Click(object sender, EventArgs e)
        {
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex != (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select the 32x9 (4-10) graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_6_Way;
                string seDescription = "Race Board (6-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }

        // Handler for add Top 7 Candidates board button
        private void btnAddRace7Way_Click(object sender, EventArgs e)
        {
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex != (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select the 32x9 (4-10) graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_7_Way;
                string seDescription = "Race Board (7-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }

        // Handler for add Top 8 Candidates board button
        private void btnAddRace8Way_Click(object sender, EventArgs e)
        {
            // Check for correct graphics concept selected
            if (cbGraphicConcept.SelectedIndex != (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
            {
                MessageBox.Show("You must first select the 32x9 (4-10) graphics concept from the drop-down before specifying this board type",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Int16 seType = (short)StackElementTypes.Race_Board_8_Way;
                string seDescription = "Race Board (8-Way)";
                Int16 seDataType = (int)DataTypes.Race_Boards;

                AddRaceBoardToStack(seType, seDescription, seDataType);
            }
        }

        // Generic method to add a race board to a stack
        private void AddRaceBoardToStack(Int16 stackElementType, string stackElementDescription, Int16 stackElementDataType)
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
                newStackElement.Stack_Element_Data_Type = stackElementDataType;
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
                // Deleted for 2020 General Election
                // newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = selectedRace.Race_Office;
                // Modified for 2020 General Election
                //newStackElement.Race_District = selectedRace.CD;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = selectedRace.Race_PollClosingTime;
                newStackElement.Race_UseAPRaceCall = selectedRace.Race_UseAPRaceCall;

                //Specific to exit polls - set to default values
                // Modified for 2020 General Election
                /*
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
                */


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
            Int16 seDataType = (int)DataTypes.Race_Boards;

            Int16 i = 0;
            foreach (DataGridViewRow rowNum in availableRacesGrid.Rows)
            {
                availableRacesGrid.CurrentCell = availableRacesGrid.Rows[i].Cells[0];
                AddRaceBoardToStack(seType, seDescription, seDataType);
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
                case Keys.Q:
                    if (e.Control == true)
                        btnAddAll_Click(sender, e);
                    break;
                // Add All Candidates Graphic
                case Keys.A:
                    if (e.Control == true)
                        btnAllCandidates_Click(sender, e);
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
                // Added 02/20/2020
                case Keys.D5:
                    if (e.Control == true)
                        btnAddRace5Way_Click(sender, e);
                    break;
                case Keys.D6:
                    if (e.Control == true)
                        btnAddRace6Way_Click(sender, e);
                    break;
                case Keys.D7:
                    if (e.Control == true)
                        btnAddRace7Way_Click(sender, e);
                    break;
                case Keys.D8:
                    if (e.Control == true)
                        btnAddRace8Way_Click(sender, e);
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
                        btnClearStack_Click(sender, e);
                    break;
                case Keys.Space:
                    if (e.Control == true)
                        btnTake_Click(sender, e);
                    break;
                case Keys.O:
                    if (e.Control == true)
                        btnSaveStack_Click(sender, e);
                    break;
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
            else if (e.KeyChar == ' ')
                btnTake_Click(sender, e);

        }

        #endregion

        #region Stack manipulation methods
        /// <summary>
        /// Stack manipulation methods
        /// </summary>
        /// 
        // 10/08/2018 Handler for change to graphics concept; previously, was not being set when concept was changed via drop-down
        private void cbGraphicConcept_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            conceptID = graphicsConceptTypes[cbGraphicConcept.SelectedIndex].ConceptID;
            conceptName = graphicsConceptTypes[cbGraphicConcept.SelectedIndex].ConceptName;
        }

        private void btnAllCandidates_Click(object sender, EventArgs e)
        {

        }

        // Handler for Save Stack button
        private void btnSaveStack_Click(object sender, EventArgs e)
        {
            try
            {
                if (stackElements.Count > 0)
                {

                    DialogResult dr = new DialogResult();
                    FrmSaveStack saveStack = new FrmSaveStack(stackID, stackDescription, Network);
                    dr = saveStack.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        // Instantiate a new top-level stack metadata model
                        StackModel stackMetadata = new StackModel();

                        stackID = saveStack.StackId;
                        stackDescription = saveStack.StackDescription;
                        stackMetadata.ixStackID = stackID;
                        stackMetadata.StackName = stackDescription;

                        // This is where we set the stack type - will be used to signal broker for special graphics types (e.g. 8-way board)
                        if (builderOnlyMode == false)
                            stackMetadata.StackType = (short)(10 + dataModeSelect.SelectedIndex);
                        else
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
        private void btnClearStack_Click(object sender, EventArgs e)
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
                frmLoadStack loadStack = new frmLoadStack(Network);

                loadStack.EnableShowControls = enableShowSelectControls;

                RefreshStacksList(Network);

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

                    // 10/08/2018 Set graphics concept in drop-down
                    cbGraphicConcept.SelectedIndex = selectedStack.ConceptID - 1;

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
                RefreshStacksList(Network);
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
                if (stackElements.Count == 0)
                {
                    MessageBox.Show("There are no elements specified in the stack to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                // Added for 2018 Mid-Terms - new concepts for 6-Way & 8-Way boards; need to check for correct number of boards
                // Check for 6-Way boards
                //else if ((cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.Six_Way - 1) && (stackElements.Count != 6))
                //{
                //    MessageBox.Show("There must be exactly six (6) elements in the stack in order to save it for this graphics concept. " +
                //        "Either set the number of boards to six (6), or choose another graphics concept from the drop-down menu.",
                //        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

                // Check for 8-Way boards
                else if (((cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.Eight_Way - 1) || 
                        (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.Eight_Way_Type_2 - 1))  && (stackElements.Count != 8))
                {
                    MessageBox.Show("There must be exactly eight (8) elements in the stack in order to save it for this graphics concept. " +
                        "Either set the number of boards to eight (8), or choose another, compatible graphics concept from the drop-down menu.",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (stackElements.Count > 0)
                {
                    // Only display dialog if checkbox for prompt for info is checked
                    if (cbPromptForInfo.Checked == true)
                    {
                        DialogResult dr = new DialogResult();
                        FrmSaveStack saveStack = new FrmSaveStack(stackID, stackDescription, Network);

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

                            // Modified 02/27/2020 to add network ID prefix to stack name
                            stackMetadata.StackName = Network + "-" + stackDescription;

                            // Modified 02/21/2020 to set to special stack type ID if 8-way multi races selected
                            if ((builderOnlyMode == true) && 
                                ((cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.Eight_Way - 1) ||
                                (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.Eight_Way_Type_2 - 1)))
                                stackMetadata.StackType = 8;
                            else
                                stackMetadata.StackType = 0;

                            //stackMetadata.StackType = 0;
                            stackMetadata.ShowName = Network + "-" + currentShowName;
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
                        
                        // Modified 02/27/2020 to add stack prefix for network
                        stackMetadata.StackName = Network + "-" + stackDescription;

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

                    // Set status strip
                    toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
                    // toolStripStatusLabel.Text = "Status Logging Message: Stack saved out to database and activated";
                    toolStripStatusLabel.Text = String.Format("Status Logging Message: Stack \"{0}\" saved out to database and activated", stackDescription);
                }
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
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_1_Way_Select:
                            raceBoardTypeDescription = "1-Way Select Board";
                            candidatesToReturn = 1;
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_2_Way:
                            raceBoardTypeDescription = "2-Way Board";
                            candidatesToReturn = 2;
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_2_Way_Select:
                            raceBoardTypeDescription = "2-Way Select Board";
                            candidatesToReturn = 2;
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_3_Way:
                            raceBoardTypeDescription = "3-Way Board";
                            candidatesToReturn = 3;
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_3_Way_Select:
                            raceBoardTypeDescription = "3-Way Select Board";
                            candidatesToReturn = 3;
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_4_Way:
                            raceBoardTypeDescription = "4-Way Board";
                            candidatesToReturn = 4;
                            dataType = (Int16)DataTypes.Race_Boards;
                            break;

                        case (Int16)StackElementTypes.Race_Board_4_Way_Select:
                            raceBoardTypeDescription = "4-Way Select Board";
                            candidatesToReturn = 4;
                            dataType = (Int16)DataTypes.Race_Boards;
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

                            bool candidateSelectEnable = false;
                            if ((_stackElements[i].Stack_Element_Type == (short)StackElementTypes.Race_Board_1_Way_Select) || (_stackElements[i].Stack_Element_Type == (short)StackElementTypes.Race_Board_2_Way_Select) ||
                               (_stackElements[i].Stack_Element_Type == (short)StackElementTypes.Race_Board_3_Way_Select) || (_stackElements[i].Stack_Element_Type == (short)StackElementTypes.Race_Board_4_Way_Select))
                            {
                                candidateSelectEnable = true;
                            }

                            // Request the race data for the element in the stack - updates raceData binding list
                            // Modified 02/25/2020 to support preview strings for select boards
                            GetRaceData(_stackElements[i].State_Number, _stackElements[i].Race_Office, _stackElements[i].CD, _stackElements[i].Election_Type, candidatesToReturn, candidateSelectEnable, 
                                _stackElements[i].Race_CandidateID_1, _stackElements[i].Race_CandidateID_2, _stackElements[i].Race_CandidateID_3, _stackElements[i].Race_CandidateID_4);

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

                                // Call method to assemble the race data into the required preview command string for the raceboards scene
                                newRacePreviewElement.Raceboard_Preview_Field_Text = GetRacePreviewString(_stackElements[i], candidatesToReturn);

                                // Append the preview element to the race preview collection
                                racePreviewCollection.AppendRacePreviewElement(newRacePreviewElement);
                            }
                            break;

                        case (Int16)DataTypes.Exit_Polls:
                            // Modified for 2020 General Election
                            // string epType = _stackElements[i].Race_RecordType[0].ToString();
                            string epType = "";
                            // Int32 epID = _stackElements[i].ExitPoll_mxID;
                            Int32 epID = 0;
                            string st = _stackElements[i].State_Mnemonic;
                            string ofc = _stackElements[i].Office_Code;
                            Int32 jCde = _stackElements[i].County_Number;
                            // Int16 rowNum = _stackElements[i].ExitPoll_xRow;
                            Int16 rowNum = 0;
                            // Int32 subID = _stackElements[i].ExitPoll_SubsetID;
                            Int32 subID = 0;
                            string eType = _stackElements[i].Election_Type;
                            // string q = _stackElements[i].ExitPoll_ShortMxLabel;
                            //string q = "";

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
                            this.referendumsDataCollection = new ReferendumsDataCollection();
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
        private BindingList<RaceDataModel> GetRaceData(Int16 stateNumber, string raceOffice, Int16 cd, string electionType, Int16 candidatesToReturn,
            bool candidateSelectEnable, int candidateId1, int candidateId2, int candidateId3, int candidateId4)
        {
            // Setup the master race collection & bind to grid
            //this.raceDataCollection = new RaceDataCollection();
            //this.raceDataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
            raceData = this.raceDataCollection.GetRaceDataCollection(stateNumber, raceOffice, cd, electionType, candidatesToReturn,
            candidateSelectEnable, candidateId1, candidateId2, candidateId3, candidateId4);

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
            // Modified 02/24/2020
            // Race modes: 0=blank; 1 = race called; 2 = just called; 3 = too close to call;  4 = runoff; 5 = race to watch
            // Party codes: 0 = Rep; 1 = Dem; 2 = Ind; 3 = Lib
            // UPDATED 02/24/2020
            // FoxIDs for all candidates separated by ^ then
            // ~state=<state name>; race=<CD # or County Name>; precincts=<Precincts Reporting>; office=<Race Description>; racemode=<see above> ~ 
            // Then for each candidate:
            // name=<candidate name>; party=<party ID>; incum=<0 or 1>; vote=<vote count no commas>; percent=<candidate percentage>; check=<winner status 0 or 1>; gain=<0 or 1>; imagepath=<base filename only> ^
            // Separated by ^

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
                            previewField += "^";
                        }

                        // Prevent blank Fox IDs - use default if none returned
                        if (raceData[i].FoxID.Trim() == "")
                        {
                            raceData[i].FoxID = "USGOV999999";
                        }

                        previewField += raceData[i].FoxID;
                    }
                    previewField += "~";

                    // Get top-level race data
                    // Race district string
                    // Modified for 2020 General Election
                    //string raceDistrict = ((stackElement.Race_Office).Trim() == "H") ? Convert.ToString(stackElement.Race_District) : " ";
                    string raceDistrict = ((stackElement.Race_Office).Trim() == "H") ? Convert.ToString(stackElement.CD) : " ";

                    // FoxIDs for all candidates separated by ^ then
                    // ~state=<state name>; race=<CD # or County Name>; precincts=<Precincts Reporting>; office=<Race Description>; racemode=<see above> ~ 
                    // Add race metadata
                    previewField += "state=" + stackElement.State_Name.ToUpper().Trim() + ";";
                    previewField += "race= ;";
                    previewField += "precincts= ;";
                    previewField += "office= ";

                    // Add race descriptor
                    //Dem primary
                    if (stackElement.Election_Type == "D")
                    {
                        previewField += "DEMOCRATIC PRIMARY";
                    }
                    //Rep primary
                    else if (stackElement.Election_Type == "R")
                    {
                        previewField += "REPUBLICAN PRIMARY";
                    }
                    //Dem caucuses
                    else if (stackElement.Election_Type == "E")
                    {
                        previewField += "DEMOCRATIC CAUCUSES";
                    }
                    //Rep caucuses
                    else if (stackElement.Election_Type == "S")
                    {
                        previewField += "REPUBLICAN CAUCUSES";
                    }
                    // Not a primary or caucus event - build string based on office type
                    else
                    {
                        if (stackElement.Race_Office == "P")
                        {
                            previewField += "President";
                        }
                        else if (stackElement.Race_Office == "G")
                        {
                            previewField += "Governor";
                        }
                        else if (stackElement.Race_Office == "L")
                        {
                            previewField += "Lt. Governor";
                        }
                        else if ((stackElement.Race_Office == "S") | (stackElement.Race_Office == "S2"))
                        {
                            previewField += "Senate";
                        }
                        else if (stackElement.Race_Office == "H")
                        {
                            if (GetStateMetadata(stackElement.State_Number).IsAtLargeHouseState)
                            {
                                previewField += "House At Large";
                            }
                            else
                            {
                                // Modified for 2020 General Election
                                // previewField += "U.S. House CD " + stackElement.Race_District.ToString();
                                previewField += "U.S. House CD " + stackElement.CD.ToString();
                            }
                        }
                    }
                    previewField += ";";

                    previewField += "racemode = " + (Int16)BoardModes.Race_Board_Normal;
                    previewField += ";";

                    // Add delegates at stake - HARD-CODED FOR DEM DELEGATES FOR 2020
                    previewField += "evdel= " + raceData[0].DemDelegatesAtStake.ToString();

                    previewField += "~";

                    // Then for each candidate:
                    // name=<candidate name>; party=<party ID>; incum=<0 or 1>; vote=<vote count no commas>; percent=<candidate percentage>; check=<winner status 0 or 1>; gain=<0 or 1>; imagepath=<base filename only> ^
                    // Separated by ^
                    // Add candidate data - only include name and headshot (no data for preview)
                    for (int j = 0; j < candidatesToReturn; ++j)
                    {
                        if (j != 0)
                        {
                            previewField += "^";
                        }
                        previewField += "name=" + raceData[j].CandidateLastName.Trim() + ";";

                        // Add party ID
                        if (raceData[j].CandidatePartyID.ToUpper() == "REP")
                        {
                            previewField += "party=0;";
                        }
                        else if (raceData[j].CandidatePartyID.ToUpper() == "DEM")
                        {
                            previewField += "party=1;";
                        }
                        // Modified 10/13/2016 to support Libertarian candidates
                        else if (raceData[j].CandidatePartyID.ToUpper() == "LIB")
                        {
                            previewField += "party=3;";
                        }
                        else
                        {
                            previewField += "party=2;";
                        }
                        // Add base filename from headshot path
                        previewField += "imagepath=" + Path.GetFileNameWithoutExtension(Path.GetFileName(raceData[j].HeadshotPathFNC.Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                log.Error("frmMain Preview String Exception occurred: " + ex.Message);
                log.Debug("frmMain Preview String Exception occurred", ex);
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
                previewField = previewField + "0|"; //Checkstate
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

            // Modified for 2020 General Election
            /*
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
            */
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
                            bopType.eType = (short)StackElementTypes.Balance_of_Power_House_Current;
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
                newStackElement.Stack_Element_Data_Type = (Int16)DataTypes.Balance_of_Power;
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
                // Deleted for 2020 General Election
                // newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = string.Empty;
                // Deleted for 2020 General Election
                // newStackElement.Race_District = 0;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = Convert.ToDateTime("2016 11 08");
                newStackElement.Race_UseAPRaceCall = false;

                //Specific to exit polls - set to default values
                // Modified for 2020 General Election
                /*
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
                */
                // Added for 2020 General Election
                newStackElement.VA_Data_ID = "";
                newStackElement.VA_Title = "";
                newStackElement.VA_Type = "";
                newStackElement.VA_Map_Color = "";
                newStackElement.VA_Map_ColorNum = 0;

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
                newStackElement.Stack_Element_Data_Type = (short)DataTypes.Exit_Polls;
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
                // Deleted for 2020 General Election
                // newStackElement.Race_RecordType = selectedPoll.questionType;
                newStackElement.Race_Office = selectedPoll.office;
                // Deleted for 2020 General Election
                // newStackElement.Race_District = selectedPoll.CD;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = Convert.ToDateTime("11/8/2016");
                newStackElement.Race_UseAPRaceCall = false;

                //Specific to exit polls - set to default values
                // Modified for 2020 General Election
                /*
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
                */
                // Added for 2020 General Election
                newStackElement.VA_Data_ID = "";
                newStackElement.VA_Title = "";
                newStackElement.VA_Type = "";
                newStackElement.VA_Map_Color = "";
                newStackElement.VA_Map_ColorNum = 0;

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
                newStackElement.Stack_Element_Data_Type = (short)DataTypes.Referendums;
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
                // Deleted for 2020 General Election
                // newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = selectedReferendum.race_OfficeCode;
                // Deleted for 2020 General Election
                // newStackElement.Race_District = 0;
                newStackElement.Race_CandidateID_1 = 0;
                newStackElement.Race_CandidateID_2 = 0;
                newStackElement.Race_CandidateID_3 = 0;
                newStackElement.Race_CandidateID_4 = 0;
                newStackElement.Race_PollClosingTime = Convert.ToDateTime("11/8/2016");
                newStackElement.Race_UseAPRaceCall = false;

                //Specific to exit polls - set to default values
                // Modified for 2020 General Election
                /*
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
                */
                // Added for 2020 General Election
                newStackElement.VA_Data_ID = "";
                newStackElement.VA_Title = "";
                newStackElement.VA_Type = "";
                newStackElement.VA_Map_Color = "";
                newStackElement.VA_Map_ColorNum = 0;

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
                // Check for correct graphics concept selected
                if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
                {
                    MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
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
                        //selectedCandidate2 = selectCand.Cand2;
                        //cand2Name = selectCand.CandName2;
                        //selectedCandidate3 = selectCand.Cand3;
                        //cand3Name = selectCand.CandName3;
                        //selectedCandidate4 = selectCand.Cand4;
                        //cand4Name = selectCand.CandName4;
                        AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
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

        // Add 2-way select board
        private void btnSelect2_Click(object sender, EventArgs e)
        {
            try
            {
                // Check for correct graphics concept selected
                if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
                {
                    MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
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
                        //selectedCandidate3 = selectCand.Cand3;
                        //cand3Name = selectCand.CandName3;
                        //selectedCandidate4 = selectCand.Cand4;
                        //cand4Name = selectCand.CandName4;
                        AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
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

        // Add 3-way select board
        private void btnSelect3_Click(object sender, EventArgs e)
        {
            try
            {
                // Check for correct graphics concept selected
                if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
                {
                    MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
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
                        //selectedCandidate4 = selectCand.Cand4;
                        //cand4Name = selectCand.CandName4;
                        AddSelectRaceBoardToStack(numCand, selectedCandidate1, selectedCandidate2, selectedCandidate3, selectedCandidate4, cand1Name, cand2Name, cand3Name, cand4Name);
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

        // Add 4-way select board
        private void btnSelect4_Click(object sender, EventArgs e)
        {
            try
            {
                // Check for correct graphics concept selected
                //if (cbGraphicConcept.SelectedIndex == (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
                //{
                //    MessageBox.Show("You must first select a valid, compatible graphics concept from the drop-down before specifying this board type",
                //        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                if (cbGraphicConcept.SelectedIndex != (short)GraphicsConcepts.ThirtyTwo_By_Nine_Five_Ten - 1)
                {
                    MessageBox.Show("You must first select the 32x9 (4-10) graphics concept from the drop-down before specifying this board type",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
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
                newStackElement.Stack_Element_Data_Type = (short)DataTypes.Race_Boards;
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
                // Modified for 2020 General Election
                // newStackElement.Race_RecordType = string.Empty;
                newStackElement.Race_Office = selectedRace.Race_Office;
                // Modified for 2020 General Election
                //newStackElement.Race_District = selectedRace.CD;
                newStackElement.Race_CandidateID_1 = cID1;
                newStackElement.Race_CandidateID_2 = cID2;
                newStackElement.Race_CandidateID_3 = cID3;
                newStackElement.Race_CandidateID_4 = cID4;
                newStackElement.Race_PollClosingTime = selectedRace.Race_PollClosingTime;
                newStackElement.Race_UseAPRaceCall = selectedRace.Race_UseAPRaceCall;

                //Specific to exit polls - set to default values
                // Modified for 2020 General Election
                /*
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
                */

                // Added for 2020 General Election
                newStackElement.VA_Data_ID = "";
                newStackElement.VA_Title = "";
                newStackElement.VA_Type = "";
                newStackElement.VA_Map_Color = "";
                newStackElement.VA_Map_ColorNum = 0;


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
                conceptID = (short)(cbGraphicConcept.SelectedIndex + 1);
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
            for (short i = 0; i < graphicsConcepts.Count; i++)
            {
                // Modified 02/21/2020 for special case of 5-8 candidates graphics concept; stack element type values = 5-way thru 8-way, lookup for template = 5
                //if ((tempElementType >= (short)StackElementTypes.Race_Board_5_Way) && (tempElementType <= (short)StackElementTypes.Race_Board_8_Way))
                if ((tempElementType >= (short)StackElementTypes.Race_Board_4_Way) && (tempElementType <= (short)StackElementTypes.Race_Board_8_Way))
                        tempElementType = (short)StackElementTypes.Race_Board_5_Way;
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
            if (GetStackGridHighlightEnableFlag((int)e.RowIndex))
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

        #region Methods for taking data to air
        private void btnTake_Click(object sender, EventArgs e)
        {
            TakeNext();
        }

        public void TakeNext()
        {
            if (stackGrid.Rows.Count > 0)
            {
                if (currentRaceIndex < stackGrid.RowCount - 1)
                    currentRaceIndex++;
                stackGrid.CurrentCell = stackGrid.Rows[currentRaceIndex].Cells[0];

                TakeCurrent();

            }
        }

        public void TakeCurrent()
        {
            if (stackGrid.Rows.Count > 0)
            {

                currentRaceIndex = stackGrid.CurrentCell.RowIndex;
                Int16 stackElementDataType = (Int16)stackElements[currentRaceIndex].Stack_Element_Data_Type;
                SetOutput(stackElementDataType);

                switch (stackElementDataType)
                {
                    case (short)DataTypes.Race_Boards:
                        TakeRaceBoards();
                        break;

                    case (short)DataTypes.Balance_of_Power:
                        TakeBOP();
                        break;

                    case (short)DataTypes.Referendums:
                        TakeReferendums();
                        break;

                }
            }
        }

        public void LoadScene(string sceneName, int EngineNo)
        {
            string cmd = $"0 RENDERER*MAIN_LAYER SET_OBJECT SCENE*{sceneName}{term}";
            byte[] bCmd = Encoding.UTF8.GetBytes(cmd);
            vizClientSockets[EngineNo - 1].Send(bCmd);
            lastSceneLoaded[EngineNo - 1] = sceneName;
        }

        private void SendToViz(string cmd, int dataType)
        {

            string vizCmd = "";

            //int index = dataModeSelect.SelectedIndex;
            int index = dataType;

            for (int i = 0; i < tabConfig[index].TabOutput.Count; i++)
            {
                string sceneName = tabConfig[index].TabOutput[i].sceneName;
                int engine = tabConfig[index].TabOutput[i].engine;

                // load scene if last scene loaded on this viz is not = sceneName
                if (lastSceneLoaded[engine - 1] != sceneName)
                    LoadScene(sceneName, engine);


                if (index == 0)
                    vizCmd = $"SEND SCENE*{sceneName}*MAP SET_STRING_ELEMENT {quot}CANDIDATE_DATA{quot} {cmd}{term}";

                if (index == 2)
                    vizCmd = $"SEND SCENE*{sceneName}*MAP SET_STRING_ELEMENT {quot}BOP_DATA{quot} {cmd}{term}";

                if (index == 3)
                    vizCmd = $"SEND SCENE*{sceneName}*MAP SET_STRING_ELEMENT {quot}REFERENDUM_DATA{quot} {cmd}{term}";


                if (vizEngines[engine - 1].enable)
                {
                    byte[] bCmd = Encoding.UTF8.GetBytes(vizCmd);
                    if (vizEngines[engine - 1].enable)
                        vizClientSockets[engine - 1].Send(bCmd);
                }
            }

            listBox2.Items.Add(vizCmd);
            listBox2.SelectedIndex = listBox2.Items.Count - 1;

        }
        private void SendToViz2(string sceneName, string cmd, int EngineNo)
        {

            string vizCmd = $"SEND SCENE*{sceneName}*MAP SET_STRING_ELEMENT {quot}CANDIDATE_DATA{quot} {cmd}{term}";

            byte[] bCmd = Encoding.UTF8.GetBytes(vizCmd);
            vizClientSockets[EngineNo - 1].Send(bCmd);
            listBox2.Items.Add(vizCmd);
            listBox2.SelectedIndex = listBox2.Items.Count - 1;

        }

        private void LiveUpdateTimer_Tick(object sender, EventArgs e)
        {
            TakeCurrent();
        }

        private void stackGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            TakeCurrent();
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            LiveUpdateTimer.Enabled = false;
            panel2.BackColor = Color.Navy;
            stackLocked = false;
            LoopTimer.Enabled = false;
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (stackGrid.Rows.Count > 0)
            {
                stackLocked = true;
                //LoadScene(RBSceneName, 1);
                currentRaceIndex = -1;
                stackGrid.CurrentCell = stackGrid.Rows[0].Cells[0];
                panel2.BackColor = Color.Lime;

                //int index = dataModeSelect.SelectedIndex;

                for (int index = 0; index < 4; index++)
                {
                    for (int i = 0; i < tabConfig[index].TabOutput.Count; i++)
                    {
                        string scenename = tabConfig[index].TabOutput[i].sceneName;
                        int engine = tabConfig[index].TabOutput[i].engine;
                        if (vizEngines.Count > 0)
                        {
                            if (vizEngines[engine - 1].enable)
                                LoadScene(scenename, engine);
                        }
                    }
                }

                // if Looping checked start
                if (cbLooping.Checked)
                {
                    if (stackLocked && stackGrid.Rows.Count > 0)
                    {
                        TakeNext();
                        LoopTimer.Enabled = true;
                    }
                }
                else
                {
                    LoopTimer.Enabled = false;
                }

            }
        }

        public static DataTable GetDBData(string cmdStr, string dbConnection)
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Instantiate the connection
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    // Create the command and set its properties
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
                        {
                            cmd.CommandText = cmdStr;
                            //cmd.Parameters.Add("@StackID", SqlDbType.Float).Value = stackID;
                            sqlDataAdapter.SelectCommand = cmd;
                            sqlDataAdapter.SelectCommand.Connection = connection;
                            sqlDataAdapter.SelectCommand.CommandType = CommandType.Text;

                            // Fill the datatable from adapter
                            sqlDataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch //(Exception ex)
            {
                // Log error
                //log.Error("GetDBData Exception occurred: " + ex.Message);
                //log.Debug("GetDBData Exception occurred", ex);
            }

            return dataTable;
        }

        public void SetOutput(int index)
        {
            //int index = dataModeSelect.SelectedIndex;
            gbEngines.Text = $"Engines used for {tabConfig[index].tabName}";
            pbEng1.Visible = tabConfig[index].outputEngine[0];
            pbEng2.Visible = tabConfig[index].outputEngine[1];
            pbEng3.Visible = tabConfig[index].outputEngine[2];
            pbEng4.Visible = tabConfig[index].outputEngine[3];
            lblScenes.Text = $"Scenes: {tabConfig[index].engineSceneDef}";

        }

        private void cbLooping_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLooping.Checked)
            {
                if (stackLocked && stackGrid.Rows.Count > 0)
                {
                    TakeNext();
                    LoopTimer.Enabled = true;
                }
            }
            else
            {
                LoopTimer.Enabled = false;
            }
        }

        private void LoopTimer_Tick(object sender, EventArgs e)
        {
            if (currentRaceIndex >= stackGrid.RowCount - 1)
            {
                currentRaceIndex = 0;
                stackGrid.CurrentCell = stackGrid.Rows[currentRaceIndex].Cells[0];
                TakeCurrent();
            }
            else
                TakeNext();
        }

        #endregion

        #region Raceboards Data processing
        public void TakeRaceBoards()
        {
            BindingList<RaceDataModel> rd = new BindingList<RaceDataModel>();

            //Get the selected race list object
            currentRaceIndex = stackGrid.CurrentCell.RowIndex;
            short stateNumber = stackElements[currentRaceIndex].State_Number;
            short cd = stackElements[currentRaceIndex].CD;
            string raceOffice = stackElements[currentRaceIndex].Office_Code;
            string electionType = stackElements[currentRaceIndex].Election_Type;
            int candidatesToReturn = (int)stackElements[currentRaceIndex].Stack_Element_Type;
            int dataType = (int)stackElements[currentRaceIndex].Stack_Element_Data_Type;
            bool candidateSelectEnable;

            candidatesToReturn = (candidatesToReturn + 1) / 2;
            if ((int)stackElements[currentRaceIndex].Stack_Element_Type % 2 == 0)
                candidateSelectEnable = true;
            else
                candidateSelectEnable = false;

            int cand1 = stackElements[currentRaceIndex].Race_CandidateID_1;
            int cand2 = stackElements[currentRaceIndex].Race_CandidateID_2;
            int cand3 = stackElements[currentRaceIndex].Race_CandidateID_3;
            int cand4 = stackElements[currentRaceIndex].Race_CandidateID_4;

            rd = GetRaceData(stateNumber, raceOffice, cd, electionType, (short)candidatesToReturn, candidateSelectEnable, cand1, cand2, cand3, cand4);

            StateMetadataModel st = GetStateMetadata(stateNumber);
            string stateName = st.State_Name;
            string dataStr = $"{stateName} {rd[0].OfficeName}";


            if (candidatesToReturn > rd.Count)
                candidatesToReturn = rd.Count;

            for (int i = 0; i < candidatesToReturn; i++)
            {
                dataStr += $" - {rd[i].CandidateLastName} {rd[i].CandidateVoteCount}";

            }


            listBox1.Items.Add(dataStr);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

            string outStr = GetRaceBoardMapkeyStr(rd, candidatesToReturn);

            //SendToViz(RBSceneName, outStr, 1);
            SendToViz(outStr, dataType);
            LiveUpdateTimer.Enabled = false;
            LiveUpdateTimer.Enabled = true;

        }


        public string GetRaceBoardMapkeyStr(BindingList<RaceDataModel> raceData, int numCand)
        {
            //Example of a 2 way raceboard with the Dem candidate winning and adding a gain

            //USGOV99991 ^ USGOV99992 ~state = New York; race = CD02; precincts = 10; office = house; racemode = 1 ~
            //name = candidate1; party = 0; incum = 0; vote = 3000; percent = 23.4; check = 0; gain = 0; imagePath = George_Bush | 
            //name = candidate2; party = 1; incum = 0; vote = 5000; percent = 33.4; check = 1; gain = 1; imagePath = barack_obama

            //“raceMode” – 0 (not called), 1(race called), 2 (just called), 3(too close to call), 4 (runoff), 5 (race to watch)

            string mapKeyStr = "";

            // Gets either simulated or actual time based on flag
            DateTime timeNow = TimeFunctions.GetTime();
            DateTime pollClosingTime = raceData[0].RacePollClosingTime;

            RaceBoardModel raceBoardData = new RaceBoardModel();

            short stateNumber = stackElements[currentRaceIndex].State_Number;
            StateMetadataModel st = GetStateMetadata(stateNumber);
            //raceBoardData.state = raceData[0].StateName.Trim();
            raceBoardData.state = st.State_Name;

            if (raceData[0].CD == 0)
                raceBoardData.cd = string.Empty;
            else
                raceBoardData.cd = raceData[0].CD.ToString();

            TimeSpan fifteenMinutes = new TimeSpan(0, 15, 0);
            bool candidateCalledWinner = false;
            DateTime raceCallTime = raceData[0].RaceWinnerCallTime;
            bool raceCalled = false;

            // get office strings
            raceBoardData.office = GetOfficeStr(raceData[0]);

            if (raceData[0].Office != "H")
            {
                raceBoardData.pctsReporting = raceData[0].PercentExpectedVote.ToString();

            }
            else
            {
                int temp = (int)(raceData[0].PrecinctsReporting * 100.0 / raceData[0].TotalPrecincts);
                raceBoardData.pctsReporting = temp.ToString();
            }

            for (int i = 0; i < numCand; i++)
            {
                candidateData_RB candidate = new candidateData_RB();
                mapKeyStr += raceData[i].FoxID;
                if (i < numCand - 1)
                    mapKeyStr += "^";

                candidate.lastName = raceData[i].CandidateLastName;
                candidate.firstName = raceData[i].CandidateFirstName;

                if (Network == "FNC")
                {
                    if (raceData[i].UseHeadshotFNC)
                    {
                        candidate.headshot = raceData[i].HeadshotPathFNC;
                    }
                }
                else if (Network == "FBN")
                {
                    if (raceData[i].UseHeadshotFBN)
                    {
                        candidate.headshot = raceData[i].HeadshotPathFBN;
                    }

                }

                // filename only, no path, no extension
                candidate.headshot = Path.GetFileNameWithoutExtension(candidate.headshot);



                // Format vote count as string with commas
                if (raceData[i].CandidateVoteCount > 0)
                    //candidate.votes = string.Format("{0:n0}", raceData[i].CandidateVoteCount);
                    candidate.votes = raceData[i].CandidateVoteCount.ToString();
                else
                    candidate.votes = " ";

                // get candidate percent str
                candidate.percent = GetCandPercent(raceData[i]);
                // Get scene Party Id number from party str
                candidate.party = GetCandParty(raceData[i]);

                // Set candidate incumbent flag
                candidate.incumbent = raceData[i].IsIncumbentFlag == "Y" ? "1" : "0";
                var winnerCandidateId = 0;


                if (timeNow >= pollClosingTime || PollClosinglockout == false)
                {
                    //Check for AP race call
                    if (raceData[i].RaceUseAPRaceCall)
                    {
                        //Check for AP called winner
                        if (raceData[i].cStat.ToUpper() == "W")
                        {
                            raceCalled = true;
                            candidateCalledWinner = true;
                            var raceCallTimeStr = raceData[i].estTS;
                            winnerCandidateId = raceData[i].CandidateID;

                            raceCallTime = GetApRaceCallDateTime(raceCallTimeStr);
                        }
                        else
                        {
                            candidateCalledWinner = false;
                        }
                    }
                    //Check for Fox race call
                    else
                    {
                        winnerCandidateId = raceData[i].RaceWinnerCandidateID;
                        if (raceData[i].RaceWinnerCalled)
                        {
                            raceCalled = true;

                            // If the race was not called by AP, use the Race_WinnerCallTime from the DB
                            raceCallTime = raceData[i].RaceWinnerCallTime;
                            candidateCalledWinner = (winnerCandidateId == raceData[i].RaceWinnerCandidateID);
                        }
                        else
                        {
                            raceCalled = false;
                            candidateCalledWinner = false;
                        }
                    }
                }
                else
                {
                    candidateCalledWinner = false;
                }

                if (winnerCandidateId == raceData[i].CandidateID)
                {
                    candidate.winner = "1";
                    candidate.gain = GetGainFlag(raceData[i]);
                }
                else
                {
                    candidate.winner = "0";
                    candidate.gain = "0";

                }
                raceBoardData.candData.Add(candidate);

            }

            // Set board mode 
            if (raceCalled)
            {

                // if race is called before the polls are closed time then use poll closing time as the race call time
                if (raceCallTime < pollClosingTime)
                    raceCallTime = pollClosingTime;

                // if race is called within 15 minutes 
                if ((timeNow - raceCallTime) < fifteenMinutes)
                    // then Just Called
                    raceBoardData.mode = (int)BoardModes.Race_Board_Just_Called;
                else
                    // Race Called
                    raceBoardData.mode = (int)BoardModes.Race_Board_Race_Called;
            }
            else if (raceData[0].RaceTooCloseToCall)
            {
                // RaceTooCloseToCall flag is set
                raceBoardData.mode = (int)BoardModes.Race_Board_To_Close_To_Call;
            }
            else
            {
                // Not called and not TCTC
                raceBoardData.mode = (int)BoardModes.Race_Board_Normal;
            }


            //USGOV99991 ^ USGOV99992 ~state = New York; race = CD02; precincts = 10; office = house; racemode = 1 ~
            //name = candidate1; party = 0; incum = 0; vote = 3000; percent = 23.4; check = 0; gain = 0; imagePath = George_Bush | 
            //name = candidate2; party = 1; incum = 0; vote = 5000; percent = 33.4; check = 1; gain = 1; imagePath = barack_obama

            //USGOV99991^ USGOV99992 ~ state=New York; race=CD02;precincts=10 ; office=house; racemode=1 ~ name=candidate1; party=0; incum=0; vote=3000; percent=23.4 ; check=0; gain=0; imagePath= George_Bush |name=candidate2; party=1; incum=0; vote=5000; percent=33.4 ; check=1; gain=1; imagePath= barack_obama



            string raceblock = $"~state={raceBoardData.state};race={raceBoardData.cd};precincts={raceBoardData.pctsReporting};office={raceBoardData.office};racemode={raceBoardData.mode}~";

            mapKeyStr += raceblock;

            for (int i = 0; i < numCand; i++)
            {
                mapKeyStr += $"name={raceBoardData.candData[i].firstName} {raceBoardData.candData[i].lastName};party={raceBoardData.candData[i].party};incum={raceBoardData.candData[i].incumbent};";
                //mapKeyStr += $"name={raceBoardData.candData[i].lastName};party={raceBoardData.candData[i].party};incum={raceBoardData.candData[i].incumbent};";
                mapKeyStr += $"vote={raceBoardData.candData[i].votes};percent={raceBoardData.candData[i].percent};check={raceBoardData.candData[i].winner};gain={raceBoardData.candData[i].gain};";
                mapKeyStr += $"imagePath={raceBoardData.candData[i].headshot}";
                if (i < numCand - 1)
                    mapKeyStr += "^";

            }
            return mapKeyStr;

        }

        private string GetCandPercent(RaceDataModel rd)
        {
            string pct = "";
            double pctVote;
            Boolean ShowTenths = ApplicationSettingsFlagsCollection.ShowTenthsofPercent;

            if (rd.CandidateVoteCount > 0)
            {
                pctVote = (rd.CandidateVoteCount * 100) / rd.TotalVoteCount;

                if (pctVote < 1.0)
                {
                    pct = "<1";
                }
                else if (ShowTenths)
                    // showing tenths
                    pct = string.Format("{0:0.0}", pctVote);
                else
                    // show no decimal 
                    pct = string.Format("{0:0}", Math.Round(pctVote, MidpointRounding.AwayFromZero));

            }
            else
                pct = " ";

            return pct;
        }
        private string GetCandParty(RaceDataModel rd)
        {
            string party = " ";
            if (rd.CandidatePartyID == "Rep")
                party = "0";
            else if (rd.CandidatePartyID == "Dem")
                party = "1";
            else if (rd.CandidatePartyID == "Lib")
                party = "3";
            else
                party = "2";

            return party;

        }
        private string GetOfficeStr(RaceDataModel rd)
        {

            string raceOfficeStr = " ";
            // Add race descriptor
            // 05/03/2018 Modified to support non-presidential primary
            if (isNonPresidentialPrimary)
            {
                // 05/02/2018 Added support for non-presidential primary - concatenate state + office
                if (rd.Office == "G")
                {
                    raceOfficeStr = "GOVERNOR";
                }
                else if (rd.Office == "L")
                {
                    raceOfficeStr = "LT. GOVERNOR";
                }
                else if ((rd.Office == "S") | (rd.Office == "S2"))
                {
                    raceOfficeStr = "SENATE";
                }
                else if (rd.Office == "H")
                {
                    if (rd.IsAtLargeHouseState)
                    {
                        raceOfficeStr = "HOUSE AT LARGE";
                    }
                    else
                    {
                        //MD Modified 03/05/2018 to support 2018 primaries
                        //raceOfficeStr = "U.S. House CD " + RaceDistrict.ToString();
                        //raceOfficeStr = "HOUSE CD " + rd.CD.ToString();
                        raceOfficeStr = "HOUSE";
                    }
                }
            }
            else
            {
                //Dem primary
                if (rd.eType == "D")
                {
                    raceOfficeStr = "DEMOCRATIC PRIMARY";
                }
                //Rep primary
                else if (rd.eType == "R")
                {
                    raceOfficeStr = "REPUBLICAN PRIMARY";
                }
                //Dem caucuses
                else if (rd.eType == "E")
                {
                    raceOfficeStr = "DEMOCRATIC CAUCUSES";
                }
                //Rep caucuses
                else if (rd.eType == "S")
                {
                    raceOfficeStr = "REPUBLICAN CAUCUSES";
                }
                // Not a primary or caucus event - build string based on office type
                else
                {
                    if (rd.Office == "P")
                    {
                        raceOfficeStr = "PRESIDENT";
                    }
                    else if (rd.Office == "G")
                    {
                        raceOfficeStr = "GOVERNOR";
                    }
                    else if (rd.Office == "L")
                    {
                        raceOfficeStr = "LT. GOVERNOR";
                    }
                    else if ((rd.Office == "S") | (rd.Office == "S2"))
                    {
                        raceOfficeStr = "SENATE";
                    }
                    else if (rd.Office == "H")
                    {
                        if (rd.IsAtLargeHouseState)
                        {
                            raceOfficeStr = "HOUSE AT LARGE";
                        }
                        else
                        {
                            //MD Modified 03/05/2018 to support 2018 primaries
                            //raceOfficeStr = "U.S. House CD " + RaceDistrict.ToString();
                            //raceOfficeStr = "HOUSE CD " + rd.CD.ToString();
                            raceOfficeStr = "HOUSE";
                        }
                    }
                }
            }
            return raceOfficeStr;
        }

        private string GetGainFlag(RaceDataModel rd)
        {
            bool gainFlag = false;
            // Do gain flag
            if (!rd.RaceIgnoreGain)
            {
                if ((rd.InIncumbentPartyFlag == "N") && ((rd.Office.Trim() == "H") || (rd.Office.Trim() == "S") || (rd.Office.Trim() == "S2")))
                {
                    gainFlag = true;
                }
                else
                {
                    gainFlag = false;
                }
            }
            else
            {
                gainFlag = false;
            }
            if (gainFlag)
                return "1";
            else
                return "0";
        }
        /// <summary>
        /// Method to convert the AP race call time from a string to a datetime value
        /// </summary>
        public static DateTime GetApRaceCallDateTime(String dateTimeStr)
        {
            DateTime apRaceCallDateTimeStr = new DateTime();

            try
            {
                // Convert formatted string to date time
                apRaceCallDateTimeStr = DateTime.ParseExact(dateTimeStr, "yyyyMMdd HHmmtt", null);
            }
            catch (Exception ex)
            {
                log.Error("SimulatedDateTimeAccess Exception occurred while trying to convert string to DateTime value: " + ex.Message);
                log.Debug("SimulatedDateTimeAccess Exception occurred while trying to convert string to DateTime value", ex);
            }
            return apRaceCallDateTimeStr;
        }
        #endregion

        #region Balance Of Power Data Processing
        public void TakeBOP()
        {
            currentRaceIndex = stackGrid.CurrentCell.RowIndex;
            int seType = (int)stackElements[currentRaceIndex].Stack_Element_Type;
            int seDataType = (int)stackElements[currentRaceIndex].Stack_Element_Data_Type;

            string ofc = "H";
            string office = "HOUSE";

            if ((int)stackElements[currentRaceIndex].Stack_Element_Type % 2 == 0)
            {
                ofc = "H";
                office = "HOUSE";

            }
            else
            {
                ofc = "S";
                office = "SENATE";

            }

            DateTime currentTime = TimeFunctions.GetTime();
            string currTime = currentTime.ToString();


            int BOPtion = (((int)stackElements[currentRaceIndex].Stack_Element_Type - 10) / 2);



            DataTable dt = new DataTable();
            BOPDataAccess bop = new BOPDataAccess();
            bop.ElectionsDBConnectionString = ElectionsDBConnectionString;

            int curNew = BOPtion;
            if (BOPtion == 2)
                curNew = 1;


            dt = bop.GetBOPData(ofc, currentTime, curNew);

            BOPDataModel BOPData = new BOPDataModel();
            DataRow row = dt.Rows[0];

            BOPData.Total = Convert.ToInt16(row["TOTAL_COUNT"]);
            BOPData.DemBaseline = Convert.ToInt16(row["DEM_BASELINE_COUNT"]);
            BOPData.RepBaseline = Convert.ToInt16(row["GOP_BASELINE_COUNT"]);
            BOPData.IndBaseline = Convert.ToInt16(row["IND_BASELINE_COUNT"]);
            BOPData.DemCount = Convert.ToInt16(row["DEM_COUNT"]);
            BOPData.RepCount = Convert.ToInt16(row["GOP_COUNT"]);
            BOPData.IndCount = Convert.ToInt16(row["IND_COUNT"]);
            BOPData.DemDelta = Convert.ToInt16(row["DEM_DELTA"]);
            BOPData.RepDelta = Convert.ToInt16(row["GOP_DELTA"]);
            BOPData.IndDelta = Convert.ToInt16(row["IND_DELTA"]);
            BOPData.Branch = office;

            if (BOPtion == 0)
            {
                BOPData.Session = "CURRENT";
                BOPData.DemDelta = 0;
                BOPData.RepDelta = 0;
                BOPData.IndDelta = 0;

            }
            else
                BOPData.Session = "NEW";


            string outStr = GetBOPMapKeyStr(BOPData, ofc, BOPtion);
            SendToViz(outStr, seDataType);


        }

        public string GetBOPMapKeyStr(BOPDataModel BOPData, string ofc, int BOPtion)
        {
            //Variables – 
            //SENATE or HOUSE
            //CURRENT / NEW

            //BOP_DATA = SENATE ^ CURRENT~RepNum = 10 | RepNetChange = 10 | DemNum = 20 | DemNetChange = 20 | IndNum = 1 | IndNetChange = 1

            //(for net gain)
            //BOP_DATA = NET_GAIN~HouseNum | SenNum
            //BRANCH ^ MODE ~ RepNum=0 | RepNetChange=0 | DemNum=0 | DemNetChange=0 | IndNum=0 | IndNetChange=0
            string MapKeyStr;

            if (BOPtion < 2)
            {
                MapKeyStr = $"{BOPData.Branch}^{BOPData.Session}~";
                if (BOPtion == 0)
                    MapKeyStr += $"RepNum={BOPData.RepBaseline}|RepNetChange={BOPData.RepDelta}|DemNum={BOPData.DemBaseline}|DemNetChange={BOPData.DemDelta}|IndNum={BOPData.IndBaseline}|IndNetChange={BOPData.IndDelta}";
                else
                    MapKeyStr += $"RepNum={BOPData.RepCount}|RepNetChange={BOPData.RepDelta}|DemNum={BOPData.DemCount}|DemNetChange={BOPData.DemDelta}|IndNum={BOPData.IndCount}|IndNetChange={BOPData.IndDelta}";
            }
            else
            {
                // BOP_DATA = NET_GAIN~party^HouseNum|party^SenNum

                MapKeyStr = $"BOP_DATA = NET_GAIN~{BOPData.Branch} ^ {BOPData.Session}~";

            }

            return MapKeyStr;

        }
        #endregion

        #region Referendums Data Processing
        public void TakeReferendums()
        {
            //Get the selected race list object
            currentRaceIndex = stackGrid.CurrentCell.RowIndex;
            short stateNumber = stackElements[currentRaceIndex].State_Number;
            string raceOffice = stackElements[currentRaceIndex].Office_Code;
            int seDataType = (int)stackElements[currentRaceIndex].Stack_Element_Data_Type;

            referendumsDataCollection = new ReferendumsDataCollection();
            referendumsDataCollection.ElectionsDBConnectionString = ElectionsDBConnectionString;
            referendumsData = referendumsDataCollection.GetReferendumsDataCollection(stateNumber, raceOffice);

            if (referendumsData[0].TotalVotes > 0)
            {

                referendumsData[0].VotePct = (Int32)referendumsData[0].VoteCount * 100 / referendumsData[0].TotalVotes;
                referendumsData[1].VotePct = (Int32)referendumsData[1].VoteCount * 100 / referendumsData[1].TotalVotes;
            }
            else
            {
                referendumsData[0].VotePct = 0;
                referendumsData[1].VotePct = 0;

            }

            string outStr = "";

            if (referendumsData.Count >= 2)
                outStr = GetReferendumsMapKeyStr(referendumsData);

            SendToViz(outStr, seDataType);


        }

        public string GetReferendumsMapKeyStr(BindingList<ReferendumDataModel> refData)
        {
            string MapKeyStr = $"{refData[0].StateName}|{refData[0].PropRefID}|{refData[0].Description}|{refData[0].Detailtext}|";


            if (refData[0].WinnerCalled)
            {
                MapKeyStr += $"{refData[0].WinnerCandidateID}|";
            }
            else
                MapKeyStr += $"0|";


            /*
            if (refData[0].WinnerCalled)
            {
                MapKeyStr += $"1|";
            }
            else if (refData[1].WinnerCalled)
            {
                MapKeyStr += $"2|";
            }
            else
                MapKeyStr += $"0|";

            */

            MapKeyStr += $"{refData[0].VotePct}| |{refData[1].VotePct}| |";

            return MapKeyStr;
        }
        #endregion
    }

}
