namespace GUILayer.Forms
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miSelectDefaultShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetStatusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miAboutBox = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentShow = new System.Windows.Forms.Label();
            this.lblCurrentShowHeader = new System.Windows.Forms.Label();
            this.lblPlaylistNameHeader = new System.Windows.Forms.Label();
            this.lblPlaylistName = new System.Windows.Forms.Label();
            this.lblTrioChannelHeader = new System.Windows.Forms.Label();
            this.lblTrioChannel = new System.Windows.Forms.Label();
            this.dataModeSelect = new System.Windows.Forms.TabControl();
            this.tpRaces = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblAvailRaceCnt = new System.Windows.Forms.Label();
            this.gbSpF = new System.Windows.Forms.GroupBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbPollClosing = new System.Windows.Forms.RadioButton();
            this.rbBattleground = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnAddRace4Way = new System.Windows.Forms.Button();
            this.btnSelect4 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAddRace3WaySelect = new System.Windows.Forms.Button();
            this.btnAddRace3Way = new System.Windows.Forms.Button();
            this.gbRCF = new System.Windows.Forms.GroupBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbCalled = new System.Windows.Forms.RadioButton();
            this.rbJustCalled = new System.Windows.Forms.RadioButton();
            this.rbTCTC = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAddRace2WaySelect = new System.Windows.Forms.Button();
            this.btnAddRace2Way = new System.Windows.Forms.Button();
            this.gbROF = new System.Windows.Forms.GroupBox();
            this.rbShowAll = new System.Windows.Forms.RadioButton();
            this.rbGovernor = new System.Windows.Forms.RadioButton();
            this.rbHouse = new System.Windows.Forms.RadioButton();
            this.rbSenate = new System.Windows.Forms.RadioButton();
            this.rbPresident = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddRace1Way = new System.Windows.Forms.Button();
            this.btnAddRace1WaySelect = new System.Windows.Forms.Button();
            this.availableRacesGrid = new System.Windows.Forms.DataGridView();
            this.Race_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Race_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpExitPolls = new System.Windows.Forms.TabPage();
            this.tpBalanceOfPower = new System.Windows.Forms.TabPage();
            this.BOPdataGridView = new System.Windows.Forms.DataGridView();
            this.eType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Session = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddBalanceOfPower = new System.Windows.Forms.Button();
            this.tpReferendums = new System.Windows.Forms.TabPage();
            this.btnAddReferendum = new System.Windows.Forms.Button();
            this.ReferendumsGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbExitPolls = new System.Windows.Forms.GroupBox();
            this.rbEPMan = new System.Windows.Forms.RadioButton();
            this.rbEPAuto = new System.Windows.Forms.RadioButton();
            this.btnAddExitPoll = new System.Windows.Forms.Button();
            this.availableExitPollsGrid = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.st = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Question = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.timerStatusUpdate = new System.Windows.Forms.Timer(this.components);
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.pnlStack = new System.Windows.Forms.Panel();
            this.cbGraphicConcept = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClearStack = new System.Windows.Forms.Button();
            this.btnDeleteStackElement = new System.Windows.Forms.Button();
            this.btnLoadStack = new System.Windows.Forms.Button();
            this.btnActivateStack = new System.Windows.Forms.Button();
            this.btnSaveStack = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStackElementDown = new System.Windows.Forms.Button();
            this.btnStackElementUp = new System.Windows.Forms.Button();
            this.txtStack = new System.Windows.Forms.Label();
            this.txtStackEntriesCount = new System.Windows.Forms.Label();
            this.lblStackEntriesCount = new System.Windows.Forms.Label();
            this.lblStackHeader = new System.Windows.Forms.Label();
            this.stackGrid = new System.Windows.Forms.DataGridView();
            this.Element_Type_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stack_Entry_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.lblHostName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.dataModeSelect.SuspendLayout();
            this.tpRaces.SuspendLayout();
            this.panel4.SuspendLayout();
            this.gbSpF.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gbRCF.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbROF.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableRacesGrid)).BeginInit();
            this.tpBalanceOfPower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BOPdataGridView)).BeginInit();
            this.tpReferendums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReferendumsGrid)).BeginInit();
            this.gbExitPolls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableExitPollsGrid)).BeginInit();
            this.gbTime.SuspendLayout();
            this.pnlStack.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stackGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.utilitiesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1423, 24);
            this.menuStrip1.TabIndex = 48;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExit});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.programToolStripMenuItem.Text = "&Program";
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(92, 22);
            this.miExit.Text = "E&xit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // utilitiesToolStripMenuItem
            // 
            this.utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSelectDefaultShow,
            this.toolStripMenuItem1,
            this.resetStatusBarToolStripMenuItem});
            this.utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            this.utilitiesToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.utilitiesToolStripMenuItem.Text = "&Utilities";
            // 
            // miSelectDefaultShow
            // 
            this.miSelectDefaultShow.Name = "miSelectDefaultShow";
            this.miSelectDefaultShow.Size = new System.Drawing.Size(170, 22);
            this.miSelectDefaultShow.Text = "&Select Default Show";
            this.miSelectDefaultShow.Click += new System.EventHandler(this.miSelectDefaultShow_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(167, 6);
            // 
            // resetStatusBarToolStripMenuItem
            // 
            this.resetStatusBarToolStripMenuItem.Name = "resetStatusBarToolStripMenuItem";
            this.resetStatusBarToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.resetStatusBarToolStripMenuItem.Text = "&Reset Status Bar";
            this.resetStatusBarToolStripMenuItem.Click += new System.EventHandler(this.resetStatusBarToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAboutBox});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // miAboutBox
            // 
            this.miAboutBox.Name = "miAboutBox";
            this.miAboutBox.Size = new System.Drawing.Size(103, 22);
            this.miAboutBox.Text = "&About";
            this.miAboutBox.Click += new System.EventHandler(this.miAboutBox_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 788);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1423, 22);
            this.statusStrip.TabIndex = 53;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.ActiveLinkColor = System.Drawing.SystemColors.AppWorkspace;
            this.toolStripStatusLabel.BackColor = System.Drawing.Color.SpringGreen;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(25, 17);
            this.toolStripStatusLabel.Text = "N/A";
            // 
            // lblCurrentShow
            // 
            this.lblCurrentShow.AutoSize = true;
            this.lblCurrentShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentShow.Location = new System.Drawing.Point(124, 33);
            this.lblCurrentShow.Name = "lblCurrentShow";
            this.lblCurrentShow.Size = new System.Drawing.Size(34, 16);
            this.lblCurrentShow.TabIndex = 86;
            this.lblCurrentShow.Text = "N/A";
            // 
            // lblCurrentShowHeader
            // 
            this.lblCurrentShowHeader.AutoSize = true;
            this.lblCurrentShowHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentShowHeader.Location = new System.Drawing.Point(12, 33);
            this.lblCurrentShowHeader.Name = "lblCurrentShowHeader";
            this.lblCurrentShowHeader.Size = new System.Drawing.Size(115, 16);
            this.lblCurrentShowHeader.TabIndex = 85;
            this.lblCurrentShowHeader.Text = "Selected Show:";
            // 
            // lblPlaylistNameHeader
            // 
            this.lblPlaylistNameHeader.AutoSize = true;
            this.lblPlaylistNameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlaylistNameHeader.Location = new System.Drawing.Point(271, 33);
            this.lblPlaylistNameHeader.Name = "lblPlaylistNameHeader";
            this.lblPlaylistNameHeader.Size = new System.Drawing.Size(108, 16);
            this.lblPlaylistNameHeader.TabIndex = 88;
            this.lblPlaylistNameHeader.Text = "Playlist Name:";
            // 
            // lblPlaylistName
            // 
            this.lblPlaylistName.AutoSize = true;
            this.lblPlaylistName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlaylistName.Location = new System.Drawing.Point(376, 33);
            this.lblPlaylistName.Name = "lblPlaylistName";
            this.lblPlaylistName.Size = new System.Drawing.Size(34, 16);
            this.lblPlaylistName.TabIndex = 89;
            this.lblPlaylistName.Text = "N/A";
            // 
            // lblTrioChannelHeader
            // 
            this.lblTrioChannelHeader.AutoSize = true;
            this.lblTrioChannelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrioChannelHeader.Location = new System.Drawing.Point(533, 33);
            this.lblTrioChannelHeader.Name = "lblTrioChannelHeader";
            this.lblTrioChannelHeader.Size = new System.Drawing.Size(100, 16);
            this.lblTrioChannelHeader.TabIndex = 90;
            this.lblTrioChannelHeader.Text = "Trio Channel:";
            // 
            // lblTrioChannel
            // 
            this.lblTrioChannel.AutoSize = true;
            this.lblTrioChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrioChannel.Location = new System.Drawing.Point(630, 33);
            this.lblTrioChannel.Name = "lblTrioChannel";
            this.lblTrioChannel.Size = new System.Drawing.Size(34, 16);
            this.lblTrioChannel.TabIndex = 91;
            this.lblTrioChannel.Text = "N/A";
            // 
            // dataModeSelect
            // 
            this.dataModeSelect.Controls.Add(this.tpRaces);
            this.dataModeSelect.Controls.Add(this.tpExitPolls);
            this.dataModeSelect.Controls.Add(this.tpBalanceOfPower);
            this.dataModeSelect.Controls.Add(this.tpReferendums);
            this.dataModeSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataModeSelect.Location = new System.Drawing.Point(12, 58);
            this.dataModeSelect.Name = "dataModeSelect";
            this.dataModeSelect.SelectedIndex = 0;
            this.dataModeSelect.Size = new System.Drawing.Size(686, 720);
            this.dataModeSelect.TabIndex = 93;
            this.dataModeSelect.SelectedIndexChanged += new System.EventHandler(this.dataModeSelect_SelectedIndexChanged);
            // 
            // tpRaces
            // 
            this.tpRaces.BackColor = System.Drawing.Color.Navy;
            this.tpRaces.Controls.Add(this.panel4);
            this.tpRaces.Controls.Add(this.gbSpF);
            this.tpRaces.Controls.Add(this.groupBox1);
            this.tpRaces.Controls.Add(this.groupBox5);
            this.tpRaces.Controls.Add(this.groupBox4);
            this.tpRaces.Controls.Add(this.gbRCF);
            this.tpRaces.Controls.Add(this.groupBox3);
            this.tpRaces.Controls.Add(this.gbROF);
            this.tpRaces.Controls.Add(this.groupBox2);
            this.tpRaces.Controls.Add(this.availableRacesGrid);
            this.tpRaces.Location = new System.Drawing.Point(4, 25);
            this.tpRaces.Name = "tpRaces";
            this.tpRaces.Padding = new System.Windows.Forms.Padding(3);
            this.tpRaces.Size = new System.Drawing.Size(678, 691);
            this.tpRaces.TabIndex = 0;
            this.tpRaces.Text = "Races";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gray;
            this.panel4.Controls.Add(this.lblAvailRaceCnt);
            this.panel4.Location = new System.Drawing.Point(6, 150);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(666, 32);
            this.panel4.TabIndex = 121;
            // 
            // lblAvailRaceCnt
            // 
            this.lblAvailRaceCnt.AutoSize = true;
            this.lblAvailRaceCnt.Location = new System.Drawing.Point(7, 8);
            this.lblAvailRaceCnt.Name = "lblAvailRaceCnt";
            this.lblAvailRaceCnt.Size = new System.Drawing.Size(131, 16);
            this.lblAvailRaceCnt.TabIndex = 0;
            this.lblAvailRaceCnt.Text = "Available Races: ";
            // 
            // gbSpF
            // 
            this.gbSpF.BackColor = System.Drawing.Color.DodgerBlue;
            this.gbSpF.Controls.Add(this.rbNone);
            this.gbSpF.Controls.Add(this.rbPollClosing);
            this.gbSpF.Controls.Add(this.rbBattleground);
            this.gbSpF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSpF.Location = new System.Drawing.Point(6, 102);
            this.gbSpF.Name = "gbSpF";
            this.gbSpF.Size = new System.Drawing.Size(666, 45);
            this.gbSpF.TabIndex = 120;
            this.gbSpF.TabStop = false;
            this.gbSpF.Text = "Additional Filters";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(540, 18);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(117, 21);
            this.rbNone.TabIndex = 9;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "Show All(12)";
            this.rbNone.UseVisualStyleBackColor = false;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
            // 
            // rbPollClosing
            // 
            this.rbPollClosing.AutoSize = true;
            this.rbPollClosing.Location = new System.Drawing.Point(260, 18);
            this.rbPollClosing.Name = "rbPollClosing";
            this.rbPollClosing.Size = new System.Drawing.Size(187, 21);
            this.rbPollClosing.TabIndex = 6;
            this.rbPollClosing.Text = "Next Poll Closing(F11)";
            this.rbPollClosing.UseVisualStyleBackColor = true;
            this.rbPollClosing.CheckedChanged += new System.EventHandler(this.rbPollClosing_CheckedChanged);
            // 
            // rbBattleground
            // 
            this.rbBattleground.AutoSize = true;
            this.rbBattleground.Location = new System.Drawing.Point(10, 18);
            this.rbBattleground.Name = "rbBattleground";
            this.rbBattleground.Size = new System.Drawing.Size(209, 21);
            this.rbBattleground.TabIndex = 5;
            this.rbBattleground.Text = "Battleground States(F10)";
            this.rbBattleground.UseVisualStyleBackColor = true;
            this.rbBattleground.CheckedChanged += new System.EventHandler(this.rbBattleground_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox1.Controls.Add(this.btnAddAll);
            this.groupBox1.Location = new System.Drawing.Point(424, 586);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 100);
            this.groupBox1.TabIndex = 119;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Quick Add";
            // 
            // btnAddAll
            // 
            this.btnAddAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddAll.Location = new System.Drawing.Point(67, 28);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(117, 55);
            this.btnAddAll.TabIndex = 70;
            this.btnAddAll.Text = "All  (CTRL A)";
            this.btnAddAll.UseVisualStyleBackColor = false;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox5.Controls.Add(this.btnAddRace4Way);
            this.groupBox5.Controls.Add(this.btnSelect4);
            this.groupBox5.Location = new System.Drawing.Point(424, 486);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(248, 88);
            this.groupBox5.TabIndex = 69;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "4 - Way";
            // 
            // btnAddRace4Way
            // 
            this.btnAddRace4Way.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace4Way.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace4Way.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace4Way.Image")));
            this.btnAddRace4Way.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace4Way.Location = new System.Drawing.Point(11, 21);
            this.btnAddRace4Way.Name = "btnAddRace4Way";
            this.btnAddRace4Way.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace4Way.TabIndex = 67;
            this.btnAddRace4Way.Text = "Top";
            this.btnAddRace4Way.UseVisualStyleBackColor = false;
            this.btnAddRace4Way.Click += new System.EventHandler(this.btnAddRace4Way_Click);
            // 
            // btnSelect4
            // 
            this.btnSelect4.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelect4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelect4.Location = new System.Drawing.Point(139, 21);
            this.btnSelect4.Name = "btnSelect4";
            this.btnSelect4.Size = new System.Drawing.Size(100, 55);
            this.btnSelect4.TabIndex = 66;
            this.btnSelect4.Text = "Select";
            this.btnSelect4.UseVisualStyleBackColor = false;
            this.btnSelect4.Click += new System.EventHandler(this.btnSelect4_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox4.Controls.Add(this.btnAddRace3WaySelect);
            this.groupBox4.Controls.Add(this.btnAddRace3Way);
            this.groupBox4.Location = new System.Drawing.Point(424, 385);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(248, 88);
            this.groupBox4.TabIndex = 68;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "3 - Way";
            // 
            // btnAddRace3WaySelect
            // 
            this.btnAddRace3WaySelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace3WaySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace3WaySelect.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace3WaySelect.Image")));
            this.btnAddRace3WaySelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace3WaySelect.Location = new System.Drawing.Point(139, 21);
            this.btnAddRace3WaySelect.Name = "btnAddRace3WaySelect";
            this.btnAddRace3WaySelect.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace3WaySelect.TabIndex = 65;
            this.btnAddRace3WaySelect.Text = "Select";
            this.btnAddRace3WaySelect.UseVisualStyleBackColor = false;
            this.btnAddRace3WaySelect.Click += new System.EventHandler(this.btnSelect3_Click);
            // 
            // btnAddRace3Way
            // 
            this.btnAddRace3Way.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace3Way.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace3Way.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace3Way.Image")));
            this.btnAddRace3Way.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace3Way.Location = new System.Drawing.Point(11, 21);
            this.btnAddRace3Way.Name = "btnAddRace3Way";
            this.btnAddRace3Way.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace3Way.TabIndex = 62;
            this.btnAddRace3Way.Text = "Top";
            this.btnAddRace3Way.UseVisualStyleBackColor = false;
            this.btnAddRace3Way.Click += new System.EventHandler(this.btnAddRace3Way_Click);
            // 
            // gbRCF
            // 
            this.gbRCF.BackColor = System.Drawing.Color.SkyBlue;
            this.gbRCF.Controls.Add(this.rbAll);
            this.gbRCF.Controls.Add(this.rbCalled);
            this.gbRCF.Controls.Add(this.rbJustCalled);
            this.gbRCF.Controls.Add(this.rbTCTC);
            this.gbRCF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRCF.Location = new System.Drawing.Point(6, 53);
            this.gbRCF.Name = "gbRCF";
            this.gbRCF.Size = new System.Drawing.Size(666, 45);
            this.gbRCF.TabIndex = 118;
            this.gbRCF.TabStop = false;
            this.gbRCF.Text = "Race Call Filters";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(540, 18);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(117, 21);
            this.rbAll.TabIndex = 9;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "Show All(F9)";
            this.rbAll.UseVisualStyleBackColor = false;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // rbCalled
            // 
            this.rbCalled.AutoSize = true;
            this.rbCalled.Location = new System.Drawing.Point(392, 18);
            this.rbCalled.Name = "rbCalled";
            this.rbCalled.Size = new System.Drawing.Size(101, 21);
            this.rbCalled.TabIndex = 7;
            this.rbCalled.Text = "Called(F8)";
            this.rbCalled.UseVisualStyleBackColor = true;
            this.rbCalled.CheckedChanged += new System.EventHandler(this.rbCalled_CheckedChanged);
            // 
            // rbJustCalled
            // 
            this.rbJustCalled.AutoSize = true;
            this.rbJustCalled.Location = new System.Drawing.Point(221, 18);
            this.rbJustCalled.Name = "rbJustCalled";
            this.rbJustCalled.Size = new System.Drawing.Size(136, 21);
            this.rbJustCalled.TabIndex = 6;
            this.rbJustCalled.Text = "Just Called(F7)";
            this.rbJustCalled.UseVisualStyleBackColor = true;
            this.rbJustCalled.CheckedChanged += new System.EventHandler(this.rbJustCalled_CheckedChanged);
            // 
            // rbTCTC
            // 
            this.rbTCTC.AutoSize = true;
            this.rbTCTC.Location = new System.Drawing.Point(10, 18);
            this.rbTCTC.Name = "rbTCTC";
            this.rbTCTC.Size = new System.Drawing.Size(185, 21);
            this.rbTCTC.TabIndex = 5;
            this.rbTCTC.Text = "Too Close To Call(F6)";
            this.rbTCTC.UseVisualStyleBackColor = true;
            this.rbTCTC.CheckedChanged += new System.EventHandler(this.rbTCTC_CheckedChanged);
            this.rbTCTC.Click += new System.EventHandler(this.rbTCTC_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox3.Controls.Add(this.btnAddRace2WaySelect);
            this.groupBox3.Controls.Add(this.btnAddRace2Way);
            this.groupBox3.Location = new System.Drawing.Point(424, 285);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 88);
            this.groupBox3.TabIndex = 67;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "2 - Way";
            // 
            // btnAddRace2WaySelect
            // 
            this.btnAddRace2WaySelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace2WaySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace2WaySelect.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace2WaySelect.Image")));
            this.btnAddRace2WaySelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace2WaySelect.Location = new System.Drawing.Point(139, 21);
            this.btnAddRace2WaySelect.Name = "btnAddRace2WaySelect";
            this.btnAddRace2WaySelect.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace2WaySelect.TabIndex = 64;
            this.btnAddRace2WaySelect.Text = "Select";
            this.btnAddRace2WaySelect.UseVisualStyleBackColor = false;
            this.btnAddRace2WaySelect.Click += new System.EventHandler(this.btnSelect2_Click);
            // 
            // btnAddRace2Way
            // 
            this.btnAddRace2Way.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace2Way.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace2Way.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace2Way.Image")));
            this.btnAddRace2Way.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace2Way.Location = new System.Drawing.Point(11, 21);
            this.btnAddRace2Way.Name = "btnAddRace2Way";
            this.btnAddRace2Way.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace2Way.TabIndex = 61;
            this.btnAddRace2Way.Text = "  Top";
            this.btnAddRace2Way.UseVisualStyleBackColor = false;
            this.btnAddRace2Way.Click += new System.EventHandler(this.btnAddRace2Way_Click);
            // 
            // gbROF
            // 
            this.gbROF.BackColor = System.Drawing.Color.PowderBlue;
            this.gbROF.Controls.Add(this.rbShowAll);
            this.gbROF.Controls.Add(this.rbGovernor);
            this.gbROF.Controls.Add(this.rbHouse);
            this.gbROF.Controls.Add(this.rbSenate);
            this.gbROF.Controls.Add(this.rbPresident);
            this.gbROF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbROF.Location = new System.Drawing.Point(6, 4);
            this.gbROF.Name = "gbROF";
            this.gbROF.Size = new System.Drawing.Size(666, 45);
            this.gbROF.TabIndex = 95;
            this.gbROF.TabStop = false;
            this.gbROF.Text = "Race Office Filters";
            this.gbROF.Enter += new System.EventHandler(this.gbROF_Enter);
            // 
            // rbShowAll
            // 
            this.rbShowAll.AutoSize = true;
            this.rbShowAll.Checked = true;
            this.rbShowAll.Location = new System.Drawing.Point(540, 18);
            this.rbShowAll.Name = "rbShowAll";
            this.rbShowAll.Size = new System.Drawing.Size(117, 21);
            this.rbShowAll.TabIndex = 9;
            this.rbShowAll.TabStop = true;
            this.rbShowAll.Text = "Show All(F5)";
            this.rbShowAll.UseVisualStyleBackColor = true;
            this.rbShowAll.CheckedChanged += new System.EventHandler(this.rbShowAll_CheckedChanged);
            // 
            // rbGovernor
            // 
            this.rbGovernor.AutoSize = true;
            this.rbGovernor.Location = new System.Drawing.Point(398, 18);
            this.rbGovernor.Name = "rbGovernor";
            this.rbGovernor.Size = new System.Drawing.Size(124, 21);
            this.rbGovernor.TabIndex = 8;
            this.rbGovernor.Text = "Governor(F4)";
            this.rbGovernor.UseVisualStyleBackColor = true;
            this.rbGovernor.CheckedChanged += new System.EventHandler(this.rbGovernor_CheckedChanged);
            // 
            // rbHouse
            // 
            this.rbHouse.AutoSize = true;
            this.rbHouse.Location = new System.Drawing.Point(278, 18);
            this.rbHouse.Name = "rbHouse";
            this.rbHouse.Size = new System.Drawing.Size(102, 21);
            this.rbHouse.TabIndex = 7;
            this.rbHouse.Text = "House(F3)";
            this.rbHouse.UseVisualStyleBackColor = true;
            this.rbHouse.CheckedChanged += new System.EventHandler(this.rbHouse_CheckedChanged);
            // 
            // rbSenate
            // 
            this.rbSenate.AutoSize = true;
            this.rbSenate.Location = new System.Drawing.Point(153, 18);
            this.rbSenate.Name = "rbSenate";
            this.rbSenate.Size = new System.Drawing.Size(107, 21);
            this.rbSenate.TabIndex = 6;
            this.rbSenate.Text = "Senate(F2)";
            this.rbSenate.UseVisualStyleBackColor = true;
            this.rbSenate.CheckedChanged += new System.EventHandler(this.rbSenate_CheckedChanged);
            // 
            // rbPresident
            // 
            this.rbPresident.AutoSize = true;
            this.rbPresident.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPresident.Location = new System.Drawing.Point(10, 18);
            this.rbPresident.Name = "rbPresident";
            this.rbPresident.Size = new System.Drawing.Size(125, 21);
            this.rbPresident.TabIndex = 5;
            this.rbPresident.Text = "President(F1)";
            this.rbPresident.UseVisualStyleBackColor = true;
            this.rbPresident.CheckedChanged += new System.EventHandler(this.rbPresident_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox2.Controls.Add(this.btnAddRace1Way);
            this.groupBox2.Controls.Add(this.btnAddRace1WaySelect);
            this.groupBox2.Location = new System.Drawing.Point(424, 186);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 88);
            this.groupBox2.TabIndex = 66;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "1 - Way";
            // 
            // btnAddRace1Way
            // 
            this.btnAddRace1Way.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace1Way.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace1Way.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace1Way.Image")));
            this.btnAddRace1Way.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace1Way.Location = new System.Drawing.Point(12, 21);
            this.btnAddRace1Way.Name = "btnAddRace1Way";
            this.btnAddRace1Way.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace1Way.TabIndex = 64;
            this.btnAddRace1Way.Text = "Top";
            this.btnAddRace1Way.UseVisualStyleBackColor = false;
            this.btnAddRace1Way.Click += new System.EventHandler(this.btnAddRace1Way_Click);
            // 
            // btnAddRace1WaySelect
            // 
            this.btnAddRace1WaySelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRace1WaySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRace1WaySelect.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRace1WaySelect.Image")));
            this.btnAddRace1WaySelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRace1WaySelect.Location = new System.Drawing.Point(139, 21);
            this.btnAddRace1WaySelect.Name = "btnAddRace1WaySelect";
            this.btnAddRace1WaySelect.Size = new System.Drawing.Size(100, 55);
            this.btnAddRace1WaySelect.TabIndex = 63;
            this.btnAddRace1WaySelect.Text = "Select";
            this.btnAddRace1WaySelect.UseVisualStyleBackColor = false;
            this.btnAddRace1WaySelect.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // availableRacesGrid
            // 
            this.availableRacesGrid.AllowUserToAddRows = false;
            this.availableRacesGrid.AllowUserToDeleteRows = false;
            this.availableRacesGrid.AllowUserToResizeColumns = false;
            this.availableRacesGrid.AllowUserToResizeRows = false;
            this.availableRacesGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.availableRacesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availableRacesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Race_ID,
            this.Race_Description});
            this.availableRacesGrid.Location = new System.Drawing.Point(6, 185);
            this.availableRacesGrid.MultiSelect = false;
            this.availableRacesGrid.Name = "availableRacesGrid";
            this.availableRacesGrid.ReadOnly = true;
            this.availableRacesGrid.RowHeadersWidth = 15;
            this.availableRacesGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.availableRacesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availableRacesGrid.Size = new System.Drawing.Size(412, 502);
            this.availableRacesGrid.TabIndex = 57;
            this.availableRacesGrid.DoubleClick += new System.EventHandler(this.availableRacesGrid_DoubleClick);
            // 
            // Race_ID
            // 
            this.Race_ID.DataPropertyName = "Race_ID";
            this.Race_ID.HeaderText = "Race ID";
            this.Race_ID.Name = "Race_ID";
            this.Race_ID.ReadOnly = true;
            this.Race_ID.Width = 90;
            // 
            // Race_Description
            // 
            this.Race_Description.DataPropertyName = "Race_Description";
            this.Race_Description.HeaderText = "Race Description";
            this.Race_Description.Name = "Race_Description";
            this.Race_Description.ReadOnly = true;
            this.Race_Description.Width = 302;
            // 
            // tpExitPolls
            // 
            this.tpExitPolls.Location = new System.Drawing.Point(4, 25);
            this.tpExitPolls.Name = "tpExitPolls";
            this.tpExitPolls.Size = new System.Drawing.Size(678, 691);
            this.tpExitPolls.TabIndex = 1;
            // 
            // tpBalanceOfPower
            // 
            this.tpBalanceOfPower.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tpBalanceOfPower.Controls.Add(this.BOPdataGridView);
            this.tpBalanceOfPower.Controls.Add(this.btnAddBalanceOfPower);
            this.tpBalanceOfPower.Location = new System.Drawing.Point(4, 25);
            this.tpBalanceOfPower.Name = "tpBalanceOfPower";
            this.tpBalanceOfPower.Padding = new System.Windows.Forms.Padding(3);
            this.tpBalanceOfPower.Size = new System.Drawing.Size(678, 691);
            this.tpBalanceOfPower.TabIndex = 2;
            this.tpBalanceOfPower.Text = "Balance of Power";
            // 
            // BOPdataGridView
            // 
            this.BOPdataGridView.AllowUserToAddRows = false;
            this.BOPdataGridView.AllowUserToDeleteRows = false;
            this.BOPdataGridView.AllowUserToResizeColumns = false;
            this.BOPdataGridView.AllowUserToResizeRows = false;
            this.BOPdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BOPdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eType,
            this.Branch,
            this.Session});
            this.BOPdataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.BOPdataGridView.Location = new System.Drawing.Point(126, 75);
            this.BOPdataGridView.Name = "BOPdataGridView";
            this.BOPdataGridView.RowHeadersWidth = 15;
            this.BOPdataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BOPdataGridView.Size = new System.Drawing.Size(473, 360);
            this.BOPdataGridView.TabIndex = 2;
            this.BOPdataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BOPdataGridView_CellDoubleClick);
            // 
            // eType
            // 
            this.eType.HeaderText = "eType";
            this.eType.Name = "eType";
            this.eType.Width = 70;
            // 
            // Branch
            // 
            this.Branch.HeaderText = "Branch";
            this.Branch.Name = "Branch";
            this.Branch.Width = 200;
            // 
            // Session
            // 
            this.Session.HeaderText = "Session";
            this.Session.Name = "Session";
            this.Session.Width = 200;
            // 
            // btnAddBalanceOfPower
            // 
            this.btnAddBalanceOfPower.Image = global::GUILayer.Properties.Resources.action_add_16xLG;
            this.btnAddBalanceOfPower.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddBalanceOfPower.Location = new System.Drawing.Point(251, 458);
            this.btnAddBalanceOfPower.Name = "btnAddBalanceOfPower";
            this.btnAddBalanceOfPower.Size = new System.Drawing.Size(219, 60);
            this.btnAddBalanceOfPower.TabIndex = 1;
            this.btnAddBalanceOfPower.Text = "Add Balance of Power";
            this.btnAddBalanceOfPower.UseVisualStyleBackColor = true;
            this.btnAddBalanceOfPower.Click += new System.EventHandler(this.button2_Click);
            // 
            // tpReferendums
            // 
            this.tpReferendums.BackColor = System.Drawing.Color.LightSkyBlue;
            this.tpReferendums.Controls.Add(this.btnAddReferendum);
            this.tpReferendums.Controls.Add(this.ReferendumsGrid);
            this.tpReferendums.Location = new System.Drawing.Point(4, 25);
            this.tpReferendums.Name = "tpReferendums";
            this.tpReferendums.Padding = new System.Windows.Forms.Padding(3);
            this.tpReferendums.Size = new System.Drawing.Size(678, 691);
            this.tpReferendums.TabIndex = 3;
            this.tpReferendums.Text = "Referendums";
            // 
            // btnAddReferendum
            // 
            this.btnAddReferendum.Image = global::GUILayer.Properties.Resources.action_add_16xLG;
            this.btnAddReferendum.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddReferendum.Location = new System.Drawing.Point(273, 606);
            this.btnAddReferendum.Name = "btnAddReferendum";
            this.btnAddReferendum.Size = new System.Drawing.Size(172, 60);
            this.btnAddReferendum.TabIndex = 2;
            this.btnAddReferendum.Text = "Add Referendum";
            this.btnAddReferendum.UseVisualStyleBackColor = true;
            this.btnAddReferendum.Click += new System.EventHandler(this.button1_Click);
            // 
            // ReferendumsGrid
            // 
            this.ReferendumsGrid.AllowUserToAddRows = false;
            this.ReferendumsGrid.AllowUserToDeleteRows = false;
            this.ReferendumsGrid.AllowUserToResizeColumns = false;
            this.ReferendumsGrid.AllowUserToResizeRows = false;
            this.ReferendumsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ReferendumsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReferendumsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6});
            this.ReferendumsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ReferendumsGrid.Location = new System.Drawing.Point(4, 104);
            this.ReferendumsGrid.MultiSelect = false;
            this.ReferendumsGrid.Name = "ReferendumsGrid";
            this.ReferendumsGrid.RowHeadersWidth = 15;
            this.ReferendumsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ReferendumsGrid.Size = new System.Drawing.Size(708, 482);
            this.ReferendumsGrid.TabIndex = 1;
            this.ReferendumsGrid.DoubleClick += new System.EventHandler(this.ReferendumsGrid_DoubleClick);
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "race_Description";
            this.dataGridViewTextBoxColumn6.HeaderText = "Referendum Description";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 700;
            // 
            // gbExitPolls
            // 
            this.gbExitPolls.BackColor = System.Drawing.Color.SteelBlue;
            this.gbExitPolls.Controls.Add(this.rbEPMan);
            this.gbExitPolls.Controls.Add(this.rbEPAuto);
            this.gbExitPolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbExitPolls.Location = new System.Drawing.Point(3, 6);
            this.gbExitPolls.Name = "gbExitPolls";
            this.gbExitPolls.Size = new System.Drawing.Size(708, 53);
            this.gbExitPolls.TabIndex = 121;
            this.gbExitPolls.TabStop = false;
            this.gbExitPolls.Text = "Exit Polls";
            this.gbExitPolls.Visible = false;
            // 
            // rbEPMan
            // 
            this.rbEPMan.AutoSize = true;
            this.rbEPMan.Location = new System.Drawing.Point(409, 22);
            this.rbEPMan.Name = "rbEPMan";
            this.rbEPMan.Size = new System.Drawing.Size(78, 21);
            this.rbEPMan.TabIndex = 6;
            this.rbEPMan.Text = "Manual";
            this.rbEPMan.UseVisualStyleBackColor = true;
            this.rbEPMan.CheckedChanged += new System.EventHandler(this.rbEPMan_CheckedChanged);
            // 
            // rbEPAuto
            // 
            this.rbEPAuto.AutoSize = true;
            this.rbEPAuto.Location = new System.Drawing.Point(221, 22);
            this.rbEPAuto.Name = "rbEPAuto";
            this.rbEPAuto.Size = new System.Drawing.Size(59, 21);
            this.rbEPAuto.TabIndex = 5;
            this.rbEPAuto.Text = "Auto";
            this.rbEPAuto.UseVisualStyleBackColor = true;
            this.rbEPAuto.CheckedChanged += new System.EventHandler(this.rbEPAuto_CheckedChanged);
            // 
            // btnAddExitPoll
            // 
            this.btnAddExitPoll.Image = global::GUILayer.Properties.Resources.action_add_16xLG;
            this.btnAddExitPoll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddExitPoll.Location = new System.Drawing.Point(257, 617);
            this.btnAddExitPoll.Name = "btnAddExitPoll";
            this.btnAddExitPoll.Size = new System.Drawing.Size(202, 60);
            this.btnAddExitPoll.TabIndex = 2;
            this.btnAddExitPoll.Text = "Add Exit Poll";
            this.btnAddExitPoll.UseVisualStyleBackColor = true;
            this.btnAddExitPoll.Click += new System.EventHandler(this.button3_Click);
            // 
            // availableExitPollsGrid
            // 
            this.availableExitPollsGrid.AllowUserToAddRows = false;
            this.availableExitPollsGrid.AllowUserToDeleteRows = false;
            this.availableExitPollsGrid.AllowUserToResizeColumns = false;
            this.availableExitPollsGrid.AllowUserToResizeRows = false;
            this.availableExitPollsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.availableExitPollsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availableExitPollsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.st,
            this.Category,
            this.Question,
            this.rowText,
            this.Subset});
            this.availableExitPollsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.availableExitPollsGrid.Location = new System.Drawing.Point(3, 65);
            this.availableExitPollsGrid.MultiSelect = false;
            this.availableExitPollsGrid.Name = "availableExitPollsGrid";
            this.availableExitPollsGrid.RowHeadersWidth = 15;
            this.availableExitPollsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availableExitPollsGrid.Size = new System.Drawing.Size(708, 532);
            this.availableExitPollsGrid.TabIndex = 0;
            this.availableExitPollsGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableExitPollsGrid_CellContentDoubleClick);
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Type.DataPropertyName = "questionType";
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.Width = 56;
            // 
            // st
            // 
            this.st.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.st.DataPropertyName = "stateOffice";
            this.st.HeaderText = "mxID/st/ofc";
            this.st.Name = "st";
            this.st.Width = 89;
            // 
            // Category
            // 
            this.Category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Category.DataPropertyName = "shortMXLabel";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.Width = 74;
            // 
            // Question
            // 
            this.Question.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Question.DataPropertyName = "fullMXLabel";
            this.Question.HeaderText = "Question";
            this.Question.Name = "Question";
            this.Question.Width = 74;
            // 
            // rowText
            // 
            this.rowText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rowText.DataPropertyName = "rowText";
            this.rowText.HeaderText = "rowText";
            this.rowText.Name = "rowText";
            this.rowText.Width = 70;
            // 
            // Subset
            // 
            this.Subset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Subset.DataPropertyName = "subsetName";
            this.Subset.HeaderText = "Subset";
            this.Subset.Name = "Subset";
            this.Subset.Width = 65;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerStatusUpdate
            // 
            this.timerStatusUpdate.Interval = 1000;
            this.timerStatusUpdate.Tick += new System.EventHandler(this.timerStatusUpdate_Tick);
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.label1);
            this.gbTime.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTime.Location = new System.Drawing.Point(1182, 27);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(227, 42);
            this.gbTime.TabIndex = 119;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "SIMULATED TIME";
            // 
            // pnlStack
            // 
            this.pnlStack.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlStack.Controls.Add(this.cbGraphicConcept);
            this.pnlStack.Controls.Add(this.label2);
            this.pnlStack.Controls.Add(this.panel3);
            this.pnlStack.Controls.Add(this.panel1);
            this.pnlStack.Controls.Add(this.txtStack);
            this.pnlStack.Controls.Add(this.txtStackEntriesCount);
            this.pnlStack.Controls.Add(this.lblStackEntriesCount);
            this.pnlStack.Controls.Add(this.lblStackHeader);
            this.pnlStack.Controls.Add(this.stackGrid);
            this.pnlStack.Location = new System.Drawing.Point(700, 83);
            this.pnlStack.Name = "pnlStack";
            this.pnlStack.Size = new System.Drawing.Size(711, 693);
            this.pnlStack.TabIndex = 120;
            // 
            // cbGraphicConcept
            // 
            this.cbGraphicConcept.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGraphicConcept.FormattingEnabled = true;
            this.cbGraphicConcept.Location = new System.Drawing.Point(449, 26);
            this.cbGraphicConcept.Name = "cbGraphicConcept";
            this.cbGraphicConcept.Size = new System.Drawing.Size(148, 24);
            this.cbGraphicConcept.TabIndex = 132;
            this.cbGraphicConcept.SelectedIndexChanged += new System.EventHandler(this.cbGraphicConcept_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(316, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 16);
            this.label2.TabIndex = 131;
            this.label2.Text = "Graphic Concept:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.btnClearStack);
            this.panel3.Controls.Add(this.btnDeleteStackElement);
            this.panel3.Controls.Add(this.btnLoadStack);
            this.panel3.Controls.Add(this.btnActivateStack);
            this.panel3.Controls.Add(this.btnSaveStack);
            this.panel3.Location = new System.Drawing.Point(12, 542);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(632, 138);
            this.panel3.TabIndex = 129;
            // 
            // btnClearStack
            // 
            this.btnClearStack.BackColor = System.Drawing.SystemColors.Control;
            this.btnClearStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearStack.Image = global::GUILayer.Properties.Resources.action_Cancel_16xLG;
            this.btnClearStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearStack.Location = new System.Drawing.Point(19, 84);
            this.btnClearStack.Name = "btnClearStack";
            this.btnClearStack.Size = new System.Drawing.Size(180, 40);
            this.btnClearStack.TabIndex = 136;
            this.btnClearStack.Text = "Clear Stack";
            this.btnClearStack.UseVisualStyleBackColor = false;
            this.btnClearStack.Click += new System.EventHandler(this.btnClearStack_Click_1);
            // 
            // btnDeleteStackElement
            // 
            this.btnDeleteStackElement.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteStackElement.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteStackElement.Image = global::GUILayer.Properties.Resources.action_Cancel_16xLG;
            this.btnDeleteStackElement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteStackElement.Location = new System.Drawing.Point(19, 16);
            this.btnDeleteStackElement.Name = "btnDeleteStackElement";
            this.btnDeleteStackElement.Size = new System.Drawing.Size(180, 40);
            this.btnDeleteStackElement.TabIndex = 135;
            this.btnDeleteStackElement.Text = "Delete Element";
            this.btnDeleteStackElement.UseVisualStyleBackColor = false;
            this.btnDeleteStackElement.Click += new System.EventHandler(this.btnDeleteStackElement_Click);
            // 
            // btnLoadStack
            // 
            this.btnLoadStack.BackColor = System.Drawing.SystemColors.Control;
            this.btnLoadStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadStack.Image = global::GUILayer.Properties.Resources.folder_Open_16xLG;
            this.btnLoadStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadStack.Location = new System.Drawing.Point(228, 16);
            this.btnLoadStack.Name = "btnLoadStack";
            this.btnLoadStack.Size = new System.Drawing.Size(180, 40);
            this.btnLoadStack.TabIndex = 133;
            this.btnLoadStack.Text = "Recall Stacks";
            this.btnLoadStack.UseVisualStyleBackColor = false;
            this.btnLoadStack.Click += new System.EventHandler(this.btnLoadStack_Click);
            // 
            // btnActivateStack
            // 
            this.btnActivateStack.BackColor = System.Drawing.SystemColors.Control;
            this.btnActivateStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivateStack.Image = global::GUILayer.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            this.btnActivateStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActivateStack.Location = new System.Drawing.Point(437, 16);
            this.btnActivateStack.Name = "btnActivateStack";
            this.btnActivateStack.Size = new System.Drawing.Size(180, 108);
            this.btnActivateStack.TabIndex = 132;
            this.btnActivateStack.Text = "  Save && Activate Stack";
            this.btnActivateStack.UseVisualStyleBackColor = false;
            this.btnActivateStack.Click += new System.EventHandler(this.btnActivateStack_Click);
            // 
            // btnSaveStack
            // 
            this.btnSaveStack.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveStack.Image = global::GUILayer.Properties.Resources.folder_Closed_16xLG;
            this.btnSaveStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveStack.Location = new System.Drawing.Point(228, 84);
            this.btnSaveStack.Name = "btnSaveStack";
            this.btnSaveStack.Size = new System.Drawing.Size(180, 40);
            this.btnSaveStack.TabIndex = 129;
            this.btnSaveStack.Text = "Save Stack Only";
            this.btnSaveStack.UseVisualStyleBackColor = false;
            this.btnSaveStack.Click += new System.EventHandler(this.btnSaveStack_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.btnStackElementDown);
            this.panel1.Controls.Add(this.btnStackElementUp);
            this.panel1.Location = new System.Drawing.Point(653, 212);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(50, 161);
            this.panel1.TabIndex = 127;
            // 
            // btnStackElementDown
            // 
            this.btnStackElementDown.BackColor = System.Drawing.SystemColors.Control;
            this.btnStackElementDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStackElementDown.Image = ((System.Drawing.Image)(resources.GetObject("btnStackElementDown.Image")));
            this.btnStackElementDown.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStackElementDown.Location = new System.Drawing.Point(5, 94);
            this.btnStackElementDown.Name = "btnStackElementDown";
            this.btnStackElementDown.Size = new System.Drawing.Size(42, 60);
            this.btnStackElementDown.TabIndex = 73;
            this.btnStackElementDown.Text = "DN";
            this.btnStackElementDown.UseVisualStyleBackColor = false;
            this.btnStackElementDown.Click += new System.EventHandler(this.btnStackElementDown_Click);
            // 
            // btnStackElementUp
            // 
            this.btnStackElementUp.BackColor = System.Drawing.SystemColors.Control;
            this.btnStackElementUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStackElementUp.Image = ((System.Drawing.Image)(resources.GetObject("btnStackElementUp.Image")));
            this.btnStackElementUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStackElementUp.Location = new System.Drawing.Point(5, 9);
            this.btnStackElementUp.Name = "btnStackElementUp";
            this.btnStackElementUp.Size = new System.Drawing.Size(42, 60);
            this.btnStackElementUp.TabIndex = 72;
            this.btnStackElementUp.Text = "UP";
            this.btnStackElementUp.UseVisualStyleBackColor = false;
            this.btnStackElementUp.Click += new System.EventHandler(this.btnStackElementUp_Click);
            // 
            // txtStack
            // 
            this.txtStack.AutoSize = true;
            this.txtStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStack.Location = new System.Drawing.Point(66, 8);
            this.txtStack.Name = "txtStack";
            this.txtStack.Size = new System.Drawing.Size(111, 16);
            this.txtStack.TabIndex = 126;
            this.txtStack.Text = "None Selected";
            // 
            // txtStackEntriesCount
            // 
            this.txtStackEntriesCount.AutoSize = true;
            this.txtStackEntriesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStackEntriesCount.Location = new System.Drawing.Point(159, 28);
            this.txtStackEntriesCount.Name = "txtStackEntriesCount";
            this.txtStackEntriesCount.Size = new System.Drawing.Size(16, 16);
            this.txtStackEntriesCount.TabIndex = 123;
            this.txtStackEntriesCount.Text = "0";
            // 
            // lblStackEntriesCount
            // 
            this.lblStackEntriesCount.AutoSize = true;
            this.lblStackEntriesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStackEntriesCount.Location = new System.Drawing.Point(12, 28);
            this.lblStackEntriesCount.Name = "lblStackEntriesCount";
            this.lblStackEntriesCount.Size = new System.Drawing.Size(151, 16);
            this.lblStackEntriesCount.TabIndex = 122;
            this.lblStackEntriesCount.Text = "Number of Elements:";
            // 
            // lblStackHeader
            // 
            this.lblStackHeader.AutoSize = true;
            this.lblStackHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStackHeader.Location = new System.Drawing.Point(12, 8);
            this.lblStackHeader.Name = "lblStackHeader";
            this.lblStackHeader.Size = new System.Drawing.Size(51, 16);
            this.lblStackHeader.TabIndex = 119;
            this.lblStackHeader.Text = "Stack:";
            // 
            // stackGrid
            // 
            this.stackGrid.AllowUserToAddRows = false;
            this.stackGrid.AllowUserToDeleteRows = false;
            this.stackGrid.AllowUserToResizeColumns = false;
            this.stackGrid.AllowUserToResizeRows = false;
            this.stackGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.stackGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.stackGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stackGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Element_Type_Description,
            this.TemplateID,
            this.Stack_Entry_Description});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.stackGrid.DefaultCellStyle = dataGridViewCellStyle12;
            this.stackGrid.Location = new System.Drawing.Point(12, 57);
            this.stackGrid.MaximumSize = new System.Drawing.Size(800, 495);
            this.stackGrid.MinimumSize = new System.Drawing.Size(558, 300);
            this.stackGrid.MultiSelect = false;
            this.stackGrid.Name = "stackGrid";
            this.stackGrid.ReadOnly = true;
            this.stackGrid.RowHeadersVisible = false;
            this.stackGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stackGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.stackGrid.Size = new System.Drawing.Size(633, 479);
            this.stackGrid.TabIndex = 118;
            // 
            // Element_Type_Description
            // 
            this.Element_Type_Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Element_Type_Description.DataPropertyName = "Stack_Element_Description";
            this.Element_Type_Description.HeaderText = "Entry Type";
            this.Element_Type_Description.Name = "Element_Type_Description";
            this.Element_Type_Description.ReadOnly = true;
            this.Element_Type_Description.Width = 160;
            // 
            // TemplateID
            // 
            this.TemplateID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TemplateID.DataPropertyName = "Stack_Element_TemplateID";
            this.TemplateID.HeaderText = "TID";
            this.TemplateID.Name = "TemplateID";
            this.TemplateID.ReadOnly = true;
            this.TemplateID.Width = 185;
            // 
            // Stack_Entry_Description
            // 
            this.Stack_Entry_Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Stack_Entry_Description.DataPropertyName = "Listbox_Description";
            this.Stack_Entry_Description.HeaderText = "Entry Description";
            this.Stack_Entry_Description.Name = "Stack_Entry_Description";
            this.Stack_Entry_Description.ReadOnly = true;
            this.Stack_Entry_Description.Width = 280;
            // 
            // lblIpAddress
            // 
            this.lblIpAddress.AutoSize = true;
            this.lblIpAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIpAddress.Location = new System.Drawing.Point(860, 33);
            this.lblIpAddress.Name = "lblIpAddress";
            this.lblIpAddress.Size = new System.Drawing.Size(34, 16);
            this.lblIpAddress.TabIndex = 121;
            this.lblIpAddress.Text = "N/A";
            // 
            // lblHostName
            // 
            this.lblHostName.AutoSize = true;
            this.lblHostName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHostName.Location = new System.Drawing.Point(968, 33);
            this.lblHostName.Name = "lblHostName";
            this.lblHostName.Size = new System.Drawing.Size(34, 16);
            this.lblHostName.TabIndex = 122;
            this.lblHostName.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(766, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 16);
            this.label3.TabIndex = 123;
            this.label3.Text = "Host PC Info:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1423, 810);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblHostName);
            this.Controls.Add(this.lblIpAddress);
            this.Controls.Add(this.pnlStack);
            this.Controls.Add(this.gbTime);
            this.Controls.Add(this.dataModeSelect);
            this.Controls.Add(this.lblTrioChannel);
            this.Controls.Add(this.lblTrioChannelHeader);
            this.Controls.Add(this.lblPlaylistName);
            this.Controls.Add(this.lblPlaylistNameHeader);
            this.Controls.Add(this.lblCurrentShow);
            this.Controls.Add(this.lblCurrentShowHeader);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Election Graphics Stack Builder Application  Version 1.0.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.dataModeSelect.ResumeLayout(false);
            this.tpRaces.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.gbSpF.ResumeLayout(false);
            this.gbSpF.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.gbRCF.ResumeLayout(false);
            this.gbRCF.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.gbROF.ResumeLayout(false);
            this.gbROF.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.availableRacesGrid)).EndInit();
            this.tpBalanceOfPower.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BOPdataGridView)).EndInit();
            this.tpReferendums.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReferendumsGrid)).EndInit();
            this.gbExitPolls.ResumeLayout(false);
            this.gbExitPolls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableExitPollsGrid)).EndInit();
            this.gbTime.ResumeLayout(false);
            this.pnlStack.ResumeLayout(false);
            this.pnlStack.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stackGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miAboutBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem utilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSelectDefaultShow;
        private System.Windows.Forms.Label lblCurrentShow;
        private System.Windows.Forms.Label lblCurrentShowHeader;
        private System.Windows.Forms.Label lblPlaylistNameHeader;
        private System.Windows.Forms.Label lblPlaylistName;
        private System.Windows.Forms.Label lblTrioChannelHeader;
        private System.Windows.Forms.Label lblTrioChannel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetStatusBarToolStripMenuItem;
        private System.Windows.Forms.TabControl dataModeSelect;
        private System.Windows.Forms.TabPage tpRaces;
        private System.Windows.Forms.TabPage tpExitPolls;
        private System.Windows.Forms.TabPage tpBalanceOfPower;
        private System.Windows.Forms.DataGridView availableRacesGrid;
        private System.Windows.Forms.Button btnAddBalanceOfPower;
        private System.Windows.Forms.DataGridView availableExitPollsGrid;
        private System.Windows.Forms.Button btnAddExitPoll;
        private System.Windows.Forms.DataGridView BOPdataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn st;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Question;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subset;
        private System.Windows.Forms.GroupBox gbROF;
        private System.Windows.Forms.RadioButton rbShowAll;
        private System.Windows.Forms.RadioButton rbGovernor;
        private System.Windows.Forms.RadioButton rbHouse;
        private System.Windows.Forms.RadioButton rbSenate;
        private System.Windows.Forms.RadioButton rbPresident;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnAddRace4Way;
        private System.Windows.Forms.Button btnSelect4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAddRace3WaySelect;
        private System.Windows.Forms.Button btnAddRace3Way;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAddRace2WaySelect;
        private System.Windows.Forms.Button btnAddRace2Way;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddRace1Way;
        private System.Windows.Forms.Button btnAddRace1WaySelect;
        private System.Windows.Forms.GroupBox gbRCF;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbCalled;
        private System.Windows.Forms.RadioButton rbJustCalled;
        private System.Windows.Forms.RadioButton rbTCTC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerStatusUpdate;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Panel pnlStack;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnLoadStack;
        private System.Windows.Forms.Button btnActivateStack;
        private System.Windows.Forms.Button btnSaveStack;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStackElementDown;
        private System.Windows.Forms.Button btnStackElementUp;
        private System.Windows.Forms.Label txtStack;
        private System.Windows.Forms.Label txtStackEntriesCount;
        private System.Windows.Forms.Label lblStackEntriesCount;
        private System.Windows.Forms.Label lblStackHeader;
        private System.Windows.Forms.DataGridView stackGrid;
        private System.Windows.Forms.GroupBox gbExitPolls;
        private System.Windows.Forms.RadioButton rbEPMan;
        private System.Windows.Forms.RadioButton rbEPAuto;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbSpF;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbPollClosing;
        private System.Windows.Forms.RadioButton rbBattleground;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblAvailRaceCnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn eType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn Session;
        private System.Windows.Forms.TabPage tpReferendums;
        private System.Windows.Forms.DataGridView ReferendumsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Button btnAddReferendum;
        private System.Windows.Forms.ComboBox cbGraphicConcept;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Race_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Race_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Element_Type_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stack_Entry_Description;
        private System.Windows.Forms.Button btnClearStack;
        private System.Windows.Forms.Button btnDeleteStackElement;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.Label lblHostName;
        private System.Windows.Forms.Label label3;
    }
}

