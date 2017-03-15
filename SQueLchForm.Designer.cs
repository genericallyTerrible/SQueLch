namespace SQueLch
{
    partial class SQueLchForm
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
            this.schemasTree = new System.Windows.Forms.TreeView();
            this.schemaLbl = new System.Windows.Forms.Label();
            this.resultDGV = new System.Windows.Forms.DataGridView();
            this.consoleTbx = new System.Windows.Forms.TextBox();
            this.consoleLbl = new System.Windows.Forms.Label();
            this.outputTbx = new System.Windows.Forms.TextBox();
            this.outputLbl = new System.Windows.Forms.Label();
            this.resultsLbl = new System.Windows.Forms.Label();
            this.updateSchemasBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.resultDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // schemasTree
            // 
            this.schemasTree.Location = new System.Drawing.Point(13, 29);
            this.schemasTree.Name = "schemasTree";
            this.schemasTree.Size = new System.Drawing.Size(202, 258);
            this.schemasTree.TabIndex = 0;
            this.schemasTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.SchemasTree_BeforeExpand);
            this.schemasTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SchemasTree_KeyDown);
            // 
            // schemaLbl
            // 
            this.schemaLbl.AutoSize = true;
            this.schemaLbl.Location = new System.Drawing.Point(13, 13);
            this.schemaLbl.Name = "schemaLbl";
            this.schemaLbl.Size = new System.Drawing.Size(51, 13);
            this.schemaLbl.TabIndex = 1;
            this.schemaLbl.Text = "Schemas";
            // 
            // resultDGV
            // 
            this.resultDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultDGV.Location = new System.Drawing.Point(221, 29);
            this.resultDGV.Name = "resultDGV";
            this.resultDGV.ReadOnly = true;
            this.resultDGV.Size = new System.Drawing.Size(512, 287);
            this.resultDGV.TabIndex = 4;
            // 
            // consoleTbx
            // 
            this.consoleTbx.Location = new System.Drawing.Point(13, 335);
            this.consoleTbx.Multiline = true;
            this.consoleTbx.Name = "consoleTbx";
            this.consoleTbx.Size = new System.Drawing.Size(357, 177);
            this.consoleTbx.TabIndex = 5;
            this.consoleTbx.WordWrap = false;
            this.consoleTbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleTbx_KeyDown);
            // 
            // consoleLbl
            // 
            this.consoleLbl.AutoSize = true;
            this.consoleLbl.Location = new System.Drawing.Point(13, 319);
            this.consoleLbl.Name = "consoleLbl";
            this.consoleLbl.Size = new System.Drawing.Size(45, 13);
            this.consoleLbl.TabIndex = 6;
            this.consoleLbl.Text = "Console";
            // 
            // outputTbx
            // 
            this.outputTbx.Location = new System.Drawing.Point(376, 335);
            this.outputTbx.Multiline = true;
            this.outputTbx.Name = "outputTbx";
            this.outputTbx.ReadOnly = true;
            this.outputTbx.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputTbx.Size = new System.Drawing.Size(357, 177);
            this.outputTbx.TabIndex = 7;
            this.outputTbx.WordWrap = false;
            // 
            // outputLbl
            // 
            this.outputLbl.AutoSize = true;
            this.outputLbl.Location = new System.Drawing.Point(376, 319);
            this.outputLbl.Name = "outputLbl";
            this.outputLbl.Size = new System.Drawing.Size(39, 13);
            this.outputLbl.TabIndex = 8;
            this.outputLbl.Text = "Output";
            // 
            // resultsLbl
            // 
            this.resultsLbl.AutoSize = true;
            this.resultsLbl.Location = new System.Drawing.Point(221, 12);
            this.resultsLbl.Name = "resultsLbl";
            this.resultsLbl.Size = new System.Drawing.Size(42, 13);
            this.resultsLbl.TabIndex = 9;
            this.resultsLbl.Text = "Results";
            // 
            // updateSchemasBtn
            // 
            this.updateSchemasBtn.Location = new System.Drawing.Point(13, 293);
            this.updateSchemasBtn.Name = "updateSchemasBtn";
            this.updateSchemasBtn.Size = new System.Drawing.Size(202, 23);
            this.updateSchemasBtn.TabIndex = 2;
            this.updateSchemasBtn.Text = "Update Schemas";
            this.updateSchemasBtn.UseVisualStyleBackColor = true;
            this.updateSchemasBtn.Click += new System.EventHandler(this.UpdateSchemasBtn_Click);
            // 
            // SQueLchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 524);
            this.Controls.Add(this.resultsLbl);
            this.Controls.Add(this.outputLbl);
            this.Controls.Add(this.outputTbx);
            this.Controls.Add(this.consoleLbl);
            this.Controls.Add(this.consoleTbx);
            this.Controls.Add(this.resultDGV);
            this.Controls.Add(this.updateSchemasBtn);
            this.Controls.Add(this.schemaLbl);
            this.Controls.Add(this.schemasTree);
            this.Name = "SQueLchForm";
            this.Text = "SQueLch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SQueLchForm_FormClosing);
            this.Shown += new System.EventHandler(this.SQueLchForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.resultDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView schemasTree;
        private System.Windows.Forms.Label schemaLbl;
        private System.Windows.Forms.DataGridView resultDGV;
        private System.Windows.Forms.TextBox consoleTbx;
        private System.Windows.Forms.Label consoleLbl;
        private System.Windows.Forms.TextBox outputTbx;
        private System.Windows.Forms.Label outputLbl;
        private System.Windows.Forms.Label resultsLbl;
        private System.Windows.Forms.Button updateSchemasBtn;
    }
}

