using System.Data.SqlClient;
using System.Threading.Tasks;
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
            //newTask.CompletedAt = null;

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


        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\thokj\\OneDrive\\Desktop\\ToDoList\\test.txt";
            var toDoList = new ToDoList(filePath);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            while (true)
            {
                Console.WriteLine("\nEnter 'add' to add a task, 'remove' to remove a task, 'view' to view all tasks, or 'exit' to quit.");
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
                    //toDoList.AddItem(input);
                }
                else if (command == "remove")
                {
                    ShowTasks();
                    //toDoList.Show();
                    Console.Write("Enter the id of the task you want to remove: ");
                    var input = int.Parse(Console.ReadLine());
                    DeleteTask(input);

                    //toDoList.RemoveItem(input);
                }

                else if (command == "view")
                {
                    ShowTasks();
                    //var tasks = await ViewTask();
                    //Console.WriteLine("{0,-5} {1,-15} {2,-25} {3,-20}", "id", "task", "CreatedAt", "CompletedAt");
                    //foreach (var task in tasks)
                    //{
                    //    Console.WriteLine("{0,-5} {1,-15} {2,-25} {3,-20}", task.Id, task.Task, task.CreatedAt, task.CompletedAt);
                    //}
                    //ViewTask();
                    //toDoList.Show();
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
