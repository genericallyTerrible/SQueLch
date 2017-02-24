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
            this.schemasTreeView = new System.Windows.Forms.TreeView();
            this.schemaLbl = new System.Windows.Forms.Label();
            this.updateSchemasBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // databaseTV
            // 
            this.schemasTreeView.Location = new System.Drawing.Point(13, 29);
            this.schemasTreeView.Name = "databaseTV";
            this.schemasTreeView.Size = new System.Drawing.Size(163, 287);
            this.schemasTreeView.TabIndex = 0;
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
            // updateSchemasBtn
            // 
            this.updateSchemasBtn.Location = new System.Drawing.Point(12, 322);
            this.updateSchemasBtn.Name = "updateSchemasBtn";
            this.updateSchemasBtn.Size = new System.Drawing.Size(164, 23);
            this.updateSchemasBtn.TabIndex = 2;
            this.updateSchemasBtn.Text = "Update Schemas";
            this.updateSchemasBtn.UseVisualStyleBackColor = true;
            this.updateSchemasBtn.Click += new System.EventHandler(this.updateSchemasBtn_Click);
            // 
            // SQueLchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 357);
            this.Controls.Add(this.updateSchemasBtn);
            this.Controls.Add(this.schemaLbl);
            this.Controls.Add(this.schemasTreeView);
            this.Name = "SQueLchForm";
            this.Text = "SQueLch!";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SQueLchForm_FormClosing);
            this.Shown += new System.EventHandler(this.SQueLchForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView schemasTreeView;
        private System.Windows.Forms.Label schemaLbl;
        private System.Windows.Forms.Button updateSchemasBtn;
    }
}

