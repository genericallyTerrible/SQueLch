﻿using System;
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
        private SQueLchAPI sqlAPI;

        public SQueLchForm()
        {
            InitializeComponent();
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
                    sqlAPI.GenerateDatabases(schemasTree);
                else
                    outputTbx.AppendText("ERROR: Failed to connect to server");
            }
            else
            {
                Application.Exit();
            }
        }

        private void SQueLchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlAPI.Close();
        }

        private void ConsoleTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;

                    List<List<string>> results = sqlAPI.Query(consoleTbx.Text);
                    foreach (List<string> row in results)
                    {
                        outputTbx.AppendText(string.Join(",",row.ToArray()));
                        outputTbx.AppendText(Environment.NewLine);
                    }
                    sqlAPI.GenerateDatabases(schemasTree);
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
            if (tn.Parent == null)
            {
                //No parent, therefore it is a db being expanded
                sqlAPI.GenerateTables(tv, tn);
            }
            else
            {
                //Has a parent, therefore is a table being expanded
                sqlAPI.GenerateColumns(tv, tn);
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
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TreeView tv = (TreeView)sender;
                TreeNode tn = tv.SelectedNode;
                while (tn.Parent != null)
                    tn = tn.Parent;

                sqlAPI.SelectDatabase(schemasTree, tn);
            }
        }
    }
}
