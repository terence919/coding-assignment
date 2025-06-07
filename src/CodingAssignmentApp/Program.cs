// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.IO.Abstractions;
using CodingAssignmentLib;
using CodingAssignmentLib.Abstractions;

namespace CodingAssignment
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Coding Assignment!");
            do
            {
                Console.WriteLine("\n---------------------------------------\n");
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\t1 - Display");
                Console.WriteLine("\t2 - Search");
                Console.WriteLine("\t3 - Exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        Display();
                        break;
                    case "2":
                        Search();
                        break;
                    case "3":
                        return;
                    default:
                        return;
                }
            } while (true);
        }
        
        public static List<string> GetAllFilesName(string filePath)
        {
            try
            {
                var searchPatterns = new[] { ".csv", ".json", ".xml" };
                var files = Directory
                    .EnumerateFiles(filePath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => searchPatterns.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                    .ToList();

                return files;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return [];
            }
        }

        public static string DisplaySearchInfo(string inputKey,  List<string> filesNames)
        {
            foreach (var file in filesNames)
            {
                var dataList = GetDataList(file);
                var foundData = dataList.FirstOrDefault(data =>
                    !string.IsNullOrEmpty(data.Key) &&
                    string.Equals(data.Key, inputKey.Trim(), StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(foundData.Key)) continue;
                var marker = "data" + Path.DirectorySeparatorChar; 
                var index = file.LastIndexOf(marker, StringComparison.OrdinalIgnoreCase);
                var relativePath = file.Substring(index);
                return $"Key:{foundData.Key} Value:{foundData.Value} FileName:{relativePath}";
            }
            return "Search not found.";
        }

        public static IEnumerable<Data> GetDataList(string fileName)
        {
            try
            {
                var fileUtility = new FileUtility(new FileSystem());
                var content = fileUtility.GetContent(fileName);

                return fileUtility.GetExtension(fileName) switch
                {
                    ".csv" => new CsvContentParser().Parse(content),
                    ".json" => new JsonContentParser().Parse(content),
                    ".xml" => new XmlContentParser().Parse(content),
                    _ => []
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return [];
            }
        }
        
        private static void Display()
        {
            Console.WriteLine("Enter the name of the file to display its content:");

            var fileName = Console.ReadLine()!;
            var dataList = GetDataList(fileName);
    
            Console.WriteLine("Data:");

            foreach (var data in dataList!)
            {
                Console.WriteLine($"Key:{data.Key} Value:{data.Value}");
            }
        }

        private static void Search()
        {
            var filePath = Path.Combine(GetRootPath(), "data");
            var filesNames = GetAllFilesName(filePath);
    
            Console.WriteLine("Enter the key to search.");
    
            var inputKey = Console.ReadLine()!;
            var displayInfo = DisplaySearchInfo(inputKey, filesNames); 
            
            Console.WriteLine($"{displayInfo}");
        }

        private static string GetRootPath()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !dir.GetFiles("CodingAssignmentApp.csproj").Any())
                dir = dir.Parent;

            if (dir == null)
                throw new Exception("Project root not found.");

            return dir.FullName;
        }
    }
}
