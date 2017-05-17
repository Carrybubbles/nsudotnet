﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Fedin.Nsudotnet.LinesCounter
{
    class Program
    {
        static  string[] GetFiles(string path, string pattern, SearchOption searchOption)
        {
            var patterns = pattern.Split('|');
            var files = new List<string>();
            foreach (var pat in patterns)
            {
                files.AddRange(Directory.GetFiles(path, pat, searchOption));
            }
            return files.ToArray();
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Enter path, pls. To select current path, push ENTER button");
            var path = Console.ReadLine();

            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory();
                Console.WriteLine("Selected current dir: {0}", path);
            }
            else
            {
                Console.WriteLine("You selected dir: {0}", path);
            }
            Console.WriteLine("Enter file extensions. For multi using '|' ");
            string extension = Console.ReadLine();
            var allFiles = GetFiles(path, extension, SearchOption.AllDirectories);
            var codeLines = 0;
            foreach (var file in allFiles)
            {
                using (var reader = new StreamReader(file))
                {
                    string line;
                    bool bigCom = false;
                    while (null != (line = reader.ReadLine()))
                    {
                        line = line.Replace(" ", string.Empty);
                        if (line.Contains("/*"))
                        {
                            // case : var lalka = /* 10;
                            var comPos = line.IndexOf("/*", StringComparison.Ordinal);
                            if(comPos != 0)
                            {
                                codeLines++;
                            }
                            bigCom = true;
                        }
                        if (line.Contains("*/"))
                        {
                            bigCom = false;
                        }
                        if (line == string.Empty || line.StartsWith("//"))
                        {
                            continue;
                        }
                        if (!bigCom && line.Length > 2)
                        {
                            codeLines++;
                        }
                    }
                }
            }
            Console.WriteLine("Amount of lines in {0} = {1}", path, codeLines);
            Console.ReadLine();
        }
    }
}