using System.Data.SqlClient;
using Dapper;

namespace To_Do_List
{
    internal class Program
    {
        //static async void CreateTask(string task)
        //{
        //    var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True";
        //    var conn = new SqlConnection(connection);

        //    var newTask = new Tasks();
        //    newTask.Task = task;
        //    newTask.CreatedAt = DateTime.Now;

        //    var rowsAffected = await conn.ExecuteAsync("INSERT INTO Task (Task, CreatedAt) VALUES (@Task, @CreatedAt)", newTask);
        //}

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

            var task = await conn.QueryFirstOrDefaultAsync<Tasks>($"SELECT * FROM Task WHERE Id like '{id}'");
            if (task?.CompletedAt != null)
            {
                //Console.WriteLine($"Task with Id {id} has already been completed on {task.CompletedAt}.");
                return;
            }

            var newTask = new Tasks();
            newTask.Id = id;

            var rowsAffected = await conn.ExecuteAsync($"UPDATE Task SET CompletedAT = '{DateTime.Now}' WHERE Id like '{id}' ");

        }


        static void Main(string[] args)
        {

            var db = new Database(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True");

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

                if (command == "exit")
                {
                    break;
                }

                else if (command == "add")
                {
                    Console.Write("Enter task: ");
                    string input = Console.ReadLine();
                    db.CreateTask(input);
                    //CreateTask(input);
                }

                else if (command == "remove")
                {
                    ShowTasks().Wait();
                    Console.Write("\nEnter the id of the task you want to remove: ");
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int id))
                    {
                        DeleteTask(id);
                    }
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
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int id))
                    {
                        CompleteTask(id);
                    }
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
