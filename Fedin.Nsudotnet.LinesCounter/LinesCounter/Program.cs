using System;
using System.IO;

namespace Fedin.Nsudotnet.LinesCounter
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter path");
            var path = Console.ReadLine();
            
            if (!string.IsNullOrEmpty(path))
            {
                var allFiles = Directory.GetFiles(path, args[0], SearchOption.AllDirectories);
                var codeLines = 0;
                foreach (var file in allFiles)
                {
                    using (var reader = new StreamReader(file))
                    {
                        string line;
                        bool comment = false;
                        while (null != (line = reader.ReadLine()))
                        {
                            line = line.Replace(" ", string.Empty);
                            if (line.Contains("/*"))
                            {
                                comment = true;
                            }
                            if (line.Contains("*/"))
                            {
                                comment = false;
                            }
                            if (line == string.Empty || line.Equals("//"))
                            {
                                continue;
                            }
                            if (!comment)
                            {
                                codeLines++;
                            }
                        }
                    }
                }
                Console.WriteLine("Amount of Code lines in {0} = {1}", path, codeLines);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("cant open dir");             
            }
        }
    }
}