namespace To_Do_List
{
    internal class Program
    {
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
                Console.WriteLine("\nEnter 'add' to add an item, 'remove' to remove an item, 'view' to view all items, or 'exit' to quit.");
                Console.Write("Your choice: ");
                string command = Console.ReadLine().ToLower();

                if (command == "exit")
                {
                    break;
                }
                else if (command == "add")
                {
                    Console.Write("Enter text to add: ");
                    string input = Console.ReadLine();
                    toDoList.AddItem(input);
                }
                else if (command == "remove")
                {
                    toDoList.Show();
                    Console.Write("Enter the number of the item you want to remove: ");
                    var input = Console.ReadLine();
                    toDoList.RemoveItem(input);
                }

                else if (command == "view")
                {
                    toDoList.Show();
                }
            }
        }
    }
}
