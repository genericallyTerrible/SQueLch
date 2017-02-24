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

        public MySqlConnection Conn { get => conn; protected set => conn = value; }

        public SQueLchForm()
        {
            InitializeComponent();
            Conn = new MySqlConnection();
        }

        private void SQueLchForm_Shown(object sender, EventArgs e)
        {
            ConnectionForm connectForm = new ConnectionForm();
            connectForm.ShowDialog();

            if (connectForm.DialogResult == DialogResult.OK)
            {
                Conn.ConnectionString = connectForm.ConnectionString;

                try
                {
                    Conn.Open();
                    UpdateSchemaTree();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
                Application.Exit();
            }
        }

        private void UpdateSchemaTree()
        {
            if (Conn.State == ConnectionState.Open)
            {
                schemasTreeView.Nodes.Clear();

                try
                {
                    MySqlCommand dbCmd = conn.CreateCommand();
                    dbCmd.CommandText = "show databases";
                    MySqlDataReader dbReader = dbCmd.ExecuteReader();
                    string database;

                    while (dbReader.Read())
                    {
                        database = "";
                        for (int i = 0; i < dbReader.FieldCount; i++)
                            database += dbReader.GetValue(i).ToString();

                        schemasTreeView.Nodes.Add(database);
                    }
                    dbReader.Close();

                    MySqlCommand tableCmd = conn.CreateCommand();
                    MySqlDataReader tableReader;
                    string table;

                    for (int i = 0; i < schemasTreeView.Nodes.Count; i++)
                    {
                        database = schemasTreeView.Nodes[i].Text;
                        tableCmd.CommandText = "SHOW TABLES IN " + database;
                        tableReader = tableCmd.ExecuteReader();
                        while (tableReader.Read())
                        {
                            table = "";
                            for (int j = 0; j < tableReader.FieldCount; j++)
                            {
                                table += tableReader.GetValue(j).ToString();
                            }

                            schemasTreeView.Nodes[i].Nodes.Add(table);
                        }
                        tableReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Failed to update treeview: No open SQL connection.");
            }
        }

        private void SQueLchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conn?.Close();
        }

        private void updateSchemasBtn_Click(object sender, EventArgs e)
        {
            UpdateSchemaTree();
        }
    }
}
