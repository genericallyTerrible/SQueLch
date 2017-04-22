using MySql.Data.MySqlClient;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SQueLch
{
    class SQueLchAPI : IDisposable
    {
        #region Maintaining TreeView scroll position
        /* Source for maintaining TreeView scroll position: http://stackoverflow.com/a/359896 */

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetScrollPos(int hWnd, int nBar);

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        #endregion
        
        private MySqlConnection connection;

        private TreeNode selectedDb;
        private TreeNode SelectedDb
        {
            get => selectedDb;
            set => selectedDb = value;
        }

        public string SelectedDatabaseName
        {
            get
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT DATABASE();";
                string database = "";
                try
                {
                    using (MySqlDataReader dbReader = cmd.ExecuteReader())
                    {
                        while (dbReader.Read())
                        {
                            database = dbReader.GetValue(0).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return database;
            }
        }

        public SQueLchAPI()
        {
            connection = new MySqlConnection();
        }

        #region Depricated TreeView generation functions
        [Obsolete("GenerateFullTree is deprecated, please use GenerateDatabases, GenerateTables, and GenerateColumns instead.")]
        public TreeView GenerateNewTree()
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
        #endregion

        #region TreeView generation
        public TreeView GenerateDatabases(TreeView tv)
        {
            Point scrollPos = GetTreeViewScrollPos(tv);
            tv.BeginUpdate();
            List<string> expandedNodes = CollectExpandedNodes(tv.Nodes);
            tv.Nodes.Clear();

            List<string> dbs = Databases();

            //If there is an open connection/are any databases
            if (dbs.Count > 0)
            {
                string selectedDb = SelectedDatabaseName; //SelectedDb?.Name ?? "";

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
                        SelectedDb = dbNode;
                        dbNode.NodeFont = new Font(tv.Font, FontStyle.Bold);
                    }
                }
            }

            RestoreExpandedNodes(tv, expandedNodes);

            tv.EndUpdate();
            SetTreeViewScrollPos(tv, scrollPos);

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
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return databases;
        }

        private List<string> Tables(string db)
        {
            List<string> tables = new List<string>();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW TABLES IN " + db;
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
        #endregion

        #region MySQL Functions
        public bool Connect(string connectionString)
        {
            //Close any possible existing connection
            connection.Close();

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

        public Result Query(string query)
        {
            bool success = false;
            DataTable dt = new DataTable();
            string message = "";
            int rowsAffected = 0;
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            try
            {
                using (MySqlDataReader queryReader = cmd.ExecuteReader())
                {
                    dt.Load(queryReader);
                    rowsAffected = queryReader.RecordsAffected;
                    if (rowsAffected == -1) //It was a select statement
                    {
                        message = string.Format("{0} row(s) selected", dt.Rows.Count);
                    }
                    else
                    {
                        message = string.Format("{0} row(s) affected", rowsAffected);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            Result result = new Result(
                success,
                dt,
                query,
                message
                );
            return result;
        }

        public DataTable QueryDT(string query)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            try
            {
                using (MySqlDataReader queryReader = cmd.ExecuteReader())
                {
                    dt.Load(queryReader);
                }
            }
            catch (Exception ex)
            {
                dt.Columns.Add("ErrorMessage", typeof(String));
                dt.Rows.Add(new Object[] { ex.Message });
            }
            return dt;
        }

        public List<List<string>> QueryStr(string query)
        {
            List<List<string>> results = new List<List<string>>();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            try
            {
                using (MySqlDataReader queryReader = cmd.ExecuteReader())
                {
                    Object[] resultRow = new Object[queryReader.FieldCount];
                    List<string> columnNames = queryReader.GetSchemaTable()
                                                          .Rows.Cast<DataRow>()
                                                          .Select(row => row[0].ToString())
                                                          .ToArray().ToList();
                    results.Add(columnNames);
                    while (queryReader.Read())
                    {
                        queryReader.GetValues(resultRow);
                        List<string> str = resultRow.Select(i => Convert.ToString(i)).ToList();
                        results.Add(str);
                    }
                }
            }
            catch (Exception ex)
            {
                results.Add(new List<string>
                {
                    "ERROR: " + ex.ToString()
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
                if (SelectedDb != null)
                {
                    SelectedDb.NodeFont = new Font(SelectedDb.NodeFont, FontStyle.Regular);
                    SelectedDb.Text = SelectedDb.Text;
                    //Forces the text to be redrawn to prevent clipping
                }
            }
            SelectedDb = db;
            SelectedDb.NodeFont = new Font(new TreeView().Font, FontStyle.Bold);
            //Forces the text to be redrawn to prevent clipping
            SelectedDb.Text = SelectedDb.Text;
            tv.EndUpdate();
        }
        #endregion

        #region Expanded TreeNode resoration
        /* Source for restoring expanded TreeNodes: http://stackoverflow.com/a/14605609 */

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
        #endregion

        #region Maintaining TreeView scroll position
        /* Source for maintaining TreeView scroll position: http://stackoverflow.com/a/359896 */

        private Point GetTreeViewScrollPos(TreeView tv)
        {
            return new Point(
                GetScrollPos((int)tv.Handle, SB_HORZ),
                GetScrollPos((int)tv.Handle, SB_VERT));
        }

        private void SetTreeViewScrollPos(TreeView tv, Point scrollPosition)
        {
            SetScrollPos((IntPtr)tv.Handle, SB_HORZ, scrollPosition.X, true);
            SetScrollPos((IntPtr)tv.Handle, SB_VERT, scrollPosition.Y, true);
        }
        #endregion

        #region Implementation of IDisposable
        public void Close()
        {
            connection.Close();
        }

        public void Dispose()
        {
            connection.Close();
        }
        #endregion
    }
}
