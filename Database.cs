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

        public async void DeleteTask(int id)
        {
            var newTask = new Tasks();
            newTask.Id = id;

            var rowsAffected = await _conn.ExecuteAsync($"DELETE FROM Task WHERE Id like '{id}' ");
        }

        public async void CompleteTask(int id)
        {
            //var task = await _conn.QueryFirstOrDefaultAsync<Tasks>($"SELECT * FROM Task WHERE Id like '{id}'");
            //if (task?.CompletedAt != null)
            //{
            //    //Console.WriteLine($"Task with Id {id} has already been completed on {task.CompletedAt}.");
            //    return;
            //}

            var newTask = new Tasks();
            newTask.Id = id;

            var rowsAffected = await _conn.ExecuteAsync($"UPDATE Task SET CompletedAT = '{DateTime.Now}' WHERE Id like '{id}' ");
        }

        public async Task ShowTasks(int input)
        {
            var tasks = await FilterTask(input);
            Console.WriteLine("\n{0,-5} {1,-15} {2,-25} {3,-20}", "Id", "Task", "CreatedAt", "CompletedAt");
            foreach (var task in tasks)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-25} {3,-20}", task.Id, task.Task, task.CreatedAt, task.CompletedAt);
            }
        }
        private async Task<IEnumerable<Tasks>> FilterTask(int input)
        {
            if (input == 1)
            {
                var tasks = await _conn.QueryAsync<Tasks>("SELECT * FROM Task");
                return tasks;
            }
            if (input == 2)
            {
                var tasks = await _conn.QueryAsync<Tasks>("SELECT * FROM Task WHERE CompletedAt IS NOT NULL");
                return tasks;
            }
            if (input == 3)
            {
                var tasks = await _conn.QueryAsync<Tasks>("SELECT * FROM Task WHERE CompletedAt IS NULL");
                return tasks;
            }
            
            Console.WriteLine("\nInvalid choice, returning no tasks.");
            return new List<Tasks>();
            
        }
    }
}
