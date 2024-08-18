using urlfuzz.src.Utils;

namespace urlfuzz
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: urlfuzz.exe <wordlist> <url>");
                return;
            }

            Console.WriteLine("Load Http module...");

            Http http = new();

            Console.WriteLine("Load wordlist...");

            try
            {
                // Read the contents of the file
                string fileContents = File.ReadAllText(args[0]);

                // Split the file contents into an array
                string[] words = fileContents.Split('\n');

                Console.WriteLine("Fuzzing...");

                List<Task<int>> tasks = new List<Task<int>>();

                foreach (string word in words)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        string url = args[1].TrimEnd('/') + '/' + word.TrimStart('/');

                        Console.WriteLine("Fuzzing: " + url);

                        HttpResponseMessage response = await http.Get(url);

                        return (int)response.StatusCode;
                    }));
                }

                await Task.WhenAll(tasks);

                foreach (Task<int> task in tasks)
                {
                    int taskCode = await task;
                    if (taskCode == 200)
                    {
                        Console.WriteLine("Found: " + taskCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the file: " + ex.Message);
            }

            Console.WriteLine("Fuzzing complete!");

            Console.ReadLine();
        }
    }
}