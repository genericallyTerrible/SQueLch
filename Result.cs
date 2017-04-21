using System.Data;

namespace SQueLch
{
    class Result
    {
        private bool success;
        private DataTable resultTable;
        private string action;
        private string message;

        public bool Success { get => success; }
        public DataTable ResultTable { get => resultTable; }
        public string Action { get => action; }
        public string Message { get => message; }

        public Result(bool success, DataTable resultTable, string action, string message)
        {
            this.success = success;
            this.resultTable = resultTable;
            this.action = action;
            this.message = message;
        } 
    }
}
