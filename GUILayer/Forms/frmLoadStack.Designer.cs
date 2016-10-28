namespace GUILayer.Forms
{
    partial class frmLoadStack
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnActivateStack = new System.Windows.Forms.Button();
            this.btnLoadStack = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.availableStacksGrid = new System.Windows.Forms.DataGridView();
            this.btnDeleteStack = new System.Windows.Forms.Button();
            this.btnCancelStackLoad = new System.Windows.Forms.Button();
            this.ixStackID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShowName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StackName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.availableStacksGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // btnActivateStack
            // 
            this.btnActivateStack.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnActivateStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivateStack.Image = global::GUILayer.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            this.btnActivateStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActivateStack.Location = new System.Drawing.Point(74, 677);
            this.btnActivateStack.Name = "btnActivateStack";
            this.btnActivateStack.Size = new System.Drawing.Size(165, 65);
            this.btnActivateStack.TabIndex = 2;
            this.btnActivateStack.Text = "       Activate Stack\r\n             (Ctrl-A)\r\n";
            this.btnActivateStack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActivateStack.UseVisualStyleBackColor = true;
            // 
            // btnLoadStack
            // 
            this.btnLoadStack.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLoadStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadStack.Image = global::GUILayer.Properties.Resources.folder_Open_16xLG;
            this.btnLoadStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadStack.Location = new System.Drawing.Point(74, 592);
            this.btnLoadStack.Name = "btnLoadStack";
            this.btnLoadStack.Size = new System.Drawing.Size(165, 65);
            this.btnLoadStack.TabIndex = 1;
            this.btnLoadStack.Text = "Load Stack\r\n(Ctrl-L)";
            this.btnLoadStack.UseVisualStyleBackColor = true;
            this.btnLoadStack.Click += new System.EventHandler(this.btnLoadStack_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Available Stacks:";
            // 
            // availableStacksGrid
            // 
            this.availableStacksGrid.AllowUserToAddRows = false;
            this.availableStacksGrid.AllowUserToDeleteRows = false;
            this.availableStacksGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.availableStacksGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.availableStacksGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.availableStacksGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availableStacksGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ixStackID,
            this.ShowName,
            this.StackName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.availableStacksGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.availableStacksGrid.Location = new System.Drawing.Point(14, 28);
            this.availableStacksGrid.MultiSelect = false;
            this.availableStacksGrid.Name = "availableStacksGrid";
            this.availableStacksGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.availableStacksGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.availableStacksGrid.RowHeadersWidth = 15;
            this.availableStacksGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.availableStacksGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availableStacksGrid.Size = new System.Drawing.Size(511, 546);
            this.availableStacksGrid.TabIndex = 0;
            this.availableStacksGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableStacksGrid_CellClick);
            this.availableStacksGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableStacksGrid_CellContentClick);
            this.availableStacksGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableStacksGrid_CellContentDoubleClick);
            this.availableStacksGrid.CurrentCellChanged += new System.EventHandler(this.availableStacksGrid_CurrentCellChanged);
            this.availableStacksGrid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.availableStacksGrid_Scroll);
            this.availableStacksGrid.DoubleClick += new System.EventHandler(this.availableStacksGrid_DoubleClick);
            this.availableStacksGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.availableStacksGrid_KeyDown);
            // 
            // btnDeleteStack
            // 
            this.btnDeleteStack.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnDeleteStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteStack.Image = global::GUILayer.Properties.Resources.StatusAnnotations_Blocked_16xLG_color;
            this.btnDeleteStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteStack.Location = new System.Drawing.Point(300, 592);
            this.btnDeleteStack.Name = "btnDeleteStack";
            this.btnDeleteStack.Size = new System.Drawing.Size(165, 65);
            this.btnDeleteStack.TabIndex = 3;
            this.btnDeleteStack.Text = "Delete Stack\r\n(Ctrl-D)";
            this.btnDeleteStack.UseVisualStyleBackColor = true;
            this.btnDeleteStack.Click += new System.EventHandler(this.btnDeleteStack_Click);
            // 
            // btnCancelStackLoad
            // 
            this.btnCancelStackLoad.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelStackLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelStackLoad.Image = global::GUILayer.Properties.Resources.action_Cancel_16xLG;
            this.btnCancelStackLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelStackLoad.Location = new System.Drawing.Point(300, 677);
            this.btnCancelStackLoad.Name = "btnCancelStackLoad";
            this.btnCancelStackLoad.Size = new System.Drawing.Size(165, 65);
            this.btnCancelStackLoad.TabIndex = 4;
            this.btnCancelStackLoad.Text = "Cancel\r\n(Ctrl-C)\r\n";
            this.btnCancelStackLoad.UseVisualStyleBackColor = true;
            this.btnCancelStackLoad.Click += new System.EventHandler(this.btnCancelStackLoad_Click);
            // 
            // ixStackID
            // 
            this.ixStackID.DataPropertyName = "ixStackID";
            this.ixStackID.HeaderText = "ID";
            this.ixStackID.Name = "ixStackID";
            this.ixStackID.ReadOnly = true;
            this.ixStackID.Visible = false;
            this.ixStackID.Width = 125;
            // 
            // ShowName
            // 
            this.ShowName.DataPropertyName = "ShowName";
            this.ShowName.HeaderText = "Show Name";
            this.ShowName.Name = "ShowName";
            this.ShowName.ReadOnly = true;
            this.ShowName.Width = 175;
            // 
            // StackName
            // 
            this.StackName.DataPropertyName = "StackName";
            this.StackName.HeaderText = "Stack Name";
            this.StackName.Name = "StackName";
            this.StackName.ReadOnly = true;
            this.StackName.Width = 327;
            // 
            // frmLoadStack
            // 
            this.AcceptButton = this.btnLoadStack;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.CancelButton = this.btnCancelStackLoad;
            this.ClientSize = new System.Drawing.Size(538, 754);
            this.Controls.Add(this.btnCancelStackLoad);
            this.Controls.Add(this.btnActivateStack);
            this.Controls.Add(this.btnLoadStack);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.availableStacksGrid);
            this.Controls.Add(this.btnDeleteStack);
            this.Location = new System.Drawing.Point(550, 150);
            this.Name = "frmLoadStack";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Stack";
            ((System.ComponentModel.ISupportInitialize)(this.availableStacksGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnActivateStack;
        private System.Windows.Forms.Button btnLoadStack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView availableStacksGrid;
        private System.Windows.Forms.Button btnDeleteStack;
        private System.Windows.Forms.Button btnCancelStackLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixStackID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShowName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StackName;
    }
}