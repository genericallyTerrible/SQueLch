namespace SQueLch
{
    partial class ConnectionForm
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
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipAddr = new IPAddressControlLib.IPAddressControl();
            this.portTB = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.colonLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTB = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(12, 9);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(58, 13);
            this.ipLabel.TabIndex = 0;
            this.ipLabel.Text = "IP Address";
            // 
            // ipAddr
            // 
            this.ipAddr.AllowInternalTab = false;
            this.ipAddr.AutoHeight = true;
            this.ipAddr.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddr.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddr.Location = new System.Drawing.Point(15, 25);
            this.ipAddr.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipAddr.Name = "ipAddr";
            this.ipAddr.ReadOnly = false;
            this.ipAddr.Size = new System.Drawing.Size(87, 20);
            this.ipAddr.TabIndex = 4;
            this.ipAddr.Text = "127.0.0.1";
            // 
            // portTB
            // 
            this.portTB.Location = new System.Drawing.Point(131, 25);
            this.portTB.MaxLength = 4;
            this.portTB.Name = "portTB";
            this.portTB.Size = new System.Drawing.Size(40, 20);
            this.portTB.TabIndex = 5;
            this.portTB.Text = "3306";
            this.portTB.TextChanged += new System.EventHandler(this.PortTB_TextChanged);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(128, 9);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 6;
            this.portLabel.Text = "Port:";
            // 
            // colonLabel
            // 
            this.colonLabel.AutoSize = true;
            this.colonLabel.Location = new System.Drawing.Point(112, 28);
            this.colonLabel.Name = "colonLabel";
            this.colonLabel.Size = new System.Drawing.Size(10, 13);
            this.colonLabel.TabIndex = 7;
            this.colonLabel.Text = ":";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(12, 60);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(58, 13);
            this.usernameLabel.TabIndex = 8;
            this.usernameLabel.Text = "Username:";
            // 
            // usernameTB
            // 
            this.usernameTB.Location = new System.Drawing.Point(15, 76);
            this.usernameTB.Name = "usernameTB";
            this.usernameTB.Size = new System.Drawing.Size(156, 20);
            this.usernameTB.TabIndex = 9;
            this.usernameTB.Text = "root";
            this.usernameTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsernameTB_KeyDown);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(12, 111);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Password:";
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(15, 127);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.PasswordChar = '*';
            this.passwordTB.Size = new System.Drawing.Size(156, 20);
            this.passwordTB.TabIndex = 11;
            this.passwordTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTB_KeyDown);
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(15, 154);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 12;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(96, 153);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 13;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // ConnectionForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(186, 183);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameTB);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.colonLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.portTB);
            this.Controls.Add(this.ipAddr);
            this.Controls.Add(this.ipLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConnectionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ipLabel;
        private IPAddressControlLib.IPAddressControl ipAddr;
        private System.Windows.Forms.TextBox portTB;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label colonLabel;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTB;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}