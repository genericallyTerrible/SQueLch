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

        public MySqlConnection Conn { get => conn; protected set => conn = value; }

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
            
        }

        private void updateSchemasBtn_Click(object sender, EventArgs e)
        {
            sqlAPI.GenerateTree(schemasTree);
            //UpdateSchemaTree();
        }
    }
}
