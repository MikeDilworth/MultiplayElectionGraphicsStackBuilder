﻿namespace GUILayer.Forms
{
    partial class FrmCandidateSelect
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
            this.dgvCand1 = new System.Windows.Forms.DataGridView();
            this.candLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.party = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CandID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvCand2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCand3 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCand4 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand4)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCand1
            // 
            this.dgvCand1.AllowUserToAddRows = false;
            this.dgvCand1.AllowUserToDeleteRows = false;
            this.dgvCand1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCand1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.candLastName,
            this.party,
            this.CandID});
            this.dgvCand1.Location = new System.Drawing.Point(35, 80);
            this.dgvCand1.MultiSelect = false;
            this.dgvCand1.Name = "dgvCand1";
            this.dgvCand1.ReadOnly = true;
            this.dgvCand1.RowHeadersWidth = 15;
            this.dgvCand1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCand1.Size = new System.Drawing.Size(320, 445);
            this.dgvCand1.TabIndex = 0;
            this.dgvCand1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand1_CellContentClick);
            this.dgvCand1.Click += new System.EventHandler(this.dgvCand1_Click);
            // 
            // candLastName
            // 
            this.candLastName.DataPropertyName = "LastNameAir";
            this.candLastName.HeaderText = "Candidate Name";
            this.candLastName.Name = "candLastName";
            this.candLastName.ReadOnly = true;
            this.candLastName.Width = 150;
            // 
            // party
            // 
            this.party.DataPropertyName = "CandidatePartyID";
            this.party.HeaderText = "Party";
            this.party.Name = "party";
            this.party.ReadOnly = true;
            this.party.Width = 50;
            // 
            // CandID
            // 
            this.CandID.DataPropertyName = "CandidateID";
            this.CandID.HeaderText = "CandID";
            this.CandID.Name = "CandID";
            this.CandID.ReadOnly = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(64, 598);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 50);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(228, 598);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 50);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of Candidates:";
            // 
            // dgvCand2
            // 
            this.dgvCand2.AllowUserToAddRows = false;
            this.dgvCand2.AllowUserToDeleteRows = false;
            this.dgvCand2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCand2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvCand2.Location = new System.Drawing.Point(35, 97);
            this.dgvCand2.MultiSelect = false;
            this.dgvCand2.Name = "dgvCand2";
            this.dgvCand2.ReadOnly = true;
            this.dgvCand2.RowHeadersWidth = 15;
            this.dgvCand2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCand2.Size = new System.Drawing.Size(320, 445);
            this.dgvCand2.TabIndex = 4;
            this.dgvCand2.Visible = false;
            this.dgvCand2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand2_CellClick);
            this.dgvCand2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand2_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "LastNameAir";
            this.dataGridViewTextBoxColumn1.HeaderText = "Candidate Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CandidatePartyID";
            this.dataGridViewTextBoxColumn2.HeaderText = "Party";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CandidateID";
            this.dataGridViewTextBoxColumn3.HeaderText = "CandID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dgvCand3
            // 
            this.dgvCand3.AllowUserToAddRows = false;
            this.dgvCand3.AllowUserToDeleteRows = false;
            this.dgvCand3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCand3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dgvCand3.Location = new System.Drawing.Point(35, 116);
            this.dgvCand3.MultiSelect = false;
            this.dgvCand3.Name = "dgvCand3";
            this.dgvCand3.ReadOnly = true;
            this.dgvCand3.RowHeadersWidth = 15;
            this.dgvCand3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCand3.Size = new System.Drawing.Size(320, 445);
            this.dgvCand3.TabIndex = 5;
            this.dgvCand3.Visible = false;
            this.dgvCand3.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand3_CellClick);
            this.dgvCand3.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand3_CellContentClick);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "LastNameAir";
            this.dataGridViewTextBoxColumn4.HeaderText = "Candidate Name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "CandidatePartyID";
            this.dataGridViewTextBoxColumn5.HeaderText = "Party";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "CandidateID";
            this.dataGridViewTextBoxColumn6.HeaderText = "CandID";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "label2";
            // 
            // dgvCand4
            // 
            this.dgvCand4.AllowUserToAddRows = false;
            this.dgvCand4.AllowUserToDeleteRows = false;
            this.dgvCand4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCand4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.dgvCand4.Location = new System.Drawing.Point(35, 136);
            this.dgvCand4.MultiSelect = false;
            this.dgvCand4.Name = "dgvCand4";
            this.dgvCand4.ReadOnly = true;
            this.dgvCand4.RowHeadersWidth = 15;
            this.dgvCand4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCand4.Size = new System.Drawing.Size(320, 445);
            this.dgvCand4.TabIndex = 7;
            this.dgvCand4.Visible = false;
            this.dgvCand4.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand4_CellClick);
            this.dgvCand4.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCand4_CellContentClick);
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "LastNameAir";
            this.dataGridViewTextBoxColumn7.HeaderText = "Candidate Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 150;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CandidatePartyID";
            this.dataGridViewTextBoxColumn8.HeaderText = "Party";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 50;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "CandidateID";
            this.dataGridViewTextBoxColumn9.HeaderText = "CandID";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // FrmCandidateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(392, 671);
            this.Controls.Add(this.dgvCand4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvCand3);
            this.Controls.Add(this.dgvCand2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvCand1);
            this.Name = "FrmCandidateSelect";
            this.Text = "Candidate Select";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCandidateSelect_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCand4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCand1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn candLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn party;
        private System.Windows.Forms.DataGridViewTextBoxColumn CandID;
        private System.Windows.Forms.DataGridView dgvCand2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView dgvCand3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCand4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
    }
}