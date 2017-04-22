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
using SQueLch.Properties;
using System.Resources;
using System.IO;
using System.Reflection;

namespace SQueLch
{
    public partial class SQueLchForm : Form
    {
        private SQueLchAPI sqlAPI;

        private List<string> commands = new List<string>(new string[]
        {
            "help",
            "exit",
            "clear",
            "update",
            "connect",
            "updat"
        });

        private string UserSettingsPath = Directory.GetCurrentDirectory() + "\\UserParameters.xml";

        public SQueLchForm()
        {
            InitializeComponent();
            sqlAPI = new SQueLchAPI();
            ActiveControl = consoleTbx;
        }

        private void SQueLchForm_Shown(object sender, EventArgs e)
        {
            Connect();
        }

        private void SQueLchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlAPI.Close();
        }

        private void Query(string query)
        {
            Result result = sqlAPI.Query(query);
            if (result.Success)
            {
                resultDGV.DataSource = result.ResultTable;
            }
            outputTbx.AppendText(result.Action + Environment.NewLine);
            outputTbx.AppendText(result.Message + Environment.NewLine);

            sqlAPI.GenerateDatabases(schemasTree);
        }

        private bool Connect()
        {
            ConnectionForm connectForm = new ConnectionForm();
            connectForm.ShowDialog();
            bool success = false;

            if (connectForm.DialogResult == DialogResult.OK)
            {
                success = sqlAPI.Connect(connectForm.ConnectionParams.ToString());
                if (success)
                    sqlAPI.GenerateDatabases(schemasTree);
                else
                {
                    outputTbx.AppendText("ERROR: Failed to connect to server" + Environment.NewLine);
                    outputTbx.AppendText("Use command \"!connect\" to try again." + Environment.NewLine);
                }
            }

            return success;
        }

        private bool Connect(string connectionString)
        {
            bool success = sqlAPI.Connect(connectionString);

            if (success)
                sqlAPI.GenerateDatabases(schemasTree);
            else
            {
                outputTbx.AppendText("ERROR: Failed to connect to server" + Environment.NewLine);
                outputTbx.AppendText("Use command \"!connect\" to try again." + Environment.NewLine);
            }

            return success;
        }

        private void ConsoleTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                //ctrl + enter
                if (e.KeyCode == Keys.Enter)
                {
                    string consoleText = consoleTbx.Text.Trim().ToLower();

                    //There is a command
                    if (consoleText.Length > 0)
                    {
                        //User command
                        if (consoleText[0] == '!')
                        {
                            string[] consoleSplit = consoleText.Substring(1).Split(' ');

                            //help command
                            if (consoleSplit[0].Equals(commands[0]))
                            {
                                if (consoleSplit.Count() == 1)
                                {
                                    outputTbx.AppendText("List of registered commands:" + Environment.NewLine);
                                    for (int i = 1; i < commands.Count() - 1; i++)
                                    {
                                        outputTbx.AppendText("\t" + commands[i] + Environment.NewLine);
                                    }
                                }
                                else
                                {
                                    //help with exit
                                    if (consoleSplit[1] == commands[1])
                                    {
                                        outputTbx.AppendText("Closes the application." + Environment.NewLine);
                                    }
                                    //help with clear
                                    else if (consoleSplit[1] == commands[2])
                                    {
                                        outputTbx.AppendText("Clears the contents of a specified container." + Environment.NewLine);
                                        outputTbx.AppendText("\t-a: Clears all elements in bot the Output TextBox" + Environment.NewLine);
                                        outputTbx.AppendText("\t-o: Clears all elements in the Output TextBox" + Environment.NewLine);
                                        outputTbx.AppendText("\t-r: Clears all elements in the Results DataGridView" + Environment.NewLine);
                                        outputTbx.AppendText("\t-s: Clears all elements in the Schemas TreeView" + Environment.NewLine);
                                        outputTbx.AppendText("\t    and the Results DataGridView" + Environment.NewLine);
                                    }
                                    //help with update
                                    else if (consoleSplit[1] == commands[3])
                                    {
                                        outputTbx.AppendText("Forces an update of the Schemas TreeView." + Environment.NewLine);
                                        outputTbx.AppendText("Functionally identical to clicking the Update Schemas Button." + Environment.NewLine);
                                    }
                                    //help with connect
                                    else if (consoleSplit[1] == commands[4])
                                    {
                                        outputTbx.AppendText("Used to change connected databases." + Environment.NewLine);
                                        outputTbx.AppendText("\t-d: Attempts to connect to a local database with default credentials" + Environment.NewLine);
                                        outputTbx.AppendText("\t-s: Prompts for user credentials to be saved for use with -u" + Environment.NewLine);
                                        outputTbx.AppendText("\t-u: Attempts to connect to user specified database with user provided parameters" + Environment.NewLine);
                                    }
                                    //unrecognized command
                                    else
                                    {
                                        outputTbx.AppendText("\"" + consoleTbx.Text.Trim().Substring(6) + "\" is not a registered command" + Environment.NewLine);
                                        outputTbx.AppendText(Environment.NewLine);
                                    }
                                }
                                outputTbx.AppendText(Environment.NewLine);
                            }
                            //exit command
                            else if (consoleSplit[0].Equals(commands[1]))
                            {
                                Application.Exit();
                            }
                            //clear command
                            else if (consoleSplit[0].Equals(commands[2]))
                            {
                                for (int i = 1; i < consoleSplit.Count(); i++)
                                {
                                    switch (consoleSplit[i])
                                    {
                                        case "-a":
                                            outputTbx.Text = "";
                                            resultDGV.DataSource = null;
                                            break;
                                        case "-o":
                                            outputTbx.Text = "";
                                            break;
                                        case "-r":
                                            resultDGV.DataSource = null;
                                            break;
                                        case "-s":
                                            schemasTree.Nodes.Clear();
                                            break;
                                    }
                                }
                            }
                            //update command
                            else if (consoleSplit[0].Equals(commands[3]))
                            {
                                UpdateSchemasBtn_Click(sender, e);
                            }
                            //connect command
                            else if (consoleSplit[0].Equals(commands[4]))
                            {
                                //Basic connect, show ConnectionForm
                                if (consoleSplit.Count() == 1)
                                {
                                    BeginInvoke(new Action(() => Connect()));
                                }
                                //Connect with flags
                                else
                                {
                                    switch (consoleSplit[1])
                                    {
                                        //attempt to connect using default localhost settings
                                        case "-d":
                                            Connect(ConnectionParameters.DefaultConnectionString());
                                            break;
                                        //attempt to connect using user settings
                                        case "-u":
                                            Connect(ConnectionParameters.Deserialize(UserSettingsPath).ToString());
                                            break;
                                        //save user settings
                                        case "-s":
                                            BeginInvoke(new Action(() =>
                                            {
                                                ConnectionForm connectForm = new ConnectionForm(ConnectionParameters.Deserialize(UserSettingsPath));

                                                connectForm.ShowDialog();
                                                if (connectForm.DialogResult == DialogResult.OK)
                                                {
                                                    ConnectionParameters cp = connectForm.ConnectionParams;
                                                    cp.Serialize(UserSettingsPath);
                                                    outputTbx.AppendText("Custom parameters written to:" + Environment.NewLine);
                                                    outputTbx.AppendText(UserSettingsPath + Environment.NewLine);
                                                }
                                            }));
                                            break;
                                    }
                                }
                            }
                            //it's a secret
                            else if (consoleSplit[0].Equals(commands[commands.Count - 1]))
                            {
                                if (consoleSplit.Count() == 2 && consoleSplit[1] == "-ass")
                                {
                                    //Dankest of memes
                                    BeginInvoke(new Action(() =>
                                    {
                                        using (Form form = new Form())
                                        {
                                            Assembly myAssembly = Assembly.GetExecutingAssembly();
                                            Stream myStream = myAssembly.GetManifestResourceStream("SQueLch.rettererDank.png");
                                            if (myStream != null)
                                            {
                                                Bitmap img = new Bitmap(myStream);

                                                form.StartPosition = FormStartPosition.CenterScreen;
                                                form.Size = img.Size;

                                                PictureBox pb = new PictureBox()
                                                {
                                                    Dock = DockStyle.Fill,
                                                    Image = img
                                                };
                                                form.Controls.Add(pb);
                                                form.ShowDialog();
                                            }
                                        }
                                    }));
                                }
                            }
                            //unrecognized command
                            else
                            {
                                outputTbx.AppendText("\"" + consoleText + "\" is not a registered command" + Environment.NewLine);
                                outputTbx.AppendText(Environment.NewLine);
                            }
                        }
                        //MySQL command
                        else
                        {
                            Query(consoleText);
                            /*
                            List<List<string>> results = sqlAPI.QueryStr(consoleTbx.Text);
                            foreach (List<string> row in results)
                            {
                                outputTbx.AppendText(string.Join(",", row.ToArray()));
                                outputTbx.AppendText(Environment.NewLine);
                            }
                            DataTable dt = sqlAPI.QueryDT(consoleTbx.Text);
                            */
                        }
                    }

                    e.SuppressKeyPress = true;
                }

                //ctrl + backspace
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

                //ctrl + a
                if ((e.KeyCode == Keys.A))
                {
                    e.SuppressKeyPress = true;
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void SchemasTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            TreeNode tn = e.Node;

            int parentCount = 0;
            TreeNode tnParent = tn.Parent;
            while (tnParent != null)
            {
                parentCount++;
                tnParent = tnParent.Parent;
            }
            switch (parentCount)
            {
                case 0:
                    //No parent, therefore it is a db being expanded
                    sqlAPI.GenerateTables(tv, tn);
                    break;
                case 1:
                    //Has one parent, therefore is a table being expanded
                    sqlAPI.GenerateColumns(tv, tn);
                    break;
            }
        }

        private void UpdateSchemasBtn_Click(object sender, EventArgs e)
        {
            updateSchemasBtn.Enabled = false;
            sqlAPI.GenerateDatabases(schemasTree);
            updateSchemasBtn.Enabled = true;
        }

        private void SchemasTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TreeView tv = (TreeView)sender;
                if (tv.SelectedNode != null)
                {
                    e.SuppressKeyPress = true;

                    TreeNode tn = tv.SelectedNode;
                    TreeNode tnTopLevel = tn;
                    int parentCount = 0;

                    while (tnTopLevel.Parent != null)
                    {
                        parentCount++;
                        tnTopLevel = tnTopLevel.Parent;
                    }

                    sqlAPI.SelectDatabase(schemasTree, tnTopLevel);

                    //If the selected node is a table node
                    if (parentCount == 1)
                    {
                        Query("select * from " + tn.Text);
                    }
                }
            }
        }
    }
}
