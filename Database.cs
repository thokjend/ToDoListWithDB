using System.Data.SqlClient;
using Dapper;

namespace To_Do_List
{
    internal class Database
    {
        private string _connection;
        private readonly SqlConnection _conn;

        public Database(string connection)
        {
            _connection = connection;
            _conn = new SqlConnection(connection);
        }

        public async void CreateTask(string task)
        {
            var newTask = new Tasks();
            newTask.Task = task;
            newTask.CreatedAt = DateTime.Now;

            var rowsAffected = await _conn.ExecuteAsync("INSERT INTO Task (Task, CreatedAt) VALUES (@Task, @CreatedAt)", newTask);
        }


    }
}
