using System.Data.SqlClient;
using Dapper;

namespace To_Do_List
{
    internal class Program
    {
        static async void CreateTask(string task)
        {
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True";
            var conn = new SqlConnection(connection);

            var newTask = new Tasks();
            newTask.Task = task;
            newTask.CreatedAt = DateTime.UtcNow;

            var rowsAffected = await conn.ExecuteAsync("INSERT INTO Task (Task, CreatedAt) VALUES (@Task, @CreatedAt)", newTask);
        }

        static async Task<IEnumerable<Tasks>> ViewTask()
        {
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True";
            var conn = new SqlConnection(connection);

            var tasks = await conn.QueryAsync<Tasks>("SELECT * FROM Task");
            return tasks;
        }

        static async void DeleteTask(int id)
        {
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True";
            var conn = new SqlConnection(connection);

            var newTask = new Tasks();
            newTask.Id = id;

            var rowsAffected = await conn.ExecuteAsync($"DELETE FROM Task WHERE Id like '{id}' ");
        }

        static async void CompleteTask(int id)
        {
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True";
            var conn = new SqlConnection(connection);

            var newTask = new Tasks();
            newTask.Id = id;

            var rowsAffected = await conn.ExecuteAsync($"UPDATE Task SET CompletedAT = '{DateTime.Now}' WHERE Id like '{id}' ");
            
        }


        static void Main(string[] args)
        {
            while (true)
            {
                var infoText = @"
-----------------------------------------------------
|  To-Do List Application                           |
-----------------------------------------------------
|  Commands:                                        |
|  'add'      - Add a new task                      |
|  'remove'   - Remove a task by its ID             |
|  'view'     - View all tasks                      |
|  'complete' - Mark a task as completed            |
|  'exit'     - Quit the application                |
-----------------------------------------------------
";
                Console.WriteLine(infoText);
                Console.Write("Your choice: ");
                string command = Console.ReadLine().ToLower();

                //if (command.StartsWith("db:"))
                //{
                //    string[] parts = command.Split(":");
                //    var cmd = parts[1];
                //    GetItems(cmd);
                //}

                if (command == "exit")
                {
                    break;
                }
                else if (command == "add")
                {
                    Console.Write("Enter task: ");
                    string input = Console.ReadLine();
                    CreateTask(input);
                }
                else if (command == "remove")
                {
                    ShowTasks().Wait();
                    Console.Write("\nEnter the id of the task you want to remove: ");
                    var input = int.Parse(Console.ReadLine());
                    DeleteTask(input);
                }

                else if (command == "view")
                {
                    ShowTasks().Wait();
                    Console.Write("\npress enter to continue");
                    Console.ReadLine();
                }
                else if (command == "complete")
                {
                    ShowTasks().Wait();
                    Console.Write("\nEnter the id of the task you want to mark as completed: ");
                    var input = int.Parse(Console.ReadLine());
                    CompleteTask(input);
                }
            }
        }

        public static async Task ShowTasks()
        {
            var tasks = await ViewTask();
            Console.WriteLine("\n{0,-5} {1,-15} {2,-25} {3,-20}", "Id", "Task", "CreatedAt", "CompletedAt");
            foreach (var task in tasks)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-25} {3,-20}", task.Id, task.Task, task.CreatedAt, task.CompletedAt);
            }
        }

    }
}
