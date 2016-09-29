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
            this.button1 = new System.Windows.Forms.Button();
            this.btnLoadStack = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.availableStacksGrid = new System.Windows.Forms.DataGridView();
            this.ixStackID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShowName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StackName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteStack = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.availableStacksGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::GUILayer.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(123, 671);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 40);
            this.button1.TabIndex = 117;
            this.button1.Text = "Activate Stack";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLoadStack
            // 
            this.btnLoadStack.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLoadStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadStack.Image = global::GUILayer.Properties.Resources.folder_Open_16xLG;
            this.btnLoadStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadStack.Location = new System.Drawing.Point(38, 614);
            this.btnLoadStack.Name = "btnLoadStack";
            this.btnLoadStack.Size = new System.Drawing.Size(165, 40);
            this.btnLoadStack.TabIndex = 116;
            this.btnLoadStack.Text = "  Load Stack";
            this.btnLoadStack.UseVisualStyleBackColor = true;
            this.btnLoadStack.Click += new System.EventHandler(this.btnLoadStack_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(40, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 16);
            this.label3.TabIndex = 115;
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
            this.availableStacksGrid.Location = new System.Drawing.Point(40, 48);
            this.availableStacksGrid.MaximumSize = new System.Drawing.Size(370, 577);
            this.availableStacksGrid.MinimumSize = new System.Drawing.Size(350, 500);
            this.availableStacksGrid.MultiSelect = false;
            this.availableStacksGrid.Name = "availableStacksGrid";
            this.availableStacksGrid.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.availableStacksGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.availableStacksGrid.RowHeadersWidth = 15;
            this.availableStacksGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.availableStacksGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availableStacksGrid.Size = new System.Drawing.Size(350, 546);
            this.availableStacksGrid.TabIndex = 114;
            this.availableStacksGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableStacksGrid_CellClick);
            this.availableStacksGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableStacksGrid_CellContentClick);
            this.availableStacksGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.availableStacksGrid_CellContentDoubleClick);
            this.availableStacksGrid.DoubleClick += new System.EventHandler(this.availableStacksGrid_DoubleClick);
            // 
            // ixStackID
            // 
            this.ixStackID.DataPropertyName = "ixStackID";
            this.ixStackID.HeaderText = "ID";
            this.ixStackID.Name = "ixStackID";
            this.ixStackID.ReadOnly = true;
            this.ixStackID.Width = 40;
            // 
            // ShowName
            // 
            this.ShowName.DataPropertyName = "ShowName";
            this.ShowName.HeaderText = "Show Name";
            this.ShowName.Name = "ShowName";
            this.ShowName.ReadOnly = true;
            this.ShowName.Width = 115;
            // 
            // StackName
            // 
            this.StackName.DataPropertyName = "StackName";
            this.StackName.HeaderText = "Stack Name";
            this.StackName.Name = "StackName";
            this.StackName.ReadOnly = true;
            this.StackName.Width = 150;
            // 
            // btnDeleteStack
            // 
            this.btnDeleteStack.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnDeleteStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteStack.Image = global::GUILayer.Properties.Resources.action_Cancel_16xLG;
            this.btnDeleteStack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteStack.Location = new System.Drawing.Point(223, 614);
            this.btnDeleteStack.Name = "btnDeleteStack";
            this.btnDeleteStack.Size = new System.Drawing.Size(165, 40);
            this.btnDeleteStack.TabIndex = 113;
            this.btnDeleteStack.Text = "  Delete Stack";
            this.btnDeleteStack.UseVisualStyleBackColor = true;
            this.btnDeleteStack.Click += new System.EventHandler(this.btnDeleteStack_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(123, 727);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 40);
            this.button2.TabIndex = 118;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmLoadStack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(433, 779);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLoadStack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView availableStacksGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixStackID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShowName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StackName;
        private System.Windows.Forms.Button btnDeleteStack;
        private System.Windows.Forms.Button button2;
    }
}