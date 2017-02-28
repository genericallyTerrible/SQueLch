using MySql.Data.MySqlClient;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQueLch
{
    class SQueLchAPI
    {
        private MySqlConnection connection;

        public SQueLchAPI()
        {
            connection = new MySqlConnection();
        }

        public bool Connect(string connectionString)
        {
            connection.ConnectionString = connectionString;

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return connection.State == ConnectionState.Open;
        }

        public void Disconnect()
        {
            connection.Close();
        }

        public TreeView GenerateTree()
        {
            return GenerateTree(new TreeView());
        }

        public TreeView GenerateTree(TreeView tv)
        {
            tv.BeginUpdate();
            tv.Nodes.Clear();

            List<string> dbs = Databases();
            foreach (string db in dbs)
            {
                TreeNode parent = new TreeNode()
                {
                    Text = db
                };
                tv.Nodes.Add(parent);

                List<string> tables = Tables(db);
                foreach (string table in tables)
                {
                    TreeNode child = new TreeNode()
                    {
                        Text = table
                    };
                    parent.Nodes.Add(child);
                }
            }

            tv.EndUpdate();
            return tv;
        }

        private List<string> Databases()
        {
            List<string> databases = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW DATABASES";
            MySqlDataReader dbReader = cmd.ExecuteReader();
            string database;

            while (dbReader.Read())
            {
                database = "";
                for (int i = 0; i < dbReader.FieldCount; i++)
                    database += dbReader.GetValue(i).ToString();

                databases.Add(database);
            }
            dbReader.Close();

            return databases;
        }

        private List<string> Tables(string database)
        {
            List<string> tables = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW TABLES IN " + database; ;
            MySqlDataReader tableReader = cmd.ExecuteReader();
            string table;

            while (tableReader.Read())
            {
                table = "";
                for (int i = 0; i < tableReader.FieldCount; i++)
                    table += tableReader.GetValue(i).ToString();

                tables.Add(table);
            }
            tableReader.Close();

            return tables;
        }
    }
}
