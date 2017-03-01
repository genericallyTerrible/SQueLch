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
                TreeNode dbNode = new TreeNode()
                {
                    Text = db
                };
                tv.Nodes.Add(dbNode);

                List<string> tables = Tables(db);
                foreach (string table in tables)
                {
                    TreeNode tableNode = new TreeNode()
                    {
                        Text = table
                    };
                    dbNode.Nodes.Add(tableNode);

                    List<string> columns = Columns(db, table);
                    foreach (string column in columns)
                    {
                        TreeNode columnNode = new TreeNode()
                        {
                            Text = column
                        };
                        tableNode.Nodes.Add(columnNode);
                    }
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
            cmd.CommandText = "SHOW TABLES IN " + database;
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

        private List<string> Columns(string db, string table)
        {
            List<string> columns = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW COLUMNS FROM " + db + "." + table;
            MySqlDataReader columnReader = cmd.ExecuteReader();
            string column;

            while (columnReader.Read())
            {
                columns.Add(columnReader.GetValue(0).ToString());
            }
            columnReader.Close();

            return columns;
        }

        public List<string> Query(string query)
        {
            List<string> results = new List<string>();
            try
            {

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader queryReader = cmd.ExecuteReader();
                string result;

                while (queryReader.Read())
                {
                    result = "";
                    for (int i = 0; i < queryReader.FieldCount; i++)
                        result += queryReader.GetValue(i).ToString();

                    results.Add(result);
                }
                queryReader.Close();

            }
            catch (Exception ex)
            {
                results.Add("ERROR: " + ex.Message);
                results.Add(ex.ToString());
            }
            return results;
        }

        public void UseDatabase(string db)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "USE " + db;
            cmd.ExecuteNonQuery();
        }
    }
}
