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
            this.databaseTV = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // databaseTV
            // 
            this.databaseTV.Location = new System.Drawing.Point(13, 13);
            this.databaseTV.Name = "databaseTV";
            this.databaseTV.Size = new System.Drawing.Size(163, 332);
            this.databaseTV.TabIndex = 0;
            // 
            // SQueLchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 357);
            this.Controls.Add(this.databaseTV);
            this.Name = "SQueLchForm";
            this.Text = "SQueLch!";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SQueLchForm_FormClosing);
            this.Shown += new System.EventHandler(this.SQueLchForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView databaseTV;
    }
}

