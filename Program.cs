using System.Data.SqlClient;
using Dapper;

namespace To_Do_List
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var db = new Database(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True");

            while (true)
            {
                Console.WriteLine(
                    "\n-----------------------------------------------------\n" +
                    "|  To-Do List Application                           |\n" +
                    "-----------------------------------------------------\n" +
                    "|  Commands:                                        |\n" +
                    "|  'add'      - Add a new task                      |\n" +
                    "|  'remove'   - Remove a task by its ID             |\n" +
                    "|  'view'     - View all tasks                      |\n" +
                    "|  'complete' - Mark a task as completed            |\n" +
                    "|  'exit'     - Quit the application                |\n" +
                    "-----------------------------------------------------"
                );
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
                }

                else if (command == "remove")
                {
                    db.ShowTasks(1).Wait();
                    Console.Write("\nEnter the id of the task you want to remove: ");
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int id))
                    {
                        db.DeleteTask(id);
                    }
                }

                else if (command == "view")
                {
                    while (true)
                    {
                        Console.Write(
                            "\n---------------------------------------------\n" +
                            "|  View Tasks:                              |\n" +
                            "|  1 - View all tasks                       |\n" +
                            "|  2 - View completed tasks                 |\n" +
                            "|  3 - View non-completed tasks             |\n" +
                            "---------------------------------------------\n" +
                            "Enter your choice: "
                        );
                        var input = Console.ReadLine();
                        if (int.TryParse(input, out int number))
                        {
                            db.ShowTasks(number).Wait();
                            break;
                        }
                        
                        Console.WriteLine("Please enter a valid choice");
                        
                    }
                }

                else if (command == "complete")
                {
                    db.ShowTasks(3).Wait();
                    Console.Write("\nEnter the id of the task you want to mark as completed: ");
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int id))
                    {
                        db.CompleteTask(id);
                    }
                }
            }
        }
    }
}
