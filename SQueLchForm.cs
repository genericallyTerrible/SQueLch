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
            }
            else
            {
                Application.Exit();
            }

            try
            {
                Conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void SQueLchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conn?.Close();
        }
    }
}
