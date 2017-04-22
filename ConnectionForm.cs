using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SQueLch
{
    public partial class ConnectionForm : Form
    {
        private ConnectionParameters connectionParams;

        public ConnectionParameters ConnectionParams { get => connectionParams; private set => connectionParams = value; }

        public ConnectionForm()
        {
            InitializeComponent();
        }

        public ConnectionForm(ConnectionParameters connectParams)
        {
            InitializeComponent();
            ipAddr.Text = connectParams.ServerIP.ToString();
            portTB.Text = connectParams.Port.ToString();
            usernameTB.Text = connectParams.UserID;
            passwordTB.Text = connectParams.Password;
        }

        private void PortTB_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = Regex.Replace(((TextBox)sender).Text, "[^0-9]", "");
            ((TextBox)sender).SelectionStart = ((TextBox)sender).SelectionStart;
        }

        private void PasswordTB_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox_ControlBackspace(sender, e);
        }

        private void UsernameTB_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox_ControlBackspace(sender, e);
        }

        private void TextBox_ControlBackspace(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Back)
                {
                    e.SuppressKeyPress = true;
                    int selStart = ((TextBox)sender).SelectionStart;
                    while (selStart > 0 && ((TextBox)sender).Text.Substring(selStart - 1, 1) == " ")
                    {
                        selStart--;
                    }
                    int prevSpacePos = -1;
                    if (selStart != 0)
                    {
                        prevSpacePos = ((TextBox)sender).Text.LastIndexOf(' ', selStart - 1);
                    }
                    ((TextBox)sender).Select(prevSpacePos + 1, ((TextBox)sender).SelectionStart - prevSpacePos - 1);
                    ((TextBox)sender).SelectedText = "";
                }
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            ConnectionParams = GenerateConnectionParams();
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private ConnectionParameters GenerateConnectionParams()
        {
            return new ConnectionParameters()
            {
                ServerIP = ipAddr.IPAddress,
                Port     = uint.Parse(portTB.Text),
                UserID   = usernameTB.Text,
                Password = passwordTB.Text
            };
        }
    }
}
