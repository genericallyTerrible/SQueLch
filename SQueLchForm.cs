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

                    MySqlCommand dbCmd = conn.CreateCommand();
                    dbCmd.CommandText = "show databases";
                    MySqlDataReader dbReader = dbCmd.ExecuteReader();
                    string database;
                    
                    while (dbReader.Read())
                    {
                        database = "";
                        for (int i = 0; i < dbReader.FieldCount; i++)
                            database += dbReader.GetValue(i).ToString();

                        databaseTV.Nodes.Add(database);
                    }
                    dbReader.Close();

                    MySqlCommand tableCmd = conn.CreateCommand();
                    MySqlDataReader tableReader;
                    string table;
                    
                    for (int i = 0; i < databaseTV.Nodes.Count; i++)
                    {
                        database = databaseTV.Nodes[i].Text;
                        tableCmd.CommandText = "SHOW TABLES IN " + database;
                        tableReader = tableCmd.ExecuteReader();
                        while (tableReader.Read())
                        {
                            table = "";
                            for (int j = 0; j < tableReader.FieldCount; j++)
                            {
                                table += tableReader.GetValue(j).ToString();
                            }

                            databaseTV.Nodes[i].Nodes.Add(table);
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
                Application.Exit();
            }
        }

        private void SQueLchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conn?.Close();
        }
    }
}
