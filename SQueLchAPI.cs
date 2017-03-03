using MySql.Data.MySqlClient;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SQueLch
{
    class SQueLchAPI : IDisposable
    {
        private MySqlConnection connection;
        private TreeNode selectedDb;

        private TreeNode SelectedDb { get => selectedDb; set => selectedDb = value; }

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

        [Obsolete("GenerateFullTree is deprecated, please use GenerateDatabases, GenerateTables, and GenerateColumns instead.")]
        public TreeView GenerateTree()
        {
            return GenerateFullTree(new TreeView());
        }

        [Obsolete("GenerateFullTree is deprecated, please use GenerateDatabases, GenerateTables, and GenerateColumns instead.")]
        public TreeView GenerateFullTree(TreeView tv)
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

        public string GetSelectedDatabaseName()
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT DATABASE();";
            string database = "";

            using (MySqlDataReader dbReader = cmd.ExecuteReader())
            {
                while (dbReader.Read())
                {
                    database = dbReader.GetValue(0).ToString();
                }
            }

            return database;
        }

        public TreeView GenerateDatabases(TreeView tv)
        {
            tv.BeginUpdate();

            List<string> expandedNodes = CollectExpandedNodes(tv.Nodes);
            tv.Nodes.Clear();

            List<string> dbs = Databases();
            string selectedDb = SelectedDb?.Name ?? "";
            foreach (string db in dbs)
            {
                TreeNode dbNode = new TreeNode()
                {
                    Text = db,
                    Name = db
                };
                tv.Nodes.Add(dbNode);
                dbNode.Nodes.Add("");
                if (dbNode.Name == selectedDb)
                {
                    dbNode.NodeFont = new Font(tv.Font, FontStyle.Bold);
                }
            }

            RestoreExpandedNodes(tv, expandedNodes);

            tv.EndUpdate();
            return tv;
        }

        public TreeView GenerateTables(TreeView tv, TreeNode database)
        {
            tv.BeginUpdate();
            database.Nodes.Clear();

            List<string> tables = Tables(database.Text);
            foreach (string table in tables)
            {
                TreeNode tableNode = new TreeNode()
                {
                    Text = table,
                    Name = table
                };
                database.Nodes.Add(tableNode);
                tableNode.Nodes.Add("");
            }

            tv.EndUpdate();
            return tv;
        }

        public TreeView GenerateColumns(TreeView tv, TreeNode table)
        {
            tv.BeginUpdate();
            table.Nodes.Clear();

            List<string> columns = Columns(table.Parent.Text, table.Text);
            foreach (string column in columns)
            {
                TreeNode columnNode = new TreeNode()
                {
                    Text = column,
                    Name = column
                };
                table.Nodes.Add(columnNode);
            }

            tv.EndUpdate();
            return tv;
        }

        private List<string> Databases()
        {
            List<string> databases = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW DATABASES";
            string database;

            using (MySqlDataReader dbReader = cmd.ExecuteReader())
            {
                while (dbReader.Read())
                {
                    database = "";
                    for (int i = 0; i < dbReader.FieldCount; i++)
                        database += dbReader.GetValue(i).ToString();

                    databases.Add(database);
                }
            }

            return databases;
        }

        private List<string> Tables(string database)
        {
            List<string> tables = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW TABLES IN " + database;
            string table;

            using (MySqlDataReader tableReader = cmd.ExecuteReader())
            {
                while (tableReader.Read())
                {
                    table = "";
                    for (int i = 0; i < tableReader.FieldCount; i++)
                        table += tableReader.GetValue(i).ToString();

                    tables.Add(table);
                }
            }
            return tables;
        }

        private List<string> Columns(string db, string table)
        {
            List<string> columns = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW COLUMNS FROM " + db + "." + table;

            using (MySqlDataReader columnReader = cmd.ExecuteReader())
            {
                while (columnReader.Read())
                {
                    columns.Add(columnReader.GetValue(0).ToString());
                }
            }

            return columns;
        }

        public List<List<string>> Query(string query)
        {
            List<List<string>> results = new List<List<string>>();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            try
            {
                using (MySqlDataReader queryReader = cmd.ExecuteReader())
                {
                    Object[] row = new Object[queryReader.FieldCount];

                    while (queryReader.Read())
                    {
                        queryReader.GetValues(row);
                        results.Add(row.Cast<string>().ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                results.Add(new List<string>
                {
                    "ERROR: " + ex.Message,
                    ex.ToString()
                });
            }
            return results;
        }

        public void SelectDatabase(TreeView tv, TreeNode db)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "USE " + db.Text;
            cmd.ExecuteNonQuery();
            tv.BeginUpdate();
            if (SelectedDb != null)
            {
                //Finds previously selected Node by name to avoid Node-TreeView dissociation
                //Does not traverse child nodes since databases are at the root level
                SelectedDb = FindNodeByName(tv.Nodes, SelectedDb.Name, false);
                SelectedDb.NodeFont = new Font(SelectedDb.NodeFont, FontStyle.Regular);
                //Forces the text to be redrawn to prevent clipping
                SelectedDb.Text = SelectedDb.Text;
            }
            SelectedDb = db;
            SelectedDb.NodeFont = new Font(new TreeView().Font, FontStyle.Bold);
            //Forces the text to be redrawn to prevent clipping
            SelectedDb.Text = SelectedDb.Text;
            tv.EndUpdate();
        }

        //Resotres nodes that were expanded
        private void RestoreExpandedNodes(TreeView tv, List<string> expandedNodes)
        {
            if (expandedNodes.Count > 0)
            {
                tv.BeginUpdate();
                TreeNode node;
                for (int i = 0; i < expandedNodes.Count; i++)
                {
                    node = FindNodeByName(tv.Nodes, expandedNodes[i], true);
                    ExpandNodePath(node);
                }
                tv.EndUpdate();
            }
        }

        //Recursively collects all expanded nodes of a tree
        private List<string> CollectExpandedNodes(TreeNodeCollection nodes)
        {
            List<string> expandedNodes = new List<string>();
            foreach (TreeNode node in nodes)
            {
                if (node.IsExpanded)
                    expandedNodes.Add(node.Name);
                if (node.Nodes.Count > 0)
                    expandedNodes.AddRange(CollectExpandedNodes(node.Nodes));
            }
            return expandedNodes;
        }

        //Recursively finds a given node by name
        private TreeNode FindNodeByName(TreeNodeCollection nodes, string name, bool traverseChildNodes)
        {
            TreeNode returnNode = null;
            foreach (TreeNode node in nodes)
            {
                if (node.Name == name)
                {
                    return node;
                }
                else if (traverseChildNodes && node.Nodes.Count > 0)
                {
                    returnNode = FindNodeByName(node.Nodes, name, true);
                }

                if (returnNode != null)
                {
                    return returnNode;
                }
            }
            return null;
        }

        //Recursively restores expanded nodes
        private void ExpandNodePath(TreeNode node)
        {
            if (node == null) return;
            if (node.Level != 0)
            {
                node.Expand();
                ExpandNodePath(node.Parent);
            }
            else node.Expand();

        }

        public void Close()
        {
            connection.Close();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
