using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SQueLch
{
    public partial class SQueLchForm : Form
    {

        private MySqlConnection conn;
        private SQueLchAPI sqlAPI;
        private TreeNode selectedDB;

        public MySqlConnection Conn { get => conn;       protected set => conn       = value; }
        public TreeNode SelectedDB  { get => selectedDB; protected set => selectedDB = value; }

        public SQueLchForm()
        {
            InitializeComponent();
            Conn = new MySqlConnection();
            sqlAPI = new SQueLchAPI();
        }

        private void SQueLchForm_Shown(object sender, EventArgs e)
        {
            ConnectionForm connectForm = new ConnectionForm();
            connectForm.ShowDialog();

            if (connectForm.DialogResult == DialogResult.OK)
            {
                bool success = sqlAPI.Connect(connectForm.ConnectionString);
                if (success)
                    sqlAPI.GenerateTree(schemasTree);
            }
            else
            {
                Application.Exit();
            }
        }

        private void SQueLchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlAPI.Disconnect();
        }

        private void UpdateSchemasBtn_Click(object sender, EventArgs e)
        {
            updateSchemasBtn.Enabled = false;
            sqlAPI.GenerateTree(schemasTree);
            updateSchemasBtn.Enabled = true;
        }

        private void ConsoleTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;

                    List<string> results = sqlAPI.Query(consoleTbx.Text);
                    foreach (string result in results)
                    {
                        outputTbx.AppendText(result);
                        outputTbx.AppendText(Environment.NewLine);
                    }
                    sqlAPI.GenerateTree(schemasTree);
                }

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

        private void SchemasTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            TreeNode tn = e.Node;
            while (tn.Parent != null)
                tn = tn.Parent;

            sqlAPI.UseDatabase(tn.Text);

            if (SelectedDB != null)
            {
                SelectedDB.NodeFont = new Font(tv.Font, FontStyle.Regular);
            }

            SelectedDB = tn;
            SelectedDB.NodeFont = new Font(tv.Font, FontStyle.Bold);
            tv.Update();
        }
    }
}
