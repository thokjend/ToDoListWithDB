namespace To_Do_List
{
    internal class ToDoList
    {
        private string _filePath;
        private List<string> _fileContent;

        public ToDoList(string filePath)
        {
            _filePath = filePath;
            _fileContent = new List<string>(File.ReadAllLines(filePath));
        }

        public void Show()
        {
            Console.WriteLine("\nCurrent content:");
            for (var i = 0; i < _fileContent.Count; i++)
            {
                var content = _fileContent[i];
                Console.WriteLine($"{i + 1}: {content}");
            }
        }

        public void RemoveItem(string input)
        {
            if (int.TryParse(input, out int index))
            {
                if (index > 0 && index <= _fileContent.Count)
                {
                    _fileContent.RemoveAt(index - 1);
                    File.WriteAllLines(_filePath, _fileContent);
                }
                else
                {
                    Console.WriteLine("Invalid index");
                }
            }
        }

        public void AddItem(string input)
        {
            _fileContent.Add(input);
            File.AppendAllText(_filePath, input + Environment.NewLine);
        }


    }
}
